#ifndef __FLCLIENT_H_
#define __FLCLIENT_H_

#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "NetType.h"



/**
 * \brief ͳһ�û�ƽ̨��½�������Ŀͻ���������
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
     * \brief ��ȡ��ʱ���
     * \return ��ʱ���
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
     * \brief ��ʱ���
     *
     */
    const WORD tempid;
    static WORD tempidAllocator;
    NetType netType;

    bool msgParse_gyList(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    bool msgParse_session(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

};



#endif
