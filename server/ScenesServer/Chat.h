#ifndef _Chat_h_
#define _Chat_h_
#include "zUniqueID.h"
#include "zEntry.h"
#include "zEntryManager.h"
#include "Command.h"

class Scene;
class SceneUser;
class SceneNpc;

class Channel:public zEntry
{
        private:
                std::vector<zEntryC> userlist;
                zEntry creater;
        public:
                static bool sendNine(SceneUser *pUser,const Cmd::stKokChatUserCmd *rev,DWORD cmdLen);
                static bool sendNine(SceneUser *pUser,const char *pattern,...);
                static bool sendNine(const SceneNpc *pNpc,const char *pattern,...);
                static bool sendPrivate(SceneUser *pUser,const Cmd::stKokChatUserCmd *rev,DWORD cmdLen);
                //static bool sendTeam(DWORD teamid,const Cmd::stChannelChatUserCmd *rev,DWORD cmdLen);
                static bool sendCountry(SceneUser *pUser,const Cmd::stKokChatUserCmd *rev,DWORD cmdLen);
                bool sendToAll(SceneUser *pUser,const Cmd::stKokChatUserCmd *rev,DWORD cmdLen);
                static bool sendMapInfo(DWORD mapID,DWORD infoType,const char *pattern,...);
                static bool sendMapInfo(Scene * s,DWORD infoType,const char *pattern,...);
                static bool sendCmdToMap(DWORD mapID,const void *cmd,int len);
                static bool sendCmdToMap(Scene * scene,const void *cmd,int len);

                static bool sendNine(SceneUser *pUser,const char *content);
                static bool sendPrivate(SceneUser *pUser,const char *toName,const char *pattern,...);
                static bool sendPrivate(const char *src_name,const char *toName,const char *pattern,...);
                static bool sendSys(SceneUser *pUser,int type,const char *pattern,...);
                static bool sendMoney(SceneUser *pUser,int type,DWORD money,const char *pattern,...);
                static bool sendGold(SceneUser *pUser,int type,DWORD gold,const char *pattern,...);
                static bool sendSys(DWORD dwUserTempID,int type,const char *pattern,...);
                //static bool sendTeam(DWORD teamid,const char *pattern,...);
                static bool sendCountry(SceneUser *pUser,const char *pattern,...);
                static bool sendCountryInfo(DWORD countryID,DWORD infoType,const char *pattern,...);
                static bool sendAllInfo(DWORD infoType,const char *pattern,...);

    static bool doPreCheck(SceneUser *pUser,Cmd::stKokChatUserCmd *cchat,DWORD cmdLen);
                bool sendToAll(SceneUser *pUser,const char *pattern,...);
                bool sendCmdToAll(const void *cmd,int len);
                Channel(SceneUser *pUser);
                bool remove(const char *name);
                bool add(const char *name);
                bool add(SceneUser *pUser);
                WORD has(const char *name);
};

/**
 * \brief 频道管理器
 *
 */
class ChannelM:public zEntryManager< zEntryTempID,zEntryName >
{
        private:
                static ChannelM * cm;
                zUniqueDWORDID *channelUniqeID;
                zMutex mlock;

                ChannelM();
                ~ChannelM();

                bool getUniqeID(DWORD &tempid);
                void putUniqeID(const DWORD &tempid);

    public:
                static ChannelM &getMe();
                static void destroyMe();
                bool add(Channel *channel);
                void remove(DWORD dwChannelID);
                Channel *get(DWORD dwChannelID);
                void removeUser(const char *name);
};

/// 超级GM的id,只有1个超级GM
#define SUPER_GM_ID 1

struct Gm
{       
        enum
    {       
      none_mode =   0x00,
      normal_mode =   0x01,
      gm_mode =    0x02,
      captain_mode =   0x04,
      super_mode =   0x08,
      debug_mode =   0x10,
      all_mode =   0x1f,/// all_mode
      //admin_mode =   0x10,
      //magic_mode =   0x20
    };
  
        const char *cmd;
        bool (*parse)( SceneUser *pUser,const char *para);
  BYTE priv;
        const char *desc;
  
        static void sendLog(SceneUser *pUser,const char *cmd,const char * content); 

