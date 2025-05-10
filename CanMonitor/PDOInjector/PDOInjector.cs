using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDOInterface;
using libCanopenSimple;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace PDOInjector
{
    public class PDOInjector : InterfaceService, IPDOParser
    {
        PDOForm frm;

        public PDOInjector()
        {
            addverb("PDO Injector", null, null, "Tools", openform);
        }

        public string decodesdo(int index, int sub, canpacket payload)
        {
            return "";
        }


        public void registerPDOS()
        {

        }

        public void openform(object sender, System.EventArgs e)
        {
            if(frm==null || frm.IsDisposed)
            {
                frm = new PDOForm(_lco); 
            }
            frm.Show(MainDockPanel, DockState.DockRight);
        }
        
    }
}
