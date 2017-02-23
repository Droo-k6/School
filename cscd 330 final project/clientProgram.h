#ifndef CLIENTPROG_H
#define CLIENTPROG_H

// Includes
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <sys/mman.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <errno.h>

#include "stringMisc.h"
#include "packet.h"
#include "parsePacket.h"
#include "packetQueue.h"
#include "packetBuffer.h"
#include "parseInputClient.h"


// Defines

// Port to use
#define PORT 3000

// Max input buffer
#define MAX_BUF_INPUT 1024
// Max recv buffer
#define MAX_BUF_RECV (PACKET_MAX_LENGTH*20)
// Max packet buffer
// (PACKET_MAX_LENGTH*20)
#define MAX_BUF_PACKETS PB_MAX
// Max size of a message
#define MAX_MSG PACKET_MAX_MSG
// Max size sendable
#define MAX_SEND PACKET_MAX_LENGTH

// Max client number possible
#define MAX_CN 10

// Defines for chat setting
#define TYPE_GLOBAL 0
#define TYPE_CHANNEL 1
#define TYPE_PRIVATE 2




// typedefs
typedef struct timeval timeVal;
typedef struct sockaddr sockAddr;
typedef struct sockaddr_in sockAddr_in;


// Function prototypes
void printHelp();
void printInfo(int, int, int, char, int);
int sendLoop(int, unsigned char *);
void sendQuit(int);
void sendQuitAll(int);
void sendGlobal(int, char *);
void sendChannel(int, char *);
void sendGetCList(int);
void sendGetChList(int);
void sendCreateCh(int, char);
void sendJoinCh(int, char);
void sendLeaveCh(int);
void printCList(char *);
void printChList(char *);



#endif
