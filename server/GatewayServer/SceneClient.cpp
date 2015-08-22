#include <time.h>
#include <unistd.h>
#include <iostream>

#include "zTCPClient.h"
#include "Zebra.h"
#include "SceneClient.h"
#include "BillClient.h"
#include "SessionCommand.h"
#include "SceneCommand.h"
#include "RecordCommand.h"
#include "SessionClient.h"
#include "Command.h"
#include "RecordClient.h"
//#include "SafetyUserCommand.h"
/*器连接客户端
 *
 */

#include "GateUserManager.h"
#include "GatewayServer.h"
#include "GatewayTask.h"



/**
 * \brief 创建到场景服务器的连接
 *
 * \return 连接是否成功
 */
bool SceneClient::connectToSceneServer()
{
  if (!zTCPClientTask::connect())
  {
    Zebra::logger->error("连接场景服务器失败");
    return false;
  }

  Cmd::Super::t_restart_ServerEntry_NotifyOther notify; 
  notify.srcID=GatewayService::getInstance().getServerID();
  notify.dstID=this->getServerID();
  GatewayService::getInstance().sendCmdToSuperServer(&notify,sizeof(notify));
  using namespace Cmd::Scene;
  t_LoginScene tCmd;
  tCmd.wdServerID = GatewayService::getInstance().getServerID();
  tCmd.wdServerType = GatewayService::getInstance().getServerType();
  Zebra::logger->debug("tCmd.wdServerID:%u tCmd.wdServerType:%u", tCmd.wdServerID, tCmd.wdServerType);

  return sendCmd(&tCmd,sizeof(tCmd));
}

int SceneClient::checkRebound()
{
  return 1;
}
void SceneClient::addToContainer()
{
  SceneClientManager::getInstance().add(this);
}

void SceneClient::removeFromContainer()
{
  SceneClientManager::getInstance().remove(this);
  GateUserManager::getInstance()->removeUserBySceneClient(this);
}
bool SceneClient::connect()
{
  return connectToSceneServer();
}


/**
 * \brief 解析来自场景服务器的指令
 *
 * \param pNullCmd 待处理的指令
 * \param nCmdLen 待处理的指令长度
 * \return 解析是否成功
 */
