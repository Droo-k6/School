// defines structures used in both linkedlist and listutils
#ifndef LLSTRUCT_H
#define LLSTRUCT_H


struct node {
    void * data;
    struct node * next;
    struct node * prev;
};
typedef struct node Node;

struct linkedlist {
    Node * head;
    int size;
};
typedef struct linkedlist LinkedList;


#endif
