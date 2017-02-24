#include "listutils.h"


Node * buildNode(FILE * fin, void *(*buildData)(FILE * in) ) {
	Node *node = (Node *)calloc(1,sizeof(Node));
	node->data = buildData(fin);
	node->next = NULL;
	node->prev = NULL;
	return node;
}

Node * buildNode_Type(void * passedIn) {
	Node *n = (Node *)calloc(1,sizeof(Node));
	n->data = passedIn;
	n->prev = NULL;
	n->next = NULL;
	return n;
}

Node * buildNode_String(char *str) {
	// create copy of string
	char *nStr = (char *)calloc(strlen(str)+1,sizeof(char));
	strcpy(nStr,str);
	// create node, return
	return buildNode_Type((void *) nStr);
}

void cleanNode(Node *n,void(*cleanData)(void *)) {
	if (n->data != NULL) {
		cleanData(n->data);
		free(n->data);
		n->data = NULL;
	}
	n->prev = NULL;
	n->next = NULL;
}

// cleaner function for when data is a pointer to a primitive
// just a dumby reall, does nothing
void cleanPrimitiveData(void *data) {}

void buildListTotal(LinkedList * myList, int total, FILE * fin, void * (*buildData)(FILE * in)) {
	// check arguments
	if (myList == NULL) {
		printf("buildListTotal: myList NULL\n");
		exit(-99);
	}
	if (total <= 0) {
		printf("buildListTotal: total <= 0\n");
		exit(-99);
	}
	
	// loop through
	int i = 0;
	for (; i < total; ++i) {
		Node *add = buildNode(fin,buildData);
		addFirst(myList,add);
	}
}

// sorting functions
void sort(LinkedList * theList, int (*compare)(const void *, const void *)) {
	// check arguments
	if (theList == NULL) {
		printf("sort: theList NULL");
		exit(-99);
	}
	if (theList->size <= 1) {
		// just return, dont sort
		return;
	}
	
	// create copy list
	LinkedList *dummyList = linkedList();
	// just move over head node
	dummyList->head->next = theList->head->next;
	theList->head->next->prev = dummyList->head;
	theList->head->next = NULL;
	theList->size = 0;
	
	// begin insertion sort
	Node *temp = NULL;
	while ((dummyList->head->next) != NULL) {
		// link is lost in insertSort, create reference to
		temp = dummyList->head->next;
		dummyList->head->next = temp->next;

		insertSorted(theList,temp,compare);
	}
	
	// freeup dummy
	free(dummyList->head);
	free(dummyList);
}

// insert into list sorted
void insertSorted(LinkedList *list, Node *nn, int (*compare)(const void *, const void *)) {
	// check arguments
	if (list == NULL) {
		printf("insertSorted: list NULL");
		exit(-99);
	}
	if (nn == NULL) {
		printf("insertSorted: nn NULL");
		exit(-99);
	}
	
	// find a next node that has a higher compare value, then insert
	Node *trav = list->head;
	while (trav->next != NULL) {
		// for alphabetical, < 0 (ascending), > 0 (descending)
		if (compare(nn->data,trav->next->data) < 0) {
			nn->prev = trav;
			nn->next = trav->next;
			trav->next->prev = nn;
			trav->next = nn;
			break;
		}
		trav = trav->next;
	}
	// if NULL, no location - add to end
	if (trav->next == NULL) {
		trav->next = nn;
		nn->prev = trav;
		nn->next = NULL;
	}
	++(list->size);
}

