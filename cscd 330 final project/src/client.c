#include "client.h"

// Builds full array, contains allocated client objects (for master array)
client ** buildClientArrayFull(int size) {
	client **arr = (client **)calloc(size, sizeof(client *));
	int i = 0;
	for (; i < size; i++) {
		arr[i] = buildClient();
	}
	return arr;
}

// Builds array of client pointers
client ** buildClientArray(int size) {
	client **arr = (client **)calloc(size, sizeof(client *));
	int i = 0;
	for (; i < size; i++) {
		arr[i] = NULL;
	}
	return arr;
}

// Create client
client * buildClient() {
	client * c = (client *)calloc(1, sizeof(client));
	setFd(c, -1);
	setCNum(c, -1);
	setQPos(c, -1);
	setCCh(c, NULL);
	
	zeroA((char *) (c->buf), PB_MAX+1);
	c->pq = pqCreate();
	
	return c;
}

// Resets client
// Moved to server.c so message updates can be done
void resetClient(client *c) {
	setFd(c, -1);
	setCNum(c, -1);
	setQPos(c, -1);
	setCCh(c, NULL);
	
	// Reset packet buffer
	zeroA((char *) (c->buf), PB_MAX+1);
	
	// Reset packet queue
	if (getPQ(c) != NULL) {
		freePQ(getPQ(c));
	}
	c->pq = pqCreate();
}

// set fd
void setFd(client *c, int fd) {
	c->fd = fd;
}

// set client number
void setCNum(client *c, int cNum) {
	c->clientNumber = cNum;
} 

// set queue position
void setQPos(client *c, int qPos) {
	c->qPos = qPos;
}

// set channel
void setCCh(client *c, struct channel *ch) {
	c->curChan = ch;
}

// get fd
int getFd(client *c) {
	return (c->fd);
}

// get client number
int getCNum(client *c) {
	return (c->clientNumber);
}

// get queue position
int getQPos(client *c) {
	return (c->qPos);
}

// get channel
struct channel * getCh(client *c) {
	return (c->curChan);
}

// get buffer
unsigned char * getBuf(client *c) {
	return (c->buf);
}

// get packet queue
packetQueue * getPQ(client *c) {
	return (c->pq);
}

// create and add client to client array
// useless
client * addCreateClient(client **arr, int size, int fd) {
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] == NULL) {
			break;
		}
	}
	// No slot
	if (i == size) {
		return NULL;
	}
	arr[i] = buildClient(fd, -1, -1, NULL);
	return arr[i];
}

// gets client from array
client * getClientFromArr(client **arr, int size, int fd) {
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			if (getFd(arr[i]) == fd) {
				return arr[i];
			}
		}
	}
	return NULL;
}

// get client from client array w/ given client number
client * getClient(client **arr, int size, int cn) {
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			if (getFd(arr[i]) != -1) {
				if (getCNum(arr[i]) == cn) {
					return arr[i];
				}
			}
		}
	}
	return NULL;
}

// free client array and members
void freeClientArrFull(client ** arr, int size) {
	if (arr == NULL) {
		printf("freeArr error, arr is null\n");
		return;
	}
	int i = 0;
	for(; i < size; i++) {
	//	printf("freeCArrFull: i-%d\n", i);
		if (arr[i] != NULL) {
			freeClient(arr[i]);
			free(arr[i]);
			arr[i] = NULL;
		}
	}
	free(arr);
}

// just free array, array should have nothing allocated on an index (just pointers)
void freeClientArr(client **arr) {
	if (arr != NULL) {
		free(arr);
	}else{
		printf("freeClientArr error, arr is null\n");
	}
}

// free client
void freeClient(client *c) {
	if (getFd(c) > 0) {
		close(getFd(c));
	}
	c->curChan = NULL;
	if (c->pq != NULL) {
		freePQ(c->pq);
	}
}
