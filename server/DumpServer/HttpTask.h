/*************************************************************************
 Author: wang
 Created Time: 2015��04��08�� ������ 11ʱ49��25��
 File Name: DumpServer/HttpTask.h
 Description: 
 ************************************************************************/
#ifndef _HttpTask_h_
#define _HttpTask_h_
#include "zHttpTask.h"
#include <vector>

class HttpTask : public zHttpTask
{
    public:
	HttpTask(zHttpTaskPool *pool, const int sock, const struct sockaddr_in *addr = NULL)
	    :zHttpTask(pool, sock, addr)
	{}

	~HttpTask();

	int httpCore();

    private:
	std::vector<char> _buffer;
};
#endif
