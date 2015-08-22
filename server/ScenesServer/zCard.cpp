#include "zCard.h"
#include "zMisc.h"
#include "SceneUser.h"
#include "Chat.h"
#include "zDatabaseManager.h"
#include "Scene.h"
#include "ScenesServer.h"
#include "TimeTick.h"
#include "ChallengeGame.h"
#include "SceneUserManager.h"
#include "CardEffectCfg.h"

/**
* \brief 构造函数
*/
zCard::zCard():zEntry()
{
	createtime=time(NULL);
	base=NULL;
	inserted=false;
	bzero(&data,sizeof(data));
	bzero(&pk, sizeof(pk));
	//data.fivetype = FIVE_NONE;
	fill(data);
	playerID = 0;
	opTimeSequence = 0;
	gameID = 0;
	playingTime = 0;
	skillStatusM.initMe(this);
}

/**
* \brief 生成对象ID
*/
void zCard::generateThisID()
{
	id=zMisc::randBetween(0,1)?zMisc::randBetween(-1000,0x80000000):zMisc::randBetween(1000,0x7FFFFFFE);
	data.qwThisID=id;
}

/**
* \brief 析构从全局物品索引中删除自己,并清空相关属性
*/
zCard::~zCard()
{
	if (inserted)
	{
		gci->removeObject(this->id);
		inserted=false;
		createtime=0;
		base=NULL;
		bzero(&data, sizeof(data));
		bzero(&pk, sizeof(pk));
	}
}

void zCard::fill(t_Card& d)
{

}

/**
* \brief 物品log
* \param createid      物品创建id
* \param objid         物品Thisid
* \param objname       物品名称
* \param num           物品数量
* \param change        物品数量
* \param type          变化类型(2表示上线加载,1表示增,0表示减)
* \param srcid         源id
* \param srcname       源名称
* \param dstid         目的id
* \param dstname       目的名称
* \param action        操作
* \param base          物品基本表里的指针
* \param kind          物品的种类
* \param upgrade       物品的等级（升级的次数）
* \brief    其中后三个参数是用来打印,追加的日志的,包括装备的颜色,星级,材料的等级,宝石的等级
*/
void zCard::logger(QWORD createid,DWORD objid,char *objname,DWORD num,DWORD change,DWORD type,DWORD srcid,char *srcname,DWORD dstid,char *dstname,const char *action,zCardB *base,BYTE kind, DWORD gameID)
{
	char tmpInfo[60] = {0};
	char *p = tmpInfo;

	if (srcname != NULL)
		strcat(tmpInfo,srcname);

	if (base != NULL)
	{
	    switch (base->type)
	    {
		case Cmd::CARDTYPE_EQUIP:
		    strcat(p + strlen(tmpInfo),":");
		    strcat(tmpInfo,"װ����");
		    break;
		case Cmd::CARDTYPE_ATTEND:
		    strcat(p + strlen(tmpInfo),":");
		    strcat(tmpInfo,"��ӿ�");
		    break;
		case Cmd::CARDCELLTYPE_HERO:
		    strcat(p + strlen(tmpInfo),":");
		    strcat(tmpInfo,"Ӣ�ۿ�");
		    break;
		case Cmd::CARDTYPE_MAGIC:
		    strcat(p + strlen(tmpInfo),":");
		    strcat(tmpInfo,"������");
		    break;
		case Cmd::CARDTYPE_SECRET:
		    strcat(p + strlen(tmpInfo),":");
		    strcat(tmpInfo,"���ؿ�");
		    break;
		case Cmd::CARDTYPE_SKILL:
		    strcat(p + strlen(tmpInfo),":");
		    strcat(tmpInfo,"Ӣ�ۼ��ܿ�");
		    break;

		default:
		    break;
	    }
	}
	ScenesService::objlogger->debug("��ս����:%u, %llu,%u,%s(%u),%u,%u,%u,%u,%s,%u,%s,%s",gameID, createid,objid,objname,base->id,num,change,type,srcid,tmpInfo,dstid,dstname,action);
}
/**
* \brief 根据物品对象复制一个新的物品对象
* \param objsrc 参照对象
* \return 失败返回NULL 否则返回生成的对象
*/
zCard *zCard::create(zCard *objsrc)
{
	if (objsrc==NULL) return NULL;
	zCard *ret=new zCard();
	if (ret)
	{
		strncpy(ret->name,objsrc->name,MAX_NAMESIZE);
		ret->tempid=objsrc->id;
		ret->base=objsrc->base;
		bcopy(&objsrc->data,&ret->data,sizeof(ret->data));
		bcopy(&objsrc->pk,&ret->pk,sizeof(ret->pk));
		ret->generateThisID();
		ret->free(true);
		ret->gameID = objsrc->gameID;

		if (!gci->addObject(ret))
		{
			SAFE_DELETE(ret);
		}
		else
		{
			ret->dwCreateThisID=ret->data.qwThisID;
			ret->inserted=true;
		}	
	}
	return ret;
}

void  zCard::destroy(zCard*& ob)
{
	//assert(!ob || ob->free());
	SAFE_DELETE(ob);
}

const stObjectLocation &zCard::reserve() const
{
	return data.pos;
}

void zCard::restore(const stObjectLocation &loc)
{
	data.pos = loc;
}

bool zCard::free() const
{
	return data.pos == Object::INVALID_POS || data.pos.loc() == Cmd::CARDCELLTYPE_NONE;
}

void zCard::free(bool flag)
{
	data.pos = Object::INVALID_POS;
}

//sky 根据物品TBL的配置生成实际物品对象

