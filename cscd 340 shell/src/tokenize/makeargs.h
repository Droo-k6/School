#ifndef MAKEARGS_H
#define MAKEARGS_H


#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "../utils/utils.h"


void clean(int, char **);
void printargs(int, char **);
int makeargs(char *, char *** );


#endif
