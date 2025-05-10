namespace CanMonitor
{
    partial class MainDockForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDockForm));
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ViewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.CanLogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CanEmergencyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NmtMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolBarMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusBarMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CanMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectionControlMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.StatusBarStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainToolBar = new System.Windows.Forms.ToolStrip();
            this.ToolBtnConnect = new System.Windows.Forms.ToolStripButton();
            this.ToolBtnSetup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolBtnCan = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolMenuCanClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenuCanHeartbeates = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenuCanPdo = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenuCanSdo = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenuCanNmt = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenuCanNmtec = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenuCanEmcy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenuCanAutoscroll = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolBtnNmt = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolMenuNmtClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenuNmtAutoscroll = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolBtnEmcy = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolMenuEmcyClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenuEmcyAutoscroll = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolBtnInfo = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolMenuInfoClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenuInfoAutoscroll = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.MainToolBar.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(3, 58);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.ShowDocumentIcon = true;
            this.dockPanel1.Size = new System.Drawing.Size(1017, 546);
            this.dockPanel1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.ViewMenu,
            this.CanMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1023, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveDataMenuItem,
            this.LoadDataMenuItem,
            this.toolStripSeparator2});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // SaveDataMenuItem
            // 
            this.SaveDataMenuItem.Name = "SaveDataMenuItem";
            this.SaveDataMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SaveDataMenuItem.Text = "Save Data";
            this.SaveDataMenuItem.Click += new System.EventHandler(this.saveDataToolStripMenuItem_Click);
            // 
            // LoadDataMenuItem
            // 
            this.LoadDataMenuItem.Name = "LoadDataMenuItem";
            this.LoadDataMenuItem.Size = new System.Drawing.Size(180, 22);
            this.LoadDataMenuItem.Text = "Load Data";
            this.LoadDataMenuItem.Click += new System.EventHandler(this.loadDataToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // ViewMenu
            // 
            this.ViewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CanLogMenuItem,
            this.CanEmergencyMenuItem,
            this.NmtMenuItem,
            this.InfoMenuItem,
            this.toolStripSeparator11,
            this.ToolBarMenuItem,
            this.StatusBarMenuItem});
            this.ViewMenu.Name = "ViewMenu";
            this.ViewMenu.Size = new System.Drawing.Size(44, 20);
            this.ViewMenu.Text = "View";
            // 
            // CanLogMenuItem
            // 
            this.CanLogMenuItem.Name = "CanLogMenuItem";
            this.CanLogMenuItem.Size = new System.Drawing.Size(180, 22);
            this.CanLogMenuItem.Text = "Can Log";
            this.CanLogMenuItem.Click += new System.EventHandler(this.canLogToolStripMenuItem_Click);
            // 
            // CanEmergencyMenuItem
            // 
            this.CanEmergencyMenuItem.Name = "CanEmergencyMenuItem";
            this.CanEmergencyMenuItem.Size = new System.Drawing.Size(180, 22);
            this.CanEmergencyMenuItem.Text = "Can Emergency";
            this.CanEmergencyMenuItem.Click += new System.EventHandler(this.canEmergencyToolStripMenuItem_Click);
            // 
            // NmtMenuItem
            // 
            this.NmtMenuItem.Name = "NmtMenuItem";
            this.NmtMenuItem.Size = new System.Drawing.Size(180, 22);
            this.NmtMenuItem.Text = "NMT";
            this.NmtMenuItem.Click += new System.EventHandler(this.nMTToolStripMenuItem_Click);
            // 
            // InfoMenuItem
            // 
            this.InfoMenuItem.Name = "InfoMenuItem";
            this.InfoMenuItem.Size = new System.Drawing.Size(180, 22);
            this.InfoMenuItem.Text = "Info";
            this.InfoMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(177, 6);
            // 
            // ToolBarMenuItem
            // 
            this.ToolBarMenuItem.Checked = true;
            this.ToolBarMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolBarMenuItem.Name = "ToolBarMenuItem";
            this.ToolBarMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ToolBarMenuItem.Text = "Tool Bar";
            this.ToolBarMenuItem.Click += new System.EventHandler(this.toolBarToolStripMenuItem_Click);
            // 
            // StatusBarMenuItem
            // 
            this.StatusBarMenuItem.Checked = true;
            this.StatusBarMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StatusBarMenuItem.Name = "StatusBarMenuItem";
            this.StatusBarMenuItem.Size = new System.Drawing.Size(180, 22);
            this.StatusBarMenuItem.Text = "Status Bar";
            this.StatusBarMenuItem.Click += new System.EventHandler(this.statusBarToolStripMenuItem_Click);
            // 
            // CanMenu
            // 
            this.CanMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectionControlMenuItem});
            this.CanMenu.Name = "CanMenu";
            this.CanMenu.Size = new System.Drawing.Size(44, 20);
            this.CanMenu.Text = "CAN";
            // 
            // ConnectionControlMenuItem
            // 
            this.ConnectionControlMenuItem.Name = "ConnectionControlMenuItem";
            this.ConnectionControlMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ConnectionControlMenuItem.Text = "Connection Control";
            this.ConnectionControlMenuItem.Click += new System.EventHandler(this.connectionControlToolStripMenuItem1_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.BackColor = System.Drawing.Color.White;
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBarStatusLabel});
            this.StatusBar.Location = new System.Drawing.Point(0, 631);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(1023, 22);
            this.StatusBar.TabIndex = 4;
            // 
            // StatusBarStatusLabel
            // 
            this.StatusBarStatusLabel.Name = "StatusBarStatusLabel";
            this.StatusBarStatusLabel.Size = new System.Drawing.Size(12, 17);
            this.StatusBarStatusLabel.Text = "?";
            // 
            // MainToolBar
            // 
            this.MainToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainToolBar.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.MainToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolBtnConnect,
            this.ToolBtnSetup,
            this.toolStripSeparator4,
            this.ToolBtnCan,
            this.ToolBtnNmt,
            this.ToolBtnEmcy,
            this.ToolBtnInfo});
            this.MainToolBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.MainToolBar.Location = new System.Drawing.Point(0, 0);
            this.MainToolBar.Name = "MainToolBar";
            this.MainToolBar.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MainToolBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MainToolBar.Size = new System.Drawing.Size(1023, 55);
            this.MainToolBar.Stretch = true;
            this.MainToolBar.TabIndex = 6;
            // 
            // ToolBtnConnect
            // 
            this.ToolBtnConnect.Image = global::CanMonitor.Properties.Resources.stock_disconnect_48;
            this.ToolBtnConnect.Name = "ToolBtnConnect";
            this.ToolBtnConnect.Size = new System.Drawing.Size(52, 52);
            this.ToolBtnConnect.ToolTipText = "Show Layout From XML";
            this.ToolBtnConnect.Click += new System.EventHandler(this.ToolBtnConnect_Click);
            // 
            // ToolBtnSetup
            // 
            this.ToolBtnSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolBtnSetup.Image = global::CanMonitor.Properties.Resources.setup;
            this.ToolBtnSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolBtnSetup.Name = "ToolBtnSetup";
            this.ToolBtnSetup.Size = new System.Drawing.Size(52, 52);
            this.ToolBtnSetup.Text = "toolStripButton1";
            this.ToolBtnSetup.Click += new System.EventHandler(this.ToolBtnSetup_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 55);
            // 
            // ToolBtnCan
            // 
            this.ToolBtnCan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolBtnCan.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenuCanClearAll,
            this.toolStripSeparator5,
            this.toolStripMenuItem1,
            this.ToolMenuCanHeartbeates,
            this.ToolMenuCanPdo,
            this.ToolMenuCanSdo,
            this.ToolMenuCanNmt,
            this.ToolMenuCanNmtec,
            this.ToolMenuCanEmcy,
            this.toolStripSeparator6,
            this.ToolMenuCanAutoscroll});
            this.ToolBtnCan.Image = global::CanMonitor.Properties.Resources.can_window;
            this.ToolBtnCan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolBtnCan.Name = "ToolBtnCan";
            this.ToolBtnCan.Size = new System.Drawing.Size(64, 52);
            this.ToolBtnCan.Text = "toolStripSplitButton1";
            this.ToolBtnCan.ButtonClick += new System.EventHandler(this.ToolBtnCan_ButtonClick);
            // 
            // ToolMenuCanClearAll
            // 
            this.ToolMenuCanClearAll.Name = "ToolMenuCanClearAll";
            this.ToolMenuCanClearAll.Size = new System.Drawing.Size(143, 22);
            this.ToolMenuCanClearAll.Text = "Clear All";
            this.ToolMenuCanClearAll.Click += new System.EventHandler(this.ToolMenuCanClearAll_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(140, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(143, 22);
            this.toolStripMenuItem1.Text = "Packet Filters";
            // 
            // ToolMenuCanHeartbeates
            // 
            this.ToolMenuCanHeartbeates.Checked = true;
            this.ToolMenuCanHeartbeates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolMenuCanHeartbeates.Name = "ToolMenuCanHeartbeates";
            this.ToolMenuCanHeartbeates.Size = new System.Drawing.Size(143, 22);
            this.ToolMenuCanHeartbeates.Text = "Heartbeates";
            this.ToolMenuCanHeartbeates.Click += new System.EventHandler(this.ToolMenuCanHeartbeates_Click);
            // 
            // ToolMenuCanPdo
            // 
            this.ToolMenuCanPdo.Checked = true;
            this.ToolMenuCanPdo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolMenuCanPdo.Name = "ToolMenuCanPdo";
            this.ToolMenuCanPdo.Size = new System.Drawing.Size(143, 22);
            this.ToolMenuCanPdo.Text = "PDO";
            this.ToolMenuCanPdo.Click += new System.EventHandler(this.ToolMenuCanPdo_Click);
            // 
            // ToolMenuCanSdo
            // 
            this.ToolMenuCanSdo.Checked = true;
            this.ToolMenuCanSdo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolMenuCanSdo.Name = "ToolMenuCanSdo";
            this.ToolMenuCanSdo.Size = new System.Drawing.Size(143, 22);
            this.ToolMenuCanSdo.Text = "SDO";
            this.ToolMenuCanSdo.Click += new System.EventHandler(this.ToolMenuCanSdo_Click);
            // 
            // ToolMenuCanNmt
            // 
            this.ToolMenuCanNmt.Checked = true;
            this.ToolMenuCanNmt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolMenuCanNmt.Name = "ToolMenuCanNmt";
            this.ToolMenuCanNmt.Size = new System.Drawing.Size(143, 22);
            this.ToolMenuCanNmt.Text = "NMT";
            this.ToolMenuCanNmt.Click += new System.EventHandler(this.ToolMenuCanNmt_Click);
            // 
            // ToolMenuCanNmtec
            // 
            this.ToolMenuCanNmtec.Checked = true;
            this.ToolMenuCanNmtec.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolMenuCanNmtec.Name = "ToolMenuCanNmtec";
            this.ToolMenuCanNmtec.Size = new System.Drawing.Size(143, 22);
            this.ToolMenuCanNmtec.Text = "NMTEC";
            this.ToolMenuCanNmtec.Click += new System.EventHandler(this.ToolMenuCanNmtec_Click);
            // 
            // ToolMenuCanEmcy
            // 
            this.ToolMenuCanEmcy.Checked = true;
            this.ToolMenuCanEmcy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolMenuCanEmcy.Name = "ToolMenuCanEmcy";
            this.ToolMenuCanEmcy.Size = new System.Drawing.Size(143, 22);
            this.ToolMenuCanEmcy.Text = "EMCY";
            this.ToolMenuCanEmcy.Click += new System.EventHandler(this.ToolMenuCanEmcy_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(140, 6);
            // 
            // ToolMenuCanAutoscroll
            // 
            this.ToolMenuCanAutoscroll.Checked = true;
            this.ToolMenuCanAutoscroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolMenuCanAutoscroll.Name = "ToolMenuCanAutoscroll";
            this.ToolMenuCanAutoscroll.Size = new System.Drawing.Size(143, 22);
            this.ToolMenuCanAutoscroll.Text = "Autoscroll";
            this.ToolMenuCanAutoscroll.Click += new System.EventHandler(this.ToolMenuCanAutoscroll_Click);
            // 
            // ToolBtnNmt
            // 
            this.ToolBtnNmt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolBtnNmt.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenuNmtClearAll,
            this.toolStripSeparator7,
            this.ToolMenuNmtAutoscroll});
            this.ToolBtnNmt.Image = global::CanMonitor.Properties.Resources.nmt_window;
            this.ToolBtnNmt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolBtnNmt.Name = "ToolBtnNmt";
            this.ToolBtnNmt.Size = new System.Drawing.Size(64, 52);
            this.ToolBtnNmt.Text = "toolStripSplitButton2";
            this.ToolBtnNmt.ButtonClick += new System.EventHandler(this.ToolBtnNmt_ButtonClick);
            // 
            // ToolMenuNmtClearAll
            // 
            this.ToolMenuNmtClearAll.Name = "ToolMenuNmtClearAll";
            this.ToolMenuNmtClearAll.Size = new System.Drawing.Size(128, 22);
            this.ToolMenuNmtClearAll.Text = "Clear All";
            this.ToolMenuNmtClearAll.Click += new System.EventHandler(this.ToolMenuNmtClearAll_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(125, 6);
            // 
            // ToolMenuNmtAutoscroll
            // 
            this.ToolMenuNmtAutoscroll.Checked = true;
            this.ToolMenuNmtAutoscroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolMenuNmtAutoscroll.Name = "ToolMenuNmtAutoscroll";
            this.ToolMenuNmtAutoscroll.Size = new System.Drawing.Size(128, 22);
            this.ToolMenuNmtAutoscroll.Text = "Autoscroll";
            this.ToolMenuNmtAutoscroll.Click += new System.EventHandler(this.ToolMenuNmtAutoscroll_Click);
            // 
            // ToolBtnEmcy
            // 
            this.ToolBtnEmcy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolBtnEmcy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenuEmcyClearAll,
            this.toolStripSeparator8,
            this.ToolMenuEmcyAutoscroll});
            this.ToolBtnEmcy.Image = global::CanMonitor.Properties.Resources.emcy_window;
            this.ToolBtnEmcy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolBtnEmcy.Name = "ToolBtnEmcy";
            this.ToolBtnEmcy.Size = new System.Drawing.Size(64, 52);
            this.ToolBtnEmcy.Text = "toolStripSplitButton3";
            this.ToolBtnEmcy.ButtonClick += new System.EventHandler(this.ToolBtnEmcy_ButtonClick);
            // 
            // ToolMenuEmcyClearAll
            // 
            this.ToolMenuEmcyClearAll.Name = "ToolMenuEmcyClearAll";
            this.ToolMenuEmcyClearAll.Size = new System.Drawing.Size(128, 22);
            this.ToolMenuEmcyClearAll.Text = "Clear All";
            this.ToolMenuEmcyClearAll.Click += new System.EventHandler(this.ToolMenuEmcyClearAll_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(125, 6);
            // 
            // ToolMenuEmcyAutoscroll
            // 
            this.ToolMenuEmcyAutoscroll.Checked = true;
            this.ToolMenuEmcyAutoscroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolMenuEmcyAutoscroll.Name = "ToolMenuEmcyAutoscroll";
            this.ToolMenuEmcyAutoscroll.Size = new System.Drawing.Size(128, 22);
            this.ToolMenuEmcyAutoscroll.Text = "Autoscroll";
            this.ToolMenuEmcyAutoscroll.Click += new System.EventHandler(this.ToolMenuEmcyAutoscroll_Click);
            // 
            // ToolBtnInfo
            // 
            this.ToolBtnInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolBtnInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenuInfoClearAll,
            this.toolStripSeparator9,
            this.ToolMenuInfoAutoscroll});
            this.ToolBtnInfo.Image = global::CanMonitor.Properties.Resources.info_window;
            this.ToolBtnInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolBtnInfo.Name = "ToolBtnInfo";
            this.ToolBtnInfo.Size = new System.Drawing.Size(64, 52);
            this.ToolBtnInfo.Text = "toolStripSplitButton4";
            this.ToolBtnInfo.ButtonClick += new System.EventHandler(this.ToolBtnInfo_ButtonClick);
            // 
            // ToolMenuInfoClearAll
            // 
            this.ToolMenuInfoClearAll.Name = "ToolMenuInfoClearAll";
            this.ToolMenuInfoClearAll.Size = new System.Drawing.Size(128, 22);
            this.ToolMenuInfoClearAll.Text = "Clear All";
            this.ToolMenuInfoClearAll.Click += new System.EventHandler(this.ToolMenuInfoClearAll_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(125, 6);
            // 
            // ToolMenuInfoAutoscroll
            // 
            this.ToolMenuInfoAutoscroll.Checked = true;
            this.ToolMenuInfoAutoscroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolMenuInfoAutoscroll.Name = "ToolMenuInfoAutoscroll";
            this.ToolMenuInfoAutoscroll.Size = new System.Drawing.Size(128, 22);
            this.ToolMenuInfoAutoscroll.Text = "Autoscroll";
            this.ToolMenuInfoAutoscroll.Click += new System.EventHandler(this.ToolMenuInfoAutoscroll_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.MainToolBar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dockPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1023, 607);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // MainDockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 653);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.StatusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainDockForm";
            this.Text = "CanOpen Monitor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.MainToolBar.ResumeLayout(false);
            this.MainToolBar.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ToolStrip MainToolBar;
        private System.Windows.Forms.ToolStripButton ToolBtnConnect;
        private System.Windows.Forms.StatusStrip StatusBar;                        
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem SaveDataMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadDataMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ViewMenu;
        private System.Windows.Forms.ToolStripMenuItem CanLogMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CanEmergencyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NmtMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InfoMenuItem;
        private System.Windows.Forms.ToolStripButton ToolBtnSetup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSplitButton ToolBtnCan;
        private System.Windows.Forms.ToolStripSplitButton ToolBtnNmt;
        private System.Windows.Forms.ToolStripSplitButton ToolBtnEmcy;
        private System.Windows.Forms.ToolStripSplitButton ToolBtnInfo;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuCanClearAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuCanHeartbeates;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuCanPdo;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuCanSdo;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuCanNmt;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuCanNmtec;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuCanEmcy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuCanAutoscroll;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuNmtClearAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuNmtAutoscroll;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuEmcyClearAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuEmcyAutoscroll;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuInfoClearAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuInfoAutoscroll;
        private System.Windows.Forms.ToolStripMenuItem CanMenu;
        private System.Windows.Forms.ToolStripMenuItem ConnectionControlMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem ToolBarMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StatusBarMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel StatusBarStatusLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}