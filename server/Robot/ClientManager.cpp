/**
* \brief 实现服务器管理容器
*
* 这个容器包括全局容器和唯一性验证容器
* 
*/

#include "Client.h"
#include "ClientManager.h"

ClientManager *ClientManager::instance = NULL;

ClientManager &ClientManager::getInstance()
{
    if (NULL == instance)
	instance = new ClientManager();

    return *instance;
}
/**
* \brief 把一个服务器连接任务添加到唯一性容器中
*
* \param task 服务器连接任务
* \return 添加是否成功，也就是唯一性验证是否成功
*/
void ClientManager::add(Client *task)
{
    if(NULL == task) return;
    rwlock.wrlock();
    this->clients[task->getName()] = task;
    rwlock.unlock();
}

/**
* \brief 从唯一性容器中删除一个连接任务
*
* \param task 服务器连接任务
* \return 删除是否成功
*/
void ClientManager::remove(Client *task)
{
    if(NULL == task) return;
    rwlock.wrlock();
    ClientContainer_iterator it;
    it = clients.find(task->getName());
    if (it != clients.end())
    {
	clients.erase(it);
    }
    rwlock.unlock();
}

Client *ClientManager::getClientByName(std::string name)
{
    ClientContainer_iterator iter=clients.find(name);
    if(iter != clients.end())
    {
	return iter->second;
    }
    return NULL;
}

void ClientManager::timeAction()
{
    rwlock.rdlock();
    for(ClientContainer_iterator it=clients.begin();it!=clients.end();it++)
    {
	it->second->timeAction();
    }
    rwlock.unlock();
}

bool ClientManager::addClientTask(Client *client, DWORD acc, DWORD tempid, DES_cblock key)
{
    if(NULL == client)
	return false;
    if(!client->connect())
    {
	Zebra::logger->error("连接网关错误");
	return false;
    }
    taskPool->addCheckwait(client);
    if(!client->init(acc, tempid, key))
    {
	Zebra::logger->error("client init 错误");
	return false;
    }
    this->add(client);
    return true;
}

bool ClientManager::init()
{
    taskPool = new zTCPClientTaskPool();
    if(NULL == taskPool 
	    || !taskPool->init())
	return false;
    return true;
}