/**
* \brief 根据物品字典创建一个物品对象  
* \param objbase 物品字典
* \param num 物品的数量
* \param level 物品的级别
* \return 物品对象
*/
zCard *zCard::create(zCardB *objbase,DWORD gameID,BYTE level)
{
	if (objbase==NULL) return NULL;

	zCard *ret=new zCard();
	if (ret)
	{
		ret->base = objbase;
		ret->generateThisID();
		strncpy(ret->name,objbase->name,MAX_NAMESIZE);
		//strncpy(ret->data.strName,objbase->name,MAX_NAMESIZE);
		/*    
		ret->data.pos.dwLocation = Cmd::CARDCELLTYPE_COMMON;
		*/
		ret->data.dwObjectID = objbase->id;

		ret->free(true);
		ret->tempid = objbase->id;
		ret->gameID = gameID;

		ret->data.mpcost = objbase->mpcost;
		ret->data.damage = objbase->damage;
		ret->data.hp = objbase->hp;
		ret->data.maxhp = objbase->hp;
		ret->data.dur = objbase->dur;
		ret->data.magicDamAdd = objbase->magicDamAdd;
		ret->data.overload = objbase->overload;

		ret->pk.taunt = objbase->taunt;
		if(ret->hasTaunt())
		{
		    ret->setCardState(CARD_STATE_TAUNT);
		}
		ret->pk.charge = objbase->charge;
		if(!ret->isCharge())
		{
		    ret->setCardState(CARD_STATE_SLEEP);
		}
		ret->pk.windfury = objbase->windfury;
		if(ret->hasWindfury())
		{
		    ret->setCardState(CARD_STATE_WINDFURY);
		}
		ret->pk.sneak = objbase->sneak;
		if(ret->isSneak())
		{
		    ret->setCardState(CARD_STATE_SNEAK);
		}
		ret->pk.shield = objbase->shield;
		if(ret->hasShield())
		{
		    ret->setCardState(CARD_STATE_SHIED);
		}
		ret->pk.antimagic = objbase->antimagic;
		ret->pk.magicID = objbase->magicID;
		ret->pk.shoutID = objbase->shoutID;

		CardEffectCfg::getMe().fullOneCardPKData(objbase->id, ret->pk);
		if(ret->pk.beCureID)
		{
		    ret->setCardState(CARD_STATE_BECURE);
		}
		if(ret->pk.beHurtID)
		{
		    ret->setCardState(CARD_STATE_BEHURT);
		}
		if(ret->hasDeadLanguage())
		{
		    ret->setCardState(CARD_STATE_DEADLAN);
		}
		if(ret->hasRoundSID())
		{
		    ret->setCardState(CARD_STATE_SRSTART);
		}
		if(ret->hasRoundEID())
		{
		    ret->setCardState(CARD_STATE_SREND);
		}
		if(ret->hasEnemyRoundSID())
		{
		    ret->setCardState(CARD_STATE_ERSTART);
		}
		if(ret->hasEnemyRoundEID())
		{
		    ret->setCardState(CARD_STATE_EREND);
		}
		if(ret->pk.selfUseMagic)
		{
		    ret->setCardState(CARD_STATE_SUSEMA);
		}
		if(ret->pk.enemyUseMagic)
		{
		    ret->setCardState(CARD_STATE_EUSEMA);
		}
		if(ret->pk.sAttackID)
		{
		    ret->setCardState(CARD_STATE_ASTART);
		}
		if(ret->pk.beAttackID)
		{
		    ret->setCardState(CARD_STATE_BASTART);
		}
		if(ret->pk.drawID)
		{
		    ret->setCardState(CARD_STATE_DRAW);
		}
		if(ret->pk.drawedID)
		{
		    ret->setCardState(CARD_STATE_DRAWED);
		}
		if(ret->pk.otherBeHurtID)
		{
		    ret->setCardState(CARD_STATE_OHURT);
		}
		if(ret->pk.otherBeCureID)
		{
		    ret->setCardState(CARD_STATE_OCURE);
		}
		if(ret->pk.otherDeadID)
		{
		    ret->setCardState(CARD_STATE_ODEAD);
		}
		if(ret->pk.otherUseAttendID)
		{
		    ret->setCardState(CARD_STATE_SUSEA);
		}
		if(ret->pk.otherAttendInID)
		{
		    ret->setCardState(CARD_STATE_ATTENDIN);
		}
		if(ret->pk.attackEndID)
		{
		    ret->setCardState(CARD_STATE_AEND);
		}
		if(ret->hasHaloID())
		{
		    ret->setCardState(CARD_STATE_HALO);
		}

		if (!gci->addObject(ret))
		{
			SAFE_DELETE(ret);
		}
		else
		{
			ret->dwCreateThisID=ret->data.qwThisID;
			ret->inserted = true;
		}
	}else {
		Zebra::logger->debug("创建物品%s失败",objbase->name);
	}

	return ret;
}

/**
* \brief 从档案服务器读物品
*
* \param o 从档案服务器中读到的物品
*
* \return load成功返回该物品,否则返回NULL
*/
zCard *zCard::load(const SaveObject *o)
{
	if (o==NULL) return NULL;
	zCardB *objbase = cardbm.get(o->object.dwObjectID);
	if (objbase==NULL) 
	{
		Zebra::logger->error("加载物品失败,道具基本表中不存在:%d",o->object.dwObjectID);
		return NULL;
	}
	int i=0;
	zCard *ret=NULL; 
	while(!ret && i < 100)
	{
		ret=new zCard();
		if (i > 1)
		{
			Zebra::logger->error("尝试new出新的物品对象次数:%d",i);
		}
		i ++;
	}
	if (ret == NULL) 
	{
		Zebra::logger->error("加载物品失败,new物品对象失败:%d",o->object.dwObjectID);
		return ret; 
	}
	bcopy(&o->object,&ret->data,sizeof(t_Card));
	ret->createid = o->createid;
	ret->id = ret->data.qwThisID;
	ret->tempid = ret->data.dwObjectID;
	//strncpy(ret->name,ret->data.strName,MAX_NAMESIZE);
	ret->base=objbase;

	if (!gci->addObject(ret))
	{
		SAFE_DELETE(ret);
	}
	else
		ret->inserted=true;
	return ret;
}
/**
* \brief 得到物品创建时间,存档时使用
*
* \return 物品创建时间
*/
bool zCard::getSaveData(SaveObject *save)
{
	bcopy(&data,&save->object,sizeof(t_Card));

	save->createid =  createid;
	//Zebra::logger->error("[拍卖] 1 %s = %s,createid = %ld",save->object.strName,data.strName,createid);
	return true;
}


