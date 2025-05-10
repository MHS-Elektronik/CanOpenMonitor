namespace CanMonitor
{
    partial class InfoLogDocument
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
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_info
            // 
            this.textBox_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_info.Location = new System.Drawing.Point(0, 0);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ReadOnly = true;
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_info.Size = new System.Drawing.Size(600, 366);
            this.textBox_info.TabIndex = 1;
            // 
            // InfoLogDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.textBox_info);
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "InfoLogDocument";
            this.Text = "InfoLogDocument";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_info;
    }
}