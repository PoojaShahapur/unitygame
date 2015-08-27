using FSM;
using Game.Msg;
using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主要处理行为相关的操作，状态、战斗等
     */
    public class BeingBehaviorControl : BeingControlBase
    {
        protected Vector3 m_srcPos;                 // 保存最初的位置
        protected FightSeqData m_fightSeqData;      // 战斗数据

        public BeingBehaviorControl(BeingEntity rhv) : 
            base(rhv)
        {

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

        public FightSeqData fightSeqData
        {
            get
            {
                return m_fightSeqData;
            }
        }

        // 是否在攻击中，攻击定义是，从开始移动，到返回来，才算是攻击结束
        public bool bInAttack()
        {
            return (m_being.fightData.attackData.curAttackItem != null);
        }

        // 是否在受伤中
        public bool bInHurt()
        {
            return (m_being.fightData.hurtData.hasExecHurtItem());
        }

        // 更新攻击
        public void updateAttack()
        {
            // 攻击需要整个攻击完成才能进入下一次攻击，不能打断
            if(!bInAttack())
            {
                // 如果当前有攻击数据
                if (m_being.fightData.attackData.attackList.Count() > 0)
                {
                    
                }
            }
        }

        // 更新受伤
        public void updateHurt()
        {
            // 只要有受伤就需要处理
            // 如果当前有攻击数据
            if (m_being.fightData.hurtData.hasHurtItem())
            {
                
            }
        }

        // 执行普通攻击
        public void execAttack(ImmeComAttackItem item)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 开始执行普通攻击 execAttack");

            // 播放攻击动作
            // 播放伤害数字
            if (item.damage > 0)
            {
                m_being.effectControl.addLinkEffect(ImmeHurtItemBase.DAMAGE_EFFECTID);   // 掉血特效必然播放
                //m_being.playFlyNum((int)item.damage);
            }
        }

        // 执行普通受伤
        public void execHurt(ImmeComHurtItem item)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 执行普通受伤 execHurt");

            LinkEffect effect = null;

            if (item.bDamage)        // 如果受伤
            {
                if (item.hurtEffectId > 0)       // 如果有特效需要播放，并且被击结束以特效为标准
                {
                    Ctx.m_instance.m_logSys.fightLog("[Fight] 执行普通播放受伤特效");

                    effect = m_being.effectControl.addLinkEffect(item.hurtEffectId);     // 被击特效
                    m_being.effectControl.addLinkEffect(ImmeHurtItemBase.DAMAGE_EFFECTID);   // 掉血特效必然播放
                    effect.addEffectPlayEndHandle(item.onHurtExecEnd);
                }

                // 播放伤害数字
                m_being.playFlyNum(-item.damage);
            }
            else if (item.bAddHp)       // 回血
            {
                // 仅仅改变属性
                //if (!item.bStateChange())   // 如果没有状态变化，直接结束，如果有，会释放特效结束
                //{
                    m_being.playFlyNum(item.addHp);
                    item.onHurtExecEnd(null);
                //}
            }
            else    // 不伤血也不加血，直接结束
            {
                item.onHurtExecEnd(null);
            }
        }

        // 执行技能攻击
        public void execAttack(ImmeSkillAttackItem item)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 开始执行技能攻击 execAttack");

            if (item.skillTableItem.m_bNeedMove > 0)         // 如果是有攻击目标的技能攻击
            {
                if (item.skillTableItem != null)
                {
                    if (item.skillTableItem.m_skillAttackEffect != 0)
                    {
                        foreach (var thisId in item.hurtIdList.list)
                        {
                            if (thisId == m_being.qwThisID)         // 如果攻击者还是被击者，就不播放攻击特效了
                            {
                                Ctx.m_instance.m_logSys.fightLog("[Fight] 攻击者 thisId 和被记者 thisId 相同，不播放攻击特效");
                            }
                            else
                            {
                                Ctx.m_instance.m_logSys.fightLog("[Fight] 技能攻击播放攻击特效");

                                Player _player = Ctx.m_instance.m_playerMgr.getPlayerByThisId(thisId);
                                m_being.effectControl.addMoveEffect((int)item.skillTableItem.m_skillAttackEffect, m_being.transform().localPosition, _player.transform().localPosition, item.skillTableItem.m_effectMoveTime);  // 攻击特效
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

                        Ctx.m_instance.m_sceneEffectMgr.addSceneEffect((int)item.skillTableItem.m_skillAttackEffect, Ctx.m_instance.m_scenePlaceHolder.m_sceneRoot);      // 添加一个场景特效
                    }
                }
            }
        }

        // 执行技能受伤
        public void execHurt(ImmeSkillHurtItem item)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 开始执行技能被击 execHurt");

            LinkEffect effect = null;

            if (item.bDamage)// 检查是否是伤血
            {
                effect = m_being.effectControl.addLinkEffect(ImmeHurtItemBase.DAMAGE_EFFECTID);  // 掉血特效必然播放
                effect.addEffectPlayEndHandle(item.onHurtExecEnd);
            }
        }

        // 执行普通死亡
        public void execHurt(ImmeDieItem item)
        {
            LinkEffect effect = null;
            effect = m_being.effectControl.addLinkEffect(item.dieEffectId);  // 死亡特效
            effect.addEffectPlayEndHandle(item.onHurtExecEnd);
        }

        // 直接移动到目标点
        public void moveToDestDirect(Vector3 pos)
        {
            UtilApi.setPos(m_being.transform(), pos);
        }

        // 播放攻击者受伤特效
        public void playAttackHurt(ImmeFightItemBase item)
        {
            if (item.damage > 0)
            {
                //m_being.effectControl.addLinkEffect(HurtItemBase.DAMAGE_EFFECTID);   // 掉血特效必然播放
                m_being.playFlyNum(-item.damage);
            }
            if (item.bAddHp)       // 回血
            {
                m_being.playFlyNum(item.addHp);
            }
        }
    }
}