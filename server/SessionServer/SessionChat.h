#ifndef _SESSIONCHAT_H_
#define _SESSIONCHAT_H_
#include <map>
#include "Session.h"
#include "zSingleton.h"
#include "zUniqueID.h"
#include "zEntryManager.h"

class SessionChannel : public zEntry
{               
  private:
    std::list<DWORD> userList;
    //char creater[MAX_NAMESIZE];
  public:
    bool sendToOthers(UserSession *pUser,const Cmd::stChannelChatUserCmd *rev,DWORD cmdLen);

    //bool sendToAll(UserSession *,const char *pattern,...);
    bool sendCmdToAll(const void *cmd,int len);
    SessionChannel(UserSession *);
    bool remove(DWORD);
    bool remove(UserSession *pUser);
    bool removeAllUser();
    //bool add(const char *name);
    bool add(UserSession *pUser);
    bool has(DWORD);
    DWORD count();

    static bool sendCountry(DWORD,const void *rev,DWORD cmdLen);
    static bool sendCountryInfo(int type,DWORD countryID,const char* mess,...);
    static bool sendAllInfo(int type,const char* mess,...);
    static bool sendAllCmd(const void *cmd,const DWORD len);
    static bool sendPrivate(UserSession * pUser,const char * fromName,const char* mess,...);

    static bool sendCmdToZone(DWORD zone, const void* cmd, int len);
};

typedef zUniqueID<DWORD> zUniqueDWORDID;
class SessionChannelManager : public zEntryManager< zEntryTempID,zEntryName >
{
  private:
    //std::map<DWORD,SessionChannel *> channelList;
    zUniqueDWORDID *channelUniqeID;
    bool getUniqeID(DWORD &tempid);
    void putUniqeID(const DWORD &tempid);
    static SessionChannelManager * scm;

    SessionChannelManager();
    ~SessionChannelManager();
  public:
    static SessionChannelManager & getMe();
    static void destroyMe();
    bool add(SessionChannel *);
    void remove(DWORD);
    SessionChannel * get(DWORD);
    //void removeUser(DWORD);
    void removeUser(UserSession *);
};
#endif

