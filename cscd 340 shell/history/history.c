#include "history.h"

AllHistory *allHistory() {
	AllHistory *allhistory = (AllHistory *)calloc(1,sizeof(AllHistory));
	allhistory->size = 100;
	allhistory->filesize = 1000;
	allhistory->list = linkedList();
	allhistory->start = NULL;
	allhistory->tail = NULL;
	return allhistory;
}

void cleanAllHistory(AllHistory *allhistory) {
	clearList(allhistory->list,cleanType_History);
	free(allhistory->list);
	allhistory->list = NULL;
	
	allhistory->start = NULL;
	allhistory->tail = NULL;
	
	free(allhistory);
}

// dumps history list to history file ".mssh_history"
void dumpHistory(AllHistory *allhistory) {
	FILE *fout = fopen(".mssh_history","w");
	
	// make sure list size matches
	trimHistoryExt(allhistory,(allhistory->filesize));

	// iterate through history LinkedList
	Node *trav = allhistory->list->head->next;
	history *hist = NULL;
	while (trav != NULL) {
		// pull string
		hist = (history *) (trav->data);
		fprintf(fout,"%s\n",(hist->str));
		
		trav=trav->next;
	}

	fclose(fout);
}

void printType_History(void * pi) {
	// check arguments
	if (pi == NULL) {
		printf("printType_History: null argument passed\n");
		exit(-99);
	}

	history *hist = (history *) pi;
	int index = hist->index;
	char *str = hist->str;

	// assuming largest will be 5 digits, else would need the max value passed down this far
	int index_width = 5;
	printf("%*d  %s\n",5,index,str);
}

void * buildType_History(char * str,int index) {
	// check arguments
	if (str == NULL) {
		printf("buildType_History: string is null\n");
		exit(-99);
	}

	history *hist = (history *)calloc(1,sizeof(history));
	hist->index = index;

	// create copy of original string
	hist->str = (char *)calloc(strlen(str)+1,sizeof(char));
	strcpy(hist->str,str);

	return ((void *)hist);
}

void cleanType_History(void * ptr) {
	// check arguments
	if (ptr == NULL) {
		printf("cleanType_History: null argument passed\n");
		exit(-99);
	}
	
	history *hist = (history *)ptr;
	
	free(hist->str);
	hist->str = NULL;
}

int historyGetIndex(Node *n) {
	if (n == NULL) {
		fprintf(stderr,"historyGetIndex: null node\n");
		exit(-1);
	}
	if (n->data == NULL) {
		fprintf(stderr,"historyGetIndex: null node data\n");
		exit(-1);
	}

	history *hist = (history *) (n->data);
	int index = hist->index;
	return index;
}

void addHistory(AllHistory *allhistory, char *str) {
	// check if tail already has command
	if ((allhistory->tail) != NULL) {
		history *hist = (history *) ((allhistory->tail)->data);
		if (strcmp(str,(hist->str)) == 0){
			// just exit
			return;
		}
	}

	// determine index
	int index = 0;
	if ((allhistory->tail) != NULL) {
		index = historyGetIndex(allhistory->tail) + 1;
	}
	Node *newNode = buildNode_Type(buildType_History(str,index));
	if (allhistory->start == NULL) {
		allhistory->start = newNode;
	}
	allhistory->tail = newNode;

	// check that linkedList is within acceptable file size
	trimHistory(allhistory);

	// check if start/tail are within acceptable size
	int dif = historyGetIndex(allhistory->tail) - historyGetIndex(allhistory->start);
	if (dif > (allhistory->size)) {
		// move start node forward as needed
		for (; dif > (allhistory->size); --dif) {
			allhistory->start = (allhistory->start)->next;
		}
	}

	// could manually add after tail, would save some processing time
	addLast(allhistory->list, newNode);
}

// cleans history list to match size/filesize
void trimHistory(AllHistory *allhistory) {
	int size = allhistory->size;
	int filesize = allhistory->filesize;
	// get largest of the 2 sizes
	int largestSize = max(size,filesize);
	// call extended trim
	trimHistoryExt(allhistory,largestSize);
}

