// definitions for functions relating to packet processing

#include "main.h"


// process a packet
// returns null if packet was filtered
packetInfo * processPacket(options * args, snifstats *stats, unsigned char *buffer, int packet_length) {
	// create packet info
	packetInfo *pi = createPacketInfo();
	
	// get ethernet header
	struct ethhdr *eth = (struct ethhdr *)(buffer);
	
	// populate packet info
	// source/destination MAC
	pi->smac = buildMACStr(eth->h_source,ETH_ALEN);
	pi->dmac = buildMACStr(eth->h_dest,ETH_ALEN);
	
	// get protocol type
	// /usr/include/linux/if_ether.h
	// proto, __be16
	// big endian 16 bit, signed ?
	int ethProto = ntohs(eth->h_proto);
	// set in packet info
	pi->eth_proto = ethProto;
	
	// check if filtered
	if (isEthFiltered(args->filter, ethProto)) {
		// clean packet info
		cleanPacketInfo(&pi);
		return NULL;
	}
	
	// increment packet summary: packet count, bytes
	stats->packets_received++;
	stats->bytes_received += packet_length;
	
	if (ethProto == ETH_P_IP) {
		// IP packet
		// process IP packet
		// send buffer incremented by size of ethernet header
		int len = sizeof(struct ethhdr);
		if (processPacketIP(args,stats,pi,buffer+len, packet_length-len) == NULL) {
			// if return null, was filtered
			// clean packet info
			cleanPacketInfo(&pi);
			return NULL;
		}
		
		// only increment count after filter
		stats->count_ip++;
	}else if (ethProto == ETH_P_ARP) {
		// ARP packet
		stats->count_arp++;
	}else{
		// unknown ethernet packet
		stats->count_unknown_ethernet++;
	}
	
	return pi;
}

// process an IP packet from given buffer
packetInfo * processPacketIP(options * args, snifstats *stats, packetInfo *pi, unsigned char *buffer, int packet_length) {
	// get IP header
	struct iphdr *ip = (struct iphdr*)(buffer);
	
	// /usr/include/netinet/ip.h
	// /usr/include/netinet/ih.h
	// iphdr
	// 		uint32_t	saddr
	// 		uint32_t	daddr
	// sockaddr_in
	// 		in_port_t sinport			port number
	// 		struct in_addr sin_addr		internet address
	// in_addr
	// 		in_addr_t	s_addr
	// in_addr_t equivalent to uin32_t
	
	// populate packet info
	// source/destination IP
	pi->sip = buildIPStr(ip->saddr);
	pi->dip = buildIPStr(ip->daddr);
	
	// get protocol type
	// /usr/src/linux-headers-3.19.0-30/include/uapi/linux/in.h
	// /etc/protocols
	// IP		IPPROTO_IP		0
	// ICMP		IPPROTO_ICMP	1
	// IGMP		IPPROTO_IGMP	2
	// TCP		IPPROTO_TCP		6
	// UDP		IPPROTO_UDP		17
	// single byte, don't need to use htons()
	uint8_t ipProto = ip->protocol;
	
	// set in packet info
	pi->ip_proto = ipProto;
	
	// check if filtered
	if (isIPFiltered(args->filter, ipProto)) {
		// return null
		return NULL;
	}
	
	// get size of IP header
	// ihl (internet header length), size of header in 32 bit words (4 bytes)
	uint32_t len = ip->ihl * 4;
	
	if (ipProto == IPPROTO_IP) {
		// not sure what kind of IP packets would have this
		stats->count_unknown_ip++;
	}else if (ipProto == IPPROTO_TCP) {
		// process TCP packet
		if (processPacketTCP(args,stats,pi,buffer+len,packet_length-len) == NULL) {
			// was filtered, return null
			return NULL;
		}
		
		stats->count_tcp++;
	}else if (ipProto == IPPROTO_UDP) {
		// process UDP packet
		if (processPacketUDP(args,stats,pi,buffer+len,packet_length-len) == NULL) {
			// was filtered, return null
			return NULL;
		}
		
		stats->count_udp++;
	}else{
		// unknown
		stats->count_unknown_ip++;
	}
	
	return pi;
}

