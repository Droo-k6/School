#include "server.h"

int main (int argc, char **argv) {	
	
	////////////////////////////////////////////////////////////////////////////////
	/* Connection Setup */
	
	// Setting up listening port
	printf("Setting up listening port...\n");
	int ret = -1, err = -1;
	int listenfd;
	sockAddr_in servaddr;

	listenfd = socket(AF_INET, SOCK_STREAM, 0);
	err = errno;
	if (listenfd == -1) {
		printf("socket error, errno: %d\n", err);
		return -1;
	}
	
	servaddr.sin_family = AF_INET;
	servaddr.sin_addr.s_addr = htonl(INADDR_ANY);
	servaddr.sin_port = htons(PORT);
	memset(servaddr.sin_zero, '\0', sizeof(servaddr.sin_zero));
	
	ret = bind(listenfd, (sockAddr *) &servaddr, sizeof(servaddr));
	err = errno;
	if (ret == -1) {
		printf("bind error, errno: %d\n", err);
		return -1;
	}

	ret = listen(listenfd, MAX_LISTEN);
	err = errno;
	if (ret == -1) {
		printf("listen error, errno: %d\n", err);
		return -1;
	}
	
	
	/* End of Connection Setup */
	////////////////////////////////////////////////////////////////////////////////
	/* Pre-loop setup */
	
	
	// Connection Queue
	queue *queueObj = buildQueue();
	
	
	// Array of all clients
	// Master copy, all allocated
	client **clientArray = buildClientArrayFull(MAX_CONNECTIONS);
	int totalClients = 0;
	
	// Array of active clients
	// Does not allocate clients, just has pointers back to master copy
	client ** activeClients = buildClientArray(MAX_ACTIVE_CONNECTIONS);
	int totalActive = 0;
	
	// Array of queued clients?
	
	// Array of all channels
	channel ** channelArray = buildChannelArray(MAX_CHANNELS);
	int totalChannels = 0;
	
	
	// fdSet: master set
	// fdSetCopy: copy of master set
	fd_set fdSet, fdSetCopy;
	
	// value for high fd, currently listenfd
	int fdHigh = listenfd;
	
	// timeout stuff, set to 0 to just poll
	// ctv to copy tv for select
	timeVal tv = {.tv_sec = 0, .tv_usec = 0}, ctv;
	
	// add stdin and listenfd
	FD_SET(0, &fdSet);
	FD_SET(listenfd, &fdSet);
	
	
	// Stuff for server to be able to type/talk
	// Current channel (channel messages)
	channel *curCh = NULL;
	// Type on global/channel
	int typeOn = 0;
	
	
	// Buffers
	// Input Buffer
	char inBuf[MAX_BUF_INPUT+1];
	// Receive Buffer
	char rvBuf[MAX_BUF_RECV+1];
	// No send buffer needed
	
	
	/* Pre-loop setup complete */
	////////////////////////////////////////////////////////////////////////////////
	/* Loop */
	
	
	printf("Setup complete\n\n");
	printHelpCmd();
	printf("\n\n");	

	int loop = 1, i = 0;

	client *cObj = NULL, *cObj1 = NULL, *cObj2 = NULL;
	channel *chObj = NULL, *chObj1 = NULL, *chObj2 = NULL;
	char ch = '\0', ch1 = '\0', ch2 = '\0';
	int fd = -1, num1 = -1, num2 = -1;

	//printf("segcheck, starting loop\n");
	while (loop) {
		// for if changes in any array/list that will need others to refresh
		
		// Need event driven
		// so everything needs to happen at instant, not next iteration
		// if queue removes disconnect, should have clientArr remove right then
		// as well as have fds refreshed
		
		//printf("segcheck, checking queue\n");
		// pop off of queue if slots available
		while ((totalActive < MAX_ACTIVE_CONNECTIONS)&&(getQSize(queueObj) > 0)) {
		//	printf("queue pop, totalActive:%d, max:%d, qSize:%d\n", totalActive, MAX_ACTIVE_CONNECTIONS, getQSize(queueObj));
			cObj = dequeue(queueObj);
			if (cObj == NULL) {
				// Something wrong
				printf("ERROR: dequeued client null\n");
			}else if (addActiveClient(cObj, activeClients, MAX_ACTIVE_CONNECTIONS, &totalActive)){
		//		printf("addActiveClient: returned true\n");
		//		printf("crash test\n");

				// disconnected client can be found from SIGPIPE signal from send
				// update client on out of queue & client number
			//	ret = sendQPos(getFd(cObj), 0);
			//	printf("sendQPos ret: %d\n", ret);

				sendCurCN(getFd(cObj), getCNum(cObj));
				
				if (getFd(cObj) > fdHigh) {
					fdHigh = getFd(cObj);
				}

		//		printf("crash yet?\n");
			}else{
				// throw error
				printf("ERROR: no slot for dequeued client\n");
				
				if (getFd(cObj) == fdHigh) {
					fdHigh = -1;
				}

				// send notification
				sendNoti(getFd(cObj), "Failed to remove off queue, disconnecting\n");
				
				// Remove client
				removeClient(cObj, clientArray, MAX_CONNECTIONS, &totalClients);

				FD_CLR(fd, &fdSet);
			}
			printf("end of queue pop\n");
		}

		//printf("segcheck, checking queue changed\n");
		// refresh queue if needed
		if (getChanged(queueObj)) {
			refreshQueue(queueObj, clientArray, MAX_CONNECTIONS, &totalClients);
		}

		
		// Find highest fd if needed
		if (fdHigh < listenfd) {
			fdHigh = listenfd;
			for (i = 0; i < MAX_CONNECTIONS; i++) {
				if (clientArray[i] != NULL) {
					fd = getFd(clientArray[i]);
					if (fd > listenfd) {
						if (fd > fdHigh) {
							fdHigh = fd;
						}
					}
				}
			}
		}
		
		//printf("segcheck, checking select\n");
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
		
		//printf("segcheck, checking stdin\n");
		// Check stdin
		if (FD_ISSET(0, &fdSetCopy)) {
		//	printf("STDIN reading\n");
			
			zeroA(inBuf, MAX_BUF_INPUT+1);
			
			fgets(inBuf, MAX_BUF_INPUT, stdin);
			
			strip(inBuf, strlen(inBuf));
			
		//	printf("input buffer: %s\n", inBuf);
			
			// Parse input and handle return
			ret = parseInput(inBuf, MAX_BUF_INPUT);
			switch (ret) {
				case INPUT_KEY_INVALID:
					printf("Invalid command, type '%s' for list of commands\n", INPUT_CMD_HELP);
					break;
				case INPUT_KEY_MSG:
					if (strlen(inBuf) > MAX_MSG) {
						printf("Input error: message too long\n");
					}else if (typeOn == TYPE_GLOBAL) {
						sendGlobalS(activeClients, MAX_ACTIVE_CONNECTIONS, inBuf);
					}else if (typeOn == TYPE_CHANNEL) {
						if (curCh == NULL) {
							printf("Input error: not in channel\n");
						}else{
							sendChannelS(curCh, inBuf);
						}
					}
					break;
				case INPUT_KEY_HELP:
					printHelpCmd();
					break;
				case INPUT_KEY_INFO:
					// display info
					printInfo();
					break;
				case INPUT_KEY_QUIT:
					printf("Shutting down server...\n");
					sendQuitAll(activeClients, MAX_ACTIVE_CONNECTIONS);
					loop = 0;
					break;
				case INPUT_KEY_QUEUE:
					printf("Display queue\n");
					printQueue(queueObj);
					break;	
				case INPUT_KEY_LIST_CH:
					printf("Display channel list\n");
					printChannels(channelArray, MAX_CHANNELS, totalChannels);
					break;
				case INPUT_KEY_LIST_CL:
					printf("Display client list\n");
					printClients(clientArray, MAX_CONNECTIONS, totalClients);
					break;
				case INPUT_KEY_CH_CREATE:
					// parseInput handles valid character
					ch = inBuf[0];
					// create channel
					curCh = createAddChannel(channelArray, &totalChannels, MAX_CHANNELS, ch);
					if (curCh == NULL) {
						printf("Channel creation failed\n");
					}
					break;
				case INPUT_KEY_CH_JOIN:
					// parseInput handles valid character
					ch = inBuf[0];
					curCh = channelExists(channelArray, MAX_CHANNELS, ch);
					if (curCh == NULL) {
						printf("Channel doesn't exist\n");
					}else{
						printf("Joined '%c'\n", ch);
					}
					break;	
				case INPUT_KEY_CH_LEAVE:
					printf("Leave channel\n");
					if (curCh != NULL) {
						// Check if channel empty
						if (getCCnt(curCh) == 0) {
							// if channel empty, delete
							removeChannel(channelArray, MAX_CHANNELS, &totalChannels, curCh);
						}
						curCh = NULL;
					}else{
						printf("Not in channel\n");
					}
					break;
				case INPUT_KEY_TYPG:
					printf("Typing on global\n");
					typeOn = TYPE_GLOBAL;
					break;
				case INPUT_KEY_TYPC:
					if (curCh != NULL) {
						printf("Typing on channel '%c'\n", getSym(curCh));
						typeOn = TYPE_CHANNEL;
					}else{
						printf("No channel selected\n");
					}
					break;
				default:
					printf("parseInput error, not valid return: %d\n", ret);
					break;
			}
		} // END STDIN
		
		//printf("segcheck, checking listenfd\n");
		// Check listenfd for new connections
		if (FD_ISSET(listenfd, &fdSetCopy)) {
			printf("listenfd reading\n");
			
			socklen_t clilen;
			sockAddr_in cliaddr;
			
			clilen = sizeof(cliaddr);
			ret = accept(listenfd, (sockAddr *) &cliaddr, &clilen);
			err = errno;
			if (ret == -1) {
				printf("\taccept error, errno: %d\n", err);
			}else if (totalClients < MAX_CONNECTIONS) {
				printf("\tConnection received, %d\n", ret);
				
				fd = ret;
				cObj = addConnection(clientArray, MAX_CONNECTIONS, &totalClients, fd);
				if (cObj == NULL) {
					printf("\tERROR: No slot for client, disconnecting\n");
					close(fd);
				}else{
					if (fd > fdHigh) {
						fdHigh = fd;
					}
					FD_SET(fd, &fdSet);
					
					// Add to connection queue, queue check will deal with
					enqueue(queueObj, cObj);

					printf("\tAdded to queue\n");
				}
			}else{
				printf("\tConnections full, closing\n");
				close(ret);
			}
		} // END LISTENFD
		
		//printf("segcheck, checking clients\n");
		// Go through active clients
		for (i = 0; i < MAX_ACTIVE_CONNECTIONS; i++) {
			cObj = activeClients[i];
			if (cObj != NULL) {
				// If client is in active array
				// fd/cn should be set
				fd = getFd(cObj);
				if (FD_ISSET(fd, &fdSetCopy)) {
					zeroA(rvBuf, MAX_BUF_RECV+1);
					
					ret = recv(fd, rvBuf, MAX_BUF_RECV, 0);
					err = errno;
					if (ret == -1) {
						printf("recv error, -1 ret, errno: %d\n", err);
					}else if (ret < 0) {
						printf("recv error, ret: %d, errno: %d\n", ret, err);
					}else if (ret == 0) {
						printf("Close received, removing fd %d\n", fd);

						// Connection closed
						if (fd == fdHigh) {
							fdHigh = -1;
						}
						close(fd);
						
						removeActiveClient(cObj, activeClients, MAX_ACTIVE_CONNECTIONS, &totalActive);
						removeClient(cObj, clientArray, MAX_CONNECTIONS, &totalClients);

						FD_CLR(fd, &fdSet);
					}else{
						unsigned char *cPBuf = getBuf(cObj);
						packetQueue *cPQ = getPQ(cObj);
						
						bufferAddQueue(cPQ, cPBuf, PB_MAX, (unsigned char *) rvBuf);
						
						while (loop&&(pqSize(cPQ) > 0)) {
							// parseStringClientPacket
							unsigned char *packet = pqDequeue(cPQ);
							ret = parsePacket(packet);
							switch (ret) {
								case PACKET_CMD_INVALID:
									printf("parsePacket: invalid packet\n");
									break;
								case PACKET_CMD_QUIT_C:
									// client quit
									break;
								case PACKET_CMD_QUIT_S:
									// Tell server to quit
									sendQuitAll(clientArray, MAX_CONNECTIONS);
									loop = 0;
									printf("server shutdown received from client (%d)\n", getCNum(cObj));
									break;
								case PACKET_CMD_MSG_G:
									sendGlobalC(activeClients, MAX_ACTIVE_CONNECTIONS, cObj, (char *) packet);
									break;
								case PACKET_CMD_MSG_C:
									// send
									if (getCh(cObj) == NULL) {
										sendCurCh(fd, '-');
										sendNoti(fd, "channel not set");
									}else{
										sendChannelC(cObj, (char *) packet, curCh);
									}
									break;
								case PACKET_CMD_CH_CREATE:
									// Client requests to create given channel
									// Send notification of current channel
									// Send message notification
									ch = (char) packet[0];
								//	printf("Channel create request '%c'\n", ch);
									// find channel
									chObj = channelExists(channelArray, MAX_CHANNELS, ch);
									if (chObj == NULL) {
										chObj = createAddChannel(channelArray, &totalChannels, MAX_CHANNELS, ch);
										if(chObj != NULL) {
											// check if in channel
											chObj1 = getCh(cObj);
											if (chObj1 != NULL) {
												removeClientFromCh(chObj1, cObj);
												if (curCh != chObj1) {
													if (getCCnt(chObj1) == 0) {
														removeChannel(channelArray, MAX_CHANNELS, &totalChannels, chObj1);
													}
												}
											}

											if (addClientToChannel(chObj, cObj)) {
								//				printf("Channel created\n");
												sendCurCh(fd, ch);
												sendNoti(fd, "channel created");
											}else{
								//				printf("Move to channel failed\n");
												sendCurCh(fd, '-');
												sendNoti(fd, "channel created but move failed");
											}
										}else{
								//			printf("Create channel failed\n");
											sendCurCh(fd, '-');
											sendNoti(fd, "channel creation failed");
										}
									}else{
								//		printf("Channel exists\n");
										// exists
										sendCurCh(fd, '-');
										sendNoti(fd, "channel already exists");
										
									}
									break;
								case PACKET_CMD_CH_JOIN:
									// Client requests to join given channel
									ch = (char) packet[0];
									// find channel
									chObj = channelExists(channelArray, MAX_CHANNELS, ch);
									if (chObj != NULL) {
									//	printf("channel join, exists\n");
										chObj1 = getCh(cObj);
										if (chObj1 != NULL) {
									//		printf("client in different channel\n");
											removeClientFromCh(chObj1, cObj);
											// delete channel if empty
											if (getCCnt(chObj1) == 0) {
									//			printf("channel empty\n");
												// if server is not part of
												if (curCh != chObj1) {
									//				printf("server not in channel\n");
													removeChannel(channelArray, MAX_CHANNELS, &totalChannels, chObj1);
												}
											}
										}
										
										// check if channel full
										if (getCCnt(chObj) >= CHANNEL_MAX_CLIENTS) {
											sendCurCh(fd, '-');
											sendNoti(fd, "channel is full");
										}else{
									//		printf("adding to channel\n");
											addClientToChannel(chObj, cObj);
											sendCurCh(fd, ch);
										}
									}else{
										// doesn't exist (dont move out of current channel)
										sendNoti(fd, "channel does not exist");
									}
									break;
								case PACKET_CMD_CH_LEAVE:
									// Client requests to leave current channel
									// Send notification of current channel
									// Send message notification
									chObj = getCh(cObj);
									if (chObj == NULL) {
										sendCurCh(fd, '-');
										sendNoti(fd, "Not in channel");
									}else{
										removeClientFromCh(chObj, cObj);
										sendCurCh(fd, '-');

										// delete if server is not part of channel
										if (curCh != chObj) {
											// Check if channel empty
											if (getCCnt(chObj) == 0) {
												// if channel empty, delete
												removeChannel(channelArray, MAX_CHANNELS, &totalChannels, chObj);
											}
										}
									}
									break;
								case PACKET_CMD_NOTI:
									// server should not receive, display anyway
									printf("(client notification): %s\n", (char *) packet);
									break;
								case PACKET_CMD_QP:
									// client requests queue position
									// should not be in active check
									sendQPos(fd, getQPos(cObj1));
									break;
								case PACKET_CMD_CN:
									// client requests client number
									sendCurCN(fd, getCNum(cObj1));
									break;
								case PACKET_CMD_CH:
									// client requests current channel name
									chObj = getCh(cObj);
									if (chObj == NULL) {
										sendCurCh(fd, ' ');
									}else{
										sendCurCh(fd, getSym(chObj));
									}
									break;
								case PACKET_CMD_CL:
									// client requests client list
									sendClList(fd, activeClients, MAX_ACTIVE_CONNECTIONS);
									break;
								case PACKET_CMD_CHL:
									// client requests channel list
									sendChList(fd, channelArray, MAX_CHANNELS);
									break;
								case PACKET_CMD_MSG_D:
									// Server should not receive, still going to display
									printf("(MSG_D): %s\n", (char *) packet);
									break;
									
								default:
									printf("parsePacket: invalid return\n");
									break;
							}
							
							free(packet);
						} // End packet queue loop
					}
				} // End FD_ISSET
			} // Index null
		} // End going through active clients
		
		
		// Go through queue clients?
		
		
		
		
		//printf("segcheck, end of loop\n");
	}
	
	// free allocated & close connections
	printf("\n\nMemory cleanup...\n");

	printf("closing listenfd\n");
	close(listenfd);

	printf("queue\n");
	freeQueue(queueObj);
	
	printf("channel array\n");
	freeChannelArrFull(channelArray, MAX_CHANNELS);

	printf("client array\n");
	freeClientArrFull(clientArray, MAX_CONNECTIONS);

	printf("active client array\n");
	freeClientArr(activeClients);
	

	printf("END OF PROGRAM\n");
}

