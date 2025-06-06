using libCanopenSimple;
using Microsoft.CSharp;
using N_SettingsMgr;
using PDOInterface;
//using PFMMeasurementService.Models.Devices.Buses;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace CanMonitor
{
    public partial class CanLogForm : DockContent, ICanDocument
    {
        public DockPanel dockpanel;

        List<ListViewItem> listitems = new List<ListViewItem>();
       
        Dictionary<UInt32, List<byte>> sdotransferdata = new Dictionary<uint, List<byte>>();

        System.Windows.Forms.Timer updatetimer = new System.Windows.Forms.Timer();

        StreamWriter sw;

   
        Timer savetimer;

        public CanLogForm()
        {
            try
            {
                SettingsMgr.readXML(Path.Combine(Program.appdatafolder, "settings.xml"));
            }
            catch (Exception e)
            {
            }

            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;        

            Program.lco.dbglevel = debuglevel.DEBUG_NONE;

            Program.lco.nmtecevent += log_NMTEC;
            Program.lco.nmtevent += log_NMT;
            Program.lco.sdoevent += log_SDO;
            Program.lco.pdoevent += log_PDO;
            Program.lco.emcyevent += log_EMCY;

            listView1.DoubleBuffering(true);                            
            listView1.ListViewItemSorter = null;

            updatetimer.Interval = 1000;
            updatetimer.Tick += updatetimer_Tick;

            updatetimer_Tick(null, new EventArgs());

            this.Shown += Form1_Shown;

            updatetimer.Enabled = true;

            savetimer = new Timer();
            savetimer.Interval = 10;
            savetimer.Tick += Savetimer_Tick;
            savetimer.Enabled = true;

            last = DateTime.Now;
        }

   
        DateTime last;

        public void clearlist()
        {
            lock (listitems)
            {
                listitems.Clear();
                listView1.Items.Clear();
            }
        }

        private void Savetimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            if (Properties.Settings.Default.AutoFileLog == true && now.Hour != last.Hour)
            {
                //Force a save

                string dtstring = now.ToString("dd-MMM-yyyy-HH-mm-ss");

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                desktopPath += Path.DirectorySeparatorChar + Properties.Settings.Default.FileLogFolder;
                dtstring = desktopPath + Path.DirectorySeparatorChar + dtstring;

                if (!Directory.Exists(desktopPath))
                {
                    Directory.CreateDirectory(desktopPath);
                }


                dtstring += ".xml";
                dosave(dtstring);

                listView1.Items.Clear();

            }

            last = DateTime.Now;
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Text = "CanOpen Log";          
        }

      

        void appendfile(string[] ss)
        {

            if (sw == null)
                return;

            StringBuilder sb = new StringBuilder();
            foreach (string s in ss)
            {
                sb.AppendFormat("{0}\t", s);
            }

            sw.WriteLine(sb.ToString());

        }

        void updatetimer_Tick(object sender, EventArgs e)
        {

            bool limit = Properties.Settings.Default.limitlines;
            int linelimit = Properties.Settings.Default.linelimit;
            bool autoscroll = Properties.Settings.Default.CanAutoscroll;

            if (listitems.Count != 0)
                lock (listitems)
                {
                    listView1.BeginUpdate();

                    listView1.Items.AddRange(listitems.ToArray());

                    listitems.Clear();

                    if (autoscroll && listView1.Items.Count > 2)
                        listView1.EnsureVisible(listView1.Items.Count - 1);

                    if (limit)
                    {
                        while (listView1.Items.Count > linelimit)
                            listView1.Items.RemoveAt(0);

                    }

                    listView1.EndUpdate();

                }
        }


        private void log_NMT(canpacket payload, DateTime dt)
        {
            string[] items = new string[6];
            items[0] = dt.ToString("MM/dd/yyyy HH:mm:ss.fff");
            items[1] = "NMT";
            items[2] = string.Format("{0:x3}", payload.cob);
            items[3] = "";
            items[4] = payload.DataToString();

            string msg = "";

            if (payload.len != 2)
                return;

            switch (payload.dataByte[0])
            {
                case 0x01:
                    msg = "Enter operational";
                    break;
                case 0x02:
                    msg = "Enter stop";
                    break;
                case 0x80:
                    msg = "Enter pre-operational";
                    break;
                case 0x81:
                    msg = "Reset node";
                    break;
                case 0x82:
                    msg = "Reset communications";
                    break;

            }

            if (payload.dataByte[1] == 0)
            {
                msg += " - All nodes";
            }
            else
            {
                msg += string.Format(" - Node 0x{0:x2}", payload.dataByte[1]);
            }

            items[5] = msg;

            if (Properties.Settings.Default.showNMTEC)
            {

                ListViewItem i = new ListViewItem(items);

                i.ForeColor = Color.Red;

                lock (listitems)
                    listitems.Add(i);
            }

            appendfile(items);
        }


        private void log_NMTEC(canpacket payload, DateTime dt)
        {
            string[] items = new string[6];
            items[0] = dt.ToString("MM/dd/yyyy HH:mm:ss.fff");
            items[1] = "NMTEC";
            items[2] = string.Format("{0:x3}", payload.cob);
            items[3] = string.Format("{0:x3}", payload.cob & 0x0FF);
            items[4] = payload.DataToString();

            string msg = "";
            switch (payload.dataByte[0])
            {
                case 0:
                    msg = "BOOT";
                    break;
                case 4:
                    msg = "STOPPED";
                    break;
                case 5:
                    msg = "Heart Beat";
                    break;
                case 0x7f:
                    msg = "Heart Beat (Pre op)";
                    break;

            }

            items[5] = msg;

            ListViewItem i = new ListViewItem(items);

            i.ForeColor = Color.DarkGreen;

            appendfile(items);

            if (Properties.Settings.Default.showNMTEC && (Properties.Settings.Default.showHB == true || payload.dataByte[0] == 0))
            {
                lock (listitems)
                {
                    listitems.Add(i);
                }
            }
        }


        private void log_SDO(canpacket payload, DateTime dt)
        {
            string[] items = new string[6];
            items[0] = dt.ToString("MM/dd/yyyy HH:mm:ss.fff");
            items[1] = "SDO";
            items[2] = string.Format("{0:x3}", payload.cob);

            if (payload.cob >= 0x580 && payload.cob < 0x600)
            {
                items[3] = string.Format("{0:x3}", ((payload.cob + 0x80) & 0x0FF));
            }
            else
            {
                items[3] = string.Format("{0:x3}", payload.cob & 0x0FF);
            }

            items[4] = payload.DataToString();

            string msg = "";

            int SCS = payload.dataByte[0] >> 5; //7-5

            int n = (0x03 & (payload.dataByte[0] >> 2)); //3-2 data size for normal packets
            int e = (0x01 & (payload.dataByte[0] >> 1)); // expidited flag
            int s = (payload.dataByte[0] & 0x01); // data size set flag //also in block
            int c = s;

            int sn = (0x07 & (payload.dataByte[0] >> 1)); //3-1 data size for segment packets
            int t = (0x01 & (payload.dataByte[0] >> 4));  //toggle flag

            int cc = (0x01 & (payload.dataByte[0] >> 2));

            UInt16 index = (UInt16)(payload.dataByte[1] + (payload.dataByte[2] << 8));
            byte sub = payload.dataByte[3];

            int valid = 7;
            int validsn = 7;

            if (n != 0)
                valid = 8 - (7 - n);

            if (sn != 0)
                validsn = 8 - (7 - sn);

            if (payload.cob >= 0x580 && payload.cob <= 0x600)
            {
                string mode = "";
                string sdoproto = "";

                string setsize = "";

                switch (SCS)
                {
                    case 0:
                        mode = "upload segment response";
                        sdoproto = string.Format("{0} {1} Valid bytes = {2} {3}", mode, t == 1 ? "TOG ON" : "TOG OFF", validsn, c == 0 ? "MORE" : "END");

                        if (c == 1)
                        {
                            //ipdo.endsdo(payload.cob, index, sub, null);
                            //END
                        }

                        if (sdotransferdata.ContainsKey(payload.cob))
                        {

                            for (int x = 1; x <= validsn; x++)
                            {
                                sdotransferdata[payload.cob].Add(payload.dataByte[x]);
                            }

                            if (c == 1)
                            {

                                StringBuilder hex = new StringBuilder(sdotransferdata[payload.cob].Count * 2);
                                StringBuilder ascii = new StringBuilder(sdotransferdata[payload.cob].Count * 2);
                                foreach (byte b in sdotransferdata[payload.cob])
                                {
                                    hex.AppendFormat("{0:x2} ", b);
                                    ascii.AppendFormat("{0}", (char)Convert.ToChar(b));
                                }

                                //  textBox_info.Invoke(new MethodInvoker(delegate
                                //  {
                                //      textBox_info.AppendText(String.Format("SDO UPLOAD COMPLETE for cob 0x{0:x3}\r\n", payload.cob))
                                //
                                //      textBox_info.AppendText(hex.ToString() + "\r\n");
                                //     textBox_info.AppendText(ascii.ToString() + "\r\n\r\n");
                                //
                                //                                }));
                            }

                        }

                        break;
                    case 1:
                        mode = "download segment response";
                        sdoproto = string.Format("{0} {1}", mode, t == 1 ? "TOG ON" : "TOG OFF");
                        break;
                    case 2:
                        mode = "initate upload response";
                        string nbytes = "";

                        if (e == 1 && s == 1)
                        {
                            //n is valid
                            nbytes = string.Format("Valid bytes = {0}", 4 - n);
                        }

                        if (e == 0 && s == 1)
                        {
                            UInt32 isize = (UInt32)Convert.ToUInt32(payload.data[0]);
                            nbytes = string.Format("Bytes = {0}", isize);

                            if (sdotransferdata.ContainsKey(payload.cob))
                                sdotransferdata.Remove(payload.cob);

                            sdotransferdata.Add(payload.cob, new List<byte>());
                        }

                        sdoproto = string.Format("{0} {1} {2} 0x{3:x4}/{4:x2}", mode, nbytes, e == 1 ? "Normal" : "Expedite", index, sub);
                        break;
                    case 3:
                        mode = "initate download response";
                        sdoproto = string.Format("{0} 0x{1:x4}/{2:x2}", mode, index, sub);
                        break;

                    case 5:
                        mode = "Block download response";

                        byte segperblock = payload.dataByte[4];
                        sdoproto = string.Format("{0} 0x{1:x4}/{2:x2} Blksize = {3}", mode, cc == 0 ? "NO SERVER CRC" : "SERVER CRC", index, sub, segperblock);

                        break;


                    default:
                        mode = string.Format("SCS {0}", SCS);
                        break;

                }
                msg = sdoproto;
            }
            else
            {
                //Client to server

                string mode = "";
                string sdoproto = "";

                switch (SCS)
                {
                    case 0:
                        mode = "download segment request";
                        sdoproto = string.Format("{0} {1} Valid bytes = {2} {3}", mode, t == 1 ? "TOG ON" : "TOG OFF", validsn, c == 0 ? "MORE" : "END");


                        if (sdotransferdata.ContainsKey(payload.cob))
                        {

                            for (int x = 1; x <= validsn; x++)
                            {
                                sdotransferdata[payload.cob].Add(payload.dataByte[x]);
                            }

                            if (c == 1)
                            {

                                StringBuilder hex = new StringBuilder(sdotransferdata[payload.cob].Count * 2);
                                StringBuilder ascii = new StringBuilder(sdotransferdata[payload.cob].Count * 2);
                                foreach (byte b in sdotransferdata[payload.cob])
                                {
                                    hex.AppendFormat("{0:x2} ", b);
                                    ascii.AppendFormat("{0}", (char)Convert.ToChar(b));
                                }

                                //sdoproto += "\nDATA = " + hex.ToString() + "(" + ascii + ")";

                                /*  textBox_info.Invoke(new MethodInvoker(delegate
                                  {
                                      textBox_info.AppendText(String.Format("SDO DOWNLOAD COMPLETE for cob 0x{0:x3}\n", payload.cob));

                                      textBox_info.AppendText(hex.ToString() + "\n");
                                      textBox_info.AppendText(ascii.ToString() + "\n");
                                  }));*/


                                //Console.WriteLine(hex.ToString());
                                //Console.WriteLine(ascii.ToString());

                                sdotransferdata.Remove(payload.cob);
                            }
                        }


                        break;
                    case 1:
                        string nbytes = "";

                        if (e == 1 && s == 1)
                        {
                            //n is valid
                            nbytes = string.Format("Valid bytes = {0}", 4 - n);
                        }

                        if (e == 0 && s == 1)
                        {
                            UInt32 isize2 = (UInt32)Convert.ToUInt32(payload.data[0]);
                            nbytes = string.Format("Bytes = {0}", isize2);
                        }

                        mode = "initate download request";
                        sdoproto = string.Format("{0} {1} {2} 0x{3:x4}/{4:x2}", mode, nbytes, e == 1 ? "Normal" : "Expedite", index, sub);
                        if (sdotransferdata.ContainsKey(payload.cob))
                            sdotransferdata.Remove(payload.cob);

                        sdotransferdata.Add(payload.cob, new List<byte>());

                        break;
                    case 2:
                        mode = "initate upload request";
                        sdoproto = string.Format("{0} 0x{1:x4}/{2:x2}", mode, index, sub);
                        break;
                    case 3:
                        mode = "upload segment request";
                        sdoproto = string.Format("{0} {1}", mode, t == 1 ? "TOG ON" : "TOG OFF");
                        break;

                    case 5:
                        mode = "Block download";
                        sdoproto = string.Format("{0}", mode);
                        break;

                    case 6:
                        mode = "Initate Block download request";

                        UInt32 isize = (UInt32)Convert.ToUInt32(payload.data[0]);

                        sdoproto = string.Format("{0} 0x{1:x4}/{2:x2} Size = {3}", mode, cc == 0 ? "NO CLIENT CRC" : "CLIENT CRC", index, sub, isize);
                        break;


                    default:
                        mode = string.Format("CSC {0}", SCS);
                        break;
                }

                msg = sdoproto;
            }

            if ((payload.dataByte[0] & 0x80) != 0)
            {
                byte[] errorcode = new byte[4];
                errorcode[0] = payload.dataByte[4];
                errorcode[1] = payload.dataByte[5];
                errorcode[2] = payload.dataByte[6];
                errorcode[3] = payload.dataByte[7];

                UInt32 err = BitConverter.ToUInt32(errorcode, 0);

                if (ErrorCodes.sdoerrormessages.ContainsKey(err))
                {

                    msg += " " + ErrorCodes.sdoerrormessages[err];

                }
            }
            else
            {
                if (Program.pluginManager.ipdo != null)
                    msg += " " + Program.pluginManager.ipdo.decodesdo(index, sub, payload);
            }
            items[5] = msg;
            appendfile(items);

            if (Properties.Settings.Default.showsdo)
            {
                ListViewItem i = new ListViewItem(items);

                if ((payload.dataByte[0] & 0x80) != 0)
                {
                    i.BackColor = Color.Orange;
                }

                i.ForeColor = Color.DarkBlue;

                lock (listitems)
                    listitems.Add(i);
            }
        }


        private void log_PDO(canpacket[] payloads, DateTime dt)
        {
            foreach (canpacket payload in payloads)
            {

                string[] items = new string[6];
                items[0] = dt.ToString("MM/dd/yyyy HH:mm:ss.fff");
                items[1] = "PDO";
                items[2] = string.Format("{0:x3}", payload.cob);
                items[3] = "";
                items[4] = payload.DataToString();

                if (Program.pluginManager.pdoprocessors.ContainsKey(payload.cob))
                {
                    string msg = null;
                    try
                    {
                        msg = Program.pluginManager.pdoprocessors[payload.cob](payload);
                    }
                    catch (Exception)
                    {
                        msg += "!! DECODE EXCEPTION !!";
                    }

                    if (msg == null)
                    {
                        continue;

                    }
                    else
                    {
                        items[5] = msg;
                    }
                }
                else
                {
                    items[5] = string.Format("Len = {0}", payload.len);
                }

                if (Properties.Settings.Default.showpdo)
                {
                    ListViewItem i = new ListViewItem(items);

                    lock (listitems)
                        listitems.Add(i);
                }

                appendfile(items);
            }
        }


        private void log_EMCY(canpacket payload, DateTime dt)
        {
            string[] items = new string[6];
            string[] items2 = new string[5];

            items[0] = dt.ToString("MM/dd/yyyy HH:mm:ss.fff");
            items[1] = "EMCY";
            items[2] = string.Format("{0:x3}", payload.cob);
            items[3] = string.Format("{0:x3}", payload.cob - 0x080);
            items[4] = payload.DataToString();
            //items[4] = "EMCY";

            items2[0] = dt.ToString("MM/dd/yyyy HH:mm:ss.fff");
            items2[1] = items[2];
            items2[2] = items[3];

            UInt16 code = (UInt16)(payload.dataByte[0] + (payload.dataByte[1] << 8));
            byte bits = (byte)(payload.dataByte[3]);
            UInt32 info = (UInt32)(payload.dataByte[4] + (payload.dataByte[5] << 8) + (payload.dataByte[6] << 16) + (payload.dataByte[7] << 24));

            if (ErrorCodes.errcode.ContainsKey(code))
            {

                string bitinfo;

                if (ErrorCodes.errbit.ContainsKey(bits))
                {
                    bitinfo = ErrorCodes.errbit[bits];
                }
                else
                {
                    bitinfo = string.Format("bits 0x{0:x2}", bits);
                }

                items[5] = string.Format("Error: {0} - {1} info 0x{2:x8}", ErrorCodes.errcode[code], bitinfo, info);
            }
            else
            {
                items[5] = string.Format("Error code 0x{0:x4} bits 0x{1:x2} info 0x{2:x8}", code, bits, info);
            }

            items2[3] = items[5];

            ListViewItem i = new ListViewItem(items);
            ListViewItem i2 = new ListViewItem(items2);

            i.ForeColor = Color.White;
            i2.ForeColor = Color.White;

            if (code == 0)
            {
                i.BackColor = Color.Green;
                i2.BackColor = Color.Green;

            }
            else
            {
                i.BackColor = Color.Red;
                i2.BackColor = Color.Red;

            }

            if (Properties.Settings.Default.showsdo)
            {
                lock (listitems)
                    listitems.Add(i);

            }
            appendfile(items);
        }

      
   
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.CanAutoscroll)
            {
                if (listView1.Items.Count > 1)
                    listView1.EnsureVisible(listView1.Items.Count - 1);
            }
        }

        #region pluginloader

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

      
        public void dosave(string filename)
        {
            XElement xeRoot = new XElement("CanOpenMonitor");

            foreach (ListViewItem i in listView1.Items)
            {
                XElement xeRow = new XElement("Packet", new XAttribute("backcol", i.BackColor.Name), new XAttribute("forcol", i.ForeColor.Name));
                int x = 0;

                foreach (ListViewItem.ListViewSubItem subItem in i.SubItems)
                {
                    XElement xeCol = new XElement(listView1.Columns[x].Text);
                    xeCol.Value = subItem.Text;
                    xeRow.Add(xeCol);
                    // To add attributes use XAttributes
                    x++;
                }
                xeRoot.Add(xeRow);

            }

            xeRoot.Save(filename);
        }


        public void doload()
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.ShowHelp = true;
            sfd.Filter = "(*.xml)|*.xml";

            try
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {

                    listView1.Items.Clear();

                    XElement xeRoot = XElement.Load(sfd.FileName);
                    XName Packet = XName.Get("Packet");


                    foreach (var packetelement in xeRoot.Elements(Packet))
                    {
                        XName XTimestamp = XName.Get("Timestamp");
                        XName XType = XName.Get("Type");
                        XName XCob = XName.Get("COB");
                        XName XNodeT = XName.Get("Node");
                        XName XPayload = XName.Get("Payload");
                        XName XInfo = XName.Get("Info");

                        string[] bits = new string[6];

                        bits[0] = packetelement.Element(XTimestamp).Value;
                        bits[1] = packetelement.Element(XType).Value;
                        bits[2] = packetelement.Element(XCob).Value;
                        bits[3] = packetelement.Element(XNodeT).Value;
                        bits[4] = packetelement.Element(XPayload).Value;
                        bits[5] = packetelement.Element(XInfo).Value;

                        string cobx = bits[2].ToUpper();
                        UInt16 cob = Convert.ToUInt16(cobx, 16);

                        byte[] b = new byte[bits[4].Length / 2];
                        for (int x = 0; x < bits[4].Length / 2; x++)
                        {
                            string s = bits[4].Substring(x * 2, 2);
                            b[x] = byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
                        }

                        canpacket[] p = new canpacket[1];
                        p[0] = new canpacket(cob, b, false, false, false);

                        string d = bits[0];
                        string[] d2 = d.Split(' ');
                        string[] d3 = d2[0].Split('/');
                        string[] d4 = d2[1].Split(':');


                        //  11/16/2023 17:56:53.474
                        //  "2018-08-18T07:22:16.0000000Z"
                        string d5 = $"{d3[2]}-{d3[0]}-{d3[1]}T{d4[0]}:{d4[1]}:{d4[2]}";


                        DateTime dt = DateTime.Parse(d5);

                        switch (bits[1])
                        {
                            case "PDO":
                                log_PDO(p, dt);
                                break;

                            case "SDO":
                                log_SDO(p[0], dt);
                                break;

                            case "NMT":
                                log_NMT(p[0], dt);
                                break;

                            case "NMTEC":
                                log_NMTEC(p[0], dt);
                                break;

                            case "EMCY":
                                log_EMCY(p[0], dt);
                                break;
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                listView1.EndUpdate();

            }

        }


        public void doload2()
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.ShowHelp = true;
            sfd.Filter = "(*.log)|*.log";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(sfd.FileName);

                foreach (string line in lines)
                {
                    try
                    {

                        string[] bits = line.Split(',');
                        UInt16 cob = Convert.ToUInt16(bits[1], 16);
                        byte len = Convert.ToByte(bits[2], 16);


                        canpacket[] p = new canpacket[1];
                        p[0] = new canpacket();
                        p[0].cob = cob;
                        p[0].len = len;

                        for (int x = 0; x < len; x++)
                        {
                            p[0].dataByte[x] = Convert.ToByte(bits[3].Substring(x * 2, 2), 16);
                        }
                        p[0].len = len;

                        DateTime dt = DateTime.Parse(bits[0]);

                        if (cob < 0x80)
                            log_NMT(p[0], dt);

                        if (cob >= 0x80 && cob < 0x100)
                            log_EMCY(p[0], dt);

                        if (cob >= 0x180 && cob < 0x580)
                            log_PDO(p, dt);

                        if (cob >= 0x580 && cob < 0x700)
                            log_PDO(p, dt);

                        if (cob >= 0x700)
                            log_NMTEC(p[0], dt);


                    }
                    catch (Exception)
                    {

                    }

                }
            }
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                // Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipText = "Can monitor is still running";
                notifyIcon1.BalloonTipTitle = "Can Monitor";
                notifyIcon1.ShowBalloonTip(2000);
            }
        }


        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }



        #endregion

        #region controls



        #endregion
    }

}
