/*************************************************************************
 Author: wang
 Created Time: 2015��01��22�� ������ 16ʱ46��28��
 File Name: ScenesServer/CardPk.cpp
 Description: 
 ************************************************************************/

#include "zCard.h"
#include "zDatabaseManager.h"
#include "SceneUserManager.h"
#include "ChallengeGameManager.h"
#include "ChallengeGame.h"
#include "SceneUser.h"
#include "HeroCardCmd.h"

void zCard::addAttackTimes()
{
    this->data.attackTimes++;
}

BYTE zCard::getAttackTimes()
{
    return data.attackTimes;
}

bool zCard::hasMagic()
{
    if(this->pk.magicID)
	return true;
    return false;
}

bool zCard::addMagic(DWORD skillID)
{
    this->pk.magicID = skillID;
    return true;
}

bool zCard::clearMagic()
{
    this->pk.magicID = 0;
    return true;
}

bool zCard::hasShout()
{
    if(this->pk.shoutID)
	return true;
    return false;
}

bool zCard::addShout(DWORD skillID)
{
    this->pk.shoutID = skillID;
    return true;
}

bool zCard::clearShout()
{
    this->pk.magicID = 0;
    return true;
}

bool zCard::hasDeadLanguage()
{
    if(this->pk.deadIDVec.empty())
	return false;
    return true;
}

bool zCard::addDeadLanguage(DWORD skillID)
{
    this->pk.deadIDVec.push_back(skillID);
    Zebra::logger->debug("[����] ��������:%u (%s)", skillID, this->base->name);
    return true;
}

bool zCard::clearDeadLanguage()
{
    this->pk.deadIDVec.clear();
    return true;
}

bool zCard::hasRoundSID()
{
    if(this->pk.roundSIDVec.empty())
	return false;
    return true;
}

bool zCard::addRoundSID(DWORD skillID, bool tmp)
{
    t_EffectUnit einfo;
    einfo.id = skillID;
    if(tmp)
    {
	einfo.temp = 1;
    }
    this->pk.roundSIDVec.push_back(einfo);
    Zebra::logger->debug("[����] ��������غϿ�ʼ:%u (%s) %s", skillID, this->base->name, tmp?"��ʱ":"����");
    return true;
}

bool zCard::clearRoundSID()
{
    this->pk.roundSIDVec.clear();
    return true;
}

bool zCard::hasRoundEID()
{
    if(this->pk.roundEIDVec.empty())
	return false;
    return true;
}

bool zCard::addRoundEID(DWORD skillID, bool tmp)
{
    t_EffectUnit einfo;
    einfo.id = skillID;
    if(tmp)
    {
	einfo.temp = 1;
    }
    this->pk.roundEIDVec.push_back(einfo);
    Zebra::logger->debug("[����] ��������غϽ���:%u (%s) %s", skillID, this->base->name, tmp?"��ʱ":"����");
    return true;
}

bool zCard::clearRoundEID()
{
    this->pk.roundEIDVec.clear();
    return true;
}

bool zCard::hasEnemyRoundSID()
{
    if(this->pk.enemyroundSIDVec.empty())
	return false;
    return true;
}

bool zCard::addEnemyRoundSID(DWORD skillID, bool tmp)
{
    t_EffectUnit einfo;
    einfo.id = skillID;
    if(tmp)
    {
	einfo.temp = 1;
    }
    this->pk.enemyroundSIDVec.push_back(einfo);
    Zebra::logger->debug("[����] ���ӵз��غϿ�ʼ:%u (%s) %s", skillID, this->base->name, tmp?"��ʱ":"����");
    return true;
}

bool zCard::clearEnemyRoundSID()
{
    this->pk.enemyroundSIDVec.clear();
    return true;
}

bool zCard::hasEnemyRoundEID()
{
    if(this->pk.enemyroundEIDVec.empty())
	return false;
    return true;
}

bool zCard::addEnemyRoundEID(DWORD skillID, bool tmp)
{
    t_EffectUnit einfo;
    einfo.id = skillID;
    if(tmp)
    {
	einfo.temp = 1;
    }
    this->pk.enemyroundEIDVec.push_back(einfo);
    Zebra::logger->debug("[����] ���ӵз��غϽ���:%u (%s) %s", skillID, this->base->name, tmp?"��ʱ":"����");
    return true;
}

bool zCard::clearEnemyRoundEID()
{
    this->pk.enemyroundEIDVec.clear();
    return true;
}

bool zCard::isDie()
{
    if(this->isAttend() || this->isHero())
    {
	if(this->data.hp == 0)
	    return true;
    }
    else if(this->isEquip())
    {
	if(this->data.dur == 0)
	    return true;
    }
    return false;
}

