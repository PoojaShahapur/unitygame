#ifndef _SceneUser_h_
#define _SceneUser_h_
#include "CharBase.h"
#include "Command.h"
#include "SessionClient.h"
#include "zSceneEntryIndex.h"
#include "SceneTask.h"
#include "SceneManager.h"
#include "SceneEntryPk.h"
#include "RecordCommand.h"
#include "CounterManager.h"
#include "Quest.h"
#include "zObject.h"
#include "HeroInfoManager.h"
#include "CardTujianManager.h"
#include "HeroCardCmd.h"
#include "GroupCardManager.h"
#include "ChallengeGameManager.h"
#include "BattleUserCmd.h"

class Scene;

//sky 用户战场数据
struct stUserBattfieldData
{
	DWORD BattfieldHonor;	//sky 战场荣誉
	DWORD SportsHonor;		//sky 竞技场荣誉点

	//sky 用户传进战场前的地图数据
	char MapName[MAX_NAMESIZE+1];	//sky 地图名
	int x;							//sky 坐标x
	int y;							//sky 坐标y

	stUserBattfieldData()
	{
		BattfieldHonor = 0;
		SportsHonor = 0;
		x = 0;
		y = 0;
		bzero(MapName, MAX_NAMESIZE+1);
	}
};

struct SceneUser:public SceneEntryPk
{

  public:


	  //para:  dupId,副本号.istemp,true从tempDups中删除,false,从circleDups中删除
	  //void delFromDups(unsigned short dupId,bool istemp);

	  //void clearDups(bool istemp)
	  //{
		//if(istemp)
		//	tempDups.clear();
		//circleDups.clear();
	 // }

	 // std::map<DWORD,unsigned short> tempDups;//临时性副本
	 // std::map<DWORD,unsigned short> circleDups;//周期性副本

	  bool userQuestEnterDup(DWORD mapId);

  private:

	//userScriptTaskContainer *_userScriptTaskContainer;
    char replyText[MAX_CHATINFO];//自动回复

	bool m_bCanJump; // [ranqd] 玩家是否处于可跳转地图的状态

    //回收延时等到(毫秒)
    zRTime recycle_delay;

    static DWORD Five_Relation[];
    /*
       zRTime tenSecondTime;
       zRTime oneSecondTime;
       zRTime oneMinuteTime;
     */
    Timer _half_sec;
    Timer _one_sec;
    Timer _five_sec;
	Timer _3_sec;
    Timer _ten_sec;
    Timer _one_min;
    Timer _five_min;
    //存档定时器,存档间隙具有随机性,可以打乱存档频率
    RandTimer _writeback_timer;
    struct box_target
	{
		//zObject *targetO;
		//zObject *defaultO;
	    //DWORD default_id;
		//DWORD default_level;
		//DWORD default_kind;
		//DWORD target_id;
		//DWORD target_level;
		//DWORD target_kind;

		//DWORD default_item;
		//DWORD default_Level;
	};

    box_target box_item;

    ///5秒计数,每5秒+1
    DWORD _5_sec_count;

    /// 杀死怪物个数
    DWORD killedNpcNum;
    const zRTime loginTime;
    /// 在线时间统计
    DWORD lastIncTime;
    /// 上次叛国时间记录
    DWORD lastChangeCountryTime;
    /// 临时关闭保护
    DWORD temp_unsafety_state;
    /// 财产保护状态
    BYTE  safety;
    /// 财产保护设置
    BYTE safety_setup;

    /// 死亡等待时间,死亡后5分钟,系统自动使角色进行回城复活
    int deathWaitTime;

    //是否正在后退中(ms)
    int backOffing;

    //用户权限等级
    BYTE priv;

    // 交战记录
    //std::vector<WarRecord> vWars;
    
    // 赠送材料数量（组）
  //  DWORD Give_MatarialNum;

    char wglog[MAX_CHATINFO];
    DWORD wglog_len;
    void getWgLog(DWORD);
  public:
    //must be deleted by Visitor
    
    //副本号索引
    //unsigned short dupIndex;

    ~SceneUser();
    void destroy();

	DWORD getID()
	{
		return this->id;
	}

#if 0
	DWORD getTeamLeader()
	{
		TeamManager* team = SceneManager::getInstance().GetMapTeam(TeamThisID);

		if(team)
			return team->getLeader();
		else
			return 0;
 	}


	bool addObjToPackByThisID(DWORD thisid);

	bool addScriptTask(int _type,const char* _funcName,int _elapse,int p1);
	bool delScriptTask(int _type);




    // 参与家族运镖的玩家ID
    std::vector<SeptGuard> venterSeptGuard;
    // 参与家族运镖的玩家ID
    //std::vector<SeptGuard> vfinishSeptGuard;

    //提取队伍成员信息
	const TeamMember *getMember(WORD index);
	/*{	
		//TeamMember _member;
	    
		return team.getTeam().getTeamMember(index);
		//DWORD _size = members->size();
		//if(index < 0 || index > members->size() - 1)
		//	return false;
		//member.begintime = (*members)[index].begintime;
		//member.id = (*members)[index].id;
		//member.offtime = (*members)[index].id;
		//member.tempid = (*members)[index].tempid;
		//strcpy_s(member.name,(*members)[index].name);
		//return  &(*members)[index];
		//return true;
	    
	}*/

	WORD getTeamSize()
	{
		TeamManager* team = SceneManager::getInstance().GetMapTeam(TeamThisID);
		return team->getSize();
	}
#endif
  public:

    DWORD getLoginTime()
	{
		return (time(NULL) - loginTime.sec()) / 60;
	}

    // 赠送材料数量（组）
    DWORD Give_MatarialNum;
    
    //道具卡张数
    DWORD Card_num;

    //Dice * miniGame;//正在玩的小游戏

    void petAutoRepair(zObject *);//宠物自动修装备