/* End of Main */
////////////////////////////////////////////////////////////////////////////////////////////
/* Define functions */


// Printing stuff

// Print commands
void printHelpCmd() {
	printf("%s\t\tDisplay list of commands\n", INPUT_CMD_HELP);
	printf("%s\t\tDisplay server information\n", INPUT_CMD_INFO);
	printf("%s\t\tShutdown server\n", INPUT_CMD_QUIT);
	printf("%s\t\tDisplay the queue\n", INPUT_CMD_QUEUE);
	printf("%s\tDisplay list of channels\n", INPUT_CMD_CHL);
	printf("%s\tDisplay list of active clients\n", INPUT_CMD_CL);
	printf("%s #\tCreate channel (# a letter or number)\n", INPUT_CMD_CCH);
	printf("%s #\t\tJoin channel (# - symbol of channel)\n", INPUT_CMD_JCH);
	printf("%s\t\tLeave current channel\n", INPUT_CMD_LCH);
	printf("%s\t\tTalk on global\n", INPUT_CMD_TYPG);
	printf("%s\tTalk on current channel\n", INPUT_CMD_TYPC);
}

// Print information
void printInfo() {
	printf("Print info\n");
}

// Prints Queue
void printQueue(queue *q) {
	printf("Queue size: %d\n", getQSize(q));
	printf("FD, Queue Position\n");
	node *trav = q->head;
	while (trav != NULL) {
		printf("%d, %d\n", getFd(trav->data), getQPos(trav->data));
		trav = trav->next;
	}
	printf("End of quue\n");
}

