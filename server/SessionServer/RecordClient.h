#ifndef _RecordClient_h_
#define _RecordClient_h_

#include <unistd.h>
#include <iostream>
#include "zTCPClient.h"
#include "RecordCommand.h"
#include "MessageQueue.h"

class RecordClient : public zTCPClient,public MessageQueue
{

  public:

    /**
     * \brief 构造函数
     * 由于档案数据已经是压缩过的，故在底层传输的时候就不需要压缩了
     * \param name 名称
     * \param ip 地址
     * \param port 端口
     */
    RecordClient(
        const std::string &name,
        const std::string &ip,
        const WORD port)
      : zTCPClient(name,ip,port,false) {};

    bool connectToRecordServer();

    void run();
    bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    bool cmdMsgParse(const Cmd::t_NullCmd *,const DWORD);

};

extern RecordClient *recordClient;
#endif
