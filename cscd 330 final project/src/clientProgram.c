#include "clientProgram.h"

int main(int argc, char **argv) {
	char *ipStr;
	if (argc !=2) {
		printf("IP missing, defaulting <127.0.0.1>\n");
		ipStr = (char *)calloc(10, sizeof(char));
		zeroA(ipStr, 10);
		strcat(ipStr, "127.0.0.1");
	}else{
		int len = strlen(argv[1]);
		ipStr = (char *)calloc(len+1, sizeof(char));
		zeroA(ipStr, len+1);
		strcat(ipStr, argv[1]);
	}
	printf("IP: %s\n", ipStr);
	printf("Port: %d\n", PORT);
	
	
	printf("Starting connection\n");
	int sockfd = -1;
	sockAddr_in servaddr;

	//Create a socket for the client
	if ((sockfd = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
		printf("Problem in creating the socket\n");
		return -1;
	}
	printf("sockfd: %d\n", sockfd);
	
	// setting up serveraddr
	servaddr.sin_family = AF_INET;
	servaddr.sin_addr.s_addr = inet_addr(ipStr);
	servaddr.sin_port =  htons(PORT);
	memset(servaddr.sin_zero, 0, sizeof(servaddr.sin_zero));
	
	// todo, replace w/ getaddrinfo
	// setup the hints.ai stuff?
	// might just be for servers
	
	if (connect(sockfd, (sockAddr *) &servaddr, sizeof(servaddr))<0) {
		printf("Problem in connecting to the server\n");
		return -1;
	}
	
	
	// fdSet: master set
	// fdSetCopy: copy of master set in loop
	fd_set fdSet, fdSetCopy;
	
	// int for highest fd read in
	int fdHigh = sockfd;
	
	// timeout stuff
	timeVal tv = {.tv_sec = 0, .tv_usec = 0}, ctv;

	// Zero descriptor sets
	FD_ZERO(&fdSet); 
	FD_ZERO(&fdSetCopy);
	
	// Add stdin and socket descriptor to fdSet
	FD_SET(0, &fdSet);
	FD_SET(sockfd, &fdSet);
	
	
	// Active Client Number
	int clientNumber = -1;
	// Queue Position
	int queuePos = -1;
	// Character for current channel
	char currentCh = '\0';
	// For typing on global/channel
	int typeOn = TYPE_GLOBAL;
	
	// for in queue or not
	int outQueue = 0;
	
	
	// Buffers
	// Buffer for input
	char inBuf[MAX_BUF_INPUT+1];
	// Buffer for recv
	char rvBuf[MAX_BUF_RECV+1];
	// Buffer for incomplete packets
	unsigned char pBuf[MAX_BUF_PACKETS+1];
	zeroA((char *) pBuf, MAX_BUF_PACKETS+1);
	
	// Packet Queue
	packetQueue *mPQ = pqCreate();
	
	
	printf("\nSetup complete, awaiting for status (the quit command may be used)\n");
	printHelp();
	printf("\n\n");
	
	int loop = 1, i = 0, ret = -1, err = -1;

	char ch = '\0', ch2 = '\0';
	int num = -1, num2 = -1;

	while (loop) {
		// copy fd set, run select on it
		fdSetCopy = fdSet;
		// copy tv, select modifies
		ctv = tv;
		ret = select(fdHigh+1, &fdSetCopy, NULL, NULL, &ctv);
		err = errno;
		if (ret == -1) {
			printf("select error, errno: %d\n", err);
		}
		// ret has # of descriptors available
		
		// check stdin
		if (FD_ISSET(0, &fdSetCopy)) {
			zeroA(inBuf, MAX_BUF_INPUT+1);
			
			fgets(inBuf, MAX_BUF_INPUT, stdin);
			strip(inBuf, strlen(inBuf));
			
			// parseStringClientInput
			ret = parseInput(inBuf, MAX_BUF_INPUT);
			if (!outQueue) {
				switch (ret) {
					case INPUT_KEY_INVALID:
						printf("Invalid input\n");
						break;
					case INPUT_KEY_HELP:
						// display help
						printHelp();
						break;
					case INPUT_KEY_INFO:
						// display info
						printInfo(sockfd, clientNumber, queuePos, currentCh, typeOn);
						break;
					case INPUT_KEY_QUIT:
						// quit
						printf("Sending quit to server & stopping...\n");
						// send quit
						sendQuit(sockfd);
						loop = 0;
						break;
					default:
						printf("error: still in queue (quit/help/info are available)\n");
						break;
				}
			}else{
				switch (ret) {
					case INPUT_KEY_INVALID:
						printf("Invalid command, type '%s' for list of commands\n", INPUT_CMD_HELP);
						break;
					case INPUT_KEY_MSG:
						// send message
						if (strlen(inBuf) > MAX_MSG) {
							printf("Input error: message too long\n");
						}else if (typeOn == TYPE_GLOBAL) {
							sendGlobal(sockfd, inBuf);
						}else if(typeOn == TYPE_CHANNEL) {
							if (validChSymbol(currentCh)) {
								sendChannel(sockfd, inBuf);
							}else{
								printf("Input error: not in channel\n");
							}
						}else{
							printf("error: typeOn is set incorrectly\n");
						}
						break;
					case INPUT_KEY_HELP:
						// display help
						printHelp();
						break;
					case INPUT_KEY_INFO:
						// display info
						printInfo(sockfd, clientNumber, queuePos, currentCh, typeOn);
						break;
					case INPUT_KEY_QUIT:
						// quit
						printf("Sending quit to server & stopping...\n");
						// send quit
						sendQuit(sockfd);
						loop = 0;
						break;
					case INPUT_KEY_QUITALL:
						// quit all
						printf("Sending quit all request to server & stopping...\n");
						// send quitall
						sendQuitAll(sockfd);
						loop = 0;
						break;
					case INPUT_KEY_LIST_CH:
						// get channel list
						printf("Sending channel list request to server...\n");
						sendGetChList(sockfd);
						break;
					case INPUT_KEY_LIST_CL:
						// get client list
						printf("Sending client list request to server...\n");
						sendGetCList(sockfd);
						break;
					case INPUT_KEY_CH_CREATE:
						// create channel
						ch = inBuf[0];
						printf("Sending create channel '%c' request to server...\n", ch);
						sendCreateCh(sockfd, ch);
						break;
					case INPUT_KEY_CH_JOIN:
						// join channel
						ch = inBuf[0];
						printf("Sending join channel '%c' request to server...\n", ch);
						sendJoinCh(sockfd, ch);
						break;
					case INPUT_KEY_CH_LEAVE:
						// leave channel
						if (validChSymbol(currentCh)) {
							printf("Sending leave channel request to server...\n");
							sendLeaveCh(sockfd);
						}else{
							printf("error: not in channel\n");
						}
						break;
					case INPUT_KEY_TYPE_GLOBAL:
						// type in global
						printf("typeOn set to global\n");
						typeOn = TYPE_GLOBAL;
						break;
					case INPUT_KEY_TYPE_CHANNEL:
						// type in channel
						if (validChSymbol(currentCh)) {
							printf("typeOn set to channel, current channel '%c'\n", currentCh);
							typeOn = TYPE_CHANNEL;
						}else{
							printf("error: not in channel\n");
						}
						break;
					default:
						printf("parseInput error, invalid return\n");
						break;
				}
			}
			
		} // STDIN CHECK
		
		// check socket
		if (FD_ISSET(sockfd, &fdSetCopy)) {
			zeroA(rvBuf, MAX_BUF_RECV+1);
			
			ret = recv(sockfd, rvBuf, MAX_BUF_RECV, 0);
			err = errno;
			if (ret == -1) {
				printf("recv error, -1 ret, errno: %d\n", err);
			}else if (ret < 0) {
				printf("recv error, ret: %d, errno: %d\n", ret, err);
			}else if (ret == 0) {
				printf("Connection closed by server\n");
				loop = 0;
			}else{
				// Add to packet buffer
				// will process buffer for packets and add to queue
				bufferAddQueue(mPQ, pBuf, MAX_BUF_PACKETS, (unsigned char *) rvBuf);
				
				/*
					int clientNumber = -1;
					int queuePos = -1;
					int privateCN = -1;
					char currentCh = '\0';
					int outQueue = 0;
				*/
				
				while (loop&&(pqSize(mPQ) > 0)) {
					// parseStringClientPacket
					unsigned char *packet = pqDequeue(mPQ);
					ret = parsePacket(packet);
					switch (ret) {
						case PACKET_CMD_INVALID:
							printf("parsePacket: invalid packet\n");
							break;
							
						// Invalid commands
						// shouldn't receive as client
						case PACKET_CMD_QUIT_C:
							// client quit
						case PACKET_CMD_MSG_G:
							// global message
						case PACKET_CMD_MSG_C:
							// channel message
						case PACKET_CMD_CH_CREATE:
							// create channel
						case PACKET_CMD_CH_JOIN:
							// join channel
						case PACKET_CMD_CH_LEAVE:
							// leave channel
							printf("parsePacket: invalid command\n");
							break;
						
						case PACKET_CMD_QUIT_S:
							// server quiting
							printf("Server quiting, quiting...\n");
							loop = 0;
							break;
						case PACKET_CMD_NOTI:
							// notification from server
							printf("(server notification): %s\n", (char *) packet);
							break;
						case PACKET_CMD_QP:
							// set queue position
							num = atoi((char *) packet);
							if (num == 0) {
								queuePos = num;
								printf("QP set 0\n");
							}else if (num == -1) {
								queuePos = num;
								printf("QP set -1\n");
							}else{
								queuePos = num;
								printf("Queue position update, #%d\n", num);
							}
							break;
						case PACKET_CMD_CN:
							// set client number
							num = atoi((char *) packet);
							if (outQueue) {
								printf("error: cn received but out of queue\n");
							}else if (num <= 0) {
								clientNumber = num;
								printf("Client Number set <= 0\n");
							}else{
								clientNumber = num;
								outQueue = 1;
								printf("Client Number received, %d\n", num);
								printf("Out of queue, all commands available\n");
							}
							break;
						case PACKET_CMD_CH:
							// set channel name
							ch = (char) packet[0];
							if (validChSymbol(ch)) {
								printf("Channel set to '%c'\n", ch);
								currentCh = ch;
							}else if (currentCh != '\0') {
								printf("Moved out of channel\n");
								currentCh = '\0';
							}else{
								currentCh = '\0';
							}
							break;
						case PACKET_CMD_CL:
							// display client list
							printCList((char *) packet);
							break;
						case PACKET_CMD_CHL:
							// display channel list
							printChList((char *) packet);
							break;
						case PACKET_CMD_MSG_D:
							// display message
							printf("%s\n", (char *) packet);
							break;
						
						default:
							printf("parsePacket: invalid return\n");
							break;
					}
					
					free(packet);
				}
			}
			
		} // SOCKET CHECK
		
		
	} // MAIN LOOP
	
	
	printf("Loop ended\n");
	
	// free/close everything
	close(sockfd);
	free(ipStr);
	freePQ(mPQ);
	
}

// Displays help
void printHelp() {
	printf("Help\nCommands:\n");
	printf("%s - Displays help\n", INPUT_CMD_HELP);
	printf("%s - Displays various information pieces\n", INPUT_CMD_INFO);
	printf("%s - Disconnect from server\n", INPUT_CMD_QUIT);
	printf("%s - Tell server to shutdown\n", INPUT_CMD_QUITALL);
	printf("%s - Request list of channels from server\n", INPUT_CMD_LIST_CH);
	printf("%s - Request list of clients from server\n", INPUT_CMD_LIST_CL);
	printf("%s # - Request creation of channel w/ given name (can be character or number)\n", INPUT_CMD_CH_CREATE);
	printf("%s # - Request to join given channel (name of channel)\n", INPUT_CMD_CH_JOIN);
	printf("%s - Request to leave current channel\n", INPUT_CMD_CH_LEAVE);
	printf("%s - Set type on to global\n", INPUT_CMD_TYPE_GLOBAL);
	printf("%s - Set type on to current channel\n", INPUT_CMD_TYPE_CHANNEL);
	printf("Inputs without a '/' at the beginning will be treated as messages and sent depending on the current type on setting\n");
}

// Displays info
void printInfo(int fd, int CN, int QP, char cCh, int type) {
	printf("Socket fd: %d\n", fd);
	if (CN == -1) {
		printf("Client Number: None (in queue?)\n");
	}else{
		printf("Client Number: %d\n", CN);
	}
	if (QP == -1) {
		printf("Queue Position: Not in queue\n");
	}else{
		printf("Queue Position: %d\n", QP);
	}
	if (cCh == '\0') {
		printf("Current Channel: None\n");
	}else{
		printf("Current Channel: %c\n", cCh);
	}
	
	printf("Typing on: ");
	if (type == TYPE_GLOBAL) {
		printf("Global\n");
	}else if (type == TYPE_CHANNEL) {
		printf("Channel\n");
	}else{
		printf("%d\n", type);
	}
}

// Send to server, loops to send all
// return -1 for error
//	0 for disconnectr
//	1 for no problems
int sendLoop(int fd, unsigned char *packet) {
	if (packet == NULL) {
		printf("sendLoop error: packet null\n");
	}
	
	int ret = -1, err = -1, loop = 1;
	int sent = 0, len = (int) strlen((char *) packet);
	
	while (loop&&(sent < len)) {
		ret = send(fd, (packet+sent), (len-sent), MSG_NOSIGNAL);
		err = errno;
		if (ret < 0) {
			printf("sendLoop, less than 0 sent, ret: %d, errno: %d\n", ret, err);
		//	loop = 0;
			return -1;
		}else if (ret == 0) {
			printf("sendLoop, 0 sent, errno: %d\n", err);
		//	loop = 0;
			return 0;
		}else{
			sent += ret;
		}
	}
	return 1;
}

// Sends quit to server (not really needed)
void sendQuit(int fd) {
	// Build packet
	unsigned char *packet = packetCreateQuit();
	
	// Send
	sendLoop(fd, packet);
	
	// Free
	free(packet);
}

// Sends quitall command to server
void sendQuitAll(int fd) {
	// Build packet
	unsigned char *packet = packetCreateQuitAll();
	
	// Send
	sendLoop(fd, packet);
	
	// Free
	free(packet);
}

// Send global message to server
void sendGlobal(int fd, char *str) {
	// Check if length within limit
	if (strlen(str) > MAX_MSG) {
		return;
	}
	
	// Build packet
	unsigned char *packet = packetCreateMsgG(str);
	
	// Send
	sendLoop(fd, packet);
	
	// Free
	free(packet);
}

// Send channel message to server
// Main loop should verify current channel is set
void sendChannel(int fd, char *str) {
	// Check if length within limit
	if (strlen(str) > MAX_MSG) {
		return;
	}
	
	// Build packet
	unsigned char *packet = packetCreateMsgC(str);
	
	// Send
	sendLoop(fd, packet);
	
	// Free
	free(packet);
}

// Send client list request to server
void sendGetCList(int fd) {
	// Build packet
	unsigned char *packet = packetCreateLClR();
	
	// Send
	sendLoop(fd, packet);
	
	// Free
	free(packet);
}

// Send channel list request to server
void sendGetChList(int fd) {
	// Build packet
	unsigned char *packet = packetCreateLChR();
	
	// Send
	sendLoop(fd, packet);
	
	// Free
	free(packet);
}

// Send create channel request to server
// Main loop checks if c is valid channel name
void sendCreateCh(int fd, char c) {
	// Build packet
	unsigned char *packet = packetCreateNewChR(c);
	
	// Send
	sendLoop(fd, packet);
	
	// Free
	free(packet);
}

// Send join channel request to server
// Main loop checks if c is valid channel name
void sendJoinCh(int fd, char c) {
	// Build packet
	unsigned char *packet = packetCreateJoinChR(c);
	
	// Send
	sendLoop(fd, packet);
	
	// Free
	free(packet);
}

// Send leave channel request to server
void sendLeaveCh(int fd) {
	// Build packet
	unsigned char *packet = packetCreateLeaveCh();
	
	// Send
	sendLoop(fd, packet);
	
	// Free
	free(packet);
}

// Prints received client list
// Little error checking, assumes correct from server
void printCList(char *arr) {
	printf("Client Number\t\tCurrent Channel\n");
	
	// the stuff in the packet should be seperated by spaces
	// CN CH CN CH ... ect
	char *tok = strtok(arr, " ");
	char *cn = NULL, *ch = NULL;
	
	while (tok != NULL) {
		cn = tok;
		tok = strtok(NULL, " ");
		if (tok != NULL) {
			ch = tok;
			if (strlen(ch) == 1) {
				if (validChSymbol(ch[0])) {
					printf("%s\t\t\t%s\n", cn, ch);
				}else{
					printf("%s\t\t\tN/A\n", cn);
				}
			}
		}
		
		tok = strtok(NULL, " ");
	}
}

// Prints received channel list
// Little error checking, assumes correct from server
void printChList(char *arr) {
	printf("Channel Name\tClients\n");
	
	char *tok = strtok(arr, " ");
	char *ch = NULL, *clients = NULL;
	
	while (tok != NULL) {
		if (strlen(tok) == 1) {
			ch = tok;
			if (validChSymbol(ch[0])) {
				tok = strtok(NULL, " ");
				if (tok != NULL) {
					clients = tok;
					printf("%s\t\t%s\n", ch, clients);
				}
			}
		}
		
		tok = strtok(NULL, " ");
	}
	printf("End of channel list\n");
}









