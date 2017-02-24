#ifndef CHANNEL_H
#define CHANNEL_H


// Includes
#include <stdlib.h>
#include <stdio.h>

#include "stringMisc.h"
#include "structs.h"
#include "client.h"


// Defines
#define CHANNEL_MAX_CLIENTS 3


// structs/typedef

// client prototype, in "client.h"
struct client;

// channel structure
// name - symbol of channel
// clientCount - number of clients in channel
// clientArr - arr of client pointers
/*
typedef struct {
	char name;
	int clientCount;
	struct client ** clientArr;
} channel;*/


// Functions prototypes
channel ** buildChannelArray(int size);
channel * buildChannel(char);
void setSym(channel *, char);
void setCCnt(channel *, int);
char getSym(channel *);
int getCCnt(channel *);
struct client ** getClientArr(channel *);
void refreshChannel(channel *);
channel * channelExists(channel **, int, char);
channel * createAddChannel(channel **, int *, int, char);
int removeChannel(channel **, int, int *, channel *);
int addClientToChannel(channel *, struct client *);
void removeClientFromCh(channel *, struct client *);
void freeChannelArrFull(channel **, int);
void freeChannelArr(channel **);
void freeChannel(channel *);


#endif
