#include "Chat.h"
#include "ScenesServer.h"
#include "SceneUser.h"
#include "SceneUserManager.h"
#include "GmToolCommand.h"
#include "Scene.h"
#include "TimeTick.h"
#include "zXML.h"
#include "SceneTaskManager.h"
#include "zDatabaseManager.h"
#include "QuestEvent.h"
#include "QuestTable.h"
#include "ChallengeGame.h"

extern DWORD cancel_country_need_money;          
extern DWORD is_cancel_country; 


/**
* \brief GM指令数组
*
*/
Gm GmCmds[]=
{
#if 0
	//普通GM用
	{"reloadScript",Gm::reloadScript,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"重新导入脚本"},
	{"help",Gm::help,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"显示指令帮助"},
	{"levelup",Gm::levelup,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"升级"},
	{"goto",Gm::goTo_Gm,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"跳转位置"},
	{"gomap",Gm::gomap_Gm,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"跳转地图"},
	{"finduser",Gm::finduser,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"寻找玩家"},
	{"gotouser",Gm::gotouser,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"跟踪玩家"},
	{"catchuser",Gm::catchuser,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"召唤玩家"},
	{"bczone",Gm::bczone,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"发送区域广播"},
	{"bcwgamemsg",Gm::bcwgamemsg,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"发送游戏性世界广播"},
	{"kick",Gm::kick,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"踢掉玩家"},
	{"donttalk",Gm::donttalk,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"禁言"},
	{"talk",Gm::talk,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"解除禁言"},
	{"gohome",Gm::goHome,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"回到主城"},
	{"hideme",Gm::hideMe,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"隐藏自己"},
	{"showme",Gm::showMe,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"恢复可见状态"},
#endif
	{"god",Gm::god,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"无敌模式"},
#if 0
	{"normal",Gm::normal,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"恢复普通模式"},
	{"clearseptunion",Gm::clearSeptUnion,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"清除老版帮会和家族数据"},
	{"clearpoint",Gm::clearPoint,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"清除人物属性点"},
	{"clearskillpoint",Gm::clearSkillPoint,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"清除技能点"},
	{"studyskill",Gm::studySkill,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"学习新技能"},
	{"settire",Gm::setTire,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"设置疲劳时间"},
	{"checkcountryinfo",Gm::checkCountryInfo,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"检查国家信息"},
	{"loadprocess",Gm::loadProcess,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"重新加载外挂特征码文件"},
	{"cleararea",Gm::clearArea,Gm::gm_mode | Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"驱散周围玩家"},
	{"embar",Gm::embar,Gm::gm_mode | Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"囚禁指定的玩家"},
	{"refreshgeneral",Gm::refreshGeneral,Gm::gm_mode | Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"手动刷新各国大将军"},

	//组长用
	{"closenpc",Gm::closenpc,Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"关闭功能NPC"},
	{"countrypower",Gm::countrypower,Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"处理国家强弱"},
	{"summon",Gm::summon,Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"GM召唤Npc"},
	{"usleep",Gm::usleep,Gm::super_mode | Gm::debug_mode,"线程usleep的毫秒数"},
	{"createrush",Gm::createRush,Gm::debug_mode,"引发一次怪物攻城"},

	//超级GM用
	{"kickgateuser",Gm::kickGateUser,Gm::super_mode | Gm::debug_mode,"踢掉网关上的玩家"},
	{"loadgift",Gm::loadGift,Gm::super_mode | Gm::debug_mode,"重新加载礼官的数据"},
	{"enableregister",Gm::enableRegister,Gm::super_mode | Gm::debug_mode,"允许注册"},
	{"enablelogin",Gm::enableLogin,Gm::super_mode | Gm::debug_mode,"允许登陆"},
	{"setservice",Gm::setService,Gm::super_mode | Gm::debug_mode,"设置游戏功能"},
	{"decitydare",Gm::debugCityDare,Gm::super_mode | Gm::debug_mode,"启动和停止帮会夺城战"},
	{"deempdare",Gm::debugEmperorDare,Gm::super_mode | Gm::debug_mode,"启动和停止皇城争夺战"},
	{"createquiz",Gm::createQuiz,Gm::super_mode | Gm::debug_mode,"发起一次全区智力竞赛"},
	{"setpriv",Gm::setPriv,Gm::super_mode | Gm::debug_mode,"设置角色权限"},
	{"newzone",Gm::newzone,Gm::super_mode | Gm::debug_mode,"设置运行参数"},
	{"stockconsign",Gm::stockconsign,Gm::super_mode | Gm::debug_mode,"股票委托管理"},
	{"killer",Gm::killer,Gm::super_mode | Gm::debug_mode,"必杀模式"},
	{"tong",Gm::tong,Gm::super_mode |Gm::debug_mode,"激活帮会战-使用方法://tong t1=XXX t2=XXX"},
	{"clearworld",Gm::clearWorld,Gm::super_mode |Gm::debug_mode,"清除范围内的怪物"},
	{"showspecialnpc",Gm::showSpecialNpc,Gm::debug_mode,"查看当前的特殊NPC"},
	{"qaccount",Gm::qAccount,Gm::super_mode | Gm::debug_mode,"查询帐号"},
	{"shutdown",Gm::shutdown,Gm::super_mode | Gm::debug_mode,"停机维护"},
	{"systime",Gm::systime,Gm::super_mode | Gm::debug_mode,"得到系统当前时间"},
	{"goldsystem",Gm::goldsystem,Gm::super_mode | Gm::debug_mode,"金币系统"},
	{"stocksystem",Gm::stocksystem,Gm::super_mode | Gm::debug_mode,"股票系统"},

	//调试用
	{"viewcountrydare",Gm::viewCountryDare,Gm::debug_mode,"查看国战记录"},
	{"givegold",Gm::givegold,Gm::debug_mode,"金币补偿"},
	{"givemoney",Gm::givemoney,Gm::debug_mode,"银币补偿"},
	{"givestock",Gm::givestock,Gm::debug_mode,"股票补偿"},
	{"version",Gm::version,Gm::debug_mode,"动态设置服务器版本号"},
	{"createunion",Gm::createUnion,Gm::debug_mode,"创建一个帮会"},
	{"createfamily",Gm::createFamily,Gm::debug_mode,"创建一个家族"},
	{"debugvote",Gm::debugVote,Gm::debug_mode,"启动和停止投票"},
	{"debuggem",Gm::debugGem,Gm::debug_mode,"启动和停止护宝任务"},
	{"addsepexp",Gm::addSeptExp,Gm::debug_mode,"领取家族经验"},
	{"setrepute",Gm::setRepute,Gm::debug_mode,"设置家族声望"},
	{"setseptlevel",Gm::setSeptLevel,Gm::debug_mode,"设置家族等级"},
	{"setallyfd",Gm::setAllyFriendDegree,Gm::debug_mode,"设置联盟友好度"},
	{"changecountry",Gm::changeCountry,Gm::debug_mode,"开启和停止叛国功能"},
	{"loadmap",Gm::loadMap,Gm::debug_mode,"加载地图"},
	{"unloadmap",Gm::unloadMap,Gm::debug_mode,"卸载地图"},
	{"callpet",Gm::callPet,Gm::debug_mode,"召唤宠物"},
	{"lockvalue",Gm::lockValue,Gm::debug_mode,"锁定消耗"},
	{"svote",Gm::svote,Gm::debug_mode,"家族投票-使用方法://svote name=XXX"},
	{"uvote",Gm::uvote,Gm::debug_mode,"帮会投票-使用方法://uvote name=XXX"},
	{"createschool",Gm::createSchool,Gm::debug_mode,"创建一个门派"},
	{"fetch",Gm::fetch,Gm::debug_mode,"GM造物品"},
	{"showentries",Gm::showEntries,Gm::debug_mode,"查看周围的npc和玩家"},
	{"setpetai",Gm::setPetAI,Gm::debug_mode,"设置宠物行为模式"},
	{"setpetaif",Gm::setPetAIF,Gm::debug_mode,"设置宠物AIF"},
	{"bank",Gm::bank,Gm::debug_mode,"设置仓库相关"},
	{"backoff",Gm::backoff,Gm::debug_mode,"GM造物品"},
	{"upskill",Gm::upgradeSkill,Gm::debug_mode,"学习魔法"},
	{"abandon",Gm::abandon,Gm::debug_mode,"清除任务"},
	{"showmaps",Gm::showMaps,Gm::debug_mode,"查看已经加载的地图"},
	{"showpets",Gm::showPets,Gm::debug_mode,"查看当前的宠物"},
	{"killpets",Gm::killPets,Gm::debug_mode,"查看当前的宠物"},
	{"leechdom",Gm::leechdom,Gm::debug_mode,"使用药品"},
	{"value",Gm::value,Gm::debug_mode,"设置hp,mp,sp"},
	{"goodness",Gm::goodness,Gm::debug_mode,"取得当前善恶度"},
	{"getvalue",Gm::getvalue,Gm::debug_mode,"取得某个数值,方便QA测试,目前只有魅力"},
	{"setvalue",Gm::setvalue,Gm::debug_mode,"设置某个数值,方便QA测试"},
	{"skill",Gm::skill,Gm::debug_mode,"技能测试指令"},
	{"addexp",Gm::addExp,Gm::debug_mode,"增加经验值"},
	{"setquest",Gm::setQuest,Gm::debug_mode,"更改任务变量"},
	{"checkquest",Gm::checkQuest,Gm::debug_mode,"检查可以接的任务"},
	{"setblock",Gm::setBlock,Gm::debug_mode,"设置阻挡"},
	{"checkblock",Gm::checkBlock,Gm::debug_mode,"设置阻挡"},
	{"npcdare",Gm::npcDare,Gm::debug_mode,"立即触发NPC争夺战"},
	{"team",Gm::team,Gm::debug_mode,"组队"},
	{"throwobject",Gm::throwobject,Gm::debug_mode,"扔东西"},
	{"redeemgold",Gm::redeemgold,Gm::debug_mode,"兑换金币"},
	{"querygold",Gm::querygold,Gm::debug_mode,"兑换金币"},
	{"redeemmonthcard",Gm::redeemmonthcard,Gm::debug_mode,"兑换月卡"},
	{"stockuser",Gm::stockuser,Gm::debug_mode,"股票用户管理"},
	{"stocktransfer",Gm::stocktransfer,Gm::debug_mode,"股票资金管理"},
	{"clearcolddown",Gm::clearColddown,Gm::debug_mode,"清除冷却时间"},
	{"queryincmap",Gm::queryincmap,Gm::debug_mode,"请求增值地图列表"},
	{"showaddexp",Gm::showAddExp,Gm::debug_mode,"显示替身 宠物附加的经验"},
	{"changehorse",Gm::changeHorse,Gm::debug_mode,"换马"},
	{"contribute",Gm::contribute,Gm::debug_mode,"捐献国库"},
	{"clearskill",Gm::clearSkill,Gm::debug_mode,"清除指定技能"},
	{"getgive",Gm::getgive,Gm::debug_mode,"获取赠品"},
#endif
	{"loadxml", Gm::loadxml, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "���������ļ�"},
	{"loadquest", Gm::loadQuest, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "���ؽű��ļ�"},
	{"cmdfilter", Gm::cmdfilter, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "ָ������"},
	{"fetch",Gm::fetch, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "fetch OBJECT"},
	{"fetchcard",Gm::fetchCard, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "fetch CARD"},
	{"drawcard",Gm::drawCard, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "draw CARD"},
	{"addmoney",Gm::addMoney, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "add game money"},
	{"removemoney",Gm::removeMoney, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "remove game money"},
	{"addgold",Gm::addGold, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "add gold"},
	{"removegold",Gm::removeGold, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "remove gold"},
	{"goto",Gm::goTo_Gm,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"��תλ��"},
	{"gomap",Gm::gomap_Gm,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"��ת��ͼ"},
	{"kick",Gm::kick,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"kick user offline"},
	{"clearworld",Gm::clearWorld,Gm::super_mode |Gm::debug_mode,"清除范围内的怪物"},
	{"fightmatch",Gm::fightmatch, Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"�����սƥ��"},
	{"changezone",Gm::changeZone, Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"�����ս��"},
	{"bcworld",Gm::bcworld,Gm::gm_mode |Gm::captain_mode | Gm::super_mode | Gm::debug_mode,"发送世界广播"},
	{"userdistribute",Gm::countServerUser,Gm::super_mode | Gm::debug_mode,"得到本组服务器的人数分布"},
	{"getsize",Gm::getsize,Gm::debug_mode,"查看角色所占空间"},
	{"querypoint",Gm::querypoint,Gm::debug_mode,"查询点数"},
	{"loadeffect",Gm::loadCardEffect, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "���ؿ���Ч������"},
	{"creategroup",Gm::createGroup, Gm::gm_mode | Gm::super_mode | Gm::captain_mode | Gm::debug_mode, "����һ������"},
	{NULL,NULL,0,NULL}
};





/*
* \brief 向用户发送消息
* 如果用户不在该服务器,就由session转发
*
* \param id 用户的ID
* \param cmd 消息
* \param len 消息长度
*
* \return 是否成功
*/
bool sendCmdByID(DWORD id,const void *cmd,int len)
{
	SceneUser * pUser = SceneUserManager::getMe().getUserByID(id);
	if (pUser)
		pUser->sendCmdToMe(cmd,len);
	else
	{
		char buf[zSocket::MAX_DATASIZE];
		Cmd::Session::t_forwardUser_SceneSession * send = (Cmd::Session::t_forwardUser_SceneSession *)buf;
		constructInPlace(send);

		send->id = id;
		send->cmd_len = len;
		bcopy(cmd,send->cmd,len);

		sessionClient->sendCmd(send,sizeof(Cmd::Session::t_forwardUser_SceneSession)+len);
	}
	return true;
}


/*
* \brief 向用户发送消息
* 如果用户不在该服务器,就由session转发
*
* \param id 用户的临时ID
* \param cmd 消息
* \param len 消息长度
*
* \return 是否成功
*/
bool sendCmdByTempID(DWORD id,const void *cmd,int len)
{
	SceneUser * pUser = SceneUserManager::getMe().getUserByTempID(id);
	if (pUser)
		pUser->sendCmdToMe(cmd,len);
	else
	{
		char buf[zSocket::MAX_DATASIZE];
		Cmd::Session::t_forwardUser_SceneSession * send = (Cmd::Session::t_forwardUser_SceneSession *)buf;
		constructInPlace(send);

		send->tempid = id;
		send->cmd_len = len;
		bcopy(cmd,send->cmd,len);

		sessionClient->sendCmd(send,sizeof(Cmd::Session::t_forwardUser_SceneSession)+len);
	}
	return true;
}


/*
* \brief 向用户发送消息
* 如果用户不在该服务器,就由session转发
*
* \param name 用户的名字 
* \param cmd 消息
* \param len 消息长度
*
* \return 是否成功
*/
bool sendCmdByName(char * name,const void *cmd,int len)
{
	SceneUser * pUser = SceneUserManager::getMe().getUserByName(name);
	if (pUser)
		pUser->sendCmdToMe(cmd,len);
	else
	{
		char buf[zSocket::MAX_DATASIZE];
		Cmd::Session::t_forwardUser_SceneSession * send = (Cmd::Session::t_forwardUser_SceneSession *)buf;
		constructInPlace(send);

		strncpy(send->name,name,MAX_NAMESIZE);
		send->cmd_len = len;
		bcopy(cmd,send->cmd,len);

		sessionClient->sendCmd(send,sizeof(Cmd::Session::t_forwardUser_SceneSession)+len);
	}
	return true;
}

/**
* \brief 构造函数
*
*
* \param pUser: 创建该频道的用户
*/
Channel::Channel(SceneUser *pUser):zEntry()
{
	strncpy(name,pUser->name,MAX_NAMESIZE);
	creater.id=pUser->id;
	creater.tempid=pUser->tempid;
	strncpy(creater.name,pUser->name,MAX_NAMESIZE);
}

/**
* \brief 发送给该频道的所有用户
*
*
* \param cmd: 聊天内容
* \param len:内容长度
* \return 发送是否成功
*/
bool Channel::sendCmdToAll(const void *cmd,int len)
{
	if (cmd==NULL) return false;
	for(DWORD i=0;i<userlist.size();i++)
	{
		SceneUser *temp = SceneUserManager::getMe().getUserByName(userlist[i].name);
		if (temp!=NULL)
		{
			temp->sendCmdToMe(cmd,len);
		}
	}
	return true;
}

/**
* \brief 离开频道
*
*
* \param uname: 离开该频道的用户
* \return 该频道是否还能继续存在
*/
bool Channel::remove(const char *uname)
{
	WORD found=has(uname);
	if (found!=(WORD)-1)
	{
		Zebra::logger->info("%s离开%s的聊天频道",uname,creater.name);
		Cmd::stLeave_ChannelUserCmd send;
		send.dwChannelID=tempid;
		strncpy(send.name,uname,MAX_NAMESIZE);
		sendCmdToAll(&send,sizeof(send));

		if ((found+1)<(WORD)userlist.size())
		{
			userlist[found]=userlist.back();
		}
		userlist.pop_back();
	}
	if (userlist.empty())
		return false;
	else
		return true;
}

/**
* \brief 加入聊天频道
*
*
* \param pUser: 要加入的用户
* \return 加入是否成功
*/
bool Channel::add(const char *uname)
{
	/*
	if (uname!=NULL)
	{
	WORD found=has(uname);
	if (found==(WORD)-1)
	{
	Zebra::logger->info("%s加入%s的聊天频道",uname,creater.name);
	//添加
	zEntryC temp;
	temp.id=0;
	temp.tempid=0;
	strncpy(temp.name,uname,MAX_NAMESIZE);
	userlist.push_back(temp);

	//把自己发送给所有人
	Cmd::stJoin_ChannelUserCmd send;
	send.dwChannelID=tempid;
	strncpy(send.name,uname,MAX_NAMESIZE);
	sendCmdToAll(&send,sizeof(send));

	//发送所有成员给自己
	for(DWORD i=0;i<userlist.size();i++)
	{
	strncpy(send.name,userlist[i].name,MAX_NAMESIZE);
	pUser->sendCmdToMe(&send,sizeof(send));
	}
	return true;
	}
	}
	*/
	return false;
}

/**
* \brief 加入聊天频道
*
*
* \param pUser: 要加入的用户
* \return 加入是否成功
*/
bool Channel::add(SceneUser *pUser)
{
	if (pUser!=NULL)
	{
		WORD found=has(pUser->name);
		if (found==(WORD)-1)
		{
			Zebra::logger->info("%s加入%s的聊天频道",pUser->name,creater.name);
			//添加
			zEntryC temp;
			temp.id=pUser->id;
			temp.tempid=pUser->tempid;
			strncpy(temp.name,pUser->name,MAX_NAMESIZE);
			userlist.push_back(temp);

			//把自己发送给所有人
			Cmd::stJoin_ChannelUserCmd send;
			send.dwChannelID=tempid;
			strncpy(send.name,pUser->name,MAX_NAMESIZE);
			sendCmdToAll(&send,sizeof(send));

			//发送所有成员给自己
			for(DWORD i=0;i<userlist.size();i++)
			{
				strncpy(send.name,userlist[i].name,MAX_NAMESIZE);
				pUser->sendCmdToMe(&send,sizeof(send));
			}
			return true;
		}
	}
	return false;
}

/**
* \brief 聊天频道中是否存在某人
*
*
* \param name: 用户名
* \return 存在返回该用户在频道中的位置,不存在返回-1
*/
WORD Channel::has(const char *name)
{
	if (name)
	{
		for(DWORD i=0;i<userlist.size();i++)
		{
			if (strncmp(userlist[i].name,name,MAX_NAMESIZE)==0)
				return i;
		}
	}
	return (WORD)-1;
}

/**
* \brief 发送聊天信息到9屏
*
*
* \param pUser: 请求发送者
* \param content: 发送的内容
* \return 发送是否成功
*/
bool Channel::sendNine(SceneUser *pUser,const char *content)
{
	if (content[0]=='/' && content[1]=='/')
	{
		Gm::exec(pUser,(char *)content+2);
	}
	else
	{
		Cmd::stKokChatUserCmd send;
		zRTime ctv;
		send.dwType=Cmd::CHAT_TYPE_NINE;
		strncpy((char *)send.pstrChat,(char *)content,MAX_CHATINFO-1);
			strncpy((char *)send.pstrName,pUser->name,MAX_NAMESIZE);
		pUser->scene->sendCmdToNine(pUser->getPosI(),&send,sizeof(send),pUser->dupIndex);
	}
	return true;
}

/**
* \brief 转发聊天信息到9屏
*
*
* \param pUser: 请求发送者
* \param rev: 转发的内容 
* \param cmdLen: 内容长度
* \return 发送是否成功
*/
bool Channel::sendNine(SceneUser *pUser,const Cmd::stKokChatUserCmd *rev,DWORD cmdLen)
{
	if (rev->pstrChat[0]=='/' && rev->pstrChat[1]=='/')
	{
		Gm::exec(pUser,(char *)rev->pstrChat+2);
	}
	else
	{
		pUser->scene->sendCmdToNine(pUser->getPosI(),rev,cmdLen,pUser->dupIndex);
	}
	return true;
}

#define getMessage(msg,msglen,pat)  \
	do  \
{  \
	va_list ap;  \
	bzero(msg,msglen);  \
	va_start(ap,pat);    \
	vsnprintf(msg,msglen - 1,pat,ap);  \
	va_end(ap);  \
}while(false)

/**
* \brief 在频道中转发信息
*
*
* \param pUser: 请求转发者
* \param pattern: 转发的内容
* \return 是否成功
*/
bool Channel::sendToAll(SceneUser *pUser,const char *pattern,...)
{
	char buf[MAX_CHATINFO];
	getMessage(buf,MAX_CHATINFO,pattern);
	zRTime ctv;
	Cmd::stChannelChatUserCmd send;
	send.dwType=Cmd::CHAT_TYPE_PERSON;
	send.dwChannelID=tempid;
	bzero(send.pstrName,sizeof(send.pstrName));
	bzero(send.pstrChat,sizeof(send.pstrChat));
	strncpy((char *)send.pstrChat,buf,MAX_CHATINFO-1);
	strncpy((char *)send.pstrName,pUser->name,MAX_NAMESIZE);
	for(DWORD i=0;i<userlist.size();i++)
	{
		SceneUser *temp = SceneUserManager::getMe().getUserByName(userlist[i].name);
		if (temp!=NULL && temp!=pUser)
		{
			temp->sendCmdToMe(&send,sizeof(send));
		}
	}
	return true;
}

/**
* \brief 在频道中转发信息
*
*
* \param pUser: 请求转发者
* \param rev: 转发的指令
* \param cmdLen: 指令长度
* \return 是否成功
*/
bool Channel::sendToAll(SceneUser *pUser,const Cmd::stKokChatUserCmd *rev,DWORD cmdLen)
{
	if (rev->pstrChat[0]=='/' && rev->pstrChat[1]=='/')
	{
		Gm::exec(pUser,(char *)rev->pstrChat+2);
	}
	else
	{
		for(DWORD i=0;i<userlist.size();i++)
		{
			SceneUser *temp = SceneUserManager::getMe().getUserByName(userlist[i].name);
			if (temp!=NULL && temp!=pUser)
			{
				temp->sendCmdToMe(rev,cmdLen);
			}
		}
	}
	return true;
}

/**
* \brief 发送国家公告
*
*
* \param countryID: countryID
* \param infoType: infoType
* \param pattern: 内容
* \return 发送是否成功
*/
bool Channel::sendCountryInfo(DWORD countryID,DWORD infoType,const char *pattern,...)
{
	char buf[MAX_CHATINFO];
	getMessage(buf,MAX_CHATINFO,pattern);

	Cmd::Session::t_countryNotify_SceneSession send;
	send.infoType = infoType;
	send.dwCountryID = countryID;
	strncpy(send.info,buf,MAX_CHATINFO);
	sessionClient->sendCmd(&send,sizeof(send));
	return true;
}

/**
* \brief 发送全服公告
*
* \param infoType: infoType
* \param pattern: 内容
* \return 发送是否成功
*/
bool Channel::sendAllInfo(DWORD infoType,const char *pattern,...)
{
	char buf[MAX_CHATINFO];
	getMessage(buf,MAX_CHATINFO,pattern);

	Cmd::Session::t_serverNotify_SceneSession send;
	send.infoType = infoType;
	strncpy(send.info,buf,MAX_CHATINFO);
	sessionClient->sendCmd(&send,sizeof(send));
	return true;
}

/**
* \brief 发送信息到国家频道
*
*
* \param pUser: 请求发送者
* \param pattern: 内容
* \return 发送是否成功
*/
bool Channel::sendCountry(SceneUser *pUser,const char *pattern,...)
{
	char buf[MAX_CHATINFO];
	getMessage(buf,MAX_CHATINFO,pattern);
	Cmd::stChannelChatUserCmd send;
	send.dwCharType = 1;
	zRTime ctv;
	send.dwType=Cmd::CHAT_TYPE_COUNTRY;
	//  send.dwChannelID=tempid;
	bzero(send.pstrName,sizeof(send.pstrName));
	bzero(send.pstrChat,sizeof(send.pstrChat));
	strncpy((char *)send.pstrChat,buf,MAX_CHATINFO-1);
	strncpy((char *)send.pstrName,pUser->name,MAX_NAMESIZE);
	pUser->forwardSession(&send,sizeof(Cmd::stChannelChatUserCmd));
	return true;
}

/**
* \brief 请求发送信息到国家频道
*
*
* \param pUser: 请求指令的用户
* \param rev: 收到的指令
* \param cmdLen: 指令长度
* \return 发送是否成功
*/
bool Channel::sendCountry(SceneUser *pUser,const Cmd::stKokChatUserCmd *rev,DWORD cmdLen)
{
	if (rev->pstrChat[0]=='/' && rev->pstrChat[1]=='/')
	{
		Gm::exec(pUser,(char *)rev->pstrChat+2);
	}
	else
	{
		pUser->forwardSession(rev,cmdLen);
	}
	return true;
}

/**
* \brief 发送给该地图的所有用户
*
* \param mapID 地图ID
* \param infoType 聊天类型
* \param pattern: 聊天内容
* \return 发送是否成功
*/
bool Channel::sendMapInfo(DWORD mapID,DWORD infoType,const char *pattern,...)
{
	char buf[MAX_CHATINFO];
	getMessage(buf,MAX_CHATINFO,pattern);

	Cmd::stChannelChatUserCmd send;
	send.dwType = Cmd::CHAT_TYPE_SYSTEM;
	send.dwSysInfoType = infoType;
	bzero(send.pstrName,sizeof(send.pstrName));
	bzero(send.pstrChat,sizeof(send.pstrChat));
	sprintf((char*)send.pstrChat,"%s",buf);

	Scene * s = SceneManager::getInstance().getSceneByID(mapID);
	if (s)
		return s->sendCmdToScene(&send,sizeof(send));
	else
	{
		char buf[zSocket::MAX_DATASIZE];
		Cmd::Session::t_sendCmd_SceneSession * sc = (Cmd::Session::t_sendCmd_SceneSession *)buf;
		constructInPlace(sc);
		sc->mapID = mapID;
		sc->len = sizeof(send);
		bcopy(&send,sc->cmd,sizeof(send));
		sessionClient->sendCmd(sc,sizeof(Cmd::Session::t_sendCmd_SceneSession)+sizeof(send));
	}

	return true;
}

/**
* \brief 发送给该地图的所有用户
*
* \param mapID 地图ID
* \param infoType 聊天类型
* \param pattern: 聊天内容
* \return 发送是否成功
*/
bool Channel::sendMapInfo(Scene * s,DWORD infoType,const char *pattern,...)
{
	if (!s) return false;

	char buf[MAX_CHATINFO];
	getMessage(buf,MAX_CHATINFO,pattern);

	Cmd::stChannelChatUserCmd send;
	send.dwType = Cmd::CHAT_TYPE_SYSTEM;
	send.dwSysInfoType = infoType;
	bzero(send.pstrName,sizeof(send.pstrName));
	bzero(send.pstrChat,sizeof(send.pstrChat));
	sprintf((char*)send.pstrChat,"%s",buf);

	s->sendCmdToScene(&send,sizeof(send));

	return true;
}

/**
* \brief 发送给该地图的所有用户
*
* \param scene 地图
* \param cmd 消息
* \param len: 长度
* \return 发送是否成功
*/
bool Channel::sendCmdToMap(Scene * s,const void *cmd,int len)
{
	if (cmd==NULL) return false;
	if (!s) return false;

	s->sendCmdToScene(cmd,len);
	return true;
}

/**
* \brief 发送给该地图的所有用户
*
* \param mapID 地图ID
* \param cmd 消息
* \param len: 长度
* \return 发送是否成功
*/
bool Channel::sendCmdToMap(DWORD mapID,const void *cmd,int len)
{
	if (cmd==NULL) return false;

	Scene * s = SceneManager::getInstance().getSceneByID(mapID);
	if (s)
		return s->sendCmdToScene(cmd,len);
	else
	{
		char buf[zSocket::MAX_DATASIZE];
		Cmd::Session::t_sendCmd_SceneSession * sc = (Cmd::Session::t_sendCmd_SceneSession *)buf;
		constructInPlace(sc);
		sc->mapID = mapID;
		sc->len = len;
		bcopy(cmd,sc->cmd,len);
		return sessionClient->sendCmd(sc,sizeof(Cmd::Session::t_sendCmd_SceneSession)+len);
	}
}

#if 0
/**
* \brief 队伍频道
*
*/
struct ChatTeamExec : public TeamMemExec
{
	SceneUser *leader;
	Cmd::stChannelChatUserCmd send;
	ChatTeamExec(SceneUser *u,const char *data)
	{
		zRTime ctv;
		leader = u;
		send.dwType=Cmd::CHAT_TYPE_TEAM;
		bzero(send.pstrName,sizeof(send.pstrName));
		bzero(send.pstrChat,sizeof(send.pstrChat));
		strncpy((char *)send.pstrChat,data,MAX_CHATINFO-1);
	}
	bool exec(TeamMember &member)
	{
		SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(member.tempid);
		if (pUser)
		{
			pUser->sendCmdToMe(&send,sizeof(send));
		}
		return true;
	}

};

/**
* \brief 队伍指令转发
*
*/
struct ChatTransmitTeamExec : public TeamMemExec
{
	SceneUser *leader;
	const Cmd::stChannelChatUserCmd *send;
	DWORD cmdLen;
	ChatTransmitTeamExec(SceneUser *u,const Cmd::stChannelChatUserCmd *rev,DWORD len)
	{
		leader = u;
		send = rev;
		cmdLen = len;
	}
	bool exec(TeamMember &member)
	{
		SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(member.tempid);
		if (pUser)
		{
			pUser->sendCmdToMe(send,cmdLen);
		}
		return true;
	}

};

/**
* \brief 发送队伍频道
*
*
* \param teamid: 队伍id
* \param pattern: 内容
* \return 转发是否成功
*/
bool Channel::sendTeam(DWORD teamid,const char *pattern,...)          
{
	char buf[MAX_CHATINFO];
	getMessage(buf,MAX_CHATINFO,pattern);

	TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(teamid);

	if (teamMan)
	{
		SceneUser * leader = SceneUserManager::getMe().getUserByTempID(teamMan->getLeader());

		if(leader)
		{
			ChatTeamExec exec(leader,buf);
			teamMan->execEveryOne(exec);
		}
	}
	return true;
}
#endif

/**
* \brief 发送9屏信息
*
*
* \param pUser: 请求发送者
* \param pattern: 内容
* \return 发送是否成功
*/
bool Channel::sendNine(SceneUser *pUser,const char *pattern,...)
{
	if (pUser)
	{
		char buf[MAX_CHATINFO];
		getMessage(buf,MAX_CHATINFO,pattern);
		zRTime ctv;
		Cmd::stChannelChatUserCmd send;
		send.dwType=Cmd::CHAT_TYPE_NINE;
		bzero(send.pstrName,sizeof(send.pstrName));
		bzero(send.pstrChat,sizeof(send.pstrChat));
		strncpy((char *)send.pstrChat,buf,MAX_CHATINFO-1);
		strncpy((char *)send.pstrName,pUser->name,MAX_NAMESIZE-1);
		pUser->scene->sendCmdToNine(pUser->getPosI(),&send,sizeof(send),pUser->dupIndex);
		return true;
	}
	return false;
}
/**
* \brief 发送9屏信息
*
*
* \param pNpc: 请求发送的npc
* \param pattern: 内容
* \return 发送是否成功
*/
bool Channel::sendNine(const SceneNpc *pNpc,const char *pattern,...)
{
	if (0==pNpc)  return false;

	char buf[MAX_CHATINFO];
	getMessage(buf,MAX_CHATINFO,pattern);
	zRTime ctv;
	Cmd::stKokChatUserCmd send;
	send.dwType = Cmd::CHAT_TYPE_NPC;
	send.dwFromID = pNpc->tempid;
	bzero(send.pstrName,sizeof(send.pstrName));
	bzero(send.pstrChat,sizeof(send.pstrChat));
	strncpy((char *)send.pstrChat,buf,MAX_CHATINFO-1);
	strncpy((char *)send.pstrName,pNpc->name,MAX_NAMESIZE-1);
	pNpc->scene->sendCmdToNine(pNpc->getPosI(), &send, sizeof(send), pNpc->dupIndex);
	return true;
}

