/*************************************************************************
 Author: wang
 Created Time: 2014��10��31�� ������ 15ʱ54��59��
 File Name: base/ZlibObject.h
 Description: 
 ************************************************************************/
#ifndef ZlibObject_h_
#define ZlibObject_h_

#include "zType.h"
struct ZlibObject
{
    DWORD version;
    DWORD count;
    unsigned char data[0];
    ZlibObject()
    {
	count = 0;
	version = 0;
    }
};
#endif
