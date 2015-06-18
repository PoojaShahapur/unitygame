﻿using BehaviorLibrary;
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
        public SceneDZData m_sceneDZData;

        protected SceneCardItem m_sceneCardItem;        // 敌人手里卡牌和白色卡牌是没有这个字段的，其余都有
        protected SceneCardBaseData m_sceneCardBaseData;

        public SceneCardBase(SceneDZData data)
        {
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

        public ClickControl clickControl
        {
            get
            {
                return m_sceneCardBaseData.m_clickControl;
            }
            set
            {
                m_sceneCardBaseData.m_clickControl = value;
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

        public DragControl dragControl
        {
            get
            {
                return m_sceneCardBaseData.m_dragControl;
            }
            set
            {
                m_sceneCardBaseData.m_dragControl = value;
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
        virtual public void setBaseInfo(EnDZPlayer m_playerFlag, CardArea area, CardType cardType)
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
                m_sceneDZData.m_sceneDZAreaArr[(int)sceneCardItem.m_playerFlag].removeOneCard(this);
            }
        }

        virtual protected void disposeBaseData()
        {
            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_clickControl != null)
                {
                    m_sceneCardBaseData.m_clickControl.dispose();
                }
                if (m_sceneCardBaseData.m_trackAniControl != null)
                {
                    m_sceneCardBaseData.m_trackAniControl.dispose();
                }
                if (m_sceneCardBaseData.m_dragControl != null)
                {
                    m_sceneCardBaseData.m_dragControl.dispose();
                }
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    m_sceneCardBaseData.m_effectControl.dispose();
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
            m_sceneCardBaseData.m_effectControl.updateCardOutState(benable);
        }

        // 更新卡牌是否可以被击
        public void updateCardAttackedState(bool benable)
        {
            m_sceneCardBaseData.m_effectControl.updateCardAttackedState(benable);
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

        // 开始卡牌动画
        virtual public void faPai2MinAni()
        {

        }

        virtual public void min2HandleAni()
        {

        }

        virtual public void start2HandleAni()
        {

        }

        virtual public void addEnterHandleEntryDisp(System.Action<IDispatchObject> eventHandle)
        {
            
        }

        virtual public void setStartIdx(int rhv)
        {

        }

        virtual public void startEnemyFaPaiAni()
        {

        }

        // 更新初始卡牌场景位置信息
        virtual public void updateInitCardSceneInfo(Transform trans)
        {

        }

        virtual public void updateOutCardScaleInfo(Transform trans)
        {

        }
    }
}