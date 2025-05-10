/*
This file is part of CanFestival, a library implementing CanOpen Stack. 

can_mhs_win32.cpp Copyright (C) 2025 Klaus Demlehner, MHS-Elektronik GmbH & Co. KG

See COPYING file for copyrights details.

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

// Tiny-CAN CANUSB adapter (http://www.mhs-elektronik.de/)
// driver for CanOpenMonitor

extern "C" {
#include "can_driver.h"
#include "can_drv.h"
#include <stdio.h>
}

//#define CAN_DRIVER_DEBUG

#define MAX_BUF_SIZE 200

#define RX_EVENT     0x00000001

#ifdef CAN_DRIVER_DEBUG
static char CanInitStr[] = {"FdMode=1;CanCallThread=0;AutoStopCan=1;CanRxDFifoSize=16384;CanTxDFifoSize=16384;LogFlags=0xFFFF;LogFile=log.txt"};
#else
static char CanInitStr[] = {"FdMode=1;CanCallThread=0;AutoStopCan=1;CanRxDFifoSize=16384;CanTxDFifoSize=16384"};
#endif
#define CREATE_DEVICE_OPTIONS "HighPollIntervall=0;IdlePollIntervall=0;MainThreadPriority=4;CanRxDFifoSize=16384;CanTxDFifoSize=16384"


struct TTCanDev
  {
  struct TTCanDev *Next;
  char Snr[MAX_BUF_SIZE];
  TMhsEvent *Event;
  uint32_t Index;
  uint32_t BusRun;
  };
    

struct TSpeedValue
  {
  const char *SpeedStr;
  uint16_t SpeedValue;
  uint16_t DSpeedValue;
  };


static const struct TSpeedValue SpeedLookupTab[] = {
    {"10K",           10,    0},
    {"20K",           20,    0},
    {"50K",           50,    0},
    {"100K",          100,   0},
    {"125K",          125,   0},
    {"250K",          250,   0},
    {"500K",          500,   0},
    {"800K",          800,   0},
    {"1M",            1000,  0},
    {"250K [FD:1M]",  250,   1000},
    {"250K [FD:2M]",  250,   2000},
    {"500K [FD:1M]",  500,   1000},
    {"500K [FD:2M]",  500,   2000},
    {"500K [FD:4M]",  500,   4000},
    {"1M [FD:2M]",    1000,  2000},    
    {"1M [FD:4M]",    1000,  4000},
    {"1M [FD:5M]",    1000,  5000},        
    {NULL,            0,     0}};
    

static struct TTCanDev *TCanDevs = NULL;
static int DrvInitFlag;


/***************************************************************/
/*  DLL Main Funktion                                          */
/***************************************************************/
int WINAPI DllMain(HINSTANCE hInstance, DWORD fdwReason, PVOID pvReserved)
{
switch (fdwReason)
  {
  case DLL_PROCESS_ATTACH:
    {
    TCanDevs = NULL;
    DrvInitFlag = 0;
    break;
    }
  case DLL_THREAD_ATTACH: break;
  case DLL_THREAD_DETACH: break;
  case DLL_PROCESS_DETACH: 
    {
    DrvInitFlag = -1;
    CanDownDriver();
    UnloadDriver();
    break;
    }
  }
return(TRUE);
}


extern "C" static int32_t TinyCanApiInit(void)
{
int32_t res;

if (DrvInitFlag > 0)
  return(0);
if ((res = LoadDriver(NULL)) < 0)
  return(res);
if ((res = CanExInitDriver(CanInitStr)) < 0)
  return(res);
DrvInitFlag = 1;
return(0);  
}

extern "C" static const struct TSpeedValue *TranslateBaudRate(const char *str)
{
const struct TSpeedValue*tab;
const char *speed_str;

if (!str)
  return(NULL);
for (tab = &SpeedLookupTab[0]; ((speed_str = tab->SpeedStr)); tab++)
  {
  if (!strcmp(speed_str, str))
    return(tab);
  }
return(NULL);  
}


extern "C" static struct TTCanDev *TinyCanDevCreate(const char *snr)
{
struct TTCanDev *dev, *list;

if (!(dev = (struct TTCanDev *)calloc(1, sizeof(struct TTCanDev))))
  return(NULL);
  
list = TCanDevs;
if (!list)
 TCanDevs = dev;
else
  {
  while (list->Next)
    list = list->Next;
  list->Next = dev;
  }
strcpy_s(dev->Snr, MAX_BUF_SIZE, snr);
dev->Index = INDEX_INVALID;  
return(dev);    
}