bool zCard::isAwake()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	if(this->pk.awake == 0)
	    return false;
    }
    return true;
}

bool zCard::setAwake()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.awake = 1;
	clearCardState(CARD_STATE_SLEEP);
	return false;
    }
    return true;
}

bool zCard::isCharge()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	if(this->pk.charge)
	    return true;
    }
    return false;
}

bool zCard::hasTaunt()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	if(this->pk.taunt)
	    return true;
    }
    return false;
}

bool zCard::hasWindfury()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	if(this->pk.windfury)
	    return true;
    }
    return false;
}

bool zCard::isSneak()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	if(this->pk.sneak)
	    return true;
    }
    return false;
}

bool zCard::breakSneak()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.sneak = 0;
	clearCardState(CARD_STATE_SNEAK);
	return true;
    }
    return false;
}

bool zCard::hasShield()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	if(this->pk.shield)
	    return true;
    }
    return false;
}

bool zCard::breakShield()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.shield = 0;
	clearCardState(CARD_STATE_SHIED);
	return true;
    }
    return false;
}

bool zCard::isFreeze()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND || this->base->type == Cmd::CARDTYPE_HERO)
    {
	if(this->pk.freeze)
	    return true;
    }
    return false;
}

bool zCard::clearFreeze()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND || this->base->type == Cmd::CARDTYPE_HERO)
    {
	this->pk.freeze = 0;
	this->pk.freeze_round = 0;
	clearCardState(CARD_STATE_FREEZE);
	return true;
    }
    return false;
}

bool zCard::setFreeze()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND || this->base->type == Cmd::CARDTYPE_HERO)
    {
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(this->gameID);
	if(!game)
	    return false;
	this->pk.freeze = 1;
	this->pk.freeze_round = game->getTotalRoundCount();
	setCardState(CARD_STATE_FREEZE);
	return true;
    }
    return false;
}

bool zCard::checkAttackTimes()
{
    BYTE limitTimes = 0;
    if(this->hasWindfury())
    {
	limitTimes = 2;
    }
    else
    {
	limitTimes = 1;
    }
    if(this->getAttackTimes() >= limitTimes)
    {
	return false;
    }
    return true;
}

void zCard::resetAttackTimes()
{
    this->data.attackTimes = 0;
}

bool zCard::isHero()
{
    if(this->base->type == Cmd::CARDTYPE_HERO)
	return true;
    return false;
}

bool zCard::isAttend()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
	return true;
    return false;
}

bool zCard::isEquip()
{
    if(this->base->type == Cmd::CARDTYPE_EQUIP)
	return true;
    return false;
}

bool zCard::isMagicCard()
{
    if(this->base->type == Cmd::CARDTYPE_MAGIC)
	return true;
    return false;
}

bool zCard::isHeroMagicCard()
{
    if(this->base->type == Cmd::CARDTYPE_SKILL)
	return true;
    return false;
}

bool zCard::hasDamage()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND || this->base->type == Cmd::CARDTYPE_HERO)
    {
	if(this->data.damage)
	    return true;
    }
    return false;
}

void zCard::setDamage(DWORD dam)
{
    DWORD oldDamage = this->data.damage;
    this->data.damage = dam;
    DWORD haloDamage = getHaloDamage();	    //�⻷��
    if(haloDamage)
    {
	this->data.damage += haloDamage;
    }
    Zebra::logger->debug("[����] ���ù�:%u �⻷��:%u (%s)�ı�ǰ��:(%u) �ı��:(%u)", dam, haloDamage, this->base->name, oldDamage, this->data.damage);
}

bool zCard::addDamage(DWORD dam)
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND || this->base->type == Cmd::CARDTYPE_HERO || this->base->type == Cmd::CARDTYPE_EQUIP)
    {
	DWORD oldDamage = this->data.damage;
	this->data.damage += dam;
	Zebra::logger->debug("[����] ����+%u�� (%s)�ı�ǰ��:(%u) �ı��:(%u)", dam, this->base->name, oldDamage, this->data.damage);
	return true;
    }
    return false;
}

bool zCard::subDamage(DWORD dam)
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND || this->base->type == Cmd::CARDTYPE_HERO || this->base->type == Cmd::CARDTYPE_EQUIP)
    {
	DWORD oldDamage = this->data.damage;
	if(this->data.damage >= dam)
	{
	    this->data.damage -= dam;
	}
	else
	{
	    this->data.damage = 0;
	}
	Zebra::logger->debug("[����] ����-%u�� (%s)�ı�ǰ��:(%u) �ı��:(%u)", dam, this->base->name, oldDamage, this->data.damage);
	return true;
    }
    return false;
}

