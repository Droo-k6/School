// for functions used by main, and utility functions
#include "main.h"

// parses program arguments into options structure
options parseArguments(options *ops, int argc, char **argv) {
	// check atleast 1 argument to check 
	if (argc < 2) {
		// display error
		fprintf(stderr,"Invalid number of arguments\n");
		return;
	}
	
	ops->valid = 1;
	
	// loop through arguments
	int i = 1;
	for (; i < argc; ++i) {
		// check argument
		if (strcmp(argv[i],"-h") == 0) {
			// display help
			ops->help = 1;
			return;
		}else if (strcmp(argv[i],"-p") == 0) {
			// promiscious mode
			ops->promiscuous = 1;
		}else if (strcmp(argv[i],"-d") == 0) {
			// display packets to terminal
			ops->toTerminal = 1;
		}else if (strcmp(argv[i],"-o") == 0) {
			// display packets to given filename
			// check that another argument exists
			if (argc <= (i+1)) {
				fprintf(stderr,"missing filename for \"-o\" argument\n");
				ops->valid = 0;
				return;
			}
			// advance index
			++i;
			// get filename
			ops->filename = argv[i];
			ops->toFile = 1;
		}else if (strcmp(argv[i],"-f") == 0) {
			// check that another argument available for filter
			if (argc <= (i+1)) {
				fprintf(stderr,"missing filter for \"-f\" argument\n");
				ops->valid = 0;
				return;
			}
			
			// increment i
			++i;
			
			// filter value
			int filter = 0;
			
			// convert to uppercase
			strToUpper(argv[i]);
			
			// parse filter
			if ((filter = parseFilterFlag(argv[i])) == -1) {
				fprintf(stderr,"filter flag \"%s\" invalid\n", argv[i]);
				ops->valid = 0;
				return;
			}
			
			// assign filter to options
			// if exists, or flag with hit
			if (ops->filter > 0) {
				ops->filter |= filter;
			}else{
				ops->filter = filter;
			}
		}else{
			fprintf(stderr,"invalid argument \"%s\"\n", argv[i]);
			ops->valid = 0;
			return;
		}
	}
}

// parses argument token as a filter flag
int parseFilterFlag(const char *flag) {
	int val = -1;
	
	if (strcmp(flag,"IP") == 0) {
		val = 2;
	}else if (strcmp(flag,"ARP") == 0) {
		val = 4;
	}else if (strcmp(flag,"TCP") == 0) {
		val = 8;
	}else if (strcmp(flag,"UDP") == 0) {
		val = 16;
	}else if (strcmp(flag,"HTTP") == 0) {
		val = 32;
	}else if (strcmp(flag,"SMTP") == 0) {
		val = 64;
	}
	
	return val;
}

// check if given ethernet header protcol is filtered
int isEthFiltered(int filter, int proto) {
	if (proto == ETH_P_IP) {
		return (filter & 2);
	}else if (proto == ETH_P_ARP) {
		return (filter & 4);
	}else{
		return 0;
	}
}

// check if given ip header protocol is filtered
int isIPFiltered(int filter, int proto) {
	if (proto == IPPROTO_TCP) {
		return (filter & 8);
	}else if (proto == IPPROTO_UDP) {
		return (filter & 16);
	}else{
		return 0;
	}
}

// check if given app header protocol is filtered
int isAppFiltered(int filter, int proto) {
	if (proto == PROT_HTTP) {
		return (filter & 32);
	}else if (proto == PROT_SMTP) {
		return (filter & 64);
	}else{
		return 0;
	}
}

