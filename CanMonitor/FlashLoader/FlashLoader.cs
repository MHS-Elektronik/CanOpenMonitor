using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDOInterface;
using libCanopenSimple;
using WeifenLuo.WinFormsUI.Docking;

namespace FlashLoader
{
    public class FlashLoader : InterfaceService, IPDOParser
    {
        public FlashLoader()
        {
            addverb("---", null, null, "File", null);
            addverb("Flash node", null, null, "File", showdlg);
        }

        public void registerPDOS()
        {

        }

        public string decodesdo(int index, int sub, canpacket payload)
        {
            return "";
        }


        public void showdlg(object sender, System.EventArgs e)
        {

            if (_lco == null)
                return;
            if (!_lco.isopen())
            {
                MessageBox.Show("CAN not open");
                return;
            }                 

            Flasher f = new Flasher(_lco);

            f.Show(MainDockPanel, DockState.DockRight);
        }
    }

}

