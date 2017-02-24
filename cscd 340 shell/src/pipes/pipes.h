#ifndef PIPES_H
#define PIPES_H


#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/wait.h>
#include <sys/types.h>
#include <errno.h>

#include "../utils/utils.h"
#include "../tokenize/makeargs.h"


int containsPipe(char *);
char ** parsePrePipe(char *, int *);
char ** parsePostPipe(char *, int *);
void pipeIt(char **,char **);
int pipeCommands(char *);


#endif 
