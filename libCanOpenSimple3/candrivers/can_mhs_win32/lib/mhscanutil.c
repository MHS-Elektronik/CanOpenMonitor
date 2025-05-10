/***************************************************************************
                        mhscanutil.c  -  description
                             -------------------
    begin             : 06.02.2022
    last modify       : 15.08.2022
    copyright         : (C) 2022 by MHS-Elektronik GmbH & Co. KG, Germany
    author            : Klaus Demlehner, klaus@mhs-elektronik.de
 ***************************************************************************/
#ifdef __WIN32__
  #include <stdint.h>
  #include <string.h>
  #include <stdlib.h>
  #include <windows.h>
#else
  #include <stdio.h>
  #include <stdlib.h>
  #include <unistd.h>
  #include <dlfcn.h>
#endif
#include "mhscanutil.h"


#ifdef __WIN32__
static HINSTANCE CanUtilDllHandle = 0;               // Handle to DLL
static char *CanUtilDllFileName = NULL;
#else
static void *CanUtilDllHandle = NULL;
#endif


#ifdef __WIN32__
  // ****** Windows
  #define mhs_sleep(x) Sleep(x)
  #define MhsGetProcAddress(h, s) (void *)GetProcAddress((h), (LPCSTR)(s))
#else
  // ****** Linux
  #define mhs_sleep(x) usleep((x) * 1000)
  #define MhsGetProcAddress(h, s) dlsym((h), (s))
#endif


#ifdef __WIN32__

#define MhsSafeFree(d) do { \
  if ((d)) \
    { \
    free((d)); \
    (d) = NULL; \
    } \
  } while(0)

static char *GetCanUtilDllWithPath(void);
#endif

typedef int32_t (API_CALL *TCanUtilInit)(void);
typedef void (API_CALL *TCanUtilDown)(void);

typedef int32_t (API_CALL *TCanUtilRegisterDriver)(void *api_handle, int32_t res);
typedef int32_t (API_CALL *TCanUtilCloseWin)(uint32_t win_idx);

typedef int32_t (API_CALL *TCanUtilBitrateSelSetHw)(const char *tiny_can, uint32_t revision, const char *snr,
      struct TCanInfoVar *hw_info, uint32_t hw_info_size, uint32_t index);
typedef int32_t (API_CALL *TCanUtilBitrateSelSetDefaults)(uint32_t flags, uint32_t bitrate, double sp, double sp_error,
                                     uint32_t fd_bitrate, double fd_sp, double fd_sp_error);      
typedef int32_t (API_CALL *TCanUtilBitrateSelGetHwResult)(uint32_t *nbtr_value, uint32_t *bitrate, uint32_t *clock_idx, char *bitrate_str, char *description,
                                                        uint32_t *dbtr_value, uint32_t *fd_bitrate, char *fd_bitrate_str, char *fd_description);                                                        
typedef int32_t (API_CALL *TCanUtilBitrateSelShowDialog)(const char *title, const char *sub_title, uint32_t flags);

typedef int32_t (API_CALL *TCanUtilGetDriverList)(struct TCanUtilDrvInfo **drv_info_list);
typedef int32_t (API_CALL *TCanUtilGetDriverListGet)(struct TCanUtilDrvInfo *item);
typedef int32_t (API_CALL *TCanUtilGetSelectedDriver)(struct TCanUtilDrvInfo *selected_drv);
typedef int32_t (API_CALL *TCanUtilShowDriverList)(const char *title, const char *sub_title, uint32_t flags, char *name);

typedef int32_t (API_CALL *TCanUtilGetHwList)(struct TCanDevicesList **list);
typedef int32_t (API_CALL *TCanUtilGetHwListGet)(struct TCanDevicesList *item);
typedef uint32_t (API_CALL *TCanUtilGetSelectedHw)(struct TCanDevicesList *sel_dev);
typedef int32_t (API_CALL *TCanUtilShowHwWin)(const char *title, const char *sub_title, uint32_t flags, char *snr);

typedef int32_t (API_CALL *TCanUtilHwInfoWin)(const char *title, const char *sub_title, uint32_t flags, struct TCanInfoVar *hw_info, uint32_t hw_info_size, uint32_t index);


static TCanUtilInit PCanUtilInit = NULL;
static TCanUtilDown PCanUtilDown = NULL;
static TCanUtilRegisterDriver PCanUtilRegisterDriver = NULL;
static TCanUtilCloseWin PCanUtilCloseWin = NULL;

