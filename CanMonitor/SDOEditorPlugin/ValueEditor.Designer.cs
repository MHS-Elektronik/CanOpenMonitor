namespace SDOEditorPlugin
{
    partial class ValueEditor
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_index = new System.Windows.Forms.Label();
            this.label_sub = new System.Windows.Forms.Label();
            this.label_name = new System.Windows.Forms.Label();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.UpdateCloseBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label_default = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.NewValueEntry = new System.Windows.Forms.TextBox();
            this.UpdateBtn = new System.Windows.Forms.Button();
            this.label_length = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.CurrentValueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Index";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sub Index";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Name";
            // 
            // label_index
            // 
            this.label_index.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_index.ForeColor = System.Drawing.Color.Blue;
            this.label_index.Location = new System.Drawing.Point(70, 9);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(60, 18);
            this.label_index.TabIndex = 3;
            this.label_index.Text = "0x0000";
            this.label_index.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_sub
            // 
            this.label_sub.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_sub.ForeColor = System.Drawing.Color.Blue;
            this.label_sub.Location = new System.Drawing.Point(280, 9);
            this.label_sub.Name = "label_sub";
            this.label_sub.Size = new System.Drawing.Size(40, 18);
            this.label_sub.TabIndex = 4;
            this.label_sub.Text = "0x00";
            this.label_sub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_name
            // 
            this.label_name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_name.ForeColor = System.Drawing.Color.Blue;
            this.label_name.Location = new System.Drawing.Point(70, 36);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(372, 18);
            this.label_name.TabIndex = 5;
            this.label_name.Text = "------";
            this.label_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_desc
            // 
            this.textBox_desc.Cursor = System.Windows.Forms.Cursors.No;
            this.textBox_desc.ForeColor = System.Drawing.Color.Blue;
            this.textBox_desc.Location = new System.Drawing.Point(70, 63);
            this.textBox_desc.Multiline = true;
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.ReadOnly = true;
            this.textBox_desc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_desc.Size = new System.Drawing.Size(372, 87);
            this.textBox_desc.TabIndex = 6;
            // 
            // UpdateCloseBtn
            // 
            this.UpdateCloseBtn.Location = new System.Drawing.Point(82, 255);
            this.UpdateCloseBtn.Name = "UpdateCloseBtn";
            this.UpdateCloseBtn.Size = new System.Drawing.Size(140, 32);
            this.UpdateCloseBtn.TabIndex = 7;
            this.UpdateCloseBtn.Text = "Update and close";
            this.UpdateCloseBtn.UseVisualStyleBackColor = true;
            this.UpdateCloseBtn.Click += new System.EventHandler(this.UpdateCloseBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Default";
            // 
            // label_default
            // 
            this.label_default.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_default.ForeColor = System.Drawing.Color.Blue;
            this.label_default.Location = new System.Drawing.Point(70, 159);
            this.label_default.Name = "label_default";
            this.label_default.Size = new System.Drawing.Size(90, 18);
            this.label_default.TabIndex = 9;
            this.label_default.Text = "Default";
            this.label_default.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "New Value";
            // 
            // NewValueEntry
            // 
            this.NewValueEntry.Location = new System.Drawing.Point(70, 213);
            this.NewValueEntry.Name = "NewValueEntry";
            this.NewValueEntry.Size = new System.Drawing.Size(223, 20);
            this.NewValueEntry.TabIndex = 11;
            this.NewValueEntry.TextChanged += new System.EventHandler(this.NewValueEntry_TextChanged);
            // 
            // UpdateBtn
            // 
            this.UpdateBtn.Location = new System.Drawing.Point(246, 255);
            this.UpdateBtn.Name = "UpdateBtn";
            this.UpdateBtn.Size = new System.Drawing.Size(140, 32);
            this.UpdateBtn.TabIndex = 12;
            this.UpdateBtn.Text = "Update";
            this.UpdateBtn.UseVisualStyleBackColor = true;
            this.UpdateBtn.Click += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // label_length
            // 
            this.label_length.AutoSize = true;
            this.label_length.Location = new System.Drawing.Point(322, 217);
            this.label_length.Name = "label_length";
            this.label_length.Size = new System.Drawing.Size(13, 13);
            this.label_length.TabIndex = 14;
            this.label_length.Text = "--";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Description";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 189);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Current";
            // 
            // CurrentValueLabel
            // 
            this.CurrentValueLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CurrentValueLabel.ForeColor = System.Drawing.Color.Blue;
            this.CurrentValueLabel.Location = new System.Drawing.Point(70, 186);
            this.CurrentValueLabel.Name = "CurrentValueLabel";
            this.CurrentValueLabel.Size = new System.Drawing.Size(223, 18);
            this.CurrentValueLabel.TabIndex = 19;
            this.CurrentValueLabel.Text = "---";
            // 
            // ValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 298);
            this.Controls.Add(this.CurrentValueLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label_length);
            this.Controls.Add(this.UpdateBtn);
            this.Controls.Add(this.NewValueEntry);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label_default);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.UpdateCloseBtn);
            this.Controls.Add(this.textBox_desc);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.label_sub);
            this.Controls.Add(this.label_index);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ValueEditor";
            this.Text = "Write SDO Value";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Label label_sub;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Button UpdateCloseBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_default;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox NewValueEntry;
        private System.Windows.Forms.Button UpdateBtn;
        private System.Windows.Forms.Label label_length;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label CurrentValueLabel;
    }
}