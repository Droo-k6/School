#include "process.h"


void processCommand(char *str) {
	char **argv = NULL;
	int argc = makeargs(str, &argv);
	
	forkIt(argv);

	// cleanup
	clean(argc,argv);
	argv = NULL;
}

void forkIt(char ** argv){
	// errno mesages setup in future for shell assignment
	// errno copied in case of error
	pid_t pid = fork();
	int err = errno;

	// if pid == -1, error occured/errno set
	if (pid == -1) {
		printf("fork() ERROR: errno-%d,error-%s\n",err,strerror(err));
	}else if (pid != 0) {
		// parent
		int status;
		
		// wait for child process to finish
		waitpid(pid,&status,0);
	}else{
		// child
		// code below execvp() only gets run if there was an error
		execvp(argv[0],argv);
		err = errno;
		// error occured, errno set
		if (err == 2) {
			printf("command \"%s\" not found\n",argv[0]);
		}else{
			// perror(NULL);
			printf("execvp() ERROR: errno-%d, error-%s\n", err, strerror(err));
		}
		// kill child
		exit(err);
	}
}
