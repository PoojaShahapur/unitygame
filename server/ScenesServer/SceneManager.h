#ifndef _SceneManager_h_
#define _SceneManager_h_

#include "zUniqueID.h"
#include "zSceneManager.h"
#include "zSceneEntry.h"

class Scene;
/**
 * \brief 对每个scene执行的回调基类
 *
 */
struct SceneCallBack
{
  virtual bool exec(Scene *scene)=0;
  virtual ~SceneCallBack(){};
};

/**
 * \brief 场景管理器
 *
 */
class SceneManager :public zSceneManager
{
  public:
    typedef std::vector<std::pair<DWORD,DWORD> > NewZoneVec;
    typedef NewZoneVec::iterator newZoneVec_iter;
  public:

	  	DWORD getMapId(DWORD countryid,DWORD mapid);
  
private:
    ///唯一实例
    static SceneManager * sm;
    ///唯一ID分配器
    zUniqueDWORDID *sceneUniqeID;

    ///是否已初始化
    bool inited;

    SceneManager();
    ~SceneManager();

    bool getUniqeID(DWORD &tempid);
    void putUniqeID(const DWORD &tempid);

	///sky 队伍唯一ID分配器
	zUniqueDWORDID *sceneTeamID;

	//sky DWORD1 队伍唯一ID  
	//TeamManager 队伍对象
	//std::map<DWORD, TeamManager*> ScenTeamMap;

    bool newzone;
    NewZoneVec newzon_vec; 

  public:
    static SceneManager &getInstance();
    static void delInstance();
    bool init();
    void final();

    NewZoneVec &queryNewZonePos();
    bool isNewZoneConfig();
    bool randzPosNewZone(Scene *intoScene,zPos &findedPos);
    void addNewZonePos(DWORD x,DWORD y); 
    void setNewZoneConfig(bool type);
    Scene * getSceneByFileName( const char * name);
    Scene * getSceneByName( const char * name);
    Scene * getSceneByTempID( DWORD tempid);
    Scene * getSceneByID( DWORD id);
    Scene * loadScene(/*Scene::SceneType type*/int type,DWORD countryid,DWORD mapid);
	//sky 新增战场动态地图生成
	Scene * loadBattleScene(DWORD baseid);
	//sky end
    void unloadScene(std::string &name);
    void unloadScene(Scene * &scene);
    void unloadAllScene();

    void freshEverySceneField();
    void execEveryScene(SceneCallBack &callback);
    //void execEveryMap(MapCallBack &callback);
    template <class YourEntry>
    void execEveryMap(execEntry<YourEntry> &callback)
    {
      SceneManager::MapMap_iter map_iter = SceneManager::getInstance().map_info.begin();
      for(; map_iter != SceneManager::getInstance().map_info.end() ; map_iter ++)
      {
        callback.exec(&map_iter->second);
      }
    }
    
    void checkUnloadOneScene();
    Scene * loadChallengeGameMap(WORD mapID, char* mapName, Scene *t);
#if 0
	//sky 队伍管理
	///sky 操作队伍唯一ID分配器的方法
	bool getTeamID(DWORD &tempid);
	void putTeamID(const DWORD &tempid);

	//sky 根据队伍的唯一ID返回队伍对象指针
	TeamManager* GetMapTeam(DWORD TeamID);
	bool SceneNewTeam(SceneUser *pUser);		//sky 新建队伍
	bool SceneNewTeam(Cmd::Session::t_Team_Data* send);	//sky 新建队伍(跨场景专用)
	//sky 删除当前场景的队伍管理器里的队伍 同时通知Session也删除队伍管理器的队伍
	bool SceneDelTeam(DWORD TeamID);
	//sky 只删除当前场景的队伍管理器里的队伍
	bool DelMapTeam(DWORD TeamID);
	//sky 遍历队伍管理器处理roll事宜
	void TeamRollItme();
	//sky 队伍 end
#endif
    /**
     * \brief 国家信息
     *
     */
    struct CountryInfo
    {
      ///编号
      DWORD id;
      ///名字
      char name[MAX_NAMESIZE];
      ///所在的地图名字
      DWORD mapid;
      ///国家功能标识字段
      DWORD function;
    };
    /**
     * \brief 地图信息
     *
     */
    struct MapInfo
    {
      ///编号
      DWORD id;
      ///名字
      char name[MAX_NAMESIZE];
      ///对应的地图文件名
      char filename[MAX_NAMESIZE];
      ///玩家在该地图死后回到的地图
      DWORD backto;
      ///玩家在该地图死后回到的城市
      DWORD backtoCity;
      ///玩家在外国地图死后回到的城市
      DWORD foreignbackto;
      ///玩家在公共国地图死后回到的城市
      DWORD commoncountrybackto;
      ///无国家人在外国地图死后回到的城市
      DWORD commonuserbackto;
      ///国战目的地
      DWORD backtodare;
      ///国战期间,在国战战场（目前在王城）死亡后,攻方复活城市
      DWORD countrydarebackto;
      ///国战期间,在国战战场（目前在王城）死亡后,守方复活城市
      DWORD countrydefbackto;
      ///可相互pk的等级
      DWORD pklevel;
      ///表示该地图某些功能是否可用,骑马等
      DWORD function;
      ///表示该地图允许的level级别的玩家进入
      BYTE level;
      ///表示该地图的经验加成(使用时/100求出比率)
      BYTE exprate;
    };
    typedef std::map<DWORD,CountryInfo> CountryMap;
    typedef CountryMap::iterator CountryMap_iter;
    typedef CountryMap::value_type CountryMap_value_type;
    ///国家信息和ID的映射
    CountryMap country_info;
    DWORD getCountryIDByCountryName(const char *name);
    const char * getCountryNameByCountryID(DWORD);
    DWORD getMapIDByMapName(const char *name);
    const char *getMapMapNameByMapID(DWORD mapid);
    DWORD buildMapID(DWORD countryid,DWORD mapid);
    bool buildMapName(DWORD countryid,const char *in,char *out);
    bool buildMapName(DWORD countryid,DWORD mapid,char *out);
    typedef std::map<DWORD,MapInfo> MapMap;
    typedef MapMap::value_type MapMap_value_type;
    typedef MapMap::iterator MapMap_iter;
    ///地图信息和ID的映射
    MapMap map_info;

    WORD serverCountryID;	//�������Ľ���ID
    WORD getServerCountryID()
    {
	return serverCountryID;
    }
};
#endif