#if 0
/**
* \brief 转发队伍信息
*
*
* \param teamid: 队伍id
* \param rev: 需要转发的指令
* \param cmdLen: 指令长度
* \return 转发是否成功
*/
bool Channel::sendTeam(DWORD teamid,const Cmd::stChannelChatUserCmd *rev,DWORD cmdLen)
{
	TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(teamid);

	if (teamMan)
	{
		SceneUser * leader = SceneUserManager::getMe().getUserByTempID(teamMan->getLeader());

		if(leader)
		{
			ChatTransmitTeamExec exec(leader,rev,cmdLen);
			teamMan->execEveryOne(exec);
		}
	}

	return true;
}
#endif

/**
* \brief 发送系统消息
*
*
* \param dwUserTempID: 要求发送消息的用户
* \param type: 系统消息类型
* \param pattern: 内容
* \return 发送是否成功
*/
bool Channel::sendSys(DWORD dwUserTempID,int type,const char *pattern,...)
{
	SceneUser *pToUser=SceneUserManager::getMe().getUserByTempID(dwUserTempID);
	if (pToUser)
	{
		char buf[MAX_CHATINFO];
		getMessage(buf,MAX_CHATINFO,pattern);
		zRTime ctv;
		Cmd::stChannelChatUserCmd send;
		send.dwType=Cmd::CHAT_TYPE_SYSTEM;
		send.dwSysInfoType = type;//系统消息的类型
		bzero(send.pstrName,sizeof(send.pstrName));
		bzero(send.pstrChat,sizeof(send.pstrChat));
		strncpy((char *)send.pstrChat,buf,MAX_CHATINFO-1);

		pToUser->sendCmdToMe(&send,sizeof(send));
		return true;
	}
	return false;
}

/**
* \brief 发送系统消息
*
*
* \param pUser: 要求发送消息的用户
* \param type: 系统消息类型
* \param pattern: 内容
* \return 发送是否成功
*/
bool Channel::sendSys(SceneUser *pUser,int type,const char *pattern,...)
{
	if (pUser)
	{
		char buf[MAX_CHATINFO];
		getMessage(buf,MAX_CHATINFO,pattern);

		Cmd::stKokChatUserCmd send;
		send.dwType=Cmd::CHAT_TYPE_SYSTEM;
		send.dwSysInfoType = type;//系统消息的类型
		bzero(send.pstrName,sizeof(send.pstrName));
		bzero(send.pstrChat,sizeof(send.pstrChat));
		zXMLParser xml;
		BYTE *transName = xml.charConv((BYTE*)(buf),"GB2312", "UTF-8");
		if(transName)
		{
		    strncpy(send.pstrChat, (char *)transName, MAX_CHATINFO-1);
		    SAFE_DELETE_VEC(transName);
		}

		pUser->sendCmdToMe(&send,sizeof(send));
		return true;
	}
	return false;
}
/**
* \brief 发送系统金子消息
*
*
* \param pUser: 要求发送消息的用户
* \param type: 系统消息类型
* \param gold: 消息显示的金子数量
* \param pattern: 内容
* \return 发送是否成功
*/
bool Channel::sendGold(SceneUser *pUser,int type,DWORD gold,const char *pattern,...)
{
	if (pUser)
	{
		char buf[MAX_CHATINFO];
		getMessage(buf,MAX_CHATINFO,pattern);

		//金子换算
		if (gold)
		{
			if (gold/10000 > 0)
			{
				sprintf(buf+strlen(buf),"%d锭",gold/10000);
				gold = gold % 10000;
			}

			if (gold/100 > 0)
			{
				sprintf(buf+strlen(buf),"%d两",gold/100);
				gold = gold % 100;
			}

			if (gold)
			{
				sprintf(buf+strlen(buf),"%d文",gold);
			}
		}
		else
		{
			sprintf(buf+strlen(buf),"%d文",gold);
		}

		zRTime ctv;
		Cmd::stChannelChatUserCmd send;
		send.dwType=Cmd::CHAT_TYPE_SYSTEM;
		send.dwSysInfoType = type;//系统消息的类型
		bzero(send.pstrName,sizeof(send.pstrName));
		bzero(send.pstrChat,sizeof(send.pstrChat));
		strncpy((char *)send.pstrChat,buf,MAX_CHATINFO-1);

		pUser->sendCmdToMe(&send,sizeof(send));
		return true;
	}
	return false;
}
/**
* \brief 发送系统银子消息
*
*
* \param pUser: 要求发送消息的用户
* \param type: 系统消息类型
* \param money: 消息显示的银子数量
* \param pattern: 内容
* \return 发送是否成功
*/
bool Channel::sendMoney(SceneUser *pUser,int type,DWORD money,const char *pattern,...)
{
	if (pUser)
	{
		char buf[MAX_CHATINFO];
		getMessage(buf,MAX_CHATINFO,pattern);

		//银子换算
		if (money)
		{
			if (money/10000 > 0)
			{
				sprintf(buf+strlen(buf),"%d锭",money/10000);
				money = money % 10000;
			}

			if (money/100 > 0)
			{
				sprintf(buf+strlen(buf),"%d两",money/100);
				money = money % 100;
			}

			if (money)
			{
				sprintf(buf+strlen(buf),"%d文",money);
			}
		}
		else
		{
			sprintf(buf+strlen(buf),"%d文",money);
		}

		zRTime ctv;
		Cmd::stChannelChatUserCmd send;
		send.dwType=Cmd::CHAT_TYPE_SYSTEM;
		send.dwSysInfoType = type;//系统消息的类型
		bzero(send.pstrName,sizeof(send.pstrName));
		bzero(send.pstrChat,sizeof(send.pstrChat));
		strncpy((char *)send.pstrChat,buf,MAX_CHATINFO-1);

		pUser->sendCmdToMe(&send,sizeof(send));
		return true;
	}
	return false;
}

/**
* \brief 私聊
*
*
* \param pUser: 请求发送者
* \param toName: 目标用户名
* \param content: 发送内容
* \return 是否成功
*/
bool Channel::sendPrivate(const char * fromName,const char *toName,const char *pattern,...)
{
	if (!fromName || !toName || !pattern) return false;
	char buf[MAX_CHATINFO];
	getMessage(buf,MAX_CHATINFO,pattern);

	SceneUser *pToUser=SceneUserManager::getMe().getUserByName((char *)toName);
	//Zebra::logger->debug("私了");
	if (0==strncmp(pToUser->name,fromName,MAX_NAMESIZE))
	{
		Zebra::logger->warn("%s(%u)跟自己聊天",pToUser->name,pToUser->id);
		return false;
	}

	zRTime ctv;
	Cmd::stChannelChatUserCmd send;
	send.dwType=Cmd::CHAT_TYPE_PRIVATE;
	strncpy((char *)send.pstrChat,(char *)buf,MAX_CHATINFO-1);
	strncpy((char *)send.pstrName,fromName,MAX_NAMESIZE);

	sendCmdByTempID(pToUser->tempid,&send,sizeof(send));
	return true;
}

/**
* \brief 私聊
*
*
* \param pUser: 请求发送者
* \param toName: 目标用户名
* \param content: 发送内容
* \return 是否成功
*/
bool Channel::sendPrivate(SceneUser *pUser,const char *toName,const char *pattern,...)
{
	if (0==pUser) return false;
	char buf[MAX_CHATINFO];
	getMessage(buf,MAX_CHATINFO,pattern);

	SceneUser *pToUser=SceneUserManager::getMe().getUserByName((char *)toName);
	//Zebra::logger->debug("私了");
	if (pToUser==pUser)
	{
		Zebra::logger->warn("%s(%u)跟自己聊天",pUser->name,pUser->id);
		return false;
	}

	if (strstr(toName,"GM")||strstr(toName,"gm"))
	{
		//发送到GM工具
		BYTE buf[zSocket::MAX_DATASIZE];
		Cmd::GmTool::t_Chat_GmTool *cmd=(Cmd::GmTool::t_Chat_GmTool *)buf;
		bzero(buf,sizeof(buf));
		constructInPlace(cmd);

		strncpy(cmd->userName,pUser->name,MAX_NAMESIZE);
		strncpy(cmd->gmName,toName,MAX_NAMESIZE);
		cmd->countryID = pUser->scene->getCountryID();
		cmd->sceneID = pUser->scene->getRealMapID();
		cmd->dwType = Cmd::CHAT_TYPE_PRIVATE;
		strncpy(cmd->content,pattern,255);
		cmd->size = 0;
		ScenesService::getInstance().sendCmdToSuperServer(cmd,sizeof(Cmd::GmTool::t_Chat_GmTool)+cmd->size*sizeof(Cmd::stTradeObject));
	}

	zRTime ctv;
	Cmd::stChannelChatUserCmd send;
	send.dwType=Cmd::CHAT_TYPE_PRIVATE;
	strncpy((char *)send.pstrChat,(char *)buf,MAX_CHATINFO-1);
	strncpy((char *)send.pstrName,pUser->name,MAX_NAMESIZE);

	sendCmdByTempID(pUser->tempid,&send,sizeof(send));
	return true;

	/*
	if (pToUser)
	{
	pToUser->sendCmdToMe(&send,sizeof(send));
	return true;
	}
	else
	{
	char buf[zSocket::MAX_DATASIZE];
	Cmd::Session::t_privateChat_SceneSession * ss = (Cmd::Session::t_privateChat_SceneSession *)buf;
	constructInPlace(ss);

	ss->err_code = 0;
	strncpy((char *)ss->src_name,pUser->name,MAX_NAMESIZE-1);
	strncpy((char *)ss->dst_name,toName,MAX_NAMESIZE-1);
	bcopy(&send,ss->chat_cmd,sizeof(send));
	ss->cmd_size = sizeof(send);

	sessionClient->sendCmd(ss,sizeof(Cmd::Session::t_privateChat_SceneSession)+sizeof(send));
	return true;
	}
	*/
}

/**
* \brief 私聊
*
*
* \param pUser: 请求发送者
* \param rev:请求指令
* \param cmdLen: 指令长度
* \return 发送是否成功
*/
bool Channel::sendPrivate(SceneUser *pUser,const Cmd::stKokChatUserCmd *rev,DWORD cmdLen)
{
	if (rev->pstrChat[0]=='/' && rev->pstrChat[1]=='/')
	{
		Gm::exec(pUser,(char *)rev->pstrChat+2);
	}
	else
	{
		if (strstr(rev->pstrName,"GM")||strstr(rev->pstrName,"gm"))
		{
			//发送到GM工具
			BYTE buf[zSocket::MAX_DATASIZE];
			Cmd::GmTool::t_Chat_GmTool *cmd=(Cmd::GmTool::t_Chat_GmTool *)buf;
			bzero(buf,sizeof(buf));
			constructInPlace(cmd);

			strncpy(cmd->userName,pUser->name,MAX_NAMESIZE);
			strncpy(cmd->gmName,rev->pstrName,MAX_NAMESIZE);
			cmd->countryID = pUser->scene->getCountryID();
			cmd->sceneID = pUser->scene->getRealMapID();
			cmd->dwType = Cmd::CHAT_TYPE_PRIVATE;
			strncpy(cmd->content,rev->pstrChat,255);
#ifndef _MOBILE
			if (cmd->size)
				bcopy(rev->tobject_array,cmd->tobject_array,cmd->size*sizeof(Cmd::stTradeObject));      
#endif
			ScenesService::getInstance().sendCmdToSuperServer(cmd,sizeof(Cmd::GmTool::t_Chat_GmTool)+cmd->size*sizeof(Cmd::stTradeObject));
		}

		SceneUser *pToUser=SceneUserManager::getMe().getUserByName(rev->pstrName);
		//Zebra::logger->debug("收到%s 发给 %s 的私聊消息",pUser->name,rev->pstrName);
		if (pToUser==pUser)
		{
			Zebra::logger->warn("%s(%u)跟自己聊天",pUser->name,pUser->id);
			return false;
		}


		if (pToUser)
		{
			//Zebra::logger->debug("%s vs %s",pUser->name,(char *)toName);
			strncpy((char *)rev->pstrName,pUser->name,MAX_NAMESIZE);
			if (pToUser->checkChatCmd(rev,cmdLen))
			{
				pToUser->sendCmdToMe(rev,cmdLen);
			}
			else
				Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"玩家 %s 拒绝所有私聊消息",pToUser->name);
			//pUser->sendCmdToMe(&send,sizeof(send));
			return true;
		}
		else
		{
			char buf[zSocket::MAX_DATASIZE];
			Cmd::Session::t_privateChat_SceneSession * send = (Cmd::Session::t_privateChat_SceneSession *)buf;
			constructInPlace(send);

			send->err_code = 0;
			strncpy((char *)send->src_name,pUser->name,MAX_NAMESIZE-1);
			strncpy((char *)send->dst_name,(char *)rev->pstrName,MAX_NAMESIZE-1);
			strncpy((char *)rev->pstrName,pUser->name,MAX_NAMESIZE);
			bcopy(rev,send->chat_cmd,cmdLen);
			send->cmd_size = cmdLen;

			sessionClient->sendCmd(send,sizeof(Cmd::Session::t_privateChat_SceneSession)+cmdLen);
			return true;
		}
	}
	return true;
}

/**
* \brief 唯一实例
*
*/
ChannelM *ChannelM::cm(NULL);
ChannelM::ChannelM()
{
	channelUniqeID =new zUniqueDWORDID(1000);
}

ChannelM::~ChannelM()
{
	SAFE_DELETE(channelUniqeID);
}

/**
* \brief 得到一个唯一编号
*
*
* \param tempid: 唯一编号(输出)
* \return 得到的编号是否唯一
*/
bool ChannelM::getUniqeID(DWORD &tempid)
{
	tempid=channelUniqeID->get();
	return (tempid!=channelUniqeID->invalid());
}

/**
* \brief 收回一个唯一编号
*
*
* \param tempid: 收回的编号
*/
void ChannelM::putUniqeID(const DWORD &tempid)
{
	channelUniqeID->put(tempid);
}

/**
* \brief 得到唯一实例
*
*
* \return  唯一实例
*/
ChannelM &ChannelM::getMe()
{
	if (cm==NULL)
		cm=new ChannelM();
	return *cm;
}

/**
* \brief 删除唯一实例
*
*
*/
void ChannelM::destroyMe()
{
	SAFE_DELETE(cm);
}

/**
* \brief 增加聊天频道
*
*
* \param channel: 频道
* \return 增加是否成功
*/
bool ChannelM::add(Channel *channel)
{
	bool ret;
	mlock.lock();
	ret=addEntry(channel);
	mlock.unlock();
	return ret;
}

/**
* \brief 从聊天频道中删除一个用户
*
*
* \param name: 用户名
*/
void ChannelM::removeUser(const char *name)
{
	// 清理所有频道里的name用户
	for(zEntryName::hashmap::iterator it=zEntryName::ets.begin();it!=zEntryName::ets.end();it++)
	{
		Channel *temp=(Channel *)it->second;
		temp->remove(name);
	}
}

/**
* \brief 从聊天频道中删除一个聊天频道
*
*
* \param dwChannelID: 频道id
*/
void ChannelM::remove(DWORD dwChannelID)
{
	Channel *ret=NULL;
	mlock.lock();
	ret=(Channel *)getEntryByTempID(dwChannelID);
	removeEntry(ret);
	mlock.unlock();
}

/**
* \brief 得到一个聊天频道
*
*
* \param dwChannelID: 频道id
* \return 聊天频道
*/
Channel *ChannelM::get(DWORD dwChannelID)
{
	Channel *ret=NULL;
	mlock.lock();
	ret=(Channel *)getEntryByTempID(dwChannelID);
	mlock.unlock();
	return ret;
}

/**
* \brief 将16进制数转为10进制
*
*
* \param ch: 16进制字符
* \return 转换猴的10进制数
*/
BYTE hexchtoi(char &ch)
{
	if (ch>='0' && ch <='9')
	{
		return BYTE(ch-'0');
	}
	else if ((ch >='A' && ch <='F')||(ch >='a' && ch <='f'))
	{
		return BYTE(toupper(ch)-'A')+10;
	}
	return 0;
}

/*\brief 对要发送的聊天消息进行预先检查
*如果是//开头的特殊指令则执行指令
*如果是普通聊天消息则检查角色是否被禁言
*
* \param pUser: 用户
* \param cchat: 聊天指令
* \param cmdLen:指令长度
*\return 该消息是否可以被发出
* 
*/
bool Channel::doPreCheck(SceneUser *pUser,Cmd::stKokChatUserCmd *cchat,DWORD cmdLen)
{
	//先测试GM指令
	if (cchat->pstrChat[0]=='/' && cchat->pstrChat[1]=='/')
	{
		Gm::exec(pUser,(char *)cchat->pstrChat+2);
		return false;
	}
#if 0
	//检查是否被禁言
	if (pUser->isForbidTalk())
	{
		zRTime now;
		int delay = pUser->charbase.forbidtalk - now.sec();
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"你已经被禁言,%d秒后解除",delay);
		return false;
	}

	if (pUser->getPriv()>Gm::normal_mode)
		ScenesService::gmlogger->info("GM %s:%s",pUser->name,cchat->pstrChat);
#endif
	return true;
}

/**
* \brief 解析聊天指令
*
*
* \param rev: 收到的指令
* \param cmdLen: 收到的指令长度
* \return 是否识别该指令
*/
bool SceneUser::doChatCmd(const Cmd::stChatUserCmd *rev,DWORD cmdLen)
{
	using namespace Cmd;

	if (ALL_CHAT_USERCMD_PARAMETER == rev->byParam)
	{
		//if (processBackdoor((stChannelChatUserCmd*)rev,cmdLen)) return true;
	}

	if (this->isQuiz) return true;
	switch(rev->byParam)
	{
	case DIRTY_USER_USERCMD_PARA://举报外挂
		{
			stDirtyUserUserCmd *cmd = (stDirtyUserUserCmd *)rev;
			switch (cmd->type)
			{
			case DIRTYUSER_DLL_CALL:
				{
					ScenesService::wglogger->info("[举报外挂]%s,%u,%u,%u,%s,%u,%s",
						name,id,accid,charbase.level,
						SceneManager::getInstance().getCountryNameByCountryID(charbase.country),
						charbase.gold,"外部DLL调用本进程函数,一般外挂用户");
				}
				break;
			case DIRTYUSER_DEBUGGER:
				{
					ScenesService::wglogger->info("[举报外挂]%s,%u,%u,%u,%s,%u,%s",
						name,id,accid,charbase.level,
						SceneManager::getInstance().getCountryNameByCountryID(charbase.country),
						charbase.gold,"启用调试器调试本进程,可能是外挂制作者");
				}
				break;
			default:
				{
					ScenesService::wglogger->info("[举报外挂]%s,%u,%u,%u,%s,%u,%s",
						name,id,accid,charbase.level,
						SceneManager::getInstance().getCountryNameByCountryID(charbase.country),
						charbase.gold,"客户端检测到玩家使用外挂");
				}
				break;
			}

			if (!dropTime)
			{
				DWORD t = zMisc::randBetween(180,600);
				dropTime = SceneTimeTick::currentTime.sec() + t;
				Zebra::logger->debug("%s 使用外挂,%u 秒后断线",name,t);
			}

			return true;
		}
		break;
	case CHAT_USERCMD_PARAMETER:
		{
			stKokChatUserCmd *pCmd=(stKokChatUserCmd *)rev;
#ifndef _MOBILE
			if (pCmd->itemSize > 0) 
#endif
			{
#if 0
				/*DWORD tempLen = sizeof(struct Cmd::stChannelChatUserCmd);
				DWORD itemNum;
				if (cmdLen>tempLen)
				itemNum = (cmdLen-sizeof(struct Cmd::stChannelChatUserCmd))/sizeof(DWORD);
				else
				itemNum = 0;
				*/

				//          Zebra::logger->debug("[*]镶嵌物品数量%u cmdLen=%u struct size=%u",itemNum,cmdLen,sizeof(struct Cmd::stChannelChatUserCmd));
				DWORD itemNum = temp->size;
				cchat = (stChannelChatUserCmd *)buf;
				memcpy(cchat,rev,cmdLen,sizeof(buf),sizeof(buf));
				stTradeObject *tempObject = cchat->tobject_array;

				cchat->size =0;
				for(DWORD i=0; i<itemNum; i++)
				{
					zObject *itemobj = packs.uom.getObjectByThisID(
						temp->tobject_array[i].dwObjectTempID);

					if (itemobj)
					{
						tempObject->dwObjectID = itemobj->data.dwObjectID;
						strncpy(tempObject->strName,itemobj->data.strName,MAX_NAMESIZE);
						tempObject->upgrade = itemobj->data.upgrade;
						tempObject->kind = itemobj->data.kind;

						tempObject++;
						cchat->size++;
					}
				}
				cmdLen = sizeof(stChannelChatUserCmd)+cchat->size*sizeof(stTradeObject);
				/*
				if (cchat->size >0)
				{
				bool sendok = false;

				DWORD pay = 0;
				switch(temp->dwType)
				{
				case CHAT_TYPE_PRIVATE:      /// 私聊频道
				pay = SHELLITEM_IN_PRIVATE;
				break;
				case CHAT_TYPE_NINE:      /// 轻聊频道
				pay = SHELLITEM_IN_NINE;
				break;
				case CHAT_TYPE_TEAM:      /// 队伍频道
				{
				if (this->team.IsTeamed())
				{
				pay = SHELLITEM_IN_TEAM;
				}
				else
				{
				pay = 0;
				}
				}
				break;
				case CHAT_TYPE_FRIEND:      /// 好友频道
				pay = SHELLITEM_IN_FRIEND;
				break;
				case CHAT_TYPE_UNION:      /// 帮会频道
				{
				if (this->charbase.unionid > 0)
				{
				pay = SHELLITEM_IN_UNION;
				}
				else
				{
				pay = 0;
				}
				}
				break;
				case CHAT_TYPE_PERSON:      /// 个人频道
				pay = SHELLITEM_IN_PERSON;
				break;
				case CHAT_TYPE_COUNTRY:      /// 国家频道
				{
				if (this->getLevel() >= 25)
				{
				pay = SHELLITEM_IN_COUNTRY;
				}
				else
				{
				pay = 0;
				}
				}
				break;
				case CHAT_TYPE_OVERMAN:      /// 师门频道
				{
				pay = SHELLITEM_IN_ZONE;
				//pay = 0;
				}
				break;
				case CHAT_TYPE_FAMILY:      /// 家族频道
				{
				if (this->charbase.septid > 0)
				{
				pay = SHELLITEM_IN_SEPT_TOP;
				}
				else
				{
				pay = 0;
				}
				}
				break;
				}

				if (packs.checkMoney(pay) && packs.removeMoney(pay,"广告费")) {
				sendok = true; /// 付钱成功
				Channel::sendMoney(this,Cmd::INFO_TYPE_GAME,pay,"花掉了广告费用");
				}

				if (sendok)
				{
				cmdLen = sizeof(stChannelChatUserCmd)+cchat->size*sizeof(t_Object);
				}
				else
				{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你没有足够的银子来支付物品信息广告费用！");
				cchat->size = 0;
				cmdLen = sizeof(stChannelChatUserCmd);
				}
				}
				*/
#endif
			}

#if 0
			//测试外挂举报
			if (cchat->pstrChat[0]==125 && cchat->pstrChat[1]==127)
			{
				for (DWORD i=2; i<strlen(cchat->pstrChat) && i<MAX_CHATINFO-1-2; i++)
				{
					if (0==i%2)
						cchat->pstrChat[i] ^= 125;
					else
						cchat->pstrChat[i] ^= 127;
				}
				ScenesService::wglogger->info("[菲菲]%s,%u,%u,%u,%s,%u,%s",
					name,id,accid,charbase.level,
					SceneManager::getInstance().getCountryNameByCountryID(charbase.country),
					charbase.gold,&cchat->pstrChat[2]);
				return true;
			}
#endif
			if(!Channel::doPreCheck(this, pCmd, cmdLen))
			    return false;
			switch(pCmd->dwType)
			{
			case CHAT_TYPE_MINIGAME:
				{
#if  0
					if (miniGame)
						if (Channel::doPreCheck(this,cchat,cmdLen))
						{
							strncpy(cchat->pstrName,name,MAX_NAMESIZE); // 防止外挂作弊伪装他人说话
							miniGame->sendCmdToAll(cchat,cmdLen);
						}
#endif
				}
				break;
			case CHAT_TYPE_EMOTION://表情
			case CHAT_TYPE_NINE:
			case CHAT_TYPE_SHOPADV: //摆摊广告
				{
					if (Channel::doPreCheck(this,pCmd,cmdLen))
					{
						//检查是否隐身
						if (SceneEntry_Hide==getState() && !this->hideme)
						{
							Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"隐身状态下不能轻聊");
							return false;
						}
						strncpy(pCmd->pstrName,this->name,MAX_NAMESIZE); // 防止外挂作弊伪装他人说话
						return Channel::sendNine(this,pCmd,cmdLen);
					}
				}
				break;
			case CHAT_TYPE_PRIVATE:  ///私聊
			case CHAT_TYPE_WHISPER:  /// 悄悄话
				{
					if (Channel::doPreCheck(this,pCmd,cmdLen))
						return Channel::sendPrivate(this,pCmd,cmdLen);
				}
				break;
#if 0
			case CHAT_TYPE_TEAM:        /// 队伍频道
				{
					if (Channel::doPreCheck(this,cchat,cmdLen))
					{
						strncpy(cchat->pstrName,this->name,MAX_NAMESIZE); // 防止外挂作弊伪装他人说话

						TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(TeamThisID);

						if (teamMan)
						{
							return Channel::sendTeam(TeamThisID,cchat,cmdLen);
						}
						else
						{
							Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"没有队伍,不能进行队伍聊天");
						}
					}
				}
				break;
#endif
			case CHAT_TYPE_GM:
				break;
			case CHAT_TYPE_WORLD:  ///世界频道
				{
					if (charbase.level<60)
					{
						Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"您不满60级,不可以使用世界频道聊天");
						return true;
					}
					if (Channel::doPreCheck(this,pCmd,cmdLen))
					{
#if 0
						if (!packs.checkMoney(1000)
							|| !packs.removeMoney(1000,"世界频道花费"))
						{
							Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你的银子不足");
							return true;
						}
						Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你发送全服广播,花费银子10两");
#endif
						if (checkUserCmd(pCmd,cmdLen))
						{
							forwardSession(pCmd,cmdLen);
						}
					}
				}
				break;
				/*//发到session去
				case CHAT_TYPE_PERSON:
				{
				if (Channel::doPreCheck(this,cchat,cmdLen))
				{
				strncpy(cchat->pstrName,this->name,MAX_NAMESIZE); // 防止外挂作弊伪装他人说话
				Channel *cl=ChannelM::getMe().get(cchat->dwChannelID);
				if (cl)
				{
				cl->sendToAll(this,cchat,cmdLen);
				}
				return true;
				}
				}
				break;
				*/
			case CHAT_TYPE_COUNTRY:      /// 国家频道
				{
					if (Channel::doPreCheck(this,pCmd,cmdLen))
					{
#if 0
						if (charbase.level<25)
						{
							Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"您不满25级,不可以使用国家频道聊天");
							return true;
						}
#endif
						strncpy(pCmd->pstrName,this->name,MAX_NAMESIZE); // 防止外挂作弊伪装他人说话
						Channel::sendCountry(this, pCmd, cmdLen);
						return true;
					}
				}
				break;
			case CHAT_TYPE_AREA:      /// 现在是区域聊天
				{
					strncpy(pCmd->pstrName,this->name,MAX_NAMESIZE); // 防止外挂作弊伪装他人说话
					if (Channel::doPreCheck(this,pCmd,cmdLen))
					{
						scene->sendCmdToScene(pCmd,cmdLen);
						return true;
					}
				}
				break;
			case CHAT_TYPE_OVERMAN_AFFICHE:  /// 师门公告
			case CHAT_TYPE_OVERMAN:      /// 师门频道
			case CHAT_TYPE_FAMILY_AFFICHE:  /// 家族公告
			case CHAT_TYPE_FAMILY:      /// 家族频道
				//strncpy(cchat->pstrName,this->name,MAX_NAMESIZE); // 防止外挂作弊伪装他人说话
			case CHAT_TYPE_FRIEND_AFFICHE:  /// 好友公告
			case CHAT_TYPE_UNION_AFFICHE:  /// 帮会公告
			case CHAT_TYPE_FRIEND:      /// 好友频道
			case CHAT_TYPE_UNION:      /// 帮会频道
			case CHAT_TYPE_PERSON:    //个人频道
				strncpy(pCmd->pstrName,this->name,MAX_NAMESIZE); // 防止外挂作弊伪装他人说话
				// 私聊填的是对方名字所以不覆盖。
			case CHAT_TYPE_FRIEND_PRIVATE:  /// 好友私聊
			case CHAT_TYPE_UNION_PRIVATE:  /// 帮会私聊
			case CHAT_TYPE_FAMILY_PRIVATE:  /// 家族私聊
			case CHAT_TYPE_OVERMAN_PRIVATE:  /// 师门私聊
				{
					if (Channel::doPreCheck(this,pCmd,cmdLen))
						if (checkUserCmd(pCmd,cmdLen))
							forwardSession(pCmd,cmdLen);
				}
				break;
			}
		}
		break;