    DWORD lastKiller;//最后一个杀死自己的玩家的临时ID
    DWORD dropTime;//客户端举报外挂后,随机断线的时间

    enum
    {
      ACCPRIV_NEWBIE_EQUIP_AT_5_15    = 0x00000001, //5、15级时给予新手装备
      ACCPRIV_GOLD_EQUIP_AT_5_15      = 0x00000002, //5、15级时给予黄金装备
      ACCPRIV_GREEN_EQUIP_AT_5_25_50    = 0x00000004  //5、25、50级时给予绿色装备
    };

    void sendGiftEquip(WORD level);//送体验卡,黄金卡,绿色卡的装备
    void givePetPoint();//30,40,50,60级送宝宝修炼时间

    DWORD processCheckTime;//下次检测外挂的时间

    //DWORD refreshPetPackSize();

    //bool npcTradeGold(Cmd::stBuyObjectNpcTradeUserCmd *ptCmd,zObjectB *b,BYTE itemlevel);
    void autoReply(char *) const;
#if 0
    DWORD adoptedCartoon;   //被收养的宠物ID
    std::map<DWORD,Cmd::t_CartoonData> cartoonList;
    CartoonPet * cartoon;//放出的卡通宠物
    std::map<DWORD,CartoonPet *> adoptList;//收养的卡通宠物

    typedef std::map<DWORD,Cmd::t_CartoonData>::iterator cartoon_it;
    typedef std::map<DWORD,CartoonPet *>::iterator adopt_it;
#endif
    //下次可以查询数据库的时间,拍卖用
    DWORD queryTime;

    bool isSendingMail;
    bool isGetingMailItem;

    /*      
     ** whj 
     ** 正在退出状态不再接受任何指令
     **/
    bool unReging;
#ifdef _TEST_DATA_LOG
    //测试数据统计
    CharTest chartest;
    void readCharTest(Cmd::Record::t_Read_CharTest_SceneRecord *rev);
    void writeCharTest(Cmd::Record::enumWriteBackTest_Type type);
#endif
    // PK模式
    BYTE pkMode;
    //系统设置
    union{
      struct{
        BYTE savePkMode;
        BYTE setting[19];
      };
      BYTE sysSetting[20];
    };
    DWORD chatColor[8];

    static const DWORD CALL_PET_REGION = 6;

    ///马
    //Horse horse;
    //ScenePet * ridepet;

    //在国外死亡复活到本国凤凰城,写死了^_^
    DWORD deathBackToMapID;

    //跨服跳地图在Session未返回时的临时记录
    char wait_gomap_name[MAX_NAMESIZE];
#if 0
    void setDiplomatState(BYTE newstate);
    int isDiplomatState(); // 返回0为外交官状态,返回1为非外交官状态,返回2为是外交官但因为有采集手套,暂时无效

    void setCatcherState(BYTE newstate);
    bool isCatcherState() const; 
    // 交战记录操作
    typedef std::vector<WarRecord>::iterator WarIter;

    void addWarRecord(DWORD type,DWORD relationid,bool isAtt=false);
    void removeWarRecord(DWORD type,DWORD relationid=0);
    
    void clearWarRecord()
    {
      vWars.clear();
    }
    
    void setAntiAttState(DWORD type,DWORD relationid);
    //   type,对战状态,relationid对敌方社会关系ID
    bool isWarRecord(DWORD type,DWORD relationid);


    /// 判断是否在财产保护状态
    // 返回TRUE,表示指定的功能,正处在保护状态,返回FALSE表示指定的功能没在保护状态
    bool isSafety(BYTE byType);
    

    // 判断是否与某位玩家处于交战状态
    bool isWar(SceneUser* entry);

    // 判断玩家自己是否处于指定类型的交战状态
    bool isSpecWar(DWORD dwType);

    // 判断是否是攻方
    bool isAtt(DWORD dwType,DWORD relationid=0);

    // 判断是否允许反攻
    bool isAntiAtt(DWORD dwType,DWORD relationid=0);

    // 当前对战记录大小
    size_t warSize()
    {
      return vWars.size();
    }
#endif
    DWORD updateNotify; //hp,mp,sp 更新通知
    BYTE updateCount;  //更新记数
    
    // 答题状态
    bool isQuiz;      // 是否处于答题状态

    // 外交官状态
    bool isDiplomat;

    // 捕头状态
    bool isCatcher;
#if 0

    // 判断是否与某位玩家处于交战状态

    //召唤宠物
	ScenePet *	summonPet(DWORD id,Cmd::petType type,DWORD standTime=0,DWORD sid=0,const char * petName = "",DWORD anpcid = 0,zPos pos=zPos(0,0),BYTE dir=4);
	//[sky]召唤幻影专用函数
	bool		MirageSummonPet(DWORD id,Cmd::petType type,DWORD standTime,WORD num,const char * petName = "",DWORD anpcid = 0,zPos pos=zPos(0,0),BYTE dir=4);
    //[sky] 召唤士兵(每个士兵都是一个独立的个体不同以召唤兽或者宠物)
	int	 summonSoldiers(DWORD id, Cmd::petType type, WORD num, DWORD sid=0, const char * name="", DWORD anpcid = 0,zPos pos=zPos(0,0),BYTE dir=4) {return NULL;}
    //SceneNpc * summonPet(DWORD id,DWORD standTime,DWORD anpcid = 0);
    bool killOnePet(ScenePet *);
    void killAllPets();
    void killSummon();
    void collectPets();
    void clearGuardNpc();
#endif
    Cmd::t_PetData petData;//弓手抓的宠物数据
    DWORD savePetState(BYTE *data);
    DWORD loadPetState(BYTE *data,int size);
    bool saveGuard;///是否保存镖车的临时档案
    bool saveAdopt;///是否保存
    DWORD saveTempPetState(BYTE *data,DWORD maxSize);
    DWORD loadTempPetState(BYTE *data);
	//sky 保存和读取冷却时间的临时档案
	DWORD saveItemCoolTimes(BYTE *data,DWORD maxSize);
	DWORD loadItemCoolTimes(BYTE *data);

