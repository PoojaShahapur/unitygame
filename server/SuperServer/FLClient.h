#ifndef __FLCLIENT_H_
#define __FLCLIENT_H_

#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "NetType.h"



/**
 * \brief 统一用户平台登陆服务器的客户端连接类
 */
class FLClient : public zTCPClientTask
{

  public:

    FLClient(const std::string &ip, const WORD port);
    ~FLClient();

    int checkRebound();
    void addToContainer();
    void removeFromContainer();
    bool connect();
    bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

    /**
     * \brief 获取临时编号
     * \return 临时编号
     */
    const WORD getTempID() const
    {
      return tempid;
    }

    const NetType getNetType() const
    {
      return netType;
    }


  private:

    /**
     * \brief 临时编号
     *
     */
    const WORD tempid;
    static WORD tempidAllocator;
    NetType netType;

    bool msgParse_gyList(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    bool msgParse_session(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

};



#endif
