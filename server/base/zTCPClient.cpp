/**
* \brief 实现类zTCPClient,TCP连接客户端。
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
//CmdAnalysis zTCPClient::analysis("Client指令发送统计",600);
/**
* \brief 建立一个到服务器的TCP连接
*
*
* \return 连接是否成功
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
		Zebra::logger->error("zTCPClient::connect 创建套接口失败 %s",strerror(errno));
		return false;
	}

	//设置套接口发送接收缓冲,并且客户端的必须在connect之前设置
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
		Zebra::logger->error("创建到服务器(%s:%u) 的连接失败",ip.c_str(),port);
		TEMP_FAILURE_RETRY(::close(nSocket));
		return false;
	}

	pSocket = new zSocket(nSocket,&addr,compress);
	if (NULL == pSocket)
	{
		Zebra::logger->fatal("没有足够的内存,不能创建zSocket实例");
		TEMP_FAILURE_RETRY(::close(nSocket));
		return false;
	}

	Zebra::logger->info("create to server connect %s:%u OK!!",ip.c_str(),port);

	return true;
}

/**
* \brief 向套接口发送指令
*
*
* \param pstrCmd 待发送的指令
* \param nCmdLen 待发送指令的大小
* \return 发送是否成功
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
* \brief 重载zThread中的纯虚函数,是线程的主回调函数,用于处理接收到的指令
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
				//Zebra::logger->debug("客户端收到测试信号");
				if (!sendCmd(pstrCmd,nCmdLen))
				{
					//发送指令失败,退出循环,结束线程
					break;
				}
			}
			else
				msgParse(pNullCmd,nCmdLen);
		}
		else if (-1 == nCmdLen)
		{
			//接收指令失败,退出循环,结束线程
			Zebra::logger->error("error::remoteport= %u localport = %u",pSocket->getPort(),pSocket->getLocalPort());

			Zebra::logger->error("接收指令失败1,关闭 %s",getThreadName().c_str());
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
		//这里只是从缓冲取数据包,所以不会出错,没有数据直接返回
		if (nCmdLen <= 0) break;
		else
		{
			Cmd::t_NullCmd *pNullCmd = (Cmd::t_NullCmd *)pstrCmd;
			if (Cmd::CMD_NULL == pNullCmd->cmd
				&& Cmd::PARA_NULL == pNullCmd->para)
			{
				//Zebra::logger->debug("客户端收到测试信号");
				//发送指令失败,退出循环,结束线程
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
	//保证缓冲的数据发送完成
	sync();
	_buffered = false;
}