    void saveCartoonState();

    DWORD dwChangeFaceID;        //易容后的NPC的类型ID

    enum {
      REST_RESTITUTE_SP = 3,//自动体力恢复
      WALK_RESTITUTE_SP = 1,//走路体力消耗
      RUN_CONSUME_SP = 1,//跑步体力消耗
    };
    WORD step_state; 

    DWORD accid;
    SceneTask *gatetask;
    CharBase charbase;
    CharState charstate;
    //const zCharacterB *charconst;
    WORD wdTire;
    WORD wdTirePer;
    bool sitdownRestitute;
    bool npcdareflag;
    DWORD npcdarePosX;
    DWORD npcdarePosY;
    DWORD npcdareCountryID;
    DWORD npcdareMapID;
    bool npcdareNotify;

    WORD  lastUseSkill; // 最后一次使用的技能
    bool  farAttack; // 箭灵箭侠专用

    //DWORD dwHorseID;
    DWORD dwBodyID;
    DWORD dwLeftHandID;
    DWORD dwRightHandID;
    DWORD dwBodyColorSystem;
    DWORD dwBodyColorCustom;

    char  unionName[MAX_NAMESIZE];          // 帮会名称
    char  septName[MAX_NAMESIZE];           // 家族名称
    char  caption[MAX_NAMESIZE];    // 国王或城主头衔
	char  armyName[MAX_NAMESIZE];    // 所在军队名称
	char  ShopAdv[MAX_SHOPADV];			//SHX 摆摊广告;


    bool  king; // 是否为国王
    bool  unionMaster; // 是否为帮主
    bool  septMaster;  // 是否为族长
    bool  emperor;  // 是否为皇帝
    BYTE  kingConsort; // 0不是国王和皇帝的配偶,1为国王的配偶,2为皇帝的配偶
    
    DWORD dwSeptRepute;  // 家族声望
    DWORD dwSeptLevel;  // 家族等级
    DWORD dwUnionActionPoint;  // 帮会行动力
    DWORD dwArmyState; // 在军队中的职位


    bool answerMarry; ///同意配偶结婚请求标志
    DWORD friendID;   ///结婚时未来配偶的ID
    QuestList quest_list;

    //物品
    Packages packs;
#if 0
    //药品
    Leechdom leechdom;

    QuestList quest_list;
    Mask mask;

    //组队
	/// sky 允许组队标志
	bool isOpen;
	/// sky 加入的队伍的唯一ID
	DWORD TeamThisID;
	//组队模式
    DWORD team_mode;

	/**
	* \brief  是否允许组队
	* \return true 允许组队 false 不允许组队
	*/
	bool IsOpen()
	{
		return isOpen;
	}

	/**
	* \brief  设置允许组队标志
	* \param  is 允许组队标志
	*/
	void setOpen(bool is)
	{
		isOpen = is;
	}

    //交易单
    TradeOrder tradeorder;
    PrivateStore privatestore;
#endif
    int messageOrder;          ///用户上一次收到的Message顺序
    zRTime lastCheckMessage;      ///最后一次检查Message时间
    DWORD  lastMoveTime;
    DWORD   moveFastCount;

    //访问Npc的信息
    DWORD npc_dwNpcDataID;        ///Npc数据编号
    DWORD npc_dwNpcTempID;        ///Npc临时编号

    //ScenePet* guard; //护镖

    struct{
      DWORD dwMapID;
      DWORD dwPosX;
      DWORD dwPosY;
    } npcHoldData;    /// 家族控制的NPC相关数据

    //快捷键数据
    char accelData[1024];

    DWORD myOverMan;
    //找到自己的师父id,没有返回0
    DWORD getOverMan()
    {
      return myOverMan;
    }
    //bool canVisitNpc(SceneNpc *pNpc);
    void visitNpc(const DWORD npcID,const DWORD npcTempID)
    {
      npc_dwNpcDataID = npcID;
      npc_dwNpcTempID = npcTempID;
    }

    bool checkVisitNpc(const DWORD npcID) const
    {
      return npcID == npc_dwNpcDataID;
    }

    void cancelVistNpc()
    {
      npc_dwNpcTempID = 0;
      npc_dwNpcDataID = 0;
    }

    SceneUser(const DWORD accid);

    void setHairType(const BYTE hairtype)
    {
      DWORD temp = hairtype;
      temp <<= 24;
      charbase.hair = (temp & HAIRTYPE_MASK) | (charbase.hair & HAIRRGB_MASK);
    }

    BYTE getHairType() const
    {
      return ((charbase.hair & HAIRTYPE_MASK) >> 24);
    }

    void setHairColor(const DWORD rgb)
    {
      charbase.hair = (rgb & HAIRRGB_MASK) | (charbase.hair & HAIRTYPE_MASK);
    }

    DWORD getHairColor() const
    {
      return (charbase.hair & HAIRRGB_MASK);
    }

    inline bool checkMessageTime(const zRTime &ct)
    {
      if (ct >= lastCheckMessage)
      {
        lastCheckMessage = ct;
        lastCheckMessage.addDelay(2000);
        return true;
      }
      else
        return false;
    }

    bool speedOutMove(DWORD time,WORD speed,int len);

    bool isNewCharBase() const
    {
      return !(CHARBASE_OK & charbase.bitmask);
    }
    bool IsPking()
    {
      return lastPkTime ? true : false;
    }
    bool getMagicType();
    int IsOppose(DWORD five);
    bool IsJoin(DWORD five);
    /*
       bool tenSecondTimer(const zRTime &ct);
       bool oneSecondTimer(const zRTime &ct);
       bool oneMinuteTimer(const zRTime &ct);
     */
    inline bool checkGoodnessTime(const zRTime &ct);
    void checkPunishTime();
    //void reduceGoodness(SceneNpc *pNpc);
    void initCharBase(Scene *intoscene);

