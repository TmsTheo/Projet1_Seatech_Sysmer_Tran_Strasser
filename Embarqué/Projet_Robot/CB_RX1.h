/* 
 * File:   CB_RX1.h
 * Author: Table2
 *
 * Created on 15 janvier 2021, 12:16
 */

#ifndef CB_RX1_H
#define	CB_RX1_H

void CB_RX1_Add(unsigned char);
unsigned char CB_RX1_Get(void);
unsigned char CB_RX1_IsDataAvailable(void);
void U1RXInterrupt(void);
int CB_RX1_GetRemainingSize(void);
int CB_RX1_GetDataSize(void);


#endif	/* CB_RX1_H */

