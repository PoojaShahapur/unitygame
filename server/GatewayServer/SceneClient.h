#ifndef __SCENECLIENT_H_
#define __SCENECLIENT_H_

#include <unistd.h>
#include <iostream>
#include <vector>
#include <map>
#include <ext/hash_map>
#include "zTCPClient.h"
#include "SceneCommand.h"
#include "zMutex.h"
#include "SuperCommand.h"
#include "ScreenIndex.h"
#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "SceneClientManager.h"
class GateUser;

/**
* \brief 定义场景服务器连接客户端类
**/
class SceneClient : public zTCPClientTask
{
    typedef __gnu_cxx::hash_map<DWORD, GateUser*> SceneIndex;
    SceneIndex sindex;
public:

	SceneClient(
		const std::string &ip,
		const WORD port);
	SceneClient( const std::string &name,const Cmd::Super::ServerEntry *serverEntry)
		: zTCPClientTask(serverEntry->pstrIP,serverEntry->wdPort)
	{
		wdServerID=serverEntry->wdServerID;
	};
	~SceneClient()
	{
		Zebra::logger->debug("SceneClient析构");
	}

	int checkRebound();
	void addToContainer();
	void removeFromContainer();
	bool connectToSceneServer();
	bool connect();
	bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
	bool doGmCmd(const Cmd::Scene::t_gmCommand_SceneGate* pCmd);
	void freshIndex(GateUser *pUser,const DWORD map,const DWORD screen)
	{
		MapIndexIter iter = mapIndex.find(map); 
		if (iter != mapIndex.end())
		{
			iter->second->refresh(pUser,screen);
		}
	}
	void removeIndex(GateUser *pUser,const DWORD map)
	{
		MapIndexIter iter = mapIndex.find(map); 
		if (iter != mapIndex.end())
		{
			iter->second->removeGateUser(pUser);
		}
	}
	GateUser* getUserByIndex(const DWORD id)
	{
	    GateUser* ret = NULL;
	    mlock.lock();
	    SceneIndex::iterator iter = sindex.find(id);
	    if(iter != sindex.end())
	    {
		ret = iter->second;
	    }
	    mlock.unlock();
	    return ret;
	}
	const WORD getServerID() const
	{
		return wdServerID;
	}

private:

	/**
	* \brief 网关屏索引
	*
	*
	* \param 
	* \return 
	*/
	MapIndex mapIndex;
	/**
	* \brief 服务器编号
	*
	*/
	WORD wdServerID;

	zMutex mlock;
};




#endif
