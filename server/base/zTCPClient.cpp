/**
* \brief ʵ����zTCPClient,TCP���ӿͻ��ˡ�
*
* 
*/


#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <unistd.h>
#include <sys/socket.h>
#include <sys/types.h>
#include <netinet/in.h>
#include <arpa/inet.h>

#include "zTCPClient.h"
#include "Zebra.h"
//CmdAnalysis zTCPClient::analysis("Clientָ���ͳ��",600);
/**
* \brief ����һ������������TCP����
*
*
* \return �����Ƿ�ɹ�
*/
bool zTCPClient::connect()
{
#ifdef _DEBUG
	Zebra::logger->debug("zTCPClient::connect");
#endif //_DEBUG
	int retcode;
	int nSocket;
	struct sockaddr_in addr;

	nSocket = ::socket(PF_INET,SOCK_STREAM,0);
	if (-1 == nSocket)
	{
		Zebra::logger->error("zTCPClient::connect �����׽ӿ�ʧ�� %s",strerror(errno));
		return false;
	}

	//�����׽ӿڷ��ͽ��ջ���,���ҿͻ��˵ı�����connect֮ǰ����
	int window_size = 128 * 1024;
	retcode = ::setsockopt(nSocket,SOL_SOCKET,SO_RCVBUF,(char*)&window_size,sizeof(window_size));
	if (-1 == retcode)
	{
		TEMP_FAILURE_RETRY(::close(nSocket));
		return false;
	}
	retcode = ::setsockopt(nSocket,SOL_SOCKET,SO_SNDBUF,(char*)&window_size,sizeof(window_size));
	if (-1 == retcode)
	{
		TEMP_FAILURE_RETRY(::close(nSocket));
		return false;
	}

	bzero(&addr,sizeof(addr));
	addr.sin_family = AF_INET;
	addr.sin_addr.s_addr = inet_addr(ip.c_str());
	addr.sin_port = htons(port);

	retcode = TEMP_FAILURE_RETRY(::connect(nSocket,(struct sockaddr *) &addr,sizeof(addr)));
	if (-1 == retcode)
	{
		Zebra::logger->error("������������(%s:%u) ������ʧ��",ip.c_str(),port);
		TEMP_FAILURE_RETRY(::close(nSocket));
		return false;
	}

	pSocket = new zSocket(nSocket,&addr,compress);
	if (NULL == pSocket)
	{
		Zebra::logger->fatal("û���㹻���ڴ�,���ܴ���zSocketʵ��");
		TEMP_FAILURE_RETRY(::close(nSocket));
		return false;
	}

	Zebra::logger->info("create to server connect %s:%u OK!!",ip.c_str(),port);

	return true;
}

/**
* \brief ���׽ӿڷ���ָ��
*
*
* \param pstrCmd �����͵�ָ��
* \param nCmdLen ������ָ��Ĵ�С
* \return �����Ƿ�ɹ�
*/
bool zTCPClient::sendCmd(const void *pstrCmd,const int nCmdLen)
{
#ifdef _DEBUG
	Zebra::logger->debug("zTCPClient::sendCmd");
#endif //_DEBUG
	if (NULL == pSocket) 
		return false;
	else
	{
		/*
		Cmd::t_NullCmd *pNullCmd = (Cmd::t_NullCmd *)pstrCmd;
		analysis.add(pNullCmd->cmd,pNullCmd->para,nCmdLen);
		// */
		return pSocket->sendCmd(pstrCmd,nCmdLen);
	}
}
/**
* \brief ����zThread�еĴ��麯��,���̵߳����ص�����,���ڴ�����յ���ָ��
*
*/
void zTCPClient::run()
{
#ifdef _DEBUG
	Zebra::logger->error("zTCPClient::remoteport= %u localport = %u",pSocket->getPort(),pSocket->getLocalPort());

#endif //_DEBUG
	while(!isFinal())
	{
		BYTE pstrCmd[zSocket::MAX_DATASIZE];
		int nCmdLen;

		nCmdLen = pSocket->recvToCmd(pstrCmd,zSocket::MAX_DATASIZE,false);
		if (nCmdLen > 0) 
		{
			Cmd::t_NullCmd *pNullCmd = (Cmd::t_NullCmd *)pstrCmd;
			if (Cmd::CMD_NULL == pNullCmd->cmd
				&& Cmd::PARA_NULL == pNullCmd->para)
			{
				//Zebra::logger->debug("�ͻ����յ������ź�");
				if (!sendCmd(pstrCmd,nCmdLen))
				{
					//����ָ��ʧ��,�˳�ѭ��,�����߳�
					break;
				}
			}
			else
				msgParse(pNullCmd,nCmdLen);
		}
		else if (-1 == nCmdLen)
		{
			//����ָ��ʧ��,�˳�ѭ��,�����߳�
			Zebra::logger->error("error::remoteport= %u localport = %u",pSocket->getPort(),pSocket->getLocalPort());

			Zebra::logger->error("����ָ��ʧ��1,�ر� %s",getThreadName().c_str());
			break;
		}
	}
}

