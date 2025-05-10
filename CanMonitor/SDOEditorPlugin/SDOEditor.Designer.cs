namespace SDOEditorPlugin
{
    partial class SDOEditor
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
            this.numericUpDown_node = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.WriteSdoBtn = new System.Windows.Forms.Button();
            this.ReadAllCheckBox = new System.Windows.Forms.CheckBox();
            this.ReadSelCheckBox = new System.Windows.Forms.CheckBox();
            this.AutoRepeatCheckBox = new System.Windows.Forms.CheckBox();
            this.CustomDeleteSelBtn = new System.Windows.Forms.Button();
            this.CustomViewCheckBox = new System.Windows.Forms.CheckBox();
            this.NormalViewCheckBox = new System.Windows.Forms.CheckBox();
            this.CustomAddSelBtn = new System.Windows.Forms.Button();
            this.StopFlushBtn = new System.Windows.Forms.Button();
            this.label_sdo_queue_size = new System.Windows.Forms.Label();
            this.checkBox_useronly = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxtype = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_node)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDown_node
            // 
            this.numericUpDown_node.Hexadecimal = true;
            this.numericUpDown_node.Location = new System.Drawing.Point(46, 8);
            this.numericUpDown_node.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDown_node.Name = "numericUpDown_node";
            this.numericUpDown_node.Size = new System.Drawing.Size(71, 20);
            this.numericUpDown_node.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Node";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader6,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader7});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 97);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(985, 610);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Index";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Sub";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 251;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Datatype";
            this.columnHeader3.Width = 148;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Default";
            this.columnHeader4.Width = 164;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Current";
            this.columnHeader5.Width = 155;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "DCF";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(991, 686);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.WriteSdoBtn);
            this.panel1.Controls.Add(this.ReadAllCheckBox);
            this.panel1.Controls.Add(this.ReadSelCheckBox);
            this.panel1.Controls.Add(this.AutoRepeatCheckBox);
            this.panel1.Controls.Add(this.CustomDeleteSelBtn);
            this.panel1.Controls.Add(this.CustomViewCheckBox);
            this.panel1.Controls.Add(this.NormalViewCheckBox);
            this.panel1.Controls.Add(this.CustomAddSelBtn);
            this.panel1.Controls.Add(this.StopFlushBtn);
            this.panel1.Controls.Add(this.label_sdo_queue_size);
            this.panel1.Controls.Add(this.checkBox_useronly);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.comboBoxtype);
            this.panel1.Controls.Add(this.numericUpDown_node);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(985, 88);
            this.panel1.TabIndex = 6;
            // 
            // WriteSdoBtn
            // 
            this.WriteSdoBtn.Location = new System.Drawing.Point(204, 61);
            this.WriteSdoBtn.Name = "WriteSdoBtn";
            this.WriteSdoBtn.Size = new System.Drawing.Size(104, 23);
            this.WriteSdoBtn.TabIndex = 24;
            this.WriteSdoBtn.Text = "Write";
            this.WriteSdoBtn.UseVisualStyleBackColor = true;
            this.WriteSdoBtn.Click += new System.EventHandler(this.WriteSdoBtn_Click);
            // 
            // ReadAllCheckBox
            // 
            this.ReadAllCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.ReadAllCheckBox.Location = new System.Drawing.Point(331, 33);
            this.ReadAllCheckBox.Name = "ReadAllCheckBox";
            this.ReadAllCheckBox.Size = new System.Drawing.Size(134, 24);
            this.ReadAllCheckBox.TabIndex = 23;
            this.ReadAllCheckBox.Text = "Read all";
            this.ReadAllCheckBox.UseVisualStyleBackColor = true;
            this.ReadAllCheckBox.CheckedChanged += new System.EventHandler(this.ReadAllCheckBox_CheckedChanged);
            // 
            // ReadSelCheckBox
            // 
            this.ReadSelCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.ReadSelCheckBox.Location = new System.Drawing.Point(331, 6);
            this.ReadSelCheckBox.Name = "ReadSelCheckBox";
            this.ReadSelCheckBox.Size = new System.Drawing.Size(134, 24);
            this.ReadSelCheckBox.TabIndex = 22;
            this.ReadSelCheckBox.Text = "Read selected";
            this.ReadSelCheckBox.UseVisualStyleBackColor = true;
            this.ReadSelCheckBox.CheckedChanged += new System.EventHandler(this.ReadSelCheckBox_CheckedChanged);
            // 
            // AutoRepeatCheckBox
            // 
            this.AutoRepeatCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.AutoRepeatCheckBox.Location = new System.Drawing.Point(331, 60);
            this.AutoRepeatCheckBox.Name = "AutoRepeatCheckBox";
            this.AutoRepeatCheckBox.Size = new System.Drawing.Size(134, 24);
            this.AutoRepeatCheckBox.TabIndex = 21;
            this.AutoRepeatCheckBox.Text = "Auto Repeat Mode";
            this.AutoRepeatCheckBox.UseVisualStyleBackColor = true;
            this.AutoRepeatCheckBox.CheckedChanged += new System.EventHandler(this.AutoRepeatCheckBox_CheckedChanged);
            // 
            // CustomDeleteSelBtn
            // 
            this.CustomDeleteSelBtn.Location = new System.Drawing.Point(580, 60);
            this.CustomDeleteSelBtn.Name = "CustomDeleteSelBtn";
            this.CustomDeleteSelBtn.Size = new System.Drawing.Size(123, 23);
            this.CustomDeleteSelBtn.TabIndex = 20;
            this.CustomDeleteSelBtn.Text = "Delete selection";
            this.CustomDeleteSelBtn.UseVisualStyleBackColor = true;
            this.CustomDeleteSelBtn.Click += new System.EventHandler(this.CustomDeleteSelBtn_Click);
            // 
            // CustomViewCheckBox
            // 
            this.CustomViewCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.CustomViewCheckBox.Location = new System.Drawing.Point(204, 33);
            this.CustomViewCheckBox.Name = "CustomViewCheckBox";
            this.CustomViewCheckBox.Size = new System.Drawing.Size(104, 24);
            this.CustomViewCheckBox.TabIndex = 19;
            this.CustomViewCheckBox.Text = "Custom View";
            this.CustomViewCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CustomViewCheckBox.UseVisualStyleBackColor = true;
            this.CustomViewCheckBox.CheckedChanged += new System.EventHandler(this.CustomViewCheckBox_CheckedChanged);
            // 
            // NormalViewCheckBox
            // 
            this.NormalViewCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.NormalViewCheckBox.Checked = true;
            this.NormalViewCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NormalViewCheckBox.Location = new System.Drawing.Point(204, 6);
            this.NormalViewCheckBox.Name = "NormalViewCheckBox";
            this.NormalViewCheckBox.Size = new System.Drawing.Size(104, 24);
            this.NormalViewCheckBox.TabIndex = 18;
            this.NormalViewCheckBox.Text = "Normal View";
            this.NormalViewCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NormalViewCheckBox.UseVisualStyleBackColor = true;
            this.NormalViewCheckBox.CheckedChanged += new System.EventHandler(this.NormalViewCheckBox_CheckedChanged);
            // 
            // CustomAddSelBtn
            // 
            this.CustomAddSelBtn.Location = new System.Drawing.Point(580, 7);
            this.CustomAddSelBtn.Name = "CustomAddSelBtn";
            this.CustomAddSelBtn.Size = new System.Drawing.Size(123, 49);
            this.CustomAddSelBtn.TabIndex = 16;
            this.CustomAddSelBtn.Text = "Add selection to custom";
            this.CustomAddSelBtn.UseVisualStyleBackColor = true;
            this.CustomAddSelBtn.Click += new System.EventHandler(this.CustomAddSelBtn_Click);
            // 
            // StopFlushBtn
            // 
            this.StopFlushBtn.Location = new System.Drawing.Point(470, 6);
            this.StopFlushBtn.Margin = new System.Windows.Forms.Padding(2);
            this.StopFlushBtn.Name = "StopFlushBtn";
            this.StopFlushBtn.Size = new System.Drawing.Size(70, 77);
            this.StopFlushBtn.TabIndex = 12;
            this.StopFlushBtn.Text = "STOP\r\nFlush SDO queue";
            this.StopFlushBtn.UseVisualStyleBackColor = true;
            this.StopFlushBtn.Click += new System.EventHandler(this.StopFlushBtn_Click);
            // 
            // label_sdo_queue_size
            // 
            this.label_sdo_queue_size.AutoSize = true;
            this.label_sdo_queue_size.Location = new System.Drawing.Point(760, 15);
            this.label_sdo_queue_size.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_sdo_queue_size.Name = "label_sdo_queue_size";
            this.label_sdo_queue_size.Size = new System.Drawing.Size(100, 13);
            this.label_sdo_queue_size.TabIndex = 11;
            this.label_sdo_queue_size.Text = "SDO Queue Size: 0";
            // 
            // checkBox_useronly
            // 
            this.checkBox_useronly.AutoSize = true;
            this.checkBox_useronly.Checked = true;
            this.checkBox_useronly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_useronly.Location = new System.Drawing.Point(9, 68);
            this.checkBox_useronly.Name = "checkBox_useronly";
            this.checkBox_useronly.Size = new System.Drawing.Size(147, 17);
            this.checkBox_useronly.TabIndex = 10;
            this.checkBox_useronly.Text = "Manufacture specific only";
            this.checkBox_useronly.UseVisualStyleBackColor = true;
            this.checkBox_useronly.CheckedChanged += new System.EventHandler(this.checkBox_useronly_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Type";
            // 
            // comboBoxtype
            // 
            this.comboBoxtype.FormattingEnabled = true;
            this.comboBoxtype.Items.AddRange(new object[] {
            "EEPROM",
            "PERSIST_COMM",
            "RAM",
            "ROM",
            "ALL"});
            this.comboBoxtype.Location = new System.Drawing.Point(46, 35);
            this.comboBoxtype.Name = "comboBoxtype";
            this.comboBoxtype.Size = new System.Drawing.Size(143, 21);
            this.comboBoxtype.TabIndex = 7;
            this.comboBoxtype.Text = "ALL";
            this.comboBoxtype.SelectedIndexChanged += new System.EventHandler(this.comboBoxtype_SelectedIndexChanged);
            // 
            // SDOEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(991, 686);
            this.Controls.Add(this.tableLayoutPanel1);
            this.HideOnClose = true;
            this.Name = "SDOEditor";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.TabText = "SDO Editor";
            this.Text = "Device OD Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SDOEditor_FormClosed);
            this.Load += new System.EventHandler(this.SDOEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_node)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown numericUpDown_node;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxtype;
        private System.Windows.Forms.CheckBox checkBox_useronly;
        private System.Windows.Forms.Button StopFlushBtn;
        private System.Windows.Forms.Label label_sdo_queue_size;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button CustomAddSelBtn;
        private System.Windows.Forms.CheckBox CustomViewCheckBox;
        private System.Windows.Forms.CheckBox NormalViewCheckBox;
        private System.Windows.Forms.Button CustomDeleteSelBtn;
        private System.Windows.Forms.CheckBox ReadAllCheckBox;
        private System.Windows.Forms.CheckBox ReadSelCheckBox;
        private System.Windows.Forms.CheckBox AutoRepeatCheckBox;
        private System.Windows.Forms.Button WriteSdoBtn;
    }
}