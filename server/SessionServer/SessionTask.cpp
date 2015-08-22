/**
 * \brief 定义登陆连接任务
 *
 */

#include "SessionTask.h"
#include "Session.h"
#include "zEntryManager.h"
#include "SessionChat.h"
#include "SessionManager.h"
#include "SessionTaskManager.h"
#include "SessionServer.h"
#include "zDBConnPool.h"
#include "zMetaData.h"
#include "GmToolCommand.h"
#include "TimeTick.h"
#include "TempArchive.h"    //temp record
#include "zXML.h"
#include "SessionCommand.h"
#include "CSort.h"
#include "HeroCardManager.h"
#include "NewbieLimit.h"
#include "MailService.h"

/**
 * \brief 遍历每个用户会话给同一国家的角色发送聊天消息
 */
struct EveryUserSessionAction: public execEntry<UserSession>
{
  DWORD country;
  DWORD cmdLen;
  Cmd::stChannelChatUserCmd * revCmd;
  bool init(Cmd::stChannelChatUserCmd * rev,DWORD len)
  {
    UserSession *pUser = UserSessionManager::getInstance()->getUserSessionByName(rev->pstrName);
    revCmd = rev;
    cmdLen = len;
    if (pUser) 
    {
      country = pUser->country;
      return true;
    }
    return false;
  }

  /**
   * \brief 遍历每个用户会话给同一国家的角色发送聊天消息
   * \param su 用户会话
   * \return true 成功 false 失败
   */
  bool exec(UserSession *su)
  {
    if (country == su->country)
      su->sendCmdToMe(revCmd,cmdLen);
    return true;
  }
};


/**
 * \brief 遍历每个用户会话给同一国家的角色发送用户命令
 */
struct OneCountryUserSessionAction: public execEntry<UserSession>
{
  DWORD country;
  DWORD cmdLen;
  Cmd::stNullUserCmd* sendCmd;

  void init(Cmd::stNullUserCmd * rev,DWORD len,DWORD countryID)
  {
    sendCmd = rev;
    cmdLen = len;

    country = countryID;
  }

  /**
   * \brief 遍历每个用户会话给同一国家的角色发送聊天消息
   * \param su 用户会话
   * \return true 成功 false 失败
   */
  bool exec(UserSession *su)
  {
    if (country == su->country)
      su->sendCmdToMe(sendCmd,cmdLen);
    return true;
  }
};

/**
 * \brief 遍历每个用户会话给角色发送聊天消息
 */
struct broadcastToEveryUser: public execEntry<UserSession>
{
  DWORD cmdLen;
  Cmd::stChannelChatUserCmd * revCmd;
  bool init(Cmd::stChannelChatUserCmd * rev,DWORD len)
  {
    if (0==rev) return false;
    revCmd = rev;
    cmdLen = len;
    return true;
  }

  /**
   * \brief 遍历
   * \param su 用户会话
   * \return true 成功 false 失败
   */
  bool exec(UserSession *su)
  {
    if (su) su->sendCmdToMe(revCmd,cmdLen);
    return true;
  }
};

/**
 * \brief 广播通知给每个角色
 */
struct broadcastRushToEveryUser: public execEntry<UserSession>
{
  char * pContent;

  bool init(char * content)
  {
    pContent = content;
    return true;
  }
  /**
   * \brief 遍历发送消息
   * \param su 用户会话
   * \return true 成功 false 失败
   */
  bool exec(UserSession *su)
  {
    su->sendSysChat(Cmd::INFO_TYPE_GAME,pContent);
    return true;
  }
};

/**
 * \brief 验证登陆会话服务器的连接指令
 *
 * 如果验证不通过直接断开连接
 *
 * \param ptCmd 登陆指令
 * \return 验证是否成功
 */
bool SessionTask::verifyLogin(const Cmd::Session::t_LoginSession *ptCmd)
{
  if (Cmd::Session::CMD_LOGIN == ptCmd->cmd
      && Cmd::Session::PARA_LOGIN == ptCmd->para
      && (SCENESSERVER == ptCmd->wdServerType || GATEWAYSERVER == ptCmd->wdServerType))
  {
    const Cmd::Super::ServerEntry *entry = SessionService::getInstance().getServerEntryById(ptCmd->wdServerID);
    char strIP[32];
    strncpy(strIP,getIP(),31);

    if (NULL != entry){
      Zebra::logger->debug("SessionTask::verifyLogin %s,%d,%d for %s,%d,%d",entry->pstrIP,entry->wdServerID,entry->wdServerType,strIP,ptCmd->wdServerID,ptCmd->wdServerType);
    }
    else{
      Zebra::logger->error("SessionTask::verifyLogin NULL for %s,%d,%d",strIP,ptCmd->wdServerID,ptCmd->wdServerType);
    }

    if (entry
        && ptCmd->wdServerType == entry->wdServerType
        && 0 == strcmp(strIP,entry->pstrIP))
    {
      wdServerID   = ptCmd->wdServerID;
      wdServerType = ptCmd->wdServerType;
      return true;
    }
  }

    Zebra::logger->error("!SessionTask::verifyLogin cmd=%d,para=%d,wdServerID=%d,wdServerType=%d",ptCmd->cmd,ptCmd->para,ptCmd->wdServerID,ptCmd->wdServerType);
  return false;
}

int SessionTask::verifyConn()
{
  int retcode = mSocket.recvToBuf_NoPoll();
  if (retcode > 0)
  {
    BYTE pstrCmd[zSocket::MAX_DATASIZE];
    int nCmdLen = mSocket.recvToCmd_NoPoll(pstrCmd,sizeof(pstrCmd));
    if (nCmdLen <= 0)
      //这里只是从缓冲取数据包，所以不会出错，没有数据直接返回
      return 0;
    else
    {
      if (verifyLogin((Cmd::Session::t_LoginSession *)pstrCmd))
      {
        Zebra::logger->debug("SessionTask::verifyConn �ͻ�������ͨ����֤");
        return 1;
      }
      else
      {
        Zebra::logger->error("SessionTask::verifyConn �ͻ���������֤ʧ��");
        return -1;
      }
    }
  }
  else
    return retcode;
}

bool SessionTask::checkRecycle()
{
  if (recycle_state == 0)
  {
    return false;
  }
  if (recycle_state == 1)
  {
    //清理已经注册的用户
    UserSessionManager::getInstance()->removeAllUserByTask(this);
    //注销已经注册的地图
    SceneSessionManager::getInstance()->removeAllSceneByTask(this);
    recycle_state=2;
    return true;
  }
  return true;
}
int SessionTask::recycleConn()
{
  switch(recycle_state)
  {
    case 0:
      {
        recycle_state=1;
        return 0;
      }
      break;
    case 1:
      {
        return 0;
      }
      break;
    case 2:
      {
        return 1;
      }
      break;
  }
  return 1;
}

void SessionTask::addToContainer()
{
  SessionTaskManager::getInstance().addSessionTask(this);
}

void SessionTask::removeFromContainer()
{
  SessionTaskManager::getInstance().removeSessionTask(this);
}

bool SessionTask::uniqueAdd()
{
  return SessionTaskManager::getInstance().uniqueAdd(this);
}

bool SessionTask::uniqueRemove()
{
  return SessionTaskManager::getInstance().uniqueRemove(this);
}

/**
 * \brief 更换国籍
 *
 * \param dwUserID : 更换国籍的用户ID
 *
 */
bool SessionTask::change_country(const Cmd::Session::t_changeCountry_SceneSession* cmd)
{
#if 0
  CUnionM::getMe().fireUnionMember(cmd->dwUserID,false);
  CSeptM::getMe().fireSeptMember(cmd->dwUserID,false); 

  //DBFieldSet* samplerelation = SessionService::metaData->getFields("SAMPLERELATION");
  UserSession* pUser = UserSessionManager::getInstance()->getUserByID(cmd->dwUserID);
  if (!pUser) return true;
  CSchoolM::getMe().fireSchoolMember(pUser->name,false);
  CCountry* pCountry = CCountryM::getMe().find(pUser->country);

  if (pCountry)
  {
    if (strncmp(pCountry->diplomatName,pUser->name,MAX_NAMESIZE) == 0)
    {
      pCountry->cancelDiplomat();
    }

    if (strncmp(pCountry->catcherName,pUser->name,MAX_NAMESIZE) == 0)
    {
      pCountry->cancelCatcher();
    }
  }
  
  /*DBRecord where;
  std::ostringstream oss;
  oss << "charid='" << cmd->dwUserID << "'";
  where.put("charid",oss.str());

  if (samplerelation)
  {
    connHandleID handle = SessionService::dbConnPool->getHandle();

    DBRecordSet* recordset = NULL;

    if ((connHandleID)-1 != handle)
    {
      recordset = SessionService::dbConnPool->exeSelect(handle,samplerelation,NULL,&where);
    }

    SessionService::dbConnPool->putHandle(handle);

    if (recordset)
    {//清除我的所有关系
      for (DWORD i=0; i<recordset->size(); i++)
      {
        DBRecord* rec = recordset->get(i);
        UserSession *pOtherUser = NULL;

        if (rec)
        {
          pOtherUser = UserSessionManager::getInstance()->getUserSessionByName
            (rec->get("relationname"));
        }

        if (pOtherUser)
        {
          pOtherUser->relationManager.removeRelation(pUser->name);
          if (pUser) pUser->relationManager.removeRelation(pOtherUser->name);
        }
        else
        {
          connHandleID handle = SessionService::dbConnPool->getHandle();

          where.clear();
          oss.str("");
          oss << "relationid='" << cmd->dwUserID << "'";
          where.put("relationid",oss.str());

          if ((connHandleID)-1 != handle)
          {
            SessionService::dbConnPool->exeDelete(handle,samplerelation,&where);
          }

          SessionService::dbConnPool->putHandle(handle);
        }

        where.clear();
        oss.str("");
        oss << "charid='" << cmd->dwUserID << "'";
        where.put("charid",oss.str());
        connHandleID handle = SessionService::dbConnPool->getHandle();

        if ((connHandleID)-1 != handle)
        {
          SessionService::dbConnPool->exeDelete(handle,samplerelation,&where);
        }
        SessionService::dbConnPool->putHandle(handle);
      }
    }

    // ---------------清除与我有关的所有社会关系记录---------------
    SAFE_DELETE(recordset);

    where.clear();
    oss.str("");
    oss << "relationid='" << cmd->dwUserID << "'";
    where.put("relationid",oss.str());
    handle = SessionService::dbConnPool->getHandle();

    if ((connHandleID)-1 != handle)
    {
      recordset = SessionService::dbConnPool->exeSelect(handle,samplerelation,NULL,&where);
    }

    SessionService::dbConnPool->putHandle(handle);

    if (recordset)
    {//清除我的所有关系
      for (DWORD i=0; i<recordset->size(); i++)
      {
        DBRecord* rec = recordset->get(i);
        UserSession *pOtherUser = NULL;
        DWORD charid = 0;

        if (rec)
        {
          charid = rec->get("charid");
#ifdef _DEBUG
          Zebra::logger->debug("charid:%d",charid);      
#endif              
          pOtherUser = UserSessionManager::getInstance()->getUserByID
            (charid);
        }

        if (pOtherUser)
        {
          pOtherUser->relationManager.removeRelation(pUser->name);
        }
        else
        {
          connHandleID handle = SessionService::dbConnPool->getHandle();

          where.clear();
          oss.str("");
          oss << "relationid='" << cmd->dwUserID << "'";
          where.put("relationid",oss.str());

          oss.str("");
          if (charid)
          {
            oss << "charid='" << charid << "'";
            where.put("charid",oss.str());
          }

          if ((connHandleID)-1 != handle)
          {
            SessionService::dbConnPool->exeDelete(handle,samplerelation,&where);
          }

          SessionService::dbConnPool->putHandle(handle);
        }
      }
      SAFE_DELETE(recordset);
    }
    SAFE_DELETE(recordset);
  }
  */
  
  Cmd::stUnmarryCmd unmarry;
  pUser->relationManager.processUserMessage(&unmarry,sizeof(unmarry));

  if (pUser)
  {
    pUser->relationManager.sendRelationList();
    pUser->country = cmd->dwToCountryID; 
  }

  Cmd::Session::t_returnChangeCountry_SceneSession send;
  send.dwUserID = cmd->dwUserID;
  if (pUser && pUser->scene) pUser->scene->sendCmd(&send,sizeof(send));
#endif
  return true;
}



/**
 * \brief 删除角色
 *
 * \param cmd : 删除角色命令
 * \param cmdLen: 命令长度
 *
 *
 */