// Print Channels
void printChannels(channel **arr, int size, int total) {
	printf("Total channels: %d\n", total);
	printf("Channel Name\t\tClients\n");
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			printf("%c\t\t\t%d\n", getSym(arr[i]), getCCnt(arr[i]));
		}
	}
	printf("End of channel list\n");
}

// Print Clients
void printClients(client **arr, int size, int count) {
	printf("Client count: %d\n", count);
	printf("FD\t\tClient Number\t\tQueue Position\t\tCurrent Channel\n");
	int i = 0;
	for (; i < size; i++) {
	//	printf("i:%d\n", i);
		if (arr[i] != NULL) {
		//	printf("\tNot Null\n");
			if (getFd(arr[i]) > 0) {
			//	printf("\tValid fd\n");
				printf("%d\t\t", getFd(arr[i]));
				printf("%d\t\t\t", getCNum(arr[i]));
				printf("%d\t\t\t", getQPos(arr[i]));
				if (getCh(arr[i]) == NULL) {
					printf("None\n");
				}else{
					printf("%c\n", getSym(getCh(arr[i])));
				}
			}
		}
	}
	printf("End of client list\n");
}


// Connection
client * addConnection(client **arr, int size, int *total, int fd) {
	// Find slot
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			// Check if open
			if (getFd(arr[i]) == -1) {
				setFd(arr[i], fd);
				
				*total++;
				return arr[i];
			}
		}
	}
	// No slot found
	return NULL;
}

