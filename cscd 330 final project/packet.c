#include "packet.h"

// Create packet
unsigned char * packetCreate(int command, char *stuff) {
	if ((command <= 0)||(command > PACKET_MAX_COMMANDS)) {
		printf("packetCreate, invalid command value\n");
		return NULL;
	}
	return packetCreateFinal((unsigned char) command, (unsigned char *) stuff);
}

// Create final packet
unsigned char * packetCreateFinal(unsigned char command, unsigned char *stuffArr) {
	// calculate length/size
	int length = PACKET_SIZE_MIN, sLen = 0;
	if (stuffArr != NULL) {
		sLen = (int) strlen((char *) stuffArr);
		length += sLen;
	}
	
	if (length > PACKET_MAX_LENGTH) {
		printf("packetCreate, length higher than max\n");
		return NULL;
	}
	
	// allocate
	unsigned char * finalArr = packetAllocate(length);
	
	// build final packet
	
	// Convert length
	// Todo: if length uses more than 1 byte, convert to network order
	unsigned char chLen = (unsigned char) length;
	finalArr[0] = chLen;
	
	// Command doesn't need converting
	finalArr[1] = command;
	
	// Add stuff and ending sequence
	if (stuffArr == NULL) {
		strncpy(finalArr+2, PACKET_ENDSEQ, PACKET_SIZE_END);
	}else{
		strncpy((char *)(finalArr+2), (char *) stuffArr, sLen);
		strncpy((char *)(finalArr+2+sLen), PACKET_ENDSEQ, PACKET_SIZE_END);
	}
	
	return finalArr;
}

// Allocate unsigned char array
// length does not include null terminator
// can be used to allocate unsigned char array, and zero it
unsigned char * packetAllocate(int len) {
	// Allocate
	unsigned char *arr = (unsigned char *)calloc(len+1, sizeof(unsigned char));
	zeroA((char *) arr, len+1);
/*	// Zero out
	int i = 0;
	for (; i < (len+1); i++) {
		arr[i] = '\0';
	}*/
	return arr;
}

// Get length of unsigned char array (not including terminator)
// Needs to be terminated w/ 0 ('\0')
// bit redundant, can just use strlen with char * cast
/*int usChArLen(unsigned char *arr) {
	int i = 0, unsigned char c;
	do {
		c = arr[i++];
	} while (c != '\0');
	return (i-1);
}*/

// Check if valid packet
int packetValid(unsigned char *arr) {
	// get length, check if under minimum
	int trueLen = (int) strlen((char *) arr);
	if (trueLen < PACKET_SIZE_MIN) {
		printf("packetValid, true length less than minimum\n");
		return 0;
	}
	
	// pull potential length
	int len = (int) arr[0];
	
	if (len != trueLen) {
		printf("packetValid, true length does not match\n");
		return 0;
	}
	
	// Check for ending sequence
	char * seqStr = strstr((char *) arr, PACKET_ENDSEQ);
	if (seqStr == NULL) {
		printf("packetValid, ending sequence not found\n");
		return 0;
	}
	
	return 1;
}

// create quit packet
unsigned char * packetCreateQuit() {
	return packetCreate(PACKET_CMD_QUIT_C, NULL);
}

// create quit all packet
unsigned char * packetCreateQuitAll() {
	return packetCreate(PACKET_CMD_QUIT_S, NULL);
}

// create display message packet
unsigned char * packetCreateMsgD(char *str) {
	return packetCreate(PACKET_CMD_MSG_D, str);
}

// create global message packet
unsigned char * packetCreateMsgG(char *str) {
	return packetCreate(PACKET_CMD_MSG_G, str);
}

// create channel message packet
unsigned char * packetCreateMsgC(char *str) {
	return packetCreate(PACKET_CMD_MSG_C, str);
}

// create private message packet
unsigned char * packetCreateMsgP(char *str) {
	return packetCreate(PACKET_CMD_MSG_P, str);
}

// create client list request packet
unsigned char * packetCreateLClR() {
	return packetCreate(PACKET_CMD_CL, NULL);
}

// create client list packet
unsigned char * packetCreateLCl(char *str) {
	return packetCreate(PACKET_CMD_CL, str);
}

// create channel list request packet
unsigned char * packetCreateLChR() {
	return packetCreate(PACKET_CMD_CHL, NULL);
}

// create channel list packet
unsigned char * packetCreateLCh(char *str) {
	return packetCreate(PACKET_CMD_CHL, str);
}

// create create channel request packet
unsigned char * packetCreateNewChR(char c) {
	char str[2];
	str[0] = c;
	str[1] = '\0';
	
	return packetCreate(PACKET_CMD_CH_CREATE, str);
}

// create join channel request packet
unsigned char * packetCreateJoinChR(char c) {
	char str[2];
	str[0] = c;
	str[1] = '\0';
	
	return packetCreate(PACKET_CMD_CH_JOIN, str);
}

// create leave channel request packet
unsigned char * packetCreateLeaveCh() {
	return packetCreate(PACKET_CMD_CH_LEAVE, NULL);
}

// create queue pos packet
unsigned char * packetCreateCurQPos(int pos) {
	char str[3];
	zeroA(str, 3);
	sprintf(str, "%d", pos);
	
	return packetCreate(PACKET_CMD_QP, str);
}

// create client number packet
unsigned char * packetCreateCurCN(int cn) {
	char str[3];
	zeroA(str, 3);
	sprintf(str, "%d", cn);
	
	return packetCreate(PACKET_CMD_CN, str);
}

// create current channel packet
unsigned char * packetCreateCurCh(char ch) {
	char str[2];
	zeroA(str, 2);
	str[0] = ch;
	
	return packetCreate(PACKET_CMD_CH, str);
}

// create private set packet
// cn shouldn't be more than 99, so only 2 bytes needed
unsigned char * packetCreateSetPr(int cn) {
	char str[3];
	zeroA(str, 3);
	sprintf(str, "%d", cn);
	
	return packetCreate(PACKET_CMD_PRVT, str);
}

// create server notification packet
unsigned char * packetCreateNoti(char *str) {
	return packetCreate(PACKET_CMD_NOTI, str);
}











