/**
* \brief ����������������
*
* �����������ȫ��������Ψһ����֤����
*
*/
#ifndef _ClientManager_h_
#define _ClientManager_h_
#include "zNoncopyable.h"
#include "zTCPClientTaskPool.h"

class Client;
struct ClientExec
{
    virtual ~ClientExec() {}
    virtual bool exec(Client &clt) = 0;
};

class ClientManager : zNoncopyable
{

public:
	bool init();
	/**
	* \brief ȱʡ��������
	*
	*/
	~ClientManager() {};

	/**
	* \brief ��ȡ���Ψһʵ��
	*
	* �����ʹ����Singleton���ģʽ,��֤��һ��������ֻ��һ�����ʵ��
	*
	*/
	static ClientManager &getInstance();

	/**
	* \brief �ͷ����Ψһʵ��
	*
	*/
	static void delInstance()
	{
		SAFE_DELETE(instance);
	}

	void add(Client *task);
	bool addClientTask(Client *client, DWORD acc, DWORD tempid, DES_cblock key);
	void remove(Client *task);
	void timeAction();
	void execEvery(ClientExec &callback);
	Client *getClientByName(std::string name);
private:

	/**
	* \brief ���캯��
	*
	*/
	ClientManager() {};

	/**
	* \brief ���Ψһʵ��ָ��
	*
	*/
	static ClientManager *instance;

	ClientManager(const ClientManager &cm);
	ClientManager &operator=(const ClientManager &cm);
	
	zRWLock rwlock;
	zTCPClientTaskPool *taskPool;
	/**
	* \brief �����˷�������Ψһ����֤��������
	* 
	**/
	typedef std::map<std::string, Client *> ClientContainer;
	/**
	* \brief ���������ĵ���������
	*
	*/
	typedef ClientContainer::iterator ClientContainer_iterator;
	/**
	* \brief �����������ĳ�������������
	*
	*/
	typedef ClientContainer::const_iterator ClientContainer_const_iterator;
	/**
	* \brief �����������ļ�ֵ������
	*
	*/
	typedef ClientContainer::value_type ClientContainer_value_type;
	/**
	* \brief Ψһ������ʵ��
	*
	*/
	ClientContainer clients;

};
#endif