bool SessionTask::del_role(const Cmd::t_NullCmd* cmd,const DWORD cmdLen)
{
  Cmd::Session::t_DelChar_GateSession *rev=(Cmd::Session::t_DelChar_GateSession *)cmd;
  using namespace std;
  //���ﴦ�����ǻỰ����ع��ܵ��������,������ѹ�ϵ�ȵ�
#if 0
  MailService::getMe().delMailByNameAndID(rev->name,rev->id);
  AuctionService::getMe().delAuctionRecordByName(rev->name);
  CartoonPetService::getMe().delPetRecordByID(rev->id);

  int retUnion  = 0;
  int retSept     = 0;
  int retSchool  = 0;

  rev->status = 0;

  retUnion  = CUnionM::getMe().fireUnionMember(rev->id,false);
  retSept   = CSeptM::getMe().fireSeptMember(rev->id,false); 
  retSchool = CSchoolM::getMe().fireSchoolMember(rev->name,false);

  //  if ((retUnion > 0 )
  //    && (retSept>0)
  //    && (retSchool>0)
  //    )
  {// 没有帮主、师尊、族长的情况。皆能正常退出社会关系。则再进行相应的处理
    // 只要有一个社会关系不能退出，则取消操作(现在直接解除社会关系，不再判断了)
    //int ret = 0;

    /*ret = CUnionM::getMe().fireUnionMember(rev->id,false);

      if (ret<=0)
      {
      rev->status = 4;
      }

      ret = CSeptM::getMe().fireSeptMember(rev->id,false);

      if (ret<=0)
      {
      rev->status = 4;
      }

      ret = CSchoolM::getMe().fireSchoolMember(rev->name,false);

      if (ret<=0)
      {
      rev->status = 4;
      }*/

    // 从数据库中读取我的社会关系，并判断对方是否在线，如果不在线，则直接删除数据库记录，并在用户上线处理中
    // 把删除关系的命令发送给场景，让场景更新数据
    // 如果在线，则删除我自己的数据库记录，并调用对方的relationManager->removeRelation(我的角色名称)
    DBFieldSet* samplerelation = SessionService::metaData->getFields("SAMPLERELATION");

    DBRecord where;
    std::ostringstream oss;
    oss << "charid='" << rev->id << "'";
    where.put("charid",oss.str());

    if (samplerelation)
    {
      connHandleID handle = SessionService::dbConnPool->getHandle();

      DBRecordSet* recordset = NULL;

      if ((connHandleID)-1 != handle)
      {
        recordset = SessionService::dbConnPool->exeSelect(handle,samplerelation,NULL,&where);
      }

      SessionService::dbConnPool->putHandle(handle);

      if (recordset)
      {//清除我的所有关系
        for (DWORD i=0; i<recordset->size(); i++)
        {
          DBRecord* rec = recordset->get(i);

          UserSession *pOtherUser = NULL;

          if (rec)
          {
            pOtherUser = UserSessionManager::getInstance()->getUserSessionByName
              (rec->get("relationname"));
          }

          if (pOtherUser)
          {
            pOtherUser->relationManager.removeRelation(rev->name);
          }
          else
          {
            connHandleID handle = SessionService::dbConnPool->getHandle();

            where.clear();
            oss.str("");
            oss << "relationid='" << rev->id << "'";
            where.put("relationid",oss.str());

            if ((connHandleID)-1 != handle)
            {
              SessionService::dbConnPool->exeDelete(handle,samplerelation,&where);
            }

            SessionService::dbConnPool->putHandle(handle);
          }

          where.clear();
          oss.str("");
          oss << "charid='" << rev->id << "'";
          where.put("charid",oss.str());
          connHandleID handle = SessionService::dbConnPool->getHandle();

          if ((connHandleID)-1 != handle)
          {
            SessionService::dbConnPool->exeDelete(handle,samplerelation,&where);
          }
          SessionService::dbConnPool->putHandle(handle);
        }
      }


      // ---------------清除与我有关的所有社会关系记录---------------
      SAFE_DELETE(recordset);

      where.clear();
      oss.str("");
      oss << "relationid='" << rev->id << "'";
      where.put("relationid",oss.str());
      handle = SessionService::dbConnPool->getHandle();


      if ((connHandleID)-1 != handle)
      {
        recordset = SessionService::dbConnPool->exeSelect(handle,samplerelation,NULL,&where);
      }

      SessionService::dbConnPool->putHandle(handle);

      if (recordset)
      {//清除我的所有关系
        for (DWORD i=0; i<recordset->size(); i++)
        {
          DBRecord* rec = recordset->get(i);
          UserSession *pOtherUser = NULL;
          DWORD charid = 0;

          if (rec)
          {
            charid = rec->get("charid");
#ifdef _DEBUG
            Zebra::logger->debug("charid:%d",charid);      
#endif              
            pOtherUser = UserSessionManager::getInstance()->getUserByID
              (charid);
          }

          if (pOtherUser)
          {
            pOtherUser->relationManager.removeRelation(rev->name);
          }
          else
          {
            connHandleID handle = SessionService::dbConnPool->getHandle();

            where.clear();
            oss.str("");
            oss << "relationid='" << rev->id << "'";
            where.put("relationid",oss.str());

            oss.str("");
            if (charid)
            {
              oss << "charid='" << charid << "'";
              where.put("charid",oss.str());
            }

            if ((connHandleID)-1 != handle)
            {
              SessionService::dbConnPool->exeDelete(handle,samplerelation,&where);
            }

            SessionService::dbConnPool->putHandle(handle);
          }
        }
        SAFE_DELETE(recordset)
      }
    }
  }

  if (rev->status == 0)
  {
    Zebra::logger->info("已解除社会关系，继续删除角色的处理");
  }
  else
  {
    Zebra::logger->info("存在不能解除的社会关系，删除角色操作被取消");
  }
#endif
  this->sendCmd(rev,sizeof(Cmd::Session::t_DelChar_GateSession));
  return true;
}

