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
        public List<string> SelectedColumns { get; private set; } = new List<string>();

        public handler_path_window(string[] columns)
        {
            InitializeComponent();
 
            // Creating checkboxess
            foreach (var col in columns)
            {
                var cb = new CheckBox { Text = col, AutoSize = true };
                flowLayoutPanelPath.Controls.Add(cb);
            }
        }

        private void buttonAcceptHandlerPath_Click(object sender, EventArgs e)
        {
            SelectedColumns.Clear();

            foreach (var cb in flowLayoutPanelPath.Controls.OfType<CheckBox>())
            {
                if (cb.Checked)
                {
                    SelectedColumns.Add(cb.Text);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
