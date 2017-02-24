#ifndef HISTORY_H
#define HISTORY_H


#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "../utils/utils.h"
#include "../linkedlist/struct.h"
#include "../linkedlist/linkedlist.h"
#include "../linkedlist/listutils.h"
#include "../tokenize/makeargs.h"


// to store all the history information (LL,start/tail,sizes)
typedef struct {
	int size,filesize;
	LinkedList *list;
	Node *start,*tail;
} AllHistory;

// for history objects in a LinkedList
struct hist {
	int index;
	char *str;
};
typedef struct hist history;

AllHistory *allHistory();
void cleanAllHistory(AllHistory *);
void dumpHistory(AllHistory *);
void printType_History(void *);
void * buildType_History(char *,int);
void cleanType_History(void *);
void addHistory(AllHistory *, char *);
void trimHistory(AllHistory *);
void trimHistoryExt(AllHistory *,int);
int displayHistory(AllHistory *,char *);
int getLastIndex(AllHistory *);
char *getHistoryIndex(AllHistory *, int);
char *historyReplace(AllHistory *, char *);
void historyReplacement(AllHistory *, char *);


#endif
