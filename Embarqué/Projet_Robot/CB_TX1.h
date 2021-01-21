/* 
 * File:   CB_TX1.h
 * Author: Table2
 *
 * Created on 15 janvier 2021, 11:08
 */

#ifndef CB_TX1_H
#define	CB_TX1_H

void SendMessage(unsigned char*n, int);
void CB_TX1_Add(unsigned char);
unsigned char CB_TX1_Get(void);
void U1TXInterrupt(void);
void SendOne();
unsigned char CB_TX1_IsTranmitting(void);

#endif	/* CB_TX1_H */