// do everything needed to add active client
int addActiveClient(client *c, client **arr, int size, int *active) {
	printf("addActiveClient, CN:%d, fd:%d\n", getCNum(c), getFd(c));

	// Find empty slot
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] == NULL) {
			break;
		}
	}
	// No empty slot found
	if (i == size) {
		return 0;
	}
	
	(*active)++;
	
	// Add to active array, 
	arr[i] = c;
	setCNum(c, i+1);
	
	return 1;
}

// Remove from active clients
void removeActiveClient(client *c, client **arr, int size, int *total) {
	// Find in array
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			if (arr[i] == c) {
				arr[i] = NULL;
				(*total)--;
				return;
			}
		}
	}
	// failed to find
	printf("removeActiveClient error: failed to find client\n");
}

// Removes client from client array
void removeClient(client *c, client **arr, int size, int *total) {
	// Find client
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			if (arr[i] == c) {
				break;
			}
		}
	}
	// client not found
	if (i == size) {
		return;
	}
	
//	resetClient(c);
//	Reset the client here so updates can be done
	
	(*total)--;
	
	// Close connection
	close(getFd(c));

	// Reset basic vlaues
	setFd(c, -1);
	setCNum(c, -1);
	setQPos(c, -1);
	
	// Remove from channel
	if (getCh(c) != NULL) {
		channel *ch = getCh(c);
		removeClientFromCh(ch, c);
	}
