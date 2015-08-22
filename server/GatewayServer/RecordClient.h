#ifndef __RECORDCLIENT_H_
#define __RECORDCLIENT_H_


#include "zTCPClient.h"
#include "zMutex.h"
/**
* \brief �����뵵��������������
*
*/
class RecordClient : public zTCPBufferClient
{

public:

	/**
	* \brief ���캯��
	*
	* \param name ����
	* \param ip ��������ַ
	* \param port �������˿�
	*/
	RecordClient(
		const std::string &name,
		const std::string &ip,
		const WORD port)
		: zTCPBufferClient(name,ip,port) {};

	bool connectToRecordServer();
	void run();
	bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

};

extern RecordClient *recordClient;



#endif
