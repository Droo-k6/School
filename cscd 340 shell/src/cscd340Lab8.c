// standard libraries
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>
// headers
#include "./alias/alias.h"
#include "./files/files.h"
#include "./history/history.h"
#include "./linkedlist/linkedlist.h"
#include "./linkedlist/listutils.h"
#include "./linkedlist/struct.h"
#include "./main/main.h"
#include "./pipes/pipes.h"
#include "./process/process.h"
#include "./tokenize/makeargs.h"
#include "./redirection/redirection.h"
#include "./utils/utils.h"
/*
NOTES

DIRECTORIES
		alias, alias stuff
		files, for reading/parsing .msshrc/.mssh_history
		history, for history related functions (and dumping to history file)
		linkedlist, for linkedlist structures/functions
		main, for command executing
		pipes, for piping 2 commands
		process, for running a single command
		tokenize, for tokenizing function arguments (lots of tokenizing in other places)(name should be changed)
		redirection, functions for redirection (>,>>,<)
		utils, utility functions, mainly for strings	

FEATURES
files read from same directory as program
	.msshrc
		will determine HISTCOUNT and HISTFILECOUNT from
		reads full file, seperates history values and PATH from the rest
		remaining lines are thrown into a linkedlist to be run as commands before input loop
	.mssh_history
		will build history list from
history
	if history list exceeds HISTORYFILESIZE, list is trimmed down to match
	the history is dumped to the history file on proper exit
		killing the process (ctrl+c) will result in history not being dumped
	bash history event
		!#
			#
				if positive, looks for index given
				if negative, starts from tail to back
			will use history list according to HISTORYSIZE
		!!
			repeats last command
		too many edges cases for all that can be typed though, so just limited to above
		treated like an alias replacement, looks for tokens that match
aliases
	printing/assigning multiple keys in one alias is supported
		alias a b c='c asignment' d='d asignment' e
			will print a,b,e value
			assign c,d
		for assignment, no spaces between, keyword=value
		nested quotes are not
		mixing double and single quotes will treat them as the same
			so alias a='invalid quotes" will work
	unaliasing multiple variables at once supported
	commands processed from .msshrc will have alias run on them (
		-for aliases that are processed at that point
mixing history events and aliases not supported
change directory (cd)
	supported, displays error if chdir() fails
	home expansion is performed if first character is '~'
		home has to be defined in the "HOME" environment variable
		not using getpwuid_r to attempt home path retrieval if previous fails
redirection
	supports 
		> (write to file)
		>> (append to file)
		< (file contents to stdin), 
		1 (stdout), 2 (stderr), & (both) for >/>>
	not supported, 
		<<
		>& syntax
		custom file descriptor redirection,
		multiple redirections in one line
	redirecting to a file in another directory is not supported
		may crash if attempted (should catch if error)
		easy to implement but nothing says I have to
piping
	1 pipe supported
	piping with |& or similar is not supported
path
	supports path assignment, with ':' between directories
	suports path expansion by $PATH (environment variable)
		no alias/other env replacement though
	if no path is set, (even by 'PATH='), the default is set
		if $PATH is not set, sets to some generic directories
	path is set w/ setenv("PATH","...",1)
		execvp() is used, will check in path env variable
		execvpe() is not used because then the PATH env variable has to be deleted before it checks the envp passed
	


lots of ineffeciencies, especially around parsing the strings, or LinkedLists
	doens't effect functionality though
	all the tokenize stuff could be squeezed down to a few functions
		same with the exec calls/forks for redirecton/pipe
stdout/stderr
	not all error messages go to stderr, just because I didnt feel like it
error catching
	not as many null checks as there should be
*/

// main
int main() {
	///////////////////////////////////////////////
	// pre-loop, setup
	
	// input buffer
	char inputString[MAX];
	
	// Setup history LL and size variables (default size=100,filesize=1000)
	AllHistory *allhistory = allHistory();
	
	// setup aliases (LL)
	LinkedList *aliasLL = linkedList();
	
	// LinkedList for path variable
	char **path = NULL;
	
	// reads/parses ".msshrc"
	// sets default path if not defined
	parseRC(&(allhistory->size),&(allhistory->filesize),aliasLL,&path);
	
	// read/build history from ".mssh_history"
	parseHistory(allhistory);
	
	///////////////////////////////////////////////
	// input loop
	while(1) {
		printf("command?: ");
		fgets(inputString,MAX,stdin);
		strip(inputString);

		// length check
		if (strlen(inputString) <= 0) {
			// skip to next iteration
			continue;
		}
		
		// exit check
		if (strcmp(inputString,"exit") == 0) {
			// break loop
			break;
		}
		
		// runs command
		runCommand(allhistory,aliasLL,inputString);
	}
	///////////////////////////////////////////////
	// post-loop, cleanup
	
	// cleanup alias list
	clearList(aliasLL,cleanAlias);
	free(aliasLL);
	aliasLL = NULL;
	
	// cleanup path list
	cleanPath(path);
	path = NULL;
	
	// dump to history to file
	dumpHistory(allhistory);
	
	// cleanup history
	cleanAllHistory(allhistory);
	allhistory = NULL;
	
	///////////////////////////////////////////////
	return 0;
}