bool SessionTask::msgParse_Scene(const Cmd::t_NullCmd *cmd,const DWORD cmdLen)
{
#if 0
  if (CUnionM::getMe().processSceneMessage(cmd,cmdLen)) return true; //帮会的场景服务器消息处理
  if (CSchoolM::getMe().processSceneMessage(cmd,cmdLen)) return true;
  if (CSeptM::getMe().processSceneMessage(cmd,cmdLen)) return true;
  if (CQuizM::getMe().processSceneMessage(cmd,cmdLen)) return true;
  if (CNpcDareM::getMe().processSceneMessage(cmd,cmdLen)) return true;
#endif
  if (MailService::getMe().doMailCmd(cmd,cmdLen)) return true;

  switch(cmd->para)
  {
    case Cmd::Session::PARA_DEBUG_COUNTRYPOWER:
      {
        time_t timValue = time(NULL);
        struct tm tmValue;
        zRTime::getLocalTime(tmValue,timValue);
        SessionService::getInstance().checkCountry(tmValue,true);
        return true;
      }
      break;
    case Cmd::Session::PARA_CLOSE_NPC:
      {
        SessionTaskManager::getInstance().broadcastScene(cmd,cmdLen);
        return true;
      }
      break;
    case Cmd::Session::PARA_SCENE_SEND_CMD:
      {
        Cmd::Session::t_sendCmd_SceneSession *rev = (Cmd::Session::t_sendCmd_SceneSession *)cmd;
        SceneSession * s = SceneSessionManager::getInstance()->getSceneByID(rev->mapID);
        if (!s) return true;

        return s->sendCmd(rev,sizeof(Cmd::Session::t_sendCmd_SceneSession)+rev->len);
      }
      break;
    case Cmd::Session::PARA_SET_SERVICE:
      {
        Cmd::Session::t_SetService_SceneSession *rev = (Cmd::Session::t_SetService_SceneSession *)cmd;

        char buf[32];
        bzero(buf,sizeof(buf));
        snprintf(buf,32,"%u",rev->flag);
        Zebra::global["service_flag"] = buf;

        SessionTaskManager::getInstance().broadcastScene(cmd,cmdLen);
        return true;
      }
      break;
#if 0
    case Cmd::Session::PARA_ADD_RELATION_ENEMY:
      {
        Cmd::Session::t_addRelationEnemy *rev = (Cmd::Session::t_addRelationEnemy *)cmd;
        UserSession* pUser = UserSessionManager::getInstance()->getUserByID(rev->dwUserID);
        if (!pUser) return false;
        pUser->relationManager.addEnemyRelation(rev->name);
        return true;
      }
      break;
    case Cmd::Session::PARA_SPEND_GOLD:
      {
        Cmd::Session::t_SpendGold_SceneSession *rev = (Cmd::Session::t_SpendGold_SceneSession *)cmd;
        UserSession* pUser = UserSessionManager::getInstance()->getUserByID(rev->userID);
        if (!pUser) return false;

        CSept * sept = CSeptM::getMe().getSeptByID(pUser->septid);
        if (sept)
        {
          DWORD num = ((DWORD)(rev->gold/100))*2;
          sept->sendGoldToMember(rev->userID,num);
        }
        /*
        CSept * sept = CSeptM::getMe().getSeptByID(pUser->septid);
        if (sept)
        {
          DWORD m = sept->dwSpendGold/10;
          sept->dwSpendGold += rev->gold;
          DWORD n = sept->dwSpendGold/10;
          if (n-m)
          {
            sept->dwRepute += n-m;
            sept->sendSeptNotify("家族成员 %s 消费金币,家族声望提高了 %u 点",pUser->name,n-m);
          }
        }
        */
        return true;
      }
      break;
    case Cmd::Session::OVERMAN_TICKET_ADD:
      {
        Cmd::Session::t_OvermanTicketAdd *command = (Cmd::Session::t_OvermanTicketAdd*)cmd;
        UserSession* pUser = UserSessionManager::getInstance()->getUserByID(command->id);
        if (pUser)
        {
          Cmd::Session::t_OvermanTicketAdd add;
          add.id=command->id;
          add.ticket=command->ticket;
          strncpy(add.name,command->name,MAX_NAMESIZE);
          pUser->scene->sendCmd(&add,sizeof(Cmd::Session::t_OvermanTicketAdd));
        }
        return true;
      }
      break;

    case Cmd::Session::QUEST_BULLETIN_USERCMD_PARA:
      {
        Cmd::Session::t_QuestBulletinUserCmd* command = (Cmd::Session::t_QuestBulletinUserCmd*)cmd;
        if (command->kind == 1) {
          CUnionM::getMe().sendUnionNotify(command->id,command->content);
          return true;
        }

        if (command->kind == 2) {
          CSeptM::getMe().sendSeptNotify(command->id,command->content);
          return true;
        }
      }
      break;

    case Cmd::Session::QUEST_CHANGE_AP:
      {
        Cmd::Session::t_ChangeAP* command = (Cmd::Session::t_ChangeAP*) cmd;
        CUnion* u = CUnionM::getMe().getUnionByID(command->id);
        if (u) {
          u->changeActionPoint(command->point);
        }
      }
      break;      

    case Cmd::Session::PARA_CHANGE_USER_DATA:
      {
        Cmd::Session::t_changeUserData_SceneSession* rev = 
          (Cmd::Session::t_changeUserData_SceneSession*)cmd;

        UserSession* pUser = UserSessionManager::getInstance()->getUserByID(rev->dwUserID);
        if (pUser)
        {
          pUser->level = rev->wdLevel;
          pUser->dwExploit = rev->dwExploit;
        }

        return true;
      }
      break;

	case Cmd::Session::GLOBAL_MESSAGE_PARA:
	{
		Cmd::Session::t_GlobalMessage_SceneSession *rev = (Cmd::Session::t_GlobalMessage_SceneSession*)cmd;
		UserSession* pUser = UserSessionManager::getInstance()->getUserByID(rev->dwUserID);
		if(pUser)
		{
			std::string msg(pUser->name);
			msg = msg + ":" + rev->msg;
		    //char *msg = new char[strlen("")]
			SessionChannel::sendAllInfo(Cmd::INFO_TYPE_SCROLL,msg.c_str());
		}
		return true;
	}
	break;

    case Cmd::Session::PARA_AUCTION_CMD:
      {
        Cmd::Session::t_AuctionCmd * rev = (Cmd::Session::t_AuctionCmd *)cmd;
        return AuctionService::getMe().doAuctionCmd(rev,cmdLen);
      }
      break;
    case Cmd::Session::PARA_CARTOON_CMD:
      {
        Cmd::Session::t_CartoonCmd * rev = (Cmd::Session::t_CartoonCmd *)cmd;
        return CartoonPetService::getMe().doCartoonCmd(rev,cmdLen);
      }
      break;
    case Cmd::Session::PARA_SERVER_NOTIFY:
      {
        Cmd::Session::t_serverNotify_SceneSession* rev = 
          (Cmd::Session::t_serverNotify_SceneSession*)cmd;

        SessionChannel::sendAllInfo(rev->infoType,rev->info);
        return true;
      }
      break;
    case Cmd::Session::PARA_COUNTRY_NOTIFY:
      {
        Cmd::Session::t_countryNotify_SceneSession* rev = 
          (Cmd::Session::t_countryNotify_SceneSession*)cmd;

        SessionChannel::sendCountryInfo(rev->infoType,
            rev->dwCountryID,"%s",rev->info);
        return true;
      }
      break;
    case Cmd::Session::PARA_CHANGE_COUNTRY:
      {
        Cmd::Session::t_changeCountry_SceneSession* rev = 
          (Cmd::Session::t_changeCountry_SceneSession*)cmd;
        this->change_country(rev);

        return true;
      }
      break;
#endif
    case Cmd::Session::PARA_SCENE_FORWARD_USER:
      {
        Cmd::Session::t_forwardUser_SceneSession * rev = (Cmd::Session::t_forwardUser_SceneSession *)cmd;

        UserSession* pUser = 0;
        if (rev->id)
          pUser = UserSessionManager::getInstance()->getUserByID(rev->id);
        if (!pUser && rev->tempid)
          pUser = UserSessionManager::getInstance()->getUserByTempID(rev->id);
        if (!pUser && !strncmp("",rev->name,MAX_NAMESIZE))
          pUser = UserSessionManager::getInstance()->getUserSessionByName(rev->name);

        if (pUser)
          pUser->sendCmdToMe(rev->cmd,rev->cmd_len);

        return true;
      }
      break;
#if 0
    case Cmd::Session::PARA_RETURN_OBJECT:
      {
        Cmd::Session::t_returnObject_SceneSession* rev = (Cmd::Session::t_returnObject_SceneSession*)cmd;
        UserSession* pUser = UserSessionManager::getInstance()->getUserSessionByName(rev->to_name);
        Cmd::stReturnQuestionObject send;

        if (pUser)
        {
          strncpy(send.name,rev->from_name,MAX_NAMESIZE);
          bcopy(&send.object,&rev->object,sizeof(t_Object));
          pUser->sendCmdToMe(&send,sizeof(Cmd::stReturnQuestionObject));
        }

        return true;
      }
      break;
#endif
    case Cmd::Session::PARA_SCENE_REGSCENE:
      {
		  Cmd::Session::t_regScene_SceneSession *reg=(Cmd::Session::t_regScene_SceneSession *)cmd;
		  SceneSession *scene=new SceneSession(this);
		  Cmd::Session::t_regScene_ret_SceneSession ret;
		  ret.dwTempID=reg->dwTempID;
		  if(SceneSessionManager::getInstance()->getSceneByTempID(reg->dwTempID))
		  {
			  //printf("发现重复注册消息,session不做处理(%s-%d-%d)\n", reg->byName, reg->dwID, reg->dwTempID);
			  return true;
		  }

		  if (scene->reg(reg) && SceneSessionManager::getInstance()->addScene(scene))
		  {
			  ret.byValue=Cmd::Session::REGSCENE_RET_REGOK;
		//	  CCountryM::getMe().refreshTax();
		//	  CCountryM::getMe().refreshTech(this,reg->dwCountryID);
		//	  if (KING_CITY_ID==(reg->dwID&0x0000ffff))
		//		  CCountryM::getMe().refreshGeneral(reg->dwCountryID);
		//	  CAllyM::getMe().refreshAlly(this);

			  Zebra::logger->info("session reg map %u(%s %s) OK!!!",reg->dwID,reg->byName,reg->fileName);
		//	  CCityM::getMe().refreshUnion(reg->dwCountryID,reg->dwID & 0x0FFF);

			  SceneSession * sc = SceneSessionManager::getInstance()->getSceneByID(reg->dwID);
			  if(!sc)
			  {
				  //fprintf(stderr,"bad\n");
			  }

			  return true;
		  }
		  else
		  {
			  ret.byValue=Cmd::Session::REGSCENE_RET_REGERR;
			  Zebra::logger->error("session reg map%u(%s %s)error !!!",reg->dwID,reg->byName,reg->fileName);
		  }
		  sendCmd(&ret,sizeof(ret));
		  return true;
	  }
	  break;
    case Cmd::Session::PARA_SCENE_UNREGUSER:
      {
        Cmd::Session::t_unregUser_SceneSession *unreg=(Cmd::Session::t_unregUser_SceneSession *)cmd;
        UserSession *pUser=UserSessionManager::getInstance()->getUserByID(unreg->dwUserID);
        if (pUser)
        {
#if 0
          CSortM::getMe().offlineCount(pUser);
          CUnionM::getMe().userOffline(pUser); // 用于处理帮会成员下线
          CSchoolM::getMe().userOffline(pUser);
          CSeptM::getMe().userOffline(pUser);
          CQuizM::getMe().userOffline(pUser);
          CGemM::getMe().userOffline(pUser);
#endif
          UserSessionManager::getInstance()->removeUser(pUser);
          if (unreg->retcode==Cmd::Session::UNREGUSER_RET_ERROR)
            Zebra::logger->info("ScenesServer错误，注销%s(%u)",pUser->name,pUser->id);
          else
            Zebra::logger->info("场景请求用户%s(%u)注销",pUser->name,pUser->id);
          SAFE_DELETE(pUser);
        }
        return true;
      }
    case Cmd::Session::PARA_SCENE_REGUSERSUCCESS:
      {
        Cmd::Session::t_regUserSuccess_SceneSession *regsuccess=(Cmd::Session::t_regUserSuccess_SceneSession *)cmd;
        UserSession *pUser=UserSessionManager::getInstance()->getUserByID(regsuccess->dwUserID);
        if (pUser)
        {
          pUser->dwExploit = regsuccess->dwExploit;
          pUser->dwUseJob = regsuccess->dwUseJob;
          pUser->qwExp = regsuccess->qwExp;
          pUser->setRelationData(regsuccess);
          pUser->relationManager.init();    // 初始化这个用户的好友列表

          pUser->updateConsort();      // 更新配偶状态到场景
	  CSortM::getMe().onlineCount(pUser);
          if (pUser->level >= 50 && SessionService::getInstance().uncheckCountryProcess)
          {
            typedef std::map<DWORD,BYTE>::value_type ValueType;
            std::pair<std::map<DWORD,BYTE>::iterator,bool> retval;
            retval = SessionService::userMap.insert(ValueType(pUser->id,pUser->level));
            if (retval.second) SessionService::getInstance().countryLevel[pUser->country]+=pUser->level;
          }
          //CNpcDareM::getMe().sendUserData(pUser);
          //CSortM::getMe().onlineCount(pUser);
 //         CUnionM::getMe().userOnline(pUser); // 当用户RecordServer读档完毕以后再通知上线
 //         CSchoolM::getMe().userOnline(pUser);
          //CSeptM::getMe().userOnline(pUser);
 //         CQuizM::getMe().userOnline(pUser);
          //CCountryM::getMe().userOnline(pUser);
          //CArmyM::getMe().userOnline(pUser);
//        CGemM::getMe().userOnline(pUser);
//        CAllyM::getMe().userOnline(pUser);
//        CDareM::getMe().userOnline(pUser);
		  
#if 0
		  //sky 查询临时列表里是否有该用户
		  g_MoveSceneMemberMapLock.lock();
		  std::map<DWORD,DWORD>::iterator iter;
		  iter = MoveSceneMemberMap.find(regsuccess->dwUserID);

		  if(iter != MoveSceneMemberMap.end())
		  {
			  DWORD TeamID = iter->second;
			  Team * pteam = GlobalTeamIndex::getInstance()->GetpTeam(TeamID);
			  if(pteam)
			  {
				  Cmd::Session::t_Team_AddMe rev;
				  rev.LeaberID = pteam->GetLeader();
				  rev.TeamThisID = TeamID;
				  rev.MeID = regsuccess->dwUserID;

				  //sky 发送用户添加队伍成员
				  pUser->scene->sendCmd( &rev, sizeof(Cmd::Session::t_Team_AddMe) );
			  }
			  //sky 清除掉临时列表里的该用户信息
			  MoveSceneMemberMap.erase(iter);
		  }
		  g_MoveSceneMemberMapLock.unlock();
#endif
#if 0
          WORD degree = CSortM::getMe().getLevelDegree(pUser);
          Cmd::stLevelDegreeDataUserCmd send;
          send.degree = degree;
          pUser->sendCmdToMe(&send,sizeof(send));

          COfflineMessage::getOfflineMessage(pUser);
#endif
        }
        return true;
      }
      break;
#if 0
    case Cmd::Session::PARA_TAXADD_COUNTRY:
      {
        Cmd::Session::t_taxAddCountry_SceneSession *rev=(Cmd::Session::t_taxAddCountry_SceneSession *)cmd;
        CCountry *pCountry = CCountryM::getMe().find(rev->dwCountryID);
        if (pCountry)
        {
          pCountry->addTaxMoney(rev->qwTaxMoney);
        }
        return true;
      }
      break;
    case Cmd::Session::PARA_FRIENDDEGREE_REQUEST:
      {
        Cmd::Session::t_RequestFriendDegree_SceneSession *rev=(Cmd::Session::t_RequestFriendDegree_SceneSession *)cmd;

        for (int i=0; i< rev->size; i++)
        {
          UserSession *pUser=UserSessionManager::getInstance()->getUserSessionByName(rev->namelist[i].name);
          if (pUser)
          {
            pUser->sendFriendDegree(rev);
          }
        }
        return true;
      }
      break;		
    case Cmd::Session::PARA_FRIENDDEGREE_COUNT:
      {
        Cmd::Session::t_CountFriendDegree_SceneSession *rev=(Cmd::Session::t_CountFriendDegree_SceneSession *)cmd;
        UserSession *pMainUser=UserSessionManager::getInstance()->getUserSessionByName(rev->name);
        if (pMainUser)
        {
          pMainUser->setFriendDegree(rev);
        }
        CSchoolMember *member = CSchoolM::getMe().getMember(rev->name);
        if (member) member->setFriendDegree(rev);
        return true;
      }
      break;
    case Cmd::Session::PARA_SCENE_LEVELUPNOTIFY:
      {
        Cmd::Session::t_levelupNotify_SceneSession *rev=(Cmd::Session::t_levelupNotify_SceneSession *)cmd;
        UserSession *pUser=UserSessionManager::getInstance()->getUserByID(rev->dwUserID);
        if (pUser)
        {
          pUser->level = rev->level;
          pUser->qwExp = rev->qwExp;

          CSortM::getMe().upLevel(pUser); //角色排名系统刷新
          WORD degree = CSortM::getMe().getLevelDegree(pUser);
          Cmd::stLevelDegreeDataUserCmd send;
          send.degree = degree;
          pUser->sendCmdToMe(&send,sizeof(send));

          CSchoolM::getMe().setUserLevel(pUser->name,rev->level);
          CartoonPetService::getMe().userLevelUp(pUser->id,rev->level);
        }
        return true;
      }
      break;
      //队伍信息
	case Cmd::Session::PARA_USER_TEAM_ADDMEMBER:
		{
			Cmd::Session::t_Team_AddMember *rev = (Cmd::Session::t_Team_AddMember *)cmd;
			UserSession * pUser = (UserSession *)UserSessionManager::getInstance()->getUserByName(rev->AddMember.name);
			if(pUser)
				GlobalTeamIndex::getInstance()->addMember(rev->dwTeam_tempid, rev->dwLeaderID,pUser->id);
			return true;
		}
		break;
	case Cmd::Session::PARA_USER_TEAM_DELMEMBER:
		{
			Cmd::Session::t_Team_DelMember *rev = (Cmd::Session::t_Team_DelMember *)cmd;
			GlobalTeamIndex::getInstance()->delMember(rev->dwTeam_tempid,rev->MemberNeam);
			return true;
		}
		break;
	case Cmd::Session::PARA_USE_TEAM_ADDMOVESCENEMAMBER:	//sky把离线的队伍成员放到临时列表里
		{
			Cmd::Session::t_Team_AddMoveSceneMember * rev = (Cmd::Session::t_Team_AddMoveSceneMember*)cmd;
			g_MoveSceneMemberMapLock.lock();
			MoveSceneMemberMap[rev->MemberID] = rev->TeamThisID;
			g_MoveSceneMemberMapLock.unlock();

			GlobalTeamIndex::getInstance()->UpDataMapID(rev->MemberID, rev->TeamThisID);
		}
		break;
	case Cmd::Session::PARA_USER_TEAM_CHANGE_LEADER:		//sky新增跟换队长消息
		{
			Cmd::Session::t_Team_ChangeLeader *rev = (Cmd::Session::t_Team_ChangeLeader *)cmd;
			if(rev->NewLeaderName[0] == 0)
				GlobalTeamIndex::getInstance()->ChangeLeader(rev->dwTeam_tempid);
			else
				GlobalTeamIndex::getInstance()->ChangeLeader(rev->dwTeam_tempid, rev->NewLeaderName);

			return true;
		}
		break;
	case Cmd::Session::PARA_USE_TEAM_DELTEAM:				//sky 删除队伍
		{
			Cmd::Session::t_Team_DelTeam * rev = (Cmd::Session::t_Team_DelTeam *)cmd;
			GlobalTeamIndex::getInstance()->DelTeam(rev->TeamThisID);
		}
		break;
	case Cmd::Session::PARA_USER_TEAM_REQUEST_TEAM:			//sky 请求组队消息[跨场景组队用]
		{
			Cmd::Session::t_Team_RequestTeam * rev = (Cmd::Session::t_Team_RequestTeam *)cmd;
			UserSession *pUser=(UserSession*)(UserSessionManager::getInstance()->getUserByName(rev->byAnswerUserName));
			
			//sky 找到回答者所在的场景通知其处理请求消息
			if(pUser)
				pUser->scene->sendCmd(rev, sizeof(Cmd::Session::t_Team_RequestTeam));
			else
			{
				UserSession *RpUser = (UserSession *)(UserSessionManager::getInstance()->getUserByName(rev->byRequestUserName));

				if(RpUser)
					RpUser->sendSysChat(Cmd::INFO_TYPE_SYS,"请求组队时无法找到玩家:[%s],该玩家可能已经下线!", rev->byAnswerUserName);
			}
		}
		break;
	case Cmd::Session::PARA_USE_TEAM_ANSWER_TEAM:			//sky 回答组队消息[跨场景组队用]
		{
			Cmd::Session::t_Team_AnswerTeam * rev = (Cmd::Session::t_Team_AnswerTeam *)cmd;
			UserSession * pUser = (UserSession*)(UserSessionManager::getInstance()->getUserByName(rev->byRequestUserName));

			//sky 找到请求者所在的场景通知其处理回答消息
			if(pUser)
				pUser->scene->sendCmd(rev, sizeof(Cmd::Session::t_Team_AnswerTeam));
			else
			{
				UserSession *RpUser = (UserSession*)(UserSessionManager::getInstance()->getUserByName(rev->byRequestUserName));

				if(RpUser)
					RpUser->sendSysChat(Cmd::INFO_TYPE_SYS,"回答组队时候无法找到玩家:[%s],该玩家可能已经下线!", rev->byAnswerUserName);
			}
		}
		break;
#endif

      //请求读临时档案
    case Cmd::Session::PARA_USER_ARCHIVE_REQ:
      {
        //TODO
        Cmd::Session::t_ReqUser_SceneArchive *rev=(Cmd::Session::t_ReqUser_SceneArchive *)cmd;
        SceneSession *scene= SceneSessionManager::getInstance()->getSceneByTempID(rev->dwMapTempID);
        if (!scene)
        {
          return true;
        }
        char buf[sizeof(Cmd::Session::t_ReadUser_SceneArchive) + MAX_TEMPARCHIVE_SIZE];
        if (buf)
        {
          Cmd::Session::t_ReadUser_SceneArchive *ret = (Cmd::Session::t_ReadUser_SceneArchive *)buf;
          constructInPlace(ret);
          ret->id = rev->id;
          ret->dwMapTempID = rev->dwMapTempID;
          ret->dwSize = MAX_TEMPARCHIVE_SIZE;
          if (GlobalTempArchiveIndex::getInstance()->readTempArchive(ret->id,ret->data,ret->dwSize))
          {
            scene->sendCmd(ret,sizeof(Cmd::Session::t_ReadUser_SceneArchive) + ret->dwSize);
            Zebra::logger->info("发送临时读档数据包%u",sizeof(Cmd::Session::t_ReadUser_SceneArchive) + ret->dwSize);
          }
        }
        return true;
      }
      break;
      //请求写临时档案
    case Cmd::Session::PARA_USER_ARCHIVE_WRITE:
      {
        //TODO
        Cmd::Session::t_WriteUser_SceneArchive *rev=(Cmd::Session::t_WriteUser_SceneArchive *)cmd;
        GlobalTempArchiveIndex::getInstance()->writeTempArchive(rev->id,rev->data,rev->dwSize);
        return true;
      }
      break;

    case Cmd::Session::PARA_SCENE_CHANEG_SCENE:
      {
	  Cmd::Session::t_changeScene_SceneSession *rev=(Cmd::Session::t_changeScene_SceneSession *)cmd;
	  SceneSession* scene = NULL;
	  if ((char)rev->map_file[0]) {
	      scene = SceneSessionManager::getInstance()->getSceneByFile((char*) rev->map_file);
	  }else if (rev->map_id){
	      scene = SceneSessionManager::getInstance()->getSceneByID(rev->map_id);
	      //Zebra::logger->debug("地图id=%u",rev->map_id);
	  }else{
	      scene = SceneSessionManager::getInstance()->getSceneByName((char*) rev->map_name);
	      //Zebra::logger->debug("地图名称%s",rev->map_name);
	  }
#ifdef _MOBILE
	  if(scene)	//���Ƿ���
	  {
	      std::vector<std::string> vs;
	      vs.clear();
	      char *pCountryName = NULL;
	      Zebra::stringtok(vs, scene->name, "��");
	      if(vs.size() == 2)
	      {
		  pCountryName = (char*)(vs[0].c_str());
		  if(0 == strcmp("��ճ�",vs[1].c_str()))
		  {
		      std::string sNewbieMapName = "";
		      sNewbieMapName = NewbieLimit::getNewbieMapName(pCountryName);
		      snprintf((char*)rev->map_name, sizeof(rev->map_name),
			      "%s��%s", vs[0].c_str(), sNewbieMapName.c_str());

		      scene = SceneSessionManager::getInstance()->getSceneByName((char*)rev->map_name);
		  }
	      }
	  }
#endif
	  if (!scene) return true;

	  UserSession *pUser = UserSessionManager::getInstance()->getUserByID(rev->id);

	  if (scene->level>0)
	  {
	      if (pUser)
	      {
		  if (pUser->level < scene->level)
		  {
		      pUser->sendSysChat(Cmd::INFO_TYPE_GAME,"%s不对等级低于%d级的玩家开放！",scene->name,scene->level);
		      Zebra::logger->info("[GOMAP]玩家%s等级不够跳到地图[%s]失败!",pUser->name,scene->name);
		      return true;
		  }
	      }
	      //else return true;
	  }


	  //Zebra::logger->debug("场景信息%s,%u,%u",scene->name,scene->id,scene->tempid);
	  //Zebra::logger->debug("场景信息%s,%s,%u",rev->map_file,rev->map_name,rev->map_id);
	  Cmd::Session::t_changeScene_SceneSession ret;
	  ret.id = rev->id;
	  ret.x = rev->x;
	  ret.y = rev->y;
	  ret.map_id = rev->map_id;

	  if (scene) {
	      ret.temp_id = scene->tempid;
	      strncpy((char *)ret.map_file,scene->file.c_str(),MAX_NAMESIZE);
	      strncpy((char *)ret.map_name,scene->name,MAX_NAMESIZE);
	  }
	  else 
	  {
	      ret.temp_id = (DWORD)-1;
	  }

	  sendCmd(&ret,sizeof(ret));
	  return true;
      }
      break;
    case Cmd::Session::PARA_SCENE_GM_COMMAND:
      {

        Cmd::Session::t_gmCommand_SceneSession * rev = (Cmd::Session::t_gmCommand_SceneSession *)cmd;

        switch (rev->gm_cmd)
        {
	    case Cmd::Session::GM_COMMAND_LOAD_QUEST:
	    case Cmd::Session::GM_COMMAND_LOAD_CARD_EFFECT:
		{
		    return SessionTaskManager::getInstance().broadcastScene(cmd, cmdLen);
		}
		break;
	    case Cmd::Session::GM_COMMAND_LOAD_AUTO_XML_CONFIG:
		{
		    if(!strncmp((char*)rev->src_name, "all", 4))
		    {
			xml::Configs::load_session();
		    }
		    else
		    {
			std::string file((char*)rev->src_name);
			xml::Configs::reload_config(file);
			xml::Configs::dump_config(file);
		    }
		    return SessionTaskManager::getInstance().broadcastScene(cmd, cmdLen);
		}
		break;
#if 0
            case Cmd::Session::GM_COMMAND_LOAD_GIFT:
                return Gift::getMe().init();
                break;
            case Cmd::Session::GM_COMMAND_LOAD_PROCESS:
                return SessionTaskManager::getInstance().broadcastScene(cmd,cmdLen);
                break;
            case Cmd::Session::GM_COMMAND_NEWZONE:
                return SessionTaskManager::getInstance().broadcastScene(cmd,cmdLen);
                break;
            case Cmd::Session::GM_COMMAND_REFRESH_GENERAL:
                CCountryM::getMe().refreshGeneral(0);
                return true;
#endif
            default:
                break;
        }

        UserSession * pUser = UserSessionManager::getInstance()->getUserSessionByName((char *)rev->dst_name);
        if (0==pUser)
        {
          Cmd::Session::t_gmCommand_SceneSession ret;
          ret.gm_cmd = rev->gm_cmd;
          strncpy((char *)ret.dst_name,(char *)rev->src_name,MAX_NAMESIZE);
          strncpy((char *)ret.src_name,(char *)rev->dst_name,MAX_NAMESIZE);
          ret.err_code = Cmd::Session::GM_COMMAND_ERR_NOUSER;

          ret.cmd_state = Cmd::Session::GM_COMMAND_STATE_RET;
          sendCmd(&ret,sizeof(ret));
          return true;
        }
	if(pUser->scene)
	{
	    pUser->scene->sendCmd(rev,sizeof(Cmd::Session::t_gmCommand_SceneSession));
	}
	else
	{
	    if(pUser && rev->gm_cmd==Cmd::Session::GM_COMMAND_KICK)
	    {
		Cmd::Session::t_unregUser_SceneSession send;
		send.retcode = Cmd::Session::UNREGUSER_RET_ERROR;
		send.dwUserID = pUser->id;
		this->msgParse_Scene(&send, sizeof(send));
	    }
	}
        return true;
      }
      break;
#if 0
    case Cmd::Session::PARA_SCENE_CREATE_SCHOOL:
      {
        Cmd::Session::t_createSchool_SceneSession * rev = (Cmd::Session::t_createSchool_SceneSession *)cmd;

        UserSession * pUser = UserSessionManager::getInstance()->getUserSessionByName((char *)rev->masterName);
        if (!pUser) return false;
        if (CSchoolM::getMe().createNewSchool(pUser->name,(char *)rev->schoolName))
        {
          Cmd::Session::t_SchoolCreateSuccess_SceneSession send;
          send.dwID = pUser->id;
          send.dwSchoolID = pUser->schoolid;
          strncpy(send.schoolName,rev->schoolName,MAX_NAMESIZE);
          pUser->scene->sendCmd(&send,sizeof(send));
        }
        else
        {
          pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,"遗憾的通知你门派名称已被占用，请换个名字再试试!");
        }
        return true;
      }
      break;
#endif
    case Cmd::Session::PARA_SCENE_PRIVATE_CHAT:
      {
        Cmd::Session::t_privateChat_SceneSession * rev = (Cmd::Session::t_privateChat_SceneSession *)cmd;
        UserSession * pUser = UserSessionManager::getInstance()->getUserSessionByName((char *)rev->dst_name);
        if (0==pUser)
        {
          if (strstr((char *)rev->dst_name,"GM")
              || strstr((char *)rev->dst_name,"gm"))  return true;//发给GM的私聊不返回

          Cmd::Session::t_privateChat_SceneSession ret;
          ret.err_code = Cmd::Session::PRIVATE_CHAT_ERR_NOUSER;
          strncpy((char *)ret.src_name,(char *)rev->dst_name,MAX_NAMESIZE);
          strncpy((char *)ret.dst_name,(char *)rev->src_name,MAX_NAMESIZE);
          //bcopy(rev->chat_cmd,ret.chat_cmd,rev->cmd_size);
          //sendCmd(&ret,sizeof(Cmd::Session::t_privateChat_SceneSession)+rev->cmd_size);
          sendCmd(&ret,sizeof(ret));
          return true;
        }

        pUser->scene->sendCmd(rev,cmdLen);
        return true;
      }
      break;
    case Cmd::Session::PARA_SCENE_SYS_SETTING:
      {
        Cmd::Session::t_sysSetting_SceneSession * rev = (Cmd::Session::t_sysSetting_SceneSession *)cmd;
        UserSession * pUser = UserSessionManager::getInstance()->getUserSessionByName((char *)rev->name);
        if (pUser)
        {
          bcopy(rev->sysSetting,pUser->sysSetting,sizeof(pUser->sysSetting));
         // pUser->face = rev->face;
          return true;
        }
      }
      break;
    case Cmd::Session::PARA_SCENE_CITY_RUSH:
      {
        //Zebra::logger->debug("收到rush公告");
        Cmd::Session::t_cityRush_SceneSession * rev = (Cmd::Session::t_cityRush_SceneSession *)cmd;
        char content[MAX_CHATINFO];
        sprintf(content,"BOSS %s 被杀死了,他的灵魂得到了魔神的救赎，将在 %d 分钟后对 %s 发起 %s",rev->bossName,rev->delay/60,rev->mapName,rev->rushName);
        SessionChannel::sendCountryInfo(Cmd::INFO_TYPE_GAME,rev->countryID,content);
        /*
           broadcastRushToEveryUser b;
           if (b.init(content))
           UserSessionManager::getInstance()->execEveryUser(b);
         */
        return true;
      }
      break;
    case Cmd::Session::PARA_SCENE_CITY_RUSH_CUST:
      {
        Cmd::Session::t_cityRushCust_SceneSession * rev = (Cmd::Session::t_cityRushCust_SceneSession *)cmd;
    if (strncmp("  ",rev->text,128))
        SessionChannel::sendCountryInfo(Cmd::INFO_TYPE_GAME,rev->countryID,rev->text);
        return true;
      }
      break;
    case Cmd::Session::PARA_SCENE_REMOVE_SCENE:
      {
        Cmd::Session::t_removeScene_SceneSession *rev = (Cmd::Session::t_removeScene_SceneSession*)cmd;
        SceneSession *scene= SceneSessionManager::getInstance()->getSceneByID(rev->map_id);
        if (scene)
        {
          SceneSessionManager::getInstance()->removeScene(scene);
          Zebra::logger->info("卸载地图%u(%s) 成功",scene->id,scene->name);
          SAFE_DELETE(scene);
        }
        return true;
      }
      break;
    case Cmd::Session::PARA_SCENE_REQ_ADD_SCENE:
      {
        Cmd::Session::t_reqAddScene_SceneSession *rev = (Cmd::Session::t_reqAddScene_SceneSession*)cmd;
        Zebra::logger->info("转发加载地图消息(%u,%u,%u)",rev->dwServerID,rev->dwCountryID,rev->dwMapID);
        SessionTaskManager::getInstance().broadcastByID(rev->dwServerID,
            rev,sizeof(Cmd::Session::t_reqAddScene_SceneSession));
        return true;
      }
      break;
    case Cmd::Session::PARA_SCENE_UNLOAD_SCENE:
      {
        Cmd::Session::t_unloadScene_SceneSession *rev = (Cmd::Session::t_unloadScene_SceneSession*)cmd;
        SceneSession *scene= SceneSessionManager::getInstance()->getSceneByID(rev->map_id);
	if(!scene)
	    scene = SceneSessionManager::getInstance()->getSceneByTempID(rev->map_tempid);
        if (scene)
        {
          //TODO
          //设置不可注册标志
          scene->setRunningState(SCENE_RUNNINGSTATE_UNLOAD);
          scene->sendCmd(rev,sizeof(Cmd::Session::t_unloadScene_SceneSession));
          Zebra::logger->info("地图%s因卸载地图停止注册",scene->name);
          /*
             SceneSessionManager::getInstance()->removeScene(scene);
             struct UnloadSceneSessionExec :public execEntry<UserSession>
             {
             SceneSession *scene;
             std::vector<DWORD> del_vec;
             UnloadSceneSessionExec(SceneSession *s):scene(s)
             {
             }
             bool exec(UserSession *u)
             {
             if (u->scene->id == scene->id)
             {
             del_vec.push_back(u->id);
             }
             return true;
             }
             };
             UnloadSceneSessionExec exec(scene);
             UserSessionManager::getInstance()->execEveryUser(exec);
             for(std::vector<DWORD>::iterator iter = exec.del_vec.begin() ; iter != exec.del_vec.end() ; iter ++)
             {
             UserSession *pUser=UserSessionManager::getInstance()->getUserByID(*iter);
             if (pUser)
             {
             Zebra::logger->info("用户%s(%u)因卸载地图注销",pUser->name,pUser->id);
             UserSessionManager::getInstance() ->removeUser(pUser);
             SAFE_DELETE(pUser);
             }
             }
             Zebra::logger->debug("卸载地图%u(%s) 成功",scene->id,scene->name);
             SAFE_DELETE(scene);
          // */
        }
        return true;
      }
      break;
    case Cmd::Session::PARA_SCENE_GUARD_FAIL:
      {
        Cmd::Session::t_guardFail_SceneSession * rev = (Cmd::Session::t_guardFail_SceneSession *)cmd;
        UserSession *pUser=UserSessionManager::getInstance()->getUserByTempID(rev->userID);
        if (pUser)
          pUser->scene->sendCmd(rev,cmdLen);
        return true;
      }
      break;
      /*
    case Cmd::Session::PARA_SCENE_LOAD_PROCESS:
      {
        SessionTaskManager::getInstance().broadcastScene(cmd,cmdLen);
        return true;
      }
      break;
      */
    default:
      break;
  }

  Zebra::logger->error("SessionTask::msgParse_Scene(%u,%u,%u)",cmd->cmd,cmd->para,cmdLen);
  return false;
}

