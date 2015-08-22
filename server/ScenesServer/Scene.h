#ifndef _Scene_h_
#define _Scene_h_

#include "LoadMap.h"
#include "zScene.h"
#include "zSceneEntryIndex.h"
#include "zXMLParser.h"
#include "WayPoint.h"
#include "SceneDefine.h"
#include "zTime.h"
#include "zDatabase.h"
#include "SceneEntryPk.h"
#include "zObject.h"
#include "SceneNpc.h"

enum
{
	MAP_FUNCTION_NORIDE		= 0x1,	//sky 不可以骑坐骑
	MAP_FUNCTION_CAPITAL	= 0x2,	//sky 首都
	MAP_FUNCTION_MAZE		= 0x4,	//sky 地下城
	MAP_FUNCTION_NOSCROLL	= 0x8,	//sky 不可以使用卷轴
	MAP_FUNCTION_NORED		= 0x10,	//sky 不红名
	MAP_FUNCTION_NOVICE		= 0x20,	//sky 新手保护
	MAP_FUNCTION_NOTEAM		= 0x40, //sky 不可以组队
	MAP_FUNCTION_NOCALLOBJ	= 0x80, //sky 不可以使用令牌
	MAP_FUNCTION_BATTLEFIEL	= 0x100,//sky 战场类地图

};

class SceneNpc;

/**
 * \brief 地图场景
 */
class Scene:public zScene,public zSceneEntryIndex
{

  protected:

    Scene();

  public:

    //由Session控制刷新的npc
    //std::map<DWORD,SceneNpc *> bossMap;

    /**
     * \brief 场景类型定义
     */
    enum SceneType
    {
      STATIC,   /// 静态地图
      GANG    /// 动态地图
    };

    virtual ~Scene();
    virtual bool save() =0;

    bool init(DWORD countryid,DWORD mapid);
    bool initByTemplet(DWORD mapID, char* mapName, Scene *templet);
    void final();
    void freshGateScreenIndex(SceneUser *pUser,const DWORD screen);

    bool sendCmdToWatchTrap(const zPosI screen,const void *pstrCmd,const int nCmdLen);
    bool sendCmdToNine(const zPosI screen,const void *pstrCmd,const int nCmdLen,unsigned short dupIndex = 0);
    bool sendCmdToNineUnWatch(const zPosI screen,const void *pstrCmd,const int nCmdLen);
    bool sendCmdToScene(const void *pstrCmd,const int nCmdLen,unsigned short dupIndex = 0);
    bool sendCmdToNineExceptMe(zSceneEntry *pEntry,const zPosI screen,const void *pstrCmd,const int nCmdLen);
    bool sendCmdToDirect(const zPosI screen,const int direct,const void *pstrCmd,const int nCmdLen,unsigned short dupIndex = 0);
    bool sendCmdToReverseDirect(const zPosI screen,const int direct,const void *pstrCmd,const int nCmdLen,unsigned short dupIndex = 0);
    bool findEntryPosInNine(const zPos &vpos,zPosI vposi,zPosVector &range);
    bool findEntryPosInOne(const zPos &vpos,zPosI vposi,zPosVector &range);
    int summonNpc(const t_NpcDefine &define,const zPos &pos,zNpcB *base,unsigned short dupIndex = 0);
    template <typename Npc>
    Npc* summonOneNpc(const t_NpcDefine &define,const zPos &pos,zNpcB *base,DWORD standTime=0,zNpcB* abase = NULL,BYTE vdir=4, SceneEntryPk * m = NULL );
    bool refreshNpc(SceneNpc *sceneNpc,const zPos & newPos);

    DWORD realMapID;
    void setMapID(DWORD i, DWORD realID);

