/*************************************************************************
 Author: wang
 Created Time: 2014年10月20日 星期一 19时43分35秒
 File Name: base/zDatabase.h
    特别注意:当前代码中的 struct xxxxBase 中的任何字段大小请以4的倍数定义
 Description: 
 ************************************************************************/
#ifndef _zDatabase_h_
#define _zDatabase_h_

#include "zEntry.h"
#include "zMisc.h"

struct ObjectBase
{
    const DWORD getUniqueID() const
    {
	return id;
    }
    union
    {
	DWORD dwField0;	
	DWORD id;
    };
    char name[64];  
    DWORD maxnum;
    DWORD kind;
    DWORD color;
};

struct zObjectB : public zEntry
{
    DWORD maxnum;
    DWORD kind;	    //类型
    typedef std::vector<DWORD> JobVector;
    JobVector jobs;
    BYTE sex;
    WORD level;
    std::vector<DWORD> blus;
    std::vector<DWORD> golds;
    std::vector<DWORD> greens;
    std::vector<DWORD> suits;
    std::vector<DWORD> needobject;

    struct leechdom_t
    {
	BYTE id;
	WORD effect;
	WORD time;
    }leechdom;
    WORD needlevel;
    WORD maxhp;
    WORD maxmp;
    WORD maxsp;


    WORD pdamage;				// 最小攻击力      // 对应马匹 物攻加成
    WORD maxpdamage;			// 最大攻击力
    WORD mdamage;				// 最小法术攻击力  // 对应马匹 魔攻加成
    WORD maxmdamage;			// 最大法术攻击力

    WORD pdefence;				// 物防            // 对应马匹 物防加成
    WORD mdefence;				// 魔防            // 对应马匹 魔防加成
    BYTE damagebonus;			// 伤害加成x% from 道具基本表

    WORD akspeed;				// 攻击速度
    WORD mvspeed;				// 移动速度
    WORD atrating;				// 命中率
    WORD akdodge;				// 躲避率
    
    DWORD color;
    struct socket
    {
	WORD odds;
	BYTE min;
	BYTE max;
    }hole;

    BYTE recast;
    BYTE recastlevel;
    WORD recastcost;

    WORD make;
    struct skills
    {
	DWORD id;
	DWORD level;
    };
    skills need_skill;

    struct material
    {
	DWORD gold;
	struct stuff
	{
	    DWORD id;
	    DWORD number;
	    DWORD level;
	};
	std::vector<std::vector<stuff> > stuffs;

    };
    material need_material;

    BYTE setpos;
    DWORD durability;
    DWORD price;
    DWORD crystalPrice;
    DWORD bluerating;
    DWORD goldrating;
    BYTE width;
    BYTE timeeffect;

    union
    {
	DWORD cardpoint;
	DWORD cointtype;
    };
    WORD bang;
    DWORD holyrating;
    DWORD coolSeconds;
    QWORD modelid;
    DWORD impression;
    DWORD effect;
    DWORD dropSendInfo;
    char classify[32];
    WORD useTimesLimitPerDay;
    void fill(ObjectBase &data);
};

//------------------------------------
// NpcBase
//------------------------------------
struct NpcBase
{
    const DWORD getUniqueID() const
    {
	return dwField0;
    }
    DWORD  dwField0;    // 编号
    char  strField1[64];    // 名称
    DWORD  dwField2;    // 类型
    DWORD  dwField3;    // 等级
    DWORD  dwField4;    // distance
    DWORD  dwField5;    // adistance
    DWORD  dwField6;    // hp
#if 0
    DWORD  dwField5;    // 经验值

    DWORD  dwField6;    // 力
    DWORD  dwField7;    // 智
    DWORD  dwField8;    // 敏捷
    DWORD  dwField9;    // 精神
    DWORD  dwField10;    // 体质
    DWORD  dwField11;    // 体质

    DWORD  dwField12;    // 颜色
    DWORD  dwField13;    // ai
    DWORD  dwField14;    // 移动间隔
    DWORD  dwField15;    // 攻击间隔
    DWORD  dwField16;    // 最小物理防御力
    DWORD  dwField17;    // 最大物理防御力
    DWORD  dwField18;    // 最小法术防御力
    DWORD  dwField19;    // 最大法术防御力
    DWORD  dwField20;    // 五行属性
    DWORD  dwField21;    // 五行点数
    char  strField22[1024];    // 攻击类型
    DWORD  dwField23;    // 最小法术攻击
    DWORD  dwField24;    // 最大法术攻击
    DWORD  dwField25;    // 最小攻击力
    DWORD  dwField26;    // 最大攻击力
    DWORD  dwField27;    // 技能
    char  strField28[4096];    // 携带物品
    DWORD  dwField29;    // 魂魄之石几率
    char  strField30[1024];    // 使用技能
    char  strField31[1024];    // 状态
    DWORD  dwField32;    // 躲避率
    DWORD  dwField33;    // 命中率
    DWORD  dwField34;    // 图片
    DWORD  dwField35;    // 品质
    DWORD  dwField36;    // 怪物类别
    DWORD  dwField37;    // 纸娃娃图片
    char  strField38[64];    // 回血
    DWORD  dwField39;    // 二进制标志
    DWORD  dwField40;    // 二进制标志
    DWORD  dwField41;    // sky 极品倍率
#endif
};

