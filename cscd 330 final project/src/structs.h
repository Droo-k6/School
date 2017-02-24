#ifndef STRUCTS_H
#define STRUCTS_H

/*
	Moving all structure defines to one file
	just to simplify
	some defines moved over as well
*/

// Defines

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

// 
#define PB_MAX (PACKET_MAX_LENGTH*20)



// Structures

struct channel {
	char name;
	int clientCount;
	struct client ** clientArr;
};

struct pqNode {
	unsigned char *packetArr;
	struct pqNode *next;
};

struct packetQueue {
	int size;
	struct pqNode *head, *tail;
};

struct client {
	int fd, clientNumber, qPos;
	struct channel *curChan;
	
	unsigned char buf[PB_MAX+1];
	struct packetQueue *pq;
};

struct node {
	struct client *data;
	struct node *next;
};

struct queue {
	struct node *head, *tail;
	int size, changed;
};

typedef struct pqNode pqNode;
typedef struct packetQueue packetQueue;
typedef struct node node;
typedef struct queue queue;
typedef struct channel channel;
typedef struct client client;

#endif