static TCanUtilBitrateSelSetHw PCanUtilBitrateSelSetHw = NULL;
static TCanUtilBitrateSelSetDefaults PCanUtilBitrateSelSetDefaults = NULL;
static TCanUtilBitrateSelGetHwResult PCanUtilBitrateSelGetHwResult = NULL;
static TCanUtilBitrateSelShowDialog PCanUtilBitrateSelShowDialog = NULL;

static TCanUtilGetDriverList PCanUtilGetDriverList = NULL;
static TCanUtilGetDriverListGet PCanUtilGetDriverListGet = NULL;
static TCanUtilGetSelectedDriver PCanUtilGetSelectedDriver = NULL;
static TCanUtilShowDriverList PCanUtilShowDriverList = NULL;

static TCanUtilGetHwList PCanUtilGetHwList = NULL;
static TCanUtilGetHwListGet PCanUtilGetHwListGet = NULL;
static TCanUtilGetSelectedHw PCanUtilGetSelectedHw = NULL;
static TCanUtilShowHwWin PCanUtilShowHwWin = NULL;

static TCanUtilHwInfoWin PCanUtilHwInfoWin = NULL;

/***************************************************************/
/*  Treiber DLL laden                                          */
/***************************************************************/
int32_t LoadCanUtil(void)
{
int32_t error;

error = 0;
UnloadCanUtil();
#ifdef __WIN32__
if (!(CanUtilDllFileName = GetCanUtilDllWithPath()))
  return(-100);
CanUtilDllHandle = LoadLibraryA(CanUtilDllFileName);
#else
CanUtilDllHandle = dlopen(file_name, RTLD_LAZY);
#endif
if (CanUtilDllHandle)
  {
  if (!(PCanUtilInit = (TCanUtilInit)MhsGetProcAddress(CanUtilDllHandle, "CanUtilInit")))
    error = -1;
  else if (!(PCanUtilDown = (TCanUtilDown)MhsGetProcAddress(CanUtilDllHandle, "CanUtilDown")))
    error = -1;
  else if (!(PCanUtilRegisterDriver = (TCanUtilRegisterDriver)MhsGetProcAddress(CanUtilDllHandle, "CanUtilRegisterDriver")))
    error = -1;
  else if (!(PCanUtilCloseWin = (TCanUtilCloseWin)MhsGetProcAddress(CanUtilDllHandle, "CanUtilCloseWin")))
    error = -1;  
  else if (!(PCanUtilBitrateSelSetHw = (TCanUtilBitrateSelSetHw)MhsGetProcAddress(CanUtilDllHandle, "CanUtilBitrateSelSetHw")))
    error = -1;
  else if (!(PCanUtilBitrateSelSetDefaults = (TCanUtilBitrateSelSetDefaults)MhsGetProcAddress(CanUtilDllHandle, "CanUtilBitrateSelSetDefaults")))
    error = -1;    
  else if (!(PCanUtilBitrateSelGetHwResult = (TCanUtilBitrateSelGetHwResult)MhsGetProcAddress(CanUtilDllHandle, "CanUtilBitrateSelGetHwResult")))
    error = -1;    
  else if (!(PCanUtilBitrateSelShowDialog = (TCanUtilBitrateSelShowDialog)MhsGetProcAddress(CanUtilDllHandle, "CanUtilBitrateSelShowDialog")))
    error = -1;
  else if (!(PCanUtilGetDriverList = (TCanUtilGetDriverList)MhsGetProcAddress(CanUtilDllHandle, "CanUtilGetDriverList")))
    error = -1;
  else if (!(PCanUtilGetDriverListGet = (TCanUtilGetDriverListGet)MhsGetProcAddress(CanUtilDllHandle, "CanUtilGetDriverListGet")))
    error = -1;
  else if (!(PCanUtilGetSelectedDriver = (TCanUtilGetSelectedDriver)MhsGetProcAddress(CanUtilDllHandle, "CanUtilGetSelectedDriver")))
    error = -1;
  else if (!(PCanUtilShowDriverList = (TCanUtilShowDriverList)MhsGetProcAddress(CanUtilDllHandle, "CanUtilShowDriverList")))
    error = -1;
  else if (!(PCanUtilGetHwList = (TCanUtilGetHwList)MhsGetProcAddress(CanUtilDllHandle, "CanUtilGetHwList")))
    error = -1;
  else if (!(PCanUtilGetHwListGet = (TCanUtilGetHwListGet)MhsGetProcAddress(CanUtilDllHandle, "CanUtilGetHwListGet")))
    error = -1;
  else if (!(PCanUtilGetSelectedHw = (TCanUtilGetSelectedHw)MhsGetProcAddress(CanUtilDllHandle, "CanUtilGetSelectedHw")))
    error = -1;
  else if (!(PCanUtilShowHwWin = (TCanUtilShowHwWin)MhsGetProcAddress(CanUtilDllHandle, "CanUtilShowHwWin")))
    error = -1;
  else if (!(PCanUtilHwInfoWin = (TCanUtilHwInfoWin)MhsGetProcAddress(CanUtilDllHandle, "CanUtilHwInfoWin")))
    error = -1;
  }
else
  error = -1000;
if (error)
  UnloadCanUtil();
return(error);
}