bool zCard::multiplyDamage(DWORD para)
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND && para)
    {
	DWORD oldDamage = this->data.damage;
	DWORD haloDamage = getHaloDamage();	    //�⻷��
	if(oldDamage >= haloDamage)
	{
	    this->data.damage -= haloDamage;    //���¹⻷
	    this->data.damage *= para;
	    this->data.damage += haloDamage;    //���Ϲ⻷
	}
	Zebra::logger->debug("[����] ������:%u �⻷��:%u (%s)�ı�ǰ��:(%u) �ı��:(%u)", para, haloDamage, this->base->name, oldDamage, this->data.damage);
	return true;
    }
    return false;
}

bool zCard::multiplyHpBuff(DWORD para)
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND && para)
    {
	DWORD oldHp = this->data.hp;
	DWORD oldMaxHp = this->data.maxhp;

	DWORD haloHp = getHaloMaxHp();
	if(oldMaxHp >= haloHp)
	{
	    this->data.maxhp -= haloHp;
	    this->data.maxhp *= para;
	    this->data.maxhp += haloHp;

	    this->data.hp *= para;
	}
	Zebra::logger->debug("[����] Ѫ����:%u �⻷Ѫ:%u (%s)�ı�ǰѪ:(%u/%u) �ı��Ѫ:(%u/%u)",para, haloHp, this->base->name, oldHp, oldMaxHp, this->data.hp, this->data.maxhp);
	return true;
    }
    return false;
}

/**
 * \brief   ��÷����˺�����+x
 * \param
 * \return
*/
bool zCard::addMagicDam(DWORD dam)
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND && dam)
    {
	DWORD oldMagicDam = this->data.magicDamAdd;
	this->data.magicDamAdd += dam;
	Zebra::logger->debug("[����] ����+%u�����˺����� (%s)�ı�ǰ:(%u) �ı��:(%u)", dam, this->base->name, oldMagicDam, this->data.magicDamAdd);
	return true;
    }
    return false;
}

/**
 * \brief   ��Ĭ
 * \param
 * \return
*/
void zCard::silentMe()
{
    ////////////////////��������������Դ�����������//////////////////
    this->clearTaunt();
    this->clearCharge();
    this->clearWindfury();
    this->breakSneak();
    this->breakShield();
    this->clearMagic();
    this->clearShout();
    this->clearDeadLanguage();
    /////////////////////////end///////////////////////////////


}

/**
 * \brief   �Ƿ���л���ֵ
 * \param
 * \return
*/
bool zCard::hasArmor()
{
    if(this->base->type == Cmd::CARDTYPE_HERO)
    {
	if(this->data.armor)
	    return true;
    }
    return false;
}

bool zCard::addArmor(DWORD value)
{
    if(this->base->type == Cmd::CARDTYPE_HERO)
    {
	this->data.armor += value;
	Zebra::logger->debug("[����] %s ���ӻ��� ����:%u ���Ӻ�:(%u)", this->base->name, value, this->data.armor);
	return true;
    }
    return false;
}

bool zCard::addDur(DWORD value)
{
    if(this->isEquip())
    {
	this->data.dur += value;
	Zebra::logger->debug("[����] %s �����;� ����:%u ���Ӻ�:(%u)", this->base->name, value, this->data.dur);
	return true;
    }
    return false;
}

bool zCard::subDur(DWORD value)
{
    if(this->isEquip())
    {
	if(this->data.dur >= value)
	{
	    this->data.dur -= value;
	}
	else
	{
	    this->data.dur = 0;
	}
	return true;
    }
    return false;
}

/**
 * \brief   �ָ�����
 * \param
 * \return
*/
void zCard::restoreLife(DWORD hp)
{   
    if(this->base->type == Cmd::CARDTYPE_ATTEND || this->base->type == Cmd::CARDTYPE_HERO)
    {
	DWORD oldHp = this->data.hp;
	DWORD oldMaxHp = this->data.maxhp;
	DWORD popValue = hp;
	if(hp == 0)
	{
	    this->data.hp = this->data.maxhp;
	    if(this->data.hp >= oldHp)
	    {
		popValue = this->data.hp-oldHp;
	    }
	}
	else
	{
	    this->data.hp += hp;
	    if(this->data.hp >= this->data.maxhp)
	    {
		this->data.hp = this->data.maxhp;
	    }
	}
	this->data.popHpValue = popValue;
	Zebra::logger->debug("[����] %s �ָ����� ����:%u �ָ�ǰ:(hp:%u/maxhp:%u) --> �ָ���:(hp:%u/maxhp:%u)", this->base->name, hp, oldHp, oldMaxHp, this->data.hp, this->data.maxhp);
	if(oldHp < oldMaxHp)	//�ܵ�����
	{
	    ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(this->gameID);
	    if(game)
	    {
		if(this->pk.beCureID)
		{
		    Cmd::stCardAttackMagicUserCmd unit;
		    unit.dwAttThisID = this->data.qwThisID;
		    unit.dwMagicType = this->pk.beCureID;
		    game->addActionList(unit);
		    Zebra::logger->debug("�����ܵ����Ƽ���ִ�б�");
		}
		game->addCureList(this->data.qwThisID);
		game->dealHaloEffectAction();
	    }
	}
    }
}

