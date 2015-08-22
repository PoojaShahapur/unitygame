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
 * \brief 服务器连接任务
 *
 */
class SceneTask : public zEntry, public zTCPTask, public MessageQueue
{

  public:

    /**
     * \brief 构造函数
     *
     * \param pool 所属连接池指针
     * \param sock TCP/IP套接口
     * \param addr 地址
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
     * \brief 虚析构函数
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
     * \brief 返回服务器编号
     *
     * 编号在一个区中是唯一的，保存在管理服务器中
     *
     * \return 服务器编号
     */
    const WORD getID() const
    {
      return wdServerID;
    }

    /**
     * \brief 返回服务器类型
     *
     * \return 服务器类型
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
    ///////////////////////////////新的消息分发机制////////////////////////////////////////////
    typedef bool (SceneTask::* SceneTask_Callback)(SceneUser *pUser, const Cmd::t_NullCmd *pNullCmd, const DWORD cmdLen);
    bool psstReqAllCardTujianDataUserCmd(SceneUser *pUser, const Cmd::t_NullCmd *pNullCmd, const DWORD cmdLen);
    ///////////////////////////////end of /////////////////////////////

};
#endif