struct CarryObject
{
    DWORD id;
    int   rate;
    WORD colorPara;
    int   minnum;
    int   maxnum;
    CarryObject()
    {
	id = 0;
	rate = 0;
	minnum = 0;
	maxnum = 0;
	colorPara = 0;
    }
};

typedef std::vector<CarryObject> NpcLostObject;

struct NpcCarryObject : private zNoncopyable
{
    NpcCarryObject() {};
    bool set(const char *objects)
    {
	bool retval = true;
	//mlock.lock();
	cov.clear();
	if (strcmp(objects,"0"))
	{
	    std::vector<std::string> obs;
	    Zebra::stringtok(obs,objects,";");
	    for(std::vector<std::string>::const_iterator it = obs.begin(); it != obs.end(); it++)
	    {
		std::vector<std::string> rt;
		Zebra::stringtok(rt,*it,":");
		if (3 == rt.size())
		{
		    CarryObject co;
		    co.id = atoi(rt[0].c_str());
		    co.rate = atoi(rt[1].c_str());
		    std::vector<std::string> nu;
		    Zebra::stringtok(nu,rt[2],"-");
		    if (2 == nu.size())
		    {
			co.minnum = atoi(nu[0].c_str());
			co.maxnum = atoi(nu[1].c_str());
			cov.push_back(co);
		    }
		    else
			retval = false;
		}
		else
		    retval = false;
	    }
	}
	//mlock.unlock();
	return retval;
    }

    /**
     * \brief 物品掉落处理
     * \param nlo npc携带物品集合
     * \param value 掉落率打折比
     * \param value1 掉落率增加
     * \param value2 银子掉落率增加
     */
    void lost(NpcLostObject &nlo,int value,int value1,int value2,int vcharm,int vlucky,int player_level,int DropRate,int DropRateLevel)
    {
	//mlock.lock();
	if (vcharm>1000) vcharm=1000;
	if (vlucky>1000) vlucky=1000;
	for(std::vector<CarryObject>::const_iterator it = cov.begin(); it != cov.end(); it++)
	{
	    //Zebra::logger->debug("%u,%u,%u,%u",(*it).id,(*it).rate,(*it).minnum,(*it).maxnum);
	    switch((*it).id)
	    {
		case 665:
		    {
			int vrate = (int)(((*it).rate/value)*(1+value1/100.0f)*(1+value2/100.0f)*(1+vcharm/1000.0f)*(1+vlucky/1000.0f));
			if (zMisc::selectByTenTh(vrate))
			{
			    nlo.push_back(*it);
			}
		    }
		    break;
		default:
		    {
			int vrate = (int)(((*it).rate/value)*(1+value1/100.0f)*(1+vcharm/1000.0f)*(1+vlucky/1000.0f));
			if (player_level<= DropRateLevel)
			{
			    if (zMisc::selectByTenTh(vrate * DropRate))
			    {
				nlo.push_back(*it);
			    }
			}
			else
			{
			    if (zMisc::selectByTenTh(vrate))
			    {
				nlo.push_back(*it);
			    }
			}
		    }
		    break;
	    }
	}
	//mlock.unlock();
    }
    /**
     * \brief 全部物品掉落处理
     * \param nlo npc携带物品集合
     * \param value 掉落率打折比
     * \param value1 掉落率增加
     * \param value2 银子掉落率增加
     */
    void lostAll(NpcLostObject &nlo)
    {
	for(std::vector<CarryObject>::const_iterator it = cov.begin(); it != cov.end(); it++)
	{
	    nlo.push_back(*it);
	}
    }

    /**
     * \brief 装备物品全部掉落处理(绿怪专用)
     * \param nlo npc携带物品集合
     * \param value 掉落率打折比
     * \param value1 掉落率增加
     * \param value2 银子掉落率增加
     */
    void lostGreen(NpcLostObject &nlo,int value=1,int value1=0,int value2=0,int vcharm = 0,int vlucky = 0);
    private:
    std::vector<CarryObject> cov;
    //zMutex mlock;
};


    struct aTypeS{
	aTypeS()
	{
	    byValue[0] = 0;
	    byValue[1] = 0;
	}
	union {
	    struct {
		BYTE byAType;
		BYTE byAction;
	    };
	    BYTE byValue[2];
	};
    };

