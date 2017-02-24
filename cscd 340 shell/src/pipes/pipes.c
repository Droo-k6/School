#include "pipes.h"


// returns if string contains pipe
int containsPipe(char *s) {
	return strContains(s,'|');
}

// parse string up to pipe
char ** parsePrePipe(char *s, int * preCount) {
	// create copy of s
	char cpyStr[100];
	strcpy(cpyStr,s);
	
	// tokenize to first |
	char *tempStr, *tok;
	tok = strtok_r(cpyStr,"|",&tempStr);
	
	// make arguments
	char **argv;
	*preCount = makeargs(tok,&argv);
	
	return argv;
}

// parse string after pipe
char ** parsePostPipe(char *s, int * postCount) {
	// create copy of s
	char cpyStr[100];
	strcpy(cpyStr,s);
	
	// tokenize, no need to count - just skip the first token
	char *tempStr, *tok;
	tok = strtok_r(cpyStr,"|",&tempStr);
	tok = strtok_r(NULL,"|",&tempStr);
	
	// make arguments
	char **argv;
	*postCount = makeargs(tok,&argv);
	
	return argv;
}

void pipeIt(char **prePipe, char **postPipe) {
	int fd[2], status;
	pid_t pid1=0,pid2=0;
	
	pid1 = fork();
	if (pid1 != 0) {
		// parent
		// wait for child to finish
		waitpid(pid1,&status,0);
	}else{
		// child
		// setup pipe
		if(pipe(fd) < 0){
			printf("Pipe Failure\n");
			exit(-1);
		}
		
		// for getting error
		int err = -1;
		char *argv = NULL;
		
		pid2 = fork();
		if (pid2 != 0) {
			// parent (child)
			// wait for child to finish in case of error
			waitpid(pid2,&status,0);
			// check exit status (may not work in all instances - exit normally macro WIFEXITED wasn't working)
			if (WIFEXITED(status) && (WEXITSTATUS(status) == 0)) {
				// run if child exited normally
				close(fd[1]);
				close(0);
				dup(fd[0]);
				close(fd[0]);
			
				execvp(postPipe[0],postPipe);
				// set error stuff
				err = errno;
				argv = postPipe[0];
				// close pipe
				close(0);
			}else{
				// have to cleanup pipe ends
				close(fd[0]);
				close(fd[1]);
			}
		}else{
			// child (grandchild)
			close(fd[0]);
			close(1);
			dup(fd[1]);
			close(fd[1]);
			
			execvp(prePipe[0],prePipe);
			// set error stuff
			err = errno;
			argv = prePipe[0];
			// close pipe
			close(1);
		}
		// only gets run if there was an error with the execvp() calls
		// cant use stdout, since that was closed in the grandchild atleast
		if (argv != NULL) {
			if (err == 2) {
				fprintf(stderr,"command \"%s\" not found\n",argv);
			}else{
				fprintf(stderr,"execvp() ERROR: errno-%d, error-%s\n", err, strerror(err));
			}
			// if child process, the pipe needs to be dumped
			// cannot exit normally so it can be caught
			exit(-1);
		}else{
			// if here, then grandchild failed and child did not run, so just exit normally
			exit(0);
		}
	}
	
	
}

int pipeCommands(char *str) {
	if (!containsPipe(str)) {
		return 0;
	}
	
	int preCount,postCount;
	char **prePipe,**postPipe;
	
	prePipe = parsePrePipe(str, &preCount);
	postPipe = parsePostPipe(str, &postCount);
	pipeIt(prePipe, postPipe);
	
	clean(preCount, prePipe);
	clean(postCount, postPipe);
	
	return 1;
}
