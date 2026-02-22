using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace dlseis_well_tie_petrel
{
    public partial class handler_path_window : Form
    {
        private List<string> SelectedColumns = new List<string>();
        private string datum;

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
            // Assuring that these will be the first ones in the list
            SelectedColumns.Add(comboBoxMD.SelectedItem.ToString());
            SelectedColumns.Add(comboBoxInclination.SelectedItem.ToString());

            foreach (var cb in flowLayoutPanelPath.Controls.OfType<CheckBox>())
            {
                if (cb.Checked && cb.Text != comboBoxMD.SelectedItem.ToString() && cb.Text != comboBoxInclination.SelectedItem.ToString())
                {
                    SelectedColumns.Add(cb.Text);
                }
            }

            if (!string.IsNullOrWhiteSpace(textBoxDatum.Text))
            {
                if (double.TryParse(textBoxDatum.Text, out double datumValue))
                {
                    datum = datumValue.ToString();
                } else
                {
                    MessageBox.Show(
                        "Datum must be a valid number.",
                        "Invalid Datum",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
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
                    "You must at least select a depth and inclination headers",
                    "Missing Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

        }

        public List<string> getSelectedColumns()
        {
            return SelectedColumns;
        }

        public string getDatum()
        {
            return datum;
        }
    }
}