//	setCCh(c, NULL);
	
	
	// Reset packet buffer
	zeroA((char *) (c->buf), PB_MAX+1);
	
	// Reset packet queue
	if (getPQ(c) != NULL) {
		freePQ(getPQ(c));
	}
	c->pq = pqCreate();
	
	// Done
}

// refresh queue, done here for update messages
void refreshQueue(queue *q, client **arr, int size, int *total) {
	if (q->head == NULL) {
		// Empty
		return;
	}
	
	node *trav = q->head;
	int i = 1;
	while (trav != NULL) {
		if (getQPos(trav->data) != i) {
			setQPos(trav->data, i);
			
			// Update client on position
			int ret = sendQUpdate(trav->data);
			if (!ret) {
				// problem sending, remove
				client *c = trav->data;
				
				// remove from queue
				// could be sped up by removing here
				removeNode(q, c);
				
				// remove from client array
				removeClient(c, arr, size, total);
			}
		}
		trav = trav->next;
		i++;
	}
	
	setChanged(q, 0);
}

// Send Queue update message
// different than others to determine disconnect
int sendQUpdate(client *c) {
	int pos = getQPos(c), fd = getFd(c), ret = -1, err = -1;
	
	unsigned char *packet = packetCreateCurQPos(pos);
	ret = sendLoop(getFd(c), packet);
	
	if (!ret) {
		printf("sendQUpdate error: error with sending\n");
	}
	
	return ret;
}

