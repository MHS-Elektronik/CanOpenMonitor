#ifndef __MHS_CAN_UTIL_H__
#define __MHS_CAN_UTIL_H__

#include "can_drv.h"

#ifdef __cplusplus
  extern "C" {
#endif

#define API_CALL  __stdcall

#define ERR_NO_CAN_UTIL_DLL_LOAD -10000

#define CAN_CFG_SP_FLAGS_CLR               0xFFFFCFFF
#define CAN_CFG_FD_SP_FLAGS_CLR            0xFFFF3FFF

#define CAN_CFG_CLOCK_IDX_MASK             0x0000007F
#define CAN_CFG_CLOCK_SELECT_ONE           0x00000080
#define CAN_CFG_USE_ONE_CLOCK              0x00000100

#define CAN_CFG_USE_CIA_SP                 0x00001000
#define CAN_CFG_USE_SP_RANGE               0x00002000
#define CAN_CFG_FD_USE_CIA_SP              0x00004000
#define CAN_CFG_FD_USE_SP_RANGE            0x00008000
#define CAN_CFG_USE_SAM                    0x00010000
#define CAN_CFG_USE_TDC                    0x00020000
#define CAN_CFG_USE_FD                     0x00040000

#define CAN_CFG_AUTO_SJW                   0x00100000
#define CAN_CFG_FD_AUTO_SJW                0x00200000

#define CAN_CFG_AUTO_CLEAN_BITRATE_ERRORS  0x01000000
#define CAN_CFG_FD_ONLY_SAME_BRP           0x02000000
#define CAN_CFG_FD_AUTO_SAME_BRP           0x04000000

#define CUTIL_WIN_IDX_ANY              0

#define CUTIL_WIN_IDX_DRIVER_LIST      1
#define CUTIL_WIN_IDX_HW_LIST          2
#define CUTIL_WIN_IDX_HW_INFO          3
#define CUTIL_WIN_IDX_BITCALC_START    4
#define CUTIL_WIN_IDX_BITCALC_FD_CFG   5
#define CUTIL_WIN_IDX_BITCALC_STD_CFG  6
#define CUTIL_WIN_IDX_BITCALC_FD       7
#define CUTIL_WIN_IDX_BITCALC_STD      8

#define CUTIL_INFO_ONLY                 0x00000001

#define CUTIL_HW_SCAN_ACTIVE            0x00000010
#define CUTIL_HW_SCAN_DISABLE           0x00000020
#define CUTIL_HW_SHOW_ALL               0x00000040

#define CUTIL_BITCALC_EXIT_BUTTON       0x00000100
#define CUTIL_BITCALC_EXTRA_BUTTONS     0x00000200
#define CUTIL_BITCALC_CONTROLLER_SEL    0x00000400

#define CUTIL_BITCALC_SHOW_BTR_REGISTER 0x00001000

#pragma pack(push, 1)
struct TCanUtilDrvInfo
  {
  char Filename[255];
  char Name[40];
  char Version[40];
  char Summary[255];
  char Description[255];
  uint32_t InterfaceType;
  };
#pragma pack(pop)

int32_t LoadCanUtil(void);
void UnloadCanUtil(void);

/**************************************************************************************/
/*                          CAN Util Init/Down                                        */
/**************************************************************************************/
int32_t CanUtilInit(void);
void CanUtilDown(void);

/**************************************************************************************/
/*                          CAN API Treiber                                           */
/**************************************************************************************/
int32_t CanUtilRegisterDriver(void *api_handle, int32_t res);

/**************************************************************************************/
/*                              Public                                                */
/**************************************************************************************/
int32_t CanUtilCloseWin(uint32_t win_idx);

/**************************************************************************************/
/*                               Bit Timing                                           */
/**************************************************************************************/
int32_t CanUtilBitrateSelSetHw(const char *tiny_can, uint32_t revision, const char *snr,
          struct TCanInfoVar *hw_info, uint32_t hw_info_size, uint32_t index);    
int32_t CanUtilBitrateSelSetDefaults(uint32_t flags, uint32_t bitrate, double sp, double sp_error,
          uint32_t fd_bitrate, double fd_sp, double fd_sp_error);
int32_t CanUtilBitrateSelGetHwResult(uint32_t *nbtr_value, uint32_t *bitrate, uint32_t *clock_idx, char *bitrate_str,
          char *description, uint32_t *dbtr_value, uint32_t *fd_bitrate, char *fd_bitrate_str, char *fd_description);                                
int32_t CanUtilBitrateSelShowDialog(const char *title, const char *sub_title, uint32_t flags);

/**************************************************************************************/
/*                          Driver DLLs Info                                          */
/**************************************************************************************/
int32_t CanUtilGetDriverList(struct TCanUtilDrvInfo **drv_info_list);
int32_t CanUtilGetDriverListGet(struct TCanUtilDrvInfo *item);
int32_t CanUtilGetSelectedDriver(struct TCanUtilDrvInfo *selected_drv);
int32_t CanUtilShowDriverList(const char *title, const char *sub_title, uint32_t flags, char *name);

/**************************************************************************************/
/*                          Connected Hardware                                        */
/**************************************************************************************/
int32_t CanUtilGetHwList(struct TCanDevicesList **list);
int32_t CanUtilGetHwListGet(struct TCanDevicesList *item);
int32_t CanUtilGetSelectedHw(struct TCanDevicesList *sel_dev);
int32_t CanUtilShowHwWin(const char *title, const char *sub_title, uint32_t flags, char *snr);

/**************************************************************************************/
/*                            Show Hardware Info                                      */
/**************************************************************************************/
int32_t CanUtilHwInfoWin(const char *title, const char *sub_title, uint32_t flags,
          struct TCanInfoVar *hw_info, uint32_t hw_info_size, uint32_t index);


#ifdef __cplusplus
  }
#endif

#endif