bool zTCPBufferClient::sendCmd(const void *pstrCmd,const int nCmdLen)
{
#ifdef _DEBUG
	Zebra::logger->debug("zTCPBufferClient::sendCmd");
#endif //_DEBUG
	if (pSocket)
 		return pSocket->sendCmd(pstrCmd,nCmdLen,_buffered);
	else
		return false;
}

bool zTCPBufferClient::ListeningRecv()
{
#ifdef _DEBUG
	Zebra::logger->debug("zTCPBufferClient::ListeningRecv");
#endif //_DEBUG
	int retcode = pSocket->recvToBuf_NoPoll();
	if (-1 == retcode) return false;
	for(;;)
	{
		BYTE pstrCmd[zSocket::MAX_DATASIZE];
		int nCmdLen = pSocket->recvToCmd_NoPoll(pstrCmd,sizeof(pstrCmd));
		//����ֻ�Ǵӻ���ȡ���ݰ�,���Բ������,û������ֱ�ӷ���
		if (nCmdLen <= 0) break;
		else
		{
			Cmd::t_NullCmd *pNullCmd = (Cmd::t_NullCmd *)pstrCmd;
			if (Cmd::CMD_NULL == pNullCmd->cmd
				&& Cmd::PARA_NULL == pNullCmd->para)
			{
				//Zebra::logger->debug("�ͻ����յ������ź�");
				//����ָ��ʧ��,�˳�ѭ��,�����߳�
				if (!sendCmd(pstrCmd,nCmdLen)) return false;
			}
			else msgParse(pNullCmd,nCmdLen);
		}
	}
	return true;
}

bool zTCPBufferClient::ListeningSend()
{  
#ifdef _DEBUG
	Zebra::logger->debug("zTCPBufferClient::ListeningSend pSocket:%p",pSocket);
#endif //_DEBUG
	if (pSocket)
		return pSocket->sync();
	else
		return false;
}

void zTCPBufferClient::sync()
{
#ifdef _DEBUG
	Zebra::logger->debug("zTCPBufferClient::sync");
#endif //_DEBUG
	if (pSocket)
		pSocket->force_sync();
}

void zTCPBufferClient::run()
{  
	_buffered = true;
	int epfd = epoll_create(256);
	assert(-1 != epfd);
	int epfd_r = epoll_create(256);
	assert(-1 != epfd_r);
	pSocket->addEpoll(epfd, EPOLLOUT|EPOLLIN|EPOLLERR|EPOLLPRI, NULL);
	pSocket->addEpoll(epfd_r, EPOLLIN|EPOLLERR|EPOLLPRI, NULL);
	struct epoll_event ep_event, ep_event_r;
	ep_event.events = 0, ep_event_r.events = 0;

	zRTime currentTime;
	zRTime _1_msec(currentTime), _50_msec(currentTime);

	while(!isFinal())
	{
		zThread::msleep(2);
		currentTime.now();
		if(_1_msec.elapse(currentTime) >= 2)
		{
			_1_msec = currentTime;
			if(TEMP_FAILURE_RETRY(::epoll_wait(epfd_r, &ep_event_r, 1, 0)) > 0)
			{
				if(ep_event_r.events & (EPOLLERR | EPOLLPRI))
				{
				    Zebra::logger->fatal("%s: socket error",__PRETTY_FUNCTION__);
					break;
				}
				else
				{
					if(ep_event_r.events & EPOLLIN)
					{
						if(!ListeningRecv())
						{
						    Zebra::logger->fatal("%s: socket read operation error",__PRETTY_FUNCTION__);
							break;
						}
					}
				}
				ep_event_r.events = 0;
			}
		}

		if(_50_msec.elapse(currentTime) >= (usleep_time/1000))
		{
			_50_msec = currentTime;
			if(TEMP_FAILURE_RETRY(::epoll_wait(epfd, &ep_event, 1, 0)) > 0)
			{
				if(ep_event.events & (EPOLLERR | EPOLLPRI))
				{
				    Zebra::logger->fatal("%s: socket error",__PRETTY_FUNCTION__);
					break;
				}
				else
				{
					if(ep_event.events & EPOLLIN)
					{
						if(!ListeningRecv())
						{
						    Zebra::logger->fatal("%s: socket read op    eration error",__PRETTY_FUNCTION__);
							break;
						}
					}
					if(ep_event.events & EPOLLOUT)
					{
						if(!ListeningSend())
						{
						    Zebra::logger->fatal("%s: socket write op    eration error",__PRETTY_FUNCTION__);
							break;
						}
					}
				}
				ep_event.events = 0;
			}
		}
	}
	//��֤��������ݷ������
	sync();
	_buffered = false;
}

