/**
* \brief ʵ�ַ�������������
*
* �����������ȫ��������Ψһ����֤����
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
* \brief ��һ������������������ӵ�Ψһ��������
*
* \param task ��������������
* \return ����Ƿ�ɹ���Ҳ����Ψһ����֤�Ƿ�ɹ�
*/
void ClientManager::add(Client *task)
{
    if(NULL == task) return;
    rwlock.wrlock();
    this->clients[task->getName()] = task;
    rwlock.unlock();
}

/**
* \brief ��Ψһ��������ɾ��һ����������
*
* \param task ��������������
* \return ɾ���Ƿ�ɹ�
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
	Zebra::logger->error("�������ش���");
	return false;
    }
    taskPool->addCheckwait(client);
    if(!client->init(acc, tempid, key))
    {
	Zebra::logger->error("client init ����");
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