    void calReliveWeaknessProperty(bool enter);
    void enterSeptGuard();
    void finishSeptGuard();

    void setupCharBase(bool lock=true);
    bool upgrade(DWORD num = 0);
    void setStateToNine(WORD state);
    void clearStateToNine(WORD state);
    void sendGoodnessToNine();
    void sendMeToNine();
    void sendMeToNineDirect(const int direct);
    void sendNineToMe();
    void sendNineToMeDirect(const int direct);
	void sendDupChangeCmdToGate();
    //void sendPetDataToNine();
    void sendInitToMe();
    void addNineSeptExp(DWORD dwSeptID);
    void addNineSeptNormalExp(DWORD dwSeptID);

    void checkAutoMP();
    bool checkUserCmd(const Cmd::stNullUserCmd *,const DWORD nCmdLen) const;
    bool checkChatCmd(const Cmd::stNullUserCmd *,const DWORD nCmdLen) const;
    void sendCmdToMe(const void *pstrCmd,const DWORD nCmdLen);
    void sendCmdToBill(const void *pstrCmd,const DWORD nCmdLen);
    void sendSceneCmdToBill(const void *pstrCmd,const DWORD nCmdLen);
    bool unreg(bool MoveScene = false);
    bool requestUser(Cmd::stRequestUserDataMapScreenUserCmd *rev);
    bool requestNpc(Cmd::stRequestMapNpcDataMapScreenUserCmd *rev);
    bool save(const Cmd::Record::WriteBack_Type writeback_type, DWORD tozone=0, DWORD secretkey=0, DWORD type=2);
    bool attackMagic(const Cmd::stAttackMagicUserCmd *rev,const DWORD cmdLen);
    bool reliveReady(const Cmd::stOKReliveUserCmd *rev,bool isOrigin = false);
    bool relive(const int relive_type,const int delaytime,const int data_percent=100);
    //void Death();
    void setDeathState();
    void lostObject(SceneUser *pAtt = NULL);
    bool switchPKMode(const Cmd::stPKModeUserCmd *rev);
    bool checkGoodness();    
    char *getGoodnessName();
    short getGoodnessState() const;
    bool isPkAddition();
    DWORD getPkAddition();
    bool isRedNamed(bool allRedMode=true) const;
    float getGoodnessPrice(DWORD price,bool isBuy);
    bool move(Cmd::stUserMoveMoveUserCmd *rev);
    bool backOff(const int dir,const int grids);
    bool goTo(zPos &newPos);
    void jumpTo(zPos &newPos);
    bool goToRandomRect(zPos center,WORD rectx = SCREEN_WIDTH,WORD recty = SCREEN_HEIGHT);
    bool doChatCmd(const Cmd::stChatUserCmd *rev,DWORD cmdLen);
    bool doRelationCmd(const Cmd::stRelationUserCmd *rev,DWORD cmdLen);

    bool ride(Cmd::stRideMapScreenUserCmd *rev);

    bool doPropertyCmd(const Cmd::stPropertyUserCmd *rev,DWORD cmdLen);
    bool doHeroCardCmd(const Cmd::stHeroCardCmd *rev,DWORD cmdLen);
    bool doBattleCmd(const Cmd::stBattleUserCmd *rev,DWORD cmdLen);
    bool useObject(zObject *obj, BYTE useType);
    bool doTradeCmd(const Cmd::stTradeUserCmd *rev,DWORD cmdLen); 
    bool doMailCmd(const Cmd::stMailUserCmd *rev,DWORD cmdLen);
#if 0
    bool useLeechdom(zObject *obj);
    bool useScroll(zObject *obj);
    bool useCallObj(zObject *obj);
    bool useAmulet(zObject *obj);
    bool useSkill(zObject *obj);
    bool doGMCmd(char *gmcmd);
    bool doCountryCmd(const Cmd::stCountryUserCmd *rev,DWORD cmdLen);
    bool doArmyCmd(const Cmd::stArmyUserCmd *rev,DWORD cmdLen);
    bool doDareCmd(const Cmd::stDareUserCmd *rev,DWORD cmdLen);
    bool doNpcDareCmd(const Cmd::stDareUserCmd *rev,DWORD cmdLen);
    bool doUnionCmd(const Cmd::stUnionUserCmd *rev,DWORD cmdLen);
    bool doSafetyCmd(const Cmd::stSafetyUserCmd *rev,DWORD cmdLen);
    bool doSeptCmd(const Cmd::stSeptUserCmd *rev,DWORD cmdLen);
    bool doSchoolCmd(const Cmd::stSchoolUserCmd *rev,DWORD cmdLen);
    bool doRelationCmd(const Cmd::stRelationUserCmd *rev,DWORD cmdLen);
    bool doPetCmd(const Cmd::stPetUserCmd *rev,DWORD cmdLen);
    bool doQuizCmd(const Cmd::stQuizUserCmd *rev,DWORD cmdLen);
    bool doAuctionCmd(const Cmd::stAuctionUserCmd *rev,DWORD cmdLen);
    bool doStockCmd(const Cmd::stStockSceneUserCmd *ptCmd,DWORD cmdLen);
    bool doPrisonCmd(const Cmd::stPrisonUserCmd *rev,DWORD cmdLen);
    bool doMiniGameCmd(const Cmd::stMiniGameUserCmd *rev,DWORD cmdLen);
    bool doCartoonCmd(const Cmd::stCartoonUserCmd *rev,DWORD cmdLen);
#endif