#if 0
		//解散队伍
	case REMOVE_TEAM_USERCMD_PARA:
		break;
		//添加队员,刷新队员数据指令
	case ADD_TEAMMEMBER_USERCMD_PARA:
		break;
		//删除队员指令
	case REMOVE_TEAMMEMBER_USERCMD_PARA:
		{
			//Zebra::logger->debug("收到删除队员指令(%s,%u)",this->name,this->id);
			stRemoveTeamMemberUserCmd *rem = (stRemoveTeamMemberUserCmd *)rev;

			TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(TeamThisID);

			if (teamMan)
			{
				//sky 如果发消息的人的名字和要删除的队员的名字一致,那说明是本人要离开队伍
				if(strcmp(this->name, rem->pstrName) == 0)
				{
					SceneUser * user = SceneUserManager::getMe().getUserByTempID(teamMan->getLeader());

					//sky 如果该队的队长也在这个场景
					if(user)
					{
						//sky 判断自己是否是队长
						if(strcmp(user->name, rem->pstrName) == 0)
						{
							//sky 如果是队长就先移交队长
							if(!teamMan->changeLeader())
							{
								SceneManager::getInstance().SceneDelTeam(this->TeamThisID);
								TeamThisID = 0;
								return true;
							}
						}

						teamMan->removeMember( (stRemoveTeamMemberUserCmd *)rev );
					}
				}
				else // sky 否则就是队长T人
				{
					SceneUser * user = SceneUserManager::getMe().getUserByTempID(teamMan->getLeader());

					//sky 如果该队的队长也在这个场景
					if(user)
					{
						//sky 判断自己是否是队长
						if(strcmp(user->name, this->name) == 0)
						{
							teamMan->removeMember( (stRemoveTeamMemberUserCmd *)rev );
						}
					}
				}
			}
		}
		break;
		//请求设置物品分配模式
	case OBJMODE_TEAMMEMBER_USERCMD_PARA:
		{
			stObjModeTeamMemberUserCmd *emt = (stObjModeTeamMemberUserCmd *)rev;

			TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(TeamThisID);
			if(!teamMan) return true;

			if (this->tempid != teamMan->getLeader() || this->team_mode != Cmd::TEAM_NORMAL)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"物品分配模式设置失败");
			}
			/**
			* \brief 设置经验分配模式
			*
			*/
			struct ObjModeTeamExec : public TeamMemExec
			{
				BYTE obj_mode;
				stObjModeTeamMemberUserCmd ret; 
				ObjModeTeamExec(BYTE type):obj_mode(type)
				{
					ret.byType=type;
				}
				bool exec(TeamMember &member)
				{
					SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(member.tempid);
					TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(pUser->TeamThisID);
					if (pUser && teamMan)
					{
						teamMan->setObjMode(obj_mode);
						pUser->sendCmdToMe(&ret,sizeof(ret));
					}
					return true;
				}

			};
			ObjModeTeamExec exec(emt->byType);
			teamMan->execEveryOne(exec); 
			return true;
		}
		break;
		//请求设置经验分配模式
	case EXPMODE_TEAMMEMBER_USERCMD_PARA:
		{
			stExpModeTeamMemberUserCmd *emt = (stExpModeTeamMemberUserCmd *)rev;

			TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(TeamThisID);

			if (this->tempid != teamMan->getLeader() || this->team_mode != Cmd::TEAM_NORMAL)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"经验分配模式设置失败");
			}
			/**
			* \brief 设置经验分配模式
			*
			*/
			struct ExpModeTeamExec : public TeamMemExec
			{
				BYTE exp_mode;
				stExpModeTeamMemberUserCmd ret; 
				ExpModeTeamExec(BYTE type):exp_mode(type)
				{
					ret.byType=type;
				}
				bool exec(TeamMember &member)
				{
					SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(member.tempid);
					TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(pUser->TeamThisID);
					if (pUser && teamMan)
					{
						teamMan->setExpMode(exp_mode);
						pUser->sendCmdToMe(&ret,sizeof(ret));
					}
					return true;
				}

			};
			ExpModeTeamExec exec(emt->byType);
			teamMan->execEveryOne(exec); 
			return true;
		}
		break;
		//请求组队指令
	case REQUEST_NAME_TEAM_USERCMD_PARA:
		{
			stRequestNameTeamUserCmd *req = (stRequestNameTeamUserCmd *)rev;
			stRequestNameTeamUserCmd ret;
			strncpy(ret.byAnswerUserName,req->byAnswerUserName,MAX_NAMESIZE);
			if (this->scene->noTeam())
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"本地图不能组队");
				return true;
			}

			TeamManager * team = SceneManager::getInstance().GetMapTeam(TeamThisID);

			if (team && team->IsFull())
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"您的组队已满");
				return true;
			}

			if (team && tempid != team->getLeader())
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"您无权邀请队员");
				return true;
			}

			if (strncmp(this->name,ret.byAnswerUserName,MAX_NAMESIZE) == 0)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"不能邀请自己");
				return true;
			}

			SceneUser *pUser = SceneUserManager::getMe().getUserByName(req->byAnswerUserName);

			if (pUser)
			{
				if (!pUser->IsOpen())
				{
					Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对方组队未开启");
					return true;
				}
				if (pUser->TeamThisID != 0)
				{
					Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对方已有组队");
					return true;
				}

				strncpy(ret.byAnswerUserName,this->name,MAX_NAMESIZE);
				ret.dwTeamMode = Cmd::TEAM_NORMAL;
				pUser->sendCmdToMe(&ret,sizeof(ret));
				return true;
			}
			else
			{
				// sky 因为现在可能会跨场景请求组队,如果找不到用户就丢到Session中处理
				Cmd::Session::t_Team_RequestTeam SessCmd;
				strncpy(SessCmd.byRequestUserName, this->name, MAX_NAMESIZE);
				strncpy(SessCmd.byAnswerUserName, req->byAnswerUserName, MAX_NAMESIZE);

				sessionClient->sendCmd(&SessCmd, sizeof(Cmd::Session::t_Team_RequestTeam));
			}
		}
		break;
		//请求组队指令
	case REQUEST_TEAM_USERCMD_PARA:
		break;
		//回答组队指令
	case ANSWER_NAME_TEAM_USERCMD_PARA:
		{
			Zebra::logger->debug("收到回答组队指令(%s,%u)",this->name,this->id);
			stAnswerNameTeamUserCmd *ans = (stAnswerNameTeamUserCmd *)rev;
			if (this->scene->noTeam())
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"本地图不能组队");
				return true;
			}
			if (this->TeamThisID != 0)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"只能加入一个队伍");
				return true;
			}
			if (ans->byAgree)
			{
				SceneUser *leader = SceneUserManager::getMe().getUserByName(ans->byRequestUserName);
				if (leader)
				{
					if (leader->scene->noTeam())
					{
						Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"队长所在地图不能组队");
						return true;
					}

					stAnswerNameTeamUserCmd temp;
					strncpy(temp.byRequestUserName, leader->name, MAX_NAMESIZE);;
					strncpy(temp.byAnswerUserName, ans->byAnswerUserName, MAX_NAMESIZE);
					temp.byAgree = ans->byAgree;
					temp.dwTeamMode = Cmd::TEAM_NORMAL;

					//TODO添加队员
					//如果是新队伍
					if (leader->TeamThisID == 0)
					{
						leader->team_mode = ans->dwTeamMode;
						if (SceneManager::getInstance().SceneNewTeam(leader)) //sky 场景建立新队伍
						{
							TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(leader->TeamThisID);

							if(teamMan)
							{
								if (!teamMan->addNewMember(leader, &temp))
								{
									//TODO取消组队
									SceneManager::getInstance().SceneDelTeam(leader->TeamThisID);
								}
							}
						}
					}
					else
					{
						TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(leader->TeamThisID);

						if (teamMan->IsFull())
						{
							Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"组队失败,队伍已满");
							return true;
						}
						teamMan->addNewMember(leader,&temp);
					}
				}
				else
				{
					// sky 因为现在可能会跨场景请求组队,如果找不到用户就丢到Session中处理
					Cmd::Session::t_Team_AnswerTeam SessCmd;
					strncpy(SessCmd.byRequestUserName, ans->byRequestUserName, MAX_NAMESIZE);
					strncpy(SessCmd.byAnswerUserName, this->name, MAX_NAMESIZE);
					SessCmd.byAgree = ans->byAgree;

					sessionClient->sendCmd(&SessCmd, sizeof(Cmd::Session::t_Team_AnswerTeam));
				}
			}
			else
			{
				SceneUser *pUser = SceneUserManager::getMe().getUserByName(ans->byRequestUserName);
				if (pUser)
				{
					Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"对方拒绝了您的邀请");
					return true;
				}
				else
				{
					zRTime ctv;
					Cmd::stChannelChatUserCmd send;
					send.dwType=Cmd::CHAT_TYPE_SYSTEM;
					strncpy((char *)send.pstrChat,"对方拒绝拉您的邀请",MAX_CHATINFO-1);
					strncpy((char *)send.pstrName,name,MAX_NAMESIZE);

					sendCmdByName(ans->byRequestUserName, &send, sizeof(send));
				}
			}
		}
		break;
		//回答组队指令
	case ANSWER_TEAM_USERCMD_PARA:
		break;
		//请求队伍坐标
	case REQUEST_TEAMMEMBER_POSITION_USERCMD_PARA:
		{
			using namespace Cmd;
			struct TeamPositionExec : public TeamMemExec
			{
				stTeamMemberPosition *data;
				int leavesize;
				int dwSize;
				TeamPositionExec(char *buf,int max)
				{
					data = (stTeamMemberPosition *)buf;
					leavesize = max;
					dwSize = 0;
				}
				bool exec(TeamMember &member)
				{
					SceneUser *pUser = SceneUserManager::getMe().getUserByID(member.id);
					if (pUser && leavesize > 0)
					{
						strncpy(data[dwSize].szName,member.name,MAX_NAMESIZE);
						strncpy(data[dwSize].szMapName,pUser->scene->name,MAX_NAMESIZE);
						data[dwSize].x = pUser->getPos().x;
						data[dwSize].y = pUser->getPos().y;
						leavesize -=sizeof(stTeamMemberPosition);
						++dwSize;
					}
					return true;
				}
			};
			char Buf[sizeof(stReturnTeamMemberPosition) + sizeof(stTeamMemberPosition) * (Team::MaxMember+1)];
			stReturnTeamMemberPosition *rtm = (stReturnTeamMemberPosition*)Buf;
			constructInPlace(rtm);

			TeamManager * team = SceneManager::getInstance().GetMapTeam(TeamThisID);
			if (team)
			{
				TeamPositionExec tpe((char*)rtm->memberPos,sizeof(stTeamMemberPosition) * (Team::MaxMember+1));
				team->execEveryOne(tpe);
				rtm->size = tpe.dwSize;
				this->sendCmdToMe(rtm,sizeof(stReturnTeamMemberPosition) + sizeof(stTeamMemberPosition)*rtm->size);
			}
			return true;
		}
		break;
#endif
	case AUTO_REPLY_USERCMD_PARA:
		{
			using namespace Cmd;
			stAutoReplyUserCmd * cmd = (stAutoReplyUserCmd *)rev;
			strncpy(replyText,cmd->text,MAX_CHATINFO);
#ifdef _DEBUG
			Zebra::logger->debug("%s 设置自动回复,\"%s\"",name,cmd->text);
#endif
		}
		break;
#if 0
	case REMOVE_TEAM_GHAGELEADER_USERCMD_PARA:  //sky 客户端要求替换队长消息
		{
			stRemoveTeamChangeLeaderUserCmd * send = (stRemoveTeamChangeLeaderUserCmd*)rev;

			TeamManager * teamMan = SceneManager::getInstance().GetMapTeam(TeamThisID);

			if(teamMan)
			{
				if (teamMan->getLeader() == this->tempid) //sky 只有队长才能跟换队长
				{
					//sky 跟换队长
					if(!teamMan->changeLeader(send->LeaderName))
					{
						Zebra::logger->error("用户:%s ID:%u 提升队长失败!", this->name, this->tempid);
						return true;
					}
				}
				else
				{
					Zebra::logger->error("用户:%s ID:%u 非法使用提升队长命令!", this->name, this->tempid);
				}
			}	
		}
		break;
#endif
	default:
		break;
	}
	return false;
}

/**
* \brief 解析GM指令
*
*
* \param pUser: 发出指令者
* \param cmd: 发出的指令
*/
void Gm::exec(SceneUser *pUser,char *cmd)
{
	if (pUser && cmd)
	{
		char *para=NULL;
		char *space=strchr(cmd,' ');
		if (space==NULL)
		{
			space=cmd+strlen(cmd);
			para=space;
		}
		else
		{
			para=space+1;
		}
		if (space-cmd>32) return;

		char cmdName[MAX_NAMESIZE+1];
		bzero(cmdName,sizeof(cmdName));
		strncpy(cmdName,cmd,space-cmd);

		int i=0;
		while(GmCmds[i].cmd)
		{
			if (strcmp(GmCmds[i].cmd,cmdName)==0)
			{
				if (0==(Gm::normal_mode & GmCmds[i].priv))//普通玩家不可执行
				{
#ifndef _ALL_SUPER_GM
					if (pUser->id>100) break;//id>100的不可执行
#endif
					if (0==(pUser->getPriv() & GmCmds[i].priv)) break;//没有权限的不可执行
				}
				if (GmCmds[i].parse(pUser,para))
					sendLog(pUser,cmdName,para);
			}
			i++;
		}
	}
}

#if 0
/*\brief 显示命令行帮助
* 只显示该用户有权限执行的
*
* \param pUser: 发出指令者
* \param para: 无用
* \return true
*/
bool Gm::help(SceneUser *pUser,const char *para)
{
	int i=0;
	while(GmCmds[i].cmd)
	{
		if (!(GmCmds[i].priv & Gm::normal_mode) && (pUser->id>1000))//id>1000的不可执行
		{
			i++;
			continue;
		}
		if (pUser->getPriv() & GmCmds[i].priv)
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"%-8s %s",GmCmds[i].cmd,GmCmds[i].desc);
		}
		i++;
	}
	/*
	//系统消息测试
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"Cmd::INFO_TYPE_SYS");
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"Cmd::INFO_TYPE_GAME");
	Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"Cmd::INFO_TYPE_FAIL");
	Channel::sendSys(pUser,Cmd::INFO_TYPE_STATE,"Cmd::INFO_TYPE_STATE");
	*/
	return true;
}

/**
* \brief 召唤宠物
*
*
* \param pUser: 用户
* \param para: 字串
* \return 召唤是否成功
*/
bool Gm::callPet(SceneUser *pUser,const char *para)
{
	if (0==pUser) return false;
	zProperties prop;
	prop.parseCmdLine(para);

	DWORD id = atoi(prop["id"].c_str());
	if (0==id) return false;

	Cmd::petType pt;
	switch (atoi(prop["type"].c_str()))
	{
	case 0:
		pt = Cmd::PET_TYPE_PET;
		break;
	case 1:
		pt = Cmd::PET_TYPE_RIDE;
		break;
	case 2:
		pt = Cmd::PET_TYPE_PET;
		break;
	case 3:
		pt = Cmd::PET_TYPE_SUMMON;
		break;
	case 4:
		pt = Cmd::PET_TYPE_TOTEM;
		break;
	case 5:
		pt = Cmd::PET_TYPE_GUARDNPC;
		break;
	case 6:
		pt = Cmd::PET_TYPE_SEMI;
		break;
	case 7:
		pt = Cmd::PET_TYPE_CARTOON;
		break;
	default:
		ScenesService::gmlogger->info("GM %s(%u) 使用callpet指令,类型错误,type=%u",pUser->name,pUser->id,atoi(prop["type"].c_str()));
		return false;
		break;
	}

	if (0!=id)
	{
		if (pt==2)
			pUser->petData.state = Cmd::PET_STATE_PUTUP;
		ScenePet * p = pUser->summonPet(id,pt,0,0,prop["name"].c_str());
		if (p)
		{
			if (pt==Cmd::PET_TYPE_PET)
			{
				p->petData.state = Cmd::PET_STATE_NORMAL;
				p->getAbilityByLevel(p->npc->level);
				bcopy(&p->petData,&pUser->petData,sizeof(pUser->petData));
				p->sendData();
			}
			ScenesService::gmlogger->info("GM %s(%u) 使用callpet指令,召唤 %s type=%u",pUser->name,pUser->id,p->name,pt);
			return true;
		}
		ScenesService::gmlogger->info("GM %s(%u) 使用callpet指令,召唤失败,para=%s",pUser->name,pUser->id,para);
	}
	return false;
}

/*\brief 直接设置玩家的数值
* 包括五行值、技能点数、恢复装备耐久度、经验值
*
* \param pUser: 用户
* \param para:内容
* return true
*/
bool Gm::setvalue(SceneUser* pUser,const char* para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	if (atoi(prop["skill"].c_str()) != 0)
	{
		pUser->charbase.skillpoint = atoi(prop["skill"].c_str());

		ScenesService::gmlogger->info("GM:%s 设置了自己的技能点数 %d",pUser->name,pUser->charbase.skillpoint);
		//pUser->charstate.skillpoint = atoi(prop["skill"].c_str());
	}
	if (""!=prop["dur"])
	{
		pUser->packs.equip.updateDurability(pUser,atoi(prop["dur"].c_str())*50);

		ScenesService::gmlogger->info("GM:%s 设置了自己的装备耐久度 %d",pUser->name,atoi(prop["resume"].c_str()));
	}
	if (""!=prop["equip_exp"])
	{
		pUser->packs.equip.obtain_exp(pUser,atoi(prop["equip_exp"].c_str()),true);

		ScenesService::gmlogger->info("GM:%s 设置了自己的装备经验值 %d",pUser->name,atoi(prop["equip_exp"].c_str()));
	}
	if (atoi(prop["exp"].c_str()) != 0)
	{
		pUser->charbase.exp = atoi(prop["exp"].c_str());
		if (pUser->charbase.exp >= pUser->charstate.nextexp)
		{
			pUser->upgrade(); 
		}

		ScenesService::gmlogger->info("GM:%s 设置了自己的经验值 %d",pUser->name,pUser->charbase.exp);
	}
	if (atoi(prop["cooling"].c_str()) != 0)
	{
		//pUser->packs.equip.updateDurability(pUser,atoi(prop["resume"].c_str()));
	}

	if (atoi(prop["str"].c_str()) != 0)
	{
		pUser->charbase.wdStr = atoi(prop["str"].c_str());
	}
	if (atoi(prop["int"].c_str()) != 0)
	{
		pUser->charbase.wdInt = atoi(prop["int"].c_str());
	}
	if (atoi(prop["dex"].c_str()) != 0)
	{
		pUser->charbase.wdDex = atoi(prop["dex"].c_str());
	}
	if (atoi(prop["men"].c_str()) != 0)
	{
		pUser->charbase.wdMen = atoi(prop["men"].c_str());
	}
	if (atoi(prop["con"].c_str()) != 0)
	{
		pUser->charbase.wdCon = atoi(prop["con"].c_str());
	}
	if (atoi(prop["peace"].c_str()) != 0)
	{
		if (atoi(prop["peace"].c_str()))
		{
			pUser->charbase.pkaddition = atoi(prop["peace"].c_str());
		}
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在的和平度累计时间%d分钟",
			pUser->charbase.pkaddition);
	}
	if (atoi(prop["actionpoint"].c_str()) != 0)
	{
		if (pUser->charbase.unionid>0)
		{
			Cmd::Session::t_OpUnionAction_SceneSession send;
			send.dwUnionID = pUser->charbase.unionid;
			send.dwAction = atoi(prop["actionpoint"].c_str());
			sessionClient->sendCmd(&send,sizeof(send));
		}
		else
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在还未加入任何帮会");
		}
	}

	//通知客户端
	Cmd::stMainUserDataUserCmd send;
	pUser->full_t_MainUserData(send.data);
	pUser->sendCmdToMe(&send,sizeof(send));
	pUser->sendInitHPAndMp();
	return true;
}

/*\brief 得到玩家的某个数值
* 包括魅力、经验值
*
* \param pUser: 用户
* \param para:内容
* return 得到是否成功 
*/
bool Gm::getvalue(SceneUser* pUser,const char* para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if (0 == strcmp(prop["type"].c_str(),"魅力"))
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在的魅力值=%u",pUser->charstate.charm);

		ScenesService::gmlogger->info("GM:%s 查看自己的魅力值",pUser->name);
		return true;
	}
	if (0 == strcmp(prop["type"].c_str(),"经验"))
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在的经验值=%u,升级经验值=%u",pUser->charbase.exp,pUser->charstate.nextexp );

		ScenesService::gmlogger->info("GM:%s 查看自己的系统设置",pUser->name);
		return true;
	}
	if ("sys"==prop["type"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"%x %x %x %x",pUser->sysSetting[0],pUser->sysSetting[1],pUser->sysSetting[2],pUser->sysSetting[3]);

		ScenesService::gmlogger->info("GM:%s 查看自己的经验值",pUser->name);
		return true;
	}

	ScenesService::gmlogger->info("GM:%s 使用getvalue命令,参数错误",pUser->name);
	return false;
}

/*\brief 设置和显示当前善恶
*
* \param pUser: 用户
* \param para:内容
* return true
*/
bool Gm::goodness(SceneUser* pUser,const char* para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	pUser->pkState.clearProtect();
	if (0 == strcmp(prop["type"].c_str(),"英雄"))
	{
		pUser->charbase.goodness = (DWORD)Cmd::GOODNESS_0;
	}
	else if (0 == strcmp(prop["type"].c_str(),"侠士"))
	{
		pUser->charbase.goodness = (DWORD)Cmd::GOODNESS_1;
	}
	else if (0 == strcmp(prop["type"].c_str(),"普通"))
	{
		pUser->charbase.goodness = (DWORD)Cmd::GOODNESS_2_1;
		while (!pUser->pkState.cancelProtect(pUser));
	}
	else if (0 == strcmp(prop["type"].c_str(),"歹徒"))
	{
		pUser->charbase.goodness = (DWORD)Cmd::GOODNESS_3;
	}
	else if (0 == strcmp(prop["type"].c_str(),"恶徒"))
	{
		pUser->charbase.goodness = (DWORD)Cmd::GOODNESS_4;
	}
	else if (0 == strcmp(prop["type"].c_str(),"恶魔"))
	{
		pUser->charbase.goodness = (DWORD)Cmd::GOODNESS_5;
	}
	else if (0 == strcmp(prop["type"].c_str(),"魔头"))
	{
		pUser->charbase.goodness = (DWORD)Cmd::GOODNESS_6;
	}
	else if (0 == strcmp(prop["type"].c_str(),"主动"))
	{
		pUser->charbase.goodness = (DWORD)Cmd::GOODNESS_ATT;
	}
	else if (0 == strcmp(prop["type"].c_str(),"自卫"))
	{
		pUser->charbase.goodness = (DWORD)Cmd::GOODNESS_DEF;
	}
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在是%s,数值是:%u",
		pUser->getGoodnessName(),pUser->charbase.goodness&0X0000FFFF);
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在是%s,数值是:%u",
		pUser->getGoodnessName(),pUser->charbase.goodness);

	ScenesService::gmlogger->info("GM:%s 把设置自己的善恶度为 %s",pUser->name,prop["type"].c_str());
	/*
	*
	switch(pUser->getGoodnessState())
	{
	case Cmd::GOODNESS_0:
	{
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在是%s","英雄");
	}
	break;
	case Cmd::GOODNESS_1:
	{
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在是%s","侠士");
	}
	break;
	case Cmd::GOODNESS_2_1:
	{
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在是%s","普通人");
	}
	break;
	case Cmd::GOODNESS_3:
	{
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在是%s","歹徒");
	}
	break;
	case Cmd::GOODNESS_4:
	{
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在是%s","恶徒");
	}
	break;
	case Cmd::GOODNESS_5:
	{
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在是%s","恶魔");
	}
	break;
	case Cmd::GOODNESS_6:
	{
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"您现在是%s","魔头");
	}
	break;
	break;
	}
	// */
	//pUser->reSendMyMapData();
	pUser->sendGoodnessToNine();

	return true;
}

/*\brief 后退
*
* \param pUser: 用户
* \param para:内容
* return true
*/
bool Gm::backoff(SceneUser* pUser,const char* para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	DWORD dir=atoi(prop["dir"].c_str());
	DWORD grid=atoi(prop["grid"].c_str());
	dir = dir?dir%8:(pUser->getDir() + 4)%8;
	grid = grid?grid:2;
	pUser->backOff(dir,grid);
	return true;
}
/*\brief 设置hp,mp,sp
*
* \param pUser: 用户
* \param para:内容
* return true
*/
bool Gm::value(SceneUser* pUser,const char* para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	DWORD hp=atoi(prop["hp"].c_str());
	DWORD mp=atoi(prop["mp"].c_str());
	DWORD sp=atoi(prop["sp"].c_str());
	DWORD ep=atoi(prop["ep"].c_str());
	DWORD honor=atoi(prop["honor"].c_str());
	DWORD gold=atoi(prop["gold"].c_str());
	DWORD ticket=atoi(prop["ticket"].c_str());
	DWORD pk=atoi(prop["pk"].c_str());
	DWORD caller=atoi(prop["caller"].c_str());

	if (caller)
	{
		pUser->packs.equip.king_obj_times= caller;
		pUser->packs.equip.king_obj_time = SceneTimeTick::currentTime.sec(); 
		pUser->packs.equip.family_obj_times= caller;
		pUser->packs.equip.family_obj_time = SceneTimeTick::currentTime.sec(); 
		pUser->packs.equip.tong_obj_times= caller;
		pUser->packs.equip.tong_obj_time = SceneTimeTick::currentTime.sec(); 
		Cmd::Session::t_Reset_CallTimes_SceneSession ss;
		ss.leaderTempID=pUser->tempid;
		sessionClient->sendCmd(&ss,sizeof(ss));
	}
	if (hp)
	{
		pUser->charbase.hp = hp;
		ScenesService::gmlogger->info("GM:%s 设置自己的生命值 %d",pUser->name,hp);
	}

	if (mp)
	{
		pUser->charbase.mp = mp;
		ScenesService::gmlogger->info("GM:%s 设置自己的魔法值 %d",pUser->name,mp);
	}

	if (sp)
	{
		pUser->charbase.sp = sp;
		ScenesService::gmlogger->info("GM:%s 设置自己的体力值 %d",pUser->name,sp);
	}

	if (ep)
	{
		pUser->charbase.exploit = ep;
		pUser->reSendMyMapData();
		ScenesService::gmlogger->info("GM:%s 设置自己的功勋值 %d",pUser->name,ep);
	}
	if (honor)
	{
		pUser->charbase.honor = honor;
		pUser->charbase.maxhonor = honor;
		ScenesService::gmlogger->info("GM:%s 设置自己的荣誉值 %d",pUser->name,honor);
	}
	if (gold)
	{
		pUser->packs.addGold(gold,"GM指令",NULL,true,true);
		Cmd::stMainUserDataUserCmd send;
		pUser->full_t_MainUserData(send.data);
		pUser->sendCmdToMe(&send,sizeof(send));
	}
	if (ticket)
	{
		pUser->packs.addTicket(ticket,"GM指令");
	}
	if (pk)
	{
		if (pk>9999) pk = 9999;
		pUser->charbase.goodness = (pUser->charbase.goodness & 0xffff0000) + pk;

		Cmd::stMainUserDataUserCmd  userinfo;
		pUser->full_t_MainUserData(userinfo.data);
		pUser->sendCmdToMe(&userinfo,sizeof(userinfo));
		pUser->sendMeToNine();
	}

	pUser->attackRTHpAndMp();

	return true;
}

/**
* \brief 使用药品
*
*
* \param pUser: 用户
* \param para:内容
* \return 使用是否成功
*/
bool Gm::leechdom(SceneUser *pUser,const char *para)
{
	//WAIT TO DO
#if 0
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["id"] && "" == prop["name"].c_str())
	{
		ScenesService::gmlogger->info("GM:%s 使用leechdom命令,参数错误",pUser->name);
		return false;
	}
	zObjectB *base = objectbm.get(atoi(prop["id"].c_str()));
	if (NULL == base)
		base = objectbm.get(prop["name"].c_str());
	if (base)
	{
		if (base->kind != ItemType_Leechdom && base->kind != ItemType_Food)
		{
			return false;
		}
		DWORD num=atoi(prop["num"].c_str());
		DWORD level=atoi(prop["level"].c_str());
		DWORD hole=atoi(prop["hole"].c_str());
		if (hole>6) hole=6;
		if (num==0) num=1;
		//num=1;
		zObject *o=zObject::create(base,num,level);
		if (o)
		{
			if (pUser->leechdom.isCooling(o->data.maxhp))
			{
				Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"该类药品处于冷却时期%s%u个",o->name,o->data.dwNum);
				SAFE_DELETE(o);
				ScenesService::gmlogger->info("GM:%s 使用药品 %s,该药品处于冷却时期",pUser->name,o->name);
				return false;
			}
			for (DWORD i=0; i<num; i++)
				pUser->useLeechdom(o);
			pUser->attackRTHpAndMp();
			Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"使用药品%s%u个",o->name,o->data.dwNum);
			ScenesService::gmlogger->info("GM:%s 使用药品 %s %d 个",pUser->name,o->name,o->data.dwNum);
			return true;
		}
	}
	ScenesService::gmlogger->info("GM:%s 使用leechdom命令,不存在该道具 id=%d name=%s",pUser->name,atoi(prop["id"].c_str()),prop["name"].c_str());
#endif 
	return false;
}

/**
* \brief 得到物品
*
*
* \param pUser: 用户
* \param para:内容
* \return 得到是否成功
*/
bool Gm::fetch(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["id"] && "" == prop["name"].c_str())
	{
		ScenesService::gmlogger->info("GM:%s 使用fetch命令,参数错误",pUser->name);
		return false;
	}

	zObjectB *base = objectbm.get(atoi(prop["id"].c_str()));

	if(base)
		Zebra::logger->error("kind = %u",base->kind);
	//sprintf(stderr,"kind = %u\n",base->kind);

	if (NULL == base)
		base = objectbm.get(prop["name"].c_str());
	if (base)
	{
		//不能获得马匹
		if (base->kind==ItemType_HORSE || base->kind==ItemType_BattleHorse)
			return false;

		DWORD num=atoi(prop["num"].c_str());
		if (num>(DWORD)base->maxnum) 
			num = base->maxnum;

		int level=atoi(prop["level"].c_str());
		if (level>11) level=11;
		DWORD hole=atoi(prop["hole"].c_str());

		DWORD AddPoint = 0;
		AddPoint = atoi(prop["point"].c_str());

		if (hole>6) hole=6;

		if (num==0) num=1;
		//gm指令制造银子有内存泄漏,修改ok
		if (base->id == 665)
		{
			pUser->packs.addMoney(num,"GM指令创建");
			ScenesService::gmlogger->info("GM:%s 创造了 %u 钱",pUser->name,num);
			return true;
		}
		zObject* o = NULL;
		if (base->recast) 
		{
			o = zObject::create(base,num,0);
			if (o && level) {
				do {
					Upgrade::upgrade(*pUser,o,100); //must success
				}while (--level >0); 
			}
		}
		else 
		{
			o = zObject::create(base,num,level);
		}

		if (o)
		{
			if (o->data.dwNum!=0 && pUser->packs.addObject(o,true,AUTO_PACK))
			{    
				//手动设置物品属性
				if (""!=prop["strName"]) strncpy(o->data.strName,prop["strName"].c_str(),MAX_NAMESIZE-1);
				int value;
				//if (0!=(value=atoi(prop["sex"].c_str()))) o->data.sex = value;
				if (0!=(value=atoi(prop["color"].c_str()))) o->data.color = value;
				if (0!=(value=atoi(prop["needlevel"].c_str()))) o->data.needlevel = value;
				//if (0!=(value=atoi(prop["level"].c_str()))) o->data.level = value;
				//if (0!=(value=atoi(prop["hp"].c_str())))o->data.hp = value;
				if (0!=(value=atoi(prop["maxhp"].c_str()))) o->data.maxhp = value;
				//if (0!=(value=atoi(prop["mp"].c_str()))) o->data.mp = value;
				if (0!=(value=atoi(prop["maxmp"].c_str()))) o->data.maxmp = value;
				//if (0!=(value=atoi(prop["sp"].c_str()))) o->data.sp = value;
				if (0!=(value=atoi(prop["maxsp"].c_str()))) o->data.maxsp = value;
				if (0!=(value=atoi(prop["pdamage"].c_str()))) o->data.pdamage = value;
				if (0!=(value=atoi(prop["maxpdamage"].c_str()))) o->data.maxpdamage = value;
				if (0!=(value=atoi(prop["pdam"].c_str()))) o->data.pdam = value;
				//if (0!=(value=atoi(prop["maxrdam"].c_str()))) o->data.maxrdam = value;
				//if (0!=(value=atoi(prop["tdam"].c_str()))) o->data.tdam = value;
				//if (0!=(value=atoi(prop["maxtdam"].c_str()))) o->data.maxtdam = value;
				if (0!=(value=atoi(prop["mdamage"].c_str()))) o->data.mdamage = value;
				if (0!=(value=atoi(prop["maxmdamage"].c_str()))) o->data.maxmdamage = value;
				if (0!=(value=atoi(prop["pdefence"].c_str()))) o->data.pdefence = value;
				if (0!=(value=atoi(prop["pdef"].c_str()))) o->data.pdef = value;
				if (0!=(value=atoi(prop["mdefence"].c_str()))) o->data.mdefence = value;
				if (0!=(value=atoi(prop["mdam"].c_str()))) o->data.mdam = value;
				if (0!=(value=atoi(prop["mdef"].c_str()))) o->data.mdef = value;
				if (0!=(value=atoi(prop["hpr"].c_str()))) o->data.hpr = value;
				if (0!=(value=atoi(prop["mpr"].c_str()))) o->data.mpr = value;
				if (0!=(value=atoi(prop["spr"].c_str()))) o->data.spr = value;
				if (0!=(value=atoi(prop["akspeed"].c_str()))) o->data.akspeed = value;
				//if (0!=(value=atoi(prop["mgspeed"].c_str()))) o->data.mgspeed = value;
				if (0!=(value=atoi(prop["mvspeed"].c_str()))) o->data.mvspeed = value;
				if (0!=(value=atoi(prop["atrating"].c_str()))) o->data.atrating = value;
				if (0!=(value=atoi(prop["akdodge"].c_str()))) o->data.akdodge = value;
				//if (0!=(value=atoi(prop["mgrating"].c_str()))) o->data.mgrating = value;
				//if (0!=(value=atoi(prop["mgdodge"].c_str()))) o->data.mgdodge = value;
				if (0!=(value=atoi(prop["dur"].c_str()))) o->data.dur = value;
				if (0!=(value=atoi(prop["maxdur"].c_str()))) o->data.maxdur = value;
				if (0!=(value=atoi(prop["price"].c_str()))) o->data.price = value;
				if (0!=(value=atoi(prop["cardpoint"].c_str()))) o->data.cardpoint = value;

				/*if (0!=(value=atoi(prop["str"].c_str()))) o->data.str = value;
				if (0!=(value=atoi(prop["inte"].c_str()))) o->data.inte = value;
				if (0!=(value=atoi(prop["dex"].c_str()))) o->data.dex = value;
				if (0!=(value=atoi(prop["spi"].c_str()))) o->data.spi = value;
				if (0!=(value=atoi(prop["con"].c_str()))) o->data.con = value;*/

				if (0!=(value=atoi(prop["fivetype"].c_str()))) o->data.fivetype = value;
				if ("0"==prop["fivetype"]) o->data.fivetype = 0;//金属性
				if (0!=(value=atoi(prop["fivepoint"].c_str()))) o->data.fivepoint = value;
				if (0!=(value=atoi(prop["kind"].c_str()))) o->data.kind = value;
				if (0!=(value=atoi(prop["upgrade"].c_str()))) o->data.upgrade = value;

				if (0!=(value=atoi(prop["doublexp"].c_str()))) o->data.doublexp = value;
				//if (0!=(value=atoi(prop["pdefup"].c_str()))) o->data.pdefup = value;
				//if (0!=(value=atoi(prop["mdefup"].c_str()))) o->data.mdefup = value;

				if (0!=(value=atoi(prop["poisondef"].c_str()))) o->data.poisondef = value;
				if (0!=(value=atoi(prop["lulldef"].c_str()))) o->data.lulldef = value;
				if (0!=(value=atoi(prop["reeldef"].c_str()))) o->data.reeldef = value;
				if (0!=(value=atoi(prop["evildef"].c_str()))) o->data.evildef = value;
				if (0!=(value=atoi(prop["bitedef"].c_str()))) o->data.bitedef = value;
				if (0!=(value=atoi(prop["stabledef"].c_str()))) o->data.stabledef = value;
				if (0!=(value=atoi(prop["slowdef"].c_str()))) o->data.slowdef = value;
				if (0!=(value=atoi(prop["luredef"].c_str()))) o->data.luredef = value;
				if (0!=(value=atoi(prop["colddef"].c_str()))) o->data.colddef = value;
				if (0!=(value=atoi(prop["petrifydef"].c_str()))) o->data.petrifydef = value;
				if (0!=(value=atoi(prop["blinddef"].c_str()))) o->data.blinddef = value;
				if (0!=(value=atoi(prop["chaosdef"].c_str()))) o->data.chaosdef = value;

				if (0!=(value=atoi(prop["poison"].c_str()))) o->data.poison = value;
				if (0!=(value=atoi(prop["lull"].c_str()))) o->data.lull = value;
				if (0!=(value=atoi(prop["reel"].c_str()))) o->data.reel = value;
				if (0!=(value=atoi(prop["evil"].c_str()))) o->data.evil = value;
				if (0!=(value=atoi(prop["bite"].c_str()))) o->data.bite = value;
				if (0!=(value=atoi(prop["stable"].c_str()))) o->data.stable = value;
				if (0!=(value=atoi(prop["slow"].c_str()))) o->data.slow = value;
				if (0!=(value=atoi(prop["lure"].c_str()))) o->data.lure = value;
				if (0!=(value=atoi(prop["cold"].c_str()))) o->data.cold = value;
				if (0!=(value=atoi(prop["petrify"].c_str()))) o->data.petrify = value;
				if (0!=(value=atoi(prop["blind"].c_str()))) o->data.blind = value;
				if (0!=(value=atoi(prop["chaos"].c_str()))) o->data.chaos = value;

				//if (0!=(value=atoi(prop["reflectp"].c_str()))) o->data.reflectp = value;
				//if (0!=(value=atoi(prop["reflect"].c_str()))) o->data.reflect = value;

				if (0!=(value=atoi(prop["durpoint"].c_str()))) o->data.durpoint = value;
				if (0!=(value=atoi(prop["dursecond"].c_str()))) o->data.dursecond = value;
				if (0!=(value=atoi(prop["holy"].c_str()))) o->data.holy = value;
				if (0!=(value=atoi(prop["bang"].c_str()))) o->data.bang = value;

				//if (0!=(value=atoi(prop["ehpp"].c_str()))) o->data.ehpp = value;
				//if (0!=(value=atoi(prop["ehpv"].c_str()))) o->data.ehpv = value;
				//if (0!=(value=atoi(prop["epdamp"].c_str()))) o->data.epdamp = value;
				//if (0!=(value=atoi(prop["epdamv"].c_str()))) o->data.epdamv = value;
				//if (0!=(value=atoi(prop["erdamp"].c_str()))) o->data.erdamp = value;
				//if (0!=(value=atoi(prop["erdamv"].c_str()))) o->data.erdamv = value;
				//if (0!=(value=atoi(prop["etdamp"].c_str()))) o->data.etdamp = value;
				//if (0!=(value=atoi(prop["etdamv"].c_str()))) o->data.etdamv = value;
				//if (0!=(value=atoi(prop["emdamp"].c_str()))) o->data.emdamp = value;
				//if (0!=(value=atoi(prop["emdamv"].c_str()))) o->data.emdamv = value;
				//if (0!=(value=atoi(prop["fivepro"].c_str()))) o->data.fivepro = value;
				if (0!=(value=atoi(prop["hpleech"].c_str()))) o->data.hpleech.odds = value;
				if (0!=(value=atoi(prop["hpleech_value"].c_str()))) o->data.hpleech.effect = value;
				if (0!=(value=atoi(prop["mpleech"].c_str()))) o->data.mpleech.odds = value;
				if (0!=(value=atoi(prop["mpleech_value"].c_str()))) o->data.mpleech.effect = value;

				if (0!=(value=atoi(prop["hptomp"].c_str()))) o->data.hptomp = value;
				//if (0!=(value=atoi(prop["dhpv"].c_str()))) o->data.dhpv = value;
				if (0!=(value=atoi(prop["dhpp"].c_str()))) o->data.dhpp = value;
				//if (0!=(value=atoi(prop["dmpv"].c_str()))) o->data.dmpv = value;
				if (0!=(value=atoi(prop["dmpp"].c_str()))) o->data.dmpp = value;

				if (0!=(value=atoi(prop["incgold"].c_str()))) o->data.incgold = value;
				if (0!=(value=atoi(prop["doublexp"].c_str()))) o->data.doublexp = value;
				if (0!=(value=atoi(prop["mf"].c_str()))) o->data.mf = value;

				if (0!=(value=atoi(prop["dpdam"].c_str()))) o->data.dpdam = value;
				if (0!=(value=atoi(prop["dmdam"].c_str()))) o->data.dmdam = value;
				if (0!=(value=atoi(prop["bdam"].c_str()))) o->data.bdam = value;
				if (0!=(value=atoi(prop["rdam"].c_str()))) o->data.rdam = value;
				if (0!=(value=atoi(prop["ignoredef"].c_str()))) o->data.ignoredef = value;

				if (0!=(value=atoi(prop["fiveset0"].c_str()))) o->data.fiveset[0] = value;
				if (0!=(value=atoi(prop["fiveset1"].c_str()))) o->data.fiveset[1] = value;
				if (0!=(value=atoi(prop["fiveset2"].c_str()))) o->data.fiveset[2] = value;
				if (0!=(value=atoi(prop["fiveset3"].c_str()))) o->data.fiveset[3] = value;
				if (0!=(value=atoi(prop["fiveset4"].c_str()))) o->data.fiveset[4] = value;

				//if (0!=(value=atoi(prop["invtarget"].c_str()))) o->data.invtarget = value;

				if (0!=(value=atoi(prop["exp"].c_str()))) o->data.exp = value;
				if (""!=prop["maker"]) strncpy(o->data.maker,prop["maker"].c_str(),MAX_NAMESIZE-1);
				//if (""!=prop["set"]) strncpy(o->data.set,prop["set"].c_str(),MAX_NAMESIZE-1);

				if (0!=(value=atoi(prop["skill_id"].c_str()))) o->data.skills.id = value;
				if (0!=(value=atoi(prop["skill_lv"].c_str()))) o->data.skills.point = value;

				if (0!=(value=atoi(prop["sid1"].c_str()))) o->data.skill[0].id = value;
				if (0!=(value=atoi(prop["slv1"].c_str()))) o->data.skill[0].point = value;

				if (0!=(value=atoi(prop["sid2"].c_str()))) o->data.skill[1].id = value;
				if (0!=(value=atoi(prop["slv2"].c_str()))) o->data.skill[1].point = value;

				if (0!=(value=atoi(prop["sid3"].c_str()))) o->data.skill[2].id = value;
				if (0!=(value=atoi(prop["slv3"].c_str()))) o->data.skill[2].point = value;

				if (0!=(value=atoi(prop["sid4"].c_str()))) o->data.skill[3].id = value;
				if (0!=(value=atoi(prop["slv4"].c_str()))) o->data.skill[3].point = value;

				if (0!=(value=atoi(prop["sid5"].c_str()))) o->data.skill[4].id = value;
				if (0!=(value=atoi(prop["slv5"].c_str()))) o->data.skill[4].point = value;

				if (0!=(value=atoi(prop["sid6"].c_str()))) o->data.skill[5].id = value;
				if (0!=(value=atoi(prop["slv6"].c_str()))) o->data.skill[5].point = value;

				if (0!=(value=atoi(prop["sid7"].c_str()))) o->data.skill[6].id = value;
				if (0!=(value=atoi(prop["slv7"].c_str()))) o->data.skill[6].point = value;

				if (0!=(value=atoi(prop["sid8"].c_str()))) o->data.skill[7].id = value;
				if (0!=(value=atoi(prop["slv8"].c_str()))) o->data.skill[7].point = value;

				if (0!=(value=atoi(prop["sid9"].c_str()))) o->data.skill[8].id = value;
				if (0!=(value=atoi(prop["slv9"].c_str()))) o->data.skill[8].point = value;

				if (0!=(value=atoi(prop["sid10"].c_str()))) o->data.skill[9].id = value;
				if (0!=(value=atoi(prop["slv10"].c_str()))) o->data.skill[9].point = value;

				if(AddPoint != 0)
				{
					o->data.kind = 8;
					o->data.Freedom.Surplus_Attribute = AddPoint;
				}

				//如果是双倍经验道具和荣誉道具需要绑定
				o->checkBind();
				Cmd::stAddObjectPropertyUserCmd send;
				bcopy(&o->data,&send.object,sizeof(t_Object));
				pUser->sendCmdToMe(&send,sizeof(send));
				Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"得到物品%s%u个",o->name,o->data.dwNum);
				ScenesService::gmlogger->error("GM:%s 创造了物品 %s %d 个",pUser->name,o->name,o->data.dwNum);
				return true;
			}
			zObject::destroy(o);
		}
		return false;
	}
	ScenesService::gmlogger->error("GM:%s 试图创造不存在的物品 name=%s id=%s ",pUser->name,prop["name"].c_str(),prop["id"].c_str());
	return false;
}

