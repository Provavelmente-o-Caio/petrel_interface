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

namespace dlseis_well_tie_petrel
{
    public partial class well_tie_window : Form
    {
        public well_tie_window()
        {
            InitializeComponent();
        }

        private void button_tie_Click(object sender, EventArgs e)
        {
            var path_logs = openFileDialog_logs.FileName;
            var path_seismic = openFileDialog_seismic.FileName;
            var path_well_path = openFileDialog_well_path.FileName;
            var path_table = openFileDialog_table.FileName;

            PetrelLogger.InfoOutputWindow(path_logs);
            PetrelLogger.InfoOutputWindow(path_seismic);
            PetrelLogger.InfoOutputWindow(path_well_path);
            PetrelLogger.InfoOutputWindow(path_table);

            if (!File.Exists(path_logs) ||
                !File.Exists(path_seismic) ||
                !File.Exists(path_well_path) ||
                !File.Exists(path_table))
            {
                MessageBox.Show("One or more selected files do not exist.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String RunURI = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            String ScriptName = "interface_well_tie.bat";
            String StringProcess = String.Concat(RunURI, "\\", ScriptName);
            var startInfo = new ProcessStartInfo
            {
                FileName = StringProcess,
                Arguments = $"\"{path_logs}\" \"{path_seismic}\" \"{path_well_path}\" \"{path_table}\"",
                UseShellExecute = true,
                CreateNoWindow = false
            };

            var process = Process.Start(startInfo);
            process.WaitForExit();
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
