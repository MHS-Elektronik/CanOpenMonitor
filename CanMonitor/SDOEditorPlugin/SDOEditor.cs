using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;
using libEDSsharp;
using System.IO;
using Xml2CSharp;
using libCanopenSimple;
using WeifenLuo.WinFormsUI.Docking;
using System.Security.Cryptography;


namespace SDOEditorPlugin
{
    public enum TReadMode
    {
        READ_STOP = 0,
        READ_ALL,
        READ_SEL
    }
    
    
    public struct sdocallbackhelper
    {
        public SDO sdo;
        public ODentry od;
    }
    
    
    public partial class SDOEditor : DockContent
    {
        public ToolStripMenuItem SDOEdFileRecent;
        EDSsharp eds;
        libCanopenSimple.libCanopenSimple lco;
        DockPanel MainDockPanel;
        string filename = null;
        private string appdatafolder;

        private List<string> _mru = new List<string>();
        private System.Timers.Timer RefreshTimer = new System.Timers.Timer();
        bool isdcf = false;

        bool ReadAutoRepeatEnable;
        bool CustomView;
        TReadMode AktivReadMode;
        List<ListViewItem> CustomList = new List<ListViewItem>();


        public SDOEditor(libCanopenSimple.libCanopenSimple lco, DockPanel main_dock_panel)
        {
            this.lco = lco;
            MainDockPanel = main_dock_panel;
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true );
            RefreshTimer.Elapsed += RefreshTimer_Tick;
            RefreshTimer.Interval = 1000; //(int)numericUpDown_refreshtime.Value*1000;<*>
            RefreshTimer.AutoReset = false;
        }                        

        
        private void InitSdoRead(ListViewItem lvi)
        {
            lvi.BackColor = Color.White;
            sdocallbackhelper help = (sdocallbackhelper)lvi.Tag;

            if (help.od.objecttype != ObjectType.DOMAIN)
            {
                SDO sdo = lco.SDOread((byte)numericUpDown_node.Value, (UInt16)help.od.Index, (byte)help.od.Subindex, gotit);
                help.sdo = sdo;
                lvi.Tag = help;
            }
        }            


        private void RefreshTimer_Tick(object sender, ElapsedEventArgs e)
        {
            listView1.Invoke(new MethodInvoker(delegate
            {
                if (AktivReadMode == TReadMode.READ_ALL)
                {
                    foreach (ListViewItem lvi in listView1.Items)
                        InitSdoRead(lvi);
                }    
                else if (AktivReadMode == TReadMode.READ_SEL)
                {
                    foreach (ListViewItem lvi in listView1.SelectedItems)
                        InitSdoRead(lvi);
                }
            }));
        }


