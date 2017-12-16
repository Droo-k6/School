**cscd 433 packet sniffer**

Basic packet sniffer, will read both ingoing and outgoing packets and attempt to retreive very basic information from each.
Such as source/destination MAC, IP and ports, as well as attempting to identify the protocol in the ethernet header, ip header and transport data/information.

---
Usage:

must be ran with sudo/super user perms
arguments
	-h		displays help
	-p		enables promiscuous mode
	-d		output results to screen
	-o *	output results to file
	-f *	filter these types of packets (exclude)
			to filter multiple types, just use multiple statements
			example: -f HTTP -f UDP
			will filter UDP and HTTP packets
		options
			IP
			ARP
			TCP
			UDP
			HTTP
			SMTP

An output mode must be selected, either "-d" or "-o filename"

To stop the sniffer, type "exit" and press enter

Promiscuous mode will attempt to enable on only 1 device, the first it finds containg "eth" in the name
Doesn't appear to need to turn it off after exiting.

Compiling:
Created using gcc 4.9.2
Included Makefile to create, output program is "snif"

---
References

[Using raw sockets](http://opensourceforu.com/2015/03/a-guide-to-using-raw-sockets/)
[Packet sniffer in c](http://www.binarytides.com/packet-sniffer-code-in-c-using-linux-sockets-bsd-part-2/)
[libpcap (github)](https://github.com/the-tcpdump-group/libpcap/)
[Enumerating network devices in C (stackoverflow)](https://stackoverflow.com/a/23577874)