bool zCard::toDie()
{
    if(this->isAttend())
    {
	this->data.hp = 0;
	Zebra::logger->debug("[����] ������� (%s)������", this->base->name);
	return true;
    }
    else if(this->isEquip())
    {
	this->data.dur = 0;
	Zebra::logger->debug("[����] �ݻ����� (%s)���ݻ�", this->base->name);
	return true;
    }
    return false;
}

bool zCard::addTaunt()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.taunt++;
	setCardState(CARD_STATE_TAUNT);
	Zebra::logger->debug("[����] %s ���� ����", this->base->name);
	return true;
    }
    return false;
}

bool zCard::subTaunt()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	if(this->pk.taunt)
	    this->pk.taunt--;
	if(this->pk.taunt == 0)
	    clearCardState(CARD_STATE_TAUNT);
	Zebra::logger->debug("[����] %s ȥ��һ�� ����", this->base->name);
	return true;
    }
    return false;
}

bool zCard::clearTaunt()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.taunt = 0;
	clearCardState(CARD_STATE_TAUNT);
	return true;
    }
    return false;
}

bool zCard::setHpBuff(DWORD hp)
{
    //if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	DWORD oldHp = this->data.hp;
	DWORD oldMaxHp = this->data.maxhp;

	this->data.maxhp = hp;
	this->data.hp = hp;
	DWORD haloHp = getHaloMaxHp();
	if(haloHp)
	{
	    this->data.maxhp += haloHp;
	    this->data.hp += haloHp;
	}
	Zebra::logger->debug("[����] ����Ѫ:%u �⻷Ѫ:%u (%s)�ı�ǰѪ:(%u/%u) �ı��Ѫ:(%u/%u)",hp, haloHp, this->base->name, oldHp, oldMaxHp, this->data.hp, this->data.maxhp);
	return true;
    }
    return false;
}

bool zCard::addHpBuff(DWORD hp)
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	DWORD oldHp = this->data.hp;
	DWORD oldMaxHp = this->data.maxhp;

	this->data.maxhp += hp;
	this->data.hp += hp;
	Zebra::logger->debug("[����] ����+%uѪ (%s)�ı�ǰѪ:(%u/%u) �ı��Ѫ:(%u/%u)",hp, this->base->name, oldHp, oldMaxHp, this->data.hp, this->data.maxhp);
	return true;
    }
    return false;
}

bool zCard::clearHpBuff(DWORD hp)
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	if(this->data.maxhp >= hp)
	    this->data.maxhp -= hp;
	if(this->data.hp >= hp)
	    this->data.hp -= hp;
	return true;
    }
    return false;
}

bool zCard::subMaxHp(DWORD hp)
{
    if(hp == 0)
	return true;
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	DWORD oldHp = this->data.hp;
	DWORD oldMaxHp = this->data.maxhp;
	if(this->data.maxhp >= hp)
	    this->data.maxhp -= hp;
	if(this->data.hp >= this->data.maxhp)
	    this->data.hp = this->data.maxhp;
	Zebra::logger->debug("[����] ȥ�� maxhp %uѪ (%s)�ı�ǰѪ:(%u/%u) �ı��Ѫ:(%u/%u)",hp, this->base->name, oldHp, oldMaxHp, this->data.hp, this->data.maxhp);
	return true;
    }
    return false;
}

/**
 * \brief this��pDef����Ѫ��
 * \param
 * \return
*/
bool zCard::exchangeHp(zCard* pDef)
{
    if(!pDef)
	return false;
    DWORD oldHp = this->data.hp;
    DWORD oldMaxHp = this->data.maxhp;
    
    DWORD oldHp2 = pDef->data.hp;
    DWORD oldMaxHp2 = pDef->data.maxhp;
    this->data.hp = oldHp2;
    this->data.maxhp = oldHp2;
    pDef->data.hp = oldHp;
    pDef->data.maxhp = oldHp;
    pDef->haloInfoMap.clear();	    //ȥ���ɵĹ⻷,���������ˢһ��

    Zebra::logger->debug("[����] ����Ѫ�� ������(%s)�ı�ǰѪ:(%u/%u)-->�ı��Ѫ:(%u/%u) ������(%s)�ı�ǰѪ:(%u/%u)-->�ı��Ѫ:(%u/%u)",
	    this->base->name, oldHp, oldMaxHp, this->data.hp, this->data.maxhp,
	    pDef->base->name, oldHp2, oldMaxHp2, pDef->data.hp, pDef->data.maxhp);
    return false;
}