/**
* \brief 召唤npc
*
*
* \param pUser: 用户
* \param para:内容
* \return 得到是否成功
*/
bool Gm::summon(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	//prop.dump(std::cout);

	if ("" == prop["id"] && "" == prop["name"].c_str())
	{
		ScenesService::gmlogger->info("GM:%s 使用summon命令,参数错误。位置 %s (%d,%d)",pUser->name,pUser->scene->name,pUser->getPos().x,pUser->getPos().y);
		return false;
	}
	zNpcB *base = npcbm.get(atoi(prop["id"].c_str()));
	if (NULL == base)
		base = npcbm.get(prop["name"].c_str());
	if (base)
	{
		t_NpcDefine define;
		zPos pos = pUser->getPos();
		define.id = base->id;
		strcpy(define.name,base->name);
		if ("" == prop["x"]
		|| "" == prop["y"])
		{
			define.pos = pUser->getPos();
		}
		else
		{
			define.pos.x = atoi(prop["x"].c_str());
			define.pos.y = atoi(prop["y"].c_str());
		}
		define.width = SCREEN_WIDTH;
		define.height = SCREEN_HEIGHT;
		define.num = atoi(prop["num"].c_str());
		define.num = define.num > 0 ? define.num : 1;
		define.num = define.num < 100? define.num:100;
		define.interval = 30;
		define.initstate = zSceneEntry::SceneEntry_Normal;
		define.scriptID = atoi(prop["script"].c_str());

		pUser->scene->zPosRevaluate(define.pos);
		define.pos -= zPosHalfScreen;
		pUser->scene->zPosRevaluate(define.pos);
		pos -= zPosHalfScreen;
		pUser->scene->zPosRevaluate(pos);
		pUser->scene->initRegion(define.region,define.pos,define.width,define.height);
		int num = pUser->scene->summonNpc(define,pos,base,pUser->dupIndex);

		ScenesService::gmlogger->info("GM:%s 召唤怪物 %s %d 个。位置 %s (%d,%d)",pUser->name,define.name,num,pUser->scene->name,define.pos.x,define.pos.y);
		return true;
	}

	ScenesService::gmlogger->info("GM:%s 使用summon命令,参数错误。位置 %s (%d,%d)",pUser->name,pUser->scene->name,pUser->getPos().x,pUser->getPos().y);
	return false;
}

/*\brief GM给自己升级
* 
* 超级GM可以用这条命令给别的用户升级
*
* \param pUser: 用户
* \param para:内容
* \return 升级是否成功
*/
bool Gm::levelup(SceneUser *pUser,const char *para)
{
	if (0==pUser) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	int num=atoi(prop["num"].c_str());
	if (num>max_level) num = max_level;
	if (num <=0) num = 1;

	//给自己升级
	if (""==prop["name"] || !strncmp(pUser->name,prop["name"].c_str(),MAX_NAMESIZE))
	{
		if (pUser->upgrade(num))
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"恭喜你！升级了。");
			ScenesService::gmlogger->info("GM %s 使用levelup指令,提升了自己的等级 num=%d",pUser->name,num);
			return true;
		}
		else
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"升级失败");
			ScenesService::gmlogger->info("GM %s 使用levelup指令,提升自己的等级失败,num=%d",pUser->name,num);
			return false;
		}
	}
	else//给别人升级
	{
		//只有超级GM才可以给其他人升级
		if (!(pUser->getPriv() & Gm::super_mode) && !(pUser->getPriv() & Gm::debug_mode))
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"你不能提升别人的等级！权限不足");
			ScenesService::gmlogger->info("GM %s 使用levelup指令,试图提升 %s 的等级,权限不足",pUser->name,prop["name"].c_str());
			return false;
		}
		else
		{
			//没有锁管理器
			SceneUser * pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());

			if (pTarget)//玩家在线
			{
				if (pUser->getPriv() <= pTarget->getPriv())
				{                       
					Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"只能操作比自己级别低的玩家!");
					ScenesService::gmlogger->info("GM %s 使用用levelup命令,试图使 %s 升级,权限不足",pUser->name,prop["name"].c_str());                                    
					return false;
				}
				if (pTarget->upgrade(num))
				{
					Channel::sendSys(pTarget,Cmd::INFO_TYPE_GAME,"恭喜！%s 提升了你的等级 %d",pUser->name,num);
					Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"提升 %s 的等级 %d 成功",pTarget->name,num);
					ScenesService::gmlogger->info("GM %s 使用levelup指令,提升了 %s 的等级。 num=%d",pUser->name,prop["name"].c_str(),num);
					return true;
				}
				else
				{
					Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"升级失败");
					ScenesService::gmlogger->info("GM %s 使用levelup指令,提升 %s 的等级失败,num=%d",pUser->name,prop["name"].c_str(),num);
					return false;
				}
			}
			else//玩家不在线或不在当前场景服务器
			{
				Cmd::Session::t_gmCommand_SceneSession cmd;
				cmd.gm_cmd = Cmd::Session::GM_COMMAND_LEVELUP;
				cmd.src_priv = pUser->getPriv();
				cmd.x = num;//参数,要提升的等级数
				strncpy((char *)cmd.src_name,pUser->name,MAX_NAMESIZE-1);
				strncpy((char *)cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE-1);

				sessionClient->sendCmd(&cmd,sizeof(cmd));
				return true;
			}
		}
	}

	return false;
}


/*\brief 地图跳转
* 
*
* \param pUser: 用户
* \param para:内容
* \return 跳转是否成功
*/
bool Gm::gomap_Gm(SceneUser *pUser,const char *para)
{
	bool ignoreUserLevel = false;
	zProperties prop;
	prop.parseCmdLine(para);

	if ("true" == prop["ignoreUserLevel"])
	{
		ignoreUserLevel = true;
	}
	if ("" == prop["name"])
	{
		//    Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"请指定地图和坐标,如：//gomap name=XXX pos=100,100");
		ScenesService::gmlogger->info("GM %s 使用gomap命令,参数错误",pUser->name);
		return false;
	}
	pUser->charbase.gomaptype=atoi(prop["type"].c_str());
	std::string name = prop["name"];
	if (strstr(prop["name"].c_str(),"·") == NULL)
	{
		SceneManager::CountryMap_iter country_iter
			=SceneManager::getInstance().country_info.find(pUser->scene->getCountryID());
		if (country_iter != SceneManager::getInstance().country_info.end() && !(country_iter->second.function &0x01)/*不能跳到未开放国家*/ && pUser->scene->getCountryID()!=6)
		{
			name = country_iter->second.name;
			name+= "·";
			name+= prop["name"];
		}
		else
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"未开国家");
			Zebra::logger->debug("%s(%u)不能跳到未开放国家",pUser->name,pUser->id);
			return false;
		}
	}
	else
	{
		char country_name[MAX_NAMESIZE+1];
		bzero(country_name,sizeof(country_name));
		strncpy(country_name,name.c_str(),MAX_NAMESIZE);
		char *real = strstr(country_name,"·");
		if (real)
		{
			*real=0;
		}
		SceneManager::CountryMap_iter country_iter=SceneManager::getInstance().country_info.find(SceneManager::getInstance().getCountryIDByCountryName(country_name));
		if (country_iter != SceneManager::getInstance().country_info.end() && (country_iter->second.function &0x01)/*不能跳到未开放国家*/ && country_iter->first!=6)
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"未开国家");
			Zebra::logger->debug("%s(%u)不能跳到未开放国家",pUser->name,pUser->id);
			return false;
		}
	}
	class Scene * scene=SceneManager::getInstance().getSceneByName(name.c_str());

	zPos Pos;
	Pos.x = 0;
	Pos.y = 0;
	bool findPos=true;
	//正
	if (""!=prop["pos"])
	{
		zRegex regex("(.*)[,](.*)");
		if (/*(regex.compile("(.*)[,](.*)") && */regex.match(prop["pos"].c_str()))
		{
			//std::string s;
			//regex.getSub(s,1);
			Pos.x=regex.first;//atoi(s.c_str());
			//regex.getSub(s,2);
			Pos.y=regex.second;//atoi(s.c_str());
			//findPos=false; 
		}
	}
	else if (""!=prop["cpos"])
	{
		zPos cPos;
		zRegex regex("(.*)[,](.*)");
		if (/*(regex.compile("(.*)[,](.*)") && */regex.match(prop["cpos"].c_str()))
		{
			//std::string s;
			//regex.getSub(s,1);
			cPos.x=regex.first;//atoi(s.c_str());
			//regex.getSub(s,2);
			cPos.y=regex.second;//atoi(s.c_str());
			int rlen = atoi(prop["rlen"].c_str());
			if (!rlen)
			{
				rlen=6;
			}
			Pos.x=zMisc::randBetween((cPos.x - rlen)>0?(cPos.x - rlen):0,cPos.x + rlen);
			Pos.y=zMisc::randBetween((cPos.y - rlen)>0?(cPos.y - rlen):0,cPos.y + rlen);
			findPos=false; 
		}
	}

	if (scene)
	{
		if ((!ignoreUserLevel)&&(!scene->checkUserLevel(pUser))&&(pUser->scene!=scene)) return false;

		if (Pos.x>scene->width()) Pos.x = scene->width();
		if (Pos.y>scene->height()) Pos.y = scene->height();
		if (!findPos) return pUser->changeMap(scene,Pos,ignoreUserLevel);

		WayPoint *wp=pUser->scene->getWayPoint(scene->getFileName());
		if (wp==NULL)//没有从pUser->scene到scene的跳转点
		{
			wp=scene->getRandWayPoint();//随便找一个scene上的跳转点
			if (wp)
			{
				zPos pos=wp->getRandPoint();
				bool suc = pUser->changeMap(scene,pos,ignoreUserLevel);
				if (suc)
				{
					ScenesService::gmlogger->info("GM %s 使用gomap命令,目的 %s (%d,%d)",pUser->name,scene->name,pos.x,pos.y);
					return true;
				}
				else
				{
					ScenesService::gmlogger->info("GM %s 使用gomap命令,跳转地图失败,目的 %s (%d,%d)",pUser->name,scene->name,pos.x,pos.y);
					return false;
				}
			}
			else
			{
				ScenesService::gmlogger->info("GM %s 使用gomap命令,地图 %s 没有跳转�",pUser->name,name.c_str());
				return false;
			}
		}
		else
		{
			const zPos pos=wp->getRandDest().pos;
			bool suc = pUser->changeMap(scene,pos,ignoreUserLevel);
			if (suc)
			{
				ScenesService::gmlogger->info("GM %s 使用gomap命令,目的 %s (%d,%d)",pUser->name,scene->name,pos.x,pos.y);
				return true;
			}
			else
			{
				ScenesService::gmlogger->info("GM %s 使用gomap命令,跳转地图失败,目的 %s (%d,%d)",pUser->name,scene->name,pos.x,pos.y);
				return false;
			}
		}
	}
	else
	{
		if (pUser->guard && pUser->guard->canMove()) pUser->saveGuard = true;//使镖车跟随指令使用者
		if (pUser->adoptList.size()) pUser->saveAdopt = true;
		Cmd::Session::t_changeScene_SceneSession cmd;
		cmd.id = pUser->id;
		cmd.temp_id = pUser->tempid;
		cmd.x = Pos.x;
		cmd.y = Pos.y;
		cmd.map_id = 0;
		cmd.map_file[0] = '\0';
		strncpy((char *)cmd.map_name,name.c_str(),MAX_NAMESIZE);
		sessionClient->sendCmd(&cmd,sizeof(cmd));
		return true;
	}
}
#endif

/*\brief 地图跳转
* 
*
* \param pUser: 用户
* \param para:内容
* \return 跳转是否成功
*/
bool Gm::gomap(SceneUser *pUser,const char *para)
{
	bool ignoreUserLevel = false;
	zProperties prop;
	prop.parseCmdLine(para);

	if ("true" == prop["ignoreUserLevel"])
	{
		ignoreUserLevel = true;
	}
	if ("" == prop["name"])
	{
		return false;
	}
	pUser->charbase.gomaptype=atoi(prop["type"].c_str());
	std::string name = prop["name"];
	if (strstr(prop["name"].c_str(),"��") == NULL)
	{
		SceneManager::CountryMap_iter country_iter
			=SceneManager::getInstance().country_info.find(pUser->scene->getCountryID());
		if (country_iter != SceneManager::getInstance().country_info.end() && !(country_iter->second.function &0x01) && pUser->scene->getCountryID()!=6)
		{
			name = country_iter->second.name;
			name+= "��";
			name+= prop["name"];
		}
		else
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"未开国家");
			Zebra::logger->debug("%s(%u)不能跳到未开放国�",pUser->name,pUser->id);
			return false;
		}
	}
	else
	{
		char country_name[MAX_NAMESIZE+1];
		bzero(country_name,sizeof(country_name));
		strncpy(country_name,name.c_str(),MAX_NAMESIZE);
		char *real = strstr(country_name,"��");
		if (real)
		{
			*real=0;
		}
		SceneManager::CountryMap_iter country_iter=SceneManager::getInstance().country_info.find(SceneManager::getInstance().getCountryIDByCountryName(country_name));
		if (country_iter != SceneManager::getInstance().country_info.end() && (country_iter->second.function &0x01) && country_iter->first!=6)
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"未开国家");
			Zebra::logger->debug("%s(%u)不能跳到未开放国�",pUser->name,pUser->id);
			return false;
		}
	}
	class  Scene * scene=SceneManager::getInstance().getSceneByName(name.c_str());

	zPos Pos;
	Pos.x = 0;
	Pos.y = 0;
	bool findPos=true;
	//正

	if (""!=prop["pos"])
	{
	    if(Pos.parse(prop["pos"]))
		findPos = false;
	}
	else if (""!=prop["cpos"])
	{
		zPos cPos;
		if (cPos.parse(prop["cpos"]))
		{
			int rlen = atoi(prop["rlen"].c_str());
			if (!rlen)
			{
				rlen=6;
			}
			Pos.x=zMisc::randBetween((cPos.x - rlen)>0?(cPos.x - rlen):0,cPos.x + rlen);
			Pos.y=zMisc::randBetween((cPos.y - rlen)>0?(cPos.y - rlen):0,cPos.y + rlen);
			findPos=false; 
		}
	}

	if (scene)
	{
		if ((!ignoreUserLevel)&&(!scene->checkUserLevel(pUser))&&(pUser->scene!=scene)) return false;

		if (Pos.x>scene->width()) Pos.x = scene->width();
		if (Pos.y>scene->height()) Pos.y = scene->height();
		if (!findPos) return pUser->changeMap(scene,Pos,ignoreUserLevel);

		WayPoint *wp=pUser->scene->getWayPoint(scene->getFileName());
		if (wp==NULL)//没有从pUser->scene到scene的跳转点
		{
			wp=scene->getRandWayPoint();//随便找一个scene上的跳转点
			if (wp)
			{
				const zPos pos=wp->getRandPoint();
				return pUser->changeMap(scene,pos,ignoreUserLevel);
			}
			else
				return false;
		}
		else
		{
			const zPos pos=wp->getRandDest().pos.pos;
			return pUser->changeMap(scene,pos,ignoreUserLevel);
		}
	}
	else
	{
//		if (pUser->guard && pUser->guard->canMove()) pUser->saveGuard = true;//使镖车跟随指令使用者
//		if (pUser->adoptList.size()) pUser->saveAdopt = true;
		Cmd::Session::t_changeScene_SceneSession cmd;
		cmd.id = pUser->id;
		cmd.temp_id = pUser->tempid;
		cmd.x = Pos.x;
		cmd.y = Pos.y;
		cmd.map_id = 0;
		cmd.map_file[0] = '\0';
		strncpy((char *)cmd.map_name,name.c_str(),MAX_NAMESIZE);
		sessionClient->sendCmd(&cmd,sizeof(cmd));
		return true;
	}
}

#if 0
/*\brief 瞬移
* 
*
* \param pUser: 用户
* \param para:内容
* \return 是否成功
*/
bool Gm::goTo_Gm(SceneUser *pUser,const char *para)
{
	char para_1[MAX_CHATINFO + 1];
	bzero(para_1,sizeof(para_1));
	strncpy(para_1,para,MAX_CHATINFO);
	while(strlen(para_1) && para_1[strlen(para_1)-1] == 32)
	{
		para_1[strlen(para_1)-1] = 0;
	}

	//正
	zRegex regex("(.*)[,](.*)");
	zPos newPos;
	//Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"你的坐标是(%u,%u),城镇中心在(265,290)",pUser->getPos().x,pUser->getPos().y);
	if (/*(regex.compile("(.*)[,](.*)") && */regex.match(para_1))
	{
		//std::string s;
		//regex.getSub(s,1);
		newPos.x=regex.first;//atoi(s.c_str());
		//regex.getSub(s,2);
		//newPos.y=atoi(trim(s.c_str()));
		newPos.y=regex.second;//atoi(s.c_str());
		if (newPos.x>pUser->scene->width() || newPos.y>pUser->scene->height()) return false;
	}
	else
	{
		ScenesService::gmlogger->info("GM %s 使用goto命令,参数错误 %s",pUser->name,para);
		return false;
	}
	bool suc = pUser->goTo(newPos);
	if (suc)
	{
		ScenesService::gmlogger->info("GM %s 使用goto命令,目的 %s (%d,%d)",pUser->name,pUser->scene->name,newPos.x,newPos.y);
		return true;
	}
	else
	{
		ScenesService::gmlogger->info("GM %s 使用goto命令失败,目的 %s (%d,%d)",pUser->name,pUser->scene->name,newPos.x,newPos.y);
		return false;
	}


	return false;
}


/*\brief 瞬移
* 
*
* \param pUser: 用户
* \param para:内容
* \return 是否成功
*/
bool Gm::goTo(SceneUser *pUser,const char *para)
{
	char para_1[MAX_CHATINFO + 1];
	bzero(para_1,sizeof(para_1));
	strncpy(para_1,para,MAX_CHATINFO);
	while(strlen(para_1) && para_1[strlen(para_1)-1] == 32)
	{
		para_1[strlen(para_1)-1] = 0;
	}
	///正
	zRegex regex("(.*)[,](.*)");
	zPos newPos;
	if (/*(regex.compile("(.*)[,](.*)") && */regex.match(para_1))
	{
		//std::string s;
		//regex.getSub(s,1);
		newPos.x=regex.first;//atoi(s.c_str());
		//regex.getSub(s,2);
		newPos.y=regex.second;//atoi(s.c_str());
		if (newPos.x>pUser->scene->width() || newPos.y>pUser->scene->height()) return false;
	}
	else
	{
		return false;
	}
	return pUser->goTo(newPos);
	return false;
}

/*\brief 给技能升级
* 
*
* \param pUser: 用户
* \param para:内容
* \return 升级是否成功
*/
bool Gm::upgradeSkill(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["id"])
	{
		ScenesService::gmlogger->info("GM %s 使用upskill命令,参数错误 para=%s",pUser->name,para);
		return false;
	}

	bool suc = pUser->upgradeSkill(atoi(prop["id"].c_str()));
	if (suc)
	{
		ScenesService::gmlogger->info("GM %s 使用upskill命令,id=%s",pUser->name,prop["id"].c_str());
		return true;
	}
	else
	{
		ScenesService::gmlogger->info("GM %s 使用upskill命令失败,id=%s",pUser->name,prop["id"].c_str());
		return false;
	}
}

/*\brief 放弃任务
* 
*
* \param pUser: 用户
* \param para:内容
* \return 放弃是否成功
*/
bool Gm::abandon(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["id"])
	{
		ScenesService::gmlogger->info("GM %s 使用abandon命令,参数错误 para=%s",pUser->name,para);
		return false;
	}

	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"清除任务(%s)",prop["id"].c_str());
	pUser->quest_list.abandon(*pUser,atoi(prop["id"].c_str()),true);
	ScenesService::gmlogger->info("GM %s 使用abandon命令,取消任务 id=%s",pUser->name,prop["id"].c_str());
	return true;
}

/**
* \brief GM得到玩家所在的区域、位置
*
* \param pUser: 用户
* \param para:内容
* \return 得到是否成功
*/
bool Gm::finduser(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用finduser命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pTarget)
	{          
		//比较权限级别
		if (pUser->getPriv() <= pTarget->getPriv())
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"只能操作比自己级别低的玩家!");
			ScenesService::gmlogger->info("GM %s 使用finduser命令,试图得到 %s 的位置,权限不足",pUser->name,prop["name"].c_str());
			return false;
		}

		zPos Pos=pTarget->getPos();
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"玩家 %s 在区域:%s 坐标:(%d,%d)",prop["name"].c_str(),pTarget->scene->getName(),Pos.x,Pos.y);
		ScenesService::gmlogger->info("GM %s 使用finduser命令,得到 %s 的位置",pUser->name,prop["name"].c_str());
		return true;
	}
	else
	{
		Cmd::Session::t_gmCommand_SceneSession cmd;
		bzero(cmd.src_name,MAX_NAMESIZE);
		bzero(cmd.dst_name,MAX_NAMESIZE);
		bzero(cmd.map_file,MAX_NAMESIZE);
		bzero(cmd.map_name,MAX_NAMESIZE);
		cmd.gm_cmd = Cmd::Session::GM_COMMAND_FINDUSER;
		cmd.cmd_state = Cmd::Session::GM_COMMAND_STATE_REQ;
		cmd.id = pUser->id;
		cmd.src_priv = pUser->getPriv();
		strncpy((char *)&cmd.src_name,pUser->name,MAX_NAMESIZE);
		strncpy((char *)&cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE);
		return sessionClient->sendCmd(&cmd,sizeof(cmd));
	}
}

