#ifndef MAIN_H
#define MAIN_H

// includes
// standard libraries
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <errno.h>
//#include <sys/mman.h>
#include <sys/select.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <unistd.h>

#include <netinet/ip_icmp.h>
#include <netinet/udp.h>
#include <netinet/tcp.h>
#include <netinet/ip.h>
#include <netinet/in.h>
#include <netinet/if_ether.h>
#include <net/if.h>
#include <net/ethernet.h>

#include <arpa/inet.h>


// defines
#define SIZE_BUFFER 1000

// enums
// for results of processing stdin
typedef enum {INPUT_INVALID, INPUT_ERROR, INPUT_EXIT} input_command;
// for type of application protocol
typedef enum {PROT_UNKNOWN, PROT_HTTP, PROT_SMTP} app_prot_type;

// structures

// structure prototyping
struct packetInfo_;
struct packetNode_;
struct packetList_;

// packet info structure
// holds basic info on a packet received
// could just have 1 string/char array to hold all information
// since all have a fixed size, or just use the int values, and convert to string when needed
// could have an inner union for structs of different types
// 		such as tcpInfo, udpInfo - to keep track of unique values such as flags/checksum
typedef struct packetInfo_ {
	char *smac, *dmac;
	char *sip, *dip;
	int sport, dport;
	int eth_proto, ip_proto, app_proto;
	int dataSize;
} packetInfo;

// packet list structure
// node structure for packetList
typedef struct packetNode_ {
	packetInfo *pi;
	struct packetNode_ *next;
} packetNode;
// holds packets, acts as simple LinkedList
typedef struct packetList_ {
	// head, tail nodes
	struct packetNode_ *head, *tail;
} packetList;

// statistics structure
typedef struct {
	long packets_received;
	long bytes_received;
	long count_unknown_ethernet, count_unknown_ip, count_unknown_app;
	long count_ip, count_tcp, count_udp, count_arp, count_http, count_smtp;
} snifstats;

// options/arguments structure
typedef struct {
	uint8_t valid;
	uint8_t help, promiscuous, toTerminal, toFile;
	int filter;
	char *filename;
	FILE *f;
} options;

// function prototypes
options parseArguments(options *,int, char **);
int parseFilterFlag(const char *);
int isEthFiltered(int, int);
int isIPFiltered(int, int);
int isAppFiltered(int, int);
int turnOnPromisc(int);
void setFD(fd_set *, int);
input_command processInput();
void reportStats(options *,snifstats *,char *,int);
void displayInfo(options *, packetInfo *, char *, int);
packetInfo * processPacket(options *,snifstats *, unsigned char *, int);
packetInfo * processPacketIP(options *,snifstats *, packetInfo *, unsigned char *, int);
packetInfo * processPacketTCP(options *,snifstats *, packetInfo *, unsigned char *, int);
packetInfo * processPacketUDP(options *,snifstats *, packetInfo *, unsigned char *, int);
char *buildMACStr(unsigned char *, const int);
char *buildIPStr(uint32_t addr);
packetInfo * createPacketInfo();
void cleanPacketInfo(packetInfo **);
packetList * createPacketList();
void addPacket(packetList *list, packetInfo *);
void cleanPacketList(packetList **);
packetNode * createPacketNode(packetInfo *);
void cleanPacketNode(packetNode **);
void strToUpper(char *);
void strip(char *);
void stripSize(char *,int);
char *newCpyStr(char *,int);
void clearStr(char *, int);

#endif
