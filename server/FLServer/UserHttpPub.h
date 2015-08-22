/*************************************************************************
 Author: 
 Created Time: 2014年10月11日 星期六 14时44分43秒
 File Name: FLServer/UserHttpPub.h
 Description: 
 ************************************************************************/
#ifndef _UserHttpPub_h_
#define _UserHttpPub_h_

#include "zSingleton.h"
#include "UserCommand.h"
#include "zRWLock.h"
#include <string>
#define NO_UID_EXIT 2;
#define NO_ACCOUNT_EXIT 3;
#define DB_ERROR 4;

class UserHttpPub
{
    public:
	void consume_func(Cmd::UserServer::t_cmd_consume *pt, Cmd::UserServer::t_cmd_ret &cmd);
	void qbalance_func(Cmd::UserServer::t_cmd_qbalance *pt, Cmd::UserServer::t_cmd_ret &cmd);
    private:
	const std::string get_account(const unsigned int uid);
};

#include <map>
class UserConfigM : public Singleton<UserConfigM>
{
    public:
	typedef std::map<DWORD, DWORD> PointMap;

	bool init();
	bool reload();
	DWORD getPointByUid(const DWORD uid);
	bool consumePoint(const DWORD uid, const DWORD cpoint, DWORD &balance);
	friend class SingletonFactory<UserConfigM>;
	UserConfigM()
	{
	}
	~UserConfigM();
    private:
	bool initPointMap();
	PointMap pointsMap;
	zRWLock pointsLock;
};
#endif
