#include "main.h"


// separated function for running commands
// allows null input for history to not push string to history or ignore history commands (.mssh_history)
// allows null for aliases
void runCommand(AllHistory *history, LinkedList *aliases,char *inputStr) {
	// check if str null/empty
	if (inputStr == NULL) {
		fprintf(stderr,"runCommand: string null\n");
		exit(-1);
	}
	if (strlen(inputStr) <= 0) {
		// just return
		return;
	}

	// if history is not null
	if (history != NULL) {
		// add to history
		addHistory(history,inputStr);
		
		// replace history events
		historyReplacement(history,inputStr);
	}

	// replace aliases
	if (aliases != NULL) {
		// check if its an alias command first
		if (!parseAlias(aliases,inputStr)) {
			aliasReplacement(aliases,inputStr);
		}else{
			// messy fix, if alias command was run return
			return;
		}
	}
		
	
	// the pipe/redirection/command seperation should all be shifted to one spot
	// just loop through, build the tokens and do as needed
	// would support multiple/mixing of redirection and piping

	if ((history != NULL) && displayHistory(history,inputStr)) {		// history check
	}else if (commandCD(inputStr)) {									// cd check
	}else if (runRedirection(inputStr)) {								// redirection check
	}else if (pipeCommands(inputStr)) {									// pipe check
	}else{processCommand(inputStr);}									// treat as command
}

// sets default path
void setDefaultPath(LinkedList *pathLL) {
	addLast(pathLL,buildNode_String("/usr/local/sbin"));
	addLast(pathLL,buildNode_String("/usr/local/bin"));
	addLast(pathLL,buildNode_String("/usr/sbin"));
	addLast(pathLL,buildNode_String("/usr/bin"));
	addLast(pathLL,buildNode_String("/sbin"));
	addLast(pathLL,buildNode_String("/bin"));
	addLast(pathLL,buildNode_String("/usr/games"));
	addLast(pathLL,buildNode_String("/usr/local/games"));
}

// pulls path variable, add to linked list
void getPathEnv(LinkedList *pathLL) {
	char *str = getenv("PATH");
	if ((str == NULL)||(strlen(str) <= 0)) {
		return;
	}
	
	addFirst(pathLL,buildNode_String(str));
}

// convert linked list to array for path
// path has to have [path,NULL] format
// so everything in the linkedlist goes into a giant string
void buildPath(char ***path, LinkedList *pathLL) {
	*path = (char **)calloc(2,sizeof(char *));
	
	// set last pointer to NULL
	(*path)[1] = NULL;

	// calculator size of string needed
	int size = 0;

	Node *trav = pathLL->head->next;
	while (trav != NULL) {
		char *str = (char *) (trav->data);
		size += strlen(str)+1;	// +1 for ':'
		trav=trav->next;
	}
	char *mainStr = (char *)calloc(size+1,sizeof(char *));

	// grab all the strings
	trav = pathLL->head->next;
	while (trav != NULL) {
		char *str = (char *) (trav->data);
		strcat(mainStr,str);
		if (trav->next != NULL) {
			strcat(mainStr,":");
		}
		trav=trav->next;
	}

	(*path)[0] = mainStr;

	// overwrite env path
	setenv("PATH",mainStr,1);
}

// don't need count, just free until NULL is hit
void cleanPath(char **path) {
	int i = 0;
	while (path[i] != NULL) {
		free(path[i]);
		path[i] = NULL;
		++i;
	}
	free(path);
}

// returns 0/1 if command was a cd command
int commandCD(char *str) {
	char temp[MAX],*tok,*tempPtr;
	strcpy(temp,str);
	
	tok = strtok_r(temp," ",&tempPtr);
	if (strcmp(tok,"cd") != 0) {
		return 0;
	}
	// path remains in tempPtr, shift over
	// could just use tempPtr from now on
	shiftStr(temp,tempPtr);
	strip(temp);
	
	// if empty, just return
	if (strlen(temp) > 0) {
		// performs home expansion
		homeExpansion(temp);
		
		// change working directory
		if (chdir(temp) == -1) {
			int err = errno;
			if (err == 2) {
			//	fprintf(stderr,"mssh: cd: %s: No such file or directory\n",temp);
				fprintf(stderr,"mssh: cd: %s: %s\n",temp,strerror(err));
			}else{
				fprintf(stderr,"mssh: cd: %s: some errno\n",temp);
			}
			
		}
	}
	return 1;
}

// expands ~ to home directory
void homeExpansion(char *str) {
	if (strlen(str) <= 0) {
		return;
	}
	// only perform if first byte is '~'
	if (str[0] != '~') {
		return;
	}

	char temp[MAX];

	// first check if HOME environment variable is available
	const char *homedir = getenv("HOME");
	if (homedir != NULL) {
		strcpy(temp,homedir);
	}else{
		// otherwise, use getpwuid_r method
		// too much for me to bother with
		return;
	}
	// add rest of path on top of, ignoring first byte
	if (strlen(str) > 1) {
		strcat(temp,(str+1));
	}
	// copy over full path
	strcpy(str,temp);
}
