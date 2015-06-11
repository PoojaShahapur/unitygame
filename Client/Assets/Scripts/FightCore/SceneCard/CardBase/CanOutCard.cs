using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 能放到场景上的牌，随从牌、法术牌，不包括技能牌、武器牌、英雄牌
     */
    public class CanOutCard : SceneCard
    {
        protected DopeSheetAni m_startAni;          // 开始动画
        protected EventDispatch m_onEnterHandleEntryDisp;

        public CanOutCard(SceneDZData data) : 
            base(data)
        {
            m_onEnterHandleEntryDisp = new AddOnceAndCallOnceEventDispatch();
        }

        // 设置一些基本信息
        override public void setBaseInfo(EnDZPlayer m_playerFlag, CardArea area, CardType cardType)
        {
            dragControl.m_centerPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition;
            // 设置初始位置为发牌位置
            //trackAniControl.startPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_NONE].transform.localPosition;
            //trackAniControl.destPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition;
            m_sceneCardBaseData.m_behaviorControl.moveToDestDirect(m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_NONE].transform.localPosition); // 移动到发牌位置

            // 设置是否可以动画
            if (m_playerFlag == EnDZPlayer.ePlayerEnemy)        // 如果是 enemy 的卡牌
            {
                dragControl.disableDrag();
                if (area == CardArea.CARDCELLTYPE_SKILL || area == CardArea.CARDCELLTYPE_EQUIP)
                {
                    trackAniControl.destScale = SceneCardBase.SMALLFACT;
                }
            }
            // 如果是放在技能或者装备的位置，是不允许拖放的
            else if (area == CardArea.CARDCELLTYPE_SKILL || area == CardArea.CARDCELLTYPE_EQUIP)
            {
                trackAniControl.destScale = SceneCardBase.SMALLFACT;
                dragControl.disableDrag();
            }

            // 更新边框
            if (EnDZPlayer.ePlayerSelf == m_playerFlag)
            {
                if (CardArea.CARDCELLTYPE_HAND == area)
                {
                    if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
                    {
                        updateCardOutState(true);
                    }
                    else
                    {
                        updateCardOutState(false);
                    }
                }
            }
        }

        // 转换成出牌模型
        override public void convOutModel()
        {
            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    m_sceneCardBaseData.m_effectControl.startConvModel();
                }
                if (m_sceneCardBaseData.m_clickControl != null)
                {
                    m_sceneCardBaseData.m_clickControl.startConvModel();
                }
            }

            if (m_render != null)
            {
                m_render.dispose();
                m_render = null;
            }

            m_render = new OutCardRender(this);
            (m_render as OutCardRender).setIdAndPnt(this.sceneCardItem.svrCard.dwObjectID, m_sceneDZData.m_centerGO);
            UtilApi.setScale(m_render.transform(), Vector3.one);

            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    m_sceneCardBaseData.m_effectControl.endConvModel();
                }
                if (m_sceneCardBaseData.m_clickControl != null)
                {
                    m_sceneCardBaseData.m_clickControl.endConvModel();
                }
            }

            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    if (!m_sceneCardBaseData.m_effectControl.checkRender())
                    {
                        Ctx.m_instance.m_logSys.log("Render Is Null");
                    }
                }
            }
        }

        // 转换成手牌模型
        override public void convHandleModel()
        {
            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    m_sceneCardBaseData.m_effectControl.startConvModel();
                }
                if (m_sceneCardBaseData.m_clickControl != null)
                {
                    m_sceneCardBaseData.m_clickControl.startConvModel();
                }
            }

            if (m_render != null)
            {
                if (UtilApi.CheckComponent<SpriteRenderer>(m_render.gameObject()))
                {
                    Ctx.m_instance.m_logSys.log("GameObject Has SpriteRenderer");
                }
                m_render.dispose();
                m_render = null;
            }

            m_render = new SceneCardPlayerRender(this);
            (m_render as SceneCardPlayerRender).setIdAndPnt(this.sceneCardItem.svrCard.dwObjectID, m_sceneDZData.m_centerGO);

            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    m_sceneCardBaseData.m_effectControl.endConvModel();
                }
                if (m_sceneCardBaseData.m_clickControl != null)
                {
                    m_sceneCardBaseData.m_clickControl.endConvModel();
                }
            }

            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    if (!m_sceneCardBaseData.m_effectControl.checkRender())
                    {
                        Ctx.m_instance.m_logSys.log("Render Is Null");
                    }
                }
            }
        }

        // 更新卡牌属性，这个主要更改卡牌经常改变的属性
        public override void updateCardDataChange(t_Card svrCard_ = null)
        {
            base.updateCardDataChange(svrCard_);

            if (svrCard_ == null)
            {
                svrCard_ = m_sceneCardItem.svrCard;
            }

            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_HAND)     // 手牌不同更新
                {
                    AuxLabel text = new AuxLabel();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/AttText");       // 攻击
                    text.text = svrCard_.damage.ToString();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/MpText");         // Magic
                    text.text = svrCard_.mpcost.ToString();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/HpText");       // HP
                    text.text = svrCard_.hp.ToString();
                }
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_COMMON)        // 场牌更新
                {

                }
            }
        }

        // 这个主要是更新卡牌不经常改变的属性
        public override void updateCardDataNoChange()
        {
            base.updateCardDataNoChange();

            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_HAND)
                {
                    UtilApi.updateCardDataNoChange(m_sceneCardItem.m_cardTableItem, m_render.gameObject());
                }
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_COMMON)
                {

                }
            }
        }

        // 根据表更新卡牌数据，这个主要是用于初始卡牌更新，只用于随从牌、法术牌，并且 Render 数据是手牌的数据
        public override void updateCardDataByTable()
        {
            base.updateCardDataByTable();

            TableItemBase tableBase = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, m_sceneCardBaseData.m_startCardID);
            if (tableBase != null)
            {
                TableCardItemBody cardTableData = tableBase.m_itemBody as TableCardItemBody;
                UtilApi.updateCardDataNoChange(cardTableData, m_render.gameObject());
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("卡表查找失败， ID = {0}", m_sceneCardBaseData.m_startCardID));
            }
        }

        protected void createAni()
        {
            if (m_startAni == null)
            {
                m_startAni = new DopeSheetAni();
                string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneAnimatorController], "CardModel.asset");
                m_startAni.setAniEndDisp(onFaPai2MinAniEnd);
                m_startAni.setControlInfo(path);
                m_startAni.setGO(this.gameObject());
                m_startAni.syncUpdateControl();
            }
        }

        // 开始动画，发牌区域到场景中间
        override public void faPai2MinAni()
        {
            createAni();

            m_startAni.stateId = 1;
            m_startAni.play();
        }

        // 开始卡牌动画播放结束，注意开始有 3 张或者 4 张卡牌做动画，只有一个有回调函数
        protected void onFaPai2MinAniEnd(NumAniBase ani)
        {
            //m_startAni.stateId = 0;
            //m_startAni.play();
            //m_startAni.stop();

            //m_sceneCardBaseData.m_behaviorControl.moveToDestDirect(trackAniControl.destPos);    // 移动到终点位置
        }

        // 开始动画，场景中间到手牌区域动画
        override public void min2HandleAni()
        {
            m_startAni.setAniEndDisp(onMin2HandleEnd);
            m_startAni.stateId = 2;
            m_startAni.play();
        }

        protected void onMin2HandleEnd(NumAniBase ani)
        {
            //m_startAni.stateId = 0;
            //m_startAni.play();
            m_onEnterHandleEntryDisp.dispatchEvent(this);
        }

        // 开始区域到手牌区域
        override public void start2HandleAni()
        {
            createAni();

            m_startAni.setAniEndDisp(onStart2HandleAni);
            m_startAni.stateId = 4;
            m_startAni.play();
        }

        protected void onStart2HandleAni(NumAniBase ani)
        {
            m_onEnterHandleEntryDisp.dispatchEvent(this);
        }

        override public void addEnterHandleEntryDisp(System.Action<IDispatchObject> eventHandle)
        {
            m_onEnterHandleEntryDisp.addEventHandle(eventHandle);
        }
    }
}