/**
* \brief ʵ����zTCPServer
*
* 
*/
#include "zTCPServer.h"
#include "Zebra.h"
#include <iostream>
#include <netinet/in.h>
#include <arpa/inet.h>
/**
* \brief ���캯��,���ڹ���һ��������zTCPServer����
* \param name ����������
*/
zTCPServer::zTCPServer(const std::string &name)
	: name(name),
	sock(-1)
{
	kdpfd = epoll_create(1);
	assert(-1 != kdpfd);
	Zebra::logger->info("zTCPServer::zTCPServer");
}

/**
* \brief ��������,��������һ��zTCPServer����
*
*
*/
zTCPServer::~zTCPServer() 
{
	Zebra::logger->info("zTCPServer::~zTCPServer");
	TEMP_FAILURE_RETRY(::close(kdpfd));
	if (-1 != sock) 
	{
		::shutdown(sock, SHUT_RD);
		TEMP_FAILURE_RETRY(::close(sock));
		sock = -1;
	}
}

/**
* \brief �󶨼�������ĳһ���˿�
* \param name �󶨶˿�����
* \param port ����󶨵Ķ˿�
* \return ���Ƿ�ɹ�
*/
bool zTCPServer::bind(const std::string &name,const WORD port)
{
	Zebra::logger->info("zTCPServer::bind");
	struct sockaddr_in addr;

	if (-1 != sock) 
	{
		Zebra::logger->error("server may be init already");;
		return false;
	}

	sock = ::socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	if (-1 == sock) 
	{
		Zebra::logger->error("zTCPServer::bind �����׽ӿ�ʧ�� %s",strerror(errno));
		return false;
	}

	//�����׽ӿ�Ϊ������״̬
	int reuse = 1;
	if (-1 == ::setsockopt(sock,SOL_SOCKET,SO_REUSEADDR,(char*)&reuse,sizeof(reuse))) 
	{
		Zebra::logger->error("���������׽ӿ�Ϊ������״̬");
		TEMP_FAILURE_RETRY(::close(sock));
		sock = -1;
		return false;
	}

	//�����׽ӿڷ��ͽ��ջ���,���ҷ������ı�����accept֮ǰ����
	int window_size = 128 * 1024;
	if (-1 == ::setsockopt(sock,SOL_SOCKET,SO_RCVBUF,(char*)&window_size,sizeof(window_size)))
	{
		TEMP_FAILURE_RETRY(::close(sock));
		return false;
	}
	if (-1 == ::setsockopt(sock,SOL_SOCKET,SO_SNDBUF,(char*)&window_size,sizeof(window_size)))
	{
		TEMP_FAILURE_RETRY(::close(sock));
		sock = -1;
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
		sock = -1;
		return false;
	}

	retcode = ::listen(sock,MAX_WAITQUEUE);
	if (-1 == retcode) 
	{
		Zebra::logger->error("zTCPServer::bind listen error");
		TEMP_FAILURE_RETRY(::close(sock));
		sock = -1;
		return false;
	}
	struct epoll_event ev;
	ev.events = EPOLLIN;
	ev.data.ptr = NULL;
	assert(0 == epoll_ctl(kdpfd, EPOLL_CTL_ADD, sock, &ev));

	Zebra::logger->info("zTCPServer::bind %s:%u OK!!!",name.c_str(),port);

	return true;
}

/**
* \brief ���ܿͻ��˵�����
*
*
* \param addr ���صĵ�ַ
* \return ���صĿͻ����׽ӿ�
*/
int zTCPServer::accept(struct sockaddr_in *addr)
{
	////  Zebra::logger->info("zTCPServer::accept");
	//  int len = sizeof(struct sockaddr_in);
	//  bzero(addr,sizeof(struct sockaddr_in));
	////// [ranqd] �˴�����poll����׽ӿ�״̬�������������̹߳�����޷������˳�
	////  struct pollfd pfd;
	////  pfd.fd = sock;
	////  pfd.events = POLLIN;
	////  pfd.revents = 0;
	////  int rc = ::poll(&pfd,1,T_MSEC);
	////  if (1 == rc && (pfd.revents & POLLIN))
	////    //׼���ý���
	//    return ::WSAAccept(sock,(struct sockaddr *)addr,&len,NULL, NULL );
	//
	//  //return -1;


	socklen_t len = sizeof(struct sockaddr_in); 
	bzero(addr, sizeof(struct sockaddr_in)); 

	struct epoll_event ev; 
	int rc = epoll_wait(kdpfd, &ev, 1, T_MSEC);//����kdpfd is the library function epoll_create(int size)��reture value.Here is called by epoll_create(1).You can man it.
	if(1 == rc && (ev.events & EPOLLIN)) 
		return TEMP_FAILURE_RETRY(::accept(sock, (struct sockaddr*)addr, &len)); 
	return -1; 
}

