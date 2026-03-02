using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Well;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;


namespace dlseis_well_tie_petrel
{
    public partial class well_tie_window : Form
    {
        private const string ScriptName = "interface_well_tie.bat";

        private const string JsonSelectedData = "selected_data.json";
        private const string JsonConfig = "config.json";
        private const string JsonOutput = "well_tie_results.json";

        private const string DialogFileErrorTitle = "File Error";


        public well_tie_window()
        {
            InitializeComponent();
        }


        private void WellTieWindow_Load(object sender, EventArgs e)
        {
            try
            {
                LoadWellsIntoComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Petrel Error");
                Close();
            }
        }


        private bool TryCollectInputs(out WellTieInputs inputs)
        {
            inputs = new WellTieInputs
            {
                LogsPath = openFileDialog_logs.FileName,
                SeismicPath = openFileDialog_seismic.FileName,
                WellPathPath = openFileDialog_well_path.FileName,
                TablePath = openFileDialog_table.FileName,
                SelectedPetrelLog = comboBoxPetrelLogs.Text
            };

            // Checking if all file paths are valid
            if (!ValidateFile(inputs.LogsPath, "The file for the well logs doesn't exist")) return false;
            if (!ValidateFile(inputs.SeismicPath, "The file for the well seismic doesn't exist")) return false;
            if (!ValidateFile(inputs.WellPathPath, "The file for the well path doesn't exist")) return false;
            if (!ValidateFile(inputs.TablePath, "The file for the well data-time table doesn't exist")) return false;

            if (string.IsNullOrWhiteSpace(inputs.SelectedPetrelLog))
            {
                MessageBox.Show("You have to select a well log", DialogFileErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        private bool RunWellTieWizard(
            WellTieInputs inputs,
            out WellTieSelection selection,
            out WellTieData data)
        {
            selection = null;
            data = null;

            bool isLas = Path.GetExtension(inputs.WellPathPath).ToLower() == ".las";
            int pathLineToRead = isLas ? 0 : 1;

            var columnsWellPath = Utils.GetColumnsFromFile(inputs.WellPathPath, pathLineToRead);
            var columnsTable = Utils.GetColumnsFromFile(inputs.TablePath, 1);

            if (columnsWellPath == null || columnsWellPath.Length == 0)
            {
                MessageBox.Show("No valid columns found in well path file.", DialogFileErrorTitle);
                return false;
            }

            if (columnsTable == null || columnsTable.Length == 0)
            {
                MessageBox.Show("No valid columns found in table file.", DialogFileErrorTitle);
                return false;
            }

            using (var logsWindow = new handler_logs_window(inputs.SelectedPetrelLog))
            using (var pathWindow = new handler_path_window(columnsWellPath))
            using (var tableWindow = new handler_table_window(columnsTable))
            {
                Hide();

                try
                {
                    if (logsWindow.ShowDialog() != DialogResult.OK)
                        return false;

                    if (pathWindow.ShowDialog() != DialogResult.OK)
                        return false;

                    if (tableWindow.ShowDialog() != DialogResult.OK)
                        return false;

                    selection = new WellTieSelection
                    {
                        Logs = logsWindow.getSelectedLogs(),
                        PathColumns = pathWindow.getSelectedColumns(),
                        TableColumns = tableWindow.getSelectedColumns(),
                        IsOWT = tableWindow.getOWT(),
                        downwardTimeUnit = tableWindow.getDownwardTimeUnit(),
                        LasUnit = logsWindow.getLasUnit(),
                        StartRange = logsWindow.getStartRange(),
                        EndRange = logsWindow.getEndRange(),
                        Datum = pathWindow.getDatum()
                    };

                    data = new WellTieData
                    {
                        Logs = logsWindow.getLogs(),
                        Path = columnsWellPath.ToList(),
                        Table = columnsTable.ToList()
                    };

                    return true;
                }
                finally
                {
                    Show();
                }
            }
        }

        private void ExecuteWellTie(
            WellTieInputs inputs,
            WellTieSelection selection,
            WellTieData data)
        {
            string runDir = Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            string scriptPath = Path.Combine(runDir, ScriptName);
            string outputPath = Path.Combine(runDir, JsonOutput);

            var exportData = new
            {
                Logs = selection.Logs,
                Path = selection.PathColumns,
                Table = selection.TableColumns,
                Entire_Logs = data.Logs,
                Entire_Path = data.Path,
                Entire_Table = data.Table
            };

            var exportConfig = new
            {
                Logs = new
                {
                    las_unit = selection.LasUnit ?? "us/ft",
                    Start_range = selection.StartRange ?? "0",
                    End_range = selection.EndRange ?? "0"
                },
                Path = new
                {
                   datum = selection.Datum ?? "0"
                },
                Table = new 
                {
                    isOWT = selection.IsOWT,
                    Unit = selection.downwardTimeUnit,
                },
                SearchSpace = new
                {
                    median_length_min = 11,
                    median_length_max = 63,
                    median_th_min = 0.1,
                    median_th_max = 5.5,
                    std_min = 0.5,
                    std_max = 5.5,
                    table_t_shift_min = -0.012,
                    table_t_shift_max = 0.012
                },
                SearchParams = new
                {
                    num_iters = 80,
                    similarity_std = 0.02
                },
                WaveletScaling = new
                {
                    wavelet_min_scale = 50000,
                    wavelet_max_scale = 500000,
                    num_iters = 60
                },
                StretchAndSqueeze = new
                {
                    window_length = 0.060,
                    max_lag = 0.010
                }
            };

            string pathJson = Utils.WriteJson(exportData, runDir, JsonSelectedData);
            string pathConfig = Utils.WriteJson(exportConfig, runDir, JsonConfig);

            ExecuteScript(
                scriptPath,
                inputs.LogsPath,
                inputs.SeismicPath,
                inputs.WellPathPath,
                inputs.TablePath,
                pathJson,
                pathConfig,
                outputPath
                );
        }

        private void ExecuteScript(
            string scriptPath,
            string pathLogs,
            string pathSeismic,
            string pathWellPath,
            string pathTable,
            string pathJson,
            string pathConfig,
            string pathOutput 
            )
        {
            if (!File.Exists(scriptPath))
            {
                MessageBox.Show(
                    "Well tie script not found:\n" + scriptPath,
                    "Execution Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Enabled = false;

                var startInfo = new ProcessStartInfo
                {
                    FileName = scriptPath,
                    Arguments =
                        $"\"{pathLogs}\" \"{pathSeismic}\" \"{pathWellPath}\" " +
                        $"\"{pathTable}\" \"{pathJson}\" \"{pathConfig}\" \"{pathOutput}\"",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                using (Process process = Process.Start(startInfo))
                {
                    if (process == null)
                        throw new InvalidOperationException("Failed to start the script process.");

                    process.WaitForExit();

                    this.Cursor = Cursors.Default;
                    this.Enabled = true;

                    if (process.ExitCode != 0)
                    {
                        MessageBox.Show(
                            "The well tie script finished with errors.",
                            "Execution Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }

                    if (!File.Exists(pathOutput))
                    {
                        MessageBox.Show(
                        "Failed to generate an output: \n" + pathOutput,
                        "Ëxecution Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    }

                    var resultsWindow = new results_window(pathOutput);
                    resultsWindow.Show(this);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                this.Enabled = true;

                MessageBox.Show(
                    ex.Message,
                    "Execution Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void LoadWellsIntoComboBox()
        {
            comboBoxPetrelLogs.Items.Clear();

            Project project = PetrelProject.PrimaryProject
                ?? throw new InvalidOperationException("No active Petrel project.");

            WellRoot wellRoot = WellRoot.Get(project)
                ?? throw new InvalidOperationException("WellRoot not found.");

            var boreholeCollections = wellRoot.BoreholeCollection?.BoreholeCollections
                ?? throw new InvalidOperationException("No borehole collections found.");

            if (!boreholeCollections.Any())
                throw new InvalidOperationException("No boreholes available in the project.");

            var nameOccurrences = new Dictionary<string, int>();
            var wellNames = boreholeCollections
                .SelectMany(c => c)
                .Select(w => w.Name)
                .Distinct()
                .OrderBy(n => n)
                .ToArray();

            comboBoxPetrelLogs.Items.AddRange(wellNames);
        }


        private bool ValidateFile(string path, string message)
        {
            if (File.Exists(path))
            {
                return true;
            }

            MessageBox.Show(message, DialogFileErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

            return false;
        }


        private void button_tie_Click(object sender, EventArgs e)
        {
            WellTieInputs inputs;
            WellTieSelection selection;
            WellTieData data;

            // Coletar e validar inputs
            if (!TryCollectInputs(out inputs))
                return;

            // Executar wizard (logs / path / table)
            if (!RunWellTieWizard(inputs, out selection, out data))
                return;

            // Executar o well tie
            ExecuteWellTie(inputs, selection, data);
        }


        private void SelectFile(OpenFileDialog dialog, TextBox target)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                target.Text = dialog.FileName;
            }
        }


        private void button_select_logs_Click(object sender, EventArgs e)
        {
            SelectFile(openFileDialog_logs, textBox_logs);
        }


        private void button_select_seismic_Click(object sender, EventArgs e)
        {
            SelectFile(openFileDialog_seismic, textBox_seismic);
        }


        private void button_select_well_path_Click(object sender, EventArgs e)
        {
            SelectFile(openFileDialog_well_path, textBox_well_path);
        }


        private void button_select_table_Click(object sender, EventArgs e)
        {
            SelectFile(openFileDialog_table, textBox_table);
        }

        private void button_edit_config_Click(object sender, EventArgs e)
        {
            string runDir = Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            string configPath = Path.Combine(runDir, JsonConfig);

            if (!File.Exists(configPath))
            {
                MessageBox.Show(
                    "Run the well tie at least once to generate the config file.",
                    "Config Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var editor = new config_editor_window(configPath);
            editor.ShowDialog(this);
        }
    }

    // Dataclass salvando os caminhos para os dados para o welltie
    public class WellTieInputs
    {
        public string LogsPath { get; set; }
        public string SeismicPath { get; set; }
        public string WellPathPath { get; set; }
        public string TablePath { get; set; }
        public string SelectedPetrelLog { get; set; }
    }

    // Dataclass salvando os dados que serão usados no welltie
    public class WellTieSelection
    {
        public Dictionary<string, string> Logs { get; set; }
        public List<string> PathColumns { get; set; }
        public List<string> TableColumns { get; set; }
        public bool IsOWT { get; set; }
        public string downwardTimeUnit { get; set; }
        public string LasUnit { get; set; }
        public string StartRange { get; set; }
        public string EndRange { get; set; }
        public string Datum { get; set; }
    }

    // Dataclass para todos os dados originais do well tie
    public class WellTieData
    {
        public List<string> Logs { get; set; }
        public List<string> Path { get; set; }
        public List<string> Table { get; set; }
    }
}