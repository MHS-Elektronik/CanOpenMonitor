/*
This file is part of CanFestival, a library implementing CanOpen Stack.

Copyright (C): Edouard TISSERANT and Francis DUPIN

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

#ifndef __can_driver_h__
#define __can_driver_h__

struct struct_s_BOARD;

typedef struct struct_s_BOARD s_BOARD;

#include <stdint.h>
#include <windows.h>
#include <string.h>
#include <stdio.h>
#include "can.h"

/**
 * @brief The CAN board configuration
 * @ingroup can
 */

//struct struct_s_BOARD {
//  char busname[100]; /**< The bus name on which the CAN board is connected */
//  char baudrate[4]; /**< The board baudrate */
//};

struct struct_s_BOARD {
  char * busname;  /**< The bus name on which the CAN board is connected */
  char * baudrate; /**< The board baudrate */
};


#ifndef DLL_CALL
#if !defined(WIN32) || defined(__CYGWIN__) || defined(__MINGW32__)
#define LIBAPI
#define DLL_CALL(funcname) funcname##_driver
#else
#define LIBAPI __stdcall
//Windows was missing the definition of the calling convention
#define DLL_CALL(funcname) LIBAPI funcname##_driver
#endif
#endif //DLL_CALL

#define LIBPUBLIC

#ifndef FCT_PTR_INIT
#define FCT_PTR_INIT
#endif


int32_t DLL_CALL(canReceive)(CAN_HANDLE, struct canfd_frame *)FCT_PTR_INIT;
int32_t DLL_CALL(canSend)(CAN_HANDLE, struct canfd_frame *)FCT_PTR_INIT;
CAN_HANDLE DLL_CALL(canOpen)(s_BOARD *)FCT_PTR_INIT;
int DLL_CALL(canClose)(CAN_HANDLE)FCT_PTR_INIT;
int32_t DLL_CALL(canChangeBaudRate)(CAN_HANDLE, char *)FCT_PTR_INIT;
int32_t DLL_CALL(canEnumerate)(char ** out,int * len)FCT_PTR_INIT;


#endif