/***************************************************************/
/*  Treiber DLL entladen                                       */
/***************************************************************/
void UnloadCanUtil(void)
{
#ifdef __WIN32__
HINSTANCE driver_handle;
#else
void *driver_handle;
#endif

driver_handle = CanUtilDllHandle;
CanUtilDllHandle = NULL;
#ifdef __WIN32__
MhsSafeFree(CanUtilDllFileName);
#endif
PCanUtilInit = NULL;
PCanUtilDown = NULL;
PCanUtilRegisterDriver = NULL;
PCanUtilCloseWin = NULL;
PCanUtilBitrateSelSetHw = NULL;
PCanUtilBitrateSelSetDefaults = NULL;
PCanUtilBitrateSelGetHwResult = NULL;
PCanUtilBitrateSelShowDialog = NULL;
PCanUtilGetDriverList = NULL;
PCanUtilGetDriverListGet = NULL;
PCanUtilGetSelectedDriver = NULL;
PCanUtilShowDriverList = NULL;
PCanUtilGetHwList = NULL;
PCanUtilGetHwListGet = NULL;
PCanUtilGetSelectedHw = NULL;
PCanUtilShowHwWin = NULL;
PCanUtilHwInfoWin = NULL;
if (driver_handle)
  {
#ifdef __WIN32__
  FreeLibrary(driver_handle);
#else
  dlclose(driver_handle);
#endif
  }
}


int32_t CanUtilInit(void)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilInit))
  err = (PCanUtilInit)();
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


void CanUtilDown(void)
{
if ((CanUtilDllHandle) && (PCanUtilInit))
  (PCanUtilDown)();
}


int32_t CanUtilRegisterDriver(void *api_handle, int32_t res)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilRegisterDriver))
  err = (PCanUtilRegisterDriver)(api_handle, res);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


int32_t CanUtilCloseWin(uint32_t win_idx)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilCloseWin))
  err = (PCanUtilCloseWin)(win_idx);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


int32_t CanUtilBitrateSelSetHw(const char *tiny_can, uint32_t revision, const char *snr,
      struct TCanInfoVar *hw_info, uint32_t hw_info_size, uint32_t index)
{      
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilBitrateSelSetHw))
  err = (PCanUtilBitrateSelSetHw)(tiny_can, revision, snr, hw_info, hw_info_size, index);      
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}

      
int32_t CanUtilBitrateSelSetDefaults(uint32_t flags, uint32_t bitrate, double sp, double sp_error,
                                     uint32_t fd_bitrate, double fd_sp, double fd_sp_error);      
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilBitrateSelSetDefaults))
  err = (PCanUtilBitrateSelSetDefaults)(flags, bitrate, sp, sp_error, fd_bitrate, fd_sp, fd_sp_error);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
} 


int32_t CanUtilBitrateSelGetHwResult(uint32_t *nbtr_value, uint32_t *bitrate, uint32_t *clock_idx, char *bitrate_str, char *description,
                                   uint32_t *dbtr_value, uint32_t *fd_bitrate, char *fd_bitrate_str, char *fd_description)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilBitrateSelGetHwResult))
  err = (PCanUtilBitrateSelGetHwResult)(nbtr_value, bitrate, clock_idx, bitrate_str, description,
                                      dbtr_value, fd_bitrate, fd_bitrate_str, fd_description);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


int32_t CanUtilBitrateSelShowDialog(const char *title, const char *sub_title, uint32_t flags)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilBitrateSelShowDialog))
  err = (PCanUtilBitrateSelShowDialog)(title, sub_title, flags);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}

