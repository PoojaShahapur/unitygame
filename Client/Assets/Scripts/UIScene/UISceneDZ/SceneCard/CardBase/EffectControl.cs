using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 特效控制
     */
    public class EffectControl : ControlBase
    {
        protected MList<EffectBase> m_effectList;
        protected GameObject m_effectRootGO;        // 特效根节点
        protected LinkEffect m_linkEffect;

        public EffectControl(SceneCardBase rhv) :
            base(rhv)
        {
            m_effectList = new MList<EffectBase>();
        }

        public override void init()
        {
            base.init();
        }

        public override void dispose()
        {
            foreach (EffectBase effect in m_effectList.list)
            {
                effect.dispose();
            }
            m_effectList.Clear();

            m_linkEffect = null;

            base.dispose();
        }

        protected void addFrameSpriteGO()
        {
            if (m_effectRootGO == null)
            {
                m_effectRootGO = UtilApi.createGameObject("FrameSprite");
                UtilApi.SetParent(m_effectRootGO, m_card.gameObject());
            }
        }

        // 特效播放解释回调
        public void onEffectEnd(IDispatchObject dispObj)
        {
            m_effectList.Remove(dispObj as EffectBase);
            (dispObj as EffectBase).dispose();          // 释放资源
        }

        // 添加连接特效，固定不动
        public LinkEffect addLinkEffect(int id, bool bLoop = false, bool bPlay = true)
        {
            addFrameSpriteGO();

            GameObject _go = UtilApi.createSpriteGameObject();
            LinkEffect effect = Ctx.m_instance.m_sceneEffectMgr.createAndAdd(EffectType.eLinkEffect) as LinkEffect;
            m_effectList.Add(effect);

            effect.addEffectPlayEndHandle(onEffectEnd);
            UtilApi.SetParent(_go, m_effectRootGO, false);
            effect.setGameObject(_go);
            effect.setLoop(bLoop);
            effect.setTableID(id);

            if(bPlay)
            {
                effect.play();
            }

            return effect;
        }

        // 添加移动特效
        public void addMoveEffect(int id)
        {
            addFrameSpriteGO();

            GameObject _go = UtilApi.createSpriteGameObject();
            MoveEffect effect = Ctx.m_instance.m_sceneEffectMgr.createAndAdd(EffectType.eMoveEffect) as MoveEffect;
            m_effectList.Add(effect);

            effect.addMoveDestEventHandle(onEffectEnd);
            UtilApi.SetParent(_go, m_effectRootGO, false);
            effect.setGameObject(_go);
            effect.setLoop(false);
            effect.setTableID(id);
            effect.play();
        }

        // 更新卡牌是否可以出牌
        public void updateCardOutState(bool benable)
        {
            if (m_linkEffect == null)
            {
                m_linkEffect = addLinkEffect(4, true);
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
                m_linkEffect = addLinkEffect(4, true);
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