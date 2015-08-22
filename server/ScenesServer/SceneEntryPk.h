#ifndef _SceneEntryPk_h_
#define _SceneEntryPk_h_
#include <map>
#include <set>
#include "zSceneEntry.h"
#include "zTime.h"

class Scene;
//sky 角色战斗状态枚举
enum useFightState
{
	USE_FIGHT_NULL	= 0,
	USE_FIGHT_PVE	= 1,
	USE_FIGHT_PVP	= 2,
};

/**
 * \brief 角色PK定义类,有待扩充
 */
struct SceneEntryPk: public zSceneEntry
{
  public:
    typedef std::set<DWORD> SelectedSet;
    typedef SelectedSet::value_type SelectedSet_value_type;
    typedef SelectedSet::iterator SelectedSet_iterator;
    ///被选中列表
    SelectedSet selected;
    //zMutex selected_lock;
  public:
    /**
     * \brief 构造函数,初始化
     *
     *
     * \param type 类型
     * \param state 初始状态
     * \return 
     */
    SceneEntryPk(SceneEntryType type,const SceneEntryState state = SceneEntry_Normal):zSceneEntry(type,state)
    {
      attackAction = true;
      moveAction = true;
      skillAction = true;
      ignoreDam = false;
      ignorePhysicDam = false; ///物理攻击免疫
      ignoreMagicDam = false;  ///魔法攻击免疫
      blind  = false;
      mplock = false;
      hplock = false;
      splock = false;
      maxattack=false;
      mdeftozero=false;   /// 魔法防御变成0
      pdeftozero=false;   /// 魔法防御变成0
      useRes=true;    // 可否使用物品
      isAutoRestitute=true;// 自动恢复HP,MP,SP
      frenzy = false; // 狂暴状态
      huntermark = false; //
      isPhysics =  false; // 走物理计算
      dread = false;// 恐吓
      dropweapon = false; //缴械
      tosh = false; //胡言乱语
      stone = false; // 石化
      watchTrap = false; //观察陷阱
      mhpd = false;
      swapdamcharid = 0; //  献身,施法者的临时ID
      swapdamcharid2 = 0; //关爱,伤害转移施法者的临时ID
      reSendData = false;
      changeface = false; // 易容术可以访问国外NPC
      hideme = false; //隐藏自己
      icebox = false; //冰灵柜状态
      liquidState = false; //液态（无阻挡）
      resist = 0;
      dmpbyhp = 0; //吸魔剑法
      mdmtomp = 0; //魔法伤害转换成魔法值
      mdmtompper = 0; //魔法伤害转换成魔法值几率
      icedam  = 0; //冰冻结界吸收的伤害值
      unitaryBattleKeepTime = 0;

      afreetMode = false; /// 恶魔杀无赦（砍人必死）
      angelMode = false;  /// 天使无敌（优于恶魔,砍不死）
      isPhysicBang = false;///是否暴击
      isMagicBang = false;///是否暴击
      isHPhysicBang = false;///是否暴击
      isHMagicBang = false;///是否暴击
      assault = false;
      curMagicManType =0;
      curMagicManID =0;
      lastPkTime=0;
      bombskillId = 0;
      dietodam = 0;
      attacklow = false; ///攻击力取下限
      killnow=0; ///一击必杀百分比
      diewithme=0; //血债血偿
      switchdie=0; //同屏内转移死亡状态
      summonsplit = 0; // 焰魔君主死亡后分裂标志
      damtohpper = 0;  /// 角色的伤害转化成生命值的几率;
      damtohpvalper = 0; /// 角色的伤害转化成生命值的百分比

	  Daggerflag = false;	//sky 匕首增加伤害标志
	  Throflag = false;		//sky 投掷武器增加伤害标志
	  Handsflag = false;	//sky 双手武器增加伤害标志
	  Handflag = false;		//sky 单手武器增加伤害标志
	  Flameflag = false;	//sky 火焰魔法增加伤害标志
	  Iceflag = false;		//sky 冰霜魔法增加伤害标志
	  Sacredflag = false;	//sky 神圣魔法增加伤害标志
	  Darkflag = false;		//sky 黑暗魔法增加伤害标志
	  Immolate = false;
	  dread = false;			/// 恐惧
	  blind = false;			/// 失明状态
	  frozen = false;			/// 冰冻
	  repentange = false;		/// 忏悔
	  Silence = false;			/// 沉默
	  Coma = false;				///	昏迷
	  Exile = false;			/// 放逐

	  ImmuneDread = false;		/// 免疫恐惧状态
	  ImmuneFrozen = false;		/// 免疫冰冻状态
	  ImmuneSilence = false;	/// 免疫沉默状态
	  ImmuneComa = false;		/// 免疫昏迷状态
	  ImmuneExile = false;		/// 免疫放逐状态 
	  Soulflag = false;

	  isUsePveOrPvp = USE_FIGHT_NULL; /// PVE状态

      reSendOther = false;

	  RandRegionX = 0;
	  RandRegionY = 0;

	  BattCampID = 0;
    }
    virtual ~SceneEntryPk(){};
#if 0
    ///pk状态
    ScenePkState pkState;
    ///PK过程中变化的数值
    PkValue pkValue;
    ///技能附加的状态
    SkillState skillValue;
    ///PK过程相对不便的数值
    PkPreValue pkpreValue;

