using BehaviorLibrary;
using FSM;
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 场景中卡牌基类
     */
    public class SceneCardBase : SceneEntity
    {
        public static Vector3 SMALLFACT = new Vector3(0.5f, 0.5f, 0.5f);    // 小牌时的缩放因子
        public static Vector3 BIGFACT = new Vector3(1.2f, 1.2f, 1.2f);      // 大牌时候的因子
        public const uint WHITECARDID = 1000;

        protected SceneCardItem m_sceneCardItem;
        public SceneDZData m_sceneDZData;

        protected ushort m_curIndex = 0;// 当前索引，因为可能初始卡牌的时候 m_sceneCardItem 
        protected ushort m_preIndex = 0;// 在牌中的索引，主要是手里的牌和打出去的牌，这个是客户端设置的索引，服务器的索引在 t_Card 类型里面

        protected GameObject m_chaHaoGo;
        protected uint m_startCardID;

        protected FightData m_fightData;            // 战斗数据
        protected AnimFSM m_animFSM;                // 动画状态机

        protected AIController m_aiController;
        protected BehaviorControl m_behaviorControl;
        protected ClickControl m_clickControl;
        protected AniControl m_aniControl;
        protected DragControl m_dragControl;
        protected EffectControl m_effectControl;

        public SceneCardBase(SceneDZData data)
        {
            m_sceneDZData = data;
            m_fightData = new FightData();
            m_animFSM = new AnimFSM();
            m_animFSM.card = this;
            m_animFSM.Start();

            m_aiController = new AIController();
            m_aiController.possess(this);
        }

        virtual public void init()
        {
            if (m_clickControl != null)
            {
                m_clickControl.init();
            }
            if (m_aniControl != null)
            {
                m_aniControl.init();
            }
            if (m_dragControl != null)
            {
                m_dragControl.init();
            }
            if (m_effectControl != null)
            {
                m_effectControl.init();
            }
        }

        public SceneCardItem sceneCardItem
        {
            get
            {
                return m_sceneCardItem;
            }
            set
            {
                m_sceneCardItem = value;

                if (m_sceneCardItem != null)
                {
                    updateCardDataChange();
                    updateCardDataNoChange();
                }
                else
                {
                    Ctx.m_instance.m_logSys.log("服务器卡牌数据为空");
                }
            }
        }

        public GameObject chaHaoGo
        {
            get
            {
                return m_chaHaoGo;
            }
            set
            {
                m_chaHaoGo = value;
            }
        }

        public ushort curIndex
        {
            get
            {
                if (m_sceneCardItem != null)
                {
                    return m_sceneCardItem.svrCard.pos.y;
                }
                else
                {
                    return m_curIndex;
                }
            }
            set
            {
                if (m_sceneCardItem != null)
                {
                    m_preIndex = m_sceneCardItem.svrCard.pos.y;
                    m_sceneCardItem.svrCard.pos.y = value;
                }
                else
                {
                    m_preIndex = m_curIndex;
                    m_curIndex = value;
                }
            }
        }

        public ushort preIndex
        {
            get
            {
                return m_preIndex;
            }
        }

        public FightData fightData
        {
            get
            {
                return m_fightData;
            }
        }

        public AIController aiController
        {
            get
            {
                return m_aiController;
            }
            set
            {
                m_aiController = value;
            }
        }

        public BehaviorControl behaviorControl
        {
            get
            {
                return m_behaviorControl;
            }
            set
            {
                m_behaviorControl = value;
            }
        }

        public ClickControl clickControl
        {
            get
            {
                return m_clickControl;
            }
            set
            {
                m_clickControl = value;
            }
        }

        public AniControl aniControl
        {
            get
            {
                return m_aniControl;
            }
            set
            {
                m_aniControl = value;
            }
        }

        public DragControl dragControl
        {
            get
            {
                return m_dragControl;
            }
            set
            {
                m_dragControl = value;
            }
        }
        public EffectControl effectControl
        {
            get
            {
                return m_effectControl;
            }
            set
            {
                m_effectControl = value;
            }
        }

        public EventDispatch clickEntityDisp
        {
            get
            {
                return (m_render as CardRenderBase).clickEntityDisp;
            }
        }

        public uint startCardID
        {
            get
            {
                return m_startCardID;
            }
            set
            {
                m_startCardID = value;
            }
        }

        override public void onTick(float delta)
        {
            m_animFSM.Update();                 // 更新状态机
            m_fightData.onTime(delta);          // 更新战斗数据
        }

        override public void dispose()
        {
            m_clickControl.dispose();
            m_aniControl.dispose();
            m_dragControl.dispose();
            m_effectControl.dispose();

            UtilApi.Destroy(m_render.gameObject());
            m_sceneCardItem = null;
        }

        // 更新卡牌属性，这个主要更改卡牌经常改变的属性
        public virtual void updateCardDataChange()
        {
            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_COMMON || m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_HAND)
                {
                    AuxLabel text = new AuxLabel();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/AttText");       // 攻击
                    text.text = m_sceneCardItem.svrCard.damage.ToString();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/MpText");         // Magic
                    text.text = m_sceneCardItem.svrCard.mpcost.ToString();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/HpText");       // HP
                    text.text = m_sceneCardItem.svrCard.hp.ToString();
                }
            }
        }

        // 这个主要是更新卡牌不经常改变的属性
        public virtual void updateCardDataNoChange()
        {
            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.cardArea != CardArea.CARDCELLTYPE_HERO)
                {
                    UtilApi.updateCardDataNoChange(m_sceneCardItem.m_cardTableItem, m_render.gameObject());
                }
            }
        }

        // 根据表更新卡牌数据，这个主要是用于初始卡牌更新
        public void updateCardDataByTable()
        {
            TableItemBase tableBase = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, m_startCardID);
            if(tableBase != null)
            {
                TableCardItemBody cardTableData = tableBase.m_itemBody as TableCardItemBody;
                UtilApi.updateCardDataNoChange(cardTableData, m_render.gameObject());
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("卡表查找失败， ID = {0}", m_startCardID));
            }
        }

        //// 关闭拖放功能
        //public virtual void disableDrag()
        //{

        //}

        //// 开启拖动
        //public virtual void  enableDrag()
        //{

        //}

        // 进入普通攻击状态
        public void enterAttack()
        {
            if (this.m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_COMMON)
            {
                // 只有点击自己的时候，才启动攻击
                if (m_sceneCardItem.m_playerFlag == EnDZPlayer.ePlayerSelf)
                {
                    m_sceneDZData.m_gameOpState.enterAttackOp(EnGameOp.eOpNormalAttack, this);
                }
            }
        }

        // 更新卡牌是否可以出牌
        public void updateCardOutState(bool benable)
        {
            m_effectControl.updateCardOutState(benable);
        }

        // 更新卡牌是否可以被击
        public void updateCardAttackedState(bool benable)
        {
            m_effectControl.updateCardAttackedState(benable);
        }

        public void playFlyNum(int num)
        {
            Ctx.m_instance.m_pFlyNumMgr.addFlyNum(num, m_render.transform().localPosition, m_sceneDZData.m_centerGO);
        }

        // 是否是客户端先从手牌区域移动到出牌区域，然后再发动攻击的卡牌
        public bool canClientMove2OutArea()
        {
            if ((m_sceneCardItem.m_cardTableItem.m_type == (int)CardType.CARDTYPE_MAGIC && m_sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget > 0) || (m_sceneCardItem.m_cardTableItem.m_zhanHou > 0 && m_sceneCardItem.m_cardTableItem.m_bNeedZhanHouTarget > 0))
            {
                return true;
            }

            return false;
        }

        virtual public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            (m_render as CardPlayerRender).setIdAndPnt(objId, pntGo_);
        }
    }
}