bool SceneClient::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#ifdef _MSGPARSE_
  Zebra::logger->debug("?? SceneClient::msgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
#endif

 // printf("收到消息(%d,%d)\n", pNullCmd->cmd, pNullCmd->para);
  using namespace Cmd::Scene;
  switch(pNullCmd->cmd)
  {
    case CMD_LOGIN:
      {
        switch(pNullCmd->para)
        {
          case PARA_CHANGE_VERIFY_VERSION:
            {
              t_ChangeVerifyVersion *rev=(t_ChangeVerifyVersion *)pNullCmd;
              GateUser *pUser=(GateUser *)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
              if (pUser)
              {
                switch (rev->oper)
                {
                  case Cmd::Scene::CHANGECODE:
                    {
                      GatewayService::getInstance().verify_client_version = rev->versionCode;
                      GatewayService::getInstance().notifyLoginServer();
                      return true;
                    }
                    break;
                  case Cmd::Scene::SHOWCODE:
                    {
                      Cmd::stChannelChatUserCmd send;
                      send.dwType=Cmd::CHAT_TYPE_SYSTEM;
                      send.dwSysInfoType = Cmd::INFO_TYPE_GAME;
                      bzero(send.pstrName,sizeof(send.pstrName));
                      bzero(send.pstrChat,sizeof(send.pstrChat));

                      if (pUser)
                      {
                        sprintf((char*)send.pstrChat,"服务器当前版本校验码:%u",
                          GatewayService::getInstance().verify_client_version);
                        pUser->sendCmd(&send,sizeof(send));
                      }
                      return true;
                    }
                    break;
                  default:
                    break;
                }
              }
            }
            break;
          case PARA_CHANGE_COUNTRY_STATE:
            {
              t_ChangeCountryStatus *rev=(t_ChangeCountryStatus *)pNullCmd;
              GateUser *pUser=(GateUser *)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
              if (pUser)  GatewayService::getInstance().country_info.processChange(pUser,rev);
              return true;
            }
            break;
          case PARA_LOGIN_REFRESH:
            {
              t_Refresh_LoginScene *rev=(t_Refresh_LoginScene *)pNullCmd;
              GateUser *pUser=(GateUser *)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
	      if (pUser)
	      {
		  if(!pUser->tempid)
		  {
		      Zebra::logger->fatal("[异常]角色 tempid怎么是0");
		      return true;
		  }
		  if(!pUser->gatewaytask)
		  {
		      Zebra::logger->fatal("[异常]角色 登陆流程异常pUser->gatewaytask为NULL");
		      return true;
		  }
		  if(pUser->gatewaytask->isTerminateWait() || pUser->gatewaytask->isTerminate())
		  {
		      Zebra::logger->fatal("[异常]角色 登陆流程异常");
		      return true;
		  }
		  //pUser->scene=this;
		  //pUser->sceneTempID=rev->dwSceneTempID;
		  pUser->playState(this,rev->dwSceneTempID );

		  mlock.lock();
		  sindex[pUser->id] = pUser;
		  mlock.unlock();
		  Zebra::logger->info("用户%s(%d,%d)登录成功",pUser->name,pUser->id,pUser->tempid);
              }
              else
              {
                Zebra::logger->fatal("用户id=%d已经不在了",rev->dwUserID);
                //清理Session中数据
                Cmd::Session::t_unregUser_GateSession send;
                send.dwUserID=rev->dwUserID;
                send.dwSceneTempID=rev->dwSceneTempID;
                send.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
                sessionClient->sendCmd(&send,sizeof(send));
                //清理Scene中数据
                Cmd::Scene::t_Unreg_LoginScene scnd;
                scnd.dwUserID=rev->dwUserID;
                scnd.dwSceneTempID=rev->dwSceneTempID;
                scnd.retcode=Cmd::Scene::UNREGUSER_RET_ERROR;
                sendCmd(&scnd,sizeof(scnd));
              }
              return true;
            }
            break;
          case PARA_LOGIN_UNREG:
            {
              t_Unreg_LoginScene *rev=(t_Unreg_LoginScene *)pNullCmd;
              GateUser *pUser=(GateUser *)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
              if (pUser)
              {
                if (rev->retcode==Cmd::Scene::UNREGUSER_RET_LOGOUT)
                {
                  if (pUser->backSelect)
                  {
                    //pUser->final();
                    Zebra::logger->info("用户(%u,%u,%s)退回人物选择界面",pUser->accid,pUser->id,pUser->name);
                    //pUser->initState();
                    //pUser->beginSelect();
                    //pUser->backSelect=false;
                  }
                  else
                  {
                    //pUser->final();
                    Zebra::logger->info("场景请求用户(%u,%u,%s)注销",pUser->accid,pUser->id,pUser->name);
                    pUser->TerminateWait();
                    //SAFE_DELETE(User);
                  }
                }
                else if (rev->retcode==Cmd::Scene::UNREGUSER_RET_UNLOAD_SCENE)
                {
                  Zebra::logger->info("用户因卸载地图(%u,%u,%s)退回人物选择界面",pUser->accid,pUser->id,pUser->name);
                  if (pUser && pUser->isPlayState())
                  {
                    pUser->backSelect=true;
                    pUser->unreg();
                  }
                }
                else if (rev->retcode==Cmd::Scene::UNREGUSER_RET_ERROR)
                {
                  Zebra::logger->error("场景请求用户(%u,%u,%s)失败注销",pUser->accid,pUser->id,pUser->name);
                  //pUser->final();
                  pUser->TerminateWait();
                  //SAFE_DELETE(pUser);
                }
                else if (rev->retcode==Cmd::Scene::UNREGUSER_RET_CHANGE_SCENE)
                {
                  Zebra::logger->error("用户(%u,%u,%s)切换场景注销",pUser->accid,pUser->id,pUser->name);

                  Cmd::Session::t_changeUser_GateSession send;
                  send.accid = pUser->accid;
                  send.dwID = pUser->id;
                  send.dwTempID = pUser->tempid;
                  strncpy((char *)send.byName,pUser->name,MAX_NAMESIZE);
                  strncpy((char *)send.byMapFileName,(char *)rev->map,MAX_NAMESIZE);
                  sessionClient->sendCmd(&send,sizeof(send));
                  pUser->waitUnregState();

                  //pUser->final();
                  //SAFE_DELETE(pUser);
                }
                else if (rev->retcode==Cmd::Scene::UNREGUSER_RET_KICKOUT)
                {
                }

              }
              return true;
            }
            break;
          case PARA_LOGIN_UNREG_OK:
            {
              t_Unreg_LoginScene_Ok *rev=(t_Unreg_LoginScene_Ok *)pNullCmd;
              GateUser *pUser=(GateUser *)GateUserManager::getInstance()->getUserByAccID(rev->accid);
              if (pUser)
              {
                switch(rev->type)
                {
                  case Cmd::Record::LOGOUT_WRITEBACK:
                    {
                      pUser->final();
                      pUser->beginSelect();
                      pUser->backSelect=false;
                    }
                    break;
                  case Cmd::Record::CHANGE_SCENE_WRITEBACK:
                    {
                      pUser->playState();
                    }
                    break;
                }
              }
              return true;
            }
            break;
          default:
            break;
        }
      }
      break;
    case CMD_SCENE_GATE_BILL:
      {
        switch(pNullCmd->para)
        {
          case PARA_REQUEST_BILL:
            {
              Cmd::Scene::t_Request_Bill *rev= (Cmd::Scene::t_Request_Bill*)pNullCmd; 
              GateUser* pUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
              if (pUser)
              {
                Cmd::Bill::t_Request_Card_Gold_Gateway send;
                if (pUser->getAccount())
                  strncpy(send.account,pUser->getAccount(),MAX_ACCNAMESIZE);
                send.accid = pUser->accid;
                send.charid = pUser->id;
                accountClient->sendCmd(&send,sizeof(send));  
                //Zebra::logger->debug("请求查询剩余月卡和金币(accid=%d)",pUser->accid);
              }
            }
            break;
          case PARA_REQUEST_POINT:
            {
              Cmd::Scene::t_Request_Point *rev= (Cmd::Scene::t_Request_Point*)pNullCmd; 
              GateUser* pUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
              if (pUser)
              {
                Cmd::Bill::t_Request_Point_Gateway send;
                if (pUser->getAccount())
                  strncpy(send.account,pUser->getAccount(),MAX_ACCNAMESIZE);
                send.accid = pUser->accid;
                send.charid = pUser->id;
                accountClient->sendCmd(&send,sizeof(send));  
                //Zebra::logger->debug("请求查询剩余点数(accid=%d)",pUser->accid);
              }
            }
            break;
            /*
               case PARA_REQUEST_CARD_GOLD:
               {
               Cmd::Scene::t_Request_Card_Gold *rev= (Cmd::Scene::t_Request_Card_Gold*)pNullCmd; 
               GateUser* pUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
               if (pUser)
               {
               Cmd::Bill::t_Request_Point_Gateway send;
               if (pUser->getAccount())
               strncpy(send.account,pUser->getAccount(),MAX_ACCNAMESIZE);
               send.accid = pUser->accid;
               send.charid = pUser->id;
               accountClient->sendCmd(&send,sizeof(send));  
               Zebra::logger->debug("请求查询剩余点数(accid=%d)",pUser->accid);
               }
               }
               break;
            // */
          case PARA_REQUEST_REDEEM_GOLD:
            {
              Cmd::Scene::t_Request_RedeemGold *rev= (Cmd::Scene::t_Request_RedeemGold*)pNullCmd; 
              GateUser* pUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
              if (pUser)
              {
                Cmd::Bill::t_Request_Redeem_Gold_Gateway send;
                if (pUser->getAccount())
                  strncpy(send.account,pUser->getAccount(),MAX_ACCNAMESIZE);
                send.accid = pUser->accid;
                send.charid = pUser->id;
                send.point = rev->dwNum;
                accountClient->sendCmd(&send,sizeof(send));  
                //Zebra::logger->debug("请求兑换金币(accid=%d)",pUser->accid);
              }
            }
            break;
          case PARA_REQUEST_REDEEM_CARD:
            {
              Cmd::Scene::t_Request_RedeemCard *rev= (Cmd::Scene::t_Request_RedeemCard*)pNullCmd; 
              GateUser* pUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
              if (pUser)
              {
                Cmd::Bill::t_Request_Redeem_MonthCard_Gateway send;
                if (pUser->getAccount())
                  strncpy(send.account,pUser->getAccount(),MAX_ACCNAMESIZE);
                send.accid = pUser->accid;
                send.charid = pUser->id;
                accountClient->sendCmd(&send,sizeof(send));  
                //Zebra::logger->debug("请求兑换月卡(accid=%d)",pUser->accid);
              }
            }
            break;
          default:
            break;
        }
        return true;
      }
      break;
    case CMD_SCENE:
      {
        switch(pNullCmd->para)
        {
	    case PARA_GM_COMMAND_SCENEGATE:
		{
		    t_gmCommand_SceneGate* gm = (t_gmCommand_SceneGate*)pNullCmd;
		    doGmCmd(gm);
		    return true;
		}
		break;
          case PARA_QUERY_ACCOUNT:
            {
              t_Query_AccountScene* rev = (t_Query_AccountScene*)pNullCmd;
              GateUser* pUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
              GateUser* pDestUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->dwDestID);
              Cmd::stChannelChatUserCmd send;
              send.dwType=Cmd::CHAT_TYPE_SYSTEM;
              send.dwSysInfoType = Cmd::INFO_TYPE_FAIL;
              bzero(send.pstrName,sizeof(send.pstrName));
              bzero(send.pstrChat,sizeof(send.pstrChat));

              if (pUser)
              {
                if (pDestUser)
                {
                  snprintf((char*)send.pstrChat,MAX_CHATINFO,"%s(%s)",pDestUser->name,pDestUser->getAccount());
                }

                pUser->sendCmd(&send,sizeof(send));
              }

              return true;
            }
            break;
          case PARA_COUNTRY_AND_SCENE:
            {
              Cmd::Scene::t_countryAndScene_GateScene* rev = (Cmd::Scene::t_countryAndScene_GateScene*)pNullCmd;
              GateUser* pUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->userID);
              if (pUser)
              {
                GateUserManager::getInstance()->removeCountryUser(pUser);
                pUser->countryID = rev->countryID;
                GateUserManager::getInstance()->addCountryUser(pUser);
                pUser->sceneID = rev->sceneID;
                //Zebra::logger->debug("%s 场景改变 %u",pUser->name,pUser->sceneID);
                return true;
              }
              else
              {
                Zebra::logger->debug("收到改变场景的消息但是玩家不存在 %u",rev->userID);
                return true;
              }
            }
            break;
          case PARA_FRESH_MAPINDEX:
            {
              t_fresh_MapIndex *rev=(t_fresh_MapIndex*)pNullCmd;
              for(int i = 0 ; i < (int)rev->dwSize ; i ++)
              {
                ScreenIndex *index = new ScreenIndex(rev->mps[i].mapx,rev->mps[i].mapy);
                if (index)
                {
                  Zebra::logger->debug("gateway reg maps(%u,%u,%u)",rev->mps[i].maptempid,rev->mps[i].mapx,rev->mps[i].mapy);
                  mapIndex[rev->mps[i].maptempid] = index;
                }
              }

              return true;
            }
            break;
          case PARA_REMOVE_MAPINDEX:
            {
              t_Remove_MapIndex *rev=(t_Remove_MapIndex*)pNullCmd;
              for(int i = 0 ; i < (int)rev->dwSize ; i ++)
              {
                MapIndexIter iter = mapIndex.find(rev->dwMapTempID[i]); 
                if (iter != mapIndex.end())
                {
                  Zebra::logger->debug("gateway unreg maps(%u)",rev->dwMapTempID[i]);
                  SAFE_DELETE(iter->second);
                  mapIndex.erase(iter);
                }
              }
            }
            break;
          case PARA_FRESH_SCREENINDEX:
            {
              t_fresh_ScreenIndex* rev = (t_fresh_ScreenIndex*)pNullCmd;
              GateUser* pUser = this->getUserByIndex(rev->dwUserTempID);
              if (pUser)
              {
                //Zebra::logger->debug("用户屏索引变化(userID=%u,oldscreen = %u,newscreen=%u)",pUser->id,pUser->getIndexKey(),rev->dwScreen);
                if ((DWORD)-1 == rev->dwScreen)
                  removeIndex(pUser,rev->dwMapTempID);
                else
		{//if (pUser->isPlayState())
                  freshIndex(pUser,rev->dwMapTempID,rev->dwScreen);
		}
              }
              return true;
            }
            break;
            //踢人
          case PARA_KICK_USER:
            {
              Cmd::Scene::t_kickUser_GateScene* rev = (Cmd::Scene::t_kickUser_GateScene*)pNullCmd;
              GateUser* pUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->userID);
              if (!pUser) pUser = (GateUser*)GateUserManager::getInstance()->getUserByAccID(rev->accid);
              if (pUser)
                pUser->TerminateWait();
              return true;
            }
            break;
          case PARA_SCENE_SYS_SETTING:
            {
              Cmd::Scene::t_sysSetting_GateScene* rev = (Cmd::Scene::t_sysSetting_GateScene*)pNullCmd;
              GateUser* pUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->id);
              if (pUser)
              {
                bcopy(rev->sysSetting,pUser->sysSetting,sizeof(pUser->sysSetting));
#ifdef _DEBUG
                Zebra::logger->debug("收到系统设置消息:%x %x %x %x",pUser->sysSetting[0],pUser->sysSetting[1],pUser->sysSetting[2],pUser->sysSetting[3]);
#endif
              }
              return true;
            }
            break;
            //停止金币服务
          case PARA_SERVICE_GOLD:
            {
              Cmd::Scene::t_ServiceGold_GateScene *rev=(Cmd::Scene::t_ServiceGold_GateScene *)pNullCmd;
              {
                if (rev->byType==Cmd::Scene::ZT_SERVICE_STOP)
                {
                  GatewayService::service_gold=false;
                  Zebra::logger->debug("停止金币服务");
                }
                else if (rev->byType==Cmd::Scene::ZT_SERVICE_START)
                {
                  GatewayService::service_gold=true;
                  Zebra::logger->debug("启动金币服务");
                }
              }
              return false;
            }
            break;
            //停止股票服务
          case PARA_SERVICE_STOCK:
            {
              Cmd::Scene::t_ServiceStock_GateScene *rev=(Cmd::Scene::t_ServiceStock_GateScene *)pNullCmd;
              {
                if (rev->byType==Cmd::Scene::ZT_SERVICE_STOP)
                {
                  GatewayService::service_stock=false;
                  Zebra::logger->debug("停止股票服务");
                }
                else if (rev->byType==Cmd::Scene::ZT_SERVICE_START)
                {
                  GatewayService::service_stock=true;
                  Zebra::logger->debug("启动股票服务");
                }
              }
              return false;
            }
            break;
          case PARA_USLEEP_TIME:
            {
              Cmd::Scene::t_Usleep_GateScene *rev = (Cmd::Scene::t_Usleep_GateScene *)pNullCmd;
              if (rev->utask)
              {
                zTCPTaskPool::setUsleepTime(rev->utask);
                Zebra::logger->info("设置系统Task的usleep时间:%dus",rev->utask);
              }
              if (rev->uclient)
              {
                SceneClientManager::getInstance().setUsleepTime(rev->uclient);
                Zebra::logger->info("设置系统Client的usleep时间:%dus",rev->uclient);
              }
            }
            break;
	    default:
            break;
        }
      }
      break;

    case CMD_FORWARD:
      {
        switch(pNullCmd->para)
        {
		  case PARA_FORWARD_DUPCHANGE:
			 {
				t_ForwardScene_dupChange  *rev=(t_ForwardScene_dupChange *)pNullCmd;
				GateUser *pUser=(GateUser *)GateUserManager::getInstance()->getUserByID(rev->dwID);
				if(pUser)
					pUser->dupIndex = rev->dupIndex;
				return true;
			 }
			 break;
          case PARA_FORWARD_NINE:
            {       
              t_Nine_ForwardScene  *rev=(t_Nine_ForwardScene *)pNullCmd;
              MapIndexIter iter = mapIndex.find(rev->maptempid); 
              if (iter != mapIndex.end())
                iter->second->sendCmdToNine(rev->screen,rev->data,rev->size);
              return true;
            }       
            break; 
          case PARA_FORWARD_NINE_DIR:
            {       
              t_Nine_dir_ForwardScene  *rev=(t_Nine_dir_ForwardScene *)pNullCmd;
              MapIndexIter iter = mapIndex.find(rev->maptempid); 
              if (iter != mapIndex.end())
                iter->second->sendCmdToDirect(rev->screen,rev->dir,rev->data,rev->size,rev->dupIndex);
              return true;
            }       
            break; 
          case PARA_FORWARD_NINE_RDIR:
            {       
              t_Nine_rdir_ForwardScene  *rev=(t_Nine_rdir_ForwardScene *)pNullCmd;
              MapIndexIter iter = mapIndex.find(rev->maptempid); 
              if (iter != mapIndex.end())
                iter->second->sendCmdToReverseDirect(rev->screen,rev->dir,rev->data,rev->size,rev->dupIndex);
              return true;
            }       
            break; 
          case PARA_FORWARD_USER:
            {
              t_User_ForwardScene *rev=(t_User_ForwardScene *)pNullCmd;
              GateUser *pUser=this->getUserByIndex(rev->dwID);
	      int container = 0;
	      if(!pUser)
	      {
		  container = 1;
		  pUser = (GateUser*)GateUserManager::getInstance()->getUserByID(rev->dwID);
	      }
	      if(pUser && !pUser->isTerminateWait() && !pUser->isTerminate() /*&& !pUser->isLogOut()*/)
	      {
		  pUser->sendCmd(rev->data, rev->size);
		  Zebra::logger->info("场景发用户%u的消息已经发出[cmd=%u para=%u] 大小:%u",rev->dwID,((Cmd::stNullUserCmd *)rev->data)->byCmd,((Cmd::stNullUserCmd *)rev->data)->byParam, rev->size);
	      }
              return true;
            }
            break;
          case PARA_FORWARD_MAP:
            {       
              t_User_ForwardMap  *rev=(t_User_ForwardMap *)pNullCmd;
              MapIndexIter iter = mapIndex.find(rev->maptempid); 
              if (iter != mapIndex.end())
                iter->second->sendCmdToAll(rev->data,rev->size);
              return true;
            }       
            break;  
          case PARA_FORWARD_SCENEUSER_TO_BILL:
            {
              t_Scene_ForwardSceneUserToBill *rev=(t_Scene_ForwardSceneUserToBill *)pNullCmd;
              GateUser *pUser=(GateUser *)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
              if (!pUser ||  !pUser->forwardBillScene((const Cmd::stNullUserCmd *)rev->data,rev->size))
              {
                Zebra::logger->debug("转发%u到Bill的消息(%u,%u)失败",rev->dwUserID,rev->cmd,rev->para);
              }
              return true;
            }
            break;
          case PARA_FORWARD_SCENE_TO_BILL:
            {
              t_Scene_ForwardSceneToBill *rev=(t_Scene_ForwardSceneToBill *)pNullCmd;
              GateUser *pUser=(GateUser *)GateUserManager::getInstance()->getUserByID(rev->dwUserID);
              if (!pUser ||  !pUser->forwardBill((const Cmd::stNullUserCmd *)rev->data,rev->size))
              {
                Zebra::logger->debug("转发%u到Bill的消息(%u,%u)失败",rev->dwUserID,rev->cmd,rev->para);
              }
              return true;
            }
            break;
          case PARA_FORWARD_NINE_EXCEPTME:
            {       
              t_Nine_ExceptMe_ForwardScene  *rev=(t_Nine_ExceptMe_ForwardScene *)pNullCmd;
              MapIndexIter iter = mapIndex.find(rev->maptempid); 
              if (iter != mapIndex.end())
                iter->second->sendCmdToNineExceptMe(rev->screen,rev->exceptme_id,rev->data,rev->size);
              return true;
            }       
            break; 
          default:
            break;
        }
      }
      break;
    default:
      break;
  }

  Zebra::logger->error("SceneClient::msgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
  return false;
}

bool SceneClient::doGmCmd(const Cmd::Scene::t_gmCommand_SceneGate* pCmd)
{
    using namespace Cmd::Scene;
    switch(pCmd->gm_cmd)
    {
	case GM_COMMAND_CMD_FILTER:
	    {
		if(pCmd->para1 == 1)
		    GatewayTask::cmdCheck.put((WORD)pCmd->para2);
		else if(pCmd->para1 == 2)
		    GatewayTask::cmdCheck.erase(pCmd->para2);
		else if(pCmd->para1 == 3)
		    GatewayTask::cmdCheck.list();

		return true;
	    }
	    break;
    }
    return true;
}