    SkillStatusManager skillStatusM;  /// 技能状态管理器
    SkillStatusCarrier carrier;      /// 技能操作投送器
    std::map<BYTE,BYTE>  _effectState;  /// 技能状态特效计数器
#endif
    SceneEntryPk *attackTarget;      /// 武器附带攻击状态攻击目标

    bool attackAction;  /// 攻击许可
    bool moveAction;  /// 移动许可
    bool skillAction;   /// 技能攻击许可
    bool ignoreDam;    /// 忽略伤害
    bool ignorePhysicDam; /// 忽略物理伤害
    bool ignoreMagicDam; /// 忽略魔法伤害
    bool notifyHMS;    /// 通知HP,MP,SP改变
    bool maxattack;    /// 攻击取上限
    bool mdeftozero;   /// 魔法防御变成0
    bool pdeftozero;   /// 魔法防御变成0
    bool useRes;    /// 可否使用物品
    bool isAutoRestitute; // 是否自动恢复
    bool frenzy;       /// 狂暴状态
    bool huntermark;  /// 猎手标记
    bool isPhysics;    /// 计算公式走物理   
    bool dropweapon;   /// 缴械
    bool tosh;        /// 胡言乱语
    bool stone;      /// 石化
    bool watchTrap;    /// 观察陷阱
    bool mhpd;  /// 魔法伤害减半,物理伤害加倍标志�
#if 0
    bool blazeflag; ///火焰系法术加成标志
    bool levinflag;  ///雷电系法术加成标志
    bool trapflag;  ///陷阱系法术加成标志
    bool iceflag;  ///冰系法术加成标志
#endif
    DWORD swapdamcharid; ///献身施法者的临时ID
    DWORD swapdamcharid2; ///关爱施法者的临时ID
    bool changeface;  ///易容术,可以访问国外NPC
    bool hideme;     /// 隐藏自己
    bool icebox;   /// 冰灵柜状态
    bool liquidState; ///液态（无阻挡）
    BYTE resist;     ///格档,有一次抵御伤害的机会
    BYTE dmpbyhp;  ///吸魔剑法
    BYTE mdmtomp;  ///将法术伤害转成魔法值百分比
    BYTE mdmtompper; ///将法术伤害转成魔法值百分比几率
    SDWORD icedam;  ///冰冻结界吸收的伤害数值
    QWORD unitaryBattleKeepTime; //一元阵法持续时间
    SDWORD dietodam; ///死亡攻击数值
    bool attacklow; ///攻击力取下限
    BYTE killnow; //一击必杀
    BYTE diewithme; //血债血偿
    BYTE switchdie; //同屏转移死亡状态的几率
    BYTE summonsplit; //焰魔君主死亡后分裂标志