/**
 * \brief ������Ѫ������
 * \param
 * \return
*/
bool zCard::exchangeHpDamage()
{
    DWORD oldHp = this->data.hp;
    DWORD oldMaxHp = this->data.maxhp;
    DWORD oldDamage = this->data.damage;

    this->data.damage = oldHp;
    this->data.hp = oldDamage;
    this->data.maxhp = oldDamage;

    this->haloInfoMap.clear();	    //ȥ���ɵĹ⻷,���������ˢһ��

    Zebra::logger->debug("[����] ��Ѫ���� (%s)�ı�ǰ��(%u)Ѫ:(%u/%u) �ı��(%u)Ѫ:(%u/%u)",this->base->name, oldDamage, oldHp, oldMaxHp, this->data.damage, this->data.hp, this->data.maxhp);
    return false;
}

bool zCard::addMpCost(DWORD mp)
{
    if(mp == 0)
	return false;
    DWORD oldMp = this->data.mpcost;
    this->data.mpcost += mp;
    Zebra::logger->debug("[����] ����+%u ���� (%s)�ı�ǰ:(%u) �ı��:(%u)", mp, this->base->name, oldMp, this->data.mpcost);
    return true;
}

bool zCard::subMpCost(DWORD mp)
{
    if(mp == 0)
	return false;
    DWORD oldMp = this->data.mpcost;
    if(oldMp >= mp)
    {
	this->data.mpcost -= mp;
    }
    else
    {
	this->data.mpcost = 0;
    }
    Zebra::logger->debug("[����] ����-%u ���� (%s)�ı�ǰ:(%u) �ı��:(%u)", mp, this->base->name, oldMp, this->data.mpcost);
    return true;
}

bool zCard::isEquipOpen()
{
    if(this->data.equipOpen)
	return true;
    return false;
}

bool zCard::addCharge()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.charge++;
	this->pk.awake = 1;
	clearCardState(CARD_STATE_SLEEP);
	Zebra::logger->debug("[����] %s ���� ���", this->base->name);
	return true;
    }
    return false;
}

bool zCard::subCharge()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	if(this->pk.charge)
	    this->pk.charge--;
	if(this->pk.charge == 0)
	    setCardState(CARD_STATE_SLEEP);
	Zebra::logger->debug("[����] %s ȥ��һ�� ���", this->base->name);
	return true;
    }
    return false;
}

bool zCard::clearCharge()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.charge = 0;
	return true;
    }
    return false;
}

bool zCard::addShield()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.shield = 1;
	setCardState(CARD_STATE_SHIED);
	Zebra::logger->debug("[����] %s ���� ʥ��", this->base->name);
	return true;
    }
    return false;
}

bool zCard::addWindfury()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.windfury++;
	setCardState(CARD_STATE_WINDFURY);
	Zebra::logger->debug("[����] %s ���� ��ŭ", this->base->name);
	return true;
    }
    return false;
}

bool zCard::subWindfury()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	if(this->pk.windfury)
	    this->pk.windfury--;
	if(this->pk.windfury == 0)
	{
	    clearCardState(CARD_STATE_WINDFURY);
	}
	Zebra::logger->debug("[����] %s ȥ��һ�� ��ŭ", this->base->name);
	return true;
    }
    return false;
}

bool zCard::clearWindfury()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.windfury = 0;
	clearCardState(CARD_STATE_WINDFURY);
	return true;
    }
    return false;
}

bool zCard::addSneak()
{
    if(this->base->type == Cmd::CARDTYPE_ATTEND)
    {
	this->pk.sneak = 1;
	setCardState(CARD_STATE_SNEAK);
	Zebra::logger->debug("[����] %s ���� Ǳ��", this->base->name);
	return true;
    }
    return false;
}

bool zCard::canNotAsFashuTarget()
{
    if(this->pk.antimagic)
	return true;
    return false;
}

bool zCard::hasHaloID()
{
    if(this->pk.haloID)
	return true;
    return false;
}

DWORD zCard::getHaloDamage()
{
    DWORD dam = 0;
    for(std::map<DWORD, t_haloInfo>::iterator it = this->haloInfoMap.begin(); it!= this->haloInfoMap.end(); it++)
    {
	dam += it->second.damage;
    }
    return dam;
}

DWORD zCard::getHaloMaxHp()
{
    DWORD maxhp = 0;
    for(std::map<DWORD, t_haloInfo>::iterator it = this->haloInfoMap.begin(); it!= this->haloInfoMap.end(); it++)
    {
	maxhp += it->second.maxhp;
    }
    return maxhp;
}