//////////////////////////////////////////�����Ƿָ���//////////////////////////////////////////////////////////
/**
* \brief 包裹构造
*/
CardSlot::CardSlot(WORD type,DWORD id,WORD w,WORD h):_type(type),_id(id),_width(w),_height(h),_space(w*h),_size(_space)
{
	WORD cap = _size;
	if (cap == 0) cap = 1;
	container = new zCard* [cap];
	memset(container,0,sizeof(zCard*)*cap);
}

CardSlot::~CardSlot()
{
	removeAll();
	SAFE_DELETE_VEC(container);
}

bool CardSlot::checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y)
{
	if (object==NULL) return true;
	zCard *temp;
	return getObjectByZone(&temp,x,y);
}

bool CardSlot::add(zCard* object,bool find)
{
	if (!object)
		return false;

	if (find)  {
		if (find_space(object->data.pos.x,object->data.pos.y))  {
			return add(object,false);
		}else {
			//Zebra::logger->warn("包裹[%d:%d]中找不到空间存放物品[%x]",_type,_id,object);
			return false;
		}  
	}

	if (!find)  {
		int pos = position(object->data.pos.xpos(),object->data.pos.ypos());
		if (pos == -1 || pos >= size()) {
			Zebra::logger->warn("包裹[%d:%d]中添加物品[%p]时索引[%d]错误",_type,_id,object,pos);
			return false;
		}
		if (container[pos]) { 
			//shouldn't be reached at all
			Zebra::logger->warn("包裹[%d]中[%d,%d]已有物品%p,不能存放物品%p",_id,object->data.pos.x,object->data.pos.y,container[pos],object);
			return false;
		}else {

			//  assert(!container[pos]);
			container[pos] = object;
			--_space;
			return true;
		}
	}

	//never reach
	return false;

}

bool CardSlot::find_space(WORD &x,WORD &y) const
{
    for(int i=0; i<_height; ++i) 
	for (int j=0; j<_width; ++j)	
	    if (!container[i*_width+j]) {
		x = i;
		y = j;
		return true;
	    }

    return false;
}

int CardSlot::position(WORD x,WORD y) const
{
	return x*_width + y;
}


bool CardSlot::remove(zCard *object)
{
	if (object)
	{
		int pos = position(object->data.pos.xpos(),object->data.pos.ypos());
		if (pos == -1 || pos >= size() || container[pos] != object) {
			Zebra::logger->warn("包裹[%d:%d]中删除物品[%p]时索引[%d]错误",_type,_id,object,pos);
			return false;
		}

		object->free(true);
		container[pos] = NULL;
		++_space;
		return true;
	}

	return false;
}

void CardSlot::removeAll()
{
    Zebra::logger->debug("�������۴�С:%u", size());
    for (int i=0; i<size(); ++i) {
	SAFE_DELETE(container[i]);
    };
}

bool CardSlot::getObjectByZone(zCard **ret,WORD x,WORD y)
{
	int pos = position(x,y);
	if (pos >= size() || pos == -1) return false;

	*ret = container[pos];
	return true;
}

bool CardSlot::getObjectByRandom(zCard **ret)
{
    if(size() > space())
    {
	DWORD total = size() - space();
	DWORD index = zMisc::randBetween(0, total-1);
	int pos = position(0, index);
	if (pos >= size() || pos == -1) return false;

	*ret = container[pos];
	return true;
    }
    return false;
}

DWORD CardSlot::getLeftObjectThisID(WORD x, WORD y)
{
    int pos = position(x,y);
    if (pos >= size() || pos == -1) return 0;
    if(pos >= 1)    //������ڶ���
    {
	if(container[pos-1])
	    return container[pos-1]->data.qwThisID;
    }
    return 0;
}

DWORD CardSlot::getRightObjectThisID(WORD x, WORD y)
{
    int pos = position(x,y);
    if (pos >= size() || pos == -1) return 0;
    if(pos < size()-1)	//������ڶ���
    {
	if(container[pos+1])
	    return container[pos+1]->data.qwThisID;
    }
    return 0;
}

bool CardSlot::getObjectByID(zCard **ret,DWORD id)
{
	for (int i=0; i<size(); ++i)
	{
		if (!container[i]) continue;

		if (container[i]->base->id==id)
		{
			*ret = container[i];
			return true;
		}
	}
	return false;
}

void CardSlot::execEvery(CardSlotCallback &callback)
{
	for (int i=0; i<size(); ++i) {
		if (!container[i]) continue;
		if (!callback.exec(container[i])) break;
	}
}

WORD CardSlot::space() const
{
	return _space;
}

WORD CardSlot::size() const
{
	return _size;
}

/**
* \brief 获取包裹类型
* \return 包裹类型
*/
WORD CardSlot::type() const
{
	return _type;
}

/**
* \brief 获取包裹ID
* \return 包裹ID
*/
DWORD CardSlot::id() const
{
	return _id;
}

void CardSlot::setSpace(WORD s)
{
	_space = s;
}

DWORD CardSlot::typeWeight(WORD type)
{
    return 1;
}

bool CardSlot::needSwap(zCard* ob1, zCard* ob2)
{
    if(!ob1 && !ob2)
	return false;

    if(!ob1 && ob2)	    //ob1��
	return true;

    if(ob1 && !ob2)	    //ob2��
	return true;

    return false;
}