    bool reSendData;  /// 重新发送NPC或角色数据给自己
    bool reSendOther;  /// 重新发送NPC或角色数据给其它人

    bool mplock;  ///对mp进行锁定
    bool hplock;  ///对hp进行锁定
    bool splock;  ///对sp进行锁定

    bool afreetMode; /// 恶魔杀无赦（砍人必死）
    bool angelMode;  /// 天使无敌（优于恶魔,砍不死）

    bool isPhysicBang; /// 是否物理暴击
    bool isMagicBang; /// 是否魔法暴击
    bool isHPhysicBang; /// 是否物理暴击
    bool isHMagicBang; /// 是否魔法暴击

    bool assault; ///冲锋

	//sky 新增角色战斗状态 begin
	bool Daggerflag;	//sky 匕首增加伤害标志
	bool Throflag;		//sky 投掷武器增加伤害标志
	bool Handsflag;		//sky 双手武器增加伤害标志
	bool Handflag;		//sky 单手武器增加伤害标志
	bool Flameflag;		//sky 火焰魔法增加伤害标志
	bool Iceflag;		//sky 冰霜魔法增加伤害标志
	bool Sacredflag;	//sky 神圣魔法增加伤害标志
	bool Darkflag;		//sky 黑暗魔法增加伤害标志
	bool Immolate;		//sky 献祭标志

	bool dread;			/// sky 恐惧
	bool blind;			/// sky 失明状态
	bool frozen;		/// sky 冰冻
	bool repentange;	/// sky 忏悔
	bool Silence;		/// sky 沉默
	bool Coma;			///	sky 昏迷
	bool Exile;			/// sky 放逐

	bool ImmuneDread;	/// sky 免疫恐惧状态
	bool ImmuneFrozen;	/// sky 免疫冰冻状态
	bool ImmuneSilence;	/// sky 免疫沉默状态
	bool ImmuneComa;	/// sky 免疫昏迷状态
	bool ImmuneExile;	/// sky 免疫放逐状态
	//sky 灵魂状态,该状态下不可以见并且是无敌的
	bool Soulflag;

	//sky 新增角色状态(PVE:PVP)
	useFightState isUsePveOrPvp; /// PVE状态
	// sky end

	//sky 新增阵营ID
	//sky 用于战场中攻击和胜利判定用(重要)
	DWORD BattCampID;
	//sky end

    zPos keepPos;  /// 保存技能的攻击目标点,传递给需要使用的功能
    BYTE keepDir;  /// 保存技能的攻击方向,传递给需要使用的功能
    BYTE  lastPkTime; //最后PK标志
    DWORD bombskillId; /// 人物死亡后投送技能ID

    BYTE damtohpper;  /// 角色的伤害转化成生命值的几率;
    BYTE damtohpvalper; /// 角色的伤害转化成生命值的百分比

	//////////////////////////////////////////////////////////////////////////////////
	///sky 用来控制NPC被恐惧或者失明后移动的最大范围
	///随机范围的中心
	zPos RandPos;
	///随机最大范围的宽和高
	int RandRegionX,RandRegionY;


    ///所在的场景指针
    Scene *scene;

    //技能
    //UserSkillM usm;

    //virtual void sendtoSelectedHpAndMp() = 0;
    //virtual void sendtoSelectedState(DWORD state,WORD value,WORD time) = 0;
    virtual void sendSevenStateToMe(DWORD state,WORD value,WORD time){}
    void sendCmdToSelected(const void *pstrCmd,const DWORD nCmdLen);

    /**
     * \brief 造成直接伤害
     * \param pAtt 攻击者
     * \param dam 伤害
     * \param notify 是否通知客户端
     * \return 伤害值
     */
    virtual SWORD directDamage(SceneEntryPk *pAtt,const SDWORD &dam,bool notify=false);

    /**
     * \brief 改变角色的hp
     * \param hp 变更的HP
     */
    virtual void changeHP(const SDWORD &hp)=0;