extern "C" static int32_t TinyCanDevValid(struct TTCanDev *dev)
{
struct TTCanDev *list;

if (!dev)
  return(-1);
for (list = TCanDevs; list; list = list->Next)
  {
  if (list == dev)
    return(0);
  }
return(-1);
}


extern "C" static  struct TTCanDev *TinyCanDevGetBySnr(const char *snr)
{
struct TTCanDev *list;

if (!snr)
  return(NULL);
for (list = TCanDevs; list; list = list->Next)
  {
  if (!strcmp(list->Snr, snr))
    return(list);
  }
return(NULL);  
}
  

extern "C" static char *GetSnrFromBusname(const char *busname)
{
char c;
int len;
char *snr;

if (!busname)
  return(NULL);
while ((c = *busname))
  {
  busname++;
  if (c == ':')
    break;
  }
len = strnlen_s(busname, MAX_BUF_SIZE);
if (len < 8)
  return(NULL);
snr = _strdup(busname);  
if (snr[len-1] == ')')
  snr[len-1] = '\0';
return(snr);
}


/********* functions which permit to communicate with the board ****************/
extern "C" int32_t __stdcall canReceive_driver(CAN_HANDLE fd0, struct canfd_frame *m)
{
struct TTCanDev *tcan_dev;
struct TCanFdMsg fd_msg;
uint32_t event, id;
uint8_t f;
int32_t res;

tcan_dev = (struct TTCanDev *)fd0;
if (TinyCanDevValid(tcan_dev) < 0)
  return(-1);  
event = CanExWaitForEvent(tcan_dev->Event, 100);  // Timeoutzeit auf 100ms
if (event & MHS_TERMINATE)      // terminate thread
  return(0);
else if (event & RX_EVENT)      // CAN Rx event      
  {
  if ((res = CanFdReceive(tcan_dev->Index, &fd_msg, 1)) < 0)
    return(res);
  if (res > 0)
    {
    id = fd_msg.Id; 
    if (fd_msg.MsgEFF)
      id |= CAN_EFF_FLAG;
    if (fd_msg.MsgRTR)
      id |= CAN_RTR_FLAG;
    else
      memcpy(m->data, fd_msg.MsgData, fd_msg.MsgLen);
    f = 0;
    if (fd_msg.MsgFD)
      f |= CANFD_FDF;
    if (fd_msg.MsgBRS)  
      f |= CANFD_BRS;      
    m->can_id = id;           /* 32 bit CAN_ID + EFF/RTR/ERR flags */
    m->len = fd_msg.MsgLen;   /* frame payload length in byte */     
    m->flags = f;             /* additional flags for CAN FD */      
    return(1);
    }
  }
return(0);  
}  


/***************************************************************************/
extern "C" int32_t __stdcall canSend_driver(CAN_HANDLE fd0, struct canfd_frame *m)
{
struct TTCanDev *tcan_dev;
struct TCanFdMsg fd_msg;

tcan_dev = (struct TTCanDev *)fd0;
if (TinyCanDevValid(tcan_dev) < 0)
  return(-1);
fd_msg.MsgFlags = 0L;   // Initialize Flag value (no RTR, Std-Frame format)
if (m->can_id & CAN_EFF_FLAG)
  fd_msg.MsgEFF = 1;
if (m->can_id & CAN_RTR_FLAG)
  fd_msg.MsgRTR = 1;
else
  memcpy(fd_msg.MsgData, m->data, m->len);
if (m->flags & CANFD_FDF)
  fd_msg.MsgFD = 1;
if (m->flags & CANFD_BRS)    
  fd_msg.MsgBRS = 1;                  
fd_msg.Id = (m->can_id & CAN_EFF_MASK);                         
fd_msg.MsgLen = m->len;     
return(CanFdTransmit(tcan_dev->Index, &fd_msg, 1));
}


/***************************************************************************/
extern "C" int32_t __stdcall canChangeBaudRate_driver(CAN_HANDLE fd, char *baud)
{
struct TTCanDev *tcan_dev;
int32_t err;
const struct TSpeedValue *speed;

tcan_dev = (struct TTCanDev *)fd;
if (TinyCanDevValid(tcan_dev) < 0)
  return(-1);
if (!(speed = TranslateBaudRate(baud)))
  return(-1); // Error
if (tcan_dev->BusRun)
  {    
  if (CanSetMode(tcan_dev->Index, OP_CAN_STOP, CAN_CMD_ALL_CLEAR) < 0)
    return(-1);
  }      
if ((err = CanExSetAsUWord(tcan_dev->Index, "CanSpeed1", speed->SpeedValue)) < 0)
  return(err);
if ((err = CanExSetAsUWord(tcan_dev->Index, "CanDSpeed1", speed->DSpeedValue)) < 0)
  return(err);
if (tcan_dev->BusRun)
  {  
  // **** CAN Bus Start
  if (CanSetMode(tcan_dev->Index, OP_CAN_START, CAN_CMD_ALL_CLEAR) < 0)
    {
    tcan_dev->BusRun = 0;
    return(-1);
    }
  }
return(0);
}