bool CardSlot::swap(WORD index1, WORD index2)
{
    if(index1 >= size() || index2 >= size())
	return false;
    zCard* ob1 = container[index1];
    zCard* ob2 = container[index2];
    if(!ob2 && !ob2)
	return true;

    if(ob1 && ob2)
    {
	return true;	    //�������Ӷ��ж�����Ҫ��
    }

    WORD WIDTH = width();
    if(!ob1)
    {
	WORD x = index1/WIDTH;
	WORD y = index1%WIDTH;
	ob2->data.pos.setXY(x, y);
	container[index1] = ob2;
	container[index2] = ob1;
	return true;
    }

    if(!ob2)
    {
	WORD x = index2/WIDTH;
	WORD y = index2%WIDTH;
	ob2->data.pos.setXY(x, y);
	container[index1] = ob2;
	container[index2] = ob1;
	return true;
    }
    return false;
}

bool CardSlot::sort()
{
    WORD iWidth = width();
    WORD iHeight = height();
    int size = iWidth * iHeight;

    for(int i=0; i<size; ++i)
    {
	zCard* ob1 = container[i];
	for(int j=i+1; j<size; ++j)
	{
	    zCard* ob2 = container[j];
	    if(needSwap(ob1, ob2))
	    {
		swap(i, j);
		ob1 = container[i];
	    }
	}
    }
    return true;
}

bool CardSlot::trim(SceneUser *pUser, WORD startIndex)
{
    if(!pUser)
	return false;
    if(space() == 0)
	return false;

    WORD iWidth = width();
    WORD iHeight = height();
    WORD size = iWidth * iHeight;
    if(size < 2)
	return false;
    if(startIndex > size - 1)
	return false;

    for(WORD i=size-1; i>startIndex; i--)
    {
	zCard* ob1 = container[i];
	WORD j = i-1;
	zCard* ob2 = container[j];
	if(needSwap(ob1, ob2))
	{
	    swap(i, j);
	    ob1 = container[i];
	}
    }
#if 0
    BYTE buf[zSocket::MAX_DATASIZE];
    bzero(buf, zSocket::MAX_DATASIZE);

    Cmd::stSortObjectPropertyUserCmd *send = (Cmd::stSortObjectPropertyUserCmd*)buf;
    constructInPlace(send);
    send->type = flag;
    WORD num = 0;
    zCard* obj = NULL;
    for(int i=0; i<size; ++i)
    {
	obj = container[i];

	if(obj)
	{
	    send->list[i].qwThisID = obj->data.qwThisID;
	    send->list[i].x = obj->data.pos.xpos();
	    send->list[i].y = obj->data.pos.ypos();
	    ++num;
	    
	}
	else
	    break;
    }
    send->num = num;
    pUser->sendCmdToMe(send, sizeof(Cmd::stSortObjectPropertyUserCmd)+sizeof(send->list[0])*num);
#endif
    return true;
}

WORD CardSlot::getObjectNum() const
{
    WORD num = 0;
    zCard* o = NULL;
    for(int i=0; i<_size; ++i)
    {
	o = container[i];
	if(o && o->data.qwThisID)
	    ++num;
    }
    return num;
}

WORD CardSlot::getObjectNumByBaseID(DWORD baseID) const
{
    WORD num = 0;
    zCard* o = NULL;
    for(int i=0; i<_size; ++i)
    {
	o = container[i];
	if(o && o->base->id==baseID)
	    ++num;
    }
    return num;
}

/////////////////////////////////////////////�����Ƿָ���////////////////////////////////////////////////////////////////////
/**
* \brief 构造函数初始化人物�
*/
BattleSlot::BattleSlot():CardSlot(Cmd::CARDCELLTYPE_COMMON,0,BattleSlot::WIDTH,BattleSlot::HEIGHT)
{
	TabNum = MIN_TAB_NUM; //sky 初始化包裹页数
}

/**
* \brief 析构函数
*/
BattleSlot::~BattleSlot()
{
}

/**
* \brief 向包裹中添加对象
* \param object 物品对象
* \param find 寻找标志
* \return true 添加成功,false 添加失败
*/
bool BattleSlot::add(zCard *object,bool find)
{
    return CardSlot::add(object,find);
}

/**
* \brief 从包裹中删除物品
* \param object  物品对象
*/
bool BattleSlot::remove(zCard *object)
{
    return CardSlot::remove(object);
}

/**
* \brief 检查并添加
* \param pUser 角色
* \param object 物品
* \param x,y 坐标
* \return true 有该物品,false 没有
*/
bool BattleSlot::checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y)
{
    return  CardSlot::checkAdd(pUser,object,x,y);
}

/**
* \brief 根据物品位置和大小获取物品
* \param ret 返回找到的物品对象
* \param x 横坐标
* \param y 纵坐标
* \param width 宽度
* \param height 高度
* \return true 有此物品 false 无此物品
*/
bool BattleSlot::getObjectByZone(zCard **ret,WORD x,WORD y)
{
    return  CardSlot::getObjectByZone(ret,x,y);
}

/**
* \brief 箭支消耗数量是以耐久度计算的,当耐久为0时删除箭桶
* \param pThis 主人
* \param id 物品的objectid
* \param num　技能消耗物品的数量
* \return 消耗是否成功
*/
bool BattleSlot::skillReduceObject(SceneUser* pThis,DWORD id,DWORD num)
{

	return false;
}


/////////////////////////////////////////////�����Ƿָ���////////////////////////////////////////////////////////////////////
/**
* \brief 构造函数初始化人物�
*/
EquipSlot::EquipSlot():CardSlot(Cmd::CARDCELLTYPE_EQUIP, 0, EquipSlot::WIDTH,EquipSlot::HEIGHT)
{
	TabNum = MIN_TAB_NUM; //sky 初始化包裹页数
}

