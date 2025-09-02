using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dlseis_well_tie_petrel
{
    public partial class handler_path_window : Form
    {
        private List<string> SelectedColumns = new List<string>();

        public handler_path_window(string[] columns)
        {
            InitializeComponent();
 
            // Creating checkboxess
            foreach (var col in columns)
            {
                var cb = new CheckBox { Text = col, AutoSize = true };
                flowLayoutPanelPath.Controls.Add(cb);
                comboBoxInclination.Items.Add(col);
                comboBoxMD.Items.Add(col);
            }
        }

        private void buttonAcceptHandlerPath_Click(object sender, EventArgs e)
        {
            if (comboBoxMD.SelectedItem == null || comboBoxInclination.SelectedItem == null)
            {
                MessageBox.Show(
                    "You must select both a depth column and an inclination column.",
                    "Missing Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            SelectedColumns.Clear();
            // sei que sempre esses dois serão os dois primeiros itens da lista :)
            SelectedColumns.Add(comboBoxMD.SelectedItem.ToString());
            SelectedColumns.Add(comboBoxInclination.SelectedItem.ToString());

            foreach (var cb in flowLayoutPanelPath.Controls.OfType<CheckBox>())
            {
                if (cb.Checked)
                {
                    SelectedColumns.Add(cb.Text);
                }
            }

            if (SelectedColumns.Count() >= 2)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(
                    "You must at least select an depth and inclination headers"
                );
            }

        }

        public List<string> getSelectedColumns()
        {
            return SelectedColumns;
        }
    }
}
