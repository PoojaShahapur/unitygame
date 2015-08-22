#ifndef _SERVERACL_H_
#define _SERVERACL_H_
#include "zType.h"
#include <string>
#include "zSingleton.h"
#include <map>
#include "zRWLock.h"

struct ACLZone
{
	GameZone_t gameZone;
	std::string ip;
	WORD port;
	std::string name;
	std::string desc;

	ACLZone()
	{
		port = 0;
	}
	ACLZone(const ACLZone &acl)
	{
		gameZone = acl.gameZone;
		ip = acl.ip;
		port = acl.port;
		name = acl.name;
		desc = acl.desc;
	}
};

class ServerACL : public Singleton<ServerACL>
{

public:
	friend class SingletonFactory<ServerACL>;
	
	ServerACL() {};
	~ServerACL() {};

	bool init();
	void final();
	bool check(const char *strIP,const WORD port,GameZone_t &gameZone,std::string &name);

private:

	bool add(const ACLZone &zone);



	/*struct less_str : public std::less<GameZone_t>
	{

	bool operator()(const GameZone_t & x, const GameZone_t & y) const 
	{
	if (x.id < y.id )
	return true;

	return false;
	}
	};*/


	/**
	* \brief hashº¯Êý
	*
	*/
	/*struct GameZone_hash :public hash_compare<GameZone_t,less_str>
	{
	size_t operator()(const GameZone_t &gameZone) const
	{
	//hash<DWORD> H;
	return 1;//Hash<DWORD>(gameZone.id);
	}

	//static const unsigned int bucket_size = 100;
	//static const unsigned int min_buckets = 100;
	};*/


	typedef std::map<const GameZone_t,ACLZone> Container;
	Container datas;
	zRWLock rwlock;

};

#endif

