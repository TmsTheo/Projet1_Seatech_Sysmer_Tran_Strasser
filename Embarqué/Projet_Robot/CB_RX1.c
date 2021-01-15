#include <xc.h>
#include <stdio.h>
#include <stdlib.h>
#include "CB_RX1.h"

#define CBRX1_BUFFER_SIZE 128

 

int cbRx1Head;
int cbRx1Tail;
unsigned char cbRx1Buffer[CBRX1_BUFFER_SIZE];

void CB_RX1_Add(unsigned char value) {
    if(CB_RX1_GetRemainingSize()>0) {
        if(cbRxHead < CBRX1_BUFFER_SIZE){
           cbRx1Head = cbRx1Head + 1;
           cbRx1Buffer[cbRx1Head] = value; 
        }
        else {
            cbRx1Head = 1;
           cbRx1Buffer[cbRx1Head] = value; 
        }
    }
}

unsigned char CB_RX1_Get(void) {
    unsigned char value=cbRx1Buffer[cbRx1Tail];
    //?
    return value;
}

unsigned char CB_RX1_IsDataAvailable(void) {
    if(cbRx1Head!=cbRx1Tail)
        return 1;
    else
        return 0;
}

void __attribute__((interrupt, no_auto_psv)) _U1RXInterrupt(void) {
    IFS0bits.U1RXIF = 0; // clear RX interrupt flag
    /* check for receive errors */
    if (U1STAbits.FERR == 1) {
        U1STAbits.FERR = 0;
    }
    /* must clear the overrun error to keep uart receiving */
    if (U1STAbits.OERR == 1) {
        U1STAbits.OERR = 0;
    }
    /* get the data */
    while(U1STAbits.URXDA == 1) {
        CB_RX1_Add(U1RXREG);
    }
}

int CB_RX1_GetRemainingSize(void) {
    int rSizeRecep;
    if(cbRx1Haed > cbRx1Tail) {
        rSizeRecep = CBRX1_BUFFER_SIZE - (cbRx1Head - cbRx1Tail);
    }
    else {
        rSizeRecep = cbRx1Tail - cbRx1Head; 
    }
    
    return rSizeRecep;
}

int CB_RX1_GetDataSize(void) {
    int rSizeRecep;
    rsizeRecep = cbRx1Buffer[cbRx1Tail].size();
    return rSizeRecep;
}
