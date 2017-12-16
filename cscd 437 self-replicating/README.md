**cscd 437 self-replicating program**
Class work was in teams-
Mathew McCain
Trae Rawls
Anh Tran Duc

Self-replicating C program, uses a python script to build the replicating logic.
Based on Ken Thompson's paper
[Reflections on Trusting Trust](https://www.win.tue.nl/~aeb/linux/hh/thompson/trust.html)

---
Source files

**convert.py**
Searches input file for `/*@replicate*/` and inserts the replicating logic

**test.c**
input file for python script

**fout.c**
resulting file from script, will replicate self to another file + stdout, then compile/execute. stops after 5 replications.