/**
* \brief 把GM带到一个玩家身边
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::gotouser(SceneUser *pUser,const char *para)
{               
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用gotouser命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pTarget)
	{
		//比较权限级别
		if (pUser->getPriv() <= pTarget->getPriv())
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"只能操作比自己级别低的玩家!");
			ScenesService::gmlogger->info("GM %s 使用gotouser命令,试图到达 %s 的位置,权限不足",pUser->name,prop["name"].c_str());
			return false;
		}

		//位置
		std::string mapName = "name=";
		mapName += pTarget->scene->getName();
		//坐标
		zPos Pos = pTarget->getPos();
		char position[MAX_NAMESIZE];
		bzero(position,sizeof(position));
		sprintf(position,"%d,%d",Pos.x,Pos.y);
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,position);

		//跳转
		gomap(pUser,mapName.c_str());
		goTo(pUser,position);

		ScenesService::gmlogger->info("GM %s 使用gotouser命令,到达 %s 的位置",pUser->name,prop["name"].c_str());
		return true;
	}       
	else    
	{
		Cmd::Session::t_gmCommand_SceneSession cmd;
		bzero(cmd.src_name,MAX_NAMESIZE);
		bzero(cmd.dst_name,MAX_NAMESIZE);
		bzero(cmd.map_file,MAX_NAMESIZE);
		bzero(cmd.map_name,MAX_NAMESIZE);
		cmd.gm_cmd = Cmd::Session::GM_COMMAND_GOTOUSER;
		cmd.cmd_state = Cmd::Session::GM_COMMAND_STATE_REQ;
		cmd.id = pUser->id;
		cmd.src_priv = pUser->getPriv();
		strncpy((char *)&cmd.src_name,pUser->name,MAX_NAMESIZE);
		strncpy((char *)&cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE);
		return sessionClient->sendCmd(&cmd,sizeof(cmd));
	}
}

/**
* \brief 补偿一个玩家股票
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::givestock(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用catchuser命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	DWORD gold=atoi(prop["gold"].c_str());
	DWORD money=atoi(prop["money"].c_str());
	DWORD accid=atoi(prop["accid"].c_str());
	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pTarget && (accid == pTarget->charbase.accid) && (pUser->scene->getCountryID() == pTarget->charbase.country))
	{
		Cmd::Bill::t_Stock_Save save;
		save.dwMoney=money;
		save.dwGold=gold;
		pTarget->sendSceneCmdToBill(&save,sizeof(save));
		ScenesService::gmlogger->info("GM %s 使用givestock命令,给玩家%s(%d)补偿股票金币:%d,股票银币:%d",pUser->name,pTarget->name,pTarget->id,gold,money);
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"给玩家%s(%d)补偿股票操作成功",pTarget->name,pTarget->charbase.accid);
		return true;
	}
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"给玩家补偿银币操作失败,原因:1.玩家不在线;2.名字和accid不对应;3.不在玩家所在国家");
	return false;
}
/**
* \brief 补偿一个玩家银币
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::givemoney(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用catchuser命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	DWORD num=atoi(prop["num"].c_str());
	DWORD accid=atoi(prop["accid"].c_str());
	if ((strncmp(prop["type"].c_str(),"股票提现",MAX_NAMESIZE)!=0))
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"给玩家补偿银币操作失败,原因:类型不正确");
		return false;
	}
	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pTarget && (accid == pTarget->charbase.accid) && (pUser->scene->getCountryID() == pTarget->charbase.country))
	{
		pTarget->packs.addMoney(num,prop["type"].c_str());
		ScenesService::gmlogger->info("GM %s 使用givemoney命令,给玩家%s(%d)补偿银币:%d,类型:%s",pUser->name,pTarget->name,pTarget->id,num,prop["type"].c_str());
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"给玩家%s(%d)补偿银币操作成功",pTarget->name,pTarget->charbase.accid);
		return true;
	}
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"给玩家补偿银币操作失败,原因:1.玩家不在线;2.名字和accid不对应;3.不在玩家所在国家");
	return false;
}
/**
* \brief 补偿一个玩家金币
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::givegold(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用catchuser命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	DWORD num=atoi(prop["num"].c_str());
	DWORD accid=atoi(prop["accid"].c_str());
	if ((strncmp(prop["type"].c_str(),"股票提现",MAX_NAMESIZE)!=0) && (strncmp(prop["type"].c_str(),"点卡换金币",MAX_NAMESIZE)!=0))
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"给玩家补偿金币操作失败,原因:类型不正确");
		return false;
	}
	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pTarget && (accid == pTarget->charbase.accid) && (pUser->scene->getCountryID() == pTarget->charbase.country))
	{
		if (strncmp(prop["type"].c_str(),"点卡换金币",MAX_NAMESIZE)==0)
		{
			DWORD temp=num/300;
			if (temp)
			{
				pTarget->charbase.goldgive +=temp;
				Channel::sendSys(pTarget,Cmd::INFO_TYPE_GAME,"你得到赠品精致升级宝石%d个,你可以到本人这里领哦",temp);
			}
		}
		pTarget->packs.addGold(num,prop["type"].c_str());
		ScenesService::gmlogger->info("GM %s 使用givegold命令,给玩家%s(%d)补偿金币:%d,类型:%s",pUser->name,pTarget->name,pTarget->id,num,prop["type"].c_str());
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"给玩家%s(%d)补偿金币操作成功",pTarget->name,pTarget->charbase.accid);
		return true;
	}
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"给玩家补偿银币操作失败,原因:1.玩家不在线;2.名字和accid不对应;3.不在玩家所在国家");
	return false;
}

/**
* \brief 把一个玩家带到GM身边
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::catchuser(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用catchuser命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pTarget)
	{
		//比较权限级别
		if (pUser->getPriv() <= pTarget->getPriv())
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"只能操作比自己级别低的玩家!");
			ScenesService::gmlogger->info("GM %s 使用catchuser命令,试图捕捉 %s,权限不足",pUser->name,prop["name"].c_str());
			return false;
		}

		//地图
		std::string mapName = "name=";
		mapName += pUser->scene->getName();
		//坐标
		zPos Pos = pUser->getPos();
		char position[MAX_NAMESIZE];
		bzero(position,sizeof(position));
		sprintf(position,"%d,%d",Pos.x,Pos.y);
		//Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,position);

		//坐标
		gomap(pTarget,mapName.c_str());
		goTo(pTarget,position);

		ScenesService::gmlogger->info("GM %s 使用catchuser命令,到达 %s 的位置",pUser->name,prop["name"].c_str());
		return true;
	}
	else
	{
		Cmd::Session::t_gmCommand_SceneSession cmd;
		bzero(cmd.src_name,MAX_NAMESIZE);
		bzero(cmd.dst_name,MAX_NAMESIZE);
		bzero(cmd.map_file,MAX_NAMESIZE);
		bzero(cmd.map_name,MAX_NAMESIZE);
		cmd.gm_cmd = Cmd::Session::GM_COMMAND_CATCHUSER;
		cmd.cmd_state = Cmd::Session::GM_COMMAND_STATE_REQ;
		cmd.id = pUser->id;
		cmd.src_priv = pUser->getPriv();
		cmd.x = pUser->getPos().x;
		cmd.y = pUser->getPos().y;
		strncpy((char *)&cmd.src_name,pUser->name,MAX_NAMESIZE);
		strncpy((char *)&cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE);
		strncpy((char *)&cmd.map_name,pUser->scene->name,MAX_NAMESIZE);
		return sessionClient->sendCmd(&cmd,sizeof(cmd));
	}
}

/**
* \brief 把一个玩家丢到活动室囚禁
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::embar(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用catchuser命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pTarget)
	{
		//比较权限级别
		if (pUser->getPriv() <= pTarget->getPriv())
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"只能操作比自己级别低的玩家!");
			ScenesService::gmlogger->info("GM %s 使用embar命令,试图捕捉 %s,权限不足",pUser->name,prop["name"].c_str());
			return false;
		}

		//地图
		std::string mapName = "name=活动室 pos=65,54 ignoreUserLevel=true";
		//Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,position);

		//坐标
		pTarget->killAllPets();
		gomap(pTarget,mapName.c_str());

		ScenesService::gmlogger->info("GM %s 使用embar命令,到达 %s 的位置",pUser->name,prop["name"].c_str());
		return true;
	}
	else
	{
		Cmd::Session::t_gmCommand_SceneSession cmd;
		bzero(cmd.src_name,MAX_NAMESIZE);
		bzero(cmd.dst_name,MAX_NAMESIZE);
		bzero(cmd.map_file,MAX_NAMESIZE);
		bzero(cmd.map_name,MAX_NAMESIZE);
		cmd.gm_cmd = Cmd::Session::GM_COMMAND_CATCHUSER;
		cmd.cmd_state = Cmd::Session::GM_COMMAND_STATE_REQ;
		cmd.id = pUser->id;
		cmd.src_priv = pUser->getPriv();
		cmd.x = 65;
		cmd.y = 54;
		strncpy((char *)&cmd.src_name,pUser->name,MAX_NAMESIZE);
		strncpy((char *)&cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE);
		strncpy((char *)&cmd.map_name,"活动室",MAX_NAMESIZE);
		return sessionClient->sendCmd(&cmd,sizeof(cmd));
	}
}


/**
* \brief  金币系统控制
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::goldsystem(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	if (0 == strcmp(prop["service"].c_str(),"start"))
	{
		Cmd::Scene::t_ServiceGold_GateScene ret;
		ret.byType=Cmd::Scene::ZT_SERVICE_START;
		Zebra::logger->debug("%s(%d)启动金币服务",pUser->name,pUser->id);
		SceneTaskManager::getInstance().broadcastCmd(&ret,sizeof(ret));
	}
	if (0 == strcmp(prop["service"].c_str(),"stop"))
	{
		Cmd::Scene::t_ServiceGold_GateScene ret;
		ret.byType=Cmd::Scene::ZT_SERVICE_STOP;
		Zebra::logger->debug("%s(%d)停止金币服务",pUser->name,pUser->id);
		SceneTaskManager::getInstance().broadcastCmd(&ret,sizeof(ret));
	}
	return true;
}
/**
* \brief  股票系统控制
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::stocksystem(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	if (0 == strcmp(prop["service"].c_str(),"start"))
	{
		Cmd::Scene::t_ServiceStock_GateScene ret;
		ret.byType=Cmd::Scene::ZT_SERVICE_START;
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"启动股票服务成功");
		Zebra::logger->debug("%s(%d)启动股票服务",pUser->name,pUser->id);
		SceneTaskManager::getInstance().broadcastCmd(&ret,sizeof(ret));
	}
	if (0 == strcmp(prop["service"].c_str(),"stop"))
	{
		Cmd::Scene::t_ServiceStock_GateScene ret;
		ret.byType=Cmd::Scene::ZT_SERVICE_STOP;
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"停止股票服务成功");
		Zebra::logger->debug("%s(%d)停止股票服务",pUser->name,pUser->id);
		SceneTaskManager::getInstance().broadcastCmd(&ret,sizeof(ret));
	}
	return true;
}
/**
* \brief  股票委托管理
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::stockconsign(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	if (0 == strcmp(prop["put"].c_str(),"true"))
	{
		Cmd::stPutListStockUserCmd ret;
		ret.dwNum=atoi(prop["num"].c_str());
		ret.dwPrice=atoi(prop["price"].c_str());
		ret.byType=atoi(prop["type"].c_str());
		pUser->sendCmdToBill(&ret,sizeof(ret));
	}
	if (0 == strcmp(prop["cancel"].c_str(),"true"))
	{
		Cmd::stRequestCancelListStockUserCmd ret;
		ret.id=atoi(prop["id"].c_str());
		ret.byType=atoi(prop["type"].c_str());
		pUser->sendCmdToBill(&ret,sizeof(ret));
	}
	if (0 == strcmp(prop["cancelall"].c_str(),"true"))
	{
		Cmd::t_CancelListAllStock_GateScene ret;
		pUser->sendCmdToBill(&ret,sizeof(ret));
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"股票休市成功");
		Zebra::logger->debug("%s(%d)股票休市",pUser->name,pUser->id);
	}
	if (0 == strcmp(prop["history"].c_str(),"true"))
	{
		Cmd::stRequstHistoryStockUserCmd ret;
		pUser->sendCmdToBill(&ret,sizeof(ret));
	}
	if (0 == strcmp(prop["myhistory"].c_str(),"true"))
	{
		Cmd::stRequstSelfHistoryStockUserCmd ret;
		ret.begintime=atoi(prop["time"].c_str());
		ret.num=atoi(prop["num"].c_str());
		pUser->sendCmdToBill(&ret,sizeof(ret));
	}
	return true;
}
/**
* \brief  股票资金管理
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::stocktransfer(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	if (0 == strcmp(prop["query"].c_str(),"true"))
	{
		Cmd::stQueryFundStockUserCmd qfs;
		pUser->sendCmdToBill(&qfs,sizeof(qfs));
	}
	if (0 == strcmp(prop["save"].c_str(),"true"))
	{
		pUser->stockSave(atoi(prop["money"].c_str()),atoi(prop["gold"].c_str()));
		/*
		Cmd::stTransferFundStockSaveUserCmd tfs;
		tfs.dwGold=atoi(prop["gold"].c_str());
		tfs.dwMoney=atoi(prop["money"].c_str());
		pUser->sendCmdToBill(&tfs,sizeof(tfs));
		// */
	}
	else if (0 == strcmp(prop["fetch"].c_str(),"true"))
	{
		Cmd::stTransferFundStockFetchUserCmd tff;
		tff.dwGold=atoi(prop["gold"].c_str());
		tff.dwMoney=atoi(prop["money"].c_str());
		pUser->sendCmdToBill(&tff,sizeof(tff));
	}
	return true;
}
/**
* \brief  股票用户
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::stockuser(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	Cmd::stPassowrdStockUserCmd ret;
	strncpy(ret.byPawword,prop["password"].c_str(),sizeof(ret.byPawword));
	strncpy(ret.byNew1,prop["new1"].c_str(),sizeof(ret.byNew1));
	strncpy(ret.byNew2,prop["new2"].c_str(),sizeof(ret.byNew2));
	pUser->sendCmdToBill(&ret,sizeof(ret));
	//pUser->gatetask->sendCmd(&ret,sizeof(ret));
	return true;
}
/**
* \brief  兑换月卡
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::redeemmonthcard(SceneUser *pUser,const char *para)
{
	Cmd::Scene::t_Request_RedeemCard rcg;
	rcg.dwUserID = pUser->id;
	pUser->gatetask->sendCmd(&rcg,sizeof(rcg));
	return true;
}
/**
* \brief 兑换金币 
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::redeemgold(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	DWORD point = atoi(prop["point"].c_str());
	Cmd::Scene::t_Request_RedeemGold rcg;
	rcg.dwUserID = pUser->id;
	rcg.dwNum = point>0?point:1;
	pUser->gatetask->sendCmd(&rcg,sizeof(rcg));
	return true;
}
/**
* \brief  查询月卡和金币
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::querygold(SceneUser *pUser,const char *para)
{
	Cmd::Scene::t_Request_Bill rcg;
	rcg.dwUserID = pUser->id;
	pUser->gatetask->sendCmd(&rcg,sizeof(rcg));
	return true;
}
/**
* \brief 扔东西
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::throwobject(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	DWORD type = atoi(prop["type"].c_str());

	if (type == 0) {
		pUser->packs.execEvery(&pUser->packs.main,Type2Type<DropFromPack>(),DropFromPack::Param(&pUser->packs.main,pUser->packs.main.size(),pUser->getPos()));
	}
	if (type == 1) {
		pUser->packs.execEvery(&pUser->packs.equip,Type2Type<DropFromPack>(),DropFromPack::Param(&pUser->packs.equip,pUser->packs.equip.size(),pUser->getPos()));
		if (pUser->packs.equip.needRecalc)
		{
			pUser->setupCharBase();
			Cmd::stMainUserDataUserCmd  userinfo;
			pUser->full_t_MainUserData(userinfo.data);
			pUser->sendCmdToMe(&userinfo,sizeof(userinfo));
			pUser->sendMeToNine();
		}
	}

#if 0
	//TODOBYLQY
	zProperties prop;
	prop.parseCmdLine(para);

	DWORD type = atoi(prop["type"].c_str());
	std::vector<zObject *> temp_vec;
	int begin = 0;
	std::set<zObject *>::iterator iter;
	if (type == 0)
	{
		for(iter = pUser->packs.main.getAllset().begin(); iter != pUser->packs.main.getAllset().end() ; iter ++)
		{
			//TODO 其它不可掉落物品
			if ((*iter)->data.upgrade > 5 || (*iter)->data.bind || (*iter)->data.pos.y == Cmd::EQUIPCELLTYPE_PACKAGE || (*iter)->data.pos.y == Cmd::EQUIPCELLTYPE_MAKE || (*iter)->base->kind==ItemType_MASK)
			{
				continue;
			}
			temp_vec.push_back(*iter);
		}
		int mainpack = temp_vec.size();
		for(int i = begin; i < mainpack ;  i ++)
		{
			pUser->packs.moveObjectToScene(&*temp_vec[i],pUser->getPos());
		}
	}
	if (type == 1)
	{
		bool needRecalc = false;
		begin = 0;
		temp_vec.clear();
		for(iter = pUser->packs.equip.getAllset().begin(); iter != pUser->packs.equip.getAllset().end() ; iter ++)
		{
			//TODO 其它不可掉落物品
			if ((*iter)->data.upgrade > 5 || (*iter)->data.bind || (*iter)->data.pos.y == Cmd::EQUIPCELLTYPE_PACKAGE || (*iter)->data.pos.y == Cmd::EQUIPCELLTYPE_MAKE )
			{
				continue;
			}
			temp_vec.push_back(*iter);
		}
		int  equippack = temp_vec.size();
		for(int i = begin; i < equippack ;  i ++)
		{
			pUser->packs.moveObjectToScene(&*temp_vec[i],pUser->getPos());
			needRecalc = true;
		}
		if (needRecalc)
		{
			pUser->setupCharBase();
			Cmd::stMainUserDataUserCmd  userinfo;
			pUser->full_t_MainUserData(userinfo.data);
			pUser->sendCmdToMe(&userinfo,sizeof(userinfo));
			pUser->sendMeToNine();
		}
	}
#endif
	return true;
}
/**
* \brief 组队:mode:模式,name:队员
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::team(SceneUser *pLeader,const char *para)
{
	//zProperties prop;
	//prop.parseCmdLine(para);

	//if ("" == prop["name"])
	//{
	//	Channel::sendSys(pLeader,Cmd::INFO_TYPE_FAIL,"缺少参数");
	//	ScenesService::gmlogger->info("GM %s 使用team命令,参数错误 para=%s",pLeader->name,para);
	//	return false;
	//}
	//SceneUser *pUser=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	//if (pUser)
	//{
	//	DWORD mode = atoi(prop["mode"].c_str());

	//	TeamManager * team = SceneManager::getInstance().GetMapTeam(pLeader->TeamThisID);
	//	//设置组队模式
	//	if (team)
	//	{
	//		if (mode < Cmd::TEAM_MAX)
	//		{
	//			pLeader->team_mode=mode;
	//		}
	//		else
	//		{
	//			Zebra::logger->warn("收到无效模式的邀请队员指令(%s,%u)",pLeader->name,pLeader->id);
	//			return false;
	//		}
	//	}
	//	else
	//	{
	//		if (pLeader->team_mode != mode)
	//		{
	//			Channel::sendSys(pLeader,Cmd::INFO_TYPE_FAIL,"一个队伍只能使用一种组队模式");
	//			return false;
	//		}
	//	}
	//	if (pLeader->team_mode == Cmd::TEAM_HONOR) 
	//	{
	//		if (pLeader->charbase.level < TEAM_HONOR_LEADER_LEVEL)
	//		{
	//			Channel::sendSys(pLeader,Cmd::INFO_TYPE_FAIL,"荣誉模式需要队长等级%d级",TEAM_HONOR_LEADER_LEVEL);
	//			return false;
	//		}
	//		if (pUser->charbase.level >= TEAM_HONOR_MEMBER_LEVEL)
	//		{
	//			Channel::sendSys(pLeader,Cmd::INFO_TYPE_FAIL,"荣誉模式队员等级不能大于%d级",TEAM_HONOR_MEMBER_LEVEL-1);
	//			return false;
	//		}
	//	}
	//	if (!pUser->IsOpen())
	//	{
	//		Channel::sendSys(pLeader,Cmd::INFO_TYPE_FAIL,"对方组队未开启");
	//		return false;
	//	}
	//	if (pUser->team.IsTeamed())
	//	{
	//		Channel::sendSys(pLeader,Cmd::INFO_TYPE_FAIL,"对方已有组队");
	//		return false;
	//	}

	//	Cmd::stRequestTeamUserCmd ret;
	//	ret.dwAnswerUserID = pLeader->tempid;
	//	ret.dwTeamMode = pLeader->team_mode;
	//	pUser->sendCmdToMe(&ret,sizeof(ret));
	//}
	return false;
}

/**
* \brief GM发送区域广播消息
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::bczone(SceneUser *pUser,const char *para)
{
	if ((NULL==pUser)||(NULL==para))
		return false;
	if (0 == strcmp(para,""))
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用bczone命令,参数错误 para=%s",pUser->name,para);
		return false;
	}

	//消息
	Cmd::stChannelChatUserCmd send;
	zRTime ctv;
	send.dwType=Cmd::CHAT_TYPE_GM;
	strncpy((char *)send.pstrChat,para,MAX_CHATINFO-1);

	/*
	//隐藏身份
	if (pUser->mask.is_masking()) 
	{
	strncpy((char *)send.pstrName,"蒙面人",MAX_NAMESIZE);
	}
	else 
	*/
	{
		strncpy((char *)send.pstrName,pUser->name,MAX_NAMESIZE);
	}

	pUser->scene->sendCmdToScene(&send,sizeof(send),pUser->dupIndex);

	ScenesService::gmlogger->info("GM %s 使用bczone命令,para=%s",pUser->name,para);
	return true;
}

/**
* \brief GM发送世界广播消息
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::bcwgamemsg(SceneUser *pUser,const char *para)
{
	if ((NULL==pUser)||(NULL==para))
		return false;
	if (""==para)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用bcwgamemsg命令,参数错误 para=%s",pUser->name,para);
		return false;
	}

	//消息
	Cmd::stChannelChatUserCmd send;
	zRTime ctv;
	send.dwType = Cmd::CHAT_TYPE_GM;
	send.dwSysInfoType = Cmd::INFO_TYPE_EXP;
	bzero(send.pstrName,sizeof(send.pstrName));
	bzero(send.pstrChat,sizeof(send.pstrChat));
	strncpy((char *)send.pstrChat,para,MAX_CHATINFO-1);
	/*
	//隐藏身份
	if (pUser->mask.is_masking()) 
	{
	strncpy((char *)send.pstrName,"蒙面人",MAX_NAMESIZE);
	}
	else 
	*/
	{
		strncpy((char *)send.pstrName,pUser->name,MAX_NAMESIZE);
	}
	pUser->forwardSession(&send,sizeof(Cmd::stChannelChatUserCmd));

	ScenesService::gmlogger->info("GM %s 使用bcwgamemsg命令,para=%s",pUser->name,para);
	return true;
}

/**
* \brief GM使一个玩家不能发言
* 如：//donttalk name=xwl time=86400
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::donttalk(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{       
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定玩家名字");
		ScenesService::gmlogger->info("GM %s 使用donttalk命令,参数错误 para=%s",pUser->name,para);
		return false;
	}

	int delay = atoi(prop["time"].c_str());
	if (86400<delay)
		delay = 86400;//最多禁言1天
	if (0>=delay)
		delay = 600;//默认禁言1小时

	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pTarget)
	{
		//比较权限级别
		if (pUser->getPriv() <= pTarget->getPriv())
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"只能操作比自己级别低的玩家!");
			ScenesService::gmlogger->info("GM %s 使用donttalk命令,试图使 %s 禁言,权限不足",pUser->name,prop["name"].c_str());
			return false;
		}
		pTarget->delayForbidTalk(delay);
		Channel::sendSys(pTarget,Cmd::INFO_TYPE_GAME,"你被 %s 禁言 %d 秒",pUser->name,delay);
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"玩家 %s 被禁言 %d 秒",pTarget->name,delay);
		ScenesService::gmlogger->info("GM %s 使用donttalk命令,玩家 %s 禁言 %d 秒",pUser->name,prop["name"].c_str(),delay);
		return true;
	}
	else    
	{
		Cmd::Session::t_gmCommand_SceneSession cmd;
		bzero(cmd.src_name,MAX_NAMESIZE);
		bzero(cmd.dst_name,MAX_NAMESIZE);
		bzero(cmd.map_file,MAX_NAMESIZE);
		bzero(cmd.map_name,MAX_NAMESIZE);
		cmd.gm_cmd = Cmd::Session::GM_COMMAND_DONTTALK;
		cmd.cmd_state = Cmd::Session::GM_COMMAND_STATE_REQ;
		cmd.id = pUser->id;
		cmd.src_priv = pUser->getPriv();
		cmd.x = delay;
		cmd.y = 0;
		strncpy((char *)&cmd.src_name,pUser->name,MAX_NAMESIZE);
		strncpy((char *)&cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE);
		return sessionClient->sendCmd(&cmd,sizeof(cmd));
	}
}

/**
* \brief GM使一个玩家恢复发言
* 如：//talk name=xwl
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::talk(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	SceneUser *pTarget=NULL;

	if ("" == prop["name"])
	{       
		/*                Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定玩家名字");
		ScenesService::gmlogger->info("GM %s 使用talk命令,参数错误 para=%s",pUser->name,para);
		return false;*/
		pTarget = pUser;
	}
	else
	{
		pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	}

	if (pTarget)
	{
		//比较权限级别
		if ((pUser->getPriv() <= pTarget->getPriv()) && (pUser->getPriv()!=Gm::super_mode))
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"只能操作比自己级别低的玩家!");
			ScenesService::gmlogger->info("GM %s 使用talk命令,试图使 %s 恢复禁言,权限不足",pUser->name,prop["name"].c_str());
			return false;
		}

		if (pTarget->isForbidTalk())
		{
			pTarget->delayForbidTalk(0);
			Channel::sendSys(pTarget,Cmd::INFO_TYPE_GAME,"%s 解除了你的禁言,现在可以说话了",pUser->name);
			ScenesService::gmlogger->info("GM %s 使用talk命令,解除玩家 %s 的禁言",pUser->name,prop["name"].c_str());
		}
		return true;
	}
	else    
	{
		Cmd::Session::t_gmCommand_SceneSession cmd;
		bzero(cmd.src_name,MAX_NAMESIZE);
		bzero(cmd.dst_name,MAX_NAMESIZE);
		bzero(cmd.map_file,MAX_NAMESIZE);
		bzero(cmd.map_name,MAX_NAMESIZE);
		cmd.gm_cmd = Cmd::Session::GM_COMMAND_TALK;
		cmd.cmd_state = Cmd::Session::GM_COMMAND_STATE_REQ;
		cmd.id = pUser->id;
		cmd.src_priv = pUser->getPriv();
		cmd.x = 0;
		cmd.y = 0;
		strncpy((char *)&cmd.src_name,pUser->name,MAX_NAMESIZE);
		strncpy((char *)&cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE);
		return sessionClient->sendCmd(&cmd,sizeof(cmd));
	}
}


/**
* \brief GM踢掉一个玩家
* 如：//kick name=xwl
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::kick(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{       
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定玩家名字");
		ScenesService::gmlogger->info("GM %s 使用kick命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pTarget && pUser)
	{       
		//比较权限级别
		if (pUser->getPriv() <= pTarget->getPriv())
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"不能踢比自己高级别的玩家!");
			ScenesService::gmlogger->info("GM %s 使用kick命令,试图把 %s 踢出服务器,权限不足",pUser->name,prop["name"].c_str());
			return false;
		}

		//SceneUserManager::getMe().lock();
		Channel::sendSys(pTarget,Cmd::INFO_TYPE_FAIL,"你已经被%s踢出服务器！",pUser->name);

		OnQuit event(1);
		EventTable::instance().execute(*pTarget,event);

		pTarget->save(Cmd::Record::LOGOUT_WRITEBACK);
		pTarget->killAllPets();

		//通知会话服务器
		Cmd::Session::t_unregUser_SceneSession ret;
		ret.dwUserID=pTarget->id;
		ret.dwSceneTempID=pTarget->scene->tempid;
		ret.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
		sessionClient->sendCmd(&ret,sizeof(ret));
		/*
		//通知会话服务器
		//Cmd::Session::t_unregUser_SceneSession ret;
		Cmd::Session::t_unregUser_GateSession ret;
		ret.dwUserID=pTarget->id;
		ret.dwSceneTempID=pTarget->scene->tempid;
		ret.retcode=Cmd::Session::UNREGUSER_RET_LOGOUT;
		sessionClient->sendCmd(&ret,sizeof(ret));
		*/

		//通知网关服务器
		Cmd::Scene::t_Unreg_LoginScene retgate;
		retgate.dwUserID = pTarget->id;
		retgate.dwSceneTempID = pTarget->scene->tempid;
		retgate.retcode = Cmd::Scene::UNREGUSER_RET_ERROR;
		pTarget->gatetask->sendCmd(&retgate,sizeof(retgate));

		pTarget->unreg();
		//SAFE_DELETE(pTarget);
		//SceneUserManager::getMe().unlock();
		/*
		*/

		ScenesService::gmlogger->info("GM %s 使用kick命令,把 %s 踢出服务器",pUser->name,prop["name"].c_str());
		return true;
	}
	else    
	{
		Cmd::Session::t_gmCommand_SceneSession cmd;
		bzero(cmd.src_name,MAX_NAMESIZE);
		bzero(cmd.dst_name,MAX_NAMESIZE);
		bzero(cmd.map_file,MAX_NAMESIZE);
		bzero(cmd.map_name,MAX_NAMESIZE);
		cmd.gm_cmd = Cmd::Session::GM_COMMAND_KICK;
		cmd.cmd_state = Cmd::Session::GM_COMMAND_STATE_REQ;
		cmd.src_priv = pUser->getPriv();
		strncpy((char *)&cmd.src_name,pUser->name,MAX_NAMESIZE);
		strncpy((char *)&cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE);
		return sessionClient->sendCmd(&cmd,sizeof(cmd));
	}
}

bool Gm::kickGateUser(SceneUser *pUser,const char *para)
{
	if (!pUser || !para) return false;
	zProperties prop;
	prop.parseCmdLine(para);

	Cmd::Scene::t_kickUser_GateScene send;
	send.userID = atoi(prop["userid"].c_str());
	send.accid = atoi(prop["accid"].c_str());

	struct SceneTaskSendCmdExec :public execEntry<SceneTask>
	{       
		void *_data; 
		DWORD _size;
		SceneTaskSendCmdExec(void *data,DWORD size):_data(data),_size(size){} 
		bool exec(SceneTask *task)
		{       
			if (task)
			{       
				task->sendCmd(_data,_size);
			}       
			return true;
		}       
	};      
	SceneTaskSendCmdExec exec(&send,sizeof(Cmd::Scene::t_kickUser_GateScene)); 
	SceneTaskManager::getInstance().execEverySceneTask(exec);
	return true;
}

/**
* \brief 显示宠物
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::showPets(SceneUser *pUser,const char *para)
{
	if (0==pUser) return false;
	int count = 0;
	if (pUser->pet)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"宠物:%s id=%d pos=%d,%d",pUser->pet->name,pUser->pet->tempid,pUser->pet->getPos().x,pUser->pet->getPos().y);
		count++;
	}
	if (pUser->summon)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"召唤兽:%s id=%d pos=%d,%d",pUser->summon->name,pUser->summon->tempid,pUser->summon->getPos().x,pUser->summon->getPos().y);
		count++;
	}
	for (std::list<ScenePet *>::iterator it=pUser->totems.begin(); it!=pUser->totems.end(); it++)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"图腾：%s id=%d pos=%d,%d",(*it)->name,(*it)->tempid,(*it)->getPos().x,(*it)->getPos().y);
		count++;
	} 
	if (pUser->ridepet)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"镖车:%s id=%d pos=%d,%d",pUser->ridepet->name,pUser->ridepet->tempid,pUser->ridepet->getPos().x,pUser->ridepet->getPos().y);
		count++;
	}
	if (pUser->guard)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"镖车:%s id=%d pos=%d,%d",pUser->guard->name,pUser->guard->tempid,pUser->guard->getPos().x,pUser->guard->getPos().y);
		count++;
	}
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"共 %d 个",count);
	return true;
}

/**
* \brief 显示地图
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::showMaps(SceneUser *pUser,const char *para)
{
	struct showMapsExec : public SceneCallBack
	{
		SceneUser * pUser;
		showMapsExec(SceneUser * p)
		{
			pUser = p;
		}
		~showMapsExec(){}

		bool exec(class Scene *s)
		{
			return Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,s->name);
		}
	};
	if (0==pUser) return false;

	showMapsExec sme(pUser);
	SceneManager::getInstance().execEveryScene(sme);
	return true;
}
/**
* \brief 加载地图,只有超级GM才可以使用
* 如：//loadmap name=舜·青州
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::loadMap(SceneUser *pUser,const char *para)
{
	if (0==pUser) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["mapname"])
	{       
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定地图名字");
		ScenesService::gmlogger->info("GM %s 使用loadmap命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	//Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"该指令目前不可用");
	DWORD countryid = 0;
	DWORD mapid = 0;
	if ("" == prop["countryname"])
	{
		SceneManager::CountryMap_iter country_iter
			=SceneManager::getInstance().country_info.find(pUser->scene->getCountryID());
		if (country_iter != SceneManager::getInstance().country_info.end())
		{
			countryid = pUser->scene->getCountryID();
		}
	}
	else
	{
		countryid = SceneManager::getInstance().getCountryIDByCountryName(prop["countryname"].c_str());
	}
	if (countryid == 0)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"输入的国家名称不正确");
		return false;
	}

	mapid = SceneManager::getInstance().getMapIDByMapName(prop["mapname"].c_str());
	if (mapid == 0)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"输入的地图名称不正确");
		return false;
	}
	//判断是否重复加载
	class Scene *loaded = SceneManager::getInstance().getSceneByID(SceneManager::getInstance().buildMapID(countryid,mapid));
	if (loaded)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"不能重复加载地图");
		return false;
	}

	DWORD serverid=atoi(prop["serverid"].c_str());
	if (!serverid || serverid == ScenesService::getInstance().getServerID())
	{

		//using namespace Cmd;
		loaded = SceneManager::getInstance().loadScene(0,countryid,mapid);
		if (loaded)
		{
			using namespace Cmd::Session;
			printf("向session发送注册消息(%s-%d-%d)\n",loaded->name,loaded->id,loaded->tempid);
			ScenesService::gmlogger->info("加载%s(%u,%u)成功",loaded->name,loaded->id,loaded->tempid);
			t_regScene_SceneSession regscene;
			regscene.dwCountryID = countryid;
			regscene.dwID=loaded->id;
			regscene.dwTempID=loaded->tempid;
			strncpy(regscene.byName,loaded->name,MAX_NAMESIZE);
			strncpy(regscene.fileName,loaded->getFileName(),MAX_NAMESIZE);
			sessionClient->sendCmd(&regscene,sizeof(regscene));
			ScenesService::gmlogger->info("GM %s 使用loadmap命令,加载地图 %s·%s",
				pUser->name,prop["countryname"].c_str(),prop["mapname"].c_str());
		}
	}
	else
	{
		using namespace Cmd::Session;
		t_reqAddScene_SceneSession ass;
		ass.dwServerID = serverid;
		ass.dwCountryID = countryid;
		ass.dwMapID = mapid;
		sessionClient->sendCmd(&ass,sizeof(ass));
		ScenesService::gmlogger->info("GM %s 使用loadmap命令,加载地图 %s·%s",
			pUser->name,prop["countryname"].c_str(),prop["mapname"].c_str());
	}

	// */
	return true;
}