/**
* \brief 析构函数
*/
EquipSlot::~EquipSlot()
{
}

/**
* \brief 向包裹中添加对象
* \param object 物品对象
* \param find 寻找标志
* \return true 添加成功,false 添加失败
*/
bool EquipSlot::add(zCard *object,bool find)
{
    return CardSlot::add(object,find);
}

/**
* \brief 从包裹中删除物品
* \param object  物品对象
*/
bool EquipSlot::remove(zCard *object)
{
    return CardSlot::remove(object);
}

/**
* \brief 检查并添加
* \param pUser 角色
* \param object 物品
* \param x,y 坐标
* \return true 有该物品,false 没有
*/
bool EquipSlot::checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y)
{
    return  CardSlot::checkAdd(pUser,object,x,y);
}

/**
* \brief 根据物品位置和大小获取物品
* \param ret 返回找到的物品对象
* \param x 横坐标
* \param y 纵坐标
* \param width 宽度
* \param height 高度
* \return true 有此物品 false 无此物品
*/
bool EquipSlot::getObjectByZone(zCard **ret,WORD x,WORD y)
{
    return  CardSlot::getObjectByZone(ret,x,y);
}

/**
* \brief 箭支消耗数量是以耐久度计算的,当耐久为0时删除箭桶
* \param pThis 主人
* \param id 物品的objectid
* \param num　技能消耗物品的数量
* \return 消耗是否成功
*/
bool EquipSlot::skillReduceObject(SceneUser* pThis,DWORD id,DWORD num)
{

	return false;
}


/////////////////////////////////////////////�����Ƿָ���////////////////////////////////////////////////////////////////////
/**
* \brief 构造函数初始化人物�
*/
HandSlot::HandSlot():CardSlot(Cmd::CARDCELLTYPE_HAND, 0, HandSlot::WIDTH, HandSlot::HEIGHT)
{
	TabNum = MIN_TAB_NUM; //sky 初始化包裹页数
}

/**
* \brief 析构函数
*/
HandSlot::~HandSlot()
{
}

/**
* \brief 向包裹中添加对象
* \param object 物品对象
* \param find 寻找标志
* \return true 添加成功,false 添加失败
*/
bool HandSlot::add(zCard *object,bool find)
{
    return CardSlot::add(object,find);
}

/**
* \brief 从包裹中删除物品
* \param object  物品对象
*/
bool HandSlot::remove(zCard *object)
{
    return CardSlot::remove(object);
}

/**
* \brief 检查并添加
* \param pUser 角色
* \param object 物品
* \param x,y 坐标
* \return true 有该物品,false 没有
*/
bool HandSlot::checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y)
{
    return  CardSlot::checkAdd(pUser,object,x,y);
}

/**
* \brief 根据物品位置和大小获取物品
* \param ret 返回找到的物品对象
* \param x 横坐标
* \param y 纵坐标
* \param width 宽度
* \param height 高度
* \return true 有此物品 false 无此物品
*/
bool HandSlot::getObjectByZone(zCard **ret,WORD x,WORD y)
{
    return  CardSlot::getObjectByZone(ret,x,y);
}

/**
* \brief 箭支消耗数量是以耐久度计算的,当耐久为0时删除箭桶
* \param pThis 主人
* \param id 物品的objectid
* \param num　技能消耗物品的数量
* \return 消耗是否成功
*/
bool HandSlot::skillReduceObject(SceneUser* pThis,DWORD id,DWORD num)
{

	return false;
}


/////////////////////////////////////////////�����Ƿָ���////////////////////////////////////////////////////////////////////
/**
* \brief 构造函数初始化人物�
*/
SkillSlot::SkillSlot():CardSlot(Cmd::CARDCELLTYPE_SKILL,0,SkillSlot::WIDTH,SkillSlot::HEIGHT)
{
	TabNum = MIN_TAB_NUM; //sky 初始化包裹页数
}

/**
* \brief 析构函数
*/
SkillSlot::~SkillSlot()
{
}

/**
* \brief 向包裹中添加对象
* \param object 物品对象
* \param find 寻找标志
* \return true 添加成功,false 添加失败
*/
bool SkillSlot::add(zCard *object,bool find)
{
    return CardSlot::add(object,find);
}

/**
* \brief 从包裹中删除物品
* \param object  物品对象
*/
bool SkillSlot::remove(zCard *object)
{
    return CardSlot::remove(object);
}

/**
* \brief 检查并添加
* \param pUser 角色
* \param object 物品
* \param x,y 坐标
* \return true 有该物品,false 没有
*/
bool SkillSlot::checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y)
{
    return  CardSlot::checkAdd(pUser,object,x,y);
}

/**
* \brief 根据物品位置和大小获取物品
* \param ret 返回找到的物品对象
* \param x 横坐标
* \param y 纵坐标
* \param width 宽度
* \param height 高度
* \return true 有此物品 false 无此物品
*/
bool SkillSlot::getObjectByZone(zCard **ret,WORD x,WORD y)
{
    return  CardSlot::getObjectByZone(ret,x,y);
}

/**
* \brief 箭支消耗数量是以耐久度计算的,当耐久为0时删除箭桶
* \param pThis 主人
* \param id 物品的objectid
* \param num　技能消耗物品的数量
* \return 消耗是否成功
*/
bool SkillSlot::skillReduceObject(SceneUser* pThis,DWORD id,DWORD num)
{

	return false;
}

/////////////////////////////////////////////�����Ƿָ���////////////////////////////////////////////////////////////////////
/**
* \brief 构造函数初始化人物�
*/
HeroSlot::HeroSlot():CardSlot(Cmd::CARDCELLTYPE_HERO,0,HeroSlot::WIDTH,HeroSlot::HEIGHT)
{
	TabNum = MIN_TAB_NUM; //sky 初始化包裹页数
}

/**
* \brief 析构函数
*/
HeroSlot::~HeroSlot()
{
}

