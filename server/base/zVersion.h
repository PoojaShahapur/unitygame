/*************************************************************************
 Author: wang
 Created Time: 2015��04��16�� ������ 11ʱ41��23��
 File Name: base/zVersion.h
 Description: 
 ************************************************************************/
#ifndef _ZVERSION_H_
#define _ZVERSION_H_


#ifndef MAJOR_VERSION
#define MAJOR_VERSION 0
#endif

#ifndef MINOR_VERSION
#define MINOR_VERSION 0
#endif

#ifndef MICRO_VERSION
#define MICRO_VERSION 0
#endif

#define _TY(x) #x
#define _S(x) _TY(x)

#ifndef VS
#define VERSION_STRING _S(MAJOR_VERSION)"."_S(MINOR_VERSION)"."_S(MICRO_VERSION)
#else
#define VERSION_STRING _S(VS)
#endif

#endif

