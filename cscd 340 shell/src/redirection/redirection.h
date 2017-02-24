#ifndef REDIRECTION_H
#define REDIRECTION_H


#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/wait.h>
#include <sys/types.h>
#include <errno.h>

#include "../utils/utils.h"
#include "../tokenize/makeargs.h"


int containsRedirection(char *);
int getRedirection(char *);
int runRedirection(char *);
void directTo(int,int,int,char *,char *);
void directFrom(char *,char *);


#endif 
