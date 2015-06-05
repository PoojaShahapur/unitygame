using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 特效控制
     */
    public class EffectControl : CardControlBase
    {
        protected GameObject m_effectRootGO;        // 特效根节点
        protected LinkEffect m_linkEffect;

        public EffectControl(SceneCardBase rhv) :
            base(rhv)
        {
            
        }

        public override void init()
        {
            base.init();
        }

        public override void dispose()
        {
            m_linkEffect = null;

            base.dispose();
        }

        public LinkEffect linkEffect
        {
            get
            {
                return m_linkEffect;
            }
            set
            {
                m_linkEffect = value;
            }
        }

        protected void addFrameSpriteGO()
        {
            if (m_effectRootGO == null)
            {
                m_effectRootGO = UtilApi.createGameObject("FrameSprite");
                UtilApi.SetParent(m_effectRootGO, m_card.gameObject());
                UtilApi.adjustEffectRST(m_effectRootGO.transform);
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
            if (m_linkEffect == null)
            {
                m_linkEffect = addLinkEffect(4, false, true);
            }

            if (benable)
            {
                if (m_card.sceneCardItem != null)
                {
                    if (m_card.sceneCardItem.svrCard.mpcost <= Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_card.sceneCardItem.m_playerFlag].m_heroMagicPoint.mp)
                    {
                        m_linkEffect.play();
                    }
                    else
                    {
                        m_linkEffect.stop();
                    }
                }
            }
            else
            {
                m_linkEffect.stop();
            }
        }

        // 更新卡牌是否可以被击
        public void updateCardAttackedState(bool benable)
        {
            if (m_linkEffect == null)
            {
                m_linkEffect = addLinkEffect(4, false, true);
            }

            if (benable)
            {
                if (m_card.sceneCardItem != null)
                {
                    m_linkEffect.play();
                }
            }
            else
            {
                m_linkEffect.stop();
            }
        }
    }
}