#include "utils.h"


void strip(char *str) {
	if(str == NULL){
		perror("array is null");
		exit(-99);
	}

	stripSize(str,strlen(str));
}

// give size for strip operation
void stripSize(char *str, int size) {
	int i = 0, start = -1, end = -1;
	char c = '\0';
	for (; i < size; ++i) {
		c = str[i];
		if (c == '\0') {
			break;
		}else if (!isspace(c)) {
			if (start == -1) {
				start = i;
			}
			end = i;		
		}
	}
	
	if ((start == -1) || (end == -1) || (start > end)) {
		str[0] = '\0';
	}else{
		end++;
		for (i = start; i < end; i++) {
			str[i - start] = str[i];
		}
		str[end - start] = '\0';
	}
}

int strContains(char *str, char value) {
	int i = 0;
	for (; i < strlen(str); ++i) {
		if (str[i] == value) {
			return 1;
		}
	}
	return 0;
}

// for pushing a character onto end of string
// assumes rest of the array is zeroed out
void pushChar(int max, char *str, char c) {
	if (strlen(str)+1 >= max) {
		fprintf(stderr,"pushChar: string is full\n");
		exit(-1);
	}
	str[strlen(str)] = c;
}

// puts '/' at beginning
// not very efficient, could be done in one simple loop
void addSlash(char *str) {
	char temp[MAX];
	strcpy(temp,str);
	sprintf(str,"/%s",temp);
}

// shifts string left
// str being beginning of the string, shift pointing to an index
void shiftStr(char *str, char *shift) {
	if (strlen(shift) <= 0) {
		// wipe string
		memset(str,'\0',sizeof(char)*MAX);
		return;
	}

	int i = 0, len = strlen(shift);
	char c = '\0';
	for (; i < len; ++i) {
		c = shift[i];
		str[i]=c;
	}
	str[i] = '\0';
}

// for clearing a string
void clearStr(char *str, int size) {
	memset(str,'\0',sizeof(char)*size);
}

// return max of 2 ints
int max(int a, int b) {
	return ((a > b) ? a:b);
}