bool SessionTask::msgParse_Gate(const Cmd::t_NullCmd *cmd,const DWORD cmdLen)
{
  switch(cmd->para)
  {
    case Cmd::Session::PARA_GATE_REGUSER:
      {
        Cmd::Session::t_regUser_GateSession *reg=(Cmd::Session::t_regUser_GateSession *)cmd;
        SceneSession *scene= SceneSessionManager::getInstance()->getSceneByName((char *)reg->byMapName);
	SceneSession *oldscene = scene;
#ifdef _MOBILE
	if(scene)	//���Ƿ���
	{
	    std::vector<std::string> vs;
	    vs.clear();
	    Zebra::stringtok(vs, scene->name, "��");
	    if(vs.size() == 2)
	    {
		if(0 == strcmp("��ճ�",vs[1].c_str()))
		{
		    const char *pCountryName = (const char*)reg->byCountryName;
		    std::string sNewbieMapName = "";
		    sNewbieMapName = NewbieLimit::getNewbieMapName(pCountryName);
		    snprintf((char*)reg->byMapName, sizeof(reg->byMapName),
			    "%s��%s", pCountryName, sNewbieMapName.c_str());

		    scene = SceneSessionManager::getInstance()->getSceneByName((char*)reg->byMapName);

		    if(scene)
		    {
			reg->dwMapID = scene->id;
		    }
		}
	    }
	}
#endif
	if (!scene||(reg->wdLevel<scene->level))
	{  
	    const char *pCountryName = (const char*)reg->byCountryName;
	    std::string sNewbieMapName = "";
	    sNewbieMapName = NewbieLimit::getNewbieMapName(pCountryName);
	    snprintf((char*)reg->byMapName, sizeof(reg->byMapName),
		    "%s��%s", pCountryName, sNewbieMapName.c_str());

	    scene = SceneSessionManager::getInstance()->getSceneByName((char*)reg->byMapName);

	    if(scene)
	    {
		reg->dwMapID = scene->id;
	    }
	    else
	    {//�������崻��ˣ�ȥ��������Ԫ�ؽ�
		snprintf((char*)reg->byMapName, sizeof(reg->byMapName), "��������Ԫ�ؽ�");
		scene = SceneSessionManager::getInstance()->getSceneByName((char*)reg->byMapName);
		if(scene)
		{
		    reg->dwMapID = scene->id;
		}
	    }
	}

        if (scene)
        {
          if (scene->getRunningState() == SCENE_RUNNINGSTATE_NORMAL)
          {
            UserSession *pUser=UserSessionManager::getInstance()->getUserByID(reg->dwID);
            if (!pUser)
            {
              pUser=new UserSession(this);
              if (pUser && pUser->reg(reg))
              {
                //Zebra::logger->debug("创建用户Session成功");
		pUser->setScene(scene);

                if (UserSessionManager::getInstance()->addUser(pUser))
                {
                  //场景读档案
                  Cmd::Session::t_regUser_SceneSession reginscene;
                  reginscene.accid = reg->accid;
                  reginscene.dwID = reg->dwID;
                  reginscene.dwTempID = reg->dwTempID;
                  reginscene.dwMapID = scene->id;
                  bcopy(reg->byName,reginscene.byName,MAX_NAMESIZE+1);
                  bcopy(scene->name,reginscene.byMapName,MAX_NAMESIZE+1);
                  reginscene.dwGatewayServerID = pUser->getTask()->getID();
                  scene->sendCmd(&reginscene,sizeof(reginscene));

                  Zebra::logger->info("addUser�û�ע��ɹ�%s(%u)",pUser->name,pUser->id);

                  //CartoonPetService::getMe().userOnline(pUser);

		  Cmd::Super::stUserOnlineBroadCast send;
		  strncpy(send.name, pUser->name, MAX_NAMESIZE);
		  SessionService::getInstance().sendCmdToSuperServer(&send, sizeof(send));

                  return true;
                }
                else
                {
                  UserSession *pUser=UserSessionManager::getInstance()->getUserByID(reg->dwID);
                  if (pUser)
                  {
                    Zebra::logger->debug("addUserʧ�� id�ظ�(id=%u,name=%s,tempid=%u" 
                        ,pUser->id,pUser->name,pUser->tempid);
                  }
                  pUser=UserSessionManager::getInstance()->getUserByTempID(reg->dwTempID);
                  if (pUser)
                  {
                    Zebra::logger->debug("addUserʧ�� tempid�ظ�(id=%u,name=%s,tempid=%u" 
                        ,pUser->id,pUser->name,pUser->tempid);
                  }
                  Zebra::logger->error("addUserʧ�� �����ǽ�ɫ�ظ���¼");
                }
                SAFE_DELETE(pUser);
              }
              else
                Zebra::logger->fatal("ע���û�ʱ �����ڴ�ʧ��(%u,%u,%s)",reg->accid,reg->dwID,reg->byName);
            }
            else
            {
#if 0
              CSortM::getMe().offlineCount(pUser);
              CUnionM::getMe().userOffline(pUser); // 用于处理帮会成员下线
              CSchoolM::getMe().userOffline(pUser);
              CSeptM::getMe().userOffline(pUser);
              CQuizM::getMe().userOffline(pUser);
              CGemM::getMe().userOffline(pUser);
#endif
              UserSessionManager::getInstance()->removeUser(pUser);

              //通知网关错误注销
              Cmd::Session::t_unregUser_GateSession ret;
              ret.dwUserID=reg->dwID;
              ret.dwSceneTempID=scene->tempid;
              ret.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
              sendCmd(&ret,sizeof(ret));

              //通知场景错误注销
              Cmd::Session::t_unregUser_SceneSession send;
              send.dwUserID=reg->dwID;
              send.dwSceneTempID=scene->tempid;
              send.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
              scene->sendCmd(&send,sizeof(send));

	      if(oldscene)
	      {
		  if(oldscene->tempid != scene->tempid)
		  {
		      Cmd::Session::t_unregUser_SceneSession send2;
		      send2.dwUserID=reg->dwID;
		      send2.dwSceneTempID=oldscene->tempid;
		      send2.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
		      scene->sendCmd(&send2, sizeof(send2));
		      Zebra::logger->debug("�ظ��û��ɳ���ע�� %s(%u)",pUser->name,pUser->id);
		  }
	      }
              Zebra::logger->info("�û�ע��ɹ� %s(%u)",pUser->name,pUser->id);
              SAFE_DELETE(pUser);
              return true;
            }
          }
          else
          {
            Zebra::logger->info("���� %s ���ڲ�����ע���û�",(char *)reg->byMapName);
          }
        }
        else
          Zebra::logger->error("δ�ҵ���ɫ���ڵ�ͼ%s",(char *)reg->byMapName);
        //通知网关注册失败
        Zebra::logger->error("�û�(%u,%u,%s,%u)ע��ʧ��",reg->accid,reg->dwID,reg->byName,reg->dwTempID);
        Cmd::Session::t_unregUser_GateSession ret;
        ret.dwUserID=reg->dwID;
        if (scene)
          ret.dwSceneTempID=scene->tempid;
        else
          ret.dwSceneTempID=0;
        ret.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
        sendCmd(&ret,sizeof(ret));
        return true;
      }
      break;
      //请求国家在线排序
    case Cmd::Session::REQUEST_GATE_COUNTRY_ORDER:
      {
        char Buf[1024];
        bzero(Buf,sizeof(Buf));
        Cmd::Session::t_order_Country_GateSession *ret_gate = 
          (Cmd::Session::t_order_Country_GateSession*)Buf;
        constructInPlace(ret_gate);
        ret_gate->order.size = UserSession::country_map.size();
        for(std::map<DWORD,DWORD>::iterator iter = 
            UserSession::country_map.begin() ; iter != UserSession::country_map.end() ;iter ++)
        {
          DWORD temp = iter->second;
          DWORD cn = iter->first;
          for(int i=ret_gate->order.size -1 ; i>=0; i--)
          {
            if (ret_gate->order.order[i].count <= temp)
            {
              DWORD temp_1 = ret_gate->order.order[i].count;
              DWORD cn_1 = ret_gate->order.order[i].country;
              ret_gate->order.order[i].count = temp;
              ret_gate->order.order[i].country = cn; 
              temp = temp_1;
              cn = cn_1;
            }
          }
        }
        for(int i = 0 ; i < (int)ret_gate->order.size ; i ++)
        {
          Zebra::logger->debug("country :%d, online userCounter:%d",ret_gate->order.order[i].country,ret_gate->order.order[i].count);
        }
        sendCmd(ret_gate,sizeof(Cmd::Session::t_order_Country_GateSession) 
            + sizeof(ret_gate->order.order[0]) * ret_gate->order.size); 
        return true;
      }
      break;
    case Cmd::Session::PARA_GATE_CHANGE_SCENE_USER:
      {
        Cmd::Session::t_changeUser_GateSession *reg=(Cmd::Session::t_changeUser_GateSession *)cmd;
        SceneSession *scene= SceneSessionManager::getInstance()->getSceneByFile((char *)reg->byMapFileName);

        if (scene)  {
          UserSession *pUser=UserSessionManager::getInstance()->getUserByID(reg->dwID);
          if (pUser)   {
            //Zebra::logger->debug("创建用户Session成功");
	    pUser->setScene(scene);

            //场景读档案
            Cmd::Session::t_regUser_SceneSession reginscene;
            reginscene.accid=reg->accid;
            reginscene.dwID=reg->dwID;
            reginscene.dwTempID=reg->dwTempID;
            reginscene.dwMapID=scene->id;
            bcopy(reg->byName,reginscene.byName,MAX_NAMESIZE+1);
            bcopy(scene->name,reginscene.byMapName,MAX_NAMESIZE+1);
            reginscene.dwGatewayServerID=pUser->getTask()->getID();
            scene->sendCmd(&reginscene,sizeof(reginscene));

            Zebra::logger->info("�û�%s(%u)�л��������ɹ�",pUser->name,pUser->id);
#if 0
			//sky 如果玩家是有队伍的,先把队伍的MAPID容器跟新下
			if(pUser && pUser->teamid != 0)
			{
				GlobalTeamIndex::getInstance()->MemberMoveScen(pUser->teamid, scene);
				//sky 把他放到临时列表里,以便他跨场景上线的时候处理
				g_MoveSceneMemberMapLock.lock();
				MoveSceneMemberMap[pUser->id] = pUser->teamid;
				g_MoveSceneMemberMapLock.unlock();
			}
#endif
            return true;
          }else {
            Zebra::logger->error("�л����� δ�ҵ��û�%s(%u)",(char*) reg->byName,reg->dwID);
          }
        }
        else
          Zebra::logger->error("�л����� δ�ҵ���ɫ���ڵ�ͼ %s",(char *)reg->byMapFileName);
        //通知网关注册失败
        Zebra::logger->error("�л����� �û�(%u,%u,%s,%u)ע��ʧ��",reg->accid,reg->dwID,reg->byName,reg->dwTempID);
        Cmd::Session::t_unregUser_GateSession ret;
        ret.dwUserID=reg->dwID;
        if (scene)
          ret.dwSceneTempID=scene->tempid;
        else
          ret.dwSceneTempID=0;
        ret.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
        sendCmd(&ret,sizeof(ret));
        return true;
      }
      break;

    case Cmd::Session::PARA_GATE_UNREGUSER:
      {
        Cmd::Session::t_unregUser_GateSession *reg=(Cmd::Session::t_unregUser_GateSession *)cmd;
        UserSession *pUser=UserSessionManager::getInstance()->getUserByID(reg->dwUserID);
        SceneSession *scene=SceneSessionManager::getInstance()->getSceneByTempID(reg->dwSceneTempID);

        if (pUser)
        {
#if 0
          CSortM::getMe().offlineCount(pUser);
          CUnionM::getMe().userOffline(pUser); // 用于处理帮会成员下线
          CSchoolM::getMe().userOffline(pUser);
          CSeptM::getMe().userOffline(pUser);
          CQuizM::getMe().userOffline(pUser);
          CGemM::getMe().userOffline(pUser);
#endif
          UserSessionManager::getInstance()->removeUser(pUser);

          if (reg->retcode==Cmd::Session::UNREGUSER_RET_ERROR)
          {
            Zebra::logger->error("�û�%s(%u)�����ش���ע��",pUser->name,pUser->id);
            SAFE_DELETE(pUser);
            return true;
          }
          else if (reg->retcode==Cmd::Session::UNREGUSER_RET_LOGOUT)
          {
            if (scene)
            {
              Cmd::Session::t_unregUser_SceneSession send;
              send.dwUserID=reg->dwUserID;
              send.dwSceneTempID=reg->dwSceneTempID;
              send.retcode=Cmd::Session::UNREGUSER_RET_LOGOUT;
              scene->sendCmd(&send,sizeof(send));
              Zebra::logger->info("���������û�ע��%s(%u)",pUser->name,pUser->id);
              SAFE_DELETE(pUser);
              return true;
            }
            else
            {
              Cmd::Session::t_unregUser_SceneSession send;
              send.dwUserID=reg->dwUserID;
              send.dwSceneTempID=reg->dwSceneTempID;
              send.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
              SessionTaskManager::getInstance().broadcastScene(&send,sizeof(send));
              Zebra::logger->error("�û�%sע��ʱ��������,���͹㲥��Ϣע�������û�",pUser->name);
            }
          }
          SAFE_DELETE(pUser);
        }
        else
          Zebra::logger->error("ע��ʱδ�ҵ��û�%u",reg->dwUserID);

        // 更改流程后注销失败无需通知网关
        /*
           if (reg->retcode==Cmd::Session::UNREGUSER_RET_LOGOUT)
           {
           Cmd::Session::t_unregUser_GateSession ret;
           ret.dwUserID=reg->dwUserID;
           ret.dwSceneTempID=reg->dwSceneTempID;
           ret.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
           sendCmd(&ret,sizeof(ret));
           }
        // */
        return true;
      }
      break;
#if 0
    case Cmd::Session::PARA_UNION_DISBAND:
      {
        CUnionM::getMe().processGateMessage(cmd,cmdLen);
        return true;
      }
      break;
    case Cmd::Session::PARA_SEPT_DISBAND:
      {
        CSeptM::getMe().processGateMessage(cmd,cmdLen);
        return true;
      }
      break;
#endif
    case Cmd::Session::PARA_GATE_DELCHAR:
      {
        this->del_role(cmd,cmdLen);
        return true;

      }
      break;
    default:
      break;
  }

  Zebra::logger->error("SessionTask::msgParse_Gate(%u,%u,%u)",cmd->cmd,cmd->para,cmdLen);
  return false;
}

