/**
 * \brief 定义技能状态管理器头文件
 *
 */

#include "SkillStatusManager.h"
#include "zCard.h"
#include "TimeTick.h"
#include "SceneUserManager.h"
#include "ChallengeGameManager.h"
#include "ChallengeGame.h"
#include "SummonCardCfg.h"

SkillStatusManager::SkillStatusFunc
SkillStatusManager::s_funlist[SkillStatusManager::MAX_SKILL_STATE_NUM];

/**
 * \brief  状态0 空状态,不做任何操作
 * \param pEntry 状态所有者
 * \param sse 技能状态
 * \return 技能状态返回值,参见头文件中的技能状态处理返回值枚举
 */
BYTE SkillStatus_0(zCard *pEntry,SkillStatusElement &sse)
{
  sse.byGoodnessType = SKILL_GOOD;
  sse.byMutexType = 0;
  return SKILL_RETURN;
}


/**
 * \brief   ��pEntry���X�㷨���˺�
 * \param
 * \return
*/
BYTE SkillStatus_1(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			pEntry->directDamage(pAtt, pEntry, sse.value, sse.value2);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry�ָ�X������
 * \param
 * \return
*/
BYTE SkillStatus_2(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->restoreLife(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry��ȡX����
 * \param
 * \return
*/
BYTE SkillStatus_3(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->drawCards(sse.value, sse.dwSkillID);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ����pEntry
 * \param
 * \return
*/
BYTE SkillStatus_4(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->toDie();
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���ӷ���ˮ��
 * \param
 * \return
*/
BYTE SkillStatus_5(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->addHeroMp(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���ӷ���ˮ������
 * \param
 * \return
*/
BYTE SkillStatus_6(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->addHeroMaxMp(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry�ٻ�X��A���
 * \param
 * \return
*/
BYTE SkillStatus_7(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->summonAttend(sse.value, sse.value2, sse.dwSkillID);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry����ʥ��
 * \param
 * \return
*/
BYTE SkillStatus_8(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->addShield();
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���ӳ��
 * \param
 * \return
*/
BYTE SkillStatus_9(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			if(sse.halo)
			{
			    t_haloInfo info;
			    info.charge = 1;
			    if(pEntry->addOneHaloInfo(sse.dwThisID, info))
			    {
				pEntry->addCharge();
			    }
			}
			else
			{
			    pEntry->addCharge();
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���ӳ���
 * \param
 * \return
*/
BYTE SkillStatus_10(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			if(sse.halo)
			{
			    t_haloInfo info;
			    info.taunt = 1;
			    if(pEntry->addOneHaloInfo(sse.dwThisID, info))
			    {
				pEntry->addTaunt();
			    }
			}
			else
			{
			    pEntry->addTaunt();
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���ӷ�ŭ
 * \param
 * \return
*/
BYTE SkillStatus_11(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			if(sse.halo)
			{
			    t_haloInfo info;
			    info.windfury = 1;
			    if(pEntry->addOneHaloInfo(sse.dwThisID, info))
			    {
				pEntry->addWindfury();
			    }
			}
			else
			{
			    pEntry->addWindfury();
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry����Ǳ��
 * \param
 * \return
*/
BYTE SkillStatus_12(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->addSneak();
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry��һ������
 * \param
 * \return
*/
BYTE SkillStatus_13(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->randomDropHand(pEntry->playerID, sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���ӻ���
 * \param
 * \return
*/
BYTE SkillStatus_14(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->addArmor(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���ù�
 * \param
 * \return
*/
BYTE SkillStatus_15(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->setDamage(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry����Ѫ
 * \param
 * \return
*/
BYTE SkillStatus_16(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->setHpBuff(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry��������ID
 * \param
 * \return
*/
BYTE SkillStatus_17(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->addDeadLanguage(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���������غϿ�ʼID
 * \param
 * \return
*/
BYTE SkillStatus_18(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			bool tmp = false;
			if(sse.value2)
			    tmp = true;
			pEntry->addRoundSID(sse.value, tmp);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���������غϽ���ID
 * \param
 * \return
*/
BYTE SkillStatus_19(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			bool tmp = false;
			if(sse.value2)
			    tmp = true;
			pEntry->addRoundEID(sse.value, tmp);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry����X�㹥��
 * \param
 * \return
*/
BYTE SkillStatus_20(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->subDamage(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry���X�㵽Y���˺�(�޷��˼ӳ�)
 * \param
 * \return
*/
BYTE SkillStatus_21(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			DWORD dam = zMisc::randBetween(sse.value, sse.value2);
			pEntry->directDamage(pAtt, pEntry, dam);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry���X�㵽Y���˺�(�з��˼ӳ�)
 * \param
 * \return
*/
BYTE SkillStatus_22(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			DWORD dam = zMisc::randBetween(sse.value, sse.value2);
			pEntry->directDamage(pAtt, pEntry, dam, 1);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry����Ǳ��
 * \param
 * \return
*/
BYTE SkillStatus_23(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->breakSneak();
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry��ɵ�ͬ��Ŀ�깥�������˺�
 * \param
 * \return
*/
BYTE SkillStatus_24(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			pEntry->directDamage(pAtt, pEntry, pEntry->data.damage, sse.value2);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry����ֵ����x
 * \param
 * \return
*/
BYTE SkillStatus_25(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->multiplyHpBuff(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry����������x
 * \param
 * \return
*/
BYTE SkillStatus_26(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->multiplyDamage(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry�ص�ӵ���ߵ�����
 * \param
 * \return
*/
BYTE SkillStatus_27(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			DWORD playerID = pEntry->playerID;	    //ӵ����
			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    game->logToObj(pEntry, "DELETE �ص�ӵ��������");
			    game->slots.removeObject(playerID, pEntry);	    
			    game->CardToHandSlot(playerID, cardID);
			    Zebra::logger->debug("%u �ص�ӵ����(%u)����", cardID, playerID);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry�ص�����Ӣ������
 * \param
 * \return
*/
BYTE SkillStatus_28(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;

			DWORD playerID = pAtt->playerID;	    //����
			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    game->logToObj(pEntry, "DELETE �ص���������");
			    game->slots.removeObject(playerID, pEntry);	    
			    game->CardToHandSlot(playerID, cardID);
			    Zebra::logger->debug("%u �ص�����(%u)����", cardID, playerID);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry�ص��з�Ӣ������
 * \param
 * \return
*/
BYTE SkillStatus_29(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;

			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    DWORD playerID = 0;	    //�з�
			    if(pAtt->playerID == game->players[0].playerID)
			    {
				playerID = game->players[1].playerID;
			    }
			    else if(pAtt->playerID == game->players[1].playerID)
			    {
				playerID = game->players[0].playerID;
			    }
			    game->logToObj(pEntry, "DELETE �ص��з�����");
			    game->slots.removeObject(playerID, pEntry);	    
			    game->CardToHandSlot(playerID, cardID);
			    Zebra::logger->debug("%u �ص��з�(%u)����", cardID, playerID);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���� �����˺�����+X
 * \param
 * \return
*/
BYTE SkillStatus_30(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->addMagicDam(sse.value);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   pEntry �ٻ���ս��
 * \param
 * \return
*/
BYTE SkillStatus_31(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			for(WORD i=0; i<sse.value2; i++)
			{
			    DWORD cardID = SummonCardCfg::getMe().randomOneIDByIndex(sse.value);
			    pEntry->summonAttend(cardID, 1, sse.dwSkillID);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   �ٻ�������
 * \param
 * \return
*/
BYTE SkillStatus_32(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    for(WORD i=0; i<sse.value2; i++)
			    {
				DWORD cardID = SummonCardCfg::getMe().randomOneIDByIndex(sse.value);
				game->CardToHandSlot(pEntry->playerID, cardID);
			    }
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���ӵз��غϿ�ʼID
 * \param
 * \return
*/
BYTE SkillStatus_33(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			bool tmp = false;
			if(sse.value2)
			    tmp = true;
			pEntry->addEnemyRoundSID(sse.value, tmp);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry���ӵз��غϽ���ID
 * \param
 * \return
*/
BYTE SkillStatus_34(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			bool tmp = false;
			if(sse.value2)
			    tmp = true;
			pEntry->addEnemyRoundEID(sse.value, tmp);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ����pEntry������������
 * \param
 * \return
*/
BYTE SkillStatus_35(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;
			DWORD playerID = pAtt->playerID;	    //����
			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    for(WORD i=0; i<sse.value; i++)
			    {
				game->CardToHandSlot(playerID, cardID);
				Zebra::logger->debug("%u �����Ƹ��Ƶ�����(%u)����", cardID, playerID);
			    }
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ����pEntry���з�������
 * \param
 * \return
*/
BYTE SkillStatus_36(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;
			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    DWORD playerID = 0;       //�з�
			    if(pAtt->playerID == game->players[0].playerID)
			    {    
				playerID = game->players[1].playerID;
			    }    
			    else if(pAtt->playerID == game->players[1].playerID)
			    {    
				playerID = game->players[0].playerID;
			    }    
			    for(WORD i=0; i<sse.value; i++)
			    {
				game->CardToHandSlot(playerID, cardID);
				Zebra::logger->debug("%u �����Ƹ��Ƶ��з�(%u)����", cardID, playerID);
			    }
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ����pEntry���������ƿ�
 * \param
 * \return
*/
BYTE SkillStatus_37(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;
			DWORD playerID = pAtt->playerID;	    //����
			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    for(WORD i=0; i<sse.value; i++)
			    {
				game->addCardToLib(playerID, cardID);
				Zebra::logger->debug("%u �����Ƹ��Ƶ�����(%u)�ƿ�", cardID, playerID);
			    }
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ����pEntry���з����ƿ�
 * \param
 * \return
*/
BYTE SkillStatus_38(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;
			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    DWORD playerID = 0;       //�з�
			    if(pAtt->playerID == game->players[0].playerID)
			    {    
				playerID = game->players[1].playerID;
			    }    
			    else if(pAtt->playerID == game->players[1].playerID)
			    {    
				playerID = game->players[0].playerID;
			    }    
			    for(WORD i=0; i<sse.value; i++)
			    {
				game->addCardToLib(playerID, cardID);
				Zebra::logger->debug("%u �����Ƹ��Ƶ��з�(%u)����", cardID, playerID);
			    }
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntryϴ�ؼ������ƿ�
 * \param
 * \return
*/
BYTE SkillStatus_39(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;
			DWORD playerID = pAtt->playerID;	    //����
			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    game->logToObj(pEntry, "DELETE ϴ�ؼ����ƿ�");
			    game->slots.removeObject(pEntry->playerID, pEntry);
			    game->addCardToLib(playerID, cardID);
			    Zebra::logger->debug("%u ϴ�ص�����(%u)�ƿ�", cardID, playerID);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntryϴ�ص��з����ƿ�
 * \param
 * \return
*/
BYTE SkillStatus_40(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;
			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    DWORD playerID = 0;       //�з�
			    if(pAtt->playerID == game->players[0].playerID)
			    {    
				playerID = game->players[1].playerID;
			    }    
			    else if(pAtt->playerID == game->players[1].playerID)
			    {    
				playerID = game->players[0].playerID;
			    }
			    game->logToObj(pEntry, "DELETE ϴ�صз��ƿ�");
			    game->slots.removeObject(pEntry->playerID, pEntry);
			    game->addCardToLib(playerID, cardID);
			    Zebra::logger->debug("%u ϴ�ص��з�(%u)�ƿ�", cardID, playerID);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry����Ѫ��
 * \param
 * \return
*/
BYTE SkillStatus_41(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;
			pAtt->exchangeHp(pEntry);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry������Ѫ������
 * \param
 * \return
*/
BYTE SkillStatus_42(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			pEntry->exchangeHpDamage();
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ɾ��pEntry,������pEntry��λ�÷�һ���µ�x(����)
 * \param
 * \return
*/
BYTE SkillStatus_43(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			DWORD playerID = pEntry->playerID;	    //ӵ����
			DWORD oldID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    game->slots.replaceObject(playerID, pEntry, sse.value);
			    Zebra::logger->debug("��%u �滻��ԭ����%u", sse.value, oldID);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ��pEntry��ɵ�ͬ�ڹ����߹��������˺�
 * \param
 * \return
*/
BYTE SkillStatus_44(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			pEntry->directDamage(pAtt, pEntry, pAtt->data.damage, sse.value2);
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ɾ��pEntry,������pEntry��λ�÷�һ���µ�X��Y(����2)
 * \param
 * \return
*/
BYTE SkillStatus_45(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			DWORD playerID = pEntry->playerID;	    //ӵ����
			DWORD oldID = pEntry->base->id;
			DWORD newID = sse.value;
			if(zMisc::selectByPercent(50))
			{
			    newID = sse.value2;
			}
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    game->slots.replaceObject(playerID, pEntry, newID);
			    Zebra::logger->debug("����2 ��%u �滻��ԭ����%u", newID, oldID);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry��������X��
 * \param
 * \return
*/
BYTE SkillStatus_46(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			if(sse.halo)
			{
			    t_haloInfo info;
			    info.incrmpcost = sse.value;
			    if(pEntry->addOneHaloInfo(sse.dwThisID, info))
			    {
				pEntry->addMpCost(sse.value);
			    }
			}
			else
			{
			    pEntry->addMpCost(sse.value);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntry��������X��
 * \param
 * \return
*/
BYTE SkillStatus_47(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			if(sse.halo)
			{
			    t_haloInfo info;
			    info.decrmpcost = sse.value;
			    if(pEntry->addOneHaloInfo(sse.dwThisID, info))
			    {
				pEntry->subMpCost(sse.value);
			    }
			}
			else
			{
			    pEntry->subMpCost(sse.value);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ����pEntry������ս��
 * \param
 * \return
*/
BYTE SkillStatus_48(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;
			DWORD playerID = pAtt->playerID;	    //����
			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    for(WORD i=0; i<sse.value; i++)
			    {
				game->copyCardToCommonSlot(playerID, pEntry, sse.dwSkillID);
				Zebra::logger->debug("%u ��Ŀ�긴�Ƶ�����(%u)ս��", cardID, playerID);
			    }
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ����pEntry���з�ս��
 * \param
 * \return
*/
BYTE SkillStatus_49(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;
    
    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;
			DWORD cardID = pEntry->base->id;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    DWORD playerID = 0;       //�з�
			    if(pAtt->playerID == game->players[0].playerID)
			    {    
				playerID = game->players[1].playerID;
			    }    
			    else if(pAtt->playerID == game->players[1].playerID)
			    {    
				playerID = game->players[0].playerID;
			    }    
			    for(WORD i=0; i<sse.value; i++)
			    {
				game->copyCardToCommonSlot(playerID, pEntry, sse.dwSkillID);
				Zebra::logger->debug("%u ��Ŀ�긴�Ƶ��з�(%u)ս��", cardID, playerID);
			    }
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ΪpEntryװ��1������
 * \param
 * \return
*/
BYTE SkillStatus_50(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			zCard *pAtt = NULL;
			pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
			if(!pAtt)
			    return SKILL_RETURN;
			ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
			if(game)
			{
			    game->CardToEquipSlot(pEntry->playerID, sse.value);
			    Zebra::logger->debug("װ��һ��(%u)����", sse.value);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

/**
 * \brief   ɾ��pEntryӢ��,�����滻Ӣ��\Ӣ�ۼ���\Ӣ������
 * \param
 * \return
 */
BYTE SkillStatus_51(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_BAD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if(pEntry && pEntry->isHero())
		{
		    zCard *pAtt = NULL;
		    pAtt = ChallengeGameManager::getMe().getUserCardByThisID(pEntry->gameID, sse.dwThisID);
		    DWORD playerID = pEntry->playerID;	    //ӵ����
		    DWORD oldID = pEntry->base->id;
		    ChallengeGame *game = ChallengeGameManager::getMe().getGameByID(pEntry->gameID);
		    if(game)
		    {
			if(sse.percent > 0)
			{
			    game->slots.replaceObject(playerID, pEntry, sse.percent);
			    Zebra::logger->debug("��%u �滻��ԭ����%u(Ӣ��)", sse.percent, oldID);
			}

			if(sse.value > 0)
			{
			    zCard* pSkill = game->getSelfSkill(playerID);
			    if(pSkill)
			    {
				game->slots.replaceObject(playerID, pSkill, sse.value);
				Zebra::logger->debug("��%u �滻��ԭ����(Ӣ�ۼ���)", sse.value);
			    }
			    else
			    {
				game->CardToSkillSlot(playerID, sse.value);
			    }
			}

			if(sse.value2)
			{
			    game->CardToEquipSlot(playerID, sse.value2);
			}
		    }
		}
		return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}


/**
 * \brief   ΪpEntry���ӹ�����Ѫ��buff
 * \param
 * \return
*/
BYTE SkillStatus_1000(zCard *pEntry,SkillStatusElement &sse)
{
    sse.byGoodnessType = SKILL_GOOD;

    switch(sse.byStep)
    {
	case ACTION_STEP_START:
	    {
		if (zMisc::selectByPercent((int)(sse.percent)))
		{
		    if(pEntry)
		    {
			if(sse.halo)
			{
			    t_haloInfo info;
			    info.damage = sse.value;
			    info.maxhp = sse.value2;
			    if(pEntry->addOneHaloInfo(sse.dwThisID, info))
			    {
				pEntry->addDamage(sse.value);
				pEntry->addHpBuff(sse.value2);
			    }
			}
			else
			{
			    pEntry->addDamage(sse.value);
			    pEntry->addHpBuff(sse.value2);
			}
		    }
		    return SKILL_RETURN;
		}
		else
		    return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_TIMER:
	    {
		return SKILL_RETURN;
	    }
	    break;
	case ACTION_STEP_STOP:
	case ACTION_STEP_CLEAR:
	    {
		return SKILL_RETURN;
	    }
	    break;
	default:
	    break;
    }

    return SKILL_RETURN;
}

//--------------------------------------------------

/**
 * \brief 技能状态管理器构造函数
 */
SkillStatusManager::SkillStatusManager()
{
  entry = NULL;
  bclearActiveSkillStatus = false;
}


/**
 * \brief 技能状态管理器析构函数
 */
SkillStatusManager::~SkillStatusManager()
{
  //if (entry->getType() == zSceneEntry::SceneEntry_Player)  saveSkillStatus();
}

/**
 * \brief 技能状态管理器析构函数
 * \param pEntry 传入技能管理器的使用对象
 */
void SkillStatusManager::initMe(zCard *pEntry)
{
  entry = pEntry;
}

void SkillStatusManager::initFunctionList()
{
    std::fill(s_funlist, s_funlist + MAX_SKILL_STATE_NUM, SkillStatus_0);
    s_funlist[MAX_SKILL_STATE_NUM -1] = NULL;	//β����������ж�
    initFunctionListUnsafe();
    if(NULL != s_funlist[MAX_SKILL_STATE_NUM-1])
    {
	Zebra::logger->error("SkillStatusManager ״̬���������б����");
	exit(-1);
    }
}

void SkillStatusManager::initFunctionListUnsafe()
{
    s_funlist[0] = SkillStatus_0;
    s_funlist[1] = SkillStatus_1;
    s_funlist[2] = SkillStatus_2;
    s_funlist[3] = SkillStatus_3;
    s_funlist[4] = SkillStatus_4;
    s_funlist[5] = SkillStatus_5;
    s_funlist[6] = SkillStatus_6;
    s_funlist[7] = SkillStatus_7;
    s_funlist[8] = SkillStatus_8;
    s_funlist[9] = SkillStatus_9;
    s_funlist[10] = SkillStatus_10;
    s_funlist[11] = SkillStatus_11;
    s_funlist[12] = SkillStatus_12;
    s_funlist[13] = SkillStatus_13;
    s_funlist[14] = SkillStatus_14;
    s_funlist[15] = SkillStatus_15;
    s_funlist[16] = SkillStatus_16;
    s_funlist[17] = SkillStatus_17;
    s_funlist[18] = SkillStatus_18;
    s_funlist[19] = SkillStatus_19;
    s_funlist[20] = SkillStatus_20;
    s_funlist[21] = SkillStatus_21;
    s_funlist[22] = SkillStatus_22;
    s_funlist[23] = SkillStatus_23;
    s_funlist[24] = SkillStatus_24;
    s_funlist[25] = SkillStatus_25;
    s_funlist[26] = SkillStatus_26;
    s_funlist[27] = SkillStatus_27;
    s_funlist[28] = SkillStatus_28;
    s_funlist[29] = SkillStatus_29;
    s_funlist[30] = SkillStatus_30;
    s_funlist[31] = SkillStatus_31;
    s_funlist[32] = SkillStatus_32;
    s_funlist[33] = SkillStatus_33;
    s_funlist[34] = SkillStatus_34;
    s_funlist[35] = SkillStatus_35;
    s_funlist[36] = SkillStatus_36;
    s_funlist[37] = SkillStatus_37;
    s_funlist[38] = SkillStatus_38;
    s_funlist[39] = SkillStatus_39;
    s_funlist[40] = SkillStatus_40;
    s_funlist[41] = SkillStatus_41;
    s_funlist[42] = SkillStatus_42;
    s_funlist[43] = SkillStatus_43;
    s_funlist[44] = SkillStatus_44;
    s_funlist[45] = SkillStatus_45;
    s_funlist[46] = SkillStatus_46;
    s_funlist[47] = SkillStatus_47;
    s_funlist[48] = SkillStatus_48;
    s_funlist[49] = SkillStatus_49;
    s_funlist[50] = SkillStatus_50;
    s_funlist[51] = SkillStatus_51;
    s_funlist[52] = SkillStatus_0;
    s_funlist[53] = SkillStatus_0;
    s_funlist[54] = SkillStatus_0;
    s_funlist[55] = SkillStatus_0;
    s_funlist[56] = SkillStatus_0;
    s_funlist[57] = SkillStatus_0;
    s_funlist[58] = SkillStatus_0;
    s_funlist[59] = SkillStatus_0;
    s_funlist[60] = SkillStatus_0;
    s_funlist[61] = SkillStatus_0;
    s_funlist[62] = SkillStatus_0;
    s_funlist[63] = SkillStatus_0;
    s_funlist[64] = SkillStatus_0;
    s_funlist[65] = SkillStatus_0;
    s_funlist[66] = SkillStatus_0;
    s_funlist[67] = SkillStatus_0;
    s_funlist[68] = SkillStatus_0;
    s_funlist[69] = SkillStatus_0;
    s_funlist[70] = SkillStatus_0;
    s_funlist[1000] = SkillStatus_1000;		//+X��,+YѪ
}
/**
 * \brief 通知某个状态数值
 */
void SkillStatusManager::sendSelectStates(zCard *pThis,DWORD state,WORD value,WORD time)
{
#if 0
  pThis->sendtoSelectedState(state,value,time);
  pThis->sendSevenStateToMe(state,value,time);
#endif
}

/**
 * \brief 得到当前7大状态的数值
 */
void SkillStatusManager::getSelectStates(Cmd::stSelectReturnStatesPropertyUserCmd *buf,unsigned long maxSize)
{

  using namespace Cmd;
  std::map<DWORD,SkillStatusElement>::iterator tIterator;
  zRTime ctv;
  for(tIterator = _activeElement.begin() ; tIterator !=_activeElement.end(); tIterator++)
  {
    if (tIterator->second.state >0 && 
        (sizeof(stSelectReturnStatesPropertyUserCmd) + sizeof(buf->states[0]) * buf->size)<= maxSize)
    {
      buf->states[buf->size].state = tIterator->second.state;
      buf->states[buf->size].result = tIterator->second.value;
      buf->states[buf->size].time = tIterator->second.dwTime;
      buf->size ++;
    }
/*        break;
      default:
        break;
    }*/
  }

  for(tIterator = _recoveryElement.begin() ; tIterator !=_recoveryElement.end(); tIterator++)
  {

    if (tIterator->second.state >0 &&
        (sizeof(stSelectReturnStatesPropertyUserCmd) + sizeof(buf->states[0]) * buf->size)<= maxSize)
    {
      buf->states[buf->size].state = tIterator->second.state;
      buf->states[buf->size].result = tIterator->second.value;
      buf->states[buf->size].time = (tIterator->second.qwTime- ctv.msecs())/1000;
      buf->size ++;
    }
  }

}

#if 0
/**
 * \brief 加载历史的技能状态,上次下线的时候技能还存留在身上的技能状态继续有效
 */
void SkillStatusManager::loadSkillStatus(char *buf,DWORD length)
{
  SkillStatusElement *value;
//  char buf[zSocket::MAX_DATASIZE];
  DWORD count;
  DWORD statelen;
  bool notify;
  /*DWORD length;

  count = 0; 
  length=0;
  bzero(buf,zSocket::MAX_DATASIZE);*/

  SkillState *pState = (SkillState *)buf;
  statelen = sizeof(SkillState);
  memcpy(&(entry->skillValue),pState,statelen,sizeof(entry->skillValue));
  length-=statelen;

#ifdef _DEBUG
  Zebra::logger->debug("[为角色(%s)(%d)加载保存的技能状态]",entry->name,entry->id);
#endif
  //COfflineSkillStatus::getOfflineSkillStatus(entry->id,buf,length);
  count = length/sizeof(SkillStatusElement);
  notify = false;
#ifdef _DEBUG
  Zebra::logger->debug("[有%d个技能状态需要加载]",count);
#endif
  value = (SkillStatusElement *)(buf+statelen);

  SceneEntryPk *pAtt = NULL;
  if (value->attacktype == zSceneEntry::SceneEntry_Player)
  {
    pAtt = SceneUserManager::getMe().getUserByID(value->dwAttackerID);
    if (pAtt) value->dwTempID = pAtt->tempid;
  }

  for(DWORD i=0; i<count; i++)
  {
    value->byStep = ACTION_STEP_RELOAD;
    switch(runStatusElement(*value))
    {
      case SKILL_RECOVERY:
        {
#ifdef _DEBUG
          Zebra::logger->debug("[%d号技能状态被加载到临时被动表内]",value->id);
#endif
          //value->refresh = 1;
          //value->qwTime = SceneTimeTick::currentTime.msecs()+value->dwTime *1000;
          _recoveryElement[value->id/*value->byMutexType*/]=*value;
          _recoveryElement[value->id].qwTime = SceneTimeTick::currentTime.msecs()+_recoveryElement[value->id].dwTime *1000;
          if (value->state >0)
          {
            sendSelectStates(entry,value->state,value->value,value->dwTime);
            //entry->setEffectStatus(value->state);
            entry->showCurrentEffect(value->state,true);
          }
          notify = true;
        }
        break;
      case SKILL_ACTIVE:
        {
#ifdef _DEBUG
          Zebra::logger->debug("[%d号技能状态被加载到主动表内]",value->id);
#endif
          _activeElement[value->id/*value->byMutexType*/]=*value;
          _recoveryElement[value->id].qwTime = SceneTimeTick::currentTime.msecs()+_recoveryElement[value->id].dwTime *1000;
          if (value->state >0)
          {
            sendSelectStates(entry,value->state,value->value,value->dwTime);
            //entry->setEffectStatus(value->state);
            entry->showCurrentEffect(value->state,true);
          }
          notify = true;
        }
        break;
      default:
#ifdef _DEBUG
          Zebra::logger->debug("[%d号技能状态无法被加到对应的表中]",value->id);
#endif
        break;
    }
    value++;
  }

  if (notify&&entry)
  {
    entry->changeAndRefreshHMS(true,true);
  }
}

/**
 * \brief 存储历史的技能状态,上次下线的时候技能还存留在身上的技能状态继续有效
 */
void SkillStatusManager::saveSkillStatus(char *buf,DWORD &size)
{
  std::map<DWORD,SkillStatusElement>::iterator tIterator;
  SkillStatusElement *value;
  DWORD count;
  DWORD statelen;
  DWORD length;

  length = 0;
  count = 0;
  statelen = 0;

  SkillState *pState = (SkillState *)buf;
  statelen = sizeof(SkillState);
  memcpy(pState,&(entry->skillValue),statelen,statelen);

  value = (SkillStatusElement *)(buf+statelen);
  length = sizeof(SkillStatusElement);
  for(tIterator = _activeElement.begin() ; tIterator !=_activeElement.end();tIterator ++)
  {
    memcpy(value,&tIterator->second,length,length);
#ifdef _DEBUG
    Zebra::logger->debug("[技能状态%d被存储]",tIterator->second.id);
#endif
    count++;
    value++;
    if (count>3000) break;
  }

  if (count<3000)
  {
    for(tIterator = _recoveryElement.begin() ; tIterator !=_recoveryElement.end();tIterator ++)
    {
      memcpy(value,&tIterator->second,length,length);
#ifdef _DEBUG
    Zebra::logger->debug("[技能状态%d被存储]",tIterator->second.id);
#endif
      count++;
      value++;
      if (count>3000) break;
    }
  }
  //if (count >0) COfflineSkillStatus::writeOfflineSkillStatus(entry->id,buf,count*length);
  size = statelen + count*length;
#ifdef _DEBUG
  Zebra::logger->debug("[有%d个技能状态需要存储]",count);
#endif
}
#endif

#if 0
/**
 * \brief 将一个技能操作施加在我的身上[SKY技能链第3步]
 * \param carrier 技能操作投送器,里面包含了技能状态
 * \param good 标志这个技能是不是一个增益的技能
 * \return true 为继续下一个操作,false为停止并返回。
 */
bool SkillStatusManager::putOperationToMe(const SkillStatusCarrier &carrier,const bool good)
{
  std::vector<SkillElement>::const_iterator tIterator;
  bool sendData = false;

//  if (!entry->preAttackMe(carrier.attacker,&carrier.revCmd)) return true;

  for (tIterator = carrier.status->_StatusElementList.begin();
    tIterator != carrier.status->_StatusElementList.end();
    tIterator++)
  {
    SkillStatusElement element;
    element.byStep  =  ACTION_STEP_START;
    element.id    =  tIterator->id;
    element.percent =  tIterator->percent;
    element.value  =  tIterator->value;
    element.value2  =  tIterator->value2;
    element.state  =  tIterator->state;

    element.dwSkillID = carrier.skillbase->skillid;//carrier.skillID;
    //element.dwTime = tIterator->time;
    //element.qwTime = SceneTimeTick::currentTime.msecs()+element.dwTime *1000;
    element.halo = tIterator->halo;

    zCard *pAtt = carrier.attacker;
    element.dwThisID = pAtt->data.qwThisID;
    element.dwAttackerID = carrier.attacker->playerID;	    //�ͷż��ܵĽ�ɫID
    if (element.dwTime <=2)
    {
      element.refresh = 0;
    }
    else
    {
      element.refresh = 1;
      sendData = true;
    }
#if 0
    entry->curMagicManType = element.attacktype;
    entry->curMagicManID = element.dwTempID;
    entry->keepPos.x = (DWORD)carrier.revCmd.xDes;
    entry->keepPos.y = (DWORD)carrier.revCmd.yDes;
    entry->keepDir = carrier.revCmd.byDirect;
#endif
    switch(runStatusElement(element))
    {
#if 0
      case SKILL_RECOVERY:
        {
#ifdef _DEBUG
          Zebra::logger->debug("[临时被动]第[%u]号状态被施加在身上持续时间为[%u]",element.id,element.dwTime);
          Channel::sendSys(entry->tempid,Cmd::INFO_TYPE_SYS,"[临时被动]第[%u]号状态被施加在身上持续时间为[%u]",element.id,element.dwTime);
#endif
          element.qwTime = SceneTimeTick::currentTime.msecs()+element.dwTime *1000;
          if (element.state >0)
          {
            clearMapElement(element.id/*element.byMutexType*/,_recoveryElement,element.id,false);
            _recoveryElement[element.id/*element.byMutexType*/]=element;
            sendSelectStates(entry,element.state,element.value,element.dwTime);
            //entry->setEffectStatus(element.state);
            entry->showCurrentEffect(element.state,true);
          }
          else
          {
            clearMapElement(element.id/*element.byMutexType*/,_recoveryElement,element.id);
            _recoveryElement[element.id/*element.byMutexType*/]=element;
          }
        }
        break;
      case SKILL_BREAK:
        {
          return false;
        }
        break;
      case SKILL_ACTIVE:
        {
#ifdef _DEBUG
          Zebra::logger->debug("[伤害状态]第[%u]号状态被施加在身上持续时间为[%u]",element.id,element.dwTime);
          Channel::sendSys(entry->tempid,Cmd::INFO_TYPE_SYS,"[攻击状态]第[%u]号状态被施加在身上持续时间为[%u]",element.id,element.dwTime);
#endif
          if (element.dwTime==0) break;
          clearMapElement(element.id/*element.byMutexType*/,_activeElement,element.id);
          _activeElement[element.id/*element.byMutexType*/]=element;
          if (element.state >0)
          {
            sendSelectStates(entry,element.state,element.value,element.dwTime);
            //entry->setEffectStatus(element.state);
            entry->showCurrentEffect(element.state,true);
          }
        }
        break;
#endif
      case SKILL_RETURN:
	{
          if (element.state > 0)
          {
            entry->showCurrentEffect(element.state, true);
          }
	}
      default:
        break;
    }
  }
  if(entry)
  {
      entry->refreshCard(entry->data.qwThisID);
  }
  return true;
}
#endif

/**
 * \brief 定时器刷新
 */
void SkillStatusManager::timer()
{
#if 0
  std::map<DWORD,SkillStatusElement>::iterator tIterator,delIterator;
  bool dirty=false;
  bool sendData =false;

  for(tIterator = _activeElement.begin() ; tIterator !=_activeElement.end() ; )
  {
#ifdef _DEBUG
    Zebra::logger->debug("[计时.伤]施加在身上的第[%u]号状态剩下时间[%u]",tIterator->second.id,tIterator->second.dwTime);
    Channel::sendSys(entry->tempid,Cmd::INFO_TYPE_SYS,"[计时.伤]施加在身上的第[%u]号状态剩下时间[%u]",tIterator->second.id,tIterator->second.dwTime);
#endif
	if (tIterator->second.dwTime>0)
	{
		//sky 献祭和灵魂状态没有时间概念
		if(tIterator->second.dwSkillID != SKILLID_IMMOLATE && tIterator->second.dwSkillID != SKILLID_SOUL)
			tIterator->second.dwTime --;
	}
	else 
	{
		tIterator->second.dwTime = 0;
	}

    tIterator->second.byStep = ACTION_STEP_TIMER;
    runStatusElement(tIterator->second);

    if (0 == tIterator->second.dwTime)
    {
      tIterator->second.byStep = ACTION_STEP_STOP;
      runStatusElement(tIterator->second);
      if (tIterator->second.refresh) sendData = true;
      if (tIterator->second.state >0)
      {
        //entry->clearEffectStatus(tIterator->second.state);
        entry->showCurrentEffect(tIterator->second.state,false);
      }
#ifdef _DEBUG
    Zebra::logger->debug("[伤害状态]施加在身上的第[%u]号状态被删除",tIterator->second.id);
    Channel::sendSys(entry->tempid,Cmd::INFO_TYPE_SYS,"[伤害状态]施加在身上的第[%u]号状态被删除",tIterator->second.id);
#endif
      delIterator = tIterator;
      tIterator ++;
      _activeElement.erase(delIterator->first);
      dirty = true;
    }
    else
    {
      tIterator ++;
    }
  }

  for(tIterator = _recoveryElement.begin() ; tIterator !=_recoveryElement.end();)
  {
    QWORD curQtime = SceneTimeTick::currentTime.msecs();
    if (curQtime >= tIterator->second.qwTime)
    {
      tIterator->second.byStep = ACTION_STEP_STOP;
      runStatusElement(tIterator->second);
      if (tIterator->second.refresh) sendData = true;
      if (tIterator->second.state >0)
      {
        //entry->clearEffectStatus(tIterator->second.state);
        entry->showCurrentEffect(tIterator->second.state,false);
      }
#ifdef _DEBUG
      Zebra::logger->debug("[临时被动]施加在身上的第[%u]号状态被删除",tIterator->second.id);
      Channel::sendSys(entry->tempid,Cmd::INFO_TYPE_SYS,"[临时被动]施加在身上的第[%u]号状态被删除",tIterator->second.id);
#endif
      delIterator = tIterator;
      tIterator ++;
      _recoveryElement.erase(delIterator->first);
      dirty = true;
    }
    else
    {
      tIterator->second.dwTime = (tIterator->second.qwTime - curQtime)/1000;
      tIterator ++;
    }
  }
  if (entry->notifyHMS)
  {
    entry->attackRTHpAndMp();
  }
  if (dirty||entry->reSendData)
  {
    entry->changeAndRefreshHMS(false,sendData);
  }
  //std::map<DWORD,SkillStatusElement>::iterator tIterator,delIterator;
  if (bclearActiveSkillStatus)
  {
    for(tIterator = _activeElement.begin() ; tIterator !=_activeElement.end() ; )
    {
      tIterator->second.byStep = ACTION_STEP_CLEAR;
      runStatusElement(tIterator->second);
      if (tIterator->second.state >0)
      {
        //entry->clearEffectStatus(tIterator->second.state);
        entry->showCurrentEffect(tIterator->second.state,false);
      }
      delIterator = tIterator;
      tIterator ++;
      _activeElement.erase(delIterator->first);
  
    }

    for(tIterator = _recoveryElement.begin() ; tIterator !=_recoveryElement.end();)
    {
      tIterator->second.byStep = ACTION_STEP_CLEAR;
      runStatusElement(tIterator->second);
      if (tIterator->second.state >0)
      {
        //entry->clearEffectStatus(tIterator->second.state);
        entry->showCurrentEffect(tIterator->second.state,false);
      }
      delIterator = tIterator;
      tIterator ++;
      _recoveryElement.erase(delIterator->first);
    }
    bclearActiveSkillStatus = false;
  }
#endif
}

/**
 * \brief 重新运行被动状态,包括永久被动和临时被动
 */
void SkillStatusManager::processPassiveness()
{
  std::map<DWORD,SkillStatusElement>::iterator tIterator;

  for(tIterator = _recoveryElement.begin() ; tIterator !=_recoveryElement.end() ; tIterator ++)
  {
    tIterator->second.byStep = ACTION_STEP_DOPASS;
    if (tIterator->second.percent<100) tIterator->second.percent=100;
    runStatusElement(tIterator->second);
  }

  for(tIterator = _passivenessElement.begin() ; tIterator !=_passivenessElement.end() ; tIterator ++)
  {
    tIterator->second.byStep = ACTION_STEP_DOPASS;
    if (tIterator->second.percent<100&&tIterator->second.id!=180) tIterator->second.percent=100;
    runStatusElement(tIterator->second);
  }
}

/**
 * \brief 执行一个具体的状态  [进入技能状态的实际处理]
 * \return 状态返回值
      SKILL_ACTIVE    //  加到活动MAP中
      SKILL_RECOVERY    //  加到临时被动MAP中
      SKILL_PASSIVENESS  //  加到永久被动MAP中
      SKILL_RETURN    //  返回不做任何操作
      SKILL_DONOW      //  立即执行属性值扣除动作
 */
BYTE SkillStatusManager::runStatusElement(SkillStatusElement &element)
{
  return  s_funlist[element.id>=(MAX_SKILL_STATE_NUM-1)?0:element.id](entry,element);
}

/**
 * \brief  清除人物身上的不良的非永久状态
 */
void SkillStatusManager::clearBadActiveSkillStatus()
{
  std::map<DWORD,SkillStatusElement>::iterator tIterator,delIterator;

  for(tIterator = _activeElement.begin() ; tIterator !=_activeElement.end() ; )
  {
    if (tIterator->second.byGoodnessType == SKILL_BAD)
    {
      tIterator->second.byStep = ACTION_STEP_CLEAR;
      runStatusElement(tIterator->second);
      if (tIterator->second.state >0)
      {
        //entry->clearEffectStatus(tIterator->second.state);
        entry->showCurrentEffect(tIterator->second.state,false);
      }
      delIterator = tIterator;
      tIterator ++;
      _activeElement.erase(delIterator->first);
    }
    else
    {
      tIterator ++;
    }
  }

  for(tIterator = _recoveryElement.begin() ; tIterator !=_recoveryElement.end();)
  {
    if (tIterator->second.byGoodnessType == SKILL_BAD)
    {
      tIterator->second.byStep = ACTION_STEP_CLEAR;
      runStatusElement(tIterator->second);
      if (tIterator->second.state >0)
      {
        //entry->clearEffectStatus(tIterator->second.state);
        entry->showCurrentEffect(tIterator->second.state,false);
      }
      delIterator = tIterator;
      tIterator ++;
      _recoveryElement.erase(delIterator->first);
    }
    else
    {
      tIterator ++;
    }
  }
}

/**
 * \brief  清除人物身上的非永久性状态
 */
void SkillStatusManager::clearActiveSkillStatus()
{
  bclearActiveSkillStatus = true;
}

/**
 * \brief  清除指定技能ID的技能状态
 */
void SkillStatusManager::clearSkill(DWORD dwSkillID)
{
  std::map<DWORD,SkillStatusElement>::iterator tIterator,delIterator;

  for(tIterator = _activeElement.begin() ; tIterator !=_activeElement.end() ; )
  {
    if (tIterator->second.dwSkillID == dwSkillID)
    {
      tIterator->second.byStep = ACTION_STEP_CLEAR;
      runStatusElement(tIterator->second);
      if (tIterator->second.state >0)
      {
        //entry->clearEffectStatus(tIterator->second.state);
        entry->showCurrentEffect(tIterator->second.state,false);
      }
	  tIterator->second.dwTime = 0;
	  tIterator ++;
      /*delIterator = tIterator;
      _activeElement.erase(delIterator->first);*/
    }
    else
    {
      tIterator++;
    }

  }

  for(tIterator = _recoveryElement.begin() ; tIterator !=_recoveryElement.end();)
  {
    if (tIterator->second.dwSkillID == dwSkillID)
    {
      tIterator->second.byStep = ACTION_STEP_CLEAR;
      runStatusElement(tIterator->second);
      if (tIterator->second.state >0)
      {
        //entry->clearEffectStatus(tIterator->second.state);
        entry->showCurrentEffect(tIterator->second.state,false);
      }
      delIterator = tIterator;
      tIterator ++;
      _recoveryElement.erase(delIterator->first);
    }
    else
    {
      tIterator++;
    }
  }
}


/**
 * \brief 按被动技能来处理这个操作
 * \param skillid 技能id
 * \param pSkillStatus 技能中的操作
 */
void SkillStatusManager::putPassivenessOperationToMe(const DWORD skillid,const SkillStatus *pSkillStatus)
{
  std::vector<SkillElement>::const_iterator tIterator;

  for (tIterator = pSkillStatus->_StatusElementList.begin();
    tIterator != pSkillStatus->_StatusElementList.end();
    tIterator++)
  {
    SkillStatusElement element;
    element.id    =  tIterator->id;
    element.percent =  tIterator->percent;
    element.value  =  tIterator->value;
    element.state  =  tIterator->state;
    element.dwSkillID = skillid;
    //element.dwTime = tIterator->time;
    _passivenessElement[element.id]=element;

#ifdef _DEBUG
      Zebra::logger->debug("[永久被动]之[%u]号状态被施加在身上",element.id);
      Channel::sendSys(entry->tempid,Cmd::INFO_TYPE_SYS,"[永久被动]之[%u]号状态被施加在身上",element.id);
#endif
  }
}

/**
 * \brief  增加不良状态的持续时间数值
 */
void SkillStatusManager::addBadSkillStatusPersistTime(const DWORD &value)
{
  std::map<DWORD,SkillStatusElement>::iterator tIterator;

  for(tIterator = _activeElement.begin() ; tIterator !=_activeElement.end() ; tIterator ++)
  {
    if (tIterator->second.byGoodnessType == SKILL_BAD)
    {
      tIterator->second.dwTime += value;
    }
  }

  for(tIterator = _recoveryElement.begin() ; tIterator !=_recoveryElement.end(); tIterator++)
  {
    if (tIterator->second.byGoodnessType == SKILL_BAD)
    {
      tIterator->second.dwTime += value;
      tIterator->second.qwTime += value;
    }
  }
}

/**
 * \brief  增加不良状态的持续时间百分比
 */
void SkillStatusManager::addBadSkillStatusPersistTimePercent(const DWORD &value)
{
  std::map<DWORD,SkillStatusElement>::iterator tIterator;

  for(tIterator = _activeElement.begin() ; tIterator !=_activeElement.end() ; tIterator ++)
  {
    if (tIterator->second.byGoodnessType == SKILL_BAD)
    {
      tIterator->second.dwTime = (DWORD)(tIterator->second.dwTime*(value/100.0f));
    }
  }

  for(tIterator = _recoveryElement.begin() ; tIterator !=_recoveryElement.end(); tIterator++)
  {
    if (tIterator->second.byGoodnessType == SKILL_BAD)
    {
      tIterator->second.dwTime = (DWORD)(tIterator->second.dwTime*(value/100.0f));
      tIterator->second.qwTime = (DWORD)(tIterator->second.qwTime*(value/100.0f));
    }
  }
}

/**
 * \brief  清除持续状态中的指定类别
 * \param byMutexType 技能大类
 * \param myMap 操作的状态map
 * \param dwID 过滤状态id
 */
void SkillStatusManager::clearMapElement(const BYTE &byMutexType,std::map<DWORD,SkillStatusElement> &myMap,DWORD dwID,bool notify)
{
/*  std::map<DWORD,SkillStatusElement>::iterator tIterator,delIterator;

  for(tIterator = myMap.begin() ; tIterator !=myMap.end();)
  {
    if (tIterator->second.byMutexType == byMutexType)
    {
      if (tIterator->second.id != dwID)
      {
        tIterator->second.byStep = ACTION_STEP_CLEAR;
        runStatusElement(tIterator->second);
      }
      if (tIterator->second.state >0)
      {
        //entry->clearEffectStatus(tIterator->second.state);
        entry->showCurrentEffect(tIterator->second.state,false);
      }
      delIterator = tIterator;
      tIterator ++;
      myMap.erase(delIterator->first);
    }
    else
    {
      tIterator ++;
    }
  }*/
  std::map<DWORD,SkillStatusElement>::iterator tIterator,delIterator;

  for(tIterator = myMap.begin() ; tIterator !=myMap.end();)
  {
    if (tIterator->second.id == byMutexType)
    {
      if (tIterator->second.id != dwID)
      {
        tIterator->second.byStep = ACTION_STEP_CLEAR;
        runStatusElement(tIterator->second);
      }
      if (tIterator->second.state >0)
      {
        //entry->clearEffectStatus(tIterator->second.state);
        entry->showCurrentEffect(tIterator->second.state,false);
      }
      delIterator = tIterator;
      tIterator ++;
      myMap.erase(delIterator->first);
    }
    else
    {
      tIterator ++;
    }
  }

}

void SkillStatusManager::clearRecoveryElement(DWORD value)
{
  clearMapElement(value,_recoveryElement,0);
}

void SkillStatusManager::clearActiveElement(DWORD value)
{
  clearMapElement(value,_activeElement,0);
}

void SkillStatusManager::processDeath()
{

}

#if 0
/**
 * \brief  测试函数用来显示技能状态当前值
 */
void SkillStatusManager::showValue()
{
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"角色身上的[SkillValue]属性列表:---------------------------");
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"伤害值增加固定数值1=%ld",entry->skillValue.dvalue);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"伤害值增加x%2=%ld",entry->skillValue.dvaluep);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"物理防御数值变更 57,86=%ld",entry->skillValue.pdefence);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"物理防御变更百分比=%ld",entry->skillValue.pdefencep);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"变为随机小动物79=%ld",entry->skillValue.topet);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"产生额外伤害83=%ld",entry->skillValue.appenddam);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"移动速度变更百分比 16,56=%ld",entry->skillValue.movespeed);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"减少技能施放间隔17=%ld",entry->skillValue.mgspeed);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"减少陷入冰冻状态几率18=%ld",entry->skillValue.coldp);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"减少陷入中毒状态几率19=%ld",entry->skillValue.poisonp);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"减少陷入石化状态几率20=%ld",entry->skillValue.petrifyp);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"减少陷入失明状态几率21=%ld",entry->skillValue.blindp);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"减少陷入混乱状态几率22=%ld",entry->skillValue.chaosp);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"命中率增加33,64=%ld",entry->skillValue.atrating);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"命中率增加33,64=%ld",entry->skillValue.reduce_atrating);

  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"生命值恢复速度增加34=%ld",entry->skillValue.hpspeedup);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"法术值恢复速度增加35=%ld",entry->skillValue.mpspeedup);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"体力值恢复速度增加36=%ld",entry->skillValue.spspeedup);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"闪避率上升37=%ld",entry->skillValue.akdodge);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"反弹45=%ld",entry->skillValue.reflect);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"反弹x%46=%ld",entry->skillValue.reflectp);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"为反弹百分之几的敌人伤害50=%ld",entry->skillValue.reflect2);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"法术防御变更59,=%ld",entry->skillValue.mdefence);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"法术防御变更百分比x%=%ld",entry->skillValue.mdefencep);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"攻击速度变更80,81=%ld",entry->skillValue.uattackspeed);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"降低陷入七大状态几率82=%ld",entry->skillValue.sevendownp);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"伤害转移百分比=%ld",entry->skillValue.tsfdamp);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"被动:额外伤害=%ld",entry->skillValue.passdam);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"角色身上的[PkValue]属性列表:---------------------------");
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"物理攻击力=%ld",entry->pkValue.pdamage);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"物理防御力=%ld",entry->pkValue.pdefence);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"法术攻击力=%ld",entry->pkValue.mdamage);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"法术防御力=%ld",entry->pkValue.mdefence);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"消耗法术值=%ld",entry->pkValue.mcost);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"消耗生命值=%ld",entry->pkValue.hpcost);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"消耗体力值=%ld",entry->pkValue.spcost);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"经验消耗=%ld",entry->pkValue.exp);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"伤害值=%ld",entry->pkValue.dvalue);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"伤害值增加百分比=%ld",entry->pkValue.dvaluep);
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"列表结束:----------------------------------------------");
}

/**
 * \brief  打印技能状态当前值到log文件中
 */
void SkillStatusManager::showValueToLog()
{
  Zebra::logger->debug("角色身上的[SkillValue]属性列表:---------------------------");
  Zebra::logger->debug("伤害值增加固定数值1=%ld",entry->skillValue.dvalue);
  Zebra::logger->debug("伤害值增加x%2=%ld",entry->skillValue.dvaluep);
  Zebra::logger->debug("物理防御数值变更 57,86=%ld",entry->skillValue.pdefence);
  Zebra::logger->debug("物理防御变更百分比=%ld",entry->skillValue.pdefencep);
  Zebra::logger->debug("变为随机小动物79=%ld",entry->skillValue.topet);
  Zebra::logger->debug("产生额外伤害83=%ld",entry->skillValue.appenddam);
  Zebra::logger->debug("移动速度变更百分比 16,56=%ld",entry->skillValue.movespeed);
  Zebra::logger->debug("减少技能施放间隔17=%ld",entry->skillValue.mgspeed);
  Zebra::logger->debug("减少陷入冰冻状态几率18=%ld",entry->skillValue.coldp);
  Zebra::logger->debug("减少陷入中毒状态几率19=%ld",entry->skillValue.poisonp);
  Zebra::logger->debug("减少陷入石化状态几率20=%ld",entry->skillValue.petrifyp);
  Zebra::logger->debug("减少陷入失明状态几率21=%ld",entry->skillValue.blindp);
  Zebra::logger->debug("减少陷入混乱状态几率22=%ld",entry->skillValue.chaosp);
  Zebra::logger->debug("命中率增加33,64=%ld",entry->skillValue.atrating);
  Zebra::logger->debug("生命值恢复速度增加34=%ld",entry->skillValue.hpspeedup);
  Zebra::logger->debug("法术值恢复速度增加35=%ld",entry->skillValue.mpspeedup);
  Zebra::logger->debug("体力值恢复速度增加36=%ld",entry->skillValue.spspeedup);
  Zebra::logger->debug("闪避率上升37=%ld",entry->skillValue.akdodge);
  Zebra::logger->debug("反弹45=%ld",entry->skillValue.reflect);
  Zebra::logger->debug("反弹x%46=%ld",entry->skillValue.reflectp);
  Zebra::logger->debug("为反弹百分之几的敌人伤害50=%ld",entry->skillValue.reflect2);
  Zebra::logger->debug("法术防御变更59,=%ld",entry->skillValue.mdefence);
  Zebra::logger->debug("法术防御变更百分比x%=%ld",entry->skillValue.mdefencep);
  Zebra::logger->debug("攻击速度变更80,81=%ld",entry->skillValue.uattackspeed);
  Zebra::logger->debug("降低陷入七大状态几率82=%ld",entry->skillValue.sevendownp);
  Zebra::logger->debug("伤害转移百分比=%ld",entry->skillValue.tsfdamp);
  Zebra::logger->debug("被动:额外伤害=%ld",entry->skillValue.passdam);
  Zebra::logger->debug("角色身上的[PkValue]属性列表:---------------------------");
  Zebra::logger->debug("物理攻击力=%ld",entry->pkValue.pdamage);
  Zebra::logger->debug("物理防御力=%ld",entry->pkValue.pdefence);
  Zebra::logger->debug("法术攻击力=%ld",entry->pkValue.mdamage);
  Zebra::logger->debug("法术防御力=%ld",entry->pkValue.mdefence);
  Zebra::logger->debug("消耗法术值=%ld",entry->pkValue.mcost);
  Zebra::logger->debug("消耗生命值=%ld",entry->pkValue.hpcost);
  Zebra::logger->debug("消耗体力值=%ld",entry->pkValue.spcost);
  Zebra::logger->debug("经验消耗=%ld",entry->pkValue.exp);
  Zebra::logger->debug("伤害值=%ld",entry->pkValue.dvalue);
  Zebra::logger->debug("伤害值增加百分比=%ld",entry->pkValue.dvaluep);
  Zebra::logger->debug("列表结束:----------------------------------------------");
}

/**
 * \brief  显示角色身上的主动技能状态
 */
void SkillStatusManager::showActive()
{
  std::map<DWORD,SkillStatusElement>::iterator tIterator;
  SkillStatusElement *element;

  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"角色身上的主动状态列表:");
  for(tIterator = _activeElement.begin() ; tIterator !=_activeElement.end(); tIterator++)
  {
    std::string myname;
    char buf [45];
    element = &tIterator->second;
    SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(element->dwTempID);
    if (pUser)
      myname = pUser->name;
    else
    {
      sprintf(buf,"临时ID:%u",element->dwTempID);
      myname = buf;
    }
    Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"技能:%u 大类:%u 状态类别:%u %s 剩余时间:%u 攻击者:%s 几率:%u 总时间:%u 状态值%u 特效值:%u",element->dwSkillID,element->byMutexType,element->id,element->byGoodnessType==1?"伤害":"和平",element->dwTime,myname.c_str(),element->percent,element->dwTime,element->value,element->state);
  }
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"----------------------------------------------");
}

/**
 * \brief  显示角色身上的临时被动技能状态
 */
void SkillStatusManager::showRecovery()
{
  std::map<DWORD,SkillStatusElement>::iterator tIterator;
  SkillStatusElement *element;

  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"角色身上的临时被动状态列表:");
  for(tIterator = _recoveryElement.begin() ; tIterator !=_recoveryElement.end(); tIterator++)
  {
    std::string myname;
    char buf [45];
    element = &tIterator->second;
    SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(element->dwTempID);
    if (pUser)
      myname = pUser->name;
    else
    {
      sprintf(buf,"临时ID:%u",element->dwTempID);
      myname = buf;
    }
    Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"技能:%u 大类:%u 状态类别:%u %s 剩余时间:%u 攻击者:%s 几率:%u 总时间:%u 状态值%u 特效值:%u",element->dwSkillID,element->byMutexType,element->id,element->byGoodnessType==1?"伤害":"和平",element->dwTime,myname.c_str(),element->percent,element->dwTime,element->value,element->state);
  }
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"----------------------------------------------");
}

/**
 * \brief  显示角色身上的永久被动技能状态
 */
void SkillStatusManager::showPassiveness()
{
  std::map<DWORD,SkillStatusElement>::iterator tIterator;
  SkillStatusElement *element;

  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"角色身上的永久被动状态列表:");
  for(tIterator = _passivenessElement.begin() ; tIterator !=_passivenessElement.end(); tIterator++)
  {
    std::string myname;
    char buf [45];
    element = &tIterator->second;
    SceneUser *pUser = SceneUserManager::getMe().getUserByTempID(element->dwTempID);
    if (pUser)
      myname = pUser->name;
    else
    {
      sprintf(buf,"临时ID:%u",element->dwTempID);
      myname = buf;
    }
    Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"技能:%u 大类:%u 状态类别:%u %s 剩余时间:%u 攻击者:%s 几率:%u 总时间:%u 状态值%u 特效值:%u",element->dwSkillID,element->byMutexType,element->id,element->byGoodnessType==1?"伤害":"和平",element->dwTime,myname.c_str(),element->percent,element->dwTime,element->value,element->state);
  }
  Channel::sendSys((SceneUser *)entry,Cmd::INFO_TYPE_SYS,"----------------------------------------------");
}
#endif
/**
 * \brief  获得当前角色所携带的技能状态数目
 */
WORD SkillStatusManager::getSaveStatusSize()
{
  WORD ret =0;

  ret = _recoveryElement.size()+_activeElement.size();
  return ret; 
}

/**
 * \brief 将一个技能操作施加在我的身上[SKY技能链第3步]
 * \param carrier 技能操作投送器,里面包含了技能状态
 * \param good 标志这个技能是不是一个增益的技能
 * \return true 为继续下一个操作,false为停止并返回。
 */
bool SkillStatusManager::putOperationToMe(const SkillStatusCarrier &carrier,const bool good)
{
    std::vector<SkillElement>::const_iterator tIterator;
    for (tIterator = carrier.status->_StatusElementList.begin();
	    tIterator != carrier.status->_StatusElementList.end();
	    tIterator++)
    {
	SkillStatusElement element;
	element.byStep  =  ACTION_STEP_START;
	element.id    =  tIterator->id;
	element.percent =  tIterator->percent;
	element.value  =  tIterator->value;
	element.value2  =  tIterator->value2;
	element.state  =  tIterator->state;

	element.dwSkillID = carrier.skillbase->skillid;//carrier.skillID;
	element.halo = tIterator->halo;

	zCard *pAtt = carrier.attacker;
	element.dwThisID = pAtt->data.qwThisID;
	element.dwAttackerID = carrier.attacker->playerID;	    //�ͷż��ܵĽ�ɫID
	switch(runStatusElement(element))
	{
	    case SKILL_RETURN:
		{
		    if (element.state > 0)
		    {
			entry->showCurrentEffect(element.state, true);
		    }
		}
	    default:
		break;
	}

	if(!carrier.status)	//���������Ҫ,��Ȼ��崻�,runStatusElement�����ɾ�����ͷ��ߣ��ٴ�forѭ���͹��� 
	{
	    return true;
	}
    }
#if 0
    if(entry)
    {
	entry->refreshCard(entry->data.qwThisID);
    }
#endif
    return true;
}

