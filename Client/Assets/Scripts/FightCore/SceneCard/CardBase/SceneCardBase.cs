using BehaviorLibrary;
using FSM;
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 场景中卡牌基类
     */
    public class SceneCardBase : SceneEntity
    {
        protected static int ID_ALLOC_IDX = 0;      // 分配 ID 索引的
        public SceneDZData m_sceneDZData;
        protected int m_ClientId;                   // 客户端自己的唯一 ID
        protected SceneCardItem m_sceneCardItem;        // 敌人手里卡牌和白色卡牌是没有这个字段的，其余都有
        protected SceneCardBaseData m_sceneCardBaseData;

        public SceneCardBase(SceneDZData data)
        {
            m_ClientId = ID_ALLOC_IDX;
            ++ID_ALLOC_IDX;
            m_sceneDZData = data;
        }

        override public void init()
        {

        }

        public SceneCardBaseData sceneCardBaseData
        {
            get
            {
                return m_sceneCardBaseData;
            }
            set
            {
                m_sceneCardBaseData = value;
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
                    updateCardDataChangeBySvr();
                    updateCardDataNoChangeByTable();
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
                return m_sceneCardBaseData.m_chaHaoGo;
            }
            set
            {
                m_sceneCardBaseData.m_chaHaoGo = value;
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
                    return m_sceneCardBaseData.m_curIndex;
                }
            }
            set
            {
                if (m_sceneCardItem != null)
                {
                    m_sceneCardBaseData.m_preIndex = m_sceneCardItem.svrCard.pos.y;
                    m_sceneCardItem.svrCard.pos.y = value;
                }
                else
                {
                    m_sceneCardBaseData.m_preIndex = m_sceneCardBaseData.m_curIndex;
                    m_sceneCardBaseData.m_curIndex = value;
                }
            }
        }

        public ushort preIndex
        {
            get
            {
                return m_sceneCardBaseData.m_preIndex;
            }
        }

        public FightData fightData
        {
            get
            {
                return m_sceneCardBaseData.m_fightData;
            }
        }

        public AIController aiController
        {
            get
            {
                return m_sceneCardBaseData.m_aiController;
            }
            set
            {
                m_sceneCardBaseData.m_aiController = value;
            }
        }

        public BehaviorControl behaviorControl
        {
            get
            {
                return m_sceneCardBaseData.m_behaviorControl;
            }
            set
            {
                m_sceneCardBaseData.m_behaviorControl = value;
            }
        }

        public TrackAniControl trackAniControl
        {
            get
            {
                return m_sceneCardBaseData.m_trackAniControl;
            }
            set
            {
                m_sceneCardBaseData.m_trackAniControl = value;
            }
        }

        public IOControlBase ioControl
        {
            get
            {
                return m_sceneCardBaseData.m_ioControl;
            }
            set
            {
                m_sceneCardBaseData.m_ioControl = value;
            }
        }
        public EffectControl effectControl
        {
            get
            {
                return m_sceneCardBaseData.m_effectControl;
            }
            set
            {
                m_sceneCardBaseData.m_effectControl = value;
            }
        }

        public CardMoveControl moveControl
        {
            get
            {
                return m_sceneCardBaseData.m_moveControl;
            }
        }

        public EventDispatch clickEntityDisp
        {
            get
            {
                return (m_render as CardRenderBase).clickEntityDisp;
            }
        }

        public EventDispatch downEntityDisp
        {
            get
            {
                return (m_render as CardRenderBase).downEntityDisp;
            }
        }

        public EventDispatch upEntityDisp
        {
            get
            {
                return (m_render as CardRenderBase).upEntityDisp;
            }
        }

        public EventDispatch dragOverEntityDisp
        {
            get
            {
                return (m_render as CardRenderBase).dragOverEntityDisp;
            }
        }

        public EventDispatch dragOutEntityDisp
        {
            get
            {
                return (m_render as CardRenderBase).dragOutEntityDisp;
            }
        }

        public uint startCardID
        {
            get
            {
                return m_sceneCardBaseData.m_startCardID;
            }
            set
            {
                m_sceneCardBaseData.m_startCardID = value;
            }
        }

        // 这个主要方便可以从卡牌 ID 直接创建卡牌，因为可能有的时候直接动卡牌 ID 创建卡牌，服务器的数据还没有
        virtual public void setBaseInfo(EnDZPlayer m_playerSide, CardArea area, CardType cardType)
        {

        }

        override public void onTick(float delta)
        {

        }

        virtual public bool getSvrDispose()
        {
            return false;
        }

        virtual public void setSvrDispose(bool rhv = true)
        {

        }

        override public void dispose()
        {
            // 从管理器中删除
            Ctx.m_instance.m_sceneCardMgr.delObject(this);
            removeRef();
            disposeBaseData();
            m_render.dispose();
            m_sceneCardItem = null;
        }
        
        // 移除所有引用当前对象的地方
        virtual protected void removeRef()
        {
            if (sceneCardItem != null)
            {
                Ctx.m_instance.m_logSys.log(string.Format("客户端彻底删除卡牌 thisId = {0}", sceneCardItem.svrCard.qwThisID));
                // 从各种引用除删除
                m_sceneDZData.m_sceneDZAreaArr[(int)sceneCardItem.playerSide].removeOneCard(this);
            }
        }

        virtual protected void disposeBaseData()
        {
            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_trackAniControl != null)
                {
                    m_sceneCardBaseData.m_trackAniControl.dispose();
                }
                if (m_sceneCardBaseData.m_ioControl != null)
                {
                    m_sceneCardBaseData.m_ioControl.dispose();
                }
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    m_sceneCardBaseData.m_effectControl.dispose();
                }
                if(m_sceneCardBaseData.m_aiController != null)
                {
                    m_sceneCardBaseData.m_aiController.dispose();
                }
                if (m_sceneCardBaseData.m_moveControl != null)
                {
                    m_sceneCardBaseData.m_moveControl.dispose();
                }
                if (m_sceneCardBaseData.m_behaviorControl != null)
                {
                    m_sceneCardBaseData.m_behaviorControl.dispose();
                }
            }
        }

        // 更新卡牌属性，这个主要更改卡牌经常改变的属性，除了初始卡牌，后来服务器发送过来的卡牌数据都要从这个刷新
        public virtual void updateCardDataChangeBySvr(t_Card svrCard_ = null)
        {

        }

        // 这个主要是更新卡牌不经常改变的属性
        public virtual void updateCardDataNoChangeByTable()
        {
            
        }

        // 根据表更新卡牌数据，这个主要是用于初始卡牌更新
        public virtual void updateCardDataByTable()
        {
            
        }

        // 更新卡牌是否可以出牌
        public void updateCardOutState(bool benable)
        {
            m_sceneCardBaseData.m_effectControl.updateCardOutState(benable);
        }

        // 更新卡牌是否可以被击
        public void updateCardAttackedState(bool benable)
        {
            m_sceneCardBaseData.m_effectControl.updateCardAttackedState(benable);
        }

        public void playFlyNum(int num)
        {
            Ctx.m_instance.m_pFlyNumMgr.addFlyNum(num, m_render.transform().localPosition + new Vector3(0, 1.2f, 0), m_sceneDZData.m_placeHolderGo.m_centerGO);
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
            //(m_render as CardPlayerRender).setIdAndPnt(objId, pntGo_);
        }

        // 转换成出牌模型
        virtual public void convOutModel()
        {

        }

        // 转换成手牌模型
        virtual public void convHandleModel()
        {

        }

        // 是否在战斗中
        virtual public bool bInFight()
        {
            return false;
        }

        virtual public bool canDelFormClient()
        {
            return false;
        }

        virtual public void setStartIdx(int rhv)
        {

        }

        virtual public int getStartIdx()
        {
            return 0;
        }

        // 更新初始卡牌场景位置信息
        virtual public void updateInitCardSceneInfo(Transform trans)
        {

        }

        virtual public void updateOutCardScaleInfo(Transform trans)
        {

        }

        public virtual string getDesc()
        {
            return "";
        }

        protected string getSideStr()
        {
            string side = "";

            if (EnDZPlayer.ePlayerSelf == sceneCardItem.playerSide)
            {
                side = "Self";
            }
            else
            {
                side = "Enemy";
            }

            return side;
        }

        protected string getAreaStr()
        {
            string area = "";

            if (CardArea.CARDCELLTYPE_COMMON == sceneCardItem.cardArea)
            {
                area = "Common";
            }
            else if (CardArea.CARDCELLTYPE_HAND == sceneCardItem.cardArea)
            {
                area = "Hand";
            }
            else if (CardArea.CARDCELLTYPE_EQUIP == sceneCardItem.cardArea)
            {
                area = "Equip";
            }
            else if (CardArea.CARDCELLTYPE_SKILL == sceneCardItem.cardArea)
            {
                area = "Skill";
            }
            else if (CardArea.CARDCELLTYPE_HERO == sceneCardItem.cardArea)
            {
                area = "Hero";
            }

            return area;
        }

        protected int getPos()
        {
            return sceneCardItem.svrCard.pos.y;
        }

        protected uint getThisId()
        {
            return sceneCardItem.svrCard.qwThisID;
        }

        virtual public void updateStateEffect()
        {

        }

        virtual public void clearAttTimes()
        {

        }
    }
}