enum
{
    NPC_TYPE_HUMAN    = 0,///人型
    NPC_TYPE_NORMAL    = 1,/// 普通类型
    NPC_TYPE_BBOSS    = 2,/// 大Boss类型
    NPC_TYPE_LBOSS    = 3,/// 小Boss类型
    NPC_TYPE_BACKBONE  = 4,/// 精英类型
    NPC_TYPE_GOLD    = 5,/// 黄金类型
    NPC_TYPE_TRADE    = 6,/// 买卖类型
    NPC_TYPE_TASK    = 7,/// 任务类型
    NPC_TYPE_GUARD    = 8,/// 士兵类型
    NPC_TYPE_PET    = 9,/// 宠物类型
    NPC_TYPE_BACKBONEBUG= 10,/// 精怪类型
    NPC_TYPE_SUMMONS  = 11,/// 召唤类型
    NPC_TYPE_TOTEM    = 12,/// 图腾类型
    NPC_TYPE_AGGRANDIZEMENT = 13,/// 强化类型
    NPC_TYPE_ABERRANCE  = 14,/// 变异类型
    NPC_TYPE_STORAGE  = 15,/// 仓库类型
    NPC_TYPE_ROADSIGN  = 16,/// 路标类型
    NPC_TYPE_TREASURE  = 17,/// 宝箱类型
    NPC_TYPE_WILDHORSE  = 18,/// 野马类型
    NPC_TYPE_MOBILETRADE  = 19,/// 流浪小贩
    NPC_TYPE_LIVENPC  = 20,/// 生活npc（不战斗，攻城时消失）
    NPC_TYPE_DUCKHIT  = 21,/// 蹲下才能打的npc
    NPC_TYPE_BANNER    = 22,/// 旗帜类型
    NPC_TYPE_TRAP    = 23,/// 陷阱类型
    NPC_TYPE_MAILBOX  =24,///邮箱
    NPC_TYPE_AUCTION  =25,///拍卖管理员
    NPC_TYPE_UNIONGUARD  =26,///帮会守卫
    NPC_TYPE_SOLDIER  =27,///士兵，只攻击外国人
    NPC_TYPE_UNIONATTACKER  =28,///攻方士兵
    NPC_TYPE_SURFACE = 29,/// 地表类型
    NPC_TYPE_CARTOONPET = 30,/// 替身宝宝
    NPC_TYPE_PBOSS = 31,/// 紫色BOSS
    NPC_TYPE_RESOURCE = 32, /// 资源类NPC

    //sky添加
    NPC_TYPE_GHOST	= 999,  /// 元神类NPC
    NPC_TYPE_ANIMON   = 33,   /// 动物类怪物
    NPC_TYPE_GOTO	= 34,	///传送点
    NPC_TYPE_RESUR  = 35,	///复活点
    NPC_TYPE_UNFIGHTPET	= 36, ///非战斗宠物
    NPC_TYPE_FIGHTPET	= 37, ///战斗宠物
    NPC_TYPE_RIDE		= 38, ///坐骑
    NPC_TYPE_TURRET	= 39, /// 炮塔
    NPC_TYPE_BARRACKS = 40, /// 兵营
    NPC_TYPE_CAMP = 41,		/// 基地
};

enum
{
    NPC_ATYPE_NEAR    = 1,/// 近距离攻击
    NPC_ATYPE_FAR    = 2,/// 远距离攻击
    NPC_ATYPE_MFAR    = 3,/// 法术远程攻击
    NPC_ATYPE_MNEAR    = 4,/// 法术近身攻击
    NPC_ATYPE_NOACTION  = 5,    /// 无攻击动作
    NPC_ATYPE_ANIMAL    = 6  /// 动物类
};

///npc使用一个技能的描述
struct npcSkill
{
    DWORD id;///技能id
    int needLevel;///技能id
    int rate;///使用几率
    int coefficient;///升级系数

    npcSkill():id(0),needLevel(0),rate(0),coefficient(0){}
    npcSkill(const npcSkill &skill)
    {
	id = skill.id;
	needLevel = skill.needLevel;
	rate = skill.rate;
	coefficient = skill.coefficient;
    }
    npcSkill& operator = (const npcSkill &skill)
    {
	id = skill.id;
	needLevel = skill.needLevel;
	rate = skill.rate;
	coefficient = skill.coefficient;
	return *this;
    }
};

struct npcRecover
{
    DWORD start;
    BYTE type;
    DWORD num;

    npcRecover()
    {
	start = 0;
	type = 0;
	num = 0;
    }

    void parse(const char * str)
    {
	if (!str) return;

	std::vector<std::string> vec;

	vec.clear();
	Zebra::stringtok(vec,str,":");
	if (3==vec.size())
	{
	    start = atoi(vec[0].c_str());
	    type = atoi(vec[1].c_str());
	    num = atoi(vec[2].c_str());
	}
    }
};

/**
 * \brief Npc基本表格数据
 *
 */
struct zNpcB : public zEntry
{
    DWORD  kind;        // 类型
    DWORD  level;        // 等级
    DWORD  hp;          // 生命值
    DWORD  exp;        // 经验值
    DWORD  str;        // 力量
    DWORD   inte;        // 智力
    DWORD   dex;        // 敏捷
    DWORD   men;        // 精神
    DWORD   con;        // 体质
    DWORD   cri;        // 暴击
    DWORD  color;        // 颜色
    DWORD  ai;          // ai
    DWORD  distance;      // 移动间隔
    DWORD  adistance;      // 攻击间隔
    DWORD  pdefence;      // 最小物理防御力
    DWORD  maxpdefence;    // 最大物理防御力
    DWORD  mdefence;      // 最小法术防御力
    DWORD  maxmdefence;    // 最大法术防御力
    DWORD  five;        // 五行属性
    DWORD   fivepoint;      // 五行点数
    std::vector<aTypeS> atypelist;  // 攻击类型
    DWORD  mdamage;      // 最小法术攻击
    DWORD  maxmdamage;      // 最大法术攻击
    DWORD  damage;        // 最小攻击力
    DWORD  maxdamage;      // 最大攻击力
    DWORD  skill;        // 技能
    //char  object[1024 + 1];  // 携带物品
    NpcCarryObject nco;
    DWORD  ChangeNpcID;     //soulrate;      //sky NPC变身ID
    char  skills[1024];    // 使用技能
    char  state[1024];    // 状态
    DWORD  dodge;        // 躲避率
    DWORD  rating;        // 命中率
    DWORD  pic;        // 图片
    DWORD  trait;        //品质
    DWORD  bear_type;      //怪物类别
    DWORD  pet_pic;      //宠物图片
    npcRecover recover;
    DWORD  flags;      //二进制标志，目前有一个，可不可被外国人杀
    DWORD  allyVisit;      //可被盟国访问的等级 0：不可访问 1：1级可访问 2：2级可访问
    DWORD radix;

