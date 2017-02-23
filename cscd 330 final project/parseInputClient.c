#include "parseInputClient.h"

// Parses console input
int parseInput(char *buf, int size) {
	// check if command identifer was placed "/"
	if (buf[0] == '/') {
		char *tok = strtok(buf, " ");
		
		// if special command, want to pull whatever is needed 
		// so main loop doesn't have to
		int ret = getCmdStr(tok);
		
		if (ret == INPUT_KEY_INVALID) {
			return ret;
		}
		
		tok = strtok(NULL, " ");
		
		char ch = '\0';

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
				char c = tok[0];
				if (strtok(NULL, " ") != NULL) {
					printf("parseInput error: invalid characters after channel name\n");
				}
				zeroA(buf, size+1);
				buf[0] = c;
				
				break;
			case INPUT_KEY_HELP:
			case INPUT_KEY_INFO:
			case INPUT_KEY_QUIT:
			case INPUT_KEY_QUITALL:
			case INPUT_KEY_LIST_CH:
			case INPUT_KEY_LIST_CL:
			case INPUT_KEY_CH_LEAVE:
			case INPUT_KEY_TYPE_GLOBAL:
			case INPUT_KEY_TYPE_CHANNEL:
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
	if (matchCmd(str, INPUT_CMD_QUITALL)) {return INPUT_KEY_QUITALL;};
	if (matchCmd(str, INPUT_CMD_LIST_CH)) {return INPUT_KEY_LIST_CH;};
	if (matchCmd(str, INPUT_CMD_LIST_CL)) {return INPUT_KEY_LIST_CL;};
	if (matchCmd(str, INPUT_CMD_CH_CREATE)) {return INPUT_KEY_CH_CREATE;};
	if (matchCmd(str, INPUT_CMD_CH_JOIN)) {return INPUT_KEY_CH_JOIN;};
	if (matchCmd(str, INPUT_CMD_CH_LEAVE)) {return INPUT_KEY_CH_LEAVE;};
	if (matchCmd(str, INPUT_CMD_TYPE_GLOBAL)) {return INPUT_KEY_TYPE_GLOBAL;};
	if (matchCmd(str, INPUT_CMD_TYPE_CHANNEL)) {return INPUT_KEY_TYPE_CHANNEL;};

	return INPUT_KEY_INVALID;
}

// to check if command matches given string
int matchCmd(char *str, char *cmd) {
	return (strcmp(str, cmd) == 0);
}
