/**
 * \brief 实现网关用户管理类
 */
#include "GateUserManager.h"
#include "GatewayServer.h"
#include "SceneClient.h"

GateUserManager *GateUserManager::gum(NULL);
//RecycleUserManager *RecycleUserManager::instance=NULL;

/**
 * \brief 得到唯一实例
 *
 *
 * \return 唯一实例
 */
GateUserManager * GateUserManager::getInstance()
{
  if (gum==NULL)
    gum=new GateUserManager();
  return gum;
}

/**
 * \brief 删除唯一实例
 *
 *
 */
void GateUserManager::delInstance()
{
  SAFE_DELETE(gum);
}

GateUserManager::GateUserManager()
{
  inited=false;
  userUniqeID=NULL;
}

GateUserManager::~GateUserManager()
{
    zEntryManager<zEntryID, zEntryTempID, zEntryName>::clear();
  final();
}

/**
 * \brief 卸载所有用户
 *
 *
 * \return 
 */
void GateUserManager::removeAllUser()
{
  struct UnloadAllExec :public execEntry<GateUser>
  {
    std::vector<DWORD> del_vec;
    UnloadAllExec()
    {
    }
    bool exec(GateUser *u)
    {
      del_vec.push_back(u->id);
      return true;
    }
  };
  UnloadAllExec exec;
  GateUserManager::getInstance()->execEveryUser(exec);
  for(std::vector<DWORD>::iterator iter = exec.del_vec.begin() ; iter != exec.del_vec.end() ; iter ++)
  {
    GateUser *pUser=(GateUser *)GateUserManager::getInstance()->getUserByID(*iter);
    if (pUser)
    {
      //Zebra::logger->info("用户%s(%u)因卸载场景注销",pUser->name,pUser->id);
      pUser->TerminateWait();
    }
  }

}
/**
 * \brief 得到一个tempid
 *
 *
 * \param tempid: 得到的tempid(输出)
 * \return 得到是否成功
 */
void GateUserManager::removeUserBySceneClient(SceneClient *scene)
{
  struct UnloadSceneExec :public execEntry<GateUser>
  {
    SceneClient *scene;
    std::vector<DWORD> del_vec;
    UnloadSceneExec(SceneClient *s):scene(s)
    {
    }
    bool exec(GateUser *u)
    {
      if (u->scene == scene)
      {
        del_vec.push_back(u->id);
      }
      return true;
    }
  };
  UnloadSceneExec exec(scene);
  GateUserManager::getInstance()->execEveryUser(exec);
  for(std::vector<DWORD>::iterator iter = exec.del_vec.begin() ; iter != exec.del_vec.end() ; iter ++)
  {
    GateUser *pUser=(GateUser *)GateUserManager::getInstance()->getUserByID(*iter);
    if (pUser)
    {
      Zebra::logger->info("用户%s(%u)因卸载场景注销",pUser->name,pUser->id);
      pUser->TerminateWait();
    }
  }

}

/**
 * \brief 得到一个tempid
 *
 *
 * \param tempid: 得到的tempid(输出)
 * \return 得到是否成功
 */
bool GateUserManager::getUniqeID(DWORD &tempid)
{
  if (userUniqeID)
  {
    tempid=userUniqeID->get();
    //Zebra::logger->debug("得到usertempid = %u",tempid);
    return (tempid!=userUniqeID->invalid());
  }
  else
    return false;
}

/**
 * \brief 收回一个tempid
 *
 *
 * \param tempid: 要收回的tempid
 */
void GateUserManager::putUniqeID(const DWORD &tempid)
{
  if (userUniqeID)
  {
    userUniqeID->put(tempid);
    //Zebra::logger->debug("回收usertempid = %u",tempid);
  }
}

/**
 * \brief 根据初始化分配tempid的范围
 *
 *
 * \return true
 */
bool GateUserManager::init()
{
	if (!inited)
	{
		//为每个网关服务器生成不相交叉的用户临时ID分配器,最小的从1000开始,每个有4998个ID可用
		DWORD firstTempID=1000+(GatewayService::getInstance().getServerID()%100)*5000;
		userUniqeID=new zUniqueDWORDID(firstTempID,firstTempID+4998);

		inited=true;
	}
	return inited;
}

