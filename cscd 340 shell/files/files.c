#include "files.h"

// run remaining as commands (does not get added to history)
// primary function call to parse ".msshrc"
// sets history variables if read, same for path
// processes everything else in file as a command
// takes aliasLL in order to set any aliases
void parseRC(int *historySize,int *historyFileSize,LinkedList *aliasLL,char ***path) {
	// LinkedList for file contents
	LinkedList *fileContents = read_file(".msshrc");
	
	// LinkedList for path, converted to char ** at end
	LinkedList *pathLL = linkedList();
	
	// LinkedList for commands
	LinkedList *commands = linkedList();
	
	// for string copies
	char tempStr[MAX],tempStr2[MAX],*tempPtr,*tok;
	// loop through contents
	Node *trav = fileContents->head->next;
	while (trav != NULL) {
		char *str = (char *) (trav->data);
		
		// checking if line is for setting HISTCOUNT/HISTFILECOUNT/PATH
		// create copy of str
		strcpy(tempStr,str);
		// tokenize
		tok = strtok_r(tempStr," =",&tempPtr);
		if (strcmp(tok,"HISTCOUNT") == 0) {
			// get value
			tok = strtok_r(NULL," =",&tempPtr);
			int size = atoi(tok);
			*historySize = size;
		}else if (strcmp(tok,"HISTFILECOUNT") == 0) {
			// get value
			tok = strtok_r(NULL," =",&tempPtr);
			int size = atoi(tok);
			*historyFileSize = size;
		}else if (strcmp(tok,"PATH") == 0) {
			// remains of string in ptr, tokenize for :
			strcpy(tempStr2,tempPtr);
			strip(tempStr2);
			tok = strtok_r(tempStr2,":",&tempPtr);
			while (tok != NULL) {
				if (strcmp(tok,"$PATH") == 0) {
					// pull path environment variable
					getPathEnv(pathLL);
				}else{
					addLast(pathLL,buildNode_String(tok));
				}
				tok = strtok_r(NULL,":",&tempPtr);
			}
		}else{
			// treat as command
			addLast(commands,buildNode_String(str));
		}
		trav=trav->next;
	}
	
	// clean file contents list
	clearList(fileContents,cleanPrimitiveData);
	free(fileContents);
	fileContents = NULL;
	
	// check if path was set at all, set default
	if (listGetSize(pathLL) <= 0) {
		// attemp $PATH first
		getPathEnv(pathLL);
		// if $PATH is empty, set to some generic directories
		if (listGetSize(pathLL) <= 0) {
			setDefaultPath(pathLL);
		}
	}

	// convert path linkedlist to array
	buildPath(path,pathLL);

	// cleanup path list
	clearList(pathLL,cleanPrimitiveData);
	free(pathLL);
	pathLL = NULL;

	// process commands list
	trav = commands->head->next;
	while (trav != NULL) {
		char *cmdStr = (char *) (trav->data);
		runCommand(NULL,aliasLL,cmdStr);
		trav=trav->next;
	}
	
	// clean commands list
	clearList(commands,cleanPrimitiveData);
	free(commands);
	commands = NULL;
}

// reads ".mssh_history" file and adds contents to the history list
void parseHistory(AllHistory *allhistory) {
	// get file contents
	LinkedList *fileContents = read_file(".mssh_history");
	
	// add to history list
	Node *trav = fileContents->head->next;
	char *str = NULL;
	while (trav != NULL) {
		str = (char *) (trav->data);
		addHistory(allhistory,str);
		trav=trav->next;
	}
	
	// clean file contents list
	clearList(fileContents,cleanPrimitiveData);
	free(fileContents);
	fileContents = NULL;
}

// get contents of given file, return as LinkedList
LinkedList *read_file(char *fileName) {
	LinkedList *contents = linkedList();
	
	FILE *fin = fopen(fileName,"r");
	if (fin == NULL) {
		// file does not exist
		return contents;
	}
	
	char temp[MAX],*ptr;
	while (!feof(fin)) {
		ptr = fgets(temp,MAX,fin);
		if (ptr != NULL) {
			strip(temp);
			if (strlen(temp) > 0) {
				addLast(contents,buildNode_String(temp));
			}
		}
	}
	
	fclose(fin);
	
	return contents;
}
