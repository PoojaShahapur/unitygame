#ifndef __MINICLIENT_H_
#define __MINICLIENT_H_


#include <unistd.h>
#include <iostream>
#include "zTCPClient.h"
#include "MiniCommand.h"

/**
* \brief 定义小游戏服务器连接客户端类
*
*/
class MiniClient : public zTCPBufferClient
{

public:

	MiniClient(
		const std::string &name,
		const std::string &ip,
		const WORD port,
		const WORD serverid)
		: zTCPBufferClient(name,ip,port) 
	{
		wdServerID=serverid;
	};

	bool connectToMiniServer();
	void run();
	bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
	/**
	* \brief 获取场景服务器的编号
	*
	* \return 场景服务器编号
	*/
	const WORD getServerID() const
	{
		return wdServerID;
	}
private:
	WORD wdServerID;

};

extern MiniClient *miniClient;



#endif
