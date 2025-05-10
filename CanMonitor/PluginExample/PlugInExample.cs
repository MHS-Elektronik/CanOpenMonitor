using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDOInterface;
using System.Windows.Forms;
using libCanopenSimple;


namespace PluginExample
{
    public class PluginExample : InterfaceService, IPDOParser
    {

     
        public PluginExample()
        {
            addverb("hello", null, null, "Tools", sayhello);
            addverb("New menu", null, null, "_root_", null);
            addverb("HELLO 2", null, null,  "New menu", sayhello2);
            addverb("---", null, null, "New menu", null);
            addverb("More", null, null, "New menu", sayhello2);
            addverb("More", null, null, "New menu", sayhello3);
            addverb("More", null, null, "File", sayhello3);

        }

        /*public void endsdo(int node, int index, int sub, byte[] payload)<*>
        {

        }*/

        void sayhello(object sender, System.EventArgs e)
        {
            if (_lco == null || !_lco.isopen())
                return;

            MessageBox.Show("HELLO WORLD");
            
            _lco.NMT_start();

        }


        void sayhello2(object sender, System.EventArgs e)
        {

            MessageBox.Show("HELLO WORLD 2");

        }

        void sayhello3(object sender, System.EventArgs e)
        {

            MessageBox.Show("HELLO WORLD 3");

        }

        public void registerPDOS()
        {

        }

        public string decodesdo(int index, int sub, canpacket payload)
        {
            return "";
        }

    }
}
