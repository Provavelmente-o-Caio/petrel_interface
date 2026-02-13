using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject;

namespace dlseis_well_tie_petrel
{
    public partial class handler_logs_window : Form
    {
        private Dictionary<string, string> SelectedLogs = new Dictionary<string, string>();
        private List<string> logs = new List<string>();
        private int log_range;
        private int startRange;
        private int endRange;
        private string lasUnit;

        public handler_logs_window(string wellname)
        {
            InitializeComponent();

            List<string> units = new List<string> { "m/s", "us/ft", "us/m", "s/m", "ft/s", "km/s" };

            // Selecting the well root
            Project project = PetrelProject.PrimaryProject;
            var wellroot = WellRoot.Get(project);
            var wellColection = wellroot.BoreholeCollection;

            var well = wellColection.BoreholeCollections.ElementAt(0).Where(w => w.Name == wellname).Select(w => w).ElementAt(0);

            var lognames = well.Logs.WellLogs.Select(w => w.Name).ToArray();
            log_range = well.Logs.WellLogs.Select(w => w.SampleCount).ElementAt(0);

            logs.Clear();
            logs = lognames.ToList();

            comboBoxVP.Items.AddRange(lognames);
            comboBoxVS.Items.AddRange(lognames);
            comboBoxRho.Items.AddRange(lognames);
            comboBoxGR.Items.AddRange(lognames);
            comboBoxCali.Items.AddRange(lognames);
            comboBoxUnit.DataSource = units;

            unitTextBox_range_start.Value = log_range;
            unitTextBox_range_end.Value = 0;
        }

        private bool ValidateRange(int startRange, int endRange, int maxRange)
        {
            if (startRange < endRange)
            {
                MessageBox.Show(
                    "Start range must be greater than or equal to end range.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (startRange < 0 || endRange < 0)
            {
                MessageBox.Show(
                    "Range values must be non-negative.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (startRange > maxRange || endRange > maxRange)
            {
                MessageBox.Show(
                    $"Range values must not exceed {maxRange}.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void buttonHandlerLogs_Click(object sender, EventArgs e)
        {
            SelectedLogs.Clear();
            this.DialogResult = DialogResult.None;

            string selectedVP = comboBoxVP.Text;
            string selectedVS = comboBoxVS.Text;
            string selectedRho = comboBoxRho.Text;
            string selectedGR = comboBoxGR.Text;
            string selectedCali = comboBoxCali.Text;
            startRange = ((int)unitTextBox_range_start.Value);
            endRange = ((int)unitTextBox_range_end.Value);
            lasUnit = comboBoxUnit.Text;

            if (!string.IsNullOrEmpty(selectedVP)  && !string.IsNullOrEmpty(selectedVS) && !string.IsNullOrEmpty(selectedRho) && startRange >= 0 && endRange >= 0)
            {
                if (ValidateRange(startRange, endRange, log_range))
                {
                    // selecionar as janelas
                    SelectedLogs.Add("VP", selectedVP);
                    SelectedLogs.Add("VS", selectedVS);
                    SelectedLogs.Add("Rho", selectedRho);

                    if (!string.IsNullOrEmpty(selectedGR))
                    {
                        SelectedLogs.Add("GR", selectedGR);
                    }
                    if (!string.IsNullOrEmpty(selectedCali))
                    {
                        SelectedLogs.Add("Cali", selectedCali);
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                } else
                {
                    MessageBox.Show("Os indices selecionados para a borda dos logs é inválido");
                };
            }
            else
            {
                MessageBox.Show("Selecione todos os campos antes de avançar para o próximo ponto");
                return;
            }
        }

        public Dictionary<string, string> getSelectedLogs()
        {
            return SelectedLogs;
        }

        public List<string> getLogs()
        {
            return logs;
        }

        public string getStartRange()
        {
            return startRange.ToString();
        }

        public string getEndRange()
        {
            return endRange.ToString();
        }

        public string getLasUnit()
        {
            return lasUnit;
        }
    }
}
