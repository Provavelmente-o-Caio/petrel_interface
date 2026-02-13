namespace dlseis_well_tie_petrel
{
    partial class handler_path_window
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
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanelPath = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonAcceptHandlerPath = new System.Windows.Forms.Button();
            this.label_extra_logs = new System.Windows.Forms.Label();
            this.label_md = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxMD = new System.Windows.Forms.ComboBox();
            this.comboBoxInclination = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDatum = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the headers for wellpath";
            // 
            // flowLayoutPanelPath
            // 
            this.flowLayoutPanelPath.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelPath.Location = new System.Drawing.Point(12, 163);
            this.flowLayoutPanelPath.Name = "flowLayoutPanelPath";
            this.flowLayoutPanelPath.Size = new System.Drawing.Size(260, 191);
            this.flowLayoutPanelPath.TabIndex = 1;
            // 
            // buttonAcceptHandlerPath
            // 
            this.buttonAcceptHandlerPath.Location = new System.Drawing.Point(12, 372);
            this.buttonAcceptHandlerPath.Name = "buttonAcceptHandlerPath";
            this.buttonAcceptHandlerPath.Size = new System.Drawing.Size(75, 23);
            this.buttonAcceptHandlerPath.TabIndex = 2;
            this.buttonAcceptHandlerPath.Text = "Accept";
            this.buttonAcceptHandlerPath.UseVisualStyleBackColor = true;
            this.buttonAcceptHandlerPath.Click += new System.EventHandler(this.buttonAcceptHandlerPath_Click);
            // 
            // label_extra_logs
            // 
            this.label_extra_logs.AutoSize = true;
            this.label_extra_logs.Location = new System.Drawing.Point(12, 144);
            this.label_extra_logs.Name = "label_extra_logs";
            this.label_extra_logs.Size = new System.Drawing.Size(57, 13);
            this.label_extra_logs.TabIndex = 3;
            this.label_extra_logs.Text = "Extra Logs";
            // 
            // label_md
            // 
            this.label_md.AutoSize = true;
            this.label_md.Location = new System.Drawing.Point(12, 35);
            this.label_md.Name = "label_md";
            this.label_md.Size = new System.Drawing.Size(86, 13);
            this.label_md.TabIndex = 4;
            this.label_md.Text = "Measured Depth";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Inclination";
            // 
            // comboBoxMD
            // 
            this.comboBoxMD.FormattingEnabled = true;
            this.comboBoxMD.Location = new System.Drawing.Point(15, 51);
            this.comboBoxMD.Name = "comboBoxMD";
            this.comboBoxMD.Size = new System.Drawing.Size(83, 21);
            this.comboBoxMD.TabIndex = 6;
            // 
            // comboBoxInclination
            // 
            this.comboBoxInclination.FormattingEnabled = true;
            this.comboBoxInclination.Location = new System.Drawing.Point(189, 51);
            this.comboBoxInclination.Name = "comboBoxInclination";
            this.comboBoxInclination.Size = new System.Drawing.Size(83, 21);
            this.comboBoxInclination.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Datum";
            // 
            // textBoxDatum
            // 
            this.textBoxDatum.Location = new System.Drawing.Point(15, 107);
            this.textBoxDatum.Name = "textBoxDatum";
            this.textBoxDatum.Size = new System.Drawing.Size(100, 20);
            this.textBoxDatum.TabIndex = 9;
            this.textBoxDatum.Text = "0";
            // 
            // handler_path_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 420);
            this.Controls.Add(this.textBoxDatum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxInclination);
            this.Controls.Add(this.comboBoxMD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_md);
            this.Controls.Add(this.label_extra_logs);
            this.Controls.Add(this.buttonAcceptHandlerPath);
            this.Controls.Add(this.flowLayoutPanelPath);
            this.Controls.Add(this.label1);
            this.Name = "handler_path_window";
            this.Text = "Wellpath Handler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPath;
        private System.Windows.Forms.Button buttonAcceptHandlerPath;
        private System.Windows.Forms.Label label_extra_logs;
        private System.Windows.Forms.Label label_md;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxMD;
        private System.Windows.Forms.ComboBox comboBoxInclination;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDatum;
    }
}