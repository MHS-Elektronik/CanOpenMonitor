using N_SettingsMgr;
using libCanopenSimple;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CanMonitor
{
    [Flags]
    public enum TLogClearFlags : uint
    {
        LOG_CLEAR_CAN   = 0x00000001,
        LOG_CLEAR_NMT   = 0x00000002,
        LOG_CLEAR_EMCY  = 0x00000004,
        LOG_CLEAR_INFO  = 0x00000008,
        LOG_CLEAR_ALL   = 0xFFFFFFFF
        
    }
                        
    public partial class MainDockForm : Form
    {
        string gitVersion;

        public MainDockForm()
        {
            InitializeComponent();

            this.FormClosing += MainDockForm_FormClosing;
            this.Load += MainDockForm_Load;

            ToolMenuCanSdo.Checked = Properties.Settings.Default.showsdo;
            ToolMenuCanPdo.Checked = Properties.Settings.Default.showpdo;
            ToolMenuCanHeartbeates.Checked = Properties.Settings.Default.showHB;
            ToolMenuCanNmtec.Checked = Properties.Settings.Default.showNMTEC;
            ToolMenuCanNmt.Checked = Properties.Settings.Default.showNMT;
            ToolMenuCanEmcy.Checked = Properties.Settings.Default.showEMCY;
            
            ToolMenuCanAutoscroll.Checked = Properties.Settings.Default.CanAutoscroll;
            ToolMenuNmtAutoscroll.Checked = Properties.Settings.Default.NmtAutoscroll;
            ToolMenuEmcyAutoscroll.Checked = Properties.Settings.Default.EmcyAutoscroll;
            ToolMenuInfoAutoscroll.Checked = Properties.Settings.Default.InfoAutoscroll;
                        
            var theme = new VS2015DarkTheme();
            dockPanel1.Theme = theme;
            dockPanel1.DocumentTabStripLocation = DocumentTabStripLocation.Top;
            
            Program.MainMenuStrip = menuStrip1;
            Program.MainToolBar = MainToolBar;
            Program.MainStatusBar = StatusBar;           
            Program.MainDockPanel = dockPanel1;

            Program.InfoWin = new InfoLogDocument();                     
            
            Program.CanWin = new CanLogForm();            
            Program.CanWin.dockpanel = dockPanel1;
            Program.CanWin.Show(dockPanel1, DockState.Document);

            Program.NmtWin = new NMTDocument();
            Program.EmcyWin = new EmcyDocument();

            Program.NmtWin.Show(dockPanel1, DockState.Document);
            Program.EmcyWin.Show(dockPanel1, DockState.Document);
            Program.InfoWin.Show(dockPanel1, DockState.Document);
                        
            Program.driverloader.finddrivers();
            Program.driverloader.enumerateports();
        
            //Properties.Settings.Default.Reload();  // <*> ?
            if (Properties.Settings.Default.autoconnect == true)
              SetConnected(true);
            
            Program.CanWin.Activate();
            
            Program.pluginManager = new PluginManager();
            Program.pluginManager.autoloadplugins();

            ToolStripSeparator separator_item;
            ToolStripMenuItem item;
            // Add "Quit" to File menu
            separator_item = new ToolStripSeparator();
            FileMenu.DropDownItems.Add(separator_item);
            item = new ToolStripMenuItem("Preferences", null, preferencesToolStripMenuItem_Click, "preferencesToolStripMenuItem");
            FileMenu.DropDownItems.Add(item);

            // Add "Quit" to File menu
            separator_item = new ToolStripSeparator();
            FileMenu.DropDownItems.Add(separator_item);
            item = new ToolStripMenuItem("Quit", null, quitToolStripMenuItem_Click, "quitToolStripMenuItem");
            FileMenu.DropDownItems.Add(item);            
        }


        private void MainDockForm_Load(object sender, EventArgs e)
        {
            //read git version string, show in title bar 
            //(https://stackoverflow.com/a/15145121)
            string gitVersion = String.Empty;
            using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("CanMonitor." + "version.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                gitVersion = reader.ReadToEnd();
            }
            if (gitVersion == "")
            {
                gitVersion = "Unknown";
            }
            this.Text += " -- " + gitVersion;
            this.gitVersion = gitVersion;
        }


        public void LogsClear(TLogClearFlags flags)
        {
            if ((flags & TLogClearFlags.LOG_CLEAR_CAN) == TLogClearFlags.LOG_CLEAR_CAN)             
            {
                if (Program.CanWin != null && !Program.CanWin.IsDisposed)
                    Program.CanWin.clearlist();
            }
            if ((flags & TLogClearFlags.LOG_CLEAR_NMT) == TLogClearFlags.LOG_CLEAR_NMT)
            {
                if (Program.NmtWin != null && !Program.NmtWin.IsDisposed)
                    Program.NmtWin.clearlist();            
            }
            if ((flags & TLogClearFlags.LOG_CLEAR_EMCY) == TLogClearFlags.LOG_CLEAR_EMCY)
            {
                if (Program.EmcyWin != null && !Program.NmtWin.IsDisposed)
                    Program.EmcyWin.clearlist();            
            }
            if ((flags & TLogClearFlags.LOG_CLEAR_INFO) == TLogClearFlags.LOG_CLEAR_INFO)
            {
                if (Program.InfoWin != null && !Program.InfoWin.IsDisposed)
                    Program.InfoWin.ClearLog();           
            }
        }


        private void MainDockForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingsMgr.writeXML(Path.Combine(Program.appdatafolder, "settings.xml"));

            Program.driverloader.Close();
            Program.pluginManager.PluginsDown();
        }


        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        

        private void saveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "(*.xml)|*.xml";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Program.CanWin.dosave(sfd.FileName);
            }
        }


        private void ExectueConnectionControl() 
        {
            ConnectionControl connectioncontrol = new ConnectionControl(this);
            connectioncontrol.ShowDialog();
        }


        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Prefs p = new Prefs(Program.appdatafolder, Program.assemblyfolder);
            p.ShowDialog();
        }


        private void canLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((Program.CanWin == null) || (Program.CanWin.IsDisposed))
                return;
            Program.CanWin.Show(dockPanel1,DockState.Document);
        }

                                                     
        private void canEmergencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((Program.EmcyWin == null) || (Program.EmcyWin.IsDisposed))
                return;
            Program.EmcyWin.Show(dockPanel1, DockState.Document);
        }

        
        private void nMTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((Program.NmtWin == null) || (Program.NmtWin.IsDisposed))
                return;
            Program.NmtWin.Show(dockPanel1, DockState.Document);
        }

        
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((Program.InfoWin == null) || (Program.InfoWin.IsDisposed))
                return;
            Program.InfoWin.Show(dockPanel1, DockState.Document);
        }


        private void loadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.CanWin?.doload();
        }


        private void toolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainToolBar.Visible = ToolBarMenuItem.Checked = !ToolBarMenuItem.Checked;
        }


        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatusBar.Visible = StatusBarMenuItem.Checked = !StatusBarMenuItem.Checked;
        }


        private void connectionControlToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExectueConnectionControl();
        }
          
          
        public bool SetConnected(bool connected)
        {
            bool open, close;
            string status_str;
            BUSSPEED rate;

            status_str = "";
            open = false;
            close = !connected;
            rate = Program.driverloader.StrToBusspeed(Properties.Settings.Default.lastrate);
            if (connected)
            {    
                driverport dp = new driverport();
                dp.port = Properties.Settings.Default.lastport;
                dp.driver = Properties.Settings.Default.lastdriver;
                            
                if (Program.lco.isopen())                
                {
                    if ((!dp.issamedriver(Program.opendp)) || (rate != Program.driverloader.rate))
                        {            
                        open = true;
                        close = true;
                        }                                                                    
                }        
                else
                    open = true;
                Program.opendp = dp;
            }
            
            if (close)
                {
                Program.driverloader.Close();
                if (open == false)
                    status_str = "CAN Interface closed"; 
                } 
            if (open)
            {
                Program.InfoWin.AddLine(String.Format("Trying to open port {0} using driver {1}", Program.opendp.port, Program.opendp.driver));
                if (Program.driverloader.Open(Program.opendp, rate) == false)
                {
                    status_str = String.Format("Failed to open CAN Interface {0} using driver {1}", Program.opendp.port, Program.opendp.driver);
                    MessageBox.Show(status_str);
                    connected = false;                
                }
                else
                    status_str = String.Format("CAN Interface {0} open, Baudrate: {1}", Program.opendp.port, Program.driverloader.BusspeedToStr(rate));
            }
                    
            if (status_str != "")
                StatusBarStatusLabel.Text = status_str;
            if (connected)
            {
                ToolBtnConnect.Checked = true;
                ToolBtnConnect.Image = global::CanMonitor.Properties.Resources.stock_connect_48;
            }
            else
            {
                ToolBtnConnect.Checked = false;
                ToolBtnConnect.Image = global::CanMonitor.Properties.Resources.stock_disconnect_48;
            }
            return(connected);
        }


        public void ExecuteToogleConnection()
        {
            if (Program.lco.isopen())
                SetConnected(false);
            else
                SetConnected(true);
        }
            

        private void ToolBtnConnect_Click(object sender, EventArgs e)
        {
            ExecuteToogleConnection();
        }


        private void ToolBtnSetup_Click(object sender, EventArgs e)
        {
            ExectueConnectionControl();
        }


        private void ToolBtnCan_ButtonClick(object sender, EventArgs e)
        {
            canLogToolStripMenuItem_Click(null, null);
        }


        private void ToolBtnNmt_ButtonClick(object sender, EventArgs e)
        {
            nMTToolStripMenuItem_Click(null, null);
        }


        private void ToolBtnEmcy_ButtonClick(object sender, EventArgs e)
        {
            canEmergencyToolStripMenuItem_Click(null, null);
        }


        private void ToolBtnInfo_ButtonClick(object sender, EventArgs e)
        {
            infoToolStripMenuItem_Click(null, null);
        }


        private void ToolMenuCanClearAll_Click(object sender, EventArgs e)
        {
            LogsClear(TLogClearFlags.LOG_CLEAR_CAN);
        }
                   

        private void SettingsSet()  
        { 
            Properties.Settings.Default.showsdo = ToolMenuCanSdo.Checked;
            Properties.Settings.Default.showpdo = ToolMenuCanPdo.Checked ;
            Properties.Settings.Default.showHB =  ToolMenuCanHeartbeates.Checked;
            Properties.Settings.Default.showNMTEC = ToolMenuCanNmtec.Checked;
            Properties.Settings.Default.showNMT = ToolMenuCanNmt.Checked;
            Properties.Settings.Default.showEMCY = ToolMenuCanEmcy.Checked;
        
            Properties.Settings.Default.CanAutoscroll = ToolMenuCanAutoscroll.Checked;
            Properties.Settings.Default.NmtAutoscroll = ToolMenuNmtAutoscroll.Checked;
            Properties.Settings.Default.EmcyAutoscroll = ToolMenuEmcyAutoscroll.Checked;
            Properties.Settings.Default.InfoAutoscroll = ToolMenuInfoAutoscroll.Checked;
            
            Properties.Settings.Default.Save();
        }    


        private void ToolMenuCanPdo_Click(object sender, EventArgs e)
        {
            ToolMenuCanPdo.Checked = !ToolMenuCanPdo.Checked;
            SettingsSet();
        }
        

        private void ToolMenuCanSdo_Click(object sender, EventArgs e)
        {
            ToolMenuCanSdo.Checked = !ToolMenuCanSdo.Checked;
            SettingsSet();
        }
                

        private void ToolMenuCanHeartbeates_Click(object sender, EventArgs e)
        {
            ToolMenuCanHeartbeates.Checked = !ToolMenuCanHeartbeates.Checked;
            SettingsSet();
        }


        private void ToolMenuCanNmt_Click(object sender, EventArgs e)
        {
            ToolMenuCanNmt.Checked = !ToolMenuCanNmt.Checked;
            SettingsSet();
        }


        private void ToolMenuCanNmtec_Click(object sender, EventArgs e)
        {
            ToolMenuCanNmtec.Checked = !ToolMenuCanNmtec.Checked;
            SettingsSet();
        }


        private void ToolMenuCanEmcy_Click(object sender, EventArgs e)
        {
            ToolMenuCanEmcy.Checked = !ToolMenuCanEmcy.Checked;
            SettingsSet();
        }


        private void ToolMenuCanAutoscroll_Click(object sender, EventArgs e)
        {
            ToolMenuCanAutoscroll.Checked = ! ToolMenuCanAutoscroll.Checked;
            SettingsSet();
        }


        private void ToolMenuNmtClearAll_Click(object sender, EventArgs e)
        {
            LogsClear(TLogClearFlags.LOG_CLEAR_NMT);
        }


        private void ToolMenuNmtAutoscroll_Click(object sender, EventArgs e)
        {
            ToolMenuNmtAutoscroll.Checked = !ToolMenuNmtAutoscroll.Checked;
            SettingsSet();
        }


        private void ToolMenuEmcyClearAll_Click(object sender, EventArgs e)
        {
            LogsClear(TLogClearFlags.LOG_CLEAR_EMCY);
        }


        private void ToolMenuEmcyAutoscroll_Click(object sender, EventArgs e)
        {
            ToolMenuEmcyAutoscroll.Checked = !ToolMenuEmcyAutoscroll.Checked;
            SettingsSet();
        }


        private void ToolMenuInfoClearAll_Click(object sender, EventArgs e)
        {
            LogsClear(TLogClearFlags.LOG_CLEAR_INFO);
        }


        private void ToolMenuInfoAutoscroll_Click(object sender, EventArgs e)
        {
            ToolMenuInfoAutoscroll.Checked = !ToolMenuInfoAutoscroll.Checked;
            SettingsSet();
        }
    }
}
