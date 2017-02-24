#ifndef PACKET_H
#define PACKET_H

/*
Stuff for packets
Used by both server & client program
For creating/parsing packets

Packet format
ABCD
A - 1 byte, unsigned char
	length of whole packet
	used to verify final packet
B - 1 byte, unsigned char
	command
C - optional, whatever the command needs
D - 4 bytes
	ending sequence, "\r\n\r\n"
	used to tell if packet found, (use w/ length to verify)
No spaces in between ABC, can be spaces in C contents
*/

// Includes
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "stringMisc.h"
#include "structs.h"


// Defines

// moved to structs.h
/*
// Packet size stuff, if one value changed - ensure others are still correct
// Bytes for length (0-255)
#define PACKET_SIZE_LENGTH 1
// Max packet length
#define PACKET_MAX_LENGTH 255
// Bytes for command (0-255)
#define PACKET_SIZE_COMMAND 1
#define PACKET_MAX_COMMANDS 255
// Bytes for ending sequence
#define PACKET_SIZE_END 4
// Ending sequence for packets
#define PACKET_ENDSEQ "\r\n\r\n"
// Minimum packet length
#define PACKET_SIZE_MIN (PACKET_SIZE_LENGTH+PACKET_SIZE_COMMAND+PACKET_SIZE_END)
// Max size of optional stuff
#define PACKET_MAX_OPTIONAL (PACKET_MAX_LENGTH-PACKET_SIZE_MIN)
// Max for messages, can't use optional since server will compile final message
// which includes displaying who sent it
#define PACKET_MAX_MSG (PACKET_MAX_OPTIONAL-150)
*/

// Command values

// Base command, don't want to use 0 (null terminator)
// even though it should be an unsigned char array, don't want to risk
#define PACKET_CMD_BASE 1

// for invalid command
#define PACKET_CMD_INVALID PACKET_CMD_BASE

// Connections
// Client quit
#define PACKET_CMD_QUIT_C (PACKET_CMD_BASE+1)
// Server quit
#define PACKET_CMD_QUIT_S (PACKET_CMD_QUIT_C+1)

// Misc
// Notification from server
#define PACKET_CMD_NOTI (PACKET_CMD_QUIT_S+1)
// Give queue position (to client), request (to server)
#define PACKET_CMD_QP (PACKET_CMD_NOTI+1)
// Give client number (to client), request (to server)
#define PACKET_CMD_CN (PACKET_CMD_QP+1)
// Give channel name (to client), request (to server)
#define PACKET_CMD_CH (PACKET_CMD_CN+1)
// Give/Request client list
#define PACKET_CMD_CL (PACKET_CMD_CH+1)
// Give/Request channel list
#define PACKET_CMD_CHL (PACKET_CMD_CL+1)

// Messages
// Global message (to server)
#define PACKET_CMD_MSG_G (PACKET_CMD_CHL+1)
// Private message (to server)
#define PACKET_CMD_MSG_P (PACKET_CMD_MSG_G+1)
// Channel message (to server)
#define PACKET_CMD_MSG_C (PACKET_CMD_MSG_P+1)
// Display message (to client)
#define PACKET_CMD_MSG_D (PACKET_CMD_MSG_C+1)

// Private messages
// Give/Request private messaging with client number
#define PACKET_CMD_PRVT (PACKET_CMD_MSG_D+1)

// Channels
// Create channel request
#define PACKET_CMD_CH_CREATE (PACKET_CMD_PRVT+1)
// Join channel request
#define PACKET_CMD_CH_JOIN (PACKET_CMD_CH_CREATE+1)
// Leave channel request
#define PACKET_CMD_CH_LEAVE (PACKET_CMD_CH_JOIN+1)



// Functions
unsigned char * packetCreate(int, char *);
unsigned char * packetCreateFinal(unsigned char, unsigned char *);
unsigned char * packetAllocate(int);
int packetValid(unsigned char *);
unsigned char * packetCreateQuit();
unsigned char * packetCreateQuitAll();
unsigned char * packetCreateMsgD(char *);
unsigned char * packetCreateMsgG(char *);
unsigned char * packetCreateMsgC(char *);
unsigned char * packetCreateMsgP(char *);
unsigned char * packetCreateLClR();
unsigned char * packetCreateLCl(char *);
unsigned char * packetCreateLChR();
unsigned char * packetCreateLCh(char *);
unsigned char * packetCreateNewChR(char);
unsigned char * packetCreateJoinChR(char);
unsigned char * packetCreateLeaveCh();
unsigned char * packetCreateCurQPos(int);
unsigned char * packetCreateCurCN(int);
unsigned char * packetCreateCurCh(char);
unsigned char * packetCreateSetPr(int);
unsigned char * packetCreateNoti(char *);



#endif
