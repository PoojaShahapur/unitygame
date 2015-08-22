#ifndef _RecordTask_h_
#define _RecordTask_h_

#include "zTCPServer.h"
#include "zTCPTask.h"
#include "zService.h"
#include "zMisc.h"
#include "RecordCommand.h"
#include "zDBConnPool.h"
#include "RolechangeCommand.h"

/**
 * \brief 定义读档连接任务类
 *
 */
class RecordTask : public zTCPTask
{

  public:

    /**
     * \brief 构造函数
     * 因为档案数据已经压缩过,在通过底层传送的时候就不需要压缩了
     * \param pool 所属连接池指针
     * \param sock TCP/IP套接口
     * \param addr 地址
     */
    RecordTask(
        zTCPTaskPool *pool,
        const int sock,
        const struct sockaddr_in *addr = NULL) : zTCPTask(pool,sock,addr,false)
    {
      wdServerID = 0;
      wdServerType = UNKNOWNSERVER;
    }

    /**
     * \brief 虚析构函数
     *
     */
    virtual ~RecordTask() {};

    int verifyConn();
    int recycleConn();
    bool msgParse(const Cmd::t_NullCmd *,const DWORD);

    /**
     * \brief 获取服务器编号
     *
     * \return 服务器编号
     */
    const WORD getID() const
    {
      return wdServerID;
    }

    /**
     * \brief 获取服务器类型
     *
     * \return 服务器类型
     */
    const WORD getType() const
    {
      return wdServerType;
    }

  private:

    WORD wdServerID;          /**< 服务器编号,一个区唯一的 */
    WORD wdServerType;          /**< 服务器类型 */

    bool verifyLogin(const Cmd::Record::t_LoginRecord *ptCmd);
    bool msgParse_Gateway(const Cmd::t_NullCmd *,const DWORD);
    bool getSelectInfo(DWORD accid, DWORD countryid);
    bool msgParse_Scene(const Cmd::t_NullCmd *,const DWORD);
    bool msgParse_Session(const Cmd::t_NullCmd*,const DWORD);

    bool readCharBase(const Cmd::Record::t_ReadUser_SceneRecord *rev);
  public:
    static bool saveCharBase(const Cmd::Record::t_WriteUser_SceneRecord *rev);
    static DWORD getZoneState(DWORD accid, DWORD userid, DWORD tozone=0);
    static DWORD getSourceZoneInfo(DWORD accid, DWORD userid, DWORD tozone, DWORD &newuserid, DWORD &offlineTime);
    static void updateZoneState(DWORD accid, DWORD userid, DWORD new_state);
    static bool delCharbase(DWORD accid, DWORD userid, bool log=true);
    static int check_valid(DWORD accid, DWORD source_id=0, DWORD source_zone=0);
    static void return_send_user(DWORD tozone, DWORD accid, DWORD userid, DWORD type, DWORD state, const char* name=NULL);
    static bool newChangeZoneCharbase(Cmd::Rolechange::t_sendUserToZone* rev);

    static const dbCol charbase_define[];

#ifdef _TEST_DATA_LOG
    static const dbCol chartest_define[];
    bool readCharTest(Cmd::Record::t_Read_CharTest_SceneRecord *rev);
    bool insertCharTest(Cmd::Record::t_Insert_CharTest_SceneRecord *rev);
    bool updateCharTest(Cmd::Record::t_Update_CharTest_SceneRecord *rev);
    bool deleteCharTest(Cmd::Record::t_Delete_CharTest_SceneRecord *rev);
#endif
};

#endif

