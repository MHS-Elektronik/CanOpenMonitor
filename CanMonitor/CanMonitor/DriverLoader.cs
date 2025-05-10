using libCanopenSimple;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace CanMonitor
{
    public class DriverLoader
    {
        public event EventHandler<EventArgs> DeviceListChanged;

        List<string> drivers = new List<string>();

        public List<driverport> _driverport;

        public BUSSPEED rate = BUSSPEED.BUS_500Kbit;

        public delegate void PortChangedEvent(object sender, EventArgs e);
        public event PortChangedEvent portchangedevent;


        public DriverLoader()
        {
        }


        public void finddrivers()
        {
            Program.InfoWin.AddLine("Searching for drivers...");
            string[] founddrivers = Directory.GetFiles("drivers\\", "*.dll");

            foreach (string driver in founddrivers)
            {
                Program.InfoWin.AddLine(string.Format("Found driver {0}", driver));
                drivers.Add(driver.Substring(0, driver.Length - 4));
            }
        }


        public void enumerateports()
        {
            _driverport = new List<driverport>();

            {

                foreach (string s in drivers)
                {
                    try
                    {
                        Program.lco.enumerate(s);
                    }
                    catch (Exception e)
                    {
                        Program.InfoWin.AddLine(string.Format("Driver: {0} Enumerate error: {1}", s, e.ToString()));
                    }
                }

                foreach (KeyValuePair<string, List<string>> kvp in Program.lco.ports)
                {
                    List<string> ps = kvp.Value;
                    foreach (string s in ps)
                    {
                        driverport dp = new driverport();
                        dp.port = s;
                        dp.driver = kvp.Key;
                        _driverport.Add(dp);
                    }
                }
            }

            portchangedevent?.Invoke(this, new EventArgs());
        }
        

        private void Cpm_DeviceListChanged(object sender, EventArgs e)
        {
            enumerateports();
            DeviceListChanged?.Invoke(this, new EventArgs());
           
        }
        

        public bool Open(driverport dp,BUSSPEED rate)
        {
            Program.lco.close();
            this.rate = rate;
            string port = dp.port;
            return Program.lco.open(port, (BUSSPEED)rate, dp.driver);
  
        }
        

        public void Close()
        {
            Program.lco.close();
        }

        public string BusspeedToStr(BUSSPEED rate)
        {
            switch (rate)
            {
                case BUSSPEED.BUS_10Kbit: return ("10K");
                case BUSSPEED.BUS_20Kbit: return ("20K");
                case BUSSPEED.BUS_50Kbit: return ("50K");
                case BUSSPEED.BUS_100Kbit: return ("100K");
                case BUSSPEED.BUS_125Kbit: return ("125K");
                case BUSSPEED.BUS_250Kbit: return ("250K");
                case BUSSPEED.BUS_500Kbit: return ("500K");
                case BUSSPEED.BUS_1Mbit: return ("1M");
                case BUSSPEED.BUS_250Kbit_FD_1Mbit: return ("250K [FD:1M]");                    
                case BUSSPEED.BUS_250Kbit_FD_2Mbit: return ("250K [FD:2M]");                    
                case BUSSPEED.BUS_500Kbit_FD_1Mbit: return ("500K [FD:1M]");                
                case BUSSPEED.BUS_500Kbit_FD_2Mbit: return ("500K [FD:2M]");                    
                case BUSSPEED.BUS_500Kbit_FD_4Mbit: return ("500K [FD:4M]");                        
                case BUSSPEED.BUS_1Mbit_FD_2Mbit: return ("1M [FD:2M]");                    
                case BUSSPEED.BUS_1Mbit_FD_4Mbit: return ("1M [FD:4M]");                    
                case BUSSPEED.BUS_1Mbit_FD_5Mbit: return ("1M [FD:5M]");                                                
                default: return ("");
            }
        }


        public BUSSPEED StrToBusspeed(string rate_str)
        {
            if (rate_str == "10K")
                return (BUSSPEED.BUS_10Kbit);
            else if (rate_str == "20K")
                return (BUSSPEED.BUS_20Kbit);
            else if (rate_str == "50K")
                return (BUSSPEED.BUS_50Kbit);
            else if (rate_str == "100K")
                return (BUSSPEED.BUS_100Kbit);
            else if (rate_str == "125K")
                return (BUSSPEED.BUS_125Kbit);
            else if (rate_str == "250K")
                return (BUSSPEED.BUS_250Kbit);
            else if (rate_str == "500K")
                return (BUSSPEED.BUS_500Kbit);
            else if (rate_str == "1M")                       
                return (BUSSPEED.BUS_1Mbit);
            else if (rate_str == "250K [FD:1M]")                
                return (BUSSPEED.BUS_250Kbit_FD_1Mbit);                  
            else if (rate_str == "250K [FD:2M]")
                return (BUSSPEED.BUS_250Kbit_FD_2Mbit);                  
            else if (rate_str == "500K [FD:1M]")
                return (BUSSPEED.BUS_500Kbit_FD_1Mbit);                  
            else if (rate_str == "500K [FD:2M]")
                return (BUSSPEED.BUS_500Kbit_FD_2Mbit);                  
            else if (rate_str == "500K [FD:4M]")
                return (BUSSPEED.BUS_500Kbit_FD_4Mbit);                  
            else if (rate_str == "1M [FD:2M]")
                return (BUSSPEED.BUS_1Mbit_FD_2Mbit);                    
            else if (rate_str == "1M [FD:4M]")
                return (BUSSPEED.BUS_1Mbit_FD_4Mbit);                    
            else if (rate_str == "1M [FD:5M]")
                return (BUSSPEED.BUS_1Mbit_FD_5Mbit);  
            else                      
                return (BUSSPEED.BUS_125Kbit);                                                                                               
        }

    }
}
