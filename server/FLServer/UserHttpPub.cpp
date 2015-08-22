/*************************************************************************
 Author: 
 Created Time: 2014年10月11日 星期六 14时53分58秒
 File Name: FLServer/UserHttpPub.cpp
 Description: 
 ************************************************************************/
#include "UserHttpPub.h"
using namespace Cmd::UserServer;

void UserHttpPub::consume_func(t_cmd_consume *pt, t_cmd_ret &cmd)
{
    cmd.at = pt->at;
    cmd.uid = pt->uid;
    strncpy(cmd.tid, pt->tid, SEQ_MAX_LENGTH);
    DWORD balance;
    bool ret = UserConfigM::getMe().consumePoint(pt->uid, (DWORD)pt->point, balance);
    if(ret)
    {
	cmd.ret = RET_OK;
	cmd.point = pt->point;
	cmd.balance = balance;
	cmd.hadfilled = 1;
	Zebra::logger->debug("消费成功:%d,%d",pt->uid,cmd.balance);
    }
    else
    {
	Zebra::logger->error("余额不足");
	cmd.ret = RET_BALANCE_NOT_ENOUGH;
    }
    return;
}

void UserHttpPub::qbalance_func(Cmd::UserServer::t_cmd_qbalance *pt, Cmd::UserServer::t_cmd_ret &cmd)
{
    Zebra::logger->debug("处理查询剩余点数 %s,%s",pt->account, pt->tid);
    cmd.at = pt->at;
    strncpy(cmd.tid, pt->tid, SEQ_MAX_LENGTH);
    cmd.uid = pt->uid;
    
    cmd.point = 0;
    cmd.balance = 10000;
    cmd.ret = RET_OK;
    Zebra::logger->debug("查询成功:%d, %d",pt->uid, cmd.balance);
}

#include "zXMLParser.h"

bool UserConfigM::init()
{
    initPointMap();
    return true;
}

bool UserConfigM::reload()
{
    pointsLock.wrlock();
    pointsMap.clear();
    initPointMap();
    pointsLock.unlock();
    return true;
}

bool UserConfigM::initPointMap()
{
    zXMLParser xml;
    if(!xml.initFile(Zebra::global["PointListFile"]))
    {
	return false;
    }
    xmlNodePtr root = xml.getRootNode("PointList");
    if(root)
    {
	xmlNodePtr node = xml.getChildNode(root, "item");
	while(node)
	{
	    if(strcmp((char*)node->name, "item") == 0)
	    {
		DWORD uid;
		DWORD points;
		if(xml.getNodePropNum(node, "uid", &uid, sizeof(uid))
			&& xml.getNodePropNum(node, "points", &points, sizeof(points)))
		{
		    pointsMap[uid] = points;
		}
	    }
	    node = xml.getNextNode(node, "item");
	}
    }
    return true;
}

bool UserConfigM::consumePoint(const DWORD uid, const DWORD cpoint, DWORD &balance)
{
    zRWLock_scope_wrlock wrscope(pointsLock);
    PointMap::iterator it = pointsMap.find(uid);
    if(it == pointsMap.end())
	return false;
    if(cpoint > it->second)
	return false;
    it->second -= cpoint;
    balance = it->second;
    return true;
}

UserConfigM::~UserConfigM()
{
    pointsLock.wrlock();
    pointsMap.clear();
    pointsLock.unlock();
}
