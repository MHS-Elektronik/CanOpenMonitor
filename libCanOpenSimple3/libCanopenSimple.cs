/*
    This file is part of libCanopenSimple.
    libCanopenSimple is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    libCanopenSimple is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with libCanopenSimple.  If not, see <http://www.gnu.org/licenses/>.
 
    Copyright(c) 2017 Robin Cornelius <robin.cornelius@gmail.com>
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace libCanopenSimple
{
    public enum BUSSPEED
    {
        BUS_10Kbit = 0,
        BUS_20Kbit,
        BUS_50Kbit,
        BUS_100Kbit,
        BUS_125Kbit,
        BUS_250Kbit,
        BUS_500Kbit,
        BUS_800Kbit,
        BUS_1Mbit,
        BUS_250Kbit_FD_1Mbit,
        BUS_250Kbit_FD_2Mbit,
        BUS_500Kbit_FD_1Mbit,
        BUS_500Kbit_FD_2Mbit,
        BUS_500Kbit_FD_4Mbit,
        BUS_1Mbit_FD_2Mbit,
        BUS_1Mbit_FD_4Mbit,
        BUS_1Mbit_FD_5Mbit
    }

    public enum debuglevel
    {
        DEBUG_ALL,
        DEBUG_NONE
    }

    /// <summary>
    /// C# representation of a CanPacket, containing the COB the length and the data. RTR is not supported
    /// as its prettly much not used on CanOpen, but this could be added later if necessary
    /// </summary>
    public class canpacket
    {
        public UInt16 cob;
        public byte len;        
        public UInt64[] data;
        public bool fd;
        public bool brs;
        public bool eff;
        public TDataByte dataByte;
        public bool bridge = false;

        public class TDataByte
        {
            private canpacket parent;
     
            public byte this[int index]
            {
                get
                {
                    int idx;
                    idx = index >> 3;
                    if (idx >= parent.data.Length)
                      return(0); 
                    index &= 0x7;
                    return (Convert.ToByte((parent.data[idx] >> (index * 8)) & 0x00000000000000FF));
                }
                set
                {
                    int idx;
                    UInt64 mask;
                    idx = index >> 3;
                    if (idx < parent.data.Length)
                    {
                        index = (index & 0x7) * 8;
                        mask = ~((UInt64)0x00000000000000FF << index);
                        parent.data[idx] &= mask;
                        parent.data[idx] |= ((UInt64)value << index);
                    }
                }
            } 
            
            public TDataByte(canpacket parent)
            {
                this.parent = parent;
            }
        }


        public canpacket()
        {
            data = new UInt64[8];
            dataByte = new TDataByte(this);
            eff = false;        
            fd = false; 
            brs = false;
            cob = 0;
            len = 0;
            bridge = false;
        }  
        
        
        // Construct a CAN message
        public canpacket(UInt16 cob, byte[] data, bool eff, bool fd, bool brs)
        {
            int i;
        
            this.data = new UInt64[8];
            this.dataByte = new TDataByte(this);
            this.cob = cob;
            this.eff = eff;
            this.fd = fd;
            this.brs = brs;
        
            len = (byte)data.Length;
            for (i = 0; i < len; i++)
                dataByte[i] = data[i];
        }
        
            
        /// <summary>
        /// Construct C# Canpacket from a CanFestival message
        /// </summary>
        /// <param name="msg">A CanFestival message struct</param>
        public canpacket(DriverInstance.canfd_frame raw_msg, bool bridge = false)
        {
            data = new UInt64[8];
            dataByte = new TDataByte(this);
            cob = (ushort)(raw_msg.can_id & 0x7FF);
            if ((raw_msg.can_id & 0x80000000) > 0) 
              eff = true;
            if ((raw_msg.flags & 0x04) > 0)
              fd = true;
            if ((raw_msg.flags & 0x01) > 0)
              brs = true;
            len = raw_msg.len;
            this.bridge = bridge;
            if (len > 0)
            {
                switch ((len - 1) >> 3)
                {
                    case 7 :
                        data[7] = raw_msg.data7;
                        data[6] = raw_msg.data6;
                        data[5] = raw_msg.data5;
                        data[4] = raw_msg.data4;
                        data[3] = raw_msg.data3;
                        data[2] = raw_msg.data2;
                        data[1] = raw_msg.data1;
                        data[0] = raw_msg.data0;
                        break;
                    case 6 :    
                        data[6] = raw_msg.data6;
                        data[5] = raw_msg.data5;
                        data[4] = raw_msg.data4;
                        data[3] = raw_msg.data3;
                        data[2] = raw_msg.data2;
                        data[1] = raw_msg.data1;
                        data[0] = raw_msg.data0;
                        break;                                           
                    case 5 :
                        data[5] = raw_msg.data5;
                        data[4] = raw_msg.data4;
                        data[3] = raw_msg.data3;
                        data[2] = raw_msg.data2;
                        data[1] = raw_msg.data1;
                        data[0] = raw_msg.data0;
                        break;                     
                    case 4 :
                        data[4] = raw_msg.data4;
                        data[3] = raw_msg.data3;
                        data[2] = raw_msg.data2;
                        data[1] = raw_msg.data1;
                        data[0] = raw_msg.data0;
                        break;                     
                    case 3 :
                        data[3] = raw_msg.data3;
                        data[2] = raw_msg.data2;
                        data[1] = raw_msg.data1;
                        data[0] = raw_msg.data0;
                        break;                     
                    case 2 :
                        data[2] = raw_msg.data2;
                        data[1] = raw_msg.data1;
                        data[0] = raw_msg.data0;
                        break;                     
                    case 1 :
                        data[1] = raw_msg.data1;
                        data[0] = raw_msg.data0;
                        break;                     
                    case 0 :
                        data[0] = raw_msg.data0;
                        break;                     
                }          
            }
        }

        /// <summary>
        /// Convert to a CanFestival message
        /// </summary>
        /// <returns>CanFestival message</returns>
        public DriverInstance.canfd_frame ToMsg()
        {
            DriverInstance.canfd_frame raw_msg = new DriverInstance.canfd_frame();
            
            raw_msg.can_id = ((ushort)(cob & 0x7FF));
            if (eff)            
                raw_msg.can_id |= 0x80000000;  
            raw_msg.flags = 0;
            if (fd)
                raw_msg.flags |= 0x04;              
            if (brs)  
                raw_msg.flags |= 0x01;            
            raw_msg.len = len;
            if (len > 0)
            {
                switch (((len - 1) >> 3))
                {
                    case 7 : 
                        raw_msg.data7 = data[7];
                        raw_msg.data6 = data[6];
                        raw_msg.data5 = data[5];
                        raw_msg.data4 = data[4];
                        raw_msg.data3 = data[3];
                        raw_msg.data2 = data[2];
                        raw_msg.data1 = data[1];
                        raw_msg.data0 = data[0];
                        break;
                    case 6 :  
                        raw_msg.data6 = data[6];
                        raw_msg.data5 = data[5];
                        raw_msg.data4 = data[4];
                        raw_msg.data3 = data[3];
                        raw_msg.data2 = data[2];
                        raw_msg.data1 = data[1];
                        raw_msg.data0 = data[0];
                        break;                        
                    case 5 :
                        raw_msg.data5 = data[5];
                        raw_msg.data4 = data[4];
                        raw_msg.data3 = data[3];
                        raw_msg.data2 = data[2];
                        raw_msg.data1 = data[1];
                        raw_msg.data0 = data[0];
                        break;
                    case 4 : 
                        raw_msg.data4 = data[4];
                        raw_msg.data3 = data[3];
                        raw_msg.data2 = data[2];
                        raw_msg.data1 = data[1];
                        raw_msg.data0 = data[0];
                        break;
                    case 3 :
                        raw_msg.data3 = data[3];
                        raw_msg.data2 = data[2];
                        raw_msg.data1 = data[1];
                        raw_msg.data0 = data[0];
                        break; 
                    case 2 : 
                        raw_msg.data2 = data[2];
                        raw_msg.data1 = data[1];
                        raw_msg.data0 = data[0];
                        break;
                    case 1 : 
                        raw_msg.data1 = data[1];
                        raw_msg.data0 = data[0];
                        break;
                    case 0 : 
                        raw_msg.data0 = data[0];
                        break;
                }          
            }            
            return raw_msg;

        }

        /// <summary>
        /// Dump current packet to string
        /// </summary>
        /// <returns>Formatted string of current packet</returns>
        public override string ToString()
        {
            string output = string.Format("{0:x3} {1:x1}", cob, len);

            for (int x = 0; x < len; x++)
            {
                output += string.Format(" {0:x2}", dataByte[x]);
            }
            return output;
        }
        
        
        public string DataToString()
        {
            string output = "";

            if (len > 0)
            {
                int ii = 0;
                for (int i = 0; i < len; i++)
                {
                    if (ii == 8)
                    {
                        ii = 0;
                        output += "\r\n";
                    }
                    output += string.Format("{0:X2} ", dataByte[i]);
                    ii++;
                }
            }
            return(output);
        }
    }


    /// <summary>
    /// A simple can open class providing callbacks for each of the message classes and allowing one to send messages to the bus
    /// Also supports some NMT helper functions and can act as a SDO Client
    /// It is not a CanDevice and does not respond to any message (other than the required SDO client handshakes) and it does not
    /// contain an object dictionary
    /// </summary>
    public class libCanopenSimple
    {
        volatile bool thread_run;
        System.Threading.Thread asyncthread;

        public debuglevel dbglevel = debuglevel.DEBUG_NONE;
       
        DriverInstance driver;

        Dictionary<UInt16, NMTState> nmtstate = new Dictionary<ushort, NMTState>();

        private Queue<SDO> sdo_queue = new Queue<SDO>();

        DriverLoader loader = new DriverLoader();

        public bool echo = true;

        public libCanopenSimple()
        {
            //preallocate all NMT guards
            for (byte x = 0; x < 0x80; x++)
            {
                NMTState nmt = new NMTState();
                nmtstate[x] = nmt;
            }
        }

        #region driverinterface

        /// <summary>
        /// Open the CAN hardware device via the CanFestival driver, NB this is currently a simple version that will
        /// not work with drivers that have more complex bus ids so only supports com port (inc usb serial) devices for the moment
        /// </summary>
        /// <param name="comport">COM PORT number</param>
        /// <param name="speed">CAN Bit rate</param>
        /// <param name="drivername">Driver to use</param>
        public bool open(string comport, BUSSPEED speed, string drivername)
        {

            driver = loader.loaddriver(drivername);
            if (driver.open(string.Format("{0}", comport), speed) == false)
                return false;

            driver.rxmessage += Driver_rxmessage;

            thread_run = true;
            asyncthread = new Thread(new ThreadStart(asyncprocess));
            asyncthread.Name = "CAN Open worker";
            asyncthread.Start();

            if (connectionevent != null) connectionevent(this, new ConnectionChangedEventArgs(true));

            return true;

        }

        public Dictionary<string, List<string>> ports = new Dictionary<string, List<string>>();
        public Dictionary<string, DriverInstance> drivers = new Dictionary<string, DriverInstance>();

        public void enumerate(string drivername)
        {

            if (!ports.ContainsKey(drivername))
                ports.Add(drivername, new List<string>());


            //Keep a cache of open drivers or else if we try to close 
            //on a hot plug event we have lost the handle and we will never close the port
            if (!drivers.ContainsKey(drivername))
            {
                driver = loader.loaddriver(drivername);
                drivers.Add(drivername, driver);
            }

            DriverInstance di = drivers[drivername];
            
            di.enumerate();
            ports[drivername] = DriverInstance.ports;

        }

        /// <summary>
        /// Is the driver open
        /// </summary>
        /// <returns>true = driver open and ready to use</returns>
        public bool isopen()
        {
            if (driver == null)
                return false;

            return driver.isOpen();
        }

        /// <summary>
        /// Send a Can packet on the bus
        /// </summary>
        /// <param name="p"></param>
        public void SendPacket(canpacket p, bool bridge=false)
        {
            DriverInstance.canfd_frame msg = p.ToMsg();

            driver.cansend(msg);

            if (echo == true)
            {
                Driver_rxmessage(msg,bridge);
            }
        }

        /// <summary>
        /// Recieved message callback handler
        /// </summary>
        /// <param name="msg">CanOpen message recieved from the bus</param>
        private void Driver_rxmessage(DriverInstance.canfd_frame msg,bool bridge=false)
        {
            packetqueue.Enqueue(new canpacket(msg, bridge));
        }


        /// <summary>
        /// Close the CanOpen CanFestival driver
        /// </summary>
        public void close()
        {
            if (asyncthread != null)
                {
                thread_run = false;
                asyncthread.Join();
                }

            if (driver == null)
                return;

            driver.close();

            if (connectionevent != null) connectionevent(this, new ConnectionChangedEventArgs(false));
        }

        #endregion

        //Dictionary<UInt16, Action<byte[]>> PDOcallbacks = new Dictionary<ushort, Action<byte[]>>();
        Dictionary<UInt16, Action<canpacket>> PDOcallbacks = new Dictionary<ushort, Action<canpacket>>();
        public Dictionary<UInt16, SDO> SDOcallbacks = new Dictionary<ushort, SDO>();
        
        ConcurrentQueue<canpacket> packetqueue = new ConcurrentQueue<canpacket>();

        public delegate void ConnectionEvent(object sender, EventArgs e);
        public event ConnectionEvent connectionevent;

        public delegate void PacketEvent(canpacket p, DateTime dt);
        public event PacketEvent packetevent;

        public delegate void SDOEvent(canpacket p, DateTime dt);
        public event SDOEvent sdoevent;

        public delegate void NMTEvent(canpacket p, DateTime dt);
        public event NMTEvent nmtevent;

        public delegate void NMTECEvent(canpacket p, DateTime dt);
        public event NMTECEvent nmtecevent;

        public delegate void PDOEvent(canpacket[] p,DateTime dt);
        public event PDOEvent pdoevent;

        public delegate void EMCYEvent(canpacket p, DateTime dt);
        public event EMCYEvent emcyevent;

        public delegate void LSSEvent(canpacket p, DateTime dt);
        public event LSSEvent lssevent;

        public delegate void TIMEEvent(canpacket p, DateTime dt);
        public event TIMEEvent timeevent;

        public delegate void SYNCEvent(canpacket p, DateTime dt);
        public event SYNCEvent syncevent;

        /// <summary>
        /// Register a parser handler for a PDO, if a PDO is recieved with a matching COB this function will be called
        /// so that additional messages can be added for bus decoding and monitoring
        /// </summary>
        /// <param name="cob">COB to match</param>
        /// <param name="handler">function(byte[] data]{} function to invoke</param>
        /*public void registerPDOhandler(UInt16 cob, Action<byte[]> handler)<*>
        {
            PDOcallbacks[cob] = handler;
        }*/

        /// <summary>
        /// Main process loop, used to get latest packets from buffer and also keep the SDO events pumped
        /// When packets are recieved they will be matched to any approprate callback handlers for this specific COB type
        /// and that handler invoked.
        /// </summary>
        void asyncprocess()
        {
            while (thread_run)
            {
                canpacket cp;
                List<canpacket> pdos = new List<canpacket>();

                while (thread_run && packetqueue.IsEmpty && pdos.Count==0 && sdo_queue.Count==0 && SDO.isEmpty())
                {
                    System.Threading.Thread.Sleep(0);
                }

                while (packetqueue.TryDequeue(out cp))
                {

                    if (cp.bridge == false)
                    {
                        if(packetevent!=null)
                            packetevent(cp, DateTime.Now);
                    }

                    //PDO 0x180 -- 0x57F
                    if (cp.cob >= 0x180 && cp.cob <= 0x57F)
                    {

                        if (PDOcallbacks.ContainsKey(cp.cob))
                            PDOcallbacks[cp.cob](cp);

                        pdos.Add(cp);
                    }

                    //SDO replies 0x601-0x67F
                    if (cp.cob >= 0x580 && cp.cob < 0x600)
                    {
                        if (cp.len != 8)
                            return;

                        lock(sdo_queue)
                        {
                            if (SDOcallbacks.ContainsKey(cp.cob))
                            {
                                if (SDOcallbacks[cp.cob].SDOProcess(cp))
                                {
                                    SDOcallbacks.Remove(cp.cob);
                                }
                            }
                            if (sdoevent != null)
                                sdoevent(cp, DateTime.Now);
                        }
                    }

                    if (cp.cob >= 0x600 && cp.cob < 0x680)
                    {
                        if (sdoevent != null)
                            sdoevent(cp,DateTime.Now);
                    }

                    //NMT
                    if (cp.cob > 0x700 && cp.cob <= 0x77f)
                    {
                        byte node = (byte)(cp.cob & 0x07F);

                        nmtstate[node].changestate((NMTState.e_NMTState)cp.dataByte[0]);
                        nmtstate[node].lastping = DateTime.Now;

                        if (nmtecevent != null)
                            nmtecevent(cp, DateTime.Now);
                    }

                    if (cp.cob == 000)
                    {

                        if (nmtevent != null)
                            nmtevent(cp, DateTime.Now);
                    }
                    if (cp.cob == 0x80)
                    {
                        if (syncevent != null)
                            syncevent(cp, DateTime.Now);
                    }

                    if (cp.cob > 0x080 && cp.cob <= 0xFF)
                    {
                        if (emcyevent != null)
                        {
                            emcyevent(cp, DateTime.Now);
                        }
                    }

                    if (cp.cob == 0x100)
                    {
                        if (timeevent != null)
                            timeevent(cp, DateTime.Now);
                    }

                    if (cp.cob > 0x7E4 && cp.cob <= 0x7E5)
                    {
                        if (lssevent != null)
                            lssevent(cp, DateTime.Now);
                    }
                }

                if (pdos.Count > 0)
                {
                    if (pdoevent != null)
                        pdoevent(pdos.ToArray(),DateTime.Now);
                }

                SDO.kick_SDO();

                lock(sdo_queue)
                {
                    if (sdo_queue.Count > 0)
                    {
                        SDO sdoobj = sdo_queue.Peek();

                        if (!SDOcallbacks.ContainsKey((UInt16)(sdoobj.node + 0x580)))
                        {
                            sdoobj = sdo_queue.Dequeue();
                            SDOcallbacks.Add((UInt16)(sdoobj.node + 0x580), sdoobj);
                            sdoobj.sendSDO();
                        }
                    }
                }
            }
        }


        #region SDOHelpers

        /// <summary>
        /// Write to a node via SDO
        /// </summary>
        /// <param name="node">Node ID</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="udata">UInt32 data to send</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains error/status codes</returns>
        public SDO SDOwrite(byte node, UInt16 index, byte subindex, UInt32 udata, Action<SDO> completedcallback)
        {
            byte[] bytes = BitConverter.GetBytes(udata);
            return SDOwrite(node, index, subindex, bytes, completedcallback);
        }


        /// <summary>
        /// Write to a node via SDO
        /// </summary>
        /// <param name="node">Node ID</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="udata">Int64 data to send</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains error/status codes</returns>
        public SDO SDOwrite(byte node, UInt16 index, byte subindex, Int64 udata, Action<SDO> completedcallback)
        {
            byte[] bytes = BitConverter.GetBytes(udata);
            return SDOwrite(node, index, subindex, bytes, completedcallback);
        }

        /// <summary>
        /// Write to a node via SDO
        /// </summary>
        /// <param name="node">Node ID</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="udata">UInt64 data to send</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains error/status codes</returns>
        public SDO SDOwrite(byte node, UInt16 index, byte subindex, UInt64 udata, Action<SDO> completedcallback)
        {
            byte[] bytes = BitConverter.GetBytes(udata);
            return SDOwrite(node, index, subindex, bytes, completedcallback);
        }

        /// <summary>
        /// Write to a node via SDO
        /// </summary>
        /// <param name="node">Node ID</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="udata">Int32 data to send</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains error/status codes</returns>
        public SDO SDOwrite(byte node, UInt16 index, byte subindex, Int32 udata, Action<SDO> completedcallback)
        {
            byte[] bytes = BitConverter.GetBytes(udata);
            return SDOwrite(node, index, subindex, bytes, completedcallback);
        }

        /// <summary>
        /// Write to a node via SDO
        /// </summary>
        /// <param name="node">Node ID</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="udata">UInt16 data to send</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains error/status codes</returns>
        public SDO SDOwrite(byte node, UInt16 index, byte subindex, Int16 udata, Action<SDO> completedcallback)
        {
            byte[] bytes = BitConverter.GetBytes(udata);
            return SDOwrite(node, index, subindex, bytes, completedcallback);
        }

        /// <summary>
        /// Write to a node via SDO
        /// </summary>
        /// <param name="node">Node ID</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="udata">UInt16 data to send</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains error/status codes</returns>
        public SDO SDOwrite(byte node, UInt16 index, byte subindex, UInt16 udata, Action<SDO> completedcallback)
        {
            byte[] bytes = BitConverter.GetBytes(udata);
            return SDOwrite(node, index, subindex, bytes, completedcallback);
        }

        /// <summary>
        /// Write to a node via SDO
        /// </summary>
        /// <param name="node">Node ID</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="udata">float data to send</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains error/status codes</returns>
        public SDO SDOwrite(byte node, UInt16 index, byte subindex, float ddata, Action<SDO> completedcallback)
        {
            byte[] bytes = BitConverter.GetBytes(ddata);
            return SDOwrite(node, index, subindex, bytes, completedcallback);
        }

        /// <summary>
        /// Write to a node via SDO
        /// </summary>
        /// <param name="node">Node ID</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="udata">a byte of data to send</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains error/status codes</returns>
        public SDO SDOwrite(byte node, UInt16 index, byte subindex, byte udata, Action<SDO> completedcallback)
        {
            byte[] bytes = new byte[1];
            bytes[0] = udata;
            return SDOwrite(node, index, subindex, bytes, completedcallback);
        }

        /// <summary>
        /// Write to a node via SDO
        /// </summary>
        /// <param name="node">Node ID</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="udata">a byte of unsigned data to send</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains error/status codes</returns>
        public SDO SDOwrite(byte node, UInt16 index, byte subindex, sbyte udata, Action<SDO> completedcallback)
        {
            byte[] bytes = new byte[1];
            bytes[0] = (byte)udata;
            return SDOwrite(node, index, subindex, bytes, completedcallback);
        }

        /// <summary>
        /// Write to a node via SDO
        /// </summary>
        /// <param name="node">Node ID</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="udata">byte[] of data (1-8 bytes) to send</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains error/status codes</returns>
        public SDO SDOwrite(byte node, UInt16 index, byte subindex, byte[] data, Action<SDO> completedcallback)
        {

            SDO sdo = new SDO(this, node, index, subindex, SDO.direction.SDO_WRITE, completedcallback, data);
            lock(sdo_queue)
            {
                sdo_queue.Enqueue(sdo);
            }    
            return sdo;
        }

        /// <summary>
        /// Read from a remote node via SDO
        /// </summary>
        /// <param name="node">Node ID to read from</param>
        /// <param name="index">Object Dictionary Index</param>
        /// <param name="subindex">Object Dictionary sub index</param>
        /// <param name="completedcallback">Call back on finished/error event</param>
        /// <returns>SDO class that is used to perform the packet handshake, contains returned data and error/status codes</returns>
        public SDO SDOread(byte node, UInt16 index, byte subindex, Action<SDO> completedcallback)
        {
            SDO sdo = new SDO(this, node, index, subindex, SDO.direction.SDO_READ, completedcallback, null);
            lock(sdo_queue)
            {
                sdo_queue.Enqueue(sdo);
            }    
            return sdo;
        }

        /// <summary>
        /// Get the current length of Enqueued items
        /// </summary>
        /// <returns></returns>
        public int getSDOQueueSize()
        {
            return sdo_queue.Count;
        }

        /// <summary>
        /// Flush the SDO queue
        /// </summary>
        public void flushSDOqueue()
        {
            lock(sdo_queue)
            {
                sdo_queue.Clear();
            }    
        }

        #endregion

        #region NMTHelpers

        public void NMT_start(byte nodeid = 0)
        {
            canpacket p = new canpacket();
            p.cob = 000;
            p.len = 2;
            p.dataByte[0] = 0x01;
            p.dataByte[1] = nodeid;
            SendPacket(p);
        }

        public void NMT_preop(byte nodeid = 0)
        {
            canpacket p = new canpacket();
            p.cob = 000;
            p.len = 2;
            p.dataByte[0] = 0x80;
            p.dataByte[1] = nodeid;
            SendPacket(p);
        }

        public void NMT_stop(byte nodeid = 0)
        {
            canpacket p = new canpacket();
            p.cob = 000;
            p.len = 2;
            p.dataByte[0] = 0x02;
            p.dataByte[1] = nodeid;
            SendPacket(p);
        }

        public void NMT_ResetNode(byte nodeid = 0)
        {
            canpacket p = new canpacket();
            p.cob = 000;
            p.len = 2;
            p.dataByte[0] = 0x81;
            p.dataByte[1] = nodeid;
            SendPacket(p);
        }

        public void NMT_ResetComms(byte nodeid = 0)
        {
            canpacket p = new canpacket();
            p.cob = 000;
            p.len = 2;
            p.dataByte[0] = 0x82;
            p.dataByte[1] = nodeid;
            SendPacket(p);
        }

        public void NMT_SetStateTransitionCallback(byte node, Action<NMTState.e_NMTState> callback)
        {
            nmtstate[node].NMT_boot = callback;
        }

        public bool NMT_isNodeFound(byte node)
        {
            return nmtstate[node].state != NMTState.e_NMTState.INVALID;
        }

        public void NMT_ReseCommunication(byte nodeid = 0)
        {
            canpacket p = new canpacket();
            p.cob = 000;
            p.len = 2;
            p.dataByte[0] = 0x81;
            p.dataByte[1] = nodeid;
            SendPacket(p);
        }

        public bool checkguard(int node, TimeSpan maxspan)
        {
            if (DateTime.Now - nmtstate[(ushort)node].lastping > maxspan)
                return false;

            return true;
        }

        #endregion

        #region PDOhelpers

        public void writePDO(UInt16 cob, byte[] payload)
        {
            canpacket p = new canpacket(cob, payload, false, false, false);
            SendPacket(p);
        }

        #endregion

    }
}
