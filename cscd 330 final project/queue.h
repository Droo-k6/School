#ifndef QUEUE_H
#define QUEUE_H


// Includes
#include <stdlib.h>
#include <stdio.h>
#include <sys/socket.h>

#include "stringMisc.h"
#include "structs.h"
#include "client.h"


// Struct/typedef

// Node structure
// cData - client pointer, to existing client
// next - to next node
//struct node;
/*
typedef struct {
	client *cData;
	struct node *next;
} node;
*/

// queue structure
// head - head of list
// tail - tail of list
// size - number of nodes
// changed - to be removed, was for refreshing the queue each iteration
//struct queue;
/*
typedef struct {
	node *head, *tail;
	int size, int changed;
} queue;
*/


// Function prototypes
queue * buildQueue();
node * buildNode(client *);
void setChanged(queue *, int);
int getChanged(queue *);
int getQSize(queue *);
void enqueue(queue *, client *);
client * dequeue(queue *);
client * peak(queue *);
int removeNode(queue *, client *);
int inQueue(queue *, client *);
int sendQUpdate(client *);
void freeQueue(queue *);


#endif
