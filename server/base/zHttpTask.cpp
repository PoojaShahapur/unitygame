/**
* \brief ʵ���̳߳���,���ڴ�������ӷ�����
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
* \brief ���׽ӿڷ���ָ��,��������־����,������ֱ�ӿ�����������������,ʵ�ʵķ��Ͷ���������һ���߳���
*
*
* \param pstrCmd �����͵�ָ��
* \param nCmdLen ������ָ��Ĵ�С
* \return �����Ƿ�ɹ�
*/
bool zHttpTask::sendCmd(const void *pstrCmd,int nCmdLen)
{
    if(pSocket)
	return pSocket->sendRawDataIM(pstrCmd, nCmdLen);
    else
	return false;
}