// Messages

// looping send
// return -1 for error
//	0 for disconnectr
//	1 for no problems
int sendLoop(int fd, unsigned char *packet) {
//	printf("sendLoop crash test starting, fd:%d\n", fd);
	if (packet == NULL) {
		printf("sendLoop error: packet null\n");
	}
	
	int ret = -1, err = -1, loop = 1;
	int sent = 0, len = (int) strlen((char *) packet);
	
	while (loop&&(sent < len)) {
		// MSG_NOSIGNAL to stop sigpipe signal
		ret = send(fd, (packet+sent), (len-sent), MSG_NOSIGNAL);
		err = errno;
		if (ret < 0) {
			printf("sendLoop, less than 0 sent, ret: %d, errno: %d\n", ret, err);
			loop = 0;
			
			return -1;
		}else if (ret == 0) {
			printf("sendLoop, 0 sent, errno: %d\n", err);
			loop = 0;
			
			return 0;
		}else{
			sent += ret;
		}
	}
//	printf("sendloop crash test ending\n");
	return 1;
}

// Send notification to the client
void sendNoti(int fd, char *str) {
/*	char temp[PACKET_MAX_OPTIONAL+1];
	zeroA(temp, PACKET_MAX_OPTIONAL+1);
	
	sprintf(temp, "(Server Notification): %s", str);
	
	printf("%s\n", temp);*/
	
	unsigned char *packet = packetCreateNoti(str);
	
	sendLoop(fd, packet);
}

