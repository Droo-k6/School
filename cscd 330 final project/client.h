#ifndef CLIENT_H
#define CLIENT_H


// Includes
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "stringMisc.h"
#include "structs.h"
#include "packetQueue.h"
#include "packetBuffer.h"
#include "channel.h"


// Defines
// Max size of private array
#define MAX_PARR 12


// structs/typedef

// channel prototype, in "channel.h"
struct channel;

// client structure
// fd - connection descriptor
// clientNumber - client #
// qPos - queue position
// curChan - pointer to channel client is in
// privateSend - pointer to client that current private is set to
// pArr - array of clients that have private set to this client
// buffer - buffer of packets
// pq - queue of whole packets (processed from buffer)
/*
struct client;
typedef struct {
	int fd, clientNumber, qPos;
	struct channel *curChan;
	
	struct client *pSend;
	struct client **pArr;
	
	unsigned char buf[PB_MAX+1];
	packetQueue *pq;
} client;*/


// Functions prototypes
client ** buildClientArrayFull(int);
client ** buildClientArray(int);
client * buildClient();
void resetClient(client *);
void setFd(client *, int);
void setCNum(client *, int);
void setQPos(client *, int);
void setCCh(client *, struct channel *);
void setPSend(client *, client *);
int getFd(client *);
int getCNum(client *);
int getQPos(client *);
struct channel * getCh(client *);
client * getPSend(client *);
client ** getPArr(client *);
unsigned char * getBuf(client *);
packetQueue * getPQ(client *);
client * addCreateClient(client **, int, int);
client * getClientFromArr(client **, int, int);
int removeClientFromArr(client **, int, int, int);
client * getClient(client **, int, int);
void freeClientArrFull(client **, int);
void freeClientArr(client **);
void freeClient(client *);


#endif
