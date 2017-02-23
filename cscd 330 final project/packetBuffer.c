#include "packetBuffer.h"

// Add contents onto end of buffer
void bufferAdd(unsigned char *pbuf, int size, char *buf) {
	int len = strlen((char *) pbuf);
	int lenAdd = strlen(buf);
	if (len+lenAdd > size) {
		printf("bufferAdd error, buffer will be full from addition - use queue variant, dumping buffer\n");
		zeroA((char *) pbuf, size+1);
	}else{
		strncpy((char *) (pbuf+len), buf, (size-len));
	}
}

// Add contents onto end of buffer, but processes for packets as well
// If there is not enough space on the buffer, it will just dump the buffer
void bufferAddQueue(packetQueue *pq, unsigned char *pbuf, int size, char *buf) {
	int reAdd = 0;
	
	// Length of packet buffer
	int len = (int) strlen((char *) pbuf);
	// Length of passed buffer
	int lenAdd = (int) strlen(buf);
	// Length to copy over
	int maxCopy = (size-len);
	
	if (len+lenAdd > size) {
		printf("bufferAddQueue, buffer not large enough, dumping buffer (increase buffer size)\n");
		zeroA((char *) pbuf, size+1);
	}
	
//	strncpy((char *) (pbuf+len), buf, (size-len));
	strncat((char *) pbuf, buf, maxCopy);
	
	// process for packets
	bufferProcess(pbuf, size, pq);
	
	// Would deal with readding values
/*	
	if (reAdd) {
		// get new length of buffer
		int nLen = (int) strlen((char *) pbuf);
		
		// get remaining on passed buffer
		int rLen = lenAdd - (size-len);
	//	int rLen = (int) strlen(buf+(size-len));
		
		if (nLen == size) {
			// still full, just dump
			printf("bufferAddQueue, buffer still full, dumping buffer\n");
			zeroA((char *) pbuf, size+1);
		}else if ((nLen + rLen) > size){
			// not empty enough, just dump
			printf("bufferAddQueue, buffer still too large, dumping buffer\n");
			zeroA((char *) pbuf, size+1);
		}else{
			strncat((char *) pbuf, (buf+(lenAdd-rLen)), (size-nLen));
			// process again
			bufferProcess(pbuf, size, pq);
		}
	}*/
	
}

// Check for ending sequence in buffer
// If found and valid, enqueue into given packet queue (if valid)
// Todo: change to loop to avoid having to keep shifting buffer
void bufferProcess(unsigned char *pbuf, int size, packetQueue *pq) {
	// Check for ending sequence
	unsigned char *endSq = (unsigned char *)strstr((char *) pbuf, PACKET_ENDSEQ);
	int shifti = 0;
	// Loop through until no more found
	while (endSq != NULL) {
		// move seq to "end" of str
		endSq += PACKET_SIZE_END;
		// get length between start and found seq
		int len = endSq - (pbuf+shifti);
		
		// allocate array
		unsigned char *packet = packetAllocate(len);
		strncpy((char *) packet, (char *) (pbuf+shifti), len);
		
		// if valid packet, enqueue
		if (packetValid(packet)) {
			pqEnqueue(pq, packet);
		}else{
			printf("bufferProcess, packet invalid\n");
		}
		
		// Check if another sequence found
		shifti += len;
		endSq = (unsigned char *)strstr((char *) (pbuf+shifti), PACKET_ENDSEQ);
	}
	
	// Shift buffer
	if (shifti > 0) {
//		printf("bufferProcess, shifting\n");
		bufferShift(pbuf, size, shifti);
	}
}

// Shift buffer contents to beginning
void bufferShift(unsigned char *pbuf, int size, int index) {
	// Just going to do loop
	/*	
	// Create copy array
	unsigned char *cbuf = packetAllocate(size);
	// Copy over
	strncpy((char *) cbuf, (char *) pbuf+index, size);
	// Clear buffer
	zeroA((char *) pbuf, size+1);
	// Copy back over
	strncpy((char *) pbuf, (char *) cbuf, size);
	free(cbuf);
	*/
	int len = strlen((char *) (pbuf+index));
	int i = 0;
	for (; i < size; i++) {
		if (i < len) {
			pbuf[i] = pbuf[index+i];
		}else{
			pbuf[i] = '\0';
		}
	}
}