/**************************************************************************************/
/*                          Driver DLLs Info                                          */
/**************************************************************************************/
int32_t CanUtilGetDriverList(struct TCanUtilDrvInfo **drv_info_list)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilGetDriverList))
  err = (PCanUtilGetDriverList)(drv_info_list);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


int32_t CanUtilGetDriverListGet(struct TCanUtilDrvInfo *item)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilGetDriverListGet))
  err = (PCanUtilGetDriverListGet)(item);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


int32_t CanUtilGetSelectedDriver(struct TCanUtilDrvInfo *selected_drv)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilGetSelectedDriver))
  err = (PCanUtilGetSelectedDriver)(selected_drv);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


int32_t CanUtilShowDriverList(const char *title, const char *sub_title, uint32_t flags, char *name)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilShowDriverList))
  err = (PCanUtilShowDriverList)(title, sub_title, flags, name);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


/**************************************************************************************/
/*                          Connected Hardware                                        */
/**************************************************************************************/
int32_t CanUtilGetHwList(struct TCanDevicesList **list)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilGetHwList))
  err = (PCanUtilGetHwList)(list);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


int32_t CanUtilGetHwListGet(struct TCanDevicesList *item)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilGetHwListGet))
  err = (PCanUtilGetHwListGet)(item);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


int32_t CanUtilGetSelectedHw(struct TCanDevicesList *sel_dev)
{
if ((CanUtilDllHandle) && (PCanUtilGetSelectedHw))
  return((PCanUtilGetSelectedHw)(sel_dev));
else
  return(ERR_NO_CAN_UTIL_DLL_LOAD);
}


int32_t CanUtilShowHwWin(const char *title, const char *sub_title, uint32_t flags, char *snr)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilShowHwWin))
  err = (PCanUtilShowHwWin)(title, sub_title, flags, snr);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}

/**************************************************************************************/
/*                            Show Hardware Info                                      */
/**************************************************************************************/
int32_t CanUtilHwInfoWin(const char *title, const char *sub_title, uint32_t flags, struct TCanInfoVar *hw_info, uint32_t *hw_info_size, uint32_t index)
{
int32_t err;

if ((CanUtilDllHandle) && (PCanUtilHwInfoWin))
  err = (PCanUtilHwInfoWin)(title, sub_title, flags, hw_info, hw_info_size, index);
else
  err = ERR_NO_CAN_UTIL_DLL_LOAD;
return(err);
}


#ifdef __WIN32__
/***************************************************************/
/*                API Treiber Path ermitteln                   */
/***************************************************************/
#define MAX_PATH_LEN 255

#define DIR_SEPARATOR '\\'
#define DIR_SEPARATOR_STR "\\"

/**************************************************************************/
/*                        C O N S T A N T E N                             */
/**************************************************************************/
#ifdef _WIN64
static const char REG_TINY_CAN_API[] = {"Software\\Wow6432Node\\Tiny-CAN\\API"};
#else
static const char REG_TINY_CAN_API[] = {"Software\\Tiny-CAN\\API"};
#endif
static const char REG_TINY_CAN_API_PATH_ENTRY[] = {"PATH"};

static const char MHS_CAN_UTIL_DLL[] = {"mhscanutil.dll"};
#ifdef _WIN64
static const char API_DRIVER_X64_DIR[] = {"x64"};
#endif

/**************************************************************************/
/*                        F U N K T I O N E N                             */
/**************************************************************************/
static char *MhsStpCpy(char *dest, const char *src)
{
register char *d = dest;
register const char *s = src;

do
  *d++ = *s;
while (*s++);

return(d - 1);
}


static char *MhsStrDup(const char *str)
{
size_t len;
char *new_str;

if (str)
  {
  len = strlen(str) + 1;
  new_str = (char *)malloc(len);
  if (!new_str)
    return(NULL);
  memcpy(new_str, str, len);
  return(new_str);
  }
else
  return(NULL);
}


static char *MhsStrconcat(const char *string1, ...)
{
size_t l;
va_list args;
char *s, *concat, *ptr;

if (!string1)
  return(NULL);

l = 1 + strlen(string1);
va_start(args, string1);
s = va_arg(args, char*);
while (s)
  {
  l += strlen(s);
  s = va_arg(args, char*);
  }
va_end(args);

concat = (char *)malloc(l);
if (!concat)
  return(NULL);
ptr = concat;

ptr = MhsStpCpy(ptr, string1);
va_start (args, string1);
s = va_arg (args, char*);
while (s)
  {
  ptr = MhsStpCpy(ptr, s);
  s = va_arg(args, char*);
  }
va_end (args);

return(concat);
}


