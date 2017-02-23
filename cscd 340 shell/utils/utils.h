#ifndef MYUTILS_H
#define MYUTILS_H


#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define MAX 100


void strip(char *);
void stripSize(char *,int);
int strContains(char *,char);
void pushChar(int,char *,char);
void addSlash(char *);
void shiftStr(char *, char *);
void clearStr(char *, int);
int max(int,int );


#endif
