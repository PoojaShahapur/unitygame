#ifndef _SessionClient_h_
#define _SessionClient_h_

#include <unistd.h>
#include <iostream>

#include "zTCPClient.h"
#include "SessionCommand.h"
#include "zMisc.h"
#include "zSocket.h"
#include "MessageQueue.h"

class SessionClient : public zTCPBufferClient,public MessageQueue
{

  public:
    /**
    * \brief 构造函数
    * \param  name 名称
    * \param  ip   地址
    * \param  port 端口
    */
    SessionClient(
        const std::string &name,
        const std::string &ip,
        const WORD port)
      : zTCPBufferClient(name,ip,port) {};

    bool connectToSessionServer();
    void run();

    //暂时注释 20140924 void requestFriendDegree(SceneUser *pUser);
    bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    bool cmdMsgParse(const Cmd::t_NullCmd *,const DWORD);
    bool cmdMsgParse_Other(const Cmd::t_NullCmd*,const DWORD);
    bool cmdMsgParse_PKGame(const Cmd::t_NullCmd*, const DWORD);
    bool doGmCmd(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    bool cmdMsgParse_Battle(const Cmd::t_NullCmd*, const DWORD);
#if 0
    bool doAuctionCmd(const Cmd::Session::t_AuctionCmd *,const DWORD);
    bool doCartoonCmd(const Cmd::Session::t_CartoonCmd *,const DWORD);
    bool cmdMsgParse_Country(const Cmd::t_NullCmd*,const DWORD);
    bool cmdMsgParse_Dare(const Cmd::t_NullCmd*,const DWORD);
    bool cmdMsgParse_Recommend(const Cmd::t_NullCmd*,const DWORD);
    bool cmdMsgParse_Other(const Cmd::t_NullCmd*,const DWORD);
    bool cmdMsgParse_Temp(const Cmd::t_NullCmd*,const DWORD);
    bool cmdMsgParse_Army(const Cmd::t_NullCmd* pNullCmd,const DWORD nCmdLen);
    bool cmdMsgParse_Sept(const Cmd::t_NullCmd*,const DWORD);
    bool cmdMsgParse_Union(const Cmd::t_NullCmd*,const DWORD);
    bool cmdMsgParse_Gem(const Cmd::t_NullCmd* pNullCmd,const DWORD nCmdLen);
#endif
};

/// 声明
extern SessionClient *sessionClient;

#endif

