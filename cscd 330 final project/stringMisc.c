#include "stringMisc.h"

// Zero array
void zeroA(char * str, int size) {
	int i = 0;
	for (; i < size; i++) {
		str[i] = '\0';
	}
}

// Strip whitespace off ends
void strip(char * str, int size) {
	int i = 0, start = -1, end = -1;
	for (i = 0; i < size; i++) {
		char c = *(str+i);
		if (c == '\0') {
			i = size;
		}else if (!((c == ' ') || (c == '\n') || (c == '\t') || (c == '\r') || (c == '\v') || (c == '\f'))) {
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

// check if given symbol is valid for channel
int validChSymbol(char c) {
	int n = (int) c;
	// 48-57, single digit numbers
	// 65-90, Capital letters
	// 97-122, Lowercase letters
	if ((n >= 48)&&(n <= 57)) {
		return 1;
	}
	if ((n >= 65)&&(n <= 90)) {
		return 1;
	}
	if ((n >= 97)&&(n <= 122)) {
		return 1;
	}
	return 0;
}