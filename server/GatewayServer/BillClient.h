#ifndef __BILLCLIENT_H_
#define __BILLCLIENT_H_

#include <unistd.h>
#include <iostream>

#include "zTCPClient.h"
#include "BillCommand.h"

/**
* \brief 定义计费服务器连接客户端类
*
*/
class BillClient : public zTCPBufferClient
{

public:

	BillClient(
		const std::string &name,
		const std::string &ip,
		const WORD port,
		const WORD serverid)
		: zTCPBufferClient(name,ip,port) 
	{
		wdServerID=serverid;
	};

	bool connectToBillServer();
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

extern BillClient *accountClient;


#endif
