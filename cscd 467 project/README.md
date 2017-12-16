**cscd 467 project**

Command server that passes commands off to worker threads to handle, uses a thread pool that will double/half the number of works depending on the number being utilized.
Tests + logging to prove that the thread pool grows/shrinks according to the number of requests.
Basic client GUI to send commands.

In java.

Commands are: add, subtract, multiply, divide, kill.

---

Provided windows batch files to use, intended to be run from outside the src directory (has to be moved from bat)
* compile.bat, will compile classes to folder "bin" in same directory
* runClient.bat, runs the GUI client
* runServer.bat, runs the server
* runTestClientLoad.bat, runs the test for the client load
testClientLoadOutput.txt, sample output of run
* runTestKill.bat, runs test of the kill command
testKillOutput.txt, sample output of run
* runTestCommands.bat, runs test of commands
testCommandsOutput.txt, sample output of run
* cleanup.bat, deletes "bin" folder and contents

