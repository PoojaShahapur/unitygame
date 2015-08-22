/**
* \brief 服务器管理容器类
*
* 这个容器包括全局容器和唯一性验证容器
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
	* \brief 缺省析构函数
	*
	*/
	~ClientManager() {};

	/**
	* \brief 获取类的唯一实例
	*
	* 这个类使用了Singleton设计模式,保证了一个进程中只有一个类的实例
	*
	*/
	static ClientManager &getInstance();

	/**
	* \brief 释放类的唯一实例
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
	* \brief 构造函数
	*
	*/
	ClientManager() {};

	/**
	* \brief 类的唯一实例指针
	*
	*/
	static ClientManager *instance;

	ClientManager(const ClientManager &cm);
	ClientManager &operator=(const ClientManager &cm);
	
	zRWLock rwlock;
	zTCPClientTaskPool *taskPool;
	/**
	* \brief 定义了服务器的唯一性验证容器类型
	* 
	**/
	typedef std::map<std::string, Client *> ClientContainer;
	/**
	* \brief 定义容器的迭代器类型
	*
	*/
	typedef ClientContainer::iterator ClientContainer_iterator;
	/**
	* \brief 定义了容器的常量迭代器类型
	*
	*/
	typedef ClientContainer::const_iterator ClientContainer_const_iterator;
	/**
	* \brief 定义了容器的键值对类型
	*
	*/
	typedef ClientContainer::value_type ClientContainer_value_type;
	/**
	* \brief 唯一性容器实例
	*
	*/
	ClientContainer clients;

};
#endif

