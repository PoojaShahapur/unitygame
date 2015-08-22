/**
* \brief 服务器连接任务
*/
#ifndef _UserTask_h_
#define _UserTask_h_
#include "zTCPTask.h"
#include "NetType.h"
#include "zSingleton.h"
#include "UserHttpPub.h"

class UserTask : public zTCPTask, public UserHttpPub
{

public:
	/**
	* \brief 构造函数
	* 用于创建一个服务器连接任务
	* \param pool 所属的连接池
	* \param sock TCP/IP套接口
	*/
	UserTask(
		zTCPTaskPool *pool,
		const int sock) : zTCPTask(pool,sock,NULL)
	{
	    netType = NetType_near;
	}

	/**
	* \brief 虚析构函数
	*/
	~UserTask() {
	};

	int verifyConn();
	int waitSync();
	void addToContainer();
	void removeFromContainer();
	bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

	const char* getLocalIP() const
	{
	    return mSocket.getLocalIP();
	}

	const GameZone_t &getZoneID() const
	{
		return gameZone;
	}
	const DWORD getGameZoneID() const
	{
	    return gameZone.id;
	}
	const NetType getNetType() const
	{
	    return netType;
	}

private:

	GameZone_t gameZone;
	std::string name;
	NetType netType;

};

class UserContainer : public Singleton<UserContainer>
{
    public:
	void add(UserTask *iTask)
	{
	    zRWLock_scope_wrlock scope_wrlock(rwlock);
	    zoneUser[iTask->getGameZoneID()].insert(value_type(iTask->getNetType(), iTask));
	}
	void remove(UserTask *iTask)
	{
	    zRWLock_scope_wrlock scope_wrlock(rwlock);
	    NetUser_multimap &mtp = zoneUser[iTask->getGameZoneID()];
	    for(iter it=mtp.begin(); it!=mtp.end(); ++it)
	    {
		if(iTask == it->second)
		{
		    mtp.erase(it);
		    break;
		}
	    }
	}
	void broadcast(Cmd::t_NullCmd *pCmd, unsigned int nCmdLen)
	{
	    zRWLock_scope_rdlock scope_rdlock(rwlock);
	    for(ZoneUser_map::iterator map_it=zoneUser.begin(); map_it!=zoneUser.end(); ++map_it)
	    {
		_sendCmdToZone(map_it, pCmd, nCmdLen);
	    }
	}
	bool sendCmdToZone(const DWORD id, Cmd::t_NullCmd *pCmd, unsigned int nCmdLen)
	{
	    zRWLock_scope_rdlock scope_rdlock(rwlock);
	    ZoneUser_map::iterator map_it = zoneUser.find(id);
	    if(map_it != zoneUser.end())
	    {
		return _sendCmdToZone(map_it, pCmd, nCmdLen);
	    }
	    return false;
	}
	friend class SingletonFactory<UserContainer>;
	UserContainer()
	{
	}
	~UserContainer()
	{
	}
    private:
	typedef std::multimap<const NetType, UserTask*> NetUser_multimap;
	typedef NetUser_multimap::iterator iter;
	typedef NetUser_multimap::const_iterator const_iter;
	typedef NetUser_multimap::value_type value_type;
	typedef std::map<const DWORD, NetUser_multimap> ZoneUser_map;
	ZoneUser_map zoneUser;
	zRWLock rwlock;

	bool _sendCmdToZone(ZoneUser_map::iterator &map_it, Cmd::t_NullCmd *pCmd, unsigned int nCmdLen)
	{
	    NetType netType = NetType_near;
	    NetUser_multimap &mtp = map_it->second;
	    std::pair<iter, iter> eqp = mtp.equal_range(netType);
	    for(iter it=eqp.first; it!=eqp.second; ++it)
	    {
		UserTask *iTask = it->second;
		if(iTask && iTask->sendCmd(pCmd, nCmdLen))
		{
		    return true;
		}
	    }
	    eqp = mtp.equal_range(netType == NetType_near?NetType_far:NetType_near);
	    for(iter it=eqp.first; it!=eqp.second; ++it)
	    {
		UserTask *iTask = it->second;
		if(iTask && iTask->sendCmd(pCmd, nCmdLen))
		{
		    return true;
		}
	    }
	    return false;
	}
};
#endif