        static void exec(SceneUser *pUser,char *cmd);
	static bool gomap(SceneUser *pUser,const char *para);
	static bool god(SceneUser *pUser,const char *para);
	static bool hideMe(SceneUser* pUser,const char* para);
	static bool showMe(SceneUser* pUser,const char* para);
	static bool loadxml(SceneUser *pUser,const char *para);
	static bool cmdfilter(SceneUser *pUser,const char *para);
        static bool fetch(SceneUser *pUser,const char *para);
        static bool fetchCard(SceneUser *pUser,const char *para);
        static bool drawCard(SceneUser *pUser,const char *para);
	static bool addMoney(SceneUser *pUser,const char *para);
	static bool removeMoney(SceneUser *pUser,const char *para);
	static bool addGold(SceneUser *pUser,const char *para);
	static bool removeGold(SceneUser *pUser,const char *para);
	static bool loadQuest(SceneUser *pUser, const char *para);
        static bool goTo_Gm(SceneUser *pUser,const char *para);
        static bool gomap_Gm(SceneUser *pUser,const char *para);
	static bool kick(SceneUser* pUser,const char* para);
	static bool clearWorld(SceneUser* pUser,const char* para);
	static bool fightmatch(SceneUser* pUser,const char* para);
	static bool changeZone(SceneUser* pUser,const char* para);
	static bool bcworld(SceneUser* pUser,const char* para);
	static bool countServerUser(SceneUser* pUser,const char* para);
	static bool getsize(SceneUser *pUser,const char *para);
	static bool querypoint(SceneUser* pLeader,const char* para);
	static bool loadCardEffect(SceneUser *pUser,const char *para);
	static bool createGroup(SceneUser *pUser,const char *para);
#if 0
        static bool fetch(SceneUser *pUser,const char *para);
        static bool getgive(SceneUser *pUser,const char *para);
        static bool summon(SceneUser *pUser,const char *para);
        static bool help(SceneUser *pUser,const char *para);
        static bool levelup(SceneUser *pUser,const char *para);
        static bool goTo(SceneUser *pUser,const char *para);
        static bool goTo_Gm(SceneUser *pUser,const char *para);
        static bool gomap(SceneUser *pUser,const char *para);
        static bool gomap_Gm(SceneUser *pUser,const char *para);
        static bool upgradeSkill(SceneUser *pUser,const char *para);
        static bool abandon(SceneUser* pUser,const char* para);
        static bool finduser(SceneUser* pUser,const char* para);
        static bool gotouser(SceneUser* pUser,const char* para);
        static bool catchuser(SceneUser* pUser,const char* para);
  static bool bczone(SceneUser* pUser,const char* para);
  static bool bcwgamemsg(SceneUser *pUser,const char *para);
  static bool leechdom(SceneUser* pUser,const char* para);
  static bool value(SceneUser* pUser,const char* para);
  static bool team(SceneUser* pLeader,const char* para);
  static bool throwobject(SceneUser* pLeader,const char* para);
  static bool redeemgold(SceneUser* pLeader,const char* para);
  static bool querygold(SceneUser* pLeader,const char* para);
  static bool redeemmonthcard(SceneUser* pLeader,const char* para);
  static bool goldsystem(SceneUser* pLeader,const char* para);
  static bool stockconsign(SceneUser* pLeader,const char* para);
  static bool stocktransfer(SceneUser* pLeader,const char* para);
  static bool stockuser(SceneUser* pLeader,const char* para);
  static bool stocksystem(SceneUser* pLeader,const char* para);
  static bool givestock(SceneUser* pLeader,const char* para);
  static bool givemoney(SceneUser* pLeader,const char* para);
  static bool givegold(SceneUser* pLeader,const char* para);
  static bool backoff(SceneUser* pUser,const char* para);
  static bool goodness(SceneUser* pUser,const char* para);
  static bool kick(SceneUser* pUser,const char* para);
  static bool kickGateUser(SceneUser* pUser,const char* para);
  static bool donttalk(SceneUser* pUser,const char* para);
  static bool talk(SceneUser* pUser,const char* para);
  static bool setPriv(SceneUser* pUser,const char* para);
  static bool hideMe(SceneUser* pUser,const char* para);
  static bool showMe(SceneUser* pUser,const char* para);
  static bool goHome(SceneUser* pUser,const char* para);
  static bool loadMap(SceneUser* pUser,const char* para);
  static bool unloadMap(SceneUser* pUser,const char* para);
  static bool createRush(SceneUser* pUser,const char* para);
  static bool createQuiz(SceneUser* pUser,const char* para);
  static bool clearWorld(SceneUser* pUser,const char* para);
  static bool clearSeptUnion(SceneUser* pUser,const char* para);
  static bool createFamily(SceneUser* pUser,const char* para);
  static bool viewCountryDare(SceneUser* pUser,const char* para);
  static bool loadProcess(SceneUser *pUser,const char * para);
  static bool loadGift(SceneUser *pUser,const char * para);
  static bool clearArea(SceneUser *pUser,const char *para);
  static bool embar(SceneUser *pUser,const char *para);
  static bool closenpc(SceneUser *pUser,const char * para);
  static bool countrypower(SceneUser *pUser,const char * para);
  static bool refreshGeneral(SceneUser *pUser,const char * para);

