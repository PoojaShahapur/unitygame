/**
* \brief ʵ�����������
*
* 
*/
#include "zNetService.h"
#include "Zebra.h"

zNetService *zNetService::instance = NULL;

/**
* \brief ��ʼ������������
*
* ʵ��<code>zService::init</code>���麯��
*
* \param port �˿�
* \return �Ƿ�ɹ�
*/
bool zNetService::init(WORD port)
{
	Zebra::logger->debug("zNetService::init");
	if (!zService::init())
		return false;

	//��ʼ��������
	tcpServer = new zTCPServer(serviceName);
	if (NULL == tcpServer)
		return false;
	if (!tcpServer->bind(serviceName,port))
		return false;
#if 0
	// [ranqd] ��ʼ�������߳�
	pAcceptThread = new zAcceptThread( this, serviceName );
	if( pAcceptThread == NULL )
		return false;
	if(!pAcceptThread->start())
		return false;
#endif
	Zebra::logger->debug("zNetService::init bind(%s:%u)",serviceName.c_str(),port);
	return true;
}

/**
* \brief ��������������ص�����
*
* ʵ���麯��<code>zService::serviceCallback</code>����Ҫ���ڼ�������˿ڣ��������false���������򣬷���true����ִ�з���
*
* \return �ص��Ƿ�ɹ�
*/
bool zNetService::serviceCallback()
{
	// [ranqd] ÿ�����һ�������������
	struct sockaddr_in addr;
	int retcode = tcpServer->accept(&addr);
	if(retcode >= 0)
	{
		newTCPTask(retcode, &addr);
	}
	return true;
}
/**
* \brief �����������������
*
* ʵ�ִ��麯��<code>zService::final</code>��������Դ
*
*/
void zNetService::final()
{
	Zebra::logger->info("zNetService::final");
	SAFE_DELETE(tcpServer);
}

