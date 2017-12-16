// main function for program

#include "main.h"

// main function
int main(int argc, char **argv) {	
	// check arguments
	options args = {.valid = 0,.help=0,.promiscuous=0,.toTerminal=0,.toFile=0,.filter=0};
	parseArguments(&args, argc, argv);
	if (!args.valid) {
		// argument parser determined arguments were invalid
		fprintf(stderr,"Invalid arguments, run with \"-h\" for help\n");
		return -1;
	}else if (args.help) {
		// display help
		printf("basic packet sniffer\nArguments-\n");
		printf("\t-h, display help\n");
		printf("\t-p, run with promiscuous mode\n");
		printf("\t-d, output results to screen\n");
		printf("\t-o *, output results to given filename\n");
		printf("\t-f *, filter these packets types\n");
		printf("\tmultiple can be filtered at once by using seperate '-f *' arguments\n");
		printf("\t\tIP, ip packets\n");
		printf("\t\tARP, arp packets\n");
		printf("\t\tTCP, tcp packets\n");
		printf("\t\tUDP, udp packets\n");
		printf("\t\tHTTP, http (tcp, port:80) packets\n");
		printf("\t\tSMTP, smtp (tcp, port:25) packets\n");
		printf("Must run with sudo, and in arguments choose '-d' to display to terminal or '-o *' with appropriate filename to write to, otherwise sniffer will not run.\nTo stop the program, type 'exit' and press enter.");
		return 0;
	}else if (!args.toTerminal && !args.toFile) {
		// missing output option
		fprintf(stderr,"Missing output type, run with \"-h\" for help\n");
		return -1;
	}
	
	if (args.toFile) {
		// attempt to open the file
		if ((args.f = fopen(args.filename,"a")) == NULL) {
			fprintf(stderr, "failed to open file \"%s\": %s", args.filename, strerror(errno));
			return -1;
		}
	}
	
	printf("sniffer starting...\n");
	
	// create raw socket
	// ETH_P_ALL
	// ETH_P_IP | ETH_P_ARP		either by themselves work, but using '|' causes it to not work
	int sockfd = -1;
	if ((sockfd = socket(AF_PACKET, SOCK_RAW, htons(ETH_P_ALL))) < 0) {
		perror("socket() error: ");
		return -1;
	}
	
	if (args.promiscuous) {
		// turn on promiscuous mode
		if (!turnOnPromisc(sockfd)) {
			// failed exit
			fprintf(stderr,"Failed to turn on promiscuous mode\n");
			return -1;
		}
	}
	
	// get size of receive buffer of socket
	int bufferPacketSize = -1;
	int sockoptlen = sizeof(bufferPacketSize);
	if (getsockopt(sockfd, SOL_SOCKET, SO_RCVBUF, &bufferPacketSize, &sockoptlen) < 0) {
		perror("getsockopt() error: ");
		return -1;
	}
	
	// for recvfrom
	//struct sockaddr saddr;
	//int saddr_size;
	// for value read
	int bufferRead;
	
	// fd_set for select() polling
	fd_set fdSet;
	// timeout for select()
	const struct timespec timeout = {.tv_sec = 0, .tv_nsec = 0};
	
	// buffers
	// general buffer
	char buffer[SIZE_BUFFER];
	// packet buffer
	unsigned char *bufferPacket = (unsigned char *)calloc(bufferPacketSize,sizeof(unsigned char));
	
	
	// summary statistics for session
	// init all vals to zero
	snifstats stats = {.packets_received=0,.bytes_received=0,.count_unknown_ethernet=0,.count_unknown_ip=0,.count_ip=0,.count_unknown_app=0,.count_tcp=0,.count_udp=0,.count_arp=0,.count_http=0,.count_smtp=0};
	
	// list of packets
	packetList *list = createPacketList();
	
	// main loop
	while(1) {
		// set fds to stdin and sockfd
		setFD(&fdSet,sockfd);
		// select() polling
		if (pselect(sockfd+1, &fdSet, NULL, NULL, &timeout, NULL) < 0) {
			// error occured, break from loop
			perror("select() error: ");
			break;
		}
		
		// check if stdin set
		if FD_ISSET(0,&fdSet) {
			// process input
			int inputResult = processInput(buffer,SIZE_BUFFER);
			// check result
			if (inputResult == INPUT_ERROR) {
				// error processing the input
				perror("processInput() error: ");
				break;
			}else if (inputResult == INPUT_EXIT) {
				// if exit, exit loop
				break;
			}else{
				// invalid command
				printf("Invalid input, type 'EXIT' in order to exit the sniffer\n");
			}
		}
		
		// check if socket set
		if FD_ISSET(sockfd,&fdSet) {
			// zero out buffer
			clearStr(bufferPacket,bufferPacketSize);
			
			// reset saddr size
			//saddr_size = sizeof(saddr);
			
			// read from socket
			// recvfrom(sockfd, bufferPacket, bufferPacketSize, 0, &saddr, (socklen_t *) &saddr_size)
			// recvfrom(sockfd, bufferPacket, bufferPacketSize, 0, NULL, NULL)
			if ((bufferRead = recvfrom(sockfd, bufferPacket, bufferPacketSize, 0, NULL, NULL)) < 0) {
				perror("recvfrom() error: ");
				break;
			}
			
			// process the buffer
			// recvfrom() appears to only popoff 1 datagram/packet
			// so don't need to worry about seperating out packets
			packetInfo *pi = processPacket(&args, &stats, bufferPacket, bufferRead);
			
			if (pi != NULL) {
				// output accordingly
				displayInfo(&args, pi, buffer, SIZE_BUFFER);
				
				// for this application, don't need to save to list, so just clean
				cleanPacketInfo(&pi);
				
				// add onto list
				//addPacket(list,pi);
				
				if (args.toFile) {
					// flush the file
					fflush(args.f);
				}
			}
		}
	} // while loop
	// exit loop
	
	// report statistics
	reportStats(&args,&stats,buffer,SIZE_BUFFER);
	
	// cleanup
	// packet list
	cleanPacketList(&list);
	// packet buffer
	free(bufferPacket);
	// close socket
	close(sockfd);
	
	// close file if being used
	if (args.toFile) {
		fclose(args.f);
	}
	
	printf("sniffer ended\n");
	
	return 0;
}