/**
* \brief 向包裹中添加对象
* \param object 物品对象
* \param find 寻找标志
* \return true 添加成功,false 添加失败
*/
bool HeroSlot::add(zCard *object,bool find)
{
    return CardSlot::add(object,find);
}

/**
* \brief 从包裹中删除物品
* \param object  物品对象
*/
bool HeroSlot::remove(zCard *object)
{
    return CardSlot::remove(object);
}

/**
* \brief 检查并添加
* \param pUser 角色
* \param object 物品
* \param x,y 坐标
* \return true 有该物品,false 没有
*/
bool HeroSlot::checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y)
{
    return  CardSlot::checkAdd(pUser,object,x,y);
}

/**
* \brief 根据物品位置和大小获取物品
* \param ret 返回找到的物品对象
* \param x 横坐标
* \param y 纵坐标
* \param width 宽度
* \param height 高度
* \return true 有此物品 false 无此物品
*/
bool HeroSlot::getObjectByZone(zCard **ret,WORD x,WORD y)
{
    return  CardSlot::getObjectByZone(ret,x,y);
}

/**
* \brief 箭支消耗数量是以耐久度计算的,当耐久为0时删除箭桶
* \param pThis 主人
* \param id 物品的objectid
* \param num　技能消耗物品的数量
* \return 消耗是否成功
*/
bool HeroSlot::skillReduceObject(SceneUser* pThis,DWORD id,DWORD num)
{

	return false;
}

/////////////////////////////////////////////�����Ƿָ���////////////////////////////////////////////////////////////////////
/**
* \brief 构造函数初始化人物�
*/
TombSlot::TombSlot():CardSlot(Cmd::CARDCELLTYPE_RECORD,0,TombSlot::WIDTH,TombSlot::HEIGHT)
{
	TabNum = MIN_TAB_NUM; //sky 初始化包裹页数
}

/**
* \brief 析构函数
*/
TombSlot::~TombSlot()
{
}

/**
* \brief 向包裹中添加对象
* \param object 物品对象
* \param find 寻找标志
* \return true 添加成功,false 添加失败
*/
bool TombSlot::add(zCard *object,bool find)
{
    return CardSlot::add(object,find);
}

/**
* \brief 从包裹中删除物品
* \param object  物品对象
*/
bool TombSlot::remove(zCard *object)
{
    return CardSlot::remove(object);
}

/**
* \brief 检查并添加
* \param pUser 角色
* \param object 物品
* \param x,y 坐标
* \return true 有该物品,false 没有
*/
bool TombSlot::checkAdd(SceneUser *pUser,zCard *object,WORD x,WORD y)
{
    return  CardSlot::checkAdd(pUser,object,x,y);
}

/**
* \brief 根据物品位置和大小获取物品
* \param ret 返回找到的物品对象
* \param x 横坐标
* \param y 纵坐标
* \param width 宽度
* \param height 高度
* \return true 有此物品 false 无此物品
*/
bool TombSlot::getObjectByZone(zCard **ret,WORD x,WORD y)
{
    return  CardSlot::getObjectByZone(ret,x,y);
}

/**
* \brief 箭支消耗数量是以耐久度计算的,当耐久为0时删除箭桶
* \param pThis 主人
* \param id 物品的objectid
* \param num　技能消耗物品的数量
* \return 消耗是否成功
*/
bool TombSlot::skillReduceObject(SceneUser* pThis,DWORD id,DWORD num)
{

	return false;
}

///////////////////////////////////////////�����Ƿָ���//////////////////////////////////////

/**
* \brief 构造函�
*/
CardSlots::CardSlots(ChallengeGame* game) : owner(game)
{

}

/**
* \brief  析构函数
*/
CardSlots::~CardSlots()
{

}

/**
* \brief 根据类型获取包裹
* \param type 包裹类型
* \param id 目前未使用
* \return 包裹对象
*/
CardSlot * CardSlots::getCardSlot(DWORD type,DWORD id)
{
    if(id == 1)
    {
	switch(type)
	{
	    case Cmd::CARDCELLTYPE_COMMON:
		return (CardSlot *)&main1;
	    case Cmd::CARDCELLTYPE_EQUIP:
		return (CardSlot *)&equip1;
	    case Cmd::CARDCELLTYPE_HAND:
		return (CardSlot *)&hand1;
	    case Cmd::CARDCELLTYPE_SKILL:
		return (CardSlot *)&skill1;
	    case Cmd::CARDCELLTYPE_HERO:
		return (CardSlot *)&hero1;
	    default:
		break;
	}
    }
    else if(id == 2)
    {
	switch(type)
	{
	    case Cmd::CARDCELLTYPE_COMMON:
		return (CardSlot *)&main2;
	    case Cmd::CARDCELLTYPE_EQUIP:
		return (CardSlot *)&equip2;
	    case Cmd::CARDCELLTYPE_HAND:
		return (CardSlot *)&hand2;
	    case Cmd::CARDCELLTYPE_SKILL:
		return (CardSlot *)&skill2;
	    case Cmd::CARDCELLTYPE_HERO:
		return (CardSlot *)&hero2;
	    default:
		break;
	}
    }
    return NULL;
}

/*
void CardSlots::clearCardSlot(CardSlot* pack)
{
ClearPack cp(this);
pack->execEvery(cp);
}
*/

