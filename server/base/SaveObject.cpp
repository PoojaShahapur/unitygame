/*************************************************************************
 Author: wang
 Created Time: 2014年10月31日 星期五 16时06分53秒
 File Name: base/SaveObject.cpp
 Description: 
 ************************************************************************/
#include "SaveObject.h"
#include "ZlibObject.h"
#include "BinaryVersion.h"
/*
 *SaveObject 是8字节对齐
*/

const size_t SAVE_OBJECT_SIZE_20131031 = 420;

struct VersionSize
{
    DWORD dwVersion;
    size_t nSaveObjectSize;
};

const VersionSize ALL_VERSIONS[]=
{
    {0, SAVE_OBJECT_SIZE_20131031}, //版本号 －－ 该版本大小
};

const int VERSION_COUNT = sizeof(ALL_VERSIONS)/sizeof(0[ALL_VERSIONS]);

size_t getSaveObjectSize(DWORD dwVersion)
{
    for(int i=VERSION_COUNT-1; i>=0; i--)
    {
	const VersionSize& vs = ALL_VERSIONS[i];
	if(dwVersion >= vs.dwVersion)
	    return vs.nSaveObjectSize;
    }

    return ALL_VERSIONS[0].nSaveObjectSize;
}

SaveObject* getSaveObjectFromZlibObject(ZlibObject& rZlibObject,
	std::vector<SaveObject>& rBuf)
{
    const ZlibObject& zo = rZlibObject;
    if(zo.version == BINARY_VERSION)	//当前版本
	return (SaveObject*)(rZlibObject.data);
    size_t nOldSaveObjectSize = getSaveObjectSize(zo.version);
    rBuf.resize(zo.count);
    const unsigned char* pData = zo.data;
    for(DWORD i=0; i<zo.count; ++i)
    {
	bzero(&rBuf[i], sizeof(SaveObject));
	bcopy(pData, &rBuf[i], nOldSaveObjectSize);
	pData += nOldSaveObjectSize;
    }
    return &rBuf[0];
}
