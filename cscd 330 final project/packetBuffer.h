#ifndef PACKETBUFFER_H
#define PACKETBUFFER_H

/*
Packet buffer, received contents dumped to buffer
*/

// Includes
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "stringMisc.h"
#include "packet.h"
#include "packetQueue.h"

// Defines
//#define PB_MAX (PACKET_MAX_LENGTH*20)

// Functions
void bufferAdd(unsigned char *, int, char *);
void bufferAddQueue(packetQueue *, unsigned char *, int, char *);
void bufferProcess(unsigned char *, int, packetQueue *);
void bufferShift(unsigned char *, int, int);

#endif
