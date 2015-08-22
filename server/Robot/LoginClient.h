#ifndef _LoginClient_h_
#define _LoginClient_h_

#include <unistd.h>
#include <iostream>
#include "zTCPClient.h"
#include "Command.h"

class LoginClient : public zTCPClient
{

  public:

    /**
     * \brief 构造函数
     * 由于档案数据已经是压缩过的，故在底层传输的时候就不需要压缩了
     * \param name 名称
     * \param ip 地址
     * \param port 端口
     */
    LoginClient(
        const std::string &name,
        const std::string &ip,
        const WORD port)
      : zTCPClient(name,ip,port,true) 
    {
	bzero(pstrIP, sizeof(pstrIP));
	wdPort = 0;
	createCountry = 0;
	accid = 0;
	loginTempID = 0;
    }

    bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    static WORD msgParse_login(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    bool connect();
    bool versionCmd();
    bool connect(const char *ip, const WORD port)
    {
	this->ip = ip;
	this->port = port;
	return connect();
    }
    loginLoginServer_retVal loginLoginServer(const char *name, const char *passwd);
    bool getGateInfo(char *ip, WORD &port, DWORD &acc, DWORD &tempid)
    {
	memmove(ip, pstrIP, sizeof(pstrIP));
	port = wdPort;
	if(wdPort == 0)
	    return false;
	acc = accid;
	tempid = loginTempID;
	accid = 0;
	loginTempID = 0;
	close();
	bzero(pstrIP, sizeof(pstrIP));
	wdPort = 0;
	return true;
    }

    void get_key_des(char* key)
    {
	memmove(key, key_des, sizeof(DES_cblock));
    }

    void UseIPEntry(BYTE *pszSrc, int iNum);
    bool requestIP();
private:
    char pstrIP[MAX_IP_LENGTH];
    BYTE lpstrIP[MAX_IP_LENGTH];
    WORD wdPort;
    DES_cblock key_des;
    DWORD createCountry;
    DWORD accid;
    DWORD loginTempID;

};

#endif
