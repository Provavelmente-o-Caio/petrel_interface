namespace dlseis_well_tie_petrel
{
    partial class well_tie_window
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
            this.button_tie = new System.Windows.Forms.Button();
            this.label_select = new System.Windows.Forms.Label();
            this.label_logs = new System.Windows.Forms.Label();
            this.label_seismic = new System.Windows.Forms.Label();
            this.label_well_path = new System.Windows.Forms.Label();
            this.label_table = new System.Windows.Forms.Label();
            this.textBox_table = new System.Windows.Forms.TextBox();
            this.textBox_well_path = new System.Windows.Forms.TextBox();
            this.textBox_seismic = new System.Windows.Forms.TextBox();
            this.textBox_logs = new System.Windows.Forms.TextBox();
            this.button_select_table = new System.Windows.Forms.Button();
            this.button_select_well_path = new System.Windows.Forms.Button();
            this.button_select_seismic = new System.Windows.Forms.Button();
            this.button_select_logs = new System.Windows.Forms.Button();
            this.openFileDialog_logs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_seismic = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_well_path = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_table = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // button_tie
            // 
            this.button_tie.Location = new System.Drawing.Point(175, 197);
            this.button_tie.Name = "button_tie";
            this.button_tie.Size = new System.Drawing.Size(82, 23);
            this.button_tie.TabIndex = 2;
            this.button_tie.Text = "Well Tie";
            this.button_tie.UseVisualStyleBackColor = true;
            this.button_tie.Click += new System.EventHandler(this.button_tie_Click);
            // 
            // label_select
            // 
            this.label_select.AutoSize = true;
            this.label_select.Location = new System.Drawing.Point(12, 9);
            this.label_select.Name = "label_select";
            this.label_select.Size = new System.Drawing.Size(245, 13);
            this.label_select.TabIndex = 4;
            this.label_select.Text = "Select the well you wish to perform the well tie with\r\n";
            // 
            // label_logs
            // 
            this.label_logs.AutoSize = true;
            this.label_logs.Location = new System.Drawing.Point(15, 54);
            this.label_logs.Name = "label_logs";
            this.label_logs.Size = new System.Drawing.Size(84, 13);
            this.label_logs.TabIndex = 5;
            this.label_logs.Text = "Load Well Logs:";
            // 
            // label_seismic
            // 
            this.label_seismic.AutoSize = true;
            this.label_seismic.Location = new System.Drawing.Point(15, 90);
            this.label_seismic.Name = "label_seismic";
            this.label_seismic.Size = new System.Drawing.Size(73, 13);
            this.label_seismic.TabIndex = 6;
            this.label_seismic.Text = "Load Seismic:";
            // 
            // label_well_path
            // 
            this.label_well_path.AutoSize = true;
            this.label_well_path.Location = new System.Drawing.Point(15, 124);
            this.label_well_path.Name = "label_well_path";
            this.label_well_path.Size = new System.Drawing.Size(83, 13);
            this.label_well_path.TabIndex = 7;
            this.label_well_path.Text = "Load Well Path:";
            // 
            // label_table
            // 
            this.label_table.AutoSize = true;
            this.label_table.Location = new System.Drawing.Point(15, 158);
            this.label_table.Name = "label_table";
            this.label_table.Size = new System.Drawing.Size(122, 13);
            this.label_table.TabIndex = 8;
            this.label_table.Text = "Load Time Depth Table:";
            // 
            // textBox_table
            // 
            this.textBox_table.Location = new System.Drawing.Point(143, 155);
            this.textBox_table.Name = "textBox_table";
            this.textBox_table.Size = new System.Drawing.Size(216, 20);
            this.textBox_table.TabIndex = 9;
            // 
            // textBox_well_path
            // 
            this.textBox_well_path.Location = new System.Drawing.Point(143, 117);
            this.textBox_well_path.Name = "textBox_well_path";
            this.textBox_well_path.Size = new System.Drawing.Size(216, 20);
            this.textBox_well_path.TabIndex = 10;
            // 
            // textBox_seismic
            // 
            this.textBox_seismic.Location = new System.Drawing.Point(143, 83);
            this.textBox_seismic.Name = "textBox_seismic";
            this.textBox_seismic.Size = new System.Drawing.Size(216, 20);
            this.textBox_seismic.TabIndex = 11;
            // 
            // textBox_logs
            // 
            this.textBox_logs.Location = new System.Drawing.Point(143, 47);
            this.textBox_logs.Name = "textBox_logs";
            this.textBox_logs.Size = new System.Drawing.Size(216, 20);
            this.textBox_logs.TabIndex = 12;
            // 
            // button_select_table
            // 
            this.button_select_table.Location = new System.Drawing.Point(365, 153);
            this.button_select_table.Name = "button_select_table";
            this.button_select_table.Size = new System.Drawing.Size(57, 23);
            this.button_select_table.TabIndex = 13;
            this.button_select_table.Text = "Select";
            this.button_select_table.UseVisualStyleBackColor = true;
            this.button_select_table.Click += new System.EventHandler(this.button_select_table_Click);
            // 
            // button_select_well_path
            // 
            this.button_select_well_path.Location = new System.Drawing.Point(365, 119);
            this.button_select_well_path.Name = "button_select_well_path";
            this.button_select_well_path.Size = new System.Drawing.Size(57, 23);
            this.button_select_well_path.TabIndex = 14;
            this.button_select_well_path.Text = "Select";
            this.button_select_well_path.UseVisualStyleBackColor = true;
            this.button_select_well_path.Click += new System.EventHandler(this.button_select_well_path_Click);
            // 
            // button_select_seismic
            // 
            this.button_select_seismic.Location = new System.Drawing.Point(365, 83);
            this.button_select_seismic.Name = "button_select_seismic";
            this.button_select_seismic.Size = new System.Drawing.Size(57, 23);
            this.button_select_seismic.TabIndex = 15;
            this.button_select_seismic.Text = "Select";
            this.button_select_seismic.UseVisualStyleBackColor = true;
            this.button_select_seismic.Click += new System.EventHandler(this.button_select_seismic_Click);
            // 
            // button_select_logs
            // 
            this.button_select_logs.Location = new System.Drawing.Point(365, 47);
            this.button_select_logs.Name = "button_select_logs";
            this.button_select_logs.Size = new System.Drawing.Size(57, 23);
            this.button_select_logs.TabIndex = 16;
            this.button_select_logs.Text = "Select";
            this.button_select_logs.UseVisualStyleBackColor = true;
            this.button_select_logs.Click += new System.EventHandler(this.button_select_logs_Click);
            // 
            // openFileDialog_logs
            // 
            this.openFileDialog_logs.Filter = "Las logs (*.las)|*.las|All Files (*.*)|*.*";
            this.openFileDialog_logs.Title = "Select a LAS file";
            // 
            // openFileDialog_seismic
            // 
            this.openFileDialog_seismic.Filter = "Seismic Data (*.sgy)|*.sgy|All Files (*.*)|*.*";
            this.openFileDialog_seismic.Title = "Select a SGY file";
            // 
            // openFileDialog_well_path
            // 
            this.openFileDialog_well_path.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            this.openFileDialog_well_path.Title = "Open a Well Path (checkshot) file";
            // 
            // openFileDialog_table
            // 
            this.openFileDialog_table.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            this.openFileDialog_table.Title = "Open a Time Depth Table file";
            // 
            // well_tie_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 232);
            this.Controls.Add(this.button_select_logs);
            this.Controls.Add(this.button_select_seismic);
            this.Controls.Add(this.button_select_well_path);
            this.Controls.Add(this.button_select_table);
            this.Controls.Add(this.textBox_logs);
            this.Controls.Add(this.textBox_seismic);
            this.Controls.Add(this.textBox_well_path);
            this.Controls.Add(this.textBox_table);
            this.Controls.Add(this.label_table);
            this.Controls.Add(this.label_well_path);
            this.Controls.Add(this.label_seismic);
            this.Controls.Add(this.label_logs);
            this.Controls.Add(this.label_select);
            this.Controls.Add(this.button_tie);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "well_tie_window";
            this.Text = "Seismic to Well Tie";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_tie;
        private System.Windows.Forms.Label label_select;
        private System.Windows.Forms.Label label_logs;
        private System.Windows.Forms.Label label_seismic;
        private System.Windows.Forms.Label label_well_path;
        private System.Windows.Forms.Label label_table;
        private System.Windows.Forms.TextBox textBox_table;
        private System.Windows.Forms.TextBox textBox_well_path;
        private System.Windows.Forms.TextBox textBox_seismic;
        private System.Windows.Forms.TextBox textBox_logs;
        private System.Windows.Forms.Button button_select_table;
        private System.Windows.Forms.Button button_select_well_path;
        private System.Windows.Forms.Button button_select_seismic;
        private System.Windows.Forms.Button button_select_logs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_logs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_seismic;
        private System.Windows.Forms.OpenFileDialog openFileDialog_well_path;
        private System.Windows.Forms.OpenFileDialog openFileDialog_table;
    }
}