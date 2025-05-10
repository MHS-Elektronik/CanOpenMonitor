using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace CanMonitor
{
    public partial class InfoLogDocument : DockContent
    {
        public InfoLogDocument()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;            
        }
    
    
        public void AddLine(string text)
        {  
            textBox_info.AppendText(text + "\r\n");
        }
    
    
        public void ClearLog()
        {
            textBox_info.Clear();
        }
    }
}