    BYTE dynMapType;
    /**
     * \brief 获取重生地图
     * \return 重生地图ID
     */
    DWORD backtoMap() const { return backtoMapID; }
    /**
     * \brief 获取回城地图
     * \return 回城地图ID
     */
    DWORD backtoCityMap() const { return backtoCityMapID; }
    /**
     * \brief 允许相互pk的等级
     * \return 是true
     */
    DWORD getPkLevel() const { return pklevel; }
    /**
     * \brief 检测该地图是否可以骑马
     * \return 可骑马时true
     */
    bool canRide() const { return !(function & MAP_FUNCTION_NORIDE); }
    /**
     * \brief 不能使用卷轴
     * \return 是主城返回true
     */
    bool canUserScroll() const { return !(function & MAP_FUNCTION_NOSCROLL); }
    /**
     * \brief 检测该地图是否可以组队
     * \return 可组队时true
     */
    bool noTeam() const { return (function & MAP_FUNCTION_NOTEAM); }
    /**
     * \brief 检测该地图是否可以使用令牌
     * \return 是否可用
     */
    bool checkCallObj() const { return (function & MAP_FUNCTION_NOCALLOBJ); }
    /**
     * \brief 是否是主城
     * \return 是主城返回true
     */
    bool isMainCity() const { return (function & MAP_FUNCTION_CAPITAL); }
    /**
     * \brief 是否是地洞类
     * \return 是主城返回true
     */
    bool isField() const { return (function & MAP_FUNCTION_MAZE); }
    /**
     * \brief sky 修改为新手保护地图
     * \return 是新手保护地图返回true
     */
    bool isNovice() const { return (function & MAP_FUNCTION_NOVICE); }
    /**
     * \brief 是否是不红名地图
     * \return 是true
     */
    bool isNoRedScene() const { return (function & MAP_FUNCTION_NORED); }
    /**
     * \brief 是否是PK地图
     * \return 是true
     */
    bool isPkMap() const { return getRealMapID()>=213 && getRealMapID()<=215; }
    /**
     * \brief 获取地图编号
     * \return 地图编号
     */
    const DWORD getRealMapID() const { return id & 0x0000FFFF; }
    /**
     * \brief 获取地图所属国家
     * \return 地图所属国家
     */
    const DWORD getCountryID() const { return countryID; }

    const char *getCountryName() const;

    /**
     * \brief 获取地图名称
     * \return 地图名称(用于服务间)
     */
    const char *getName() const { return name; }
    /**
     * \brief 获取地图名称
     * \return 地图名称(未进行组合的名称)
     */
    const char *getRealName() const
    {
      char *real = const_cast<char*>(strstr(name,"·"));
      if (real != NULL)
        return real + 2;
      else
        return name;
    }
    /**
     * \brief 获取地图文件名称
     * 名称不包括前缀
     * \return 地图文件名称(用于服务间)
     */
    const char *getFileName() const { return fileName.c_str(); }
    /**
     * \brief 获取地图文件名称
     * 名称不包括前缀
     * \return 地图文件名称(为进行组合的名称)
     */
    const char *getRealFileName() const { return fileName.c_str() + fileName.find(".") + 1; }
    /**
     * \brief 获取指定地图文件名称
     * 名称不包括前缀
     * \param file 文件名
     * \return 地图文件名称(为进行组合的名称)
     */
    const char *getRealFileName(std::string file) const { return file.c_str() + file.find(".") + 1; }
    /**
     * \brief 检查坐标阻挡信息
     * \param pos 坐标
     * \param block 阻挡标记
     * \return 是否阻挡点
     */
    const bool checkBlock(const zPos &pos,const BYTE block) const
    {
      if (zPosValidate(pos))
        return (0 != (allTiles[pos.y * width() + pos.x].flags & block));
      else
        return true;
    }
    /**
     * \brief 检查坐标阻挡信息
     * \param pos 坐标
     * \return 是否阻挡点
     */
    const bool checkBlock(const zPos &pos) const { return checkBlock(pos,TILE_BLOCK | TILE_ENTRY_BLOCK); }
    /**
     * \brief 设置目标阻挡点标记
     * \param pos 坐标
     * \param block 阻挡标记
     */
    void setBlock(const zPos &pos,const BYTE block)
    {
      if (zPosValidate(pos))
        allTiles[pos.y * width() + pos.x].flags |= block;
    }
    /**
     * \brief 设置目标阻挡点标记
     * \param pos 坐标
     */
    void setBlock(const zPos &pos) { setBlock(pos,TILE_ENTRY_BLOCK); }

	// [ranqd] 设置区域阻挡点标记
	void setBlock(const zZone &zone,const BYTE block)
	{
		for(DWORD x = zone.pos.x; x < zone.pos.x + zone.width; x++)
		{
			for(DWORD y = zone.pos.y; y < zone.pos.y + zone.height; y++)
			{
				zPos pos;
				pos.x = x;
				pos.y = y;
				if (zPosValidate(pos))
					allTiles[pos.y * width() + pos.x].flags |= block;
			}
		}
	} 

