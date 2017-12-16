// holds function definitions for functions relating to the structures used
#include "main.h"


// build packetInfo
packetInfo * createPacketInfo() {
	packetInfo *pi = (packetInfo *)calloc(1,sizeof(packetInfo));
	pi->sport = -1;
	pi->dport = -1;
	pi->eth_proto = -1;
	pi->ip_proto = -1;
	pi->app_proto = -1;
	pi->dataSize = -1;
	return pi;
}

// clean packetInfo struct
void cleanPacketInfo(packetInfo **pi) {
	packetInfo *api = *pi;
	if (api->smac != NULL) {
		free(api->smac);
	}
	if (api->dmac != NULL) {
		free(api->dmac);
	}
	if (api->sip != NULL) {
		free(api->sip);
	}
	if (api->dip != NULL) {
		free(api->dip);
	}
	free(*pi);
	*pi = NULL;
}

// build packetList
packetList * createPacketList() {
	packetList *list = (packetList *)calloc(1,sizeof(packetList));
	list->head = NULL;
	list->tail = NULL;
	return list;
}

// add packetInfo to packetList
void addPacket(packetList *list, packetInfo *pi) {
	if (list->head == NULL) {
		list->head = createPacketNode(pi);
		list->tail = list->head;
	}else{
		list->tail->next = createPacketNode(pi);
		list->tail = list->tail->next;
	}
}

// clean packetList
void cleanPacketList(packetList **list) {
	packetList *alist = *list;
	while (alist->head != NULL) {
		packetNode *node = alist->head;
		alist->head = node->next;
		cleanPacketNode(&node);
	}
	free(*list);
	*list = NULL;
}

// build packetNode
packetNode * createPacketNode(packetInfo *pi) {
	packetNode *node = (packetNode *)calloc(1,sizeof(packetNode));
	node->pi = pi;
	node->next = NULL;
	return node;
}

// clean packetNode
void cleanPacketNode(packetNode **node) {
	packetNode *anode = *node;
	if (anode->pi != NULL) {
		cleanPacketInfo(&(anode->pi));
	}
	free(*node);
	*node = NULL;
}