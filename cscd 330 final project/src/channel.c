#include "channel.h"

// create array of channel pointers (actually 2d pointer)
channel ** buildChannelArray(int size) {
	channel **arr = (channel **)calloc(size, sizeof(channel *));
	int i = 0;
	for (; i < size; i++) {
		arr[i] = NULL;
	}
	return arr;
}

// create channel
channel * buildChannel(char sym) {
	channel * c = (channel *)calloc(1, sizeof(channel));
	setSym(c, sym);
	setCCnt(c, 0);
	c->clientArr = buildClientArray(CHANNEL_MAX_CLIENTS);
	return c;
}

// set symbol
void setSym(channel *c, char sym) {
	c->name = sym;
}

// set client count
void setCCnt(channel *c, int count) {
	c->clientCount = count;
}

// get symbol
char getSym(channel *c) {
	return (c->name);
}

// get client count
int getCCnt(channel *c) {
	return (c->clientCount);
}

// get client array
client ** getClientArr(channel *c) {
	return (c->clientArr);
}

// check channels client array, set client count accordingly
// checks channels clients as well, if mismatch - throw error and correct
// actually may not be needed...
void refreshChannel(channel *chObj) {
	client ** arr = getClientArr(chObj);
	
	int cnt = 0, i = 0;
	for (; i < CHANNEL_MAX_CLIENTS; i++) {
		if (arr[i] != NULL) {
			cnt++;
			if (getCh(arr[i]) != chObj) {
				printf("refreshChannel, correcting clients channel\n");
				setCCh(arr[i], chObj);
			}
		}
	}
	
	setCCnt(chObj, cnt);
}

// check if channel exists in master array
// return channel *
channel * channelExists(channel **chArr, int size, char sym) {
	int i = 0;
	for (; i < size; i++) {
		if (chArr[i] != NULL) {
			if (getSym(chArr[i]) == sym) {
				return chArr[i];
			}
		}
	}
	return NULL;
}

// creates channel w/ given symbol on array, if can
// assumes valid symbol
channel * createAddChannel(channel **arr, int *total, int size, char sym) {
	printf("createAddChannel: start total %d\n", *total);

	// check if space for channel
	if (*total >= size) {
		printf("createAddChannel: channel limit reached\n");
		return NULL;
	}
	// find open slot and check if channel w/ sym exists
	int i = 0, slot = -1;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			if (getSym(arr[i]) == sym) {
				printf("createAddChannel: channel exists\n");
				return NULL;
			}
		}else if (slot == -1) {
			slot = i;
		}
	}
	// check if slot available
	if (slot == -1) {
		printf("createAddChannel: no slot found\n");
		return NULL;
	}
	
	arr[slot] = buildChannel(sym);
	(*total)++;
	
	printf("createAddChannel: end total %d\n", *total);

	return (arr[slot]);
}

// Remove channel from array
// will force clients out as well
int removeChannel(channel **arr, int size, int *total, channel *ch) {
	// No channels
	if (*total <= 0) {
		printf("removeChannel error: total <= 0\n");
		return 0;
	}
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			if (arr[i] == ch) {
				break;
			}
		}
	}
	// Didn't find channel
	if (i == size) {
		printf("removeChannel error: channel not found");
		return 0;
	}
	// if clients still in, force out
	if (getCCnt(arr[i]) > 0) {
		client **cArr = getClientArr(ch);
		int j = 0;
		for (; j < CHANNEL_MAX_CLIENTS; j++) {
			if (cArr[j] != NULL) {
				setCCh(cArr[j], NULL);
				cArr[j] = NULL;
			}
		}
		setCCnt(ch, 0);
	}
	freeChannel(arr[i]);
	arr[i] = NULL;
	(*total)--;
	
	return 1;
}

// add C, returns 0/1 if addition successful
int addClientToChannel(channel *chObj, client *clientObj) {
	int cnt = getCCnt(chObj);
	if (cnt >= CHANNEL_MAX_CLIENTS) {
		// channel full
		return 0;
	}
	// find open spot in client array, add
	client ** arr = getClientArr(chObj);
	int i = 0;
	for (; i < CHANNEL_MAX_CLIENTS; i++) {
		if (arr[i] == NULL) {
			break;
		}
	}
	// no slot found
	if (i == CHANNEL_MAX_CLIENTS) {
		printf("addClientToChannel error, no open slots\n");
		return 0;
	}
	
	arr[i] = clientObj;
	setCCnt(chObj, cnt+1);
	setCCh(clientObj, chObj);
	
	return 1;
}

// remove client from channel
void removeClientFromCh(channel *chObj, client *clientObj) {
	if (chObj == NULL) {
		return;
	}
	
	int cnt = getCCnt(chObj);
	if (cnt <= 0) {
		printf("removeClientFromCh error: client count <= 0\n");
		// empty channel
		return;
	}
	client ** arr = getClientArr(chObj);
	int i = 0;
	for (; i < CHANNEL_MAX_CLIENTS; i++) {
		if (arr[i] == clientObj) {
			printf("removeClientFromCh: found client\n");
			setCCh(arr[i], NULL);
			setCCnt(chObj, cnt-1);
			arr[i] = NULL;
			return;
		}
	}
	// client not found
	printf("removeClientFromCh error: client not found\n");
}

// free channel pointer array + members
void freeChannelArrFull(channel **arr, int size) {
	if (arr == NULL) {
		printf("freeChannelArrFull error, arr is null\n");
		return;
	}
	int i = 0;
	for(; i < size; i++) {
		if (arr[i] != NULL) {
			freeChannel(arr[i]);
		//	free(arr[i]);
			arr[i] = NULL;
		}
	}
	free(arr);
}

// free channel pointer array
void freeChannelArr(channel **arr) {
	if (arr != NULL) {
		free(arr);
	}else{
		printf("freeChannelArr error, arr is null\n");
	}
}

// free channel pointer
void freeChannel(channel *c) {
	if (c != NULL) {
		free(getClientArr(c));
		free(c);
	}
}
