using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDOInterface;
using libCanopenSimple;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Controls;

namespace SDOEditorPlugin
{
    public class SDOEditorPlugin : InterfaceService, IPDOParser
    {
        private SDOEditor sdoeditor = null;

        public SDOEditorPlugin()
        {   
            addverb("Tools", null, null, "_root_", null);
            addverb("Device SOD Editor", null, null, "Tools", showdlg);
            
            addverb("SDO", null, null, "_root_", null);
            addverb("Write DCF to device", null, null, "SDO", write_dcf_cb);
            addverb("Send save req via SDO 0x1010", null, null, "SDO", send_save_req_cb);            
                    
            addverb("Load Datasheet/Device file XDD/EDS/DCF", "", null, "File", load_cb);
            addverb("Save difference", "", null, "File", save_diff_cb);
            addverb("Recent", "SDOEdFileRecent", null, "File", null);
            
            addverb(null, null, "sdo_window.png", "_button_", showdlg);
        }

        public void registerPDOS()
        {
            sdoeditor = new SDOEditor(_lco, MainDockPanel);
            
        }


        public string decodesdo(int index, int sub, canpacket payload)
        {
            return "";
        }


        private ToolStripMenuItem GetSDOEdFileRecent()
        { 
            foreach (ToolStripItem root_item in MainMenuStrip.Items)
            {
                if (root_item.Name == "FileMenu")
                {
                    ToolStripMenuItem menu_item = (ToolStripMenuItem)root_item;
                    foreach (ToolStripItem sub_item in menu_item.DropDownItems)
                    {
                        if (sub_item.Name == "SDOEdFileRecent")
                            return ((ToolStripMenuItem)sub_item);
                    }
                }
            }    
        return(null);    
        }
        
        
        public override void AppEvent(PL_APP_EVENT e)
        {
            if ((sdoeditor != null) && (!sdoeditor.IsDisposed))
            {
                if (e == PL_APP_EVENT.INIT)
                {
                    sdoeditor.SDOEdFileRecent = GetSDOEdFileRecent();
                    sdoeditor.LoadMruList();
                }
                else if (e == PL_APP_EVENT.DOWN)
                {
                    sdoeditor.SaveMruList();
                }
            }    
        }


        void load_cb(object sender, System.EventArgs e)
        {
        if ((sdoeditor != null) && (!sdoeditor.IsDisposed))
            sdoeditor.SdoEditorLoadFile();
        }
        
        
        void save_diff_cb(object sender, System.EventArgs e)
        {
        if ((sdoeditor != null) && (!sdoeditor.IsDisposed))
            sdoeditor.SdoEditorSaveDifference();        
        }
        
        
        void write_dcf_cb(object sender, System.EventArgs e)
        {
        if ((sdoeditor != null) && (!sdoeditor.IsDisposed))
            sdoeditor.SdoEditorWriteDcf();
        }        
        
        
        void send_save_req_cb(object sender, System.EventArgs e)
        {
        if ((sdoeditor != null) && (!sdoeditor.IsDisposed))
            sdoeditor.SdoEditorSendSaveReq();
        }    
        
             
        void showdlg(object sender, System.EventArgs e)
        {
            if ((sdoeditor != null) && (!sdoeditor.IsDisposed))
                sdoeditor.Show(MainDockPanel, DockState.Document);
        }
    }
}
