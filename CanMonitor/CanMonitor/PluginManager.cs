using libCanopenSimple;
using Microsoft.CSharp;
using PDOInterface;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CanMonitor
{
    public class PluginManager
    {

        public Dictionary<string, object> plugins = new Dictionary<string, object>();
        public IPDOParser ipdo;
        public Dictionary<UInt16, Func<canpacket, string>> pdoprocessors = new Dictionary<ushort, Func<canpacket, string>>();        


        public PluginManager()
        {
            if (Program.lco == null)
                return;
            Program.lco.connectionevent += Lco_connectionevent; 
        }
        

        public void autoloadplugins()
        {

            var autoloadPath = Path.Combine(Program.assemblyfolder, "autoload.txt");
            if (File.Exists(autoloadPath))
            {
                string[] autoload = System.IO.File.ReadAllLines(autoloadPath);

                foreach (string plugin in autoload)
                {
                    loadplugin(plugin);
                }
            }

            if (Program.appdatafolder != Program.assemblyfolder)
            {
                autoloadPath = Path.Combine(Program.appdatafolder, "autoload.txt");
                if (File.Exists(autoloadPath))
                {
                    string[] autoload = System.IO.File.ReadAllLines(autoloadPath);

                    foreach (string plugin in autoload)
                    {
                        loadplugin(plugin);
                    }
                }
            }
            DriverStateChange(PL_APP_EVENT.INIT);
        }


        private void Lco_connectionevent(object sender, EventArgs e)
        {
            //invoked when the underlying libcanopensimple opens or closes a driver conenction
            //send this message to all plugins that care

            foreach (KeyValuePair<string, object> kvp in plugins)
            {
                IInterfaceService iis = (IInterfaceService)kvp.Value;
                iis.DriverStateChange((ConnectionChangedEventArgs)e);
            }

        }
        
        
        public void PluginsDown()
        {
            DriverStateChange(PL_APP_EVENT.DOWN);
        }
        
        
        private void DriverStateChange(PL_APP_EVENT e)
        {
            foreach (KeyValuePair<string, object> kvp in plugins)
            {
                IInterfaceService iis = (IInterfaceService)kvp.Value;
                iis.AppEvent(e);
            }    
        }     
        

        private void loadplugin(String pfilename)
        {                
            foreach (KeyValuePair<string, object> kvp in plugins)
            {               
                string s = Path.GetFileName(kvp.Key);
                if (s == null)
                    continue;                            
                if (s.ToLower() == pfilename.ToLower())
                    return;                
            }

            try
            {
                string filename = pfilename;

                if (!File.Exists(filename))
                {

                    filename = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + pfilename;
                    if (!File.Exists(filename))
                        filename = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "privateplugins" + Path.DirectorySeparatorChar + pfilename;
                    if (!File.Exists(filename))
                        filename = Program.appdatafolder + Path.DirectorySeparatorChar + "plugins" + pfilename;
                    if (!File.Exists(filename))
                        filename = Program.appdatafolder + Path.DirectorySeparatorChar + "privateplugins" + pfilename;

                    if (!File.Exists(filename))
                    {
                        MessageBox.Show(string.Format("Could not find plugin {0}", pfilename));
                        return;
                    }
                }


                string ext = Path.GetExtension(filename);
                string plugin_path = Path.GetDirectoryName(filename);

                Assembly assembly;

                if (ext == ".cs")
                {
                    CSharpCodeProvider provider = new CSharpCodeProvider();
                    CompilerParameters parameters = new CompilerParameters();

                    // Reference to System.Drawing library
                    parameters.ReferencedAssemblies.Add("System.dll");
                    parameters.ReferencedAssemblies.Add("System.Core.dll");
                    parameters.ReferencedAssemblies.Add("System.Data.dll");
                    parameters.ReferencedAssemblies.Add("PDOInterface.dll");
                    parameters.ReferencedAssemblies.Add("libCanopenSimple.dll");
                    parameters.ReferencedAssemblies.Add("System.Windows.Forms");
                    parameters.ReferencedAssemblies.Add("System.Drawing");

                    // True - memory generation, false - external file generation
                    parameters.GenerateInMemory = true;
                    // True - exe file generation, false - dll file generation
                    parameters.GenerateExecutable = false;

                    string code = File.ReadAllText(filename);

                    CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

                    if (results.Errors.HasErrors)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (CompilerError error in results.Errors)
                        {
                            sb.AppendLine(String.Format("{0}: Error ({1}): {2}", error.Line, error.ErrorNumber, error.ErrorText));
                        }

                        MessageBox.Show(sb.ToString());
                        return;

                    }

                    assembly = results.CompiledAssembly;

                }
                else
                {
                    assembly = Assembly.LoadFrom(filename);
                }

                Type[] types = assembly.GetExportedTypes();

                for (int i = 0; i < types.Length; i++)
                {
                    object obj = null;

                    Type type = assembly.GetType(types[i].FullName);
                    if (type.GetInterface("PDOInterface.IInterfaceService") != null)
                    {
                        obj = Activator.CreateInstance(type);
                        if (obj != null)
                        {
                            plugins.Add(filename, obj);
                            IInterfaceService iis = (IInterfaceService)obj;
                            ipdo = (IPDOParser)obj;

                            Dictionary<UInt16, Func<byte[], string>> dictemp = new Dictionary<ushort, Func<byte[], string>>();

                            iis.setlco(Program.lco);

                            iis.preregisterPDOS(pdoprocessors);
                            iis.SetMainWidgets(Program.MainMenuStrip, Program.MainToolBar, Program.MainStatusBar, Program.MainDockPanel);
                            ipdo.registerPDOS();
                            Program.InfoWin.AddLine(string.Format("SUCCESS loading plugin {0}", filename));
                        }

                    }
            
                    if (type.GetInterface("PDOInterface.IInterfaceService") != null)
                    {
                        if (obj != null && obj is PDOInterface.IInterfaceService)
                        {
                            // do nothing use the object to save recreate
                        }
                        else
                        {
                            obj = Activator.CreateInstance(type);
                        }

                        if (obj != null)
                        {
                            IInterfaceService iss = (IInterfaceService)obj;
                            IVerb[] verbsroot;

                            verbsroot = iss.GetVerbs("_button_");

                            if (verbsroot != null)
                            {
                                foreach (IVerb v in verbsroot)
                                {
                                    if (v.Text == "---")
                                    {
                                        ToolStripSeparator item = new ToolStripSeparator();
                                        Program.MainToolBar.Items.Add(item);
                                    }
                                    else
                                    {
                                        if (v.Pic != null)
                                        {
                                            Image img = Image.FromFile(plugin_path + Path.DirectorySeparatorChar + v.Pic);
                                            ToolStripButton item = new ToolStripButton(v.Text, img, v.Action, v.Name);
                                            Program.MainToolBar.Items.Add(item);
                                        }
                                    }                            
                                }
                            }

                            verbsroot = iss.GetVerbs("_root_");

                            if (verbsroot != null)
                            {
                                foreach (IVerb v in verbsroot)
                                {
                                    bool found = false;
                                    foreach (ToolStripItem ii in Program.MainMenuStrip.Items)
                                    {
                                        if (ii.Text == v.Name)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }

                                    if (found == false)
                                    {
                                        Program.MainMenuStrip.Items.Add(v.Name);
                                    }

                                }
                            }

                            foreach (ToolStripMenuItem ii in Program.MainMenuStrip.Items)
                            {
                                IVerb[] verbs = iss.GetVerbs(ii.Text);

                                if (verbs != null)
                                {
                                    foreach (IVerb v in verbs)
                                    {
                                        if (v.Text == "---")
                                        {
                                            ToolStripSeparator item = new ToolStripSeparator();
                                            ii.DropDownItems.Add(item);
                                        }
                                        else
                                        {
                                            ToolStripMenuItem item;
                                            if (v.Pic != null)
                                            {
                                                Image img = Image.FromFile(plugin_path + v.Pic);
                                                item = new ToolStripMenuItem(v.Text, img, v.Action, v.Name);
                                            }
                                            else
                                            {
                                                item = new ToolStripMenuItem(v.Text, null, v.Action, v.Name);
                                            }
                                            ii.DropDownItems.Add(item);
                                        }
                                    }
                                }
                            }                                                      
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.InfoWin.AddLine("Failed loading plugin \r\n" + ex.ToString() + "\r\n");
            }


        }


    }
}
