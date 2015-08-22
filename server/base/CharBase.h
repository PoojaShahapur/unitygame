
/**
 * \brief 定义人物角色相关信息
 */
#ifndef _CharBase_h_
#define _CharBase_h_

#include "zType.h"
#include "Object.h"
//#include "ModelIndex.h"

#pragma pack(1)

#define HAIRTYPE_MASK  0xff000000    /// 头发发型
#define HAIRRGB_MASK  0x00ffffff    /// 头发颜色
const int exploit_arg = 100;      // 功勋值同比放大系数

//---------------------------------
#define LEVELUP_HP_N      15
#define LEVELUP_MP_N      1
#define LEVELUP_SP_N      5

#define LEVELUP_RESUMEHP_N    0
#define  LEVELUP_RESUMEMP_N    0
#define LEVELUP_RESUMESP_N    0

#define LEVELUP_ATTACKRATING_N  1
#define LEVELUP_ATTACKDODGE_N  1

#define LEVELUP_MOVESPEED_N    0
#define LEVELUP_ATTACKSPEED_N  0

#define LEVELUP_PDAMAGE_N    0
#define LEVELUP_MDAMAGE_N    0
#define LEVELUP_PDEFENCE_N    0
#define LEVELUP_MDEFENCE_N    0

#define LEVELUP_BANG_N    0
//----------------------------------
#define BASEDATA_M_HP      500
#define BASEDATA_M_MP      60
#define BASEDATA_M_SP      100

#define BASEDATA_M_RESUMEHP    1
#define  BASEDATA_M_RESUMEMP    1
#define BASEDATA_M_RESUMESP    1

#define BASEDATA_M_ATTACKRATING  1
#define BASEDATA_M_ATTACKDODGE  1

#define BASEDATA_M_MOVESPEED  0
#define BASEDATA_M_ATTACKSPEED  0

#define BASEDATA_M_PDAMAGE    11
#define BASEDATA_M_MDAMAGE    11
#define BASEDATA_M_PDEFENCE    1
#define BASEDATA_M_MDEFENCE    1

#define BASEDATA_M_BANG    1
//----------------------------------
#define BASEDATA_F_HP      500
#define BASEDATA_F_MP      60
#define BASEDATA_F_SP      90

#define BASEDATA_F_RESUMEHP    1
#define  BASEDATA_F_RESUMEMP    1
#define BASEDATA_F_RESUMESP    1

#define BASEDATA_F_ATTACKRATING  1
#define BASEDATA_F_ATTACKDODGE  1

#define BASEDATA_F_MOVESPEED  0
#define BASEDATA_F_ATTACKSPEED  0

#define BASEDATA_F_PDAMAGE    11
#define BASEDATA_F_MDAMAGE    11
#define BASEDATA_F_PDEFENCE    1
#define BASEDATA_F_MDEFENCE    1

#define BASEDATA_F_BANG    1
//----------------------------------

struct CharBase
{
  DWORD accid;            /// 账号
  DWORD id;              /// 角色编号
  char  name[MAX_NAMESIZE + 1];    /// 角色名称
  WORD  type;              /// 性别
  WORD  level;            /// 角色等级
  WORD  face;              /// 头像
  DWORD hair;              /// 头发，发型和颜色
  DWORD bodyColor;              /// 光身颜色
  DWORD goodness;            /// 善恶度
  DWORD mapid;            /// 角色所在地图编号
  char  mapName[MAX_NAMESIZE + 1];  /// 角色所在地图名称
  DWORD x;              /// 角色所在坐标x
  DWORD y;              /// 角色所在坐标y
  DWORD unionid;            /// 帮会ID
  DWORD schoolid;            /// 门派ID
  DWORD septid;            /// 家族ID
  DWORD hp;              /// 当前生命值
  DWORD mp;              /// 当前法术值
  DWORD sp;              /// 当前体力值
  QWORD exp;              /// 当前经验值
  WORD  skillpoint;          /// 技能点数
  WORD  points;            /// 未分配人物属性点数
  DWORD  country;            /// 国家
  DWORD  consort;              /// 配偶
 
