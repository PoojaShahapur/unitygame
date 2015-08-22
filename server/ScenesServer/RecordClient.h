#ifndef _RecordClient_h_
#define _RecordClient_h_

#include <unistd.h>
#include <iostream>

#include "zTCPClient.h"
#include "RecordCommand.h"
#include "zMisc.h"
#include "zSocket.h"
#include "MessageQueue.h"

/**
 * brief ���� �������������ӿͻ�����
 *
 * ���� �뵵����������������ȡ����
 * TODO ��ʱֻ��һ������������
 * 
 */
class RecordClient : public zTCPBufferClient,public MessageQueue
{

  public:

    /**
     * \brief ���캯��
     * ���ڵ��������Ѿ���ѹ�����ģ����ڵײ㴫���ʱ��Ͳ���Ҫѹ����
     * \param name ����
     * \param ip ��ַ
     * \param port �˿�
     */
    RecordClient(const std::string &name,const std::string &ip,const WORD port)
      : zTCPBufferClient(name,ip,port,false) {};

    bool connectToRecordServer();

    void run();
    bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    bool cmdMsgParse(const Cmd::t_NullCmd *,const DWORD);

};

extern RecordClient *recordClient;

#endif

