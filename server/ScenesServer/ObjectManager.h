#ifndef _ObjectManager_h_
#define _ObjectManager_h_
#include "zType.h"
#include "Object.h"
#include "zEntryManager.h"
#include "zUniqueID.h"

class zObject;
class SceneUser;

class GlobalObjectIndex:private zEntryManager< zEntryID >
{
  private:
    static GlobalObjectIndex *onlyme;
    zMutex mlock;

    GlobalObjectIndex();
    ~GlobalObjectIndex();
  public:
    static GlobalObjectIndex *getInstance();
    static void delInstance();
    void removeObject(DWORD thisid);
    bool addObject(zObject * o);
    zObject *getObjectByThisid(DWORD thisid);
};

extern GlobalObjectIndex *const goi;

class UserObjectCompare
{
  public:
    virtual bool isIt(zObject * object) =0;
    virtual ~UserObjectCompare() {}
};

class UserObjectExec
{
  public:
    virtual bool exec(zObject * object) =0;
    virtual ~UserObjectExec() {}
};

class UserObjectM:private zEntryManager< zEntryID >
{
  public:
    typedef std::vector<DWORD > ObjID_vec;
    typedef std::vector<zObject *> Obj_vec;

    UserObjectM();
    ~UserObjectM();
    zObject * getObjectByThisID( DWORD thisid);
    zObject *getObjectByPos(const stObjectLocation &dst);
    void removeObjectByThisID(DWORD thisid);
    void removeObject(zObject * o);
    bool addObject(zObject * o);
    zObject *getObject(UserObjectCompare &comp);
    void execEvery(UserObjectExec &exec);
    
    DWORD exist(DWORD id,DWORD number,BYTE upgrade = 0,BYTE type = 0 ) const;
    int space(const SceneUser* user) const;

    zObject* getObjectByID(DWORD id,BYTE upgrade = 0,bool not_need_space = false,int bindflagi=-1) const;
    int reduceObjectNum(SceneUser* user,DWORD id,DWORD number,zObject*& update_ob,ObjID_vec& del_obs,BYTE upgrade = 0);    
    int addObjectNum(SceneUser* user,DWORD id,DWORD number,zObject* & orig_ob,Obj_vec& new_obs,BYTE upgrade,bool bindit);
    int addGreenObjectNum(SceneUser* user,DWORD id,DWORD number,zObject* & orig_ob,Obj_vec& new_obs,BYTE upgrade);
};
#endif

