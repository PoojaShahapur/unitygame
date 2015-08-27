using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 特效控制，好像特效基本都一样
     */
    public class EffectControl : BeingControlBase
    {
        protected GameObject m_effectRootGO;        // 特效根节点
        protected LinkEffect m_frameEffect;         // 边框特效，自己手牌，这个就是表示是否可以出牌，自己场牌、英雄卡、技能卡表示是否可以发起攻击，对方场牌、英雄卡、技能卡表示是否可以作为当前攻击卡牌的攻击对象
        protected LinkEffect m_skillAttPrepareEffect;         // 技能攻击准备特效
        protected int m_frameEffectId;             // 边框特效 Id
        protected MList<LinkEffect> m_stateEffectList;
        protected MList<LinkEffect> m_linkEffectList;   // 保存所有添加的连接特效

        public EffectControl(BeingEntity rhv) :
            base(rhv)
        {
            m_frameEffectId = 4;        // 默认是手牌特效 4 
            m_linkEffectList = new MList<LinkEffect>();

            m_stateEffectList = new MList<LinkEffect>((int)StateID.CARD_STATE_MAX);
            int idx = 0;
            for(idx = 0; idx < (int)StateID.CARD_STATE_MAX; ++idx)
            {
                m_stateEffectList.Add(null);
            }
        }

        public override void init()
        {
            base.init();
        }

        public override void dispose()
        {
            if (m_frameEffect != null)
            {
                //m_frameEffect.dispose();
                m_frameEffect = null;
            }

            if (m_skillAttPrepareEffect != null)
            {
                m_skillAttPrepareEffect = null;
            }

            m_stateEffectList.Clear();

            foreach (var effect in m_linkEffectList.list)
            {
                effect.dispose();
            }
            m_linkEffectList.Clear();

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
                UtilApi.SetParent(m_effectRootGO, m_being.gameObject(), false);
            }
        }

        // 添加连接特效，固定不动，连接特效在关联的实体释放的时候，需要释放资源
        public LinkEffect addLinkEffect(int id, bool bAutoRemove = true, bool bLoop = false, bool bPlay = true)
        {
            addFrameSpriteGO();

            LinkEffect effect = Ctx.m_instance.m_sceneEffectMgr.addLinkEffect(id, m_effectRootGO, bAutoRemove, bLoop, bPlay) as LinkEffect;
            m_linkEffectList.Add(effect);
            effect.addEffectPlayEndHandle(onLinkEffectPlayEnd);
            effect.linkedEntity = this.m_being;
            return effect;
        }

        // 添加移动特效
        public MoveEffect addMoveEffect(int id, Vector3 srcPos, Vector3 destPos, float moveTime, bool bAutoRemove = true, bool bLoop = false, bool bPlay = true)
        {
            MoveEffect effect = Ctx.m_instance.m_sceneEffectMgr.addMoveEffect(id, Ctx.m_instance.m_scenePlaceHolder.m_sceneRoot, srcPos, destPos, moveTime, bAutoRemove, bLoop, bPlay) as MoveEffect;

            return effect;
        }

        // 添加边框特效
        protected void addFrameEffect()
        {
            if (m_frameEffect == null)
            {
                m_frameEffect = addLinkEffect(m_frameEffectId, false, true);
            }
        }

        // 添加技能准备特效，主要是法术卡，但是释放的时候基本都是英雄卡上
        public void startSkillAttPrepareEffect(int effectId)
        {
            if (m_skillAttPrepareEffect == null)
            {
                m_skillAttPrepareEffect = addLinkEffect(effectId, false, true);
            }
            else
            {
                m_skillAttPrepareEffect.setTableID(effectId);
                if (m_skillAttPrepareEffect.bPlay())
                {
                    m_skillAttPrepareEffect.stop();                       // 直接停止掉
                }
                m_skillAttPrepareEffect.play();
            }
        }

        public void stopSkillAttPrepareEffect()
        {
            if (m_skillAttPrepareEffect != null)
            {
                m_skillAttPrepareEffect.stop();
            }
        }

        public LinkEffect startStateEffect(StateID stateId, int effectId)
        {
            if(m_stateEffectList[(int)stateId] == null)
            {
                m_stateEffectList[(int)stateId] = addLinkEffect(effectId, false, true);
            }
            else
            {
                m_stateEffectList[(int)stateId].setTableID(effectId);
                if (m_stateEffectList[(int)stateId].bPlay())
                {
                    m_stateEffectList[(int)stateId].stop();                       // 直接停止掉
                }
                m_stateEffectList[(int)stateId].play();
            }

            return m_stateEffectList[(int)stateId];
        }

        public void stopStateEffect(StateID stateId)
        {
            if (m_stateEffectList[(int)stateId] != null)
            {
                m_stateEffectList[(int)stateId].stop();
            }
        }

        // 更新卡牌是否可以出牌，自己手牌
        public void updateCardOutState(bool benable)
        {
            if (benable)
            {
                if (true)
                {
                    if (true)
                    {
                        addFrameEffect();
                        m_frameEffect.play();
                    }
                    else
                    {
                        if (m_frameEffect != null)
                        {
                            m_frameEffect.stop();
                        }
                    }
                }
            }
            else
            {
                if (m_frameEffect != null)
                {
                    m_frameEffect.stop();
                }
            }
        }

        // 更新卡牌是否可以被击，对方场牌、英雄卡、技能卡、自己场牌、英雄卡、技能卡
        virtual public void updateCardAttackedState()
        {
            if (true)
            {
                if (m_frameEffect == null)
                {
                    addFrameEffect();
                }
                else
                {
                    m_frameEffect.play();
                }
            }
        }

        // 开始转换模型 type == 0 是手牌   1 是场牌，自己手牌、场牌转换
        public void startConvModel(int type)
        {
            if (m_effectRootGO != null)         // 如果存在
            {
                UtilApi.removeFromSceneGraph(m_effectRootGO.transform);
            }
        }

        // 结束转换模型，自己手牌、场牌转换
        public void endConvModel(int type)
        {
            if (m_effectRootGO != null)         // 如果存在
            {
                UtilApi.SetParent(m_effectRootGO, m_being.gameObject(), false);
                UtilApi.normalRST(m_effectRootGO.transform);
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
                changeFrameEffectId();
            }
        }

        protected void changeFrameEffectId()
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

        public bool checkRender()
        {
            if (m_frameEffect != null)
            {
                return m_frameEffect.checkRender();
            }

            return true;
        }

        // 连接特效播放结束
        protected void onLinkEffectPlayEnd(IDispatchObject dispObj)
        {
            m_linkEffectList.Remove(dispObj as LinkEffect);
        }
    }
}