    DWORD kokGroupID;
    DWORD kokAttGroupID;
    DWORD kok3DZoom;
    DWORD kok3DInterval;
    DWORD lingqiexp;
    DWORD lingqiPK;
    char tips[32];
    DWORD hurtType;
    DWORD hurtValue;
    DWORD tiredType;
    DWORD delayClearBodyTime;
    DWORD elementHurt;
    DWORD allHitCount;
    DWORD skillPKType;
    std::map<int,std::vector<npcSkill> > skillMap;

    //DWORD  Need_Probability; //sky 极品概率

    bool parseSkills(const char * str)
    {
	skillMap.clear();
	strncpy(skills,str,sizeof(skills));

	bool ret = false;
	std::vector<std::string> type_v;
	Zebra::stringtok(type_v,str,";");
	if (type_v.size()>0)
	{
	    std::vector<std::string> type_sub_v,skill_v,prop_v;
	    std::vector<std::string>::iterator type_it,skill_it;

	    for (type_it=type_v.begin();type_it!=type_v.end();type_it++)
	    {
		type_sub_v.clear();
		Zebra::stringtok(type_sub_v,type_it->c_str(),":");
		if (2==type_sub_v.size())
		{
		    int type = atoi(type_sub_v[0].c_str());

		    std::vector<npcSkill> oneTypeSkills;
		    skill_v.clear();
		    Zebra::stringtok(skill_v,type_sub_v[1].c_str(),",");
		    for (skill_it=skill_v.begin();skill_it!=skill_v.end();skill_it++)
		    {
			prop_v.clear();
			Zebra::stringtok(prop_v,skill_it->c_str(),"-");
			if (4==prop_v.size())
			{
			    npcSkill oneSkill;
			    oneSkill.id = atoi(prop_v[0].c_str());
			    oneSkill.needLevel = atoi(prop_v[1].c_str());
			    oneSkill.rate = atoi(prop_v[2].c_str());
			    oneSkill.coefficient = atoi(prop_v[3].c_str());

			    oneTypeSkills.push_back(oneSkill);
			}
		    }
		    if (oneTypeSkills.size()>0)
		    {
			skillMap[type] = oneTypeSkills;
			ret = true;
		    }
		}
	    }
	}
	return ret;
    }

    /**
     * \brief 根据类型随机取出一个npc技能的描述
     *
     * \param type 技能类型
     * \param skill 返回值，取得的技能描述
     * \return 是否取得成功
     */
    bool getRandomSkillByType(int type,npcSkill &skill)
    {
	if (skillMap.find(type)==skillMap.end()) return false;

	skill = skillMap[type][zMisc::randBetween(0,skillMap[type].size()-1)];
	return true;
    }

    /**
     * \brief 取得所有可用的技能ID
     *
     *
     * \param list 技能ID列表
     * \return bool 是否有技能
     */
    bool getAllSkills(std::vector<DWORD> & list,WORD level)
    {
	std::map<int,std::vector<npcSkill> >::iterator type_it;
	std::vector<npcSkill>::iterator skill_it;
	for (type_it=skillMap.begin();type_it!=skillMap.end();type_it++)
	{
	    for (skill_it=type_it->second.begin();skill_it!=type_it->second.end();skill_it++)
		if (level>=skill_it->needLevel)
		    list.push_back(skill_it->id);
	}
	return list.size()>0;
    }

    /**
     * \brief 增加一个npc技能
     * \param type 技能分类
     * \param id 要增加的技能id
     * \param rate 施放几率
     * \param coefficient 系数
     */
    void addSkill(int type,DWORD id,int needLevel,int rate,int coefficient = 0)
    {
	npcSkill s;
	s.id = id;
	s.needLevel = needLevel;
	s.rate = rate;
	s.coefficient = coefficient;
	skillMap[type].push_back(s);
    }

    /**
     * \brief 删除一个npc技能
     *
     *
     * \param id 要删除的技能id
     * \return npc没有该技能则返回false
     */
    bool delSkill(DWORD id)
    {
	std::map<int,std::vector<npcSkill> >::iterator v_it;
	for (v_it=skillMap.begin();v_it!=skillMap.end();v_it++)
	{
	    std::vector<npcSkill> v = v_it->second;
	    std::vector<npcSkill>::iterator s_it;
	    for (s_it=v.begin();s_it!=v.end();s_it++)
	    {
		if (s_it->id==id)
		{
		    v.erase(s_it);
		    return true;
		}
	    }
	}
	return false;
    }