    /**
     * \brief 改变角色的sp
     * \param sp 变更的SP
     */
    virtual void changeSP(const SDWORD &sp)=0;

    /**
     * \brief 改变角色的mp
     * \param mp 变更的MP
     */
    virtual void changeMP(const SDWORD &mp)=0;

    /**
     * \brief 获得最大的hp
     * \return 返回最大值
     */
    virtual DWORD getMaxHP(){return 0;}

    /**
     * \brief 获得最大的hp
     * \return 返回最大值
     */
    virtual DWORD getBaseMaxHP(){return 0;}

    /**
     * \brief 获得最大的mp
     * \return 返回最大值
     */
    virtual DWORD getMaxMP(){return 0;}

    /**
     * \brief 获得最大的mp
     * \return 返回最大值
     */
    virtual DWORD getBaseMaxMP(){return 0;}

    /**
     * \brief 获得最大的sp
     * \return 返回最大值
     */
    virtual DWORD getMaxSP(){return 0;}

    /**
     * \brief 获得当前魔法攻击力
     * \return 魔法攻击力
     */
    virtual DWORD getMaxMDamage(){return 0;}

    /**
     * \brief 获得当前物理攻击力
     * \return 物理攻击力
     */
    virtual DWORD getMaxPDamage(){return 0;}

    /**
     * \brief 获得当前物理防御力
     * \return 物理防御力
     */
    virtual DWORD getPDefence(){return 0;}

    /**
     * \brief 获得当前魔法防御力
     * \return 魔法防御力
     */
    virtual DWORD getMDefence(){return 0;}

    /**
     * \brief 重置最大的hp
     */
    virtual void changeAndRefreshHMS(bool lock=true,bool sendData=true){}

    /**
     * \brief 在被自己攻击之前的处理,包括,装备损耗处理,攻击有效几率判断等
     * \param pUser 攻击者
     * \param rev 本次攻击的触发指令
     * \param physics 是否物理攻击
     * \param good 为true则必中,false要判断回避率
     * \return true为这次攻击是有效的,false为一次无效的攻击
     */
    virtual bool preAttackMe(SceneEntryPk *pUser,const Cmd::stAttackMagicUserCmd *rev,bool physics=true,const bool good = false)=0;

    /**
     * \brief 角色被攻击
     * \param pUser 攻击者
     * \param rev 本次攻击的触发消息
     * \param physics 是否物理攻击
     * \return true为这次攻击是有效的,false为一次无效的攻击
     */
    virtual bool AttackMe(SceneEntryPk *pUser,const Cmd::stAttackMagicUserCmd *rev,bool physics=true,SWORD rangDamageBonus=0);

    /**
     * \brief 设置角色身上的特效状态,对应状态的访问计数将累加
     * \param state 状态ID 根据enum SceneEntryState取值
     */
    //void setEffectStatus(const WORD &state)
    //{
      //_effectState[state]=_effectState[state]+1;
      //if (1 == _effectState[state])
      //{
        //showCurrentEffect(state,true);
      //}
    //}


    /**
     * \brief 清除角色身上的特效状态,对应状态的访问计数将减,减为0的时候清除该状态
     * \param state 状态ID 根据enum SceneEntryState取值
     */
    //void clearEffectStatus(const WORD &state)
    //{
      //_effectState[state]=_effectState[state]-1;
      //if (0 == _effectState[state])
      //{
        //showCurrentEffect(state,false);
      //}
    //}

    /**
     * \brief 让宠物重生
     */
    //virtual void relivePet(){};

    /**
     * \brief 让角色重生
     */
    virtual void relive(){};

    /**
     * \brief 得到玩家的权限等级
     * \return 玩家的权限等级
     */
    virtual BYTE getPriv(){return 0;}

    /**
     * \brief 角色被击退N格
     */
    virtual void standBack(const DWORD dwAttTempID,DWORD grids)=0;

    /**
     * \brief 将攻击目标换成dwTempID所指向的角色玩家
     * \param dwTempID 目标角色的临时ID
     */
    virtual void changeAttackTarget(const DWORD &dwTempID){};