	void setBlock(const zZone &pos) { setBlock(pos,TILE_ENTRY_BLOCK); }
    /**
     * \brief 清除目标阻挡点标记
     * \param pos 坐标
     * \param block 阻挡标记
     */
    void clearBlock(const zPos &pos,const BYTE block)
    {
      if (zPosValidate(pos))
        allTiles[pos.y * width() + pos.x].flags &= ~block;
    }
    /**
     * \brief 清除目标阻挡点标记
     * \param pos 坐标
     */
    void clearBlock(const zPos &pos) { clearBlock(pos,TILE_ENTRY_BLOCK); }
    /**
     * \brief 检查坐标阻挡信息
     * 主要在丢物品的时候使用
     * \param pos 坐标
     * \return 是否阻挡点
     */
    const bool checkObjectBlock(const zPos &pos) const { return checkBlock(pos,TILE_BLOCK | TILE_OBJECT_BLOCK); }
    /**
     * \brief 设置目标阻挡点标记
     * 主要在丢物品的时候使用
     * \param pos 坐标
     */
    void setObjectBlock(const zPos &pos) { setBlock(pos,TILE_OBJECT_BLOCK); }
    /**
     * \brief 清除目标阻挡点标记
     * 主要在丢物品的时候使用
     * \param pos 坐标
     */
    void clearObjectBlock(const zPos &pos) { clearBlock(pos,TILE_OBJECT_BLOCK); }
    /**
     * \brief 获取地表数据
     * \param pos 坐标
     * \return 返回地表数据
     */
    const Tile* getTile(const zPos &pos) const
    {
      if (zPosValidate(pos))
        return &allTiles[pos.y * width() + pos.x];
      else
        return NULL;
    }
    /**
     * \brief 根据位置得到路点
     * \param pos 要查找的位置
     * \return 找到的路点,失败返回0
     */
    WayPoint *getWayPoint(const zPos &pos) { return wpm.getWayPoint(pos); }
    /**
     * \brief 根据目标找到路点
     * \param filename 目标地图文件名
     * \return 找到的路点,失败返回0
     */
    WayPoint *getWayPoint(const char *filename) { return wpm.getWayPoint(filename); }
    /**
     * \brief 随机选一个路点
     * \return 找到的路点
     */
    WayPoint *getRandWayPoint() { return wpm.getRandWayPoint(); }
    /**
     * \brief 返回地图上的人数
     * \return 一张地图上的人数
     */
    const DWORD countUser() const { return userCount; }
    /**
     * \brief 人数增加
     * \return 增加后的人数
     */
    const DWORD addUserCount() { return ++userCount; }
    /**
     * \brief 人数减少
     * \return 减少后的人数
     */
    const DWORD subUserCount() { return --userCount; }

    bool findPosForObject(const zPos &pos,zPos &findedPos);
    bool findPosForUser(const zPos &pos,zPos &findedPos);
    SceneUser *getSceneUserByPos(const zPos &pos,const bool bState = true,const zSceneEntry::SceneEntryState byState = zSceneEntry::SceneEntry_Normal);
    SceneUser *getUserByID(DWORD userid);
    SceneUser *getUserByTempID(DWORD usertempid);
    SceneUser *getUserByName(const char *username);
    SceneNpc *getSceneNpcByPos(const zPos &pos,const bool bState = true,const zSceneEntry::SceneEntryState byState = zSceneEntry::SceneEntry_Normal);
    SceneNpc *getNpcByTempID(DWORD npctempid);
    zSceneObject *getSceneObjectByPos(const zPos &pos);

    bool addObject(unsigned short dupIndex,zObject *ob,const zPos &pos,const unsigned long overdue_msecs=0,const unsigned long dwID=0,int protime=10);
    bool addObject(unsigned short dupIndex,zObjectB *ob,const int num,const zPos &pos,const unsigned long dwID=0,DWORD npc_mul=0, DWORD teamID=0);
    void removeUser(SceneUser *so);
    void removeObject(zSceneObject *so);
    void removeNpc(SceneNpc *sn);
    bool checkZoneType(const zPos &pos,const int type) const;
    int getZoneType(const zPos &pos) const;
    bool randzPosByZoneType(const int type,zPos &pos) const;
    int changeMap(SceneUser *pUser,bool deathBackto=true,bool ignoreWar=false);
    bool randzPosByZoneType(const int type,zPos &pos,const zPos orign);
    bool randzPosOnRect(const zPos &center,zPos &pos,WORD rectx = SCREEN_WIDTH,WORD recty = SCREEN_HEIGHT) const;