  QWORD forbidtalk;          ///禁言
  DWORD bitmask;            /// 标志掩码
  DWORD onlinetime;          /// 在线时间统计
  union {
    struct {
      WORD wdCon;  //体质
      WORD wdStr;  //体力
      WORD wdDex;  //敏捷
      WORD wdInt;  //智力
      WORD wdMen;  //精神
    };
    WORD wdProperty[5];
  };
  WORD reliveWeakTime;      /// 复活虚弱剩余时间
  DWORD useJob;        // sky 角色职业(数据库位置使用原文采"GRACE"段)
  DWORD exploit;        /// 功勋值
  char tiretime[36+1];      /// 疲劳时间标记
  DWORD offlinetime;        /// 上次下线时间
  DWORD fivetype;          /// 五行类型
  DWORD fivelevel;        /// 五行点数
  DWORD pkaddition;        /// 善恶追加值
  DWORD money;        /// 当前银子,只存储维护,不能使用
  DWORD answerCount;      /// 当天答题次数
  DWORD honor;        //荣誉值
  DWORD maxhonor;        //最大荣誉值
  DWORD gomaptype;      //跳地图类型
  DWORD msgTime;        //下次发送GM留言的时间
  DWORD accPriv;      //帐号权限
  DWORD gold;          /// 金币
  DWORD ticket;         //点券数
  DWORD createtime;      /// 角色创建时间
  DWORD goldgive;        /// 金币冲值赠品数量
  WORD petPack;        /// 宠物包裹大小
  DWORD petPoint;        /// 奖励的宠物修炼时间
  DWORD levelsept;      /// 离开家族的时间
  DWORD punishTime;      /// 杀人被抓的时间，分钟为单位
  DWORD trainTime;      /// 在练级地图逗留的时间
  DWORD  zs;                   ///转生次数
  DWORD doubletime;          ///双倍物品剩余时间
  DWORD target_zone;	//Ŀ����ID
  DWORD source_zone;	//Դ��ID
  DWORD zone_state;	//ת��״̬,�μ�ö��
  DWORD source_id;	//Դ����charid
};

enum
{
    CHANGEZONE_NONE,		//δת
    CHANGEZONE_CHANGED,		//��ת
    CHANGEZONE_TARGETZONE,	//ս���ɵ�½
    CHANGEZONE_RETURNED,	//�ѻش�
    CHANGEZONE_FOREVER,		//����ת��(�����״̬��,����ֶν������ٱ��޸�)
};

struct CharState
{
  DWORD maxhp;            /// 最大生命值 *
  DWORD resumehp;            /// 生命值恢复 *
  DWORD maxmp;            /// 最大法术值 *
  DWORD resumemp;            /// 法术值恢复 *
  DWORD maxsp;                     /// 最大体力值 *
  DWORD resumesp;                  /// 体力恢复值 *

  DWORD pdamage;                   /// 最小物理攻击力 *
  DWORD maxpdamage;          /// 最大物理攻击力 *
  DWORD mdamage;            /// 最小法术攻击力 *
  DWORD maxmdamage;          /// 最大法术攻击力 *
  DWORD pdefence;            /// 物理防御力 *
  DWORD mdefence;            /// 法术防御力 *
  QWORD nextexp;            /// 升级经验值 *
  WORD  attackspeed;          /// 攻击速度 *
  WORD  movespeed;          /// 移动速度 *
  SWORD  attackrating;        /// 攻击命中 *
  SWORD  attackdodge;          /// 攻击躲避 *
  WORD  bang;              /// 重击 *
  WORD  charm;                     /// 魅力值 *
  BYTE  attackfive;          /// 攻击五行 *
  BYTE  defencefive;          /// 防御五行 *
  union {
    struct {
      WORD wdCon;  //体质
      WORD wdStr;  //体力
      WORD wdDex;  //敏捷
      WORD wdInt;  //智力
      WORD wdMen;  //精神
    };
    WORD wdProperty[5];
  };

  DWORD stdpdamage;          /// 标准物理攻击力
  DWORD stdmdamage;          /// 标准法术攻击力
  DWORD stdpdefence;          /// 标准物理防御力
  DWORD stdmdefence;          /// 标准法术防御力
  WORD  stdbang;            /// 标准重击率
};

#ifdef _TEST_DATA_LOG
struct CharTest
{
  DWORD upgrade_time;//升级时间
  DWORD upgrade_usetime;//本级使用时间
  DWORD death_times;//死亡次数
  DWORD hp_leechdom;//使用生命值药品个数
  DWORD mp_leechdom;//使用法术值药品个数
  DWORD sp_leechdom;//使用体力值药品个数
  DWORD get_money;//得到银子数
  DWORD get_heigh;//得到高级装备个数
  DWORD get_socket;//得到带孔装备个数
  DWORD get_material;//得到原料个数
  DWORD get_stone;//得到宝石个数
  DWORD get_scroll;//得到卷轴个数
  DWORD money;//当前银子数
};
#endif

struct CharSave
{
  CharBase   charbase;
  DWORD    dataSize;
  char     data[0];
};

#define MAX_TEMPARCHIVE_SIZE 0x10000
struct TempArchiveMember
{
  DWORD type;
  DWORD size;
  char data[0];
};

enum TempArchiveType
{
  TEAM,//队伍
  ENTRY_STATE,//队伍
  PET,//宠物
  SAFETY_STATE,// 临时关闭密码保护
  ITEM_COOLTIME,// sky 物品冷却时间
};

