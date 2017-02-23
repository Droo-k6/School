#include "alias.h"


Alias *alias(char *key, char *value) {
	Alias *aliasPtr = (Alias *)calloc(1,sizeof(Alias));
	
	aliasPtr->key = (char *)calloc(strlen(key)+1,sizeof(char));
	strcpy(aliasPtr->key,key);
	
	if (value != NULL) {
		aliasPtr->value = (char *)calloc(strlen(value)+1,sizeof(char));
		strcpy(aliasPtr->value,value);
	}else{
		aliasPtr->value = NULL;
	}
	
	
	return aliasPtr;
}

void printAlias(void *data) {
	if (data == NULL) {
		fprintf(stderr,"printAlias: data is null\n");
		exit(-1);
	}
	Alias *aliasPtr = (Alias *) data;
	printf("alias %s='%s'\n",aliasPtr->key,aliasPtr->value);
}

// not found message, title is alias or unalias
void printNoAlias(char *title,char *key) {
	fprintf(stderr,"mssh: %s: %s: not found\n",title,key);
}

void cleanAlias(void *data) {
	if (data == NULL) {
		fprintf(stderr,"cleanAlias: data is null\n");
		exit(-1);
	}
	Alias *aliasPtr = (Alias *) data;
	
	free(aliasPtr->key);
	aliasPtr->key = NULL;
	
	if (aliasPtr->value != NULL) {
		free(aliasPtr->value);
		aliasPtr->value = NULL;
	}
}

// simply compares the keys
int compareAlias(const void *data1, const void *data2) {
	Alias *a1 = (Alias *) data1, *a2 = (Alias *) data2;
	char *str1 = a1->key, *str2 = a2->key;
	return strcmp(str1,str2);
}

// checks if string starts w/ alias or unalias
// formats
//	alias name = "string"
// 	unalias name
// can be double quotes or single quotes
// guaranteed to be consistant though
int containsAlias(char *str) {
	// copy string
	char temp[MAX],*tok,*tempPtr;
	strcpy(temp,str);
	
	// tokenise
	tok = strtok_r(temp," ",&tempPtr);
	
	// return test if matches alias or unalias
	return ((strcmp(tok,"alias") == 0)||(strcmp(tok,"unalias") == 0));
}

// parse alias/unalias command
int parseAlias(LinkedList *aliasList,char *str) {
	// check if valid string
	if (!containsAlias(str)) {
		return 0;
	}
	
	// copy string
	char temp[MAX],*tok=NULL,*tempPtr=NULL,*key=NULL,*value=NULL;
	strcpy(temp,str);
	
	// tokenise first word by spaces
	tok = strtok_r(temp," ",&tempPtr);
	if (strcmp(tok,"alias") == 0) {
		// tok has the rest of the line after alias (spaces at the beginning)
		// easier to manually loop through from now on instead of trying to work with strtok
		char temp2[MAX],temp3[MAX],tempKey[MAX],tempValue[MAX];
		strcpy(temp2,tempPtr);
		strip(temp2);
		
		// loop to seperate out statements, using strtok_r is too much of a pain
		int i = 0;
		char c = '\0';
		int readingKey=0,readingValue=0,isWhiteSpace=0,inQuote=0;
		for (; i < strlen(temp2)+1; ++i) {
			c = temp2[i];

			if (c == '\0') {
				if (readingKey) {
					findPrintAlias(aliasList,tempKey);
				}else if (readingValue) {
					updateAlias(aliasList,tempKey,tempValue);
				}
				// just break from loop
				break;
			}
			
			isWhiteSpace = isspace((int) c);
			if (readingKey) {
				if (isWhiteSpace) {
					// no longer reading key
					// print out key value pair
					readingKey=0;
					findPrintAlias(aliasList,tempKey);
				}else if (c == '=') {
					// no longer reading key, set reading value
					readingKey=0;
					readingValue=1;
					
					// reset value string
					memset(tempValue,'\0',sizeof(char)*MAX);
				}else{
					// add onto key
					pushChar(MAX,tempKey,c);
				}
			}else if (readingValue) {
				if (isWhiteSpace) {
					// check if in quotes
					if (inQuote) {
						// add onto value
						pushChar(MAX,tempValue,c);
					}else{
						// treat as end of definition
						readingValue=0;
						updateAlias(aliasList,tempKey,tempValue);
					}
				}else if ((c == '\'')||(c == '\"')) {
					if (inQuote) {
						// treat as end of value definition
						inQuote=0;
						readingValue=0;
						updateAlias(aliasList,tempKey,tempValue);
					}else{
						// set as reading in quote now
						inQuote=1;
					}
				}else{
					pushChar(MAX,tempValue,c);
				}
			}else{
				if (isWhiteSpace) {
					// do nothing
				}else{
					// not reading anything, set key
					readingKey=1;
				
					// reset key string
					memset(tempKey,'\0',sizeof(char)*MAX);
					// add on to key
					pushChar(MAX,tempKey,c);
				}
			}
		}
	}else{
		// unalias
		// tokenize by spaces
		tok = strtok_r(NULL," ",&tempPtr);
		while (tok != NULL) {
			removeAlias(aliasList,tok);
			tok = strtok_r(NULL," ",&tempPtr);
		}
	}
	return 1;
}

