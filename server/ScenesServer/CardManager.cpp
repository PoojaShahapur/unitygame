#include "ObjectManager.h"
#include "zCard.h"
#include "SceneUser.h"
#include "zDatabaseManager.h"
GlobalCardIndex *GlobalCardIndex::onlyme=NULL;

GlobalCardIndex *const gci=GlobalCardIndex::getInstance();

GlobalCardIndex *GlobalCardIndex::getInstance()
{
  if (onlyme==NULL)
    onlyme= new GlobalCardIndex();
  return onlyme;
}

void GlobalCardIndex::delInstance()
{
  SAFE_DELETE(onlyme);
}

GlobalCardIndex::~GlobalCardIndex()
{
  clear();
}

GlobalCardIndex::GlobalCardIndex()
{
}

bool GlobalCardIndex::addObject(zCard * o)
{
  bool bret=false;
  if (o)
  {
    mlock.lock();
    zCard *ret =(zCard *)getEntryByID(o->id);
    if (ret)//物品重复处理
    {
      if (ret==o)
      {
        bret=true;
        Zebra::logger->debug("全局物品管理器中发现有重复添加物品(%s,%d)",ret->base->name,ret->data.qwThisID);
      }
      else if (ret->createtime==o->createtime && ret->data.dwObjectID==o->data.dwObjectID && ret->data.qwThisID == o->data.qwThisID)
      {
        Zebra::logger->warn("复制物品:%s 创建时间:%llu",ret->name,ret->createid);
        ret=false;
      }
      else
      {
        Zebra::logger->debug("id冲突%s(%d)",ret->name,ret->id);
        do {
          //重新生成ID
          o->generateThisID();
          bret = addEntry((zEntry *)o);
        } while (!bret);
        //if (!bret) Zebra::logger->fatal("添加物品到物品表失败1");
      }
    }
    else
    {
      bret=addEntry((zEntry *)o);
      if (!bret) Zebra::logger->fatal("添加物品到物品表失败");
    }
    mlock.unlock();
  }else {
    Zebra::logger->fatal("添加非法物品");
  }
  
  return bret;
}

void GlobalCardIndex::removeObject(DWORD thisid)
{
  mlock.lock();
  zEntry *e=getEntryByID(thisid);
  if (e)
    removeEntry(e);
  mlock.unlock();
}


zCard *GlobalCardIndex::getObjectByThisid(DWORD thisid)
{
  mlock.lock();
  zEntry *e=getEntryByID(thisid); 
  mlock.unlock();
  return (zCard *)e;
}

GameCardM::GameCardM()
{
}

GameCardM::~GameCardM()
{
}

zCard * GameCardM::getObjectByThisID( DWORD thisid)
{
  return (zCard *)getEntryByID(thisid);
}

class UserCardComparePos:public UserCardCompare 
{
  public:
    stObjectLocation dst;

    bool isIt(zCard *object)
    {
      return true;
    }
};

zCard *GameCardM::getObjectByPos(const stObjectLocation &dst)
{
  UserCardComparePos comp;
  comp.dst=dst;
  return getObject(comp);
}

void GameCardM::removeObjectByThisID(DWORD thisid)
{
  zEntry *e=getEntryByID(thisid);
  if (e)
    removeEntry(e);
}

void GameCardM::removeObject(zCard * o)
{
  removeEntry(o);
}

bool GameCardM::addObject(zCard * o)
{
  return addEntry(o);
}

zCard *GameCardM::getObject(UserCardCompare &comp)
{
  for(hashmap::iterator it=ets.begin();it!=ets.end();it++)
  {
    if (((zCard *)it->second)->data.pos.loc()!=Cmd::OBJECTCELLTYPE_COMMON &&
        ((zCard *)it->second)->data.pos.loc()!=Cmd::OBJECTCELLTYPE_PACKAGE) continue;

    if (comp.isIt((zCard *)it->second))
      return (zCard *)it->second;
  }
  return NULL;
}

void GameCardM::execEvery(UserCardExec &exec)
{
    for(hashmap::iterator it=ets.begin();it!=ets.end();it++)
    {
	zCard* tmp = (zCard *)it->second;
	if (!exec.exec(tmp))
	    return;
    }
}

