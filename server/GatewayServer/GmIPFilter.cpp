#include "GmIPFilter.h"
#include "Zebra.h"
#include "zXMLParser.h"
#include <arpa/inet.h>
///////////////////////////////////////////////
//
//code[GmIPFilter.cpp] defination by codemokey
//
//
///////////////////////////////////////////////




GmIPFilter::GmIPFilter()
{
    filterList.clear();
    exceptList.clear();
}

GmIPFilter::~GmIPFilter()
{}

bool GmIPFilter::init()
{
    zXMLParser xml;
    if(!xml.initFile(Zebra::global["gmconfig"]))
    {
	Zebra::logger->error("打开文件 gm过滤配置 失败");
	return false;
    }
    xmlNodePtr root = xml.getRootNode("conf");
    if(!root)
	return false;
    filterList.clear();
    exceptList.clear();

    char tmp[32];
    DWORD ip = 0;
    DWORD mask = 0;
    xmlNodePtr gmNode = xml.getChildNode(root, "gm");
    while(gmNode)
    {
	bzero(tmp, sizeof(tmp));
	xml.getNodePropStr(gmNode, "ip", tmp, sizeof(tmp));
	ip = inet_addr(tmp);

	bzero(tmp, sizeof(tmp));
	xml.getNodePropStr(gmNode, "mask", tmp, sizeof(tmp));
	mask = inet_addr(tmp);

	if(!mask)
	    mask = (DWORD)-1;

	if(ip && mask)
	    filterList.push_back(std::make_pair(ip, mask));

	gmNode = xml.getNextNode(gmNode, "gm");
	
    }

    DWORD id = 0;
    xmlNodePtr exceptNode = xml.getChildNode(root, "except");
    while(exceptNode)
    {
	xml.getNodePropNum(exceptNode, "charid", &id, sizeof(id));
	exceptList.insert(id);
	exceptNode = xml.getNextNode(exceptNode, "except");
    }

    Zebra::logger->debug("初始化GM 登陆过滤 filterList:%u条 exceptList:%u个", filterList.size(), exceptList.size());
    return true;
}

bool GmIPFilter::check(DWORD id, DWORD ip)
{
    if(filterList.empty())
	return true;
    if(exceptList.find(id) != exceptList.end())
	return true;
    for(filter_iter it=filterList.begin(); it!=filterList.end(); it++)
    {
	if((it->first & it->second) == (ip & it->second))
	    return true;
    }
    return false;
}