  static bool createUnion(SceneUser* pUser,const char* para);
  static bool createSchool(SceneUser* pUser,const char* para);
  static bool showEntries(SceneUser* pUser,const char* para);
  static bool setPetAI(SceneUser *pUser,const char *para);
  static bool setPetAIF(SceneUser *pUser,const char *para);
  static bool addExp(SceneUser *pUser,const char *para);
  static bool shutdown(SceneUser* pUser,const char* para);
  static bool systime(SceneUser* pUser,const char* para);
  static bool usleep(SceneUser* pUser,const char* para);
  static bool qAccount(SceneUser *pUser,const char *para);
  static bool setQuest(SceneUser* pUser,const char* para);
  static bool checkQuest(SceneUser* pUser,const char* para);
  static bool debugVote(SceneUser* pUser,const char* para);
  static bool debugGem(SceneUser* pUser,const char* para);
  static bool addSeptExp(SceneUser* pUser,const char* para);

  static bool setRepute(SceneUser* pUser,const char* para);
  static bool setSeptLevel(SceneUser* pUser,const char* para);
  static bool setAllyFriendDegree(SceneUser* pUser,const char* para);
  static bool changeCountry(SceneUser* pUser,const char* para);
  static bool debugCityDare(SceneUser* pUser,const char* para);
  static bool debugEmperorDare(SceneUser* pUser,const char* para);
  static bool setService(SceneUser* pUser,const char* para);
  static bool setTire(SceneUser *pUser,const char * para);
  static bool enableLogin(SceneUser *pUser,const char * para);
  static bool enableRegister(SceneUser *pUser,const char * para);
  static bool checkCountryInfo(SceneUser *pUser,const char * para);
  static bool version(SceneUser *pUser,const char * para);
  
  /**
   * \brief 技能测试指令
   */
  static bool skill(SceneUser *pUser,const char *para);
  static bool lockValue(SceneUser *pUser,const char *para);
  static bool getvalue(SceneUser *pUser,const char *para);
  static bool setvalue(SceneUser* pUser,const char* para);
  static bool god(SceneUser *pUser,const char *para);
  static bool killer(SceneUser *pUser,const char *para);
  static bool newzone(SceneUser *pUser,const char *para);
  static bool normal(SceneUser *pUser,const char *para);
  static bool svote(SceneUser *pUser,const char *para);
  static bool uvote(SceneUser *pUser,const char *para);
  static bool tong(SceneUser *pUser,const char *para);
  static bool callPet(SceneUser *pUser,const char *para);
  static bool showMaps(SceneUser* pUser,const char* para);
  static bool showPets(SceneUser* pUser,const char* para);
  static bool killPets(SceneUser* pUser,const char* para);
  static bool showSpecialNpc(SceneUser* pUser,const char* para);
  static bool bank(SceneUser* pUser,const char* para);
  static bool setBlock(SceneUser* pUser,const char* para);
  static bool checkBlock(SceneUser* pUser,const char* para);
  static bool npcDare(SceneUser *pUser,const char * para);
  static bool clearPoint(SceneUser *pUser,const char * para);
  static bool clearSkillPoint(SceneUser *pUser,const char * para);
  static bool studySkill(SceneUser *pUser,const char * para);
  static bool clearColddown(SceneUser *pUser,const char * para);
  static bool queryincmap(SceneUser *pUser,const char * para);
  static bool showAddExp(SceneUser *pUser,const char * para);
  static bool changeHorse(SceneUser *pUser,const char * para);
  static bool contribute(SceneUser *pUser,const char * para);
  static bool clearSkill(SceneUser *pUser,const char * para);
#endif
};

extern bool sendCmdByID(DWORD id,const void *cmd,int len);
extern bool sendCmdByTempID(DWORD tempid,const void *cmd,int len);
extern bool sendCmdByName(char * name,const void *cmd,int len);
#endif

