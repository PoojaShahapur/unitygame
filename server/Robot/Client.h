/*************************************************************************
 Author: wang
 Created Time: 2014年11月11日 星期二 11时53分21秒
 File Name: Robot/Client.h
 Description: 
 ************************************************************************/
#ifndef _ClientClient_h_
#define _ClientClient_h_
#include "zType.h"
#include <string>
#include "zTCPClientTask.h"
#include "Command.h"
struct ClientBase
{
    DWORD accid;
    DWORD loginTempID;
    BYTE role_num;
    QWORD qwGameTime;
    QWORD dwGameRunTime;

    bool waitingCharInfo;
    bool roleExist;
    
    zRTime startTime;

    std::string user;
    std::string name;
    std::string mapName;
    ClientBase()
    {
	accid = 0;
	loginTempID = 0;
	role_num = 0;
	qwGameTime = 0;
	dwGameRunTime = 0;

	roleExist = false;
	waitingCharInfo = false;
    }
};

class Client : private ClientBase, public zTCPClientTask
{
    public:
	Client(const std::string &user, const std::string &ip, const WORD &port);
	~Client();
	bool msgParse(const Cmd::t_NullCmd *ptNullCmd, const unsigned int nCmdLen);
	bool msgParse_time(const Cmd::stNullUserCmd *ptNull, const unsigned int nCmdLen);
	bool sendCmdIM(const void *pstrCmd, const int nCmdLen);
	bool sendCmd(const void *pstrCmd, const int nCmdLen);

	template<class T> inline bool SEND_CMD(T cmd)
	{
	    return sendCmd(&(cmd), sizeof(cmd));
	}

	inline std::string& getUser() {return user;}
	inline std::string& getName() {return name;}
	bool connect();
	bool loginGatewayServer();
	bool versionCmd();
	bool init(DWORD acc, DWORD tempid, DES_cblock key);
	bool timeAction();
	bool logonRole(const WORD &i=0);
	char pstrIP[MAX_IP_LENGTH];
	WORD wdPort;
    private:
	DES_cblock key_des;
	
	Cmd::SelectUserInfo charInfo[Cmd::MAX_CHARINFO];
};
#endif

