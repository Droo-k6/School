#ifndef ALIAS_H
#define ALIAS_H


#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

#include "../utils/utils.h"
#include "../linkedlist/struct.h"
#include "../linkedlist/listutils.h"
#include "../linkedlist/linkedlist.h"


// alias struct
typedef struct {
	char *key,*value;
} Alias;


Alias *alias(char *,char *);
void printAlias(void *);
void printNoAlias(char *,char *);
void cleanAlias(void *);
int compareAlias(const void *, const void *);
int containsAlias(char *);
int parseAlias(LinkedList *,char *);
void *findAlias(LinkedList *, char *);
char *aliasFindValue(LinkedList *, char *);
void findPrintAlias(LinkedList *, char *);
void updateAlias(LinkedList *,char *,char *);
void removeAlias(LinkedList *,char *);
void aliasReplacement(LinkedList *, char *);


#endif