struct BinaryArchiveMember
{
  DWORD type;
  DWORD size;
  unsigned char data[0];
};

enum BinaryArchiveType
{
#if 0
  BINARY_DOUBLE_EXP_OBJ,///双倍经验道具
  BINARY_TONG_OBJ,///帮会令牌类道具
  BINARY_KING_OBJ,///国王令牌类道具
  BINARY_FAMILY_OBJ,///家族令牌类道具
  BINARY_CHANGE_COUNTRY_TIME,/// 最后一次叛国的时间
  BINARY_SAFETY,/// 是否进行财产保护
  BINARY_GIVE_MATARIAL_NUM,//道具卡兑换材料的组数
  BINARY_CARD_NUM,     //道具卡张数
  BINARY_SAFETY_SETUP,/// 保护设置
#endif
  BINARY_ZOBJECT	= 1,	//OBJECT DATA
  BINARY_COUNTER	= 2,	//COUNTER MANAGER
  BINARY_CARD_TUJIAN	= 3,	//CARD ͼ��
  BINARY_CARD_GROUP	= 4,	//������Ϣ 
  BINARY_HERO_DATA	= 5,	//����Ӣ������
  BINARY_TMP_CHALLENGE	= 6,	//��¼��ս����ʱ����
  BINARY_QUEST		= 7,	//����

  BINARY_MAX	= 255, ///最大数值(占位用)
};

enum CounterType
{

};

enum ChangeType
{
    CHANGE_NONE	    = 0,
    CHANGE_DAY	    = 1,
    CHANGE_WEEK	    = 2,
    CHANGE_MONTH    = 3,

};

#define COUNTER_VERSION 1

struct CounterMember
{
    BYTE ver;
    DWORD id;
    DWORD type;
    DWORD last_time;
    QWORD count;
    CounterMember()
    {
	ver = COUNTER_VERSION;
	id = 0;
	type = 0;
	last_time = 0;
	count = 0;
    }
};
const WORD  MAX_LEVEL      = 150;          /// 最大角色等级
const DWORD MAX_GOODNESS    = 9999;          /// 最大善恶度
const DWORD MAX_HP      = 999999;        /// 最大生命值
const DWORD MAX_RESUMEHP    = 1000;          /// 生命值恢复
const DWORD MAX_MP      = 999999;        /// 最大法术值
const DWORD MAX_RESUMEMP    = 1000;          /// 法术值恢复
const DWORD MAX_SP      = 999999;        /// 最大体力值
const DWORD MAX_RESUMESP    = 1000;          /// 体力值恢复
const DWORD MAX_PDAMAGE      = 999999;        /// 最大物理攻击力
const DWORD MAX_RDAMAGE      = 999999;        /// 最大远程攻击力
const DWORD MAX_TDAMAGE     = 999999;        /// 最大仙术攻击力
const DWORD MAX_MDAMAGE      = 999999;        /// 最大法术攻击力
const DWORD MAX_PDEFENCE    = 999999;        /// 最大物理防御力
const DWORD MAX_MDEFENCE    = 999999;        /// 最大法术防御力
#ifdef _MSC_VER
const QWORD MAX_EXP      = 9999999999i64;
#else //!_MSC_VER
const QWORD MAX_EXP      = 9999999999LL;      /// 最大经验值
#endif //_MSC_VER
const WORD  MAX_ATTACKSPEED    = 100;          /// 攻击速度
const WORD  MAX_MAGICSPEED    = 100;          /// 施法速度
const WORD  MAX_MOVESPEED    = 200;          /// 移动速度
const WORD  MAX_ATTACKRATING    = 999;          /// 攻击命中
const WORD  MAX_ATTACKDODGE    = 999;          /// 攻击躲避
const WORD  MAX_MAGICRATING    = 999;          /// 法术命中
const WORD  MAX_MAGICDODGE    = 999;          /// 法术躲避
const WORD  MAX_LUCKY      = 999;          /// 幸运值
const WORD  MAX_METAL      = 999;          /// 金
const WORD  MAX_WOOD      = 999;          /// 木
const WORD  MAX_WATER      = 999;          /// 水
const WORD  MAX_FIRE      = 999;          /// 火
const WORD  MAX_SOIL      = 999;          /// 土
const WORD  MAX_METALRESISTANCE    = 10000;        /// 金系抵抗
const WORD  MAX_WOODRESISTANCE    = 10000;        /// 木系抵抗
const WORD  MAX_WATERRESISTANCE    = 10000;        /// 水系抵抗
const WORD  MAX_FIRERESISTANCE    = 10000;        /// 火系抵抗
const WORD  MAX_SOILRESISTANCE    = 10000;        /// 土系抵抗
const WORD  MAX_POINTS      = 1500;          /// 点数

const DWORD PUBLIC_COUNTRY = 6;

#pragma pack()

#endif