DWORD zCard::getHaloIncMpCost()
{
    DWORD mp = 0;
    for(std::map<DWORD, t_haloInfo>::iterator it = this->haloInfoMap.begin(); it!= this->haloInfoMap.end(); it++)
    {
	mp += it->second.incrmpcost;
    }
    return mp;
}

DWORD zCard::getHaloDecMpCost()
{
    DWORD mp = 0;
    for(std::map<DWORD, t_haloInfo>::iterator it = this->haloInfoMap.begin(); it!= this->haloInfoMap.end(); it++)
    {
	mp += it->second.decrmpcost;
    }
    return mp;
}

bool zCard::preAttackMe(zCard *pAtt, const Cmd::stCardAttackMagicUserCmd *rev)
{
    if(pAtt->isDie())
	return false;
    if(!pAtt->isAwake())
	return false;
    if(!pAtt->checkAttackTimes())
	return false;

    return true;
}

bool zCard::AttackMe(zCard *pAtt, const Cmd::stCardAttackMagicUserCmd *rev)
{
    return true;
}

DWORD zCard::doNormalPK(zCard *pAtt, zCard *pDef)
{
    if(!pDef || !pAtt)
	return 0;
    if(pDef->isDie() || pAtt->isDie())
	return 0;
    DWORD attDamage = pAtt->data.damage;
    DWORD defDamage = pDef->data.damage;
    pDef->directDamage(pAtt, pDef, attDamage, 0, true);	    //pDef��Ѫ
    if(pDef->isAttend())
    {
	pAtt->directDamage(pDef, pAtt, defDamage, 0, true);	    //pAtt��Ѫ
    }
    pAtt->addAttackTimes();
    if(pAtt->isSneak())
    {
	pAtt->breakSneak();		//������
    }
    if(pAtt->isHero())
    {//�۳������;�
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pAtt->gameID);
	if(!game)
	    return 0;

	zCard *equip = game->getSelfEquip(pAtt->playerID);
	if(equip && !equip->isDie())
	{
	    equip->subDur();
	    if(!pAtt->checkAttackTimes() && !equip->isDie())	//�ر�����
	    {
		Zebra::logger->debug("[PK] ������� Ӣ������ %s �ر�", equip->base->name);
		pAtt->data.equipOpen = 0;
		game->sendCardInfo(pAtt);
	    }
	}

    }
    return 0;
}

/**
 * \brief ��Ѫ
 * \param pAtt:������
 *	  pDef:��������
 *	  hp:��Ѫ��
 *	  addMagic:�����˺����Ӽӳ�(1,0)
 *	  isnormal:�Ƿ�����ͨ����(true,false)
 * \return
*/
DWORD zCard::directDamage(zCard *pAtt, zCard *pDef, DWORD hp, const BYTE addMagic, bool isnormal)
{
    if(!pDef)
	return 0;
    if(pDef->isDie() || hp==0)
	return 0;
    if(pDef->hasShield() && (hp > 0))	//��ʥ��
    {
	pDef->breakShield();
	Zebra::logger->debug("[����] ֱ�ӿ�Ѫ ��Ѫ����:%u (%s) (ʥ�ֵܵ���)",hp, pDef->base->name);
	return 0;
    }
    if(addMagic)
    {
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pAtt->gameID);
	if(game)
	{
	    DWORD addDam = game->recalMagicAddDam(pAtt->playerID);
	    hp += addDam;
	    Zebra::logger->debug("[����] ֱ�ӿ�Ѫ%s ��Ѫ����:%u(������(%u)��ķ����˺�����)  ",pDef->base->name, hp, addDam);
	}
	else
	{
	    Zebra::logger->error("[����]û���ҵ�IDΪ:%u��game",pAtt->gameID);
	}
    }
    if(!isnormal)   //��������
    {
	pDef->data.popDamValue = hp;
    }
    DWORD oldArmor = pDef->data.armor;
    if(pDef->hasArmor())    //�л���ֵ
    {
	if(pDef->data.armor >= hp)
	{
	    pDef->data.armor -= hp;
	    hp = 0;
	}
	else
	{
	    hp -= pDef->data.armor;
	    pDef->data.armor = 0;
	}
    }
    DWORD oldHp = pDef->data.hp;
    if(pDef->data.hp >= hp)
	pDef->data.hp -= hp;
    else 
	pDef->data.hp = 0;
    Zebra::logger->debug("[����] ֱ�ӿ�Ѫ ��Ѫ����:%u (%s)Ѫ���仯(Ѫ:%u-->%u) ���ױ仯(����:%u-->%u)",hp, pDef->base->name, oldHp, pDef->data.hp, oldArmor, pDef->data.armor);

    ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pDef->gameID);
    if(game)
    {
	if(pDef->pk.beHurtID)
	{
	    Cmd::stCardAttackMagicUserCmd unit;
	    unit.dwAttThisID = pDef->data.qwThisID;
	    unit.dwMagicType = pDef->pk.beHurtID;
	    game->addActionList(unit);
	    Zebra::logger->debug("�������˼���ִ�б�");
	}
	game->addHurtList(pDef->data.qwThisID);	    //�ռ����˵Ľ�ɫ
	if(!isnormal)
	    game->dealHaloEffectAction();	    //�⻷���
    }
    return 0;
}