    /**
     * \brief 设置npc的攻击类型
     *
     *
     * \param data 传入的字符串
     * \param size 字符串大小
     */
    void setAType(const char *data,int size)
    {

	//Zebra::logger->error("address = %x",data);
	if(NULL == data)
	{
	    fprintf(stderr,"data == NULL");
	    return;
	}
	atypelist.clear();
	size = 1024;

	char Buf[1024];
	bzero(Buf,size);
	strncpy(Buf,data,size);
	std::vector<std::string> v_fir;
	Zebra::stringtok(v_fir,Buf,":");
	for(std::vector<std::string>::iterator iter = v_fir.begin() ; iter != v_fir.end() ; iter++)
	{
	    std::vector<std::string> v_sec;
	    Zebra::stringtok(v_sec,iter->c_str(),"-");

	    if (v_sec.size() != 2)
	    {
		return;
	    }

	    aTypeS aValue;
	    std::vector<std::string>::iterator iter_1 = v_sec.begin();

	    for(int i=0; i<2; i++)
	    {
		aValue.byValue[i] = (BYTE)atoi(iter_1->c_str());
		iter_1 ++;
	    }
	    atypelist.push_back(aValue);
	}
	return;
    }

    /**
     * \brief 取得npc的攻击类型和动画类型
     *
     *
     * \param type 输出 攻击类型
     * \param action
     */
    void getATypeAndAction(BYTE &type,BYTE &action)
    {    
	int size = atypelist.size();
	if (size == 0)
	{
	    type = NPC_ATYPE_NEAR;
	    action = 4 ;//Cmd::AniTypeEnum::Ani_Attack;//Cmd::Ani_Attack
	    return;
	}
	int num = zMisc::randBetween(0,size-1);
	type = atypelist[num].byAType;
	action = atypelist[num].byAction;
    }

    /**
     * \brief 根据表格中读出的数据填充zNpcB结构
     *
     *
     * \param npc 从表中读出的数据
     */
    void fill(NpcBase &data)
    {
	id= data.dwField0;
	strncpy(name, data.strField1, MAX_NAMESIZE);
	kind= data.dwField2;
	level= data.dwField3;
	distance = data.dwField4;
	adistance = data.dwField5;
	hp= data.dwField6;
//#if 0
//#endif
    }

    zNpcB() : zEntry()
    {
	id=          0;
	bzero(name,sizeof(name));
	kind=        0;
	level=        0;
	hp=        0;
	exp=        0;
	str=        0;
	inte=        0;
	dex=        0;
	men=        0;
	con=        0;
	cri=        0;
	color=        0;
	ai=        0;
	distance=      0;
	adistance=       0;
	pdefence=      0;
	maxpdefence=    0;
	mdefence=      0;
	maxmdefence=    0;
	five=        0;
	fivepoint=      0;
	atypelist.clear();
	mdamage=      0;
	maxmdamage=      0;
	damage=        0;
	maxdamage=      0;
	skill=        0;
	//bzero(object,sizeof(object));
	ChangeNpcID=      0;
	bzero(skills,sizeof(skills));
	bzero(state,sizeof(state));
	dodge=        0;
	rating=        0;
	pic=        0;
	trait=        0;
	bear_type=      0;
	pet_pic=      0;
	flags=        0;
	allyVisit=      0;
	//Need_Probability = 0;
    }

};
#if 0

//------------------------------------
// SkillBase
//------------------------------------
/**
* \brief 根据技能类型和等级计算一个临时唯一编号
*
*/
#define skill_hash(type,level) ((type - 1) * 100 + level)

struct SkillBase
{
	const DWORD getUniqueID() const
	{
		return skill_hash(dwField0,dwField2);
	}

	DWORD  dwField0;      // 技能ID
	char  strField1[32];    // 技能名称
	DWORD  dwField2;      // 技能等级
	DWORD  dwField3;      // 技能系别
	DWORD  dwField4;      // 技能树别
	DWORD  dwField5;      // 需要本线技能点数
	DWORD  dwField6;      // 前提技能一
	DWORD  dwField7;      // 前提技能一等级
	DWORD  dwField8;      // 前提技能二
	DWORD  dwField9;      // 前提技能二等级
	DWORD  dwField10;      // 前提技能三
	DWORD  dwField11;      // 前提技能三等级
	DWORD  dwField12;      // 间隔时间
	DWORD  dwField13;      // 攻击方式
	DWORD  dwField14;      // 能否骑马使用
	DWORD  dwField15;      // 需要物品
	char  strField16[128];  // 需要武器
	DWORD  dwField17;      // 消耗体力值
	DWORD  dwField18;      // 消耗法术值
	DWORD  dwField19;      // 消耗生命值
	DWORD  dwField20;      // 伤害加成
	char  strField21[1024];  // 效果
	DWORD  dwField22;      // 消耗物品类型
	DWORD  dwField23;      // 物品消耗数量
};//导出 SkillBase 成功，共 1 条记录

#define BENIGNED_SKILL_STATE 2
#define BAD_SKILL_STATE 4
#define NONE_SKILL_STATE 1 

