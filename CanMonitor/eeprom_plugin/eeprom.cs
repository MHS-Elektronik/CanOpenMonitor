using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libCanopenSimple;
using PDOInterface;
using WeifenLuo.WinFormsUI.Docking;

namespace eeprom_plugin
{
    public class eeprom_plugin : InterfaceService, IPDOParser
    {
        public eeprom_plugin()
        {
            addverb("Tools", null, null, "_root_", null);
            addverb("Reset EEprom", null, null, "Tools", showdlg);
        }

        public void registerPDOS()
        {

        }


        public string decodesdo(int index, int sub, canpacket payload)
        {
            if(index==0x1010)
            {
                return "STORE_PARAM_FUNC";
            }

            if(index==0x1011)
            {      
                return "RESET_PARAM_FUNC";
            }

            return "";
        }

        public void showdlg(object sender, System.EventArgs e)
        {

            if (_lco == null || !_lco.isopen())
                return;

            ResetEEPROM re = new ResetEEPROM(_lco);

            re.Show(MainDockPanel, DockState.DockLeft);

        }
    }
}
