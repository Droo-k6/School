#ifndef FILES_H
#define FILES_H


#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "../utils/utils.h"
#include "../linkedlist/struct.h"
#include "../linkedlist/listutils.h"
#include "../linkedlist/linkedlist.h"
#include "../history/history.h"
#include "../main/main.h"


void parseRC(int *,int *,LinkedList *,char***);
void parseHistory(AllHistory *);
LinkedList *read_file(char *);


#endif