struct SkillElement
{
	SkillElement()
	{
		id = 0;
		value = 0;
		percent = 0;
		time = 0;
		state = 0;
	}
	union {
		struct {
			DWORD id;
			DWORD percent;
			DWORD value;
			DWORD time;
			DWORD state;
		};
		DWORD element[5];
	};
	static SkillElement *create(SkillElement elem);
};
struct SkillStatus
{
	SkillStatus()
	{
		for(int i = 0 ; i < (int)(sizeof(status) / sizeof(WORD)) ; i ++)
		{
			status[i] = 0;
		}
	}
	union {
		struct {
			WORD id;//技能id
			WORD target;//目标
			WORD center;//中心点
			WORD range;//范围
			WORD mode;//飞行模式
			WORD clear;//能否清除
			WORD isInjure;//是否需要伤害计算
		};
		WORD status[7];
	};
	std::vector<SkillElement> _StatusElementList;
};
struct zSkillB : public zEntry
{
	bool has_needweapon(const WORD weapontype) const
	{
		std::vector<WORD>::const_iterator iter;
		if (weaponlist.empty()) return true;
		for(iter = weaponlist.begin(); iter != weaponlist.end(); iter++)
		{
			if (*iter == weapontype) return true;
		}
		return false;
	}

	bool set_weaponlist(const char *data)
	{
		weaponlist.clear(); 
		std::vector<std::string> v_fir;
		stringtok(v_fir,data,":");
		for(std::vector<std::string>::iterator iter = v_fir.begin() ; iter != v_fir.end() ; iter++)
		{
			WORD weaponkind = (WORD)atoi(iter->c_str());
			weaponlist.push_back(weaponkind);
		}
		return true;
	}

	bool set_skillState(const char *data)
	{
		skillStatus.clear(); 
		std::vector<std::string> v_fir;
		stringtok(v_fir,data,".");
		for(std::vector<std::string>::iterator iter = v_fir.begin() ; iter != v_fir.end() ; iter++)
		{
			//Zebra::logger->debug("%s",iter->c_str());
			std::vector<std::string> v_sec;
			stringtok(v_sec,iter->c_str(),":");
			/*
			if (v_sec.size() != 2)
			{
			return false;
			}
			// */
			SkillStatus status;
			std::vector<std::string>::iterator iter_1 = v_sec.begin() ;
			std::vector<std::string> v_thi;
			stringtok(v_thi,iter_1->c_str(),"-");
			if (v_thi.size() != 7)
			{
				//Zebra::logger->debug("操作!=7");
				continue;
				//return false;
			}
			std::vector<std::string>::iterator iter_2 = v_thi.begin() ;
			for(int i = 0 ; i < 7 ; i ++)
			{
				status.status[i] = (WORD)atoi(iter_2->c_str());
				//Zebra::logger->debug("status.status[%ld]=%ld",i,status.status[i]);
				iter_2 ++;
			}
			iter_1 ++;
			if (iter_1 == v_sec.end())
			{
				//Zebra::logger->debug("空操作");
				skillStatus.push_back(status);
				continue;
			}
			std::vector<std::string> v_fou;
			stringtok(v_fou,iter_1->c_str(),";");
			std::vector<std::string>::iterator iter_3 = v_fou.begin() ;
			for( ; iter_3 != v_fou.end() ; iter_3 ++)
			{
				std::vector<std::string> v_fiv;
				stringtok(v_fiv,iter_3->c_str(),"-");
				if (v_fiv.size() != 5)
				{
					//Zebra::logger->debug("元素个数不对");
					continue;
					//return false;
				}
				std::vector<std::string>::iterator iter_4 = v_fiv.begin() ;
				SkillElement element;
				for(int i = 0 ; i < 5 ; i ++)
				{
					element.element[i] = (DWORD)atoi(iter_4->c_str());
					//Zebra::logger->debug("element.element[%u]=%u",i,element.element[i]);
					iter_4 ++;
				}
				status._StatusElementList.push_back(element);
			}
			skillStatus.push_back(status);
		}
		return true;
	}
	DWORD  skillid;            //技能ID
	DWORD  level;              //技能等级
	DWORD  kind;              //技能系别
	DWORD  subkind;            //技能树别
	DWORD  needpoint;            //需要本线技能点数
	DWORD  preskill1;            //前提技能1
	DWORD  preskilllevel1;          //前提技能级别1
	DWORD  preskill2;            //前提技能2
	DWORD  preskilllevel2;          //前提技能级别2
	DWORD  preskill3;            //前提技能3
	DWORD  preskilllevel3;          //前提技能级别3
	DWORD  dtime;              //间隔时间
	DWORD  usetype;            //攻击方式
	DWORD  ride;              //可否骑马使用
	DWORD  useBook;            //需要物品
	DWORD  spcost;              //消耗体力值
	DWORD  mpcost;              //消耗法术值
	DWORD  hpcost;              //消耗生命值
	DWORD  damnum;              //伤害加成
	DWORD  objcost;            //消耗物品类型
	DWORD  objnum;              //消耗物品数量
	std::vector<SkillStatus> skillStatus;  //效果
	std::vector<WORD> weaponlist;      //武器列表



