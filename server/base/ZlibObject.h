/*************************************************************************
 Author: wang
 Created Time: 2014年10月31日 星期五 15时54分59秒
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
