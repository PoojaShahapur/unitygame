#ifndef __BROADCLIENT_H_
#define __BROADCLIENT_H_

#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "NetType.h"



/**
 * \brief 统一用户平台登陆服务器的客户端连接类
 */
class BroadClient : public zTCPClientTask
{

  public:

    BroadClient(const std::string &ip, const WORD port);
    ~BroadClient();

    int checkRebound();
    void addToContainer();
    void removeFromContainer();
    bool connect();
    bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    bool msgParse_Broad(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

    const NetType getNetType() const
    {
      return netType;
    }


  private:

    NetType netType;

};



#endif