bool SessionTask::msgParse_Forward(const Cmd::t_NullCmd *pNullCmd,const DWORD cmdLen)
{
  if (pNullCmd->cmd==Cmd::Session::CMD_FORWARD && pNullCmd->para==Cmd::Session::PARA_FORWARD_USER)
  {
    Zebra::logger->error("msgParse_Forward");
    Cmd::Session::t_Session_ForwardUser *rev=(Cmd::Session::t_Session_ForwardUser *)pNullCmd;
    UserSession *pUser=UserSessionManager::getInstance()->getUserByID(rev->dwID);
    Cmd::stNullUserCmd * cmd = (Cmd::stNullUserCmd *)rev->data;
    if (pUser)
    {
      switch(cmd->byCmd)
      {
#if 0
        case Cmd::GIFT_USERCMD:
          {
            return Gift::getMe().doGiftCmd(pUser,(Cmd::stNullUserCmd*)rev->data,rev->size);
          }
          break;
        case Cmd::NPCDARE_USERCMD:
          {
            return CNpcDareM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd*)rev->data,
                rev->size);
          }
          break;
        case Cmd::QUIZ_USERCMD:
          {
            return CQuizM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd*)rev->data,
                rev->size);
          }
          break;
        case Cmd::DARE_USERCMD:
          {
            return CDareM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd *)rev->data,rev->size);
          }
          break;
        case Cmd::SCHOOL_USERCMD:
          {
            return CSchoolM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd *)rev->data,rev->size);
          }
          break;
        case Cmd::UNION_USERCMD:
          {
            return CUnionM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd *)rev->data,rev->size);
          }
          break;
