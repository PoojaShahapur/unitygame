/**
* \brief 服务器连接任务
*/
#ifndef _RoleTask_h_
#define _RoleTask_h_
#include "zTCPTaskPool.h"
#include "RoleregCommand.h"
/**
 * \brief 存放角色信息的结构体
*/
struct RoleData
{
    char name[MAX_NAMESIZE];
    unsigned short game;
    unsigned short zone;
    unsigned int accid;

    RoleData()
    {
	bzero(this, sizeof(*this));
    }
    RoleData(const RoleData& rd)
    {
	accid = rd.accid;
	zone = rd.zone;
	game = rd.game;
	bcopy(rd.name, name, sizeof(name));
    }
    const RoleData &operator= (const RoleData &rd)
    {
	accid = rd.accid;
	zone = rd.zone;
	game = rd.game;
	bcopy(rd.name, name, sizeof(name));
	return *this;
    }
};

class RoleTask : public zTCPTask
{

public:
	/**
	* \brief 构造函数
	* 用于创建一个服务器连接任务
	* \param pool 所属的连接池
	* \param sock TCP/IP套接口
	*/
	RoleTask(
		zTCPTaskPool *pool,
		const int sock, const struct sockaddr_in *addr=NULL) : zTCPTask(pool,sock,NULL)
	{
	}

	/**
	* \brief 虚析构函数
	*/
	~RoleTask() {
	};

	int verifyConn();
	int waitSync();

	const GameZone_t &getZoneID() const
	{
		return gameZone;
	}

private:

	GameZone_t gameZone;
	std::string name;
	
	bool ifExistName(const char *name, const char* tableName);

	bool msgParse(const Cmd::t_NullCmd* ptNullCmd, const unsigned int nCmdLen);
	bool msgParse_regwithId(const Cmd::t_NullCmd *ptNullCmd, const unsigned int nCmdLen);
	bool msgParse_loginServer_withcharId(const Cmd::RoleReg::t_Charname_reg_withID *ptCmd, const unsigned int nCmdLen);
	

};
#endif

