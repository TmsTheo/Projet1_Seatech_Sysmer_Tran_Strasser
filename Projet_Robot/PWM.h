/* 
 * File:   PWM.h
 * Author: TP-EO-1
 *
 * Created on 15 octobre 2020, 09:41
 */

#ifndef PWM_H
#define	PWM_H
#define MOTEUR_DROIT 6
#define MOTEUR_GAUCHE 1

#ifdef	__cplusplus
extern "C" {
#endif

void InitPWM(void);
//void PWMSetSpeed(float, int);
void PWMUpdateSpeed();
void PWMSetSpeedConsigne(float, char);


#ifdef	__cplusplus
}
#endif

#endif	/* PWM_H */

