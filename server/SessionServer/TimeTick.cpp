/**
* \brief 时间回调函数
*
* 
*/

#include "SessionServer.h"
#include "TimeTick.h"
#include "SessionChat.h"
#include "SessionTaskManager.h"
#include "RecordClient.h"
#include "HeroCardManager.h"

zRTime SessionTimeTick::currentTime;
SessionTimeTick *SessionTimeTick::instance = NULL;

#if 0
struct rushCallback : public CCountryM::countryCallback
{
	DWORD rushID;
	DWORD delay;
	DWORD cityID;

	rushCallback(DWORD r,DWORD d,DWORD c):rushID(r),delay(d),cityID(c){}

	void exec(CCountry * c)
	{
		if (rushID && cityID && c)
		{
			Cmd::Session::t_createRush_SceneSession cr;
			cr.rushID = rushID;
			cr.delay = delay;
			cr.countryID = c->dwID;

			SceneSession * ss = SceneSessionManager::getInstance()->getSceneByID((c->dwID<<16)+cityID);
			if (ss)
			{
				ss->sendCmd(&cr,sizeof(cr));
				Zebra::logger->warn("向 %s 发送怪物攻城消息",c->name);
			}
			else
				Zebra::logger->warn("向 %s 发送怪物攻城消息失败",c->name);
		}
	}
};
#endif

void SessionTimeTick::run()
{
	//int rush = 0;
	while(!isFinal())
	{
		zThread::msleep(50);

		//获取当前时间
		currentTime.now();

		recordClient->doCmd();
		SessionTaskManager::getInstance().execEvery();
		if (_five_sec(currentTime))
		{
		    HeroCardManager::getMe().timer();	//��������ƥ��
		}
#if 0
		if (_five_sec(currentTime)) {
			CDareM::getMe().timer();
			CNpcDareM::getMe().timer();
			CCityM::getMe().timer();
			CArmyM::getMe().timer();
		}

		if (_one_sec(currentTime))
		{
			CQuizM::getMe().timer();
			CUnionM::getMe().timer();
			CAllyM::getMe().timer();
			CArenaManager::getInstance().Arena_timer();
		}

		if (_ten_min(currentTime))
		{
			AuctionService::getMe().checkDB();
		}
#endif
		// [ranqd Add] Session每五分钟同步一次服务器在线状态
		if( _five_min(currentTime) )
		{
			UserSessionManager::getInstance()->notifyOnlineToGate();
		}

		if (_one_min(currentTime))
		{
			SessionService::getInstance().checkShutdown();
#if 0
			//SessionService::getInstance().checkGumu();
			CCountryM::getMe().timer();
			//CGemM::getMe().timer();
			CartoonPetService::getMe().writeAllToDB();
			EmperorForbid::getMe().timer();
#endif
			//定时动作
			time_t timValue = time(NULL);
			struct tm tmValue;
			zRTime::getLocalTime(tmValue,timValue);
			SessionService::getInstance().checkCountry(tmValue);

			//GM公告
			for (int i=0; i<=5; i++)
			{
				if (SessionService::wMsg[i].time)
				{
					if (SessionService::wMsg[i].count)
						SessionService::wMsg[i].count--;
					else
					{
						if (SessionService::wMsg[i].country)
							if (SessionService::wMsg[i].mapID)//区域公告
							{
								SceneSession *scene = SceneSessionManager::getInstance()
									->getSceneByID((SessionService::wMsg[i].country<<16)+SessionService::wMsg[i].mapID);
								if (scene)
								{
									Cmd::Session::t_broadcastScene_SceneSession send;
									strncpy(send.info,SessionService::wMsg[i].msg,MAX_CHATINFO);
									strncpy(send.GM,SessionService::wMsg[i].GM,MAX_NAMESIZE);
									send.mapID = (SessionService::wMsg[i].country<<16)+SessionService::wMsg[i].mapID;
									scene->sendCmd(&send,sizeof(send));
#ifdef _DEBUG
									Zebra::logger->debug("GM公告:%s:%s mapID=%u",send.GM,send.info,send.mapID);
#endif
								}
							}
							else//国家公告
								SessionChannel::sendCountryInfo(Cmd::INFO_TYPE_SCROLL,SessionService::wMsg[i].country,SessionService::wMsg[i].msg);
						else//世界公告
							SessionChannel::sendAllInfo(Cmd::INFO_TYPE_SCROLL,SessionService::wMsg[i].msg);

						SessionService::wMsg[i].count = SessionService::wMsg[i].interval;
						SessionService::wMsg[i].time--;
					}
				}
				if (0==SessionService::wMsg[i].time)
					SessionService::wMsg.erase(i);
			}
		}
#if 0
		if (_one_hour(currentTime))
		{
			MailService::getMe().checkDB();
			CVoteM::getMe().timer();
			CCountryM::getMe().timerPerHour();
			CCountryM::getMe().save();
		}
#endif
	}
}