/***************************************************************************/
extern "C" CAN_HANDLE __stdcall canOpen_driver(s_BOARD * board)
{
const struct TSpeedValue *speed;
struct TTCanDev *tcan_dev;
char *snr;
char dev_open_str[MAX_BUF_SIZE];

if (TinyCanApiInit())
  return(NULL);
if (!(speed = TranslateBaudRate(board->baudrate)))
  return(NULL); // Error
if (!(snr = GetSnrFromBusname(board->busname)))
  return(NULL);  
if (!(tcan_dev = TinyCanDevGetBySnr(snr)))
  tcan_dev = TinyCanDevCreate(snr);
free(snr);        
if (tcan_dev->Index == INDEX_INVALID)
  {
  // **** Create Tiny-CAN Devices    
  if (CanExCreateDevice(&tcan_dev->Index, CREATE_DEVICE_OPTIONS) >= 0)
    {
    // **** 0 = disable transmit message request
    (void)CanExSetAsUByte(tcan_dev->Index, "CanTxAckEnable", 0);
    // **** 0 = disable Hardware time-stamps
    (void)CanExSetAsUByte(tcan_dev->Index, "TimeStampMode", 0);
    tcan_dev->Event = CanExCreateEvent();   // Create Event Object
    // Attach Tiny-CAN RX_EVENT to RX-Fifo
    CanExSetObjEvent(tcan_dev->Index, MHS_EVS_OBJECT, tcan_dev->Event, RX_EVENT);      
    }
  }
sprintf_s(dev_open_str, MAX_BUF_SIZE, "Snr=%s", tcan_dev->Snr);          
(void)CanExSetAsUWord(tcan_dev->Index, "CanSpeed1", speed->SpeedValue);
(void)CanExSetAsUWord(tcan_dev->Index, "CanDSpeed1", speed->DSpeedValue);    
if (!CanDeviceOpen(tcan_dev->Index, dev_open_str))   
  {
  // **** CAN Bus Start
  if (CanSetMode(tcan_dev->Index, OP_CAN_START, CAN_CMD_ALL_CLEAR) < 0)
    {
    // Device schließen
    (void)CanDeviceClose(tcan_dev->Index);
    return(NULL);
    } 
  else
    tcan_dev->BusRun = 1;  
  }
else
  return(NULL); 
return(tcan_dev);  // OK
}


/***************************************************************************/
extern "C" int32_t __stdcall canClose_driver(CAN_HANDLE fd0)
{
struct TTCanDev *tcan_dev;

tcan_dev = (struct TTCanDev *)fd0;
if (TinyCanDevValid(tcan_dev) < 0)
  return(1);
(void)CanSetMode(tcan_dev->Index, OP_CAN_STOP, CAN_CMD_ALL_CLEAR);  
CanExSetEvent(tcan_dev->Event, MHS_TERMINATE);
// Device schließen
(void)CanDeviceClose(tcan_dev->Index);
tcan_dev->BusRun = 0;
return 0;
}


typedef void (__stdcall *setStringValuesCB_t) (char *pStringValues[], int nValues);
static setStringValuesCB_t __stdcall gSetStringValuesCB;

void __stdcall NativeCallDelegate(char *pStringValues[], int nValues)
{
  if (gSetStringValuesCB)
    gSetStringValuesCB(pStringValues, nValues);
}


extern "C" void __stdcall canEnumerate2_driver(setStringValuesCB_t callback)
{
int32_t num_devs, i;
char str[MAX_BUF_SIZE];
struct TCanDevicesList *device_list;
char **values;
int len;

(void)TinyCanApiInit();
gSetStringValuesCB = callback;
if ((num_devs = CanExGetDeviceList(&device_list, 0)) > 0)
  {  
  values = (char **)malloc(sizeof(void *) * num_devs);
  for (i = 0; i < num_devs; i++)
    {  
    sprintf_s(str, MAX_BUF_SIZE, "%s (Snr:%s)", device_list[i].Description, device_list[i].SerialNumber);
    len = 1 + strnlen_s(str, MAX_BUF_SIZE);
    *(values + i) = (char *)malloc(len);
    strcpy_s(*(values + i), len, str);
    }
  CanExDataFree((void **)&device_list);  
  NativeCallDelegate(values, num_devs);  
  }
}  
