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

namespace libCanopenSimple
{
    /// <summary>
    /// The SDO class encapusulates an SDO transfer and its associated data
    /// </summary>
    public class SDO
    {

        /// <summary>
        /// Direction of the SDO transfer
        /// </summary>
        public enum direction
        {
            SDO_READ = 0,
            SDO_WRITE = 1,
        }

        /// <summary>
        /// Possible SDO states used by simple handshake statemachine
        /// </summary>
        public enum SDO_STATE
        {
            SDO_INIT,
            SDO_SENT,
            SDO_HANDSHAKE,
            SDO_FINISHED,
            SDO_ERROR,
        }

        /// <summary>
        /// Expitided data buffer, if transfer is 4 bytes or less, its here
        /// </summary>

        public readonly byte node;

        //FIX me i was using all these from outside this class
        //should see if that is really needed or if better accessors are required
        //may be readonly access etc.
        public byte[] databuffer = null;
        public SDO_STATE state;
        public UInt16 index;
        public byte subindex;
        public UInt32 expitideddata;
        public bool expitided = false;

        public int returnlen = 0;

        static List<SDO> activeSDO = new List<SDO>();

        private Action<SDO> completedcallback;
      
      
        private direction dir;
      
        private UInt32 totaldata;
        private libCanopenSimple can;
        private bool lasttoggle = false;
        private DateTime timeout;
        private ManualResetEvent finishedevent;
        private debuglevel dbglevel;


        /// <summary>
        /// Construct a new SDO object
        /// </summary>
        /// <param name="can">a libCanopenSimple object that will give access to the hardware</param>
        /// <param name="node">The note to talk to (UInt16)</param>
        /// <param name="index">The index in the object dictionary to access</param>
        /// <param name="subindex">The subindex in the object dictionary to access</param>
        /// <param name="dir">Direction of transfer</param>
        /// <param name="completedcallback">Optional, completed callback (or null if not required)</param>
        /// <param name="databuffer">A byte array of data to be transfered to or from if more than 4 bytes</param>
        public SDO(libCanopenSimple can, byte node, UInt16 index, byte subindex, direction dir, Action<SDO> completedcallback, byte[] databuffer)
        {
            this.can = can;
            this.index = index;
            this.subindex = subindex;
            this.node = node;
            this.dir = dir;
            this.completedcallback = completedcallback;
            this.databuffer = databuffer;

            finishedevent = new ManualResetEvent(false);
            state = SDO_STATE.SDO_INIT;
            dbglevel = can.dbglevel;

        }

        /// <summary>
        /// Add this SDO object to the active list
        /// </summary>
        public void sendSDO()
        {
            lock (activeSDO)
                activeSDO.Add(this);
        }

        public static bool isEmpty()
        {
            return activeSDO.Count == 0;
        }

        /// <summary>
        /// Has the SDO transfer finished?
        /// </summary>
        /// <returns>True if the SDO has finished and fired its finished event</returns>
        public bool WaitOne()
        {
            return finishedevent.WaitOne();
        }


        /// <summary>
        /// SDO pump, call this often
        /// </summary>
        public static void kick_SDO()
        {
            List<SDO> tokill = new List<SDO>();

            lock (activeSDO)
            {
                foreach (SDO s in activeSDO)
                {
                    s.kick_SDOp();
                    if (s.state == SDO_STATE.SDO_FINISHED || s.state == SDO_STATE.SDO_ERROR)
                    {
                        tokill.Add(s);
                    }
                }
            }

            foreach (SDO s in tokill)
            {
                lock (activeSDO)
                    activeSDO.Remove(s);
                s.SDOFinish();
            }
        }