#endif
        case Cmd::RELATION_USERCMD:
          {
            return pUser->relationManager.processUserMessage((Cmd::stNullUserCmd *)rev->data,rev->size);
          }
          break;
	case Cmd::HEROLIST_USERCMD:
	  {
	      if(cmd->byParam == Cmd::PARA_QUERY_SORTLIST_CMD)
	      {
		  CSortM::getMe().sendSortList(pUser);
	      }
	      return true;
	  }
#if 0
        case Cmd::SEPT_USERCMD:
          {
            return CSeptM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd *)rev->data,rev->size);
          }
          break;
        case Cmd::COUNTRY_USERCMD:
          {
            return CCountryM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd*)rev->data,rev->size);
          }
          break;
        case Cmd::ARMY_USERCMD:
          {
            return CArmyM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd*)rev->data,rev->size);
          }
          break;
        case Cmd::ALLY_USERCMD:
          {
            return CAllyM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd*)rev->data,rev->size);
          }
          break;
        case Cmd::GEM_USERCMD:
          {
            return CGemM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd*)rev->data,rev->size);
          }
          break;
        case Cmd::VOTE_USERCMD:
          {
            return CVoteM::getMe().processUserMessage(pUser,(Cmd::stNullUserCmd*)rev->data,rev->size);
          }
          break;
        case Cmd::DATA_USERCMD:
          {
            WORD degree = CSortM::getMe().getLevelDegree(pUser);
            Cmd::stLevelDegreeDataUserCmd send;
            send.degree = degree;
            pUser->sendCmdToMe(&send,sizeof(send));
            return true;
          }
          break;