    bool stockSave(DWORD mon,DWORD go);
    void full_t_UserData(Cmd::t_UserData &data);
    void full_t_MapUserData(Cmd::t_MapUserData &data);
    void full_t_MapUserDataPos(Cmd::t_MapUserDataPos &data);
    void full_t_MapUserDataState(Cmd::t_MapUserDataState &data);
    void full_t_MapUserDataPosState(Cmd::t_MapUserDataPosState &data);
    void full_t_MainUserData(Cmd::t_MainUserData &data) const;
    void sendInitHPAndMp();
    bool addSkillData(const Cmd::stAddUserSkillPropertyUserCmd *rev);
    bool removeSkill(const Cmd::stRemoveUserSkillPropertyUserCmd *rev);
    bool upgradeSkill(DWORD dwSkillID,bool needSkillPoint=true);
    bool changeMap(Scene *newscene,const zPos &pos,bool ignoreUserLevel= false);
    bool intoScene(Scene *newscene,bool needInitInfo,const zPos &initPos);
    void setDeathBackToMapID(Scene *s);
    int saveSysSetting(BYTE*);
    int loadSysSetting(BYTE*);
    bool LeaveScene();
	//bool userEnterDup(unsigned short _dupIndex,DWORD mapid,userDupMap *_userDupMap);
	//void userLeaveDup();
    bool refreshMe();
    void sendExpToSept(const WORD &exp);
    void dreadProcess();				//sky 控制恐惧的胡乱移动
	void blindProcess();				//sky 控制失明的胡乱移动
    WORD getMyMoveSpeed() const;
    bool unCombin(Cmd::stUnCombinUserCmd *rev);
    void setNpcHoldData(Cmd::Session::t_notifyNpcHoldData *rev);
    inline void checkNpcHoldDataAndPutExp();
    void addObjectToUserPacket(int objectid,SceneUser *pUser);
    void catchMyPet();
    void removeNineEntry(zPosI posi);

    void addExp(DWORD,bool=true,DWORD dwTempID=0,BYTE byType=0,bool=false);
    void addPetExp(DWORD,bool=true,bool=false);

	//sky 检测玩家家的单前的战斗状态
	useFightState IsPveOrPvp();
	//sky 设置角色战斗状态
	void SetPveOrPvp(useFightState type); //设置pve或者Pvp状态
	//sky 设置角色战斗状态的时间
	int PkTime;					//sky 保存玩家进入战斗的时间
	void SetPkTime(DWORD time=10);		//sky 重新设置玩家进入战斗的时间
	bool IsPkTimeOver();			//sky 判断是否玩家能脱离战斗

#if 0
	//sky 使用物品触发技能函数
	bool ItemUseSkill(zObject * obj, DWORD preUserID=0);

	//sky 新的物品冷却管理器
	CItmeCoolTime m_ItemCoolTimes;

	//sky 检测是否超过设定的随机范围
	virtual bool outOfRandRegion(zPos * pos);

	//sky 新增战场类数据对象
	stUserBattfieldData BattfieldData;
	DWORD saveBattfieldData(BYTE * data);
	DWORD loadBattfieldData(BYTE * data);

    //void petLevelUp(ScenePet *);
    /**
     * \brief 设置战斗类宠物的跟踪攻击目标
     */
    void setPetsChaseTarget(SceneEntryPk *entry);
	bool doBoxCmd(const Cmd::stCowBoxUserCmd *ptCmd,DWORD cmdLen);
	bool doTurnCmd(const Cmd::stTurnUserCmd *ptCmd,DWORD cmdLen);
	bool doReMakeObjCmd(const Cmd::stReMakUserCmd *ptCmd,DWORD cmdLen);

	// [sky] 训马指令处理函数
	bool doHorseTrainingCmd(const Cmd::stReMakUserCmd *ptCmd,DWORD cmdLen);

	//[sky] 装备自由加点处理函数
	bool doAddItemAttCmd(const Cmd::stAttruByteUserCmd *ptCmd,DWORD cmdLen);
	//[sky] 宝石镶嵌处理函数
	bool doMosaicGenCmd(const Cmd::stMakeObjectUserCmd *ptCmd,DWORD cmdLen);
	//[sky] 战场竞技场相关处理函数 begin
	bool doArenaCmd(const Cmd::stArenaUserCmd *ptCmd, DWORD cmdLen);
	//[sky] 战场竞技场相关处理函数 end
	zObject * GetObjectBydst( stObjectLocation * dst );			//根据格子位置获取格子里的物品指针
#endif
    /**
     * \brief 根据箭支的变化重算攻击属性
     * \calcflag 标志是否在函数内自行计算并通知
     * \return true为需要重算,false为不需要重算
     */
    //bool recalcBySword(bool calcflag = true);

    bool isAllied(SceneUser *pUser);

    /**
     * \brief 计算组队中各种社会关系的友好度
     */
    inline void countFriendDegree();

    /**
     * \brief 将客户端消息转发到会话服务器
     */
    bool forwardSession(const Cmd::stNullUserCmd *pNullCmd,const DWORD nCmdLen);

    void full_stRTMagicPosUserCmd(Cmd::stRTMagicPosUserCmd &ret) const
    {
      ret.pos.dwTempID=tempid;
      ret.pos.byDir=getDir();
      ret.pos.x=getPos().x;
      ret.pos.y=getPos().y;
      ret.byTarget=Cmd::MAPDATATYPE_USER;
    }

    /**
     * \brief 向包裹中添加物品
     *
     * \param srcObj 目标物品
     * \param needFind 需要查找位置
     * \param from_record 
     * \param calcflag
     *
     * \return 失败返回false,否则返回true
     */
    //bool packsaddObject(zObject *srcObj,bool needFind,bool from_record = false,bool calcflag = true);

    /**
     * \brief 根据物品id调整物品在包裹中的数量
     *
     * \param id 物品的objectid
     * \param num 减少的数量
     * \param upgrade 升级
     *
     *
     * \return 失败返回-1,否则返回0
     */
    int reduceObjectNum(DWORD id,DWORD num,BYTE upgrade = 0);

