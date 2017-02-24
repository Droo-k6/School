#ifndef PROCESS_H
#define PROCESS_H


#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <errno.h>

#include "../utils/utils.h"


void forkIt(char **);
void processCommand(char *);

#endif
