mssh:	./cscd340Lab8.c ./main/main.o ./files/files.o ./alias/alias.o ./linkedlist/linkedlist.o ./linkedlist/listutils.o ./history/history.o ./tokenize/makeargs.o ./utils/utils.o ./process/process.o ./pipes/pipes.o ./redirection/redirection.o
	gcc -g cscd340Lab8.c ./main/main.o ./files/files.o ./alias/alias.o ./linkedlist/linkedlist.o ./linkedlist/listutils.o ./history/history.o ./tokenize/makeargs.o ./utils/utils.o ./process/process.o ./pipes/pipes.o ./redirection/redirection.o -o mssh

main.o:		./main/main.c ./main/main.h
	gcc -g -c ./main/main.c

files.o:	./files/files.c ./files/files.h
	gcc -g -c ./files/files.c

alias.o:	./alias/alias.c ./alias/alias.h
	gcc -g -c ./alias/alias.c

linkedlist.o: 	./linkedlist/linkedlist.c ./linkedlist/linkedlist.h ./linkedlist/listutils.h ./linkedlist/struct.h
	gcc -g -c ./linkedlist/linkedlist.c
	
listutils.o: 	./linkedlist/listutils.c ./linkedlist/listutils.h ./linkedlist/linkedlist.h ./linkedlist/struct.h
	gcc -g -c ./linkedlist/listutils.c
	
history.o:	./history/history.c ./history/history.h
	gcc -g -c ./history/history.c
	
makeargs.o:	./tokenize/makeargs.c ./tokenize/makeargs.h
	gcc -g -c ./tokenize/makeargs.c

utils.o:	./utils/utils.o ./utils/utils.h
	gcc -g -c ./utils/utils.c

process.o:	./process/process.c ./process/process.h
	gcc -g -c ./process/process.c

pipes.o:	./pipes/pipes.c ./pipes/pipes.h
	gcc -g -c ./pipes/pipes.c

redirection.o:	./redirection/redirection.c ./redirection/redirection.h
	gcc -g -c ./redirection/redirection.c

clean:
	rm ./alias/alias.o
	rm ./files/files.o
	rm ./main/main.o
	rm ./linkedlist/linkedlist.o
	rm ./linkedlist/listutils.o
	rm ./utils/utils.o
	rm ./tokenize/makeargs.o
	rm ./history/history.o
	rm ./process/process.o
	rm ./pipes/pipes.o
	rm ./redirection/redirection.o
	rm mssh
