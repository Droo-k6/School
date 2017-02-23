#ifndef PARSEPACKET_H
#define PARSEPACKET_H

/*
functions for parsing received packets
used in both client and server program
*/

// Includes
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "stringMisc.h"
#include "packet.h"

// Defines

// return keys are in "packet.h"


// Functions
int parsePacket(unsigned char *);
void parsePacketStuff(unsigned char *);


#endif
