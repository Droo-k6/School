#include "linkedlist.h"


LinkedList * linkedList() {
	LinkedList *list = (LinkedList *)calloc(1,sizeof(LinkedList));
	list->size = 0;
	// create dummy head
	Node *dummy = buildNode_Type(NULL);
	list->head = dummy;
	return list;
}

int listGetSize(LinkedList *list) {
	if (list == NULL) {
		fprintf(stderr,"listGetSize: list null\n");
	}
	return (list->size);
}

void addLast(LinkedList * theList, Node * nn) {
	// check for null arguments
	if (theList == NULL) {
		printf("addLast: theList NULL");
		exit(-99);
	}
	if (nn == NULL) {
		printf("addLast: nn NULL");
		exit(-99);
	}
	// check dummy head
	if (theList->head == NULL) {
		theList->head = buildNode_Type(NULL);
	}
	// add to end of list
	Node *trav = theList->head;
	while ((trav->next) != NULL) {
		trav=trav->next;
	}
	trav->next = nn;
	nn->prev=trav;
	
	++(theList->size);
}

void addFirst(LinkedList * theList, Node * nn) {
	// check for null arguments
	if (theList == NULL) {
		printf("addFirst: theList NULL");
		exit(-99);
	}
	if (nn == NULL) {
		printf("addFirst: nn NULL");
		exit(-99);
	}
	// check dummy head
	if (theList->head == NULL) {
		theList->head = buildNode_Type(NULL);
	}

	// add to start of list
	nn->prev = theList->head;
	nn->next = theList->head->next;
	if (theList->head->next != NULL) {
		theList->head->next->prev=nn;
	}
	theList->head->next = nn;

	++(theList->size);
}

void removeFirst(LinkedList * theList, void (*removeData)(void *)) {
	// check for null arguments
	if (theList == NULL) {
		printf("removeFirst: theList NULL");
		exit(-99);
	}
	// check dummy head
	if (theList->head == NULL) {
		theList->head = buildNode_Type(NULL);
	}
	// check size
	if (theList->size <= 0) {
		// just return
		return;
	}
	
	// remove first
	Node *rem = theList->head->next;
	theList->head->next = rem->next;
	theList->head->next->prev = theList->head;
	
	// free up node
	cleanNode(rem,removeData);
	free(rem);
	
	--(theList->size);
}

void removeLast(LinkedList * theList, void (*removeData)(void *)) {
	if (theList == NULL) {
		printf("removeLast: theList NULL");
		exit(-99);
	}
	// check if anything to remove
	if (theList->size <= 0) {
		// just skip
		return;
	}

	Node *trav = theList->head;
	while(trav->next != NULL) {
		trav=trav->next;
	}
	// if null, empty list (something wrong)
	if (trav != NULL) {
		// destroy links
		trav->prev->next = NULL;
		// free up node
		cleanNode(trav,removeData);
		free(trav);
		trav = NULL;
		--(theList->size);
	}
}

int removeItem(LinkedList * theList, Node * nn, void (*removeData)(void *), int (*compare)(const void *, const void *)) {
	// check for null arguments
	if (theList == NULL) {
		printf("removeItem: theList NULL");
		exit(-99);
	}
	if (nn == NULL) {
		printf("removeItem: nn NULL");
		exit(-99);
	}
	// check dummy head
	if (theList->head == NULL) {
		theList->head = buildNode_Type(NULL);
	}
	
	int found = 0;

	// check size
	if (theList->size > 0) {
		// find the node
		Node *trav = theList->head->next;
		while (trav != NULL) {
			// compare data
			if (compare(trav->data,nn->data) == 0) {
				break;
			}
			trav=trav->next;
		}
		// if trav is NULL, wasn't found
		if (trav != NULL) {
			// remove from list
			if (trav->prev != NULL) {
				trav->prev->next = trav->next;
			}
			if (trav->next != NULL) {
				trav->next->prev = trav->prev;
			}
		
			// free up
			cleanNode(trav,removeData);
			free(trav);
		
			--(theList->size);
			found = 1;
		}
	}
	cleanNode(nn,removeData);
	free(nn);

	return found;
}

void clearList(LinkedList * theList, void (*removeData)(void *)) {
	// check for null arguments
	if (theList == NULL) {
		// checked already, just return
		return;
	}
	// check for null head
	if (theList->head == NULL) {
		// set size to 0, return
		theList->size = 0;
		return;
	}
	
	// remove/free elements
	Node *trav = theList->head->next;
	while (trav != NULL) {
		theList->head->next = trav->next;
		
		cleanNode(trav,removeData);
		free(trav);
		
		trav=theList->head->next;
	}
	
	// free dummy head, if list is used again - other functions are built to remake it
	free(theList->head);
	theList->head = NULL;
	
	// set size
	theList->size = 0;
}

void printList(const LinkedList * theList, void (*convertData)(void *)) {
	// check for null arguments or empty list
	if ((theList == NULL) || (theList->size <= 0)) {
		printf("Empty List\n");
		return;
	}
	
	// print out list
	int i = 0;
	Node *trav = theList->head->next;
	while (trav != NULL) {
		printf("%d: ",++i);
		convertData(trav->data);
		trav=trav->next;
	}
	
	printf("\n");
}

