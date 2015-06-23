using FSM;
using SDK.Common;
using SDK.Lib;
using System;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 主要处理行为相关的操作，状态、战斗等
     */
    public class BehaviorControl : CardControlBase
    {
        protected Vector3 m_srcPos;                 // 保存最初的位置
        protected SceneStateFSM m_attStateFSM;      // 攻击流程状态机
        protected SceneStateFSM m_hurtStateFSM;     // 被击流程状态机

        public BehaviorControl(SceneCardBase rhv) : 
            base(rhv)
        {
            m_attStateFSM = new SceneStateFSM(m_card);
            m_attStateFSM.Start();

            m_hurtStateFSM = new SceneStateFSM(m_card);
            m_hurtStateFSM.Start();
        }

        public Vector3 srcPos
        {
            get
            {
                return m_srcPos;
            }
            set
            {
                m_srcPos = value;
            }
        }

        // 是否在攻击中，攻击定义是，从开始移动，到返回来，才算是攻击结束
        public bool bInAttack()
        {
            return (m_card.fightData.attackData.curAttackItem != null);
        }

        // 是否在受伤中
        public bool bInHurt()
        {
            return (m_card.fightData.hurtData.hasExecHurtItem());
        }

        // 更新攻击
        public void updateAttack()
        {
            // 攻击需要整个攻击完成才能进入下一次攻击，不能打断
            if(!bInAttack())
            {
                // 如果当前有攻击数据
                if (m_card.fightData.attackData.attackList.Count() > 0)
                {
                    m_attStateFSM.MoveToState(SceneStateId.SSInplace2DestStart);         // 开始攻击
                }
            }
        }

        // 更新受伤
        public void updateHurt()
        {
            // 只要有受伤就需要处理
            // 如果当前有攻击数据
            if (m_card.fightData.hurtData.hasHurtItem())
            {
                m_hurtStateFSM.MoveToState(SceneStateId.SSHurtStart);     // 开始受伤
            }
        }

        public void onMove2DestEnd(NumAniBase ani)
        {
            m_attStateFSM.MoveToState(SceneStateId.SSInplace2Dested);
        }

        public void onMove2InplaceEnd(NumAniBase ani)
        {
            m_attStateFSM.MoveToState(SceneStateId.SSDest2Inplaced);
        }

        // 执行普通攻击
        public void execAttack(ComAttackItem item)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 开始执行普通攻击 execAttack");

            // 播放攻击动作
            // 播放伤害数字
            if (item.damage > 0)
            {
                m_card.effectControl.addLinkEffect(HurtItemBase.DAMAGE_EFFECTID);   // 掉血特效必然播放
                //m_card.playFlyNum((int)item.damage);
            }

            m_card.sceneCardBaseData.m_effectControl.updateAttHurtStateEffect(item);

            // 更新自己的属性显示
            m_card.updateCardDataChangeBySvr(item.svrCard);
        }

        // 执行普通受伤
        public void execHurt(ComHurtItem item)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 执行普通受伤 execHurt");

            LinkEffect effect = null;

            if (item.bDamage)        // 如果受伤
            {
                if (item.hurtEffectId > 0)       // 如果有特效需要播放，并且被击结束以特效为标准
                {
                    Ctx.m_instance.m_logSys.fightLog("[Fight] 执行普通播放受伤特效");

                    effect = m_card.effectControl.addLinkEffect(item.hurtEffectId);     // 被击特效
                    m_card.effectControl.addLinkEffect(HurtItemBase.DAMAGE_EFFECTID);   // 掉血特效必然播放
                    effect.addEffectPlayEndHandle(item.onHurtExecEnd);
                }

                // 播放伤害数字
                m_card.playFlyNum(-item.damage);
            }
            else if (item.bAddHp)       // 回血
            {
                // 仅仅改变属性
                //if (!item.bStateChange())   // 如果没有状态变化，直接结束，如果有，会释放特效结束
                //{
                    m_card.playFlyNum(item.addHp);
                    item.onHurtExecEnd(null);
                //}
            }

            m_card.sceneCardBaseData.m_effectControl.updateAttHurtStateEffect(item);
            
            // 更新自己的属性显示
            m_card.updateCardDataChangeBySvr(item.svrCard);
        }

        // 执行技能攻击
        public void execAttack(SkillAttackItem item)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 开始执行技能攻击 execAttack");
            // 技能攻击开始，需要将技能准备特效移除
            if (m_card.m_sceneDZData.m_sceneDZAreaArr[(int)m_card.sceneCardItem.m_playerSide].centerHero.sceneCardItem.svrCard.qwThisID == item.attThisId)
            {
                m_card.m_sceneDZData.m_sceneDZAreaArr[(int)m_card.sceneCardItem.m_playerSide].centerHero.effectControl.stopSkillAttPrepareEffect();
            }

            if (item.skillTableItem.m_bNeedMove > 0)         // 如果是有攻击目标的技能攻击
            {
                if (item.skillTableItem != null)
                {
                    if (item.skillTableItem.m_skillAttackEffect != 0)
                    {
                        foreach (var thisId in item.hurtIdList.list)
                        {
                            if (thisId == m_card.sceneCardItem.svrCard.qwThisID)         // 如果攻击者还是被击者，就不播放攻击特效了
                            {
                                Ctx.m_instance.m_logSys.fightLog("[Fight] 攻击者 thisId 和被记者 thisId 相同，不播放攻击特效");
                            }
                            else
                            {
                                Ctx.m_instance.m_logSys.fightLog("[Fight] 技能攻击播放攻击特效");

                                SceneCardBase hurtCard = Ctx.m_instance.m_sceneCardMgr.getCardByThisId(thisId);
                                m_card.effectControl.addMoveEffect((int)item.skillTableItem.m_skillAttackEffect, m_card.transform().localPosition, hurtCard.transform().localPosition, item.skillTableItem.m_effectMoveTime);  // 攻击特效
                            }
                        }
                    }
                }
            }
            else    // 没有攻击目标的技能攻击
            {
                if (item.skillTableItem != null)
                {
                    if (item.skillTableItem.m_skillAttackEffect != 0)
                    {
                        Ctx.m_instance.m_logSys.fightLog("[Fight] 技能攻击播放场景特效");

                        Ctx.m_instance.m_sceneEffectMgr.addSceneEffect((int)item.skillTableItem.m_skillAttackEffect, m_card.m_sceneDZData.m_placeHolderGo.m_centerGO);      // 添加一个场景特效
                    }
                }
            }

            m_card.sceneCardBaseData.m_effectControl.updateAttHurtStateEffect(item);
        }

        // 执行技能受伤
        public void execHurt(SkillHurtItem item)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 开始执行技能被击 execHurt");

            LinkEffect effect = null;

            if (item.bDamage)// 检查是否是伤血
            {
                effect = m_card.effectControl.addLinkEffect(HurtItemBase.DAMAGE_EFFECTID);  // 掉血特效必然播放
                effect.addEffectPlayEndHandle(item.onHurtExecEnd);

                // 播放伤害数字
                if (item.damage > 0)
                {
                    m_card.playFlyNum(-item.damage);
                }
            }
            else if (item.bAddHp)       // 回血
            {
                //if (!item.bStateChange())
                //{
                    m_card.playFlyNum(item.addHp);
                    item.onHurtExecEnd(null);       // 直接结束当前技能被击 Item
                //}
            }

            m_card.sceneCardBaseData.m_effectControl.updateAttHurtStateEffect(item);

            // 更新自己的属性显示
            m_card.updateCardDataChangeBySvr(item.svrCard);
        }

        // 执行普通死亡
        public void execHurt(DieItem item)
        {
            LinkEffect effect = null;
            effect = m_card.effectControl.addLinkEffect(item.dieEffectId);  // 死亡特效
            effect.addEffectPlayEndHandle(item.onHurtExecEnd);
        }

        // 直接移动到目标点
        public void moveToDestDirect(Vector3 pos)
        {
            UtilApi.setPos(m_card.transform(), pos);
        }

        // 播放攻击者受伤特效
        public void playAttackHurt(FightItemBase item)
        {
            if (item.damage > 0)
            {
                //m_card.effectControl.addLinkEffect(HurtItemBase.DAMAGE_EFFECTID);   // 掉血特效必然播放
                m_card.playFlyNum(-item.damage);
            }
            if (item.bAddHp)       // 回血
            {
                m_card.playFlyNum(item.addHp);
            }
        }
    }
}