void *findAlias(LinkedList *aliasList, char *key) {
	void *dataPtr = NULL;
	void *tempData = (void *) alias(key,NULL);
	
	Node *trav = aliasList->head->next;
	while (trav != NULL) {
		if (compareAlias(trav->data,tempData) == 0) {
			dataPtr = (trav->data);
			break;
		}
		trav=trav->next;
	}
	cleanAlias(tempData);
	free(tempData);

	return dataPtr;
}

char *aliasFindValue(LinkedList *aliasList, char *key) {
	void *data = findAlias(aliasList,key);
	if (data == NULL) {
		return NULL;
	}
	
	Alias *alias = (Alias *) data;
	return (alias->value);
}

// finds alias key, prints value
void findPrintAlias(LinkedList *aliasList, char *key) {
	void *aliasPtr = findAlias(aliasList,key);
	if (aliasPtr != NULL) {
		printAlias(aliasPtr);
	}else{
		printNoAlias("alias",key);
	}
}

// tries to find node, then update value
// if not found, add new
void updateAlias(LinkedList *aliasList, char *key, char *value) {
	// for simplicity, just run remove then addLast
	removeItem(aliasList,buildNode_Type((void *) alias(key,NULL)),cleanAlias,compareAlias);
	addLast(aliasList,buildNode_Type((void *) alias(key,value)));
}

// removes node that contains given key
// if not found, message is displayed
void removeAlias(LinkedList *aliasList, char *str) {
	if (!removeItem(aliasList,buildNode_Type((void *) alias(str,NULL)),cleanAlias,compareAlias)) {
		printNoAlias("unalias",str);
	}
}

// looks for alias tokens, runs replacement
void aliasReplacement(LinkedList *aliasList, char *str) {
	// tokenize by spaces, replace any tokens that match an alias key
	char temp[MAX],*tok,*tempPtr,*replacement;
	strcpy(temp,str);

	// first check if starts with alias, if so-ignore
	tok = strtok_r(temp," ",&tempPtr);
	if (strcmp(tok,"alias") == 0) {
		return;
	}
	
	// create a LinkedList to keep track of tokens
	LinkedList *tokens = linkedList();
	
	while (tok != NULL) {
		// find/replace
		replacement = aliasFindValue(aliasList,tok);
		// add to list
		if (replacement == NULL) {
			addLast(tokens,buildNode_String(tok));
		}else{
			
			addLast(tokens,buildNode_String(replacement));
		}
		
		// get next token
		tok = strtok_r(NULL," ",&tempPtr);
	}

	// build new string
	memset(str,'\0',sizeof(char)*MAX);
	Node *trav = tokens->head->next;
	while (trav != NULL) {
		tok = (char *)(trav->data);
		strcat(str,tok);
		if (trav->next != NULL) {
			strcat(str," ");
		}
		trav=trav->next;
	}
	
	// clean list
	clearList(tokens,cleanPrimitiveData);
	free(tokens);
	tokens = NULL;
}


