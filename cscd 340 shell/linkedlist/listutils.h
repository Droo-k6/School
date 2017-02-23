#ifndef LISTUTILS_H
#define LISTUTILS_H


#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "../utils/utils.h"
#include "struct.h"
#include "linkedlist.h"


Node * buildNode(FILE *, void *(*)(FILE *));
Node * buildNode_Type(void *);
Node * buildNode_String(char *);
void cleanNode(Node *,void(*)(void *));
void cleanPrimitiveData(void *);
void sort(LinkedList *, int (*)(const void *, const void *));
void insertSorted(LinkedList *, Node *, int (*)(const void *, const void *));
void buildListTotal(LinkedList *, int, FILE *, void *(*)(FILE *));


#endif