    /**
     * \brief 让角色死亡
     * \param dwTempID 攻击者的临时ID
     */
    virtual void toDie(const DWORD &dwTempID)=0;

    /**
     * \brief 在屏幕内随机移动
     */
    virtual void goToRandomScreen(){};

    /**
     * \brief 在屏幕内定点移动
     */
    virtual void jumpTo(zPos &newPos){};

    /**
     * \brief 通知客户端生命值的变化
     */
    virtual void attackRTHpAndMp(){};

    /**
     * \brief 判断角色是否死亡
     */
    virtual bool isDie() = 0;

    /**
     * \brief 获取角色的级别
     */
    virtual DWORD getLevel() const =0;

    /**
     * \brief 获取五行点数
     * \return 返回该类型的五行点数
     */
    virtual DWORD getFiveType() {return 0;}

    /**
     * \brief 需要的职业类型,决定可以使用的技能类型
     */
    virtual bool needType(const DWORD &needtype)=0;

    /**
     * \brief 需要的职业类型,决定可以使用的技能类型
     */
    //virtual bool addSkillToMe(zSkill *skill)=0;

    /**
     * \brief 是否有该技能需要的武器
     * \return true 有 false 没有
     */
    //virtual bool needWeapon(DWORD skillid)=0;

    /**
     * \brief 是否Pk区域
     * \param other PK相关人
     * \return true 是 false 否
     */
    virtual bool isPkZone(SceneEntryPk *other=NULL)=0;

    /**
     * \brief 依赖物品消耗型法术
     * \param object 消耗物品的类型
     * \param num 消耗物品的数量
     * \return true 消耗成功 false 失败
     */
    virtual bool reduce(const DWORD &object,const BYTE num)=0;

    /**
     * \brief 检查可消耗物品是否足够
     * \param object 消耗物品的类型
     * \param num 消耗物品的数量
     * \return true 足够 false 不够
     */
    virtual bool checkReduce(const DWORD &object,const BYTE num)=0;
    /**
     * \brief 获取装备伤害加成
     * \return 伤害加成
     */
    //virtual WORD getDamageBonus() {return 0;}

    /**
     * \brief 施放技能所导致的消耗MP,HP,SP
     * \param base 技能基本属性对象
     * \return true 消耗成功 false 失败
     */
    //virtual bool doSkillCost(const zSkillB *base) = 0;

    /**
     * \brief 检查是否骑马
     * \return true 骑马 false 没骑
     */
    virtual bool checkMountHorse() {return false;}

    /**
     * \brief 检查施放技能所导致的消耗MP,HP,SP是否足够
     * \param base 技能基本属性对象
     * \return true 消耗成功 false 失败
     */
    //virtual bool checkSkillCost(const zSkillB *base) = 0;

    /**
     * \brief 检查自身的施放成功几率,决定这次技能是否可以施放
     * \return true 成功 false 失败
     */
    virtual bool checkPercent()=0;

    /**
     * \brief 判断是否是敌人
     * \return true 是 false 不是
     */
    virtual int isEnemy(SceneEntryPk *entry,bool notify = false,bool good = false)=0;

    /**
     * \brief 判断是否可攻击
     * \return true 是 false 不是
     */
    virtual bool isBugbear(){return true;}

    /**
     * \brief 获取自己的主人,一般针对NPC而言,Player的主人是自己
     * \return NULL或者主人的对象指针
     */
    virtual SceneEntryPk *getMaster()=0 ;

    /**
     * \brief 得到最上层的主人
     *
     * \return 主人
     */
    virtual SceneEntryPk *getTopMaster() =0;

    /**
     * \brief 检查飞行路径
     * \param pTarget 被攻击者
     * \param aType 飞行方式
     * \return 是否顺利攻击
     */
    virtual bool checkMagicFlyRoute(zSceneEntry *pTarget,BYTE aType);

    /**
     * \brief 角色死亡检查
     * \param pAtt 攻击者
     */
    virtual bool processDeath(SceneEntryPk *pAtt);
    virtual void leaveBattle();

