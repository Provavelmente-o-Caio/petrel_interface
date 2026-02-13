using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace dlseis_well_tie_petrel
{
    public partial class handler_table_window : Form
    {
        private List<string> SelectedColumns = new List<string>();

        public handler_table_window(string[] columns)
        {
            InitializeComponent();

            foreach (var col in columns)
            {
                var cb = new CheckBox { Text = col, AutoSize = true };
                flowLayoutPanelTable.Controls.Add(cb);
                comboBoxTWT.Items.Add(col);
                comboBoxTVDSS.Items.Add(col);
            }
        }

        private void buttonAcceptHanlderTable_Click(object sender, EventArgs e)
        {
            if (comboBoxTWT.SelectedItem == null || comboBoxTVDSS.SelectedItem == null)
            {
                MessageBox.Show("You must select both a two-way time columns and a true vertical depth column.",
                    "Missing Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            SelectedColumns.Clear();
            SelectedColumns.Add(comboBoxTWT.SelectedItem.ToString());
            SelectedColumns.Add(comboBoxTVDSS.SelectedItem.ToString());

            foreach (var cb in flowLayoutPanelTable.Controls.OfType<CheckBox>())
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
            } else
            {
                MessageBox.Show("You must at least select a TWT and TVDSS headers");
            }

        }

        public List<string> getSelectedColumns()
        {
            return SelectedColumns;
        }

        public bool getOWT()
        {
            return checkBoxOWT.Checked;
        }
    }
}