        /// <summary>
        /// State machine for a specific SDO instance
        /// </summary>
        private void kick_SDOp()
        {

            if (state != SDO_STATE.SDO_INIT && DateTime.Now > timeout)
            {
                state = SDO_STATE.SDO_ERROR;

                Console.WriteLine("SDO Timeout Error on {0:x4}/{1:x2} {2:x8}", this.index, this.subindex, expitideddata);

                if (completedcallback != null)
                    completedcallback(this);

                return;
            }

            if (state == SDO_STATE.SDO_INIT)
            {
                timeout = DateTime.Now + new TimeSpan(0, 0, 1);
                state = SDO_STATE.SDO_SENT;

                if (dir == direction.SDO_READ)
                {
                    byte cmd = 0x40;
                    byte[] payload = new byte[4];
                    sendpacket(cmd, payload);
                }

                if (dir == direction.SDO_WRITE)
                {
                    bool wpsent = false;
                    byte cmd = 0;

                    expitided = true;

                    switch (databuffer.Length)
                    {
                        case 1:
                            cmd = 0x2f;
                            break;
                        case 2:
                            cmd = 0x2b;
                            break;
                        case 3:
                            cmd = 0x27;
                            break;
                        case 4:
                            cmd = 0x23;
                            break;
                        default:
                            //Bigger than 4 bytes we use segmented transfer
                            cmd = 0x21;
                            expitided = false;

                            byte[] payload = new byte[4];
                            payload[0] = (byte)databuffer.Length;
                            payload[1] = (byte)(databuffer.Length >> 8);
                            payload[2] = (byte)(databuffer.Length >> 16);
                            payload[3] = (byte)(databuffer.Length >> 24);

                            expitideddata = (UInt32)databuffer.Length;
                            totaldata = 0;

                            wpsent = true;
                            sendpacket(cmd, payload);
                            break;

                    }

                    if (wpsent == false)
                        sendpacket(cmd, databuffer);

                }
            }
        }

        /// <summary>
        /// Send a SDO packet, with command and payload, should be only called from SDO state machine
        /// </summary>
        /// <param name="cmd">SDO command byte</param>
        /// <param name="payload">Data payload to send</param>
        private void sendpacket(byte cmd, byte[] payload)
        {

            canpacket p = new canpacket();
            p.cob = (UInt16)(0x600 + node);
            p.len = 8;
            p.dataByte[0] = cmd;
            p.dataByte[1] = (byte)index;
            p.dataByte[2] = (byte)(index >> 8);
            p.dataByte[3] = subindex;

            int sendlength = 4;

            if (payload.Length < 4)
                sendlength = payload.Length;

            for (int x = 0; x < sendlength; x++)
            {
                p.dataByte[4 + x] = payload[x];
            }

            if (dbglevel == debuglevel.DEBUG_ALL)
                Console.WriteLine(String.Format("Sending a new SDO packet: {0}", p.ToString()));

            if(can.isopen())
                can.SendPacket(p);
        }

        /// <summary>
        /// Segmented transfer update function, should be only called from SDO state machine
        /// </summary>
        /// <param name="cmd">SDO command byte</param>
        /// <param name="payload">Data payload</param>
        private void sendpacketsegment(byte cmd, byte[] payload)
        {
            canpacket p = new canpacket();
            p.cob = (UInt16)(0x600 + node);
            p.len = 8;
            p.dataByte[0] = cmd;

            for (int x = 0; x < payload.Length; x++)
            {
                p.dataByte[1 + x] = payload[x];
            }

            if (dbglevel == debuglevel.DEBUG_ALL)
                Console.WriteLine(String.Format("Sending a new segmented SDO packet: {0}", p.ToString()));

            can.SendPacket(p);
        }

        /// <summary>
        /// Force finish the SDO and trigger its finished event
        /// </summary>
        public void SDOFinish()
        {
            can.SDOcallbacks.Remove((UInt16)(this.node + 0x580));
            finishedevent.Set();
        }