    /**
     * \brief 根据物品id调整物品在包裹中的数量,如果大于该物品最大数量则创建一个新物品
     *
     * \param id 物品的objectid
     * \param num 增加的数量
     * \param upgrade 升级
     * \param notify 通知
     * \param bindit 是否绑定
     *
     *
     * \return 失败返回-1,没有物品被创建返回0,包裹满返回1,成功添加所有创建的物品返回2
     */
    int addObjectNum(DWORD id,DWORD num,BYTE upgrade = 0,int notify = 0,bool bindit = false);


    /**
     * \brief 根据物品id调整物品在包裹中的数量,如果大于该物品最大数量则创建一个新物品（任务接口）加绿装绑定装备
     *
     * \param id 物品的objectid
     * \param num 增加的数量
     * \param upgrade 升级
     * \param notify 通知
     * \param bindit 是否绑定
     *
     *
     * \return 失败返回-1,没有物品被创建返回0,包裹满返回1,成功添加所有创建的物品返回2
     */
    int addGreenObjectNum(DWORD id,DWORD num,BYTE upgrade = 0,int notify = 0,bool bindit = true);


    /**
     * \brief 自动恢复用户相关属性,包括生命,魔法,体力等,该函数在UpdateSceneUserCallBack中被调用
     */
    inline void restitute();
    DWORD autoRestitute(DWORD &updated);

    /**
     * \brief 更新用户数据到会话
     */
    void updateUserData();

    /**
     * \brief 改变角色的hp
     *
     * \param hp 变更的HP
     *
     */
    void changeHP(const SDWORD &hp);

    /**
     * \brief 直接伤害
     *
     * \param pAtt 攻击者
     * \param dam 伤害值
     * \param notify 通知伤害显示
     */
    SWORD directDamage(SceneEntryPk *pAtt,const SDWORD &dam,bool notify=false);

    /**
     * \brief 改变角色的sp
     * \param sp 变更的SP
     */
    void changeSP(const SDWORD &sp);

    /**
     * \brief 改变角色的mp
     * \param mp 变更的MP
     */
    void changeMP(const SDWORD &mp);

    /**
     * \brief 获得最大的hp
     * \return 返回最大值
     */
    DWORD getMaxHP();

    /**
     * \brief 获得最大的hp
     * \return 返回最大值
     */
    DWORD getBaseMaxHP();

    /**
     * \brief 获得最大的mp
     * \return 返回最大值
     */
    DWORD getMaxMP();

    /**
     * \brief 获得最大的mp
     * \return 返回最大值
     */
    DWORD getBaseMaxMP();
    /**
     * \brief 重算并通知
     */
    void changeAndRefreshHMS(bool lock=true,bool sendData=true);
    /**
     * \brief 获得当前魔法攻击力
     * \return 魔法攻击力
     */
    virtual DWORD getMaxMDamage();

    /**
     * \brief 获得当前物理攻击力
     * \return 物理攻击力
     */
    virtual DWORD getMaxPDamage();

    /**
     * \brief 获得当前物理防御力
     * \return 物理防御力
     */
    virtual DWORD getPDefence();

    /**
     * \brief 获得当前魔法防御力
     * \return 魔法防御力
     */
    virtual DWORD getMDefence();

    /**
     * \brief 在自己被攻击之前的处理,包括,装备损耗处理,攻击有效几率判断等
     * \param pUser 攻击者
     * \param rev 本次攻击的触发指令
     * \param physics
     * \param good
     * \return true为这次攻击是有效的,false为一次无效的攻击
     */
    bool preAttackMe(SceneEntryPk *pUser,const Cmd::stAttackMagicUserCmd *rev,bool physics=true,const bool good = false);

    /**
     * \brief 角色被攻击
     * \param pEntry 攻击者
     * \param rev 本次攻击的触发消息
     * \return true为这次攻击是有效的,false为一次无效的攻击
     */
    //    virtual bool AttackMe(SceneEntryPk *pEntry,const Cmd::stAttackMagicUserCmd *rev,bool physics=true);

    /**
     * \brief 让角色重生
     */
    virtual void relive();

    /**
     * \brief 角色被击退N格
     * \param dwAttTempID 攻击者的临时ID
     * \param grids
     */
    void standBack(const DWORD dwAttTempID,DWORD grids);

    /**
     * \brief 读取临时存档数据
     * \param revData 得到的临时数据
     * \param dwSize 临时数据的大小
     */
    void setupTempArchive(const char *revData,const DWORD dwSize);

    DWORD setupBinaryArchive(const char *revData);
    bool needSaveBinayArchive(BinaryArchiveType type);
    DWORD addBinaryArchiveMember(DWORD type,char **out,DWORD maxSize);
    DWORD saveBinaryArchive(BYTE *out,const int maxsize);
    /**
     * \brief 让角色死亡
     */
    void toDie(const DWORD &dwTempID);

    /**
     * \brief 屏幕内随机移动
     */
    void goToRandomScreen();

    /**
     * \brief 通知客户端生命值的变化
     */
    void attackRTHpAndMp();

    /**
     * \brief 判断角色是否死亡
     * \return true为死亡
     */
    bool isDie();

    /**
     * \brief 获取角色的级别
     */
    DWORD getLevel() const;

    /**
     * \brief 获取角色的五行类型
     * \return 五行类型
     */
    DWORD getFiveType() const;

    /**
     * \brief 获取角色的五行点数
     * \return 五行点数
     */
    DWORD getFivePoint() const;

    /**
     * \brief 获取角色的五行级别
     * \return 五行级别
     */
    DWORD getFiveLevel() const;

    /**
     * \brief 需要的职业类型,决定可以使用的技能类型
     */
    //bool addSkillToMe(zSkill *skill);

    /**
     * \brief 需要的职业类型,决定可以使用的技能类型
     */
    bool needType(const DWORD &needtype);

    /**
     * \brief 是否有该技能需要的武器
     * \return true 有 false 没有
     */
    bool needWeapon(DWORD skillid);

    /**
     * \brief 获取当前手持的武器类型
     * \return 武器类型
     */
    BYTE getWeaponType();

