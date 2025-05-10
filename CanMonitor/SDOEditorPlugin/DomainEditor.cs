using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using libEDSsharp;

namespace SDOEditorPlugin
{
    public partial class DomainEditor : Form
    {

        public delegate void OnUpdateValue(string value);
        public event OnUpdateValue UpdateValue;
        
        public delegate void OnSaveValue(string file, ODentry od);
        public event OnSaveValue SaveValue;


        ODentry tod;

        public string newvalue;
        public DomainEditor(ODentry od)
        {
            DialogResult = DialogResult.Cancel;

            InitializeComponent();
                        
            label_index.Text = string.Format("0x{0:X4}", od.Index);
            label_sub.Text = string.Format("0x{0:X2}", od.Subindex);
            label_name.Text = od.parameter_name;
            textBox_desc.Text = od.Description;

            tod = od;
        }




        private void CloseBtn_Click(object sender, EventArgs e)
        {
        DialogResult = DialogResult.OK;
        }


        private void DownloadFileBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveValue?.Invoke(sfd.FileName, tod);
            }
        }


        private void UploadFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            
            if(ofd.ShowDialog()== DialogResult.OK)
            {

                byte[] b = File.ReadAllBytes(ofd.FileName);

                string str = System.Text.Encoding.ASCII.GetString(b);
                UpdateValue?.Invoke(str);

            }
        }
    }
}