        /// <summary>
        /// SDO Instance processor, process current SDO reply and decide what to do next
        /// </summary>
        /// <param name="cp">SDO Canpacket to process</param>
        /// <returns></returns>
        public bool SDOProcess(canpacket cp)
        {

            int SCS = cp.dataByte[0] >> 5; //7-5

            int n = (0x03 & (cp.dataByte[0] >> 2)); //3-2 data size for normal packets

            returnlen = 8*(4-n);

            int e = (0x01 & (cp.dataByte[0] >> 1)); // expidited flag
            int s = (cp.dataByte[0] & 0x01); // data size set flag

            int sn = (0x07 & (cp.dataByte[0] >> 1)); //3-1 data size for segment packets
            int t = (0x01 & (cp.dataByte[0] >> 4));  //toggle flag

            int c = 0x01 & cp.dataByte[0]; //More segments to upload?

            //ERROR abort
            if (SCS == 0x04)
            {

                expitideddata = (UInt32)(cp.dataByte[4] + (cp.dataByte[5] << 8) + (cp.dataByte[6] << 16) + (cp.dataByte[7] << 24));
                databuffer = BitConverter.GetBytes(expitideddata);

                state = SDO_STATE.SDO_ERROR;

                Console.WriteLine("SDO Error on {0:x4}/{1:x2} {2:x8}", this.index, this.subindex, expitideddata);

                if (completedcallback != null)
                    completedcallback(this);

                return true;
            }

            //Write complete
            if (SCS == 0x03)
            {
                UInt16 index = (UInt16)(cp.dataByte[1] + (cp.dataByte[2] << 8));
                byte sub = cp.dataByte[3];

                int node = cp.cob - 0x580;
                foreach (SDO sdo in activeSDO)
                {
                    if (sdo.node == node)
                    {
                        if ((index == sdo.index && sub == sdo.subindex)) //if segments break its here
                        {
                            if (expitided == false)
                            {
                                state = SDO_STATE.SDO_HANDSHAKE;
                                requestNextSegment(false);
                                return false;
                            }
                        }

                    }
                }

                state = SDO_STATE.SDO_FINISHED;

                if (completedcallback != null)
                    completedcallback(this);

                return true;
            }

            //Write segment complete
            if (SCS == 0x01)
            {
                if (totaldata < expitideddata)
                {
                    lasttoggle = !lasttoggle;
                    requestNextSegment(lasttoggle);
                }
                else
                {
                    state = SDO_STATE.SDO_FINISHED;
                    if (completedcallback != null)
                        completedcallback(this);
                }

            }

            //if expedited just handle the data
            if (SCS == 0x02 && e == 1)
            {
                //Expidited and length are set so its a regular short transfer

                expitideddata = (UInt32)(cp.dataByte[4] + (cp.dataByte[5] << 8) + (cp.dataByte[6] << 16) + (cp.dataByte[7] << 24));
                databuffer = BitConverter.GetBytes(expitideddata);

                state = SDO_STATE.SDO_FINISHED;

                if (completedcallback != null)
                {
                    try
                    {
                        completedcallback(this);
                    }
                    catch { }

                }
                return true;
            }

            if (SCS == 0x02)
            {
                UInt32 count = (UInt32)(cp.dataByte[4] + (cp.dataByte[5] << 8) + (cp.dataByte[6] << 16) + (cp.dataByte[7] << 24));

                Console.WriteLine("RX Segmented transfer start length is {0}", count);
                expitideddata = count;
                databuffer = new byte[expitideddata];
                totaldata = 0;
                //Request next segment

                requestNextSegment(false); //toggle off on first request
                return false;

            }

            //segmented transfer
            UInt32 scount = (UInt32)(7 - sn);

            //Segments toggle on
            if (SCS == 0x00)
            {

               // Console.WriteLine("RX Segmented transfer update length is {0} -- {1}", scount, totaldata);

                for (int x = 0; x < scount; x++)
                {
                    if ((totaldata + x) < databuffer.Length)
                        databuffer[totaldata + x] = cp.dataByte[1 + x];
                }

                totaldata += 7;

                if ((totaldata < expitideddata) && c == 0)
                {
                    lasttoggle = !lasttoggle;
                    requestNextSegment(lasttoggle);
                }
                else
                {
                    state = SDO_STATE.SDO_FINISHED;
                    if (completedcallback != null)
                        completedcallback(this);
                }

            }

            return false;
        }

        /// <summary>
        /// Request the next segment in a segmented transfet
        /// </summary>
        /// <param name="toggle">Segmented transfer toggle flag, should alternate for each successive transfer</param>
        private void requestNextSegment(bool toggle)
        {

            timeout = DateTime.Now + new TimeSpan(0, 0, 1);

            if (dir == direction.SDO_READ)
            {
                byte cmd = 0x60;
                if (toggle)
                    cmd |= 0x70;

                sendpacket(cmd, new byte[4]);
            }
            else
            {
                byte cmd = 0x00;
                if (toggle)
                    cmd |= 0x10;

                int bytecount = (int)(databuffer.Length - totaldata); //11 - 7
                byte[] nextdata;
                if (bytecount >= 7)
                {
                    bytecount = 7;
                }


                nextdata = new byte[bytecount];

                for (int x = 0; x < bytecount; x++)
                {
                    if (databuffer.Length > (totaldata + x))
                        nextdata[x] = databuffer[totaldata + x];

                }

                if (totaldata + 7 >= databuffer.Length)
                {
                    cmd |= 0x01; //END of packet sequence
                }

                if (bytecount != 7)
                {
                    int n = 7 - bytecount;
                    n = n << 1;
                    cmd |= (byte)n;
                }

                sendpacketsegment(cmd, nextdata);
                totaldata += (uint)bytecount;
            }

        }

    }
}