	void fill(const SkillBase &data)
	{
		id=skill_hash(data.dwField0,data.dwField2);
		skillid=data.dwField0;                //技能ID
		strncpy(name,data.strField1,MAX_NAMESIZE);
		level      = data.dwField2;          //技能等级
		kind      = data.dwField3;          //技能系别
		subkind      = data.dwField4;          //技能树别
		needpoint    = data.dwField5;          //需要本线技能点数
		preskill1    = data.dwField6;          //前提技能1
		preskilllevel1  = data.dwField7;;          //前提技能级别1
		preskill2    = data.dwField8;          //前提技能2
		preskilllevel2  = data.dwField9;          //前提技能级别2
		preskill3    = data.dwField10;          //前提技能3
		preskilllevel3  = data.dwField11;          //前提技能级别3
		dtime      = data.dwField12;          //间隔时间
		usetype      = data.dwField13;          //攻击方式
		ride      = data.dwField14;          //可否骑马使用
		useBook      = data.dwField15;          //学习需要物品
		set_weaponlist(data.strField16);          //需要武器
		spcost      = data.dwField17;          //消耗体力值
		mpcost      = data.dwField18;          //消耗法术值
		hpcost      = data.dwField19;          //消耗生命值
		damnum      = data.dwField20;          //伤害加成
		set_skillState(data.strField21);
		objcost      = data.dwField22;          //消耗物品类型
		objnum      = data.dwField23;          //消耗物品数量
	}


	zSkillB() : zEntry()
	{
		id = 0;
		skillid = 0;
		bzero(name,sizeof(name));        //说明
		level      = 0;          //技能等级
		kind      = 0;          //技能系别
		subkind      = 0;          //技能树别
		needpoint    = 0;          //需要本线技能点数
		preskill1    = 0;          //前提技能1
		preskilllevel1  = 0;          //前提技能级别1
		preskill2    = 0;          //前提技能2
		preskilllevel2  = 0;          //前提技能级别2
		preskill3    = 0;          //前提技能3
		preskilllevel3  = 0;          //前提技能级别3
		dtime      = 0;          //间隔时间
		usetype      = 0;          //攻击方式
		ride      = 0;          //可否骑马使用
		useBook      = 0;          //需要物品
		spcost      = 0;          //消耗体力值
		mpcost      = 0;          //消耗法术值
		hpcost      = 0;          //消耗生命值
		damnum      = 0;          //伤害加成
		objcost      = 0;          //消耗物品类型
		objnum      = 0;          //消耗物品数量
	}

};
#endif

struct CardBase
{
    const DWORD getUniqueID() const
    {
	return id;
    }
    union
    {
	DWORD dwField0;
	DWORD id;
    };
    char name[64];
    DWORD type;		    //类型
    DWORD occupation;	    //职业
    DWORD race;		    //种族
    DWORD kind;		    //品质
    DWORD mpcost;	    //蓝耗
    DWORD damage;	    //攻击力
    DWORD hp;		    //血量
    DWORD dur;		    //耐久
    DWORD source;	    //来源
    
    DWORD taunt;		//嘲讽(1,0)
    DWORD charge;		//冲锋(1,0)
    DWORD windfury;		//风怒(1,0)
    DWORD sneak;	    //潜行(1,0)
    DWORD shield;	    //圣盾(1,0)
    DWORD antimagic;	    //魔免(1,0)
    DWORD magicDamAdd;	    //法术伤害增加(X)
    DWORD overload;	    //过载(X)
    DWORD magicID;	    //法术ID(skill)
    DWORD needTarget;   //需要目标
    DWORD shoutID;	    //战吼ID(skill)
    DWORD shoutTarget;	    //战吼需要目标
};

struct ConditionStatus
{
    ConditionStatus()
    {
	for(int i = 0 ; i < (int)(sizeof(status) / sizeof(WORD)) ; i ++)
	{
	    status[i] = 0;
	}
    }
    union {
	struct {
	    WORD range;	    //范围(作用范围,参见枚举)
	    WORD condition; //筛选目标条件见枚举
	    WORD mode;	    //1,包括施法者本体;0,不包括施法者
	};
	WORD status[3];
    };
    ConditionStatus & operator= (const ConditionStatus &other)
    {
	range = other.range;
	condition = other.condition;
	mode = other.mode;
	return *this;
    }
};

struct zCardB : public zEntry
{
    DWORD type;		    //类型
    DWORD occupation;	    //职业
    DWORD race;		    //种族
    DWORD kind;		    //品质
    DWORD mpcost;	    //蓝耗
    DWORD damage;	    //攻击力
    DWORD hp;		    //血量
    DWORD dur;		    //耐久
    DWORD source;	    //来源
    
    BYTE taunt;		    //嘲讽(1,0)
    BYTE charge;	    //冲锋(1,0)
    BYTE windfury;	    //风怒(1,0)

    BYTE sneak;		    //潜行(1,0)
    BYTE shield;	    //圣盾(1,0)
    BYTE antimagic;        //魔免(1,0)
    BYTE magicDamAdd;	    //法术伤害增加(X)
    BYTE overload;	    //过载(X)

    DWORD magicID;	    //法术ID(skill)
    DWORD needTarget;   //需要目标
    DWORD shoutID;	    //战吼ID(skill)
    DWORD shoutTarget;	    //战吼需要目标

