using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.Data;
using Slb.Ocean.Petrel.Well;
using Slb.Ocean.Petrel.Base;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;


namespace dlseis_well_tie_petrel
{
    public partial class well_tie_window : Form
    {
        public well_tie_window()
        {
            InitializeComponent();

            // Selecting the well root
            Project project = PetrelProject.PrimaryProject;
            var wellroot = WellRoot.Get(project);
            var wellCollection = wellroot.BoreholeCollection;
            var wellnames = wellCollection.BoreholeCollections.ElementAt(0).Select(w => w.Name).ToArray();

            comboBoxPetrelLogs.Items.AddRange(wellnames);
        }

        private void button_tie_Click(object sender, EventArgs e)
        {
            var path_logs = openFileDialog_logs.FileName;
            var path_seismic = openFileDialog_seismic.FileName;
            var path_well_path = openFileDialog_well_path.FileName;
            var path_table = openFileDialog_table.FileName;
            var selectedPetrelLog = comboBoxPetrelLogs.Text;

            // Checking if all file paths are valid
            if (!File.Exists(path_logs))
            {
                MessageBox.Show("The file for the well logs doesn't exist", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(path_seismic))
            {
                MessageBox.Show("The file for the well seismic doesn't exist", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(path_well_path))
            {
                MessageBox.Show("The file for the well path doesn't exist", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(path_table))
            {
                MessageBox.Show("The file for the well data-time table doesn't exist", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (selectedPetrelLog == "")
            {
                MessageBox.Show("You have to select a well log", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // isso eu preciso fazer com o blue button
            // infelizmente a extensão presente no computador está na versão errada e logo não funciona
            var columnsLogs = Utils.GetColumnsFromFile(path_logs, 0);

            // esses eu acho que está tudo bem por enquanto :)
            var columnsWellPath = Utils.GetColumnsFromFile(path_well_path, 1);
            var columnsTable = Utils.GetColumnsFromFile(path_table, 1);

            var instanceWindowLog = new handler_logs_window(selectedPetrelLog);
            var instanceWindowPath = new handler_path_window(columnsWellPath);
            var instanceWindowTable = new handler_table_window(columnsTable);

            this.Hide();

            if (instanceWindowLog.ShowDialog() == DialogResult.OK
                && instanceWindowPath.ShowDialog() == DialogResult.OK
                && instanceWindowTable.ShowDialog() == DialogResult.OK)
            {
                var exportData = new
                {
                    Logs = instanceWindowLog.getSelectedLogs(),
                    Path = instanceWindowPath.getSelectedColumns(),
                    Table = instanceWindowTable.getSelectedColumns()
                };

                var exportConfig = new
                {
                    isTWT = instanceWindowTable.getOWT()
                };

                String RunURI = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                String ScriptName = "interface_well_tie.bat";
                String StringProcess = String.Concat(RunURI, "\\", ScriptName);

                string path_Json = Utils.WriteJson(exportData, RunURI, "selected_data.json");
                string path_config = Utils.WriteJson(exportConfig, RunURI, "config.json");

                var startInfo = new ProcessStartInfo
                {
                    FileName = StringProcess,
                    Arguments = $"\"{path_logs}\" \"{path_seismic}\" \"{path_well_path}\" \"{path_table}\" \"{path_Json}\" \"{path_config}\"",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                var process = Process.Start(startInfo);
                process.WaitForExit();
            }
            else
            {
                return;
                // TODO: Error handling
            }
        }

        private void button_select_logs_Click(object sender, EventArgs e)
        {
            if (openFileDialog_logs.ShowDialog() == DialogResult.OK)
            {
                textBox_logs.Text = openFileDialog_logs.FileName;
            }
        }

        private void button_select_seismic_Click(object sender, EventArgs e)
        {
            if (openFileDialog_seismic.ShowDialog() == DialogResult.OK)
            {
                textBox_seismic.Text = openFileDialog_seismic.FileName;
            }
        }

        private void button_select_well_path_Click(object sender, EventArgs e)
        {
            if (openFileDialog_well_path.ShowDialog() == DialogResult.OK)
            {
                textBox_well_path.Text = openFileDialog_well_path.FileName;
            }
        }

        private void button_select_table_Click(object sender, EventArgs e)
        {
            if (openFileDialog_table.ShowDialog() == DialogResult.OK)
            {
                textBox_table.Text = openFileDialog_table.FileName;
            }
        }
    }
}
