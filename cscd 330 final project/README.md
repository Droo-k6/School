***cscd 330 Final Project***

Final project to create a multiple client/server application incorporating multiplexing, connection queueing, and grouping of clients.
Achieved through a console chatroom, users connect - enter queue if server's set limit is full, once in can exchange messages and create/join seperate channels.
Both clients and server rely on multiplexing of sockets and stdin, and parses the inputs.

---
***Summary of Program Details***

Both server and client operate from a loop that uses multiplexing to check for stdin/socket input, then processes the messages. 
Server and client programs contain own version of parseInput (parseInputServer, parseInputClient) to deal with stdin input. Mainly for issueing commands and sending messages.

For packets, the client will add the packet onto a buffer - that is then parsed for possible packets and each added onto a packet queue to be processed (all in the same iteration), and acts on each.
When sending packets, the client only sends to the server - it holds no connection information on other clients.
On the server side, it will throw each received packet onto the sending clients buffer to be parsed.
No queue exists for sending packets, they are sent - if they fail it is looped until the message sends completely or the connection is lost, for both client and server programs.

For connection, the client provides a IP for the program (defaults to 127.0.0.1) to connect to, uses a default port. The server watches for connection, when one is received it accepts and then checks if there is room on the active clients - if not then the client is added onto the queue.
The client is notified of changes in queue position as it occurs.
Server loop will monitor for changes in client connection (new or dropped) and updates the client list/queue as needed. As well as handling received packets as explained previously.

Packet parsing - treated as an unsigned char array, complete packets are seperated by 2 pieces, the header and ending sequence. The ending sequence is set by a define in **structs.h** as "\r\n\r\n".
The header only contains 2 bytes, the first byte for what the full length of the packet should be (to handle issues in client/server sending, which shouldn't be an issue), and the second byte for what type of packet it is - such as a message to display, request to change channel, disconnect, etc.
The header and tail pieces are used to verify the packet, although not necessarily needed - just in case.
Anything in between those 2 pieces is treated as the contents for the command, which could be the name of a request channel to switch to - or a message to display/relay.
The packet is then added onto a packetQueue, which the main server/client loop will popoff and handle.

As before, each individual client holds no information about other clients. For simplicity messages sent by clients to others are modified by the server to include the clients information(channel name, client number), then relayed as a display command to each client as needed (for example clients not in the same channel are not sent the message).

---
***Files***
The file are all stuck together in one directory, some for the server, some for the client and some used by both.
Header files are not included below unless necessary.

**Server Files**
Specific to the server program.
 * **server.c**
Contains main (includes main loop) for the server. As well as function definitions specific to the server loop.
 * **client.c**
Function for client structure.
 * **channel.c**
Functions for channel structure.
 * **queue.c**
Functions for queue structure
 * **parseInputerServer.c**
Functions for parsing console input on server.

**Client Files**
Specific to the client program.
 * **clientProgram.c**
Main for client program (main loop) as well. Function definitions specific to client loop.
 * **parseInputClient.c**
 Functions for parsing console input on client.

**Common Files**
Used by both the server and client programs
 * **stringMisc.c**
String utility functions.
 * **packet.c**
Functions for creating packets to send.
 * **packetQueue.c**
Functions for packet queue structure.
 * **packetBuffer.c**
Functions for packet buffer (unsigned char array).
 * **parsePacket.c**
Functions for parsing packets (from client or server).
 * **structs.h**
Definitions for all structures used, mostly for server specific structures. Contains some additional defines as well, mainly for packet specifics.
 
**Structures - structs.h**
Structures defined in structs.h, their purpose and files that contain their related functions/defines.
 * **queue**: **queue.c**, **queue.h**
Simple queue structure for client connection queue, holds client references through a singley linked list.
 * **node**: **queue.c**, **queue.h**
Nodes for the connection queue structure.
 * **client**: **client.h**, **client.h**
Holds information for a connected client such as :
  * socket descriptor
  * client number
  * queue position
  * current channel reference
  * buffer for unsigned chars received from client to be parsed into packets
  * queue of packets parsed from buffer
 * **channel**: **channel.c**, **channel.h**
Used to represent a user channel, contains name of channel (single character), number of clients and an array of references to clients connected.
 * **packetQueue**: **packetQueue.c**, **packetQueue.h**
Queue for packets received from client/server, to be processed in order.
 * **pqNode**: **packetQueue.c**, **packetQueue.h**
Node for packetQueue structure, holds unsigned char array of the packet.

**Makefile**
For compiling the server and client programs.

---
***Reflection***
A lot of room for improvement.

**Documentation**
For one, more documentation across the project - mainly small comments for what each piece does. 
There was a lot at the beginning, but the project ended up being restructured/pactched up a couple times and those comments were removed for some reason. 
There was a personal notes file I had to keep track of what everything did/todo and possible improvement, however its not very presentable.
Definitely an area for improvement, would go great along with modularization.

**Modularization**
Certain areas such as parsing input/packets are seperated into 1/2 big functions that are just a bunch of switch/case statements, as well as a final processing piece in the main loop of both the server and client programs.
The main loops especially could be split up, as well as the files for the server/client programs split up to only have the main functions, merging the specific functions as needed (there is a lot of overlap within them).
Folders should be introduced as well, just to help navigate the files.
Abstraction of areas such as the send/receive could be done as well to help error handling in odd places, and to easily swap out to different socket types/or swapping between unix and windows.
Used of ifdef statements could help as well for compiling between unix and windows, since the project could be ported over to windows without having to keep 2 code bases.

**Enumeration**
Preprocesor defines are used to identify what each input/packet requested to do, enums could be used to reduce the size of some pieces, and provide an easy way to add more onto/inbetween.

**Object Oriented/C++**
Since there is a reliance on structures, c++ could be used without having to change any major parts of the c code. Allowing to use objects to more easily seperate out each section.
Could be made to work with both g++/microsoft compiler so usable on both unix and windows (in combination with preprocessor ifdef blocks to avoid using different code bases).

**Socket Types**
The current socket type is using SOCK_STREAM (TCP), but another type that exists in some unix distributions is SOCK_SEQPACKET which will seperate out each packet on its own unlike SOCK_STREAM which throws packets into a byte stream.

**Restructure to work as DLL**
Could be restructed to work as a DLL to work with a GUI, which could be easily used within C# to make the menus and wire in.

