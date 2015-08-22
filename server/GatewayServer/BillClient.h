#ifndef __BILLCLIENT_H_
#define __BILLCLIENT_H_

#include <unistd.h>
#include <iostream>

#include "zTCPClient.h"
#include "BillCommand.h"

/**
* \brief ����Ʒѷ��������ӿͻ�����
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
	* \brief ��ȡ�����������ı��
	*
	* \return �������������
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