        private void loadeds(string filename)
        {
            if (filename == null || filename == "")
                return;
            
            bool isemptydcf = false;

            try
            {
                switch (Path.GetExtension(filename).ToLower())
                {
                    case ".xdd":
                        {

                            CanOpenXDD_1_1 xdd = new CanOpenXDD_1_1();
                            eds = xdd.ReadXML(filename);

                            if (eds == null)
                            {
                                CanOpenXDD xdd2 = new CanOpenXDD();
                                xdd2.readXML(filename);

                                Bridge b = new Bridge();
                                eds = xdd2.convert(xdd2.dev);
                            }

                            /*   
                            eds.xmlfilename = filename;
                            */
                        }

                        break;

                    case ".xml":
                        {
                            CanOpenXML coxml = new CanOpenXML();
                            coxml.readXML(filename);

                            Bridge b = new Bridge();

                            eds = b.convert(coxml.dev);
                            eds.xmlfilename = filename;
                        }

                        break;

                    case ".dcf":
                        {
                            isdcf = true;
                            if (listView1.Items.Count == 0)
                                isemptydcf = true;

                            eds = new EDSsharp();
                            eds.Loadfile(filename);
                        }
                        break;

                    case ".eds":
                        {
                            eds = new EDSsharp();
                            eds.Loadfile(filename);
                        }
                        break;


                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
            }

            CustomView = false;
            CustomList.Clear();
            UpdateTable(isdcf, isemptydcf);

            this.ToolTipText = eds.di.ProductName;
            this.DockHandler.TabText = "OD: " + eds.di.ProductName;

            this.filename = filename;
            addtoMRU(SDOEdFileRecent, filename);
        }


        private void UpdateTable(bool isdcf = false, bool isemptydcf = false)
        {
            listView1.BeginUpdate();
            if (!isdcf)
                listView1.Items.Clear();

            //           StorageLocation loc = StorageLocation
            foreach (ODentry tod in eds.ods.Values)
            {
                if (comboBoxtype.SelectedItem.ToString() != "ALL")
                {
                    if (comboBoxtype.SelectedItem.ToString() == "EEPROM" && (tod.prop.CO_storageGroup.ToUpper() != "EEPROM"))
                        continue;
                    if (comboBoxtype.SelectedItem.ToString() == "ROM" && (tod.prop.CO_storageGroup.ToUpper() != "ROM"))
                        continue;
                    if (comboBoxtype.SelectedItem.ToString() == "RAM" && (tod.prop.CO_storageGroup.ToUpper() != "RAM"))
                        continue;
                }
                if (tod.prop.CO_disabled == true)
                    continue;

                if (tod.Index < 0x2000 && checkBox_useronly.Checked == true)
                    continue;

                if (tod.objecttype == ObjectType.ARRAY || tod.objecttype == ObjectType.RECORD)
                {
                    foreach (ODentry subod in tod.subobjects.Values)
                    {
                        if (subod.Subindex == 0)
                            continue;

                        addtolist(subod, isdcf, isemptydcf);
                    }

                    continue;
                }
                addtolist(tod, isdcf, isemptydcf);
            }
            listView1.EndUpdate();
        }


        void adddcfvalue(ODentry od)
        {

            foreach (ListViewItem lvi in listView1.Items)
            {
                sdocallbackhelper help = (sdocallbackhelper)lvi.Tag;

                if ((help.od.Index == od.Index) && (help.od.Subindex == od.Subindex))
                {
                    lvi.SubItems[6].Text = od.actualvalue;
                }
            }
        }


        void addtolist(ODentry od, bool dcf, bool isemptydcf)
        {
            if (!dcf || isemptydcf)
            {
                string[] items = new string[7];
                items[0] = string.Format("0x{0:x4}", od.Index);
                items[1] = string.Format("0x{0:x2}", od.Subindex);

                if (od.parent == null)
                    items[2] = od.parameter_name;
                else
                    items[2] = od.parent.parameter_name + " -- " + od.parameter_name;

                if (od.datatype == DataType.UNKNOWN && od.parent != null)
                {
                    items[3] = od.parent.datatype.ToString();
                }
                else
                {
                    items[3] = od.datatype.ToString();
                }
                items[4] = od.defaultvalue;
                items[5] = "";

                ListViewItem lvi = new ListViewItem(items);

                sdocallbackhelper help = new sdocallbackhelper();
                help.sdo = null;
                help.od = od;
                lvi.Tag = help;

                listView1.Items.Add(lvi);
            }

            if (dcf)
            {
                adddcfvalue(od);
            }

        }


        void upsucc(SDO sdo)
        {
            listView1.BeginInvoke(new MethodInvoker(delegate
            {
                foreach (ListViewItem lvi in listView1.Items)
                {
                    sdocallbackhelper help = (sdocallbackhelper)lvi.Tag;

                    if (help.sdo != sdo)
                        continue;

                    if (help.od.objecttype != ObjectType.DOMAIN)
                    {
                        sdo = lco.SDOread((byte)numericUpDown_node.Value, (UInt16)help.od.Index, (byte)help.od.Subindex, gotit);
                        help.sdo = sdo;
                        lvi.Tag = help;

                        break;
                    }
                }
            }));


        }


        void testnumber(ListViewItem lvi)
        {
            Int64 i1, i2;

            if (Int64.TryParse(lvi.SubItems[5].Text, out i1) && Int64.TryParse(lvi.SubItems[4].Text, out i2))
            {
                if (i1 != i2)
                {
                    lvi.BackColor = Color.Red;
                }
            }
        }


        void gotit(SDO sdo)
        {
            try
            {
                System.Threading.Thread.Sleep(10);

                listView1.Invoke(new MethodInvoker(delegate
                {

                    if (lco.getSDOQueueSize() == 0)
                    {
                        if ((ReadAutoRepeatEnable == true) && (AktivReadMode != TReadMode.READ_STOP))
                            RefreshTimer.Start();
                        else    
                            {
                            AktivReadMode = TReadMode.READ_STOP;
                            UpdateReadWriteButtons();
                            }    
                    }
                    label_sdo_queue_size.Text = string.Format("SDO Queue Size: {0}", lco.getSDOQueueSize());
                    //listView1.BeginUpdate();
                    foreach (ListViewItem lvi in listView1.Items)
                    {
                        sdocallbackhelper h = (sdocallbackhelper)lvi.Tag;
                        if (h.sdo == sdo)
                        {
                            if (sdo.state == SDO.SDO_STATE.SDO_ERROR)
                            {
                                lvi.SubItems[5].Text = " **ERROR **";
                                return;
                            }

                            //if (sdo.exp == true)
                            {

                                DataType meh = h.od.datatype;
                                if (meh == DataType.UNKNOWN && h.od.parent != null)
                                    meh = h.od.parent.datatype;


                                //item 5 is the read value item 4 is the actual value

                                switch (meh)
                                {
                                    case DataType.REAL32:

                                        float myFloat = System.BitConverter.ToSingle(BitConverter.GetBytes(h.sdo.expitideddata), 0);
                                        lvi.SubItems[5].Text = myFloat.ToString();

                                        float fout;
                                        if (float.TryParse(lvi.SubItems[4].Text, out fout))
                                        {
                                            if (fout != myFloat)
                                            {
                                                lvi.BackColor = Color.Red;
                                            }
                                        }

                                        break;

                                    case DataType.REAL64:
                                        {
                                            double myDouble = System.BitConverter.ToDouble(h.sdo.databuffer, 0);
                                            lvi.SubItems[5].Text = myDouble.ToString();

                                            //fixme bad test
                                            if (lvi.SubItems[5].Text != lvi.SubItems[4].Text)
                                            {
                                                lvi.BackColor = Color.Red;
                                            }

                                            break;
                                        }
                                    case DataType.INTEGER8:
                                        {
                                            testnumber(lvi);

                                            byte[] data = BitConverter.GetBytes(h.sdo.expitideddata);
                                            byte num = data[0];
                                            lvi.SubItems[5].Text = String.Format("[0x{0:X2}]  {0}", num);
                                            break;
                                        }
                                    case DataType.INTEGER16:
                                        {
                                            testnumber(lvi);

                                            byte[] data = BitConverter.GetBytes(h.sdo.expitideddata);
                                            Int16 num = BitConverter.ToInt16(data, 0);
                                            lvi.SubItems[5].Text = String.Format("[0x{0:X4}]  {0}", num);
                                            break;
                                        }
                                    case DataType.INTEGER32:
                                        {
                                            testnumber(lvi);

                                            byte[] data = BitConverter.GetBytes(h.sdo.expitideddata);
                                            Int32 num = BitConverter.ToInt32(data, 0);
                                            lvi.SubItems[5].Text = String.Format("[0x{0:X8}]  {0}", num);
                                            break;
                                        }
                                    case DataType.UNSIGNED8:
                                        {
                                            h.sdo.expitideddata &= 0x000000FF;
                                            lvi.SubItems[5].Text = String.Format("[0x{0:X2}]  {0}", h.sdo.expitideddata);
                                            testnumber(lvi);
                                            break;                                            
                                        }
                                    case DataType.UNSIGNED16:
                                        {
                                            h.sdo.expitideddata &= 0x0000FFFF;
                                            lvi.SubItems[5].Text = String.Format("[0x{0:X4}]  {0}", h.sdo.expitideddata);
                                            testnumber(lvi);
                                            break;                                            
                                        }
                                    case DataType.UNSIGNED32:
                                        {                                                
                                            lvi.SubItems[5].Text = String.Format("[0x{0:X8}]  {0}", h.sdo.expitideddata);
                                            testnumber(lvi);
                                            break;
                                        }
                                    case DataType.VISIBLE_STRING:

                                        lvi.SubItems[5].Text = System.Text.Encoding.UTF8.GetString(h.sdo.databuffer);
                                        if (lvi.SubItems[5].Text != lvi.SubItems[4].Text)
                                        {
                                            lvi.BackColor = Color.Red;
                                        }

                                        break;

                                    case DataType.OCTET_STRING:

                                        StringBuilder sb = new StringBuilder();

                                        foreach (byte b in h.sdo.databuffer)
                                        {
                                            sb.Append(string.Format("{0:x} ", b));
                                        }

                                        lvi.SubItems[5].Text = sb.ToString();

                                        //fixme bad test
                                        if (lvi.SubItems[5].Text != lvi.SubItems[4].Text)
                                        {
                                            lvi.BackColor = Color.Red;
                                        }

                                        break;


                                    case DataType.UNSIGNED64:
                                        {
                                            testnumber(lvi);

                                            UInt64 data = (UInt64)System.BitConverter.ToUInt64(h.sdo.databuffer, 0);
                                            lvi.SubItems[5].Text = String.Format("{0:x}", data);
                                        }
                                        break;

                                    case DataType.INTEGER64:
                                        {
                                            testnumber(lvi);

                                            Int64 data = (Int64)System.BitConverter.ToInt64(h.sdo.databuffer, 0);
                                            lvi.SubItems[5].Text = String.Format("{0:x}", data);
                                        }
                                        break;

                                    case DataType.BOOLEAN:
                                        {
                                            lvi.SubItems[5].Text = String.Format("{0}", h.sdo.expitideddata);
                                            testnumber(lvi);
                                        }
                                        break;


                                    case DataType.DOMAIN:
                                        // Console.WriteLine(sdo.ToString());

                                        lvi.SubItems[5].Text = String.Format("Domain Len = {0}", h.sdo.databuffer.Length);
                                        
                                        //       StreamWriter file = new StreamWriter(string.Format("DOMAIN_{0}-{1}.txt",h.sdo.index,h.sdo.subindex));

                                        //       for(int p=0;p<h.sdo.databuffer.Length;p++)
                                        //       {
                                        //           file.WriteLine(h.sdo.databuffer[p].ToString());
                                        //       }


                                        //       file.Close();


                                        break;


                                    default:
                                        lvi.SubItems[5].Text = " **UNSUPPORTED **";
                                        break;
                                }

                                if (lvi.BackColor == Color.Red)
                                {
                                    if (h.od.accesstype == EDSsharp.AccessType.ro || h.od.accesstype == EDSsharp.AccessType.@const)
                                    {
                                        lvi.BackColor = Color.Yellow;
                                    }
                                }
                            }
                            break;
                        }

                        h.od.actualvalue = lvi.SubItems[5].Text;
                    }
                    //listView1.EndUpdate();
                }));
            }
            catch (Exception e)
            {

            }
            return;
        }


        private SDO dovalueupdate(sdocallbackhelper h, string sval)
        {
            DataType dt = h.od.datatype;

            if (dt == DataType.UNKNOWN && h.od.parent != null)
                dt = h.od.parent.datatype;

            SDO sdo = null;

            switch (dt)
            {
                case DataType.REAL32:
                    {
                        float val = (float)new SingleConverter().ConvertFromString(sval);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, val, upsucc);
                        break;
                    }

                case DataType.REAL64:
                    {
                        double val = (double)new DoubleConverter().ConvertFromString(sval);
                        byte[] payload = BitConverter.GetBytes(val);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, payload, upsucc);
                        break;
                    }

                case DataType.INTEGER8:
                    {
                        sbyte val = (sbyte)new SByteConverter().ConvertFromString(sval);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, val, upsucc);
                        break;
                    }

