﻿using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 特效控制
     */
    public class EffectControl : CardControlBase
    {
        protected GameObject m_effectRootGO;        // 特效根节点
        protected LinkEffect m_frameEffect;         // 边框特效
        protected int m_frameEffectId;             // 边框特效 Id

        public EffectControl(SceneCardBase rhv) :
            base(rhv)
        {
            m_frameEffectId = 4;        // 默认是手牌特效 4 
        }

        public override void init()
        {
            base.init();
        }

        public override void dispose()
        {
            if (m_frameEffect != null)
            {
                m_frameEffect.dispose();
                m_frameEffect = null;
            }

            base.dispose();
        }

        public LinkEffect frameEffect
        {
            get
            {
                return m_frameEffect;
            }
            set
            {
                m_frameEffect = value;
            }
        }

        protected void addFrameSpriteGO()
        {
            if (m_effectRootGO == null)
            {
                m_effectRootGO = UtilApi.createGameObject("FrameSprite");
                UtilApi.SetParent(m_effectRootGO, m_card.gameObject(), false);
            }
        }

        // 添加连接特效，固定不动
        public LinkEffect addLinkEffect(int id, bool bAutoRemove = true, bool bLoop = false, bool bPlay = true)
        {
            addFrameSpriteGO();

            LinkEffect effect = Ctx.m_instance.m_sceneEffectMgr.addLinkEffect(id, m_effectRootGO, bAutoRemove, bLoop, bPlay) as LinkEffect;

            return effect;
        }

        // 添加移动特效
        public MoveEffect addMoveEffect(int id, Vector3 srcPos, Vector3 destPos, float moveTime, bool bAutoRemove = true, bool bLoop = false, bool bPlay = true)
        {
            MoveEffect effect = Ctx.m_instance.m_sceneEffectMgr.addMoveEffect(id, m_card.m_sceneDZData.m_centerGO, srcPos, destPos, moveTime, bAutoRemove, bLoop, bPlay) as MoveEffect;

            return effect;
        }

        // 更新卡牌是否可以出牌
        public void updateCardOutState(bool benable)
        {
            addFrameEffect();

            if (benable)
            {
                if (m_card.sceneCardItem != null)
                {
                    if (m_card.sceneCardItem.svrCard.mpcost <= Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_card.sceneCardItem.m_playerFlag].m_heroMagicPoint.mp)
                    {
                        m_frameEffect.play();
                    }
                    else
                    {
                        m_frameEffect.stop();
                    }
                }
            }
            else
            {
                m_frameEffect.stop();
            }
        }

        // 更新卡牌是否可以被击
        public void updateCardAttackedState(bool benable)
        {
            addFrameEffect();

            if (benable)
            {
                if (m_card.sceneCardItem != null)
                {
                    m_frameEffect.play();
                }
            }
            else
            {
                m_frameEffect.stop();
            }
        }

        protected void addFrameEffect()
        {
            if (m_frameEffect == null)
            {
                m_frameEffect = addLinkEffect(m_frameEffectId, false, true);
            }
        }

        // 开始转换模型 type == 0 是手牌   1 是场牌
        public void startConvModel(int type)
        {
            if (m_effectRootGO != null)         // 如果存在
            {
                UtilApi.removeFromSceneGraph(m_effectRootGO.transform);
            }
        }

        // 结束转换模型
        public void endConvModel(int type)
        {
            if (m_effectRootGO != null)         // 如果存在
            {
                UtilApi.SetParent(m_effectRootGO, m_card.gameObject());
            }

            if (0 == type)
            {
                m_frameEffectId = 4;
            }
            else
            {
                m_frameEffectId = 1;
            }

            if (m_frameEffect != null)
            {
                m_frameEffect.setTableID(m_frameEffectId);
                if (m_frameEffect.bPlay())
                {
                    m_frameEffect.stop();                       // 直接停止掉
                    m_frameEffect.play();
                }
                else
                {
                    m_frameEffect.stop();
                }
            }
        }

        public bool checkRender()
        {
            if (m_frameEffect != null)
            {
                return m_frameEffect.checkRender();
            }

            return true;
        }
    }
}