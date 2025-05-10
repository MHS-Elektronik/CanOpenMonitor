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

#ifndef __can_h__
#define __can_h__

#include <stdint.h>

/* special address description flags for the CAN_ID */
#define CAN_EFF_FLAG 0x80000000U /* EFF/SFF is set in the MSB */
#define CAN_RTR_FLAG 0x40000000U /* remote transmission request */
#define CAN_ERR_FLAG 0x20000000U /* error message frame */

/* valid bits in CAN ID for frame formats */
#define CAN_SFF_MASK 0x000007FFU /* standard frame format (SFF) */
#define CAN_EFF_MASK 0x1FFFFFFFU /* extended frame format (EFF) */
#define CAN_ERR_MASK 0x1FFFFFFFU /* omit EFF, RTR, ERR flags */

/*
 * Controller Area Network Identifier structure
 *
 * bit 0-28	: CAN identifier (11/29 bit)
 * bit 29	: error message frame flag (0 = data frame, 1 = error message)
 * bit 30	: remote transmission request flag (1 = rtr frame)
 * bit 31	: frame format flag (0 = standard 11 bit, 1 = extended 29 bit)
 */

#define CANFD_MAX_DLEN 64

#define CANFD_BRS 0x01 /* bit rate switch (second bitrate for payload data) */
#define CANFD_ESI 0x02 /* error state indicator of the transmitting node */
#define CANFD_FDF 0x04 /* mark CAN FD for dual use of struct canfd_frame */


/** 
 * @brief The CAN message structure (identical to LINUX socketcan)
 * @ingroup can
 */
struct canfd_frame {
  uint32_t can_id;   /* 32 bit CAN_ID + EFF/RTR/ERR flags */
  uint8_t len;       /* frame payload length in byte */     
  uint8_t flags;     /* additional flags for CAN FD */      
  uint8_t __res0;    /* reserved / padding */               
  uint8_t __res1;    /* reserved / padding */               
  uint8_t data[CANFD_MAX_DLEN]; 
};


typedef void* CAN_HANDLE;

typedef void* CAN_PORT;

//#define Message_Initializer {0,0,0,{0,0,0,0,0,0,0,0}}<*>

//typedef uint8_t (*canSend_t)(canfd_frame *);<*>

#endif /* __can_h__ */