#endif		
        case Cmd::CHAT_USERCMD:
          {
            switch (cmd->byParam)
            {
		case CHAT_USERCMD_PARAMETER:
		    {
			Cmd::stKokChatUserCmd *chatCmd = (Cmd::stKokChatUserCmd*)cmd;
			switch(chatCmd->dwType)
			{
			    case Cmd::CHAT_TYPE_FRIEND:
				{
				    pUser->relationManager.sendChatToMyFriend(chatCmd, rev->size);
				    return true;
				}
				break;
			    case Cmd::CHAT_TYPE_COUNTRY:
				{
				    bool ret = SessionChannel::sendCountry(pUser->country, chatCmd, rev->size);
				    return ret;
				}
				break;
			    case Cmd::CHAT_TYPE_GM:
				{
				    chatCmd->dwType = Cmd::CHAT_TYPE_SYSTEM;
				    chatCmd->dwSysInfoType = Cmd::INFO_TYPE_GM_BROADCAST;
				}
			    case Cmd::CHAT_TYPE_WORLD:
				{
				    SessionTaskManager::getInstance().sendCmdToWorld(chatCmd, rev->size);
				}
				break;
			}
		    }
		    break;
              case REQUEST_COUNTRY_HELP_USERCMD_PARA:
              case KILL_FOREIGNER_USERCMD_PARA:
              case REFRESH_BOSS_USERCMD_PARA:
              case KILL_BOSS_USERCMD_PARA:
                {
                  SessionTaskManager::getInstance().sendCmdToCountry(pUser->country,cmd,cmdLen);
                }
                break;
              case QUESTION_OBJECT_USERCMD_PARA:
                {
                  Cmd::stQuestionObject* questionCmd = (Cmd::stQuestionObject*)cmd;
#ifdef _DEBUG
                  Zebra::logger->debug("收到物品查询命令");
#endif              
                  if (questionCmd)
                  {
                    UserSession* pFromUser = UserSessionManager::getInstance()->
                      getUserSessionByName(questionCmd->name);
                    Cmd::Session::t_questionObject_SceneSession send;


                    if (pFromUser && pFromUser->scene && pUser)
                    {
                      strncpy(send.from_name,questionCmd->name,MAX_NAMESIZE);
                      strncpy(send.to_name,pUser->name,MAX_NAMESIZE);

                      send.dwObjectTempID = questionCmd->dwObjectTempID;

                      pFromUser->scene->sendCmd(&send,sizeof(Cmd::Session::t_questionObject_SceneSession));
                    }
                    else
                    {
                      if (pUser)
                      {
                        pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,
                            "对方已不在线");
                      }
                    }
                  }

                  return true;
                }
                break;
              case CREATE_CHANNEL_USERCMD_PARAMETER:
                {
                  Cmd::stCreateChannelUserCmd *create=(Cmd::stCreateChannelUserCmd *)cmd;
                  SessionChannel * sc = new SessionChannel(pUser);
                  if (!sc) return false;
                  if (!SessionChannelManager::getMe().add(sc))
                  {
                    pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,"你只能创建一个频道");
                    SAFE_DELETE(sc);
                    return true;
                  }
                  Cmd::stCreateChannelUserCmd ret;
                  ret.dwChannelID=sc->tempid;
                  ret.dwClientID=create->dwClientID;
                  strncpy(ret.name,create->name,MAX_NAMESIZE);
                  pUser->sendCmdToMe(&ret,sizeof(ret));
                  sc->add(pUser);

                  UserSession * us1 = UserSessionManager::getInstance()->getUserSessionByName(create->name);
                  sc->add(us1);
                  UserSession * us = UserSessionManager::getInstance()->getUserSessionByName(create->name2);
                  if (us)
                  {
                    Cmd::stInvite_ChannelUserCmd inv;
                    inv.dwChannelID=sc->tempid;
                    inv.dwCharType = pUser->face;
                    strncpy(inv.name,pUser->name,MAX_NAMESIZE);
                    us->sendCmdToMe(&inv,sizeof(inv));
                  }
                  return true;
                }
                break;
              case INVITE_CHANNEL_USERCMD_PARAMETER:
                {
                  Cmd::stInvite_ChannelUserCmd *invite=(Cmd::stInvite_ChannelUserCmd *)cmd;
                  //SceneUser *pUser=SceneUserManager::getMe().getUserByName(invite->name);
                  UserSession * us = UserSessionManager::getInstance()->getUserSessionByName(invite->name);
                  if (us)
                  {
                    SessionChannel *cl=SessionChannelManager::getMe().get(invite->dwChannelID);
                    if (cl)
                    {
                      if (strncmp(pUser->name,cl->name,MAX_NAMESIZE)!=0)
                      {
                        pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,"你不能邀请用户");
                        return true;
                      }
                      if (cl->has(us->tempid))
                      {
                        pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,"对方已经在频道里了");
                        return true;
                      }
                      if (cl->count()>=20)
                      {
                        pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,"频道人数已满");
                        return true;
                      }
                    }
                    else
                    {
                      pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,"聊天频道不存在");
                      return true;
                    }

                    Cmd::stInvite_ChannelUserCmd inv;
                    inv.dwChannelID=invite->dwChannelID;
                    inv.dwCharType = pUser->face;
                    strncpy(inv.name,pUser->name,MAX_NAMESIZE);
                    us->sendCmdToMe(&inv,sizeof(inv));
                  }
                  else
                    pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,"玩家 %s 不在线",invite->name);
                  return true;
                }
                break;
              case JOIN_CHANNEL_USERCMD_PARAMETER:
                {
                  Cmd::stJoin_ChannelUserCmd *join=(Cmd::stJoin_ChannelUserCmd *)cmd;
                  //UserSession *pHost=UserSessionManager::getMe().getUserSessionByName(join->host_name);
                  //if (pHost)
                  {
                    SessionChannel *cl = SessionChannelManager::getMe().get(join->dwChannelID);
                    if (cl)
                    {       
                      cl->add(pUser);
                    }
                    else    
                      pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,"不存在此聊天频道");
                  }
                }
                break;
              case LEAVE_CHANNEL_USERCMD_PARAMETER:
                {
                  Cmd::stLeave_ChannelUserCmd *leave=(Cmd::stLeave_ChannelUserCmd *)cmd;
                  //SceneUser *pHost=SceneUserManager::getMe().getUserByName(leave->host_name);
                  //if (pHost)
                  {       
                    SessionChannel *cl=SessionChannelManager::getMe().get(leave->dwChannelID);
                    if (cl)
                    {       
                      if (!cl->remove(pUser->tempid))
                      {       
                        SessionChannelManager::getMe().remove(cl->tempid);
                      }
                    }
                    else    
                      pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,"不存在此聊天频道");
                  }
                }
                break;
              default:
                break;
            }

            Cmd::stChannelChatUserCmd * chatCmd = (Cmd::stChannelChatUserCmd *)cmd;
            switch(chatCmd->dwType)
            {
#if 0
              case Cmd::CHAT_TYPE_FRIEND_AFFICHE:
              case Cmd::CHAT_TYPE_FRIEND:        /// 好友频道
                {
                  pUser->relationManager.sendChatToMyFriend(chatCmd,rev->size);
                  return true;
                }
                break;
              case Cmd::CHAT_TYPE_FRIEND_PRIVATE:      /// 好友私聊
                {
                  pUser->relationManager.sendPrivateChatToFriend(chatCmd,rev->size);
                  return true;
                }
                break;
              case Cmd::CHAT_TYPE_UNION_AFFICHE:    /// 帮会公告
              case Cmd::CHAT_TYPE_UNION:        /// 帮会频道
                {
                  CUnionM::getMe().sendUnionChatMessages(pUser,chatCmd,rev->size);
                  return true;
                }
                break;
              case Cmd::CHAT_TYPE_UNION_PRIVATE:      /// 帮会私聊
                {
                  CUnionM::getMe().sendUnionPrivateChatMessages(pUser,chatCmd,rev->size);
                  return true;
                }
                break;
#endif
        case Cmd::CHAT_TYPE_COUNTRY_MARRY:
              case Cmd::CHAT_TYPE_COUNTRY_PK:
              case Cmd::CHAT_TYPE_COUNTRY:      /// 国家频道
                {
                  UserSession *pUser = UserSessionManager::getInstance()->getUserSessionByName(chatCmd->pstrName);

          if (pUser && pUser->unionid>0 && chatCmd->dwType != Cmd::CHAT_TYPE_COUNTRY_PK && chatCmd->dwType != Cmd::CHAT_TYPE_COUNTRY_MARRY)
                  {
                    if (chatCmd->dwSysInfoType == 
                      Cmd::INFO_TYPE_KING 
                || chatCmd->dwSysInfoType == Cmd::INFO_TYPE_CASTELLAN)
                    {
                      chatCmd->dwSysInfoType = 0;
                    }
#if 0 
                    CUnion* pUnion = CUnionM::getMe().getUnionByID(pUser->unionid);

                    if (pUnion && pUnion->master && pUnion->master->id == pUser->id)
                    {//是帮主
                      if (CCityM::getMe().findByUnionID(pUser->unionid) != NULL)
                      {//城主
                        CCity* pCity = CCityM::getMe().findByUnionID(pUser->unionid);
                        SceneSession * pScene = SceneSessionManager::getInstance()->getSceneByID((pCity->dwCountry<<16)+pCity->dwCityID);
                        if (pScene){
                      //    sprintf(chatCmd->pstrName,"%s 城主",pScene->name);
                          chatCmd->dwSysInfoType = 
                            Cmd::INFO_TYPE_CASTELLAN;

                        }
                      }

                      if (CCityM::getMe().
                          find(pUser->country,KING_CITY_ID,pUser->unionid) !=NULL)
                      {//是国王
                      //  strncpy(chatCmd->pstrName,"国王",MAX_NAMESIZE);
                        chatCmd->dwSysInfoType = Cmd::INFO_TYPE_KING;
                        CCountry* pEmperor = CCountryM::getMe().find(NEUTRAL_COUNTRY_ID);
                        if (pEmperor && pEmperor->dwKingUnionID == pUser->unionid)
                        {
                          chatCmd->dwSysInfoType = Cmd::INFO_TYPE_EMPEROR;
                        }

                      }
                    }
#endif
                  }

                  if (pUser)
                  {
                    if (chatCmd->dwSysInfoType == Cmd::INFO_TYPE_EXP &&
                    chatCmd->dwType == Cmd::CHAT_TYPE_COUNTRY)
                    {
                      Zebra::logger->error("怀疑玩家%s使用外挂利用自定义消息刷屏",pUser->name);
                    }
                    SessionChannel::sendCountry(pUser->country,chatCmd,rev->size);
                    BYTE buf[zSocket::MAX_DATASIZE];
                    Cmd::GmTool::t_Chat_GmTool *cmd=(Cmd::GmTool::t_Chat_GmTool *)buf;
                    bzero(buf,sizeof(buf));
                    constructInPlace(cmd);

                    strncpy(cmd->userName,pUser->name,MAX_NAMESIZE);
                    cmd->countryID = pUser->country;
                    cmd->sceneID = pUser->scene->id;
                    cmd->dwType = chatCmd->dwType;
                    strncpy(cmd->content,chatCmd->pstrChat,255);
                    cmd->size = chatCmd->size;
                    if (cmd->size)
                      bcopy(chatCmd->tobject_array,cmd->tobject_array,cmd->size*sizeof(Cmd::stTradeObject));
                    SessionService::getInstance().sendCmdToSuperServer(cmd,sizeof(Cmd::GmTool::t_Chat_GmTool)+cmd->size*sizeof(Cmd::stTradeObject));
                  }
                  return true;
                }
                break;
#if 0
              case Cmd::CHAT_TYPE_OVERMAN_AFFICHE:  /// 师门公告
              case Cmd::CHAT_TYPE_OVERMAN:      /// 师门频道
                {
                  CSchoolM::getMe().sendSchoolChatMessages(pUser,chatCmd,rev->size);
                  return true;
                }
                break;
              case Cmd::CHAT_TYPE_OVERMAN_PRIVATE:      /// 师门私聊
                {
                  CSchoolM::getMe().sendSchoolPrivateChatMessages(pUser,chatCmd,rev->size);
                  return true;
                }
                break;
              case Cmd::CHAT_TYPE_FAMILY_AFFICHE:    /// 家族公告
              case Cmd::CHAT_TYPE_FAMILY:        /// 家族频道
                {
                  CSeptM::getMe().sendSeptChatMessages(pUser,chatCmd,rev->size);
                  return true;
                }
                break;
              case Cmd::CHAT_TYPE_FAMILY_PRIVATE:      /// 家族私聊
                {
                  CSeptM::getMe().sendSeptPrivateChatMessages(pUser,chatCmd,rev->size);
                  return true;
                }
                break;
#endif
              case Cmd::CHAT_TYPE_GM:
                chatCmd->dwType = Cmd::CHAT_TYPE_SYSTEM;
                chatCmd->dwSysInfoType = Cmd::INFO_TYPE_SCROLL;
              case Cmd::CHAT_TYPE_WORLD:
                {
                  SessionTaskManager::getInstance().sendCmdToWorld(chatCmd,rev->size);
                  BYTE buf[zSocket::MAX_DATASIZE];
                  Cmd::GmTool::t_Chat_GmTool *cmd=(Cmd::GmTool::t_Chat_GmTool *)buf;
                  bzero(buf,sizeof(buf));
                  constructInPlace(cmd);

                  strncpy(cmd->userName,pUser->name,MAX_NAMESIZE);
                  cmd->countryID = pUser->country;
                  cmd->sceneID = pUser->scene->id;
                  cmd->dwType = chatCmd->dwType;
                  strncpy(cmd->content,chatCmd->pstrChat,255);


                  cmd->size = chatCmd->size;
                  if (cmd->size)
                    bcopy(chatCmd->tobject_array,cmd->tobject_array,cmd->size*sizeof(Cmd::stTradeObject));
                  SessionService::getInstance().sendCmdToSuperServer(cmd,sizeof(Cmd::GmTool::t_Chat_GmTool)+cmd->size*sizeof(Cmd::stTradeObject));
                  //}
                  return true;
                }
                break;

              case Cmd::CHAT_TYPE_PERSON:
                {
                  SessionChannel *cl=SessionChannelManager::getMe().get(chatCmd->dwChannelID);
                  if (cl)                                  
                  {
                    strncpy(chatCmd->pstrName,pUser->name,MAX_NAMESIZE);
                    cl->sendToOthers(pUser,chatCmd,rev->size);
                  }               
                  else                    
                    pUser->sendSysChat(Cmd::INFO_TYPE_FAIL,"不存在此聊天频道");
                  return true;
                }
                break;
              case Cmd::CHAT_TYPE_BLESS_MSG:
                {
                  UserSession *otherUser = UserSessionManager::getInstance()->getUserSessionByName(chatCmd->pstrName);
                  if (otherUser)
                  {
                    if (strncmp(pUser->name,chatCmd->pstrName,MAX_NAMESIZE)!=0)
                    {
                      if (SessionTimeTick::currentTime<pUser->nextBlessTime)
                      {
                        pUser->sendSysChat(Cmd::INFO_TYPE_MSG,"对不起两分钟内你只能发送一次祝福现在距离下次可发送时间还有%u秒",(pUser->nextBlessTime.msecs()-SessionTimeTick::currentTime.msecs())/1000);
                        return true;
                      }
                      else
                      {
                        pUser->nextBlessTime.now();
                        pUser->nextBlessTime.addDelay(120000);
                        strncpy(chatCmd->pstrName,pUser->name,MAX_NAMESIZE);
                        otherUser->sendCmdToMe(chatCmd,cmdLen);
                        pUser->sendSysChat(Cmd::INFO_TYPE_MSG,"祝福已送出");
                        return true;
                      }
                    }
                    else
                    {
                      pUser->sendSysChat(Cmd::INFO_TYPE_MSG,"这是你的名字");
                    }
                  }
                  else
                  {
                    pUser->sendSysChat(Cmd::INFO_TYPE_MSG,"输入的名称不正确或指定的玩家不在线");
                  }
                  return true;
                }
                break;
            }
            return true;
          }
          break;
      }
    }
    else
    {
      switch(cmd->byCmd)
      {
        case Cmd::CHAT_USERCMD:
          {
            switch (cmd->byParam)
            {
              case REFRESH_BOSS_USERCMD_PARA:
                {
                  Cmd::stRefreshBossUserCmd * msg = (Cmd::stRefreshBossUserCmd *)cmd;
                  SessionTaskManager::getInstance().sendCmdToCountry(msg->country,cmd,cmdLen);
                }
                break;
            }
          }
          break;
        default:
          Zebra::logger->error("处理用户指(%d,%d)令时,未找到角色为%u的用户",pNullCmd->cmd,pNullCmd->para,rev->dwID);
          break;
      }
      return true;
    }
  }
  Zebra::logger->error("SessionTask::msgParse_Forward(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,cmdLen);
  return false;
}