// attempts to turn on promiscuous mode on socket
// returns true is turned on
int turnOnPromisc(int sockfd) {
	// /usr/include/x86_64-linux-gnu/sys/ioctl.h
	// /usr/include/net/if.h
	// interface request struct
	//		struct ifreq
	// used in SIOCGIFCONF (interface list) request
	//		struct ifconf
	//	gets interface configuration
	
	// for getting interface list
	struct ifconf iocfg;
	
	// buffer
	char buf[SIZE_BUFFER];
	
	iocfg.ifc_len = SIZE_BUFFER;
	iocfg.ifc_buf = buf;
	
	// get interface list
	if (ioctl(sockfd, SIOCGIFCONF, &iocfg) < 0) {
		perror("ioctl() get interface list error");
		return 0;
	}
	
	// go through device list
	int interfaceCount = iocfg.ifc_len / sizeof(struct ifreq);
	int i = 0;
	for (; i < interfaceCount; ++i) {
		// get ifreq, interface request structure
		struct ifreq *ioreq = (iocfg.ifc_req + i);
		
		// if contains "eth", enable promisc
		if (strstr(ioreq->ifr_name,"eth") != NULL) {
			struct ifreq ifr;
			memset(&ifr,0,sizeof(ifr));
			// set device name
			strcpy(ifr.ifr_name,ioreq->ifr_name);
			if (ioctl(sockfd,SIOCGIFFLAGS,&ifr) < 0) {
				perror("ioctl() get error");
				return 0;
			}
			// check if promiscuous flag already set
			if (ifr.ifr_flags & IFF_PROMISC) {
				printf("device \"%s\": Promiscuous mode already on\n", ifr.ifr_name);
				return 1;
			}else{
				// turn on promisc flag
				ifr.ifr_flags |= IFF_PROMISC;
				if (ioctl(sockfd,SIOCGIFFLAGS,&ifr) < 0) {
					perror("ioctl() set error");
				}
				printf("device \"%s\": Promiscuous mode on\n", ifr.ifr_name);
				return 1;
			}
		}
	}
	
	perror("No suitable device found to enable promiscuous mode");
	return 0;
}

// sets up the fdSet for polling
void setFD(fd_set *fdSet, int sockfd) {
	// clear fdSet
	FD_ZERO(fdSet);
	// set fd for stdin and sockfd
	FD_SET(0, fdSet);
	FD_SET(sockfd, fdSet);
}

// process stdin, return result of enum input_command
input_command processInput(char *buffer, const int bufferSize) {
	// get input from stdin
	if (fgets(buffer, bufferSize, stdin) == NULL) {
		return INPUT_ERROR;
	}
	// strip whitespace, convert to uppercase
	strip(buffer);
	strToUpper(buffer);

	// check if exit command
	if (strcmp(buffer,"EXIT") == 0) {
		return INPUT_EXIT;
	}
	
	// return invalid command
	return INPUT_INVALID;
}

// reports statistics to screen
void reportStats(options *args,snifstats *stats,char *buffer,int bufferSize) {
	// clear buffer
	clearStr(buffer,bufferSize);
	
	sprintf(buffer,"Summary\n");
	sprintf(buffer+strlen(buffer),"\tPackets received: %ld\n", stats->packets_received);
	sprintf(buffer+strlen(buffer),"\t\tARP packets: %ld\n", stats->count_arp);
	sprintf(buffer+strlen(buffer),"\t\tIP packets: %ld\n", stats->count_ip);
	sprintf(buffer+strlen(buffer),"\t\tUnknown ethernet packets: %ld\n",stats->count_unknown_ethernet);
	sprintf(buffer+strlen(buffer),"\t\tTCP packets: %ld\n", stats->count_tcp);
	sprintf(buffer+strlen(buffer),"\t\tUDP packets: %ld\n", stats->count_udp);
	sprintf(buffer+strlen(buffer),"\t\tUnknown IP packets: %ld\n",stats->count_unknown_ip);
	sprintf(buffer+strlen(buffer),"\t\tHTTP packets: %ld\n", stats->count_http);
	sprintf(buffer+strlen(buffer),"\t\tSMTP packets: %ld\n", stats->count_smtp);
	sprintf(buffer+strlen(buffer),"\t\tUnknown App packets: %ld\n", stats->count_unknown_app);
	// %.2f
	sprintf(buffer+strlen(buffer),"\tBytes received: %ld, Average per packet: %ld\n", stats->bytes_received,(stats->bytes_received / stats->packets_received));

	// output
	if (args->toFile) {
		fprintf(args->f,"%s\n",buffer);
	}
	// always to screen
	printf("%s\n",buffer);
}