// Build global message from server input
void sendGlobalS(client **arr, int size, char *str) {
	char temp[PACKET_MAX_OPTIONAL+1];
	zeroA(temp, PACKET_MAX_OPTIONAL+1);
	
	sprintf(temp, "Global(server): %s", str);
	
	printf("%s\n", temp);
	
	unsigned char *packet = packetCreateMsgD(temp);
	
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			if (getFd(arr[i]) > 0) {
				sendLoop(getFd(arr[i]), packet);
			}
		}
	}
}

// Build global message from client message
void sendGlobalC(client **arr, int size, client *c, char *str) {
	char temp[PACKET_MAX_OPTIONAL+1];
	zeroA(temp, PACKET_MAX_OPTIONAL+1);
	
	sprintf(temp, "Global(#%d): %s", getCNum(c), str);
	
	printf("%s\n", temp);
	
	unsigned char *packet = packetCreateMsgD(temp);
	
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			if (getFd(arr[i]) > 0) {
				sendLoop(getFd(arr[i]), packet);
			}
		}
	}
}

// Build channel message from server input
void sendChannelS(channel *ch, char *str) {
	char temp[PACKET_MAX_OPTIONAL+1];
	zeroA(temp, PACKET_MAX_OPTIONAL+1);
	
	sprintf(temp, "Channel(%c)(server): %s", getSym(ch), str);
	
	printf("%s\n", temp);
	
	unsigned char *packet = packetCreateMsgD(temp);
	
	client **arr = getClientArr(ch);
	int i = 0;
	for (; i < CHANNEL_MAX_CLIENTS; i++) {
		if (arr[i] != NULL) {
			if (getFd(arr[i]) > 0) {
				sendLoop(getFd(arr[i]), packet);
			}
		}
	}
}

