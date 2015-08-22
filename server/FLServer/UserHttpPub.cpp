/*************************************************************************
 Author: 
 Created Time: 2014��10��11�� ������ 14ʱ53��58��
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
	Zebra::logger->debug("���ѳɹ�:%d,%d",pt->uid,cmd.balance);
    }
    else
    {
	Zebra::logger->error("����");
	cmd.ret = RET_BALANCE_NOT_ENOUGH;
    }
    return;
}

void UserHttpPub::qbalance_func(Cmd::UserServer::t_cmd_qbalance *pt, Cmd::UserServer::t_cmd_ret &cmd)
{
    Zebra::logger->debug("�����ѯʣ����� %s,%s",pt->account, pt->tid);
    cmd.at = pt->at;
    strncpy(cmd.tid, pt->tid, SEQ_MAX_LENGTH);
    cmd.uid = pt->uid;
    
    cmd.point = 0;
    cmd.balance = 10000;
    cmd.ret = RET_OK;
    Zebra::logger->debug("��ѯ�ɹ�:%d, %d",pt->uid, cmd.balance);
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
