/* 
 * File:   ADC.h
 * Author: TP-EO-1
 *
 * Created on 21 octobre 2020, 14:14
 */

#ifndef ADC_H
#define	ADC_H

#ifdef	__cplusplus
extern "C" {
#endif

void InitADC1(void);
void __attribute__((interrupt, no_auto_psv)) _AD1Interrupt(void);
void ADC1StartConversionSequence();
unsigned int * ADCGetResult(void);
unsigned char ADCIsConversionFinished(void);
void ADCClearConversionFinishedFlag(void);


#ifdef	__cplusplus
}
#endif

#endif	/* ADC_H */

