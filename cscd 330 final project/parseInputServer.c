#include "parseInputServer.h"

// initial function
// for parsing input on server console
// really just checks for commands
int parseInput(char *buf, int size) {
	// check if command identifer was placed "/"
	if (buf[0] == '/') {
		char *tok = strtok(buf, " ");
		
		int ret = getCmdStr(tok);
		
		if (ret == INPUT_KEY_INVALID) {
			return ret;
		}
		
		tok = strtok(NULL, " ");
		
		char ch = '\0';
		int num = -1;

		switch (ret) {
			case INPUT_KEY_INVALID:
				break;
			case INPUT_KEY_CH_CREATE:
				if (tok == NULL) {
					printf("parseInput error: need channel name after command\n");
					return INPUT_KEY_INVALID;
				}
				if (strlen(tok) > 1) {
					printf("parseInput error: invalid channel name length\n");
					return INPUT_KEY_INVALID;
				}
				if (!validChSymbol(tok[0])) {
					printf("parseInput error: invalid channel name\n");
					return INPUT_KEY_INVALID;
				}
				ch = tok[0];
				if (strtok(NULL, " ") != NULL) {
					printf("parseInput error: invalid characters after channel name\n");
				}
				zeroA(buf, size+1);
				buf[0] = ch;
				
				break;
			case INPUT_KEY_CH_JOIN:
				if (tok == NULL) {
					printf("parseInput error: need channel name after command\n");
					return INPUT_KEY_INVALID;
				}
				if (strlen(tok) > 1) {
					printf("parseInput error: invalid channel name length\n");
					return INPUT_KEY_INVALID;
				}
				if (!validChSymbol(tok[0])) {
					printf("parseInput error: invalid channel name\n");
					return INPUT_KEY_INVALID;
				}
				ch = tok[0];
				if (strtok(NULL, " ") != NULL) {
					printf("parseInput error: invalid characters after channel name\n");
				}
				zeroA(buf, size+1);
				buf[0] = ch;
				
				break;
			case INPUT_KEY_HELP:
			case INPUT_KEY_INFO:
			case INPUT_KEY_QUIT:
			case INPUT_KEY_QUEUE:
			case INPUT_KEY_LIST_CH:
			case INPUT_KEY_LIST_CL:
			case INPUT_KEY_CH_LEAVE:
			case INPUT_KEY_TYPG:
			case INPUT_KEY_TYPC:
				if (tok != NULL) {
					printf("parseInput error: characters after command\n");
					return INPUT_KEY_INVALID;
				}
				break;
			default:
				// nothing
				break;
		}
		
		return ret;
		
	}
	// otherwise, treat as message
	return INPUT_KEY_MSG;
};

// get command from string
int getCmdStr(char *str) {
	if (matchCmd(str, INPUT_CMD_HELP)) {return INPUT_KEY_HELP;};
	if (matchCmd(str, INPUT_CMD_INFO)) {return INPUT_KEY_INFO;};
	if (matchCmd(str, INPUT_CMD_QUIT)) {return INPUT_KEY_QUIT;};
	if (matchCmd(str, INPUT_CMD_QUEUE)) {return INPUT_KEY_QUEUE;};
	if (matchCmd(str, INPUT_CMD_CHL)) {return INPUT_KEY_LIST_CH;};
	if (matchCmd(str, INPUT_CMD_CL)) {return INPUT_KEY_LIST_CL;};
	if (matchCmd(str, INPUT_CMD_CCH)) {return INPUT_KEY_CH_CREATE;};
	if (matchCmd(str, INPUT_CMD_JCH)) {return INPUT_KEY_CH_JOIN;};
	if (matchCmd(str, INPUT_CMD_LCH)) {return INPUT_KEY_CH_LEAVE;};
	if (matchCmd(str, INPUT_CMD_TYPG)) {return INPUT_KEY_TYPG;};
	if (matchCmd(str, INPUT_CMD_TYPC)) {return INPUT_KEY_TYPC;};
	
	return INPUT_KEY_INVALID;
}

// to check if command matches given string
int matchCmd(char *str, char *cmd) {
	return (strcmp(str, cmd) == 0);
}