static char *MhsCreateFileName(const char *dir, const char *sub_dir, const char *file_name)
{
if (!dir)
  return(MhsStrDup(file_name));
if (!file_name)
  return(NULL);
if (strchr(file_name, DIR_SEPARATOR))
  return(MhsStrDup(file_name));
else
  {
  if (sub_dir)
    {
    if (dir[strlen(dir)-1] == DIR_SEPARATOR)
      {
      if (sub_dir[strlen(sub_dir)-1] == DIR_SEPARATOR)
        return(MhsStrconcat(dir, sub_dir, file_name, NULL));
      else
        return(MhsStrconcat(dir, sub_dir, DIR_SEPARATOR_STR, file_name, NULL));
      }
    else
      {
      if (sub_dir[strlen(sub_dir)-1] == DIR_SEPARATOR)
        return(MhsStrconcat(dir, DIR_SEPARATOR_STR, sub_dir, file_name, NULL));
      else
        return(MhsStrconcat(dir, DIR_SEPARATOR_STR, sub_dir, DIR_SEPARATOR_STR, file_name, NULL));
      }
    }
  else
    {
    if (dir[strlen(dir)-1] == DIR_SEPARATOR)
      return(MhsStrconcat(dir, file_name, NULL));
    else
      return(MhsStrconcat(dir, DIR_SEPARATOR_STR, file_name, NULL));
    }
  }
}


static char *MhsRegGetString(HKEY key, const char *value_name)
{
DWORD type, data_size;
char *str;

type = 0;
data_size = 0;
if ((!key) || (!value_name))
  return(NULL);
// Länge des Strings ermitteln
if (RegQueryValueExA(key, value_name, 0, &type, NULL, &data_size) != ERROR_SUCCESS)
  return(NULL);
// Wert ist String ?
if (type != REG_SZ)
  return(NULL);
str = malloc(data_size+1);
if (!str)
  return(NULL);
// String lesen
if (RegQueryValueExA(key, value_name, 0, &type, (LPBYTE)str, &data_size) != ERROR_SUCCESS)
  {
  free(str);
  return(NULL);
  }
str[data_size] = '\0';
return(str);
}


static char *MhsRegReadStringEntry(const char *path, const char *entry)
{
HKEY key;
char *value;

value = NULL;
// HKEY_CURRENT_USER ?
if (RegOpenKeyExA(HKEY_LOCAL_MACHINE, path, 0, KEY_QUERY_VALUE, &key) == ERROR_SUCCESS) // KEY_READ
  {
  value = MhsRegGetString(key, entry);
  RegCloseKey(key);
  }
return(value);
}


static int32_t TestApi(const char *file_name)
{
HINSTANCE driver_dll;
TCanUtilBitrateSelSetup func;

func = NULL;
driver_dll = LoadLibraryA(file_name);
if (driver_dll)
  {
  func = (TCanUtilBitrateSelSetup)(void *)GetProcAddress(driver_dll, (LPCSTR)"CanUtilBitrateSelSetup");
  FreeLibrary(driver_dll);
  }
if (func)
  return(0);
else
  return(-1);
}


static char *GetCanUtilDllWithPath(void)
{
char *path, *file_name;
char app_file[MAX_PATH_LEN];
char app_path[MAX_PATH_LEN];
char *ptr;

file_name = NULL;
if ((path = MhsRegReadStringEntry(REG_TINY_CAN_API, REG_TINY_CAN_API_PATH_ENTRY)))
  {
#ifdef _WIN64
  file_name = MhsCreateFileName(path, API_DRIVER_X64_DIR, MHS_CAN_UTIL_DLL);
#else
  file_name = MhsCreateFileName(path, NULL, MHS_CAN_UTIL_DLL);
#endif
  if (TestApi(file_name))
    MhsSafeFree(file_name);
  free(path);
  }
if (!file_name)
  {
  // Programm Path ermitteln
  GetModuleFileNameA(GetModuleHandle(NULL), app_file, MAX_PATH_LEN);
  GetFullPathNameA(app_file, sizeof(app_file), app_path, &ptr);
  app_path[ptr-app_path] = '\0';
  file_name = MhsCreateFileName(app_path, NULL, MHS_CAN_UTIL_DLL);
  if (TestApi(file_name))
    MhsSafeFree(file_name);
  }
return(file_name);
}

#endif
