#ifndef __INFOCLIENT_H_
#define __INFOCLIENT_H_


#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "NetType.h"

/**
 * \brief 信息收集客户端连接类
 */
class InfoClient : public zTCPClientTask
{

  public:

    InfoClient(
        const std::string &ip,
        const WORD port);
    ~InfoClient();

    int checkRebound();
    void addToContainer();
    void removeFromContainer();
    bool connect();
    bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

    const DWORD getTempID() const
    {
      return tempid;
    }

    const NetType getNetType() const
    {
      return netType;
    }

  private:
    const DWORD tempid;
    static DWORD tempidAllocator;
    NetType netType;

};



#endif
