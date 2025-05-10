using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libCanopenSimple;
using PDOInterface;

namespace EmergencySimulator
{
    public class Emergency : InterfaceService, IPDOParser
    {

        EmergencyFrm frm;

        public Emergency()
        {
            addverb("EMCY Injector", null, null, "Tools", openform);
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
            if (frm == null || frm.IsDisposed)
            {
                frm = new EmergencyFrm(_lco);
            }
            frm.Show();
        }

    }
}
