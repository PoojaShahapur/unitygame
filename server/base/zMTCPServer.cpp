/**
* \brief ʵ����zMTCPServer
*
* 
*/
#include "zMTCPServer.h"
#include "Zebra.h"
#include "zSocket.h"

#include <iostream>
#include <netinet/in.h>
#include <arpa/inet.h>
/**
* \brief ���캯��,���ڹ���һ��������zMTCPServer����
* \param name ����������
*/
zMTCPServer::zMTCPServer(const std::string &name)
	: name(name)
{
	kdpfd = epoll_create(1);
	assert(-1 != kdpfd);
	epfds.resize(8);
	Zebra::logger->info("zMTCPServer::zMTCPServer");
}

/**
* \brief ��������,��������һ��zMTCPServer����
*
*
*/
zMTCPServer::~zMTCPServer() 
{
	Zebra::logger->info("zMTCPServer::~zMTCPServer");
	TEMP_FAILURE_RETRY(::close(kdpfd));
	for(Sock2Port_const_iterator it = mapper.begin(); it != mapper.end(); ++it)
	{
	if (-1 != it->first) 
	{
		::shutdown(it->first, SHUT_RD);
		TEMP_FAILURE_RETRY(::close(it->first));
	}
	}
	mapper.clear();
}

/**
* \brief �󶨼�������ĳһ���˿�
* \param name �󶨶˿�����
* \param port ����󶨵Ķ˿�
* \return ���Ƿ�ɹ�
*/
bool zMTCPServer::bind(const std::string &name,const WORD port)
{
	Zebra::logger->info("zMTCPServer::bind");
	zMutex_scope_lock scope_lock(mlock);
	struct sockaddr_in addr;
	int sock;
	for(Sock2Port_const_iterator it = mapper.begin(); it != mapper.end(); ++it)
	{
	    if(it->second == port)
	    {
		Zebra::logger->warn("port:%u was be used",port);
		return false;
	    }
	}

	sock = ::socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	if (-1 == sock) 
	{
		Zebra::logger->error("zMTCPServer::bind �����׽ӿ�ʧ�� %s",strerror(errno));
		return false;
	}

	//�����׽ӿ�Ϊ������״̬
	int reuse = 1;
	if (-1 == ::setsockopt(sock,SOL_SOCKET,SO_REUSEADDR,&reuse,sizeof(reuse))) 
	{
		Zebra::logger->error("���������׽ӿ�Ϊ������״̬");
		TEMP_FAILURE_RETRY(::close(sock));
		return false;
	}

	//�����׽ӿڷ��ͽ��ջ���,���ҷ������ı�����accept֮ǰ����
	int window_size = 128 * 1024;
	if (-1 == ::setsockopt(sock,SOL_SOCKET,SO_RCVBUF,&window_size,sizeof(window_size)))
	{
		TEMP_FAILURE_RETRY(::close(sock));
		return false;
	}
	if (-1 == ::setsockopt(sock,SOL_SOCKET,SO_SNDBUF,&window_size,sizeof(window_size)))
	{
		TEMP_FAILURE_RETRY(::close(sock));
		return false;
	}

	bzero(&addr,sizeof(addr));
	addr.sin_family = AF_INET;
	addr.sin_addr.s_addr = htonl(INADDR_ANY);
	addr.sin_port = htons(port);

	int retcode = ::bind(sock,(struct sockaddr *) &addr,sizeof(addr));
	if (-1 == retcode) 
	{
		Zebra::logger->error("���ܰ󶨷������˿�");
		TEMP_FAILURE_RETRY(::close(sock));
		return false;
	}

	retcode = ::listen(sock,MAX_WAITQUEUE);
	if (-1 == retcode) 
	{
		Zebra::logger->error("�����׽ӿ�ʧ��");
		TEMP_FAILURE_RETRY(::close(sock));
		return false;
	}
	struct epoll_event ev;
	ev.events = EPOLLIN;
	ev.data.fd = sock;
	assert(0 == epoll_ctl(kdpfd, EPOLL_CTL_ADD, sock, &ev));
	
	mapper.insert(Sock2Port_value_type(sock, port));
	if(mapper.size() > epfds.size())
	{
	    epfds.resize(mapper.size() + 8);
	}
	Zebra::logger->info("��ʼ�� %s:%u �ɹ�",name.c_str(),port);

	return true;
}

/**
* \brief ���ܿͻ��˵�����
*
*
* \param addr ���صĵ�ַ
* \return ���صĿͻ����׽ӿ�
*/
int zMTCPServer::accept(Sock2Port &res)
{
    zMutex_scope_lock scope_lock(mlock);
    int retval = 0;
    int rc = epoll_wait(kdpfd, &epfds[0], mapper.size(), T_MSEC);
    if(rc > 0)
    {
	for(int i = 0; i < rc; i++)
	{
	    if(epfds[i].events & EPOLLIN)
	    {
		res.insert(Sock2Port_value_type(TEMP_FAILURE_RETRY(::accept(epfds[i].data.fd, NULL,NULL)), mapper[epfds[i].data.fd]));
		retval++;
	    }
	}
    }
    return retval;
}