CardSlot** CardSlots::getCardSlot(int packs)
{
	CardSlot** p = new CardSlot*[11];
	memset(p, 0, 11*sizeof(CardSlot *));

	//notice the sequence
	int i = 0;
	if (packs & MAIN1_PACK) p[i++] = (CardSlot *)&main1;
	if (packs & MAIN2_PACK) p[i++] = (CardSlot *)&main2;

	if (packs & HAND1_PACK) p[i++] = (CardSlot *)&hand1;
	if (packs & HAND2_PACK) p[i++] = (CardSlot *)&hand2;

	if (packs & EQUIP1_PACK) p[i++] = (CardSlot *)&equip1;
	if (packs & EQUIP2_PACK) p[i++] = (CardSlot *)&equip2;

	if (packs & SKILL1_PACK) p[i++] = (CardSlot *)&skill1;
	if (packs & SKILL2_PACK) p[i++] = (CardSlot *)&skill2;

	if (packs & HERO1_PACK) p[i++] = (CardSlot *)&hero1;
	if (packs & HERO2_PACK) p[i++] = (CardSlot *)&hero2;

	if (packs & TOMB_PACK) p[i++] = (CardSlot *)&record;

	return p;
}

/**
* \brief 获得当前金子数量
* \return 金子数量
*/
DWORD CardSlots::getGoldNum()
{
	return 0;
}

/**
* \brief 获取身上的金子
* \return 物品对象,或NULL
*/
zCard *CardSlots::getGold()
{
	return 0;
}

/**
* \brief 将物品丢到地上
* \param o 目标物品 
* \param pos 位置
* \return true 无聊的返回值
*/
bool CardSlots::moveObjectToScene(zCard *o,const zPos &pos,DWORD overdue_msecs,const unsigned long dwID)
{
	return true;
}

/**
* \brief 移动物品
* \param pUser 角色对象
* \param srcObj 被移动对象
* \param dst 对象的目的位置
* \return true 移动成功,false 移动失败
*/
bool CardSlots::moveObject(SceneUser *pUser,zCard*& srcObj,stObjectLocation &dst)
{
    using namespace Cmd;
    if(!owner)
	return false;
    if(!owner->isInGame(pUser->id))
	return false;

    DWORD id = 0;
    if(owner->players[0].playerID == pUser->id)
    {
	id = 1;
    }
    else if(owner->players[1].playerID == pUser->id)
    {
	id = 2;
    }
    CardSlot *srcpack = getCardSlot(srcObj->data.pos.loc(), id);
    if (!srcpack)  
    {
	Zebra::logger->error("[�ƶ�] �Ҳ���Դ����(%s)���ڵĿ���", srcObj->name);    
	return false;
    }

    CardSlot *destpack = getCardSlot(dst.loc(), id);
    if (!destpack) 
    {
	Zebra::logger->error("[�ƶ�] �Ҳ���Դ����(%s)Ҫȥ��Ŀ�꿨��", srcObj->name);    
	return false;
    }

    WORD srcType = srcpack->type();
    WORD destType = destpack->type();
    if(srcType == Cmd::CARDCELLTYPE_NONE || destType == Cmd::CARDCELLTYPE_NONE)	    //�۲����Ƿ�
	return false;
    if(srcpack == destpack)	//������ͬһ�����з����ƶ�
	return false;

    switch(srcType)
    {
	case Cmd::CARDCELLTYPE_HAND:	//����	
	    {
		if(destType == Cmd::CARDCELLTYPE_COMMON)
		{
		    if(destpack->space() == 0)
		    {
			Zebra::logger->error("[�����ƶ�] �������Ѿ�����Ŷ ������������ ���:%s(%u)",pUser->name, pUser->id);
			return false;
		    }
		}
	    }
	    break;
	case Cmd::CARDCELLTYPE_COMMON:	//��ս��	    
	    {}
	    break;
	case Cmd::CARDCELLTYPE_EQUIP:	//������
	    {}
	    break;
    }
    zCard *preDestObj = NULL;
    if(destpack->getObjectByZone(&preDestObj,dst.xpos(),dst.ypos()))	    //��Ŀ��λ���ڳ���
    {
	if(preDestObj && preDestObj != srcObj)
	{
	    if(destpack->trim(pUser, dst.ypos()))
	    {
		Zebra::logger->debug("[�����ƶ�] %uĿ��λ���������Խ��к��Ʋ���", dst.ypos());
	    }
	}
    }


    zCard *destObj = NULL;
    if (destpack->getObjectByZone(&destObj,dst.xpos(),dst.ypos()) && destObj != srcObj)
    {
	if (destpack->checkAdd(pUser,srcObj,dst.xpos(),dst.ypos()))
	{
	    if (srcpack->checkAdd(pUser,destObj,srcObj->data.pos.xpos(),srcObj->data.pos.ypos()))
	    {

		destpack->remove(destObj);
		if (destObj) destObj->data.pos=srcObj->data.pos;

		srcpack->remove(srcObj);
		srcpack->add(destObj,false);

		srcObj->data.pos=dst;
		destpack->add(srcObj,false);

		if (dst.loc() ==Cmd::CARDCELLTYPE_EQUIP)//装备时消耗耐久
		{
		    switch(srcObj->base->kind)
		    {
			default:
			    break;
		    }
		}

		srcpack->sort();	//�ѿյ�λ�÷������
		destpack->sort();	//�ѿյ�λ�÷������
		return true;
	    }
	}
    }
    else
    {
	if (destObj == srcObj )
	{
	    Zebra::logger->error("srcObj��destObj��Ȼ��ͬһ���ڴ��ַ");
	}
    }
    return false;
}