    bool getNextPos(int &side,const int direct,const zPos &orgPos,const int clockwise,zPos &crtPos) const;
    void getNextPos(const zPos &orgPos,const int dir,zPos &newPos) const;
    bool SceneEntryAction(const zRTime& ctv,const DWORD group);
    void removeSceneObjectInOneScene();
#if 0
    bool addRush(Rush *);
    void processRush();
#endif
    void setUnionDare(bool flag){this->isUnionDare = flag;}
    bool getUnionDare() const { return this->isUnionDare; }
    void setHoldUnion(DWORD dwUnionID) { this->dwHoldUnionID = dwUnionID; }
    DWORD getHoldUnion() const { return this->dwHoldUnionID; }
    void setHoldCountry(DWORD dwCountryID) { this->dwHoldCountryID = dwCountryID; }
    DWORD getHoldCountry() const { return this->dwHoldCountryID; }
    DWORD getCommonCountryBacktoMapID() const { return this->commonCountryBacktoMapID; }
    DWORD getForeignerBacktoMapID() const { return this->foreignerBacktoMapID; }
    DWORD getCountryDareBackToMapID() const { return this->countryDareBackToMapID; }
    DWORD getCountryDefBackToMapID() const { return this->countryDefBackToMapID; }
    DWORD getCommonUserBacktoMapID() const { return this->commonUserBacktoMapID; }
    void setCountryDare(bool flag) { this->isCountryFormalDare = flag; }
    bool getCountryDare() const { return this->isCountryFormalDare; }
    void setEmperorDare(bool flag,DWORD dwDefCountryID) { this->isEmperorDare = flag; this->dwEmperorDareDef = dwDefCountryID; }
    void reliveSecGen();
    bool getEmperorDare() const { return this->isEmperorDare; }
    DWORD getEmperorDareDef() const { return this->dwEmperorDareDef; }
    void setTax(DWORD dwTax) { countryTax = dwTax; }
    const DWORD getTax() const { return countryTax; }
    void addFieldMapName(const char *name)
    {
      stMapName mapname;
      strncpy(mapname.strMapName,name,MAX_NAMESIZE);
      fieldMapName.push_back(mapname);
    }
    void clearFieldMapName() { fieldMapName.clear(); }
    DWORD getField(const char *ou) const
    {
      stMapName *out = (stMapName *)ou; 
      int i=0;
      for(std::vector<stMapName>::const_iterator iter = fieldMapName.begin();iter!=fieldMapName.end();iter++)
      {
        strncpy((&out[i])->strMapName,(*iter).strMapName,MAX_NAMESIZE);
        i++;
      }
      return i;
    }
    bool checkField(const char *out) const
    {
      for(std::vector<stMapName>::const_iterator iter = fieldMapName.begin();iter!=fieldMapName.end();iter++)
      {
        if (strncmp((char *)out,(*iter).strMapName,MAX_NAMESIZE) == 0)
        {
          return true;
        }
      }
      return false;
    }
    void addMainMapName(const char *name)
    {
      stMapName mapname;
      strncpy(mapname.strMapName,name,MAX_NAMESIZE);
      mainMapName.push_back(mapname);
    }

    void clearMainMapName() { mainMapName.clear(); }
    DWORD getMainCity(const char *ou) const
    {
      stMapName *out = (stMapName *)ou; 
      int i=0;
      for(std::vector<stMapName>::const_iterator iter = mainMapName.begin();iter!=mainMapName.end();iter++)
      {
        strncpy((&out[i])->strMapName,(*iter).strMapName,MAX_NAMESIZE);
        i++;
      }
      return i;
    }
    bool checkMainCity(const char *out) const
    {
      for(std::vector<stMapName>::const_iterator iter = mainMapName.begin();iter!=mainMapName.end();iter++)
      {
        if (strncmp((char *)out,(*iter).strMapName,MAX_NAMESIZE) == 0)
        {
          return true;
        }
      }
      return false;
    }

    void addIncMapName(const char *name)
    {
      stMapName mapname;
      strncpy(mapname.strMapName,name,MAX_NAMESIZE);
      incMapName.push_back(mapname);
    }