// Build channel message from client
// 3rd parameter for servers current channel
void sendChannelC(client *c, char *str, channel *sCh) {
	if (getCh(c) == NULL) {
		// Shouldn't happen
		return;
	}
	
	channel *ch = getCh(c);
	
	char temp[PACKET_MAX_OPTIONAL+1];
	zeroA(temp, PACKET_MAX_OPTIONAL+1);
	
	sprintf(temp, "Channel(%c)(#%d): %s", getSym(ch), getCNum(c), str);
	
	if (sCh == ch) {
		printf("%s\n", temp);
	}
	
	unsigned char *packet = packetCreateMsgD(temp);
	
	client **arr = getClientArr(ch);
	int i = 0;
	for (; i < CHANNEL_MAX_CLIENTS; i++) {
		if (arr[i] != NULL) {
			if (getFd(arr[i]) > 0) {
				sendLoop(getFd(arr[i]), packet);
			}
		}
	}
}

// Sends quit message to all clients
void sendQuitAll(client **arr, int size) {
	unsigned char *packet = packetCreateQuitAll();
	
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			if (getFd(arr[i]) > 0) {
				sendLoop(getFd(arr[i]), packet);
			}
		}
	}
}

// Send client list
void sendClList(int fd, client **arr, int size) {
	char temp[PACKET_MAX_OPTIONAL+1];
	zeroA(temp, PACKET_MAX_OPTIONAL+1);
	
	char templine[5];
	
	// Build list
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			if (getFd(arr[i]) > 0) {
			//	sendLoop(getFd(arr[i]), packet);
			// seperate all by spaces
			// client number-channel
				
				char ch = '-';
				if (getCh(arr[i]) != NULL) {
					ch = getSym(getCh(arr[i]));
				}
				
				zeroA(templine, 5);
				sprintf(templine, "%d %c ", getCNum(arr[i]), ch);
				
				strcat(temp, templine);
			}
		}
	}
	
	unsigned char *packet = packetCreateLCl(temp);
	
	sendLoop(fd, packet);
}

// Send channel ist
void sendChList(int fd, channel **arr, int size) {
	char temp[PACKET_MAX_OPTIONAL+1];
	zeroA(temp, PACKET_MAX_OPTIONAL+1);
	
	char templine[4];
	
	// Build list
	int i = 0;
	for (; i < size; i++) {
		if (arr[i] != NULL) {
			// Build line
			// channel name (space) client count
			zeroA(templine, 4);
			sprintf(templine, "%c %d ", getSym(arr[i]), getCCnt(arr[i]));
			
			strcat(temp, templine);
		}
	}
	
	unsigned char *packet = packetCreateLCh(temp);
	
	sendLoop(fd, packet);
}

// Send current client number
void sendCurCN(int fd, int cn) {
//	printf("sendCurCN crash test: start, fd:%d, cn:%d\n", fd, cn);
	unsigned char *packet = packetCreateCurCN(cn);
//	printf("sendCurCN crash test: send loop\n");
	int ret = sendLoop(fd, packet);
//	printf("sendCurCN crash test: ending, ret:%d\n", ret);
}

// Send current queue position
int sendQPos(int fd, int pos) {
	unsigned char *packet = packetCreateCurQPos(pos);
	
	return sendLoop(fd, packet);
}

// Send current channel
void sendCurCh(int fd, char ch) {
	unsigned char *packet = packetCreateCurCh(ch);
	
	sendLoop(fd, packet);
}



