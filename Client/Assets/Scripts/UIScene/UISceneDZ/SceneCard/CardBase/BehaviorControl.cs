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
    public class BehaviorControl : ControlBase
    {
        protected NumAniSequence m_numAniSeq;       // 攻击动画序列，这个所有的都有
        protected Vector3 m_srcPos;                 // 保存最初的位置
        protected SceneStateFSM m_sceneStateFSM;    // 状态的转换以动作结束为标准

        public BehaviorControl(SceneCardBase rhv) : 
            base(rhv)
        {
            m_numAniSeq = new NumAniSequence();
            m_sceneStateFSM = new SceneStateFSM();
            m_sceneStateFSM.card = m_card;
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
            return (!m_sceneStateFSM.equalCurState(SceneStateId.SSInplace) && 
                    m_card.fightData.attackData.curAttackItem != null);
        }

        // 是否在受伤中
        public bool bInHurt()
        {
            return (!m_sceneStateFSM.equalCurState(SceneStateId.SSInplace) &&
                    m_card.fightData.hurtData.curHurtItem != null);
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
            //if (!bInHurt())
            //{
                // 如果当前有攻击数据
                if (m_card.fightData.hurtData.hasEnableItem())
                {
                    m_sceneStateFSM.MoveToState(SceneStateId.SSHurtStart);     // 开始受伤
                }
            //}
        }

        // 播放攻击动画，就是移动过去砸一下
        public void moveToDest(Vector3 srcPos, Vector3 destPos, Action<NumAniBase> handle)
        {
            Vector3 midPt;      // 中间点
            midPt = (srcPos + destPos) / 2;
            midPt.y = 2;

            SimpleCurveAni curveAni = new SimpleCurveAni();
            m_numAniSeq.addOneNumAni(curveAni);
            curveAni.setGO(m_card.gameObject());
            curveAni.setTime(0.3f);
            curveAni.setPlotCount(3);
            curveAni.addPlotPt(0, srcPos);
            curveAni.addPlotPt(1, midPt);
            curveAni.addPlotPt(2, destPos);
            if (handle != null)
            {
                curveAni.setAniEndDisp(handle);
            }

            curveAni.setEaseType(iTween.EaseType.easeInExpo);

            m_numAniSeq.play();
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
            if (item.damage > 0)        // 如果受伤
            {
                if (item.hurtEffectId > 0)       // 如果有特效需要播放，并且被击结束以特效为标准
                {
                    LinkEffect effect = m_card.effectControl.addLinkEffect(item.hurtEffectId);  // 被击特效
                    m_card.effectControl.addLinkEffect(HurtItemBase.DAMAGE_EFFECTID);  // 掉血特效必然播放
                    effect.addEffectPlayEndHandle(item.onHurtExecEnd);
                }
                //else    // 以动作为标准，动作结束就算受伤结束
                //{
                //}

                // 播放伤害数字
                m_card.playFlyNum((int)item.damage);
            }
            else        // 可能是状态改变
            {

            }

            // 更新自己的属性显示
            m_card.updateCardDataChange(item.svrCard);
        }

        // 执行技能攻击
        public void execAttack(SkillAttackItem item)
        {
            TableSkillItemBody skillTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, item.skillId).m_itemBody as TableSkillItemBody;
            if(skillTableItem != null)
            {
                if (skillTableItem.m_skillAttackEffect != 0)
                {
                    m_card.effectControl.addMoveEffect((int)skillTableItem.m_skillAttackEffect);  // 攻击特效
                }
            }
            else // 如果没有配置这个技能，直接结束攻击
            {

            }
        }

        // 执行技能受伤
        public void execHurt(SkillHurtItem item)
        {
            if(item.bDamage)// 检查是否是伤血
            {
                LinkEffect effect = m_card.effectControl.addLinkEffect(HurtItemBase.DAMAGE_EFFECTID);  // 掉血特效必然播放
                effect.addEffectPlayEndHandle(item.onHurtExecEnd);

                // 播放伤害数字
                if (item.damage > 0)
                {
                    m_card.playFlyNum((int)item.damage);
                }
            }
            else        // 如果是回血
            {
                item.onHurtExecEnd(null);       // 直接结束当前技能被击 Item
            }

            // 更新自己的属性显示
            m_card.updateCardDataChange(item.svrCard);
        }
    }
}