#ifndef _SESSION_TASK_H_
#define _SESSION_TASK_H_

#include "zTCPServer.h"
#include "zTCPTask.h"
#include "zService.h"
#include "zMisc.h"
#include "zEntry.h"
#include "MessageQueue.h"
#include "SessionCommand.h"

class UserSession;
/**
 * \brief 服务器连接任务
 *
 */
class SessionTask : public zTCPTask, public zEntry,public MessageQueue
{

  public:

    /**
     * \brief 构造函数
     *
     * \param pool 所属连接池指针
     * \param sock TCP/IP套接口
     * \param addr 地址
     */
    SessionTask(
        zTCPTaskPool *pool,
        const int sock,
        const struct sockaddr_in *addr = NULL) : zTCPTask(pool,sock,addr)
    {
      wdServerID = 0;
      wdServerType = UNKNOWNSERVER;
      recycle_state = 0;
    }

    /**
     * \brief 虚析构函数
     *
     */
    virtual ~SessionTask();

    int verifyConn();
    int recycleConn();
    void addToContainer();
    void removeFromContainer();
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

    /**
     * \brief 服务器编号
     *
     */
    WORD wdServerID;

    /**
     * \brief 服务器类型
     *
     */
    WORD wdServerType;
    ///设置回收标志
    int recycle_state;

    bool verifyLogin(const Cmd::Session::t_LoginSession *ptCmd);

    /**
     * \brief 删除角色的处理
     *
     * 从角色相应的社会关系中把它删除，如果角色是某一社会关系的建立者则不允许删除该角色
     * 如果允许删除，则通知GATEWAY让其从档案中删除。如果不允许删除，则发送一条通知命令给客户端。
     *
     *
     */
    bool del_role(const Cmd::t_NullCmd* cmd,const DWORD cmdLen);
    /**
     * \brief 更换国籍
     *
     * \param dwUserID : 更换国籍的用户ID
     *
     */
    bool change_country(const Cmd::Session::t_changeCountry_SceneSession* cmd);
    bool msgParse_Scene(const Cmd::t_NullCmd *cmd,const DWORD cmdLen);
    bool msgParse_Gate(const Cmd::t_NullCmd *cmd,const DWORD cmdLen);
    bool msgParse_Forward(const Cmd::t_NullCmd *pNullCmd,const DWORD cmdLen);

	//sky 竞技战场副本类Session消息处理函数
	bool msgParse_Arena(const Cmd::t_NullCmd *cmd, const DWORD cmdLen);

};
#endif