// display info on packet accordingly
// bit messy
void displayInfo(options *args, packetInfo *pi, char *buffer, int bufferSize) {
	// wipe buffer
	clearStr(buffer,bufferSize);
	// second buffer
	char buf[SIZE_BUFFER];
	
	// check if ethernet protocol available, otherwise skip to display
	if (pi->eth_proto == -1) {
		sprintf(buffer,"packet: no information\n");
		goto displayOutput;
	}
	
	// build message onto buffer
	// get string for eth_proto
	sprintf(buf,"unknown");
	if (pi->eth_proto == ETH_P_IP) {
		sprintf(buf,"IP");
	}else if (pi->eth_proto == ETH_P_ARP) {
		sprintf(buf,"ARP");
	}
	sprintf(buffer,"packet:\n\tEthernet header protocol: %d (%s)\n", pi->eth_proto,buf);
	
	// if source mac not available, skip to display
	if (pi->smac == NULL)
		goto displayOutput;
	sprintf(buffer+strlen(buffer),"\tsrc MAC: %s\n\tdst MAC: %s\n",pi->smac,pi->dmac);
	
	// if ip protocol not available, skip to display
	if (pi->ip_proto == -1)
		goto displayOutput;
	
	sprintf(buf,"unknown");
	if (pi->ip_proto == IPPROTO_TCP) {
		sprintf(buf,"TCP");
	}else if (pi->ip_proto == IPPROTO_UDP) {
		sprintf(buf,"UDP");
	}
	sprintf(buffer+strlen(buffer),"\tIP header protocol: %d (%s)\n",pi->ip_proto,buf);
	
	// if source ip not available, skip to display
	if (pi->sip == NULL)
		goto displayOutput;
	sprintf(buffer+strlen(buffer),"\tsrc IP: %s\n\tdst IP: %s\n",pi->sip,pi->dip);
	
	// if source port not available, skip to display
	if (pi->sport == -1)
		goto displayOutput;
	
	sprintf(buffer+strlen(buffer),"\tsrc port: %d\n\tdst port: %d\n",pi->sport,pi->dport);
	
	// if application protocol not available, skip to display
	if (pi->app_proto == -1)
		goto displayOutput;
	
	sprintf(buf,"unknown");
	if (pi->app_proto == PROT_HTTP) {
		sprintf(buf,"HTTP");
	}else if (pi->app_proto == PROT_SMTP) {
		sprintf(buf,"SMTP");
	}
	sprintf(buffer+strlen(buffer),"\tApplication protocol: %s\n",buf);
	
	// label just to make it faster
displayOutput:
	sprintf(buffer+strlen(buffer),"\tend of packet information\n");
	// output
	if (args->toFile) {
		fprintf(args->f,"%s\n",buffer);
	}
	if (args->toTerminal) {
		printf("%s\n",buffer);
	}
}

// utility functions

// converts a string to uppercase if applicable
void strToUpper(char *str) {
	int i = 0;
	for(; i < strlen(str); ++i) {
		str[i] = (char)toupper(str[i]);
	}
}

// strip whitespaces from ends
void strip(char *str) {
	if(str == NULL){
		perror("strip(): array is null");
		exit(-99);
	}
	// call extended strip with strlen
	stripSize(str,strlen(str));
}

// strip with limited size
void stripSize(char *str, int size) {
	// locate beginning and end of string, not including surrounding whitespace
	int i = 0, start = -1, end = -1;
	char c = '\0';
	for (; i < size; ++i) {
		c = str[i];
		if (c == '\0') {
			break;
		}else if (!isspace(c)) {
			if (start == -1) {
				start = i;
			}
			end = i;		
		}
	}
	
	// shift contents over
	if ((start == -1) || (end == -1) || (start > end)) {
		str[0] = '\0';
	}else{
		end++;
		for (i = start; i < end; i++) {
			str[i - start] = str[i];
		}
		str[end - start] = '\0';
	}
}

// returns new string allocation with the given content
char *newCpyStr(char *str,int size) {
	char *nStr = (char *)calloc(size+1,sizeof(char));
	strncpy(nStr,str,size);
	return nStr;
}

// for clearing a string
void clearStr(char *str, int size) {
	memset(str,'\0',sizeof(char)*size);
}