//sky 竞技战场副本类Session消息处理函数
bool SessionTask::msgParse_Arena(const Cmd::t_NullCmd *cmd,const DWORD cmdLen)
{
#if 0
	switch(cmd->para)
	{
	case Cmd::Session::PARA_USE_SPORTS_ADDMETOQUEUING:	//sky 通知sess把排队的用户放到管理器中处理
		{
			Cmd::Session::t_Sports_AddMeToQueuing * pCmd = (Cmd::Session::t_Sports_AddMeToQueuing*)cmd;
			CArenaManager::getInstance().Arena_AddUser(pCmd);
		}
		break;
	case Cmd::Session::PARA_SCENE_MEISBATTLEFIELD:
		{
			CArenaManager::getInstance().InsertBattleTask(this);
		}
		break;
	case Cmd::Session::PARA_USE_SPORTS_RETURNMAPID:		//sky 场景找到可用的战场地图后通知session地图ID
		{
			Cmd::Session::t_Sports_ReturnMapID *reg=(Cmd::Session::t_Sports_ReturnMapID *)cmd;

			if(reg->dwID != 0)		//sky 生成动态地图成功
			{
				SceneSession *scene=new SceneSession(this);
				Cmd::Session::t_regScene_ret_SceneSession ret;
				ret.dwTempID=reg->dwTempID;

				//sky 先注册地图
				if (scene->reg((Cmd::Session::t_regScene_SceneSession *)reg) 
					&& SceneSessionManager::getInstance()->addScene(scene))
				{
					ret.byValue=Cmd::Session::REGSCENE_RET_REGOK;
					CCountryM::getMe().refreshTax();
					CCountryM::getMe().refreshTech(this,reg->dwCountryID);
					if (KING_CITY_ID==(reg->dwID&0x0000ffff))
						CCountryM::getMe().refreshGeneral(reg->dwCountryID);
					CAllyM::getMe().refreshAlly(this);

					Zebra::logger->info("注册地图%u(%s %s) 成功",reg->dwID,reg->byName,reg->fileName);
					CCityM::getMe().refreshUnion(reg->dwCountryID,reg->dwID & 0x0FFF);

					SceneSession * sc = SceneSessionManager::getInstance()->getSceneByID(reg->dwID);
					if(!sc)
					{
						fprintf(stderr,"bad\n");
					}

					//sky 将战场地图的ID放到发出请求的队列管理器里去
					CArenaManager::getInstance().AddMapToQueuing(reg);
					//sky 一次申请成功后把锁恢复为真
					CArenaManager::getInstance().NewMap_Lock(reg->AddMeType, true);
					return true;
				}
				else
				{
					ret.byValue=Cmd::Session::REGSCENE_RET_REGERR;
					Zebra::logger->error("注册地图%u(%s %s)失败",reg->dwID,reg->byName,reg->fileName);
				}
				sendCmd(&ret,sizeof(ret));
				return true;
			}
			else
			{
				//sky 把特定队列管理器的申请场景偏移向后移动
				CArenaManager::getInstance().MoveSceneM(1);
			}
			
		}
		break;	
	default:
		break;
	}
#endif
	return true;
}

bool SessionTask::msgParse(const Cmd::t_NullCmd *cmd,const DWORD cmdLen)
{
  return MessageQueue::msgParse(cmd,cmdLen);
}

bool SessionTask::cmdMsgParse(const Cmd::t_NullCmd *cmd,const DWORD cmdLen)
{
#if 0
	NFilterModuleArray::const_iterator pIterator;

	//command
	for(pIterator=g_nFMA.begin(); pIterator != g_nFMA.end();pIterator++)
	{
		if (pIterator->filter_command((PBYTE)cmd,cmdLen)) return true;
	}
#endif
	if (Cmd::Session::CMD_FORWARD == cmd->cmd && Cmd::Session::PARA_FORWARD_USER == cmd->para)
	{
		Zebra::logger->error("cmdMsgParse");
		Cmd::Session::t_Session_ForwardUser *pSFU=(Cmd::Session::t_Session_ForwardUser *)cmd;
		//UserSession *pUser=UserSessionManager::getInstance()->getUserByID(pSFU->dwID);
		Cmd::stNullUserCmd * pNUC = (Cmd::stNullUserCmd *)pSFU->data;
		Cmd::stChannelChatUserCmd *pCCUC;

		Zebra::logger->error("SessionTask::cmdMsgParse(%u,%u,%u)",pNUC->byCmd,pNUC->byParam,cmdLen);

		if (Cmd::CHAT_USERCMD == pNUC->byCmd && ALL_CHAT_USERCMD_PARAMETER == pNUC->byParam)
		{
			pCCUC = (Cmd::stChannelChatUserCmd*)pNUC;
			Zebra::logger->error("SessionTask::cmdMsgParse dwType=%d,dwSysInfoType=%d,dwChannelID=%d,size=%d,pstrChat=%s",pCCUC->dwType,pCCUC->dwSysInfoType,pCCUC->dwChannelID,pCCUC->size,pCCUC->pstrChat);
		}


		using namespace Cmd;
#if 0
		if(pNUC->byCmd == TURN_USERCMD && pNUC->byParam == PARA_CHECKRELATION_EMPTY)

		{



			Zebra::logger->error("收到用户关系检查消息");
			bool isEmpty = true;
			for(int i = 0; i < 6; ++i)
			{
				if( i == Cmd::RELATION_TYPE_FRIEND || i == Cmd::RELATION_TYPE_OVER )
					continue;
				CRelation* relation = pUser->relationManager.getRelationByType(i);
				if(relation != NULL)
					isEmpty = false;

			}


			Zebra::logger->error("isEmpty = %d",isEmpty);

			Cmd::t_CheckRelationEmptyResult sendM;
			sendM.isEmpty = isEmpty;
			sendM.dwUserID = pSFU->dwID;

			pUser->scene->sendCmd(&sendM,sizeof(sendM));

			//forwardScene(&sendM,sizeof(sendM));

			return true;
		}
#endif

	}

	switch(cmd->cmd)
	{
	case Cmd::CMD_NULL:
		break;
	case Cmd::Session::CMD_PKGAME:
		{
		    switch(cmd->para)
		    {
			case Cmd::Session::REQ_FIGHT_MATCH_PARA:
			    {//�����սƥ��
				Cmd::Session::t_ReqFightMatch_SceneSession *rev= (Cmd::Session::t_ReqFightMatch_SceneSession *)cmd;
				HeroCardManager::getMe().processMessage(rev);
			    }
			    break;
			case Cmd::Session::NOTIFY_SCENESERVER_GAME_COUNT_PARA:
			    {
				Cmd::Session::t_NotifySceneServerGameCount_SceneSession *rev=(Cmd::Session::t_NotifySceneServerGameCount_SceneSession*)cmd;
				HeroCardManager::getMe().updateSessionGameCount(rev->countryID, rev->gameCount);
			    }
			    break;
			case Cmd::Session::PUT_ONE_GAMEID_BACK_PARA:
			    {
				Cmd::Session::t_PutOneGameIDBack_SceneSession *rev = (Cmd::Session::t_PutOneGameIDBack_SceneSession *)cmd;
				HeroCardManager::getMe().putGameIDBack(rev->type, rev->gameID);
			    }
			    break;
			default:
			    break;
		    }
		    return true;
		}
		break;
	case Cmd::Session::CMD_OTHER:
		{
		    if(Cmd::Session::RELATION_ADD_FRIEND == cmd->para)
		    {
			Cmd::Session::t_RelationAddFriend *rec = (Cmd::Session::t_RelationAddFriend *)cmd;
			UserSession* pUser=UserSessionManager::getInstance()->getUserByID(rec->userID);
			if(pUser)
			{
			    Cmd::stRelationStatusCmd send;
			    strncpy(send.name, rec->name, MAX_NAMESIZE);
			    send.byCmd = Cmd::RELATION_USERCMD;
			    send.type = rec->type;
			    send.byParam = Cmd::RELATION_STATUS_PARA;
			    send.byState = Cmd::RELATION_QUESTION;
			    
			    pUser->relationManager.processUserMessage(&send, sizeof(send));
			}
			return true;
		    }
		}
		break;
	case Cmd::Session::CMD_SCENE:
		return msgParse_Scene(cmd,cmdLen);
	case Cmd::Session::CMD_GATE:
		return msgParse_Gate(cmd,cmdLen);
	case Cmd::Session::CMD_FORWARD:
		return msgParse_Forward(cmd,cmdLen);
	case Cmd::Session::CMD_SCENE_SHUTDOWN:
		{
			switch(cmd->para)
			{
			case Cmd::Session::PARA_SHUTDOWN:
				{
					Cmd::Session::t_shutdown_SceneSession *sss = (Cmd::Session::t_shutdown_SceneSession*)cmd; 
					struct tm tm_1;
					time_t timval=time(NULL);
					//tm_1=*localtime(&timval);
					zRTime::getLocalTime(tm_1,timval);
					Zebra::logger->info("系统当前时间%d年%d月%d日%d时%d分%d秒",tm_1.tm_year+1900,tm_1.tm_mon+1,tm_1.tm_mday,tm_1.tm_hour,tm_1.tm_min,tm_1.tm_sec);
					if (sss->time)
					{
						SessionChannel::sendAllInfo(Cmd::INFO_TYPE_SCROLL,"系统当前时间%d年%d月%d日%d时%d分%d秒",tm_1.tm_year+1900,tm_1.tm_mon+1,tm_1.tm_mday,tm_1.tm_hour,tm_1.tm_min,tm_1.tm_sec);
						//tm_1=*localtime(&sss->time);
						zRTime::getLocalTime(tm_1,sss->time);
						SessionChannel::sendAllInfo(Cmd::INFO_TYPE_SCROLL,"系统将于%d年%d月%d日%d时%d分%d秒停机维护",tm_1.tm_year+1900,tm_1.tm_mon+1,tm_1.tm_mday,tm_1.tm_hour,tm_1.tm_min,tm_1.tm_sec);
						Zebra::logger->debug("系统将于%d年%d月%d日%d时%d分%d秒停机维护",tm_1.tm_year+1900,tm_1.tm_mon+1,tm_1.tm_mday,tm_1.tm_hour,tm_1.tm_min,tm_1.tm_sec);
						if (strlen(sss->info)>0)
						{
							SessionChannel::sendAllInfo(Cmd::INFO_TYPE_SCROLL,"%s",sss->info);
						}
						SessionService::getInstance().shutdown_time=*sss;
					}
					else
					{
						Zebra::logger->debug("取消停机维护");
						SessionChannel::sendAllInfo(Cmd::INFO_TYPE_SCROLL,"取消停机维护");
						SessionService::getInstance().shutdown_time=*sss;
						Cmd::Session::t_SetService_SceneSession send;
						send.flag |= Cmd::Session::SERVICE_MAIL;
						send.flag |= Cmd::Session::SERVICE_AUCTION;
						SessionTaskManager::getInstance().broadcastScene(&send,sizeof(send));

						SessionChannel::sendAllInfo(Cmd::INFO_TYPE_SYS,"邮件系统和拍卖系统已经启动，可以正常使用了");
						Zebra::logger->info("取消停机，开启邮件和拍卖服务");
					}
					return true;
				}
				break;
			}
		}
		break;
#if 0
	case Cmd::Session::CMD_SCENE_SEPT:
		CSeptM::getMe().processSceneSeptMessage(cmd,cmdLen);
		return true;
	case Cmd::Session::CMD_SCENE_UNION:
		CUnionM::getMe().processSceneUnionMessage(cmd,cmdLen);
		return true;
	case Cmd::Session::CMD_SCENE_COUNTRY:
		CCountryM::getMe().processSceneMessage(cmd,cmdLen);
		return true;
	case Cmd::Session::CMD_SCENE_DARE:
		CDareM::getMe().processSceneMessage(cmd,cmdLen);
		return true;
	case Cmd::Session::CMD_SCENE_ARMY:
		CArmyM::getMe().processSceneMessage(cmd,cmdLen);
		return true;
	case Cmd::Session::CMD_SCENE_GEM:
		CGemM::getMe().processSceneMessage(cmd,cmdLen);
		return true;
	case Cmd::Session::CMD_SCENE_TMP:
		{
			switch(cmd->para)
			{
			case Cmd::Session::CLEARRELATION_PARA:
				{
					Cmd::Session::t_ClearRelation_SceneSession* rev=
						(Cmd::Session::t_ClearRelation_SceneSession*)cmd;

					//CSeptM::getMe().delSeptAllMember();
					CUnionM::getMe().delAllUnion(rev->dwUserID);
					return true;
				}
				break;
			case Cmd::Session::RETURN_CREATE_UNION_ITEM_PARA:
				{//给指定ID的用户添加两封邮件
					Cmd::Session::t_ReturnCreateUnionItem_SceneSession* rev=
						(Cmd::Session::t_ReturnCreateUnionItem_SceneSession*)cmd;

					Cmd::Session::t_sendMail_SceneSession sm;

					sm.mail.state = Cmd::Session::MAIL_STATE_NEW;
					strncpy(sm.mail.fromName,"系统",MAX_NAMESIZE);
					sm.mail.toID = rev->dwUserID;
					strncpy(sm.mail.title,"天羽令",MAX_NAMESIZE);
					sm.mail.type = Cmd::Session::MAIL_TYPE_MAIL;
					sm.mail.createTime = rev->item.createtime;
					sm.mail.delTime = sm.mail.createTime + 60*60*24*7;
					sm.mail.accessory = 1;
					sm.mail.itemGot = 0;
					_snprintf(sm.mail.text,255-1,"%s","建立帮会所用道具");
					sm.mail.sendMoney = 0;
					sm.mail.recvMoney = 0;
					bcopy(&rev->item,&sm.item,sizeof(sm.item));

					MailService::getMe().sendMail(sm);
					MailService::getMe().sendMoneyMail("系统",0,"",rev->dwUserID,
						UnionDef::CREATE_UNION_NEED_PRICE_GOLD,
						"建立帮会所需银两",(DWORD)-1,
						Cmd::Session::MAIL_TYPE_MAIL);

					return true;
				}
				break;
			}
		}
		break;
	case Cmd::Session::CMD_SCENE_SPORTS:
		{
			return msgParse_Arena(cmd,cmdLen);
		}
		break;
#endif
  }
  return false;
}

SessionTask::~SessionTask()
{
}

