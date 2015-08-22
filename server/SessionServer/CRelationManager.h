#ifndef _CRelationManager_h_
#define _CRelationManager_h_
#include "zType.h"
#include "zEntry.h"
#include "zEntryManager.h"
#include "Command.h"
#include "SessionCommand.h"

class UserSession;

/// 间隔六十小时以秒为单位的数字
#define MAX_GROUP_TIME_GAP 60*60*60

/// 友好度扣除点数单位为分钟
#define DEDUCT_POINT 5*60

struct CRelation : public zEntry
{
public:
  ///  关系类型 
  BYTE  type;

  ///  关系级别
  WORD  level;

  ///  用户Session对象
//  UserSession *user;
  bool online;

  /// 用户角色ID
  DWORD charid;

  ///  最后更新时间
  DWORD lasttime;

  ///  职业
  WORD  occupation;

  CRelation();
  void sendNotifyToScene();
  bool isOnline();
private:
  /// 读写锁
  //zRWLock rwlock;
};

class CRelationManager : public zEntryManager<zEntryID,zEntryName>
{
private:
  /// 读写锁
  //zRWLock rwlock;

  /// 用户对象
  UserSession *user;

public:
  ~CRelationManager();
  template <class YourEntry>
  bool execEveryOne(execEntry<YourEntry> &exec)
  {
    //rwlock.rdlock();
    bool ret=execEveryEntry<>(exec);
    //rwlock.unlock();
    return ret;
  }


  void init();
  void loadFromDatabase();
  void deleteDBRecord(const DWORD dwID);
  bool insertDBRecord(const CRelation *);
  void updateDBRecord(const CRelation *relation);
  void updateOtherOfflineUserDBRecord(const CRelation *relation);
  void writeOfflineNotify(const CRelation *relation);
//  void writeDatabase();
  bool processUserMessage(const Cmd::stNullUserCmd *pNullCmd,const DWORD cmdLen);
  void removeRelation(const char *);
  void addBadRelation(const char *name);
  void addEnemyRelation(const char *name);
  inline void sendStatusChange(const CRelation *,const BYTE byState);
  void addRelation(const DWORD dwID,const BYTE type);
  void changeRelationType(const char * name,const BYTE type);
  void sendRelationList();
  void online(const DWORD dwID);
  void offline(const DWORD dwID,const char* name);
  void setUser(UserSession *);
  void sendChatToMyFriend(const Cmd::stKokChatUserCmd *,const DWORD cmdLen);
  void sendCmdToMyFriend(const void *,const DWORD,bool sendMe=true);
  void sendCmdToMyFriendExcept(const void *,const DWORD,bool sendMe=true,const char * = "");
  void sendPrivateChatToFriend(const Cmd::stChannelChatUserCmd *,const DWORD cmdLen);
  void sendBlackListToGateway(const char *,const BYTE oper);
  void sendAllBlackListToGateway();
  CRelation*  getRelationByType(int relationType);
  CRelation * getRelationByName(const char *);
  CRelation*  getRelationByID(DWORD dwRelationID);
  CRelation*  getMarryRelation();
  void setFriendDegree(Cmd::Session::t_CountFriendDegree_SceneSession *);
  void setFriendDegreeByOtherUser(const DWORD dwUserID,const WORD wdDegree,const DWORD currTime);
};

#endif