/**
* \brief  删除包裹中的对象
* \param   srcObj 目标物品
*/
bool CardSlots::removeObject(DWORD playerID, zCard*& srcObj,bool notify,bool del, BYTE opType)
{
    if (!srcObj) return false;

    if(!owner)
	return false;
    if(!owner->isInGame(playerID))
	return false;

    DWORD id = 0;
    if(owner->players[0].playerID == srcObj->playerID)
    {
	id = 1;
    }
    else if(owner->players[1].playerID == srcObj->playerID)
    {
	id = 2;
    }

    if (notify) 
    {
	Cmd::stRetRemoveBattleCardUserCmd send;
	send.dwThisID = srcObj->data.qwThisID;
	send.opType = opType;
	SceneUser *pUser = SceneUserManager::getMe().getUserByID(playerID);
	if(pUser)
	    pUser->sendCmdToMe(&send, sizeof(send));

	SceneUser *pOther = owner->getOther(playerID);
	if(pOther)
	{
	    if(srcObj->data.pos.loc() == Cmd::CARDCELLTYPE_HAND)
	    {
		Cmd::stDelEnemyHandCardPropertyUserCmd delsend;
		delsend.index = srcObj->data.pos.y;
		pOther->sendCmdToMe(&delsend, sizeof(delsend));
	    }
	    else
	    {
		pOther->sendCmdToMe(&send, sizeof(send));
	    }
	}
    }

    gcm.removeObject(srcObj);
    CardSlot *p=getCardSlot(srcObj->data.pos.loc(), id);
    if (p && p->remove(srcObj))
    {
	//only delete when remove from package and del is true
	if (del) 
	{
	    zCard::destroy(srcObj);
	}
	p->sort();	    //�ѿյ�λ��ȫ���������
	return true;
    }

    Zebra::logger->warn("[����]ɾ��%s[%p]ʧ��",srcObj->name,srcObj);    
    return false;
}

/**
* \brief 增加物品
* \param srcObj 物品对象
* \param needFind 是否要查找位置
* \param from_record 是否来自记录
* \return false 添加失败 true 添加成功
*/
bool CardSlots::addObject(zCard *srcObj,bool needFind,int packs)
{
	if (srcObj && srcObj->base)
	{
		if( !needFind && srcObj->free() ) //be sure a object do not be added into 2 diff package
		{
		    return false;
		}

		if (gcm.addObject(srcObj))
		{
#if 0
			if (!packs)
			{
				CardSlot *p = getCardSlot(srcObj->data.pos.loc(),srcObj->data.pos.tab());
				if (p && p->add(srcObj,needFind)) return true;
			}
#endif

			if (packs) 
			{
				//save location infomation
				stObjectLocation loc = srcObj->reserve();

				CardSlot** p = getCardSlot(packs);
				int i = 0;
				while (p && p[i])
				{
					srcObj->data.pos = stObjectLocation(p[i]->type(),p[i]->id(),(WORD)-1,(WORD)-1);
					if (p[i]->add(srcObj,needFind))
					{
						SAFE_DELETE_VEC(p);
						return true;
					}
					++i;
				}
				SAFE_DELETE_VEC(p);

				//can not be add,resotre the location
				srcObj->restore(loc);
			}
			Zebra::logger->warn("����%s[%p]��λ���� �������� �ܿ����ǿ�������",srcObj->name,srcObj);
			gcm.removeObject(srcObj);

			return false;
		}
		else 
		{
			Zebra::logger->warn("��������%s[%p]�ظ� ��������",srcObj->name,srcObj);
		}
	}
	return false;
}

/**
 * \brief ��newObj�滻��oldObj
 * \param
 * \return
*/
bool CardSlots::replaceObject(DWORD playerID, zCard*& oldObj, DWORD newObjID)
{
    int slot = 0;

    DWORD id = 0;
    if(owner->players[0].playerID == oldObj->playerID)
    {
	id = 1;
	switch(oldObj->data.pos.loc())
	{
	    case Cmd::CARDCELLTYPE_COMMON:
		{
		    slot = CardSlots::MAIN1_PACK;
		}
		break;
	    case Cmd::CARDCELLTYPE_HAND:
		{
		    slot = CardSlots::HAND1_PACK;
		}
		break;
	    case Cmd::CARDCELLTYPE_EQUIP:
		{
		    slot = CardSlots::EQUIP1_PACK;
		}
		break;
	    case Cmd::CARDCELLTYPE_SKILL:
		{
		    slot = CardSlots::SKILL1_PACK;
		}
		break;
	    case Cmd::CARDCELLTYPE_HERO:
		{
		    slot = CardSlots::HERO1_PACK;
		}
		break;
	}
    }
    else if(owner->players[1].playerID == oldObj->playerID)
    {
	id = 2;
	switch(oldObj->data.pos.loc())
	{
	    case Cmd::CARDCELLTYPE_COMMON:
		{
		    slot = CardSlots::MAIN2_PACK;
		}
		break;
	    case Cmd::CARDCELLTYPE_HAND:
		{
		    slot = CardSlots::HAND2_PACK;
		}
		break;
	    case Cmd::CARDCELLTYPE_EQUIP:
		{
		    slot = CardSlots::EQUIP2_PACK;
		}
		break;
	    case Cmd::CARDCELLTYPE_SKILL:
		{
		    slot = CardSlots::SKILL2_PACK;
		}
		break;
	    case Cmd::CARDCELLTYPE_HERO:
		{
		    slot = CardSlots::HERO2_PACK;
		}
		break;
	}
    }
    
    DWORD gameID = oldObj->gameID;
    zCardB *base = cardbm.get(newObjID);
    if(!base)
	return false;
    zCard *o = zCard::create(base, gameID, 0);
    if(!o)
	return false;

    o->playerID = playerID;
    o->playingTime = SceneTimeTick::currentTime.sec();

    zCard::logger(oldObj->createid,oldObj->data.qwThisID,oldObj->base->name,0,0,0,0,NULL,playerID, "��ɫ��","DELETE ���滻",oldObj->base,oldObj->base->kind,gameID);
    if(removeObject(playerID, oldObj, true, true, Cmd::OP_REPLACE_DELETE))
    {
	zCard::logger(o->createid,o->data.qwThisID,o->base->name,0,0,0,0,NULL,playerID, "��ɫ��","(�滻)ADD ��ս��",o->base,o->base->kind,gameID);
	if(addObject(o, true, slot))
	{
	    ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(gameID);
	    if(game)
	    {
		game->sendCardInfo(o);
	    }
	    return true;
	}
    }
    return false;
}