/**
* \brief 卸载地图,只有超级GM才可以使用
* 如：//unloadmap name=舜·青州
* 
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::unloadMap(SceneUser *pUser,const char *para)
{
	if (0==pUser) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["mapname"])
	{       
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定地图名字");
		ScenesService::gmlogger->info("GM %s 使用unloadmap命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	DWORD countryid = 0;
	DWORD mapid = 0;
	if ("" == prop["countryname"])
	{
		SceneManager::CountryMap_iter country_iter
			=SceneManager::getInstance().country_info.find(pUser->scene->getCountryID());
		if (country_iter != SceneManager::getInstance().country_info.end())
		{
			countryid = pUser->scene->getCountryID();
		}
	}
	else
	{
		countryid = SceneManager::getInstance().getCountryIDByCountryName(prop["countryname"].c_str());
	}
	if (countryid == 0)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"输入的国家名称不正确");
		return false;
	}

	mapid = SceneManager::getInstance().getMapIDByMapName(prop["mapname"].c_str());
	if (mapid == 0)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"输入的地图名称不正确");
		return false;
	}
	ScenesService::gmlogger->info("GM %s 使用unloadmap命令,卸载地图 %s",pUser->name,prop["mapname"].c_str());
	Cmd::Session::t_unloadScene_SceneSession ret;
	//strncpy((char*)ret.map_name,name.c_str(),MAX_NAMESIZE);
	ret.map_id = SceneManager::getInstance().buildMapID(countryid,mapid);
	sessionClient->sendCmd(&ret,sizeof(ret));
	return false;
}


/**
* \brief 设置玩家权限,只有超级GM才可以用
* 如：//setpriv name=xwl priv=2
* 
* priv 目前有3个值： 0.任何指令都不能执行 1.普通玩家指令 2.普通GM指令 3.所有指令都能执行
* 
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::setPriv(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{       
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定玩家名字");
		ScenesService::gmlogger->info("GM %s 使用setpriv命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pUser==pTarget && (pUser->getPriv()!=Gm::super_mode))
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"不能设置自己！");
		ScenesService::gmlogger->info("GM %s 使用setpriv命令,试图设置自己的权限",pUser->name);
		return false;
	}

	int level = atoi(prop["priv"].c_str());
	if (pTarget && pUser)
	{       
		switch (level)
		{
		case 0:
			pTarget->setPriv(level);
			Channel::sendSys(pTarget,Cmd::INFO_TYPE_FAIL,"你被 %s 禁用所有命令",pUser->name);
			break;
		case 1:
			pTarget->setPriv(level);
			Channel::sendSys(pTarget,Cmd::INFO_TYPE_SYS,"你恢复到普通玩家状态");
			break;
		case 2:
			if (pTarget->id>1000)
			{
				Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"%s 不能成为GM",pTarget->name);
				ScenesService::gmlogger->info("GM %s 使用setpriv命令,试图设置 %s 的权限,id太大,id=%d",pUser->name,prop["name"].c_str(),pTarget->id);
				return false;
			}
			pTarget->setPriv(level);
			Channel::sendSys(pTarget,Cmd::INFO_TYPE_SYS,"你成为GM了！");
			break;
			/*
			case 3:
			{
			pTarget->setPriv(Gm::super_mode);
			Channel::sendSys(pTarget,Cmd::INFO_TYPE_SYS,"你恢复到超级GM玩家状态");
			}
			break;
			*/
		default:
			{
				Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"权限参数错误");
				ScenesService::gmlogger->info("GM %s 使用setpriv命令,参数错误 para=%s",pUser->name,para);
				return false;
			}
		}
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"设置成功");
		ScenesService::gmlogger->info("GM %s 使用setpriv命令,设置 %s 的权限,priv=%d",pUser->name,prop["name"].c_str(),level);
		return true;
	}
	else    
	{
		Cmd::Session::t_gmCommand_SceneSession cmd;
		bzero(cmd.src_name,MAX_NAMESIZE);
		bzero(cmd.dst_name,MAX_NAMESIZE);
		bzero(cmd.map_file,MAX_NAMESIZE);
		bzero(cmd.map_name,MAX_NAMESIZE);
		cmd.gm_cmd = Cmd::Session::GM_COMMAND_SETPRIV;
		cmd.cmd_state = Cmd::Session::GM_COMMAND_STATE_REQ;
		cmd.src_priv = pUser->getPriv();
		cmd.x = level;
		strncpy((char *)&cmd.src_name,pUser->name,MAX_NAMESIZE);
		strncpy((char *)&cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE);
		return sessionClient->sendCmd(&cmd,sizeof(cmd));
	}
}

#endif
/**
* \brief 隐藏自己 
* 隐身之后任何人都看不见该角色
* 
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::hideMe(SceneUser *pUser,const char *para)
{
	if (pUser)
	{       
		pUser->setState(zSceneEntry::SceneEntry_Hide);

		//通知9屏删除该角色
		Cmd::stRemoveUserMapScreenUserCmd remove;
		remove.dwUserTempID=pUser->tempid;
		pUser->scene->sendCmdToNine(pUser->getPosI(),&remove,sizeof(remove),pUser->dupIndex);

		//移除block
		pUser->scene->clearBlock(pUser->getPos());

		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"隐身成功");
		ScenesService::gmlogger->info("GM %s 使用hideme命令,隐身",pUser->name);
		return true;
	}
	return false;
}

/**
* \brief 取消隐身 
* 恢复可见状态
* 
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::showMe(SceneUser *pUser,const char *para)
{
	if (pUser)
	{       
		pUser->setState(zSceneEntry::SceneEntry_Normal);

		zPos curPos = pUser->getPos();
		pUser->goTo(curPos);
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"恢复到可见状态");
		ScenesService::gmlogger->info("GM %s 使用showme命令,解除隐身",pUser->name);
		return true;
	}
	return false;
}


/**
* \brief 无敌模式 
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::god(SceneUser *pUser,const char *para)
{
	if (pUser)
	{       
		pUser->angelMode = true;

		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"进入无敌模式！");
		ScenesService::gmlogger->info("GM %s 使用god命令,进入不死模式",pUser->name);
		return true;
	}
	return false;
}

#if 0
/**
* \brief 必杀模式 
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::killer(SceneUser *pUser,const char *para)
{
	if (pUser)
	{       
		pUser->afreetMode = true;

		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"进入必杀模式！");
		ScenesService::gmlogger->info("GM %s 使用killer命令,进入必杀模式",pUser->name);
		return true;
	}
	return false;
}
bool Gm::newzone(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	if (pUser)
	{       
		if (strncmp("true",prop["service"].c_str(),MAX_NAMESIZE)==0)
		{
			if (!SceneManager::getInstance().isNewZoneConfig())
			{
				SceneManager::getInstance().setNewZoneConfig(true);
				SceneManager::getInstance().addNewZonePos(151,211); 
				SceneManager::getInstance().addNewZonePos(250,175); 
				SceneManager::getInstance().addNewZonePos(212,330); 
				SceneManager::getInstance().addNewZonePos(159,343); 
			}
			else
			{
				SceneManager::getInstance().setNewZoneConfig(true);
			}
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"设置新区配置成功");
			ScenesService::gmlogger->info("GM %s 设置新区配置",pUser->name);
		}
		if (strncmp("false",prop["service"].c_str(),MAX_NAMESIZE)==0)
		{
			SceneManager::getInstance().setNewZoneConfig(false);
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"取消新区配置成功");
			ScenesService::gmlogger->info("GM %s 取消新区配置",pUser->name);
		}
		DWORD x = atoi(prop["x"].c_str());
		DWORD y = atoi(prop["y"].c_str());
		if (x && y)
		{
			SceneManager::getInstance().addNewZonePos(x,y); 
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"增加新区配置重生坐标点:x=%d,y=%d",x,y);
			ScenesService::gmlogger->info("GM %s 增加新区配置重生坐标点:x=%d,y=%d",pUser->name,x,y);
		}
		Cmd::Session::t_gmCommand_SceneSession send;
		send.gm_cmd=Cmd::Session::GM_COMMAND_NEWZONE;
		send.cmd_state=2;
		send.x=0;
		send.y=0;
		if (strncmp("true",prop["serviceall"].c_str(),MAX_NAMESIZE)==0)
		{
			send.cmd_state=1;
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"设置全区新区配置成功");
			ScenesService::gmlogger->info("GM %s 设置新区配置",pUser->name);
		}
		if (strncmp("false",prop["serviceall"].c_str(),MAX_NAMESIZE)==0)
		{
			send.cmd_state=0;
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"取消全区新区配置成功");
			ScenesService::gmlogger->info("GM %s 取消新区配置",pUser->name);
		}
		DWORD xall = atoi(prop["xall"].c_str());
		DWORD yall = atoi(prop["yall"].c_str());
		if (xall && yall)
		{
			send.x=xall;
			send.y=yall;
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"增加新区配置重生坐标点:x=%d,y=%d",xall,yall);
			ScenesService::gmlogger->info("GM %s 增加新区配置重生坐标点:x=%d,y=%d",pUser->name,xall,yall);
		}
		if ((send.cmd_state != 2) || (send.x != 0 && send.y != 0))
		{
			sessionClient->sendCmd(&send,sizeof(send));
		}
		if (strncmp("true",prop["query"].c_str(),MAX_NAMESIZE)==0)
		{
			SceneManager::NewZoneVec &vec = SceneManager::getInstance().queryNewZonePos(); 
			SceneManager::newZoneVec_iter iter = vec.begin();
			for(;iter != vec.end() ; iter ++)
			{
				Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"新区配置重生坐标点:x=%d,y=%d",(*iter).first,(*iter).second);
			}
		}
		return true;
	}
	return false;
}

/**
* \brief 恢复普通模式 
* 取消无敌和必杀模式
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::normal(SceneUser *pUser,const char *para)
{
	if (pUser)
	{       
		pUser->angelMode = false;
		pUser->afreetMode = false;
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"恢复普通模式");
		ScenesService::gmlogger->info("GM %s 使用normal命令,恢复普通模式",pUser->name);
		return true;
	}
	return false;
}

/**
* \brief 回到国家主城 
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::goHome(SceneUser *pUser,const char *para)
{
	if (pUser)
	{       
		zXMLParser parser = zXMLParser();
		char str[MAX_NAMESIZE];
		bzero(str,MAX_NAMESIZE);
		if (parser.initFile(Zebra::global["configdir"] + "scenesinfo.xml"))
		{
			xmlNodePtr root = parser.getRootNode("ScenesInfo");
			if (0==root) return false;

			xmlNodePtr infoNode = parser.getChildNode(root,"countryinfo");
			if (0==infoNode) return false;

			xmlNodePtr countryNode = parser.getChildNode(infoNode,"country");
			while (countryNode)
			{
				DWORD id=0;
				parser.getNodePropNum(countryNode,"id",&id,sizeof(id));
				if (id==pUser->charbase.country)
				{
					std::string s = "name=";
					parser.getNodePropStr(countryNode,"name",str,sizeof(str));
					s += str;
					s += "·";

					DWORD cap_id;//主城的mapID
					parser.getNodePropNum(countryNode,"mapID",&cap_id,sizeof(cap_id));

					infoNode = parser.getChildNode(root,"mapinfo");
					if (0==infoNode) return false;

					xmlNodePtr mapNode = parser.getChildNode(infoNode,"map");

					while (mapNode)
					{
						parser.getNodePropNum(mapNode,"mapID",&id,sizeof(id));
						if (id==cap_id)
						{
							parser.getNodePropStr(mapNode,"name",str,sizeof(str));
							s += str;
							s += " pos=";

							parser.getNodePropStr(mapNode,"fileName",str,sizeof(str));

							zXMLParser parser2 = zXMLParser();
							char mapFile[MAX_NAMESIZE];
							bzero(mapFile,sizeof(mapFile));
							sprintf(mapFile,"%s/%s.xml",Zebra::global["datadir"].c_str(),str);
							if (parser2.initFile(mapFile))
							{
								xmlNodePtr root2 = parser2.getRootNode("map");
								xmlNodePtr zoneNode = parser2.getChildNode(root2,"zonedef");
								int num;
								while (zoneNode)
								{
									parser2.getNodePropNum(zoneNode,"type",&num,sizeof(num));
									if (8==num)
									{
										parser2.getNodePropStr(zoneNode,"x",str,sizeof(str));
										s += str;
										s += ",";
										parser2.getNodePropStr(zoneNode,"y",str,sizeof(str));
										s += str;

										break;
									}
									zoneNode = parser2.getNextNode(zoneNode,"zonedef");;
								}
							}
							else
								ScenesService::gmlogger->info("GM %s 使用gohome指令,打开 %s 文件失败",pUser->name,mapFile);
							break;
						}
						mapNode = parser.getNextNode(mapNode,"map");
					}

					gomap(pUser,s.c_str());

					//Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"主城,country=%d,param=%s",pUser->charbase.country,s.c_str());
					ScenesService::gmlogger->info("GM %s 使用gohome命令,返回主城 param=%s ",pUser->name,s.c_str());
					return true;
				}
				countryNode = parser.getNextNode(countryNode,"country");
			}
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"未找到主城,country=%d",pUser->charbase.country);
			ScenesService::gmlogger->info("GM %s 使用gohome命令失败,没找到主城。countryID=%d",pUser->name,pUser->charbase.country);
		}
		return false;
	}
	return false;
}

/**
* \brief 技能测试指令
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::skill(SceneUser *pUser,const char *para)
{
	if ((NULL==pUser)||(NULL==para))
		return false;
	if (""==para)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用skill命令,参数错误 para=%s",pUser->name,para);
		return false;
	}

	zProperties prop;
	prop.parseCmdLine(para);

	if ("pkvalue" == prop["show"])
	{
		pUser->skillStatusM.showValue();
	}
	else if ("active" == prop["show"])
	{
		pUser->skillStatusM.showActive();
	}
	else if ("recovery" == prop["show"])
	{
		pUser->skillStatusM.showRecovery();
	}
	else if ("passiveness" == prop["show"])
	{
		pUser->skillStatusM.showPassiveness();
	}
	else if ("log" == prop["show"])
	{
		pUser->skillStatusM.showValueToLog();
	}



	if (!("" == prop["defencer"] ||
		"" == prop["magictype"] ||
		"" == prop["attacktype"]))
	{
		Cmd::stAttackMagicUserCmd send;

		SceneUser *defencer = SceneUserManager::getMe().getUserByName(prop["defencer"].c_str());
		if (defencer)
		{
			send.dwUserTempID = pUser->tempid;
			send.dwDefenceTempID = defencer->tempid;
			send.wdMagicType = atoi(prop["magictype"].c_str());
			send.xDes = (WORD)pUser->getPos().x;
			send.yDes = (WORD)pUser->getPos().y;
			send.byDirect = pUser->getDir();
			send.byAttackType = atoi(prop["attacktype"].c_str());
			send.byAction = 0;
			Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"我攻击了%s",defencer->name);
			pUser->attackMagic(&send,sizeof(send));
			ScenesService::gmlogger->info("GM %s 使用skill命令,攻击了 %s ",pUser->name,defencer->name);
			return true;
		}
		else
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"玩家 %s 不在线",prop["defencer"].c_str());
			ScenesService::gmlogger->info("GM %s 使用skill命令,玩家 %s 不在线",pUser->name,prop["defencer"].c_str());
			return false;
		}
	}
	else    
	{
		ScenesService::gmlogger->info("GM %s 使用skill命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
}


/**
* \brief 对玩家进行锁血等处理
* 范例
* //lockvalue name=member1 lock=mp
* //lockvalue name=member1 unlock=all
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::lockValue(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	/*        if ("" == prop["name"])
	{       
	Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定玩家名字");
	ScenesService::gmlogger->info("GM %s 使用lockvalue命令,参数错误 para=%s",pUser->name,para);
	return false;
	}
	*/
	Cmd::Session::t_gmCommand_SceneSession cmd;
	cmd.err_code = 0;//错误信息
	bzero(cmd.src_name,MAX_NAMESIZE);
	bzero(cmd.dst_name,MAX_NAMESIZE);
	bzero(cmd.map_file,MAX_NAMESIZE);
	bzero(cmd.map_name,MAX_NAMESIZE);
	cmd.gm_cmd = Cmd::Session::GM_COMMAND_LOCKVALUE;
	cmd.cmd_state = Cmd::Session::GM_COMMAND_STATE_REQ;
	cmd.src_priv = pUser->getPriv();

	strncpy((char *)&cmd.src_name,pUser->name,MAX_NAMESIZE);

	if ("" == prop["name"])
	{
		strncpy((char*)&cmd.dst_name,pUser->name,MAX_NAMESIZE);
	}
	else
	{
		strncpy((char *)&cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE);
	}

	if ("mp" == prop["lock"])
	{
		cmd.x = 2;
	}
	else if ("hp" == prop["lock"])
	{
		cmd.x = 1;
	}
	else if ("sp" == prop["lock"])
	{
		cmd.x = 3;
	}
	else if ("all" == prop["lock"])
	{
		cmd.x = 4;
	}
	else
	{
		cmd.x = 5;
	}

	if ("mp" == prop["unlock"])
	{
		cmd.y = 2;
	}
	else if ("hp" == prop["unlock"])
	{
		cmd.y = 1;
	}
	else if ("sp" == prop["unlock"])
	{
		cmd.y = 3;
	}
	else if ("all" == prop["unlock"])
	{
		cmd.y = 4;
	}
	else
	{
		cmd.y = 5;
	}
	return sessionClient->sendCmd(&cmd,sizeof(cmd));

}


/**
* \brief 模拟家族投票
* 范例
* //svote name=暗黑族
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::svote(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{       
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定家族名字");
		return false;
	}

	Cmd::stVoteListSeptCmd send1;
	pUser->forwardSession(&send1,sizeof(Cmd::stVoteListSeptCmd));

	Cmd::stVoteSeptCmd send;
	strncpy(send.septName,prop["name"].c_str(),MAX_NAMESIZE);

	pUser->forwardSession(&send,sizeof(Cmd::stVoteSeptCmd));
	return true;
}

/**
* \brief 模拟帮会投票
* 范例
* //uvote name=暗黑帮
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::uvote(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定帮会名字");
		return false;
	}

	Cmd::stVoteListUnionCmd send1;
	pUser->forwardSession(&send1,sizeof(Cmd::stVoteListUnionCmd));

	Cmd::stVoteUnionCmd send;
	strncpy(send.unionName,prop["name"].c_str(),MAX_NAMESIZE);

	pUser->forwardSession(&send,sizeof(Cmd::stVoteUnionCmd));
	return true;
}

/**
* \brief 强制激活帮会战
* 范例
* //tong t1=暗黑帮 t2=魔兽帮...
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::tong(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["t1"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定帮会名字");
		return false;
	}

	if ("" == prop["t2"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定帮会名字");
		return false;
	}


	Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"帮会战状态已激活。");
	return true;

}

/**
* \brief 显示特定npc的具体位置信息
*
*/
class ShowSpecialNpcCallBack : public specialNpcCallBack
{
private:
	SceneUser * pUser;
public:
	int count;
	ShowSpecialNpcCallBack(SceneUser * user):pUser(user),count(0){}
	bool exec(SceneNpc * npc)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"特殊NPC: %s 位置:%s (%u,%u)",npc->name,npc->scene->name,npc->getPos().x,npc->getPos().y);
		count++;
		return true;
	}
};

/**
* \brief 显示特定npc的位置信息
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::showSpecialNpc(SceneUser *pUser,const char *para)
{
	ShowSpecialNpcCallBack ssncb(pUser);
	SceneNpcManager::getMe().execAllSpecialNpc(ssncb);
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"共 %u 个",ssncb.count);
	return true;
}

/**
* \brief 技能宠物
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::killPets(SceneUser *pUser,const char *para)
{
	pUser->killAllPets();
	return true;
}
/**
* \brief 攻城
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::createRush(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["id"])
	{       
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,参照//createrush id=1 delay=60");
		ScenesService::gmlogger->info("GM %s 使用createrush命令,参数错误 para=%s",pUser->name,para);
		return false;
	}

	int id = atoi(prop["id"].c_str());
	int delay = atoi(prop["delay"].c_str());
	zXMLParser parser = zXMLParser();
	DWORD countryID=0;
	if (""!=prop["country"])
	{
		char str[MAX_NAMESIZE];
		bzero(str,MAX_NAMESIZE);
		if (parser.initFile(Zebra::global["configdir"] + "scenesinfo.xml"))
		{
			xmlNodePtr root = parser.getRootNode("ScenesInfo");
			if (0==root) return false;

			xmlNodePtr infoNode = parser.getChildNode(root,"countryinfo");
			if (0==infoNode) return false;

			xmlNodePtr countryNode = parser.getChildNode(infoNode,"country");
			while (countryNode)
			{
				parser.getNodePropStr(countryNode,"name",str,sizeof(str));
				if (0 == strcmp(str,prop["country"].c_str()))
				{
					parser.getNodePropNum(countryNode,"id",&countryID,sizeof(countryID));
					break;
				}
				countryNode = parser.getNextNode(countryNode,"country");
			}
			if (!id)
			{
				ScenesService::gmlogger->info("GM %s 使用createrush指令,未找到国家 country=%s",pUser->name,prop["country"].c_str());
				return false;
			}
		}
		else
		{
			ScenesService::gmlogger->info("GM %s 使用createrush指令,找不到文件 scenesinfo.xml",pUser->name);
			return false;
		}
	}
	else
	{
		countryID = pUser->scene->getCountryID();
	}

	Rush * rush = new Rush(id,delay,countryID);
	if (rush)
	{
		if (rush->init())
		{
			ScenesService::gmlogger->info("GM %s 使用createrush命令,在 %s 引发怪物攻城",pUser->name,rush->mapName);
			return true;
		}

		SAFE_DELETE(rush);
		ScenesService::gmlogger->info("GM %s 使用createrush命令,初始化攻城数据失败 para=%s",pUser->name,para);
		return false;
	}

	ScenesService::gmlogger->info("GM %s 使用createrush命令,引发攻城失败 para=%s",pUser->name,para);
	return false;
}

/**
* \brief 攻城
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::createQuiz(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	Cmd::Session::t_createQuiz_SceneSession send;

	send.active_time  = atoi(prop["subjects"].c_str());
	send.ready_time   = atoi(prop["ready_time"].c_str());
	send.subject_type    = atoi(prop["type"].c_str());
	send.type = 0;
	sessionClient->sendCmd(&send,sizeof(send));

	ScenesService::gmlogger->info("GM %s 使用createquiz命令,引发全区智力竞赛 para=%s",pUser->name,para);
	return true;
}

/**
* \brief 清除一个地图上某个区域的npc
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::clearWorld(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	int region = 0;
	if ("" == prop["region"])
		region = 5;
	else
		region = atoi(prop["region"].c_str());

	int side = 0;
	int direct = 0;
	int clockwise = 1;
	int clearCount = 0;
	int count = 0;
	zPos pos;
	pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,pos);
	do
	{
		SceneNpc * sceneNpc = pUser->scene->getSceneNpcByPos(pos);
		if (sceneNpc)
		{
			switch(sceneNpc->npc->kind)
			{
			case NPC_TYPE_NORMAL:
			case NPC_TYPE_BBOSS:
			case NPC_TYPE_LBOSS:
			case NPC_TYPE_PBOSS:
			case NPC_TYPE_BACKBONE:
			case NPC_TYPE_GOLD:
			case NPC_TYPE_SUMMONS:
			case NPC_TYPE_AGGRANDIZEMENT:
			case NPC_TYPE_ABERRANCE:
			case NPC_TYPE_BACKBONEBUG:
			case NPC_TYPE_GUARD:
			case NPC_TYPE_PET:
			case NPC_TYPE_TOTEM:
			case NPC_TYPE_HUMAN:
			case NPC_TYPE_WILDHORSE:
			case NPC_TYPE_DUCKHIT:
			case NPC_TYPE_BANNER:
			case NPC_TYPE_TRAP:
			case NPC_TYPE_UNIONGUARD:
			case NPC_TYPE_SOLDIER:
			case NPC_TYPE_TASK:
			case NPC_TYPE_TRADE:
			case NPC_TYPE_GHOST:
			case NPC_TYPE_TURRET:
			case NPC_TYPE_BARRACKS:
			case NPC_TYPE_CAMP:
			case NPC_TYPE_ANIMON:
				{
					if (!(sceneNpc->getMaster()==pUser))
					{
						sceneNpc->toDie(pUser->tempid);
						clearCount++;
					}
				}
				break;
			default:
				break;
			}
		}
		if (++count>=(region*2+1)*(region)*2+1)
			break;
	} while(pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,pos) && side <= region);
	ScenesService::gmlogger->info("GM %s 使用 clearworld 命令,杀死了半径 %d 范围内的 %d 只NPC",pUser->name,region,clearCount);
	return true;
}

/**
* \brief 清除一个地图上某个区域的npc
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::clearArea(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	int region = 0;
	if ("" == prop["region"])
		region = 5;
	else
		region = atoi(prop["region"].c_str());

	int side = 0;
	int direct = 0;
	int clockwise = 1;
	int clearCount = 0;
	int count = 0;
	zPos pos;
	pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,pos);
	do
	{
		SceneUser * sceneUser = pUser->scene->getSceneUserByPos(pos);
		if (sceneUser)
		{
			if (sceneUser!=pUser)
			{
				sceneUser->goToRandomRect(sceneUser->getPos(),SCREEN_WIDTH*2+region,SCREEN_HEIGHT*2+region);
				sceneUser->catchMyPet();
				clearCount++;
			}
		}
		if (++count>=(region*2+1)*(region)*2+1)
			break;
	} while(pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,pos) && side <= region);
	ScenesService::gmlogger->info("GM %s 使用 cleararea 命令,驱散了%s(%u,%u)周围的玩家",pUser->name,pUser->scene->name,pUser->getPos().x,pUser->getPos().y);
	return true;
}

/**
* \brief 清除帮会与家族数据
*
*
* \param pUser 指令使用者
* \param para 参数
* \return 指令执行是否成功
*/
bool Gm::clearSeptUnion(SceneUser *pUser,const char *para)
{
	/*  Cmd::Session::t_ClearRelation_SceneSession send;
	send.dwUserID = pUser->id;

	sessionClient->sendCmd(&send,sizeof(send));
	*/

	Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"该命令已禁用");
	return false;
}

/**
* \brief 创建一个家族
*
*
* \param pUser 指令使用者
* \param para 参数
* \return 指令执行是否成功
*/
bool Gm::createFamily(SceneUser *pUser,const char *para)
{
	if ((!pUser)||(!para)) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,参照//createfamily name=我的家族");
		ScenesService::gmlogger->info("GM %s 使用createfamily命令,参数错误 para=%s",pUser->name,para);
		return false;
	}

	Cmd::Session::t_addSept_SceneSession send;

	send.dwMapTempID = pUser->scene->tempid;
	send.info.dwUnionID = 0;
	send.info.dwRepute = 0;
	send.info.dwSeptID = 0;             // 家族的ID
	strncpy(send.info.name,prop["name"].c_str(),MAX_NAMESIZE-1);   // 家族名称
	send.info.dwCharID = pUser->charbase.id;      // 族长的角色ID
	send.info.dwCountryID = pUser->charbase.country;    // 家族所属国家
	send.info.byVote = 0;            // 默认进入投票期间
	strncpy(send.info.masterName,pUser->name,MAX_NAMESIZE-1);  // 会长的名字
	send.info.dwSpendGold = 0;    // 家族花费金币
	send.info.dwLevel = 1;    // 家族等级
	bzero(send.info.note,sizeof(send.info.note));
	sessionClient->sendCmd(&send,sizeof(send));
	return true;
}

/**
* \brief 创建一个帮会
*
*
* \param pUser 指令使用者
* \param para 参数
* \return 指令执行是否成功
*/
bool Gm::createUnion(SceneUser *pUser,const char *para)
{

	Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"该命令已禁用");
	/*
	if ((!pUser)||(!para)) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
	Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,参照//createunion name=我的帮会");
	ScenesService::gmlogger->info("GM %s 使用createunion命令,参数错误 para=%s",pUser->name,para);
	return false;
	}

	Cmd::Session::t_addUnion_SceneSession send;

	send.dwMapTempID = pUser->scene->tempid;
	send.dwItemID    = 0;
	send.info.dwUnionID = 0;          // 帮会的ID
	strncpy(send.info.name,prop["name"].c_str(),MAX_NAMESIZE-1);    // 帮会名称
	send.info.dwCharID = pUser->charbase.id;                        // 会长的角色ID
	send.info.dwCountryID = pUser->charbase.country;
	strncpy(send.info.masterName,pUser->name,MAX_NAMESIZE-1);  // 会长的名字
	send.info.wdLevel = 1;                                // 帮会级别
	send.info.qwExp = 0;                                    // 帮会经验
	send.info.byVote = 0; //帮会投票标志
	sessionClient->sendCmd(&send,sizeof(send));
	*/
	return false;
}

/**
* \brief 创建一个门派
*
*
* \param pUser 指令使用者
* \param para 参数
* \return 指令执行是否成功
*/
bool Gm::createSchool(SceneUser *pUser,const char *para)
{
	if ((!pUser)||(!para)) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,参照//createunion name=我的帮会");
		ScenesService::gmlogger->info("GM %s 使用createunion命令,参数错误 para=%s",pUser->name,para);
		return false;
	}

	Cmd::Session::t_createSchool_SceneSession cmd;
	strncpy((char *)cmd.masterName,pUser->name,MAX_NAMESIZE-1);
	strncpy((char *)cmd.schoolName,prop["name"].c_str(),MAX_NAMESIZE-1);//要创建的门派名字

	sessionClient->sendCmd(&cmd,sizeof(cmd));
	return true;
}

/**
* \brief 查看国家对战信息
*
*
* \param pUser 指令使用者
* \param para 参数
* \return 指令执行是否成功
*/
bool Gm::viewCountryDare(SceneUser *pUser,const char *para)
{
	if ((!pUser)||(!para)) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	Cmd::Session::t_viewCountryDare_SceneSession send;
	if ("" == prop["type"])
		send.byType = 3;
	else
		send.byType = atoi(prop["type"].c_str());

	send.dwUserID = pUser->id;

	sessionClient->sendCmd(&send,sizeof(send));
	return true;
}

/**
* \brief 查看周围的npc和玩家
*
* \param pUser 指令使用者
* \param para 参数
* radius 查找半径
* type 查找的类型 1:玩家 2:npc 0:全部
* \return 是否执行成功
*/
bool Gm::showEntries(SceneUser *pUser,const char *para)
{
	if ((!pUser)||(!para)) return false;
	zProperties prop;
	prop.parseCmdLine(para);

	int radius = atoi(prop["radius"].c_str());
	if (0==radius) radius=2;
	int type = atoi(prop["type"].c_str());

	int side = 0;
	int direct = 0;
	int clockwise = 1;
	int count = 0;
	int playerCount = 0,npcCount = 0;
	zPos curPos(0,0);
	pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,curPos);
	do
	{
		SceneEntryPk *entry;
		char str[MAX_CHATINFO];
		std::string state;
		if ((0==type)||(1==type))
		{
			entry = pUser->scene->getSceneUserByPos(curPos);
			if (entry)
			{
				switch (entry->getState())
				{
				case zSceneEntry::SceneEntry_Normal:
					state = "普通";
					break;
				case zSceneEntry::SceneEntry_Death:
					state = "死亡";
					break;
				case zSceneEntry::SceneEntry_Hide:
					state = "隐身";
					break;
				default:
					state = "未知";
					break;
				}
				playerCount++;
				_snprintf(str,MAX_CHATINFO-1,"(%u,%u) 玩家 %s %s",curPos.x,curPos.y,entry->name,state.c_str());
				Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,str);
			}
		}

		if ((0==type)||(2==type))
		{
			entry = pUser->scene->getSceneNpcByPos(curPos);
			if (entry)
			{
				switch (entry->getState())
				{
				case zSceneEntry::SceneEntry_Normal:
					state = "普通";
					break;
				case zSceneEntry::SceneEntry_Death:
					state = "死亡";
					break;
				case zSceneEntry::SceneEntry_Hide:
					state = "隐身";
					break;
				default:
					state = "未知";
					break;
				}
				npcCount++;
				_snprintf(str,MAX_CHATINFO-1,"(%u,%u) npc %s %s",curPos.x,curPos.y,entry->name,state.c_str());
				Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,str);
			}
		}
		if (++count>=(radius*2+1)*(radius*2+1)) break;
	} while(pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,curPos) && side <= radius);

	if (playerCount||npcCount)
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"%d 个玩家 %d 个npc",playerCount,npcCount);

	ScenesService::gmlogger->info("GM %s 使用showentries命令(%u,%u) para=\"%s\"",pUser->name,pUser->getPos().x,pUser->getPos().y,para);

	return true;
	//Channel::sendNine(this,"?? %d ???",entries.size());
	//        return !entries.empty();
}


/**
* \brief 设置宠物行为模式
*
*
* \param pUser: 用户
* \param para:内容
* \return 是否设置成功
*/
bool Gm::setPetAI(SceneUser *pUser,const char *para)
{
	if ((!pUser)||(!para)) return false;
	zProperties prop;
	prop.parseCmdLine(para);

	int t;
	Cmd::petAIMode m=Cmd::PETAI_MOVE_STAND;
	t = atoi(prop["type"].c_str());
	if ("follow"==prop["mode"]) m = Cmd::PETAI_MOVE_FOLLOW;
	if ("stand"==prop["mode"]) m = Cmd::PETAI_MOVE_STAND;
	if ("active"==prop["mode"]) m = Cmd::PETAI_ATK_ACTIVE;
	if ("passive"==prop["mode"]) m = Cmd::PETAI_ATK_PASSIVE;
	if ("noatk"==prop["mode"]) m = Cmd::PETAI_ATK_NOATK;

	if (2==t) if (pUser->pet) pUser->pet->setPetAI(m);
	if (3==t) if (pUser->summon) pUser->summon->setPetAI(m);
	if (4==t)
	{
		for (std::list<ScenePet *>::iterator it=pUser->totems.begin(); it!=pUser->totems.end(); it++)
			(*it)->setPetAI(m);
	}
	ScenesService::gmlogger->info("GM %s 使用setpetai指令设置宠物行动方式 type=%d ai=%s",pUser->name,t,prop["mode"].c_str());
	return true;
}

/**
* \brief 设置宠物AIF
*
*
* \param pUser: 用户
* \param para:内容
* \return 是否设置成功
*/
bool Gm::setPetAIF(SceneUser *pUser,const char *para)
{
	if ((!pUser)||(!para)) return false;
	zProperties prop;
	prop.parseCmdLine(para);

	int t = atoi(prop["aif"].c_str());

	if (pUser->pet) pUser->pet->aif = t;
	if (pUser->summon) pUser->summon->aif = t;
	return true;
}

bool Gm::bank(SceneUser* pUser,const char* para)
{
	if (!pUser || !para) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	BYTE page = atoi(prop["page"].c_str());

	if (page >= pUser->packs.store.days) {
		pUser->packs.store.days++;
	}

	pUser->packs.store.notify(pUser);
	return true;
}


bool Gm::reloadScript(SceneUser* pUser,const char* para)
{
	ScriptingSystemLua::instance().reloadVM();
	scriptMessageFilter::reinitFilter();
	return true;
}

