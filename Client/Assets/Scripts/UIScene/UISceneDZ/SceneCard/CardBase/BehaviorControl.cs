using SDK.Common;
using System;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 主要处理行为相关的操作
     */
    public class BehaviorControl : ControlBase
    {
        protected CardSceneState m_cardPreSceneState;
        protected CardSceneState m_cardSceneState;
        protected NumAniSequence m_numAniSeq;       // 攻击动画序列，这个所有的都有
        protected Vector3 m_srcPos;                 // 保存最初的位置

        public BehaviorControl(SceneCardBase rhv) : 
            base(rhv)
        {
            m_cardPreSceneState = CardSceneState.eInplace;         // 默认原地状态
            m_cardSceneState = CardSceneState.eInplace;         // 默认原地状态
            m_numAniSeq = new NumAniSequence();
        }

        public CardSceneState cardSceneState
        {
            get
            {
                return m_cardSceneState;
            }
            set
            {
                m_cardPreSceneState = m_cardSceneState;
                m_cardSceneState = value;
            }
        }

        public void setInplace()
        {
            cardSceneState = CardSceneState.eInplace;
        }

        public void setInplace2DestStart()
        {
            cardSceneState = CardSceneState.eInplace2DestStart;
            m_srcPos = m_card.transform.localPosition;
            // 获取一项攻击数值
            m_card.fightData.attackData.getFirstItem();
        }

        public void setInplace2Desting()
        {
            cardSceneState = CardSceneState.eInplace2Desting;
            SceneCardBase hurtCard = Ctx.m_instance.m_sceneCardMgr.getCard(m_card.fightData.attackData.curAttackItem.getHurterId());
            playAttackAni(m_srcPos, hurtCard.transform.localPosition, onMove2DestEnd);
        }

        public void setInplace2Dested()
        {
            cardSceneState = CardSceneState.eInplace2Dested;
            // 如果没有延时，直接进入下个状态
            setAttackStart();
        }

        public void setAttackStart()
        {
            cardSceneState = CardSceneState.eAttackStart;
            m_card.fightData.attackData.execCurItem();
            setAttacking();
        }

        public void setAttacking()
        {
            cardSceneState = CardSceneState.eAttacking;
            setAttacked();
        }

        public void setAttacked()
        {
            cardSceneState = CardSceneState.eAttacked;
            setDest2InplaceStart();
        }

        public void setDest2InplaceStart()
        {
            cardSceneState = CardSceneState.eDest2InplaceStart;
            setDest2Inplaceing();
        }

        public void setDest2Inplaceing()
        {
            cardSceneState = CardSceneState.eDest2Inplaceing;
            playAttackAni(m_card.transform.localPosition, m_srcPos, onMove2InplaceEnd);
        }

        public void setDest2InplaceEnd()
        {
            cardSceneState = CardSceneState.eDest2Inplaced;
            setInplace();
        }

        public void setHurtStart()
        {
            cardSceneState = CardSceneState.eHurtStart;
            setHurting();
        }

        public void setHurting()
        {
            cardSceneState = CardSceneState.eHurting;
            // 播放受伤动画和特效
        }

        public void setHurted()
        {
            cardSceneState = CardSceneState.eHurted;
            setInplace();
        }

        // 播放攻击动画，就是移动过去砸一下
        public void playAttackAni(Vector3 srcPos, Vector3 destPos, Action<NumAniBase> handle)
        {
            Vector3 midPt;      // 中间点
            midPt = (srcPos + destPos) / 2;
            midPt.y = 2;

            SimpleCurveAni curveAni = new SimpleCurveAni();
            m_numAniSeq.addOneNumAni(curveAni);
            curveAni.setGO(m_card.gameObject);
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

        protected void onMove2DestEnd(NumAniBase ani)
        {
            setInplace2Dested();
        }

        protected void onMove2InplaceEnd(NumAniBase ani)
        {
            setDest2InplaceEnd();
        }
    }
}