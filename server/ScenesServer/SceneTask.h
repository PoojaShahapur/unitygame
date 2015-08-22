#ifndef _SceneTask_h_
#define _SceneTask_h_

#include "zTCPServer.h"
#include "zTCPTask.h"
#include "zService.h"
#include "zMisc.h"
#include "zEntry.h"
#include "SceneCommand.h"
#include "zSocket.h"
#include "MessageQueue.h"
#include <list>

class SceneUser;
/**
 * \brief ��������������
 *
 */
class SceneTask : public zEntry, public zTCPTask, public MessageQueue
{

  public:

    /**
     * \brief ���캯��
     *
     * \param pool �������ӳ�ָ��
     * \param sock TCP/IP�׽ӿ�
     * \param addr ��ַ
     */
    SceneTask(
        zTCPTaskPool *pool,
        const int sock,
        const struct sockaddr_in *addr = NULL) : zTCPTask(pool,sock,addr)
    {
      wdServerID    = 0;
      wdServerType  = UNKNOWNSERVER;
      recycle_state = 0;
      veriry_ok     = false; 
    }

    /**
     * \brief ����������
     *
     */
    virtual ~SceneTask();

    int verifyConn();
    int waitSync();
    int recycleConn();
    bool uniqueAdd();
    bool uniqueRemove();
    bool msgParse(const Cmd::t_NullCmd *,const DWORD);
    bool cmdMsgParse(const Cmd::t_NullCmd *,const DWORD);

    /**
     * \brief ���ط��������
     *
     * �����һ��������Ψһ�ģ������ڹ����������
     *
     * \return ���������
     */
    const WORD getID() const
    {
      return wdServerID;
    }

    /**
     * \brief ���ط���������
     *
     * \return ����������
     */
    const WORD getType() const
    {
      return wdServerType;
    }
    bool checkRecycle();

  private:

    bool usermsgParse(SceneUser *pUser,const Cmd::t_NullCmd *pNullCmd,const DWORD cmdLen);
    bool usermsgParseBill(SceneUser *pUser,const Cmd::t_NullCmd *pNullCmd,const DWORD cmdLen);
    bool loginmsgParse(const Cmd::t_NullCmd *pNullCmd,DWORD cmdLen);
    WORD wdServerID;
    WORD wdServerType;

    bool verifyLogin(const Cmd::Scene::t_LoginScene *ptCmd);
    int recycle_state;
    bool veriry_ok;
  public: 
    ///////////////////////////////�µ���Ϣ�ַ�����////////////////////////////////////////////
    typedef bool (SceneTask::* SceneTask_Callback)(SceneUser *pUser, const Cmd::t_NullCmd *pNullCmd, const DWORD cmdLen);
    bool psstReqAllCardTujianDataUserCmd(SceneUser *pUser, const Cmd::t_NullCmd *pNullCmd, const DWORD cmdLen);
    ///////////////////////////////end of /////////////////////////////

};
#endif