bool zCard::drawCards(DWORD num, DWORD dwSkillID)
{
    if(this->isHero())
    {
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(this->gameID);
	if(!game)
	    return false;
	for(DWORD index=0; index<num; index++)
	{
	    if(this->playerID == game->players[0].playerID)
	    {
		game->drawOneCard(this->playerID, game->players[0].cardsLibVec, dwSkillID);
	    }
	    else if(this->playerID == game->players[1].playerID)
	    {
		game->drawOneCard(this->playerID, game->players[1].cardsLibVec, dwSkillID);
	    }

	}
    }
    return false;
}

bool zCard::addHeroMp(DWORD value)
{
    if(this->isHero())
    {
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(this->gameID);
	if(!game)
	    return false;
	if(this->playerID == game->players[0].playerID)
	{
	    game->players[0].addMp(value);
	}
	else if(this->playerID == game->players[1].playerID)
	{
	    game->players[1].addMp(value);
	}
	game->sendMpInfo();
    }
    return false;
}

bool zCard::addHeroMaxMp(DWORD value)
{
    if(this->isHero())
    {
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(this->gameID);
	if(!game)
	    return false;
	if(this->playerID == game->players[0].playerID)
	{
	    game->players[0].addMaxMp(value);
	}
	else if(this->playerID == game->players[1].playerID)
	{
	    game->players[1].addMaxMp(value);
	}
	game->sendMpInfo();
    }
    return false;
}

bool zCard::summonAttend(DWORD cardID, DWORD num, DWORD dwMagicType)
{
    if(this->isHero())
    {
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(this->gameID);
	if(!game)
	    return false;
	for(DWORD index=0; index<num; index++)
	{
	    game->CardToCommonSlot(this->playerID, cardID, dwMagicType);
	}
	return true;
    }
    return false;
}

bool zCard::randomDropHand(DWORD playerID, DWORD num)
{
    if(this->isHero())
    {
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(this->gameID);
	if(!game)
	    return false;
	for(DWORD index=0; index<num; index++)
	{
	    game->dropOneRandomCardFromHandSlot(this->playerID);
	}
	return true;
    }
    return false;
}

bool zCard::processDeath(zCard *pAtt, zCard*& pDef)
{
    if(!pDef)
    {
	Zebra::logger->debug("�Ѿ�����һ��");
	return false;
    }
    if((pDef->isAttend() && pDef->isDie())
	    ||(pDef->isEquip() && pDef->isDie()))
    {
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pDef->gameID);
	if(game)
	{
	    DWORD playerID = pDef->playerID;
	    if(pDef->hasDeadLanguage())
	    {//������������,ȱ�ټ����ͷ�����,�������Լ�Ӣ����������
		zCard *hero = game->getSelfHero(playerID);    
		if(hero)
		{
		    Cmd::stCardAttackMagicUserCmd in;
		    in.dwAttThisID = hero->data.qwThisID;
		    in.dwDefThisID = 0;
		    for(std::vector<DWORD>::iterator it=pDef->pk.deadIDVec.begin(); it!=pDef->pk.deadIDVec.end(); it++)
		    {
			in.dwMagicType = *it;
			game->addActionList(in);
		    }
		}
	    }
	    game->dealOtherEffectAction(pDef->data.qwThisID, Cmd::ACTION_TYPE_DEAD);
	    
	    game->logToObj(pDef, "DELETE ����");
	    game->slots.removeObject(pDef->playerID, pDef, true, true, Cmd::OP_ATTACK_DELETE); 
	}
	else
	{
	    Zebra::logger->debug("��ս�Ѿ���������"); 
	}

    }
    else if(pDef->isHero() && pDef->isDie())
    {
	Zebra::logger->debug("[����]��ս:%u ���%u Ӣ������", pDef->gameID, pDef->playerID);
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pDef->gameID);
	if(game)
	{
	    game->dealGameResult(pDef->playerID, false);
	    game->on_GameOver();
	}
    }
    else if(pDef->isMagicCard())
    {
	ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pDef->gameID);
	if(game)
	{
	    game->logToObj(pDef, "DELETE ������");
	    game->slots.removeObject(pDef->playerID, pDef, true, true, Cmd::OP_FASHUCARD_DELETE); 
	}
    }
    return true;
}

