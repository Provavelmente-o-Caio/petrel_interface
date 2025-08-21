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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Label Teste";
            // 
            // flowLayoutPanelPath
            // 
            this.flowLayoutPanelPath.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelPath.Location = new System.Drawing.Point(12, 17);
            this.flowLayoutPanelPath.Name = "flowLayoutPanelPath";
            this.flowLayoutPanelPath.Size = new System.Drawing.Size(260, 305);
            this.flowLayoutPanelPath.TabIndex = 1;
            // 
            // buttonAcceptHandlerPath
            // 
            this.buttonAcceptHandlerPath.Location = new System.Drawing.Point(12, 328);
            this.buttonAcceptHandlerPath.Name = "buttonAcceptHandlerPath";
            this.buttonAcceptHandlerPath.Size = new System.Drawing.Size(75, 23);
            this.buttonAcceptHandlerPath.TabIndex = 2;
            this.buttonAcceptHandlerPath.Text = "Accept";
            this.buttonAcceptHandlerPath.UseVisualStyleBackColor = true;
            this.buttonAcceptHandlerPath.Click += new System.EventHandler(this.buttonAcceptHandlerPath_Click);
            // 
            // handler_path_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 363);
            this.Controls.Add(this.buttonAcceptHandlerPath);
            this.Controls.Add(this.flowLayoutPanelPath);
            this.Controls.Add(this.label1);
            this.Name = "handler_path_window";
            this.Text = "handler_path_window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPath;
        private System.Windows.Forms.Button buttonAcceptHandlerPath;
    }
}