// extended trim history call, trims according to size
void trimHistoryExt(AllHistory *allhistory,int size) {
	Node *start = allhistory->start, *tail = allhistory->tail;
	// trim off nodes starting from the beginning
	while (listGetSize(allhistory->list) > size) {
		// grab node to be removed
		Node *remove = allhistory->list->head->next;
		// check if start node is first node
		if (start == remove) {
			// move start forward
			allhistory->start = (allhistory->start)->next;
			start = allhistory->start;
		}
		// check if tail node is first node
		if (tail == remove) {
			// set tail null
			allhistory->tail = NULL;
			tail = NULL;
		}
		removeFirst(allhistory->list,cleanType_History);
	}
}

int displayHistory(AllHistory *allhistory, char *str) {
	if (strcmp(str,"history") != 0) {
		return 0;
	}
	
	// cant use LinkedList print, have to start at given node
	Node *trav = allhistory->start;
	while (trav != NULL) {
		printType_History(trav->data);
		trav = trav->next;
	}
	
	return 1;
}

// get last index in history
int getLastIndex(AllHistory *allhistory) {
	int index = -1;
	
	Node *last = allhistory->tail;
	if (last != NULL) {
		history *hist = (history *) (last->data);
		index = hist->index;
	}
	
	return index;
}

// get the history string from given index
char *getHistoryIndex(AllHistory *allhistory, int index) {
	Node *trav = allhistory->list->head->next;
	while (trav != NULL) {
		history *hst = (history *) (trav->data);
		if (hst->index == index) {
			return (hst->str);
		}
		trav=trav->next;
	}
	return NULL;
}

// replace history event if available
char *historyReplace(AllHistory *allhistory, char *str) {
	if (str[0] == '!') {
		// get value from string
		int index = atoi(str+1);
		
		char *replacement = NULL;
		int lastIndex = getLastIndex(allhistory);
		if (lastIndex >= 0) {
			// for negative index values
			if (index < 0) {
				index += lastIndex;
			}
			
			// check if index is within range
			if (
				(index >= 0)&&
				(index <= lastIndex)&&
				(index >= (lastIndex - allhistory->size))
				) {
				replacement = getHistoryIndex(allhistory,index);
			}
		}
		return replacement;
	}else if (strcmp(str,"!!") == 0) {
		char *replacement = NULL;
		int lastIndex = getLastIndex(allhistory);
		if (lastIndex >= 0) {
			replacement = getHistoryIndex(allhistory,lastIndex);
		}
		return replacement;
	}
	return NULL;
}

// check if given command is a valid history event call
int validHistoryEvent(char *str) {
	return ((str[0] == '!')||(strcmp(str,"!!")==0));
}

// find/replace history event (!#/!!)
void historyReplacement(AllHistory *allhistory, char *str) {
	// create a LinkedList to keep track of tokens
	LinkedList *tokens = linkedList();
	
	// tokenize by spaces, replace any tokens that match an alias key
	char temp[MAX],*tok,*tempPtr,*replace;
	strcpy(temp,str);
	
	// first check if starts with alias, if so-ignore
	tok = strtok_r(temp," ",&tempPtr);
	while (tok != NULL) {
		// find/replace
		if (validHistoryEvent(tok)) {
			replace = historyReplace(allhistory,tok);
			if (replace == NULL) {
				fprintf(stderr,"mssh: %s: event not found\n",tok);
			}else{
				tok = replace;
			}
		}
		addLast(tokens,buildNode_String(tok));

		// get next token
		tok = strtok_r(NULL," ",&tempPtr);
	}

	// build new string
	memset(str,'\0',sizeof(char)*MAX);
	Node *trav = tokens->head->next;
	while (trav != NULL) {
		tok = (char *)(trav->data);
		strcat(str,tok);
		if (trav->next != NULL) {
			strcat(str," ");
		}
		trav=trav->next;
	}
	
	// clean list
	clearList(tokens,cleanPrimitiveData);
	free(tokens);
	tokens = NULL;
}