bool zCard::refreshCard(DWORD pAttThisID)
{
    ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(this->gameID);
    if(!game)
	return false;
    zCard* pDef = game->slots.gcm.getObjectByThisID(pAttThisID);
    game->refreshCardInfo(pDef);
    return true;
}

bool zCard::isEnemy(zCard* entry)
{
    if(this->playerID != entry->playerID)
	return true;
    return false;
}

void zCard::showCurrentEffect(const DWORD state,bool isShow)
{   
    ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(this->gameID);
    if(!game)
	return;
    zStateB *statebase = statebm.get(state);
    if(!statebase)
	return;
    SceneUser *pUser = SceneUserManager::getMe().getUserByID(this->playerID);
    SceneUser *pOther = game->getOther(this->playerID);
    if(isShow)
    {
	DWORD realState = state;
	if(statebase->mainBuff)
	{
	    while(!this->setCardState(realState))
	    {
		realState++;
		if(realState >= state+5)
		    return;
	    }
	}
	else
	{
	    if(!this->setCardState(realState))
		return;
	}
	Cmd::stRetSetCardOneStateUserCmd send;
	send.dwThisID = this->data.qwThisID;
	send.stateNum = realState;
	if(pUser)
	{
	    send.who = 1;
	    pUser->sendCmdToMe(&send, sizeof(send));
	}
	if(pOther)
	{
	    send.who = 2;
	    pOther->sendCmdToMe(&send, sizeof(send));
	}
    }
    else
    {
	this->clearCardState(state);
	Cmd::stRetClearCardOneStateUserCmd send;
	send.dwThisID = this->data.qwThisID;
	send.stateNum = state;
	if(pUser)
	{
	    send.who = 1;
	    pUser->sendCmdToMe(&send, sizeof(send));
	}
	if(pOther)
	{
	    send.who = 2;
	    pOther->sendCmdToMe(&send, sizeof(send));
	}
	Zebra::logger->debug("[���״̬] state:%u", state);
    }
}

bool zCard::addOneHaloInfo(DWORD pAttThisID, t_haloInfo info)
{
    std::map<DWORD, t_haloInfo>::iterator it = this->haloInfoMap.find(pAttThisID);
    if(it == this->haloInfoMap.end())
    {
	//this->haloInfoMap.insert(std::make_pair(pAttThisID, info));
	this->haloInfoMap[pAttThisID] = info;
	return true;
    }
    return false;
}

/**
 * \brief   ȥ�����ϵ�һ���⻷
 * \param   pAttThisID���⻷�ͷ���
 * \return
*/
bool zCard::clearOneHaloInfo(DWORD pAttThisID)
{
    std::map<DWORD, t_haloInfo>::iterator it = this->haloInfoMap.find(pAttThisID);
    if(it != this->haloInfoMap.end())
    {
	this->subDamage(it->second.damage);
	this->subMaxHp(it->second.maxhp);
	if(it->second.taunt)
	{
	    subTaunt();
	}
	if(it->second.charge)
	{
	    subCharge();
	}
	if(it->second.windfury)	
	{
	    subWindfury(); 
	}
	if(it->second.incrmpcost)
	{
	    subMpCost(it->second.incrmpcost);
	}
	if(it->second.decrmpcost)
	{
	    addMpCost(it->second.decrmpcost);
	}
	this->refreshCard(this->data.qwThisID);
	return true;
    }
    return false;
}

/**
 * \brief   ��ȫɾ�����ϵ�һ���⻷
 * \param   pAttThisID���⻷�ͷ���
 * \return
*/
bool zCard::removeOneHaloInfo(DWORD pAttThisID)
{
    std::map<DWORD, t_haloInfo>::iterator it = this->haloInfoMap.find(pAttThisID);
    if(it != this->haloInfoMap.end())
    {
	this->subDamage(it->second.damage);
	this->subMaxHp(it->second.maxhp);
	if(it->second.taunt)
	{
	    subTaunt();
	}
	if(it->second.charge)
	{
	    subCharge();
	}
	if(it->second.windfury)	
	{
	    subWindfury(); 
	}
	if(it->second.incrmpcost)
	{
	    subMpCost(it->second.incrmpcost);
	}
	if(it->second.decrmpcost)
	{
	    addMpCost(it->second.decrmpcost);
	}
	this->refreshCard(this->data.qwThisID);
	this->haloInfoMap.erase(it);
	return true;
    }
    return false;
}

/////////////////////////////////һϵ�е�condition//////////////////////////////////
bool zCard::injured()
{
    return (this->data.maxhp > this->data.hp);
}

bool zCard::damageGreat2()
{
    return (this->data.damage >= 2);
}

bool zCard::damageLess3()
{
    return (this->data.damage < 3);
}
