using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libCanopenSimple;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


public enum PL_APP_EVENT
{
    INIT = 0,
    DOWN
}

namespace PDOInterface
{
/*
    public enum APP_EVENT
    {
    INIT = 0,
    DOWN
    }*/

    public interface IPDOParser
    {
        void deregisterplugin();
        void registerPDOS();
        string decodesdo(int index, int sub, canpacket payload);

        void setlco(libCanopenSimple.libCanopenSimple lco);
    }


    public interface IInterfaceService
    {
        IVerb[] GetVerbs(string category);
        void setlco(libCanopenSimple.libCanopenSimple lco);
        void preregisterPDOS(Dictionary<UInt16, Func<canpacket, string>> dic);
        void deregisterplugin();
        void DriverStateChange(libCanopenSimple.ConnectionChangedEventArgs e);
        void SetMainWidgets(MenuStrip ms, ToolStrip ts, StatusStrip ss, DockPanel dp);
        void AppEvent(PL_APP_EVENT e);
    }


    public interface IVerb
    {
        string Category { get; }        
        string Name { get; }    
        string Text { get; }    
        string Pic { get; }
        void Action(object sender, System.EventArgs e);
    }

    public class InterfaceService: IInterfaceService
    {
        public MenuStrip MainMenuStrip = null;
        public ToolStrip MainToolBar = null;
        public StatusStrip MainStatusBar = null;
        public DockPanel MainDockPanel = null;
        public libCanopenSimple.libCanopenSimple _lco;
        Dictionary<UInt16, Func<canpacket, string>> _dic;
        Dictionary<UInt16, Func<canpacket, string>> _dic2;

        public void setlco(libCanopenSimple.libCanopenSimple lco)
        {
            this._lco = lco;
        }

        public void deregisterplugin()
        {
            foreach(KeyValuePair<UInt16, Func<canpacket, string>> kvp in _dic2)
            {
                _dic.Remove(kvp.Key);
            }
        }

        public void preregisterPDOS(Dictionary<UInt16, Func<canpacket, string>> dic)
        {
            _dic = dic;
            _dic2 = new Dictionary<ushort, Func<canpacket, string>>();
            
        }

        public void addpdohook(UInt16 cob, Func<canpacket, string> functor)
        {
            _dic.Add(cob, functor);
            _dic2.Add(cob, functor);
        }

        Dictionary<string, List<IVerb>> verbs = new Dictionary<string, List<IVerb>>();

        public IVerb[] GetVerbs(string category)
        {
            if (category == null)
                return null;
            if (verbs.ContainsKey(category))
                return verbs[category].ToArray();
            else
                return null;
        }

        
        protected void addverb(string text, string name, string pic, string category, Action<object, System.EventArgs> action)
        {
            if (name == null)
                name = text;
            verb v = new verb(text, name, pic, category, action);

            if (!verbs.ContainsKey(category))
                verbs.Add(category, new List<IVerb>());

            verbs[category].Add(v);

        }


        public virtual void DriverStateChange(ConnectionChangedEventArgs e)
        {
        }
        
        
        public virtual void AppEvent(PL_APP_EVENT e)
        {
        }


        public void SetMainWidgets(MenuStrip ms, ToolStrip ts, StatusStrip ss, DockPanel dp)
        {
        this.MainMenuStrip = ms;
        this.MainToolBar = ts;
        this.MainStatusBar = ss;
        this.MainDockPanel = dp;
        }

    }

    public class verb : IVerb
    {
        private string _category;
        private string _name;
        private string _text;
        private string _pic;
        Action<object, System.EventArgs> _action;

        public verb(string text, string name, string pic, string category, Action<object, System.EventArgs> action)
        {
            _action = action;
            _category = category;
            _name = name;
            _text = text;
            _pic = pic;
        }

        public string Category
        {
            get { return this._category; }
        }

        public string Pic
        {
            get { return this._pic; }
        }

        public string Text
        {
            get { return this._text; }
        }
        
        public string Name
        {
            get { return this._name; }
        }

        public void Action(object sender, System.EventArgs e)
        {
            if(_action!=null)
                _action(sender,e);
        }

    }
}