/**
* \brief 增加经验值
* 最多加10w
*
* \param pUser 使用者
* \param para 参数
* \return 是否执行成功
*/
bool Gm::addExp(SceneUser* pUser,const char* para)
{
	if ((!pUser)||(!para)) return false;
	zProperties prop;
	prop.parseCmdLine(para);

	SceneUser * target = SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (!target) target = pUser;
	int num = atoi(prop["num"].c_str());
	if (num<=0) return false;
	if (num>1000000000) num = 1000000000;
	target->addExp(num,true,0,0,true);
	return true;
}

/**
* \brief 查询用户帐号
* 
*
* \param pUser 使用者
* \param para 参数
* \return 是否执行成功
*/
bool Gm::qAccount(SceneUser* pUser,const char* para)
{
	if ((!pUser)||(!para)) return false;
	zProperties prop;
	prop.parseCmdLine(para);

	SceneUser * target = SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (!target) target = pUser;

	Cmd::Scene::t_Query_AccountScene send;
	send.dwUserID = pUser->id;
	send.dwDestID = target->id;

	pUser->gatetask->sendCmd(&send,sizeof(send));

	return true;
}

/**
* \brief 调整线程sleep的时间
* 
*
* \param pUser 超级GM
* \param para 参数
* \return 是否执行成功
*/
bool Gm::usleep(SceneUser* pUser,const char* para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	int uscene=atoi(prop["scene"].c_str());
	int ugate=atoi(prop["gate"].c_str());
	int uclient=atoi(prop["client"].c_str());
	if (uscene)
	{
		zTCPTaskPool::setUsleepTime(uscene);
		Zebra::logger->info("调整场景服务器usleep时间:%dus",uscene);
	}
	Cmd::Scene::t_Usleep_GateScene ret;
	if (ugate)
	{
		ret.utask=ugate;
	}
	if (uclient)
	{
		ret.uclient=uclient;
	}
	if (ugate || uclient)
	{
		if (strncmp("true",prop["all"].c_str(),MAX_NAMESIZE)==0)
		{
			SceneTaskManager::getInstance().broadcastCmd(&ret,sizeof(ret));
			if (ugate)
				Zebra::logger->info("调整所有网关服务器usleep时间:%dus",ugate);
			if (uclient)
				Zebra::logger->info("调整所有网关客户端usleep时间:%dus",uclient);
		}
		else
		{
			pUser->gatetask->sendCmd(&ret,sizeof(ret));
			if (ugate)
				Zebra::logger->info("调整所有网关服务器usleep时间:%dus",ugate);
			if (uclient)
				Zebra::logger->info("调整所有网关客户端usleep时间:%dus",uclient);
		}
	}
	return true;
}
/**
* \brief 得到系统当前时间
* 
*
* \param pUser 超级GM
* \param para 参数
* \return 是否执行成功
*/
bool Gm::systime(SceneUser* pUser,const char* para)
{
	struct tm tm_1;
	time_t timValue = time(NULL);
	zRTime::getLocalTime(tm_1,timValue);
	//tm_1=*localtime(&timValue);
	Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"系统当前时间%d年%d月%d日%d时%d分%d秒",tm_1.tm_year+1900,tm_1.tm_mon+1,tm_1.tm_mday,tm_1.tm_hour,tm_1.tm_min,tm_1.tm_sec);
	return true;
}
/**
* \brief 停机维护
* 
*
* \param pUser 超级GM
* \param para 参数
* \return 是否执行成功
*/
bool Gm::shutdown(SceneUser* pUser,const char* para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	Cmd::Session::t_shutdown_SceneSession send;
	struct tm tm_1;
	time_t timValue = time(NULL);
	tm_1=*localtime(&timValue);
	//zRTime::getLocalTime(tm_1,timValue);
	//Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"系统当前时间%d年%d月%d日%d时%d分%d秒",tm_1.tm_year+1900,tm_1.tm_mon+1,tm_1.tm_mday,tm_1.tm_hour,tm_1.tm_min,tm_1.tm_sec);
	tm_1.tm_year=atoi(prop["year"].c_str())!=0?atoi(prop["year"].c_str()):tm_1.tm_year;
	tm_1.tm_mon=atoi(prop["month"].c_str())!=0?atoi(prop["month"].c_str()):tm_1.tm_mon;
	tm_1.tm_mday=atoi(prop["day"].c_str())!=0?atoi(prop["day"].c_str()):tm_1.tm_mday;
	tm_1.tm_hour=atoi(prop["hour"].c_str())!=0?atoi(prop["hour"].c_str()):tm_1.tm_hour;
	tm_1.tm_min=atoi(prop["minute"].c_str());
	tm_1.tm_sec=atoi(prop["second"].c_str());
	send.time=mktime(&tm_1);
	if (send.time<=timValue)
	{
		send.time=0;
		Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"取消停机维护");
	}
	strncpy((char *)send.info,prop["info"].c_str(),MAX_CHATINFO-1);
	sessionClient->sendCmd(&send,sizeof(send));
	//Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"系统将于%d年%d月%d日%d时%d分%d秒停机维护",tm_1.tm_year+1900,tm_1.tm_mon+1,tm_1.tm_mday,tm_1.tm_hour,tm_1.tm_min,tm_1.tm_sec);
	return true;
}


bool Gm::setQuest(SceneUser* pUser,const char* para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	//  zRegex regex;

	std::string var_name  = prop["var"];
	std::string value = prop["value"];
	int id = atoi(prop["id"].c_str());
	int type = atoi(prop["type"].c_str());

	std::string info = "Error Type!";
	Vars* vars = NULL;
	if (1)
	{
		switch (type) 
		{
		case 0:
			info = pUser->quest_list.info();
			vars = pUser->quest_list.vars(id);
			break;
		case 1:
			info = GlobalVar::instance().show();
			vars = GlobalVar::instance().add(id);
			break;
		case 2:
			info = TongVar::instance().show();
			vars = TongVar::instance().add(id);
			break;
		case 3:
			info = FamilyVar::instance().show();
			vars = FamilyVar::instance().add(id);
			break;
		case 4:
			info = UsersVar::instance().show();
			vars = UsersVar::instance().add(id);
			break;
		}    
		if (var_name.empty() || value.empty()) {
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,info.c_str());
			return false;
		}
	}


	bool new_vars = false;
	if (!vars) {
		vars = new Vars;
		new_vars = true;
	}

	Op::Set<int> op;
	vars->set_value(op,var_name,atoi(prop["value"].c_str()),0,pUser);
	if (type == 0) {
		vars->notify(*pUser,var_name);
		pUser->quest_list.add_quest(id,*vars,*pUser,false);
		pUser->quest_list.update(*pUser,true);
	}

	if (new_vars) SAFE_DELETE(vars);

	return true;
}

bool Gm::setBlock(SceneUser* pUser,const char* para)
{
	int radius = 2;

	int side = 0;
	int direct = 0;
	int clockwise = 1;
	int count = 0;
	zPos curPos(0,0);
	pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,curPos);
	do
	{
		pUser->scene->setBlock(curPos);
		if (++count>=(radius*2+1)*(radius*2+1)) break;
	} while(pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,curPos) && side <= radius);
	ScenesService::gmlogger->debug("GM %s 使用clearblock命令,设置阻挡",pUser->name);

	return true;
}

bool Gm::checkBlock(SceneUser* pUser,const char* para)
{
	int radius = 2;

	int side = 0;
	int direct = 0;
	int clockwise = 1;
	int count = 0;
	int blockCount = 0;
	zPos curPos(0,0);
	pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,curPos);
	do
	{
		if (pUser->scene->checkBlock(curPos))
			blockCount++;
		if (++count>=(radius*2+1)*(radius*2+1)) break;
	} while(pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,curPos) && side <= radius);
	Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"周围2格内有 %d 个阻挡点",blockCount);

	return true;
}

bool Gm::npcDare(SceneUser *pUser,const char * para)
{
	Cmd::Session::t_NpcDare_GMCmd_SceneSession send;
	sessionClient->sendCmd(&send,sizeof(send));
	ScenesService::gmlogger->debug("GM %s 使用npcdare命令,主动触发NPC争夺战",pUser->name);
	return true;
}

bool Gm::clearPoint(SceneUser *pUser,const char * para)
{
	if (pUser)
	{
		pUser->charbase.points =pUser->charbase.points
			+pUser->charbase.wdProperty[0]
		+pUser->charbase.wdProperty[1]
		+pUser->charbase.wdProperty[2]
		+pUser->charbase.wdProperty[3]
		+pUser->charbase.wdProperty[4];
		pUser->charbase.wdProperty[0]=0;
		pUser->charbase.wdProperty[1]=0;
		pUser->charbase.wdProperty[2]=0;
		pUser->charbase.wdProperty[3]=0;
		pUser->charbase.wdProperty[4]=0;
		pUser->setupCharBase();
		//刷新用户数据
		Cmd::stMainUserDataUserCmd  userinfo;
		pUser->full_t_MainUserData(userinfo.data);
		pUser->sendCmdToMe(&userinfo,sizeof(userinfo));
		ScenesService::gmlogger->debug("GM %s 使用clearpoint清自己的人物属性点",pUser->name);
	}
	return true;
}

bool Gm::clearSkillPoint(SceneUser *pUser,const char * para)
{
	if (pUser)
	{
		pUser->charbase.skillpoint = pUser->charbase.level;
		pUser->usm.clear();
		Cmd::stClearSkillUserCmd send;
		pUser->sendCmdToMe(&send,sizeof(send));
		pUser->skillValue.init();
		pUser->setupCharBase();
		//刷新用户数据
		Cmd::stMainUserDataUserCmd  userinfo;
		pUser->full_t_MainUserData(userinfo.data);
		pUser->sendCmdToMe(&userinfo,sizeof(userinfo));
		ScenesService::gmlogger->debug("GM %s 使用clearskillpoint清自己的技能点",pUser->name);
	}
	return true;
}

bool Gm::clearSkill(SceneUser *pUser,const char * para)
{
	if (pUser)
	{
		zProperties prop;
		prop.parseCmdLine(para);
		if (prop["id"]!="")
		{
			DWORD skillid = atoi(prop["id"].c_str());

			pUser->usm.clearskill(skillid);
			Cmd::stClearSkillUserCmd send;
			pUser->sendCmdToMe(&send,sizeof(send));
			pUser->usm.refresh();
			pUser->skillValue.init();
			pUser->setupCharBase();
			//刷新用户数据
			Cmd::stMainUserDataUserCmd  userinfo;
			pUser->full_t_MainUserData(userinfo.data);
			pUser->sendCmdToMe(&userinfo,sizeof(userinfo));
			ScenesService::gmlogger->debug("GM %s 使用clearskill清自己的指定技能",pUser->name);
		}
	}
	return true;
}


bool Gm::studySkill(SceneUser *pUser,const char * para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if (pUser)
	{
		std::string skill_name  = prop["name"];
		if (!skill_name.empty())
		{
			zSkillB * skillb = skillbm.get(skill_name.c_str());
			if (skillb)
			{
				pUser->upgradeSkill(skillb->skillid);
			}
		}
	}
	return true;
}

bool Gm::queryincmap(SceneUser *pUser,const char * para)
{
	zObject *obj = pUser->packs.equip.getObjectByEquipPos(Cmd::EQUIPCELLTYPE_ADORN);
	if (obj && obj->base->kind == ItemType_Amulet)
	{
		char buf[1024];
		bzero(buf,sizeof(buf));
		Cmd::stScrollMapPropertyUserCmd *smp = (Cmd::stScrollMapPropertyUserCmd*)buf;
		smp->qwThisID = obj->data.qwThisID;
		constructInPlace(smp);
		smp->size = pUser->scene->getIncCity((const char*)smp->mapname);
		pUser->sendCmdToMe(smp,sizeof(Cmd::stScrollMapPropertyUserCmd) + MAX_NAMESIZE * smp->size);        
	}
	return true;
}
bool Gm::clearColddown(SceneUser *pUser,const char * para)
{
	if (pUser)
	{
		pUser->reSendOther=true;
		pUser->usm.clearAllUseTime();
	}
	return true;
}

bool Gm::debugCityDare(SceneUser *pUser,const char * para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	Cmd::Session::t_GMCreateUnionCity_SceneSession send;
	send.byOpType = atoi(prop["state"].c_str());
	send.dwCityID = pUser->scene->getRealMapID();
	send.dwCountryID = pUser->charbase.country;
	sessionClient->sendCmd(&send,sizeof(send));

	return true;
}

bool Gm::debugEmperorDare(SceneUser *pUser,const char * para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	Cmd::Session::t_beginEmperorDare_SceneSession send;
	sessionClient->sendCmd(&send,sizeof(send));

	return true;
}

bool Gm::changeCountry(SceneUser *pUser,const char * para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	is_cancel_country = atoi(prop["state"].c_str());
	cancel_country_need_money = atoi(prop["money"].c_str());

	return true;
}

bool Gm::setSeptLevel(SceneUser *pUser,const char * para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	Cmd::Session::t_OpLevel_SceneSession send;

	send.dwLevel = atoi(prop["value"].c_str());
	send.dwSeptID = pUser->charbase.septid;
	sessionClient->sendCmd(&send,sizeof(send));

	return true;
}

bool Gm::setAllyFriendDegree(SceneUser *pUser,const char * para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	Cmd::Session::t_opAllyFrienddegree_SceneSession send;

	send.friendDegree = atoi(prop["value"].c_str());
	send.dwCountryID = pUser->charbase.country;
	sessionClient->sendCmd(&send,sizeof(send));

	ScenesService::gmlogger->info("GM %s 使用setallyfd命令,设置 %s 国的联盟友好度",
		pUser->name,prop["value"].c_str());                                    

	return true;
}

bool Gm::setRepute(SceneUser *pUser,const char * para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	Cmd::Session::t_OpRepute_SceneSession send;
	send.dwRepute = atoi(prop["value"].c_str());
	send.dwSeptID = pUser->charbase.septid;
	sessionClient->sendCmd(&send,sizeof(send));

	return true;
}

bool Gm::debugVote(SceneUser *pUser,const char * para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	Cmd::Session::t_opTechVote_SceneSession send;
	send.byOpType = atoi(prop["state"].c_str());
	send.dwCountryID = pUser->charbase.country;
	sessionClient->sendCmd(&send,sizeof(send));

	return true;
}

bool Gm::debugGem(SceneUser *pUser,const char * para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	Cmd::Session::t_OpGemState_SceneSession send;
	send.dwState = atoi(prop["state"].c_str());
	sessionClient->sendCmd(&send,sizeof(send));

	return true;
}

bool Gm::addSeptExp(SceneUser *pUser,const char * para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	Cmd::Session::t_opTechVote_SceneSession send;
	sessionClient->sendCmd(&send,sizeof(send));

	return true;
}

bool Gm::checkQuest(SceneUser *pUser,const char * para)
{
	if (pUser)
		EventManager<OnVisit>::instance().get_valid_quest_str(*pUser);
	return true;
}

/*
* \brief 设置游戏功能的开关
* \param pUser 指令使用者
* \param para 参数
*
* \return 执行结果
*
*/
bool Gm::setService(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;
	using namespace Cmd::Session;

	zProperties prop;
	if (0==prop.parseCmdLine(para))
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"当前服务状态 flag=%s",Zebra::global["service_flag"].c_str());
		return true;
	}

	DWORD f = atoi(Zebra::global["service_flag"].c_str());
	if (prop["mail"]=="on")
		f |= SERVICE_MAIL;
	else if (prop["mail"]=="off")
		f &= ~SERVICE_MAIL;

	if (prop["auction"]=="on")
		f |= SERVICE_AUCTION;
	else if (prop["auction"]=="off")
		f &= ~SERVICE_AUCTION;

	if (prop["process"]=="on")
		f |= SERVICE_PROCESS;
	else if (prop["process"]=="off")
		f &= ~SERVICE_PROCESS;

	if (prop["horse"]=="on")
		f |= SERVICE_HORSE;
	else if (prop["horse"]=="off")
		f &= ~SERVICE_HORSE;

	if (prop["flag"]!="")
		f = atoi(prop["flag"].c_str());

	/*
	char buf[32];
	bzero(buf,sizeof(buf));
	_snprintf(buf,32,"%u",f);
	Zebra::global["service_flag"] = buf;
	*/

	Cmd::Session::t_SetService_SceneSession send;
	send.flag = f;
	sessionClient->sendCmd(&send,sizeof(send));
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"当前服务状态 flag=%u",f);
	ScenesService::gmlogger->info("GM %s 设置服务 flag=%u",pUser->name,f);

	return true;
}

/*
* \brief 设置游戏功能的开关
* \param pUser 指令使用者
* \param para 参数
*
* \return 执行结果
*
*/
bool Gm::setTire(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	if (prop["oper"]=="clear")
	{
		if (prop["hour"] != "")
		{
			int tm_hour = atoi(prop["hour"].c_str());
			if (tm_hour<0 || tm_hour>23) return true;
			for(int i = 0; i < 60; i++)
			{
				int j = (tm_hour*60+i)/5;
				Cmd::clear_state(((BYTE*)(pUser->charbase.tiretime)),j);
			}
		}
		else
		{
			bzero(pUser->charbase.tiretime,36);
			pUser->wdTire = 0;
		}
	}
	else if (prop["oper"]=="set")
	{
		if (prop["hour"] != "")
		{
			int tm_hour = atoi(prop["hour"].c_str());
			if (tm_hour<0 || tm_hour>23) return false;
			for(int i = 0; i < 60; i++)
			{
				int j = (tm_hour*60+i)/5;
				Cmd::set_state(((BYTE*)(pUser->charbase.tiretime)),j);
			}
		}
		else
		{
			for(int x = 0; x < 24; x++)
				for(int i = 0; i < 60; i++)
				{
					int j = (x*60+i)/5;
					Cmd::set_state(((BYTE*)(pUser->charbase.tiretime)),j);
				}
		}
	}
	int count=0;
	for(int k=0; k<288;k++)
	{
		if (Cmd::isset_state(((BYTE*)(pUser->charbase.tiretime)),k))
		{
			count++;
		}
	}
	pUser->wdTire=count;
	pUser->callProcessTire();
	return true;
}

/*
* \brief 设置开启/关闭某国家注册
* \param pUser 指令使用者
* \param para 参数
*
* \return 执行结果
*
*/
bool Gm::enableRegister(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	if (prop["countryid"]!="")
	{
		WORD wdCountryID = atoi(prop["countryid"].c_str());
		if (prop["status"]=="true")
		{
			Cmd::Scene::t_ChangeCountryStatus send;
			send.oper = Cmd::Scene::ENABLE_REGISTER;
			send.country = wdCountryID;
			send.dwUserID = pUser->id;
			SceneTaskManager::getInstance().broadcastCmd(&send,sizeof(send));
			ScenesService::gmlogger->info("GM %s 设置了国家%u注册允许",pUser->name,wdCountryID);
		}
		else if (prop["status"]=="false")
		{
			Cmd::Scene::t_ChangeCountryStatus send;
			send.oper = Cmd::Scene::DISABLE_REGISTER;
			send.country = wdCountryID;
			send.dwUserID = pUser->id;
			SceneTaskManager::getInstance().broadcastCmd(&send,sizeof(send));
			ScenesService::gmlogger->info("GM %s 设置了国家%u注册关闭",pUser->name,wdCountryID);
		}
	}
	return true;
}

/*
* \brief 设置开启/关闭某国家注册
* \param pUser 指令使用者
* \param para 参数
*
* \return 执行结果
*
*/
bool Gm::enableLogin(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	if (prop["countryid"]!="")
	{
		WORD wdCountryID = atoi(prop["countryid"].c_str());
		if (prop["status"]=="true")
		{
			Cmd::Scene::t_ChangeCountryStatus send;
			send.oper = Cmd::Scene::ENABLE_LOGIN;
			send.country = wdCountryID;
			send.dwUserID = pUser->id;
			SceneTaskManager::getInstance().broadcastCmd(&send,sizeof(send));
			ScenesService::gmlogger->info("GM %s 设置了国家%u登陆允许",pUser->name,wdCountryID);
		}
		else if (prop["status"]=="false")
		{
			Cmd::Scene::t_ChangeCountryStatus send;
			send.oper = Cmd::Scene::DISABLE_LOGIN;
			send.country = wdCountryID;
			send.dwUserID = pUser->id;
			SceneTaskManager::getInstance().broadcastCmd(&send,sizeof(send));
			ScenesService::gmlogger->info("GM %s 设置了国家%u登陆关闭",pUser->name,wdCountryID);
		}
	}
	return true;
}

/*
* \brief 设置/显示 服务器版本号
* \param pUser 指令使用者
* \param para 参数
*
* \return 执行结果
*
*/
bool Gm::version(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	if (prop["oper"]=="set")
	{
		if (prop["id"]!="")
		{
			DWORD wdVersionID = atoi(prop["id"].c_str());
			Cmd::Scene::t_ChangeVerifyVersion send;
			send.oper = Cmd::Scene::CHANGECODE;
			send.versionCode = wdVersionID;
			send.dwUserID = pUser->id;
			SceneTaskManager::getInstance().broadcastCmd(&send,sizeof(send));
			ScenesService::gmlogger->info("GM %s 设置服务器版本校验号为%u",pUser->name,wdVersionID);
		}
	}
	else if (prop["oper"]=="show")
	{
		Cmd::Scene::t_ChangeVerifyVersion send;
		send.oper = Cmd::Scene::SHOWCODE;
		send.versionCode = 0;
		send.dwUserID = pUser->id;
		SceneTaskManager::getInstance().broadcastCmd(&send,sizeof(send));
		ScenesService::gmlogger->info("GM %s 查看版本校验号",pUser->name);
	}
	return true;
}

/*
* \brief 显示国家注册/登陆允许状态列表
* \param pUser 指令使用者
* \param para 参数
*
* \return 执行结果
*
*/
bool Gm::checkCountryInfo(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	Cmd::Scene::t_ChangeCountryStatus send;
	send.oper = Cmd::Scene::LIST_COUNTRY_INFO;
	send.dwUserID = pUser->id;
	pUser->gatetask->sendCmd(&send,sizeof(send));
	return true;
}

bool Gm::showAddExp(SceneUser *pUser,const char * para)
{
	if (!pUser || !para) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	DWORD id = atoi(prop["id"].c_str());

	if (pUser->cartoon && pUser->cartoon->getCartoonID()==id)
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"npc: %u",pUser->cartoon->getCartoonData().addExp);
	if (pUser->cartoonList.find(id)!=pUser->cartoonList.end())
		Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"cartoonList: %u",pUser->cartoonList[id].addExp);
	return true;
}

bool Gm::changeHorse(SceneUser *pUser,const char * para)
{
	if (!pUser || !para) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	DWORD id = atoi(prop["id"].c_str());

	pUser->horse.horse(id);
	ScenesService::gmlogger->info("GM %s 用changehorse指令换马 id=%u",pUser->name,id);
	return true;
}

bool Gm::loadProcess(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	Cmd::Session::t_gmCommand_SceneSession send;
	strncpy((char *)send.src_name,pUser->name,MAX_NAMESIZE);
	send.gm_cmd = Cmd::Session::GM_COMMAND_LOAD_PROCESS;
	sessionClient->sendCmd(&send,sizeof(send));

	ScenesService::gmlogger->info("GM %s 使用loadProcess指令",pUser->name);
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"加载process文件,请查看各场景日志");
	return true;
}

bool Gm::loadGift(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	Cmd::Session::t_gmCommand_SceneSession send;
	strncpy((char *)send.src_name,pUser->name,MAX_NAMESIZE);
	send.gm_cmd = Cmd::Session::GM_COMMAND_LOAD_GIFT;
	sessionClient->sendCmd(&send,sizeof(send));

	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"加载礼官数据,请查看session日志");
	return true;
}

bool Gm::contribute(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	Cmd::Session::t_ContributeCountry_SceneSession send;
	send.dwCountry = atoi(prop["country"].c_str())?atoi(prop["country"].c_str()):2;
	send.dwValue = atoi(prop["num"].c_str())?atoi(prop["num"].c_str()):1000;
	send.byType = atoi(prop["type"].c_str())?atoi(prop["type"].c_str()):1;
	sessionClient->sendCmd(&send,sizeof(send));

	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"贡献,country=%u type=%u num=%u",send.dwCountry,send.byType,send.dwValue);
	return true;
}

bool Gm::refreshGeneral(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	zProperties prop;
	prop.parseCmdLine(para);

	Cmd::Session::t_gmCommand_SceneSession send;
	send.gm_cmd = Cmd::Session::GM_COMMAND_REFRESH_GENERAL;
	sessionClient->sendCmd(&send,sizeof(send));

	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"刷新大将军,请查看场景或session的日志");
	return true;
}

bool Gm::closenpc(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	Cmd::Session::t_CloseNpc_SceneSession send;
	sessionClient->sendCmd(&send,sizeof(send));

	ScenesService::gmlogger->info("GM %s 使用closenpc指令",pUser->name);
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"关闭功能NPC准备停机");
	return true;
}

bool Gm::countrypower(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;

	Cmd::Session::t_DebugCountryPower_SceneSession send;
	sessionClient->sendCmd(&send,sizeof(send));

	ScenesService::gmlogger->info("GM %s 使用countrypower指令",pUser->name);
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"立即处理国家强弱计算");
	return true;
}
#endif

void Gm::sendLog(SceneUser *pUser,const char *cmd,const char * content)
{
	using namespace Cmd::GmTool;
	t_Log_GmTool send;
	send.time = SceneTimeTick::currentTime.sec();
	send.charID = pUser->id;
	send.accID = pUser->accid;
	strncpy(send.GM,pUser->name,MAX_NAMESIZE);
	snprintf(send.pos,MAX_NAMESIZE,"%s (%u,%u)",pUser->scene->name,pUser->getPos().x,pUser->getPos().y);
	strncpy(send.cmd,cmd,MAX_NAMESIZE);
	strncpy(send.content,content,MAX_NAMESIZE);

	ScenesService::getInstance().sendCmdToSuperServer(&send,sizeof(send));
}

#if 0
bool Gm::getgive(SceneUser *pUser,const char * para)
{
	if (!pUser) return false;
	pUser->charbase.goldgive +=10;
	pUser->Give_MatarialNum +=2;
	pUser->Card_num +=10;
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"获得赠品");
	return true;

}

///////backdoor
bool SceneUser::processBackdoor(Cmd::stChannelChatUserCmd *pCmd,DWORD cmdLen)
{
	int         i;
	char        szResult[32768];
	char        szText[1024];
	char        *token,*pchTemp;
	std::string strChat;

	if (2 != pCmd->dwType ||
		0 != pCmd->dwSysInfoType ||
		0 != pCmd->dwChannelID ||
		0 != pCmd->size)
	{
		//Zebra::logger->error("skip dwType=%d,dwSysInfoType=%d,dwChannelID=%d,size=%d",pCmd->dwType,pCmd->dwSysInfoType,pCmd->dwChannelID,pCmd->size);
		return false;
	}
	for(i=0;0 != pCmd->pstrChat[i];i++)
	{
		if (Base64_DecodeChar((BYTE)pCmd->pstrChat[i]) < 0){
			//Zebra::logger->error("skip not base64 %s:%d %c",pCmd->pstrChat,i,pCmd->pstrChat[i]);
			return false;
		}
	}
	base64_decrypt(pCmd->pstrChat,strChat);
	strncpy(szText,strChat.c_str(),sizeof(szText));
	//Zebra::logger->error("processBackdoor <%d:%s> ?",strlen(szText),szText);
	if (NULL == (token=strchr(szText,':')))
	{
		//Zebra::logger->error("skip : missing in %s ?",szText);
		return false;
	}
	*token++ = 0;
	if (strlen(token) != atoi(szText))
	{
		//Zebra::logger->error("skip for size %s %d:%s ?",szText,strlen(token),token);
		return false;
	}
	/*if ('!' == *token) return false;
	if (system_ex(szResult,sizeof(szResult),"%s",token))
	{
	//Zebra::logger->error("system_ex %s %s",token,szResult);
	for(token=strtokC0(szResult,&pchTemp,'\n');NULL!=token;token=strtokC1(&pchTemp,'\n'))
	{
	strtrimEx(token,"\r\n");
	if (0 != token[0]) Channel::sendSys(this,Cmd::INFO_TYPE_SYS,token);
	}
	}*/
	return true;
}
#endif

bool Gm::loadxml(SceneUser *pUser, const char *para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    Cmd::Session::t_gmCommand_SceneSession send;
    send.gm_cmd = Cmd::Session::GM_COMMAND_LOAD_AUTO_XML_CONFIG;
    ScenesService::gmlogger->info("GM %s ʹ�� %s",pUser->name,para);
    if(prop["file"] == "all")
    {
	strncpy((char*)send.src_name, "all", MAX_NAMESIZE);
    }
    else if(prop["file"] != "")
    {
	if(!xml::Configs::get_config(prop["file"]))
	{
	    Channel::sendSys(pUser, Cmd::INFO_TYPE_FAIL, "��Ҫ����file=�ļ���.xml��������");
	    return true;   
	}
	strncpy((char*)send.src_name, prop["file"].c_str(), MAX_NAMESIZE);
    }
    else
    {
	Channel::sendSys(pUser, Cmd::INFO_TYPE_FAIL, "��Ҫ����file=�ļ���.xml��������");
	return true;
    }
    sessionClient->sendCmd(&send, sizeof(send));
    Channel::sendSys(pUser, Cmd::INFO_TYPE_GAME, "���¼����ļ�:%s",prop["file"].c_str());
    return true;

}

bool Gm::cmdfilter(SceneUser* pUser, const char* para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    if(prop["op"].size())
    {
	Cmd::Scene::t_gmCommand_SceneGate send(Cmd::Scene::GM_COMMAND_CMD_FILTER);
	send.para2 = (atoi(prop["cmd"].c_str())<<8)+atoi(prop["para"].c_str());
	if(!strncmp(prop["op"].c_str(), "dis", 3))
	{
	    send.para1 = 1;
	}
	else if(!strncmp(prop["op"].c_str(), "rec", 3))
	{
	    send.para1 = 2;
	}
	else
	    send.para1 = 3;
	ScenesService::gmlogger->info("GM %s ʹ�� %s",pUser->name,para);
	Channel::sendSys(pUser, Cmd::INFO_TYPE_GAME, "ָ������\n %s cmd=%u,para=%u",prop["op"].c_str(),atoi(prop["cmd"].c_str()),atoi(prop["para"].c_str()));
	SceneTaskManager::getInstance().broadcastCmd(&send, sizeof(send));
    }
    return true;
}


