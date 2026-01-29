namespace dlseis_well_tie_petrel
{
    partial class handler_table_window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanelTable = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonAcceptHanlderTable = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTWT = new System.Windows.Forms.ComboBox();
            this.labelTWT = new System.Windows.Forms.Label();
            this.comboBoxTVDSS = new System.Windows.Forms.ComboBox();
            this.labelTVDSS = new System.Windows.Forms.Label();
            this.checkBoxOWT = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // flowLayoutPanelTable
            // 
            this.flowLayoutPanelTable.Location = new System.Drawing.Point(13, 122);
            this.flowLayoutPanelTable.Name = "flowLayoutPanelTable";
            this.flowLayoutPanelTable.Size = new System.Drawing.Size(200, 91);
            this.flowLayoutPanelTable.TabIndex = 0;
            // 
            // buttonAcceptHanlderTable
            // 
            this.buttonAcceptHanlderTable.Location = new System.Drawing.Point(13, 219);
            this.buttonAcceptHanlderTable.Name = "buttonAcceptHanlderTable";
            this.buttonAcceptHanlderTable.Size = new System.Drawing.Size(75, 23);
            this.buttonAcceptHanlderTable.TabIndex = 1;
            this.buttonAcceptHanlderTable.Text = "Ok";
            this.buttonAcceptHanlderTable.UseVisualStyleBackColor = true;
            this.buttonAcceptHanlderTable.Click += new System.EventHandler(this.buttonAcceptHanlderTable_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Extra Logs";
            // 
            // comboBoxTWT
            // 
            this.comboBoxTWT.FormattingEnabled = true;
            this.comboBoxTWT.Location = new System.Drawing.Point(15, 51);
            this.comboBoxTWT.Name = "comboBoxTWT";
            this.comboBoxTWT.Size = new System.Drawing.Size(81, 21);
            this.comboBoxTWT.TabIndex = 3;
            // 
            // labelTWT
            // 
            this.labelTWT.AutoSize = true;
            this.labelTWT.Location = new System.Drawing.Point(12, 35);
            this.labelTWT.Name = "labelTWT";
            this.labelTWT.Size = new System.Drawing.Size(80, 13);
            this.labelTWT.TabIndex = 4;
            this.labelTWT.Text = "Downward time";
            // 
            // comboBoxTVDSS
            // 
            this.comboBoxTVDSS.FormattingEnabled = true;
            this.comboBoxTVDSS.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBoxTVDSS.Location = new System.Drawing.Point(191, 51);
            this.comboBoxTVDSS.Name = "comboBoxTVDSS";
            this.comboBoxTVDSS.Size = new System.Drawing.Size(81, 21);
            this.comboBoxTVDSS.TabIndex = 5;
            // 
            // labelTVDSS
            // 
            this.labelTVDSS.AutoSize = true;
            this.labelTVDSS.Location = new System.Drawing.Point(198, 35);
            this.labelTVDSS.Name = "labelTVDSS";
            this.labelTVDSS.Size = new System.Drawing.Size(74, 13);
            this.labelTVDSS.TabIndex = 6;
            this.labelTVDSS.Text = "Vertical Depth";
            // 
            // checkBoxOWT
            // 
            this.checkBoxOWT.AutoSize = true;
            this.checkBoxOWT.Location = new System.Drawing.Point(15, 79);
            this.checkBoxOWT.Name = "checkBoxOWT";
            this.checkBoxOWT.Size = new System.Drawing.Size(108, 17);
            this.checkBoxOWT.TabIndex = 7;
            this.checkBoxOWT.Text = "Is One-Way Time";
            this.checkBoxOWT.UseVisualStyleBackColor = true;
            // 
            // handler_table_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.checkBoxOWT);
            this.Controls.Add(this.labelTVDSS);
            this.Controls.Add(this.comboBoxTVDSS);
            this.Controls.Add(this.labelTWT);
            this.Controls.Add(this.comboBoxTWT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAcceptHanlderTable);
            this.Controls.Add(this.flowLayoutPanelTable);
            this.Name = "handler_table_window";
            this.Text = "Time Depth Table Handler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTable;
        private System.Windows.Forms.Button buttonAcceptHanlderTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxTWT;
        private System.Windows.Forms.Label labelTWT;
        private System.Windows.Forms.ComboBox comboBoxTVDSS;
        private System.Windows.Forms.Label labelTVDSS;
        private System.Windows.Forms.CheckBox checkBoxOWT;
    }
}