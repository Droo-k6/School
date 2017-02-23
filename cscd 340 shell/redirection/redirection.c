#include "redirection.h"


// returns true if contains '>' or '<'
int containsRedirection(char *str) {
	return (
		strContains(str,'<')||
		strContains(str,'>')
	);
}

// returns -1 if no redirection symbol
// returns 0 if '<' hit first, 1 if '>' hit first
int getRedirection(char *str) {
	int i = 0;
	for (; i < strlen(str); ++i) {
		if (str[i] == '<') {
			return 0;
		}else if (str[i] == '>') {
			return 1;
		}
	}
	return -1;
}

// performs redirection based on string
// really messy
// could combine with pipeIt, just need to modify pipeIt to support what to open/close
int runRedirection(char *str) {
	// check if valid string (and find if < or >
	int direction = getRedirection(str);
	if (direction == -1) {
		return 0;
	}
	
	if (direction) {
		// prep for > or >>
		char temp[MAX],*tempPtr;
		char *cmd = NULL, *filename = NULL;
		int append = 0;
		
		strcpy(temp,str);
		
		// get string before and after
		cmd=strtok_r(temp,">",&tempPtr);
		
		if (strlen(cmd) <= 0) {
			fprintf(stderr,"mssh: command specified\n");
			return 1;
		}
		if (strlen(tempPtr) <= 0) {
			fprintf(stderr,"mssh: no file specified\n");
			return 1;
		}
		
		// shift inside temp
		shiftStr(temp,cmd);
		
		// get filename section
		filename = tempPtr;
		strip(filename);
		if (strlen(filename) <= 0) {
			fprintf(stderr,"mssh: no file specified\n");
			return 1;
		}
		
		int direct_stdout = 0, direct_stderr = 0;
		// check what is being redirection
		char dirChar = temp[strlen(temp)-1];
		if (!isspace(dirChar)) {
			if (dirChar == '&') {
				direct_stdout = 1;
				direct_stderr = 1;
			}else if (dirChar == '1'){
				direct_stdout = 1;
			}else if (dirChar == '2'){
				direct_stderr = 1;
			}else{
				fprintf(stderr,"mssh: invalid fd for redirection-%c\n",dirChar);
				return 1;
			}
			// set byte to 0 to strip it off
			temp[strlen(temp)-1] = 0;
		}else{
			direct_stdout = 1;
		}
		
		// check if appending
		if (filename[0] == '>') {
			append = 1;
			// get rid of '>'
			shiftStr(filename,filename+1);
			// strip again for whitespace between > and filename
			strip(filename);
		}
		
		// move filename to own section
		char temp2[MAX];
		strcpy(temp2,filename);
		
		// run direction operation
		directTo(direct_stdout,direct_stderr,append,temp,temp2);
	}else{
		char temp[MAX],temp2[MAX];
		strcpy(temp,str);

		char *cmd,*filename;
		cmd = strtok_r(temp,"<",&filename);

		strip(cmd);
		strip(filename);

		if (strlen(cmd) <= 0) {
			fprintf(stderr,"mssh: command specified\n");
			return 1;
		}
		if (strlen(filename) <= 0) {
			fprintf(stderr,"mssh: no file specified\n");
			return 1;
		}
		
		
		shiftStr(temp,cmd);
		strcpy(temp2,filename);

		directFrom(temp, temp2);
	}
	return 1;
}

// direct 
// fromout - to redirect stdout
// fromerr - to redirect stderr
void directTo(int fromout, int fromerr, int append, char *cmd, char *filename) {
	if (!fromout && !fromerr) {
		// redirecting nothing then
		return;
	}
	
	int status;
	pid_t pid=0;
	
	// open file
	FILE *fout = fopen(filename,(append ? "a":"w"));
	int err = errno;
	if (fout == NULL) {
		if (err == 2) {
			fprintf(stderr,"mssh: %s: No such file or directory\n", filename);
		}else{
			fprintf(stderr,"ERROR '%s': %s\n",filename,strerror(err));
		}
		return;
	}
	
	// prep cmd for running
	char **argv = NULL;
	int argc = makeargs(cmd, &argv);
	
	// initial fork
	pid = fork();
	if (pid != 0) {
		// parent
		// wait for child
		waitpid(pid,&status,0);
		// check exit status
		if (!(WIFEXITED(status) && (WEXITSTATUS(status) == 0))) {
			// command failed, get exit status
			err = WEXITSTATUS(status);
			if (err == 2) {
				fprintf(stderr,"command \"%s\" not found\n",argv[0]);
			}else{
				fprintf(stderr,"execvp() ERROR: errno-%d, error-%s\n", err, strerror(err));
			}
		}
		// cleanup
		fclose(fout);
		clean(argc,argv);
		argv = NULL;
	}else{
		// child
		if (fromout) {
			dup2(fileno(fout),1);		// redirect stdout to file
		}
		if (fromerr) {
			dup2(fileno(fout),2);		// redirect stderr to file
		}
		
		execvp(argv[0],argv);
		// error occured
		err = errno;
		// close file
		fclose(fout);
		// can't print error, use as exit status
		exit(err);
	}
}

// direct file contents to stdin
void directFrom(char *cmd, char *filename) {
	// open file
	FILE *fout = fopen(filename,"r");
	int err = errno;
	if (fout == NULL) {
		if (err == 2) {
			fprintf(stderr,"mssh: %s: No such file or directory\n", filename);
		}else{
			fprintf(stderr,"ERROR '%s': %s\n",filename,strerror(err));
		}
		return;
	}

	int status;
	pid_t pid=0;
	
	// prep cmd for running
	char **argv = NULL;
	int argc = makeargs(cmd, &argv);
	
	// fork
	pid = fork();
	if (pid != 0) {
		// parent
		// wait for child
		waitpid(pid,&status,0);
		// check exit status
		if (!(WIFEXITED(status) && (WEXITSTATUS(status) == 0))) {
			// command failed, get exit status
			err = WEXITSTATUS(status);
			if (err == 2) {
				fprintf(stderr,"command \"%s\" not found\n",argv[0]);
			}else{
				fprintf(stderr,"execvp() ERROR: errno-%d, error-%s\n", err, strerror(err));
			}
		}
		// cleanup
		fclose(fout);
		clean(argc,argv);
		argv = NULL;
	}else{
		// child
		
		// redirect stdin to file
		dup2(fileno(fout),0);
		
		execvp(argv[0],argv);
		// error occured
		err = errno;
		// close file
		fclose(fout);
		// can't print error, use as exit status
		exit(err);
	}
}












