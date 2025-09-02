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
            }
        }

        private void buttonAcceptHanlderTable_Click(object sender, EventArgs e)
        {
            SelectedColumns.Clear();
            this.DialogResult = DialogResult.None;

            foreach (var cb in flowLayoutPanelTable.Controls.OfType<CheckBox>())
            {
                if (cb.Checked)
                {
                    SelectedColumns.Add(cb.Text);
                }
            }

            if (SelectedColumns.Count() > 0)
            {
                this.DialogResult = DialogResult.OK;
            }

            this.Close();
        }

        public List<string> getSelectedColumns()
        {
            return SelectedColumns;
        }
    }
}