    /**
     * \brief 是否Pk区域
     * \param other 相关人
     * \return true 是 false 否
     */
    bool isPkZone(SceneEntryPk *other=NULL);

    /**
     * \brief 依赖物品消耗型法术
     * \param object 消耗物品的类型
     * \param num 消耗物品的数量
     * \return true 消耗成功 false 失败
     */
    bool reduce(const DWORD &object,const BYTE num);

    bool checkReduce(const DWORD &object,const BYTE num);
#if 0
    /**
     * \brief 施放技能所导致的消耗MP,HP,SP
     * \param base 技能基本属性对象
     * \return true 消耗成功 false 失败
     */
    bool doSkillCost(const zSkillB *base);

    /**
     * \brief 检查施放技能所导致的消耗MP,HP,SP是否足够
     * \param base 技能基本属性对象
     * \return true 消耗成功 false 失败
     */
    bool checkSkillCost(const zSkillB *base);
#endif
    /**
     * \brief 检查自身的施放成功几率,决定这次技能是否可以施放
     * \return true 成功 false 失败
     */
    bool checkPercent();

    /**
     * \brief 判断是否是敌人
     * \return true 是 false 不是
     */
    int isEnemy(SceneEntryPk *entry,bool notify = false,bool good = false);

    /**
     * \brief 获取自己的主人,一般针对NPC而言,Player的主人是自己
     * \return NULL或者主人的对象指针
     */
    SceneEntryPk *getMaster();
    /**
     * \brief 得到最上层的主人
     * 返回自己
     * \return  自己
     */
    SceneEntryPk *getTopMaster();// const;

    /**
     * \brief 检查是否在禁言期
     * 
     * \return 如果在禁言期返回true,否则false
     */ 
    bool isForbidTalk()
    {
      zRTime ctv;
      return ctv.sec() < charbase.forbidtalk;
    }
    /** 
     * \brief 设置禁言时间
     * \param delay 延迟的秒数
     * \return 禁言到期的时间()
     */
    zRTime delayForbidTalk(int delay)
    {
      zRTime ctv(delay*1000);
      charbase.forbidtalk = ctv.sec();
      return ctv;
    }

    /**
     * \brief 设置玩家的权限
     * \param lv 等级
     */
    void setPriv(BYTE lv)
    {
      priv = lv;
    }

    /**
     * \brief 得到玩家的权限等级
     * \return 玩家的权限等级
     */
    BYTE getPriv()
    {
      return priv;
    }

    /**
     * \brief 重新发送本角色的地图数据
     */
    void reSendMyMapData();

    /**
     * \brief 通知选中自己的用户的hp和mp发生变化
     */
    //void sendtoSelectedHpAndMp();
    void sendtoSelectedPkAdditionState();
    void sendtoSelectedPkAdditionStateToUser(SceneUser *pUser);
    void sendtoSelectedReliveWeakStateToUser(SceneUser *pUser);
    void sendtoSelectedReliveWeakState();
    void sendtoSelectedTrainStateToUser(SceneUser *pUser);
    void sendtoSelectedTrainState();


    /**
     * \brief 获取抗毒增加  
     */
    SWORD getPoisondef();

    /**
     * \brief 获取抗麻痹增加        
     */
    SWORD getLulldef();

    /**
     * \brief 获取抗眩晕增加        
     */
    SWORD getReeldef();

    /**
     * \brief 获取抗噬魔增加        
     */
    SWORD getEvildef();

    /**
     * \brief 获取抗噬力增加        
     */
    SWORD getBitedef();

    /**
     * \brief 获取抗混乱增加        
     */
    SWORD getChaosdef();

    /**
     * \brief 获取抗冰冻增加        
     */
    SWORD getColddef();

    /**
     * \brief 获取抗石化增加        
     */
    SWORD getPetrifydef();

    /**
     * \brief 获取抗失明增加        
     */
    SWORD getBlinddef();

    /**
     * \brief 获取抗定身增加        
     */
    SWORD getStabledef();

    /**
     * \brief 获取抗减速增加        
     */
    SWORD getSlowdef();

    /**
     * \brief 获取抗诱惑增加
     */
    SWORD getLuredef();


    /**
     * \brief 获取毒增加  
     */
    SWORD getPoison();

    /**
     * \brief 获取麻痹增加        
     */
    SWORD getLull();

    /**
     * \brief 获取眩晕增加        
     */
    SWORD getReel();

    /**
     * \brief 获取噬魔增加        
     */
    SWORD getEvil();

    /**
     * \brief 获取噬力增加        
     */
    SWORD getBite();

    /**
     * \brief 获取混乱增加        
     */
    SWORD getChaos();

    /**
     * \brief 获取冰冻增加        
     */
    SWORD getCold();

    /**
     * \brief 获取石化增加        
     */
    SWORD getPetrify();

    /**
     * \brief 获取失明增加        
     */
    SWORD getBlind();

    /**
     * \brief 获取定身增加        
     */
    SWORD getStable();

    /**
     * \brief 获取减速增加        
     */
    SWORD getSlow();

    /**
     * \brief 获取诱惑增加
     */
    SWORD getLure();



    /**
     * \brief 发送选中玩家的状态
     * \param state 状态
     * \param value 值
     * \param time 持续时间
     * */
    //void sendtoSelectedState(DWORD state,WORD value,WORD time);
    /**
     * \brief 发送玩家的状态给自己
     * \param state 状态
     * \param value 值
     * \param time 持续时间
     * */
    void sendSevenStateToMe(DWORD state,WORD value,WORD time);

    /**
     * \brief 获取装备伤害加成
     * \return 伤害加成
     */
    //WORD getDamageBonus();

    /**
     * \brief 设置战斗类宠物的跟踪攻击目标
     * \param weaponType 武器类型
     * \return true武器类型符合,false武器类型不符合
     */
    bool checkWeapon(BYTE weaponType);

