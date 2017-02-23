#ifndef PACKETQUEUE_H
#define PACKETQUEUE_H

/*
Linked List Queue for packets
Used with packet buffer, for getting all whole packets before shifting

Used in client & server program
*/

// Includes
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "stringMisc.h"
#include "structs.h"
#include "packet.h"


// Structures
// Node for linked list
// Stores pointer for the unsigned char array (doesn't allocate)
//typedef struct pqNode pqNode;
/*
typedef struct {
	unsigned char *packetArr;
	struct pqNode *next;
} pqNode;
*/
// Queue
//typedef struct packetQueue packetQueue;
/*
typedef struct {
	int size;
	struct pqNode *head, *tail;
} packetQueue;
*/

// Functions
packetQueue * pqCreate();
pqNode * pqNodeCreate(unsigned char *);
int pqSize(packetQueue *);
void pqEnqueue(packetQueue *, unsigned char *);
unsigned char * pqDequeue(packetQueue *);
void freePQ(packetQueue *);


#endif