/**
 * \brief 卸载网关用户管理器
 *
 */
void GateUserManager::final()
{
  SAFE_DELETE(userUniqeID);
  inited=false;
}

/**
 * \brief 根据accid得到一个用户
 *
 *
 * \param accid: 角色的accid
 * \return 用户
 */
GateUser * GateUserManager::getUserByAccID(DWORD accid)
{
  arwlock.rdlock();
  GateUser *ret =NULL;
  GateUserAccountID::find(accid,ret);
  arwlock.unlock();
  return ret;
}

/**
 * \brief 从网关管理器中删除一个用户
 *
 *
 * \param accid: 好删除用户的accid
 */
void GateUserManager::removeUserOnlyByAccID(DWORD accid)
{
  arwlock.wrlock();
  GateUserAccountID::remove(accid);
  arwlock.unlock();
}

/**
 * \brief 将一个用户添加到到网关用户管理器中
 *
 *
 * \param user: 需要添加到管理器中的用户
 * \return 添加成功返回图若,否则返回false 
 */
bool GateUserManager::addUserOnlyByAccID(GateUser *user)
{
  arwlock.wrlock();
  bool ret=GateUserAccountID::push(user);
  arwlock.unlock();
  return ret;
}
bool GateUserManager::addCountryUser(GateUser *user)
{
  crwlock.wrlock();
  bool ret=countryindex[user->countryID].insert(user).second;
  crwlock.unlock();
  return ret;
}
void GateUserManager::removeCountryUser(GateUser *user)
{
  crwlock.wrlock();
  CountryUserMap_iter iter = countryindex.find(user->countryID);
  if (iter != countryindex.end())
  {
    iter->second.erase(user);
  }
  crwlock.unlock();
}
template <class YourNpcEntry>
void GateUserManager::execAllOfCountry(const DWORD country,execEntry<YourNpcEntry> &callback)
{
  crwlock.rdlock();
  CountryUserMap_iter iter = countryindex.find(country);
  if (iter != countryindex.end())
  {
    for(GateUser_SET_iter iter_1 = iter->second.begin() ; iter_1 != iter->second.end() ; iter_1++)
    {
      callback.exec(*iter_1);
    }
  }
  crwlock.unlock();
}
void GateUserManager::sendCmdToCountry(const DWORD country,const void *pstrCmd,const DWORD nCmdLen)
{
  int _sendLen;
  t_StackCmdQueue cmd_queue;
  DWORD _type=0;
  char _name[MAX_NAMESIZE];
  bzero(_name,sizeof(_name));
  DWORD cmdLen=nCmdLen;
  GateUser::cmdFilter((Cmd::stNullUserCmd *)pstrCmd,_type,_name,cmdLen);
  _sendLen = zSocket::packetPackZip(pstrCmd,cmdLen,cmd_queue); 
  crwlock.rdlock();
  CountryUserMap_iter iter = countryindex.find(country);
  if (iter != countryindex.end())
  {
    for(GateUser_SET_iter iter_1 = iter->second.begin() ; iter_1 != iter->second.end() ; iter_1++)
    {
      (*iter_1)->sendCmd(cmd_queue.rd_buf(),_sendLen,_type,_name,true);
    }
  }
  crwlock.unlock();
}

zUser *GateUserManager::getUserByID(DWORD id)
{
    zUser *ret = NULL;
    mlock.lock();
    ret = (zUser*)getEntryByID(id);
    mlock.unlock();
    return ret;
}

bool GateUserManager::addUser(zSceneEntry *user)
{
    bool ret = false;
    GateUser* tmp = NULL;
    mlock.lock();
    tmp = getUserByAccID(((GateUser*)user)->accid);
    if(tmp)
	ret = addEntry((zEntry *)user);
    mlock.unlock();
    if(tmp == NULL)
	Zebra::logger->error("GateUserManager::addUser ���ִ���");
    return ret;
}

void GateUserManager::removeUser(zSceneEntry *user)
{
    mlock.lock();
    removeEntry((zEntry*)user);
    mlock.unlock();
}