                case DataType.INTEGER16:
                    {
                        Int16 val = (Int16)new Int16Converter().ConvertFromString(sval);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, val, upsucc);
                        break;
                    }


                case DataType.INTEGER32:
                    {
                        Int32 val = (Int32)new Int32Converter().ConvertFromString(sval);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, val, upsucc);
                        break;
                    }
                case DataType.UNSIGNED8:
                    {
                        byte val = (byte)new ByteConverter().ConvertFromString(sval);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, val, upsucc);
                        break;
                    }
                case DataType.UNSIGNED16:
                    {
                        UInt16 val = (UInt16)new UInt16Converter().ConvertFromString(sval);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, val, upsucc);
                        break;
                    }

                case DataType.UNSIGNED32:
                    {
                        UInt32 val = (UInt32)new UInt32Converter().ConvertFromString(sval);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, val, upsucc);
                        break;
                    }

                case DataType.INTEGER64:
                    {
                        Int64 val = (Int64)new Int64Converter().ConvertFromString(sval);
                        byte[] payload = BitConverter.GetBytes(val);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, payload, upsucc);
                        break;
                    }

                case DataType.UNSIGNED64:
                    {
                        UInt64 val = (UInt64)new UInt64Converter().ConvertFromString(sval);
                        byte[] payload = BitConverter.GetBytes(val);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, payload, upsucc);
                        break;
                    }

                case DataType.VISIBLE_STRING:
                    {
                        byte[] payload = Encoding.ASCII.GetBytes(sval);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, payload, upsucc);
                        break;
                    }

                case DataType.BOOLEAN:
                    {
                        byte val = (byte)new ByteConverter().ConvertFromString(sval);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, val, upsucc);
                        break;
                    }

                case DataType.DOMAIN:
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(sval);
                        sdo = lco.SDOwrite((byte)numericUpDown_node.Value, (UInt16)h.od.Index, (byte)h.od.Subindex, bytes, upsucc);
                        break;
                    }
                default:

                    break;
            }
            return sdo;
        }


        private void WriteSelectedSDOValue()
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            if (!lco.isopen())
            {
                MessageBox.Show("CAN not open");
                return;
            }

            sdocallbackhelper h = (sdocallbackhelper)listView1.SelectedItems[0].Tag;
            if (h.od.objecttype == ObjectType.DOMAIN)
            {    
                DomainEditor de = new DomainEditor(h.od);
                de.ShowDialog();
            }    
            else
            { 
                ValueEditor ve = new ValueEditor(h.od, listView1.SelectedItems[0].SubItems[5].Text);
                
                ve.UpdateValue += delegate (string s)
                {
                    h.sdo = dovalueupdate(h, s);
                    listView1.SelectedItems[0].Tag = h;
                };

/*                ve.SaveValue += delegate (string file, ODentry od)<*>
                {                
                    lco.SDOread((byte)this.numericUpDown_node.Value, od.Index, (byte)od.Subindex, delegate (SDO s)
                    {
                        try
                        {
                            if (s.state == SDO.SDO_STATE.SDO_FINISHED)
                            {
                                System.IO.File.WriteAllBytes(file, s.databuffer);
                            }
                            else
                            {
                                throw new Exception("SDO transfer failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.ToString());
                        }
                    });
                }; */
                ve.ShowDialog();
            }                        
        }


        void OpenRecentFile(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var filepath = (string)menuItem.Tag;
            loadeds(filepath);
            Show(MainDockPanel, DockState.Document);
        }


        private void addtoMRU(ToolStripMenuItem menue_item, string path)
        {
            // if it already exists remove it then let it readd itsself
            // so it will be promoted to the top of the list
            if (_mru.Contains(path))
                _mru.Remove(path);

            _mru.Insert(0, path);

            if (_mru.Count > 10)
                _mru.RemoveAt(10);

            populateMRU(menue_item);
        }


        private void populateMRU(ToolStripMenuItem menue_item)
        {
            menue_item.DropDownItems.Clear();

            foreach (var path in _mru)
            {
                var item = new ToolStripMenuItem(path);
                item.Tag = path;
                item.Click += OpenRecentFile;
                switch (Path.GetExtension(path))
                {
                    case ".xml":
                    case ".xdd":
                        item.Image = Properties.Resource1.GenericVSEditor_9905;
                        break;

                    case ".eds":
                        item.Image = Properties.Resource1.EventLog_5735;
                        break;
                }

                menue_item.DropDownItems.Add(item);
            }
        }


        private void SDOEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
        }


        public void SaveMruList()
        {
            var mruFilePath = Path.Combine(appdatafolder, "SDOMRU.txt");
            System.IO.File.WriteAllLines(mruFilePath, _mru);
        }


        private void SDOEditor_Load(object sender, EventArgs e)
        {

        }


        public void LoadMruList()
        {
            //First lets create an appdata folder

            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            appdatafolder = Path.Combine(folder, "CanMonitor");

            // Check if folder exists and if not, create it
            if (!Directory.Exists(appdatafolder))
                Directory.CreateDirectory(appdatafolder);

            var mruFilePath = Path.Combine(appdatafolder, "SDOMRU.txt");
            if (System.IO.File.Exists(mruFilePath))
                _mru.AddRange(System.IO.File.ReadAllLines(mruFilePath));

            populateMRU(SDOEdFileRecent);
        }


        public void SdoEditorSendSaveReq()
        {
            lco.SDOwrite((byte) numericUpDown_node.Value, (UInt16)0x1010, (byte)0x01, (UInt32)0x65766173, null);
        }


        public void SdoEditorWriteDcf()
        {
            if (isdcf == false)
            {
                MessageBox.Show("No DCF file loaded");
                return;
            }              
            if(numericUpDown_node.Value==0)
            {
                MessageBox.Show("Cannot write to node 0, please select a valid node");
                return;
            }

            foreach (ListViewItem lvi in listView1.Items)
            {

                string index = lvi.SubItems[0].Text;
                string sub = lvi.SubItems[1].Text;
                string name = lvi.SubItems[2].Text;

                UInt16 key = Convert.ToUInt16(index, 16);
                byte subi = Convert.ToByte(sub, 16);

                sdocallbackhelper help = (sdocallbackhelper)lvi.Tag;

                string edsstring = help.od.defaultvalue; //the eds value
                string actualstring = lvi.SubItems[5].Text;  // the dcf value
                string dcfstring = lvi.SubItems[6].Text;

                if(actualstring!=dcfstring )
                {
                    if (dcfstring != "")
                    {
                        sdocallbackhelper h = (sdocallbackhelper)lvi.Tag;
                        try
                        {
                            h.sdo = dovalueupdate(h, dcfstring);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(string.Format("Error writing to 0x{0:x4}/{1:x2} details :-\n{2}", h.od.Index, h.od.Subindex, ex.ToString()));
                        }

                        lvi.Tag = h;
                    }
                }
            }
        }
        
        
        public void SdoEditorLoadFile()
        {
            OpenFileDialog odf = new OpenFileDialog();
            odf.Filter = "All supported files (*.eds;*.xml;*.xdd;*.dcf)|*.eds;*.xml;*.xdd;*.dcf|XML Electronic Data Sheet (*.xdd)|*.xdd|Legacy CanOpenNode project (*.xml)|*.xml|Electronic Datasheet (*.eds)|*.eds|Device Configuration File (*.dcf)|*.dcf";
            if (odf.ShowDialog() == DialogResult.OK)
            {
                loadeds(odf.FileName);
                Show(MainDockPanel, DockState.Document);
            }

        }
        

        public void SdoEditorSaveDifference()
        {
            SaveFileDialog odf = new SaveFileDialog();
            odf.Filter = "(*.dcf)|*.dcf";
            if (odf.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem lvi in listView1.Items)
                {

                    string index = lvi.SubItems[0].Text;
                    string sub = lvi.SubItems[1].Text;
                    string name = lvi.SubItems[2].Text;

                    sdocallbackhelper help = (sdocallbackhelper)lvi.Tag;

                    string defaultstring = help.od.defaultvalue;
                    string currentstring = help.od.actualvalue;

                    UInt16 key = Convert.ToUInt16(index, 16);
                    UInt16 subi = Convert.ToUInt16(sub, 16);

                    if (subi == 0)
                    {
                        eds.ods[key].actualvalue = currentstring;
                    }
                    else
                    {
                        ODentry subod = eds.ods[key].Getsubobject(subi);
                        if (subod != null)
                        {
                            subod.actualvalue = currentstring;
                        }
                    }

                    // file.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}",index,sub,name,defaultstring,currentstring));
                }

                eds.Savefile(odf.FileName, InfoSection.Filetype.File_DCF);

                //file.Close();
            }
        }

        
        private void SetCustomView(bool custom_view)
        {
            if (CustomView == custom_view)
                return;
            if (custom_view) 
            {   
                CustomAddSelBtn.Enabled = false;  
                CustomDeleteSelBtn.Enabled = true;
            }
            else
            {
                CustomAddSelBtn.Enabled = true;  
                CustomDeleteSelBtn.Enabled = false;            
            }     
            CustomView = custom_view;            
            if (custom_view == true)
            {      
                listView1.BeginUpdate();
                listView1.Items.Clear();
                foreach(ListViewItem lvi in CustomList)
                {
                    listView1.Items.Add(lvi);
                }
                listView1.EndUpdate();
            }
            else
            { 
                CustomList.Clear();
                foreach(ListViewItem lvi in listView1.Items)
                {
                    CustomList.Add(lvi);
                }
                listView1.Items.Clear();
                UpdateTable();    
            }
        } 
        

        private void NormalCustomChange(bool custom_view)
        {                       
            if (AktivReadMode != TReadMode.READ_STOP)
            {
                ReadAllSelChange(TReadMode.READ_STOP);
                System.Threading.Thread.Sleep(30);
            }              
            NormalViewCheckBox.CheckedChanged -= NormalViewCheckBox_CheckedChanged;
            CustomViewCheckBox.CheckedChanged -= CustomViewCheckBox_CheckedChanged;
            if (custom_view == false)
            {
                NormalViewCheckBox.Checked = true;
                CustomViewCheckBox.Checked = false;
            }
            else
            {
                NormalViewCheckBox.Checked = false;
                CustomViewCheckBox.Checked = true;
            }
            CustomViewCheckBox.CheckedChanged += CustomViewCheckBox_CheckedChanged;    
            NormalViewCheckBox.CheckedChanged += NormalViewCheckBox_CheckedChanged;
            SetCustomView(custom_view);
        }     

        
        private void NormalViewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            NormalCustomChange(false);
        }


        private void CustomViewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            NormalCustomChange(true);
        }


        private void SetReadMode(TReadMode read_mode)
        {
            if (read_mode == TReadMode.READ_STOP)
                {
                AktivReadMode = TReadMode.READ_STOP;
                RefreshTimer.Stop();            
                lco.flushSDOqueue();
                return;
                }
            if (!lco.isopen())
            {
                MessageBox.Show("CAN not open");
                AktivReadMode = TReadMode.READ_STOP;
                return;
            }

            if (numericUpDown_node.Value == 0)
            {
                MessageBox.Show("You cannot read from Node 0, please select a node");
                AktivReadMode = TReadMode.READ_STOP;
                return;
            }

            AktivReadMode = read_mode;

            listView1.Invoke(new MethodInvoker(delegate
            {
                if (read_mode == TReadMode.READ_ALL)
                {
                    foreach (ListViewItem lvi in listView1.Items)
                        InitSdoRead(lvi);
                }    
                else
                {
                    foreach (ListViewItem lvi in listView1.SelectedItems)
                        InitSdoRead(lvi);
                }        
            }));
        }


        private void UpdateReadWriteButtons()
        {
            if (ReadAutoRepeatEnable == true)
            {
                if (ReadAllCheckBox.Checked == true)                            
                    ReadAllCheckBox.Text = "STOP Read all";                
                else                
                    ReadAllCheckBox.Text = "START Read all";                
                if (ReadSelCheckBox.Checked == true)                
                    ReadSelCheckBox.Text = "STOP Read selected";            
                else                
                    ReadSelCheckBox.Text = "START Read selected";                            
            }
            else
            {
                ReadSelCheckBox.Text = "Read selected";
                ReadAllCheckBox.Text = "Read all";            
            }
            if (AktivReadMode == TReadMode.READ_STOP)
                WriteSdoBtn.Enabled = true;
            else    
                WriteSdoBtn.Enabled = false; 
        }
            

        private void ReadAllSelChange(TReadMode read_mode)
        {            
            SetReadMode(read_mode);
            ReadAllCheckBox.CheckedChanged -= ReadAllCheckBox_CheckedChanged;
            ReadSelCheckBox.CheckedChanged -= ReadSelCheckBox_CheckedChanged;            
            if (ReadAutoRepeatEnable == true)
            {
                if (AktivReadMode == TReadMode.READ_SEL)
                {
                    ReadSelCheckBox.Checked = true;
                    ReadAllCheckBox.Checked = false;
                }
                else if (AktivReadMode == TReadMode.READ_ALL)
                {
                    ReadSelCheckBox.Checked = false;
                    ReadAllCheckBox.Checked = true;
                }
                else
                {
                    ReadSelCheckBox.Checked = false;
                    ReadAllCheckBox.Checked = false;
                }                
            }  
            else
            {
                ReadSelCheckBox.Checked = false;
                ReadAllCheckBox.Checked = false;
            }    
            ReadAllCheckBox.CheckedChanged += ReadAllCheckBox_CheckedChanged;
            ReadSelCheckBox.CheckedChanged += ReadSelCheckBox_CheckedChanged;
            UpdateReadWriteButtons();                        
        }
        
        
        private void ReadSelCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ReadSelCheckBox.Checked == true)
                ReadAllSelChange(TReadMode.READ_SEL);
            else
                ReadAllSelChange(TReadMode.READ_STOP);
        }


        private void ReadAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ReadAllCheckBox.Checked == true)
                ReadAllSelChange(TReadMode.READ_ALL);
            else
                ReadAllSelChange(TReadMode.READ_STOP);           
        }
        

        private void StopFlushBtn_Click(object sender, EventArgs e)
        {
            ReadAllSelChange(TReadMode.READ_STOP);
        }        


        private void AutoRepeatCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ReadAutoRepeatEnable = AutoRepeatCheckBox.Checked;
            ReadAllSelChange(TReadMode.READ_STOP);            
        }

        
        private void CustomAddSelBtn_Click(object sender, EventArgs e)
        {
            if (CustomView == true)
                return;
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                CustomList.Add(lvi);
            }            
        }


        private void CustomDeleteSelBtn_Click(object sender, EventArgs e)
        {
            if (CustomView == false)
                return;
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                listView1.Items.Remove(lvi);
            }
        }


        private void checkBox_useronly_CheckedChanged(object sender, EventArgs e)
        {
            if (CustomView == false)
            {
                if (AktivReadMode != TReadMode.READ_STOP)
                {
                    ReadAllSelChange(TReadMode.READ_STOP);
                    System.Threading.Thread.Sleep(30);
                }
            UpdateTable();
            } 
        }
        
        
        private void comboBoxtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CustomView == false)
            {
                if (AktivReadMode != TReadMode.READ_STOP)
                {
                    ReadAllSelChange(TReadMode.READ_STOP);
                    System.Threading.Thread.Sleep(30);
                }
            UpdateTable();
            }    
        }        
        
        
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            WriteSelectedSDOValue();
        }

        private void WriteSdoBtn_Click(object sender, EventArgs e)
        {
            WriteSelectedSDOValue();
        }
    }
}
