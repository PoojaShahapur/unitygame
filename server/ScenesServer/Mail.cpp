/*************************************************************************
 Author: wang
 Created Time: 2015年03月26日 星期四 11时09分42秒
 File Name: ScenesServer/Mail.cpp
 Description: 
 ************************************************************************/
#include "Mail.h"
#include "SaveObject.h"
#include "SessionCommand.h"
#include "SessionClient.h"
#include "zObject.h"
#include "zTime.h"

namespace Mail
{
    
    void send(const std::string& sFromName, DWORD dwFromId, const std::string& sToName, DWORD dwToId,
	    BYTE type, DWORD dwMoney, const zObject* o, const std::string& sText, 
	    const zObject* o1, const zObject* o2, const std::string& sTitle)
    {
	Cmd::Session::t_sendMail_SceneSession sm;
	sm.mail.state = Cmd::Session::MAIL_STATE_NEW;
	
	strncpy(sm.mail.fromName, sFromName.c_str(), MAX_NAMESIZE);
	sm.mail.fromID = dwFromId;
	sm.mail.toID = dwToId;
	strncpy(sm.mail.toName, sToName.c_str(), MAX_NAMESIZE);
	if(sTitle.empty())
	    strncpy(sm.mail.title, "SYSTEM_MAIL", MAX_NAMESIZE);
	else
	    strncpy(sm.mail.title, sTitle.c_str(), MAX_NAMESIZE);
	sm.mail.type = type;

	zRTime ct;
	sm.mail.createTime = ct.sec();
	sm.mail.delTime = sm.mail.createTime + 60*60*24*7;	//过期时间

	const size_t TEXT_SIZE = sizeof(sm.mail.text);
	strncpy(sm.mail.text, sText.c_str(), TEXT_SIZE-1);

	sm.mail.sendMoney = dwMoney;
	if(o)
	{
	    o->getSaveData((SaveObject *)&sm.item[0]);
	    sm.mail.itemID[0] = o->data.qwThisID;
	}

	if(o1)
	{
	    o1->getSaveData((SaveObject *)&sm.item[1]);
	    sm.mail.itemID[1] = o1->data.qwThisID;
	}

	if(o2)
	{
	    o2->getSaveData((SaveObject *)&sm.item[2]);
	    sm.mail.itemID[2] = o2->data.qwThisID;
	}

	if(dwMoney || o || o1 || o2)
	    sm.mail.accessory = 1;

	sessionClient->sendCmd(&sm, sizeof(sm));
	Zebra::logger->trace("[邮件]系统发送给玩家 %s item=%s item1=%s item2=%s money=%u",
		sToName.c_str(), o?o->data.strName:"", o1?o1->data.strName:"",
		o2?o2->data.strName:"", dwMoney);


    }

    void sendSysText(const std::string& sFromName, DWORD dwToId, const std::string& sText)
    {
	send(sFromName, 0, "", dwToId, Cmd::Session::MAIL_TYPE_SYS, 0, NULL, sText);
    }
} //end of namespace Mail