    /*
    ///脱离战斗状态的时间
    zRTime endBattleTime;
    ///当前的对手
    SceneEntryPk * curTarget;

    void setEndBattleTime(const zRTime &,int);
    bool checkEndBattleTime(const zRTime &);
    void leaveBattle();
     */


    // 脱离队伍
    void leaveTeam();

    /**
     * \brief 判断是否可以回收用户内存
     * \param cur: 当前时间
     * \return 是否可以回收内存
     * */
    bool canRecycle(const zRTime &cur)
    {
      return recycle_delay <= cur;
    }

    /**
     * \brief 易容处理
     * \param cmd 易容消息
     * \param cmdLen 消息长度
     * \return true 处理成功 false 失败
     **/
    bool changeFace(const Cmd::stChangeFaceMapScreenUserCmd *cmd,const DWORD cmdLen);

    /**
     * \brief 武器提升对应技能等级
     * \param skilltype 技能的类型
     * \param skillkind 技能的系别
     * \return 提升的技能登记数
     **/
    WORD skillUpLevel(WORD skilltype,WORD skillkind);

    /**
     * \brief 检查是否骑马
     * \return true 骑马 false 没骑
     */
    bool checkMountHorse();
    inline void initTire();
    void  initAnswerCount();
    inline void processTire();
    void callProcessTire();
    inline void processAnswerCount();
    void sitdown();
    void standup();
    bool isSitdown();
    void getSummonAppendDamage(WORD &minDamage,WORD &maxDamage);
    void exchangeMeAndSummonPet();
    DWORD getMP();
    void clearMana();
    SceneEntryPk* getSceneEntryPk(DWORD entryType,DWORD entryID) const;
    //void appendAttack(const Cmd::stAttackMagicUserCmd *rev);
    //bool captureIt(SceneNpc *,BYTE);
    //void leech(DWORD dwDam);
    //void checkNpcDare(Cmd::Session::t_NpcDare_NotifyScene_SceneSession * rev);
    //void checkNpcDareState();
    //void notifySessionNpcDareResult();
    //void npcDareGetGold(DWORD dwGold);
    void sendMessageToMe(const char *pattern);
    //void reflectSkill(SceneEntryPk *pAtt,const Cmd::stAttackMagicUserCmd *rev);
    SWORD getWeaponPower(int powerkind);
    //void relivePet();
    void sendSkill(WORD wdSkillID,BYTE level=1,DWORD target=0,BYTE attackType=Cmd::ATTACKTYPE_U2U,BYTE action=Cmd::Ani_Null);
    /**
     * \brief 设置角色的当前状态,并根据当前状态呈现角色的特效或者关闭特效
     * \param state 状态ID 根据enum SceneEntryState取值
     * \return true为这次攻击是有效的,false为一次无效的攻击
     */
    void showCurrentEffect(const WORD &state,bool isShow,bool notify=true);

  private:
#if 0
    /**
     * \brief 处于蒙面状态时攻击别人发出系统提示
     * \param entry 攻击对象
     * */
    void processMaskOnAttack(SceneEntryPk *entry);
    /**
     * \brief 处于蒙面状态时被攻击
     * */
    void processMaskOnDefence();
    /**
     * \brief 计算伤害累加值
     * 
     * \param dwDam 输出,伤害值
     * \param dwDamDef 防御力
     * \param physics 是否物理攻击
     * \return
     * */
    void processAddDam(int &dwDam,int &dwDamDef,bool physics);
    /**
     * \brief 计算减少伤害
     * \param dwDam 伤害值
     * \param dwDamDef 防御力
     * \param 是否物理攻击
     * 
     * */
    void reduceDam(int &dwDam,int &dwDamDef,bool physics);
    /**
     * \brief 计算伤害反弹
     * \param dwDam 伤害值
     * \param dwDamDef 防御力
     * \param 是否物理攻击
     * */
    void reflectDam(int &dwDamDef,int &dwDamSelf,DWORD skillID);
    /**
     * \brief hp到mp转换
     * \param dwDamDef 防御力
     * 
     * */
    void hp2mp(int &dwDamDef);
#endif
    bool processDeath(SceneEntryPk *pAtt);
    /**
     * \brief 计算预处理值
     * 
     * */
    void calPreValue();
    /**
     * \brief 设置魅力值
     * 
     * */
    void setupCharm();
    /**
     * \brief 保存玩家临时数据
     * 
     * */
    void saveTempArchive();
    /**
     * \brief 存储玩家临时文档
     * \param type 临时数据类型
     * \param out 保存数据开始的位置
     * \param maxSize 最大容量
     * \return 增加的字节
     * */
    DWORD addTempArchiveMember(DWORD type,char *out,DWORD maxSize);
    /**
     * \brief 检查某类型数据是否需要保存到临时文档
     * \param 要保存的数据类型
     * \return 是否需要保存
     * */
    bool needSaveTempArchive(TempArchiveType type);

    zRTime& waitRecycle();
  public:
    CounterManager cm;

    AllHeroData ahData; 
    CardTujianData tujianData;
    GroupCardData groupcardData;
    ChallengeTmpData ctData;
    
    void sendAllMobileObjectList();
    bool notifyUnfinished;
#if 0
    void notifyEquipChange();

    bool do_trade_rs_cmd(const Cmd::stTradeUserCmd *rev,DWORD cmdLen);

        bool processBackdoor(Cmd::stChannelChatUserCmd *pCmd,DWORD cmdLen);


	//Shx Add 装备中套装附加的属性;
	stIncrementBySuit Increment;
	/**
	* \ 计算装备中套装附加的属性
	* \param type 临时数据类型
	* \param out 保存数据开始的位置
	* \param maxSize 最大容量
	* \return 增加的字节
	* */
	void GetIncrementBySuit();
	//End shx
#endif
};


extern void sendMail(char * fromName,DWORD fromID,char * toName,DWORD toID,BYTE type,DWORD money/*,zObject * o=NULL*/,char * text);
#endif

