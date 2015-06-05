using FSM;
using SDK.Common;
using SDK.Lib;
using System;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 主要处理行为相关的操作
     */
    public class BehaviorControl : CardControlBase
    {
        protected Vector3 m_srcPos;                 // 保存最初的位置
        protected SceneStateFSM m_sceneStateFSM;    // 状态的转换以动作结束为标准

        public BehaviorControl(SceneCardBase rhv) : 
            base(rhv)
        {
            m_sceneStateFSM = new SceneStateFSM(m_card);
            m_sceneStateFSM.Start();
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
                    m_sceneStateFSM.MoveToState(SceneStateId.SSInplace2DestStart);         // 开始攻击
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
                m_sceneStateFSM.MoveToState(SceneStateId.SSHurtStart);     // 开始受伤
            }
        }

        public void onMove2DestEnd(NumAniBase ani)
        {
            m_sceneStateFSM.MoveToState(SceneStateId.SSInplace2Dested);
        }

        public void onMove2InplaceEnd(NumAniBase ani)
        {
            m_sceneStateFSM.MoveToState(SceneStateId.SSDest2Inplaced);
        }

        //// 受伤动画和特效播放完成
        //protected void onHurtEnd(IDispatchObject dispObj)
        //{
        //    m_sceneStateFSM.MoveToState(SceneStateId.SSHurted);
        //}

        // 执行普通攻击
        public void execAttack(ComAttackItem item)
        {
            // 播放攻击动作
            // 播放伤害数字
            if (item.damage > 0)
            {
                m_card.playFlyNum((int)item.damage);
            }

            // 更新自己的属性显示
            m_card.updateCardDataChange(item.svrCard);
        }

        // 执行普通受伤
        public void execHurt(ComHurtItem item)
        {
            LinkEffect effect = null;
            bool bAddEffect = false;

            if (item.bDamage)        // 如果受伤
            {
                if (item.hurtEffectId > 0)       // 如果有特效需要播放，并且被击结束以特效为标准
                {
                    bAddEffect = true;
                    effect = m_card.effectControl.addLinkEffect(item.hurtEffectId);  // 被击特效
                    m_card.effectControl.addLinkEffect(HurtItemBase.DAMAGE_EFFECTID);  // 掉血特效必然播放
                    effect.addEffectPlayEndHandle(item.onHurtExecEnd);
                }
                //else    // 以动作为标准，动作结束就算受伤结束
                //{
                //}

                // 播放伤害数字
                m_card.playFlyNum((int)item.damage);
            }
            else if (item.bAddHp)       // 回血
            {
                // 仅仅改变属性
                item.onHurtExecEnd(null);
            }
            if (item.bStateChange())       // 每一个状态对应一个特效，需要播放特效
            {
                
                int idx = 0;
                for(idx = 0; idx < (int)StateID.CARD_STATE_MAX; ++idx)
                {
                    if(UtilMath.checkState((StateID)idx, item.state))   // 如果这个状态改变
                    {
                        effect = m_card.effectControl.addLinkEffect(item.getStateEffect((StateID)idx));

                        if(!bAddEffect)
                        {
                            bAddEffect = true;    
                            effect.addEffectPlayEndHandle(item.onHurtExecEnd);
                        }
                    }
                }
            }
            
            // 更新自己的属性显示
            m_card.updateCardDataChange(item.svrCard);
        }

        // 执行技能攻击
        public void execAttack(SkillAttackItem item)
        {
            if(item.skillTableItem != null)
            {
                if (item.skillTableItem.m_skillAttackEffect != 0)
                {
                    foreach(var thisId in item.hurtIdList.list)
                    {
                        SceneCardBase hurtCard = Ctx.m_instance.m_sceneCardMgr.getCard(thisId);
                        m_card.effectControl.addMoveEffect((int)item.skillTableItem.m_skillAttackEffect, m_card.transform().localPosition, hurtCard.transform().localPosition, item.skillTableItem.m_effectMoveTime);  // 攻击特效
                    }
                }
            }
            else // 如果没有配置这个技能，直接结束攻击
            {

            }
        }

        // 执行技能受伤
        public void execHurt(SkillHurtItem item)
        {
            LinkEffect effect = null;
            bool bAddEffect = false;

            if (item.bDamage)// 检查是否是伤血
            {
                effect = m_card.effectControl.addLinkEffect(HurtItemBase.DAMAGE_EFFECTID);  // 掉血特效必然播放
                effect.addEffectPlayEndHandle(item.onHurtExecEnd);

                // 播放伤害数字
                if (item.damage > 0)
                {
                    m_card.playFlyNum((int)item.damage);
                }
            }
            else if (item.bAddHp)       // 回血
            {
                item.onHurtExecEnd(null);       // 直接结束当前技能被击 Item
            }
            if (item.bStateChange())       // 每一个状态对应一个特效，需要播放特效
            {

                int idx = 0;
                for (idx = 0; idx < (int)StateID.CARD_STATE_MAX; ++idx)
                {
                    if (UtilMath.checkState((StateID)idx, item.state))   // 如果这个状态改变
                    {
                        effect = m_card.effectControl.addLinkEffect(item.getStateEffect((StateID)idx));

                        if (!bAddEffect)
                        {
                            bAddEffect = true;
                            effect.addEffectPlayEndHandle(item.onHurtExecEnd);
                        }
                    }
                }
            }

            // 更新自己的属性显示
            m_card.updateCardDataChange(item.svrCard);
        }
    }
}