    void fill(CardBase &data);
};


struct SkillBase
{
	const DWORD getUniqueID() const
	{
		return id;
	}
	union
	{
	    DWORD  dwField0;      // 技能ID
	    DWORD id;
	};
	char  name[32];    // 技能名称
	char  function[256];  // 效果
	DWORD conditionType;
	DWORD conditionID;
	DWORD skillAID;  // true效果
	DWORD skillBID;  // false效果
};//导出 SkillBase 成功，共 1 条记录

struct SkillElement
{
    SkillElement()
    {
	id = 0;
	value = 0;
	value2 = 0;
	percent = 0;
	halo = 0;
	state = 0;
    }
    union {
	struct {
	    DWORD id;
	    DWORD percent;
	    DWORD value;
	    DWORD value2;
	    DWORD halo;
	    DWORD state;
	};
	DWORD element[6];
    };
    static SkillElement *create(SkillElement elem);
};
struct SkillStatus
{
    SkillStatus()
    {
	for(int i = 0 ; i < (int)(sizeof(status) / sizeof(WORD)) ; i ++)
	{
	    status[i] = 0;
	}
    }
    union {
	struct {
	    WORD id;//技能id
	    WORD attack;    //1,单攻;2,群攻,3,随机目标(次数),4,随机目标(个数)
	    WORD num;	    //当(attack==3 || attack==4)该字段有效  
	    WORD range;	    //范围(作用范围,参见枚举)
	    WORD mode;	    //1,包括施法者本体;0,不包括施法者
	    WORD condition; //筛选目标条件见枚举
	    WORD useHand;    //是否使用手选目标(1,使用;0,不使用)
	};
	WORD status[7];
    };
    std::vector<SkillElement> _StatusElementList;
};
struct zSkillB : public zEntry
{


    bool set_skillState(const char *data, std::vector<SkillStatus> &_skillStatus)
    {
	_skillStatus.clear(); 
	std::vector<std::string> v_fir;
	Zebra::stringtok(v_fir,data,".");
	for(std::vector<std::string>::iterator iter = v_fir.begin() ; iter != v_fir.end() ; iter++)
	{
	    //Zebra::logger->debug("%s",iter->c_str());
	    std::vector<std::string> v_sec;
	    Zebra::stringtok(v_sec,iter->c_str(),":");
	    /*
	       if (v_sec.size() != 2)
	       {
	       return false;
	       }
	    // */
	    SkillStatus status;
	    std::vector<std::string>::iterator iter_1 = v_sec.begin() ;
	    std::vector<std::string> v_thi;
	    Zebra::stringtok(v_thi,iter_1->c_str(),"-");
	    if (v_thi.size() != 7)
	    {
		//Zebra::logger->debug("操作!=7");
		continue;
		//return false;
	    }
	    std::vector<std::string>::iterator iter_2 = v_thi.begin() ;
	    for(int i = 0 ; i < 7 ; i ++)
	    {
		status.status[i] = (WORD)atoi(iter_2->c_str());
		//Zebra::logger->debug("status.status[%ld]=%ld",i,status.status[i]);
		iter_2 ++;
	    }
	    iter_1 ++;
	    if (iter_1 == v_sec.end())
	    {
		//Zebra::logger->debug("空操作");
		_skillStatus.push_back(status);
		continue;
	    }
	    std::vector<std::string> v_fou;
	    Zebra::stringtok(v_fou,iter_1->c_str(),";");
	    std::vector<std::string>::iterator iter_3 = v_fou.begin() ;
	    for( ; iter_3 != v_fou.end() ; iter_3 ++)
	    {
		std::vector<std::string> v_fiv;
		Zebra::stringtok(v_fiv,iter_3->c_str(),"-");
		if (v_fiv.size() != 6)
		{
		    //Zebra::logger->debug("元素个数不对");
		    continue;
		    //return false;
		}
		std::vector<std::string>::iterator iter_4 = v_fiv.begin() ;
		SkillElement element;
		for(int i = 0 ; i < 6 ; i ++)
		{
		    element.element[i] = (DWORD)atoi(iter_4->c_str());
		    //Zebra::logger->debug("element.element[%u]=%u",i,element.element[i]);
		    iter_4 ++;
		}
		status._StatusElementList.push_back(element);
	    }
	    _skillStatus.push_back(status);
	}
	return true;
    }
    DWORD   skillid;            //技能ID
    std::vector<SkillStatus> skillStatus;  //效果
    DWORD   conditionType;
    DWORD   conditionID;
    DWORD skillAID;  //A效果
    DWORD skillBID;  //B效果

    zSkillB() : zEntry()
    {
	id = 0;
	skillid = 0;
	bzero(name,sizeof(name));        //说明
    }

    void fill(const SkillBase &data);

};

struct StateBase
{
    const DWORD getUniqueID() const
    {
	return id;
    }
    union
    {
	DWORD dwField0;
	DWORD id;
    };
    char name[32];
    DWORD mainBuff;
};

struct zStateB : public zEntry
{
    DWORD mainBuff;		    //主BUFF
    zStateB() : zEntry()
    {
	id = 0;
	mainBuff = 0;
	bzero(name,sizeof(name));        //说明
    }

    void fill(StateBase &data);
};

#endif

