#ifndef PARSEINPUTC_H
#define PARSEINPUTC_H

/*
For parsing input on client program
*/

// Includes
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "stringMisc.h"


// Defines

// Commands
#define INPUT_CMD_HELP "/help"
#define INPUT_CMD_INFO "/info"
#define INPUT_CMD_QUIT "/quit"
#define INPUT_CMD_QUITALL "/quitall"
#define INPUT_CMD_LIST_CH "/channels"
#define INPUT_CMD_LIST_CL "/clients"
#define INPUT_CMD_CH_CREATE "/create"
#define INPUT_CMD_CH_JOIN "/join"
#define INPUT_CMD_CH_LEAVE "/leave"
#define INPUT_CMD_TYPE_GLOBAL "/g"
#define INPUT_CMD_TYPE_CHANNEL "/ch"

// Keys
#define INPUT_KEY_INVALID (-1)
#define INPUT_KEY_HELP (INPUT_KEY_INVALID+1)
#define INPUT_KEY_INFO (INPUT_KEY_HELP+1)
#define INPUT_KEY_QUIT (INPUT_KEY_INFO+1)
#define INPUT_KEY_QUITALL (INPUT_KEY_QUIT+1)
#define INPUT_KEY_LIST_CH (INPUT_KEY_QUITALL+1)
#define INPUT_KEY_LIST_CL (INPUT_KEY_LIST_CH+1)
#define INPUT_KEY_CH_CREATE (INPUT_KEY_LIST_CL+1)
#define INPUT_KEY_CH_JOIN (INPUT_KEY_CH_CREATE+1)
#define INPUT_KEY_CH_LEAVE (INPUT_KEY_CH_JOIN+1)
#define INPUT_KEY_TYPE_GLOBAL (INPUT_KEY_CH_LEAVE+1)
#define INPUT_KEY_TYPE_CHANNEL (INPUT_KEY_TYPE_GLOBAL+1)
#define INPUT_KEY_MSG (INPUT_KEY_TYPE_CHANNEL+1)


// Function prototypes
int parseInput(char *, int);
int getCmdStr(char *);
int matchCmd(char *, char *);



#endif
