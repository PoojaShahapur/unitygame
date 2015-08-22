/**
 * \brief Zebra游戏中所有基本数据结构的声明定义
 */

#include "zDatabase.h"
#include "zXMLParser.h"

void zObjectB::fill(ObjectBase &data)
{
    id = data.id;
    zXMLParser xml;
    BYTE *transName = xml.charConv((BYTE*)(data.name),"UTF-8", "GB2312");
    if(transName)
    {
	strncpy(name, (char *)transName, MAX_NAMESIZE);
	SAFE_DELETE_VEC(transName);
    }
    maxnum = data.maxnum;
    kind = data.kind;
    color = data.color;
}

void zCardB::fill(CardBase &data)
{
    id = data.id;
    zXMLParser xml;
    BYTE *transName = xml.charConv((BYTE*)(data.name),"UTF-8", "GB2312");
    if(transName)
    {
	strncpy(name, (char *)transName, MAX_NAMESIZE);
	SAFE_DELETE_VEC(transName);
    }
    type = data.type;
    occupation = data.occupation;
    race = data.race;
    kind = data.kind;
    mpcost = data.mpcost;
    damage = data.damage;
    hp = data.hp;
    dur = data.dur;
    source = data.source;

    taunt = data.taunt;
    charge = data.charge;
    windfury = data.windfury;
    sneak = data.sneak;
    shield = data.shield;
    antimagic = data.antimagic;
    magicDamAdd = data.magicDamAdd;
    overload = data.overload;

    magicID = data.magicID;
    needTarget = data.needTarget;
    shoutID = data.shoutID;
    shoutTarget = data.shoutTarget;
}

void zSkillB::fill(const SkillBase &data)
{

    id=data.dwField0;
    skillid=data.dwField0;                //技能ID
    zXMLParser xml;
    BYTE *transName = xml.charConv((BYTE*)(data.name),"UTF-8", "GB2312");
    if(transName)
    {
	strncpy(name, (char *)transName, MAX_NAMESIZE-1);
	SAFE_DELETE_VEC(transName);
    }
    set_skillState(data.function, skillStatus);
    conditionType = data.conditionType;
    conditionID = data.conditionID;
#if 0
    set_skillState(data.functionA, skillStatusA);
    set_skillState(data.functionB, skillStatusB);
#endif
    skillAID = data.skillAID;
    skillBID = data.skillBID;
}

void zStateB::fill(StateBase &data)
{
    id = data.id;
    zXMLParser xml;
    BYTE *transName = xml.charConv((BYTE*)(data.name),"UTF-8", "GB2312");
    if(transName)
    {
	strncpy(name, (char *)transName, MAX_NAMESIZE-1);
	SAFE_DELETE_VEC(transName);
    }
    mainBuff = data.mainBuff;
}
#if 0
void NpcCarryObject::lostGreen(NpcLostObject &nlo,int value,int value1,int value2,int vcharm,int vlucky)
{
  //mlock.lock();
  if (vcharm>1000) vcharm=1000;
  if (vlucky>1000) vlucky=1000;
  for(std::vector<CarryObject>::const_iterator it = cov.begin(); it != cov.end(); it++)
  {
    //Zebra::logger->debug("%u,%u,%u,%u",(*it).id,(*it).rate,(*it).minnum,(*it).maxnum);
    zObjectB *ob = objectbm.get((*it).id);
    if (ob)
    {
      if (ob->kind>=101 && ob->kind <=118)
      {
        nlo.push_back(*it);
      }
      else
      {
        switch((*it).id)
        {
          case 665:
          {
            int vrate = (int)(((*it).rate/value)*(1+value1/100.0f)*(1+value2/100.0f)*(1+vcharm/1000.0f)*(1+vlucky/1000.0f));
            if (selectByTenTh(vrate))
            {
              nlo.push_back(*it);
            }
          }
          break;
          default:
          {
            int vrate = (int)(((*it).rate/value)*(1+value1/100.0f)*(1+vcharm/1000.0f)*(1+vlucky/1000.0f));
            if (selectByTenTh(vrate))
            {
              nlo.push_back(*it);
            }
          }
          break;
        }
      }
    }
  }
  //mlock.unlock();
}
#endif
