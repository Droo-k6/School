#include "queue.h"

// Create queue
queue * buildQueue() {
	queue *q = (queue *)calloc(1, sizeof(queue));
	q->head = NULL;
	q->tail = NULL;
	q->size = 0;
	setChanged(q, 0);
	return q;
}

// Create node
node * buildNode(client *c) {
	node * n = (node *)calloc(1, sizeof(node));
	n->data = c;
	n->next = NULL;
}

// set changed
void setChanged(queue *q, int changed) {
	q->changed = changed;
}

// get changed
int getChanged(queue *q) {
	return (q->changed);
}

// get size
int getQSize(queue *q) {
	return (q->size);
}

// add to queue, at end
void enqueue(queue *q, client *c) {
	node * n = buildNode(c);
	if (q->head == NULL) {
		q->head = n;
		q->tail = n;
		q->size++;
		setChanged(q, 1);
		setQPos(n->data, -1);
	}else{
		q->tail->next = n;
		q->tail = n;
		q->size++;
		setChanged(q, 1);
		setQPos(n->data, -1);
	}
}

// remove/return head
client * dequeue(queue *q) {
	if (q->head == NULL) {
		printf("dequeue error, head null\n");
		return NULL;
	}
	
	node *n = q->head;
	q->head = q->head->next;
	q->size--;
	setChanged(q, 1);
	client *c = n->data;
	setQPos(c, -1);
	free(n);
	
	return c;
}

// peak at head
client * peak(queue *q) {
	if (q->head == NULL) {
		printf("peak error, head null\n");
		return NULL;
	}
	return (q->head->data);
}

// remove given client
int removeNode(queue *q, client *c) {
	if (q->head == NULL) {
		return 0;
	}
	
	node *trav = q->head;
	// check if head node
	if (trav->data == c) {
		q->head = trav->next;
		free(trav);
		q->size--;
		setChanged(q, 1);
		return 1;
	}
	
	// loop to find
	int loop = 1;
	while ((trav->next != NULL)&&(loop)) {
		loop = (trav->next->data != c);
		if (!loop) {
			trav = trav->next;
		}
	}
	
	if (trav->next == NULL) {
		// didn't find client
		return 0;
	}
	
	// check if tail
	if (trav->next == q->tail) {
		free(trav->next);
		trav->next = NULL;
		q->tail = trav;
		q->size--;
		setChanged(q, 1);
		return 1;
	}else{
		node *rem = trav->next;
		trav->next = rem->next;
		free(rem);
		q->size--;
		setChanged(q, 1);
		return 1;
	}
}

// check if given client is in queue
int inQueue(queue *q, client *c) {
	if (q->head == NULL) {
		return 0;
	};
	node *trav = q->head;
	while(trav != NULL) {
		if (trav->data == c) {
			return 1;
		}else{
			trav = trav->next;
		}
	}
	return 0;
}

// free queue
void freeQueue(queue *q) {
	if (q == NULL) {
		printf("freeQueue error, q null\n");
		return;
	}
	node *trav = NULL;
	while ((q->head) != NULL) {
		trav = q->head->next;
		free(q->head);
		q->head = trav;
	}
	free(q);
}