    /**
     * \brief 通过杀死比自己等级高的npc来减少善恶度
     *
     * \param pNpc 杀死的npc
     */
    //virtual void reduceGoodness(SceneNpc *pNpc){};

    virtual void setupCharBase(bool lock=true) {};
    /**
     * \brief 重新发送本角色的地图数据
     */
    virtual void reSendMyMapData()=0;

    /**
     * \brief 攻击者冲向我
     * \param attacktype 攻击者的类型
     * \param tempid 攻击者的临时id
     */
    virtual void assaultMe(BYTE attacktype,DWORD tempid) {}
#if 0
    /**
     * \brief 召唤宠物
     */
    //virtual ScenePet* summonPet(DWORD id,Cmd::petType type) {return NULL;}
    /**
     * \brief 召唤图腾
     */
	virtual ScenePet*	summonPet(DWORD id,Cmd::petType type,DWORD standTime = 0,DWORD sid=0,const char * petName = "",DWORD anpcid=0,zPos pos=zPos(0,0),BYTE dir=4) = 0;

	//[sky]幻影专用召唤函数
	virtual	bool	MirageSummonPet(DWORD id,Cmd::petType type,DWORD standTime,WORD num,const char * petName = "",DWORD anpcid = 0,zPos pos=zPos(0,0),BYTE dir=4) = 0;
    //召唤指定持续时间的宠物

	//[sky] 召唤士兵(每个士兵都是一个独立的个体不同以召唤兽或者宠物)
	virtual int summonSoldiers(DWORD id, Cmd::petType type, WORD num, DWORD sid=0, const char * name="", DWORD anpcid = 0,zPos pos=zPos(0,0),BYTE dir=4) = 0;

    /**
     * \brief 设置战斗类宠物的跟踪攻击目标
     */
    virtual void setPetsChaseTarget(SceneEntryPk *entry){};

    /**
     * \brief 设置战斗类宠物的跟踪攻击目标
     * \param weaponType 武器类型
     * \return true武器类型符合,false武器类型不符合
     */
    virtual bool checkWeapon(BYTE weaponType){return true;}


    /**
     * \brief 获取抗毒增加  
     */
    virtual SWORD getPoisondef(){return 0;}
    /**
     * \brief 获取抗麻痹增加  
     */
    virtual SWORD getLulldef(){return 0;}
    /**
     * \brief 获取抗眩晕增加  
     */
    virtual SWORD getReeldef(){return 0;}
    /**
     * \brief 获取抗噬魔增加  
     */
    virtual SWORD getEvildef(){return 0;}
    /**
     * \brief 获取抗噬力增加  
     */
    virtual SWORD getBitedef(){return 0;}
    /**
     * \brief 获取抗混乱增加  
     */
    virtual SWORD getChaosdef(){return 0;}
    /**
     * \brief 获取抗冰冻增加  
     */
    virtual SWORD getColddef(){return 0;}
    /**
     * \brief 获取抗石化增加  
     */
    virtual SWORD getPetrifydef(){return 0;}
    /**
     * \brief 获取抗失明增加  
     */
    virtual SWORD getBlinddef(){return 0;}
    /**
     * \brief 获取抗定身增加  
     */
    virtual SWORD getStabledef(){return 0;}
    /**
     * \brief 获取抗减速增加  
     */
    virtual SWORD getSlowdef(){return 0;}
    /**
     * \brief 获取抗诱惑增加
     */
    virtual SWORD getLuredef(){return 0;}


