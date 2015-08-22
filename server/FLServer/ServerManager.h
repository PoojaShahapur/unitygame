/**
* \brief 服务器管理容器类
*
* 这个容器包括全局容器和唯一性验证容器
*
*/
#ifndef _ServerManager_h_
#define _ServerManager_h_
#include "zNoncopyable.h"
#include "ServerTask.h"

class ServerManager : zNoncopyable
{

public:

	/**
	* \brief 缺省析构函数
	*
	*/
	~ServerManager() {};

	/**
	* \brief 获取类的唯一实例
	*
	* 这个类使用了Singleton设计模式,保证了一个进程中只有一个类的实例
	*
	*/
	static ServerManager &getInstance()
	{
		if (NULL == instance)
			instance = new ServerManager();

		return *instance;
	}

	/**
	* \brief 释放类的唯一实例
	*
	*/
	static void delInstance()
	{
		SAFE_DELETE(instance);
	}

	bool uniqueAdd(ServerTask *task);
	bool uniqueRemove(ServerTask *task);
	bool broadcast(const GameZone_t &gameZone,const void *pstrCmd,int nCmdLen);

private:

	/**
	* \brief 构造函数
	*
	*/
	ServerManager() {};

	/**
	* \brief 类的唯一实例指针
	*
	*/
	static ServerManager *instance;


	/*struct less_str 
	{

	bool operator()(const GameZone_t & x, const GameZone_t & y) const 
	{
	if (x.id < y.id )
	return true;

	return false;
	}
	};*/

	/**
	* \brief hash函数
	*
	*/
	/*struct GameZone_hash : public hash_compare<GameZone_t,less_str>
	{
	size_t operator()(const GameZone_t &gameZone) const
	{
	//__gnu_cxx::hash<DWORD> H;
	//return H(gameZone.id);
	//return Hash<DWORD>(gameZone.id);
	return 1;
	}

	};*/
	/**
	* \brief 定义了服务器的唯一性验证容器类型
	* 
	**/
	typedef std::map<const GameZone_t,ServerTask *> ServerTaskContainer;
	/**
	* \brief 定义容器的迭代器类型
	*
	*/
	typedef ServerTaskContainer::iterator ServerTaskContainer_iterator;
	/**
	* \brief 定义了容器的常量迭代器类型
	*
	*/
	typedef ServerTaskContainer::const_iterator ServerTaskContainer_const_iterator;
	/**
	* \brief 定义了容器的键值对类型
	*
	*/
	typedef ServerTaskContainer::value_type ServerTaskContainer_value_type;
	/**
	* \brief 容器访问的互斥变量
	*
	*/
	zMutex mlock;
	/**
	* \brief 唯一性容器实例
	*
	*/
	ServerTaskContainer taskUniqueContainer;

};
#endif

