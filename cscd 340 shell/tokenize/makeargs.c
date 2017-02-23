#include "makeargs.h"


void clean(int argc, char **argv) {
	int i = 0;
	for (; i < argc; ++i) {
		if (argv[i] != NULL) {
			free(argv[i]);
			argv[i] = NULL;
		}
	}
	free(argv);
}

void printargs(int argc, char **argv) {
	int x;
	for(x = 0; x < argc; x++)
		printf("%s\n", argv[x]);
}

int makeargs(char *s, char *** argv) {
	// copy string
	char cpyStr[MAX];
	strcpy(cpyStr,s);

	char *tempStr;
	
	// count number of tokens
	char *tok;
	int count = 0;
	
	tok = strtok_r(cpyStr," ",&tempStr);
	while (tok != NULL) {
		++count;
		tok = strtok_r(NULL, " ",&tempStr);
	}
	
	// recopy
	strcpy(cpyStr,s);

	// build array of strings
	// +1 for null pointer
	*argv = (char **)calloc(count+1,sizeof(char *));
	
	int i = 0;
	tok = strtok_r(cpyStr," ",&tempStr);
	while (tok != NULL) {
		(*argv)[i] = (char *)calloc(strlen(tok)+1,sizeof(char));
		strcpy((*argv)[i],tok);
		
		tok = strtok_r(NULL, " ",&tempStr);
		++i;
	}
	// set last value to NULL
	(*argv)[i] = NULL;
	// don't increment count for the NULL pointer
	
	return count;
}
