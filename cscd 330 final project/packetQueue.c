#include "packetQueue.h"


// Create packetQueue
packetQueue * pqCreate() {
	packetQueue *pq = (packetQueue *)calloc(1, sizeof(packetQueue));
	pq->size = 0;
	pq->head = NULL;
	pq->tail = NULL;
	return pq;
}

// Create pqNode
pqNode * pqNodeCreate(unsigned char *arr) {
	pqNode *n = (pqNode *)calloc(1, sizeof(pqNode));
	n->packetArr = arr;
	n->next = NULL;
	return n;
}

// Get size
int pqSize(packetQueue *pq) {
	return (pq->size);
}

// enqueue
void pqEnqueue(packetQueue *pq, unsigned char *arr) {
	if (pq->head == NULL) {
		pq->head = pqNodeCreate(arr);
		pq->tail = pq->head;
	}else{
		pq->tail->next = pqNodeCreate(arr);
		pq->tail = pq->tail->next;
	}
	pq->size++;
}

// dequeue
unsigned char * pqDequeue(packetQueue *pq) {
	int size = pq->size;
	if (size == 0) {
		return NULL;
	}else if (size == 1) {
		pqNode *n = pq->head;
		unsigned char * arr = n->packetArr;
		free(n);
		pq->head = NULL;
		pq->tail = NULL;
		pq->size--;
		return arr;
	}else{
		pqNode *n = pq->head;
		unsigned char * arr = n->packetArr;
		pq->head = n->next;
		free(n);
		pq->size--;
		return arr;
	}
}

// Frees pq
void freePQ(packetQueue *pq) {
	pqNode *n = NULL;
	unsigned char *garbageStr = NULL;
	while (pq->head != NULL) {
		n = pq->head->next;
		garbageStr = (pq->head)->packetArr;
		if (garbageStr != NULL) {
			free(garbageStr);
		}
		free(pq->head);
		pq->head = n;
	}
/*	while (pq->size > 0) {
		garbageStr = pqDequeue(pq);
		free(garbageStr);
	}*/
}
