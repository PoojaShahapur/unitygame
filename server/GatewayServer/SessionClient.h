#ifndef __SESSIONCLIENT_H_
#define __SESSIONCLIENT_H_

#include <unistd.h>
#include <iostream>
#include "zTCPClient.h"
#include "SessionCommand.h"




/**
* \brief 定义会话服务器连接客户端类
*
*/
class SessionClient : public zTCPBufferClient
{

public:

	SessionClient(
		const std::string &name,
		const std::string &ip,
		const WORD port)
		: zTCPBufferClient(name,ip,port) {};

	bool connectToSessionServer();
	void run();
	bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

};

extern SessionClient *sessionClient;


#endif
