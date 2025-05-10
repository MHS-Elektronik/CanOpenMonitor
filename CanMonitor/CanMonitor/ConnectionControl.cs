using libCanopenSimple;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanMonitor
{
    public partial class ConnectionControl : Form
    {
        private MainDockForm mainDockForm;
        
        public ConnectionControl(MainDockForm main_form)
        {
            mainDockForm = main_form;
            InitializeComponent();
    
            this.FormClosing += FormClosingFunc;
            PaintPortList();
            
            comboBox_rate.SelectedIndex = (int)Program.driverloader.StrToBusspeed(Properties.Settings.Default.lastrate);
                        
            if (Program.lco.isopen())
              SetupOpenButton(false);
            else
              SetupOpenButton(true);            
        }


        private void FormClosingFunc(object sender, FormClosingEventArgs e)
        {
            GetAndSaveConnectionSetup();
        }
   
   
        private void PaintPortList()
        {        
            comboBox_port.Text = "";
            comboBox_port.Items.Clear();
            driverport select_dp = new driverport();
            select_dp.port = Properties.Settings.Default.lastport;
            select_dp.driver = Properties.Settings.Default.lastdriver;
               
            foreach (driverport dp in Program.driverloader._driverport)            
                comboBox_port.Items.Add(dp);
   
            foreach(driverport dp in comboBox_port.Items)
            {
                if (dp.issamedriver(select_dp))
                {
                    comboBox_port.SelectedItem = dp;
                    break;
                }                
            }     
        }
        
        
        private void SetupOpenButton(bool open)
        {
            if (open)
            {
                button_open.BackColor = Color.Green;
                button_open.Text = "Open";                  
            }
            else
            {
                button_open.BackColor = Color.Red;
                button_open.Text = "Close";
            }
        }    


        private void button_open_Click(object sender, EventArgs e)
        {
        
            if (button_open.Text == "Close")
            {
                // close PC-CAN device
                mainDockForm.SetConnected(false);
                SetupOpenButton(true);
            }
            else
            {
                GetAndSaveConnectionSetup();
                // open PC-CAN device 
                if (mainDockForm.SetConnected(true))
                  SetupOpenButton(false);
            }
        }


        private void button_refresh_Click(object sender, EventArgs e)
        {
            Program.driverloader.enumerateports();
            PaintPortList();                        
        }
        
        
        private void GetAndSaveConnectionSetup()
        {
            driverport dp = (driverport)comboBox_port.SelectedItem;
            Properties.Settings.Default.lastport = dp.port;
            Properties.Settings.Default.lastdriver = dp.driver;
            Properties.Settings.Default.lastrate = Program.driverloader.BusspeedToStr((BUSSPEED)comboBox_rate.SelectedIndex);
            Properties.Settings.Default.Save();
        }

    }
}
