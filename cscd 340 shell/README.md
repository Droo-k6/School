***cscd 340 Shell Project***

Project to build a basic unix shell program, supports history viewing, path variable setting, aliases, redirection/piping of programs/files, running programs w/ arguments (arguments work with piping).

---
***Files***

The functions are seperated into folders, **cscd340Lab8.c** contains documentation for each folder contains and what the shell can do in detail.

---
***Reflection***

Pipeing and redirection are both distinctly seperated and each command line only supports 1 of such a call (cannot string together multiple redirections/pipes/mixes). This would be a simple change to a parsing loop, the different directions may be an issue but can easily be put into a queue/stack to be done. First by seperating pipes/redirection from program names and arguments, then arranging into the proper order in a queue - using a stack for the arguments.

Certain programs have issues running within the shell, some in console editors and man pages, or gcc. Should be addressed.

Should also allow other envrionment variables to be set within the .msshrc file aside from path and HISTCOUNT/HISTFILECOUNT.

Passing around the structures for aliases/history seperately, could be made more readible by having 1 struct to hold references to both and similar structs.

More error checks should be implemented as well, for future safety.
