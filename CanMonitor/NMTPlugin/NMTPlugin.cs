using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDOInterface;
using libCanopenSimple;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace NMTPlugin
{
    public class NMTPlugin : InterfaceService, IPDOParser
    {
        public NMTPlugin()
        {
            addverb("NMT", null, null, "_root_", null);
            addverb("Start Bus", null, null, "NMT", startbus);
            addverb("Pre-op Bus", null, null, "NMT", preopbus);
            addverb("Stop Bus", null, null, "NMT", stopbus);
            addverb("Reset Bus", null, null, "NMT", resetbus);
            addverb("Reset Communication", null, null, "NMT", resetcomms);
            addverb("---", null, null, "NMT", null);
            addverb("Advanced", null, null, "NMT", showdlg);

        }
        
        
        public void registerPDOS()
        {

        }

        public string decodesdo(int index, int sub, canpacket payload)
        {
            return "";
        }

    
        void startbus(object sender, System.EventArgs e)
        {
            if (_lco == null)            
                return;
            if (!_lco.isopen())
            {
                MessageBox.Show("CAN not open");
                return;
            }    

            _lco.NMT_start();
        }

        void preopbus(object sender, System.EventArgs e)
        {
            if (_lco == null)
                return;
            if (!_lco.isopen())
            {
                MessageBox.Show("CAN not open");
                return;
            } 

            _lco.NMT_preop();
        }

        void stopbus(object sender, System.EventArgs e)
        {
            if (_lco == null)
                return;
            if (!_lco.isopen())
            {
                MessageBox.Show("CAN not open");
                return;
            } 

            _lco.NMT_stop();
        }

        void resetbus(object sender, System.EventArgs e)
        {
            if (_lco == null)
                return;
            if (!_lco.isopen())
            {
                MessageBox.Show("CAN not open");
                return;
            }                 

            _lco.NMT_ResetNode();
        }

        void resetcomms(object sender, System.EventArgs e)
        {
            if (_lco == null)
                return;
            if (!_lco.isopen())
            {
                MessageBox.Show("CAN not open");
                return;
            }                 

            _lco.NMT_ResetComms();
        }

        void showdlg(object sender, System.EventArgs e)
        {
            if (_lco == null)
                return;
            if (!_lco.isopen())
            {
                MessageBox.Show("CAN not open");
                return;
            }                 

            NMTFrm FRM = new NMTFrm(_lco);
            FRM.Show(MainDockPanel, DockState.DockLeft);
        }
    }


}
