#ifndef LINKEDLIST_H
#define LINKEDLIST_H


#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "../utils/utils.h"
#include "struct.h"
#include "listutils.h"


LinkedList * linkedList();
int listGetSize(LinkedList *);
void addLast(LinkedList *, Node *);
void addFirst(LinkedList *, Node *);
void removeFirst(LinkedList *, void (*)(void *));
void removeLast(LinkedList *, void (*)(void *));
int removeItem(LinkedList *, Node *, void (*)(void *), int (*)(const void *, const void *));
void clearList(LinkedList *, void (*)(void *));
void printList(const LinkedList *, void (*)(void *));


#endif