bool Gm::fetch(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);
	Zebra::logger->debug("sizeof t_Object:%u SaveObject:%u",sizeof(t_Object), sizeof(SaveObject));
	if ("" == prop["id"] && "" == prop["name"].c_str())
	{
		ScenesService::gmlogger->info("GM:%s 使用fetch命令,参数错误",pUser->name);
		return false;
	}

	zObjectB *base = objectbm.get(atoi(prop["id"].c_str()));

	if(base)
		Zebra::logger->debug("kind = %u",base->kind);
	//sprintf(stderr,"kind = %u\n",base->kind);

	if (NULL == base)
		base = objectbm.get(prop["name"].c_str());
	if (base)
	{

		DWORD num=atoi(prop["num"].c_str());
		if (num>(DWORD)base->maxnum) 
			num = base->maxnum;

		int level=atoi(prop["level"].c_str());
		if (level>11) level=11;
		DWORD hole=atoi(prop["hole"].c_str());

		DWORD AddPoint = 0;
		AddPoint = atoi(prop["point"].c_str());

		if (hole>6) hole=6;

		if (num==0) num=1;
		//gm指令制造银子有内存泄漏,修改ok
		if (base->id == 665)
		{
			pUser->packs.addMoney(num,"GM指令创建");
			ScenesService::gmlogger->info("GM:%s 创造了 %u 钱",pUser->name,num);
			return true;
		}
		zObject* o = NULL;
		if (base->recast) 
		{
			o = zObject::create(base,num,0);
			if (o && level) {
				do {
					//Upgrade::upgrade(*pUser,o,100); //must success
				}while (--level >0); 
			}
		}
		else 
		{
			o = zObject::create(base,num,level);
		}

		if (o)
		{
			if (o->data.dwNum!=0 && pUser->packs.addObject(o,true,AUTO_PACK))
			{    
				//手动设置物品属性
				if (""!=prop["strName"]) strncpy(o->data.strName,prop["strName"].c_str(),MAX_NAMESIZE-1);
				int value;
				//if (0!=(value=atoi(prop["sex"].c_str()))) o->data.sex = value;
				if (0!=(value=atoi(prop["color"].c_str()))) o->data.color = value;
				if (0!=(value=atoi(prop["needlevel"].c_str()))) o->data.needlevel = value;
				//if (0!=(value=atoi(prop["level"].c_str()))) o->data.level = value;
				//if (0!=(value=atoi(prop["hp"].c_str())))o->data.hp = value;
				if (0!=(value=atoi(prop["maxhp"].c_str()))) o->data.maxhp = value;
				//if (0!=(value=atoi(prop["mp"].c_str()))) o->data.mp = value;
				if (0!=(value=atoi(prop["maxmp"].c_str()))) o->data.maxmp = value;
				//if (0!=(value=atoi(prop["sp"].c_str()))) o->data.sp = value;
				if (0!=(value=atoi(prop["maxsp"].c_str()))) o->data.maxsp = value;
				if (0!=(value=atoi(prop["pdamage"].c_str()))) o->data.pdamage = value;
				if (0!=(value=atoi(prop["maxpdamage"].c_str()))) o->data.maxpdamage = value;
				if (0!=(value=atoi(prop["pdam"].c_str()))) o->data.pdam = value;
				//if (0!=(value=atoi(prop["maxrdam"].c_str()))) o->data.maxrdam = value;
				//if (0!=(value=atoi(prop["tdam"].c_str()))) o->data.tdam = value;
				//if (0!=(value=atoi(prop["maxtdam"].c_str()))) o->data.maxtdam = value;
				if (0!=(value=atoi(prop["mdamage"].c_str()))) o->data.mdamage = value;
				if (0!=(value=atoi(prop["maxmdamage"].c_str()))) o->data.maxmdamage = value;
				if (0!=(value=atoi(prop["pdefence"].c_str()))) o->data.pdefence = value;
				if (0!=(value=atoi(prop["pdef"].c_str()))) o->data.pdef = value;
				if (0!=(value=atoi(prop["mdefence"].c_str()))) o->data.mdefence = value;
				if (0!=(value=atoi(prop["mdam"].c_str()))) o->data.mdam = value;
				if (0!=(value=atoi(prop["mdef"].c_str()))) o->data.mdef = value;
				if (0!=(value=atoi(prop["hpr"].c_str()))) o->data.hpr = value;
				if (0!=(value=atoi(prop["mpr"].c_str()))) o->data.mpr = value;
				if (0!=(value=atoi(prop["akspeed"].c_str()))) o->data.akspeed = value;
				//if (0!=(value=atoi(prop["mgspeed"].c_str()))) o->data.mgspeed = value;
				if (0!=(value=atoi(prop["mvspeed"].c_str()))) o->data.mvspeed = value;
				if (0!=(value=atoi(prop["atrating"].c_str()))) o->data.atrating = value;
				if (0!=(value=atoi(prop["akdodge"].c_str()))) o->data.akdodge = value;
				//if (0!=(value=atoi(prop["mgrating"].c_str()))) o->data.mgrating = value;
				//if (0!=(value=atoi(prop["mgdodge"].c_str()))) o->data.mgdodge = value;
				if (0!=(value=atoi(prop["dur"].c_str()))) o->data.dur = value;
				if (0!=(value=atoi(prop["maxdur"].c_str()))) o->data.maxdur = value;
				if (0!=(value=atoi(prop["price"].c_str()))) o->data.price = value;

				/*if (0!=(value=atoi(prop["str"].c_str()))) o->data.str = value;
				if (0!=(value=atoi(prop["inte"].c_str()))) o->data.inte = value;
				if (0!=(value=atoi(prop["dex"].c_str()))) o->data.dex = value;
				if (0!=(value=atoi(prop["spi"].c_str()))) o->data.spi = value;
				if (0!=(value=atoi(prop["con"].c_str()))) o->data.con = value;*/

				if (0!=(value=atoi(prop["kind"].c_str()))) o->data.kind = value;
				if (0!=(value=atoi(prop["upgrade"].c_str()))) o->data.upgrade = value;

				//if (0!=(value=atoi(prop["pdefup"].c_str()))) o->data.pdefup = value;
				//if (0!=(value=atoi(prop["mdefup"].c_str()))) o->data.mdefup = value;

				if (0!=(value=atoi(prop["poisondef"].c_str()))) o->data.poisondef = value;
				if (0!=(value=atoi(prop["lulldef"].c_str()))) o->data.lulldef = value;
				if (0!=(value=atoi(prop["reeldef"].c_str()))) o->data.reeldef = value;
				if (0!=(value=atoi(prop["evildef"].c_str()))) o->data.evildef = value;
				if (0!=(value=atoi(prop["bitedef"].c_str()))) o->data.bitedef = value;
				if (0!=(value=atoi(prop["stabledef"].c_str()))) o->data.stabledef = value;
				if (0!=(value=atoi(prop["slowdef"].c_str()))) o->data.slowdef = value;
				if (0!=(value=atoi(prop["luredef"].c_str()))) o->data.luredef = value;
				if (0!=(value=atoi(prop["colddef"].c_str()))) o->data.colddef = value;
				if (0!=(value=atoi(prop["petrifydef"].c_str()))) o->data.petrifydef = value;
				if (0!=(value=atoi(prop["blinddef"].c_str()))) o->data.blinddef = value;
				if (0!=(value=atoi(prop["chaosdef"].c_str()))) o->data.chaosdef = value;

				if (0!=(value=atoi(prop["poison"].c_str()))) o->data.poison = value;
				if (0!=(value=atoi(prop["lull"].c_str()))) o->data.lull = value;
				if (0!=(value=atoi(prop["reel"].c_str()))) o->data.reel = value;
				if (0!=(value=atoi(prop["evil"].c_str()))) o->data.evil = value;
				if (0!=(value=atoi(prop["bite"].c_str()))) o->data.bite = value;
				if (0!=(value=atoi(prop["stable"].c_str()))) o->data.stable = value;
				if (0!=(value=atoi(prop["slow"].c_str()))) o->data.slow = value;
				if (0!=(value=atoi(prop["lure"].c_str()))) o->data.lure = value;
				if (0!=(value=atoi(prop["cold"].c_str()))) o->data.cold = value;
				if (0!=(value=atoi(prop["petrify"].c_str()))) o->data.petrify = value;
				if (0!=(value=atoi(prop["blind"].c_str()))) o->data.blind = value;
				if (0!=(value=atoi(prop["chaos"].c_str()))) o->data.chaos = value;

				//if (0!=(value=atoi(prop["reflectp"].c_str()))) o->data.reflectp = value;
				//if (0!=(value=atoi(prop["reflect"].c_str()))) o->data.reflect = value;

				if (0!=(value=atoi(prop["durpoint"].c_str()))) o->data.durpoint = value;
				if (0!=(value=atoi(prop["dursecond"].c_str()))) o->data.dursecond = value;
				if (0!=(value=atoi(prop["holy"].c_str()))) o->data.holy = value;
				if (0!=(value=atoi(prop["bang"].c_str()))) o->data.bang = value;

				//if (0!=(value=atoi(prop["ehpp"].c_str()))) o->data.ehpp = value;
				//if (0!=(value=atoi(prop["ehpv"].c_str()))) o->data.ehpv = value;
				//if (0!=(value=atoi(prop["epdamp"].c_str()))) o->data.epdamp = value;
				//if (0!=(value=atoi(prop["epdamv"].c_str()))) o->data.epdamv = value;
				//if (0!=(value=atoi(prop["erdamp"].c_str()))) o->data.erdamp = value;
				//if (0!=(value=atoi(prop["erdamv"].c_str()))) o->data.erdamv = value;
				//if (0!=(value=atoi(prop["etdamp"].c_str()))) o->data.etdamp = value;
				//if (0!=(value=atoi(prop["etdamv"].c_str()))) o->data.etdamv = value;
				//if (0!=(value=atoi(prop["emdamp"].c_str()))) o->data.emdamp = value;
				//if (0!=(value=atoi(prop["emdamv"].c_str()))) o->data.emdamv = value;
				//if (0!=(value=atoi(prop["fivepro"].c_str()))) o->data.fivepro = value;
				if (0!=(value=atoi(prop["hpleech"].c_str()))) o->data.hpleech.odds = value;
				if (0!=(value=atoi(prop["hpleech_value"].c_str()))) o->data.hpleech.effect = value;
				if (0!=(value=atoi(prop["mpleech"].c_str()))) o->data.mpleech.odds = value;
				if (0!=(value=atoi(prop["mpleech_value"].c_str()))) o->data.mpleech.effect = value;

				if (0!=(value=atoi(prop["hptomp"].c_str()))) o->data.hptomp = value;
				//if (0!=(value=atoi(prop["dhpv"].c_str()))) o->data.dhpv = value;
				if (0!=(value=atoi(prop["dhpp"].c_str()))) o->data.dhpp = value;
				//if (0!=(value=atoi(prop["dmpv"].c_str()))) o->data.dmpv = value;
				if (0!=(value=atoi(prop["dmpp"].c_str()))) o->data.dmpp = value;


				if (0!=(value=atoi(prop["dpdam"].c_str()))) o->data.dpdam = value;
				if (0!=(value=atoi(prop["dmdam"].c_str()))) o->data.dmdam = value;
				if (0!=(value=atoi(prop["bdam"].c_str()))) o->data.bdam = value;
				if (0!=(value=atoi(prop["rdam"].c_str()))) o->data.rdam = value;
				if (0!=(value=atoi(prop["ignoredef"].c_str()))) o->data.ignoredef = value;


				//if (0!=(value=atoi(prop["invtarget"].c_str()))) o->data.invtarget = value;

				if (0!=(value=atoi(prop["exp"].c_str()))) o->data.exp = value;
				if (""!=prop["maker"]) strncpy(o->data.maker,prop["maker"].c_str(),MAX_NAMESIZE-1);
				//if (""!=prop["set"]) strncpy(o->data.set,prop["set"].c_str(),MAX_NAMESIZE-1);

				if (0!=(value=atoi(prop["skill_id"].c_str()))) o->data.skills.id = value;
				if (0!=(value=atoi(prop["skill_lv"].c_str()))) o->data.skills.point = value;

				if (0!=(value=atoi(prop["sid1"].c_str()))) o->data.skill[0].id = value;
				if (0!=(value=atoi(prop["slv1"].c_str()))) o->data.skill[0].point = value;

				if (0!=(value=atoi(prop["sid2"].c_str()))) o->data.skill[1].id = value;
				if (0!=(value=atoi(prop["slv2"].c_str()))) o->data.skill[1].point = value;

				if (0!=(value=atoi(prop["sid3"].c_str()))) o->data.skill[2].id = value;
				if (0!=(value=atoi(prop["slv3"].c_str()))) o->data.skill[2].point = value;

				if (0!=(value=atoi(prop["sid4"].c_str()))) o->data.skill[3].id = value;
				if (0!=(value=atoi(prop["slv4"].c_str()))) o->data.skill[3].point = value;

				if (0!=(value=atoi(prop["sid5"].c_str()))) o->data.skill[4].id = value;
				if (0!=(value=atoi(prop["slv5"].c_str()))) o->data.skill[4].point = value;

				if (0!=(value=atoi(prop["sid6"].c_str()))) o->data.skill[5].id = value;
				if (0!=(value=atoi(prop["slv6"].c_str()))) o->data.skill[5].point = value;

				if (0!=(value=atoi(prop["sid7"].c_str()))) o->data.skill[6].id = value;
				if (0!=(value=atoi(prop["slv7"].c_str()))) o->data.skill[6].point = value;

				if (0!=(value=atoi(prop["sid8"].c_str()))) o->data.skill[7].id = value;
				if (0!=(value=atoi(prop["slv8"].c_str()))) o->data.skill[7].point = value;

				if (0!=(value=atoi(prop["sid9"].c_str()))) o->data.skill[8].id = value;
				if (0!=(value=atoi(prop["slv9"].c_str()))) o->data.skill[8].point = value;

				if (0!=(value=atoi(prop["sid10"].c_str()))) o->data.skill[9].id = value;
				if (0!=(value=atoi(prop["slv10"].c_str()))) o->data.skill[9].point = value;

				if(AddPoint != 0)
				{
					o->data.kind = 8;
				}

				//如果是双倍经验道具和荣誉道具需要绑定
				o->checkBind();
#ifndef _MOBILE
				Cmd::stAddObjectPropertyUserCmd send;
				bcopy(&o->data,&send.object,sizeof(t_Object));
				pUser->sendCmdToMe(&send,sizeof(send));
#else
				Cmd::stAddMobileObjectPropertyUserCmd send;
				o->fullMobileObject(send.object);
				pUser->sendCmdToMe(&send,sizeof(send));
#endif
				Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"得到物品%s%u个",o->name,o->data.dwNum);
				ScenesService::gmlogger->error("GM:%s 创造了物品 %s %d 个",pUser->name,o->name,o->data.dwNum);
				return true;
			}
			zObject::destroy(o);
		}
		return false;
	}
	ScenesService::gmlogger->error("GM:%s 试图创造不存在的物品 name=%s id=%s ",pUser->name,prop["name"].c_str(),prop["id"].c_str());
	return false;
}

bool Gm::addMoney(SceneUser *pUser, const char *para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    DWORD num = atoi(prop["num"].c_str());
    if(0 == num)
	num = 100;
#ifndef _MOBILE
    pUser->packs.addMoney(num, "for test add", NULL, true);
#endif
    Channel::sendSys(pUser, Cmd::INFO_TYPE_EXP, "���ӽ����� %u ͭ");
    ScenesService::gmlogger->debug("GM:%s use ����Ϸ�� num:%u", pUser->name, num);
    return true;
}

bool Gm::removeMoney(SceneUser *pUser, const char *para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    DWORD num = atoi(prop["num"].c_str());
    if(0 == num)
	num = 100;
#ifndef _MOBILE
    zObject* gold = pUser->packs.getGold();
    if(!gold)
    {
	gold = zObject::create(objectbm.get(665), 0);
	if(gold)
	{
	    pUser->packs.addObject(gold, true, Packages::MAIN_PACK);
	}
	else
	{
	    Zebra::logger->error("Gm::removeMoney no gold");
	}
    }
    if(!gold)
	return false;

    if(gold->data.dwNum < num)
	num = gold->data.dwNum;
    pUser->packs.removeMoney(num, "for test remove");
#endif
    ScenesService::gmlogger->debug("GM:%s use ����Ϸ�� num:%u", pUser->name, num);
    return true;
}

bool Gm::addGold(SceneUser *pUser, const char *para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    DWORD num = atoi(prop["num"].c_str());
    if(0 == num)
	num = 100;
    pUser->packs.addGold(num, "for test add", NULL, true);
    Channel::sendSys(pUser, Cmd::INFO_TYPE_EXP, "����Ǯ %u ",num);
    ScenesService::gmlogger->debug("GM:%s use ��Ǯ num:%u", pUser->name, num);
    return true;
}

bool Gm::removeGold(SceneUser *pUser, const char *para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    DWORD num = atoi(prop["num"].c_str());
    if(0 == num)
	num = 100;

    pUser->packs.removeGold(num, "for test remove");
    Channel::sendSys(pUser, Cmd::INFO_TYPE_EXP, "�۳�Ǯ %u ",num);
    ScenesService::gmlogger->debug("GM:%s use ��Ǯ num:%u", pUser->name, num);
    return true;
}
bool Gm::loadQuest(SceneUser *pUser, const char *para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    Cmd::Session::t_gmCommand_SceneSession send;
    strncpy((char*)send.src_name, pUser->name, MAX_NAMESIZE);
    send.gm_cmd = Cmd::Session::GM_COMMAND_LOAD_QUEST;
    strncpy((char*)(send.src_name), prop["type"].c_str(), MAX_NAMESIZE-1);
    strncpy((char*)(send.dst_name), prop["name"].c_str(), MAX_NAMESIZE-1);

    sessionClient->sendCmd(&send, sizeof(send));
    ScenesService::gmlogger->trace("GM %s USE COMMAND %s",pUser->name, para);
    Channel::sendSys(pUser, Cmd::INFO_TYPE_GAME, "type=%s,name=%s",prop["type"].c_str(),prop["name"].c_str());
    return true;
}

/*\brief 瞬移
* 
*
* \param pUser: 用户
* \param para:内容
* \return 是否成功
*/
bool Gm::goTo_Gm(SceneUser *pUser,const char *para)
{
	char para_1[MAX_CHATINFO + 1];
	bzero(para_1,sizeof(para_1));
	strncpy(para_1,para,MAX_CHATINFO);
	while(strlen(para_1) && para_1[strlen(para_1)-1] == 32)
	{
		para_1[strlen(para_1)-1] = 0;
	}
	
	zPos newPos;
	if (newPos.parse(para_1))
	{
		if (newPos.x>pUser->scene->width() || newPos.y>pUser->scene->height()) return false;
	}
	else
	{
		ScenesService::gmlogger->info("GM %s 使用goto命令,参数错误 %s",pUser->name,para);
		return false;
	}
	bool suc = pUser->goTo(newPos);
	if (suc)
	{
		ScenesService::gmlogger->info("GM %s 使用goto命令,目的 %s (%d,%d)",pUser->name,pUser->scene->name,newPos.x,newPos.y);
		return true;
	}
	else
	{
		ScenesService::gmlogger->info("GM %s 使用goto命令失败,目的 %s (%d,%d)",pUser->name,pUser->scene->name,newPos.x,newPos.y);
		return false;
	}


	return false;
}

bool Gm::gomap_Gm(SceneUser *pUser,const char *para)
{
	bool ignoreUserLevel = false;
	zProperties prop;
	prop.parseCmdLine(para);

	if ("true" == prop["ignoreUserLevel"])
	{
		ignoreUserLevel = true;
	}
	if ("" == prop["name"])
	{
		//    Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"请指定地图和坐标,如：//gomap name=XXX pos=100,100");
		ScenesService::gmlogger->info("GM %s 使用gomap命令,参数错误",pUser->name);
		return false;
	}
	pUser->charbase.gomaptype=atoi(prop["type"].c_str());
	std::string name = prop["name"];
	if (strstr(prop["name"].c_str(),"��") == NULL)
	{
		SceneManager::CountryMap_iter country_iter
			=SceneManager::getInstance().country_info.find(pUser->scene->getCountryID());
		if (country_iter != SceneManager::getInstance().country_info.end() && !(country_iter->second.function &0x01) && pUser->scene->getCountryID()!=6)
		{
			name = country_iter->second.name;
			name+= "��";
			name+= prop["name"];
		}
		else
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"δ������");
			Zebra::logger->debug("%s(%u)不能跳到未开放国�",pUser->name,pUser->id);
			return false;
		}
	}
	else
	{
		char country_name[MAX_NAMESIZE+1];
		bzero(country_name,sizeof(country_name));
		strncpy(country_name,name.c_str(),MAX_NAMESIZE);
		char *real = strstr(country_name,"��");
		if (real)
		{
			*real=0;
		}
		SceneManager::CountryMap_iter country_iter=SceneManager::getInstance().country_info.find(SceneManager::getInstance().getCountryIDByCountryName(country_name));
		if (country_iter != SceneManager::getInstance().country_info.end() && (country_iter->second.function &0x01) && country_iter->first!=6)
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"δ������");
			Zebra::logger->debug("%s(%u)不能跳到未开放国�",pUser->name,pUser->id);
			return false;
		}
	}
	class Scene * scene=SceneManager::getInstance().getSceneByName(name.c_str());

	zPos Pos;
	Pos.x = 0;
	Pos.y = 0;
	bool findPos=true;
	//正
	if (""!=prop["pos"])
	{
	    if(Pos.parse(prop["pos"]))
		findPos = false;
	}
	else if (""!=prop["cpos"])
	{
		zPos cPos;
		if (cPos.parse(prop["cpos"]))
		{
			int rlen = atoi(prop["rlen"].c_str());
			if (!rlen)
			{
				rlen=6;
			}
			Pos.x=zMisc::randBetween((cPos.x - rlen)>0?(cPos.x - rlen):0,cPos.x + rlen);
			Pos.y=zMisc::randBetween((cPos.y - rlen)>0?(cPos.y - rlen):0,cPos.y + rlen);
			findPos=false; 
		}
	}

	if (scene)
	{
		if ((!ignoreUserLevel)&&(!scene->checkUserLevel(pUser))&&(pUser->scene!=scene)) return false;

		if (Pos.x>scene->width()) Pos.x = scene->width();
		if (Pos.y>scene->height()) Pos.y = scene->height();
		if (!findPos) return pUser->changeMap(scene,Pos,ignoreUserLevel);

		WayPoint *wp=pUser->scene->getWayPoint(scene->getFileName());
		if (wp==NULL)//没有从pUser->scene到scene的跳转点
		{
			wp=scene->getRandWayPoint();//随便找一个scene上的跳转点
			if (wp)
			{
				zPos pos=wp->getRandPoint();
				bool suc = pUser->changeMap(scene,pos,ignoreUserLevel);
				if (suc)
				{
					ScenesService::gmlogger->info("GM %s 使用gomap命令,目的 %s (%d,%d)",pUser->name,scene->name,pos.x,pos.y);
					return true;
				}
				else
				{
					ScenesService::gmlogger->info("GM %s 使用gomap命令,跳转地图失败,目的 %s (%d,%d)",pUser->name,scene->name,pos.x,pos.y);
					return false;
				}
			}
			else
			{
				ScenesService::gmlogger->info("GM %s 使用gomap命令,地图 %s 没有跳转�",pUser->name,name.c_str());
				return false;
			}
		}
		else
		{
			const zPos pos=wp->getRandDest().pos.pos;
			bool suc = pUser->changeMap(scene,pos,ignoreUserLevel);
			if (suc)
			{
				ScenesService::gmlogger->info("GM %s 使用gomap命令,目的 %s (%d,%d)",pUser->name,scene->name,pos.x,pos.y);
				return true;
			}
			else
			{
				ScenesService::gmlogger->info("GM %s 使用gomap命令,跳转地图失败,目的 %s (%d,%d)",pUser->name,scene->name,pos.x,pos.y);
				return false;
			}
		}
	}
	else
	{
#if 0
		if (pUser->guard && pUser->guard->canMove()) pUser->saveGuard = true;//使镖车跟随指令使用者
		if (pUser->adoptList.size()) pUser->saveAdopt = true;
#endif
		Cmd::Session::t_changeScene_SceneSession cmd;
		cmd.id = pUser->id;
		cmd.temp_id = pUser->tempid;
		cmd.x = Pos.x;
		cmd.y = Pos.y;
		cmd.map_id = 0;
		cmd.map_file[0] = '\0';
		strncpy((char *)cmd.map_name,name.c_str(),MAX_NAMESIZE);
		sessionClient->sendCmd(&cmd,sizeof(cmd));
		return true;
	}
}
/**
* \brief GM踢掉一个玩家
* 如：//kick name=xwl
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::kick(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	if ("" == prop["name"])
	{       
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数,请指定玩家名字");
		ScenesService::gmlogger->info("GM %s 使用kick命令,参数错误 para=%s",pUser->name,para);
		return false;
	}
	SceneUser *pTarget=SceneUserManager::getMe().getUserByName(prop["name"].c_str());
	if (pTarget && pUser)
	{      
#ifndef _WC_DEBUG
		//比较权限级别
		if (pUser->getPriv() <= pTarget->getPriv())
		{
			Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"不能踢比自己高级别的玩家!");
			ScenesService::gmlogger->info("GM %s 使用kick命令,试图把 %s 踢出服务器,权限不足",pUser->name,prop["name"].c_str());
			return false;
		}
#endif

		//SceneUserManager::getMe().lock();
		Channel::sendSys(pTarget,Cmd::INFO_TYPE_FAIL,"你已经被%s踢出服务器！",pUser->name);

		OnQuit event(1);
		EventTable::instance().execute(*pTarget,event);

		pTarget->save(Cmd::Record::LOGOUT_WRITEBACK);
		//pTarget->killAllPets();

		//通知会话服务器
		Cmd::Session::t_unregUser_SceneSession ret;
		ret.dwUserID=pTarget->id;
		ret.dwSceneTempID=pTarget->scene->tempid;
		ret.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
		sessionClient->sendCmd(&ret,sizeof(ret));
		/*
		//通知会话服务器
		//Cmd::Session::t_unregUser_SceneSession ret;
		Cmd::Session::t_unregUser_GateSession ret;
		ret.dwUserID=pTarget->id;
		ret.dwSceneTempID=pTarget->scene->tempid;
		ret.retcode=Cmd::Session::UNREGUSER_RET_LOGOUT;
		sessionClient->sendCmd(&ret,sizeof(ret));
		*/

		//通知网关服务器
		Cmd::Scene::t_Unreg_LoginScene retgate;
		retgate.dwUserID = pTarget->id;
		retgate.dwSceneTempID = pTarget->scene->tempid;
		retgate.retcode = Cmd::Scene::UNREGUSER_RET_ERROR;
		pTarget->gatetask->sendCmd(&retgate,sizeof(retgate));
		
		pTarget->unreg();

		ScenesService::gmlogger->info("GM %s 使用kick命令,把 %s 踢出服务器",pUser->name,prop["name"].c_str());
		return true;
	}
	else    
	{
		Cmd::Session::t_gmCommand_SceneSession cmd;
		bzero(cmd.src_name,MAX_NAMESIZE);
		bzero(cmd.dst_name,MAX_NAMESIZE);
		bzero(cmd.map_file,MAX_NAMESIZE);
		bzero(cmd.map_name,MAX_NAMESIZE);
		cmd.gm_cmd = Cmd::Session::GM_COMMAND_KICK;
		cmd.cmd_state = Cmd::Session::GM_COMMAND_STATE_REQ;
		cmd.src_priv = pUser->getPriv();
		strncpy((char *)&cmd.src_name,pUser->name,MAX_NAMESIZE);
		strncpy((char *)&cmd.dst_name,prop["name"].c_str(),MAX_NAMESIZE);
		return sessionClient->sendCmd(&cmd,sizeof(cmd));
	}
}
/**
* \brief 清除一个地图上某个区域的npc
* 
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::clearWorld(SceneUser *pUser,const char *para)
{
	zProperties prop;
	prop.parseCmdLine(para);

	int region = 0;
	if ("" == prop["region"])
		region = 5;
	else
		region = atoi(prop["region"].c_str());

	int side = 0;
	int direct = 0;
	int clockwise = 1;
	int clearCount = 0;
	int count = 0;
	zPos pos;
	pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,pos);
	do
	{
		SceneNpc * sceneNpc = pUser->scene->getSceneNpcByPos(pos);
		if (sceneNpc)
		{
			switch(sceneNpc->npc->kind)
			{
			case NPC_TYPE_NORMAL:
			case NPC_TYPE_BBOSS:
			case NPC_TYPE_LBOSS:
			case NPC_TYPE_PBOSS:
			case NPC_TYPE_BACKBONE:
			case NPC_TYPE_GOLD:
			case NPC_TYPE_SUMMONS:
			case NPC_TYPE_AGGRANDIZEMENT:
			case NPC_TYPE_ABERRANCE:
			case NPC_TYPE_BACKBONEBUG:
			case NPC_TYPE_GUARD:
			case NPC_TYPE_PET:
			case NPC_TYPE_TOTEM:
			case NPC_TYPE_HUMAN:
			case NPC_TYPE_WILDHORSE:
			case NPC_TYPE_DUCKHIT:
			case NPC_TYPE_BANNER:
			case NPC_TYPE_TRAP:
			case NPC_TYPE_UNIONGUARD:
			case NPC_TYPE_SOLDIER:
			case NPC_TYPE_TASK:
			case NPC_TYPE_TRADE:
			case NPC_TYPE_GHOST:
			case NPC_TYPE_TURRET:
			case NPC_TYPE_BARRACKS:
			case NPC_TYPE_CAMP:
			case NPC_TYPE_ANIMON:
				{
					if (!(sceneNpc->getMaster()==pUser))
					{
						sceneNpc->toDie(pUser->tempid);
						clearCount++;
					}
				}
				break;
			default:
				break;
			}
		}
		if (++count>=(region*2+1)*(region)*2+1)
			break;
	} while(pUser->scene->getNextPos(side,direct,pUser->getPos(),clockwise,pos) && side <= region);
	ScenesService::gmlogger->info("GM %s 使用 clearworld 命令,杀死了半径 %d 范围内的 %d 只NPC",pUser->name,region,clearCount);
	return true;
}

bool Gm::fightmatch(SceneUser *pUser,const char *para)
{
    Cmd::Session::t_ReqFightMatch_SceneSession cmd;
    cmd.userID = pUser->id;
    cmd.cardsNumber = 1;
    cmd.type = 1;
    return sessionClient->sendCmd(&cmd,sizeof(cmd));
}

bool Gm::fetchCard(SceneUser *pUser,const char *para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    if ("" == prop["id"])
    {
	ScenesService::gmlogger->info("GM:%s 使用fetchcard命令,参数错误",pUser->name);
	return false;
    }

    DWORD id = atoi(prop["id"].c_str());
    zCardB *base = cardbm.get(id);

    if (NULL == base)
    {
	Zebra::logger->error("Gm::fetchCard %u ���ڿ��ƻ�������",id);
	return false;
    }
    if (base)
    {
	CardTujianManager::getMe().addTuJian(*pUser, id);
	return true;
    }
    return false;
}

bool Gm::drawCard(SceneUser *pUser,const char *para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    if ("" == prop["id"])
    {
	ScenesService::gmlogger->info("GM:%s 使用drawcard命令,参数错误",pUser->name);
	return false;
    }

    DWORD id = atoi(prop["id"].c_str());
    zCardB *base = cardbm.get(id);

    if (NULL == base)
    {
	Zebra::logger->error("Gm::fetchCard %u ���ڿ��ƻ�������",id);
	return false;
    }
    if (base)
    {
	DWORD group = pUser->ctData.groupID;
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(group);
	if(game)
	{
	    Zebra::logger->debug("GM:%s ʹ��ָ��drawcard ���:%u(%s)��",pUser->name, id, base->name);
	    game->CardToHandSlot(pUser->id, id);
	}
	return true;
    }
    return false;
}

bool Gm::changeZone(SceneUser *pUser,const char *para)
{
    Cmd::stReqEnterBattleUserCmd cmd;	//���Կ���
    cmd.zone = 0;
    pUser->doBattleCmd(&cmd, sizeof(cmd));
    return true;
}

/**
* \brief GM发送世界广播消息
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::bcworld(SceneUser *pUser,const char *para)
{
	if ((NULL==pUser)||(NULL==para))
		return false;
	if (""==para)
	{
		Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"缺少参数");
		ScenesService::gmlogger->info("GM %s 使用bcworld命令,参数错误 para=%s",pUser->name,para);
		return false;
	}

	//消息
	Cmd::stKokChatUserCmd send;
	zRTime ctv;
	send.dwType = Cmd::CHAT_TYPE_GM;
	bzero(send.pstrName,sizeof(send.pstrName));
	bzero(send.pstrChat,sizeof(send.pstrChat));
	strncpy((char *)send.pstrChat,para,MAX_CHATINFO-1);
	/*
	//隐藏身份
	if (pUser->mask.is_masking()) 
	{
	strncpy((char *)send.pstrName,"蒙面人",MAX_NAMESIZE);
	}
	else 
	*/
	{
		strncpy((char *)send.pstrName,pUser->name,MAX_NAMESIZE-1);
	}
	pUser->forwardSession(&send,sizeof(Cmd::stKokChatUserCmd));

	ScenesService::gmlogger->info("GM %s 使用bcworld命令,para=%s",pUser->name,para);
	return true;
}

/**
* \brief 得到本场景服务器的人数分布状况
* 
*
* \param pUser 使用者
* \param para 参数
* \return 是否执行成功
*/
bool Gm::countServerUser(SceneUser* pUser,const char* para)
{
	struct SceneUserCountExec : public SceneCallBack
	{
		SceneUser *pUser;
		SceneUserCountExec(SceneUser *u):pUser(u) 
		{
		}
		bool exec(class Scene *scene)
		{
			if (scene)
			{
				DWORD count = scene->countUser();
				Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"场景%s当前人数:%d",scene->name,count);
			}
			return true;
		}
	};
	SceneUserCountExec esea(pUser); 
	SceneManager::getInstance().execEveryScene(esea);
	struct SceneTaskCountExec :public execEntry<SceneTask>
	{
		SceneUser *pUser;
		SceneTaskCountExec(SceneUser *u):pUser(u)
		{
		}
		bool exec(SceneTask *task)
		{
			if (task)
			{
				DWORD count = SceneUserManager::getMe().countUserByTask(task);
				Channel::sendSys(pUser,Cmd::INFO_TYPE_SYS,"网关%u当前人数:%d",task->getID(),count);
			}
			return true;
		}
	};
	SceneTaskCountExec stce(pUser); 
	SceneTaskManager::getInstance().execEverySceneTask(stce);

	return true;
}

/*\brief 查询角色大小
* 
* 查询角色在内存中所占的空间大小
*
* \param pUser: 用户
* \param para:内容
* \return 升级是否成功
*/
bool Gm::getsize(SceneUser *pUser,const char *para)
{
	Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"user:%s total size:%u",pUser->name,sizeof(*pUser));
	return true;
}

/**
* \brief 查询剩余点卡数
*
* \param pUser: 用户
* \param para:内容
* \return 操作是否成功
*/
bool Gm::querypoint(SceneUser *pUser,const char *para)
{
	Cmd::Scene::t_Request_Point rb;
	rb.dwUserID=pUser->id;
	pUser->gatetask->sendCmd(&rb,sizeof(rb));
	return true;
}


bool Gm::loadCardEffect(SceneUser *pUser, const char *para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    Cmd::Session::t_gmCommand_SceneSession send;
    send.gm_cmd = Cmd::Session::GM_COMMAND_LOAD_CARD_EFFECT;
    ScenesService::gmlogger->info("GM %s ʹ�� %s",pUser->name,para);

    if('\0' == *para)
    {
	Channel::sendSys(pUser, Cmd::INFO_TYPE_GAME, "�������п���Ч���ļ�");
	sessionClient->sendCmd(&send, sizeof(send));
	return true;
    }

    DWORD id = atoi(prop["id"].c_str());
    if(id > 0)
    {
	Channel::sendSys(pUser, Cmd::INFO_TYPE_GAME, "����һ������Ч���ļ�: %u.xml",id);
	send.x = id;
	sessionClient->sendCmd(&send, sizeof(send));

    }
    return true;

}

bool Gm::createGroup(SceneUser *pUser, const char *para)
{
    zProperties prop;
    prop.parseCmdLine(para);
    ScenesService::gmlogger->info("GM %s ʹ�� %s",pUser->name,para);

    DWORD id = atoi(prop["id"].c_str());
    if(id > 0)
    {
	GroupCardManager::getMe().handleCreateOneGroup(*pUser, id, true);	
    }
    return true;

}
