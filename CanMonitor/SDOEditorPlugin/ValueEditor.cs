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
    public partial class ValueEditor : Form
    {

        public delegate void OnUpdateValue(string value);
        public event OnUpdateValue UpdateValue;
        
        ODentry tod;

        public string newvalue;
        public ValueEditor(ODentry od, string currentval)
        {
            DialogResult = DialogResult.Cancel;

            InitializeComponent();
            label_default.Text = od.defaultvalue;
            label_index.Text = string.Format("0x{0:X4}", od.Index);
            label_sub.Text = string.Format("0x{0:X2}", od.Subindex);
            label_name.Text = od.parameter_name;
            textBox_desc.Text = od.Description.Replace("\n", Environment.NewLine);
            CurrentValueLabel.Text = currentval;

            tod = od;
            updatestring();
        }

        private void updatestring()
        {
            if (tod == null)
                return;

            if(tod.datatype == DataType.VISIBLE_STRING)
            {
                label_length.Text = String.Format("{0}/{1}", NewValueEntry.Text.Length, tod.defaultvalue.Length);
            }
            else
            {
                label_length.Text = "";
            }
        }

    
        private void UpdateCloseBtn_Click(object sender, EventArgs e)
        {

            newvalue = NewValueEntry.Text;
            try
            {
                UpdateValue?.Invoke(NewValueEntry.Text);
            }
            catch (Exception ex)
            {

            }

            DialogResult = DialogResult.OK;
        }


        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            newvalue = NewValueEntry.Text;

            try
            {
                UpdateValue?.Invoke(NewValueEntry.Text);
            }
            catch(Exception ex)
            {

            }
            CurrentValueLabel.Text = newvalue;
        }
                                  
                                  
        private void NewValueEntry_TextChanged(object sender, EventArgs e)
        {
            updatestring();     
        }
    }
}
