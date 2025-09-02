using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject;

namespace dlseis_well_tie_petrel
{
    public partial class handler_logs_window : Form
    {
        private Dictionary<string, string> SelectedLogs = new Dictionary<string, string>();
        public handler_logs_window()
        {
            InitializeComponent();

            // Selecting the well root
            Project project = PetrelProject.PrimaryProject;
            var wellroot = WellRoot.Get(project);
            var wellColection = wellroot.BoreholeCollection;

            var wellname = "Boreas 1"; // Isso tá horrível
            var well = wellColection.BoreholeCollections.ElementAt(0).Where(w => w.Name == wellname).Select(w => w).ElementAt(0);

            // var lognames = well.Logs.WellLogs.ToList();
            var lognames = well.Logs.WellLogs.Select(w => w.Name).ToList();

            comboBoxVP.Items.AddRange(lognames.ToArray());
            comboBoxVS.Items.AddRange(lognames.ToArray());
            comboBoxRho.Items.AddRange(lognames.ToArray());
            comboBoxGR.Items.AddRange(lognames.ToArray());
            comboBoxCali.Items.AddRange(lognames.ToArray());
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

            if (selectedVP != "" && selectedVS != "" && selectedRho != "" && selectedGR != "" && selectedCali != "")
            {
                // selecionar as janelas
                SelectedLogs.Add("VP", selectedVP);
                SelectedLogs.Add("VS", selectedVS);
                SelectedLogs.Add("Rho", selectedRho);
                SelectedLogs.Add("GR", selectedGR);
                SelectedLogs.Add("Cali", selectedCali);
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
    }
}