// processes a TCP packet from given buffer
packetInfo * processPacketTCP(options * args, snifstats *stats, packetInfo *pi, unsigned char *buffer, int packet_length) {
	// get tcp header
	struct tcphdr *tcp = (struct tcphdr *)(buffer);
	
	// /usr/include/netinet/tcp.h
	
	// populate packet info
	// source/destination port
	// uint16_t		source, dest	ports
	pi->sport = ntohs(tcp->source);
	pi->dport = ntohs(tcp->dest);
	
	// get data offset (size of tcp header), doff = data offset, size of header in 32 bit words (4 bytes)
	// don't need ntohs(), because the structure checks the local machines endian in its definition
	// /usr/include/netinet/tcp.h
	int doff = tcp->doff * 4;
	
	// move buffer forward, although not needed currently
	buffer += doff;
	packet_length -= doff;
	
	// set size of data in packet information
	pi->dataSize = packet_length;
	
	// check destination port, attempt to determine protocol
	if (pi->sport == 80 || pi->dport == 80) {
		// port 80, http
		// check if filtered
		if (isAppFiltered(args->filter,PROT_HTTP)) {
			return NULL;
		}
		pi->app_proto = PROT_HTTP;
		stats->count_http++;
	}else if (pi->sport == 25 || pi->dport == 25) {
		// port 25, stmp
		// check if filtered
		if (isAppFiltered(args->filter,PROT_SMTP)) {
			return NULL;
		}
		pi->app_proto = PROT_SMTP;
		stats->count_smtp++;
	}else{
		stats->count_unknown_app++;
		// unknown protocol
		pi->app_proto = PROT_UNKNOWN;
	}
	
	return pi;
}

// processes a UDP packet from given buffer
packetInfo * processPacketUDP(options * args, snifstats *stats, packetInfo *pi, unsigned char *buffer, int packet_length) {
	// get udp header
	struct udphdr *udp = (struct udphdr *)(buffer);
	
	// /usr/include/netinet/udp.h
	
	// populate packet info
	// source/destination port
	// uint16_t		source, dest	ports
	pi->sport = ntohs(udp->source);
	pi->dport = ntohs(udp->dest);
	
	// get length of whole UDP packet
	// UDP header is always 8 bytes
	int len = ntohs(udp->len);
	
	// move buffer forward, although not needed currently
	buffer += 8;
	packet_length -= 8;
	
	// set size of data in packet information
	pi->dataSize = packet_length;
	
	// unknown protocol
	pi->app_proto = PROT_UNKNOWN;
	stats->count_unknown_app++;
	
	return pi;
}

// builds mac address from given char array, second value is size
// assumes the size is divisible by 2
char *buildMACStr(unsigned char *arr, const int size) {
	// calc size of str
	// 2 characters for each of the array, then the space for the spacers '-', +1 for null terminator
	int strSize = 2*size + size - 1 + 1;
	
	// allocate str
	char *str = (char *)calloc(strSize,sizeof(char));
	str[strSize-1] = '\0';
	
	// populate str
	int i = 0, j = 0;
	while (j < size) {
		if ((i+1) % 3 == 0) {
			str[i] = '-';
			++i;
		}else{
			sprintf(str+i,"%.2X",arr[j]);
			++j;
			i += 2;
		}
	}
	
	return str;
}

// builds ip address from given uin32_t value, can be from sockaddr_in.saddr or sockaddr_in.daddr
// does not need to use intermediate structures for printing 
char *buildIPStr(uint32_t addrVal) {
	// calculate size of str
	// INET_ADDRSTRLEN: size of IPv4 addresses
	int size = INET_ADDRSTRLEN + 1;
	
	// allocate str
	char *str = (char *)calloc(size,sizeof(char));
	str[size-1] = '\0';
	
	// convert address to string
	// inet_ntoa() deprecated
	// use inet_ntop()
	
	// throw value into adress structure
	// can actually just pass &addrVal and it will work
	struct in_addr addr = {.s_addr = addrVal};
	
	inet_ntop(AF_INET, &addr, str, size-1);	
	
	return str;
}