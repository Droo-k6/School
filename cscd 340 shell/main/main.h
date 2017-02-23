#ifndef MAIN_H
#define MAIN_H


#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>

#include "../utils/utils.h"
#include "../linkedlist/struct.h"
#include "../linkedlist/listutils.h"
#include "../linkedlist/linkedlist.h"
#include "../history/history.h"
#include "../process/process.h"
#include "../pipes/pipes.h"
#include "../redirection/redirection.h"


void runCommand(AllHistory *,LinkedList *,char *);
void setDefaultPath(LinkedList *);
void getPathEnv(LinkedList *);
void buildPath(char ***, LinkedList *);
void cleanPath(char **);
int commandCD(char *);
void homeExpansion(char *);


#endif
