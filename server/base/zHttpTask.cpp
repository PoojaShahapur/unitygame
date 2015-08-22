/**
* \brief 实现线程池类,用于处理多连接服务器
*
* 
*/
#include "zSocket.h"
#include "zThread.h"
#include "zHttpTask.h"
#include "Zebra.h"
#include "zHttpTaskPool.h"

#include <assert.h>
#include <sstream>
#include <string>
#include <vector>
#include <iostream>

/**
* \brief 向套接口发送指令,如果缓冲标志设置,则发送是直接拷贝到缓冲区队列中,实际的发送动作在另外一个线程做
*
*
* \param pstrCmd 待发送的指令
* \param nCmdLen 待发送指令的大小
* \return 发送是否成功
*/
bool zHttpTask::sendCmd(const void *pstrCmd,int nCmdLen)
{
    if(pSocket)
	return pSocket->sendRawDataIM(pstrCmd, nCmdLen);
    else
	return false;
}
