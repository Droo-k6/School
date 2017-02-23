#ifndef PARSESINPUTS_H
#define PARSESINPUTS_H


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
#define INPUT_CMD_QUEUE "/queue"
#define INPUT_CMD_CHL "/channels"
#define INPUT_CMD_CL "/clients"
#define INPUT_CMD_CCH "/create"
#define INPUT_CMD_JCH "/join"
#define INPUT_CMD_LCH "/leave"
#define INPUT_CMD_TYPG "/global"
#define INPUT_CMD_TYPC "/channel"

// Keys
#define INPUT_KEY_INVALID (-1)
#define INPUT_KEY_INFO (INPUT_KEY_INVALID + 1)
#define INPUT_KEY_HELP (INPUT_KEY_INFO + 1)
#define INPUT_KEY_QUIT (INPUT_KEY_HELP + 1)
#define INPUT_KEY_QUEUE (INPUT_KEY_QUIT + 1)
#define INPUT_KEY_LIST_CH (INPUT_KEY_QUEUE + 1)
#define INPUT_KEY_LIST_CL (INPUT_KEY_LIST_CH + 1)
#define INPUT_KEY_CH_CREATE (INPUT_KEY_LIST_CL + 1)
#define INPUT_KEY_CH_JOIN (INPUT_KEY_CH_CREATE + 1)
#define INPUT_KEY_CH_LEAVE (INPUT_KEY_CH_JOIN + 1)
#define INPUT_KEY_TYPG (INPUT_KEY_CH_LEAVE + 1)
#define INPUT_KEY_TYPC (INPUT_KEY_TYPG + 1)
#define INPUT_KEY_MSG (INPUT_KEY_TYPC + 1)


// Function prototypes
int parseInput(char *, int size);
int getCmdStr(char *);
int matchCmd(char *, char *);


#endif
