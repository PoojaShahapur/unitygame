#ifndef _zMTCPServer_h_
#define _zMTCPServer_h_
#include <unistd.h>
#include <sys/socket.h>
#include <sys/types.h>
#include <string>
#include <map>
#include "zSocket.h"
#include "zNoncopyable.h"
/**
* \brief zMTCPServer�࣬��װ�˷���������ģ�飬���Է���Ĵ���һ�����������󣬵ȴ��ͻ��˵�����
*
*/
class zMTCPServer : private zNoncopyable
{

public:
	typedef std::map<int, unsigned short> Sock2Port;
	typedef Sock2Port::value_type Sock2Port_value_type;
	typedef Sock2Port::iterator Sock2Port_iterator;
	typedef Sock2Port::const_iterator Sock2Port_const_iterator;

	zMTCPServer(const std::string &name);
	~zMTCPServer();
	bool bind(const std::string &name,const WORD port);
	int accept(Sock2Port &res);

private:

	static const int T_MSEC =2100;      /**< ��ѯ��ʱ������ */
	static const int MAX_WAITQUEUE = 2000;  /**< ���ȴ����� */

	std::string name;            /**< ���������� */
	Sock2Port mapper;
	zMutex mlock;
	int kdpfd;
	std::vector<struct epoll_event> epfds;
}; 

#endif


