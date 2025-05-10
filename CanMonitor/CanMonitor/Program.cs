using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CanMonitor
{
    static class Program
    {
        static private string _appdatafolder;
        static public string appdatafolder { get { return _appdatafolder; } set { } }
        static private string _assemblyfolder;
        static public string assemblyfolder { get { return _assemblyfolder; } set { } }

        static public PluginManager pluginManager;

        static public driverport opendp = null;
        static public libCanopenSimple.libCanopenSimple lco = new libCanopenSimple.libCanopenSimple();

        static public DriverLoader driverloader = new DriverLoader();
        
        
        static public InfoLogDocument InfoWin = null;
        static public NMTDocument NmtWin = null;
        static public EmcyDocument EmcyWin = null;
        static public CanLogForm CanWin = null;

        static public MenuStrip MainMenuStrip = null;
        static public ToolStrip MainToolBar = null;
        static public StatusStrip MainStatusBar = null;
        static public DockPanel MainDockPanel = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.ThreadException += Application_ThreadException;

            ErrorCodes.interror();


            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _appdatafolder = Path.Combine(folder, "CanMonitor");
            _assemblyfolder = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(appdatafolder))
            {
                Directory.CreateDirectory(appdatafolder);
            }
            
            try 
            { 
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                Application.Run(new MainDockForm());

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
        }
    }
}
