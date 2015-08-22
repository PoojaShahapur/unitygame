/*************************************************************************
 Author: wang
 Created Time: 2015年04月08日 星期三 11时52分28秒
 File Name: DumpServer/HttpTask.cpp
 Description: 
 ************************************************************************/
#include "HttpTask.h"
#include "zDBConnPool.h"
#include "DumpServer.h"
#pragma pack(1)

struct DumpData
{
    char account[MAX_NAMESIZE];
    char userName[MAX_NAMESIZE];
    char gameName[MAX_NAMESIZE];
    char zoneName[MAX_NAMESIZE];
    DWORD version;
    DWORD logSize;
    DWORD descSize;
    DWORD dataSize;
};
#pragma pack()

HttpTask::~HttpTask()
{
    if(!_buffer.empty())
    {
	bool unzip_ok = false;
	int retry_count = 0;
	unsigned int nRecvCmdLen = *((unsigned int *)&_buffer[0]);
	Zebra::logger->debug("HttpTask::~HttpTask %u,%u", _buffer.size(), nRecvCmdLen);
	std::vector<char> _unzip_buffer(_buffer.size()*3);
	uLong nUnzipLen = _unzip_buffer.size();
do_retry:
	switch(uncompress((Bytef *)&_unzip_buffer[0], &nUnzipLen, (const Bytef *)&_buffer[4], nRecvCmdLen))
	{
	    case Z_OK:
		Zebra::logger->debug("Z_OK");
		unzip_ok = true;
		break;
	    case Z_MEM_ERROR:
		Zebra::logger->debug("Z_MEM_ERROR");
		break;
	    case Z_BUF_ERROR:
		Zebra::logger->debug("Z_BUF_ERROR");
		if(++retry_count <= 16)
		{
		    _unzip_buffer.resize(_unzip_buffer.size()*2);
		    nUnzipLen = _unzip_buffer.size();
		    goto do_retry;
		}
		break;
	    case Z_DATA_ERROR:
		Zebra::logger->debug("Z_DATA_ERROR");
		break;
	}

	if(unzip_ok)
	{
	    connHandleID handle = DumpServer::dbConnPool->getHandle();
	    if((connHandleID)-1 == handle)
	    {
		Zebra::logger->error("不能获取数据库句柄");
	    }
	    else
	    {
		const char *sql_template = "";
		char sqlBuf[512];
		memset(sqlBuf, 0, sizeof(sqlBuf));

		char timeBuffer[128];
		memset(timeBuffer, 0, sizeof(timeBuffer));
		time_t  t;
		struct tm *tmp;
		t = time(NULL);
		tmp = localtime(&t);
		strftime(timeBuffer, sizeof(timeBuffer),"%Y%m%d", tmp);

		sprintf(sqlBuf, sql_template, timeBuffer);
		if(DumpServer::dbConnPool->execSql(handle, sqlBuf, strlen(sqlBuf)))
		{
		    Zebra::logger->error("执行建表sql出错");
		}
		else
		{
		    DumpData *pData = (DumpData*)&_unzip_buffer[0];
		    pData->account[MAX_NAMESIZE-1] = '\0';
		    pData->userName[MAX_NAMESIZE-1] = '\0';
		    pData->gameName[MAX_NAMESIZE-1] = '\0';
		    pData->zoneName[MAX_NAMESIZE-1] = '\0';
		    std::vector<char> log(2*pData->logSize+1), desc(2*pData->descSize+1), data(2*pData->dataSize+1);
		    DumpServer::dbConnPool->escapeString(handle, &_unzip_buffer[sizeof(DumpData)], &log[0], pData->logSize);
		    DumpServer::dbConnPool->escapeString(handle, &_unzip_buffer[sizeof(DumpData)+pData->logSize], &desc[0], pData->descSize);
		    DumpServer::dbConnPool->escapeString(handle, &_unzip_buffer[sizeof(DumpData)+pData->logSize+pData->descSize], &data[0], pData->dataSize);

		    std::ostringstream strSql;
		    strSql<<"INSERT INTO 'ErrorDump" <<timeBuffer<<"' VALUES(\'"
			<<pData->account<<"\', \'"
			<<pData->userName<<"\', \'"
			<<pData->gameName<<"\', \'"
			<<pData->zoneName<<"\', \'"
			<<pData->version<<"\', \'"
			<<(char *)&log[0]<<"\', \'"
			<<(char *)&desc[0]<<"\', \'"
			<<(char *)&data[0]<<"\'";

		    char *tmpversion = strstr((char *)&log[0], "Exception Address");
		    std::string Ver="";
		    if(NULL != tmpversion)
		    {
			char *temp = NULL;
			temp = strtok((char *)&tmpversion[18], "\\r\\n");
			if(NULL != temp)
			{
			    Ver = temp;
			}
		    }
		    strSql <<",\'"<<Ver<<"\'";

		    char *tmpaddress = strstr((char *)&log[0], "Version");
		    std::string Add="";
		    if(NULL != tmpaddress)
		    {
			char *temp = NULL;
			temp = strtok((char *)&tmpaddress[10], "\\r\\n");
			if(NULL != temp)
			{
			    Add = temp;
			}
		    }
		    strSql <<",\'"<<Add<<"\'";

		    memset(timeBuffer, 0, sizeof(timeBuffer));
		    strftime(timeBuffer, sizeof(timeBuffer),"%Y%m%d%H%M%S", tmp);
		    strSql<<",\'"<<timeBuffer<<"\')";
			
		    if(DumpServer::dbConnPool->execSql(handle, strSql.str().c_str(), strSql.str().length()))
		    {
			Zebra::logger->error("执行sql语句出错");
		    }

		}
		DumpServer::dbConnPool->putHandle(handle);
	    }
	}

    }
}

int HttpTask::httpCore()
{
    char my_buffer[zSocket::MAX_DATABUFFERSIZE];
    int retcode;

do_retry:
    retcode = pSocket->recvToCmd_NoPoll(my_buffer, sizeof(my_buffer));
    if(retcode > 0)
    {
	size_t len = _buffer.size();
	_buffer.resize(len+retcode);
	bcopy(my_buffer, &_buffer[len], retcode);
	if(sizeof(my_buffer) == retcode)
	    goto do_retry;
	return 0;
    }
    else
	return retcode;
}
