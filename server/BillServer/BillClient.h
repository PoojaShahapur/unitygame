#ifndef __BILLCLIENT_H_
#define __BILLCLIENT_H_

#include "BillCallback.h"
#include "zTCPClientTask.h"
#include "MessageQueue.h"

#include "NetType.h"
typedef NetType NetServiceType;

/**
 * \brief 计费客户端连接
 */
class BillClient : public zTCPClientTask, public MessageQueue
{
  public:

    BillClient(const std::string &ip,const WORD port,BillCallback &bc,const DWORD my_id);
    ~BillClient();

    int checkRebound();
    void addToContainer();
    void removeFromContainer();
    bool connect();
    bool action(BillData *bd);
    bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    bool cmdMsgParse(const Cmd::t_NullCmd *,const DWORD);

    const NetType getNetType() const
    {
      return netType;
    }

    const DWORD getID() const
    {
      return my_id;
    }

  private:

    BillCallback &bc;
    NetType netType;
    DWORD my_id;
    GameZone_t gameZone;
    char gameZone_str[6];

};

#endif

