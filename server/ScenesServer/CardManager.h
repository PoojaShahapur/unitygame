#ifndef _CardManager_h_
#define _CardManager_h_
#include "zType.h"
#include "Card.h"
#include "zEntryManager.h"
#include "zUniqueID.h"

class zCard;
class SceneUser;

class GlobalCardIndex:private zEntryManager< zEntryID >
{
  private:
    static GlobalCardIndex *onlyme;
    zMutex mlock;

    GlobalCardIndex();
    ~GlobalCardIndex();
  public:
    static GlobalCardIndex *getInstance();
    static void delInstance();
    void removeObject(DWORD thisid);
    bool addObject(zCard * o);
    zCard *getObjectByThisid(DWORD thisid);
};

extern GlobalCardIndex *const gci;

class UserCardCompare
{
  public:
    virtual bool isIt(zCard * object) =0;
    virtual ~UserCardCompare() {}
};

class UserCardExec
{
  public:
    virtual bool exec(zCard * object) =0;
    virtual ~UserCardExec() {}
};

class GameCardM:private zEntryManager< zEntryID >
{
  public:
    typedef std::vector<DWORD > ObjID_vec;
    typedef std::vector<zCard *> Obj_vec;

    GameCardM();
    ~GameCardM();
    zCard *getObjectByThisID( DWORD thisid);
    zCard *getObjectByPos(const stObjectLocation &dst);
    void removeObjectByThisID(DWORD thisid);
    void removeObject(zCard * o);
    bool addObject(zCard * o);
    zCard *getObject(UserCardCompare &comp);
    void execEvery(UserCardExec &exec);
    
};
#endif

