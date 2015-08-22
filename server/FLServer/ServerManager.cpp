/**
* \brief 实现服务器管理容器
*
* 这个容器包括全局容器和唯一性验证容器
* 
*/

#include "FLServer.h"
#include "ServerManager.h"
#include "zMNetService.h"

ServerManager *ServerManager::instance = NULL;

/**
* \brief 把一个服务器连接任务添加到唯一性容器中
*
* \param task 服务器连接任务
* \return 添加是否成功，也就是唯一性验证是否成功
*/
bool ServerManager::uniqueAdd(ServerTask *task)
{
	Zebra::logger->debug("ServerManager::uniqueAdd");
	zMutex_scope_lock scope_lock(mlock);
	ServerTaskContainer_const_iterator it;
	it = taskUniqueContainer.find(task->getZoneID());
	if (it != taskUniqueContainer.end())
	{
		Zebra::logger->error("ServerManager::uniqueAdd");
		return false;
	}
	taskUniqueContainer.insert(ServerTaskContainer_value_type(task->getZoneID(),task));
	return true;
}

/**
* \brief 从唯一性容器中删除一个连接任务
*
* \param task 服务器连接任务
* \return 删除是否成功
*/
bool ServerManager::uniqueRemove(ServerTask *task)
{
	Zebra::logger->debug("ServerManager::uniqueRemove");
	zMutex_scope_lock scope_lock(mlock);
	ServerTaskContainer_iterator it;
	it = taskUniqueContainer.find(task->getZoneID());
	if (it != taskUniqueContainer.end())
	{
		taskUniqueContainer.erase(it);
	}
	else
		Zebra::logger->warn("ServerManager::uniqueRemove");
	return true;
}

bool ServerManager::broadcast(const GameZone_t &gameZone,const void *pstrCmd,int nCmdLen)
{
	Zebra::logger->debug("ServerManager::broadcast");
	zMutex_scope_lock scope_lock(mlock);
	ServerTaskContainer_iterator it;
	it = taskUniqueContainer.find(gameZone);
	if (it != taskUniqueContainer.end())
	{
		return it->second->sendCmd(pstrCmd,nCmdLen);
	}
	else
		return false;
}