/////////////////////////备用代码////////////////////////////

/*定时怪物攻城
if ((tmValue.tm_mday>=27 && tmValue.tm_mday<=31)
&& (tmValue.tm_hour==21 && tmValue.tm_min>=13 && tmValue.tm_min<=18))
{
switch (rush)
{
case 0:
case 1:
rush++;
Zebra::logger->info("运营活动，2分钟后开始攻城");
break;
case 2:
rushCallback rcb(4,900,102);
CCountryM::getMe().execEveryCountry(rcb);

SceneSession * ss = SceneSessionManager::getInstance()->getSceneByID((6<<16)+102);
if (ss)
{
Cmd::Session::t_createRush_SceneSession cr;
cr.rushID = 4;
cr.delay = 900;
cr.countryID = 102;

ss->sendCmd(&cr,sizeof(cr));
}
else
Zebra::logger->warn("向 无国籍国家 发送怪物攻城消息失败");

rush ++;
Zebra::logger->info("运营活动，9点全国怪物攻城");
break;
}
}
else if (3==rush)
rush = 0;
*/


/*
if (tmValue.tm_mday==28 &&
(tmValue.tm_hour==17 && tmValue.tm_min>=43 && tmValue.tm_min<=59))
//if (tmValue.tm_hour==8 && tmValue.tm_min>=43 && tmValue.tm_min<=59)
{
switch (rush)
{
case 0:
{
rush++;
Zebra::logger->info("运营活动，2分钟后福神拜年");
}
break;
case 1:
{
rush++;
Zebra::logger->info("运营活动，1分钟后福神拜年");
}
break;
case 2:
{
rushCallback rcb(7,900,102);
CCountryM::getMe().execEveryCountry(rcb);

SceneSession * ss = SceneSessionManager::getInstance()->getSceneByID((6<<16)+102);
if (ss)
{
Cmd::Session::t_createRush_SceneSession cr;
cr.rushID = 7;
cr.delay = 900;
cr.countryID = 6;

ss->sendCmd(&cr,sizeof(cr));
}
else
Zebra::logger->warn("向 无国籍国家 发送怪物攻城消息失败");

rush ++;
Zebra::logger->info("运营活动，18点福神拜年");
}
break;
case 3:
{
rush++;
Zebra::logger->info("运营活动，4分钟后财神拜年");
}
break;
case 4:
{
rush++;
Zebra::logger->info("运营活动，3分钟后财神拜年");
}
break;
case 5:
{
rush++;
Zebra::logger->info("运营活动，2分钟后财神拜年");
}
break;
case 6:
{
rush++;
Zebra::logger->info("运营活动，1分钟后财神拜年");
}
break;
case 7:
{
rushCallback rcb(8,900,102);
CCountryM::getMe().execEveryCountry(rcb);

SceneSession * ss = SceneSessionManager::getInstance()->getSceneByID((6<<16)+102);
if (ss)
{
Cmd::Session::t_createRush_SceneSession cr;
cr.rushID = 8;
cr.delay = 900;
cr.countryID = 6;

ss->sendCmd(&cr,sizeof(cr));
}
else
Zebra::logger->warn("向 无国籍国家 发送怪物攻城消息失败");

rush ++;
Zebra::logger->info("运营活动，18点05财神拜年");
}
break;
}
}
else if (8==rush)
rush = 0;
*/
