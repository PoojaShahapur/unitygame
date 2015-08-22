#ifndef _zType_h_
#define _zType_h_
#include <time.h>
#include <strings.h>
#include "EncDec/EncDec.h"

#define SAFE_DELETE(x) { if (x) { delete (x); (x) = NULL; } }
#define SAFE_DELETE_VEC(x) { if (x) { delete [] (x); (x) = NULL; } }

typedef unsigned char BYTE;

typedef unsigned short WORD;
/**
 * \brief 双字节符号整数
 *
 */
typedef signed short SWORD;


typedef unsigned int DWORD;
/**
 * \brief 四字节符号整数
 *
 */
typedef long int SDWORD;


/**
 * \brief 八字节无符号整数
 *
 */
typedef unsigned long long QWORD;

/**
 * \brief 八字节符号整数
 *
 */
typedef signed long long SQWORD;


/**
 * \brief 名字的最大长度
 */
#define MAX_NAMESIZE 32

/**
 * \brief 账号最大长度
 */
#define MAX_ACCNAMESIZE  48

/**
 * \brief IP地址最大长度
 *
 */
#define MAX_IP_LENGTH  16

/**
 * \brief 网关最大容纳用户数目
 *
 */
#define MAX_GATEWAYUSER 4000  

/**
 * \brief 密码最大长度
 *
 */
#define MAX_PASSWORD  16

/**
 * \brief 屏宽
 */
#define SCREEN_WIDTH 20

/**
 * \brief 屏高
 */
#define SCREEN_HEIGHT 20

/**
* \brief sky 最大孔数量
*/
#define SOCKET_MAXNUM	6

//[Shx Add] 商店广告
#define MAX_SHOPADV		32

//sky 战场国家的特殊判断ID
#define BATTLEFIELD_SCENE_ID	100

/**
 * \brief 连接线程池的状态标记位
 *
 */
enum{
  state_none    =  0,          /**< 空的状态 */
  state_maintain  =  1            /**< 维护中，暂时不允许建立新的连接 */
};

/**
 * \brief 数字密码
 */
#define MAX_NUMPASSWORD  32

#pragma pack(1)
/**
 * \brief 定义游戏区
 * 对游戏进行分类，然后在同种游戏中再分区
 */
struct GameZone_t
{

  operator int() const
	  {
		return id;
	  }

  union
  {
    /**
     * \brief 唯一编号
     */
    DWORD id;
    struct
    {
      /**
       * \brief 游戏分区编号
       */
      WORD zone;
      /**
       * \brief 游戏种类编号
       */
      WORD game;
    };
  };

  GameZone_t()
  {
    this->game = 0;
    this->zone = 0;
  }
  GameZone_t(const GameZone_t &gameZone)
  {
    this->id = gameZone.id;
  }
  GameZone_t & operator= (const GameZone_t &gameZone)
  {
    this->id = gameZone.id;
    return *this;
  }
  bool operator== (const GameZone_t &gameZone) const
  {
    return this->id == gameZone.id;
  }
};

struct t_NewLoginSession
{
  WORD wdLoginID;
  WORD wdGatewayID;
  DWORD loginTempID;
  char pstrIP[MAX_IP_LENGTH];
  WORD wdPort;
  GameZone_t gameZone;
  union{
    DWORD accid;
    DWORD uid;
  };
  BYTE state;
  //BYTE type;
  DWORD type;
  union{
    char name[48];
    char account[48];
  };
  char passwd[MAX_PASSWORD];
  DES_cblock des_key;//des密钥
  char client_ip[MAX_IP_LENGTH];
  char numpasswd[MAX_NUMPASSWORD];
  char passwd2[MAX_PASSWORD];
  BYTE accSafe;
  WORD wdNetType;
  WORD userType;
  DWORD createTime;
  char mobileAccount[48];
  t_NewLoginSession()
  {
    //wdLoginID = 0;
    //wdGatewayID = 0;
    //loginTempID = 0;
    //bzero(pstrIP,sizeof(pstrIP));
    //wdPort = 0;
    //accid = 0;
    //state = 0;
    //type = 0;
    //bzero(name,sizeof(name));
    //bzero(passwd,sizeof(passwd));
    bzero(numpasswd,sizeof(numpasswd));
    bzero(passwd2, sizeof(passwd2));
  }
  t_NewLoginSession(const t_NewLoginSession& session)
  {
    wdLoginID = session.wdLoginID;
    wdGatewayID = session.wdGatewayID;
    loginTempID = session.loginTempID;
    bcopy(session.pstrIP,pstrIP,sizeof(pstrIP));
    wdPort = session.wdPort;
    gameZone = session.gameZone;
    accid = session.accid;
    state = session.state;
    type = session.type;
    bcopy(session.name,name,sizeof(name));
    bcopy(session.passwd,passwd,sizeof(passwd));
    bcopy(session.des_key,des_key,sizeof(des_key));
    bcopy(session.client_ip,client_ip,sizeof(client_ip));
    bcopy(session.numpasswd,numpasswd,sizeof(numpasswd));
    bcopy(session.passwd2, passwd2, sizeof(passwd2));
    accSafe = session.accSafe;
    userType = session.userType;
    createTime = session.createTime;
    bcopy(session.mobileAccount, mobileAccount, sizeof(mobileAccount));
  }
  t_NewLoginSession & operator= (const t_NewLoginSession &session)
  {
    wdLoginID = session.wdLoginID;
    wdGatewayID = session.wdGatewayID;
    loginTempID = session.loginTempID;
    bcopy(session.pstrIP,pstrIP,sizeof(pstrIP));
    wdPort = session.wdPort;
    gameZone = session.gameZone;
    accid = session.accid;
    state = session.state;
    type = session.type;
    bcopy(session.name,name,sizeof(name));
    bcopy(session.passwd,passwd,sizeof(passwd));
    bcopy(session.des_key,des_key,sizeof(des_key));
    bcopy(session.client_ip,client_ip,sizeof(client_ip));
    bcopy(session.numpasswd,numpasswd,sizeof(numpasswd));
    bcopy(session.passwd2, passwd2, sizeof(passwd2));
    accSafe = session.accSafe;
    userType = session.userType;
    createTime = session.createTime;
    bcopy(session.mobileAccount, mobileAccount, sizeof(mobileAccount));
    return *this;
  }
};

struct ZoneData
{
    ZoneData()
    {
	bzero(this, sizeof(*this));
    }
    WORD id;
    char ip[MAX_IP_LENGTH + 1];
    WORD port;
};

enum taskType
{
    SPAMap = 0,//��Ȫ��ͼ
    end,
};

#pragma pack()
#endif