    void clearIncMapName() { incMapName.clear(); }
    DWORD getIncCity(const char *ou) const
    {
      stMapName *out = (stMapName *)ou; 
      int i=0;
      for(std::vector<stMapName>::const_iterator iter = incMapName.begin();iter!=incMapName.end();iter++)
      {
        strncpy((&out[i])->strMapName,(*iter).strMapName,MAX_NAMESIZE);
        i++;
      }
      return i;
    }
    bool checkIncCity(const char *out) const
    {
      for(std::vector<stMapName>::const_iterator iter = incMapName.begin();iter!=incMapName.end();iter++)
      {
        if (strncmp((char *)out,(*iter).strMapName,MAX_NAMESIZE) == 0)
        {
          return true;
        }
      }
      return false;
    }


    DWORD sceneExp(DWORD exp) const { return (DWORD)(exp * exprate); }
    DWORD winnerExp(DWORD exp) const
    {
      if (winner_exp)
      {
        return (DWORD)(exp * 0.5f);
      }
      else
      {
        return 0;
      }
    }
    BYTE getLevel() const { return level; }
    bool checkUserLevel(SceneUser *pUser);
    ///战胜国经验加成标志 
    bool winner_exp; 

    /// 国战战场死亡后,攻方死亡复活地
    DWORD countryDareBackToMapID;


    /**
     * \brief 是否是收费地图
     * \return 是否是收费地图
     */
    bool isTrainingMap()
    {
      DWORD i = id & 0x0000FFFF;
      return (i>=193 && i<=202);
    }
    void initRegion(zRegion &reg,const zPos &pos,const WORD width,const WORD height);

    bool randPosByRegion(const zPosIndex &index,zPos &pos) const;
  public:
    struct stMapName
    {
      stMapName()
      {
        bzero(strMapName,sizeof(strMapName));
      }
      char strMapName[MAX_NAMESIZE];
    };
    //练功点地图
    std::vector<stMapName> fieldMapName;
    //主城地图
    std::vector<stMapName> mainMapName;
    //增值地图
    std::vector<stMapName> incMapName;
    std::map<std::string,std::string> params;

    ///给npc分组处理
    DWORD execGroup;

    /// 帮会夺城战进行标志
    bool isUnionDare;
    
    /// 该场景所属帮会
    DWORD dwHoldUnionID;

    /// 该场景占领者国家ID
    DWORD dwHoldCountryID;


    /// 正式国战正在该场景进行的标志
    bool isCountryFormalDare;

    /// 皇城战正在该场景进行的标志
    bool isEmperorDare;
    
    /// 皇城战的守方
    DWORD dwEmperorDareDef;

    ///本场景的攻城列表
    //std::list<Rush *> rushList;

    ///一秒定时器
    Timer _one_sec;

    ///已经初始化
    bool inited;

    ///所有的地图格子
    zTiles allTiles;
    ///所有的npc定义
    NpcDefineVector npcDefine;
    ///场景中各种区域的定义
    ZoneTypeDefVector zoneTypeDef;

    ///本场景用户数
    DWORD userCount;
    ///如果本地图没有重生区时需要跳转到的地图
    DWORD backtoMapID;
    ///回到主城的地图id
    DWORD backtoCityMapID;
    ///回到国战目的地
    DWORD backtoDareMapID;
    /// 外国人死亡后应该回到的地图id(没有国家信息)
    DWORD foreignerBacktoMapID;
    /// 在公共国死亡后应该回到的地图id(没有国家信息)
    DWORD commonCountryBacktoMapID; 
    /// 无国籍人在外国死亡重生地
    DWORD commonUserBacktoMapID; 
    /// 国战战场死亡后,守方死亡复活地
    DWORD countryDefBackToMapID;
    ///地图特殊说明
    DWORD function;
    /// 可相互pk的等级
    DWORD pklevel;
    ///国家id
    DWORD countryID;
    ///场景对应的文件名
    std::string fileName;
    ///路点管理器
    WayPointM wpm;
    ///本地图收取税费
    DWORD countryTax;
    ///本地图允许进入的最小玩家等级
    BYTE level;
    ///场景地图加成
    float exprate;

    struct FixedRush
    {
      DWORD id;//ID
      DWORD nextTime;//下次的时间
      DWORD allStart;//总开始时间
      DWORD allEnd;//总结束时间
      int weekDay;//星期几
      tm startTime;//一天中开始的时间
      tm endTime;//一天中结束的时间
      DWORD delay;//开始延迟

      FixedRush()
      {
        id = 0;//ID
        nextTime = 0;//下次的时间
        allStart = 0;//总开始时间
        allEnd = 0;//总结束时间
        weekDay = 0;//星期几
        delay = 0;//开始延迟
      }
    } fixedRush;