    /**
     * \brief 获取毒增加  
     */
    virtual SWORD getPoison(){return 0;}
    /**
     * \brief 获取麻痹增加  
     */
    virtual SWORD getLull(){return 0;}
    /**
     * \brief 获取眩晕增加  
     */
    virtual SWORD getReel(){return 0;}
    /**
     * \brief 获取噬魔增加  
     */
    virtual SWORD getEvil(){return 0;}
    /**
     * \brief 获取噬力增加  
     */
    virtual SWORD getBite(){return 0;}
    /**
     * \brief 获取混乱增加  
     */
    virtual SWORD getChaos(){return 0;}
    /**
     * \brief 获取冰冻增加  
     */
    virtual SWORD getCold(){return 0;}
    /**
     * \brief 获取石化增加  
     */
    virtual SWORD getPetrify(){return 0;}
    /**
     * \brief 获取失明增加  
     */
    virtual SWORD getBlind(){return 0;}
    /**
     * \brief 获取定身增加  
     */
    virtual SWORD getStable(){return 0;}
    /**
     * \brief 获取减速增加  
     */
    virtual SWORD getSlow(){return 0;}
    /**
     * \brief 获取诱惑增加
     */
    virtual SWORD getLure(){return 0;}


    virtual void appendAttack(const Cmd::stAttackMagicUserCmd *rev) {};


    ///宠物
    ScenePet * pet;
    ///召唤兽
    ScenePet * summon;
    ///图腾
    std::list<ScenePet *> totems;

	///[sky]幻影专用多NPC召唤
	std::list<ScenePet *> MirageSummon;
    /**
     * \brief 取消召唤的宠物
     */
    virtual bool killOnePet(ScenePet *)=0;
    virtual void killAllPets()=0;
    virtual void killSummon() {};

    virtual void sendPetDataToNine();
    virtual void addPetExp(DWORD,bool,bool) = 0;
    void petLevelUp(ScenePet *);
#endif

    virtual bool isRedNamed(bool allRedMode=true) const = 0;
    DWORD getHp();
    DWORD getMaxHp();
    bool isFighting();
    ///脱离战斗状态的时间
    zRTime endBattleTime;
    ///施法者
    DWORD curMagicManID;
    ///对施法者类型
    DWORD curMagicManType;
    ///对手
    DWORD curTargetID;
    ///对手类型
    DWORD curTargetType;
    virtual bool setCurTarget(SceneEntryPk *,bool=false);
    virtual bool setCurTarget(DWORD,DWORD,bool=false);
    SceneEntryPk * getCurTarget();

    ///防御的对手
    DWORD defTargetID;
    ///防御的对手类型
    DWORD defTargetType;
    virtual bool setDefTarget(SceneEntryPk *,bool=false);
    virtual bool setDefTarget(DWORD,DWORD,bool=false);
    SceneEntryPk * getDefTarget();
    void clearDefTarget();


    void setEndBattleTime(const zRTime &,int);
    bool checkEndBattleTime(const zRTime &);

    virtual WORD skillUpLevel(WORD skilltype,WORD skillkind){return 0;}

    virtual void getSummonAppendDamage(WORD &minDamage,WORD &maxDamage){};
    virtual void exchangeMeAndSummonPet(){};
    virtual DWORD getMasterMana(){return 0;}
    virtual DWORD getMP(){return 0;}
    virtual void clearMana(){}
    virtual void leech(DWORD dwDam) {}
    virtual void sendMessageToMe(const char *pattern) {};
    virtual void setSpeedRate(float rate){};
    virtual void resetSpeedRate(){};
    /**
     * \brief 设置角色的当前状态,并根据当前状态呈现角色的特效或者关闭特效
     * \param state 状态ID 根据enum SceneEntryState取值
     * \param isShow 是否显示特效
     */
    virtual void showCurrentEffect(const WORD &state,bool isShow,bool notify=true) = 0;
  private:
#if 0
    virtual void processMaskOnAttack(SceneEntryPk *){};
    virtual void processMaskOnDefence(){};
    virtual void processAddDam(int &dwDam,int &dwDamDef,bool physics){};
    virtual void reduceDam(int &dwDam,int &dwDamDef,bool physics){};
    virtual void reflectDam(int &dwDamDef,int &dwDamSelf,DWORD skillID){};
    virtual void hp2mp(int &dwDamDef){};
    virtual void reflectSkill(SceneEntryPk *pAtt,const Cmd::stAttackMagicUserCmd *rev){};
#endif
};
typedef std::vector<SceneEntryPk *> SceneEntryPk_vec;
#endif

