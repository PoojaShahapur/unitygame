/**
* \brief ����������������
*
* �����������ȫ��������Ψһ����֤����
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
	* \brief ȱʡ��������
	*
	*/
	~ServerManager() {};

	/**
	* \brief ��ȡ���Ψһʵ��
	*
	* �����ʹ����Singleton���ģʽ,��֤��һ��������ֻ��һ�����ʵ��
	*
	*/
	static ServerManager &getInstance()
	{
		if (NULL == instance)
			instance = new ServerManager();

		return *instance;
	}

	/**
	* \brief �ͷ����Ψһʵ��
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
	* \brief ���캯��
	*
	*/
	ServerManager() {};

	/**
	* \brief ���Ψһʵ��ָ��
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
	* \brief hash����
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
	* \brief �����˷�������Ψһ����֤��������
	* 
	**/
	typedef std::map<const GameZone_t,ServerTask *> ServerTaskContainer;
	/**
	* \brief ���������ĵ���������
	*
	*/
	typedef ServerTaskContainer::iterator ServerTaskContainer_iterator;
	/**
	* \brief �����������ĳ�������������
	*
	*/
	typedef ServerTaskContainer::const_iterator ServerTaskContainer_const_iterator;
	/**
	* \brief �����������ļ�ֵ������
	*
	*/
	typedef ServerTaskContainer::value_type ServerTaskContainer_value_type;
	/**
	* \brief �������ʵĻ������
	*
	*/
	zMutex mlock;
	/**
	* \brief Ψһ������ʵ��
	*
	*/
	ServerTaskContainer taskUniqueContainer;

};
#endif

