//#include <zebra/ScenesServer.h>
#include <math.h>
#include "Scene.h"
#include "Chat.h"
#include "SceneUser.h"
#include "SceneUserManager.h"
#include "TimeTick.h"
#include "RecordClient.h"
#include "QuestEvent.h"
#include "QuestTable.h"
#include "ZlibObject.h"
#include "BinaryVersion.h"
#include "SaveObject.h"
#include "zObject.h"
#include "ObjectManager.h"
#include "zDatabaseManager.h"
#include "ChallengeGameManager.h"
#include "GiftBagManager.h"

#define MAX_UZLIB_CHAR  (400 * 1024)
#if 0
DWORD SceneUser::Five_Relation[]= 
{
	FIVE_METAL,      /// 金
	FIVE_WOOD,      /// 木
	FIVE_SOIL,      /// 土
	FIVE_WATER,      /// 水
	FIVE_FIRE,      /// 火
	FIVE_NONE      /// 无
};

class ObjectCompare:public UserObjectCompare 
{
public:
	DWORD  dwObjectID;

	bool isIt(zObject *object)
	{
		if (object->data.dwObjectID == dwObjectID) return true;
		return false;
	}
};


bool SceneUser::userQuestEnterDup(DWORD mapId)
{


	return duplicateManager::getInstance().userQuestEnterDup(this,mapId);
	/*std::map<DWORD,unsigned short>::iterator it = tempDups.find(mapId);
	//存在对应地图的副本则进入
	if(it != tempDups.end())
	{
	return userEnterDup(it->second,mapId);
	}

	//否则查看所有队伍中是否有人在副本中,有则进入那个副本


	int tSize = team.getSize();

	for( int i = 0; i < tSize; ++i)
	{
	const TeamMember * member = getMember(i);
	SceneUser *u = SceneUserManager::getMe().getUserByID(member->id);
	if(NULL == u)
	continue;
	if(u->dupIndex != 0)
	{
	DWORD mapid = u->scene->getRealMapID();
	tempDups[mapid] = u->dupIndex;
	return userEnterDup(u->dupIndex,SceneManager::getInstance().getMapId(charbase.country,mapid));
	}

	}

	//没有队伍在副本中,创建新副本,进入那个副本
	unsigned short _dupIndex = duplicateManager::getInstance().CreateDup();
	if(_dupIndex != 0)
	return userEnterDup(_dupIndex,mapId);

	return false;*/



}



bool SceneUser::addObjToPackByThisID(DWORD thisid)
{
	zObject* o = goi->getObjectByThisid(thisid);
	if(o)
	{

		if(!packs.addObject(o,true,AUTO_PACK))
		{
			zObject::destroy(o);
			return false;
		}

		Cmd::stAddObjectPropertyUserCmd ret;
		memcpy(&ret.object,&(o->data),sizeof(t_Object),sizeof(ret.object));
		sendCmdToMe(&ret,sizeof(ret));
		return true;
	}
}
#endif

/**
* \brief 构造函数
*
*/
SceneUser::SceneUser(const DWORD accid):SceneEntryPk(SceneEntry_Player),recycle_delay(0),_half_sec(0.3f),_one_sec(1),_five_sec(5),_3_sec(3),_ten_sec(10),_one_min(60),_five_min(5*58-1),_writeback_timer(ScenesService::getInstance().writeBackTimer,ScenesService::getInstance().writeBackTimer),loginTime(),lastIncTime(0),packs(this),lastCheckMessage(2000),lastMoveTime(0),moveFastCount(0)
{

#ifdef _TEST_DATA_LOG
	bzero(&chartest,sizeof(chartest));
#endif
	killedNpcNum = 0;
	step_state = 0;
	backOffing = 0;
	bzero(accelData,sizeof(accelData));
	bzero(sysSetting,sizeof(sysSetting));
	lastChangeCountryTime = 0;
	temp_unsafety_state = 0;
	safety = 0;
	safety_setup = 0;
	Card_num = 0;
	Give_MatarialNum = 0;
	//dupIndex = 0;
	/*
	zRTime ctv1;
	tenSecondTime = ctv1;
	oneSecondTime = ctv1;
	oneMinuteTime = ctv1;
	*/
	notifyHMS = false;
	deathWaitTime = 0; 
	wdTire = 0;
	wdTirePer = 100;
	sitdownRestitute=false;
	npcdareflag  = false;
	npcdarePosX =0;
	npcdarePosY =0;
	npcdareCountryID=0;
	npcdareMapID=0;
	npcdareNotify = true;

	lastUseSkill = 0;
	this->accid=accid;
	gatetask=NULL;
	scene=NULL;
	bzero(&charbase,sizeof(charbase));
	bzero(&charstate,sizeof(charstate));
	//bzero(&pkState,sizeof(pkState));
	dwBodyID=0;
	dwLeftHandID=0;
	dwRightHandID=0;
	dwBodyColorSystem=zMisc::randBetween(0xFF000001,0xFFFFFFFE);
	dwBodyColorCustom=zMisc::randBetween(0xFF000001,0xFFFFFFFE);
	//  packs.initMe(this);
	//skillStatusM.initMe(this);
	messageOrder = 0;
	npc_dwNpcDataID=0;
	npc_dwNpcTempID=0;
	unReging=false;
	curTargetID = 0;
	curTargetType = 0;

	updateNotify=0; //hp,mp,sp 更新通知
	updateCount=0;  //更新记数

	//guard = NULL;
	//ridepet = 0;
	//pet = 0;
	//summon = 0;
	deathBackToMapID=0;
	//totems.clear();
	//MirageSummon.clear();
	//vWars.clear();
	isQuiz = false;
	isDiplomat = false;
	isCatcher = false;

	//bzero(unionName,sizeof(unionName));
	//bzero(septName,sizeof(septName));

	bzero(caption,sizeof(caption));
	bzero(armyName,sizeof(armyName));

	bzero(&petData,sizeof(petData));
	king = false;
	emperor = false;
	unionMaster = false;
	septMaster = false;
	kingConsort = 0;
	dwArmyState = 0;

	isSendingMail = false;
	isGetingMailItem = false;

	queryTime = 0;
	//cartoon = 0;
	//adoptedCartoon = 0;

	bzero(replyText,sizeof(replyText));

	myOverMan =0;
	saveGuard = false;
	saveAdopt = false;

	_5_sec_count = 0;
	bzero(&npcHoldData,sizeof(npcHoldData));

#ifdef _DEBUG
	processCheckTime = 0;
#else
	processCheckTime = zMisc::randBetween(5,10);
#endif
#ifdef _DEBUG
	Zebra::logger->debug("%s(%u) 下次检查间隔 %u 分钟",name,id,processCheckTime);
#endif
	bzero(wait_gomap_name,sizeof(wait_gomap_name));

	bzero(wglog,sizeof(wglog));
	wglog_len = 0;

	lastKiller = 0;
	dropTime = 0;

	//miniGame = 0;
	//box_item.targetO = NULL;
	//box_item.defaultO = NULL;

	PkTime = 0;

	//TeamThisID = 0;

	//isOpen = true;

	m_bCanJump = true; // [ranqd] 用户进游戏时默认为可跳转状态
	notifyUnfinished = false;
}

SceneUser::~SceneUser()
{
	/*
	//把自己从个人聊天频道里删除
	Zebra::logger->debug("清理用户数据");
	if (team.IsTeamed()) team.setLeader(0); // 如果退出的时候还在组队状态则设置成组队退出。结算社会关系中的友好度
	ChannelM::getMe().removeUser(name);
	scene = NULL;
	*/
}

/**
* \brief 删除用户时,进行相关的清理工作
*
*
*/
void SceneUser::destroy()
{



	//把自己从个人聊天频道里删除
	if (scene) scene->removeUser(this);
	SceneUserManager::getMe().removeUser(this);
	//  Zebra::logger->debug("%s(%x) really destroied",this->name,this);
//	if (TeamThisID != 0) TeamThisID = 0; // 如果退出的时候还在组队状态则设置成组队退出。结算社会关系中的友好度
	ChannelM::getMe().removeUser(name);
//	if (guard) guard->reset();






}

void SceneUser::initCharBase(Scene *intoscene)
{
	using namespace Cmd;
	//设置坐标
	pos.x = charbase.x;
	pos.y = charbase.y;
	//dupIndex = 0;
	if (isNewCharBase())
	{
		Zebra::logger->info("初始化角色信息 %u,%u",charbase.accid,charbase.id);

//Shx Delete 不再通过头像ID来判定性别和职业;
// 		charbase.face = charbase.type; 
 		//charbase.type = PROFESSION_1 ; // getCharTypeByFace(charbase.face);


		charbase.fivetype = 5; //默认五行类型为无五行
		charbase.fivelevel = 1;//
		//设置性别
		/*
		switch(charbase.type)
		{
		case PROFESSION_1:    //侠客
		case PROFESSION_3:    //箭侠
		case PROFESSION_5:    //天师
		case PROFESSION_7:    //法师
		charbase.sex = MALE;
		break;
		case PROFESSION_2:    //侠女
		case PROFESSION_4:    //箭灵
		case PROFESSION_6:    //美女
		case PROFESSION_8:    //仙女
		charbase.sex = FEMALE;
		break;
		case PROFESSION_NONE:  //无业
		default:
		Zebra::logger->error("错误的职业类型");
		break;
		}
		// */
		//在新手出生点随机查找坐标
		bool founded=false;
		if (SceneManager::getInstance().isNewZoneConfig())
		{
			founded=SceneManager::getInstance().randzPosNewZone(intoscene,pos);
			if (founded)
				Zebra::logger->info("查找新手出生点成功：%s,%u,%u",intoscene->name,pos.x,pos.y);
			else
				Zebra::logger->error("查找新手出生点失败：%s,%u,%u",intoscene->name,pos.x,pos.y);
		}
		if (!founded)
		{
			if (intoscene->randzPosByZoneType(ZoneTypeDef::ZONE_NEWBIE,pos))
				Zebra::logger->info("查找新手出生点成功：%s,%u,%u",intoscene->name,pos.x,pos.y);
			else
				Zebra::logger->error("查找新手出生点失败：%s,%u,%u",intoscene->name,pos.x,pos.y);
		}

		//charbase.lucky = 10;

		charbase.bodyColor=zMisc::randBetween(0xFF000001,0xFFFFFFFE);
		//Zebra::logger->debug("bodyColor%u",charbase.bodyColor);
		//设置属性
		setupCharBase();
#ifndef _MOBILE
		//give gold object,will not delete
		zObject* gold = zObject::create(objectbm.get(665),0);
		if (gold && packs.addObject(gold,true,Packages::MAIN_PACK)) 
		{
			Zebra::logger->debug("��ʼ����Ϸ�ҳɹ�");
			zObject::logger(gold->createid,gold->data.qwThisID,gold->data.strName,gold->data.dwNum,gold->data.dwNum,1,0,NULL,this->id,this->name,"create",NULL,0,0);
		}
		else
		{
			Zebra::logger->fatal("��ʼ����Ϸ��ʧ��");
		}
#endif

#ifdef _MOBILE
		HeroInfoManager::getMe().initData(*this);
		CardTujianManager::getMe().initTujian(*this);
#endif

#ifdef _ALL_SUPER_GM
		charbase.gold = 10000000;
#endif
		//默认系统设置
		using namespace Cmd;
		memset(sysSetting,0xff,sizeof(sysSetting));
		sysSetting[0] = 0;//pk模式
		//clear_state(sysSetting,Cmd::USER_SETTING_AUTOFINDPATH);//自动寻路
		clear_state(sysSetting,Cmd::USER_SETTING_SHOW_PLAYERNAME);//显示玩家名字
		clear_state(sysSetting,Cmd::USER_SETTING_AUTO_KILL_SUMMON);//自动打怪
		//通知session
		Cmd::Session::t_sysSetting_SceneSession send;
		strncpy((char *)send.name,name,MAX_NAMESIZE-1);
		bcopy(sysSetting,&send.sysSetting,sizeof(send.sysSetting));
		send.face = charbase.face;
		sessionClient->sendCmd(&send,sizeof(send));

		//通知网关
		Cmd::Scene::t_sysSetting_GateScene gate_send;
		bcopy(sysSetting,gate_send.sysSetting,sizeof(gate_send.sysSetting));
		gate_send.id=this->id;
		this->gatetask->sendCmd(&gate_send,sizeof(gate_send));

		chatColor[0] = 0xffffffff;
		chatColor[1] = 0xffffd100;//COLOR_ARGB(255,255,209,0);//国家频道
		chatColor[2] = 0xff4eaa00;//COLOR_ARGB(255,78,170,0);//地区频道
		chatColor[3] = 0xffff4818;//COLOR_ARGB(255,246,0,255);//密
		chatColor[4] = 0xff34ffbb;//COLOR_ARGB(255,52,255,187);//帮会频道
		chatColor[5] = 0xff98f417;//COLOR_ARGB(255,152,244,23);//队伍频道
		chatColor[6] = 0xffff627c;//COLOR_ARGB(255,255,98,124);//家族频道
		chatColor[7] = 0xff007fff;//COLOR_ARGB(255,0,127,255);//好友频道
		//chatColor[8] = 0xffff0fa0;//COLOR_ARGB(255,255,240,160);//世界频道
		//chatColor[9] = 0xffffb4ff;//COLOR_ARGB(255,255,180,255);//师门频道

		//通知客户端
		Cmd::stSystemSettingsUserCmd sendClient;
		bcopy(sysSetting,&sendClient.data.bySettings,sizeof(sendClient.data.bySettings));
		bcopy(chatColor,&sendClient.data.dwChatColor,sizeof(sendClient.data.dwChatColor));
		sendCmdToMe(&sendClient,sizeof(sendClient));

		charbase.hp = charstate.maxhp;
		charbase.mp = charstate.maxmp;
		charbase.sp = charstate.maxsp;

		charbase.goodness = Cmd::GOODNESS_2_1;
		charbase.points =3;
		//charbase.skillpoint = 20;//临时
		charbase.skillpoint = 1;//真是
		charbase.bitmask |= CHARBASE_OK;
		charbase.createtime=time(NULL);


#ifdef _TEST_DATA_LOG
		writeCharTest(Cmd::Record::NEWCHAR_WRITEBACK);
#endif // _TEST_DATA_LOG测试数据
		bzero(this->charbase.tiretime,36);
	}
	else
	{
		setupCharBase();
#ifdef _TEST_DATA_LOG
		Cmd::Record::t_Read_CharTest_SceneRecord ret;
		strncpy(ret.name,name,MAX_NAMESIZE);
		ret.level = charbase.level;
		recordClient->sendCmd(&ret,sizeof(ret));
#endif // _TEST_DATA_LOG测试数据
	}
	//skillStatusM.processPassiveness();

	initTire();
	//初始化当天答题次数
//	initAnswerCount();
//	_userScriptTaskContainer = new userScriptTaskContainer;
}


void SceneUser::calReliveWeaknessProperty(bool enter)
{
#ifdef _DEBUG
	Zebra::logger->debug("计算复活虚弱状态的人物属性值:property(%d,%d,%d,%d,%d)",this->charstate.wdCon,this->charstate.wdStr,
		this->charstate.wdDex,this->charstate.wdInt,this->charstate.wdMen);
#endif  

	if (this->charbase.reliveWeakTime > 0)
	{

		this->charstate.wdCon = (DWORD)(this->charstate.wdCon * 0.6);
		this->charstate.wdStr = (DWORD)(this->charstate.wdStr * 0.6);
		this->charstate.wdDex = (DWORD)(this->charstate.wdDex * 0.6);
		this->charstate.wdInt = (DWORD)(this->charstate.wdInt * 0.6);
		this->charstate.wdMen = (DWORD)(this->charstate.wdMen * 0.6);
#ifdef _DEBUG
		Zebra::logger->debug("计算复活虚弱状态的人物属性值:property(%d,%d,%d,%d,%d)",this->charstate.wdCon,this->charstate.wdStr,
			this->charstate.wdDex,this->charstate.wdInt,this->charstate.wdMen);
#endif  
	}
}
/**
* \brief 设置角色属性信息
*
* 包括裸身和装备以及各种状态影响的角色属性数值
*
*/
void SceneUser::setupCharBase(bool lock)
{
#if 0
	if ((charbase.goodness & 0XFF000000) == 0XFF000000)
	{
		DWORD temp=this->charbase.goodness & 0X00FF0000;
		this->charbase.goodness &= 0X0000FFFF;
		this->pkState.addProtect(this,temp);
	}

	charstate.resumehp = BASEDATA_M_RESUMEHP;
	charstate.resumemp = BASEDATA_M_RESUMEMP;
	charstate.resumesp = BASEDATA_M_RESUMESP;

	charstate.attackspeed = 640;
	if( (getPriv() & Gm::debug_mode) || (getPriv() & Gm::super_mode) || (getPriv() & Gm::captain_mode) )
	{
		charstate.movespeed = 340;
	}
	else
	{
		charstate.movespeed = 640;
	}

	if (charbase.pkaddition > 1800 /*30*60*/)
	{
		this->setUState(Cmd::USTATE_PK);
	}
	if (charbase.trainTime)
		this->setUState(Cmd::USTATE_DAOJISHI);

	//sky 根据装备和技能重新计算人物的基本属性（新增自由属性点的计算）
	charstate.wdCon = charbase.wdCon + packs.equip.getEquips().get_con() + packs.equip.getEquips().get_Freedom_con() + skillValue.upattribute + skillValue.upcon;
	charstate.wdStr = charbase.wdStr + packs.equip.getEquips().get_str() + packs.equip.getEquips().get_Freedom_str() + skillValue.upattribute;
	charstate.wdDex = charbase.wdDex + packs.equip.getEquips().get_dex() + packs.equip.getEquips().get_Freedom_dex() + skillValue.upattribute + skillValue.updex;
	charstate.wdInt = charbase.wdInt + packs.equip.getEquips().get_inte() + packs.equip.getEquips().get_Freedom_inte() + skillValue.upattribute + skillValue.upint;
	charstate.wdMen = (WORD)((charbase.wdMen + packs.equip.getEquips().get_spi() + packs.equip.getEquips().get_Freedom_spi() + skillValue.upattribute)*(1+skillValue.addmenp/100.0f));

	if (this->issetUState(Cmd::USTATE_TOGETHER_WITH_TIGER))
	{//虎魄附体,所有属性加成50%
		charstate.wdCon = (WORD)(charstate.wdCon + charstate.wdCon*0.2);
		charstate.wdStr = (WORD)(charstate.wdStr + charstate.wdStr*0.2);
		charstate.wdDex = (WORD)(charstate.wdDex + charstate.wdDex*0.2);
		charstate.wdInt = (WORD)(charstate.wdInt + charstate.wdInt*0.2);
		charstate.wdMen = (WORD)(charstate.wdMen + charstate.wdMen*0.2);
	}

	charstate.wdCon = (WORD)(charstate.wdCon + charbase.wdCon*(skillValue.upattribcon/100.0f));
	charstate.wdStr = (WORD)(charstate.wdStr + charbase.wdStr*(skillValue.upattribstr/100.0f));
	charstate.wdDex = (WORD)(charstate.wdDex + charbase.wdDex*(skillValue.upattribdex/100.0f));
	charstate.wdInt = (WORD)(charstate.wdInt + charbase.wdInt*(skillValue.upattribint/100.0f));
	charstate.wdMen = (WORD)(charstate.wdMen + charbase.wdMen*(skillValue.upattribmen/100.0f));

	charstate.wdStr = (WORD)(charstate.wdStr*(1-skillValue.dpstrdex/100.0f));
	charstate.wdDex = (WORD)(charstate.wdDex*(1-skillValue.dpstrdex/100.0f));
	charstate.wdInt = (WORD)(charstate.wdInt*(1-skillValue.dpintmen/100.0f));
	charstate.wdMen = (WORD)(charstate.wdMen*(1-skillValue.dpintmen/100.0f));

	charstate.wdStr += (DWORD)(charbase.wdStr * (skillValue.upstr/100.0f));
	if (skillValue.upallattrib>0)
	{
		charstate.wdCon = (WORD)(charstate.wdCon + skillValue.upallattrib);
		charstate.wdStr = (WORD)(charstate.wdStr + skillValue.upallattrib);
		charstate.wdDex = (WORD)(charstate.wdDex + skillValue.upallattrib);
		charstate.wdInt = (WORD)(charstate.wdInt + skillValue.upallattrib);
		charstate.wdMen = (WORD)(charstate.wdMen + skillValue.upallattrib);
	}
	if (skillValue.dnallattrib>0)
	{
		if (charstate.wdCon >= (WORD)skillValue.dnallattrib)
			charstate.wdCon = (WORD)(charstate.wdCon - (WORD)skillValue.dnallattrib);
		else
			charstate.wdCon =0;
		if (charstate.wdStr >= (WORD)skillValue.dnallattrib)
			charstate.wdStr = (WORD)(charstate.wdStr - (WORD)skillValue.dnallattrib);
		else
			charstate.wdStr =0;
		if (charstate.wdDex >= (WORD)skillValue.dnallattrib)
			charstate.wdDex = (WORD)(charstate.wdDex - (WORD)skillValue.dnallattrib);
		else
			charstate.wdDex =0;
		if (charstate.wdInt >= (WORD)skillValue.dnallattrib)
			charstate.wdInt = (WORD)(charstate.wdInt - (WORD)skillValue.dnallattrib);
		else
			charstate.wdInt =0;
		if (charstate.wdMen >= (WORD)skillValue.dnallattrib)
			charstate.wdMen = (WORD)(charstate.wdMen - (WORD)skillValue.dnallattrib);
		else
			charstate.wdMen =0;
	}

	if (skillValue.dpallattrib>0)
	{
		charstate.wdCon = (DWORD)(charstate.wdCon * (1-(skillValue.dpallattrib/100.0f)));
		charstate.wdStr = (DWORD)(charstate.wdStr * (1-(skillValue.dpallattrib/100.0f)));
		charstate.wdDex = (DWORD)(charstate.wdDex * (1-(skillValue.dpallattrib/100.0f)));
		charstate.wdInt = (DWORD)(charstate.wdInt * (1-(skillValue.dpallattrib/100.0f)));
		charstate.wdMen = (DWORD)(charstate.wdMen * (1-(skillValue.dpallattrib/100.0f)));
	}

	if (this->charbase.reliveWeakTime > 0)
	{// 只要是复活虚弱状态,则把五个属性降为60%,不因任何原因而改变
#ifdef _DEBUG
		Zebra::logger->debug("复活虚弱状态剩余时间:%d",(this->charbase.reliveWeakTime-(SceneTimeTick::currentTime.sec())%10000));    
#endif    
		this->calReliveWeaknessProperty(true);
	}

	switch(charbase.type)
	{
	case  PROFESSION_1:
		{
			charstate.maxhp=DWORD(BASEDATA_M_HP+LEVELUP_HP_N*charbase.level + charstate.wdStr*2 + charstate.wdCon*15);
			charstate.maxmp=DWORD(BASEDATA_M_MP+LEVELUP_MP_N*charbase.level + charstate.wdInt*1 + charstate.wdMen*3);
			charstate.maxsp=DWORD(BASEDATA_M_SP+LEVELUP_SP_N*charbase.level);

			//          charstate.resumehp=DWORD(BASEDATA_M_RESUMEHP+LEVELUP_RESUMEHP_N*charbase.level + charstate.wdMen*0.5);
			//          charstate.resumemp=DWORD(BASEDATA_M_RESUMEMP+LEVELUP_RESUMEMP_N*charbase.level + charstate.wdMen*1);
			//          charstate.resumesp=DWORD(BASEDATA_M_RESUMESP+LEVELUP_RESUMESP_N*charbase.level + charstate.wdMen*1);

			//          charstate.attackrating=DWORD(BASEDATA_M_ATTACKRATING+LEVELUP_ATTACKRATING_N*charbase.level + charstate.wdDex*1);
			//          charstate.attackdodge=DWORD(BASEDATA_M_ATTACKDODGE+LEVELUP_ATTACKDODGE_N*charbase.level + charstate.wdDex*1);

			//          charstate.attackspeed=DWORD(BASEDATA_M_ATTACKSPEED+LEVELUP_ATTACKSPEED_N*charbase.level + charstate.wdDex*1);

			charstate.pdamage=DWORD(BASEDATA_M_PDAMAGE+LEVELUP_PDAMAGE_N*charbase.level + charstate.wdStr*1);
			charstate.mdamage=DWORD(BASEDATA_M_MDAMAGE+LEVELUP_MDAMAGE_N*charbase.level + charstate.wdInt*1.1f);
			charstate.pdefence=DWORD(BASEDATA_M_PDEFENCE+LEVELUP_PDEFENCE_N*charbase.level + charstate.wdStr*0.3f + charstate.wdDex*1.5f + charstate.wdCon*0.4f);
			charstate.mdefence=DWORD(BASEDATA_M_MDEFENCE+LEVELUP_MDEFENCE_N*charbase.level + charstate.wdInt*0.3f + charstate.wdDex*1.5f + charstate.wdMen*0.8f + charstate.wdCon*0.6f);
			charstate.bang = (WORD)(BASEDATA_M_BANG + charstate.wdCon*0.002f);

			charstate.stdpdamage=DWORD(BASEDATA_M_PDAMAGE+LEVELUP_PDAMAGE_N*charbase.level + charbase.wdStr*1);
			charstate.stdmdamage=DWORD(BASEDATA_M_MDAMAGE+LEVELUP_MDAMAGE_N*charbase.level + charbase.wdInt*1.1f);
			charstate.stdpdefence=DWORD(BASEDATA_M_PDEFENCE+LEVELUP_PDEFENCE_N*charbase.level + charbase.wdStr*0.3f + charbase.wdDex*1.5f + charbase.wdCon*0.4f);
			charstate.stdmdefence=DWORD(BASEDATA_M_MDEFENCE+LEVELUP_MDEFENCE_N*charbase.level + charbase.wdInt*0.3f + charbase.wdDex*1.5f + charbase.wdMen*0.8f + charbase.wdCon*0.6f);
			charstate.stdbang = (WORD)(BASEDATA_M_BANG + charbase.wdCon*0.002f);
		}
		break;
	case  PROFESSION_2:
		{
			charstate.maxhp=DWORD(BASEDATA_F_HP+LEVELUP_HP_N*charbase.level + charstate.wdStr*2 + charstate.wdCon*15);
			charstate.maxmp=DWORD(BASEDATA_F_MP+LEVELUP_MP_N*charbase.level + charstate.wdInt*1 + charstate.wdMen*3);
			charstate.maxsp=DWORD(BASEDATA_F_SP+LEVELUP_SP_N*charbase.level);

			//          charstate.resumehp=DWORD(BASEDATA_F_RESUMEHP+LEVELUP_RESUMEHP_N*charbase.level + charstate.wdMen*0.5);
			//          charstate.resumemp=DWORD(BASEDATA_F_RESUMEMP+LEVELUP_RESUMEMP_N*charbase.level + charstate.wdMen*1);
			//          charstate.resumesp=DWORD(BASEDATA_F_RESUMESP+LEVELUP_RESUMESP_N*charbase.level + charstate.wdMen*1);

			//          charstate.attackrating=DWORD(BASEDATA_F_ATTACKRATING+LEVELUP_ATTACKRATING_N*charbase.level + charstate.wdDex*1);
			//          charstate.attackdodge=DWORD(BASEDATA_F_ATTACKDODGE+LEVELUP_ATTACKDODGE_N*charbase.level + charstate.wdDex*1);

			//          charstate.attackspeed=DWORD(BASEDATA_F_ATTACKSPEED+LEVELUP_ATTACKSPEED_N*charbase.level + charstate.wdDex*1);

			charstate.pdamage=DWORD(BASEDATA_F_PDAMAGE+LEVELUP_PDAMAGE_N*charbase.level + charstate.wdStr*1);
			charstate.mdamage=DWORD(BASEDATA_F_MDAMAGE+LEVELUP_MDAMAGE_N*charbase.level + charstate.wdInt*1.1f);
			charstate.pdefence=DWORD(BASEDATA_F_PDEFENCE+LEVELUP_PDEFENCE_N*charbase.level + charstate.wdStr*0.3f + charstate.wdDex*1.5f + charstate.wdCon*0.4f);
			charstate.mdefence=DWORD(BASEDATA_F_MDEFENCE+LEVELUP_MDEFENCE_N*charbase.level + charstate.wdInt*0.3f + charstate.wdDex*1.5f + charstate.wdMen*0.8f + charstate.wdCon*0.6f);
			charstate.bang = (WORD)(BASEDATA_F_BANG + charstate.wdCon*0.002f);

			charstate.stdpdamage=DWORD(BASEDATA_F_PDAMAGE+LEVELUP_PDAMAGE_N*charbase.level + charbase.wdStr*1);
			charstate.stdmdamage=DWORD(BASEDATA_F_MDAMAGE+LEVELUP_MDAMAGE_N*charbase.level + charbase.wdInt*1.1f);
			charstate.stdpdefence=DWORD(BASEDATA_F_PDEFENCE+LEVELUP_PDEFENCE_N*charbase.level + charbase.wdStr*0.3f + charbase.wdDex*1.5f + charbase.wdCon*0.4f);
			charstate.stdmdefence=DWORD(BASEDATA_F_MDEFENCE+LEVELUP_MDEFENCE_N*charbase.level + charbase.wdInt*0.3f + charbase.wdDex*1.5f + charbase.wdMen*0.8f + charbase.wdCon*0.6f);
			charstate.stdbang = (WORD)(BASEDATA_F_BANG + charbase.wdCon*0.002f);
		}
		break;
	}

	charstate.maxhp=charstate.maxhp+packs.equip.getEquips().get_maxhp()+skillValue.maxhp+skillValue.sept_maxhp+skillValue.pmaxhp+skillValue.hpupbylevel*charbase.level+skillValue.introject_maxhp;
	charstate.maxhp += (DWORD)(charstate.maxhp * packs.equip.getEquips().get_maxhprate()/100.0f);
	charstate.maxmp=charstate.maxmp+packs.equip.getEquips().get_maxmp()+skillValue.maxmp+skillValue.sept_maxmp;
	charstate.maxmp += (DWORD)(charstate.maxmp * packs.equip.getEquips().get_maxmprate()/100.0f);
	if (charbase.mp > charstate.maxmp) charbase.mp = charstate.maxmp;
	charstate.maxsp=charstate.maxsp+packs.equip.getEquips().get_maxsp()+skillValue.maxsp;



	SDWORD temp=0; 

	temp = (SDWORD)(
		(
		charstate.pdamage+packs.equip.getEquips().get_maxpdamage())*
		(1+packs.equip.getEquips().get_pdam()/100.0f)*
		(1.0f+((int)skillValue.updamp+
		(int)skillValue.theurgy_updamp+
		(int)skillValue.lm_updamp+
		(int)skillValue.nsc_updamp+
		(int)skillValue.sept_updamp+
		(int)skillValue.array_udamp+
		(int)skillValue.spupdamp+
		horse.pkData.pdam-
		(int)skillValue.dpdamp)/100.0f)+
		skillValue.introject_maxpdam+skillValue.updam +
		skillValue.theurgy_updam+skillValue.lupdam+
		skillValue.pupdam+skillValue.spupdam+
		skillValue.supdam+skillValue.pdeftodam+
		(int)skillValue.sword_udam-skillValue.pdamtodef+
		skillValue.rpupdam-
		skillValue.dpdam-
		skillValue.theurgy_dpdam+packs.equip.getEquips().get_maxpdamage()*
		(skillValue.weaponupdamp/100.0f));

	DWORD AttPlus = 0;
	DWORD DefPlus = 0;


	charstate.maxpdamage = (temp<0?0:temp)+AttPlus - (this->dropweapon?getWeaponPower(1):0);

	temp = (SDWORD)((charstate.mdamage+packs.equip.getEquips().get_maxmdamage())*(1+packs.equip.getEquips().get_mdam()/100.0f)*
		(1.0f+((int)skillValue.umdamp+(int)skillValue.pumdamp+(int)skillValue.theurgy_umdamp+(int)skillValue.array_udamp+(int)skillValue.sept_umdamp+horse.pkData.mdam-(int)skillValue.dmdamp)/100.0f)+skillValue.introject_maxmdam+skillValue.umdam+skillValue.theurgy_umdam+skillValue.mdeftodam-skillValue.mdamtodef+
		skillValue.pumdam-skillValue.dmdam-skillValue.theurgy_dmdam+packs.equip.getEquips().get_maxmdamage()*(skillValue.weaponumdamp/100.0f));
	charstate.maxmdamage = (temp<0?0:temp) - (this->dropweapon?getWeaponPower(3):0);

	temp = (SDWORD)((charstate.pdamage+packs.equip.getEquips().get_pdamage())*(1+packs.equip.getEquips().get_pdam()/100.0f)*
		(1.0f+((int)skillValue.updamp+(int)skillValue.theurgy_updamp+(int)skillValue.lm_updamp+(int)skillValue.nsc_updamp+(int)skillValue.array_udamp+(int)skillValue.sept_updamp+(int)skillValue.spupdamp+horse.pkData.pdam-(int)skillValue.dpdamp)/100.0f)+skillValue.introject_pdam+skillValue.updam+
		skillValue.theurgy_updam+skillValue.lupdam+skillValue.pupdam+skillValue.spupdam+skillValue.supdam+skillValue.pdeftodam+(int)skillValue.sword_udam-skillValue.pdamtodef
		+skillValue.rpupdam-skillValue.dpdam-skillValue.theurgy_dpdam+packs.equip.getEquips().get_pdamage()*(skillValue.weaponupdamp/100.0f));
	charstate.pdamage = (temp<0?0:temp) - (this->dropweapon?getWeaponPower(0):0);

	temp = (SDWORD)((charstate.mdamage+packs.equip.getEquips().get_mdamage())*(1+packs.equip.getEquips().get_mdam()/100.0f)*
		(1.0f+((int)skillValue.umdamp+(int)skillValue.pumdamp+(int)skillValue.theurgy_umdamp+(int)skillValue.array_udamp+(int)skillValue.sept_umdamp+horse.pkData.mdam-(int)skillValue.dmdamp)/100.0f)+skillValue.introject_mdam+skillValue.umdam+skillValue.theurgy_umdam+skillValue.mdeftodam-skillValue.mdamtodef+
		skillValue.pumdam-skillValue.dmdam-skillValue.theurgy_dmdam+packs.equip.getEquips().get_mdamage()*(skillValue.weaponumdamp/100.0f));
	charstate.mdamage = (temp<0?0:temp) - (this->dropweapon?getWeaponPower(2):0);


	temp = (SDWORD)((charstate.mdefence+packs.equip.getEquips().get_mdefence())*(1+packs.equip.getEquips().get_mdef()/100.0f)*
		(1.0f+((int)skillValue.umdefp+(int)skillValue.theurgy_umdefp+(int)skillValue.ice_umdefp+(int)skillValue.sept_umdefp+horse.pkData.mdef-(int)skillValue.array_ddefp-(int)skillValue.dmdefp)/100.0f)+skillValue.introject_mdef+skillValue.umdef+skillValue.theurgy_umdef+
		skillValue.pumdef+skillValue.udef-skillValue.dmdef-skillValue.theurgy_dmdef+skillValue.pdamtodef-skillValue.pdeftodam);
	charstate.mdefence = (temp<0?0:temp);
	if (this->issetUState(Cmd::USTATE_SITDOWN)) charstate.mdefence = (DWORD)(((float)(charstate.mdefence))*0.6f);

	temp = (SDWORD)((charstate.pdefence+packs.equip.getEquips().get_pdefence())*(1+packs.equip.getEquips().get_pdef()/100.0f)*
		(1.0f+((int)skillValue.updefp+(int)skillValue.tgyt_updefp+(int)skillValue.sept_updefp+(int)skillValue.theurgy_updefp+horse.pkData.pdef-(int)skillValue.array_ddefp-(int)skillValue.dpdefp)/100.0f)+skillValue.introject_pdef+skillValue.updef+skillValue.theurgy_updef+
		skillValue.pupdef+skillValue.udef+(int)skillValue.tgzh_updef-skillValue.dpdef-skillValue.theurgy_dpdef+skillValue.mdamtodef-skillValue.mdeftodam);
	charstate.pdefence = (temp<0?0:temp);
	if (this->issetUState(Cmd::USTATE_SITDOWN)) charstate.pdefence = (DWORD)(((float)(charstate.pdefence))*0.6f);

	charstate.resumehp    =  (DWORD)((charstate.resumehp*(skillValue.hpspeedup==0?1:(1+skillValue.hpspeedup/100.0f)) + packs.equip.getEquips().get_hpr())*(skillValue.enervation==0?1:(skillValue.enervation/100.0f)));
	charstate.resumemp    =  (DWORD)((charstate.resumemp*(skillValue.mpspeedup==0?1:(1+skillValue.mpspeedup/100.0f)) + packs.equip.getEquips().get_mpr())*(skillValue.enervation==0?1:(skillValue.enervation/100.0f)));
	charstate.resumesp    =  (DWORD)((charstate.resumesp*(skillValue.spspeedup==0?1:(1+skillValue.spspeedup/100.0f)) + packs.equip.getEquips().get_spr())*(skillValue.enervation==0?1:(skillValue.enervation/100.0f)));


	if ((int)charstate.attackspeed - (int)packs.equip.getEquips().get_akspeed() - (int)skillValue.pattackspeed - (int)(charstate.wdMen*0.4f)>0)
	{
		charstate.attackspeed= (WORD)((int)charstate.attackspeed - (int)packs.equip.getEquips().get_akspeed() - (int)skillValue.pattackspeed - (int)(charstate.wdMen*0.4f));
		charstate.attackspeed = (WORD)(charstate.attackspeed*(1+((int)skillValue.dattackspeed+(int)skillValue.enervation-(int)skillValue.uattackspeed)/100.0f));
	}
	else
	{
		charstate.attackspeed = 0;
	}

	if ((int)charstate.movespeed -  (int)packs.equip.getEquips().get_mvspeed()>=0)
	{
		charstate.movespeed  =  (WORD)(((int)charstate.movespeed - (int)packs.equip.getEquips().get_mvspeed())*(skillValue.enervation==0?1:(1+skillValue.enervation/100.0f)));
	}
	else
	{
		charstate.movespeed  = 0;
	}

	if (horse.pkData.pdam) {
		charstate.maxpdamage += (DWORD)(charstate.maxpdamage * (horse.pkData.pdam/100.0f)); //最大物理攻击力
		charstate.pdamage    += (DWORD)(charstate.pdamage * (horse.pkData.pdam/100.0f));    //最小物理攻击力
	}

	if (horse.pkData.pdef) {
		charstate.pdefence += (DWORD)(charstate.pdefence * (horse.pkData.pdef/100.0f)); //物理防御力
	}

	if (horse.pkData.mdam) {
		charstate.maxmdamage += (DWORD)(charstate.maxmdamage * (horse.pkData.mdam/100.0f)); //最大魔法攻击力
		charstate.mdamage    += (DWORD)(charstate.mdamage * (horse.pkData.mdam/100.0f));    //最小魔法攻击力
	}

	if (horse.pkData.mdef) {
		charstate.mdefence += (DWORD)(charstate.mdefence * (horse.pkData.mdef/100.0f)); //最大魔法防御力
	}

	if (horse.pkData.maxhp) {
		charstate.maxhp += (DWORD)(horse.pkData.maxhp); //最大生命值
	}

	if (horse.pkData.maxmp) {
		charstate.maxmp += (DWORD)(horse.pkData.maxmp); //最大法力值
	}

	//高级战马
	/*if (horse.pkData.speed)
	{
		charstate.movespeed -= horse.pkData.speed;
	}*/

	//TODO 加入装备魅力值
	setupCharm();
	//charstate.lucky=charbase.lucky;


	/*
	命中率={98+（攻击方等级 – 防御方等级）*0.5+装备增加或减少（此时为 -）命中点数+技能增加或减少（此时为 -）命中点数）}/100
	*/
	charstate.attackrating=98+packs.equip.getEquips().get_atrating() +skillValue.atrating + skillValue.satrating + skillValue.patrating - skillValue.reduce_atrating ;

	/*
	闪避率={10+（防御方等级 – 攻击方等级）*2+装备增加闪避点数+技能增加或减少（此时为 -）闪避点数）}/100
	*/
	charstate.attackdodge=10+packs.equip.getEquips().get_akdodge()+skillValue.akdodge - skillValue.reduce_akdodge;
	if (charstate.attackdodge >25) charstate.attackdodge = 25;
	charstate.bang = charstate.bang + packs.equip.getEquips().get_bang() + skillValue.bang;

	if (this->issetUState(Cmd::USTATE_TOGETHER_WITH_DRAGON))
	{//龙精附体,暴击提高100%
		charstate.bang = charstate.bang + charstate.bang;
	}

	//签名套装
	int qianmin = 0;
	int lanzhuan = 0;
	int huanzhuan = 0;
	int lvzhuan  = 0;

	for(int i = 0; i < 16;++i)
	{
		zObject *Objdata = packs.equip.getObjectByEquipNo((EquipPack::EQUIPNO)i);
		if( Objdata == NULL )
		{
			continue;
		}
		Zebra::logger->error("Objdata->data.kind:%d", Objdata->data.kind);
		if( Objdata->data.maker[0] != '\0')
		{
			qianmin++;
			if( Objdata->data.kind & 4 )
			{
				lvzhuan++;
			}
			else if( Objdata->data.kind & 2 )
			{
				huanzhuan++;
			}
			else if( Objdata->data.kind & 1 )
			{
				lanzhuan++;
			}
		}
	}

	if( qianmin > 5 )
	{
		int a = lanzhuan > huanzhuan? lanzhuan:huanzhuan;
		a = a > lvzhuan? a:lvzhuan;
		if( a > 5 )
		{
			if( a < 8 )
			{
				Zebra::logger->debug("地之圣威效果");
				charstate.maxhp += (DWORD)( charstate.maxhp * (4/100.0f)); //最大生命值
				charstate.maxmp += (DWORD)( charstate.maxmp * (4/100.0f) ); //最大法力值
				charstate.pdefence += (DWORD)( charstate.pdefence * (4/100.0f) ); //物理防御力
				charstate.maxpdamage += (DWORD)( charstate.maxpdamage * (2/100.0f) ); //最大物理攻击力
				charstate.pdamage    += (DWORD)( charstate.pdamage * (2/100.0f) );    //最小物理攻击力
				if( charbase.bitmask & CHARBASE_TIANTAO )
					charbase.bitmask &= ( ~CHARBASE_TIANTAO );

				if( charbase.bitmask & CHARBASE_SHENTAO )
					charbase.bitmask &= ( ~CHARBASE_SHENTAO );

				charbase.bitmask    |= CHARBASE_DITAO;
			}
			else if( ( a > 7 ) && ( a < 10 ) )
			{
				Zebra::logger->debug("天之圣威效果");
				charstate.maxhp += (DWORD)( charstate.maxhp * (8/100.0f)); //最大生命值
				charstate.maxmp += (DWORD)( charstate.maxmp * (8/100.0f) ); //最大法力值
				charstate.pdefence += (DWORD)( charstate.pdefence * (8/100.0f) ); //物理防御力
				charstate.maxpdamage += (DWORD)( charstate.maxpdamage * (5/100.0f) ); //最大物理攻击力
				charstate.pdamage    += (DWORD)( charstate.pdamage * (5/100.0f) );    //最小物理攻击力
				if( charbase.bitmask & CHARBASE_DITAO )
					charbase.bitmask &= ( ~CHARBASE_DITAO );

				if( charbase.bitmask & CHARBASE_SHENTAO )
					charbase.bitmask &= ( ~CHARBASE_SHENTAO );

				charbase.bitmask    |= CHARBASE_TIANTAO;
			}
			else if( a >= 10 )
			{
				Zebra::logger->debug("神之圣威效果");
				charstate.maxhp += (DWORD)( charstate.maxhp * (8/100.0f)); //最大生命值
				charstate.maxmp += (DWORD)( charstate.maxmp * (8/100.0f) ); //最大法力值
				charstate.pdefence += (DWORD)( charstate.pdefence * (8/100.0f) ); //物理防御力
				charstate.maxpdamage += (DWORD)( charstate.maxpdamage * (10/100.0f) ); //最大物理攻击力
				charstate.pdamage    += (DWORD)( charstate.pdamage * (10/100.0f) );    //最小物理攻击力
				if( charbase.bitmask & CHARBASE_DITAO )
					charbase.bitmask &= ( ~CHARBASE_DITAO );

				if( charbase.bitmask & CHARBASE_TIANTAO )
					charbase.bitmask &= ( ~CHARBASE_TIANTAO );

				charbase.bitmask    |= CHARBASE_SHENTAO;
			}
		}
	}
	else
	{
		if( charbase.bitmask & CHARBASE_DITAO )
			charbase.bitmask &= ( ~CHARBASE_DITAO );

		if( charbase.bitmask & CHARBASE_TIANTAO )
			charbase.bitmask &= ( ~CHARBASE_TIANTAO );

		if( charbase.bitmask & CHARBASE_SHENTAO )
			charbase.bitmask &= ( ~CHARBASE_SHENTAO );
	}

	//设置最大经验值
	zExperienceB *base_exp = experiencebm.get(charbase.level);
	if (base_exp)
	{
		charstate.nextexp = base_exp->nextexp;
	}

	charstate.attackfive = (BYTE)this->getFivePoint();//packs.equip.getEquips().getAttFive();
	charstate.defencefive = (BYTE)this->getFivePoint();//packs.equip.getEquips().getDefFive();

	using namespace Cmd;
	if (charbase.hp>charstate.maxhp) charbase.hp = charstate.maxhp;
	if (charbase.mp>charstate.maxmp) charbase.mp = charstate.maxmp;

	packs.equip.needRecalc=false;

	Zebra::logger->debug("马的速度加成:%d——人物移动速度:%u",horse.pkData.speed,getMyMoveSpeed() );

	//装备改变攻击力预处理
	calPreValue();
#endif
}

/**
* \brief 设置魅力值
*
*/
void SceneUser::setupCharm()
{
	charstate.charm = (WORD)(charbase.level / 15);
}

/**
* \brief 计算魔法、物理的伤害和防御值
*
* 通过装备等信息,计算出相关的伤害和防御值
*
*/
void SceneUser::calPreValue()
{
#if 0
	//装备改变攻击力预处理
	pkpreValue.fiveexpress = 1 + (float)(charstate.attackfive/100.0f);
	float five_def_express = 1 + (float)(charstate.defencefive/100.0f);

	pkpreValue.fivedam = (WORD)(charstate.pdamage * pkpreValue.fiveexpress); 
	pkpreValue.fivemaxdam = (WORD)(charstate.maxpdamage  * pkpreValue.fiveexpress);

	pkpreValue.nofivedam = charstate.pdamage;
	pkpreValue.nofivemaxdam = charstate.maxpdamage;

	pkpreValue.fivedef  = (WORD)(charstate.pdefence * five_def_express);
	pkpreValue.nofivedef = charstate.pdefence;

	pkpreValue.fivemdam = (WORD)(charstate.mdamage * pkpreValue.fiveexpress); 
	pkpreValue.fivemaxmdam = (WORD)(charstate.maxmdamage  * pkpreValue.fiveexpress);

	pkpreValue.nofivemdam = charstate.mdamage;
	pkpreValue.nofivemaxmdam = charstate.maxmdamage;

	pkpreValue.fivemdef  = (WORD)(charstate.mdefence * five_def_express);
	pkpreValue.nofivemdef = charstate.mdefence;

#ifdef _DEBUG 
	Zebra::logger->debug("calPreValue():fiveexpress=%f",pkpreValue.fiveexpress);
	Zebra::logger->debug("calPreValue():five_def_express=%f",five_def_express);
#endif
#endif
}


/**
* \brief 打包发送玩家的所有物品信息给自己
*
*
*/
class sendAllObjectListToUser:public UserObjectExec
{
public:
	SceneUser *pUser;
	BYTE action;
	char buffer[zSocket::MAX_USERDATASIZE];
#ifndef _MOBILE
	Cmd::stAddObjectListPropertyUserCmd *send;
	sendAllObjectListToUser(SceneUser *u,BYTE act):pUser(u),action(act)
	{
		send = (Cmd::stAddObjectListPropertyUserCmd *)buffer;
		constructInPlace(send);
	}
	bool exec(zObject *object)
	{
		if (sizeof(Cmd::stAddObjectListPropertyUserCmd) + (send->num + 1) * sizeof(send->list[0]) >= sizeof(buffer))
		{
			pUser->sendCmdToMe(send,sizeof(Cmd::stAddObjectListPropertyUserCmd) + send->num * sizeof(send->list[0]));
			send->num=0;
		}
		bcopy(&object->data,&send->list[send->num].object,sizeof(t_Object));
		send->num++;
		return true;
	}
#else
	Cmd::stAddMobileObjectListPropertyUserCmd *send;
	sendAllObjectListToUser(SceneUser *u,BYTE act):pUser(u),action(act)
	{
		send = (Cmd::stAddMobileObjectListPropertyUserCmd *)buffer;
		constructInPlace(send);
	}
	bool exec(zObject *object)
	{
	    if (sizeof(Cmd::stAddMobileObjectListPropertyUserCmd) + (send->num + 1) * sizeof(send->list[0]) >= sizeof(buffer))
	    {
		pUser->sendCmdToMe(send,sizeof(Cmd::stAddMobileObjectListPropertyUserCmd) + send->num * sizeof(send->list[0]));
		send->num=0;
	    }
	    if(object->fullMobileObject(send->list[send->num].object))
	    {
		send->num++;
	    }
	    return true;
	}
#endif
};
/**
* \brief 发送玩家的所有物品信息给自己
*
*
*/
class sendAllObjectToUser:public UserObjectExec
{
public:
	SceneUser *pUser;
	Cmd::stAddObjectPropertyUserCmd send;
	bool exec(zObject *object)
	{
		bcopy(&object->data,&send.object,sizeof(t_Object));
		pUser->sendCmdToMe(&send,sizeof(send));
		//Zebra::logger->debug("发送物品 %s",object->name);
		return true;
	}
};

void SceneUser::sendAllMobileObjectList()
{
    sendAllObjectListToUser sendao(this,Cmd::EQUIPACTION_INIT);
    packs.uom.execEvery(sendao);
    if (sendao.send->num)
    {
	this->sendCmdToMe(sendao.send,sizeof(Cmd::stAddMobileObjectListPropertyUserCmd) + sendao.send->num * sizeof(sendao.send->list[0]));
	sendao.send->num=0;
    }
}
/*
* \brief 判断相克
* 
* \five 五行
* \return  0,无相克关系,1,克对方,2,对方克自己
*/
int SceneUser::IsOppose(DWORD five)
{
#if 0
	if (this->getFiveType() == SceneUser::Five_Relation[(five + 4)%5])
	{
		return 1;
	}
	else
	{
		if (this->getFiveType() == SceneUser::Five_Relation[(five + 1)%5])
		{
			return 2;
		}
	}

	if ((this->getFiveType() != 5) &&(five == 5))
		return 1;
	if ((this->getFiveType() == 5) &&(five != 5))
		return 2;
#endif
	return 0;
}

/*
* \brief 判断相生
* 
* \five 五行
* \return 是否相生
*/
bool SceneUser::IsJoin(DWORD five)
{
    return true;
//	return five == SceneUser::Five_Relation[(this->getFiveType() + 2)%5] || five == SceneUser::Five_Relation[(this->getFiveType() + 3)%5];
}

#if 0
/**
* \brief 发送玩家的所有技能给玩家自己
*
*/
class sendAllSkillToUser :public UserSkillExec
{
public:
	SceneUser *pUser;
	Cmd::stAddUserSkillPropertyUserCmd ret;
	sendAllSkillToUser(SceneUser *me)
	{
		pUser = me;
	}
	bool exec(zSkill *skill)
	{
		if (skill == NULL)
		{
			return false;
		}
		else
		{
			zRTime ctv;
			DWORD colddown = ctv.msecs() - skill->lastUseTime;
			if (colddown >= skill->base->dtime)
				colddown = 0;
			else
				colddown = skill->base->dtime - colddown;
			ret.dwSkillID = skill->data.skillid;
			ret.wdLevel = skill->data.level;
			ret.wdUpNum = pUser->skillUpLevel(skill->base->skillid,skill->base->kind);
			if (ret.wdUpNum+ret.wdLevel>10) ret.wdUpNum = 10 - ret.wdLevel;
			ret.dwExperience = colddown;
			ret.dwMaxExperience = 0;
			pUser->sendCmdToMe(&ret,sizeof(ret));
		}
		return true;
	}

};
#endif
/**
* \brief 发送玩家所有信息给玩家自己
*
*
*/
void SceneUser::sendInitToMe()
{
	using namespace Cmd;
#ifndef _MOBILE
	//主人物信息
	stMainUserDataUserCmd  userinfo;
	full_t_MainUserData(userinfo.data);
	//Zebra::logger->debug("t_MainUserData %u",sizeof(Cmd::t_MainUserData));
	sendCmdToMe(&userinfo,sizeof(userinfo));
	sendInitHPAndMp();

//	packs.store.notify(this);
#if 0
	//物品信息
	sendAllObjectToUser sendao;
	sendao.send.byActionType=EQUIPACTION_INIT;
	sendao.pUser=this;
	packs.uom.execEvery(sendao);
#endif

	sendAllObjectListToUser sendao(this,EQUIPACTION_INIT);
	packs.uom.execEvery(sendao);
	if (sendao.send->num)
	{
	    this->sendCmdToMe(sendao.send,sizeof(Cmd::stAddObjectListPropertyUserCmd) + sendao.send->num * sizeof(sendao.send->list[0]));
	    sendao.send->num=0;
	}
#endif
	//技能信息
//	sendAllSkillToUser sexec(this);
//	usm.execEvery(sexec);

	//quest info
//	Quest::notify(*this);

	//快捷键信息
	char Buf[1024];
	bzero(Buf,1024);

	Cmd::stAccekKeyPropertyUserCmd *acc = (Cmd::stAccekKeyPropertyUserCmd *)Buf;//&accelData;
	constructInPlace(acc);
	Cmd::stAccekKeyPropertyUserCmd *acc_1 = (Cmd::stAccekKeyPropertyUserCmd *)accelData;
	acc->accelNum = acc_1->accelNum;
	acc->activeGroup = acc_1->activeGroup;
	int len = sizeof(Cmd::stAccekKeyPropertyUserCmd) + acc->accelNum * sizeof(Cmd::stGameAccelKey);
	if (acc->accelNum > 0 && len < 1024)
	{
		memcpy(acc->accelKeys,acc_1->accelKeys,acc->accelNum * sizeof(Cmd::stGameAccelKey));
		sendCmdToMe(acc,len);
	}

	//请求临时档案数据
	Cmd::Session::t_ReqUser_SceneArchive req;
	req.id = this->id;
	req.dwMapTempID = this->scene->tempid;
	sessionClient->sendCmd(&req,sizeof(req));
#if 0
	if (this->safety)
	{
		Cmd::stNotifySafetyUserCmd send;
		send.byState = Cmd::SAFETY_OPEN;
		send.safe_setup = this->safety_setup;
		sendCmdToMe(&send,sizeof(send));
	}
#endif

#ifdef _MOBILE
	Cmd::stMainUserDataUserCmd  userinfo;	//����������
	full_t_MainUserData(userinfo.data);
	Zebra::logger->debug("���ؽ�ɫ������ Ǯ(%u)��ɫ��:%s", userinfo.data.gold, userinfo.data.name);
	sendCmdToMe(&userinfo,sizeof(userinfo));

	sendAllMobileObjectList();

	CardTujianManager::getMe().notifyAllTujianDataToMe(*this);
	GroupCardManager::getMe().notifyAllGroupListToMe(*this);
	HeroInfoManager::getMe().notifyAllHeroInfoToMe(*this);
	Zebra::logger->debug("���ؽ�ɫ������ %s",this->name);
#endif
}


/*
* \brief 通知9屏添加用户
*/
void SceneUser::sendMeToNine()
{
#ifndef _MOBILE
	if (SceneEntry_Hide!=getState() && !this->hideme && !this->Soulflag)
	{//检查是否隐身
		clearUState( Cmd::USTATE_HIDE );
		BUFFER_CMD(Cmd::stAddUserAndPosMapScreenStateUserCmd,send,zSocket::MAX_USERDATASIZE);
		this->full_t_MapUserDataPosState(send->data);
		this->scene->sendCmdToNine(getPosI(),send,send->size(),dupIndex);
		this->setStateToNine(Cmd::USTATE_WAR);
	}
	else
	{
		BUFFER_CMD(Cmd::stAddUserAndPosMapScreenStateUserCmd,send,zSocket::MAX_USERDATASIZE);
		this->full_t_MapUserDataPosState(send->data);
		sendCmdToMe(send,send->size());
	}
#if 0
	if (privatestore.step() == PrivateStore::BEGIN)
	{	
		BYTE adv[zSocket::MAX_DATASIZE];
		Cmd::stUpdateShopAdvcmd* pBuf = (Cmd::stUpdateShopAdvcmd*)adv;
		constructInPlace(pBuf);
		pBuf->size = 1;
		pBuf->Datas[0].dwID = tempid;
		strncpy(pBuf->Datas[0].strShopAdv, ShopAdv, MAX_SHOPADV);
		sendCmdToMe(pBuf, sizeof(Cmd::stUpdateShopAdvcmd));	
	}
#endif
#endif
}

#if 0
struct SendStateWarToNineEveryOne : public zSceneEntryCallBack
{
private:
	SceneUser *pUser;
	Cmd::stSetStateMapScreenUserCmd send;
public:
	SendStateWarToNineEveryOne(SceneUser *pUser,WORD state) : pUser(pUser)
	{
		send.dwTempID = pUser->tempid;
		send.type=Cmd::MAPDATATYPE_USER;
		send.wdState =state;
	}
	bool exec(zSceneEntry *entry)
	{
		if (pUser && entry && entry->dupIndex == pUser->dupIndex)
		{
			if (pUser->isWar((SceneUser*)entry))
			{
#if _DEBUG
				Zebra::logger->info("角色%s将自己的对战状态发给了%s,%u",pUser->name,entry->name,send.wdState);
#endif
				((SceneUser*)entry)->sendCmdToMe(&send,sizeof(send));
			}
		}
		return true;
	}
};
#endif

void SceneUser::sendMeToNineDirect(const int direct)
{
#if 0
	if (SceneEntry_Hide!=getState()&& !this->hideme && !Soulflag)
	{
		BUFFER_CMD(Cmd::stAddUserAndPosMapScreenStateUserCmd,send,zSocket::MAX_USERDATASIZE);
		this->full_t_MapUserDataPosState(send->data);
		this->scene->sendCmdToDirect(getPosI(),direct,send,send->size(),dupIndex);

		Cmd::stSetStateMapScreenUserCmd send1;
		send1.type=Cmd::MAPDATATYPE_USER;
		send1.dwTempID = this->tempid;
		send1.wdState =Cmd::USTATE_WAR;
		SendStateWarToNineEveryOne one(this,send1.wdState);
		const zPosIVector &pv = scene->getDirectScreen(getPosI(),direct);
		for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
		{
			scene->execAllOfScreen(zSceneEntry::SceneEntry_Player,*it,one);
		}
	}
	else
	{
		BUFFER_CMD(Cmd::stAddUserAndPosMapScreenStateUserCmd,send,zSocket::MAX_USERDATASIZE);
		this->full_t_MapUserDataPosState(send->data);
		sendCmdToMe(send,send->size());
		this->scene->sendCmdToWatchTrap(getPosI(),send,send->size());
	}
#endif
}

#if 0
bool SceneUser::isSpecWar(DWORD dwType)
{
	WarIter iter;
	for (iter = vWars.begin(); iter!=vWars.end(); iter++)
	{       
		if (iter->type == dwType || (dwType == Cmd::COUNTRY_FORMAL_DARE && iter->type==Cmd::COUNTRY_FORMAL_ANTI_DARE))
		{
			break;
		} 
	}       

	if (iter!=vWars.end())
	{       
		return true;
	}

	return false;
}

/**
* \brief 判断自己与某玩家是否处于对战状态
*
*
* \param entry 对方玩家
*
* \return 只要与对方玩家处于（帮会战,师门战,家族战）之一时,返回TRUE,否则为FALSE
*
*/
bool SceneUser::isWar(SceneUser* entry)
{
	if (this == entry)
	{// 过滤自己
		return false;
	}

	WarIter iter;
	for (iter = vWars.begin(); iter!=vWars.end(); iter++)
	{       
		if (entry->charbase.septid == iter->relationid && (iter->type == Cmd::SEPT_DARE))
		{//判断有没有家族战
			return true;
		}

		if (entry->charbase.septid == iter->relationid && (iter->type == Cmd::SEPT_NPC_DARE))
		{//判断有没有家族NPC争夺战
			if (entry->npcdareflag && this->npcdareflag) return true;
		}

		if (entry->charbase.unionid == iter->relationid && 
			(iter->type == Cmd::UNION_DARE || iter->type == Cmd::UNION_CITY_DARE))
		{//判断有没有帮会战,帮会夺城战
			return true;
		}

		if (entry->charbase.country == iter->relationid && 
			(iter->type == Cmd::COUNTRY_FORMAL_DARE || iter->type == Cmd::COUNTRY_FORMAL_ANTI_DARE))
		{//判断有没有正式国战,国战反攻
			return true;
		}
	}       

	return false;
}

struct GetEnvryOneAddSeptNormalExp : public zSceneEntryCallBack
{
private:
	DWORD dwSeptID;
	DWORD counter;

public:
	bool  isTotal;

	GetEnvryOneAddSeptNormalExp(DWORD septid)
	{
		dwSeptID = septid;
		isTotal = true;
		counter = 0;
	}

	inline void total()
	{//统计九屏内该家族的人数
		counter++;
	}

	bool exec(zSceneEntry *entry)
	{
		if (entry && dwSeptID==((SceneUser*)entry)->charbase.septid)
		{
			if (isTotal)
			{
				this->total();
			}
			else
			{
				SceneUser* pUser = (SceneUser*)entry;
				//10*(1.4*主人角色当前等级^2+5*主人角色当前等级)   
				DWORD exp = 0;

				if (pUser->septMaster)
				{
					exp = (DWORD)((50+3*counter) * pUser->charbase.level*pUser->charbase.level);
				}
				else
				{
					exp = (DWORD)((25+3*counter)*pUser->charbase.level*pUser->charbase.level);
				}

				pUser->addExp(exp);

				Channel::sendSys(pUser,Cmd::INFO_TYPE_EXP,"您周围有 %u 个族员,领取到家族经验 %d",
					counter,exp);
				Zebra::logger->info("[家族]: %s(%u) 领取家族经验 %d",pUser->name,pUser->id,exp);
			}
		}

		return true;
	}
};

struct GetEnvryOneAddSeptExp : public zSceneEntryCallBack
{
private:
	DWORD dwSeptID;

public:
	GetEnvryOneAddSeptExp(DWORD septid)
	{
		dwSeptID = septid;
	}

	bool exec(zSceneEntry *entry)
	{
		if (entry && dwSeptID==((SceneUser*)entry)->charbase.septid)
		{
			SceneUser* pUser = (SceneUser*)entry;
			//10*(1.4*主人角色当前等级^2+5*主人角色当前等级)   
			DWORD exp = (DWORD)(10*(1.4*pow((double)pUser->charbase.level,2)+5*pUser->charbase.level));
			pUser->addExp(exp);
			Channel::sendSys(pUser,Cmd::INFO_TYPE_EXP,"领取到家族经验 %d",exp);

			Zebra::logger->info("[家族]: %s(%u) 领取家族经验 %d",pUser->name,pUser->id,exp);
		}

		return true;
	}
};

#endif

typedef std::vector<SceneNpc * > SceneNpc_vec;
const int temp__ = zSocket::MAX_USERDATASIZE; 

const DWORD SCENE_PACKET_USERDATASIZE = temp__/4;

struct GetEnvryOneAndSend : public zSceneEntryCallBack
{
private:
	SceneUser *pUser;
public:
	static BYTE _buf__map_user[SCENE_PACKET_USERDATASIZE];
	static BYTE _buf_map_npc[SCENE_PACKET_USERDATASIZE];
	static SceneNpc_vec _npc_vec;
#if 0
	static BYTE _buf_map_npc[SCENE_PACKET_USERDATASIZE];
	static BYTE _buf_map_shopAdv[zSocket::MAX_USERDATASIZE];		//Shx Add
	static BYTE _buf_map_ghost[zSocket::MAX_USERDATASIZE];
	static SceneNpc_vec _npc_vec;
	//static BYTE _buf_pos_user[SCENE_PACKET_USERDATASIZE];
	//static BYTE _buf_pos_npc[SCENE_PACKET_USERDATASIZE];
	static BYTE petBuf[zSocket::MAX_USERDATASIZE];
#endif
	Cmd::stMapDataMapScreenUserCmd *_map_user; 
	Cmd::stMapDataMapScreenUserCmd *_map_npc;
#if 0
	Cmd::stUpdateShopAdvcmd * _map_shopAdv;	//Shx Add

	//Cmd::stAllMapScreenUserCmd *_pos_user; 
	//Cmd::stAllMapScreenUserCmd *_pos_npc; 
	Cmd::stMapDataMapScreenUserCmd *_map_pet;
	Cmd::stMapDataMapScreenUserCmd * _map_ghost;
#endif
	void initMapUser()
	{
		_map_user = (Cmd::stMapDataMapScreenUserCmd *)_buf__map_user;
		constructInPlace(_map_user);
		_map_user->mdih.type = Cmd::MAPDATATYPE_USER;
		_map_user->mdih.size=0;
	}
	void initmapNpc()
	{
		_map_npc = (Cmd::stMapDataMapScreenUserCmd *)_buf_map_npc;
		constructInPlace(_map_npc);
		_map_npc->mdih.type = Cmd::MAPDATATYPE_NPC;
		_map_npc->mdih.size=0;
		_map_npc->mdih.oneSize= sizeof(Cmd::t_MapNpcDataPos);
		_npc_vec.clear();
	}
	void initShopAdv()		//Shx Add 
	{
#if 0
		_map_shopAdv = (Cmd::stUpdateShopAdvcmd *)_buf_map_shopAdv;
		constructInPlace(_map_shopAdv);
		_map_shopAdv->size = 0;
#endif
	}
	void initPet()
	{
#if 0
		_map_pet = (Cmd::stMapDataMapScreenUserCmd *)petBuf;
		constructInPlace(_map_pet);
		_map_pet->mdih.type = Cmd::MAPDATATYPE_PET;
		_map_pet->mdih.size=0;
#endif
	}
	void initMapGhost()
	{
#if 0
		_map_ghost = (Cmd::stMapDataMapScreenUserCmd *)_buf_map_ghost;
		constructInPlace(_map_ghost);
		_map_ghost->mdih.type = Cmd::MAPDATATYPE_USER;
		_map_ghost->mdih.size=0;
#endif
	}
	/*
	void initPosUser()
	{
	//bzero(_buf_pos_user,sizeof(_buf_pos_user));
	_pos_user = (Cmd::stAllMapScreenUserCmd *)_buf_pos_user;
	constructInPlace(_pos_user);
	_pos_user->mdih.type = Cmd::MAPDATATYPE_USER;
	_pos_user->mdih.size=0;
	}
	void initPosNpc()
	{
	//bzero(_buf_pos_npc,sizeof(_buf_pos_npc));
	_pos_npc = (Cmd::stAllMapScreenUserCmd *)_buf_pos_npc;
	constructInPlace(_pos_npc);
	_pos_npc->mdih.type = Cmd::MAPDATATYPE_NPC;
	_pos_npc->mdih.size=0;
	}
	// */
public:
	GetEnvryOneAndSend(SceneUser *pUser) : pUser(pUser) 
	{
		initMapUser();
		initmapNpc();
		initShopAdv();	//shx Add

		initPet();
		initMapGhost();
		//initPosUser();
		//initPosNpc();
	}
	bool exec(zSceneEntry *entry)
	{
		if (pUser && entry /*&& entry->dupIndex == pUser->dupIndex*/)
		{
			switch(entry->getType())
			{
			case zSceneEntry::SceneEntry_Player:
				{
					if ((entry->getState() == zSceneEntry::SceneEntry_Normal
						|| entry->getState() == zSceneEntry::SceneEntry_Death)
						&& (!((SceneEntryPk *)entry)->hideme
						|| pUser->watchTrap))
					{
						if (user_outofbound())
						{
							pUser->sendCmdToMe(user_getSendBuffer(),user_getSize());
							initMapUser();
						}
						//Cmd::stAddUserMapScreenUserCmd cmd;
						//((SceneUser *)entry)->full_t_MapUserData(cmd.data);
						//_map_user->mud[_map_user->mdih.size] 
						((SceneUser *)entry)->full_t_MapUserData(_map_user->mud[_map_user->mdih.size]);
						_map_user->mud[_map_user->mdih.size].x=entry->getPos().x;
						_map_user->mud[_map_user->mdih.size].y=entry->getPos().y;
						_map_user->mud[_map_user->mdih.size].byDir=entry->getDir();
#if 0

						if (pUser->isWar((SceneUser*)entry))
						{
							set_state(_map_user->mud[_map_user->mdih.size].state,Cmd::USTATE_WAR);
						}
						else
						{
							clear_state(_map_user->mud[_map_user->mdih.size].state,Cmd::USTATE_WAR);
						}
#endif
						_map_user->mdih.size++;
#if 0
						if (user_outofbound())
						{
							pUser->sendCmdToMe(ShopAdv_GetSendBuffer(),ShopAdv_getSize());
							initShopAdv();
						}
						//[Shx Add 发送广告给他人]
						if (((SceneUser*)entry)->privatestore.step() == PrivateStore::BEGIN) 
						{	
							if(strlen(((SceneUser*)entry)->ShopAdv) > 0)
							{
								int Index = _map_shopAdv->size;
								_map_shopAdv->Datas[Index].dwID = ((SceneUser*)entry)->tempid;
								strncpy(_map_shopAdv->Datas[Index].strShopAdv, ((SceneUser*)entry)->ShopAdv, MAX_SHOPADV);
								_map_shopAdv->size ++;
							}
						}
#endif
					}
				}
				break;
			case zSceneEntry::SceneEntry_NPC:
				{
					SceneNpc *sceneNpc=(SceneNpc *)entry;
					if (entry->getState() == zSceneEntry::SceneEntry_Normal
						|| (entry->getState() == zSceneEntry::SceneEntry_Death
						&& !sceneNpc->isUse))
					{
#if 0
						if( sceneNpc->npc->kind == NPC_TYPE_GHOST )   //sky 元神处理
						{
							SceneUser * master = (SceneUser *)(((ScenePet *)entry)->getMaster());
							if(master)
							{
								((SceneGhost *)entry)->full_t_MapUserData( _map_ghost->mud[_map_ghost->mdih.size], master );

								_map_ghost->mud[_map_ghost->mdih.size].x = entry->getPos().x;
								_map_ghost->mud[_map_ghost->mdih.size].y = entry->getPos().y;
								_map_ghost->mud[_map_ghost->mdih.size].byDir = entry->getDir();
								_map_ghost->mdih.size++;
								pUser->sendCmdToMe(ghost_getSendBuffer(),ghost_getSize());
								initMapGhost();
							}
						}
						else
#endif
						{
							if (sceneNpc->npc->kind == NPC_TYPE_TRAP)
							{
								if (sceneNpc->getMaster() != pUser && !pUser->watchTrap) break;
							}

							if (sceneNpc->isTaskNpc()) 
							{
								sceneNpc->set_quest_status(pUser); //填充任务状态
							}
							if (npc_outofbound())
							{
								pUser->sendCmdToMe(npc_getSendBuffer(),npc_getSize());
								if (!_npc_vec.empty())
								{
									for(SceneNpc_vec::iterator iter = _npc_vec.begin() ; iter != _npc_vec.end() ; iter ++)
									{
										(*iter)->showHP(pUser,(*iter)->hp);
									}
								}
								initmapNpc();
							}
							sceneNpc->full_t_MapNpcData(_map_npc->mnd[_map_npc->mdih.size]);
							_map_npc->mnd[_map_npc->mdih.size].x=entry->getPos().x;
							_map_npc->mnd[_map_npc->mdih.size].y=entry->getPos().y;
							_map_npc->mnd[_map_npc->mdih.size].byDir=entry->getDir();
							_map_npc->mdih.size++;
#if 0
							if (sceneNpc->isAttackMe(pUser))
							{
								_npc_vec.push_back(sceneNpc); 
							}

							if (sceneNpc->getPetType()!=Cmd::PET_TYPE_NOTPET
								&& sceneNpc->getPetType()!=Cmd::PET_TYPE_SEMI
								&& !sceneNpc->needClear())
							{
								if (pet_outofbound())
								{
									pUser->sendCmdToMe(pet_getSendBuffer(),pet_getSize());
									initPet();
								}
								((ScenePet *)entry)->full_t_MapPetData(_map_pet->mpd[_map_pet->mdih.size]);
								_map_pet->mdih.size++;
							}
#endif
						}

					}
				}
				break;
			case zSceneEntry::SceneEntry_Build:
				break;
			case zSceneEntry::SceneEntry_Object:
				{
					zSceneObject *o=(zSceneObject *)entry;
					Cmd::stAddMapObjectMapScreenUserCmd add;

					add.action = Cmd::OBJECTACTION_UPDATE;
					add.data.dwMapObjectTempID=entry->id;
					add.data.dwObjectID=entry->tempid;
					//strncpy(add.data.pstrName,entry->name,MAX_NAMESIZE);
					add.data.x=entry->getPos().x;
					add.data.y=entry->getPos().y;
					add.data.wdNumber=o->getObject()->data.dwNum;
					//add.data.wdLevel=o->getObject()->base->level;
					//add.data.upgrade = o->getObject()->data.upgrade;
					add.data.kind = o->getObject()->data.kind;              
					pUser->sendCmdToMe(&add,sizeof(add));
				}
				break;
			case zSceneEntry::SceneEntry_MAX:
				break;
			default:
				break;
			}
		}

		return true;
	}
#if 0
	//[Shx Add] 还是摆摊广告...
	inline const bool ShopAdv_canSend() const
	{
		return _map_shopAdv->size > 0;
	}
	inline const int ShopAdv_getSize() const
	{
		return sizeof(Cmd::stUpdateShopAdvcmd)
			+ sizeof(Cmd::stUpdateShopAdvcmd::stAdv) *  _map_shopAdv->size;
	}
	inline const BYTE *ShopAdv_GetSendBuffer()
	{
		return (LPBYTE)_map_shopAdv;
	}
	//End Shx
#endif
	inline const bool user_outofbound() const
	{
		return (sizeof(Cmd::stMapDataMapScreenUserCmd)+ sizeof(Cmd::t_MapUserDataPos)*(_map_user->mdih.size+1) + sizeof(Cmd::MapData_ItemHeader)) >= SCENE_PACKET_USERDATASIZE;
	}
	inline const bool user_canSend() const
	{
		return _map_user->mdih.size > 0;
	}
	inline const BYTE *user_getSendBuffer()
	{
		Cmd::MapData_ItemHeader *pheader =
			(Cmd::MapData_ItemHeader *)&_buf__map_user[sizeof(Cmd::stMapDataMapScreenUserCmd) + sizeof(Cmd::t_MapUserDataPos) * _map_user->mdih.size];
		pheader->size = 0; pheader->type = 0;
		return _buf__map_user;
	}
	inline const int user_getSize() const
	{
		return sizeof(Cmd::stMapDataMapScreenUserCmd)
			+ sizeof(Cmd::t_MapUserDataPos) * _map_user->mdih.size + sizeof(Cmd::MapData_ItemHeader);
	}
	inline const bool npc_outofbound() const
	{
		return (sizeof(Cmd::stMapDataMapScreenUserCmd)+ sizeof(Cmd::t_MapNpcDataPos) *(_map_npc->mdih.size+1) + sizeof(Cmd::MapData_ItemHeader)) >= SCENE_PACKET_USERDATASIZE;
	}
	inline const bool npc_canSend() const
	{
		return _map_npc->mdih.size > 0;
	}
	inline const BYTE *npc_getSendBuffer()
	{
		Cmd::MapData_ItemHeader *pheader =
			(Cmd::MapData_ItemHeader *)&_buf_map_npc[sizeof(Cmd::stMapDataMapScreenUserCmd)+ sizeof(Cmd::t_MapNpcDataPos) * _map_npc->mdih.size];
		pheader->size = 0; pheader->type = 0;
		return _buf_map_npc;
	}
	inline const int npc_getSize() const
	{
		return sizeof(Cmd::stMapDataMapScreenUserCmd)
			+ sizeof(Cmd::t_MapNpcDataPos) * _map_npc->mdih.size + sizeof(Cmd::MapData_ItemHeader);
	}
#if 0
	inline const bool pet_outofbound() const
	{
		return _map_pet->mdih.size >= 200;
	}
	inline const bool pet_canSend() const
	{
		return _map_pet->mdih.size > 0;
	}
	inline const BYTE *pet_getSendBuffer()
	{
		Cmd::MapData_ItemHeader *pheader =
			(Cmd::MapData_ItemHeader *)&petBuf[sizeof(Cmd::stMapDataMapScreenUserCmd)+sizeof(Cmd::t_MapPetData)*(_map_pet->mdih.size)];
		pheader->size = 0; pheader->type = 0;
		return petBuf;
	}
	inline const int pet_getSize() const
	{
		return sizeof(Cmd::stMapDataMapScreenUserCmd)
			+ sizeof(Cmd::t_MapPetData) * _map_pet->mdih.size + sizeof(Cmd::MapData_ItemHeader);
	}

	//sky 元神消息相关处理
	inline const bool ghost_outofbound() const
	{
		return (sizeof(Cmd::stMapDataMapScreenUserCmd)+ sizeof(Cmd::t_MapUserDataPos)*(_map_ghost->mdih.size+1) + sizeof(Cmd::MapData_ItemHeader)) >= SCENE_PACKET_USERDATASIZE;
	}

	inline const BYTE *ghost_getSendBuffer()
	{
		Cmd::MapData_ItemHeader *pheader =
			(Cmd::MapData_ItemHeader *)&_buf_map_ghost[sizeof(Cmd::stMapDataMapScreenUserCmd) + sizeof(Cmd::t_MapUserDataPos) * _map_ghost->mdih.size];
		pheader->size = 0; pheader->type = 0;
		return _buf_map_ghost;
	}

	inline const int ghost_getSize() const
	{
		return sizeof(Cmd::stMapDataMapScreenUserCmd)
			+ sizeof(Cmd::t_MapUserDataPos) * _map_ghost->mdih.size + sizeof(Cmd::MapData_ItemHeader);
	}
#endif
};

BYTE GetEnvryOneAndSend::_buf__map_user[SCENE_PACKET_USERDATASIZE];
BYTE GetEnvryOneAndSend::_buf_map_npc[SCENE_PACKET_USERDATASIZE];
SceneNpc_vec GetEnvryOneAndSend::_npc_vec;
#if 0
BYTE GetEnvryOneAndSend::petBuf[zSocket::MAX_USERDATASIZE];
BYTE GetEnvryOneAndSend::_buf_map_ghost[zSocket::MAX_USERDATASIZE];
BYTE GetEnvryOneAndSend::_buf_map_shopAdv[zSocket::MAX_USERDATASIZE];		//Shx Add
#endif
/**
* \brief 发送九屏数据给自己
*
*/
void SceneUser::sendNineToMe()
{
#ifndef _MOBILE
	GetEnvryOneAndSend one(this);
	const zPosIVector &pv = scene->getNineScreen(getPosI());
	for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
	{
		//Zebra::logger->debug("sceneindex=%d,%d",this->getPosI(),*it);
		scene->execAllOfScreen(*it,one);
	}
	if (one.user_canSend())
	{
		this->sendCmdToMe(one.user_getSendBuffer(),one.user_getSize());
		//Zebra::logger->debug("one._map_user->mdih.size=%d",one._map_user->mdih.size);
	}
	if (one.npc_canSend())
	{
		this->sendCmdToMe(one.npc_getSendBuffer(),one.npc_getSize());
		if (!one._npc_vec.empty())
		{
			for(SceneNpc_vec::iterator iter = one._npc_vec.begin() ; iter != one._npc_vec.end() ; iter ++)
			{
				(*iter)->showHP(this,(*iter)->hp);
			}
		}
		//Zebra::logger->debug("one._map_npc->mdih.size=%d",one._map_npc->mdih.size);
	}
#if 0
	//Shx Add 发送周围玩家商店广告
	if (one.ShopAdv_canSend())
	{
		this->sendCmdToMe(one.ShopAdv_GetSendBuffer(), one.ShopAdv_getSize());
	}

	if (one.pet_canSend())
	{
		this->sendCmdToMe(one.pet_getSendBuffer(),one.pet_getSize());
	}
#endif
#endif
}

/**
* \brief 给九屏同家族的所有玩家增加家族经验
*
*/
void SceneUser::addNineSeptNormalExp(DWORD dwSeptID)
{
#if 0
	GetEnvryOneAddSeptNormalExp one(dwSeptID);
	const zPosIVector &pv = scene->getNineScreen(getPosI());

	for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
	{
		scene->execAllOfScreen(SceneEntry_Player,*it,one);
	}

	one.isTotal = false;

	for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
	{
		scene->execAllOfScreen(SceneEntry_Player,*it,one);
	}
#endif
}

/**
* \brief 给九屏同家族的所有玩家增加普通家族经验
*
*/
void SceneUser::addNineSeptExp(DWORD dwSeptID)
{
//	GetEnvryOneAddSeptExp one(dwSeptID);
//	const zPosIVector &pv = scene->getNineScreen(getPosI());
//	for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
//	{
//		scene->execAllOfScreen(SceneEntry_Player,*it,one);
//	}
}

/**
* \brief 发送指定方向五屏数据给自己
*
* \param direct 方向
*/
void SceneUser::sendNineToMeDirect(const int direct)
{
	GetEnvryOneAndSend one(this);
	const zPosIVector &pv = scene->getDirectScreen(getPosI(),direct);
	for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
	{
		scene->execAllOfScreen(*it,one);
	}
	if (one.user_canSend())
	{
		this->sendCmdToMe(one.user_getSendBuffer(),one.user_getSize());
		//Zebra::logger->debug("one._map_user->mdih.size=%d",one._map_user->mdih.size);
	}
	if (one.npc_canSend())
	{
		this->sendCmdToMe(one.npc_getSendBuffer(),one.npc_getSize());
		if (!one._npc_vec.empty())
		{
			for(SceneNpc_vec::iterator iter = one._npc_vec.begin() ; iter != one._npc_vec.end() ; iter ++)
			{
				(*iter)->showHP(this,(*iter)->hp);
			}
		}
		//Zebra::logger->debug("one._map_npc->mdih.size=%d",one._map_npc->mdih.size);
	}
#if 0
	//Shx Add 发送周围玩家商店广告
	if (one.ShopAdv_canSend())
	{
		this->sendCmdToMe(one.ShopAdv_GetSendBuffer(), one.ShopAdv_getSize());
	}

	if (one.pet_canSend())
	{
		this->sendCmdToMe(one.pet_getSendBuffer(),one.pet_getSize());
	}

#endif
}

/**
* \brief 检查用户聊天指令
*
* \param pstrCmd 用户命令
* \param nCmdLen 命令长度
*/
bool SceneUser::checkChatCmd(const Cmd::stNullUserCmd *pstrCmd,const DWORD nCmdLen) const
{               
	using namespace Cmd;
	switch (pstrCmd->byCmd)
	{
	case CHAT_USERCMD:
		{
			switch (pstrCmd->byParam)
			{
			case ALL_CHAT_USERCMD_PARAMETER:
				{
					Cmd::stChannelChatUserCmd *rev = (Cmd::stChannelChatUserCmd *)pstrCmd;

					switch (rev->dwType)
					{                               
					case CHAT_TYPE_NINE:
						//                case CHAT_TYPE_SHOPADV:
						{
							if (!isset_state(sysSetting,USER_SETTING_CHAT_NINE))
								return false;
						}
						break;  
						/*
						case CHAT_TYPE_PRIVATE: 
						case CHAT_TYPE_FRIEND_PRIVATE:
						case CHAT_TYPE_UNION_PRIVATE:
						case CHAT_TYPE_OVERMAN_PRIVATE:
						case CHAT_TYPE_FAMILY_PRIVATE:
						{
						if (!isset_state(sysSetting,USER_SETTING_CHAT_PRIVATE))
						return false;
						}
						break;
						*/
					case CHAT_TYPE_UNION_AFFICHE: 
					case CHAT_TYPE_UNION:
						{
							if (!isset_state(sysSetting,USER_SETTING_CHAT_UNION))
								return false;
						}
						break;
					case CHAT_TYPE_OVERMAN_AFFICHE:
					case CHAT_TYPE_OVERMAN:
						{
							return true;
							if (!isset_state(sysSetting,USER_SETTING_CHAT_SCHOOL))
								return false;
						}
						break;
					case CHAT_TYPE_FAMILY_AFFICHE:
					case CHAT_TYPE_FAMILY:
						{
							if (!isset_state(sysSetting,USER_SETTING_CHAT_FAMILY))
								return false;
						}
						break;
					case CHAT_TYPE_COUNTRY:
						{
							if (!isset_state(sysSetting,USER_SETTING_CHAT_COUNTRY))
								return false;
						}
						break;
					case CHAT_TYPE_TEAM:
						{
							if (!isset_state(sysSetting,USER_SETTING_CHAT_TEAM))
								return false;
						}
						break;
					case CHAT_TYPE_WHISPER:
						{
							if (!isset_state(sysSetting,USER_SETTING_CHAT_WHISPER))
								return false;
						}
						break;
					case CHAT_TYPE_AREA:
						{
							if (!isset_state(sysSetting,USER_SETTING_CHAT_AREA))
								return false;
						}
						break;
					case CHAT_TYPE_WORLD:
						{
							if (!isset_state(sysSetting,USER_SETTING_CHAT_WORLD))
								return false;
						}
						break;
					default:
						break;
					}
				}
				break;
				// */
			case REQUEST_TEAM_USERCMD_PARA://邀请组队
				{
					if (!isset_state(sysSetting,USER_SETTING_TEAM))
						return false;
				}
				break;
			default:
				break;
			}
		}
		break;
	case SEPT_USERCMD://邀请加入家族
		{
			if (ADD_MEMBER_TO_SEPT_PARA==pstrCmd->byParam)
			{
				stAddMemberToSeptCmd * rev = (stAddMemberToSeptCmd*)pstrCmd;
				if (SEPT_QUESTION==rev->byState)
					if (!isset_state(sysSetting,USER_SETTING_FAMILY))
					{
						SceneUser * u = SceneUserManager::getMe().getUserByName(rev->memberName);
						if (u) Channel::sendSys(u,Cmd::INFO_TYPE_FAIL,"%s 加入家族未开启",name);
						return false;
					}
			}
		}
		break;
	default:
		break;
	}
	return true;
}
/**
* \brief 检查用户命令是否有效
*
* \param pstrCmd 用户命令
* \param nCmdLen 命令长度
*/
bool SceneUser::checkUserCmd(const Cmd::stNullUserCmd *pstrCmd,const DWORD nCmdLen) const
{
#if 0
	using namespace Cmd;
	switch (pstrCmd->byCmd)
	{
	case CHAT_USERCMD:
		{
			switch (pstrCmd->byParam)
			{
			case ALL_CHAT_USERCMD_PARAMETER:
				{
					Cmd::stChannelChatUserCmd *rev = (Cmd::stChannelChatUserCmd *)pstrCmd;

					switch (rev->dwType)
					{                               
					case CHAT_TYPE_PRIVATE: 
					case CHAT_TYPE_FRIEND_PRIVATE:
					case CHAT_TYPE_UNION_PRIVATE:
					case CHAT_TYPE_OVERMAN_PRIVATE:
					case CHAT_TYPE_FAMILY_PRIVATE:
					case CHAT_TYPE_WHISPER:
						{
							if (0 == replyText[0])
								autoReply(rev->pstrName);
							return true;
						}
						break;
						/*
						case CHAT_TYPE_NINE:
						// case CHAT_TYPE_SHOPADV:
						{
						if (!isset_state(sysSetting,USER_SETTING_CHAT_NINE))
						return false;
						}
						break;  
						case CHAT_TYPE_PRIVATE: 
						case CHAT_TYPE_FRIEND_PRIVATE:
						case CHAT_TYPE_UNION_PRIVATE:
						case CHAT_TYPE_OVERMAN_PRIVATE:
						case CHAT_TYPE_FAMILY_PRIVATE:
						{
						if (!isset_state(sysSetting,USER_SETTING_CHAT_PRIVATE))
						return false;
						}
						break;
						case CHAT_TYPE_UNION_AFFICHE: 
						case CHAT_TYPE_UNION:
						{
						if (!isset_state(sysSetting,USER_SETTING_CHAT_UNION))
						return false;
						}
						break;
						case CHAT_TYPE_OVERMAN_AFFICHE:
						case CHAT_TYPE_OVERMAN:
						{
						if (!isset_state(sysSetting,USER_SETTING_CHAT_AREA))
						return false;
						}
						break;
						case CHAT_TYPE_FAMILY_AFFICHE:
						case CHAT_TYPE_FAMILY:
						{
						if (!isset_state(sysSetting,USER_SETTING_CHAT_FAMILY))
						return false;
						}
						break;
						case CHAT_TYPE_COUNTRY:
						{
						if (!isset_state(sysSetting,USER_SETTING_CHAT_COUNTRY))
						return false;
						}
						break;
						case CHAT_TYPE_TEAM:
						{
						if (!isset_state(sysSetting,USER_SETTING_CHAT_TEAM))
						return false;
						}
						break;
						case CHAT_TYPE_WHISPER:
						{
						if (!isset_state(sysSetting,USER_SETTING_CHAT_WHISPER))
						return false;
						}
						break;
						// */
					default:
						break;
					}
				}
				break;
			case REQUEST_TEAM_USERCMD_PARA://邀请组队
				{
					if (!isset_state(sysSetting,USER_SETTING_TEAM))
						return false;
				}
				break;
			default:
				break;
			}
		}
		break;
	case TRADE_USERCMD:
		{
			//请求交易
			/*
			if (REQUEST_TRADE_USERCMD_PARAMETER==pstrCmd->byParam)
			if (!isset_state(sysSetting,USER_SETTING_TRADE))
			{
			SceneUser * asker = scene->getUserByTempID(((stRequestTradeUserCmd *)pstrCmd)->dwAskerTempID);
			if (asker)
			{
			//tradeorder.finish();
			asker->tradeorder.finish();
			Channel::sendSys(asker,Cmd::INFO_TYPE_FAIL,"对方已经关闭交易");
			}
			return false;
			}
			*/
		}
		break;
	case SCHOOL_USERCMD://邀请加入师门
		{
			if (ADD_MEMBER_TO_SCHOOL_PARA==pstrCmd->byParam)
				if (TEACHER_QUESTION==((stAddMemberToSchoolCmd *)pstrCmd)->byState)
					if (!isset_state(sysSetting,USER_SETTING_SCHOOL))
						return false;
		}
		break;
	case UNION_USERCMD://邀请加入帮会
		{
			if (ADD_MEMBER_TO_UNION_PARA==pstrCmd->byParam)
				if (QUESTION==((stAddMemberToUnionCmd*)pstrCmd)->byState)
					if (!isset_state(sysSetting,USER_SETTING_UNION))
						return false;
		}
		break;
	case SEPT_USERCMD://邀请加入家族
		{
			if (ADD_MEMBER_TO_SEPT_PARA==pstrCmd->byParam)
				if (SEPT_QUESTION==((stAddMemberToSeptCmd*)pstrCmd)->byState)
					if (!isset_state(sysSetting,USER_SETTING_FAMILY))
						return false;
		}
		break;
	case RELATION_USERCMD://邀请加入家族
		{
			/*
			if (RELATION_STATUS_PARA==pstrCmd->byParam)
			if ((RELATION_TYPE_FRIEND==((stRelationStatusCmd*)pstrCmd)->type)
			&& (RELATION_QUESTION==((stRelationStatusCmd*)pstrCmd)->byState))
			if (!isset_state(sysSetting,USER_SETTING_FRIEND))
			return false;
			*/
		}
		break;
	default:
		break;
	}
#endif
	return true;
}     

/**
* \brief 发送场景用户的消息到Bill
*
*/
void SceneUser::sendSceneCmdToBill(const void *pstrCmd,const DWORD nCmdLen)
{
	using namespace Cmd;
	using namespace Cmd::Scene;
	if (gatetask)
	{
		if (nCmdLen > zSocket::MAX_USERDATASIZE)
		{
			Zebra::logger->debug("消息越界(%d,%d)",((stNullUserCmd *)pstrCmd)->byCmd,((stNullUserCmd *)pstrCmd)->byParam);
		}
		BYTE buf[zSocket::MAX_DATASIZE];
		t_Scene_ForwardSceneUserToBill *sendCmd=(t_Scene_ForwardSceneUserToBill *)buf;
		constructInPlace(sendCmd);
		sendCmd->dwUserID=id;
		sendCmd->size=nCmdLen;
		bcopy(pstrCmd,sendCmd->data,nCmdLen);
		gatetask->sendCmd(buf,sizeof(t_Scene_ForwardSceneUserToBill)+nCmdLen);
	}
}
//
/**
* \brief 模拟用户给BillUser发消息
*
*/
void SceneUser::sendCmdToBill(const void *pstrCmd,const DWORD nCmdLen)
{
	using namespace Cmd;
	using namespace Cmd::Scene;
	if (gatetask)
	{
		if (nCmdLen > zSocket::MAX_USERDATASIZE)
		{
			Zebra::logger->debug("消息越界(%d,%d)",((stNullUserCmd *)pstrCmd)->byCmd,((stNullUserCmd *)pstrCmd)->byParam);
		}
		BYTE buf[zSocket::MAX_DATASIZE];
		t_Scene_ForwardSceneToBill *sendCmd=(t_Scene_ForwardSceneToBill *)buf;
		constructInPlace(sendCmd);
		sendCmd->dwUserID=id;
		sendCmd->size=nCmdLen;
		bcopy(pstrCmd,sendCmd->data,nCmdLen);
		gatetask->sendCmd(buf,sizeof(t_Scene_ForwardSceneToBill)+nCmdLen);
	}
}
/**
* \brief 给自己发送命令
*
*/
void SceneUser::sendCmdToMe(const void *pstrCmd,const DWORD nCmdLen)
{
	using namespace Cmd::Scene;
	using namespace Cmd;
	if (gatetask)
	{
	//	Zebra::logger->debug("消息(%d,%d)",((stNullUserCmd *)pstrCmd)->byCmd,((stNullUserCmd *)pstrCmd)->byParam);
		if (!checkUserCmd((stNullUserCmd *)pstrCmd,nCmdLen)) return;

		if (nCmdLen > zSocket::MAX_USERDATASIZE)
		{
			Zebra::logger->debug("消息越界(%d,%d)",((stNullUserCmd *)pstrCmd)->byCmd,((stNullUserCmd *)pstrCmd)->byParam);
		}
		BYTE buf[zSocket::MAX_DATASIZE];
		t_User_ForwardScene *sendCmd=(t_User_ForwardScene *)buf;
		constructInPlace(sendCmd);
		sendCmd->dwID=id;
		sendCmd->size=nCmdLen;
		bcopy(pstrCmd,sendCmd->data,nCmdLen);
		gatetask->sendCmd(buf,sizeof(t_User_ForwardScene)+nCmdLen);
	}
}

//通知网关改变了dupIndex
void SceneUser::sendDupChangeCmdToGate()
{
	using namespace Cmd::Scene;
	using namespace Cmd;
	if (gatetask)
	{
		BYTE buf[zSocket::MAX_DATASIZE];
		t_ForwardScene_dupChange *sendCmd=(t_ForwardScene_dupChange *)buf;
		constructInPlace(sendCmd);
		sendCmd->dupIndex = this->dupIndex;
		sendCmd->dwID=id;
		sendCmd->size=sizeof(*sendCmd);
		//bcopy(pstrCmd,sendCmd->data,nCmdLen);
		gatetask->sendCmd(buf,sendCmd->size);	
	}
}





#if 0
bool SceneUser::addScriptTask(int _type,const char* _funcName,int _elapse,int p1)
{
	scriptTask *_task = new scriptTask(_funcName,_elapse,this);
	_task->p1 = p1;
	if(_userScriptTaskContainer->add((taskType)_type,_task))
		return true;
	delete _task;
	return false;
}
bool SceneUser::delScriptTask(int _type)
{
	return _userScriptTaskContainer->del((taskType)_type);
}
#endif
/**
* \brief 注销用户
*
*
*/
bool SceneUser::unreg(bool MoveScene)
{
#if 0
	//dupIndex = 0;
	execute_script_event(this,"event_userLogout");
	if( box_item.targetO != NULL)
	{
		zObject::destroy(box_item.targetO);
		box_item.targetO = NULL;
	}

	if( box_item.defaultO != NULL)
	{
		zObject::destroy(box_item.defaultO);
		box_item.defaultO = NULL;
	}

	//如果用户在副本中则离开副本
	if(dupIndex != 0)
		userLeaveDup();
#endif
	unReging=true;
//	tradeorder.cancel();
//	privatestore.step(PrivateStore::NONE,this);

//	tradeorder.cancel();
//	privatestore.step(PrivateStore::NONE,this);
	if (this->getState() == zSceneEntry::SceneEntry_Hide)
	{
		// 恢复可见
		this->setState(zSceneEntry::SceneEntry_Normal);
		zPos curPos = this->getPos();
		this->goTo(curPos);
	}

	if (hasInScene())
	{
		//如果处在死亡等待状态
		if (deathWaitTime)
		{
			//this->Death();
			charbase.hp = charstate.maxhp;
			charbase.mp = charstate.maxmp;
			charbase.sp = charstate.maxsp;
		}

//		if (miniGame)
//		{           
//			Dice * temp = miniGame;
//			miniGame->endGame();
//			delete temp;
//			miniGame = 0;
//		}

		/*
		//通知宠物领养者
		if (adoptedCartoon!=0)
		{
		Cmd::Session::t_notifyCartoon_SceneSession send;
		strncpy(send.adopter,cartoonList[adoptedCartoon].adopter,MAX_NAMESIZE);
		send.state = 0;
		send.cartoonID = adoptedCartoon;
		sessionClient->sendCmd(&send,sizeof(send));
		}
		*/
		//临时数据存档,需要做退出方式判断
		saveTempArchive();
//		if (guard && saveGuard) clearGuardNpc();
		//保存之后才能删除宠物
//		killAllPets();
		//Zebra::logger->debug("[宠物]%s(%u) 下线,删除所有宠物",name,id);
#if 0
		if (this->TeamThisID != 0)
		{
			Cmd::stTeamMemberOfflineUserCmd off;
			off.dwTempID=this->tempid;
			TeamManager * team = SceneManager::getInstance().GetMapTeam(TeamThisID); 
			
			if (team)
			{
				//team->sendCmdToTeam(this,&off,sizeof(Cmd::stTeamMemberOfflineUserCmd));

				if (!MoveScene)
				{
					if(team->getLeader() == this->tempid)	//sky 非跨场景注销的时候才跟换队长
					{
						//sky 队长下线时跟换新队长
						if(!team->changeLeader())
						{
							//队伍已经没人可以移交队长拉就直接删除队伍拉
							SceneManager::getInstance().SceneDelTeam(TeamThisID);
							TeamThisID = 0;
						}
					}

					//sky 非跨场景的时候还要告诉session添加自己到临时容器里和跟新session的队伍场景Map
					Cmd::Session::t_Team_AddMoveSceneMember send;
					send.TeamThisID = team->getTeamtempId();
					send.MemberID = this->id;
					sessionClient->sendCmd(&send, sizeof(Cmd::Session::t_Team_AddMoveSceneMember));
				}

				//sky 下线就把他在队伍中的位置变为不存在
				team->SetMemberType(this->id, 0, false);
			}
		}
#endif
		//Zebra::logger->info("用户%s(%d)注销,时间=%u",name,id,SceneTimeTick::currentTime.msecs());
		LeaveScene();
		SceneUserManager::getMe().removeUser(this);
		//if (scene) scene->removeUser(this);
		//把自己从个人聊天频道里删除
		ChannelM::getMe().removeUser(name);
//		if (guard) guard = 0;
	}
	else
	{
		SceneUserManager::getMe().removeUser(this);
	}
	//设置回收时间
	//scriptTaskManagement::getInstance().deleteTask(this);
//	delete _userScriptTaskContainer;
	this->waitRecycle();
	SceneRecycleUserManager::getInstance().addUser(this);
	return true;
}

#if 0
struct GetRequestUserAndSend
{
private:
	SceneUser *pUser;
public:
	static BYTE _buf_map_user[zSocket::MAX_USERDATASIZE];
	Cmd::stMapDataMapScreenUserCmd *_map_user; 
	void initmapUser()
	{
		//bzero(_buf_map_user,sizeof(_buf_map_user));
		_map_user = (Cmd::stMapDataMapScreenUserCmd *)_buf_map_user;
		constructInPlace(_map_user);
		_map_user->mdih.type = Cmd::MAPDATATYPE_USER;
		_map_user->mdih.size=0;
	}
public:
	GetRequestUserAndSend(SceneUser *user):pUser(user)
	{
		initmapUser();
	};
	bool get(SceneUser *entry)
	{
		//目前好像不会超过64k
		/*
		if (outofbound())
		{
		pUser->sendCmdToMe(getSendBuffer(),getSize());
		initMapUser();
		}
		// */
		entry->full_t_MapUserData(_map_user->mud[_map_user->mdih.size]);
		_map_user->mud[_map_user->mdih.size].x=entry->getPos().x;
		_map_user->mud[_map_user->mdih.size].y=entry->getPos().y;
		_map_user->mud[_map_user->mdih.size].byDir=entry->getDir();

		if (pUser->isWar(entry))
		{
			set_state(_map_user->mud[_map_user->mdih.size].state,Cmd::USTATE_WAR);
		}
		else
		{
			clear_state(_map_user->mud[_map_user->mdih.size].state,Cmd::USTATE_WAR);
		}
		_map_user->mdih.size++;

		return true;
	}
	inline const bool outofbound() const
	{
		return (sizeof(Cmd::stMapDataMapScreenUserCmd)+ sizeof(Cmd::t_MapUserDataPos)*(_map_user->mdih.size+1) + sizeof(Cmd::MapData_ItemHeader)) >= zSocket::MAX_USERDATASIZE;
	}
	inline const bool canSend() const
	{
		return _map_user->mdih.size > 0;
	}
	inline const BYTE *getSendBuffer()
	{
		Cmd::MapData_ItemHeader *pheader =
			(Cmd::MapData_ItemHeader *)&_buf_map_user[sizeof(Cmd::stMapDataMapScreenUserCmd) + sizeof(Cmd::t_MapUserDataPos) * _map_user->mdih.size];
		pheader->size = 0; pheader->type = 0;
		return _buf_map_user;
	}
	inline const int getSize() const
	{
		return sizeof(Cmd::stMapDataMapScreenUserCmd)
			+ sizeof(Cmd::t_MapUserDataPos) * _map_user->mdih.size + sizeof(Cmd::MapData_ItemHeader);
	}
};
BYTE GetRequestUserAndSend::_buf_map_user[zSocket::MAX_USERDATASIZE];
bool SceneUser::requestUser(Cmd::stRequestUserDataMapScreenUserCmd *rev)
{
	WORD loop = rev->size < 200 ? rev->size : 200;

	GetRequestUserAndSend request_user(this);
	for(WORD i =0 ;i<loop ;i++)
	{
		if (rev->dwUserTempID[i]==0 || rev->dwUserTempID[i]==tempid)
		{
			Zebra::logger->error("%s(%ld)请求无效的其他用户ID%ld",name,tempid,rev->dwUserTempID[i]);
			continue;
		}
		SceneUser *pUser=scene->getUserByTempID(rev->dwUserTempID[i]);
		if (pUser)
		{
			Zebra::logger->debug("%s请求用户编号存在：%s,%u",this->name,pUser->name,rev->dwUserTempID[i]);
			request_user.get(pUser);
		}
		else
		{
			Zebra::logger->debug("%s请求用户编号不存在：%u",this->name,rev->dwUserTempID[i]);
		}
		if (request_user.canSend())
		{
			this->sendCmdToMe(request_user.getSendBuffer(),request_user.getSize());
			//Zebra::logger->debug("request_user._map_user->mdih.size=%d",request_user._map_user->mdih.size);
		}
	}

	return true;
}

struct GetRequestNpcAndSend
{
private:
	SceneUser *pUser;
public:
	static BYTE _buf_map_npc[zSocket::MAX_USERDATASIZE];
	static SceneNpc_vec _npc_vec;
	static BYTE petBuf[zSocket::MAX_USERDATASIZE];
	static BYTE _buf_map_ghost[zSocket::MAX_USERDATASIZE];
	Cmd::stMapDataMapScreenUserCmd *_map_npc; 
	Cmd::stMapDataMapScreenUserCmd *_map_pet;
	Cmd::stMapDataMapScreenUserCmd * _map_ghost;
	void initmapNpc()
	{
		//bzero(_buf_map_npc,sizeof(_buf_map_npc));
		_map_npc = (Cmd::stMapDataMapScreenUserCmd *)_buf_map_npc;
		constructInPlace(_map_npc);
		_map_npc->mdih.type = Cmd::MAPDATATYPE_NPC;
		_map_npc->mdih.size=0;
		_npc_vec.clear();
	}
	void initPet()
	{
		//bzero(petBuf,sizeof(petBuf));
		_map_pet = (Cmd::stMapDataMapScreenUserCmd *)petBuf;
		constructInPlace(_map_pet);
		_map_pet->mdih.type = Cmd::MAPDATATYPE_PET;
		_map_pet->mdih.size=0;
	}
	void initMapGhost()
	{
		_map_ghost = (Cmd::stMapDataMapScreenUserCmd *)_buf_map_ghost;
		constructInPlace(_map_ghost);
		_map_ghost->mdih.type = Cmd::MAPDATATYPE_USER;
		_map_ghost->mdih.size=0;
	}
public:
	GetRequestNpcAndSend(SceneUser *user):pUser(user)
	{
		initmapNpc();
		initPet();
		initMapGhost();
	};
	bool get(SceneNpc *entry)
	{
		//目前好像不会超过64k
		/*
		if (npc_outofbound())
		{
		pUser->sendCmdToMe(npc_getSendBuffer(),npc_getSize());
		if (!_npc_vec.empty())
		{
		for(SceneNpc_vec::iterator iter = _npc_vec.begin() ; iter != _npc_vec.end() ; iter ++)
		{
		(*iter)->showHP(this,(*iter)->hp);
		}
		}
		initmapNpc();
		}
		// */
		entry->full_t_MapNpcData(_map_npc->mnd[_map_npc->mdih.size]);
		_map_npc->mnd[_map_npc->mdih.size].x=entry->getPos().x;
		_map_npc->mnd[_map_npc->mdih.size].y=entry->getPos().y;
		_map_npc->mnd[_map_npc->mdih.size].byDir=entry->getDir();
		_map_npc->mdih.size++;

		if (entry->isAttackMe(pUser))
		{
			_npc_vec.push_back(entry); 
		}
		// */

		/*
		if (pet_outofbound())
		{
		pUser->sendCmdToMe(pet_getSendBuffer(),pet_getSize());
		initPet();
		}
		// */
		((ScenePet *)entry)->full_t_MapPetData(_map_pet->mpd[_map_pet->mdih.size]);
		_map_pet->mdih.size++;

		return true;
	}
	inline const bool npc_outofbound() const
	{
		return (sizeof(Cmd::stMapDataMapScreenUserCmd)+ sizeof(Cmd::t_MapNpcDataPos) *(_map_npc->mdih.size+1) + sizeof(Cmd::MapData_ItemHeader))>=zSocket::MAX_USERDATASIZE;
	}
	inline const bool npc_canSend() const
	{
		return _map_npc->mdih.size > 0;
	}
	inline const BYTE *npc_getSendBuffer()
	{
		Cmd::MapData_ItemHeader *pheader =
			(Cmd::MapData_ItemHeader *)&_buf_map_npc[sizeof(Cmd::stMapDataMapScreenUserCmd) + sizeof(Cmd::t_MapNpcDataPos) * _map_npc->mdih.size];
		pheader->size = 0; pheader->type = 0;
		return _buf_map_npc;
	}
	inline const int npc_getSize() const
	{
		return sizeof(Cmd::stMapDataMapScreenStateUserCmd)
			+ sizeof(Cmd::t_MapNpcDataPos) * _map_npc->mdih.size + sizeof(Cmd::MapData_ItemHeader);
	}
	inline const bool pet_outofbound() const
	{
		return _map_pet->mdih.size >= 200;
	}

	inline const bool pet_canSend() const
	{
		return _map_pet->mdih.size > 0;
	}
	inline const BYTE *pet_getSendBuffer()
	{
		Cmd::MapData_ItemHeader *pheader =
			(Cmd::MapData_ItemHeader *)&petBuf[sizeof(Cmd::stMapDataMapScreenStateUserCmd)+sizeof(Cmd::t_MapPetData)*(_map_pet->mdih.size)];
		pheader->size = 0; pheader->type = 0;
		return petBuf;
	}
	inline const int pet_getSize() const
	{
		return sizeof(Cmd::stMapDataMapScreenStateUserCmd)
			+ sizeof(Cmd::t_MapPetData) * _map_pet->mdih.size + sizeof(Cmd::MapData_ItemHeader);
	}

	//sky 元神消息相关处理
	inline const bool ghost_outofbound() const
	{
		return (sizeof(Cmd::stMapDataMapScreenUserCmd)+ sizeof(Cmd::t_MapUserDataPos)*(_map_ghost->mdih.size+1) + sizeof(Cmd::MapData_ItemHeader)) >= SCENE_PACKET_USERDATASIZE;
	}

	inline const BYTE *ghost_getSendBuffer()
	{
		Cmd::MapData_ItemHeader *pheader =
			(Cmd::MapData_ItemHeader *)&_buf_map_ghost[sizeof(Cmd::stMapDataMapScreenUserCmd) + sizeof(Cmd::t_MapUserDataPos) * _map_ghost->mdih.size];
		pheader->size = 0; pheader->type = 0;
		return _buf_map_ghost;
	}
	inline const int ghost_getSize() const
	{
		return sizeof(Cmd::stMapDataMapScreenUserCmd)
			+ sizeof(Cmd::t_MapUserDataPos) * _map_ghost->mdih.size + sizeof(Cmd::MapData_ItemHeader);
	}
};
BYTE GetRequestNpcAndSend::_buf_map_npc[zSocket::MAX_USERDATASIZE];
SceneNpc_vec GetRequestNpcAndSend::_npc_vec;
BYTE GetRequestNpcAndSend::petBuf[zSocket::MAX_USERDATASIZE];
BYTE GetRequestNpcAndSend::_buf_map_ghost[zSocket::MAX_USERDATASIZE];
bool SceneUser::requestNpc(Cmd::stRequestMapNpcDataMapScreenUserCmd *rev)
{
	GetRequestNpcAndSend request_npc(this);
	WORD loop = loop < 200 ? rev->size : 200;

	for(WORD i =0 ;i<loop ;i++)
	{
		SceneNpc *sceneNpc=SceneNpcManager::getMe().getNpcByTempID(rev->dwNpcTempID[i]);
		if (sceneNpc)
		{
			Zebra::logger->debug("%s请求Npc编号存在：%s,%u",this->name,sceneNpc->npc->name,rev->dwNpcTempID[i]);
			request_npc.get(sceneNpc); 
			//Zebra::logger->debug("%u,%u,%s",sceneNpc->id,sceneNpc->tempid,sceneNpc->name);

		}
		else
		{
			Zebra::logger->debug("%s请求Npc编号不存在：%u",this->name,rev->dwNpcTempID[i]);
		}
	}
	if (request_npc.npc_canSend())
	{
		this->sendCmdToMe(request_npc.npc_getSendBuffer(),request_npc.npc_getSize());
		if (!request_npc._npc_vec.empty())
		{
			for(SceneNpc_vec::iterator iter = request_npc._npc_vec.begin() ; iter != request_npc._npc_vec.end() ; iter ++)
			{
				(*iter)->showHP(this,(*iter)->hp);
			}
		}
	}   

	if (request_npc.pet_canSend())
	{
		this->sendCmdToMe(request_npc.pet_getSendBuffer(),request_npc.pet_getSize());
	}
	return true;
}
#endif

class SaveObjectExec : public UserObjectExec
{
    public:
	ZlibObject *full;
	SaveObject *o;
	bool exec(zObject *object)
	{
	    o = (SaveObject *)full->data;
	    if (object == NULL)
	    {
		return false;
	    }
	    else
	    {
		object->getSaveData(&o[full->count]);
		full->count++;
	    }
	    return true;
	}
};

#if 0
class SaveSkillExec : public UserSkillExec
{
public:
	ZlibSkill *full;
	SaveSkill *s;
	bool exec(zSkill *skill)
	{
		s = (SaveSkill *)full->data;
		if (skill == NULL)
		{
			return false;
		}
		else
		{
			skill->getSaveData(&s[full->count]);
			full->count ++ ;
		}
		return true;
	}
};
#endif
/**
* \brief 压缩存档数据,没有检测数据超过最大值
*
* \pUser 存档数据所属用户
* \zlib 压缩输出buf
*
* \return 压缩后数据大小,0 表示压缩出错
*/
int compressSaveData(SceneUser *pUser,BYTE *zlib)
{
	BYTE unBuf[MAX_UZLIB_CHAR];
	bzero(unBuf,sizeof(unBuf));

	int uzSize = 0;
	uzSize = pUser->saveBinaryArchive(unBuf+uzSize, MAX_UZLIB_CHAR-uzSize-sizeof(Cmd::Record::t_WriteUser_SceneRecord));

	uLongf zsize = zSocket::MAX_DATASIZE - sizeof(Cmd::Record::t_WriteUser_SceneRecord);
	int retcode;
	retcode = compress((BYTE*)zlib, &zsize, (BYTE*)unBuf, (uLongf)uzSize);
	switch(retcode)
	{
	case Z_OK:
		{
			Zebra::logger->debug("compress user data OK!(%s(%u),uzsize = %u,size = %ld)",pUser->name,pUser->id,uzSize,zsize);
			break;
		}
	case Z_MEM_ERROR:
	case Z_BUF_ERROR:
		{
			Zebra::logger->debug("compress user data error!!!(%s(%u))",pUser->name,pUser->id);
			zsize = 0;
			break;
		}
	default:
		{
			Zebra::logger->debug("compress user data error!! UNKNOWN ERROR(%s(%u))",pUser->name,pUser->id);
			zsize = 0;
			break;
		}
	}
	return zsize;
}


/**
* \brief 解压缩存档数据
*
* \pUser 数据所属用户
* \rev 收到档案服务器的数据包
* \dataSize 解压前数据大小
* \petData 宠物数据的拷贝
*
* \return 解压缩后数据大小,0 表示压缩出错
*/

#define CHECK_OUT(x,prop) \
	if (o->data.x > 50000) {\
	Zebra::logger->info("用户%s物品数据非法%s(%s:%d)",pUser->name,o->data.strName,prop,o->data.x);\
	}

bool uncompressSaveData(SceneUser *pUser,const BYTE *data,const DWORD dataSize,BYTE * petData) 
{
	BYTE uzBuf[MAX_UZLIB_CHAR];
	bzero(uzBuf,sizeof(uzBuf));
	uLongf bufSize = MAX_UZLIB_CHAR;

	int retcode;
	retcode = uncompress(uzBuf,&bufSize,(Bytef *)data,dataSize);
	switch(retcode)
	{
	case Z_OK:
		Zebra::logger->debug("uncompress user data OK!!!(%s(%u),size = %u,usize = %ld)",pUser->name,pUser->id,dataSize,bufSize);
		break;
	case Z_MEM_ERROR:
	case Z_BUF_ERROR:
	case Z_DATA_ERROR:
		{
			Zebra::logger->error("uncompress user data error!(%s(%u),size = %u,usize = %ld)",pUser->name,pUser->id,dataSize,bufSize);
			bufSize = 0;
			return false;
		}
		break;
	default:
		{
			Zebra::logger->error("uncompress user data error!UNKNOWN ERROR (%s(%u))",pUser->name,pUser->id);
			bufSize = 0;
			return false;
		}
		break;
	}
	int len = 0;
	len += pUser->setupBinaryArchive((const char*)uzBuf);
	//pUser->parseLoadRefreshTime(); ����Ҫ
	return true;
}

/**
* \brief 保存角色数据到档案服务器
*
* \param writeback_type 保存类型
*
* \return 保存成功,返回TRUE,否则返回FALSE
*
*/
bool SceneUser::save(const Cmd::Record::WriteBack_Type writeback_type, DWORD tozone, DWORD secretkey, DWORD type)
{
	if (scene==NULL)
	{
		Zebra::logger->error("scene is NULL when save record!!!");
		return false;
	}

	if (writeback_type == Cmd::Record::CHANGE_SCENE_WRITEBACK) {


	}
	else 
	{
		if (writeback_type == Cmd::Record::OPERATION_WRITEBACK){
			_writeback_timer.next(SceneTimeTick::currentTime);
		}

		if(this->charbase.zone_state == CHANGEZONE_RETURNED)
		{
		}
		else
		{
		    charbase.mapid = scene->tempid;
		    strncpy(charbase.mapName,scene->name,MAX_NAMESIZE);
		    charbase.x = pos.x;
		    charbase.y = pos.y;
		}
		//Zebra::logger->debug("%u,%u",charbase.x,charbase.y);

		if (writeback_type == Cmd::Record::LOGOUT_WRITEBACK)
		{//如果是离线,判断是否是在夺城战,如果是,则送到皇宫外
			//在外国王城下线回到东郊
			if (scene->countryDareBackToMapID && scene->getCountryID()!=charbase.country)
			{
				char mapName[MAX_NAMESIZE];
				bzero(mapName,MAX_NAMESIZE);
				SceneManager::getInstance().buildMapName(scene->getCountryID(),scene->countryDareBackToMapID,mapName);

				strncpy(charbase.mapName,mapName,MAX_NAMESIZE);
				charbase.mapid = scene->countryDareBackToMapID;
				charbase.gomaptype = ZoneTypeDef::ZONE_PRIVATE_RELIVE;
			}
		}
	}
#if 0
	zObject *gold = packs.getGold();
	//temp solution,just for record before
	if (gold) {
		charbase.money=gold->data.dwNum;
	}
	//  BYTE unBuf[MAX_UZLIB_CHAR];
#endif
	//保存角色档案信息
	BYTE zlibBuf[zSocket::MAX_DATASIZE];
	Cmd::Record::t_WriteUser_SceneRecord *saveChar = (Cmd::Record::t_WriteUser_SceneRecord *)zlibBuf;
	constructInPlace(saveChar);

	// 保存离线时间
	this->processTire();

	this->charbase.reliveWeakTime = this->charbase.reliveWeakTime-SceneTimeTick::currentTime.sec()%10000;
	if (this->charbase.reliveWeakTime >1000) this->charbase.reliveWeakTime = 0;
	
	if(writeback_type == Cmd::Record::LOGOUT_WRITEBACK && charbase.zone_state == CHANGEZONE_CHANGED)
	{
	    charbase.source_id = charbase.id;
	}

	saveChar->accid = accid;
	saveChar->id = id;
	saveChar->dwMapTempID = scene->tempid;
	saveChar->writeback_type = writeback_type;
	bcopy(&charbase,&saveChar->charbase,sizeof(CharBase));
	if (this->wait_gomap_name[0] && strncmp(this->wait_gomap_name,saveChar->charbase.mapName,MAX_NAMESIZE) == 0)
	{
		bcopy(this->wait_gomap_name,&saveChar->charbase.mapName,sizeof(this->wait_gomap_name));
		saveChar->charbase.x=0;
		saveChar->charbase.y=0;
		bzero(this->wait_gomap_name,sizeof(this->wait_gomap_name));
	}

	//善恶度只保存低16位
	saveChar->charbase.goodness &= 0x0000FFFF;
	//if (this->pkState.hasProtected())
	//{
	//	saveChar->charbase.goodness += 0xFF000000+(this->pkState.leftProtectTime() << 16);
	//}
	//Zebra::logger->debug("lastIncTime=%d,onlinetime=%d",lastIncTime,charbase.onlinetime);
	charbase.onlinetime -= lastIncTime; 
	lastIncTime=loginTime.elapse(SceneTimeTick::currentTime) / 1000;
	charbase.onlinetime += lastIncTime; 
	saveChar->charbase.onlinetime = charbase.onlinetime; 

	saveChar->to_game_zone = tozone;
	saveChar->secretkey = secretkey;
	saveChar->type = type;
	saveChar->dataSize = compressSaveData(this,(BYTE*)saveChar->data);
	if (saveChar->dataSize == 0)
	{
		return false;
	}
	if(saveChar->dataSize >= 64*1024)
	{
	    Zebra::logger->error("save record error!!! %u %u %s��С����64K",saveChar->charbase.accid, saveChar->id, saveChar->charbase.name);
	    return false;
	}

	Zebra::logger->debug("[save record] %s, x:%u y:%u",saveChar->charbase.name,saveChar->charbase.x,saveChar->charbase.y);

	recordClient->sendCmd(saveChar,sizeof(Cmd::Record::t_WriteUser_SceneRecord) + saveChar->dataSize);
	return true;
}

struct GetAllRemovePosNpc : public zSceneEntryCallBack
{
private:
	BYTE buf2[zSocket::MAX_USERDATASIZE];
	Cmd::stBatchRemoveNpcMapScreenUserCmd *pos;
	SceneUser *user;
public:
	GetAllRemovePosNpc(SceneUser* pUser) {
		pos=(Cmd::stBatchRemoveNpcMapScreenUserCmd *)buf2;
		constructInPlace(pos);
		pos->num=0;
		user = pUser;
	}
	bool exec(zSceneEntry *entry)
	{
		//TODO 防止指令溢出
		if (entry->getState() != zSceneEntry::SceneEntry_Hide)
		{
			pos->id[pos->num]=entry->tempid;
			pos->num++;
		}
		return true;
	}
	inline const bool canSend() const
	{
		return pos->num > 0;
	}
	inline const BYTE *getSendBuffer()
	{
		return buf2;
	}
	inline const int getSize() const
	{
		return sizeof(Cmd::stBatchRemoveNpcMapScreenUserCmd) + pos->num * sizeof(DWORD);
	}
};
struct GetAllRemovePosUser : public zSceneEntryCallBack
{
private:
	BYTE buf2[zSocket::MAX_USERDATASIZE];
	Cmd::stBatchRemoveUserMapScreenUserCmd *pos;
	SceneUser *user;
public:
	GetAllRemovePosUser(SceneUser* pUser) {
		pos=(Cmd::stBatchRemoveUserMapScreenUserCmd  *)buf2;
		constructInPlace(pos);
		pos->num=0;
		user = pUser;
	}
	bool exec(zSceneEntry *entry)
	{
		//TODO 防止指令溢出
		if (entry->getState() != zSceneEntry::SceneEntry_Hide)
		{
			pos->id[pos->num]=entry->tempid;
			pos->num++;
		}
		return true;
	}
	inline const bool canSend() const
	{
		return pos->num > 0;
	}
	inline const BYTE *getSendBuffer()
	{
		return buf2;
	}
	inline const int getSize() const
	{
		return sizeof(Cmd::stBatchRemoveUserMapScreenUserCmd) + pos->num * sizeof(DWORD);
	}
};

/**
* \brief 骑马命令的处理
*
* \param rev 骑马命令
*
* \return 处理成功返回TRUE,否则返回FALSE
*/
bool SceneUser::ride(Cmd::stRideMapScreenUserCmd *rev)
{
#if 0
	if (this->isSitdown())
	{
		return Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你处于打坐状态!");
	}

	if (!scene->canRide())
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对不起,这里不能骑马");
		return false;
	}

	if (!horse.horse()) {
		return Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你还没有马匹!");
	}

	//horse.putAway();

	OnRide event(1);
	if (EventTable::instance().execute(*this,event) & Action::DISABLE) return false;

	bool mount = horse.mount();
	horse.mount(!mount);
#endif
	return true;
}
#if 0
/*\brief 使用令牌类道具
*
*/
bool SceneUser::useCallObj(zObject *obj)
{
	if (0==obj) return false;
	/*
	switch(obj->base->kind)
	{
	case ItemType_TONG:
	{
	if (this->packs.equip.tong_obj_time/86400 == SceneTimeTick::currentTime.sec()/86400 && this->packs.equip.tong_obj_times == 0)
	{
	Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"帮主令超过使用次数");
	return true;
	}
	else
	{
	if (this->unionMaster)
	{
	this->packs.equip.tong_obj_times --;
	this->packs.equip.tong_obj_time = SceneTimeTick::currentTime.sec(); 
	}
	else
	{
	Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"只有帮主才可以使用帮主令");
	return true;
	}
	}
	}
	break;
	case ItemType_FAMILY:
	{
	if (this->packs.equip.family_obj_time/86400 == SceneTimeTick::currentTime.sec()/86400 && this->packs.equip.family_obj_times == 0)
	{
	Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"家族令超过使用次数");
	return true;
	}
	else
	{
	if (this->septMaster)
	{
	this->packs.equip.family_obj_times --;
	this->packs.equip.family_obj_time = SceneTimeTick::currentTime.sec(); 
	}
	else
	{
	Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"只有族长才可以使用族长令");
	return true;
	}
	}
	}
	break;
	case ItemType_KING:
	{
	if (this->packs.equip.king_obj_time/86400 == SceneTimeTick::currentTime.sec()/86400 && this->packs.equip.king_obj_times == 0)
	{
	Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"国王令超过使用次数");
	return true;
	}
	else
	{
	if (this->king)
	{
	this->packs.equip.king_obj_times --;
	this->packs.equip.king_obj_time = SceneTimeTick::currentTime.sec(); 
	}
	else
	{
	Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"只有国王才可以使用国王令");
	return true;
	}
	}
	}
	break;
	default:
	return true;
	break;
	}
	//检查本地图是否可用
	if (this->scene->checkCallObj())
	{
	return true;
	}
	//检查在国外特殊地图是否可用
	if (this->charbase.country != this->scene->getCountryID() && this->scene->getRealMapID() == 139)
	{
	return true;
	}
	// */
	Cmd::Session::t_GoTo_Leader_SceneSession cmd;
	switch(obj->base->kind)
	{
	case ItemType_KING:
		{
			cmd.type=Cmd::CALL_DUTY_KING;
		}
		break;
	case ItemType_TONG:
		{
			cmd.type=Cmd::CALL_DUTY_UNION;
		}
		break;
	case ItemType_FAMILY:
		{
			cmd.type=Cmd::CALL_DUTY_SEPT;
		}
		break;
	}
	cmd.leaderTempID=this->tempid;
	strncpy(cmd.mapName,this->scene->name,MAX_NAMESIZE);
	cmd.x=getPos().x;
	cmd.y=getPos().y;
	sessionClient->sendCmd(&cmd,sizeof(cmd));
	//通知客户端
	zObject::logger(obj->createid,obj->data.qwThisID,obj->data.strName,obj->data.dwNum,obj->data.dwNum,0,this->id,this->name,0,NULL,"用令牌",obj->base,obj->data.kind,obj->data.upgrade);
	packs.removeObject(obj); //notify and delete
	return true;
}
/*\brief 使用护身符类道具
*
*/
bool SceneUser::useAmulet(zObject *obj)
{
	if (0==obj) return false;
	if (!this->scene->checkIncCity(obj->data.maker))
	{
		return true;
	}
	std::ostringstream os;
	os << "name=" << obj->data.maker;
	bzero(obj->data.maker,sizeof(obj->data.maker));
	strncpy(obj->data.maker,scene->name,MAX_NAMESIZE);
	obj->data.durpoint=getPos().x;
	obj->data.dursecond=getPos().y;
	//notify client,added by liqingyu
	Cmd::stAddObjectPropertyUserCmd ret;
	ret.byActionType = Cmd::EQUIPACTION_REFRESH;
	bcopy(&obj->data,&ret.object,sizeof(t_Object));
	sendCmdToMe(&ret,sizeof(ret));        
	Gm::gomap(this,os.str().c_str());
	return true;
}
/*\brief 使用卷轴类道具
*
*/
bool SceneUser::useScroll(zObject *obj)
{
	if (0==obj) return false;
	if (!this->scene->canUserScroll())
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"本地图不能使用卷轴类道具!");
		return false;
	}
	if (guard)
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你正在运镖,保护好你的镖车！");
		return false;
	}
	if (isSpecWar(Cmd::COUNTRY_FORMAL_DARE))
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"战争中不能使用!");
		return false;
	}
	if (miniGame)
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"正在玩小游戏不能使用卷轴");
		return false;
	}

	if (this->isSitdown())
	{
		standup();
	}

	//通知客户端
	zObject::logger(obj->createid,obj->data.qwThisID,obj->data.strName,obj->data.dwNum,obj->data.dwNum,0,this->id,this->name,0,NULL,"用卷轴",NULL,0,0);
	packs.removeObject(obj); //notify and delete

	return true;
}

/*\brief 使用卷轴类道具
*
*/
bool SceneUser::useSkill(zObject *obj)
{
	if (0==obj) return false;

	if (this->isSitdown())
	{
		standup();
	}

	Cmd::stAttackMagicUserCmd cmd;

	cmd.byAttackType = Cmd::ATTACKTYPE_U2P;
	cmd.dwDefenceTempID = 0;
	cmd.dwUserTempID = this->tempid;
	cmd.wdMagicType = obj->base->make;
	cmd.byAction = Cmd::Ani_Num;
	cmd.byDirect = this->getDir();

	zSkill *s = NULL;

	s = zSkill::createTempSkill(this,obj->base->make,obj->base->recastlevel);
	if (s)
	{
		s->action(&cmd,sizeof(cmd));
		SAFE_DELETE(s);
	}

	if (obj->data.dur && --obj->data.dur)
	{
		Cmd::stAddObjectPropertyUserCmd ret;
		ret.byActionType = Cmd::EQUIPACTION_REFRESH;
		bcopy(&obj->data,&ret.object,sizeof(t_Object));
		sendCmdToMe(&ret,sizeof(ret));        
		return true;
	}

	//通知客户端
	zObject::logger(obj->createid,obj->data.qwThisID,obj->data.strName,obj->data.dwNum,obj->data.dwNum,0,this->id,this->name,0,NULL,"用技能卷轴",NULL,0,0);
	packs.removeObject(obj); //notify and delete

	return true;
}
#endif

/*\brief 使用道具
*
*
*/
bool SceneUser::useObject(zObject *obj, BYTE useType)
{
    if(!obj)
	return false;
    if(useType == 1)	    //������
    {
	//if(obj->base->kind == ItemType_GiftBag_Card)
	{
	    if(GiftBagManager::getMe().useGiftBag(*this, obj))
		return true;
	}
    }
    return true;
}
/*
* 得到外挂日志
*
*/
void SceneUser::getWgLog(DWORD data)
{
#ifdef _DEBUG
	//Zebra::logger->debug("%s 外挂日志 %u %u %u %u %u",name,data,((BYTE*)&data)[0],((BYTE*)&data)[1],((BYTE*)&data)[2],((BYTE*)&data)[3]);
#endif
	for (DWORD i=0; i<sizeof(data); i++)
	{
		//if (2==i) continue;

		if (0==i%2)
			((BYTE*)&data)[i] ^= 'F';
		else
			((BYTE*)&data)[i] ^= 'X';
	}

	if ((!(((BYTE*)&data)[2] & 0x80)) && (!wglog_len)) return;

	if (!wglog_len)
	{
		wglog_len = ((BYTE*)&data)[3];
#ifdef _DEBUG
		//Zebra::logger->debug("%s 外挂日志长度%u",name,wglog_len);
#endif
		return;
	}

	/*
	DWORD len = strlen(wglog);
	for (int i=0; i<3; i++)
	{
	BYTE b = ((BYTE*)&data)[0];

	if (0==len+i)
	wglog[0] = b;
	else
	}
	*/

	//wglog[strlen(wglog)] = ((BYTE*)&data)[0];
	//wglog[strlen(wglog)+1] = ((BYTE*)&data)[1];
	//wglog[strlen(wglog)+2] = ((BYTE*)&data)[2];
	//wglog[strlen(wglog)+3] = ((BYTE*)&data)[3];
	bcopy(&data,&wglog[strlen(wglog)],sizeof(DWORD));


	wglog_len -= wglog_len;
	wglog_len = wglog_len < 4 ? wglog_len : 4;
#ifdef _DEBUG
	//Zebra::logger->debug("%s 外挂日志 %s 长度%u",name,(char *)&data,wglog_len);
#endif

	if (0==wglog_len)
	{
		ScenesService::wglogger->info("[英雄无双]%s,%u,%u,%u,%s,%u,%s",
			name,id,accid,charbase.level,
			SceneManager::getInstance().getCountryNameByCountryID(charbase.country),
			charbase.gold,wglog);
		bzero(wglog,sizeof(wglog));
	}
}

/**
* \brief 处理角色向前移动命令
*
* \param rev 移动命令
*
* \return 移动命令处理成功返回TRUE,否则返回FALSE
*/
bool SceneUser::move(Cmd::stUserMoveMoveUserCmd *rev)
{
#if 0
	if (isSitdown())
	{
		//打坐状态,首先站立
		standup();
	}
	if (tradeorder.hasBegin()) {
		//交易状态,取消交易
		tradeorder.cancel();
	}

	this->keepDir = rev->byDirect;
	bool canmove = false;
	Cmd::stUserMoveMoveUserCmd ret;
	ret.dwUserTempID=tempid;

	//检查外挂日志
	getWgLog(rev->dwUserTempID);

	if (backOffing)
	{
		//如果正在后退中则不能前进,直接返回自己当前坐标
		ret.byDirect=getDir();
		ret.bySpeed=0;
		ret.x=getPos().x;
		ret.y=getPos().y;
		sendCmdToMe(&ret,sizeof(ret));
		return true;
	}
	int xlen = abs((long)(pos.x - rev->x));
	int ylen = abs((long)(pos.y - rev->y));
	//检测移动间隔
	if((!dread && !blind))
	{
		if ((!speedOutMove(rev->dwTimestamp,getMyMoveSpeed(),(xlen>ylen)?xlen:ylen)))
		{
			ret.byDirect=getDir();
			ret.bySpeed=0;
			ret.x=getPos().x;
			ret.y=getPos().y;
			sendCmdToMe(&ret,sizeof(ret));
			return true;
		}
	}
	
	if((dread || blind) && (RandPos.x!=0 || RandPos.y!=0) )
	{
		zPos newPos;
		newPos.x = rev->x;
		newPos.y = rev->y;

		if(!outOfRandRegion(&newPos))
			return false;
	}

	if (this->isQuiz)
	{
		//答题状态,不能移动
		ret.byDirect=getDir();
		ret.bySpeed=0;
		ret.x=getPos().x;
		ret.y=getPos().y;
		sendCmdToMe(&ret,sizeof(ret));
		return true;
	}

	setDir(rev->byDirect);
	lastPos2 = lastPos1;
	lastPos1 = pos;

	zPosI oldPosI=getPosI();
	zPos oldPos=getPos();

	WORD speed=rev->bySpeed>3?1:rev->bySpeed;
	if (horse.mount())
	{
		//骑马状态
		if (speed==2) speed=5;
	}
	else
	{
		//普通状态
		if (speed==3) speed=2;
	}
	//保证坐标合法
	if ((xlen + ylen) <= ((getDir()%2)?(speed*2):speed)) 
	{
		//sprintf(stderr,"walk here2\n");
		zPos newPos(rev->x,rev->y);
		if (moveAction&&(!scene->checkBlock(newPos,TILE_BLOCK) && (!scene->checkBlock(newPos,TILE_ENTRY_BLOCK)|| this->liquidState)))
		{
			if (scene->refresh(this,newPos))
			{
				//高8位表示跑得步数,低8位表示走的步数
				if (!horse.mount() && speed==2) step_state += 0x0100;
				if (!horse.mount() && speed==1) ++step_state;

				canmove = true;
				scene->setBlock(newPos);
				scene->clearBlock(oldPos);

				ret.byDirect=getDir();
				ret.bySpeed=speed;
				ret.x=newPos.x;
				ret.y=newPos.y;
				//检查是否隐身
				if (SceneEntry_Hide!=getState() && !this->hideme && !Soulflag)
					scene->sendCmdToNine(oldPosI,&ret,sizeof(ret),dupIndex);
				else
				{
					//隐身则不占用block
					scene->clearBlock(newPos);
					sendCmdToMe(&ret,sizeof(ret));
					scene->sendCmdToWatchTrap(oldPosI,&ret,sizeof(ret));
				}

				if (oldPosI != getPosI())
				{

					Cmd::stRemoveUserMapScreenUserCmd removeUser;
					removeUser.dwUserTempID = tempid;
					scene->sendCmdToReverseDirect(oldPosI,
						scene->getScreenDirect(oldPosI,getPosI()),&removeUser,sizeof(removeUser),dupIndex);

					sendMeToNineDirect(scene->getScreenDirect(oldPosI,getPosI()));
					sendNineToMeDirect(scene->getScreenDirect(oldPosI,getPosI()));

					//校验9屏所有玩家以及Npc坐标
#if 0
					GetAllPos allNpcs(Cmd::MAPDATATYPE_NPC,this);
					GetAllPos allUsers(Cmd::MAPDATATYPE_USER,this);
					const zPosIVector &pv = scene->getNineScreen(getPosI());
					//{
					for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
					{
						scene->execAllOfScreen(zSceneEntry::SceneEntry_NPC,*it,allNpcs);
						scene->execAllOfScreen(zSceneEntry::SceneEntry_Player,*it,allUsers);
					}
					//}
					if (allNpcs.canSend())
					{
						sendCmdToMe(allNpcs.getSendBuffer(),allNpcs.getSize());
					}
					if (allUsers.canSend())
					{
						sendCmdToMe(allUsers.getSendBuffer(),allUsers.getSize());
					}
#else
					GetAllRemovePosNpc allNpcs(this);
					GetAllRemovePosUser allUsers(this);
					//const zPosIVector &pv = scene->getNineScreen(getPosI());
					const zPosIVector &pv = scene->getReverseDirectScreen(oldPosI,scene->getScreenDirect(oldPosI,getPosI()));
					//{
					for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
					{
						scene->execAllOfScreen(zSceneEntry::SceneEntry_NPC,*it,allNpcs);
						scene->execAllOfScreen(zSceneEntry::SceneEntry_Player,*it,allUsers);
					}
					//}
					if (allNpcs.canSend())
					{
						sendCmdToMe(allNpcs.getSendBuffer(),allNpcs.getSize());
					}
					if (allUsers.canSend())
					{
						sendCmdToMe(allUsers.getSendBuffer(),allUsers.getSize());
					}
#endif
				}
				//检查是否在跳转点
				//[ranqd] 需要判断用户是否移出过传送点，避免重复传送
				if (scene->checkBlock(newPos,TILE_DOOR) && m_bCanJump )
				{
					WayPoint *wp=scene->getWayPoint(newPos);
					if (wp) {
						const Point dp=wp->getRandDest();
						//if (dp)
						{
							Scene *toscene=SceneManager::getInstance().getSceneByFileName(dp.name);
							if (toscene)
							{
								changeMap(toscene,dp.pos);
							}
							else 
							{
								if (this->scene->getCountryID() == 6)
								{
									std::string fileName;
									char temp[128];
									bzero(temp,sizeof(temp));
									sprintf(temp,"%d",this->charbase.country);
									fileName = temp;
									fileName+= ".";
									fileName+= this->scene->getRealFileName(dp.name);
									toscene=SceneManager::getInstance().getSceneByFileName(fileName.c_str());
									if (toscene)
									{
										changeMap(toscene,dp.pos);
									}
									else
									{
										if (this->scene->getCountryID() == 6)
										{
											std::string fileName;
											char temp[128];
											bzero(temp,sizeof(temp));
											sprintf(temp,"%d",this->charbase.country);
											fileName = temp;
											fileName+= ".";
											fileName+= this->scene->getRealFileName(dp.name);
											toscene=SceneManager::getInstance().getSceneByFileName(fileName.c_str());
											if (toscene)
											{
												changeMap(toscene,dp.pos);
											}
											else
											{
												if (guard && guard->canMove()) saveGuard = true;
												if (adoptList.size()) saveAdopt = true;
												Cmd::Session::t_changeScene_SceneSession cmd;
												//Zebra::logger->debug("切换场景服务器(%s,%d,%d)",fileName.c_str(),dp->pos.x,dp->pos.y);
												cmd.id = id;
												cmd.temp_id = tempid;
												cmd.x = dp.pos.x;
												cmd.y = dp.pos.y;
												cmd.map_id = 0;
												strncpy((char *)cmd.map_file,fileName.c_str(),MAX_NAMESIZE);
												sessionClient->sendCmd(&cmd,sizeof(cmd));
											}
										}
										else
										{
											if (guard && guard->canMove()) saveGuard = true;
											if (adoptList.size()) saveAdopt = true;
											Cmd::Session::t_changeScene_SceneSession cmd;
											//Zebra::logger->debug("切换场景服务器(%s,%d,%d)",dp->name,dp->pos.x,dp->pos.y);
											cmd.id = id;
											cmd.temp_id = tempid;
											cmd.x = dp.pos.x;
											cmd.y = dp.pos.y;
											cmd.map_id = 0;
											strncpy((char *)cmd.map_file,dp.name,MAX_NAMESIZE);
											sessionClient->sendCmd(&cmd,sizeof(cmd));
										}
									}
								}
								else
								{
									if (guard && guard->canMove()) saveGuard = true;
									if (adoptList.size()) saveAdopt = true;
									Cmd::Session::t_changeScene_SceneSession cmd;
									//Zebra::logger->debug("切换场景服务器(%s,%d,%d)",dp->name,dp->pos.x,dp->pos.y);
									cmd.id = id;
									cmd.temp_id = tempid;
									cmd.x = dp.pos.x;
									cmd.y = dp.pos.y;
									cmd.map_id = 0;
									strncpy((char *)cmd.map_file,dp.name,MAX_NAMESIZE);
									sessionClient->sendCmd(&cmd,sizeof(cmd));
								}
							}
						}
					}
				}
			}
		}
	}


	//sprintf(stderr,"walk here3\n");
	if (!canmove)
	{
		//  sprintf(stderr,"walk here4\n");
		ret.byDirect=getDir();
		ret.bySpeed=0;
		ret.x=getPos().x;
		ret.y=getPos().y;
		sendCmdToMe(&ret,sizeof(ret));
		//不能移动,试图清空前面阻挡,避免空气墙
		zPos p(rev->x,rev->y);
		scene->clearBlock(p);
	}
	// [ranqd] 判断玩家是否在传送点上，避免重复传送
	if(scene->checkBlock(this->pos,TILE_DOOR))
	{
		m_bCanJump = false;
	}
	else
	{
		m_bCanJump = true;
	}
#endif
	return true;
}

/**
* \brief 处理角色后退移动命令
*
*
* \param direct 移动方向
* \param grids 格数
*
* \return 移动命令处理成功返回TRUE,否则返回FALSE
*/
bool SceneUser::backOff(const int direct,const int grids)
{
#if 0
	const int walk_adjust[9][2]= { {0,-1},{1,-1},{1,0},{1,1},{0,1},{-1,1},{-1,0},{-1,-1},{0,0} };
	int i = 0;

	zPosI oldPosI = getPosI();
	zPos oldPos = getPos(),newPos = getPos();
	for(i = 1; i <= grids; i++)
	{
		newPos.x += walk_adjust[direct][0];
		newPos.y += walk_adjust[direct][1];
		if (scene->checkBlock(newPos))
			break;
	}
	if (i > 1)
	{
		Zebra::logger->debug("后退前坐标(%u,%u)",oldPos.x,oldPos.y);
		if (scene->refresh(this,newPos))
		{
			scene->setBlock(newPos);
			scene->clearBlock(oldPos);
			setDir(scene->getReverseDirect(direct));

			Zebra::logger->debug("后退成功坐标(%u,%u)",newPos.x,newPos.y);
			Cmd::stBackOffMagicUserCmd ret;
			ret.dwTempID = this->tempid;
			ret.byType = Cmd::MAPDATATYPE_USER;
			ret.byDirect = direct;
			ret.x = newPos.x;
			ret.y = newPos.y;
			//检查是否隐身
			if (SceneEntry_Hide!=getState() && !this->hideme && !Soulflag)
				scene->sendCmdToNine(oldPosI,&ret,sizeof(ret),dupIndex);
			else
			{
				//隐身则不占用block
				scene->clearBlock(newPos);
				sendCmdToMe(&ret,sizeof(ret));
			}
			if (oldPosI != getPosI())
			{
				Cmd::stRemoveUserMapScreenUserCmd removeUser;
				removeUser.dwUserTempID = tempid;
				scene->sendCmdToReverseDirect(oldPosI,
					scene->getScreenDirect(oldPosI,getPosI()),&removeUser,sizeof(removeUser),dupIndex);
				sendMeToNineDirect(scene->getScreenDirect(oldPosI,getPosI()));
				sendNineToMeDirect(scene->getScreenDirect(oldPosI,getPosI()));

				//校验9屏所有玩家以及Npc坐标
#if 0
				GetAllPos allNpcs(Cmd::MAPDATATYPE_NPC,this);
				GetAllPos allUsers(Cmd::MAPDATATYPE_USER,this);
				const zPosIVector &pv = scene->getNineScreen(getPosI());
				for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
				{
					scene->execAllOfScreen(zSceneEntry::SceneEntry_NPC,*it,allNpcs);
					scene->execAllOfScreen(zSceneEntry::SceneEntry_Player,*it,allUsers);
				}
				if (allNpcs.canSend())
				{
					sendCmdToMe(allNpcs.getSendBuffer(),allNpcs.getSize());
				}
				if (allUsers.canSend())
				{
					sendCmdToMe(allUsers.getSendBuffer(),allUsers.getSize());
				}
#else
				GetAllRemovePosNpc allNpcs(this);
				GetAllRemovePosUser allUsers(this);
				//const zPosIVector &pv = scene->getNineScreen(getPosI());
				const zPosIVector &pv = scene->getReverseDirectScreen(oldPosI,scene->getScreenDirect(oldPosI,getPosI()));
				//{
				for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
				{
					scene->execAllOfScreen(zSceneEntry::SceneEntry_NPC,*it,allNpcs);
					scene->execAllOfScreen(zSceneEntry::SceneEntry_Player,*it,allUsers);
				}
				//}
				if (allNpcs.canSend())
				{
					sendCmdToMe(allNpcs.getSendBuffer(),allNpcs.getSize());
				}
				if (allUsers.canSend())
				{
					sendCmdToMe(allUsers.getSendBuffer(),allUsers.getSize());
				}
#endif
			}
		}
	}
	backOffing = 1000;
#endif
	return true;
}

/**
* \brief 根据场景计算玩家死亡后应该回到的地图
*
* \param s 场景
*
*/
void SceneUser::setDeathBackToMapID(Scene *s)
{
	if (s->getCountryID() == 6)
	{
		if (s->getCommonCountryBacktoMapID())
		{
			deathBackToMapID = (s->getCountryID() << 16 ) + s->getCommonCountryBacktoMapID();
		}
		else
		{
			deathBackToMapID = (this->charbase.country << 16 ) + s->getForeignerBacktoMapID();
		}
	}
	else
	{
		if (this->charbase.country == 6)
		{
			deathBackToMapID = (this->charbase.country << 16 ) + s->getCommonUserBacktoMapID();
		}
		else
		{
			deathBackToMapID = (this->charbase.country << 16 ) + s->getForeignerBacktoMapID();
		}
	}
}

/**
* \brief 使角色进入一个指定的场景的指定位置
*
* \param newscene 要进入的场景
* \param needInitInfo 进入场景所需要的一些初始化信息
* \param initPos 进入的坐标位置
*
*/
bool SceneUser::intoScene(Scene *newscene,bool needInitInfo,const zPos & initPos)
{
	zPos findedPos;
	//if (scene!=NULL || newscene==NULL /*&& !newscene->addUser(this)*/)
	if (newscene==NULL /*&& !newscene->addUser(this)*/)
	{
		Zebra::logger->error("%s(%d)进入场景%s失败",name,id,newscene?newscene->name:"");
		return false;
	}
	bool founded=false;
	if (this->charbase.gomaptype)
	{
		founded = newscene->randzPosByZoneType(this->charbase.gomaptype,findedPos);
		if (!founded)
		{
			founded = newscene->randzPosByZoneType(ZoneTypeDef::ZONE_NONE,findedPos);
		}
		this->charbase.gomaptype=0;
	}
	else
	{
		if ((!initPos.x && !initPos.y) || (initPos.x > newscene->width() || initPos.y > newscene->height()))
		{
			if (this->charbase.country == newscene->getCountryID())
			{
				founded = newscene->randzPosByZoneType(ZoneTypeDef::ZONE_RELIVE,findedPos);
			}
			else
			{
				founded = newscene->randzPosByZoneType(ZoneTypeDef::ZONE_FOREIGN_RELIVE,findedPos);
				if (!founded)
				{
					founded = newscene->randzPosByZoneType(ZoneTypeDef::ZONE_RELIVE,findedPos);
				}
			}
			// */
			if (!founded)
			{
				founded = newscene->randzPosByZoneType(ZoneTypeDef::ZONE_NONE,findedPos);
			}
		}
		else
		{
			founded = newscene->findPosForUser(initPos,findedPos);
			if (!founded)
			{
				founded = newscene->randzPosByZoneType(ZoneTypeDef::ZONE_NONE,findedPos);
			}
		}
	}
	if (newscene->refresh(this,founded ? findedPos : initPos))
	{
		//本地图不能组队
		if (newscene->noTeam())
		{
			//leaveTeam(); 
		}

		//sky 判断是否是动态场景类
		if(newscene->IsGangScene())
		{
			//sky 是就特殊处理下阵营问题
//			((GangScene*)newscene)->AddUserToScene(this->tempid, initPos);
		}

		//通知网关用户刷新场景服务器
		Cmd::Scene::t_Refresh_LoginScene refresh;
		refresh.dwUserID=id;
		refresh.dwSceneTempID=newscene->tempid;
		gatetask->sendCmd(&refresh,sizeof(refresh));
		//再刷屏索引到网关
		newscene->freshGateScreenIndex(this,getPosI());

		//设置死亡后回到的地图
		setDeathBackToMapID(newscene); 
		scene=newscene;
		scene->addUserCount();
		//Zebra::logger->debug("场景%s目前在线人数%u",scene->name,scene->countUser());
		//检查是否隐身
		if (SceneEntry_Hide!=getState() && !this->hideme && !Soulflag)
			newscene->setBlock(getPos());
		Zebra::logger->info("��(%s,%d)����%s, ������Ŀǰ��������%u", name,id,newscene->name,scene->countUser());
//		set_me(this);
#if 0
		//功能npc
		struct FunctionNpc : public zSceneEntryCallBack
		{
			Cmd::stMapScreenSizeDataUserCmd *mapscreen;
			FunctionNpc(Cmd::stMapScreenSizeDataUserCmd *buf)
			{
				mapscreen = buf;
			}

			bool exec(zSceneEntry *entry)
			{
				SceneNpc *npc = (SceneNpc *)entry;
				if (npc->npc)
				{
					mapscreen->npc_list[mapscreen->npc_count].id = npc->npc->id;
					mapscreen->npc_list[mapscreen->npc_count].x = npc->define->pos.x;
					mapscreen->npc_list[mapscreen->npc_count].y=npc->define->pos.y;
					++mapscreen->npc_count;
					if (mapscreen->npc_count >= 300)
					{
						return false;
					}
				}
				return true;
			}
		};
#endif

		//this->skillStatusM.processPassiveness();
#ifndef _MOBILE
		//地图信息
		Cmd::stMapScreenSizeDataUserCmd *mapscreen;
		char Buf[sizeof(Cmd::stMapScreenSizeDataUserCmd) + 300 * sizeof(mapscreen->npc_list[0])];
		bzero(Buf,sizeof(Buf));
		mapscreen = (Cmd::stMapScreenSizeDataUserCmd *)Buf;
		constructInPlace(mapscreen);
		mapscreen->width=scene->width();
		mapscreen->height=scene->height();
		mapscreen->mainRoleX = getPos().x;
		mapscreen->mainRoleY = getPos().y;
		mapscreen->PlayerX = getPos().x;
		mapscreen->PlayerY = getPos().y;
		mapscreen->m_sRegion = scene->getCountryID();
		mapscreen->area = scene->getRealMapID();
		strncpy(mapscreen->area_name, scene->getName(), MAX_NAMESIZE-1);
		strncpy(mapscreen->pstrMapName,scene->getName(),MAX_NAMESIZE-1);
		strncpy(mapscreen->pstrFilename,scene->getRealFileName(),MAX_NAMESIZE-1);
		Zebra::logger->debug("map file name %s",mapscreen->pstrFilename);
		if (SceneManager::getInstance().getCountryNameByCountryID(scene->getCountryID()))
		{
			strncpy(mapscreen->pstrCountryName,SceneManager::getInstance().getCountryNameByCountryID(scene->getCountryID()),MAX_NAMESIZE-1);
		}
		mapscreen->setting=0;
		mapscreen->rgb=0;
//		FunctionNpc npc_exec(mapscreen);
//		scene->execAllOfScene_functionNpc(npc_exec);
		sendCmdToMe(mapscreen,sizeof(Cmd::stMapScreenSizeDataUserCmd)+mapscreen->npc_count*sizeof(mapscreen->npc_list[0]));
		Zebra::logger->debug("map data size:%u mapName:%s",sizeof(Cmd::stMapScreenSizeDataUserCmd)+mapscreen->npc_count*sizeof(mapscreen->npc_list[0]),scene->name);
#endif
		//骑马检测
		if (!scene->canRide())
		{
			//dwHorseID=0;
			//clear_state(byState,Cmd::USTATE_RIDE);
//			horse.mount(false,false);
			/*
			Cmd::stRideMapScreenUserCmd ret;
			ret.bySwitch=0;
			ret.dwUserTempID=tempid;
			scene->sendCmdToNine(getPosI(),&ret,sizeof(ret));
			*/
		}
		//必要时发送主用户信息
		if (needInitInfo)
		{
			sendInitToMe();
			//MessageSystem::getInstance().check(this,true);
		    	OnEnter enter(0);
			EventTable::instance().execute(*this,enter);
		}
		else
		{
			Cmd::stMainUserDataUserCmd  userinfo;
			full_t_MainUserData(userinfo.data);
			sendCmdToMe(&userinfo,sizeof(userinfo));
			sendInitHPAndMp();
			/*
			//结束初始化
			Cmd::stEndOfInitDataDataUserCmd  endcmd;
			sendCmdToMe(&endcmd,sizeof(endcmd));
			// */
		}

		if ((this->getPriv() & Gm::super_mode)||(this->getPriv() & Gm::gm_mode)||(this->getPriv() & Gm::captain_mode))
		{
			Gm::hideMe(this,"");
			Gm::god(this,"");
		}

		//通知网关用户场景变化
		Cmd::Scene::t_countryAndScene_GateScene noti;
		noti.userID = id;
		noti.countryID = charbase.country;
		noti.sceneID = SceneManager::getInstance().getMapIDByMapName(scene->name);
		gatetask->sendCmd(&noti,sizeof(noti));

		/*
		//宠物
		zPos petPos = getPos();
		for (std::list<ScenePet *>::iterator it=totems.begin(); it!=totems.end(); it++)
		{
		(*it)->changeMap(scene,petPos);
		}
		*/

		// 请求金币和月卡时间
		/*
		Cmd::Scene::t_Request_Bill rb;
		rb.dwUserID=this->id;
		this->gatetask->sendCmd(&rb,sizeof(rb));
		// */
		if (this->scene->getRealMapID() == 190)//如果是古墓
		{
			/*
			struct tm tm_1;
			time_t timValue = time(NULL);
			tm_1=*localtime(&timValue);
			if (tm_1.tm_hour%2 == 0 && tm_1.tm_min <= 55)
			{
			}
			else
			{
			Gm::gomap(this,"name=中立区·皇城 type=4");
			}
			// */
		}
		else if (this->scene->getRealMapID()==139
			&& this->charbase.country == this->scene->getCountryID() && this->scene->getCountryDare())
		{
			this->deathBackToMapID = SceneManager::getInstance().buildMapID(this->scene->getCountryID(),
				this->scene->getCountryDefBackToMapID());  
		}
		else if (this->scene->getRealMapID()==134 && this->scene->getEmperorDare())
		{
			Cmd::stEnterEmperorDareZone send;
			send.state = 1;
			send.dwDefCountryID = this->scene->getEmperorDareDef();

			if (this->charbase.country == this->scene->getEmperorDareDef())
			{
				this->deathBackToMapID = SceneManager::getInstance().buildMapID(this->scene->getCountryID(),
					134);  
				this->charbase.gomaptype = ZoneTypeDef::ZONE_EMPEROR_DEF_RELIVE;
			}
			else
			{
				this->deathBackToMapID = SceneManager::getInstance().buildMapID(this->scene->getCountryID(),
					134);  
				this->charbase.gomaptype = ZoneTypeDef::ZONE_EMPEROR_ATT_RELIVE;
			}

			this->sendCmdToMe(&send,sizeof(send));
		}
		/*
		else
		{
		struct tm tm_1;
		time_t timValue = time(NULL);
		tm_1=*localtime(&timValue);
		if ((tm_1.tm_hour%2) && (tm_1.tm_min >= 55))
		{
		Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"古墓地图将在%d分钟后开放",60 - tm_1.tm_min);
		}
		else if (((tm_1.tm_hour%2) == 0) && tm_1.tm_min <= 30)
		{
		Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"古墓地图已经开放%d分钟",tm_1.tm_min);
		}
		}
		// */
		if (!scene->isTrainingMap() && 0!=charbase.trainTime)
		{
			charbase.trainTime = 0;
			showCurrentEffect(Cmd::USTATE_DAOJISHI,false); // 更新客户端状态
			//sendtoSelectedTrainState();
		}

		//通知九屏添加用户
		sendMeToNine();
		//得到九屏东西发给自己
		sendNineToMe();

//		Cmd::stRTMagicPosUserCmd send;
//		this->full_stRTMagicPosUserCmd(send);
//		sendCmdToMe(&send,sizeof(send));
#if 0
		//同时处理下经验倍率时间的事情
		if( this->charbase.doubletime > 0)
		{
			if( this->issetUState(Cmd::USTATE_EXP_125) || this->issetUState(Cmd::USTATE_EXP_150) || this->issetUState(Cmd::USTATE_EXP_175 ) )
			{
				Zebra::logger->debug("已经存在特殊经验状态不需要刷新拉");
			}
			else
			{
				char Buf[200]; 
				bzero(Buf,sizeof(Buf));
				Cmd::stSelectReturnStatesPropertyUserCmd *srs=(Cmd::stSelectReturnStatesPropertyUserCmd*)Buf;
				constructInPlace(srs);
				srs->byType = Cmd::MAPDATATYPE_USER;
				srs->dwTempID = this->tempid;
				srs->size=1;

				if(charbase.bitmask & CHARBASE_EXP125) 
				{
					Zebra::logger->debug("检测到上次遗留的双倍经验时间和状态，重新设置1.25经验");
					showCurrentEffect(Cmd::USTATE_EXP_125, true);
					srs->states[0].state = Cmd::USTATE_EXP_125;
				}

				if(charbase.bitmask & CHARBASE_EXP150) 
				{
					Zebra::logger->debug("检测到上次遗留的双倍经验时间和状态，重新设置1.50经验");
					showCurrentEffect(Cmd::USTATE_EXP_150, true);
					srs->states[0].state = Cmd::USTATE_EXP_150;
				}

				if(charbase.bitmask & CHARBASE_EXP175) 
				{
					Zebra::logger->debug("检测到上次遗留的双倍经验时间和状态，重新设置1.75经验");
					showCurrentEffect(Cmd::USTATE_EXP_125, true);
					srs->states[0].state = Cmd::USTATE_EXP_175;
				}
				//通知客户端跟新状态的图标

				srs->states[0].result=charbase.doubletime/60;
				srs->states[0].time=charbase.doubletime/60;
				sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + sizeof(srs->states[0]) * srs->size);
			}
		}

		// sky 上线后在来添加以前遗留的技能状态
		skillStatusM.processPassiveness();
#endif
		ChallengeGameManager::getMe().initGameData(this);
		if(ChallengeGameManager::getMe().hasUnfinishedGame(this))
		{
		    Zebra::logger->debug("���ߺ��ٴ����� ��Ϸ���� ֪ͨ�ͻ���");
		    Cmd::stRetNotifyUnfinishedGameUserCmd ret;
		    this->sendCmdToMe(&ret, sizeof(ret));
		}
		return true;
	}
	else
	{
		if (founded) newscene->clearBlock(findedPos);
		Zebra::logger->debug("%s intoScene ����false %s ",name,newscene->name);
		return false;
	}
}

/**
* \brief 使角色离开一个场景
*
*
*/
bool SceneUser::LeaveScene()
{
	if (scene) scene->removeUser(this);
	Cmd::stRemoveUserMapScreenUserCmd remove;
	remove.dwUserTempID=tempid;
	//检查是否隐身
	//if (SceneEntry_Hide!=getState())
	scene->sendCmdToNine(getPosI(),&remove,sizeof(remove),dupIndex);
	//由于客户端需要靠这个指令来关闭对话框,所以必须发给自己一份 
	sendCmdToMe(&remove,sizeof(remove));
	//else
	//  sendCmdToMe(&ret,sizeof(ret));
	//  scene=NULL;
	//if (SceneUserManager::getMe().countUserInOneScene(scene) <= 1 
	scene->subUserCount();
	//Zebra::logger->debug("场景%s目前在线人数%u",scene->name,scene->countUser());
	if (scene->countUser() ==0 && (scene->getRunningState() == SCENE_RUNNINGSTATE_UNLOAD))
	{
		scene->setRunningState(SCENE_RUNNINGSTATE_REMOVE);
		Cmd::Session::t_removeScene_SceneSession rem;
		rem.map_id = scene->id;
		sessionClient->sendCmd(&rem,sizeof(rem));
	}

	return true;
}

/**
* \brief 角色中了恐惧效果
*/
void SceneUser::dreadProcess()
{
#if 0
	if (dread)
	{
		Cmd::stUserMoveMoveUserCmd send;
		int count=10;
		int curDir=0;

		do {
			send.dwUserTempID = this->tempid;

			send.bySpeed = 2;

			send.x = pos.x;
			send.y = pos.y;

			if (count == 10)
				curDir = getDir()+zMisc::randBetween(-1,1);
			else
				curDir++;
			if (curDir <0) curDir = 7;

			send.byDirect = curDir%8;

			const int walk_adjust[9][2]= { {0,-1},{1,-1},{1,0},{1,1},{0,1},{-1,1},{-1,0},{-1,-1},{0,0} };

			
			send.x += (2*walk_adjust[send.byDirect][0]);
			send.y += (2*walk_adjust[send.byDirect][1]);

			count--;	
			
		} while(!move(&send)&&count>0);
	}
#endif
}

/**
* \brief 角色中了失明效果
*/
void SceneUser::blindProcess()
{
#if 0
	if (blind)
	{
		Cmd::stUserMoveMoveUserCmd send;
		int count=10;
		int curDir=0;

		do {
			send.dwUserTempID = this->tempid;

			send.bySpeed = 2;

			send.x = pos.x;
			send.y = pos.y;

			if (count == 10)
				curDir = getDir()+zMisc::randBetween(-1,1);
			else
				curDir++;
			if (curDir <0) curDir = 7;

			send.byDirect = curDir%8;

			const int walk_adjust[9][2]= { {0,-1},{1,-1},{1,0},{1,1},{0,1},{-1,1},{-1,0},{-1,-1},{0,0} };

			send.x += (1*walk_adjust[send.byDirect][0]);
			send.y += (1*walk_adjust[send.byDirect][1]);

			count--;	

		} while(!move(&send)&&count>0);
	}
#endif
}


void SceneUser::initAnswerCount()
{
	time_t timValue = time(NULL);
	int value = timValue - this->charbase.offlinetime;
	struct tm tv1;
	zRTime::getLocalTime(tv1,timValue);
	time_t tempvalue = (time_t)this->charbase.offlinetime;
	struct tm tv2;
	zRTime::getLocalTime(tv2,tempvalue);


	if ((value > 24*60*60) || ((tv1.tm_mday!=tv2.tm_mday)))
	{
		this->charbase.answerCount = 1;
	}
}

void SceneUser::initTire()
{
	time_t timValue = time(NULL);
	int value = timValue - this->charbase.offlinetime;
	struct tm tv1;
	zRTime::getLocalTime(tv1,timValue);
	time_t tempvalue = (time_t)this->charbase.offlinetime;
	struct tm tv2;
	zRTime::getLocalTime(tv2,tempvalue);


	if ((value > 24*60*60) || ((tv1.tm_mday!=tv2.tm_mday)))
	{
		bzero(this->charbase.tiretime,36);
		this->wdTire = 0;
		this->wdTirePer = 100;
	}
	else
	{
		int count=0;
		for(int k=0; k<288;k++)
		{
			if (Cmd::isset_state(((BYTE*)(this->charbase.tiretime)),k))
			{
				count++;
			}
		}
		if (count > 228)
		{
			for(time_t i=this->charbase.offlinetime; i<timValue; i+=60)
			{
				struct tm tmValue;
				zRTime::getLocalTime(tmValue,i);
				int j = (tmValue.tm_hour*60+tmValue.tm_min)/5;
				Cmd::set_state(((BYTE*)(this->charbase.tiretime)),j);
			}
		}
		else
		{
			for(time_t i=this->charbase.offlinetime; i<timValue; i+=60)
			{
				struct tm tmValue;
				zRTime::getLocalTime(tmValue,i);
				int j = (tmValue.tm_hour*60+tmValue.tm_min)/5;
				Cmd::clear_state(((BYTE*)(this->charbase.tiretime)),j);
			}
		}
		count=0;
		for(int k=0; k<288;k++)
		{
			if (Cmd::isset_state(((BYTE*)(this->charbase.tiretime)),k))
			{
				count++;
			}
		}
		//Zebra::logger->debug("角色%s上线疲劳值为[%u]",this->name,count);
		this->wdTire=count;

		if (this->wdTire>=287)
		{
			bzero(this->charbase.tiretime,36);
			this->wdTire = 0;
			this->wdTirePer = 100;
		}

		if (this->wdTire > 144) //12
		{
			if (this->wdTire > 156) //13
			{
				if (this->wdTire > 168) //14
				{
					if (this->wdTire > 180) //15
					{
						if (this->wdTire > 192) //16
						{
							if (this->wdTire > 204) //17
							{
								this->wdTirePer =0;
							}
							else
							{
								this->wdTirePer =20;
							}
						}
						else
						{
							this->wdTirePer =40;
						}
					}
					else
					{
						this->wdTirePer =60;
					}
				}
				else
				{
					this->wdTirePer =80;
				}
			}
			else
			{
				this->wdTirePer =90;
			}
		}
		else
		{
			this->wdTirePer =100;
		}
	}
	//Cmd::stMainUserDataUserCmd  userinfo;
	//full_t_MainUserData(userinfo.data);
	//sendCmdToMe(&userinfo,sizeof(userinfo));
}

/**             
* \brief 处理答题次数
* 
*/     
void SceneUser::processAnswerCount()
{               
	time_t timValue = time(NULL);
	struct tm tmValue;
	zRTime::getLocalTime(tmValue,timValue);

	if (tmValue.tm_hour==0 && (tmValue.tm_min==0 || tmValue.tm_min == 1))
	{       
		this->charbase.answerCount = 1;
	}       
}    

/**
* \brief 处理疲劳功能计算
* 五分钟采样 8*60/5=96 24*60/5=288
*/
void SceneUser::callProcessTire()
{
	this->processTire();
}

/**
* \brief 处理疲劳功能计算
* 五分钟采样 8*60/5=96 24*60/5=288
*/
void SceneUser::processTire()
{
	bool bProcess;
	bProcess = true;
	time_t timValue = time(NULL);
	struct tm tmValue;
	WORD bchange = this->wdTirePer;
	zRTime::getLocalTime(tmValue,timValue);
	if ((tmValue.tm_hour==0 && ((tmValue.tm_min >=0) && (tmValue.tm_min < 5)))||(this->wdTire >= 287))
	{
		bzero(this->charbase.tiretime,36);
		this->wdTire = 0;
		this->wdTirePer =100;
	}
	if (this->scene &&(this->charbase.country==this->scene->getCountryID()))
	{
		DWORD dwMapID = this->scene->getRealMapID();
		if ((dwMapID == 102) || (dwMapID == 139))
		{
			if (this->scene->getZoneType(this->pos)>ZoneTypeDef::ZONE_NONE)
			{
				bProcess = false;
			}
		}
	}
	else
	{
		bProcess = false; //在国外也不涨疲劳度
	}
	if (this->wdTire > 228 || (bProcess && this->wdTire <= 228))
	{
		int value = (tmValue.tm_hour*60+tmValue.tm_min)/5;
		if (!Cmd::isset_state(((BYTE*)(this->charbase.tiretime)),value))   this->wdTire++;
		Cmd::set_state(((BYTE*)(this->charbase.tiretime)),value);
	}

#ifdef _DEBUG
	Zebra::logger->debug("-------------------------------------------------------------------------");
	Zebra::logger->debug("[%u][%u][%u][%u][%u][%u][%u][%u][%u][%u]",(BYTE)this->charbase.tiretime[0],
		(BYTE)this->charbase.tiretime[1],
		(BYTE)this->charbase.tiretime[2],
		(BYTE)this->charbase.tiretime[3],
		(BYTE)this->charbase.tiretime[4],
		(BYTE)this->charbase.tiretime[5],
		(BYTE)this->charbase.tiretime[6],
		(BYTE)this->charbase.tiretime[7],
		(BYTE)this->charbase.tiretime[8],
		(BYTE)this->charbase.tiretime[9]);
#endif
#ifdef _DEBUG
	int value = (tmValue.tm_hour*60+tmValue.tm_min)/5;
	Zebra::logger->debug("当前疲劳记数[%u] 小时[%u] 分钟[%u] 当前时间值value[%u]",this->wdTire,tmValue.tm_hour,tmValue.tm_min,value);
#endif

	if (this->wdTire > 144) //12
	{
		if (this->wdTire > 156) //13
		{
			if (this->wdTire > 168) //14
			{
				if (this->wdTire > 180) //15
				{
					if (this->wdTire > 192) //16
					{
						if (this->wdTire > 204) //17
						{
							this->wdTirePer =0;
						}
						else
						{
							this->wdTirePer =20;
						}
					}
					else
					{
						this->wdTirePer =40;
					}
				}
				else
				{
					this->wdTirePer =60;
				}
			}
			else
			{
				this->wdTirePer =80;
			}
		}
		else
		{
			this->wdTirePer =90;
		}
	}
	else
	{
		this->wdTirePer =100;
	}

	if (bchange != this->wdTirePer)
	{
		Cmd::stMainUserDataUserCmd  userinfo;
		full_t_MainUserData(userinfo.data);
		sendCmdToMe(&userinfo,sizeof(userinfo));
	}
	this->charbase.offlinetime = timValue;
}

/**
* \brief 定时更新角色的属性,并保存到档案服务器
*
* \return 始终返回TRUE
*/
bool SceneUser::refreshMe()
{
#if 0
	if (mplock)
	{
		charbase.mp = charstate.maxmp;
	}
	if (hplock)
	{
		charbase.hp = charstate.maxhp;
	}
	//if (splock)
	//{
	//  charbase.sp = charstate.maxsp;
	//}
	//if (checkMessageTime(SceneTimeTick::currentTime))
	//  MessageSystem::getInstance().check(this);

	//检查善恶度
	if (checkGoodnessTime(SceneTimeTick::currentTime))
		checkGoodness();

	if (_five_min(SceneTimeTick::currentTime))
	{
		Cmd::Session::t_countryNotify_SceneSession send_gem;
		if (this->issetUState(Cmd::USTATE_TOGETHER_WITH_TIGER))
		{
			_snprintf(send_gem.info,sizeof(send_gem.info),
				"虎魄附体持有者正在向 %s %d,%d 方向逃窜",this->scene->getName(),
				getPos().x,getPos().y);

			send_gem.dwCountryID = this->charbase.country;
			sessionClient->sendCmd(&send_gem,sizeof(send_gem));
		}

		if (this->issetUState(Cmd::USTATE_TOGETHER_WITH_DRAGON))
		{
			_snprintf(send_gem.info,sizeof(send_gem.info),
				"龙精附体持有者正在向 %s %d,%d 方向逃窜",this->scene->getName(),
				getPos().x,getPos().y);
			send_gem.dwCountryID = this->charbase.country;
			sessionClient->sendCmd(&send_gem,sizeof(send_gem));
		}
	}

	if(_half_sec(SceneTimeTick::currentTime))
	{
		if (dread) dreadProcess();
	}
	
	if (_one_sec(SceneTimeTick::currentTime))
	{
		if (npcdareflag) checkNpcDareState();
		//上次pk时间
		if (lastPkTime)
		{
			lastPkTime --;
		}
		if (backOffing)
		{
			if (backOffing- 1000 > 0)
			{
				//backOff(getDir(),3);
				backOffing -= 1000;
			}
			else
			{
				backOffing = 0;
			}
		}

		if (this->getState() == zSceneEntry::SceneEntry_Death)
		{

			//如果处在死亡等待状态
			if (deathWaitTime > 0)
			{
				deathWaitTime --;
			}

			if (deathWaitTime == 1)
			{
				deathWaitTime = 0;
				this->relive(Cmd::ReliveHome,0,70);
				//this->reliveReady(NULL,true);
			}
		}

		// TODO 复活虚拟状态不进行自动恢复
		if (this->charbase.reliveWeakTime > 0)
		{
			if (this->charbase.reliveWeakTime <= SceneTimeTick::currentTime.sec()%10000)
			{// 调用预处理,重新计算五项属性
				this->charbase.reliveWeakTime = 0;
				this->setupCharBase();

				showCurrentEffect(Cmd::USTATE_RELIVEWEAK,false); // 更新客户端状态
				Cmd::stMainUserDataUserCmd  userinfo;
				full_t_MainUserData(userinfo.data);
				sendCmdToMe(&userinfo,sizeof(userinfo));
				//this->save(Cmd::Record::OPERATION_WRITEBACK);
			}
		}

		restitute();

		skillStatusM.timer();

		if (miniGame) miniGame->timer(SceneTimeTick::currentTime.sec(),this);

		//sky 在秒定时器里检测下战斗时间是否到拉
		if(IsPveOrPvp() != USE_FIGHT_NULL)
		{
			if(IsPkTimeOver())
			{
				//sky 玩家脱离战斗把战斗状态设置为NULL状态
				switch(IsPveOrPvp())
				{
				case USE_FIGHT_PVE:
					Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你已经退PVE模式");
					break;
				case USE_FIGHT_PVP:
					Channel::sendSys(this, Cmd::INFO_TYPE_GAME, "你已经退PVP模式");
				}

				SetPveOrPvp(USE_FIGHT_NULL);
			}
		}

		if (_3_sec(SceneTimeTick::currentTime))
		{
			if(blind) blindProcess();
		}

		// 将多秒钟定时器放到5秒钟里可以做一点优化^_^
		if (_five_sec(SceneTimeTick::currentTime)) 
		{

			_5_sec_count++;

		}

		// 将1分钟定时器放到5秒钟里可以做一点优化^_^
		if (_one_min(SceneTimeTick::currentTime))
		{
			if ((atoi(Zebra::global["service_flag"].c_str())&Cmd::Session::SERVICE_PROCESS))
			{
				if (dropTime && dropTime<SceneTimeTick::currentTime.sec()//断线时间到了
					&& !guard)//运镖时不踢
				{
					Cmd::stMapDataMapScreenUserCmd send;
					send.mdih.size = zMisc::randBetween(1024,60000);
					send.mdih.type = Cmd::MAPDATATYPE_NPC;
					sendCmdToMe(&send,sizeof(send));

					Zebra::logger->info("%s 向使用外挂的玩家发送非法信息",name);
					dropTime = 0;
				}

				if (0==processCheckTime)
				{
					if (ScenesService::pStampData->dwChannelID)
					{
						sendCmdToMe(ScenesService::pStampData,sizeof(Cmd::stChannelChatUserCmd)+ScenesService::pStampData->dwFromID);
						Zebra::logger->debug("%s(%u) 客户端检查外挂",name,id);
					}
#ifdef _DEBUG
					processCheckTime = 0;
#else
					processCheckTime = zMisc::randBetween(60,120);
#endif
				}
				else
					processCheckTime--;
#ifdef _DEBUG
				Zebra::logger->debug("%s(%u) 下次检查间隔 %u 分钟",name,id,processCheckTime);
#endif
			}

			//检查被捕时间
			checkPunishTime();

			//和平追加善恶描述
			if (getGoodnessState() <= (short)Cmd::GOODNESS_2_1)
			{
				charbase.pkaddition++;
				if (charbase.pkaddition > 1800 /*30*60*/)
				{
					if (!this->issetUState(Cmd::USTATE_PK))
					{
						Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"您进入了PK保护状态！");
					}
					this->setUState(Cmd::USTATE_PK);
					reSendMyMapData();
					/*
					char Buf[200]; 
					bzero(Buf,sizeof(Buf));
					Cmd::stSelectReturnStatesPropertyUserCmd *srs=(Cmd::stSelectReturnStatesPropertyUserCmd*)Buf;
					constructInPlace(srs);
					srs->byType = Cmd::MAPDATATYPE_USER;
					srs->dwTempID = this->tempid;
					srs->size=1;
					srs->states[0].state = Cmd::USTATE_PK;
					srs->states[0].result=charbase.pkaddition;
					srs->states[0].time=0XFFFF;
					this->sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + 
					sizeof(srs->states[0]) * srs->size);
					// */
					this->sendtoSelectedPkAdditionState();
				}
			}

			if (charbase.trainTime)
			{
				if (charbase.trainTime>=60)
					charbase.trainTime -= 60;
				else
				{
					charbase.trainTime = 0;
					this->clearUState(Cmd::USTATE_DAOJISHI);
				}
				sendtoSelectedTrainState();
			}

			//踢出修炼地图
			if (scene->isTrainingMap() && 0==charbase.trainTime)
			{
				Scene * s = SceneManager::getInstance().getSceneByName("中立区·皇城");
				if (s)
				{
					bool suc = changeMap(s,zPos(806,716));
					if (!suc)
						Zebra::logger->error("%s 离开训练地图失败,目的 %s (%d,%d)",name,s->name,pos.x,pos.y);
					else
						Zebra::logger->error("%s 离开训练地图",name);
				}
				else
				{
					Cmd::Session::t_changeScene_SceneSession cmd;
					cmd.id = id;
					cmd.temp_id = tempid;
					cmd.x = 806;
					cmd.y = 716;
					cmd.map_id = 0;
					cmd.map_file[0] = '\0';
					strncpy((char *)cmd.map_name,"中立区·皇城",MAX_NAMESIZE);
					sessionClient->sendCmd(&cmd,sizeof(cmd));
				}
			}

			//检测马匹训练时间
			if( horse.data.horseXLlevel > 0 )
			{
				if(horse.data.horseXLtime > 60)
				{
					Zebra::logger->debug("原训练剩余时间为:%u",horse.data.horseXLtime);
					horse.data.horseXLtime -= 60;
					Zebra::logger->debug("扣除后的训练时间为:%u",horse.data.horseXLtime);
					horse.sendData();
				}
				else
				{
					horse.data.horseXLtime	= 0;
					if( horse.data.horseXLlevel == 1 )
					{
						horse.data.speed	-= 10;
					}
					else if( horse.data.horseXLlevel == 2 )
					{
						horse.data.speed	-= 20;
					}
					else if( horse.data.horseXLlevel == 3 )
					{
						horse.data.speed	-= 30;
					}
					horse.data.horseXLlevel = 0;
					horse.mount(false);
					horse.sendData();
					Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"训练有效时间已到，移除战马训练效果");
				}
			}

			//检测经验倍率时间
			if( this->issetUState(Cmd::USTATE_EXP_125) || this->issetUState(Cmd::USTATE_EXP_150) || this->issetUState(Cmd::USTATE_EXP_175 ))
			{
				if (charbase.doubletime>=60)
				{
					charbase.doubletime -= 60;
				}
				else
				{
					charbase.doubletime = 0;
					if(this->issetUState(Cmd::USTATE_EXP_125))
					{
						if(charbase.bitmask & CHARBASE_EXP125) {
							charbase.bitmask &= (~CHARBASE_EXP125);
						}
						Zebra::logger->debug("时间到拉移除特殊经验状态");
						showCurrentEffect(Cmd::USTATE_EXP_125, false);
					}
					if(this->issetUState(Cmd::USTATE_EXP_150))
					{
						if(charbase.bitmask & CHARBASE_EXP150) {
							charbase.bitmask &= (~CHARBASE_EXP150);
						}
						Zebra::logger->debug("时间到拉移除特殊经验状态");
						showCurrentEffect(Cmd::USTATE_EXP_150, false);
					}
					if(this->issetUState(Cmd::USTATE_EXP_175))
					{
						if(charbase.bitmask & CHARBASE_EXP175) {
							charbase.bitmask &= (~CHARBASE_EXP175);
						}
						Zebra::logger->debug("时间到拉移除特殊经验状态");
						showCurrentEffect(Cmd::USTATE_EXP_175, false);
					}
				}
			}

			countFriendDegree(); //计算友好度
			/*
			* whj 应林总强烈要求暂时屏蔽疲劳度功能
			*/
			processTire();
			processAnswerCount();

			//时间消耗类装备,现在只有双倍经验类和自动补给类
			if (this->getState() != SceneUser::SceneEntry_Death)
			{
				zObject *obj = this->packs.equip.getObjectByEquipPos(Cmd::EQUIPCELLTYPE_ADORN);
				if (obj)
					switch(obj->base->kind)
				{
					case ItemType_DoubleExp:
						{
							packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN,ItemType_DoubleExp,60,true,true);
						}
						break;
					case ItemType_Tonic:
						{
							packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN,ItemType_Tonic,60,true,false);
						}
						break;
					case ItemType_FashionBody:
						{
							packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN,ItemType_FashionBody,1,true,false);
						}
						break;
					case ItemType_HighFashionBody:
						{
							packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN,ItemType_HighFashionBody,1,true,false);
						}
						break;
					case ItemType_Amulet:
						/*{ //sky 这个类型处理已经无用
							if (this->scene->isIncScene())
							{
								packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN,ItemType_Amulet,60,true,false);
							}
						}*/
						break;
				}
				obj = this->packs.equip.getObjectByEquipPos(Cmd::EQUIPCELLTYPE_ADORN + 1);
				if (obj)
					switch(obj->base->kind)
				{
					case ItemType_DoubleExp:
						{
							packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN + 1,ItemType_DoubleExp,60,true,true);
						}
						break;
					case ItemType_Tonic:
						{
							packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN + 1,ItemType_Tonic,60,true,false);
						}
						break;
					case ItemType_FashionBody:
						{
							packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN + 1,ItemType_FashionBody,1,true,false);
						}
						break;
					case ItemType_HighFashionBody:
						{
							packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN + 1,ItemType_HighFashionBody,1,true,false);
						}
						break;
					case ItemType_Amulet:
						/*{ //sky 这个类型处理已经无用
							if (this->scene->isIncScene())
							{
								packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN + 1,ItemType_Amulet,60,true,false);
							}
						}*/
						break;
				}
			}
			if (this->packs.equip.pack(EquipPack::L_PACK)) this->packs.equip.pack(EquipPack::L_PACK)->consume_dur_by(this,SceneTimeTick::currentTime);
			if (this->packs.equip.pack(EquipPack::R_PACK)) this->packs.equip.pack(EquipPack::R_PACK)->consume_dur_by(this,SceneTimeTick::currentTime);
		}

		if (0==_5_sec_count%6)//30sec
		{
			for (cartoon_it it=cartoonList.begin(); it!=cartoonList.end(); it++)
			{
				if ((cartoon && cartoon->getCartoonID()==it->first)
					|| it->second.state==Cmd::CARTOON_STATE_WAITING
					|| it->second.state==Cmd::CARTOON_STATE_ADOPTED)
					continue;

				if (it->second.sp<it->second.maxSp)
				{
					it->second.sp++;
					if (0==it->second.sp%10)
					{
						Cmd::stAddCartoonCmd send;
						send.isMine = true;
						send.cartoonID = it->first;
						send.data = it->second;
						sendCmdToMe(&send,sizeof(send));
					}
				}
			}
		}

		//PK地图15秒加经验
		if (0==_5_sec_count%3 && scene->isPkMap())//15sec
		{
			addExp(charbase.level*charbase.level/4);
		}

		//if (_ten_sec(SceneTimeTick::currentTime))
		//{
			//mask.on_timer();
			//checkNpcHoldDataAndPutExp();
			////队长负责队员两分钟掉线的检查
			//TeamManager * team = SceneManager::getInstance().GetMapTeam(TeamThisID);

			//if(team)
			//{
			//	SceneUser *leader = SceneUserManager::getMe().getUserByTempID(team->getLeader());
			//	if (leader&&(leader->tempid == tempid))
			//	{
			//		team->checkOffline(leader);
			//	}
			//}
		//}

		//回写档案
		if (_writeback_timer(SceneTimeTick::currentTime))
		{
			save(Cmd::Record::TIMETICK_WRITEBACK);
		}
	}

	if (summon || pet)
	{
		SceneEntryPk * dt = getDefTarget();
		if (dt)
		{
			if (dt->scene!=this->scene
				|| dt->getState()!=SceneEntry_Normal
				|| !scene->zPosShortRange(dt->getPos(),getPos(),20))
				clearDefTarget();
		}

		if (checkEndBattleTime(SceneTimeTick::currentTime)&&(0!=curTargetID||0!=defTargetID))
		{
			leaveBattle();
		}
	}
#endif
	if (_one_sec(SceneTimeTick::currentTime))
	{
	    if (_writeback_timer(SceneTimeTick::currentTime))
	    {
		save(Cmd::Record::TIMETICK_WRITEBACK);
	    }
	}
	return true;
}



/**
* \brief 取消某个状态给用户
*
*/

void SceneUser::clearStateToNine(WORD state)
{
	//Zebra::logger->debug("clearStateToNine:%d",state);
	if (!scene) return;
	Cmd::stClearStateMapScreenUserCmd send;
	send.type=Cmd::MAPDATATYPE_USER;
	send.dwTempID = this->tempid;
	send.wdState =state;
	//检查是否隐身
	if (SceneEntry_Hide!=getState() && !this->hideme && !Soulflag)
	{
		this->scene->sendCmdToNine(getPosI(),&send,sizeof(send),dupIndex);
	}
	else
	{
		sendCmdToMe(&send,sizeof(send));
	}
}
/**
* \brief 设置某个状态给用户
*/

void SceneUser::setStateToNine(WORD state)
{
#if 0
	//Zebra::logger->debug("setStateToNine:%d",state);
	if (!scene) return;
	Cmd::stSetStateMapScreenUserCmd send;
	send.type=Cmd::MAPDATATYPE_USER;
	send.dwTempID = this->tempid;
	send.wdState =state;
	//检查是否隐身
	if (SceneEntry_Hide!=getState() && !this->hideme && !Soulflag)
	{
		if (state == Cmd::USTATE_WAR)
		{
			SendStateWarToNineEveryOne one(this,send.wdState);
			const zPosIVector &pv = scene->getNineScreen(getPosI());
			for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
			{
				scene->execAllOfScreen(zSceneEntry::SceneEntry_Player,*it,one);
			}
		}
		else
		{
			this->scene->sendCmdToNine(getPosI(),&send,sizeof(send),dupIndex);
		}
	}
	else
	{
		sendCmdToMe(&send,sizeof(send));
	}
#endif
}

void SceneUser::sendGoodnessToNine()
{
	//Zebra::logger->debug("setStateToNine:%d",state);
	if (!scene) return;
	Cmd::stGoodnessStateMapScreenUserCmd send;
	send.dwTempID = this->tempid;
	send.dwGoodness =this->charbase.goodness;
	//检查是否隐身
	if (SceneEntry_Hide!=getState() && !this->hideme && !Soulflag)
	{
		this->scene->sendCmdToNine(getPosI(),&send,sizeof(send),dupIndex);
	}
	else
	{
		sendCmdToMe(&send,sizeof(send));
	}
}
bool SceneUser::changeMap(Scene *newscene,const zPos &pos,bool ignoreUserLevel)
{
#if 0
	if (!ignoreUserLevel &&(this->charbase.level <10) && (newscene != this->scene))
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"10级内无法离开清源村进行冒险!");
		return false;
	}
#endif
	if (189==scene->getRealMapID() && isRedNamed())
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你正在服刑期间,无法传送!");
		return false;
	}
	if (203==scene->getRealMapID() && charbase.punishTime)
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你正在服刑期间,无法传送!");
		return false;
	}

	if (newscene)
	{
		Zebra::logger->info("%s(%d)本服切换场景(%s-->%s),坐标(%d,%d):(%d,%d)",this->name,this->id,this->scene->name,newscene->name,this->getPos().x,this->getPos().y,pos.x,pos.y);
		if ((!newscene->checkUserLevel(this))&&(newscene!=this->scene)) return false;
	}
	//取消交易摆摊状态
//	tradeorder.cancel();
//	privatestore.step(PrivateStore::NONE,this);

//	tradeorder.cancel();
//	privatestore.step(PrivateStore::NONE,this);
	//先从目前场景删除


//	Scene *oldScene = this->scene;
	if (LeaveScene())
	{
	    //添加到新场景
	    if (intoScene(newscene,false,pos))
	    {
		return true;
	    }
	}
	return false;

}
#if 0
/**
* \brief 招唤宠物
* \param id 宠物ID
* \param type 宠物类型
* \param standTime 持续时间
* \param sid 脚本ID
* \param petName 宠物名称
* \param anpcid NPC的ID
* \return 如果该宠物存在,返回宠物对象的指针,否则为NULL
*/
ScenePet * SceneUser::summonPet(DWORD id,Cmd::petType type,DWORD standTime,DWORD sid,const char * petName,DWORD anpcid,zPos pos,BYTE vdir)
{
	if ((type <= Cmd::PET_TYPE_NOTPET)||(type > Cmd::PET_TYPE_TURRET))
	{
		Zebra::logger->info("SceneUser::summonPet(): %s 召唤未知类型的宠物 type=%d",name,type);
		return false;
	}

	if( !MirageSummon.empty() )
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"错误:幻影的效果不可以在召唤其他的NPC！");
		return false;
	}


	//国王的马
	if (king && type==Cmd::PET_TYPE_RIDE) id = KING_HORSE_ID;
	if (emperor && type==Cmd::PET_TYPE_RIDE) id = EMPEROR_HORSE_ID;
	zNpcB *base = npcbm.get(id);
	zNpcB *abase = NULL;
	if (anpcid>0) abase = npcbm.get(anpcid);
	if (NULL == base) return false;

	t_NpcDefine define;
	define.id = base->id;
	strncpy(define.name,base->name,MAX_NAMESIZE-1);
	if (pos.x != 0 && pos.y !=0)
		define.pos = pos;
	else
		define.pos = getPos();
	define.num = 1;
	define.interval = 5;
	define.initstate = zSceneEntry::SceneEntry_Normal;
	define.width = CALL_PET_REGION;
	define.height = CALL_PET_REGION;
	define.pos -= zPos(CALL_PET_REGION/2,CALL_PET_REGION/2);
	define.scriptID = sid;
	scene->initRegion(define.region,define.pos,define.width,define.height);

	ScenePet * newPet = NULL;
	if (type==Cmd::PET_TYPE_GUARDNPC)
		newPet = scene->summonOneNpc<GuardNpc>(define,pos,base,dupIndex,standTime,abase,vdir);
	else if (type==Cmd::PET_TYPE_CARTOON)
		newPet = scene->summonOneNpc<CartoonPet>(define,pos,base,dupIndex,standTime,abase,vdir);
	else
		newPet = scene->summonOneNpc<ScenePet>(define,pos,base,dupIndex,standTime,abase,vdir);

	if (newPet)
	{
		if (petName&&(0!=strncmp(petName,"",MAX_NAMESIZE)))
			strncpy(newPet->name,petName,MAX_NAMESIZE-1);

		newPet->setPetType(type);
		newPet->setMaster(this);
		SceneNpcManager::getMe().addSpecialNpc(newPet);
		//newPet->petData.type = type;

		using namespace Cmd;
		switch (type)
		{
		case PET_TYPE_RIDE:
			{
				if (ridepet)
				{
					//ridepet->toDie(tempid);
					killOnePet(ridepet);
				}
				ridepet = newPet;
			}
			break;
		case PET_TYPE_PET:
			{
				if (pet)
				{
					pet->toDie(tempid);
					killOnePet(pet);
				}
				if (summon)
				{
					summon->toDie(tempid);
					killOnePet(summon);
				}
				pet = newPet;
				pet->petData.state = Cmd::PET_STATE_NORMAL;
			}
			break;
		case PET_TYPE_SUMMON:
			{
				if (pet)
				{
					pet->toDie(tempid);
					killOnePet(pet);
				}
				if (summon)
				{
					summon->toDie(tempid);
					killOnePet(summon);
				}
				summon = newPet;
			}
			break;
		case PET_TYPE_GUARDNPC:
			{
				if (guard)
				{
					guard->toDie(tempid);
					killOnePet(guard);
				}
				guard = newPet;
			}
			break;
		case PET_TYPE_TOTEM:
			totems.push_back(newPet);
			break;
		case PET_TYPE_CARTOON:
			{
				if (cartoon)
				{
					cartoon->getCartoonData().state = Cmd::CARTOON_STATE_PUTAWAY;
					cartoon->save(Cmd::Session::SAVE_TYPE_PUTAWAY);
					cartoon->toDie(tempid);
					killOnePet(cartoon);
				}
				cartoon = (CartoonPet *)newPet;
			}
			break;
		default:
			Zebra::logger->info("summonPet(): 未知的宠物类型 %d",type);
			break;
		}

		this->skillStatusM.processPassiveness();  // 处理我的被动状态影响
		newPet->setDir(vdir);
		newPet->setPetAI( PETAI_MOVE_FOLLOW );
		newPet->sendData();
		newPet->sendPetDataToNine();

		// sky 如果被召唤的是炮塔就把他设置为无敌状态(炮塔是不允许被攻击的)
		if( newPet->getPetType() == Cmd::PET_TYPE_TURRET )
		{
			newPet->setPetAI( PETAI_ATK_ACTIVE );
			newPet->angelMode = true;
		}

		Zebra::logger->debug("[宠物]%s(%u) 增加宠物 %s(%u) 类型 %d",name,id,newPet->name,newPet->tempid,type);
	}
	else
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"太拥挤了,召唤失败-。-");

	return newPet;
}


/**
* \brief [sky]幻影专用招唤
* \param id 宠物ID
* \param type 宠物类型
* \param standTime 持续时间
* \param sid 脚本ID
* \param petName 宠物名称
* \param anpcid NPC的ID
* \return 无类型
*/
bool SceneUser::MirageSummonPet(DWORD id,Cmd::petType type,DWORD standTime,WORD num,const char * petName,DWORD anpcid,zPos pos,BYTE vdir)
{
	using namespace Cmd;

	if( num > 5 )
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"朋友！你想召唤出NPC海啊！数量我限制拉的！你自己看着办吧 -。-||");
		return false;
	}

	if( !MirageSummon.empty() )
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"错误:幻影的效果还没消失！不可以使用！");
		return false;
	}

	zNpcB *base = npcbm.get(id);
	zNpcB *abase = NULL;
	if (anpcid>0) abase = npcbm.get(anpcid);
	if (NULL == base) return false;

	t_NpcDefine define;
	define.id = base->id;
	strncpy(define.name,base->name,MAX_NAMESIZE-1);
	if (pos.x != 0 && pos.y !=0)
		define.pos = pos;
	else
		define.pos = getPos();
	define.num = 1;
	define.interval = 5;
	define.initstate = zSceneEntry::SceneEntry_Normal;
	define.width = CALL_PET_REGION;
	define.height = CALL_PET_REGION;
	define.pos -= zPos(CALL_PET_REGION/2,CALL_PET_REGION/2);
	define.scriptID = 0;
	scene->initRegion(define.region,define.pos,define.width,define.height);

	MirageSummon.resize( num );

	for( int i=0; i< num; i++ )
	{
		SceneGhost * newPet = NULL;

		newPet = scene->summonOneNpc<SceneGhost>(define,pos,base,dupIndex,standTime,abase,vdir,this);

		if (newPet)
		{
			if (petName&&(0!=strncmp(petName,"",MAX_NAMESIZE)))
				strncpy(newPet->name,petName,MAX_NAMESIZE-1);

			newPet->setPetType(type);
			newPet->setMaster(this);
			newPet->full_zNpcB( this );
			SceneNpcManager::getMe().addSpecialNpc(newPet);

			if (pet)
			{
				pet->toDie(tempid);
				killOnePet(pet);
			}

			if (summon)
			{
				summon->toDie(tempid);
				killOnePet(summon);
			}

			MirageSummon.push_back( newPet );

			this->skillStatusM.processPassiveness();  // 处理我的被动状态影响
			newPet->setDir(vdir);
			newPet->setPetAI( PETAI_MOVE_FOLLOW );
			newPet->setSpeedRate( 2.0 ); 
			newPet->sendData();
			newPet->sendPetDataToNine();
		}
		else
			Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"太拥挤了,召唤失败-。-");
	}

	return true;
}

#endif
/**
* \brief  招唤宠物或图腾
*
* \param id 宠物ID或图腾ID
* \param standTime  图腾持续时间
*
* \return 如果该宠物存在,返回宠物对象的指针,否则为NULL
SceneNpc * SceneUser::summonPet(DWORD id,DWORD standTime,DWORD anpcid)
{
zNpcB *base = npcbm.get(id);
zNpcB *abase = NULL;
if (anpcid>0) abase = npcbm.get(anpcid);
if (NULL == base) return false;

t_NpcDefine define;
zPos pos = getPos();
define.id = base->id;
strcpy(define.name,base->name);
define.pos = getPos();
define.num = 1;
define.interval = 30;
define.initstate = zSceneEntry::SceneEntry_Normal;
define.width = 2;
define.height = 2;
define.pos -= zPos(1,1);
scene->initRegion(define.region,define.pos,define.width,define.height);

SceneNpc * pet = scene->summonOneNpc<SceneNpc>(define,pos,base,standTime,abase);

if (pet)
totems.push_back(pet);
return pet;
}
*/

void SceneUser::removeNineEntry(zPosI posi)
{
	GetAllRemovePosNpc allNpcs(this);
	GetAllRemovePosUser allUsers(this);
	const zPosIVector &pv = scene->getNineScreen(posi);
	for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
	{
		scene->execAllOfScreen(zSceneEntry::SceneEntry_NPC,*it,allNpcs);
		scene->execAllOfScreen(zSceneEntry::SceneEntry_Player,*it,allUsers);
	}
	if (allNpcs.canSend())
	{
		sendCmdToMe(allNpcs.getSendBuffer(),allNpcs.getSize());
	}
	if (allUsers.canSend())
	{
		sendCmdToMe(allUsers.getSendBuffer(),allUsers.getSize());
	}
}

#if 0
//用户进入副本地图
bool SceneUser::userEnterDup(unsigned short _dupIndex,DWORD mapid,userDupMap *_userDupMap)
{

	unsigned short oldIndex = this->dupIndex;
	if(0 != _dupIndex)
	{
		//进入副本地图
		class Scene * _scene=SceneManager::getInstance().getSceneByID(mapid);
		if(_scene)
		{
			WayPoint *wp=_scene->getRandWayPoint();
			if (wp)
			{
				const zPos pos=wp->getRandPoint();
				bool suc = changeMap(_scene,pos,true);
				if(!suc)
					return false;
			}
		}
		this->dupIndex = _dupIndex;
		sendDupChangeCmdToGate();
		//通知别的用户删除自己
		zPos oldPos = getPos();
		zPosI oldPosI=getPosI();
		Cmd::stRemoveUserMapScreenUserCmd remove;
		remove.dwUserTempID=tempid;
		scene->clearBlock(oldPos);
		scene->sendCmdToNine(oldPosI,&remove,sizeof(remove),oldIndex);
		removeNineEntry(oldPosI);

		sendMeToNine();
		sendNineToMe();

		(_userDupMap->tempDups)[mapid] = _dupIndex;
		_userDupMap->_index = _dupIndex;
		_userDupMap->currentScene = _scene;



		return duplicateManager::getInstance().EnterDup(this,this->dupIndex);

	}
	return false;

}

//用户离开副本地图
void SceneUser::userLeaveDup()
{
	unsigned short oldIndex = this->dupIndex;
	this->dupIndex = 0;
	sendDupChangeCmdToGate();

	//通知用户删除自己
	zPos oldPos = getPos();
	zPosI oldPosI=getPosI();
	Cmd::stRemoveUserMapScreenUserCmd remove;
	remove.dwUserTempID=tempid;
	scene->clearBlock(oldPos);
	scene->sendCmdToNine(oldPosI,&remove,sizeof(remove),oldIndex);
	removeNineEntry(oldPosI);

	sendMeToNine();
	sendNineToMe();

	duplicateManager::getInstance().leaveDup(this,oldIndex);
}
#endif

/**
* \brief 使角色移动到新的坐标位置
*
* \param newPos 新坐标
*
* \return 移动成功返回TRUE,否则返回FALSE
*
*/
bool SceneUser::goTo(zPos &newPos)
{
	zPos findedPos;
	zPos oldPos = getPos();
	zPosI oldPosI=getPosI();
	bool founded = scene->findPosForUser(newPos,findedPos);
	if (scene->refresh(this,founded ? findedPos : newPos))
	{
		Cmd::stRemoveUserMapScreenUserCmd remove;
		remove.dwUserTempID=tempid;
		//scene->clearBlock(oldPos);
		Cmd::stUserInstantJumpMoveUserCmd goToPos;
		goToPos.x = getPos().x;
		goToPos.y = getPos().y;
		goToPos.mapID = scene->getRealMapID();
		goToPos.dwUserTempID = tempid;
		sendCmdToMe(&goToPos, sizeof(goToPos));

		//检查是否隐身
		if (SceneEntry_Hide!=getState() && !this->hideme && !Soulflag)
		{
			scene->sendCmdToNine(oldPosI,&remove,sizeof(remove),dupIndex);
			//scene->setBlock(getPos());
		}
		else
		{
			sendCmdToMe(&remove,sizeof(remove));
		}
		removeNineEntry(oldPosI);

		sendMeToNine();
		sendNineToMe();
		//校验9屏所有玩家以及Npc坐标
#if 0
		GetAllPos allNpcs(Cmd::MAPDATATYPE_NPC,this);
		GetAllPos allUsers(Cmd::MAPDATATYPE_USER,this);
		const zPosIVector &pv = scene->getNineScreen(getPosI());
		for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
		{
			scene->execAllOfScreen(zSceneEntry::SceneEntry_NPC,*it,allNpcs);
			scene->execAllOfScreen(zSceneEntry::SceneEntry_Player,*it,allUsers);
		}
		if (allNpcs.canSend())
		{
			sendCmdToMe(allNpcs.getSendBuffer(),allNpcs.getSize());
		}
		if (allUsers.canSend())
		{
			sendCmdToMe(allUsers.getSendBuffer(),allUsers.getSize());
		}
#endif
		return true;
	}
	return false;
}

/**
* \brief 在屏幕内定点移动
*/
void SceneUser::jumpTo(zPos &newPos)
{
	goTo(newPos);
}

/**
* \brief 使角色移动到一个区域的随机的一点上
*
* \param center 区域中心点
* \param rectx 区域X坐标
* \param recty 区域Y坐标
*
* \return 移动成功返回TRUE,否则返回FALSE
*
*/
bool SceneUser::goToRandomRect(zPos center,WORD rectx,WORD recty)
{
	if (center.x == 0 && center.y == 0)
	{
		center = getPos();
	}
	zPos randPos;
	int count=0;
	do {
		if (this->scene->randzPosOnRect(center,randPos,rectx,recty))
		{
			if (!scene->checkBlock(randPos,TILE_BLOCK) && (!scene->checkBlock(randPos,TILE_ENTRY_BLOCK)|| this->liquidState))
			{
				return goTo(randPos);
			}
			count++;
		}
	}while(count<10);
	return false;
}


void SceneUser::sendInitHPAndMp()
{
	Cmd::stSetHPAndMPDataUserCmd ret;
	ret.dwHP = charbase.hp;
	ret.dwMP = charbase.mp;
	//ret.dwSP = charbase.sp;
	sendCmdToMe(&ret,sizeof(ret));
}
/**
* \brief 填充t_MainUserData命令
*
* \param data 待填充的命令
*/
void SceneUser::full_t_MainUserData(Cmd::t_MainUserData &data) const
{
	bzero(&data,sizeof(data));
#ifndef _MOBILE
	data.dwUserTempID=tempid;
	data.level=charbase.level;
	data.hp=charbase.hp;
	data.maxhp=charstate.maxhp;
	data.resumehp=charstate.resumehp;
	data.mp=charbase.mp;
	data.maxmp=charstate.maxmp;
	data.resumemp=charstate.resumemp;
	data.sp=charbase.sp;
	data.maxsp=charstate.maxsp;
	data.resumesp=charstate.resumesp;
	data.pdamage=charstate.pdamage;
	data.maxpdamage=charstate.maxpdamage;
	data.mdamage=charstate.mdamage;
	data.maxmdamage=charstate.maxmdamage;
	data.pdefence=charstate.pdefence;
	data.mdefence=charstate.mdefence;
	data.exp=charbase.exp;
	data.nextexp=charstate.nextexp;
	data.attackrating=charstate.attackrating;
	data.attackdodge=charstate.attackdodge;
	data.charm=charstate.charm;
	data.skillPoint=charbase.skillpoint;          /// 技能点数
	data.points=charbase.points;
	data.country=charbase.country;
	data.pkmode=pkMode;
	data.bang=charstate.bang;

	data.wdProperty[0]=charstate.wdProperty[0];
	data.wdProperty[1]=charstate.wdProperty[1];
	data.wdProperty[2]=charstate.wdProperty[2];
	data.wdProperty[3]=charstate.wdProperty[3];
	data.wdProperty[4]=charstate.wdProperty[4];
#if 0
	data.stdpdamage    = charstate.stdpdamage;          /// 标准物理攻击力
	data.stdmdamage    = charstate.stdmdamage;          /// 标准法术攻击力
	data.stdpdefence  = charstate.stdpdefence;        /// 标准物理防御力
	data.stdmdefence  = charstate.stdmdefence;        /// 标准法术防御力
	data.stdbang    = charstate.stdbang;          /// 标准重击率
#endif
	data.wdStdProperty[0]=charbase.wdProperty[0];
	data.wdStdProperty[1]=charbase.wdProperty[1];
	data.wdStdProperty[2]=charbase.wdProperty[2];
	data.wdStdProperty[3]=charbase.wdProperty[3];
	data.wdStdProperty[4]=charbase.wdProperty[4];

	data.wdTire = this->wdTire;
//	data.fivetype = this->getFiveType();
//	data.fivepoint = this->getFiveLevel();
	data.honor = charbase.honor;
	data.maxhonor = charbase.maxhonor;
	data.gold = charbase.gold;
	data.ticket = charbase.ticket;
	data.bitmask = charbase.bitmask;
//	data.zs      = charbase.zs;      //转身次数
	//all the model
	data.model[0] = 2;//1 or 2
	data.model[1] = 2049;
	data.model[2] = 7937;
	data.model[3] = 210601;
	data.model[4] = 210601;
	data.model[5] = 210601;
	data.model[6] = 0;
	data.model[7] = 0;
	data.model[8] = 210501;
	data.model[9] = 210501;
	data.model[10] = 389052928;
	data.model[11] = 0;
	data.model[12] = 8071;
	data.model[13] = 0;
	data.model[14] = 0;
	data.model[15] = 210601;
	data.height = 100;
	data.weight = 100;
#else
	strncpy(data.name,name,MAX_NAMESIZE);
	data.gold = charbase.gold;
#endif
}

/**
* \brief  获取我的移动速度
* \return 移动速度 毫秒
*/
WORD SceneUser::getMyMoveSpeed() const
{
#if 0
	WORD curSpeed = charstate.movespeed;
	if (!horse.mount()||skillValue.movespeed<0)
	{
		if (charstate.movespeed > skillValue.movespeed)
		{
			curSpeed = charstate.movespeed-skillValue.movespeed;
		}
		else
		{
			curSpeed = 0;
		}
	}
	curSpeed = (WORD)(curSpeed*(1 + skillValue.array_dmvspeed/100.0f));
	if (curSpeed <100) curSpeed=100;
	if (this->assault) curSpeed = 640/4;
#ifdef _DEBUG
	//Channel::sendNine((SceneUser *)this,"移动速度 %u",curSpeed);
#endif
	return curSpeed;
#endif
	return 500;
}

/**
* \brief 填充t_UserData命令
*
* \param data 待填充的命令
*/
void SceneUser::full_t_UserData(Cmd::t_UserData &data)
{
	data.dwUserTempID=tempid;

	data.type=charbase.type;
	//data.face=charbase.face;
	data.goodness=charbase.goodness;

	//if (!mask.is_masking() )
	{
		strncpy(data.name,name,MAX_NAMESIZE);
	//	data.country=charbase.country;
		//live_skills.titles(data.live_skills,sizeof(data.live_skills));
	}
	//else
	{
	//	strncpy(data.name,"蒙面人",MAX_NAMESIZE);
	}

	//data.sculpt.dwHorseID=horse.horse();
//	if (king) data.sculpt.dwHorseID = KING_HORSE_ID;
//	if (emperor) data.sculpt.dwHorseID = EMPEROR_HORSE_ID;
	/*
	//Zebra::logger->debug("马牌:%d",data.sculpt.dwHorseID);
	if (packs.equip.body)
	data.sculpt.dwBodyID=packs.equip.body->base->id;
	else
	data.sculpt.dwBodyID=0;
	// */
#if 0
	if (packs.equip.equip(EquipPack::HANDL))
	{
		data.sculpt.dwLeftHandID=packs.equip.equip(EquipPack::HANDL)->base->id;
		data.dwLeftWeaponColor=packs.equip.equip(EquipPack::HANDL)->data.color;
	}
	else
	{
		data.sculpt.dwLeftHandID=0;
		data.dwLeftWeaponColor=0;
	}
	if (packs.equip.equip(EquipPack::HANDR))
	{
		data.sculpt.dwRightHandID=packs.equip.equip(EquipPack::HANDR)->base->id;
		data.dwRightWeaponColor=packs.equip.equip(EquipPack::HANDR)->data.color;
	}
	else
	{
		data.sculpt.dwRightHandID=0;
		data.dwRightWeaponColor=0;
	}
	data.sculpt.dwHairID=getHairType();
	data.dwHairRGB=getHairColor();
	//如果有装备Custom颜色取物品颜色,System取道具表中颜色,否则Custom取人物属性随机后的color,系统色取0
	if (packs.equip.equip(EquipPack::OTHERS2)&& (packs.equip.equip(EquipPack::OTHERS2)->base->kind == ItemType_FashionBody || packs.equip.equip(EquipPack::OTHERS2)->base->kind == ItemType_HighFashionBody) )
	{
		data.sculpt.dwBodyID=packs.equip.equip(EquipPack::OTHERS2)->base->id;
		data.dwBodyColorCustom = packs.equip.equip(EquipPack::OTHERS2)->data.color;
	}
	else if (packs.equip.equip(EquipPack::OTHERS3)&& (packs.equip.equip(EquipPack::OTHERS3)->base->kind == ItemType_FashionBody || packs.equip.equip(EquipPack::OTHERS3)->base->kind == ItemType_HighFashionBody) )
	{
		data.sculpt.dwBodyID=packs.equip.equip(EquipPack::OTHERS3)->base->id;
		data.dwBodyColorCustom = packs.equip.equip(EquipPack::OTHERS3)->data.color;
	}
	else if (packs.equip.equip(EquipPack::BODY)) 
	{
		data.sculpt.dwBodyID=packs.equip.equip(EquipPack::BODY)->base->id;
		data.dwBodyColorCustom = packs.equip.equip(EquipPack::BODY)->data.color;
	}
	else 
	{
		data.dwBodyColorCustom = charbase.bodyColor;
		data.sculpt.dwBodyID=0;
	}
#endif
//	data.attackspeed=(WORD)((((float)charstate.attackspeed)/640.0f)*100.0f);
	//data.movespeed=  charstate.movespeed-skillValue.movespeed;
	//if (this->assault) data.movespeed = 640/4;
//	data.movespeed = getMyMoveSpeed();
	//bcopy(byState,data.state,sizeof(byState));
//	data.dwChangeFaceID = this->dwChangeFaceID;
//	data.level = this->charbase.level;
	/*
	if (this->charbase.level<10)
	{
	data.level = 1;
	}
	else if (this->charbase.level>=10&&this->charbase.level<20)
	{
	data.level = 11;
	}
	else if (this->charbase.level>=20&&this->charbase.level<TEAM_HONOR_MEMBER_LEVEL)
	{
	data.level = 31; // 荣誉之星按是否等于21来判断是否不可带人
	}
	else
	{
	data.level = 21;
	}
	// */
//	data.useJob = this->charbase.useJob;   //sky 填充角色职业
//	data.exploit = this->charbase.exploit;
//	data.dwArmyState = this->dwArmyState;

	//strncpy(data.unionName,this->unionName,sizeof(data.unionName));
	//strncpy(data.septName,this->septName,sizeof(data.septName));
	data.dwUnionID = this->charbase.unionid;
	data.dwSeptID = this->charbase.septid;
#if 0
	if (this->king && !this->emperor)
	{
		strncpy(data.caption,(this->charbase.type==1)?"国王":"女王",sizeof(data.caption));
	}
	else if (this->emperor)
	{
		strncpy(data.caption,(this->charbase.type==1)?"皇帝":"女皇",sizeof(data.caption));
	}
	else if (this->kingConsort == 1)
	{
		strncpy(data.caption,(this->charbase.type==1)?"王夫":"王后",sizeof(data.caption));
	}
	else if (this->kingConsort == 2)
	{
		strncpy(data.caption,(this->charbase.type==1)?"皇夫":"皇后",sizeof(data.caption));
	}
	else
	{
		strncpy(data.caption,this->caption,sizeof(data.caption));
	}
#endif
#if 0
	TeamManager * team = SceneManager::getInstance().GetMapTeam(TeamThisID);
	if(team)
	{
		if (team->getLeader() == this->tempid)
		{
			data.dwTeamState=Cmd::TEAM_STATE_LEADER;
		}
		else
		{
			data.dwTeamState=Cmd::TEAD_STATE_MEMBER;
		}
	}
	else
	{
		data.dwTeamState=Cmd::TEAD_STATE_NONE;
	}
#endif
	data.model[0] = 2;//1 or 2
	data.model[1] = 2049;
	data.model[2] = 7937;
	data.model[3] = 210601;
	data.model[4] = 210601;
	data.model[5] = 210601;
	data.model[6] = 0;
	data.model[7] = 0;
	data.model[8] = 210501;
	data.model[9] = 210501;
	data.model[10] = 389052928;
	data.model[11] = 0;
	data.model[12] = 8071;
	data.model[13] = 0;
	data.model[14] = 0;
	data.model[15] = 210601;
	data.height = 100;
	data.weight = 100;
}
/**
* \brief 填充t_MapUserData命令
*
* \param data 待填充的命令
*/
void SceneUser::full_t_MapUserData(Cmd::t_MapUserData &data)
{
	bzero(&data,sizeof(data));
	full_t_UserData(*((Cmd::t_UserData *)&data));
	full_all_UState(data.state);
}
/**
* \brief 填充t_MapUserDataPos命令
*
* \param data 待填充的命令
*/
void SceneUser::full_t_MapUserDataPos(Cmd::t_MapUserDataPos &data)
{
	full_t_MapUserData(*((Cmd::t_MapUserData *)&data));
	data.byDir=getDir();
	data.x=getPos().x;
	data.y=getPos().y;
}
/**
* \brief 填充t_MapUserDataState命令
*
* \param data 待填充的命令
*/
void SceneUser::full_t_MapUserDataState(Cmd::t_MapUserDataState &data)
{
	bzero(&data,sizeof(data));
	full_t_UserData(*((Cmd::t_UserData*) &data));
	data.num=full_UState(data.state);
	data.state[data.num] = 0xFFFF;
	++data.num;
}
/**
* \brief 填充t_MapUserDataPos命令
*
* \param data 待填充的命令
*/
void SceneUser::full_t_MapUserDataPosState(Cmd::t_MapUserDataPosState &data)
{
	full_t_UserData(*((Cmd::t_UserData *)&data));
	data.byDir=getDir();
	data.x=getPos().x;
	data.y=getPos().y;
#if 0
	data.num=full_UState(data.state);
	data.state[data.num] = 0xFFFF;
	++data.num;
#endif
	data.num = 0;
}

#if 0
/**
* \brief 
*
*/
bool SceneUser::checkGoodnessTime(const zRTime &ct)
{
	if (ct >= pkState.lastCheckGoodness)
	{
		pkState.lastCheckGoodness.addDelay(ScenePkState::protectPeriod);
		return true;
	}
	else
	{
		return false;
	}
}


/**
* \brief 重新计算箭侠箭灵的攻击模式
*
* \param calcflag 为TRUE通知客户端改变,为FALSE则不进行通知
*
* \return 如果模式改变返回TRUE,否则返回FALSE
*
*/
/*
bool SceneUser::recalcBySword(bool calcflag)
{
bool needRecalc = false;
switch(charbase.type)
{
case PROFESSION_3://箭侠
{
if (packs.uom.exist(BOW_ARROW_ITEM_TYPE,1) != farAttack)  // 没有箭的时候攻击模式由远程变近程,有箭的时候变化过程相反
{
needRecalc = true;
}
}
break;
case PROFESSION_4://箭灵
{
if (packs.uom.exist(661,1) != farAttack)  // 没有箭的时候攻击模式由远程变近程,有箭的时候变化过程相反
{
needRecalc = true;
}
}
break;
default:
break;
}
if (needRecalc && calcflag)
{
setupCharBase();
Cmd::stMainUserDataUserCmd  userinfo;
full_t_MainUserData(userinfo.data);
sendCmdToMe(&userinfo,sizeof(userinfo));
// sendMeToNine(); 计算自己的攻击属性变化没必要广播九屏
}
return needRecalc;
}
*/
#endif

/**
* \brief 增加道具数量
*
* \param id: 道具ID
* \param num: 要增加的道具数量
* \param upgrade: 升级
* \param notify: 是否通知玩家
* \param obj: 返回的物品
*
* \return 返回增加的数量
*
*/
int SceneUser::addObjectNum(DWORD id,DWORD num,BYTE upgrade,int notify,bool bindit)
{
	zObject *orig_ob = NULL;
	UserObjectM::Obj_vec new_obs;

	int result = packs.uom.addObjectNum(this,id,num,orig_ob,new_obs,upgrade,bindit);
	if (result >= 0) {    
		zObject* ob = orig_ob;
		if (orig_ob) {
			Cmd::stRefCountObjectPropertyUserCmd ret;
			ret.qwThisID = orig_ob->data.qwThisID;
			ret.dwNum = orig_ob->data.dwNum;
			sendCmdToMe(&ret,sizeof(ret));
		}
		UserObjectM::Obj_vec::iterator it = new_obs.begin();
		while (it != new_obs.end()) {
			ob = *it;
#ifndef _MOBILE
			Cmd::stAddObjectPropertyUserCmd ret;
			memcpy(&ret.object,&(*it)->data,sizeof(t_Object));
			sendCmdToMe(&ret,sizeof(ret));
#else
			Cmd::stAddMobileObjectPropertyUserCmd ret;
			ob->fullMobileObject(ret.object);
			sendCmdToMe(&ret,sizeof(ret));
#endif
			++it;
		}

		if (ob) {
			OnGet event(ob->data.dwObjectID);
			EventTable::instance().execute(*this,event);
#if 0
			if (ScriptQuest::get_instance().has(ScriptQuest::OBJ_GET,ob->data.dwObjectID)) { 
				char func_name[32];
				sprintf(func_name,"%s_%d","get",ob->data.dwObjectID);
				execute_script_event(this,func_name,ob);
			}
#endif
			if (bindit) ob->data.bind=1;
		}

		if (notify && ob)
			Channel::sendSys(this,notify,"得到物品%s%d个",ob->data.strName,num);  

	}

	return result;
}
#if 0
/**
* \brief 增加绿色绑定道具数量
*
* \param id: 道具ID
* \param num: 要增加的道具数量
* \param upgrade: 升级
* \param notify: 是否通知玩家
* \param obj: 返回的物品
*
* \return 返回增加的数量
*
*/
int SceneUser::addGreenObjectNum(DWORD id,DWORD num,BYTE upgrade,int notify,bool bindit)
{
	zObject *orig_ob = NULL;
	UserObjectM::Obj_vec new_obs;

	int result = packs.uom.addGreenObjectNum(this,id,num,orig_ob,new_obs,upgrade);
	if (result >= 0) {    
		zObject* ob = orig_ob;
		if (orig_ob) {
			Cmd::stRefCountObjectPropertyUserCmd ret;
			ret.qwThisID = orig_ob->data.qwThisID;
			ret.dwNum = orig_ob->data.dwNum;
			sendCmdToMe(&ret,sizeof(ret));
		}
		UserObjectM::Obj_vec::iterator it = new_obs.begin();
		while (it != new_obs.end()) {
			ob = *it;
			Cmd::stAddObjectPropertyUserCmd ret;
			memcpy(&ret.object,&(*it)->data,sizeof(t_Object),sizeof(ret.object));
			sendCmdToMe(&ret,sizeof(ret));
			++it;
		}

		if (ob) {
			OnGet event(ob->data.dwObjectID);
			EventTable::instance().execute(*this,event);
			if (ScriptQuest::get_instance().has(ScriptQuest::OBJ_GET,ob->data.dwObjectID)) { 
				char func_name[32];
				sprintf(func_name,"%s_%d","get",ob->data.dwObjectID);
				execute_script_event(this,func_name,ob);
			}
			if (bindit) ob->data.bind=1;
		}

		if (notify && ob)
			Channel::sendSys(this,notify,"得到物品%s%d个",ob->data.strName,num);  

	}

	return result;
}
#endif

/**
* \brief 减少道具数量
*
* \param id: 道具ID
* \param num: 要减少的道具数量
* \param upgrade: 升级
* \param notify: 是否通知玩家
*
* \return 返回减少的数量
*
*/
int SceneUser::reduceObjectNum(DWORD id,DWORD num,BYTE upgrade)
{
	zObject *update_ob = NULL;
	UserObjectM::ObjID_vec del_obs;

	int result = packs.uom.reduceObjectNum(this,id,num,update_ob,del_obs,upgrade);
	if (update_ob) {
		Cmd::stRefCountObjectPropertyUserCmd ret;
		ret.qwThisID = update_ob->data.qwThisID;
		ret.dwNum = update_ob->data.dwNum;
		sendCmdToMe(&ret,sizeof(ret));
	}
	UserObjectM::ObjID_vec::iterator it = del_obs.begin();
	while (it != del_obs.end()) {
		Cmd::stRemoveObjectPropertyUserCmd rm;
		rm.qwThisID = *it;
		sendCmdToMe(&rm,sizeof(rm));
		++it;
	}

	return result;
}
/**
* \brief 计算友好度
*
*
*/
void SceneUser::countFriendDegree()
{
	//sky 队伍已经没有友好度这种概念拉！！
	//if (scene->getZoneType(pos) == ZoneTypeDef::ZONE_NONE) team.countFriendDegree();
}

/**
* \brief 将命令转发到会话服务器
*
* \param pNullCmd 待转发的命令
* \param nCmdLen 命令长度
*/
bool SceneUser::forwardSession(const Cmd::stNullUserCmd *pNullCmd,const DWORD nCmdLen)
{
	if (nCmdLen > zSocket::MAX_USERDATASIZE)
	{
		Zebra::logger->debug("消息越界(%d,%d)",pNullCmd->byCmd,pNullCmd->byParam);
	}
	BYTE buf[zSocket::MAX_DATASIZE];
	Cmd::Session::t_Session_ForwardUser *sendCmd=(Cmd::Session::t_Session_ForwardUser *)buf;
	constructInPlace(sendCmd);
	sendCmd->dwID=id;
	sendCmd->size=nCmdLen;
	bcopy(pNullCmd,sendCmd->data,nCmdLen);
	return sessionClient->sendCmd(buf,sizeof(Cmd::Session::t_Session_ForwardUser)+nCmdLen);
}

/**
* \brief 得到角色的魔法攻击类型(前4种职业属物理技能,后四种属于魔法技能)
* 物理技能就是使用对方的物理防御,魔法技能就是指使用对方的法术防御
* \return 物理技能为true 还是法术技能false
*/
bool SceneUser::getMagicType()
{
	switch(charbase.type)
	{
	case PROFESSION_1:
	case PROFESSION_2:
	case PROFESSION_3:
	case PROFESSION_4:
		{
			return true;
		}
		break;
	case PROFESSION_5:
	case PROFESSION_6:
	case PROFESSION_7:
	case PROFESSION_8:
		{
			return false;
		}
		break;
	}
	return false;
}

bool SceneUser::isAllied(SceneUser *pUser)
{
	//TODO 具体完成
	return this->charbase.country == pUser->charbase.country;
}

/**
* \brief 判断是否是敌人
* \return 1 是 0 不是
*/
int SceneUser::isEnemy(SceneEntryPk * entry,bool notify,bool good)
{
#if 0
	// TODO 判断传入角色与本身是否为朋友关系
	if (this==entry) return 0;

	using namespace Cmd;
	//if (PKMODE_ENTIRE==pkMode) return 1;

	//sky 如果用户存在阵营状态并且在战场中
	if(BattCampID != 0 && this->scene->IsGangScene())
	{
		if(entry)
		{
			//sky 如果对方的阵营ID和自己是一样的
			if(entry->BattCampID == BattCampID)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"不可以攻击同阵营的角色!");
				return 0;
			}
		}
	}

	switch (entry->getType())
	{
	case zSceneEntry::SceneEntry_Player:
		{
			if (this->isDiplomatState() == 0) return 0;
			SceneUser *pUser = (SceneUser *)entry;
			bool def_gem = false;
			bool my_gem = false;

			if (this->issetUState(Cmd::USTATE_TOGETHER_WITH_TIGER)
				|| this->issetUState(Cmd::USTATE_TOGETHER_WITH_DRAGON))
			{
				my_gem = true;
			}

			if (pUser->issetUState(Cmd::USTATE_TOGETHER_WITH_TIGER)
				|| pUser->issetUState(Cmd::USTATE_TOGETHER_WITH_DRAGON))
			{
				def_gem = true;
			}

			if (pUser == this) return 0;

			//sky 检测当前地图是否是新手保护地图
			if(this->scene->isNovice())
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"该地图是新手保护地图,不可以恶意PK玩家!");
				return 0;
			}

			if (((pUser->charbase.level<20) 
				&& (!pUser->isWarRecord(Cmd::COUNTRY_FORMAL_DARE,this->charbase.country)))
				&& !def_gem
				)
			{
				//if (notify) Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"不能攻击低于20级的玩家!");
				return 0;
			}

			if (((this->charbase.level<20) 
				&& (!this->isWarRecord(Cmd::COUNTRY_FORMAL_DARE,pUser->charbase.country)))
				&& !my_gem
				)
			{
				//if (notify) Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"你等级低于20级不能进行PK!");
				return 0;
			}

			//sky 无敌的你都敢攻击啊！我怕你死的很惨^_^
			if( pUser->angelMode)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"目标是无敌的！",this->scene->getPkLevel());
				return 0;
			}

			if (!(my_gem || def_gem))
			{
				//if (this->charbase.country == pUser->charbase.country)
				//{
				if (this->charbase.level <= this->scene->getPkLevel() && pUser->charbase.level >this->scene->getPkLevel() 
					&& (!this->isWarRecord(Cmd::SEPT_NPC_DARE,pUser->charbase.septid)))
				{
					if (notify) Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"你不能攻击%d以上的玩家",this->scene->getPkLevel());
					return 0;
				}

				if (this->charbase.level > this->scene->getPkLevel() && pUser->charbase.level <= this->scene->getPkLevel() 
					&& (!this->isWarRecord(Cmd::SEPT_NPC_DARE,pUser->charbase.septid)))
				{
					if (notify) Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"你不能攻击%d级以下的玩家",this->scene->getPkLevel());
					return 0;
				}
			}

			switch(pkMode)
			{
			case PKMODE_NORMAL:
				{
					return 0;
				}
				break;
			case PKMODE_TEAM:
				{
					//是同一组队或者是增益类魔法
					if ((TeamThisID != 0) && (pUser->TeamThisID == this->TeamThisID))
						return 0;
				}
				break;
			case PKMODE_TONG:
				{
					if (charbase.unionid != 0 && charbase.unionid == pUser->charbase.unionid)
					{
						return 0;
					}
				}
				break;
			case PKMODE_SEPT:
				{
					if (charbase.septid != 0 && charbase.septid == pUser->charbase.septid)
					{
						return 0;
					}
				}
				break;
				/*case PKMODE_SCHOOL:
				{
				if (charbase.schoolid != 0 && charbase.schoolid == pUser->charbase.schoolid)
				{
				return 0;
				}
				}
				break;*/
			case PKMODE_COUNTRY:
				{
					if (charbase.country != 0 
						&& ((charbase.country == pUser->charbase.country) ||
						(CountryAllyM::getMe().getFriendLevel(charbase.country,pUser->charbase.country)>0
						&& (pUser->isSpecWar(Cmd::COUNTRY_FORMAL_DARE) || this->isSpecWar(Cmd::COUNTRY_FORMAL_DARE)))))
					{
						return 0;
					}
				}
				break;
			case PKMODE_GOODNESS:
				{
					if (!pUser->isRedNamed(false)&&pUser->charbase.country==this->charbase.country)
					{
						return 0;
					}
				}
				break;
			case PKMODE_ALLY:
				{
					if ((CountryAllyM::getMe().getFriendLevel(pUser->charbase.country,this->charbase.country)>0) ||
						(pUser->charbase.country == this->charbase.country))
					{
						return 0;
					}
				}
				break;
				//case PKMODE_CHALLENGE:
			case PKMODE_ENTIRE:
			default:
				break;
			}
			return 1;
		}
		break;
	case zSceneEntry::SceneEntry_NPC:
		{
			SceneNpc * n = (SceneNpc *)entry;
			SceneEntryPk * m = n->getMaster();

			if (n->id==COUNTRY_MAIN_FLAG  //这几个不在这里判断
				|| n->id==COUNTRY_SEC_FLAG
				|| n->isMainGeneral()
				|| n->id==COUNTRY_KING_MAIN_FLAG
				|| n->id==COUNTRY_KING_SEC_FLAG
				|| n->id==COUNTRY_SEC_GEN
				|| n->id==COUNTRY_EMPEROR_MAIN_GEN
				|| n->id==COUNTRY_EMPEROR_SEC_GEN)
			{
				if (this->isDiplomatState() == 0)
					return 0;
				return 1;
			}

			if (!n->isBugbear())
			{
				if (n->npc->flags==1 && charbase.country!=n->scene->getCountryID())
					return 1;
				else
					return 0;
			}
			if (m)
			{
				if (m->getType() == zSceneEntry::SceneEntry_Player)
				{
					SceneUser *pUser = (SceneUser *)m;
					if (pUser)
					{
						bool def_gem = false;
						bool my_gem = false;

						if (this->issetUState(Cmd::USTATE_TOGETHER_WITH_TIGER)
							|| this->issetUState(Cmd::USTATE_TOGETHER_WITH_DRAGON))
						{
							my_gem = true;
						}

						if (pUser->issetUState(Cmd::USTATE_TOGETHER_WITH_TIGER)
							|| pUser->issetUState(Cmd::USTATE_TOGETHER_WITH_DRAGON))
						{
							def_gem = true;
						}

						if (pUser == this) return 0;
						if (((pUser->charbase.level<20)  && 
							(!pUser->isWarRecord(Cmd::COUNTRY_FORMAL_DARE,this->charbase.country)))
							&& !def_gem)
						{
							//if (notify) Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"不能攻击低于20级的玩家!");
							return 0;
						}
						if ((this->charbase.level<20)  
							&& (!this->isWarRecord(Cmd::COUNTRY_FORMAL_DARE,pUser->charbase.country))
							&& !my_gem)
						{
							//if (notify) Channel::sendSys(this,Cmd::INFO_TYPE_SYS,"你等级低于10级不能进行PK!");
							return 0;
						}
						if (this->scene == pUser->scene && !(my_gem || def_gem))
						{
							//if (this->charbase.country == pUser->charbase.country)
							//{
							if (this->charbase.level <= this->scene->getPkLevel() && pUser->charbase.level >this->scene->getPkLevel())
							{
								return 0;
							}
							if (this->charbase.level > this->scene->getPkLevel() && pUser->charbase.level <= this->scene->getPkLevel())
							{
								return 0;
							}
						}
					}
				}

				if (n->getPetType()==Cmd::PET_TYPE_GUARDNPC
					&& !scene->zPosShortRange(n->getPos(),m->getPos(),20)
					&& pkMode==PKMODE_ENTIRE)
					return 1;
				else
				{
					if (good && this == m && n->getPetType() == Cmd::PET_TYPE_GUARDNPC) return 1;
					return isEnemy(m);
				}
			}

			if ((n->aif&AIF_ATK_REDNAME)||(n->npc->kind==NPC_TYPE_GUARD))
			{
				if (isRedNamed()) return 1;
				if (charbase.country!=scene->getCountryID())
					return 1;
				if (charbase.goodness&Cmd::GOODNESS_ATT)
					return 1;
			}
			switch (n->npc->kind)
			{
			case NPC_TYPE_HUMAN:                    ///人型
			case NPC_TYPE_NORMAL:                   /// 普通类型
			case NPC_TYPE_BBOSS:                    /// 大Boss类型
			case NPC_TYPE_LBOSS:                    /// 小Boss类型
			case NPC_TYPE_PBOSS:                    /// 紫Boss类型
			case NPC_TYPE_BACKBONE:                 /// 精英类型
			case NPC_TYPE_GOLD:                             /// 黄金类型
			case NPC_TYPE_SUMMONS:                  /// 召唤类型
			case NPC_TYPE_AGGRANDIZEMENT:   /// 强化类型
			case NPC_TYPE_ABERRANCE:                /// 变异类型
			case NPC_TYPE_BACKBONEBUG:              /// 精怪类型
			case NPC_TYPE_PET:      /// 宠物类型
			case NPC_TYPE_TOTEM:                    /// 图腾类型
			case NPC_TYPE_DUCKHIT:    /// 花草
			case NPC_TYPE_RESOURCE:  /// 资源类NPC
			case NPC_TYPE_GHOST:     /// 元神类
			case NPC_TYPE_TURRET:			/// 炮塔
			case NPC_TYPE_BARRACKS:
			case NPC_TYPE_CAMP:
			case NPC_TYPE_ANIMON:  // 动物类
				return 1;
			case NPC_TYPE_GUARD:    /// 士兵类型
			case NPC_TYPE_SOLDIER:    /// 士兵类型
				{
					if (charbase.country!=scene->getCountryID())
						return 1;
					if (pkMode==PKMODE_ENTIRE)
						return 1;
					return 0;
				}
			case NPC_TYPE_UNIONGUARD:
				if (isAtt(Cmd::UNION_CITY_DARE))
					return 1;
				else
					if (scene->getUnionDare() && !isSpecWar(Cmd::UNION_CITY_DARE)
						&& !n->isMainGeneral())//大将军第三方不能打
						return 1;//中立方
					else
						return 0;//城战期间打城战而且不是攻方,就是守方
				break;
			case NPC_TYPE_UNIONATTACKER:
				if (isAtt(Cmd::UNION_CITY_DARE))
					return 0;
				else
					if (scene->getUnionDare() && !isSpecWar(Cmd::UNION_CITY_DARE))
						return 1;//中立方
					else
						return 1;//城战期间打城战而且不是攻方,就是守方
				break;
			default:
				return 0;
			}
		}
		break;
	default:
		return 0;
		break;
	}
#endif
	return 0;
}

/**
* \brief 修改HP
*
*/
void SceneUser::changeHP(const SDWORD &hp)
{
	SDWORD changeValue = 0;
	if (((int)charbase.hp)+(int)hp>=0)
	{
		changeValue = charbase.hp;
		charbase.hp += hp;
		if (charbase.hp > charstate.maxhp) charbase.hp = charstate.maxhp;
		changeValue = (int)charbase.hp-changeValue;
	}
	else
	{
		changeValue = charbase.hp;
		charbase.hp=0;
	}
	notifyHMS = true;

	if (changeValue !=0)
	{
		Cmd::stObjectHpMpPopUserCmd ret;
		ret.dwUserTempID = this->tempid;
		ret.byTarget = Cmd::MAPDATATYPE_USER;
		ret.vChange = (int)changeValue;
		ret.type = Cmd::POP_HP;
		this->scene->sendCmdToNine(getPosI(),&ret,sizeof(ret),dupIndex);
	}
}

/**
* \brief 直接伤害
*
*/
SWORD SceneUser::directDamage(SceneEntryPk *pAtt,const SDWORD &hp,bool notify)
{
	SDWORD attHp = 0;

	attHp = SceneEntryPk::directDamage(pAtt,hp,notify);

	SDWORD reduceHP;
	if ((SDWORD)charbase.hp - attHp>=0)
	{
		charbase.hp -= attHp;
		reduceHP = attHp;
	}
	else
	{
		reduceHP = charbase.hp;
		charbase.hp = 0;
	}

	if (reduceHP !=0 && notify)
	{
		Cmd::stObjectHpMpPopUserCmd ret;
		ret.dwUserTempID = this->tempid;
		ret.byTarget = Cmd::MAPDATATYPE_USER;
		ret.vChange = 0 - (int)reduceHP;
		ret.type = Cmd::POP_HP;
		this->scene->sendCmdToNine(getPosI(),&ret,sizeof(ret),dupIndex);
	}

	notifyHMS = true;
	return reduceHP;
}

/**
* \brief 修改MP
*
*/
void SceneUser::changeMP(const SDWORD &mp)
{
	SDWORD changeValue = 0;
	if (((int)charbase.mp)+(int)mp>=0)
	{
		changeValue = charbase.mp;
		charbase.mp += mp;
		if (charbase.mp > charstate.maxmp) charbase.mp = charstate.maxmp;
		changeValue = (int)charbase.mp-changeValue;
	}
	else
	{
		changeValue = charbase.mp;
		charbase.mp=0;
	}
	notifyHMS = true;

	//检查是否有自动补魔道具
	//checkAutoMP();

	if (changeValue !=0)
	{
		Cmd::stObjectHpMpPopUserCmd ret;
		ret.dwUserTempID = this->tempid;
		ret.byTarget = Cmd::MAPDATATYPE_USER;
		ret.vChange = (int)changeValue;
		ret.type = Cmd::POP_MP;
		this->scene->sendCmdToNine(getPosI(),&ret,sizeof(ret),dupIndex);
	}
}

/**
* \brief 修改SP
*
*/
void SceneUser::changeSP(const SDWORD &sp)
{
	if (charbase.sp+sp>=0)
	{
		charbase.sp += sp;
		if (charbase.sp > charstate.maxsp) charbase.sp = charstate.maxsp;
	}
	else
	{
		charbase.sp=0;
	}
	notifyHMS = true;
}

/**
* \brief 获得最大的hp
* \return 返回最大值
*/
DWORD SceneUser::getMaxHP()
{
	return charstate.maxhp;
}

/**
* \brief 获得最大的hp
* \return 返回最大值
*/
DWORD SceneUser::getBaseMaxHP()
{
	return charstate.maxhp;
}

/**
* \brief 获得最大的mp
* \return 返回最大值
*/
DWORD SceneUser::getMaxMP()
{
	return charstate.maxmp;
}

/**
* \brief 获得最大的mp
* \return 返回最大值
*/
DWORD SceneUser::getBaseMaxMP()
{
	return charstate.maxmp;
}

/**
* \brief 一次攻击的预处理
* //sky 加入战斗状态的设置处理
*/
bool SceneUser::preAttackMe(SceneEntryPk *pUser,const Cmd::stAttackMagicUserCmd *rev,bool physics,const bool good)
{
#if 0
	if (this->issetUState(Cmd::USTATE_PRIVATE_STORE))
	{
		ScenePk::attackFailToMe(rev,this);
		return false;
	}

	if ((pUser!=this)&&isSitdown())
	{
		standup();
	}

	if (pUser->getType() == zSceneEntry::SceneEntry_Player) //sky 如果攻击方是用户
	{
		std::vector<DWORD>::iterator iter;
		for(iter=UseableMagicList.begin(); iter!=UseableMagicList.end(); iter++)
		{
			if( rev->wdMagicType == *iter ) //sky 判断攻击方用的魔法是不是有益魔法
			{
				useFightState state = IsPveOrPvp();
				if(state != USE_FIGHT_NULL)
				{
					//sky 把攻击方设置为防守方一样的战斗模式
					if(state == USE_FIGHT_PVE)
					{
						//sky 如果防守方是PVE 还需要确定下攻击方不是PVP模式才可以设置
						if(((SceneUser*)pUser)->IsPveOrPvp() != USE_FIGHT_PVP && ((SceneUser*)pUser)->IsPveOrPvp() != USE_FIGHT_PVE)
						{
							((SceneUser*)pUser)->SetPveOrPvp(state);
							((SceneUser*)pUser)->sendMessageToMe("你已进入pve模式");
						}
					}
					else if(state == USE_FIGHT_PVP)
					{
						if(((SceneUser*)pUser)->IsPveOrPvp() != USE_FIGHT_PVP)
						{
							((SceneUser*)pUser)->SetPveOrPvp(state);
							((SceneUser*)pUser)->sendMessageToMe("你已进入pvp模式");
						}
					}

				}
				break;
			}
		}

		if(iter == UseableMagicList.end())
		{
			if(IsPveOrPvp() == USE_FIGHT_NULL || IsPveOrPvp() == USE_FIGHT_PVE)
			{
				SetPveOrPvp(USE_FIGHT_PVP);  //sky 对手如果是角色,设置自己的状态为pvp模式
				Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你已进入pvp模式");
			}

			if(((SceneUser*)pUser)->IsPveOrPvp() == USE_FIGHT_NULL || ((SceneUser*)pUser)->IsPveOrPvp() == USE_FIGHT_PVE)
			{
				((SceneUser*)pUser)->SetPveOrPvp(USE_FIGHT_PVP); //sky 并且把对方也设置成pvp模式
				((SceneUser*)pUser)->sendMessageToMe("你已进入pvp模式");
			}	
		}

		((SceneUser*)pUser)->SetPkTime();
	}
	else if (pUser->getType() == zSceneEntry::SceneEntry_NPC) //sky 如果攻对手是NPC
	{
		if(IsPveOrPvp() != USE_FIGHT_PVP && IsPveOrPvp() != USE_FIGHT_PVE)	//sky 先检测自己不是处在pvp状态
		{
			SetPveOrPvp(USE_FIGHT_PVE);		//sky 设置自己的战斗状态为pve模式
			Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你已进入pve模式");
		}
	}

	//sky 不管什么模式都把战斗时间刷新下
	SetPkTime();

	if (pUser->getType() == zSceneEntry::SceneEntry_Player)
	{

		SceneUser *pDef = (SceneUser *)pUser;

		if (this->npcdareflag != pDef->npcdareflag)
		{
			if (rev) ScenePk::attackFailToMe(rev,pUser,true);
			if (this->npcdareflag)
			{
				Channel::sendSys(pDef,Cmd::INFO_TYPE_GAME,"对方处于家族战状态!");
			}
			else
			{
				Channel::sendSys(pDef,Cmd::INFO_TYPE_GAME,"你处于家族战状态!");
			}
			return false;
		}

		if (this->isDiplomatState() == 0 || pDef->isDiplomatState() == 0)
		{
			if (rev) ScenePk::attackFailToMe(rev,pUser,true);
			return false;
		}
	}

	if (this->getState() == zSceneEntry::SceneEntry_Death && 
		rev->wdMagicType != 295 //复活技能
		)  
	{
		if (rev) ScenePk::attackFailToMe(rev,pUser,true);
		return false;
	}


	if (!good)
	{
		int pkAttackDodge = (int)( (int)skillValue.akdodge + (int)skillValue.dodge - (int)((int)skillValue.reduce_akdodge));

		if (pkAttackDodge<0)
		{
			pkAttackDodge = 1;
		}
		else if (pkAttackDodge>99)
		{
			pkAttackDodge = 99;
		}

		if (zMisc::selectByPercent(pkAttackDodge))
		{
			if (rev) ScenePk::attackFailToMe(rev,pUser,false);
			return false;
		}
	}
	//初始化pk数据
	this->pkValue.init();
	this->skillValue.init();



	this->skillStatusM.processPassiveness();  // 处理我的被动状态影响


	if (pUser->getType() == zSceneEntry::SceneEntry_Player)
	{
		SceneUser *pAtt = (SceneUser *)pUser;
		ScenePk::calpdamU2U(rev,pAtt,this);
	}
	else if (pUser->getType() == zSceneEntry::SceneEntry_NPC)
	{
		SceneNpc *pAtt = (SceneNpc *)pUser;
		ScenePk::calpdamN2U(rev,pAtt,this);
	}

	if (pUser->getType() == zSceneEntry::SceneEntry_Player)
	{
		SceneUser *pAtt = (SceneUser *)pUser;
		ScenePk::calmdamU2U(rev,pAtt,this);
	}
	else if (pUser->getType() == zSceneEntry::SceneEntry_NPC)
	{
		SceneNpc *pAtt = (SceneNpc *)pUser;
		ScenePk::calmdamN2U(rev,pAtt,this);
	}

	// 修改耐久度只在被击成功后才扣除耐久
	packs.equip.costDefenceDur(this);
#endif
	return true;
}

/**
* \brief 得到善恶度的名称
*
*  \return 返回当前善恶度数值相对称的名称。如果当前数值无效,返回长度为0的一个字符串
*
*/
char *SceneUser::getGoodnessName()
{
	switch(getGoodnessState())
	{
	case Cmd::GOODNESS_0:
		{
			return "英雄";
		}
		break;
	case Cmd::GOODNESS_1:
		{
			return "侠士";
		}
		break;
	case Cmd::GOODNESS_2_1:
		{
			return "普通人";
		}
		break;
	case Cmd::GOODNESS_3:
		{
			return "歹徒";
		}
		break;
	case Cmd::GOODNESS_4:
		{
			return "恶徒";
		}
		break;
	case Cmd::GOODNESS_5:
		{
			return "恶魔";
		}
		break;
	case Cmd::GOODNESS_6:
		{
			return "魔头";
		}
		break;
	default:
		{
			return "";
		}
		break;
	}
}

bool SceneUser::isPkAddition()
{
	return charbase.pkaddition > 1800;
}
DWORD SceneUser::getPkAddition()
{
	if (charbase.pkaddition > 1800)
	{
		DWORD ret = (DWORD)((charbase.pkaddition - 1800)/12 + 60);
		return ret>240?240:ret;
	}
	return 0;
}
/**
* \brief 根据善恶值,计算善恶等级
*
*
* \return 返回善恶等级
*/
short SceneUser::getGoodnessState() const 
{
	if (charbase.goodness & 0x0000FFFF == Cmd::GOODNESS_7)
	{
		return Cmd::GOODNESS_7;
	}
	short good = 0x0000FFFF & charbase.goodness;
	if (good < -60)
	{
		good = Cmd::GOODNESS_0;
	}
	else if (good < 0 && good >= -60)
	{
		good = Cmd::GOODNESS_1;
	}
	else if (good == 0)
	{
		good = Cmd::GOODNESS_2_1;
	}
	else if (good > 0 && good <= 60)
	{
		good = Cmd::GOODNESS_3;
	}
	else if (good > 60 && good <= 120)
	{
		good = Cmd::GOODNESS_4;
	}
	else if (good > 120 && good <= 180)
	{
		good = Cmd::GOODNESS_5;
	}
	else if (good > 180 && good < 10000)
	{
		good = Cmd::GOODNESS_6;
	}
	return good;
}

/**
* \brief 判断是否红名
*
* 
* \return 如果是红名返回TRUE,否则返回FALSE
*/
bool SceneUser::isRedNamed(bool allRedMode) const
{
#if 0
	if (allRedMode)
	{
		if (mask.is_masking()) return false;
		if (((charbase.goodness&0x0000ffff)>0)
			&& ((charbase.goodness&0x0000ffff)<=MAX_GOODNESS))
			return true;
	}
	else
	{
		if (((charbase.goodness&0x0000ffff)>=Cmd::GOODNESS_3)
			&& ((charbase.goodness&0x0000ffff)<=MAX_GOODNESS))
			return true;
	}
	//if ((this->getGoodnessState()>=60)&&(this->getGoodnessState()<=(short)MAX_GOODNESS))
	//  return true;
	if (charbase.goodness&Cmd::GOODNESS_ATT)
		return true;
#endif
	return false;
}


/**
* \brief 根据善恶度计算物品价格
*
* \param price 物品原价格
* \param isBuy 
*
* \return 真实交易的价格
*/
float SceneUser::getGoodnessPrice(DWORD price,bool isBuy)
{
	float ret = price;
	if (isBuy)
	{
		switch(getGoodnessState())
		{
		case Cmd::GOODNESS_3:
			{
				ret = price * (0.2f + 1.0f);
			}
			break;
		case Cmd::GOODNESS_4:
			{
				ret = price * (0.5f + 1.0f);
			}
			break;
		case Cmd::GOODNESS_5:
		case Cmd::GOODNESS_6:
			{
				ret = price * (1.0f + 1.0f);
			}
			break;
		default:
			break;
		}
	}
	else
	{
		switch(getGoodnessState())
		{
		case Cmd::GOODNESS_3:
			{
				ret = price * 0.7f;
			}
			break;
		case Cmd::GOODNESS_4:
			{
				ret = price * (0.4f);
			}
			break;
		case Cmd::GOODNESS_5:
		case Cmd::GOODNESS_6:
			{
				ret = price * (0.1f);
			}
			break;
		default:
			break;
		}
	}
	return ret;
}


bool SceneUser::needSaveBinayArchive(BinaryArchiveType type)
{
	switch (type)
	{
#if 0
	case BINARY_DOUBLE_EXP_OBJ:
		{
			return packs.equip.doubleexp_obj?true:false;
		}
		break;
	case BINARY_TONG_OBJ:
		{
			return packs.equip.tong_obj_time;
		}
		break;
	case BINARY_FAMILY_OBJ:
		{
			return packs.equip.family_obj_time;
		}
		break;
	case BINARY_KING_OBJ:
		{
			return packs.equip.king_obj_time;
		}
		break;
	case BINARY_CHANGE_COUNTRY_TIME:
		{
			return this->lastChangeCountryTime;
		}
		break;
	case BINARY_SAFETY:
		{
			return this->safety;
		}
		break;
	case BINARY_GIVE_MATARIAL_NUM:
		{       
			return this->Give_MatarialNum;
		}
		break;
	case BINARY_CARD_NUM:
		{
			return this->Card_num;
		}
	case BINARY_SAFETY_SETUP:
		{
			return this->safety_setup;
		}
		break;
#endif
	case BINARY_ZOBJECT:
		return true;
	case BINARY_COUNTER:
		{
		    if(this->cm.getSize())
			return true;
		}
		break;
	case BINARY_CARD_TUJIAN:
		{
		    return true;
		}
		break;
	case BINARY_CARD_GROUP:
		{
		    return true;
		}
		break;
	case BINARY_HERO_DATA:
		{
		    return true;
		}
		break;
	case BINARY_TMP_CHALLENGE:
		{
		    return true;
		}
		break;
	case BINARY_QUEST:
		{
		    return true;
		}
		break;
	default:
		break;
	}
	return false;
}

bool SceneUser::needSaveTempArchive(TempArchiveType type)
{
#if 0
	switch (type)
	{
	case TEAM:
		{
			TeamManager * team = SceneManager::getInstance().GetMapTeam(TeamThisID);

			if(team)
				return (team->getLeader() == this->tempid) && (team->getSize() > 1);
			else
				return false;
		}
		break;
	case ENTRY_STATE:
		{
			return skillStatusM.getSaveStatusSize()>0;
		}
		break;
	case PET:
		{
			return summon || (guard && saveGuard) || (saveAdopt && adoptList.size());
			//return (pet);
		}
		break;
	case SAFETY_STATE:
		{
			return this->temp_unsafety_state;
		}
		break;
	case ITEM_COOLTIME: //sky 物品冷却类型
		{
			return this->m_ItemCoolTimes.vCoolTimeType.size()>0;
		}
	default:
		break;
	}
#endif
	return false;
}

#if 0
struct SaveTeamExec : public TeamMemExec
{
	struct PairIDAndName
	{
		DWORD id;
		char name[MAX_NAMESIZE];
	};
	SceneUser *leader;
	TempArchiveMember *data;
	int leavesize;
	SaveTeamExec(SceneUser * l,char *buf,int max)
	{
		leader = l; 
		data = (TempArchiveMember *)buf;
		leavesize = max;
	}
	bool exec(TeamMember &member)
	{
		if (leader->id == member.id)
		{
			return true;
		}
		if (leavesize - data->size >= sizeof(member.id))
		{
			PairIDAndName s;
			s.id = member.id;
			bcopy(member.name,s.name,MAX_NAMESIZE);
			bcopy(&s,&data->data[data->size],sizeof(member.id));
			data->size += sizeof(s);
			return true;
		}
		else
		{
			return false;
		}
	}
};
#endif
/**
* \brief 保存宠物状态
*
*/
DWORD SceneUser::savePetState(BYTE * data)
{
#if 0
	if (0==data) return 0;
	int num = 0;

	Cmd::t_PetData * p = (Cmd::t_PetData *)(data+sizeof(int));
	if (pet)
	{
		pet->petData.hp = pet->hp;
		bcopy(&pet->petData,p,sizeof(petData));
	}
	else
		bcopy(&petData,p,sizeof(petData));
	/*
	Cmd::t_PetData * p = (Cmd::t_PetData *)(data+sizeof(int));
	p->type = Cmd::PET_TYPE_PET;
	p->id = pet->npc->id;
	strncpy(p->name,pet->name,MAX_NAMESIZE-1);
	p->exp = 0;//pet->exp;
	p->ai = pet->getPetAI();
	*/

	num++;

	*data = num;
	//Zebra::logger->debug("%s 有 %d 个宠物记录",name,num);

	return sizeof(int)+sizeof(Cmd::t_PetData)*num;
#endif
	return 0;
}

/**
* \brief 载入宠物状态
*
*/
DWORD SceneUser::loadPetState(BYTE * data,int size)
{
#if 0
	if (0==data) return 0;

	int num = *data;
	//Zebra::logger->debug("%s 有 %d 个宠物记录",name,num);
	int off = sizeof(int);

	for (int i=0; i<num; i++)
	{
		if (off>=size) break;

		Cmd::t_PetData * p = (Cmd::t_PetData *)(data+off);
		bcopy(p,&petData,sizeof(petData));

		if (petData.state==Cmd::PET_STATE_NORMAL)
		{
			ScenePet * newPet = NULL;
			if (0==strncmp(p->name,"",MAX_NAMESIZE))
				newPet = summonPet(p->id,Cmd::PET_TYPE_PET);
			else
				newPet = summonPet(p->id,Cmd::PET_TYPE_PET,0,0,p->name);

			if (newPet)
			{
				bcopy(p,&newPet->petData,sizeof(Cmd::t_PetData));
				newPet->getAbilityByLevel(newPet->petData.lv);
				newPet->hp = p->hp;
				if (newPet->hp>newPet->petData.maxhp)
					newPet->hp = newPet->petData.maxhp;
				newPet->setPetAI((Cmd::petAIMode)p->ai);
				newPet->sendData();
			}
		}

		off += sizeof(Cmd::t_PetData);
	}

	return off;
#endif
	return 0;
}

DWORD SceneUser::saveTempPetState(BYTE *data,DWORD maxSize)
{
#if 0
	if (maxSize<4)
	{
		Zebra::logger->error("[宠物]保存宠物临时文档失败,没有空间 maxSize=0");
		return 0;
	}

	BYTE num=0;
	DWORD size = sizeof(num);
	if (summon)
	{
		*(data+size) = summon->getPetType();
		size++;

		bcopy(&summon->petData,data+size,sizeof(Cmd::t_PetData));
		size += sizeof(Cmd::t_PetData);
		DWORD *length = (DWORD*)(data+size);
		size += sizeof(DWORD);
		summon->skillStatusM.saveSkillStatus((char *)(data+size),*length);
		size += *length;
		num++;
	}
	if (guard && saveGuard)
	{
		*(data+size) = guard->getPetType();
		size++;
		size += ((GuardNpc *)guard)->save(data+size);
		DWORD *length = (DWORD*)(data+size);
		size += sizeof(DWORD);
		guard->skillStatusM.saveSkillStatus((char *)(data+size),*length);
		size += *length;
		num++;
	}
	if (saveAdopt)
	{
		for (adopt_it it=adoptList.begin(); it!=adoptList.end(); it++)
		{
			*(data+size) = it->second->getPetType();
			size++;

			size += it->second->save(data+size);
			DWORD *length = (DWORD*)(data+size);
			size += sizeof(DWORD);
			it->second->skillStatusM.saveSkillStatus((char *)(data+size),*length);
			size += *length;
			num++;
		}
	}

	if (size>maxSize)
	{
		Zebra::logger->error("[宠物]保存宠物临时文档失败,空间不足 size=%u",size);
		*data = 0;
		return 1;
	}
	*data = num;
	return size;
#endif
	return 0;
}

DWORD SceneUser::loadTempPetState(BYTE * data)
{
#if 0
	if (0==data) return 0;

	BYTE num = *data;
	Zebra::logger->debug("%s 有 %d 个临时宠物记录",name,num);
	DWORD off = sizeof(num);

	for (DWORD i=0; i<num; i++)
	{
		off++;//宠物类型

		switch(*(data+off-1))
		{
		case Cmd::PET_TYPE_SUMMON:
		case Cmd::PET_TYPE_GUARDNPC:
			{
				Cmd::t_PetData * p = (Cmd::t_PetData *)(data+off);
				//bcopy(p,&petData,sizeof(petData));

				ScenePet * newPet = NULL;
				if (p->state==Cmd::PET_STATE_NORMAL)
				{
					if (strncmp(p->name,"",MAX_NAMESIZE) && p->type!=Cmd::PET_TYPE_GUARDNPC)
						newPet = summonPet(p->id,p->type,0,0,p->name);
					else
						newPet = summonPet(p->id,p->type);

					if (newPet)
					{
						bcopy(p,&newPet->petData,sizeof(Cmd::t_PetData));
						newPet->hp = p->hp;
						newPet->setPetAI((Cmd::petAIMode)p->ai);
						if (p->type==Cmd::PET_TYPE_GUARDNPC)//镖车数据
						{
							((GuardNpc *)newPet)->owner(this);
							((GuardNpc *)newPet)->dest(zPos(p->str,p->intel));
							((GuardNpc *)newPet)->map(p->name);
							((GuardNpc *)newPet)->gold(p->agi);
							((GuardNpc *)newPet)->exp(p->men);
							strncpy(newPet->petData.name,"镖车",MAX_NAMESIZE);
						}
						newPet->sendData();
					}
				}

				off += sizeof(Cmd::t_PetData);
				DWORD *length = (DWORD*)(data+off);
				off += sizeof(DWORD);
				if (newPet) newPet->skillStatusM.loadSkillStatus((char *)(data+off),*length);
				off += *length;
			}
			break;
		case Cmd::PET_TYPE_CARTOON:
			{
				Cmd::t_CartoonData * p = (Cmd::t_CartoonData *)(data+off+sizeof(DWORD));

				zNpcB *base = npcbm.get(p->npcID);
				if (!base) return false;

				t_NpcDefine define;
				define.id = base->id;
				strncpy(define.name,p->name,MAX_NAMESIZE-1);
				define.pos = getPos();
				define.num = 1;
				define.interval = 5;
				define.initstate = zSceneEntry::SceneEntry_Normal;
				define.width = SceneUser::CALL_PET_REGION;
				define.height = SceneUser::CALL_PET_REGION;
				define.pos -= zPos(SceneUser::CALL_PET_REGION/2,SceneUser::CALL_PET_REGION/2);
				define.scriptID = 0;
				scene->initRegion(define.region,define.pos,define.width,define.height);

				CartoonPet * newPet = (CartoonPet *)scene->summonOneNpc<CartoonPet>(define,zPos(0,0),base,dupIndex,0,0,0);
				if (newPet) 
				{
					//strncpy(newPet->name,p->name,MAX_NAMESIZE);
					newPet->setPetType(Cmd::PET_TYPE_CARTOON);
					newPet->setMaster(this);
					SceneNpcManager::getMe().addSpecialNpc(newPet);

					newPet->setCartoonID(*(DWORD *)(data+off));
					newPet->setCartoonData(*p);
					adoptList[newPet->getCartoonID()] = newPet;
				}

				off += sizeof(DWORD)+sizeof(Cmd::t_CartoonData);
				DWORD *length = (DWORD*)(data+off);
				off += sizeof(DWORD);
				if (newPet) newPet->skillStatusM.loadSkillStatus((char *)(data+off),*length);
				off += *length;
			}
			break;
		default:
			break;
		}
	}

	return off;
#endif
	return 0;
}

/**
* \brief 保存卡通宠物
*
*/
void SceneUser::saveCartoonState()
{
#if 0
	for (cartoon_it it=cartoonList.begin(); it!=cartoonList.end(); it++)
	{
		if (it->second.state==Cmd::CARTOON_STATE_WAITING
			|| it->second.state==Cmd::CARTOON_STATE_ADOPTED)
			continue;

		Cmd::Session::t_saveCartoon_SceneSession send;
		strncpy(send.userName,name,MAX_NAMESIZE);
		send.type = Cmd::Session::SAVE_TYPE_TIMETICK;
		send.cartoonID = it->first;
		send.data = it->second;
		sessionClient->sendCmd(&send,sizeof(send));
	}
	/*
	for (adopt_it it=adoptList.begin(); it!=adoptList.end(); it++)
	{
	Cmd::Session::t_saveCartoon_SceneSession send;
	send.type = Cmd::Session::SAVE_TYPE_SYN;
	send.cartoonID = it->first;
	send.data = it->second->getCartoonData();
	sessionClient->sendCmd(&send,sizeof(send));
	}
	*/
#endif
}

#if 0
/**
* \brief 删除召唤兽
*
*/
void SceneUser::killSummon()
{
	if (summon)
	{
		summon->skillStatusM.clearActiveSkillStatus();

		summon->killAllPets();
		summon->leaveBattle();
		summon->scene->clearBlock(summon->getPos());
		summon->setState(SceneEntry_Death);
		summon->setMoveTime(SceneTimeTick::currentTime,summon->define->interval*1000);//尸体消失的时间

		Cmd::stNpcDeathUserCmd death;
		death.dwNpcTempID = summon->tempid;
		summon->scene->sendCmdToNine(summon->getPosI(),&death,sizeof(death),dupIndex);

		Cmd::stDelPetPetCmd del;
		del.id= summon->tempid;
		del.type = Cmd::PET_TYPE_SUMMON;
		sendCmdToMe(&del,sizeof(del));

		summon->clearMaster();
		summon = 0;

		//summon->toDie(tempid);
		//killOnePet(summon);
	}
}

/**
* \brief 删除所有宠物
*
*/
void SceneUser::killAllPets()
{
	//删除所有宠物
#ifdef _DEBUG
	//Zebra::logger->debug("SceneUser::killAllPets(): lock %s",name);
#endif
	std::list<ScenePet *> copy(totems);
	for (std::list<ScenePet *>::iterator it=copy.begin(); it!=copy.end(); it++)
	{
		(*it)->skillStatusM.clearActiveSkillStatus();

		(*it)->killAllPets();
		(*it)->leaveBattle();
		(*it)->scene->clearBlock((*it)->getPos());
		(*it)->setState(SceneEntry_Death);
		(*it)->setMoveTime(SceneTimeTick::currentTime,(*it)->define->interval*1000);//尸体消失的时间

		Cmd::stDelPetPetCmd del;
		del.id= (*it)->tempid;
		del.type = Cmd::PET_TYPE_TOTEM;
		sendCmdToMe(&del,sizeof(del));

		(*it)->clearMaster();

		Cmd::stNpcDeathUserCmd death;
		death.dwNpcTempID = (*it)->tempid;
		(*it)->scene->sendCmdToNine((*it)->getPosI(),&death,sizeof(death),dupIndex);
	}
	totems.clear();

	if (ridepet)
	{
		horse.data.state = Cmd::HORSE_STATE_PUTUP;
		horse.sendData();

		ridepet->setClearState();
		ridepet->clearMaster();
		ridepet = 0;
		/*
		Cmd::stDelPetPetCmd del;
		del.id= ridepet->tempid;
		del.type = Cmd::PET_TYPE_RIDE;
		sendCmdToMe(&del,sizeof(del));

		ridepet->clearMaster();
		ridepet->setClearState();
		ridepet = 0;
		*/
		//ridepet->toDie(tempid);
		//killOnePet(ridepet);
	}
	if (pet)
	{
		//pet->skillStatusM.clearActiveSkillStatus();

		//pet->petData.hp = pet->hp;
		//bcopy(&pet->petData,&petData,sizeof(petData));

		//pet->killAllPets();
		//pet->leaveBattle();
		//pet->scene->clearBlock(pet->getPos());
		//pet->setState(SceneEntry_Death);
		//pet->setMoveTime(SceneTimeTick::currentTime,pet->define->interval*1000);//尸体消失的时间

		Cmd::stNpcDeathUserCmd death;
		death.dwNpcTempID = pet->tempid;
		pet->scene->sendCmdToNine(pet->getPosI(),&death,sizeof(death),dupIndex);

		Cmd::stDelPetPetCmd del;
		del.id= pet->tempid;
		del.type = Cmd::PET_TYPE_PET;
		sendCmdToMe(&del,sizeof(del));

		pet->clearMaster();
		pet->toDie(tempid);
		killOnePet(pet);

		pet = 0;
	}
	//sky 删除幻影
	if( !MirageSummon.empty() )
	{
		std::list<ScenePet *>::iterator it;
		for( it=MirageSummon.begin(); it!=MirageSummon.end(); it++ )
		{
			if( (*it) )
			{
				(*it)->clearMaster();

				(*it)->toDie(tempid);

				killOnePet((*it));
			}
		}
		MirageSummon.clear( );
	}
	if (summon)
	{
		summon->skillStatusM.clearActiveSkillStatus();

		summon->killAllPets();
		summon->leaveBattle();
		summon->scene->clearBlock(summon->getPos());
		summon->setState(SceneEntry_Death);
		summon->setMoveTime(SceneTimeTick::currentTime,summon->define->interval*1000);//尸体消失的时间

		Cmd::stNpcDeathUserCmd death;
		death.dwNpcTempID = summon->tempid;
		summon->scene->sendCmdToNine(summon->getPosI(),&death,sizeof(death),dupIndex);

		Cmd::stDelPetPetCmd del;
		del.id= summon->tempid;
		del.type = Cmd::PET_TYPE_SUMMON;
		sendCmdToMe(&del,sizeof(del));

		summon->clearMaster();

		summon->toDie(tempid);
		killOnePet(summon);

		summon = 0;
	}
	if (guard)
	{
		Cmd::stDelPetPetCmd del;
		del.id= guard->tempid;
		del.type = Cmd::PET_TYPE_GUARDNPC;
		sendCmdToMe(&del,sizeof(del));

		//guard->reset();
		//guard->toDie(tempid);
		/*
		OnOther event(2);
		EventTable::instance().execute(*this,event);
		guard->reset();
		//guard->clearMaster();
		guard = 0;
		*/
		//killOnePet(guard);
	}

	//保存跟随的宠物
	if (cartoon)
		cartoon->putAway(Cmd::Session::SAVE_TYPE_PUTAWAY);
	//保存收起的宠物
	for (cartoon_it it=cartoonList.begin(); it!=cartoonList.end(); it++)
	{
		if ((cartoon && cartoon->getCartoonID()==it->first)
			|| it->second.state==Cmd::CARTOON_STATE_WAITING
			|| it->second.state==Cmd::CARTOON_STATE_ADOPTED)
			continue;

		Cmd::Session::t_saveCartoon_SceneSession send;
		strncpy(send.userName,name,MAX_NAMESIZE);
		send.type = Cmd::Session::SAVE_TYPE_PUTAWAY;
		send.cartoonID = it->first;
		it->second.state = Cmd::CARTOON_STATE_PUTAWAY;
		it->second.repair = 0;
		send.data = it->second;
		sessionClient->sendCmd(&send,sizeof(send));
	}
	//归还领养的宠物
	for (adopt_it it=adoptList.begin(); it!=adoptList.end();)
		(it++)->second->putAway(saveAdopt?Cmd::Session::SAVE_TYPE_DONTSAVE:Cmd::Session::SAVE_TYPE_RETURN);
#ifdef _DEBUG
	//Zebra::logger->debug("SceneUser::killAllPets(): unlock %s",name);
#endif
}

void SceneUser::clearGuardNpc()
{
	if (guard)
	{
		guard->skillStatusM.clearActiveSkillStatus();
		guard->killAllPets();
		guard->leaveBattle();
		guard->scene->clearBlock(guard->getPos());
		guard->setState(SceneEntry_Death);
		guard->setMoveTime(SceneTimeTick::currentTime,guard->define->interval*1000);//尸体消失的时间

		Cmd::stNpcDeathUserCmd death;
		death.dwNpcTempID = guard->tempid;
		guard->scene->sendCmdToNine(guard->getPosI(),&death,sizeof(death),dupIndex);

		Cmd::stDelPetPetCmd del;
		del.id= guard->tempid;
		del.type = Cmd::PET_TYPE_GUARDNPC;
		sendCmdToMe(&del,sizeof(del));

		guard->clearMaster();
		guard = 0;
	}
}
#endif
/**
* \brief 保存二进制数据扩充部分数据
*
*
* \param type 类型(BinaryArchiveType)
* \param out 输出数据
* \param maxSize 可用out大小
* \return 使用out的字节数
*/
DWORD SceneUser::addBinaryArchiveMember(DWORD type,char **out,DWORD maxSize)
{
    DWORD dwSize = 0;
    if (!out)
    {
	return dwSize;
    }
    dwSize = sizeof(BinaryArchiveMember);
    BinaryArchiveMember *data = (BinaryArchiveMember *)*out;
    data->type = type;
    data->size = 0;

    switch(data->type)
    {
#if 0
	case BINARY_DOUBLE_EXP_OBJ:
	    {
		*(DWORD*)&data->data[data->size]=packs.equip.doubleexp_obj;
		data->size += sizeof(DWORD);
		*(DWORD*)&data->data[data->size]=packs.equip.doubleexp_obj_time;
		data->size += sizeof(DWORD);
		dwSize += data->size;
	    }
	    break;
	case BINARY_TONG_OBJ:
	    {
		*(DWORD*)&data->data[data->size]=packs.equip.tong_obj_times;
		data->size += sizeof(DWORD);
		*(DWORD*)&data->data[data->size]=packs.equip.tong_obj_time;
		data->size += sizeof(DWORD);
		dwSize += data->size;
	    }
	    break;
	case BINARY_FAMILY_OBJ:
	    {
		*(DWORD*)&data->data[data->size]=packs.equip.family_obj_times;
		data->size += sizeof(DWORD);
		*(DWORD*)&data->data[data->size]=packs.equip.family_obj_time;
		data->size += sizeof(DWORD);
		dwSize += data->size;
	    }
	    break;
	case BINARY_KING_OBJ:
	    {
		*(DWORD*)&data->data[data->size]=packs.equip.king_obj_times;
		data->size += sizeof(DWORD);
		*(DWORD*)&data->data[data->size]=packs.equip.king_obj_time;
		data->size += sizeof(DWORD);
		dwSize += data->size;
	    }
	    break;
	case BINARY_CHANGE_COUNTRY_TIME:
	    {
		*(DWORD*)&data->data[data->size]=this->lastChangeCountryTime;
		data->size += sizeof(DWORD);
		dwSize += data->size;
	    }
	    break;
	case BINARY_SAFETY:
	    {
		*(BYTE*)&data->data[data->size]=this->safety;
		data->size += sizeof(BYTE);
		dwSize += data->size;
	    }
	    break;
	case BINARY_GIVE_MATARIAL_NUM:
	    {
		*(DWORD*)&data->data[data->size]=this->Give_MatarialNum;
		data->size += sizeof(DWORD);
		dwSize += data->size;
	    }
	    break;
	case BINARY_CARD_NUM:
	    {
		*(DWORD*)&data->data[data->size]=this->Card_num;
		data->size += sizeof(DWORD);
		dwSize += data->size;
	    }
	    break;
	case BINARY_SAFETY_SETUP:
	    {
		*(BYTE*)&data->data[data->size]=this->safety_setup;
		data->size += sizeof(BYTE);
		dwSize += data->size;
	    }
	    break;
#endif
	case BINARY_ZOBJECT:
	    {
		SaveObjectExec exec;
		exec.full = (ZlibObject *)(&data->data[data->size]);
		constructInPlace(exec.full);
		exec.full->version = BINARY_VERSION;
		this->packs.uom.execEvery(exec);
		data->size += sizeof(ZlibObject)+exec.full->count*sizeof(SaveObject);
		dwSize += data->size;
	    }
	    break;
	case BINARY_COUNTER:
	    {
		data->size += this->cm.save(&data->data[data->size]);
		dwSize += data->size;
	    }
	    break;
	case BINARY_CARD_TUJIAN:
	    {
		data->size += this->tujianData.saveCardTujianData(&data->data[data->size]);
		dwSize += data->size;
	    }
	    break;
	case BINARY_CARD_GROUP:
	    {
		data->size += this->groupcardData.saveCardGroupData(&data->data[data->size]);
		dwSize += data->size;
	    }
	    break;
	case BINARY_HERO_DATA:
	    {
		data->size += this->ahData.saveHeroData(&data->data[data->size]);
		dwSize += data->size;
	    }
	    break;
	case BINARY_TMP_CHALLENGE:
	    {
		data->size += this->ctData.saveChallengeData(&data->data[data->size]);
		dwSize += data->size;
	    }
	    break;
	case BINARY_QUEST:
	    {
		data->size += quest_list.save(&data->data[data->size]);
		dwSize += data->size;
	    }
	    break;
	case BINARY_MAX:
	    {
	    }
	    break;
	default:
	    break;
    }
    Zebra::logger->debug("�������� %u, ��С %u",data->type, data->size);
    *out += dwSize;
    return dwSize;
}
/**
* \brief 存储小组到临时文档
*
*/
DWORD SceneUser::addTempArchiveMember(DWORD type,char *out,DWORD maxSize)
{
#if 0
	DWORD dwSize = 0;
	if (!out)
	{
		return dwSize;
	}
	dwSize = sizeof(TempArchiveMember);
	TempArchiveMember *data = (TempArchiveMember *)out;
	data->type = type;
	data->size = 0;
	switch(data->type)
	{
	//case TEAM:
	//	{
	//		SaveTeamExec st(this,out,maxSize);
	//		team.execEveryOne(st);
	//		*(DWORD*)&data->data[data->size]=this->team_mode;
	//		data->size += sizeof(DWORD);
	//		*(DWORD*)&data->data[data->size]=this->team.getNextObjOwnerID();
	//		data->size += sizeof(DWORD);
	//		*(BYTE*)&data->data[data->size]=this->team.getObjMode();
	//		data->size += sizeof(BYTE);
	//		*(BYTE*)&data->data[data->size]=this->team.getExpMode();
	//		data->size += sizeof(BYTE);
	//		//Zebra::logger->debug("临时存储队伍信息大小%u!",data->size);
	//		dwSize += data->size;
	//	}
	//	break;
	case ENTRY_STATE:
		{
			skillStatusM.saveSkillStatus(data->data,data->size);
			dwSize += data->size;
			//Zebra::logger->debug("状态临时文档大小%u",data->size);
		}
		break;
	case PET:
		{
			data->size = saveTempPetState((BYTE*)data->data,maxSize-dwSize);
			dwSize += data->size;
			//Zebra::logger->debug("宠物临时文档大小%u",data->size);
		}
		break;
	case SAFETY_STATE:
		{
			*(DWORD*)&data->data[data->size]=this->temp_unsafety_state;
			data->size += sizeof(DWORD);
			dwSize += data->size;		
		}
		break;
	case ITEM_COOLTIME:
		{
			data->size = saveItemCoolTimes((BYTE*)data->data,maxSize-dwSize);
			dwSize += data->size;
		}
		break;
	default:
		break;
	}
	return dwSize;
#endif
	return 0;
}

DWORD SceneUser::saveBinaryArchive(BYTE *out,const int maxsize)
{
	DWORD size=0;
	BYTE *data=out;
#if 0
	if (needSaveBinayArchive(BINARY_DOUBLE_EXP_OBJ))
	{
		size += addBinaryArchiveMember(BINARY_DOUBLE_EXP_OBJ,(char *)data,maxsize-size);
		data = &data[size];
	}
	if (needSaveBinayArchive(BINARY_TONG_OBJ))
	{
		size += addBinaryArchiveMember(BINARY_TONG_OBJ,(char *)data,maxsize-size);
		data = &data[size];
	}
	if (needSaveBinayArchive(BINARY_FAMILY_OBJ))
	{
		size += addBinaryArchiveMember(BINARY_FAMILY_OBJ,(char *)data,maxsize-size);
		data = &data[size];
	}
	if (needSaveBinayArchive(BINARY_KING_OBJ))
	{
		size += addBinaryArchiveMember(BINARY_KING_OBJ,(char *)data,maxsize-size);
		data = &data[size];
	}
	if (needSaveBinayArchive(BINARY_CHANGE_COUNTRY_TIME))
	{
		size += addBinaryArchiveMember(BINARY_CHANGE_COUNTRY_TIME,(char*)data,maxsize-size);
		data = &data[size];
	}

	if (needSaveBinayArchive(BINARY_SAFETY))
	{
		size += addBinaryArchiveMember(BINARY_SAFETY,(char*)data,maxsize-size);
		data = &data[size];
	}

	if (needSaveBinayArchive(BINARY_GIVE_MATARIAL_NUM))
	{
		size += addBinaryArchiveMember(BINARY_GIVE_MATARIAL_NUM,(char*)data,maxsize-size);
		data = &data[size];
	}
	if (needSaveBinayArchive(BINARY_CARD_NUM))
	{
		size += addBinaryArchiveMember(BINARY_CARD_NUM,(char*)data,maxsize-size);
		data = &data[size];
	}

	if (needSaveBinayArchive(BINARY_SAFETY_SETUP))
	{
		size += addBinaryArchiveMember(BINARY_SAFETY_SETUP,(char*)data,maxsize-size);
		data = &data[size];
	}
#endif
	if (needSaveBinayArchive(BINARY_ZOBJECT))
	{
	    size += addBinaryArchiveMember(BINARY_ZOBJECT,(char**)&data,maxsize-size);
	}

	if (needSaveBinayArchive(BINARY_COUNTER))
	{
	    size += addBinaryArchiveMember(BINARY_COUNTER,(char**)&data,maxsize-size);
	}

	if (needSaveBinayArchive(BINARY_CARD_TUJIAN))
	{
	    size += addBinaryArchiveMember(BINARY_CARD_TUJIAN,(char**)&data,maxsize-size);
	}

	if (needSaveBinayArchive(BINARY_CARD_GROUP))
	{
	    size += addBinaryArchiveMember(BINARY_CARD_GROUP,(char**)&data,maxsize-size);
	}

	if (needSaveBinayArchive(BINARY_HERO_DATA))
	{
	    size += addBinaryArchiveMember(BINARY_HERO_DATA,(char**)&data,maxsize-size);
	}
	if (needSaveBinayArchive(BINARY_TMP_CHALLENGE))
	{
	    size += addBinaryArchiveMember(BINARY_TMP_CHALLENGE,(char**)&data,maxsize-size);
	}
	if (needSaveBinayArchive(BINARY_QUEST))
	{
	    size += addBinaryArchiveMember(BINARY_QUEST,(char**)&data,maxsize-size);
	}

	size += addBinaryArchiveMember(BINARY_MAX,(char **)&data,maxsize-size);
	return sizeof(DWORD) + size;
}
/**
* \brief 保存所有临时档案
*
*/
void SceneUser::saveTempArchive()
{
	bool saved = false;
	char buf[MAX_TEMPARCHIVE_SIZE + sizeof(TempArchiveMember) + sizeof(Cmd::Session::t_WriteUser_SceneArchive)];

	bzero(buf,sizeof(buf));
	Cmd::Session::t_WriteUser_SceneArchive *ws = (Cmd::Session::t_WriteUser_SceneArchive *)buf;
	constructInPlace(ws);
	ws->id = this->id;
	ws->dwMapTempID = this->scene->tempid; 
	char *data = ws->data;
	//组队临时档案
	if (needSaveTempArchive(TEAM))
	{
		saved = true;
		ws->dwSize += addTempArchiveMember(TEAM,data,MAX_TEMPARCHIVE_SIZE - ws->dwSize);
		data = &ws->data[ws->dwSize];
		//*(DWORD*)data = this->team_mode;
		//data = data + sizeof(DWORD);
	}
	//状态临时档案
	if (needSaveTempArchive(ENTRY_STATE))
	{
		saved = true;
		ws->dwSize += addTempArchiveMember(ENTRY_STATE,data,MAX_TEMPARCHIVE_SIZE - ws->dwSize);
		data = &ws->data[ws->dwSize];
	}
	//宠物状态
	if (needSaveTempArchive(PET))
	{
		saved = true;
		ws->dwSize += addTempArchiveMember(PET,data,MAX_TEMPARCHIVE_SIZE - ws->dwSize);
		data = &ws->data[ws->dwSize];
	}
	// 密码保护状态
	if (needSaveTempArchive(SAFETY_STATE))
	{
		saved = true;
		ws->dwSize += addTempArchiveMember(SAFETY_STATE,data,MAX_TEMPARCHIVE_SIZE - ws->dwSize);
		data = &ws->data[ws->dwSize];
	}
	// sky 物品冷却时间
	if(needSaveTempArchive(ITEM_COOLTIME))
	{
		saved = true;
		ws->dwSize += addTempArchiveMember(ITEM_COOLTIME, data, MAX_TEMPARCHIVE_SIZE - ws->dwSize);
		data = &ws->data[ws->dwSize];
	}
	//Zebra::logger->debug("临时存档数据大小%u",ws->dwSize);
	//TODO 其他档案数据

	if (saved)
	{
		sessionClient->sendCmd(ws,sizeof(Cmd::Session::t_WriteUser_SceneArchive) + ws->dwSize);
	}
}

DWORD SceneUser::setupBinaryArchive(const char *revData)
{
	DWORD size=0;
	BinaryArchiveMember *data = (BinaryArchiveMember *)revData;
	ZlibObject *zo = NULL;
	zo = (ZlibObject *)(&data->data);
	while(data->size)//为0是BINARY_MAX占位的
	{
		switch(data->type)
		{
#if 0
		case BINARY_DOUBLE_EXP_OBJ:
			{
				std::pair<DWORD,DWORD> *pair=(std::pair<DWORD,DWORD>*)data->data;
				//判断是否在同一天
				if (SceneTimeTick::currentTime.sec()/86400 == pair->second/86400)
				{
					this->packs.equip.doubleexp_obj=pair->first;
					this->packs.equip.doubleexp_obj_time=pair->second;
				}
			}
			break;
		case BINARY_TONG_OBJ:
			{
				std::pair<DWORD,DWORD> *pair=(std::pair<DWORD,DWORD>*)data->data;
				//判断是否在同一天
				if (SceneTimeTick::currentTime.sec()/86400 == pair->second/86400)
				{
					this->packs.equip.tong_obj_times=pair->first;
					this->packs.equip.tong_obj_time=pair->second;
				}
			}
			break;
		case BINARY_FAMILY_OBJ:
			{
				std::pair<DWORD,DWORD> *pair=(std::pair<DWORD,DWORD>*)data->data;
				//判断是否在同一天
				if (SceneTimeTick::currentTime.sec()/86400 == pair->second/86400)
				{
					this->packs.equip.family_obj_times=pair->first;
					this->packs.equip.family_obj_time=pair->second;
				}
			}
			break;
		case BINARY_KING_OBJ:
			{
				std::pair<DWORD,DWORD> *pair=(std::pair<DWORD,DWORD>*)data->data;
				//判断是否在同一天
				if (SceneTimeTick::currentTime.sec()/86400 == pair->second/86400)
				{
					this->packs.equip.king_obj_times=pair->first;
					this->packs.equip.king_obj_time=pair->second;
				}
			}
			break;
		case BINARY_CHANGE_COUNTRY_TIME:
			{
				this->lastChangeCountryTime = *(DWORD*)data->data;
			}
			break;
		case BINARY_SAFETY:
			{
				this->safety = *(BYTE*)data->data;
			}
			break;
		case BINARY_GIVE_MATARIAL_NUM:
			{
				this->Give_MatarialNum = *(DWORD*)data->data;
			}
			break;
		case BINARY_CARD_NUM:
			{
				this->Card_num = *(DWORD*)data->data;
			}
			break; 
		case BINARY_SAFETY_SETUP:
			{
				this->safety_setup = *(BYTE*)data->data;
			}
			break;
#endif
		case BINARY_ZOBJECT:
			{
			    ZlibObject *zo = NULL;
			    zo = (ZlibObject *)(&data->data);
			    std::vector<SaveObject> buf;
			    SaveObject* object = getSaveObjectFromZlibObject(*zo, buf);
			    //int now_time = time(NULL);
			    zObject *o = NULL;
			    Zebra::logger->debug("packs binary count:%u",zo->count);
			    for(DWORD i=0; i<zo->count; i++)
			    {
				DWORD table_id = 0;
				table_id = object[i].object.qwThisID;
				o = zObject::load(&object[i]);
				if(table_id && o && o->data.qwThisID != table_id)
				{//this id changed
				    for(DWORD j=i+1; j<zo->count; j++)
				    {
					if(object[j].object.pos.tab() == table_id)
					{
					    object[j].object.pos.tab(o->data.qwThisID);
					}
				    }
				}
				if(o == NULL)
				{
				    Zebra::logger->error("load object error");
				}
				else
				{
				    if(this->packs.addObject(o, false))
				    {//����һЩʱЧ�Ե���
					
				    }
				    else
				    {
					//zObject::logger(o->createid, o->data.qwThisID, o->data.strName, o->data.dwNum, o->data.dwNum, 0, this->id, this->name,0,NULL,"load_error",o->base,o->data.kind,o->data.upgrade, o->base->id);
				    }
				}

			    }
			}
			break;
		case BINARY_COUNTER:
			this->cm.load(data->data, data->size);
			break;
		case BINARY_CARD_TUJIAN:
			{
			    this->tujianData.loadCardTujianData(data->data);
			}
			break;
		case BINARY_CARD_GROUP:
			{
			    this->groupcardData.loadCardGroupData(data->data);
			}
			break;
		case BINARY_HERO_DATA:
			{
			    this->ahData.loadHeroData(data->data);
			}
			break;
		case BINARY_TMP_CHALLENGE:
			{
			    this->ctData.loadChallengeData(data->data);
			}
			break;
		case BINARY_QUEST:
			{
			    this->quest_list.load(data->data);
			}
			break;
		default:
			break;
		}
		size += data->size;
		data = (BinaryArchiveMember *)&data->data[data->size];
	}
	return size;
}
void SceneUser::setupTempArchive(const char *revData,const DWORD dwSize)
{
#if 0
	if (dwSize > MAX_TEMPARCHIVE_SIZE)
	{
		return ;
	}
	char buf[MAX_TEMPARCHIVE_SIZE + sizeof(TempArchiveMember)];
	bzero(buf,sizeof(buf));
	bcopy(revData,buf,dwSize);
	TempArchiveMember *data = (TempArchiveMember *)buf;
	//Zebra::logger->debug("临时文档大小%u",data->size);
	while(data->size)
	{
		switch(data->type)
		{
		case TEAM:
			{
				//team.loadTeam(this,data);
				//Zebra::logger->debug("读取队伍临时文档大小%u",data->size);
			}
			break;
		case ENTRY_STATE:
			{
				//Zebra::logger->debug("读取状态临时文档大小%u",data->size);
				skillStatusM.loadSkillStatus(data->data,data->size);
			}
			break;
		case PET:
			{
				loadTempPetState((BYTE*)data->data);
				//Zebra::logger->debug("读取宠物临时文档大小%u",data->size);
			}
			break;
		case SAFETY_STATE:
			{
				temp_unsafety_state = *(DWORD*)data->data;
				Zebra::logger->debug("%s  密码保护状态: %u",this->name,temp_unsafety_state);

				if (this->safety==1)
				{
					Cmd::stNotifySafetyUserCmd send;
					if (this->temp_unsafety_state==1)
						send.byState = Cmd::SAFETY_TEMP_CLOSE;
					else
						send.byState = Cmd::SAFETY_OPEN;

					this->sendCmdToMe(&send,sizeof(send));
				}
			}
			break;
		case ITEM_COOLTIME:
			{
				DWORD size = loadItemCoolTimes((BYTE*)data->data);

				if(size != data->size)
					Zebra::logger->debug("加载临时档案冷却时间的时候读取出错误的长度!原始长度%u.读取长度%u", data->size, size);
			}
			break;
		default:
			{
				Zebra::logger->debug("加载临时档案失败.");
				return;
			}
			break;
		}
		data = (TempArchiveMember *)&data->data[data->size];
	}
#endif
}

/**
* \brief 掉落装备的处理
*
*
*/
void SceneUser::lostObject(SceneUser *pAtt)
{
#if 0
	if (this->scene && this->scene->isNoRedScene())
	{
		return;
	}
	int mainpack = 0;
	int equippack = 0;
	//如果是被npc杀死
	/*
	if (!pAtt)//npc
	{
	// */
	switch(this->getGoodnessState())
	{
	case Cmd::GOODNESS_0:
		{
			return;
		}
		break;
	case Cmd::GOODNESS_1:
		{
			if (zMisc::selectByPercent(2))
			{
				mainpack = 1;
			}
		}
		break;
	case Cmd::GOODNESS_2_1:
		{
			if (zMisc::selectByPercent(30))
			{
				mainpack = zMisc::randBetween(1,3);
			}
		}
		break;
	case Cmd::GOODNESS_3:
		{
			if (zMisc::selectByPercent(30))
			{
				mainpack = zMisc::randBetween(1,3);
			}
			/*
			if (zMisc::selectByPercent(50))
			{
			mainpack = 1;//zMisc::randBetween(1,3);
			}
			if (zMisc::selectByPercent(20))
			{
			equippack = 1;
			}
			// */
		}
		break;
	case Cmd::GOODNESS_4:
		{
			if (zMisc::selectByPercent(50))
			{
				mainpack = zMisc::randBetween(1,5);
			}
			if (zMisc::selectByPercent(50))
			{
				equippack = 1;
			}
		}
		break;
	case Cmd::GOODNESS_5:
		{
			if (zMisc::selectByPercent(100))
			{
				mainpack = zMisc::randBetween(5,10);
			}
			if (zMisc::selectByPercent(100))
			{
				equippack = 1;//zMisc::randBetween(1,3);
			}
		}
		break;
	case Cmd::GOODNESS_6:
		{
			if (zMisc::selectByPercent(100))
			{
				mainpack = zMisc::randBetween(5,20);
			}
			if (zMisc::selectByPercent(100))
			{
				equippack = 1;//zMisc::randBetween(1,4);
			}
		}
		break;
	case Cmd::GOODNESS_7:
		{
			if (zMisc::selectByPercent(100))
			{
				mainpack = zMisc::randBetween(5,20);
			}
			if (zMisc::selectByPercent(100))
			{
				equippack = zMisc::randBetween(1,4);
			}
		}
		break;
	}
	/*
	}
	else if (isAllied(pAtt))//本国,盟国人物
	{

	switch(this->getGoodnessState())
	{
	case Cmd::GOODNESS_0:
	{
	return ;
	}
	break;
	case Cmd::GOODNESS_1:
	{
	return ;
	}
	break;
	case Cmd::GOODNESS_2_1:
	{
	return ;
	}
	break;
	case Cmd::GOODNESS_3:
	{
	if (zMisc::selectByPercent(10))
	{
	mainpack = zMisc::randBetween(1,3);
	}
	}
	break;
	case Cmd::GOODNESS_4:
	{
	if (zMisc::selectByPercent(20))
	{
	mainpack = zMisc::randBetween(1,4);
	}
	if (zMisc::selectByPercent(10))
	{
	equippack = zMisc::randBetween(1,2);
	}
	}
	break;
	case Cmd::GOODNESS_5:
	{
	if (zMisc::selectByPercent(40))
	{
	mainpack = zMisc::randBetween(1,5);
	}
	if (zMisc::selectByPercent(20))
	{
	equippack = zMisc::randBetween(1,3);
	}
	}
	break;
	case Cmd::GOODNESS_6:
	{
	if (zMisc::selectByPercent(60))
	{
	mainpack = zMisc::randBetween(1,6);
	}
	if (zMisc::selectByPercent(40))
	{
	equippack = zMisc::randBetween(1,4);
	}
	}
	break;
	case Cmd::GOODNESS_7:
	{
	if (zMisc::selectByPercent(40))
	{
	mainpack = zMisc::randBetween(1,6);
	}
	if (zMisc::selectByPercent(40))
	{
	equippack = zMisc::randBetween(1,4);
	}
	}
	break;
	}
	}
	else //敌国,中立国人物
	{

	switch(this->getGoodnessState())
	{
	case Cmd::GOODNESS_0:
	{
	if (zMisc::selectByPercent(4))
	{
	mainpack = zMisc::randBetween(1,2);
	}
	if (zMisc::selectByPercent(2))
	{
	equippack = zMisc::randBetween(1,2);
	}
	}
	break;
	case Cmd::GOODNESS_1:
	{
	if (zMisc::selectByPercent(4))
	{
	mainpack = zMisc::randBetween(1,6);
	}
	if (zMisc::selectByPercent(2))
	{
	equippack = zMisc::randBetween(1,2);
	}
	}
	break;
	case Cmd::GOODNESS_2_1:
	{
	if (zMisc::selectByPercent(4))
	{
	mainpack = zMisc::randBetween(1,2);
	}
	}
	break;
	case Cmd::GOODNESS_3:
	{
	if (zMisc::selectByPercent(4))
	{
	mainpack = zMisc::randBetween(1,4);
	}
	}
	break;
	case Cmd::GOODNESS_4:
	{
	if (zMisc::selectByPercent(10))
	{
	mainpack = zMisc::randBetween(1,4);
	}
	if (zMisc::selectByPercent(4))
	{
	equippack = zMisc::randBetween(1,2);
	}
	}
	break;
	case Cmd::GOODNESS_5:
	{
	if (zMisc::selectByPercent(10))
	{
	mainpack = zMisc::randBetween(1,6);
	}
	if (zMisc::selectByPercent(10))
	{
	equippack = zMisc::randBetween(1,2);
	}
	}
	break;
	case Cmd::GOODNESS_6:
	{
	if (zMisc::selectByPercent(20))
	{
	mainpack = zMisc::randBetween(1,6);
	}
	if (zMisc::selectByPercent(10))
	{
	equippack = zMisc::randBetween(1,4);
	}
	}
	break;
	case Cmd::GOODNESS_7:
	{
	if (zMisc::selectByPercent(20))
	{
	mainpack = zMisc::randBetween(1,6);
	}
	if (zMisc::selectByPercent(20))
	{
	equippack = zMisc::randBetween(1,4);
	}
	}
	break;
	}
	}
	// */
	if (mainpack>0) {
		packs.execEvery(&packs.main,Type2Type<DropFromPack>(),DropFromPack::Param(&packs.main,mainpack,getPos()));
	}
	if (equippack > 0){
		packs.execEvery(&packs.equip,Type2Type<DropFromPack>(),DropFromPack::Param(&packs.equip,equippack,getPos()));
		if (packs.equip.needRecalc)
		{
			setupCharBase();
			Cmd::stMainUserDataUserCmd  userinfo;
			full_t_MainUserData(userinfo.data);
			sendCmdToMe(&userinfo,sizeof(userinfo));
			sendMeToNine();
		}

	}
#endif
#if 0
	//TODOBYLQY
	std::vector<zObject *> temp_vec;
	int begin = 0;
	std::set<zObject *>::iterator iter;
	if (mainpack > 0)
	{
		for(iter = packs.main.getAllset().begin(); iter != packs.main.getAllset().end() ; iter ++)
		{
			//TODO 其它不可掉落物品
			if ((*iter)->data.upgrade > 5 || (*iter)->data.bind || (*iter)->data.pos.y == Cmd::EQUIPCELLTYPE_PACKAGE || (*iter)->data.pos.y == Cmd::EQUIPCELLTYPE_MAKE || (*iter)->base->kind==ItemType_MASK  || (*iter)->base->kind==ItemType_Quest)
			{
				continue;
			}
			temp_vec.push_back(*iter);
		}
		if (mainpack < (int)temp_vec.size())
		{
			begin = zMisc::randBetween(0,temp_vec.size() - mainpack);
		}
		else
		{
			mainpack = temp_vec.size();
		}
		for(int i = begin; i < mainpack ;  i ++)
		{
			this->packs.moveObjectToScene(&*temp_vec[i],this->getPos());
		}
	}
	if (equippack > 0)
	{
		bool needRecalc = false;
		begin = 0;
		temp_vec.clear();
		for(iter = packs.equip.getAllset().begin(); iter != packs.equip.getAllset().end() ; iter ++)
		{
			//TODO 其它不可掉落物品
			if ((*iter)->data.upgrade > 5 || (*iter)->data.bind || (*iter)->data.pos.y == Cmd::EQUIPCELLTYPE_PACKAGE || (*iter)->data.pos.y == Cmd::EQUIPCELLTYPE_MAKE  || (*iter)->base->kind==ItemType_Quest)
			{
				continue;
			}
			temp_vec.push_back(*iter);
		}
		if (equippack < (int)temp_vec.size())
		{
			begin = zMisc::randBetween(0,temp_vec.size() - equippack);
		}
		else
		{
			equippack = temp_vec.size();
		}
		for(int i = begin; i < equippack ;  i ++)
		{
			this->packs.moveObjectToScene(&*temp_vec[i],this->getPos());
			needRecalc = true;
		}
		if (needRecalc)
		{
			setupCharBase();
			Cmd::stMainUserDataUserCmd  userinfo;
			full_t_MainUserData(userinfo.data);
			sendCmdToMe(&userinfo,sizeof(userinfo));
			sendMeToNine();
		}
	}
#endif
}

void SceneUser::showCurrentEffect(const WORD &state,bool isShow,bool notify)
{
	if (isShow)
	{
#ifdef _DEBUG
		Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"[设置第%u号特效状态]",state);
#endif
		if (this->setUState(state) && notify)
			this->setStateToNine(state);
	}
	else
	{
#ifdef _DEBUG
		Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"[清除第%u号特效状态]",state);
#endif
		if (this->clearUState(state) && notify)
			this->clearStateToNine(state);
	}

}

void SceneUser::reSendMyMapData()
{
	if (SceneEntry_Hide!=getState() && !this->hideme && !Soulflag)
	{//检查是否隐身
		BUFFER_CMD(Cmd::stAddUserAndPosMapScreenStateUserCmd,send,zSocket::MAX_USERDATASIZE);
		this->full_t_MapUserDataPosState(send->data);
		this->scene->sendCmdToNine(getPosI(),send,send->size(),dupIndex);
		this->setStateToNine(Cmd::USTATE_WAR);
	}
	else
	{
		BUFFER_CMD(Cmd::stAddUserMapScreenStateUserCmd,send,zSocket::MAX_USERDATASIZE);
		this->full_t_MapUserDataState(send->data);
		sendCmdToMe(send,send->size());
	}
}

/**
* \brief 让角色重生
*/
void SceneUser::relive()
{
	//relive(Cmd::ReliveHome,0,100);
}

/**
* \brief 角色被击退N格
*/
void SceneUser::standBack(const DWORD dwAttTempID,DWORD grids)
{
	SceneUser *att = SceneUserManager::getMe().getUserByTempID(dwAttTempID);
	if (att)
	{
		backOff(Scene::getCompDir(att->getPos(),this->pos),grids);
	}
	else
	{
		backOff(getDir(),grids);
	}
}

/**
* \brief 让角色死亡
*/
void SceneUser::toDie(const DWORD &dwTempID)
{
	//setDeathState();
}

void SceneUser::goToRandomScreen()
{
	this->goToRandomRect(this->pos,SCREEN_WIDTH*3,SCREEN_HEIGHT*3);
}

/**
* \brief 通知客户端生命值的变化
*/
void SceneUser::attackRTHpAndMp()
{
//	ScenePk::attackRTHpAndMp(this);
	notifyHMS = false;
}
DWORD SceneUser::autoRestitute(DWORD &updated)
{
#if 0
	/*
	if (this->live_skills.skill_id) {
	//劳动中
	return updated;
	}*/

	if (!this->isAutoRestitute)
	{
		return updated;
	}

	//this->lock();
	if (this->getState() == SceneUser::SceneEntry_Death) {
		//this->unlock();
		return updated;
	}
	/*
	Zebra::logger->debug("自动恢复hp(%d),mp(%d),sp(%d)",charstate.resumehp + packs.equip.getAllObject().hpr,
	charstate.resumemp + packs.equip.getAllObject().mpr,
	charstate.resumesp + packs.equip.getAllObject().spr
	);
	*/
	//Zebra::logger->debug("用户跑动(%d),走路(%d),run(%d),walk(%d)",step_state >> 8,step_state & 0x0ff,step_state & 0xff00,step_state & 0x00ff );

	if (!this->IsPking())  {

		if (this->isSitdown())
		{
			if (sitdownRestitute)
			{
				//sky 根据全局的恢复系数算出人物恢复率
				WORD rate = recoverRate * 0.01f;

				DWORD tmp = charbase.hp;
				DWORD value = (DWORD)(((float)charstate.maxhp)*rate)+charstate.resumehp*2;
				if (value<4) value = 4;
				charbase.hp += value;

				if (charbase.hp>charstate.maxhp) charbase.hp = charstate.maxhp;
				if (tmp != charbase.hp) updated |= 0x1;

				tmp = charbase.mp;
				value = (DWORD)(((float)charstate.maxmp)*rate)+charstate.resumemp*2;
				if (value<6) value=6;
				charbase.mp +=  value;
				if (charbase.mp>charstate.maxmp) charbase.mp = charstate.maxmp;
				if (tmp != charbase.mp) updated |= 0x02;
			}

			sitdownRestitute = !sitdownRestitute;
		}
		else
		{
			sitdownRestitute = false;
			DWORD tmp = charbase.hp;
			if (step_state & 0xff00) { //user run
				charbase.hp += (packs.equip.getEquips().get_hpr());
			}
			else if (step_state & 0x00ff) 
			{ //user walk
				//走路时生命恢复减半
				charbase.hp += (DWORD)((charstate.resumehp) >> 1);
			}
			else 
			{ //user rest
				charbase.hp += charstate.resumehp;
			}

			if (charbase.hp>charstate.maxhp) charbase.hp = charstate.maxhp;
			if (tmp != charbase.hp) updated |= 0x1;

			tmp = charbase.mp;
			charbase.mp +=  charstate.resumemp;
			if (charbase.mp>charstate.maxmp) charbase.mp = charstate.maxmp;
			if (tmp != charbase.mp) updated |= 0x02;
		}
	}

	DWORD tmp = charbase.sp;
	if (step_state & 0xff00) { //user run
		charbase.sp += (packs.equip.getEquips().get_spr() - RUN_CONSUME_SP);
		if ((int)charbase.sp <=1) charbase.sp =0;
	}else if (step_state & 0x00ff) { //user walk
		if (!this->IsPking()) charbase.sp +=  (DWORD)(/*charconst->resumesp*/ WALK_RESTITUTE_SP*(skillValue.spspeedup==0?1:(skillValue.spspeedup/100.0f)) + packs.equip.getEquips().get_spr());
	}else {
		if (!this->IsPking()) charbase.sp +=  (DWORD)(/*charconst->resumesp*/ REST_RESTITUTE_SP*(skillValue.spspeedup==0?1:(skillValue.spspeedup/100.0f)) + packs.equip.getEquips().get_spr());
	}
	if (charbase.sp>charstate.maxsp) charbase.sp = charstate.maxsp;
	if (tmp != charbase.sp) updated |= 0x04;

	step_state &= 0x0000;
	/*
	if (updated) {
	//notify me
	Cmd::stSetHPAndMPDataUserCmd ret;
	ret.wdHP = charbase.hp;
	ret.wdMP = charbase.mp;
	ret.wdSP = charbase.sp;
	this->sendCmdToMe(&ret,sizeof(ret));
	}
	if (updated & 0x01) {
	//notify other
	this->team.sendtoTeamCharData(this);
	}
	*/
	return updated;
	//this->unlock();
#endif
	return 0;
}
void SceneUser::restitute()
{
#if 0
	autoRestitute(updateNotify);
	leechdom.fresh(this,updateNotify);
	updateCount=(updateCount+1)%3;
	if (updateNotify&&updateCount==0) {
		//notify me
		Cmd::stSetHPAndMPDataUserCmd ret;
		ret.dwHP = charbase.hp;
		ret.dwMP = charbase.mp;
		//ret.dwSP = charbase.sp;
		this->sendCmdToMe(&ret,sizeof(ret));

		TeamManager * team = SceneManager::getInstance().GetMapTeam(TeamThisID);

		if(team)
			team->sendtoTeamCharData(this);

		if ((updateNotify & 0x01) || (updateNotify & 0x02)) {
			this->sendtoSelectedHpAndMp();
		}
		updateNotify = 0;
	}
#endif
}

/**
* \brief 判断角色是否死亡
* \return true为死亡
*/
bool SceneUser::isDie()
{
	if (this->getState() == SceneEntry_Death) return true;
	return false;
}


DWORD SceneUser::getLevel() const
{
	return charbase.level;
}

/**
* \brief 需要的职业类型,决定可以使用的技能类型
*/
bool SceneUser::needType(const DWORD &needtype)
{
	return charbase.type == needtype;
}
#if 0
/**
* \brief 需要的职业类型,决定可以使用的技能类型
*/
bool SceneUser::addSkillToMe(zSkill *skill)
{
	return usm.addSkill(skill);
}
#endif
/**
* \brief 是否有该技能需要的武器
* \return true 有 false 没有
*/
bool SceneUser::needWeapon(DWORD skillid)
{
#if 0
	bool bret = false;
	zObject *temp;

	zSkill *s = usm.findSkill(skillid);
	if (s)
	{
		if (packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDL))
		{
			if (temp)
			{
				if (0 != temp->data.dur)
				{
					WORD kind = temp->base->kind;
					bret = s->base->has_needweapon(kind);
				}
			}
		}
		if (!bret)
		{
			if (packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDR))
			{
				if (temp)
				{
					if (0 != temp->data.dur)
					{
						WORD kind = temp->base->kind;
						bret = s->base->has_needweapon(kind);
					}
				}
			}
			if (!bret)
			{
				bret = s->base->has_needweapon(65535);
			}
		}
	}
	return bret;
#endif
	return true;
}

/**
* \brief 获取武器提供的攻击力 
* \param powerkind 攻击力种类
0 最小物理攻击
1 最大物理攻击
2 最小法术攻击
3 最大法术攻击
* \return 攻击力
*/
SWORD SceneUser::getWeaponPower(int powerkind)
{
#if 0
	zObject *temp;
	SWORD power = 0;
	if (packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDL))
	{
		if (temp)
		{
			if (0 != temp->data.dur)
			{
				switch (powerkind)
				{
				case 0:
					power += temp->data.pdamage;
					break;
				case 1:
					power += temp->data.maxpdamage;
					break;
				case 2:
					power += temp->data.mdamage;
					break;
				case 3:
					power += temp->data.maxmdamage;
					break;
				}
			}
		}
	}

	if (packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDR))
	{
		if (temp)
		{
			if (0 != temp->data.dur)
			{
				switch (powerkind)
				{
				case 0:
					power += temp->data.pdamage;
					break;
				case 1:
					power += temp->data.maxpdamage;
					break;
				case 2:
					power += temp->data.mdamage;
					break;
				case 3:
					power += temp->data.maxmdamage;
					break;
				}
			}
		}
	}

	return power;
#endif
	return 0;
}

/**
* \brief 是否Pk区域
* \param other PK相关人
* \return true 是 false 否
*/
bool SceneUser::isPkZone(SceneEntryPk *other)
{
#if 0

#ifdef  _DEBUG 
	Channel::sendSys(tempid,Cmd::INFO_TYPE_GAME,"PK区域：%s",!(scene->checkZoneType(this->pos,ZoneTypeDef::ZONE_PK_SAFE) ||
		scene->checkZoneType(this->pos,ZoneTypeDef::ZONE_ABSOLUTE_SAFE))?"是":"不是PK区域");
#endif

	if (this->isSpecWar(Cmd::COUNTRY_FORMAL_DARE) && scene->checkZoneType(this->pos,ZoneTypeDef::ZONE_DARE_SAFE))
	{
		return false;
	}

	if (scene->checkZoneType(this->pos,ZoneTypeDef::ZONE_ABSOLUTE_SAFE))
	{
		/*  if (other)
		{
		SceneEntryPk *curEntry = other->getMaster();
		if (curEntry)
		{
		if (zSceneEntry::SceneEntry_Player == curEntry->getType())
		{
		SceneUser *pUser = (SceneUser *)curEntry;
		if (this->charbase.country == pUser->charbase.country && !isWar(pUser)) return false;
		}
		}
		}
		*/
		return false;
	}
	else if (scene->checkZoneType(this->pos,ZoneTypeDef::ZONE_PK_SAFE))
	{
		if (other)
		{
			SceneEntryPk *curEntry = other->getMaster();
			if (curEntry)
			{
				if (zSceneEntry::SceneEntry_Player == curEntry->getType())
				{
					SceneUser *pUser = (SceneUser *)curEntry;
					if (this->charbase.country == pUser->charbase.country && !isWar(pUser)) return false;
				}
			}
		}
		else
		{
			return false;
		}
	}

	return true;
	//  return !(scene->checkZoneType(this->pos,ZoneTypeDef::ZONE_PK_SAFE) ||
	//      scene->checkZoneType(this->pos,ZoneTypeDef::ZONE_ABSOLUTE_SAFE));
#endif
	return false;
}
/**
* \brief 依赖物品消耗型法术
* \param object 消耗物品的类型
* \param num 消耗物品的数量
* \return true 消耗成功 false 失败
*/
bool SceneUser::reduce(const DWORD &object,const BYTE num)
{
#if 0
	// [ranqd] 攻击消耗箭支物品不需要了
	if( object == BOW_ARROW_ITEM_TYPE ) return true;

	if (object >0)
	{
		if (!packs.equip.skillReduceObject(this,object,num))
		{
			//Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"缺少材料,不能使用此技能.");
			return false;
		}
	}
#endif
	return true;
}

/**
* \brief 检查可消耗物品是否足够
* \param object 消耗物品的类型
* \param num 消耗物品的数量
* \return true 足够 false 不够
*/
bool SceneUser::checkReduce(const DWORD &object,const BYTE num)
{
#if 0
	if (object>0)
	{
		if (!packs.equip.skillCheckReduceObject(this,object,num))
		{
			//Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"缺少材料,不能使用此技能.");
			return false;
		}
	}
#endif
	return true;
}
#if 0
/**
* \brief 施放技能所导致的消耗MP,HP,SP
* \param base 技能基本属性对象
* \return true 消耗成功 false 失败
*/
bool SceneUser::doSkillCost(const zSkillB *base)
{
	if (angelMode) return true;
	if ((int)charbase.mp - (int)base->mpcost>=0)
	{
		if ((int)charbase.sp - (int)base->spcost>=0)
		{
			if ((int)charbase.hp - (int)base->hpcost>0)
			{
				charbase.mp -= base->mpcost;
				//目前不处理体力所以注释掉charbase.sp -= base->spcost;
				charbase.hp -= base->hpcost;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}
	else
	{
		return false;
	}

	//检查是否有自动补魔道具
	checkAutoMP();
	ScenePk::attackRTHpAndMp(this);
	return true;
}
#endif
void SceneUser::checkAutoMP()
{
	if (charbase.mp <= charstate.maxmp*0.3)
	{
		DWORD temp=charstate.maxmp-charbase.mp + 49;
		DWORD dur=temp/50;
		DWORD reduce=packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN,ItemType_GreatLeechdomMp,dur,true,false);
		if (!reduce)
		{
			reduce=packs.equip.reduceDur(this,Cmd::EQUIPCELLTYPE_ADORN + 1,ItemType_GreatLeechdomMp,dur,true,false);
		}
		if (reduce)
		{
			charbase.mp += (reduce*50);
			if (charbase.mp > charstate.maxmp)
			{
				charbase.mp=charstate.maxmp;
			}
		}
	}
}
#if 0
/**
* \brief 检查施放技能所导致的消耗MP,HP,SP是否足够
* \param base 技能基本属性对象
* \return true 消耗成功 false 失败
*/
bool SceneUser::checkSkillCost(const zSkillB *base)
{
	if (angelMode) return true;
	if ((int)base->mpcost == 0 ||((int)charbase.mp - (int)base->mpcost>=0))
	{
		//    if ((int)charbase.sp - (int)base->spcost>=0)
		//    {
		if ((int)base->hpcost ==0 || ((int)charbase.hp - (int)base->hpcost>0))
		{
			return true;
		}
		else
		{
			return false;
		}
		//    }
		//    else
		//    {
		//      return false;
		//    }
	}
	else
	{
		return false;
	}
	return true;
}
#endif
/**
* \brief 检查自身的施放成功几率,决定这次技能是否可以施放
* \return true 成功 false 失败
*/
bool SceneUser::checkPercent()
{
	//法术不命中
	//  if (getMagicType())
	//  {
	/*  if (!zMisc::selectByPercent(charstate.attackrating+skillValue.atrating))
	{
	return false;
	}*/
	//  }
	//  else
	//  {
	//    if (!zMisc::selectByPercent(charstate.magicrating+skillValue.atrating))
	//    {
	//      return false;
	//    }
	//  }
	return true;
}

#if 0
void SceneUser::processMaskOnAttack(SceneEntryPk *pDef)
{
	mask.on_attack((SceneUser *)pDef);
}

void SceneUser::processMaskOnDefence()
{
	mask.on_defence();
}

void SceneUser::processAddDam(int &dwDam,int &dwDamDef,bool physics)
{
	//计算增加伤害率
	if (this->packs.equip.getEquips().get_damage()> 0)
	{
		// 对防御者的伤害+=伤害值*伤害率
		dwDamDef += (int)(dwDam * (this->packs.equip.getEquips().get_damage() / 100.0f));
	}
	if (this->packs.equip.getEquips().get_bdam()> 0)
	{
		// 对防御者的伤害+=伤害值*伤害率
		dwDamDef += (int)(dwDam * (this->packs.equip.getEquips().get_bdam() / 100.0f));
	}
#ifdef _DEBUG 
	Zebra::logger->debug("根据套装装备的伤害加深值计算出来的结果累加值dwDamDef:%ld",dwDamDef);
#endif
}

void SceneUser::reduceDam(int &dwDam,int &dwDamDef,bool physics)
{
	if (physics)
	{
		//计算减物理少伤害值
		if (this->packs.equip.getEquips().get_dhpp() > 0)
		{
			dwDamDef -= (int)(dwDam * (this->packs.equip.getEquips().get_dhpp() / 100.0f));
		}
#ifdef _DEBUG 
		Zebra::logger->debug("根据物理减少伤害值计算出来的结果累加值dwDamDef:%ld",dwDamDef);
#endif
		//计算减物理少伤害值
		if (this->packs.equip.getEquips().get_dpdam() > 0)
		{
			dwDamDef -= (int)(dwDam * (this->packs.equip.getEquips().get_dpdam() / 100.0f));
		}
#ifdef _DEBUG 
		Zebra::logger->debug("根据五行套装物理减少伤害值计算出来的结果累加值dwDamDef:%ld",dwDamDef);
#endif
	}
	else
	{
		//计算减法术少伤害值
		if (this->packs.equip.getEquips().get_dmpp() > 0)
		{
			dwDamDef -= (int)(dwDam * (this->packs.equip.getEquips().get_dmpp() / 100.0f));
		}
#ifdef _DEBUG 
		Zebra::logger->debug("根据法术减少伤害值计算出来的结果累加值dwDamDef:%ld",dwDamDef);
#endif
		//计算减法术少伤害值
		if (this->packs.equip.getEquips().get_dmdam() > 0)
		{
			dwDamDef -= (int)(dwDam * (this->packs.equip.getEquips().get_dmdam() / 100.0f));
		}
#ifdef _DEBUG 
		Zebra::logger->debug("根据五行套装法术减少伤害值计算出来的结果累加值dwDamDef:%ld",dwDamDef);
#endif
	}
}

//sky 计算伤害反射
void SceneUser::reflectDam(int &dwDamDef,int &dwDamSelf,DWORD skillID)
{
	//计算伤害反射
	// 计算被攻击者身上装备对伤害的反弹
	if (this->packs.equip.getEquips().get_rdam() > 0)
	{
		dwDamSelf += (int)(dwDamDef * (this->packs.equip.getEquips().get_rdam() / 100.0f));
	}

	zSkill *s = NULL;

	//sky 反弹魔法攻击伤害百分比
	if (this->skillValue.reflect2 > 0)
	{
		if(	!(skillID == SERVER_SKILL_ATTACK_NORMAL) &&
			!(skillID == SERVER_SKILL_DAGGER_ATTACK_NORMAL) &&
			!(skillID == SERVER_SKILL_DART_ATTACK_NORMAL) &&
			!(skillID == SERVER_SKILL_HANDS_ATTACK_NORMAL) )
		{
			s = zSkill::createTempSkill(this,skillID,1);

			if(s && s->IsMagicSkill() && !s->IsBuffSkill())
				dwDamSelf += (int)(dwDamDef * (this->skillValue.reflect2 / 100.0f ));
		}
		
	}

	//sky 反弹物理攻击伤害百分比
	if (this->skillValue.reflectp > 0)
	{
		if(	skillID == SERVER_SKILL_ATTACK_NORMAL ||
			skillID == SERVER_SKILL_DAGGER_ATTACK_NORMAL ||
			skillID == SERVER_SKILL_DART_ATTACK_NORMAL ||
			skillID == SERVER_SKILL_HANDS_ATTACK_NORMAL)
		{
			dwDamSelf += (int)(dwDamDef * (this->skillValue.reflectp / 100.0f ));
		}
		else
		{
			s = zSkill::createTempSkill(this,skillID,1);

			if(s && s->IsPhysicsSkill() && !s->IsBuffSkill())
				dwDamSelf += (int)(dwDamDef * (this->skillValue.reflectp / 100.0f ));
		}

	}

	//sky 反弹伤害固定值
	if (this->skillValue.reflect > 0)
	{
		dwDamSelf += this->skillValue.reflect;
	}

	/*
	sky 这里只反弹伤害,免伤的计算在其他地方进行
	if (dwDamDef - dwDamSelf >=0)
	{
		dwDamDef -=dwDamSelf;
	}
	else
	{
		dwDamDef = 0;
	}*/
}

//sky 反射技能
void SceneUser::reflectSkill(SceneEntryPk *pAtt,const Cmd::stAttackMagicUserCmd *rev)
{
	Cmd::stAttackMagicUserCmd cmd;

	//zSkill::createTempSkill(this,rev->wdMagicType,skillValue.reflect_ardor);

	if (pAtt && skillValue.MagicReflex>0)
	{
		zSkill *s = NULL;

		s = pAtt->usm.findSkill(rev->wdMagicType);

		if(s && s->IsMagicSkill())
		{
			memcpy(&cmd,rev,sizeof(cmd),sizeof(cmd));

			switch (pAtt->getType())
			{
			case zSceneEntry::SceneEntry_Player:
				{
					cmd.byAttackType = Cmd::ATTACKTYPE_U2U;
				}
				break;
			case zSceneEntry::SceneEntry_NPC:
				{
					cmd.byAttackType = Cmd::ATTACKTYPE_U2N;
				}
				break;
			default:
				{
					cmd.byAttackType = Cmd::ATTACKTYPE_U2U;
				}
				break;
			}

			cmd.dwDefenceTempID = pAtt->tempid;
			cmd.dwUserTempID = this->tempid;
			cmd.wdMagicType = rev->wdMagicType;
			cmd.byAction = Cmd::Ani_Run;
			cmd.xDes = pAtt->getPos().x;
			cmd.yDes = pAtt->getPos().y;
			cmd.byDirect = getDir();

			if (s)
			{
				zSkill * useSkill = NULL;
				useSkill = zSkill::createTempSkill(this,s->data.skillid,s->data.level);

				if(useSkill)
				{
					useSkill->action(&cmd,sizeof(cmd));
					SAFE_DELETE(useSkill);
				}
			}
		}
	}
}
#endif

bool SceneUser::processDeath(SceneEntryPk *pAtt)
{
#if 0
	if (charbase.hp ==0 && getState()!=zSceneEntry::SceneEntry_Death)
	{
		SceneEntryPk * m = pAtt->getTopMaster();
		if (!m) return false;

		if (m->getType() == zSceneEntry::SceneEntry_Player)
		{
			ScenePk::attackDeathUser((SceneUser*)m,this);
		}
		else if (m->getType() == zSceneEntry::SceneEntry_NPC)
		{
			Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你被%s杀死了.",m->name);
			//lostObject(NULL);	//Shx Delete 任何情况下玩家死亡不丢装备;
			setDeathState();
			leaveBattle();
		}

		//sky 玩家死亡把战斗状态设置为NULL状态
		switch(IsPveOrPvp())
		{
		case USE_FIGHT_PVE:
			Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你已经退PVE模式");
			break;
		case USE_FIGHT_PVP:
			Channel::sendSys(this, Cmd::INFO_TYPE_GAME, "你已经退PVP模式");
		}

		SetPkTime(0);	//sky 把战斗时间也设置为0
		SetPveOrPvp(USE_FIGHT_NULL);

		if (this->diewithme >0 && pAtt)
		{
			if (zMisc::selectByPercent(this->diewithme))
			{
				pAtt->toDie(0);
				this->skillStatusM.clearSkill(345);//清除血债血偿
			}
		}
		if (this->switchdie >0)
		{
			if (zMisc::selectByPercent(this->switchdie))
			{
				zPosVector range;
				this->scene->findEntryPosInOne(this->getPos(),this->getPosI(),range);
				if (range.size()==0) return true;
				int num = zMisc::randBetween(0,range.size()-1);
				zPos pd=range[num];
				SceneUser *pUser = this->scene->getSceneUserByPos(pd);
				if (pUser&&pUser!=this)
				{
					Channel::sendSys(pUser,Cmd::INFO_TYPE_GAME,"你被转移的死亡状态击中");
					pUser->toDie(0);
					return true;
				}
				SceneNpc *pNpc = this->scene->getSceneNpcByPos(pd);
				if (pNpc)
				{
					pNpc->toDie(0);
				}
			}
		}

		//sky 如果是在战场用就要特殊处理下
		if(this->scene->IsGangScene())
		{
			((GangScene*)scene)->UserDeathRun(this->tempid, pAtt->tempid);
		}
		return true;
	}
#endif
	return false;
}

#if 0
void SceneUser::hp2mp(int &dwDamDef)
{
	int dwMP =0;
	if (this->charbase.mp > 0 && this->packs.equip.getEquips().get_hptomp() > 0)
	{
		dwMP = (int)(dwDamDef * (this->packs.equip.getEquips().get_hptomp()  / 100.0f));
		if ((int)this->charbase.mp > dwMP)
		{
			dwDamDef -= dwMP;
			this->charbase.mp -= dwMP;
		}
		else
		{
			dwDamDef -= this->charbase.mp;
			this->charbase.mp = 0;
		}
		this->checkAutoMP();
	}

}
#endif

/**
* \brief 获取自己的主人,一般针对NPC而言,Player的主人是自己
* \return NULL或者主人的对象指针
*/
SceneEntryPk *SceneUser::getMaster()
{
	return this;
}

SceneEntryPk *SceneUser::getTopMaster()
{
	return this;
}

#if 0
bool SceneUser::packsaddObject(zObject *srcObj,bool needFind,bool from_record,bool calcflag)
{
	bool ret;
	stObjectLocation old = srcObj->data.pos;

	//if (needFind)
	//{
	//  srcObj->data.pos.dwLocation=Cmd::OBJECTCELLTYPE_COMMON;
	//  srcObj->data.pos.dwTableID=0;
	//}
	ret = packs.addObject(srcObj,needFind,from_record);

#if 0
	//TODOBYLQY
	//try left package
	if (!ret && needFind &&  /*srcObj->data.pos.dwLocation == Cmd::OBJECTCELLTYPE_COMMON &&*/ packs.equip.left) {
		srcObj->data.pos.dwLocation = Cmd::OBJECTCELLTYPE_PACKAGE;
		srcObj->data.pos.dwTableID = packs.equip.left->object()->data.qwThisID;
		ret = packs.addObject(srcObj,needFind,from_record);
	}

	//try right package
	if (!ret && needFind && /*srcObj->data.pos.dwLocation == Cmd::OBJECTCELLTYPE_PACKAGE &&*/ packs.equip.right) {
		srcObj->data.pos.dwLocation = Cmd::OBJECTCELLTYPE_PACKAGE;
		srcObj->data.pos.dwTableID = packs.equip.right->object()->data.qwThisID;
		ret = packs.addObject(srcObj,needFind,from_record);
	}
#endif

	if (!ret) {
		srcObj->data.pos = old;
		Zebra::logger->warn("警告,用户(%s)添加物品(%s)失败.",name,srcObj->name);
	}
	/*
	//检查是否马匹
	if (ItemType_HORSE==srcObj->base->kind)
	{
	if (horse.horse())
	{
	horse.mount(false);
	horse.putAway();
	horse.horse(0);
	}
	horse.horse(srcObj->base->id);
	}
	*/
	//  recalcBySword(calcflag);
	return ret;
}
#endif

void SceneUser::sendExpToSept(const WORD &exp)
{
	if (charbase.septid !=0)
	{
		Cmd::Session::t_distributeSeptExp_SceneSession send;
		send.dwExp = (DWORD)(exp*(0.02f));
		if (send.dwExp == 0) send.dwExp=1;
		send.dwUserID = id;
		sessionClient->sendCmd(&send,sizeof(send));
	}
}

/**
* \brief 广播最新属性
*/
void SceneUser::changeAndRefreshHMS(bool lock,bool sendData)
{
	//Zebra::logger->debug("sendData=%d,reSendOther=%d,getMyMoveSpeed()=%d",sendData,reSendOther,getMyMoveSpeed());
	setupCharBase(lock);
	if (sendData)
	{
		Cmd::stMainUserDataUserCmd  userinfo;
		full_t_MainUserData(userinfo.data);
		sendCmdToMe(&userinfo,sizeof(userinfo));
		sendInitHPAndMp();
		if (reSendOther)
		{
			sendMeToNine();
		}
	}
	this->reSendData = false;
	this->reSendOther=false; 
}

int SceneUser::saveSysSetting(BYTE * buf)
{
	int size = 0;
	sysSetting[0] = pkMode; // 将PK模式保存回系统设置
	bcopy(sysSetting,buf+size,sizeof(sysSetting));
	size += sizeof(sysSetting);
	bcopy(chatColor,buf+size,sizeof(chatColor));
	size += sizeof(chatColor);

	return size;
}

int SceneUser::loadSysSetting( BYTE * buf)
{
	int size = 0;
	bcopy(buf,sysSetting,sizeof(sysSetting));
	pkMode = sysSetting[0]; // 读取PK模式

	//sky 测试战斗模式
	pkMode = 1;

	size += sizeof(sysSetting);
	bcopy(buf+size,&chatColor[0],sizeof(chatColor));
	size += sizeof(chatColor);

	//通知客户端
	Cmd::stSystemSettingsUserCmd sendClient;
	bcopy(sysSetting,&sendClient.data.bySettings,sizeof(sendClient.data.bySettings));
	bcopy(chatColor,&sendClient.data.dwChatColor,sizeof(sendClient.data.dwChatColor));
	sendCmdToMe(&sendClient,sizeof(sendClient));

	//通知session
	Cmd::Session::t_sysSetting_SceneSession send;
	strncpy((char *)send.name,name,MAX_NAMESIZE-1);
	bcopy(sysSetting,&send.sysSetting,sizeof(send.sysSetting));
	send.face = charbase.face;
	sessionClient->sendCmd(&send,sizeof(send));

	//通知网关
	Cmd::Scene::t_sysSetting_GateScene gate_send;
	bcopy(sysSetting,gate_send.sysSetting,sizeof(gate_send.sysSetting));
	gate_send.id=this->id;
	this->gatetask->sendCmd(&gate_send,sizeof(gate_send));

	return size;
}

void SceneUser::sendSevenStateToMe(DWORD state,WORD value,WORD time)
{
	using namespace Cmd;
	char Buf[200]; 
	bzero(Buf,sizeof(Buf));
	stSelectReturnStatesPropertyUserCmd *srs=(stSelectReturnStatesPropertyUserCmd*)Buf;
	constructInPlace(srs);
	srs->byType = MAPDATATYPE_USER;
	srs->dwTempID = this->tempid;
	srs->states[0].state = state;
	srs->states[0].result = value;
	srs->states[0].time = time;
	srs->size=1;
	this->sendCmdToMe(srs,sizeof(stSelectReturnStatesPropertyUserCmd) + sizeof(srs->states[0]));
}

#if 0
void SceneUser::sendtoSelectedState(DWORD state,WORD value,WORD time)
{
	using namespace Cmd;
	char Buf[200]; 
	bzero(Buf,sizeof(Buf));
	stSelectReturnStatesPropertyUserCmd *srs=(stSelectReturnStatesPropertyUserCmd*)Buf;
	constructInPlace(srs);
	srs->byType = MAPDATATYPE_USER;
	srs->dwTempID = this->tempid;
	srs->states[0].state = state;
	srs->states[0].result = value;
	srs->states[0].time = time;
	srs->size=1;
	TeamManager * team = SceneManager::getInstance().GetMapTeam(TeamThisID);

	if(team)
		team->sendCmdToTeam(this,srs,sizeof(stSelectReturnStatesPropertyUserCmd) + sizeof(srs->states[0]));

	SelectedSet_iterator iter = selected.begin();
	for(; iter != selected.end() ;)
	{
		SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(*iter);
		if (pUser)
		{
			if (this->scene->checkTwoPosIInNine(this->getPosI(),pUser->getPosI()))
			{
				pUser->sendCmdToMe(srs,sizeof(stSelectReturnStatesPropertyUserCmd) + sizeof(srs->states[0]));
				iter ++ ;
				continue;
			}
		}
		SelectedSet_iterator iter_del = iter;
		iter_del ++;
		selected.erase(iter);
		iter = iter_del;
	}
	//selected_lock.unlock();
}
//---------------------------------------------------------------------
void SceneUser::sendtoSelectedReliveWeakStateToUser(SceneUser *pUser)
{
	if (!pUser || this->charbase.reliveWeakTime == 0  || this->charbase.reliveWeakTime <= SceneTimeTick::currentTime.sec()%10000)
	{
		return;
	}
	char Buf[200]; 
	bzero(Buf,sizeof(Buf));
	Cmd::stSelectReturnStatesPropertyUserCmd *srs=(Cmd::stSelectReturnStatesPropertyUserCmd*)Buf;
	constructInPlace(srs);
	srs->byType = Cmd::MAPDATATYPE_USER;
	srs->dwTempID = this->tempid;
	srs->size=1;
	srs->states[0].state = Cmd::USTATE_RELIVEWEAK;
	srs->states[0].result=0;
	srs->states[0].time=this->charbase.reliveWeakTime-SceneTimeTick::currentTime.sec()%10000;
	pUser->sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + 
		sizeof(srs->states[0]) * srs->size);
}
void SceneUser::sendtoSelectedReliveWeakState()
{
	if (this->charbase.reliveWeakTime == 0 || this->charbase.reliveWeakTime <= SceneTimeTick::currentTime.sec()%10000)
	{
		return;
	}
	char Buf[200]; 
	bzero(Buf,sizeof(Buf));
	Cmd::stSelectReturnStatesPropertyUserCmd *srs=(Cmd::stSelectReturnStatesPropertyUserCmd*)Buf;
	constructInPlace(srs);
	srs->byType = Cmd::MAPDATATYPE_USER;
	srs->dwTempID = this->tempid;
	srs->size=1;
	srs->states[0].state = Cmd::USTATE_RELIVEWEAK;
	srs->states[0].result=0;
	srs->states[0].time= this->charbase.reliveWeakTime-SceneTimeTick::currentTime.sec()%10000;
	this->sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + 
		sizeof(srs->states[0]) * srs->size);
	//selected_lock.lock();
	SelectedSet_iterator iter = selected.begin();
	for(; iter != selected.end() ;)
	{
		SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(*iter);
		if (pUser)
		{
			if (this->scene->checkTwoPosIInNine(this->getPosI(),pUser->getPosI()))
			{
				pUser->sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + 
					sizeof(srs->states[0]) * srs->size);
				iter ++ ;
				continue;
			}
		}
		SelectedSet_iterator iter_del = iter;
		iter_del ++;
		selected.erase(iter);
		iter = iter_del;
	}
	//selected_lock.unlock();
}

//---------------------------------------------------------------------
void SceneUser::sendtoSelectedPkAdditionStateToUser(SceneUser *pUser)
{
	if (!pUser || this->charbase.pkaddition <= 1800)
	{
		return;
	}
	char Buf[200]; 
	bzero(Buf,sizeof(Buf));
	Cmd::stSelectReturnStatesPropertyUserCmd *srs=(Cmd::stSelectReturnStatesPropertyUserCmd*)Buf;
	constructInPlace(srs);
	srs->byType = Cmd::MAPDATATYPE_USER;
	srs->dwTempID = this->tempid;
	srs->size=1;
	srs->states[0].state = Cmd::USTATE_PK;
	srs->states[0].result=this->getPkAddition();
	srs->states[0].time=0XFFFF;
	pUser->sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + 
		sizeof(srs->states[0]) * srs->size);
}
void SceneUser::sendtoSelectedPkAdditionState()
{
	if (this->charbase.pkaddition <= 1800)
	{
		return;
	}
	char Buf[200]; 
	bzero(Buf,sizeof(Buf));
	Cmd::stSelectReturnStatesPropertyUserCmd *srs=(Cmd::stSelectReturnStatesPropertyUserCmd*)Buf;
	constructInPlace(srs);
	srs->byType = Cmd::MAPDATATYPE_USER;
	srs->dwTempID = this->tempid;
	srs->size=1;
	srs->states[0].state = Cmd::USTATE_PK;
	srs->states[0].result=this->getPkAddition();
	srs->states[0].time=0XFFFF;
	this->sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + 
		sizeof(srs->states[0]) * srs->size);
	//selected_lock.lock();
	SelectedSet_iterator iter = selected.begin();
	for(; iter != selected.end() ;)
	{
		SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(*iter);
		if (pUser)
		{
			if (this->scene->checkTwoPosIInNine(this->getPosI(),pUser->getPosI()))
			{
				pUser->sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + 
					sizeof(srs->states[0]) * srs->size);
				iter ++ ;
				continue;
			}
		}
		SelectedSet_iterator iter_del = iter;
		iter_del ++;
		selected.erase(iter);
		iter = iter_del;
	}
	//selected_lock.unlock();
}

//---------------------------------------------------------------------
void SceneUser::sendtoSelectedTrainStateToUser(SceneUser *pUser)
{
	if (!pUser || !this->charbase.trainTime) return;

	char Buf[200]; 
	bzero(Buf,sizeof(Buf));
	Cmd::stSelectReturnStatesPropertyUserCmd *srs=(Cmd::stSelectReturnStatesPropertyUserCmd*)Buf;
	constructInPlace(srs);
	srs->byType = Cmd::MAPDATATYPE_USER;
	srs->dwTempID = this->tempid;
	srs->size=1;
	srs->states[0].state = Cmd::USTATE_DAOJISHI;
	srs->states[0].result=charbase.trainTime/60;
	srs->states[0].time=charbase.trainTime/60;
	pUser->sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + 
		sizeof(srs->states[0]) * srs->size);
}
void SceneUser::sendtoSelectedTrainState()
{
	if (!this->charbase.trainTime) return;

	char Buf[200]; 
	bzero(Buf,sizeof(Buf));
	Cmd::stSelectReturnStatesPropertyUserCmd *srs=(Cmd::stSelectReturnStatesPropertyUserCmd*)Buf;
	constructInPlace(srs);
	srs->byType = Cmd::MAPDATATYPE_USER;
	srs->dwTempID = this->tempid;
	srs->size=1;
	srs->states[0].state = Cmd::USTATE_DAOJISHI;
	srs->states[0].result=charbase.trainTime/60;
	srs->states[0].time=charbase.trainTime/60;
	this->sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + 
		sizeof(srs->states[0]) * srs->size);
	//selected_lock.lock();
	SelectedSet_iterator iter = selected.begin();
	for(; iter != selected.end() ;)
	{
		SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(*iter);
		if (pUser)
		{
			if (this->scene->checkTwoPosIInNine(this->getPosI(),pUser->getPosI()))
			{
				pUser->sendCmdToMe(srs,sizeof(Cmd::stSelectReturnStatesPropertyUserCmd) + 
					sizeof(srs->states[0]) * srs->size);
				iter ++ ;
				continue;
			}
		}
		SelectedSet_iterator iter_del = iter;
		iter_del ++;
		selected.erase(iter);
		iter = iter_del;
	}
	//selected_lock.unlock();
}
void SceneUser::sendtoSelectedHpAndMp()
{
	Cmd::stRTSelectedHpMpPropertyUserCmd ret;
	ret.byType = Cmd::MAPDATATYPE_USER;
	ret.dwTempID = this->tempid;//临时编号
	ret.dwHP = this->charbase.hp;//当前血
	ret.dwMaxHp = this->charstate.maxhp;//最大hp
	ret.dwMP = this->charbase.mp;//当前mp
	ret.dwMaxMp = this->charstate.maxmp;//最大mp
	//selected_lock.lock();
	SelectedSet_iterator iter = selected.begin();
	for(; iter != selected.end() ;)
	{
		SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(*iter);
		if (pUser)
		{
			if (this->scene->checkTwoPosIInNine(this->getPosI(),pUser->getPosI()))
			{
				pUser->sendCmdToMe(&ret,sizeof(ret));
				iter ++ ;
				continue;
			}
		}
		SelectedSet_iterator iter_del = iter;
		iter_del ++;
		selected.erase(iter);
		iter = iter_del;
	}
	//selected_lock.unlock();
}

/**
* \brief 获取装备伤害加成
* \return 伤害加成
*/
WORD SceneUser::getDamageBonus()
{
    return 0;
//	return packs.equip.getEquips().get_damagebonus();
}

/**
* \brief 检查武器类型是否匹配
* \param weaponType 武器类型
* \return true武器类型符合,false武器类型不符合
*/
bool SceneUser::checkWeapon(BYTE weaponType)
{
	bool bret = false;
	zObject *temp;
	if (packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDL))
	{
		if (temp)
		{
			bret = (weaponType == temp->base->kind);
		}
	}
	if (!bret)
	{
		if (packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDR))
		{
			if (temp)
			{
				bret = (weaponType == temp->base->kind);
			}
		}
	}
	return bret;
}

void SceneUser::leaveTeam()
{
	TeamManager * team = SceneManager::getInstance().GetMapTeam(TeamThisID);

	if (team)
	{
		if (team->getLeader() == this->tempid)
		{//如果自己是队长则跟换队长T自己出队
			Cmd::stRemoveTeamMemberUserCmd cmd;
			cmd.dwTeamID = team->getTeamtempId();
			strncpy(cmd.pstrName, this->name, MAX_NAMESIZE);

			if(team->changeLeader())
			{
				SceneUser * leader = SceneUserManager::getMe().getUserByTempID(team->getLeader());
				this->TeamThisID = 0;
				team->removeMember((Cmd::stRemoveTeamMemberUserCmd*)&cmd );

				if (team->getSize() <= 1)
				{
					SceneManager::getInstance().SceneDelTeam(team->getTeamtempId());
				}
			}
		}
		else
		{
			Cmd::stRemoveTeamMemberUserCmd cmd;
			cmd.dwTeamID = team->getTeamtempId();
			strncpy(cmd.pstrName ,this->name, MAX_NAMESIZE);

			SceneUser *leader=SceneUserManager::getMe().getUserByTempID(team->getLeader());

			if (leader)
			{
				this->TeamThisID = 0;
				team->removeMember((Cmd::stRemoveTeamMemberUserCmd*)&cmd);

				if (team->getSize() <= 1)
				{
					SceneManager::getInstance().SceneDelTeam(team->getTeamtempId());
				}
			}
		}
	}
}

/**
* \brief 获得武器类型
* \return 武器类型
*/
BYTE SceneUser::getWeaponType()
{
	BYTE weaponType = 0;
	zObject *temp;
	if (packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDL))
	{
		if (temp)
		{
			weaponType = temp->base->kind;
		}
		if (weaponType <104 || weaponType >111)
		{
			weaponType = 0;
		}
	}
	if (0 == weaponType)
	{
		if (packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDR))
		{
			if (temp)
			{
				weaponType = temp->base->kind;
			}
		}
	}
	return weaponType;
}

void SceneUser::setPetsChaseTarget(SceneEntryPk *entry)
{
#ifdef _DEBUG
	//Zebra::logger->debug("SceneUser::setPetsChaseTarget(): lock %s",name);
#endif
	for (std::list<ScenePet *>::iterator it=totems.begin(); it!=totems.end(); it++)
	{
		(*it)->setCurTarget(entry->tempid,entry->getType(),true);
	}
	if (pet) pet->setCurTarget(entry->tempid,entry->getType(),true);
	if (summon) summon->setCurTarget(entry->tempid,entry->getType(),true);
#ifdef _DEBUG
	//Zebra::logger->debug("SceneUser::setPetsChaseTarget(): unlock %s",name);
#endif
}

#endif

/**
* \brief 获取对象实体
* \param entryType 目标类型
* \param entryID 目标类型
* \return 对象实体或NULL
*/
SceneEntryPk* SceneUser::getSceneEntryPk(DWORD entryType,DWORD entryID) const
{  
	switch (entryType)  
	{    
	case zSceneEntry::SceneEntry_Player:      
		{        
			return SceneUserManager::getMe().getUserByTempID(entryID);      
		}      
		break;    
	//case zSceneEntry::SceneEntry_NPC:
	//	{
	//		return SceneNpcManager::getMe().getNpcByTempID(entryID);
	//	}
	//	break;
	default:
		{
			return NULL;
		}
		break;
	}
	return NULL;
}

#if 0
/**
* \brief 获取抗毒增加  
*/
SWORD SceneUser::getPoisondef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_poisondef());//-(temp?temp->getPoison():0));
}

/**
* \brief 获取抗麻痹增加        
*/
SWORD SceneUser::getLulldef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_lulldef());//-(temp?temp->getLull():0));
}

/**
* \brief 获取抗眩晕增加        
*/
SWORD SceneUser::getReeldef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_reeldef());//-(temp?temp->getReel():0));
}

/**
* \brief 获取抗噬魔增加        
*/
SWORD SceneUser::getEvildef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_evildef());//-(temp?temp->getEvil():0));
}

/**
* \brief 获取抗噬力增加        
*/
SWORD SceneUser::getBitedef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_bitedef());//-(temp?temp->getBite():0));
}

/**
* \brief 获取抗混乱增加        
*/
SWORD SceneUser::getChaosdef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_chaosdef());//-(temp?temp->getChaos():0));
}

/**
* \brief 获取抗冰冻增加        
*/
SWORD SceneUser::getColddef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_colddef());//-(temp?temp->getCold():0));
}

/**
* \brief 获取抗石化增加        
*/
SWORD SceneUser::getPetrifydef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_petrifydef());//-(temp?temp->getPetrify():0));
}

/**
* \brief 获取抗失明增加        
*/
SWORD SceneUser::getBlinddef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_blinddef());//-(temp?temp->getBlind():0));
}

/**
* \brief 获取抗定身增加        
*/
SWORD SceneUser::getStabledef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_stabledef());//-(temp?temp->getStable():0));
}

/**
* \brief 获取抗减速增加        
*/
SWORD SceneUser::getSlowdef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_slowdef());//-(temp?temp->getSlow():0));
}

/**
* \brief 获取抗诱惑增加
*/
SWORD SceneUser::getLuredef()
{
	//SceneEntryPk *temp = getSceneEntryPk(curMagicManType,curMagicManID);
	return (SWORD)(packs.equip.getEquips().get_luredef());//-(temp?temp->getLure():0));
}

/**
* \brief 获取毒增加  
*/
SWORD SceneUser::getPoison()
{
	return (SWORD)packs.equip.getEquips().get_poison();
}

/**
* \brief 获取麻痹增加        
*/
SWORD SceneUser::getLull()
{
	return (SWORD)packs.equip.getEquips().get_lull();
}

/**
* \brief 获取眩晕增加        
*/
SWORD SceneUser::getReel()
{
	return (SWORD)packs.equip.getEquips().get_reel();
}

/**
* \brief 获取噬魔增加        
*/
SWORD SceneUser::getEvil()
{
	return (SWORD)packs.equip.getEquips().get_evil();
}

/**
* \brief 获取噬力增加        
*/
SWORD SceneUser::getBite()
{
	return (SWORD)packs.equip.getEquips().get_bite();
}

/**
* \brief 获取混乱增加        
*/
SWORD SceneUser::getChaos()
{
	return (SWORD)packs.equip.getEquips().get_chaos();
}

/**
* \brief 获取冰冻增加        
*/
SWORD SceneUser::getCold()
{
	return (SWORD)packs.equip.getEquips().get_cold();
}

/**
* \brief 获取石化增加        
*/
SWORD SceneUser::getPetrify()
{
	return (SWORD)packs.equip.getEquips().get_petrify();
}

/**
* \brief 获取失明增加        
*/
SWORD SceneUser::getBlind()
{
	return (SWORD)packs.equip.getEquips().get_blind();
}

/**
* \brief 获取定身增加        
*/
SWORD SceneUser::getStable()
{
	return (SWORD)packs.equip.getEquips().get_stable();
}

/**
* \brief 获取减速增加        
*/
SWORD SceneUser::getSlow()
{
	return (SWORD)packs.equip.getEquips().get_slow();
}

/**
* \brief 获取诱惑增加
*/
SWORD SceneUser::getLure()
{
	return (SWORD)packs.equip.getEquips().get_lure();
}
#endif

/**
* \brief 设置回收用户内存时间
* \return 时间回收内存时间
* */
zRTime& SceneUser::waitRecycle()
{
	zRTime ct;
	recycle_delay = ct;
	recycle_delay.addDelay(3000);
	return recycle_delay;
}

#if 0
/**
* \brief 易容处理
* \param cmd 易容消息
* \param cmdLen 消息长度
* \return true 处理成功 false 失败
**/
bool SceneUser::changeFace(const Cmd::stChangeFaceMapScreenUserCmd *cmd,const DWORD cmdLen)
{
	dwChangeFaceID = cmd->dwChangeFaceID;
	return scene->sendCmdToNine(getPosI(),cmd,cmdLen,dupIndex);
}

/**
* \brief 执行拍卖相关的指令
*
*
* \param rev 消息指针
* \param cmdLen 指令长度
* \return 是否执行成功
*/
bool SceneUser::doAuctionCmd(const Cmd::stAuctionUserCmd *cmd,DWORD cmdLen)
{
	//禁止拍卖行
	//  Channel::sendSys(this, Cmd::INFO_TYPE_FAIL, "拍卖功系统正在开发中！");
	//  return true;

	//#if 0
	if (!(atoi(Zebra::global["service_flag"].c_str())&Cmd::Session::SERVICE_AUCTION))
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对不起,拍卖功能暂时关闭,请留意官方公告");
		return true;
	}
	using namespace Cmd;
	using namespace Cmd::Session;
	using namespace Cmd::Record;

	/*
	//检查是不是在访问拍卖npc
	SceneNpc * npc = SceneNpcManager::getMe().getNpcByTempID(npc_dwNpcTempID);
	if (!npc)
	{
	Zebra::logger->info("[拍卖]%s 非法拍卖 npcID=%u npcTempID=%u",name,npc_dwNpcDataID,npc_dwNpcTempID);
	return false;
	}
	if (scene != npc->scene)
	{
	Zebra::logger->info("[拍卖]%s 非法拍卖,不在同一场景",name);
	return false;
	}
	if (!scene->zPosShortRange(npc->getPos(),getPos(),SCREEN_WIDTH,SCREEN_HEIGHT))
	{
	Zebra::logger->info("[拍卖]%s 非法拍卖,距离太远",name);
	return false;
	}
	*/
	/*
	if (npc->npc->kind!=NPC_TYPE_MAILBOX)
	{
	Zebra::logger->info("[拍卖]%s 非法拍卖,npc类型错误",name);
	return false;
	}
	*/

	switch (cmd->byParam)
	{
	case SALE_AUCTION_PARA:
		{
			if (charbase.level<40)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对不起,40级以上才可以拍卖物品");
				return true;
			}

			stSaleAuction * rev = (stSaleAuction *)cmd;
#ifndef _DEBUG
			if (rev->bidType!=0) return true;//暂时关闭金币拍卖

			rev->minGold = 0;//暂时关闭金币拍卖
			rev->maxGold = 0;//暂时关闭金币拍卖
#endif

			if ((rev->bidType!=0 && rev->bidType!=1)
				|| (rev->minMoney<1 && rev->minGold<1)
				|| rev->minMoney>10000000
				//|| rev->maxMoney>10000000
				//|| (0==rev->bidType && rev->minMoney>rev->maxMoney)
				|| (1==rev->bidType && rev->minGold>rev->maxGold)
				|| rev->itemID == 0
				|| rev->itemID == 0xffffffff)
			{
				Zebra::logger->warn("[拍卖]%s(%ld)非法的拍卖信息",name,id);
				return true;
			}
			t_saleAuction_SceneSession sa;
			sa.userID = tempid;

			zObject* ob = packs.uom.getObjectByThisID(rev->itemID);
			if (!ob)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你的包裹里没有该物品");
				Zebra::logger->warn("[拍卖]%s(%ld)试图拍卖的物品不存在",name,id);
				return true;
			}
			if (!ob->canMail())
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你不能拍卖这件物品");
				return true;
			}

			Zebra::logger->error("[拍卖]%s非法的拍卖信息",ob->data.strName,ob->data.dwObjectID);

			switch (ob->base->kind)
			{
			case ItemType_Blade:
			case ItemType_Sword:
				sa.info.type = AUCTION_WEAPOM_SWORD;
				break;
			case ItemType_Axe:
			case ItemType_Hammer:
				sa.info.type = AUCTION_WEAPOM_AXE;
				break;
			case ItemType_Crossbow:
				sa.info.type = AUCTION_WEAPOM_BOW;
				break;
			case ItemType_Stick:
				sa.info.type = AUCTION_WEAPOM_STICK;
				break;
			case ItemType_Staff:
				sa.info.type = AUCTION_WEAPOM_WAND;
				break;
			case ItemType_Fan:
				sa.info.type = AUCTION_WEAPOM_FAN;
				break;
				/*sky 新增板和皮类型防具支持*/
			case ItemType_Helm:
			case ItemType_Helm_Paper:
			case ItemType_Helm_Plate:
				sa.info.type = AUCTION_EQUIP_HEAD;
				break;
			case ItemType_ClothBody:
			case ItemType_FellBody:
			case ItemType_MetalBody:
				sa.info.type = AUCTION_EQUIP_BODY;
				break;
			case ItemType_Cuff:
			case ItemType_Cuff_Paper:
			case ItemType_Cuff_Plate:
				sa.info.type = AUCTION_EQUIP_WRIST;
				break;
			case ItemType_Shield:
				sa.info.type = AUCTION_EQUIP_SHIELD;
				break;
			case ItemType_Caestus:
			case ItemType_Caestus_Paper:
			case ItemType_Caestus_Plate:
				sa.info.type = AUCTION_EQUIP_WAIST;
				break;
			case ItemType_Shoes:
			case ItemType_Shoes_Paper:
			case ItemType_Shoes_Plate:
				sa.info.type = AUCTION_EQUIP_FOOT;
				break;
			case ItemType_Necklace:
				sa.info.type = AUCTION_ACCESSORY_NECKLACE;
				break;
			case ItemType_Fing:
				sa.info.type = AUCTION_ACCESSORY_RING;
				break;
			case ItemType_Adonment:
				sa.info.type = AUCTION_ACCESSORY_ADORNMENT;
				break;
			case ItemType_Book:
				switch (ob->base->maxsp)
				{
				case 1:
					sa.info.type = AUCTION_BOOK_FIGHTER;
					break;
				case 2:
					sa.info.type = AUCTION_BOOK_ARCHER;
					break;
				case 3:
					sa.info.type = AUCTION_BOOK_WIZARD;
					break;
				case 4:
					sa.info.type = AUCTION_BOOK_SUMMONER;
					break;
				case 5:
					sa.info.type = AUCTION_BOOK_PRIEST;
					break;
				default:
					sa.info.type = AUCTION_BOOK_FIGHTER;//不知道类型的分到战士组里
					break;
				}
				break;
			case ItemType_SpecialBook:
				sa.info.type = AUCTION_BOOK_SPECIAL;
				break;
			case ItemType_LevelUp:
			case ItemType_SOULSTONE:
				sa.info.type = AUCTION_OTHER_GEM;
				break;
			case ItemType_Pack:
			case ItemType_Scroll:
			case ItemType_Move:
			case ItemType_CaptureWeapon:
			case ItemType_Tonic:
			case ItemType_Gift:
			case ItemType_MASK:
			case ItemType_Wedding:
			case ItemType_Auto:
			case ItemType_SkillUp:
			case ItemType_Renew:
			case ItemType_Repair:
				sa.info.type = AUCTION_OTHER_ITEM;
				break;
			case ItemType_Resource:
				sa.info.type = AUCTION_OTHER_MATERIAL;
				break;
			case ItemType_Leechdom:
				sa.info.type = AUCTION_OTHER_LEECHDOM;
				break;
			default:
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你不能拍卖这件物品");
				return true;
			}
			DWORD charge = 0;
			switch (ob->data.kind%8)
			{
			case 0:
			case 4:
				charge = ob->base->price*ob->data.dwNum/10;
				sa.info.quality = 0;
				break;
			case 1:
			case 5:
				charge = ob->base->price*ob->data.dwNum*3/2/10;//蓝色1.5倍
				sa.info.quality = 1;
				break;
			case 2:
			case 3:
				charge = ob->base->price*ob->data.dwNum*2/10;//金色2倍
				sa.info.quality = 2;
				break;
			case 6:
			case 7:
				charge = ob->base->price*ob->data.dwNum*4/10;//绿色4倍
				sa.info.quality = 4;
				break;
			default:
				Zebra::logger->warn("[拍卖]%s 拍卖物品品质类型错误 kind=%u",name,ob->data.kind);
				return false;
				break;
			}
			if (charge<100) charge = 100;
			if (!packs.checkMoney(charge)
				|| !packs.removeMoney(charge,"拍卖扣除"))
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你的金钱不足");
				return true;
			}

			strncpy(sa.info.owner,name,MAX_NAMESIZE);

			sa.info.ownerID = id;
			sa.info.state = AUCTION_STATE_NEW;
			sa.info.charge = charge*3;
			strncpy(sa.info.itemName,ob->data.strName,MAX_NAMESIZE);
			//sa.info.type = ob->base->kind;
			sa.info.needLevel = ob->data.needlevel;
			sa.info.minMoney = rev->bidType?0:rev->minMoney;
			sa.info.maxMoney = rev->bidType?0:rev->maxMoney;
			sa.info.minGold = rev->bidType?rev->minGold:0;
			sa.info.maxGold = rev->bidType?rev->maxGold:0;
			zRTime ct;
			sa.info.startTime = ct.sec();
			sa.info.endTime = sa.info.startTime + 60*60*12;
			//sa.info.bidType = rev->bidType;
			sa.info.bidType = 0;//关闭金币拍卖
			sa.info.itemID = ob->data.qwThisID;//关闭金币拍卖

			packs.removeObject(ob,true,false); //notify but not delete
			//ob->getSaveData((SaveObject *)&sa.item);


			bcopy(&ob->data,&sa.item.object,sizeof(t_Object));


			//Zebra::logger->error("[拍卖]%s = %s createid = %ld",sa.item.object.strName,ob->data.strName,(SaveObject *)&sa.item.createid);

			zObject::logger(ob->createid,ob->data.qwThisID,ob->data.strName,ob->data.dwNum,ob->data.dwNum,0,this->id,this->name,0,NULL,"拍卖",ob->base,ob->data.kind,ob->data.upgrade);
			zObject::destroy(ob);

			sessionClient->sendCmd(&sa,sizeof(sa));
			save(OPERATION_WRITEBACK);
		}
		break;
	case BID_AUCTION_PARA:
		{
			stBidAuction * rev = (stBidAuction *)cmd;

			if (!packs.checkMoney(rev->money))
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你的金钱不足");
				return true;
			}

			t_checkBidAuction_SceneSession cba;
			cba.userID = tempid;
			cba.auctionID = rev->auctionID;
			cba.money = rev->money;
			cba.gold = rev->gold;

			sessionClient->sendCmd(&cba,sizeof(cba));
		}
		break;
	case QUERY_AUCTION_PARA:
		{
			if (SceneTimeTick::currentTime.sec()<queryTime)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"正在检索,请5秒后重试");
				return true;
			}

			stQueryAuction * rev = (stQueryAuction *)cmd;

			/*
			std::string s(rev->name);
			char * filter = "`~!@#$%^&*;:'\",<.>/?-_=+\\|";
			if (std::string::npos!=s.find(filter,0,strlen(filter)))
			{
			Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"请输入正确的物品名字");
			return true;
			}
			*/
			if (strchr(rev->name,'\'')
				|| strchr(rev->name,';')
				|| strchr(rev->name,'\"'))
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"请输入正确的物品名字");
				return true;
			}

			t_queryAuction_SceneSession qa;
			qa.userID = tempid;
			qa.type = rev->type;
			strncpy(qa.name,rev->name,MAX_NAMESIZE);
			qa.quality = rev->type>30?0:rev->quality;//除了装备其他物品都不考虑品质
			qa.level = rev->level;
			qa.page = rev->page;

			sessionClient->sendCmd(&qa,sizeof(qa));

			queryTime = SceneTimeTick::currentTime.sec()+5;
		}
		break;
	case CANCEL_SALE_AUCTION_PARA:
		{
			stCancelSaleAuction * rev = (stCancelSaleAuction *)cmd;

			t_checkCancelAuction_SceneSession cca;
			cca.userID = tempid;
			cca.auctionID = rev->auctionID;

			sessionClient->sendCmd(&cca,sizeof(cca));
		}
		break;
	case GET_LIST_AUCTION_PARA:
		{
			stGetListAuction * rev = (stGetListAuction *)cmd;
			if (0==rev->list) break;

			t_getListAuction_SceneSession gla;
			gla.userID = tempid;
			gla.list = rev->list;

			sessionClient->sendCmd(&gla,sizeof(gla));
		}
		break;
	default:
		return false;
	}
	/*
	*/
	return false;
	//#endif
}

/**
* \brief 执行监狱相关的指令
*
*
* \param rev 消息指针
* \param cmdLen 指令长度
* \return 是否执行成功
*/
bool SceneUser::doPrisonCmd(const Cmd::stPrisonUserCmd *cmd,DWORD cmdLen)
{
	//using namespace Cmd;
	switch (cmd->byParam)
	{
	case Cmd::OUT_PRISON_PARA:
		{
			//stOutPrison * rev = (stOutPrison *)cmd;

			//if (scene->getRealMapID()!=189) return false;
			if ((charbase.goodness&0x0000ffff)>0 && (charbase.goodness&0x0000ffff)<=MAX_GOODNESS)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"努力改造,提早出狱！");
				return true;
			}

			Scene * s = SceneManager::getInstance().getSceneByName("中立区·皇城");
			//Scene * s = SceneManager::getInstance().getSceneByID(tomapid);
			if (s)
			{
				//const zPos pos=wp->getRandDest()->pos;
				bool suc = changeMap(s,zPos(806,716));
				if (!suc)
				{
					Zebra::logger->error("%s PK值 %u,出狱失败,目的 %s (%d,%d)",name,charbase.goodness,s->name,pos.x,pos.y);
					return false;
				}
				else
					Zebra::logger->error("%s PK值 %u,出狱",name,charbase.goodness);

				return true;
			}
			else
			{
				//if (guard && guard->canMove()) saveGuard = true;//使镖车跟随指令使用者
				//if (adoptList.size()) saveAdopt = true;
				Cmd::Session::t_changeScene_SceneSession cmd;
				cmd.id = id;
				cmd.temp_id = tempid;
				cmd.x = 806;
				cmd.y = 716;
				cmd.map_id = 0;
				cmd.map_file[0] = '\0';
				strncpy((char *)cmd.map_name,"中立区·皇城",MAX_NAMESIZE);
				sessionClient->sendCmd(&cmd,sizeof(cmd));

				return true;
			}
		}
		break;
	case Cmd::BRIBE_PRISON_PARA:
		{
			Cmd::stBribePrison * rev = (Cmd::stBribePrison *)cmd;

			//检查是不是访问狱卒

			DWORD good = (charbase.goodness&0x0000ffff);
			if (good>0 && good<=MAX_GOODNESS)
			{
				if (!packs.checkMoney(rev->money) || !packs.removeMoney(rev->money,"贿赂狱卒"))
				{
					Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"小样,没那么多钱就别来找我！");
					return true;
				}

				DWORD minus = min(charbase.goodness,rev->money/200);
				charbase.goodness -= minus;
				if (minus>0)
				{
					Channel::sendPrivate("狱卒",name,"嘿嘿,您真是大方,我让您提前%u分钟出去~",minus);
					Cmd::stMainUserDataUserCmd  userinfo;
					full_t_MainUserData(userinfo.data);
					sendCmdToMe(&userinfo,sizeof(userinfo));
					sendMeToNine();
				}
				else
					Channel::sendPrivate("狱卒",name,"好...  (%s大名鼎鼎,没想到出手这么寒碜)",name);
			}
			return true;
		}
		break;
	case Cmd::BAIL_PRISON_PARA:
		{
			Cmd::stBailPrison * rev = (Cmd::stBailPrison *)cmd;

			//检查是不是访问典狱官

			SceneUser * pUser = SceneUserManager::getMe().getUserByName(rev->name);
			if (!pUser)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"%s 现在不在线",rev->name);
				return true;
			}

			DWORD good = (pUser->charbase.goodness&0x0000ffff);
			if (good>0 && good<=MAX_GOODNESS)
			{
				if (!packs.checkMoney(rev->money)
					|| !packs.removeMoney(rev->money,"保释玩家"))
				{
					Channel::sendPrivate("狱卒",name,"小样,没那么多钱就别来找我！");
					return true;
				}

				DWORD minus = min(pUser->charbase.goodness,rev->money/200);
				pUser->charbase.goodness -= minus;
				if (minus>0)
				{
					Channel::sendPrivate("狱卒",name,"嘿嘿,您真是大方,我让 %s 提前%u分钟出去~",rev->name,minus);
					Channel::sendPrivate("狱卒",rev->name,"嘿嘿,%s 关照过了,我让你提前%u分钟出狱~",name,minus);
					Cmd::stMainUserDataUserCmd  userinfo;
					pUser->full_t_MainUserData(userinfo.data);
					pUser->sendCmdToMe(&userinfo,sizeof(userinfo));
					pUser->sendMeToNine();
				}
				else
					Channel::sendPrivate("狱卒",name,"好...  (%s大名鼎鼎,没想到出手这么寒碜)",name);
			}
			return true;
		}
		break;
	case Cmd::LEAVE_PRISON_PARA:
		{
			//stOutPrison * rev = (stOutPrison *)cmd;

			//if (scene->getRealMapID()!=189) return false;
			if (charbase.punishTime>0)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"努力改造,提早出狱！你还有 %u 分钟的刑期",charbase.punishTime);
				return true;
			}

			Scene * s = SceneManager::getInstance().getSceneByName("中立区·皇城");
			//Scene * s = SceneManager::getInstance().getSceneByID(tomapid);
			if (s)
			{
				//const zPos pos=wp->getRandDest()->pos;
				bool suc = changeMap(s,zPos(806,716));
				if (!suc)
				{
					Zebra::logger->error("%s PK值 %u,出狱失败,目的 %s (%d,%d)",name,charbase.goodness,s->name,pos.x,pos.y);
					return false;
				}
				else
					Zebra::logger->error("%s PK值 %u,出狱",name,charbase.goodness);

				return true;
			}
			else
			{
				//if (guard && guard->canMove()) saveGuard = true;//使镖车跟随指令使用者
				//if (adoptList.size()) saveAdopt = true;
				Cmd::Session::t_changeScene_SceneSession cmd;
				cmd.id = id;
				cmd.temp_id = tempid;
				cmd.x = 806;
				cmd.y = 716;
				cmd.map_id = 0;
				cmd.map_file[0] = '\0';
				strncpy((char *)cmd.map_name,"中立区·皇城",MAX_NAMESIZE);
				sessionClient->sendCmd(&cmd,sizeof(cmd));

				return true;
			}
		}
		break;
	default:
		break;
	}

	return false;
}
#endif

/**
* \brief 执行邮件相关的指令
*
*
* \param rev 消息指针
* \param cmdLen 指令长度
* \return 是否执行成功
*/
bool SceneUser::doMailCmd(const Cmd::stMailUserCmd *rev,DWORD cmdLen)
{
	//free 禁止邮件服务
	//  Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"邮件服务正在开发中！");
	//  return true;

	//#if 0
	using namespace Cmd;
	using namespace Cmd::Session;

	if (!(atoi(Zebra::global["service_flag"].c_str())&SERVICE_MAIL))
	{
		Channel::sendSys(this,INFO_TYPE_FAIL,"邮件服务已停止,请留意系统公告");
		return true;
	}
	//检查是不是在访问邮箱
	if (rev->byParam!=SEND_MAIL_PARA)
	{
		SceneNpc * npc = SceneNpcManager::getMe().getNpcByTempID(npc_dwNpcTempID);
		if (!npc)
		{
			Zebra::logger->info("[邮件]%s 非法访问邮箱 npcID=%u npcTempID=%u",name,npc_dwNpcDataID,npc_dwNpcTempID);
			return false;
		}
#if 0
		if (npc->npc->kind!=NPC_TYPE_MAILBOX)
		{
			Zebra::logger->info("[邮件]%s 非法访问邮箱,npc类型错误 %d",name,npc->npc->kind);
			return false;
		}
#endif
		if (scene != npc->scene)
		{
			Zebra::logger->info("[邮件]%s 非法访问邮箱,不在同一场景",name);
			return false;
		}
		if (!scene->zPosShortRange(npc->getPos(),getPos(),SCREEN_WIDTH,SCREEN_HEIGHT))
		{
			Zebra::logger->info("[邮件]%s 非法访问邮箱,距离太远",name);
			return false;
		}
	}

	switch (rev->byParam)
	{
	case SEND_MAIL_PARA:
		{
#if 0
			if (charbase.level<30)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对不起,30级以上才可以发送邮件");
				return true;
			}
#endif
			if (isSendingMail)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对不起,你发送邮件速度过快");
				Zebra::logger->warn("[邮件]%s 发送邮件速度过快",name);
				return false;
			}
			stSendMail * cmd = (stSendMail *)rev;

			if (0!=cmd->sendGold || 0!=cmd->recvGold)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对不起,不可以邮寄金币");
				return true;//关闭邮寄金币
			}

			if (0==strncmp(name,cmd->toName,MAX_NAMESIZE))
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"不可以给自己发邮件");
				Zebra::logger->warn("[邮件]%s(%u)试图给自己发邮件",name,id);
				return true;
			}

			if (strchr(cmd->toName,'\'')
				|| strchr(cmd->toName,';')
				|| strchr(cmd->toName,'\"'))
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"请输入正确的角色名字");
				return true;
			}

			Cmd::Record::t_userExist_SceneRecord ue;
			ue.fromID = tempid;

			//SceneUser * pUser = SceneUserManager::getMe().getUserByName(cmd->toName);

			//if(NULL == pUser)
			//	return true;

			//ue.toID = pUser->id;
			bcopy(rev,&ue.sm,sizeof(Cmd::stSendMail));
			recordClient->sendCmd(&ue,sizeof(ue));

			isSendingMail = true;
			return true;

			/*
			Session::t_checkSend_SceneSession sm;

			if (cmd->itemID && cmd->itemID!=INVALID_THISID)
			{
			zObject* ob = packs.uom.getObjectByThisID(cmd->itemID);
			if (!ob)
			{
			Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你的包裹里没有该物品");
			Zebra::logger->warn("[邮件]%s(%ld)试图邮寄的物品不存在",name,id);
			return true;
			}
			if (!ob->canMail())
			{
			Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你不能邮寄这件物品");
			return true;
			}
			sm.mail.accessory = 1;
			}
			if (cmd->sendMoney)
			{
			if (!packs.checkMoney(cmd->sendMoney+mail_postage))
			{
			Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你的金钱不足,邮资需要50文");
			return true;
			}
			sm.mail.accessory = 1;
			}

			sm.mail.state = MAIL_STATE_NEW;
			strncpy(sm.mail.fromName,name,MAX_NAMESIZE);
			strncpy(sm.mail.toName,cmd->toName,MAX_NAMESIZE);
			strncpy(sm.mail.title,cmd->title,MAX_NAMESIZE);
			sm.mail.type = MAIL_TYPE_MAIL;
			zRTime ct;
			sm.mail.createTime = ct.sec();
			sm.mail.delTime = sm.mail.createTime + 60*60*24*7;
			strncpy(sm.mail.text,cmd->text,256);
			sm.mail.sendMoney = cmd->sendMoney;
			sm.mail.recvMoney = cmd->recvMoney;
			sm.mail.sendGold = cmd->sendGold;
			sm.mail.recvGold = cmd->recvGold;
			sm.itemID = cmd->itemID;
			sm.mail.itemGot = 0;
			sessionClient->sendCmd(&sm,sizeof(sm));
			return true;
			*/
		}
		break;
	case GET_LIST_MAIL_PARA:
		{
			t_getMailList_SceneSession gml;
			gml.tempID = tempid;
			sessionClient->sendCmd(&gml,sizeof(gml));
		}
		break;
	case OPEN_MAIL_PARA:
		{
			stOpenMail * cmd = (stOpenMail *)rev;

			t_openMail_SceneSession om;
			om.tempID = tempid;
			om.mailID = cmd->mailID;
			sessionClient->sendCmd(&om,sizeof(om));
		}
		break;
	case GET_ITEM_MAIL_PARA:
		{
			/*
			if (isGetingMailItem)
			{
			Zebra::logger->warn("[邮件]%s 收取附件速度过快",name);
			Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对不起,你点太快了");
			return false;
			}
			*/
			isGetingMailItem = true;

			stGetItemMail * cmd = (stGetItemMail *)rev;

			t_getMailItem_SceneSession gmi;
			gmi.space = packs.uom.space(this);
			gmi.tempID = tempid;
			gmi.mailID =  cmd->mailID;
			zObject *gold = packs.getGold();//银子
			gmi.money = gold?gold->data.dwNum:0;//金子
			gmi.gold =  charbase.gold;
			gmi.mailID =  cmd->mailID;
			sessionClient->sendCmd(&gmi,sizeof(gmi));
		}
		break;
	case DEL_MAIL_PARA:
		{
			stDelMail * cmd = (stDelMail *)rev;

			t_delMail_SceneSession dm;
			dm.tempID = tempid;
			dm.mailID =  cmd->mailID;
			sessionClient->sendCmd(&dm,sizeof(dm));
		}
		break;
	case TURN_BACK_MAIL_PARA:
		{
			stTurnBackMail * cmd = (stTurnBackMail *)rev;

			t_turnBackMail_SceneSession tm;
			tm.userID = tempid;
			tm.mailID =  cmd->mailID;
			sessionClient->sendCmd(&tm,sizeof(tm));
		}
		break;
	default:
		break;
	}
	return false;
	//#endif 
}

#if 0
/**
* \brief 执行宠物相关的指令
*
*
* \param rev 消息指针
* \param cmdLen 指令长度
* \return 是否执行成功
*/
bool SceneUser::doPetCmd(const Cmd::stPetUserCmd *rev,DWORD cmdLen)
{
	using namespace Cmd;
	switch (rev->byParam)
	{
	case SETAI_PET_PARA:
		{
			stSetAIPetCmd * cmd = (stSetAIPetCmd *)rev;
			//Zebra::logger->debug("doPetCmd():%s 设置宠物AI type=%u,mode=%4x",name,cmd->type,cmd->mode);
			switch (cmd->type)
			{
			case PET_TYPE_PET:
				{
					if (!pet) return false;
					pet->setPetAI(cmd->mode);
				}
				break;
			case PET_TYPE_SUMMON:
				{
					if (!summon) return false;
					summon->setPetAI(cmd->mode);
				}
				break;
			default:
				Zebra::logger->error("doPetCmd SETAI_PET_PARA: 未知的宠物类型 %d",cmd->type);
				break;
			}
		}
		break;
	case REQUESTDATA_PET_PARA:
		{
			stRequestDataPetCmd * cmd = (stRequestDataPetCmd *)rev;
#ifdef _DEBUG
			Zebra::logger->debug("doPetCmd():  %s 请求宠物状态 type=%u,type=%u",name,cmd->type,cmd->type);
#endif
			switch (cmd->type)
			{
			case PET_TYPE_RIDE:
				{
					if (!horse.horse()) return true;
					stHorseDataPetCmd ret;
					ret.type = PET_TYPE_RIDE;
					ret.id = horse.data.horseid;
					horse.full_HorseDataStruct(&ret.data);
					sendCmdToMe(&ret,sizeof(ret));
				}
				break;
			case PET_TYPE_PET:
				{
					if (!pet) return true;
					stRefreshDataPetCmd ret;
					ret.type = PET_TYPE_PET;
					ret.id = pet->tempid;
					pet->full_PetDataStruct(ret.data);
					sendCmdToMe(&ret,sizeof(ret));
				}
				break;
			case PET_TYPE_SUMMON:
				{
					if (!summon) return true;
					stRefreshDataPetCmd ret;
					ret.type = PET_TYPE_SUMMON;
					ret.id = summon->tempid;
					summon->full_PetDataStruct(ret.data);
					sendCmdToMe(&ret,sizeof(ret));
				}
				break;
			default:
				{
					Zebra::logger->error("doPetCmd(): %s 请求宠物信息,错误的类型 %d",name,cmd->type);
				}
				break;
			}
		}
		break;
	case CHANGENAME_PET_PARA:
		{
			stChangeNamePetCmd * cmd = (stChangeNamePetCmd *)rev;
			//Zebra::logger->debug("doPetCmd():  %s 更改宠物名字 type=%u name=%s",name,cmd->type,cmd->name);
			switch (cmd->type)
			{
			case PET_TYPE_RIDE:
				{
				}
				break;
			case PET_TYPE_PET:
				{
					if (!pet) return true;

					strncpy(pet->name,cmd->name,MAX_NAMESIZE-1);
					strncpy(pet->petData.name,cmd->name,MAX_NAMESIZE-1);

					stRefreshDataPetCmd ret;
					ret.type = PET_TYPE_PET;
					ret.id = pet->tempid;
					pet->full_PetDataStruct(ret.data);
					sendCmdToMe(&ret,sizeof(ret));

					pet->sendPetDataToNine();
					/*
					Cmd::stAddMapNpcMapScreenUserCmd addNpc;
					pet->full_t_MapNpcData(addNpc.data);
					scene->sendCmdToNine(getPosI(),&addNpc,sizeof(addNpc));
					*/
					//sendNineToMe();
				}
				break;
			case PET_TYPE_SUMMON:
				{
					if (!summon) return true;

					strncpy(summon->name,cmd->name,MAX_NAMESIZE-1);
					strncpy(summon->petData.name,cmd->name,MAX_NAMESIZE-1);

					stRefreshDataPetCmd ret;
					ret.type = PET_TYPE_SUMMON;
					ret.id = summon->tempid;
					summon->full_PetDataStruct(ret.data);
					sendCmdToMe(&ret,sizeof(ret));

					summon->sendPetDataToNine();
					/*
					Cmd::stAddMapNpcMapScreenUserCmd addNpc;
					summon->full_t_MapNpcData(addNpc.data);
					scene->sendCmdToNine(getPosI(),&addNpc,sizeof(addNpc));
					//sendNineToMe();
					*/
				}
				break;
			default:
				{
					Zebra::logger->error("doPetCmd(): %s 设置宠物名字,错误的类型 %d",name,cmd->type);
					return true;
				}
				break;
			}
		}
		break;
	case CALLHORSE_PET_PARA:
		{
			//Zebra::logger->debug("%s 放出马",name);
			if (horse.horse())
			{
				if (ridepet)
					horse.putAway();
				else
					horse.comeOut();
				//horse.comeOut();
			}
			else
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你还没有马匹");
		}
		break;
	case PUTHORSE_PET_PARA:
		{
			//Zebra::logger->debug("%s 收起马",name);
			if (horse.horse())
			{
				if (ridepet)
					horse.putAway();
				else
					horse.comeOut();
			}
			else
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你还没有马匹");
		}
		break;
	case SETTARGET_PET_PARA:
		{
			stSetTargetPetCmd * cmd = (stSetTargetPetCmd *)rev;
			//Zebra::logger->debug("doPetCmd():  %s设置宠物攻击目标 type=%u name=%s",name,cmd->type,cmd->name);
			SceneEntryPk * target = NULL;
			if (0==cmd->targetType)//玩家
				target = scene->getUserByTempID(cmd->id);
			else if (1==cmd->targetType)//NPC
			{
				target = SceneNpcManager::getMe().getNpcByTempID(cmd->id);
				//if (captureIt((SceneNpc *)target))
				//  return true;
			}

#ifdef _DEBUG
			Zebra::logger->debug("%s 指定宠物攻击目标 type=%u id=%u",name,cmd->targetType,cmd->id);
#endif
			if (!target || zSceneEntry::SceneEntry_Normal!=target->getState())
				return true;

			switch (cmd->type)
			{
			case PET_TYPE_RIDE:
				{
				}
				break;
			case PET_TYPE_PET:
				{
					if (!pet) return true;
					pet->lockTarget = false;
					if (pet->setCurTarget(target,true))
						pet->lockTarget = true;
					else
						Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"无法攻击该目标或距离太远");
				}
				break;
			case PET_TYPE_SUMMON:
				{
					if (!summon) return true;
					summon->lockTarget = false;
					if (summon->setCurTarget(target,true))
						summon->lockTarget = true;
					else
						Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"无法攻击该目标或距离太远");
				}
				break;
			default:
				{
					return true;
				}
				break;
			}
		}
		break;
	default:
		{
			Zebra::logger->error("doPetCmd(): %s 未知的宠物消息类型 byParam=%d",name,rev->byParam);
			return true;
		}
	}
	return true;
}

#endif
/**
* \brief 执行卡通宠物自动修理装备
*
* \param obj 物品指针
*/
void SceneUser::petAutoRepair(zObject *obj)
{
#if 0
	if (cartoonList.empty()) return;

	bool can = false;

	//优先利用跟随的宠物
	if (cartoon && cartoon->getCartoonData().repair && cartoon->getCartoonData().time>=14400)
	{
		Cmd::t_CartoonData d = cartoon->getCartoonData();
		d.time -= 14400;
		cartoon->setCartoonData(d);
		cartoon->save(Cmd::Session::SAVE_TYPE_TIMETICK);

		Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你的宝宝 %s 帮你修理了所有装备,消耗其4小时修炼时间",cartoon->name);
		Zebra::logger->info("[宠物]%s 的宝宝 %s(%u) 修理装备,消耗 14400 秒修炼时间,剩余 %u ",name,cartoon->name,cartoon->getCartoonID(),d.time);
		can = true;
	}

	if (!can) return;

	//obj->data.dur = obj->data.maxdur;
	RepairEquipUseGold repair(this);
	packs.equip.execEvery(repair);

	packs.equip.calcAll();
#endif
}

#if 0
/**
* \brief 执行卡通宠物相关的指令
*
*
* \param cmd 消息指针
* \param cmdLen 指令长度
* \return 是否执行成功
*/
bool SceneUser::doCartoonCmd(const Cmd::stCartoonUserCmd *cmd,DWORD cmdLen)
{
	using namespace Cmd;
	using namespace Cmd::Session;

	switch (cmd->byParam)
	{
	case REPAIR_CARTOON_PARA:
		{
			stRepairCartoonCmd * rev = (stRepairCartoonCmd *)cmd;
			if (cartoonList.find(rev->cartoonID)==cartoonList.end())
				return true;
			if (cartoonList[rev->cartoonID].repair == rev->repair)
				return true;

			if (cartoon && cartoon->getCartoonID()==rev->cartoonID)
			{
				t_CartoonData d = cartoon->getCartoonData();
				d.repair = rev->repair;
				cartoon->setCartoonData(d);

				cartoonList[rev->cartoonID] = d;
			}
			else return true;

			Cmd::stAddCartoonCmd a;
			a.isMine = true;
			a.cartoonID = rev->cartoonID;
			a.data = cartoonList[rev->cartoonID];
			sendCmdToMe(&a,sizeof(a));

			/*
			t_setRepairCartoon_SceneSession send;
			send.userID = id;
			send.cartoonID = rev->cartoonID;
			send.repair = rev->repair;
			sessionClient->sendCmd(&send,sizeof(send));
			*/

			Zebra::logger->info("[自动修理]%s(%u) %s自动修理",name,id,rev->repair?"打开":"关闭");
			Zebra::logger->info("[自动修理]%s(%u) %s自动修理",name,id,rev->repair?"打开":"关闭");
		}
		break;
	case SELL_ALL_CARTOON_PARA:
		{
			if (!cartoon) return true;

			PetPack *pack = (PetPack *)packs.getPackage(Cmd::OBJECTCELLTYPE_PET,0);
			if (!pack) return false;

			if (pack->isEmpty()) return true;

			DWORD cartoonID = cartoon->getCartoonID();
			cartoonList[cartoonID] = cartoon->getCartoonData();
			if (cartoonList[cartoonID].sp<30)
			{
				Channel::sendSys(this,INFO_TYPE_FAIL,"%s 体力值不够了",cartoonList[cartoonID].name);
				return true;
			}

			cartoonList[cartoonID].sp -= 30;
			cartoon->setCartoonData(cartoonList[cartoonID]);

			struct SellPack : public PackageCallback
			{
			public:
				SellPack(Packages *p,SceneUser * u) : count(0),pPack(p),pUser(u)
				{ }

				virtual bool exec(zObject* p)
				{
					if (!p || !p->canMail()) return true;

					if (p->data.price>0)
					{
						DWORD price = get_sell_price_by_dur(p);
						DWORD real_price = 0;

						if (p->base->maxnum && (p->base->kind == ItemType_Arrow))
						{
							if (p->base->durability)
								real_price = (DWORD)(pUser->getGoodnessPrice((p->data.dwNum*price),false));
						}
						else
						{
							if (p->base->durability)
								real_price = (DWORD)(pUser->getGoodnessPrice((p->data.dwNum*price),false));
							else
								real_price = (DWORD)(pUser->getGoodnessPrice((p->data.dwNum*price),false));
						}

						if (p->base->id == 655)
						{
							if (p->base->durability)
								real_price = (DWORD)(((float)((p->data.dur+49)/50.0f)/((p->base->durability+49)/50.0f)) * 4000.0f);
						}

						count += real_price;
					}

					if (p->data.exp && p->base->kind != ItemType_Pack )
						pUser->addExp(p->data.exp);

					zObject::logger(p->createid,p->data.qwThisID,p->base->name,p->data.dwNum,p->data.dwNum,0,0,NULL,pUser->id,pUser->name,"pet_sell",NULL,0,0);

					pPack->removeObject(p,false,true);

					return true;
				}

				DWORD count;
			private:
				Packages * pPack;

				SceneUser * pUser;
			};

			SellPack sp(&packs,this);
			pack->execEvery(sp);

			if (sp.count)
			{
				packs.addMoney(sp.count,"宠物出售");
				Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"%s 帮你卖掉了一些道具",cartoonList[cartoonID].name);
				Zebra::logger->debug("%s 出售宠物包裹物品,获得金钱%u",name,sp.count);
				sendCmdToMe(cmd,cmdLen);//发回去清空包裹
			}
			else
				Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"没什么好卖的");

			return true;
		}
		break;
	case CHARGE_CARTOON_PARA:
		{
			stChargeCartoonCmd * rev = (stChargeCartoonCmd *)cmd;
			if (cartoonList.find(rev->cartoonID)==cartoonList.end())
				return true;
			if (cartoonList[rev->cartoonID].state == CARTOON_STATE_WAITING
				|| cartoonList[rev->cartoonID].state == CARTOON_STATE_ADOPTED)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"宠物不在你身边,不能充值");
				return true;
			}
			if (cartoonList[rev->cartoonID].time+rev->time > 25920000)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对不起,一个宝宝最多可以充值7200小时");
				return true;
			}

			if (0==rev->time) return true;

			DWORD need = (rev->time%144)>72?1:0;
			need += rev->time/144;
			if (!packs.checkGold(need)||!packs.removeGold(need,"给宠物充值"))
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对不起,你的金币不足");
				return true;
			}

			//一定要先保存
			if (cartoon && cartoon->getCartoonID()==rev->cartoonID)
			{
				cartoon->save(Cmd::Session::SAVE_TYPE_TIMETICK);

				cartoonList[rev->cartoonID] = cartoon->getCartoonData();
				cartoonList[rev->cartoonID].time += rev->time;
				cartoon->setCartoonData(cartoonList[rev->cartoonID]);
			}
			else
			{
				Cmd::Session::t_saveCartoon_SceneSession send;
				strncpy(send.userName,name,MAX_NAMESIZE);
				send.type = Cmd::Session::SAVE_TYPE_TIMETICK;
				send.cartoonID = rev->cartoonID;
				send.data = cartoonList[rev->cartoonID];
				sessionClient->sendCmd(&send,sizeof(send));

				cartoonList[rev->cartoonID].time += rev->time;
			}

			t_chargeCartoon_SceneSession send;
			send.masterID = id;
			send.cartoonID = rev->cartoonID;
			send.time = rev->time;
			sessionClient->sendCmd(&send,sizeof(send));

			Cmd::stAddCartoonCmd a;
			a.isMine = true;
			a.cartoonID = rev->cartoonID;
			a.data = cartoonList[rev->cartoonID];
			sendCmdToMe(&a,sizeof(a));
		}
		break;
	case SALE_CARTOON_PARA:
		{
			stSaleCartoonCmd * rev = (stSaleCartoonCmd *)cmd;
			if (cartoonList.find(rev->cartoonID)==cartoonList.end())
				return true;
			if (cartoonList[rev->cartoonID].state == CARTOON_STATE_WAITING
				|| cartoonList[rev->cartoonID].state == CARTOON_STATE_ADOPTED)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"%s 不在你身边,不能出售",cartoonList[rev->cartoonID].name);
				return true;
			}

			if (cartoonList[rev->cartoonID].time>=600)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"%s 还有10分钟以上的修炼时间,请不要卖掉它",cartoonList[rev->cartoonID].name);
				return true;
			}

			PetPack *pack = (PetPack *)packs.getPackage(Cmd::OBJECTCELLTYPE_PET,0);
			if (!pack) return false;
			if (!pack->isEmpty())
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"请先清空宠物包裹再出售宠物");
				return true;
			}

			t_saleCartoon_SceneSession send;
			send.userID = id;
			send.cartoonID = rev->cartoonID;
			sessionClient->sendCmd(&send,sizeof(send));
		}
		break;
	case REQUEST_LIST_CARTOON_PARA:
		{
			t_getListCartoon_SceneSession send;
			send.userID = id;
			sessionClient->sendCmd(&send,sizeof(send));
		}
		break;
	case BUY_CARTOON_PARA:
		{
			if (cartoonList.size()>=2)
			{
				//Channel::sendSys(this,INFO_TYPE_FAIL,"你的宠物太多了");
				Channel::sendSys(this,INFO_TYPE_FAIL,"你同时只能拥有两只替身宝宝,请先卖掉再购买");
				return true;
			}

			stBuyCartoonCmd * rev = (stBuyCartoonCmd *)cmd;

			DWORD needMoney = 100;
			/*
			if (9005==rev->npcID)//情人节买丘比特
			{
			struct tm tv1;
			time_t timValue = time(NULL);
			zRTime::getLocalTime(tv1,timValue);
			if (tv1.tm_mon!=1||tv1.tm_mday!=14)
			{
			Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"情人节已经过了,不能购买");
			return true;
			}
			needMoney = 5000;
			}
			*/

			if (!packs.checkMoney(needMoney) || !packs.removeMoney(needMoney,"购买替身宝宝"))
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你的金钱不足");
				return true;
			}       

			//stBuyCartoonCmd * rev = (stBuyCartoonCmd *)cmd;
			zNpcB *base = npcbm.get(rev->npcID);
			if (!base) return false;

			t_buyCartoon_SceneSession send;
			send.data.masterID = id;
			send.data.npcID = base->id;
			strncpy(send.data.masterName,name,MAX_NAMESIZE);
			strncpy(send.data.name,base->name,MAX_NAMESIZE);
			send.data.lv = 10;
			zExperienceB *base_exp = experiencebm.get(10);
			if (base_exp)
				send.data.maxExp = base_exp->nextexp/10;
			send.data.sp = 100;
			send.data.maxSp = 100;
			send.data.state = Cmd::CARTOON_STATE_PUTAWAY;
			send.data.time = 0;
			send.data.masterLevel = charbase.level;
			sessionClient->sendCmd(&send,sizeof(t_buyCartoon_SceneSession));
			return true;
		}
		break;
	case FOLLOW_CARTOON_PARA:
		{
			stFollowCartoonCmd * rev = (stFollowCartoonCmd *)cmd;

			if (cartoon)//取消跟随
			{
				DWORD i = cartoon->getCartoonID();
				cartoon->putAway(SAVE_TYPE_PUTAWAY);

				cartoonList[i].state=Cmd::CARTOON_STATE_PUTAWAY;

				Cmd::stAddCartoonCmd send;
				send.isMine = true;
				send.cartoonID = i;
				send.data = cartoonList[i];
				sendCmdToMe(&send,sizeof(send));
				return true;
			}

			if (cartoonList.find(rev->cartoonID)==cartoonList.end())
				return true;
			if (cartoonList[rev->cartoonID].state==Cmd::CARTOON_STATE_ADOPTED
				|| cartoonList[rev->cartoonID].state==Cmd::CARTOON_STATE_WAITING)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"这个宝宝已经寄养,请先收回它");
				return true;
			}

			cartoonList[rev->cartoonID].repair = 0;
			summonPet(cartoonList[rev->cartoonID].npcID,Cmd::PET_TYPE_CARTOON);
			if (cartoon)
			{
				cartoonList[rev->cartoonID].state = Cmd::CARTOON_STATE_FOLLOW;
				cartoon->setCartoonID(rev->cartoonID);
				cartoon->setCartoonData(cartoonList[rev->cartoonID]);
				cartoon->sendData();

				if (cartoon->id==9005)
					Channel::sendNine(cartoon,"我是伟大的爱神~");
				else
				{
					time_t timValue = time(NULL);
					struct tm tmValue;
					zRTime::getLocalTime(tmValue,timValue);
					if (tmValue.tm_hour<6)
						Channel::sendNine(cartoon,"主人,你现在应该去睡觉~");
					else if (tmValue.tm_hour<9)
						Channel::sendNine(cartoon,"主人早上好~");
					else if (tmValue.tm_hour<12)
						Channel::sendNine(cartoon,"主人上午好~");
					else if (tmValue.tm_hour<14)
						Channel::sendNine(cartoon,"主人中午好~");
					else if (tmValue.tm_hour<17)
						Channel::sendNine(cartoon,"主人下午好~");
					else
						Channel::sendNine(cartoon,"主人晚上好~");
				}
			}
			return true;
		}
		break;
	case LETOUT_CARTOON_PARA:
		{
			stLetOutCartoonCmd * rev = (stLetOutCartoonCmd *)cmd;
			if (cartoonList.find(rev->cartoonID)==cartoonList.end())
				return true;
			if (cartoonList[rev->cartoonID].time==0)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"不能训练这只宠物,请先充值");
				return true;
			}

			if (cartoon && cartoon->getCartoonID()==rev->cartoonID)
				cartoon->putAway(SAVE_TYPE_PUTAWAY);
			cartoonList[rev->cartoonID].state = Cmd::CARTOON_STATE_WAITING;
			Cmd::Session::t_saveCartoon_SceneSession send;
			strncpy(send.userName,name,MAX_NAMESIZE);
			send.type = SAVE_TYPE_LETOUT;
			send.cartoonID = rev->cartoonID;
			send.data = cartoonList[rev->cartoonID];
			sessionClient->sendCmd(&send,sizeof(send));
			return true;
		}
		break;
	case ADOPT_CARTOON_PARA:
		{
			stAdoptCartoonCmd * rev = (stAdoptCartoonCmd *)cmd;

			if (cartoonList.find(rev->cartoonID)!=cartoonList.end())
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你不能领养自己的宠物");
				return true;
			}
			if (adoptList.size()>=5)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你已经领养了五只宠物了,请先归还再领养");
				return true;
			}

			t_adoptCartoon_SceneSession send;
			send.userID = id;
			send.cartoonID = rev->cartoonID;
			sessionClient->sendCmd(&send,sizeof(send));
			return true;
		}
		break;
	case RETURN_CARTOON_PARA:
		{
			stReturnCartoonCmd * rev = (stReturnCartoonCmd *)cmd;
			if (adoptList.find(rev->cartoonID)==adoptList.end()) return true;

			adoptList[rev->cartoonID]->putAway(SAVE_TYPE_RETURN);
			return true;
		}
		break;
	case GETBACK_CARTOON_PARA:
		{
			bool b = false;
			for (cartoon_it it=cartoonList.begin(); it!=cartoonList.end(); it++)
			{
				if (it->second.state==Cmd::CARTOON_STATE_ADOPTED
					|| it->second.state==Cmd::CARTOON_STATE_WAITING)
				{
					b = true;
					break;
				}
			}
			if (!b)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你没有宠物被收养");
				return true;
			}

			t_getBackCartoon_SceneSession send;
			send.userID = id;
			sessionClient->sendCmd(&send,sizeof(send));

			return true;
		}
		break;
	case DRAWEXP_CARTOON_PARA:
		{
			stDrawExpCartoonCmd * rev = (stDrawExpCartoonCmd *)cmd;
			if (cartoonList.find(rev->cartoonID)==cartoonList.end())
				return true;

			if (cartoonList[rev->cartoonID].state==Cmd::CARTOON_STATE_WAITING
				|| cartoonList[rev->cartoonID].state==Cmd::CARTOON_STATE_ADOPTED)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"宝宝不在你身边");
				return true;
			}

			if (cartoonList[rev->cartoonID].addExp)
			{
				t_drawCartoon_SceneSession send;
				send.userID = id;
				send.cartoonID = rev->cartoonID;
				send.num = 0;
				sessionClient->sendCmd(&send,sizeof(send));

				if (cartoon&&cartoon->getCartoonID()==rev->cartoonID)
					cartoonList[rev->cartoonID] = cartoon->getCartoonData();
				cartoonList[rev->cartoonID].addExp = 0;
				if (cartoon&&cartoon->getCartoonID()==rev->cartoonID)
					cartoon->setCartoonData(cartoonList[rev->cartoonID]);
			}
			return true;
		}
		break;
	case CHANGENAME_CARTOON_PARA:
		{
			stChangeNameCartoonCmd * rev = (stChangeNameCartoonCmd *)cmd;
			if (cartoonList.find(rev->cartoonID)==cartoonList.end())
				return true;

			if (cartoonList[rev->cartoonID].state==Cmd::CARTOON_STATE_WAITING
				|| cartoonList[rev->cartoonID].state==Cmd::CARTOON_STATE_ADOPTED)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"宝宝不在你身边");
				return true;
			}

			if (cartoon) cartoon->setName(rev->name);
			strncpy(cartoonList[rev->cartoonID].name,rev->name,MAX_NAMESIZE);
			stAddCartoonCmd send;
			send.isMine = true;
			send.cartoonID = rev->cartoonID;
			send.data = cartoonList[rev->cartoonID];
			sendCmdToMe(&send,sizeof(send));

			return true;
		}
		break;
	case CONSIGN_CARTOON_PARA:
		{
			return true;//关闭此功能

			stConsignCartoonCmd * rev = (stConsignCartoonCmd *)cmd;
			if (cartoonList.find(rev->cartoonID)==cartoonList.end())
				return true;

			if (cartoon && cartoon->getCartoonID()==rev->cartoonID)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"请先把 %s 收起来",cartoonList[rev->cartoonID].name);
				return true;
			}

			if (cartoonList[rev->cartoonID].state==Cmd::CARTOON_STATE_ADOPTED)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"%s 已经被 %s 领养了",cartoonList[rev->cartoonID].name,cartoonList[rev->cartoonID].adopter);
				return true;
			}

			for (cartoon_it it=cartoonList.begin(); it!=cartoonList.end(); it++)
			{
				if (it->second.state==Cmd::CARTOON_STATE_ADOPTED
					|| it->second.state==Cmd::CARTOON_STATE_WAITING)
				{
					Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你的宝宝 %s 在等待状态或已经被领养了,请先收回它",it->second.name);
					return true;
				}
			}

			if (cartoonList[rev->cartoonID].time==0)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"请先给 %s 充值",cartoonList[rev->cartoonID].name);
				return true;
			}

			t_consignCartoon_SceneSession send;
			send.userID = id;
			send.cartoonID = rev->cartoonID;
			strncpy(send.name,rev->name,MAX_NAMESIZE);
			sessionClient->sendCmd(&send,sizeof(send));

			return true;
		}
		break;
	case CONSIGN_RET_CARTOON_PARA:
		{
			return true;//关闭此功能

			stConsignRetCartoonCmd * rev = (stConsignRetCartoonCmd *)cmd;

			Cmd::Session::t_consignRetCartoon_SceneSession send;
			send.userID = id;
			send.ret = rev->ret;
			send.cartoonID = rev->cartoonID;

			if (rev->ret==1 && adoptList.size()>=5)
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你已经领养5只宠物了,不能继续领养");
				send.ret = 2;
			}

			sessionClient->sendCmd(&send,sizeof(send));

			return true;
		}
		break;
	default:
		break;
	}
	return false;
}


/**
* \brief 删除一个宠物
*
*/
bool SceneUser::killOnePet(ScenePet * kill)
{
	if (!kill) return false;
	if (kill->getMaster()!=this)
	{
		Zebra::logger->error("[宠物]killOnePet %s(%u) 不是 %s 的宠物 getMaster()==%u",kill->name,kill->tempid,name,kill->getMaster());
		return false;
	}

	using namespace Cmd;

	//if (PET_TYPE_RIDE!=kill->getPetType())
	{
		stDelPetPetCmd del;
		del.id= kill->tempid;
		del.type = kill->getPetType();
		sendCmdToMe(&del,sizeof(del));
	}
	//if (PET_TYPE_RIDE==kill->getPetType())
	//  kill->setClearState();
	//else
	//  kill->toDie(this->tempid);

#ifdef _DEBUG
	//Zebra::logger->debug("SceneUser::killOnePet(): lock %s",name);
#endif
	switch (kill->getPetType())
	{
	case PET_TYPE_RIDE:
		{
			if (ridepet)
			{
				//if (0==ridepet->hp)//马死了,删除马
				{
					/*
					stDelPetPetCmd del;
					del.id= kill->tempid;
					del.type = PET_TYPE_RIDE;
					sendCmdToMe(&del,sizeof(del));
					*/

					//  horse.horse(0);
				}
				//else
				{
					if (horse.horse())//马活着
					{
						horse.data.state = Cmd::HORSE_STATE_PUTUP;
						horse.sendData();
						ridepet->setClearState();
					}
				}
			}
			ridepet = 0;
		}
		break;
	case PET_TYPE_PET:
		{
			pet->petData.hp = pet->hp;
			bcopy(&pet->petData,&petData,sizeof(petData));
			pet = 0;
		}
		break;
	case PET_TYPE_SUMMON:
		{
			if( !MirageSummon.empty() )   //sky 删除幻影的容器
				MirageSummon.clear();

			summon = 0;
		}
		break;
	case PET_TYPE_TOTEM:
		{
			totems.remove(kill);
		}
		break;
	case PET_TYPE_GUARDNPC:
		{
			guard = 0;
		}
		break;
	case PET_TYPE_CARTOON:
		{
			if (kill==cartoon)
				cartoon = 0;
			else
				adoptList.erase(((CartoonPet *)kill)->getCartoonID());
			/*
			if (((CartoonPet *)kill)->isAdopted())
			adoptList.erase(((CartoonPet *)kill)->getCartoonID());
			else
			cartoon = 0;
			*/
		}
		break;
	default:
		break;
	}

	kill->clearMaster();

	return true;
#ifdef _DEBUG
	//Zebra::logger->debug("SceneUser::killOnePet(): unlock %s",name);
#endif
	//kill->setClearState();
	//Zebra::logger->debug("标记npc %s",kill->name);
}
#endif
/**
* \brief 武器提升对应技能等级
* \param skilltype 技能的类型
* \param skillKind 技能的系别
* \return 提升的技能登记数
**/
WORD SceneUser::skillUpLevel(WORD skilltype,WORD skillkind)
{
	WORD upLevel = 0;
//	WORD temp = 0;
//	upLevel = packs.equip.getEquips().getMaxSkill(skilltype);
//	temp = packs.equip.getEquips().getMaxSkills(skillkind);
//	if (temp > upLevel) upLevel = temp;
	return upLevel;
}

/**
* \brief 检查是否骑马
* \return true 骑马 false 没骑
*/
bool SceneUser::checkMountHorse()
{
    return false;
//	return horse.mount();
}

/**
* \brief 人物坐下处理
*/
void SceneUser::sitdown()
{
	if (this->issetUState(Cmd::USTATE_SITDOWN))
	{
		this->clearUState(Cmd::USTATE_SITDOWN);
		this->setupCharBase();
	}
	else
	{
		this->setUState(Cmd::USTATE_SITDOWN);
		this->setupCharBase();
	}

	Cmd::stMainUserDataUserCmd  userinfo;
	full_t_MainUserData(userinfo.data);
	sendCmdToMe(&userinfo,sizeof(userinfo));

	this->sendMeToNine();
}

/**
* \brief 人物站立
*/
void SceneUser::standup()
{
	this->clearUState(Cmd::USTATE_SITDOWN);
	this->setupCharBase();

	Cmd::stMainUserDataUserCmd  userinfo;
	full_t_MainUserData(userinfo.data);
	sendCmdToMe(&userinfo,sizeof(userinfo));

	this->sendMeToNine();
}

bool SceneUser::isSitdown()
{
	return this->issetUState(Cmd::USTATE_SITDOWN);
}

#if 0
/**
* \brief 通知装备改变
*/
void SceneUser::notifyEquipChange()
{
	if (summon)
	{
		zObject *temp=NULL;
		if (packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDR))
		{
			if (temp)
			{
				if (0 != temp->data.dur)
				{
					if (temp->base->kind == ItemType_Stick)
					{
#ifdef _DEBUG
						Zebra::logger->debug("棍子增强召唤兽攻击力 （%u-%u)",packs.equip.getEquips().get_appendminpet(),packs.equip.getEquips().get_appendmaxpet());
#endif
						summon->setAppendDamage((WORD)(packs.equip.getEquips().get_appendminpet()*(1+packs.equip.getEquips().get_mdam()/100.0f)),
							(WORD)(packs.equip.getEquips().get_appendmaxpet()*(1+packs.equip.getEquips().get_mdam()/100.0f)));
						this->usm.refresh(); // 装备改变可能会影响技能等级
						return;
					}
				}
			}
		}
		summon->setAppendDamage(0,0);
	}
	else
	{
		if (skillValue.introject_maxhp!=0)
		{
			this->skillStatusM.clearRecoveryElement(173);
			this->changeAndRefreshHMS();
		}
	}

	GetIncrementBySuit();


	this->usm.refresh(); // 装备改变可能会影响技能等级
}
#endif
/**
* \brief 获得宠物类的附加攻击力
*/
void SceneUser::getSummonAppendDamage(WORD &minDamage,WORD &maxDamage)
{
#if 0
	zObject *temp;
	if (packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDR))
	{
		if (temp)
		{
			if (0 != temp->data.dur)
			{
				if (temp->base->kind == ItemType_Stick)
				{
#ifdef _DEBUG
					Zebra::logger->debug("棍子增强召唤兽攻击力 （%u-%u)",packs.equip.getEquips().get_appendminpet(),packs.equip.getEquips().get_appendmaxpet());
#endif
					minDamage = (WORD)(packs.equip.getEquips().get_appendminpet()*(1+packs.equip.getEquips().get_mdam()/100.0f));
					maxDamage = (WORD)(packs.equip.getEquips().get_appendmaxpet()*(1+packs.equip.getEquips().get_mdam()/100.0f));
					return;
				}
			}
		}
	}

	minDamage = 0;
	maxDamage = 0;
#endif
}

/**
* \brief 增加经验值
* \param num 数量
* \param addPet 是否给宠物加经验
* \param dwTempID 经验值来源者的temp id
* \param byType 经验值来源的类型 enum enumMapDataType
* \param addPet 是否给宠物增加
* \param addCartoon 是否给卡通宠物加经验
* \param addPet 是否给替身宠物增加
*/
void SceneUser::addExp(DWORD num,bool addPet,DWORD dwTempID,BYTE byType,bool addCartoon)
{
#if 0
	if (0==num) num = 1;

	charbase.exp += num * ScenesService::getInstance().ExpRate;

	Cmd::stObtainExpUserCmd ret;
	ret.dwTempID = dwTempID;        /** 经验值来源临时编号 */
	ret.byType = byType;        /** 经验值来源 enumMapDataType */
	ret.dwExp = num;
	if (this->charbase.exp >= this->charstate.nextexp)
	{
		ret.dwUserExp = this->charbase.exp-this->charstate.nextexp;
	}
	else
	{
		ret.dwUserExp = this->charbase.exp;
	}

#ifdef _DEBUG
	Zebra::logger->info("[发送经验增加通知]获得经验：%u 用户当前经验：%u",ret.dwExp,ret.dwUserExp);
#endif
	this->sendCmdToMe(&ret,sizeof(ret));

	if(charbase.level< max_level)
	{

		if (charbase.exp >= charstate.nextexp) {
			upgrade();
		}
	}

	addPetExp(num,addPet,addCartoon);
	if(charbase.level == max_level)
	{
		Cmd::stMainUserDataUserCmd  userinfo;
		full_t_MainUserData(userinfo.data);
		Zebra::logger->error("sizeof QWORD %d,%d",sizeof(QWORD),sizeof(userinfo.data.nextexp));
		userinfo.data.nextexp = max_exp;
		sendCmdToMe(&userinfo,sizeof(userinfo));
	}
#endif
}

/**
* \brief 增加所有宠物的经验
*
* \param num 数量
* \param addPet 是否给宠物增加
* \param addCartoon 是否给替身增加
*/
void SceneUser::addPetExp(DWORD num,bool addPet,bool addCartoon)
{
#if 0
	if (pet && addPet)
		pet->addExp(num);
	if (summon)
		summon->addExp(num);
	if (cartoon && addCartoon)
	{
		cartoon->addExp(num*4/10);
		cartoonList[cartoon->getCartoonID()] = cartoon->getCartoonData();
	}
#endif
}

/**
* \brief 交换自己与宠物的位置
*/
void SceneUser::exchangeMeAndSummonPet()
{
#if 0
	if (summon)
	{
		if (summon->scene == this->scene)
		{
			if (this->scene->zPosShortRange(this->getPos(),summon->getPos(),SCREEN_WIDTH*3,SCREEN_HEIGHT*3))
			{
				zPos myPos = this->pos;
				zPos petPos = summon->getPos();

				this->scene->clearBlock(petPos);
				Cmd::stRemoveMapNpcMapScreenUserCmd send;
				send.dwMapNpcDataPosition = summon->tempid;
				this->sendCmdToMe(&send,sizeof(send));

				zPos vPos = summon->getPos();
				this->goTo(vPos);

				summon->warp(myPos,true);
			}
			else
				Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你和召唤兽之间的距离过远!");
		}
		else
		{
			Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你和召唤兽不在一张地图内不可以交换位置!");
		}
	}
	else
	{
		Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"没有召唤兽可以交换位置!");
	}
#endif
}

/**
* \brief 返回当前法术值
* \return 当前法术值
*/
DWORD SceneUser::getMP()
{
	return this->charbase.mp;
}

/**
* \brief 清除当前法术值
*/
void SceneUser::clearMana()
{
	this->charbase.mp = 0;
	//this->checkAutoMP();
}

#if 0
/**
* \brief 向9屏发送宠物信息
*
*/
void SceneUser::sendPetDataToNine()
{
	if (ridepet) ridepet->sendPetDataToNine();
	if (guard) guard->sendPetDataToNine();
	if (cartoon) cartoon->sendPetDataToNine();
	for (adopt_it it=adoptList.begin(); it!=adoptList.end(); it++)
		it->second->sendPetDataToNine();
	SceneEntryPk::sendPetDataToNine();
}
#endif

DWORD SceneUser::getFiveType() const
{
	return charbase.fivetype;
}

DWORD SceneUser::getFivePoint() const
{
	return charbase.fivelevel*5+5;
}

DWORD SceneUser::getFiveLevel() const
{
	return charbase.fivelevel;
}

#if 0
/*
* \brief 尝试捕获一只npc
*
* \param npc 要抓的npc
* \param type 要抓的npc类型
*
* \return 是否成功
*
*/
bool SceneUser::captureIt(SceneNpc *npc,BYTE type)
{
	if (!npc) return false;
	if (type!=npc->npc->bear_type) return false;
	if (npc->npc->level>charbase.level) return false;
	if (npc->getPetType()!=Cmd::PET_TYPE_NOTPET) return false;

	ScenePet * newPet = summonPet(npc->npc->id,Cmd::PET_TYPE_PET,0,0,0,0,npc->getPos());
	if (newPet)
	{
		npc->hp = npc->npc->hp;
		npc->hideMe(60000);

		newPet->petData.state = Cmd::PET_STATE_NORMAL;
		newPet->getAbilityByLevel(newPet->npc->level);
		bcopy(&newPet->petData,&petData,sizeof(petData));
		newPet->sendData();
		return true;
	}
	return false;
}
#endif
bool SceneUser::speedOutMove(DWORD time,WORD speed,int len)
{
#if 0
	//zRTime ctv;
	DWORD steptime=300;

	if (horse.mount())
	{
		steptime=200;
	}

	steptime = (DWORD)((steptime*speed)/640.0f);
	if (len == 0) len=1;
	if ((time - lastMoveTime)/len > steptime)
	{
		lastMoveTime = time;
		if (moveFastCount>0) moveFastCount--;
		return true;
	}
	else
	{
		if (moveFastCount<5)
		{
			lastMoveTime = time;
			moveFastCount++;
			return true;
		}
		else
			if (moveFastCount<10000) moveFastCount++;
	}
#endif
	return false;
}

#if 0
/**
* \brief 根据装备上带的几率增加装备指定数值的血和蓝,根据对敌伤害增加自身数值
* \param rev 争夺战开始通知消息
*/
void SceneUser::leech(DWORD dwDam)
{
	if (zMisc::selectByPercent(packs.equip.getEquips().get_hpleech_odds()))
	{
		this->changeHP(packs.equip.getEquips().get_hpleech_effect());
	}
	if (zMisc::selectByPercent(packs.equip.getEquips().get_mpleech_odds()))
	{
		this->changeMP(packs.equip.getEquips().get_mpleech_effect());
	}
	if (skillValue.attackaddhpnum > 0)
	{
		this->changeHP((SDWORD)(dwDam *(skillValue.attackaddhpnum/100.0f)));
	}
}

/**
* \brief 处理npc争夺战状态检查消息设置争夺状态标志
* \param rev 争夺战开始通知消息
*/
void SceneUser::checkNpcDare(Cmd::Session::t_NpcDare_NotifyScene_SceneSession * rev)
{
	//by RAY 去掉NPC争夺
	//  Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"争夺NPC正在开发中！");

	//#if 0
	if (scene)
	{
		if (charbase.level >19 &&
			rev->dwCountryID == scene->getCountryID() &&
			rev->dwMapID == scene->getRealMapID() &&
			abs((long)(rev->dwPosX - this->pos.x))<=26 &&
			abs((long)(rev->dwPoxY - this->pos.y))<=38)
		{
			if (this->charbase.level >=60)
			{
				DWORD mapid = this->scene->getRealMapID();
				if (mapid == 101 ||
					mapid == 102 ||
					mapid == 104)
				{
					/// 超过59的玩家不允许在凤凰城,凤尾村和清源村进行NPC争夺战。
					npcdareflag = false;
					return;
				}
			}
			npcdareflag = true;
			npcdarePosX = rev->dwPosX;
			npcdarePosY = rev->dwPoxY;
			npcdareCountryID = rev->dwCountryID;
			npcdareMapID = rev->dwMapID;
			npcdareNotify = true;
			Zebra::logger->info("[家族争夺NPC]家族[%u]中的角色[%s]进入NPC争夺对战状态",this->charbase.septid,this->name);
			return;
		}
	}
	npcdareflag = false;
	//#endif
}

/**
* \brief 实时检查与被争夺NPC的距离,如果超过距离则取消状态,如果离边界太近则提示
*/
void SceneUser::checkNpcDareState()
{
	if (scene&&npcdareflag)
	{
		if (npcdareCountryID == scene->getCountryID() &&
			npcdareMapID == scene->getRealMapID() &&
			abs((long)(npcdarePosX - this->pos.x))<=26 &&
			abs((long)(npcdarePosY - this->pos.y))<=38)
		{
			if ((abs((long)(npcdarePosX - this->pos.x))>=20 ||
				abs((long)(npcdarePosY - this->pos.y))>=30)&&
				npcdareNotify)
			{
				npcdareNotify = false;
				Channel::sendSys(this,Cmd::INFO_TYPE_EXP,"你距离战场边界太近,交战时间没结束,再往外走将脱离战斗！");
			}
			else
			{
				npcdareNotify = true;
			}
		}
		else
		{
			Channel::sendSys(this,Cmd::INFO_TYPE_EXP,"你离开战场已经失去对战资格,这也许会导致你的家族战败！！！");
			Zebra::logger->info("[家族争夺NPC]家族[%u]中的角色%s因为走出战场失去参战资格",this->charbase.septid,this->name);
			npcdareflag = false;
			this->removeWarRecord(Cmd::SEPT_NPC_DARE);

			this->sendNineToMe();
			this->sendMeToNine();
		}
	}
}

/**
* \brief 实时检查与被争夺NPC的距离,如果超过距离则取消状态,如果离边界太近则提示
*/
void SceneUser::notifySessionNpcDareResult()
{
	if (npcdareflag)
	{
		npcdareflag = false;
		if (this->getState() != SceneUser::SceneEntry_Death)
		{
			Cmd::Session::t_NpcDare_Result_SceneSession send;
			send.dwSeptID = this->charbase.septid;
			sessionClient->sendCmd(&send,sizeof(send));
			Zebra::logger->info("[家族争夺NPC]家族[%u]中的角色%s还活着向会话发出战果统计",this->charbase.septid,this->name);
		}
	}
}

/**
* \brief 商人向角色交保护费
* \param dwGold 保护费的数额
*/
void SceneUser::npcDareGetGold(DWORD dwGold)
{
	Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"管理费发出");
	this->packs.addMoney(dwGold,"npc对战商人保护费");
}
#endif
/**
* \brief 发送消息给自己
* \param pattern 消息体
*/
void SceneUser::sendMessageToMe(const char *pattern)
{
	Channel::sendSys(this,Cmd::INFO_TYPE_GAME,pattern);
}

#if 0
/**
* \brief 让宠物重生
*/
void SceneUser::relivePet()
{
	if (!pet&&petData.id!=0)
	{
		petData.state = Cmd::PET_STATE_NORMAL;

		ScenePet * newPet = NULL;
		if (0==strncmp(petData.name,"",MAX_NAMESIZE))
			newPet = summonPet(petData.id,Cmd::PET_TYPE_PET);
		else
			newPet = summonPet(petData.id,Cmd::PET_TYPE_PET,0,0,petData.name);

		if (newPet)
		{
			bcopy(&petData,&newPet->petData,sizeof(Cmd::t_PetData));
			newPet->getAbilityByLevel(petData.lv);
			newPet->hp = petData.maxhp;
			newPet->setPetAI((Cmd::petAIMode)petData.ai);
			newPet->sendData();
			newPet->sendMeToNine();
		}
	}
}

/**
* \brief 让宠物回到主人身边
*/
void SceneUser::collectPets()
{
	for (std::list<ScenePet *>::iterator it=totems.begin(); it!=totems.end(); it++)
	{
		if ((*it)->canMove())
			(*it)->changeMap(scene,getPos());
	}

	if (ridepet && ridepet->canMove())
		ridepet->changeMap(scene,getPos());
	if (pet && pet->canMove())
		pet->changeMap(scene,getPos());
	if (summon && summon->canMove())
		summon->changeMap(scene,getPos());
	if (guard && guard->canMove())
		guard->changeMap(scene,getPos());
}

bool SceneUser::unCombin(Cmd::stUnCombinUserCmd *rev)
{
	switch (rev->type)
	{
	case Cmd::UN_STATE_COMBIN:
		{
			this->skillStatusM.clearRecoveryElement(173);
		}
		break;
	case Cmd::UN_STATE_CHANGE_FACE:
		{
			this->skillStatusM.clearRecoveryElement(132);
			this->skillStatusM.clearRecoveryElement(16);
		}
		break;
	case Cmd::UN_STATE_TEAM_ATTACK_BIRD:
		{
			this->skillStatusM.clearActiveElement(230);
		}
		break;
	case Cmd::UN_STATE_TEAM_ATTACK_FLOW:
		{
			this->skillStatusM.clearActiveElement(238);
		}
		break;
	case Cmd::UN_STATE_TEAM_ATTACK_ONE_DIM:
		{
			this->skillStatusM.clearActiveElement(264);
		}
		break;
	default:
		break;
	}
	this->changeAndRefreshHMS();
	return true;
}
#endif
/**
* \brief 获得当前魔法攻击力
* \return 魔法攻击力
*/
DWORD SceneUser::getMaxMDamage()
{
	return charstate.maxpdamage;
}

/**
* \brief 获得当前物理攻击力
* \return 物理攻击力
*/
DWORD SceneUser::getMaxPDamage()
{
	return charstate.maxpdamage;
}

/**
* \brief 获得当前物理防御力
* \return 物理防御力
*/
DWORD SceneUser::getPDefence()
{
	return charstate.pdefence;
}

/**
* \brief 获得当前魔法防御力
* \return 魔法防御力
*/
DWORD SceneUser::getMDefence()
{
	return charstate.mdefence;
}

/**
* \brief 更新用户数据到会话
*/
void SceneUser::updateUserData()
{
	Cmd::Session::t_changeUserData_SceneSession send;
	send.wdLevel = this->charbase.level;
	send.dwExploit = this->charbase.exploit;
	send.dwUserID = this->id;
	sessionClient->sendCmd(&send,sizeof(send));
}

#if 0
/**
* \brief 加入一条新的对战记录
*
*
* \param type 对战类型
* \param relationid 对方社会类型ID
* \param isAtt 在该对战中,自己是否属于攻击方
* \return 
*/
void SceneUser::addWarRecord(DWORD type,DWORD relationid,bool isAtt)
{       
	if (!this->isWarRecord(type,relationid))
	{
		WarRecord record;
		record.type = type; 
		record.relationid = relationid;
		record.isAtt = isAtt;
		vWars.push_back(record);
	}
}

/**             
* \brief 清除指定的对战记录
*              
*      
* \param type 对战类型
* \return 敌对方社会关系ID
*/     
void SceneUser::removeWarRecord(DWORD type,DWORD relationid)
{
	WarIter iter;
	for (iter = vWars.begin(); iter!=vWars.end();)
	{       
		if (iter->type == type && (iter->relationid == relationid || relationid==0))
		{
			iter = vWars.erase(iter);
		} 
		else {
			iter++;
		}
	}       

	if (iter!=vWars.end())
	{       
		vWars.erase(iter);
	}
}

void SceneUser::setAntiAttState(DWORD type,DWORD relationid)
{
	WarIter iter;
	for (iter = vWars.begin(); iter!=vWars.end(); iter++)
	{       
		if (iter->type == type && iter->relationid == relationid)
		{
			break;
		} 
	}       

	if (iter!=vWars.end())
	{       
		(*iter).isAntiAtt = true;
	}
}

bool SceneUser::isWarRecord(DWORD type,DWORD relationid)
{
	WarIter iter;
	for (iter = vWars.begin(); iter!=vWars.end(); iter++)
	{       
		if ((iter->type == type || (type == Cmd::COUNTRY_FORMAL_DARE && iter->type==Cmd::COUNTRY_FORMAL_ANTI_DARE) )
			&& iter->relationid == relationid)
		{
			break;
		} 
	}       

	if (iter!=vWars.end())
	{       
		return true;
	}

	return false;
}

bool SceneUser::isAtt(DWORD dwType,DWORD relationid)
{
	WarIter iter;
	for (iter = vWars.begin(); iter!=vWars.end(); iter++)
	{       
		if ((iter->type == dwType || (dwType == Cmd::COUNTRY_FORMAL_DARE && iter->type==Cmd::COUNTRY_FORMAL_ANTI_DARE) ) 
			&& (iter->relationid == relationid || relationid==0))
		{
			break;
		} 
	}       

	if (iter!=vWars.end())
	{       
		return iter->isAtt;
	}

	return false;
}

bool SceneUser::isAntiAtt(DWORD dwType,DWORD relationid)
{
	WarIter iter;
	for (iter = vWars.begin(); iter!=vWars.end(); iter++)
	{       
		if ((iter->type == dwType || (dwType == Cmd::COUNTRY_FORMAL_DARE && iter->type==Cmd::COUNTRY_FORMAL_ANTI_DARE) ) 
			&& (iter->relationid == relationid || relationid==0))
		{
			break;
		} 
	}       

	if (iter!=vWars.end())
	{       
		return iter->isAntiAtt;
	}

	return false;

}

/**
* \brief 把宠物抓到自己跟前
*/
void SceneUser::catchMyPet()
{
	if (guard) guard->warp(this->getPos(),true);
	if (pet) pet->warp(this->getPos(),true);
	if (summon) summon->warp(this->getPos(),true);
}
#endif
/**
* \brief 聊天自动回复
* \param toName 回复给谁
*/
void SceneUser::autoReply(char * toName) const 
{
	if (!toName) return;

	zRTime ctv;
	Cmd::stChannelChatUserCmd send;
	send.dwType=Cmd::CHAT_TYPE_AUTO;
	send.dwCharType = charbase.face;
	send.dwChatTime = ctv.sec();
	strncpy((char *)send.pstrChat,(char *)replyText,MAX_CHATINFO-1);
	strncpy((char *)send.pstrName,name,MAX_NAMESIZE);

	sendCmdByName(toName,&send,sizeof(send));
}

#if 0
bool SceneUser::isSafety(BYTE byType)
{
	if (this->safety==1 && this->temp_unsafety_state==0)
	{
		if (Cmd::isset_state((BYTE*)&this->safety_setup,byType))
			return true;
	}

	return false;
}
#endif
//sky 检测玩家的战斗状态
useFightState SceneUser::IsPveOrPvp()
{
	return isUsePveOrPvp;
}

//sky 设置玩家的战斗状态
void SceneUser::SetPveOrPvp(useFightState type)
{
	isUsePveOrPvp = type;
}


//sky 重新设置玩家进入战斗的时间
void SceneUser::SetPkTime(DWORD time)
{
	PkTime = time;
}

//sky 判断是否玩家能脱离战斗
bool SceneUser::IsPkTimeOver()
{
	PkTime--;
	if(PkTime == 0)
		return true;

	return false;
}

/**
* \brief 给该玩家发送物品,通过邮件
*
* \param fromName 发送人
* \param toName 接受者名字
* \param type 邮件类型
* \param money 发送金钱
* \param 发送物品
* \param 文字内容
*/
void sendMail(char * fromName,DWORD fromID,char * toName,DWORD toID,BYTE type,DWORD money,/*zObject * o,*/char * text)
{
	if (!fromName||!toName) return;

	Cmd::Session::t_sendMail_SceneSession sm;
	sm.mail.state = Cmd::Session::MAIL_STATE_NEW;
	strncpy(sm.mail.fromName,fromName,MAX_NAMESIZE);
	sm.mail.fromID = fromID;
	sm.mail.toID = toID;
	strncpy(sm.mail.toName,toName,MAX_NAMESIZE);
	strncpy(sm.mail.title,"系统发送的邮件",MAX_NAMESIZE);
	sm.mail.type = type;
	zRTime ct;
	sm.mail.createTime = ct.sec();
	sm.mail.delTime = sm.mail.createTime + 60*60*24*7;
	snprintf(sm.mail.text, 255, text);
	sm.mail.sendMoney = money;
	sm.mail.recvMoney = 0;
	sm.mail.sendGold = 0;
	sm.mail.recvGold = 0;
	sm.mail.itemGot = 0;
#if 0
	if (o)
	{
		//o->getSaveData((SaveObject *)&sm.item);
		bcopy(&o->data,&sm.item.object,sizeof(t_Object));
		sm.mail.itemID = o->data.qwThisID;
	}

	if (money || o) sm.mail.accessory = 1;
#endif
	sessionClient->sendCmd(&sm,sizeof(sm));
	Zebra::logger->info("[mailSystem]���͸���� %s  money=%u",toName,money);
}
#if 0
bool SceneUser::canVisitNpc(SceneNpc *pNpc)
{
	//不在同一地图不可以访问
	if (!pNpc || !pNpc->scene || pNpc->scene != this->scene)
	{
		return false;
	}
	//本国人都可以访问
	if (pNpc->scene->getCountryID() == charbase.country )
	{
		return true;
	}
	//盟国访问
	if (pNpc->npc->allyVisit && 
		(CountryAllyM::getMe().getFriendLevel(pNpc->scene->getCountryID(),charbase.country) >= pNpc->npc->allyVisit))
		return true;

	//易容术可以访问
	if (changeface)
	{
		return true;
	}

	//5000-59999范围的外国人也可以访问
	if (pNpc->id>=5000&&pNpc->id<=5999)
	{
		return true;
	}

	if (pNpc->id>=7000&&pNpc->id<=7100)
	{
		return true;
	}

	//6000外国人非国战时间可以访问,国战期间不可以访问
	if (pNpc->id==6000 && !this->isSpecWar(Cmd::COUNTRY_FORMAL_DARE))
	{
		return true;
	}

	return false;
}
#endif
void SceneUser::setNpcHoldData(Cmd::Session::t_notifyNpcHoldData *rev)
{
	this->npcHoldData.dwPosX = rev->dwPosX;
	this->npcHoldData.dwPosY = rev->dwPosY;
	this->npcHoldData.dwMapID = rev->dwMapID;
}

void SceneUser::checkNpcHoldDataAndPutExp()
{

	Zebra::logger->debug("checkNpcDataAndPutExp");
	if (this->isSitdown())
	{
		if (this->scene->getRealMapID() != npcHoldData.dwMapID) return;
		if (abs((int)npcHoldData.dwPosX - (int)this->getPos().x)<=26 &&
			abs((int)npcHoldData.dwPosY - (int)this->getPos().y)<=38)
		{
			DWORD dwExp =  (DWORD)((1.4*charbase.level*charbase.level+5*charbase.level)/6/10+1);
			Zebra::logger->debug("exp = %u",dwExp);
			this->addExp(dwExp);
		}
	}
}

void SceneUser::checkPunishTime()
{
	if (charbase.punishTime>0)
		charbase.punishTime--;
}

#if 0
const TeamMember *SceneUser::getMember(WORD index)
{	
	TeamManager * team = SceneManager::getInstance().GetMapTeam(TeamThisID);

	if (team)
	{
		return team->getTeam().getTeamMember(index);
	}
	else
		return NULL;
}


DWORD SceneUser::refreshPetPackSize()
{
	DWORD levelCount = 0;
	for (cartoon_it it=cartoonList.begin(); it!=cartoonList.end(); it++)
		levelCount += it->second.lv;

	PetPack *pack = (PetPack *)packs.getPackage(Cmd::OBJECTCELLTYPE_PET,0);
	if (!pack) return false;

	DWORD cellCount = cartoonList.size() + levelCount/10;
	pack->setAvailable(cellCount);

	charbase.petPack = (cellCount+1>80) ? 80 : (cellCount+1);//多一个格子,避免宠物回档造成物品丢失

#ifdef _DEBUG
	Zebra::logger->debug("%s 宠物包裹大小 %u",name,cellCount);
#endif
	return cellCount;
}

void SceneUser::setDiplomatState(BYTE newstate)
{
	if (newstate == 0)
	{
		isDiplomat = false;
	}
	else
	{
		isDiplomat = true;
	}
}

int SceneUser::isDiplomatState() // 返回0为外交官状态,返回1为非外交官状态,返回2为是外交官但因为有采集手套,暂时无效
{
	if (isDiplomat)
	{
		zObject *temp=NULL;
		if (this->packs.equip.getObjectByZone(&temp,0,Cmd::EQUIPCELLTYPE_HANDR))
		{
			if (temp)
			{
				if (0 != temp->data.dur)
				{
					if (temp->base->id == 876) //采集手套
					{
						return 2;
					}
				}
			}
		}

		return 0;
	}

	return 1;
}

void SceneUser::setCatcherState(BYTE newstate)
{
	if (newstate == 0)
	{
		isCatcher = false;
	}
	else
	{
		isCatcher = true;
	}
}

bool SceneUser::isCatcherState() const
{
	return isCatcher;
}

struct TotalSeptGuard : public zSceneEntryCallBack
{
private:
	SceneUser* master;
	BYTE cmd_type;

public:
	TotalSeptGuard(SceneUser* u,BYTE cmd=0)
	{
		master = u;
		cmd_type = cmd;
	}

	bool exec(zSceneEntry *entry)
	{
		if (cmd_type == 0)
		{// 开始时统计人数
			if (entry && master->charbase.septid==((SceneUser*)entry)->charbase.septid)
			{
				SceneUser* u = (SceneUser*)entry;
				DWORD money = 0;

				if (u->charbase.level<50)
				{
					money = 20*100;
				}
				else if (u->charbase.level>=50 && u->charbase.level<70)
				{
					money = 60*100;
				}
				else if (u->charbase.level>=70 && u->charbase.level<90)
				{
					money = 100 * 100;
				}
				else if (u->charbase.level>=90 && u->charbase.level<110)
				{
					money = 140 * 100;
				}
				else if (u->charbase.level>=110 && u->charbase.level<130)
				{
					money = 180 * 100;
				}
				else if (u->charbase.level>=130)
				{
					money = 220 * 100;
				}

				if (u->packs.checkMoney(money) && u->packs.removeMoney(money,"家族运镖押金"))
				{
					SeptGuard tmp;
					tmp.id = u->id;
					tmp.money = money;
					tmp.is_finish = false;
					master->venterSeptGuard.push_back(tmp);

					Channel::sendSys(u,Cmd::INFO_TYPE_GAME,"扣除运镖押金 %u 文",money);
				}
				else
				{
					Channel::sendSys(u,Cmd::INFO_TYPE_FAIL,"银两不足 %u,不能进入家族运镖",money);
				}
			}
		}
		else if (cmd_type == 1)
		{//结束时统计人数(首先查找,如果在enterSeptGuard中找到,则加到完成人员列表中)
			for (DWORD i=0; i<master->venterSeptGuard.size(); i++)
			{
				if (((SceneUser*)entry)->id == master->venterSeptGuard[i].id)
				{
					master->venterSeptGuard[i].is_finish = true;
					break;
				}
			}
		}

		return true;
	}
};

void SceneUser::enterSeptGuard()
{
	TotalSeptGuard sept(this);
	const zPosIVector &pv = scene->getNineScreen(getPosI());

	for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
	{
		scene->execAllOfScreen(SceneEntry_Player,*it,sept);
	}

	SceneManager::CountryMap_iter country_iter = SceneManager::getInstance().country_info.find(this->charbase.country);
	if (country_iter != SceneManager::getInstance().country_info.end())
	{
		Channel::sendAllInfo(Cmd::INFO_TYPE_EXP," %s 的 %s 家族 开始家族运镖",country_iter->second.name,this->septName);
	}
}

void SceneUser::finishSeptGuard()
{
	TotalSeptGuard sept(this,1);
	const zPosIVector &pv = scene->getNineScreen(getPosI());

	for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
	{
		scene->execAllOfScreen(SceneEntry_Player,*it,sept);
	}

	int finishnum = 0;
	for (DWORD i=0; i<venterSeptGuard.size(); i++)
	{
		if (venterSeptGuard[i].is_finish)
			finishnum++;
	}

	for (DWORD i=0; i<venterSeptGuard.size(); i++)
	{
		SceneUser* pUser = SceneUserManager::getMe().getUserByID(venterSeptGuard[i].id);
		if (pUser && venterSeptGuard[i].is_finish)
		{  
			DWORD addExp = (80+5*finishnum)*pUser->charbase.level*pUser->charbase.level;
			pUser->addExp(addExp);
			Channel::sendSys(pUser,Cmd::INFO_TYPE_EXP,"得到经验值 %d",addExp);
			pUser->packs.addMoney(venterSeptGuard[i].money,"返还家族运镖押金");
		}
	}

	venterSeptGuard.clear();
}


//sky 使用物品触发技能函数
bool SceneUser::ItemUseSkill(zObject * obj, DWORD preUserID)
{
	if(obj)
	{
		if(0==obj) return false;

		if (this->isSitdown())
		{
			standup();
		}

		Cmd::stAttackMagicUserCmd cmd;

		cmd.byAttackType = Cmd::ATTACKTYPE_U2U;

		if(this->tempid != preUserID)
		{
			if(scene->getUserByTempID(preUserID))
				cmd.byAttackType = Cmd::ATTACKTYPE_U2U;
			else if(SceneNpcManager::getMe().getNpcByTempID(preUserID))
				cmd.byAttackType = Cmd::ATTACKTYPE_U2N;
			else
			{
				Channel::sendSys(this, Cmd::INFO_TYPE_FAIL, "目标已经消失!无法对其使用物品");
				return false;
			}
		}

		cmd.dwDefenceTempID = preUserID;

		cmd.dwUserTempID = this->tempid;
		cmd.wdMagicType = obj->base->leechdom.id;
		cmd.byAction = Cmd::Ani_Num;
		cmd.byDirect = this->getDir();

		zSkill *s = NULL;

		s = zSkill::createTempSkill(this, obj->base->leechdom.id, obj->base->leechdom.effect);
		if (s)
		{
			s->action(&cmd,sizeof(cmd));
			SAFE_DELETE(s);
		}

		//sky 物品使用成功向客户端发送成功消息
		Cmd::stItemUseItemSuccessUserCmd SuccessSend;
		SuccessSend.ItemID = obj->base->id;
		sendCmdToMe(&SuccessSend, sizeof(Cmd::stItemUseItemSuccessUserCmd));

		if(--obj->data.dwNum)
		{
			Cmd::stRefCountObjectPropertyUserCmd send;
			send.qwThisID=obj->data.qwThisID;
			send.dwNum=obj->data.dwNum;
			sendCmdToMe(&send,sizeof(send));
		}
		else
		{
			return packs.removeObject(obj);
		}

		return true;
	}
}

//sky 保存和读取冷却时间的临时档案
DWORD SceneUser::saveItemCoolTimes(BYTE *data,DWORD maxSize)
{
	int count = m_ItemCoolTimes.vCoolTimeType.size();
	*(int*)data = count;

	data += sizeof(int);

	memcpy(data, &(m_ItemCoolTimes.vCoolTimeType[0]), sizeof(stCoolTimeType)*count,maxSize);
	return sizeof(stCoolTimeType)*count + sizeof(int);
	
}

DWORD SceneUser::loadItemCoolTimes(BYTE *data)
{
	int count = *(int*)data;

	m_ItemCoolTimes.vCoolTimeType.resize(count);

	memcpy(&(m_ItemCoolTimes.vCoolTimeType[0]), data+sizeof(int),  sizeof(stCoolTimeType)*count,  sizeof(stCoolTimeType)*count);

	return sizeof(stCoolTimeType)*count + sizeof(int);
}




/**
* \ SHX 计算装备中套装附加的属性
* \param st 保存到
* \param nSuit 套装ID
* \param nPart 有效部件数;
* \return 增加的字节
* */
void Add_SuitPPT(stIncrementBySuit& st, int nSuit, int nPart)
{
	if(nSuit < 0 || nPart < 1 || nSuit > vXmlSuitAttribute.size() -1)
	{
		return;
	}
	stxml_SuitAttribute& rSuit = vXmlSuitAttribute[nSuit];


	if(nPart > rSuit.count )
	{
		nPart = rSuit.count;
	}

	for(int i = 0; i < rSuit.eCount; i ++)
	{
		if(nPart >= rSuit.EffectList[i].eRequire)
		{
			switch( rSuit.EffectList[i].eKey)
			{
			case	101	:	//	力量
				st.x_str += rSuit.EffectList[i].eValue;
				continue;
			case	102	:	//	智力
				st.x_inte += rSuit.EffectList[i].eValue;
				continue;
			case	103	:	//	敏捷	
				st.x_dex += rSuit.EffectList[i].eValue;
				continue;
			case	104	:	//	精神
				st.x_spi += rSuit.EffectList[i].eValue;
				continue;
			case	105	:	//	耐力
				st.x_con += rSuit.EffectList[i].eValue;
				continue;	
			case	106	:	//	增加物理攻击力
				st.x_pdam += rSuit.EffectList[i].eValue;
				continue;
			case	107	:	//	增加物理防御力
				st.x_pdef += rSuit.EffectList[i].eValue;
				continue;
			case	108	:	//	增加魔法攻击力
				st.x_mdam += rSuit.EffectList[i].eValue;
				continue;
			case	109	:	//	增加魔法防御力
				st.x_mdef += rSuit.EffectList[i].eValue;
				continue;	
			case	110	:	//	增加物理攻击力%
				st.x_p_pdam += rSuit.EffectList[i].eValue;
				continue;
			case	111	:	//	增加物理防御力%
				st.x_p_pdef += rSuit.EffectList[i].eValue;
				continue;
			case	112	:	//	增加魔法攻击力%
				st.x_p_mdam += rSuit.EffectList[i].eValue;
				continue;
			case	113	:	//	增加魔法防御力%
				st.x_p_mdef += rSuit.EffectList[i].eValue;
				continue;
			case	114	:	//	攻击速度
				st.x_akspeed += rSuit.EffectList[i].eValue;
				continue;
			case	115	:	//	移动速度
				st.x_mvspeed += rSuit.EffectList[i].eValue;
				continue;
			case	116	:	//	命中率
				st.x_atrating += rSuit.EffectList[i].eValue;
				continue;
			case	117	:	//	躲避率
				st.x_akdodge += rSuit.EffectList[i].eValue;
				continue;
			case	118	:	//	生命值恢复
				st.x_hpr += rSuit.EffectList[i].eValue;
				continue;
			case	119	:	//	法术值恢复
				st.x_mpr += rSuit.EffectList[i].eValue;
				continue;
			case	120	:	//	爆击
				st.x_bang += rSuit.EffectList[i].eValue;
				continue;
			case	121	:	//	x%吸收生命值y
				st.x_hpleech += rSuit.EffectList[i].eValue;
				continue;
			case	122	:	//	x%吸收法术值y
				st.x_mpleech += rSuit.EffectList[i].eValue;
				continue;
			case	123	:	//	增加金钱掉落x%
				st.x_incgold += rSuit.EffectList[i].eValue;
				continue;
			case	124	:	//	x%双倍经验    
				st.x_doublexp += rSuit.EffectList[i].eValue;
				continue;

			case	125	:	//	眩晕
				st.x_giddy += rSuit.EffectList[i].eValue;
				continue;
			case	126	:	//	昏迷
				st.x_coma += rSuit.EffectList[i].eValue;
				continue;
			case	127	:	//	定身
				st.x_halt += rSuit.EffectList[i].eValue;
				continue;
			case	128	:	//	恐惧
				st.x_dread += rSuit.EffectList[i].eValue;
				continue;
			case	129	:	//	减速
				st.x_slowdown += rSuit.EffectList[i].eValue;
				continue;
			case	130	:	//	放逐
				st.x_banish += rSuit.EffectList[i].eValue;
				continue;
			case	131	:	//	防眩晕
				st.x_giddy_def += rSuit.EffectList[i].eValue;
				continue;
			case	132	:	//	防昏迷
				st.x_coma_def += rSuit.EffectList[i].eValue;
				continue;
			case	133	:	//	防定身
				st.x_halt_def += rSuit.EffectList[i].eValue;
				continue;
			case	134	:	//	防恐惧
				st.x_dread_def += rSuit.EffectList[i].eValue;
				continue;
			case	135	:	//	防减速
				st.x_slowdown_def += rSuit.EffectList[i].eValue;
				continue;
			case	136	:	//	防放逐
				st.x_banish_def += rSuit.EffectList[i].eValue;
				continue;
			default:
				printf("Error :套装属性中出现未知代码 name= %s ,ekey = %d",
					rSuit.Name, rSuit.EffectList[i].eKey);
				return;
			}			
		}
	}

}
void SceneUser::GetIncrementBySuit()
{
	
	vector<zObjectB*> EqpList;
	Increment.clear();
	int k = packs.equip.size();
	for(int i = 0; i < packs.equip.size(); i ++)
	{
		zObject* pObj = packs.equip.equip((EquipPack::EQUIPNO)i);
		if(pObj && pObj->base->nSuitData >= 0)
		{
			EqpList.push_back(pObj->base);
		}
	}
	k = EqpList.size();

	vector<zObjectB*>::iterator it = EqpList.begin();

	while (it != EqpList.end())
	{
		int nSuit = (*it)->nSuitData;
		int num = 0;
		for(it = EqpList.begin(); it != EqpList.end();)
		{
		
			if( (*it)->nSuitData == nSuit )
			{
				num ++;
				it = EqpList.erase(it);
				continue;
			}
			else
			{
				it++;
			}
		}
		Add_SuitPPT(Increment, nSuit, num);		

		it = EqpList.begin();
	}
	if(Increment.	x_str	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	力量	增加 %d",	Increment.	x_str	);
	if(Increment.	x_inte	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	智力	增加 %d",	Increment.	x_inte	);
	if(Increment.	x_dex	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	敏捷	增加 %d",	Increment.	x_dex	);
	if(Increment.	x_spi	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	精神	增加 %d",	Increment.	x_spi	);
	if(Increment.	x_con	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	耐力	增加 %d",	Increment.	x_con	);
	if(Increment.	x_pdam	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	增加物理攻击力	增加 %d",	Increment.	x_pdam	);
	if(Increment.	x_pdef	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	增加物理防御力	增加 %d",	Increment.	x_pdef	);
	if(Increment.	x_mdam	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	增加魔法攻击力	增加 %d",	Increment.	x_mdam	);
	if(Increment.	x_mdef	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	增加魔法防御力	增加 %d",	Increment.	x_mdef	);
	if(Increment.	x_p_pdam	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	增加物理攻击力%	增加 %d",	Increment.	x_p_pdam	);
	if(Increment.	x_p_pdef	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	增加物理防御力%	增加 %d",	Increment.	x_p_pdef	);
	if(Increment.	x_p_mdam	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	增加魔法攻击力%	增加 %d",	Increment.	x_p_mdam	);
	if(Increment.	x_p_mdef	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	增加魔法防御力%	增加 %d",	Increment.	x_p_mdef	);
	if(Increment.	x_akspeed	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	攻击速度	增加 %d",	Increment.	x_akspeed	);
	if(Increment.	x_mvspeed	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	移动速度	增加 %d",	Increment.	x_mvspeed	);
	if(Increment.	x_atrating	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	命中率	增加 %d",	Increment.	x_atrating	);
	if(Increment.	x_akdodge	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	躲避率	增加 %d",	Increment.	x_akdodge	);
	if(Increment.	x_hpr	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	生命值恢复	增加 %d",	Increment.	x_hpr	);
	if(Increment.	x_mpr	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	法术值恢复	增加 %d",	Increment.	x_mpr	);
	if(Increment.	x_bang	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	爆击	增加 %d",	Increment.	x_bang	);
	if(Increment.	x_hpleech	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	x%吸收生命值y	增加 %d",	Increment.	x_hpleech	);
	if(Increment.	x_mpleech	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	x%吸收法术值y	增加 %d",	Increment.	x_mpleech	);
	if(Increment.	x_incgold	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	增加金钱掉落x%	增加 %d",	Increment.	x_incgold	);
	if(Increment.	x_doublexp	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	x%双倍经验    	增加 %d",	Increment.	x_doublexp	);
	if(Increment.	x_giddy	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	眩晕	增加 %d",	Increment.	x_giddy	);
	if(Increment.	x_coma	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	昏迷	增加 %d",	Increment.	x_coma	);
	if(Increment.	x_halt	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	定身	增加 %d",	Increment.	x_halt	);
	if(Increment.	x_dread	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	恐惧	增加 %d",	Increment.	x_dread	);
	if(Increment.	x_slowdown	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	减速	增加 %d",	Increment.	x_slowdown	);
	if(Increment.	x_banish	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	放逐	增加 %d",	Increment.	x_banish	);
	if(Increment.	x_giddy_def	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	防眩晕	增加 %d",	Increment.	x_giddy_def	);
	if(Increment.	x_coma_def	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	防昏迷	增加 %d",	Increment.	x_coma_def	);
	if(Increment.	x_halt_def	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	防定身	增加 %d",	Increment.	x_halt_def	);
	if(Increment.	x_dread_def	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	防恐惧	增加 %d",	Increment.	x_dread_def	);
	if(Increment.	x_slowdown_def	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	防减速	增加 %d",	Increment.	x_slowdown_def	);
	if(Increment.	x_banish_def	> 0 )	Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "	防放逐	增加 %d\n ",	Increment.	x_banish_def	);
Channel::sendSys(this, Cmd::INFO_TYPE_SYS, "Finish!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
}

//sky 保存用户战场竞技场相关数据
DWORD SceneUser::saveBattfieldData(BYTE * data)
{
	if (0==data) return 0;
	int num = 0;

	stUserBattfieldData * p = (stUserBattfieldData *)data;

	bcopy(&BattfieldData, p, sizeof(stUserBattfieldData));

	return sizeof(stUserBattfieldData);
}

//sky 读取用户战场竞技场相关数据
DWORD SceneUser::loadBattfieldData(BYTE * data)
{
	stUserBattfieldData * p = (stUserBattfieldData *)data;
	BattfieldData.BattfieldHonor = p->BattfieldHonor;
	BattfieldData.SportsHonor = p->SportsHonor;
	BattfieldData.x = p->x;
	BattfieldData.y = p->y;
	strncpy(BattfieldData.MapName, p->MapName, MAX_NAMESIZE+1);

	return sizeof(stUserBattfieldData);
}
#endif

#ifdef _TEST_DATA_LOG
void SceneUser::readCharTest(Cmd::Record::t_Read_CharTest_SceneRecord *rev)
{
	bcopy(&rev->chartest,&chartest,sizeof(chartest));
}
void SceneUser::writeCharTest(Cmd::Record::enumWriteBackTest_Type type)
{
	using namespace Cmd::Record;
	switch(type)
	{
	case NEWCHAR_WRITEBACK:
		{
			t_Insert_CharTest_SceneRecord ret;
			strncpy(ret.name,charbase.name,MAX_NAMESIZE);
			ret.level=1;
			bzero(&ret.chartest,sizeof(ret.chartest));
			bzero(&chartest,sizeof(chartest));
			recordClient->sendCmd(&ret,sizeof(ret));
		}
		break;
	case LEVELUP_WRITEBACK:
		{
			chartest.upgrade_usetime = charbase.onlinetime - chartest.upgrade_time;
			t_Update_CharTest_SceneRecord ret; 
			strncpy(ret.name,charbase.name,MAX_NAMESIZE);
			ret.level= charbase.level - 1;
			chartest.get_money =packs.getGoldNum() - chartest.money;
			bcopy(&chartest,&ret.chartest,sizeof(chartest));
			recordClient->sendCmd(&ret,sizeof(ret));

			t_Insert_CharTest_SceneRecord ret_1;
			strncpy(ret_1.name,charbase.name,MAX_NAMESIZE);
			ret_1.level=charbase.level;
			bzero(&chartest,sizeof(chartest));
			chartest.upgrade_time = charbase.onlinetime;
			chartest.money=packs.getGoldNum();
			bcopy(&chartest,&ret_1.chartest,sizeof(chartest));
			recordClient->sendCmd(&ret_1,sizeof(ret_1));
		}
		break;
	case DEATH_WRITEBACK:
		{
			t_Update_CharTest_SceneRecord ret; 
			strncpy(ret.name,charbase.name,MAX_NAMESIZE);
			ret.level= charbase.level;
			chartest.death_times++;
			bcopy(&chartest,&ret.chartest,sizeof(chartest));
			recordClient->sendCmd(&ret,sizeof(ret));
		}
		break;
	case HP_WRITEBACK:
		{
			t_Update_CharTest_SceneRecord ret; 
			strncpy(ret.name,charbase.name,MAX_NAMESIZE);
			ret.level = charbase.level;
			chartest.hp_leechdom++;
			bcopy(&chartest,&ret.chartest,sizeof(chartest));
			recordClient->sendCmd(&ret,sizeof(ret));
		}
		break;
	case MP_WRITEBACK:
		{
			t_Update_CharTest_SceneRecord ret; 
			strncpy(ret.name,charbase.name,MAX_NAMESIZE);
			ret.level = charbase.level;
			chartest.mp_leechdom++;
			bcopy(&chartest,&ret.chartest,sizeof(chartest));
			recordClient->sendCmd(&ret,sizeof(ret));
		}
		break;
	case SP_WRITEBACK:
		{
			t_Update_CharTest_SceneRecord ret; 
			strncpy(ret.name,charbase.name,MAX_NAMESIZE);
			ret.level = charbase.level;
			chartest.sp_leechdom++;
			bcopy(&chartest,&ret.chartest,sizeof(chartest));
			recordClient->sendCmd(&ret,sizeof(ret));
		}
		break;
	default:
		break;
	}
}



#endif // _TEST_DATA_LOG测试数据
