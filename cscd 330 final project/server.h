#ifndef SERVER_H
#define SERVER_H


// Includes
#include <stdlib.h>
#include <stdio.h>
#include <sys/mman.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <fcntl.h>
#include <string.h>
#include <unistd.h>
#include <errno.h>

#include "stringMisc.h"
#include "structs.h"
#include "packet.h"
#include "packetBuffer.h"
#include "packetQueue.h"
#include "parsePacket.h"
#include "client.h"
#include "channel.h"
#include "queue.h"
#include "parseInputServer.h"

// Defines
// Port for connection
#define PORT 3000
// Max amount of total connections
#define MAX_LISTEN 8

// max total connections (active + queue)
#define MAX_CONNECTIONS MAX_LISTEN
// max number of active people
#define MAX_ACTIVE_CONNECTIONS 5
// max in queue
#define MAX_QUEUE (MAX_CONNECTIONS-MAX_ACTIVE_CONNECTIONS)
// max number of channels
#define MAX_CHANNELS MAX_ACTIVE_CONNECTIONS+1
// max in channel, defined in channel.h

// For strings
// max buffer for receiving
#define MAX_RECV_BUFFER MAX_PACKET
// max buffer for sendings
#define MAXSEND MAX_PACKET

// Max input buffer
#define MAX_BUF_INPUT 1024
// Max recv buffer
#define MAX_BUF_RECV (PACKET_MAX_LENGTH*20)
// Max packet buffer
#define MAX_BUF_PACKETS (PACKET_MAX_LENGTH*20);
// Max size of a message
#define MAX_MSG PACKET_MAX_MSG
// Max size sendable
#define MAX_SEND PACKET_MAX_LENGTH

// Max client number possible
#define MAX_CN 10

// Defines for chat setting
#define TYPE_GLOBAL 0
#define TYPE_CHANNEL 1


// typedef
typedef struct timeval timeVal;
typedef struct sockaddr sockAddr;
typedef struct sockaddr_in sockAddr_in;

//struct queue;
//typedef struct queue queue;
//typedef struct channel channel;
//typedef struct client client;

// Function prototypes
void printHelpCmd();
void printInfo();
void printQueue(struct queue *);
void printChannels(struct channel **, int, int);
void printClients(struct client **, int, int);
client * addConnection(struct client **, int, int *, int);
int addActiveClient(struct client *, struct client **arr, int, int *);
void removeActiveClient(struct client *, struct client **arr, int, int *);
void removeClient(struct client *, struct client **arr, int, int *);
void refreshQueue(struct queue *, struct client **arr, int, int *);
int sendQUpdate(struct client *);
void addPrivateCN(struct client *c, int ocn, struct client **, int);
int sendLoop(int, unsigned char *);
void sendNoti(int, char *);
void sendGlobalS(struct client **, int, char *);
void sendGlobalC(struct client **, int, struct client *, char *);
void sendChannelS(struct channel *, char *);
void sendChannelC(struct client *, char *, struct channel *);
void sendPrivateS(struct client *, char *);
void sendPrivateC(struct client *, char *);
void sendQuitAll(struct client **, int);
void sendClList(int, struct client **, int);
void sendChList(int, struct channel **, int);
void sendCurCN(int, int);
int sendQPos(int, int);
void sendCurCh(int, char);
void sendPrivateCN(int, int);





#endif
