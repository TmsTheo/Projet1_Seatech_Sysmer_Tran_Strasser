/* 
 * File:   ToolBox.h
 * Author: TP-EO-1
 *
 * Created on 15 octobre 2020, 09:36
 */

#ifndef TOOLBOX_H
#define	TOOLBOX_H

#ifdef	__cplusplus
extern "C" {
#endif

float Abs(float);
float Max(float, float);
float Min(float, float); 
float LimitToInterval(float, float, float);
float RadianToDegree(float);
float DegreeToRadian(float);


#ifdef	__cplusplus
}
#endif

#endif	/* TOOLBOX_H */

