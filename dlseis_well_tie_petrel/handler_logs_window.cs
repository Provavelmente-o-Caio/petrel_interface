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

        public handler_logs_window(string wellname)
        {
            InitializeComponent();

            // Selecting the well root
            Project project = PetrelProject.PrimaryProject;
            var wellroot = WellRoot.Get(project);
            var wellColection = wellroot.BoreholeCollection;

            var well = wellColection.BoreholeCollections.ElementAt(0).Where(w => w.Name == wellname).Select(w => w).ElementAt(0);

            // var lognames = well.Logs.WellLogs.ToList();
            var lognames = well.Logs.WellLogs.Select(w => w.Name).ToArray();
            // Gambierra de novo :D --> Feito para rapidamente conseguir acessar os lognames de fora como uma lista
            logs.Clear();
            logs = lognames.ToList();

            comboBoxVP.Items.AddRange(lognames);
            comboBoxVS.Items.AddRange(lognames);
            comboBoxRho.Items.AddRange(lognames);
            comboBoxGR.Items.AddRange(lognames);
            comboBoxCali.Items.AddRange(lognames);
        }

        private void buttonHandlerLogs_Click(object sender, EventArgs e)
        {
            // Saving the selected columns
            SelectedLogs.Clear();
            this.DialogResult = DialogResult.None;

            var selectedVP = comboBoxVP.Text;
            var selectedVS = comboBoxVS.Text;
            var selectedRho = comboBoxRho.Text;
            var selectedGR = comboBoxGR.Text;
            var selectedCali = comboBoxCali.Text;

            if (selectedVP != "" && selectedVS != "" && selectedRho != "")
            {
                // selecionar as janelas
                SelectedLogs.Add("VP", selectedVP);
                SelectedLogs.Add("VS", selectedVS);
                SelectedLogs.Add("Rho", selectedRho);
                if (selectedGR != "")
                {
                    SelectedLogs.Add("GR", selectedGR);
                }
                if (selectedCali != "")
                {
                    SelectedLogs.Add("Cali", selectedCali);
                }
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                // levantar erro pedindo para o usuário selecionar todas as janelas
                MessageBox.Show("Selecione todos os campos antes de avançar para o próximo ponto");
            }

            this.Close();
        }

        public Dictionary<string, string> getSelectedLogs()
        {
            return SelectedLogs;
        }

        public List<string> getLogs()
        {
            return logs;
        }
    }
}
