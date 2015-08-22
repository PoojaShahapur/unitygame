#ifndef _zMNetService_h_
#define _zMNetService_h_

#include <iostream>
#include <string>
#include <ext/numeric>

#include "zThread.h"
#include "zService.h"
#include "zMTCPServer.h"
#include "zSocket.h"
/**
* \brief �����������
*
* ʵ���������������ܴ��룬�����Ƚ�ͨ��һ��
*
*/
class zMNetService : public zService
{

public:
	/**
	* \brief ����������
	*
	*/
	virtual ~zMNetService() { instance = NULL; };

	/**
	* \brief ���ݵõ���TCP/IP���ӻ�ȡһ����������
	*
	* \param sock TCP/IP�׽ӿ�
	* \param addr ��ַ
	*/
	virtual void newTCPTask(const int sock,const unsigned short srcPort) = 0;
	
	bool bind(const std::string &name, const unsigned short port)
	{
	    if(tcpServer)
		return tcpServer->bind(name, port);
	    else
		return false;
	}
protected:

	/**
	* \brief ���캯��
	* 
	* �ܱ����Ĺ��캯����ʵ����Singleton���ģʽ����֤��һ��������ֻ��һ����ʵ��
	*
	* \param name ����
	*/
	zMNetService(const std::string &name) : zService(name)
	{
		instance = this;

		serviceName = name;
		tcpServer = NULL;
	}

	bool init();
	bool serviceCallback();
	void final();

private:

	static zMNetService *instance;    /**< ���Ψһʵ��ָ�룬���������࣬��ʼ��Ϊ��ָ�� */
	std::string serviceName;      /**< ������������� */

public:
	zMTCPServer *tcpServer;        /**< TCP������ʵ��ָ�� */
};

#endif