	//sky 战场规则文件配置相对路径
	//char DulesFileName[MAX_PATH];

    bool initWayPoint(zXMLParser *parser,const xmlNodePtr node,DWORD countryid);
    bool loadMapFile();
    void initNpc(SceneNpc *sceneNpc,zRegion *init_region,zPos myPos=zPos(0,0));
    bool initByNpcDefine(const t_NpcDefine *pDefine);

    void runCircle_anti_clockwise(const int side,const DWORD X,const DWORD Y,DWORD &CX,DWORD &CY) const;
    void runCircle_clockwise(const int side,const DWORD X,const DWORD Y,DWORD &CX,DWORD &CY) const;

    void updateSceneObject();
    zPosIndex _index;  /// 非阻挡点索引
public:
	//sky 新增战场类场景虚函数
	virtual bool IsGangScene() { return false; }
	virtual void GangSceneTime(const zRTime& ctv) { return; }
	virtual DWORD ReCampThisID(BYTE index) { return 0; }
	//sky 新增战场类场景虚函数 end
};

/**
 * \brief �ٻ�һ��npc
 * \param define npc����ṹ
 * \param pos �ٻ�λ��
 * \param base npc������Ϣ
 * \param standTime ͼ��ϵ�ĳ���ʱ��
 * \param abase ��ǿnpc�Ļ�����Ϣ
 * \return �ٻ���npc��ָ��,ʧ�ܷ���0
 */
    template <typename Npc>
Npc* Scene::summonOneNpc(const t_NpcDefine &define,const zPos &pos,zNpcB *base,DWORD standTime,zNpcB* abase,BYTE vdir, SceneEntryPk * m)
{
    t_NpcDefine *pDefine = new t_NpcDefine(define);
    if (pDefine)
    {
	Npc *sceneNpc = new Npc(this,base,pDefine,SceneNpc::GANG,zSceneEntry::SceneEntry_NPC,abase);
	if (sceneNpc)
	{
	    sceneNpc->setDir(vdir);
	    sceneNpc->setStandingTime(standTime);
	    initNpc(sceneNpc,NULL,pos);//zPos(0,0));//��NULL����define.region��Χ��ѡ��λ��
	    if (sceneNpc->getState() == zSceneEntry::SceneEntry_Normal)
	    {
		if (base->kind != NPC_TYPE_TRAP)
		{
		    Cmd::stAddMapNpcMapScreenUserCmd addNpc;
		    sceneNpc->full_t_MapNpcData(addNpc.data);
		    sendCmdToNine(sceneNpc->getPosI(),&addNpc,sizeof(addNpc));
		    Cmd::stRTMagicPosUserCmd ret;
		    sceneNpc->full_stRTMagicPosUserCmd(ret);
		    sendCmdToNine(sceneNpc->getPosI(),&ret,sizeof(ret));
		}
		else
		{
		    SceneEntryPk *entry = sceneNpc->getMaster();
		    if (entry&& entry->getType() == zSceneEntry::SceneEntry_Player)
		    {
			SceneUser *pUser = (SceneUser *)entry;
			Cmd::stAddMapNpcMapScreenUserCmd addNpc;
			sceneNpc->full_t_MapNpcData(addNpc.data);
			pUser->sendCmdToMe(&addNpc,sizeof(addNpc));
			Cmd::stRTMagicPosUserCmd ret;
			sceneNpc->full_stRTMagicPosUserCmd(ret);
			pUser->sendCmdToMe(&ret,sizeof(ret));
		    }
		}
	    }
#ifdef _DEBUG
	    else
		Zebra::logger->debug("%s ��ʼ״̬ %u",sceneNpc->name,sceneNpc->getState());
#endif
	    return sceneNpc;
	}
	else
	{
	    Zebra::logger->fatal("Scene::summonOneNpc:SceneNpc�����ڴ�ʧ��");
	    SAFE_DELETE(pDefine);
	}
    }
    else
    {
	Zebra::logger->fatal("Scene::summonOneNpc:t_NpcDefine�����ڴ�ʧ��");
    }
    return NULL;
}

/**
 *  * \brief 静态场景
 *   *
 *    */
class StaticScene:public Scene
{

    public:
	StaticScene();
	~StaticScene();
	bool save();
	virtual bool IsGangScene() 
	{
	    return false; 
	}
};

class GangScene : public Scene
{
    public:
	GangScene();
	~GangScene();
	bool save();
};

#endif

