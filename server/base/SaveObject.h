/*************************************************************************
 Author: wang
 Created Time: 2014��10��31�� ������ 15ʱ57��33��
 File Name: base/SaveObject.h
 Description: 
 ************************************************************************/
#ifndef SaveObject_h_
#define SaveObject_h_

#include "Object.h"
#include <vector>
struct ZlibObject;

struct SaveObject
{
    union
    {
	struct
	{
	    DWORD createtime;
	    DWORD dwCreateThisID;
	};
	unsigned long long createid;
    };
    t_Object object;
};

SaveObject* getSaveObjectFromZlibObject(ZlibObject& rZlibObject,
	std::vector<SaveObject>& rBuf);
#endif

