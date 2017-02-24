#include "parsePacket.h"

// parse packet
int parsePacket(unsigned char *packet) {
	// validate packet
	if (!packetValid(packet)) {
		printf("parsePacket: packet invalid\n");
		return PACKET_CMD_INVALID;
	}
	
	// grab command, to int
	int cmd = (int) packet[1];
	// shift stuff
	parsePacketStuff(packet);
	
	// don't need to do anything
	// main loop will handle specifics
	// should probably use on both client/server
	
	return cmd;
}

// shift stuff value to beginning, zeroes out ending sequence
void parsePacketStuff(unsigned char *packet) {
	int len = (int) strlen((char *) (packet+2));
	
	int i = 0;
	for (; i < len; i++) {
		if ((len - i) <= PACKET_SIZE_END) {
			packet[i] = '\0';
		}else{
			packet[i] = packet[2+i];
		}
	}
}
