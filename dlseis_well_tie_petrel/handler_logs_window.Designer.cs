namespace dlseis_well_tie_petrel
{
    partial class handler_logs_window
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelExplain = new System.Windows.Forms.Label();
            this.labelVP = new System.Windows.Forms.Label();
            this.labelVS = new System.Windows.Forms.Label();
            this.labelRho = new System.Windows.Forms.Label();
            this.labelGR = new System.Windows.Forms.Label();
            this.labelCali = new System.Windows.Forms.Label();
            this.buttonHandlerLogs = new System.Windows.Forms.Button();
            this.comboBoxVP = new System.Windows.Forms.ComboBox();
            this.comboBoxVS = new System.Windows.Forms.ComboBox();
            this.comboBoxRho = new System.Windows.Forms.ComboBox();
            this.comboBoxGR = new System.Windows.Forms.ComboBox();
            this.comboBoxCali = new System.Windows.Forms.ComboBox();
            this.label_range_start = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.unitTextBox_range_start = new Slb.Ocean.Petrel.UI.Controls.UnitTextBox();
            this.unitTextBox_range_end = new Slb.Ocean.Petrel.UI.Controls.UnitTextBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.comboBoxUnit = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(77, 13);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Form a GridSet";
            // 
            // labelExplain
            // 
            this.labelExplain.AutoSize = true;
            this.labelExplain.Location = new System.Drawing.Point(0, 22);
            this.labelExplain.Name = "labelExplain";
            this.labelExplain.Size = new System.Drawing.Size(336, 13);
            this.labelExplain.TabIndex = 1;
            this.labelExplain.Text = "Select from the LAS logs the headers necessary to perform the well tie";
            // 
            // labelVP
            // 
            this.labelVP.AutoSize = true;
            this.labelVP.Location = new System.Drawing.Point(3, 39);
            this.labelVP.Name = "labelVP";
            this.labelVP.Size = new System.Drawing.Size(111, 13);
            this.labelVP.TabIndex = 2;
            this.labelVP.Text = "VP (Acoustic Velocity)";
            // 
            // labelVS
            // 
            this.labelVS.AutoSize = true;
            this.labelVS.Location = new System.Drawing.Point(3, 67);
            this.labelVS.Name = "labelVS";
            this.labelVS.Size = new System.Drawing.Size(98, 13);
            this.labelVS.TabIndex = 3;
            this.labelVS.Text = "VS (Shear Velocity)";
            // 
            // labelRho
            // 
            this.labelRho.AutoSize = true;
            this.labelRho.Location = new System.Drawing.Point(3, 95);
            this.labelRho.Name = "labelRho";
            this.labelRho.Size = new System.Drawing.Size(95, 13);
            this.labelRho.TabIndex = 4;
            this.labelRho.Text = "Rho (Bulk Density)";
            // 
            // labelGR
            // 
            this.labelGR.AutoSize = true;
            this.labelGR.Location = new System.Drawing.Point(3, 123);
            this.labelGR.Name = "labelGR";
            this.labelGR.Size = new System.Drawing.Size(89, 13);
            this.labelGR.TabIndex = 5;
            this.labelGR.Text = "Gamma Ray (opt)";
            // 
            // labelCali
            // 
            this.labelCali.AutoSize = true;
            this.labelCali.Location = new System.Drawing.Point(3, 151);
            this.labelCali.Name = "labelCali";
            this.labelCali.Size = new System.Drawing.Size(63, 13);
            this.labelCali.TabIndex = 6;
            this.labelCali.Text = "Caliper (opt)";
            // 
            // buttonHandlerLogs
            // 
            this.buttonHandlerLogs.Location = new System.Drawing.Point(204, 246);
            this.buttonHandlerLogs.Name = "buttonHandlerLogs";
            this.buttonHandlerLogs.Size = new System.Drawing.Size(75, 23);
            this.buttonHandlerLogs.TabIndex = 7;
            this.buttonHandlerLogs.Text = "Ok";
            this.buttonHandlerLogs.UseVisualStyleBackColor = true;
            this.buttonHandlerLogs.Click += new System.EventHandler(this.buttonHandlerLogs_Click);
            // 
            // comboBoxVP
            // 
            this.comboBoxVP.FormattingEnabled = true;
            this.comboBoxVP.Location = new System.Drawing.Point(120, 36);
            this.comboBoxVP.Name = "comboBoxVP";
            this.comboBoxVP.Size = new System.Drawing.Size(121, 21);
            this.comboBoxVP.TabIndex = 8;
            // 
            // comboBoxVS
            // 
            this.comboBoxVS.FormattingEnabled = true;
            this.comboBoxVS.Location = new System.Drawing.Point(120, 63);
            this.comboBoxVS.Name = "comboBoxVS";
            this.comboBoxVS.Size = new System.Drawing.Size(121, 21);
            this.comboBoxVS.TabIndex = 9;
            // 
            // comboBoxRho
            // 
            this.comboBoxRho.FormattingEnabled = true;
            this.comboBoxRho.Location = new System.Drawing.Point(120, 93);
            this.comboBoxRho.Name = "comboBoxRho";
            this.comboBoxRho.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRho.TabIndex = 10;
            // 
            // comboBoxGR
            // 
            this.comboBoxGR.FormattingEnabled = true;
            this.comboBoxGR.Location = new System.Drawing.Point(120, 123);
            this.comboBoxGR.Name = "comboBoxGR";
            this.comboBoxGR.Size = new System.Drawing.Size(121, 21);
            this.comboBoxGR.TabIndex = 11;
            // 
            // comboBoxCali
            // 
            this.comboBoxCali.FormattingEnabled = true;
            this.comboBoxCali.Location = new System.Drawing.Point(120, 150);
            this.comboBoxCali.Name = "comboBoxCali";
            this.comboBoxCali.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCali.TabIndex = 12;
            // 
            // label_range_start
            // 
            this.label_range_start.AutoSize = true;
            this.label_range_start.Location = new System.Drawing.Point(3, 192);
            this.label_range_start.Name = "label_range_start";
            this.label_range_start.Size = new System.Drawing.Size(64, 13);
            this.label_range_start.TabIndex = 13;
            this.label_range_start.Text = "Range Start";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 221);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Range End";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // unitTextBox_range_start
            // 
            this.unitTextBox_range_start.Location = new System.Drawing.Point(120, 185);
            this.unitTextBox_range_start.Name = "unitTextBox_range_start";
            this.unitTextBox_range_start.Size = new System.Drawing.Size(121, 20);
            this.unitTextBox_range_start.TabIndex = 15;
            // 
            // unitTextBox_range_end
            // 
            this.unitTextBox_range_end.Location = new System.Drawing.Point(120, 211);
            this.unitTextBox_range_end.Name = "unitTextBox_range_end";
            this.unitTextBox_range_end.Size = new System.Drawing.Size(121, 20);
            this.unitTextBox_range_end.TabIndex = 16;
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Location = new System.Drawing.Point(271, 39);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(26, 13);
            this.labelUnit.TabIndex = 18;
            this.labelUnit.Text = "Unit";
            // 
            // comboBoxUnit
            // 
            this.comboBoxUnit.FormattingEnabled = true;
            this.comboBoxUnit.Location = new System.Drawing.Point(303, 36);
            this.comboBoxUnit.Name = "comboBoxUnit";
            this.comboBoxUnit.Size = new System.Drawing.Size(121, 21);
            this.comboBoxUnit.TabIndex = 19;
            // 
            // handler_logs_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 299);
            this.Controls.Add(this.comboBoxUnit);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.unitTextBox_range_end);
            this.Controls.Add(this.unitTextBox_range_start);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_range_start);
            this.Controls.Add(this.comboBoxCali);
            this.Controls.Add(this.comboBoxGR);
            this.Controls.Add(this.comboBoxRho);
            this.Controls.Add(this.comboBoxVS);
            this.Controls.Add(this.comboBoxVP);
            this.Controls.Add(this.buttonHandlerLogs);
            this.Controls.Add(this.labelCali);
            this.Controls.Add(this.labelGR);
            this.Controls.Add(this.labelRho);
            this.Controls.Add(this.labelVS);
            this.Controls.Add(this.labelVP);
            this.Controls.Add(this.labelExplain);
            this.Controls.Add(this.labelTitle);
            this.Name = "handler_logs_window";
            this.Text = "LAS Log Handler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelExplain;
        private System.Windows.Forms.Label labelVP;
        private System.Windows.Forms.Label labelVS;
        private System.Windows.Forms.Label labelRho;
        private System.Windows.Forms.Label labelGR;
        private System.Windows.Forms.Label labelCali;
        private System.Windows.Forms.Button buttonHandlerLogs;
        private System.Windows.Forms.ComboBox comboBoxVP;
        private System.Windows.Forms.ComboBox comboBoxVS;
        private System.Windows.Forms.ComboBox comboBoxRho;
        private System.Windows.Forms.ComboBox comboBoxGR;
        private System.Windows.Forms.ComboBox comboBoxCali;
        private System.Windows.Forms.Label label_range_start;
        private System.Windows.Forms.Label label1;
        private Slb.Ocean.Petrel.UI.Controls.UnitTextBox unitTextBox_range_start;
        private Slb.Ocean.Petrel.UI.Controls.UnitTextBox unitTextBox_range_end;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.ComboBox comboBoxUnit;
    }
}