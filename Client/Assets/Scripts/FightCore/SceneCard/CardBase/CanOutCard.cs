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
        protected int m_startIdx;                   // 开始卡牌索引，因为播放动画需要用到索引，索引从 0 开始
        protected DopeSheetAni m_startAni;          // 开始动画
        protected EventDispatch m_onEnterHandleEntryDisp;

        public CanOutCard(SceneDZData data) : 
            base(data)
        {
            m_onEnterHandleEntryDisp = new AddOnceAndCallOnceEventDispatch();
        }

        override public void dispose()
        {
            if (m_startAni != null)         // 如果对手的场牌是没有这个动画的，因为对手的手牌和场牌是不同的对象，场牌是出到场中才生成的，这个时候没有动画
            {
                m_startAni.dispose();
                m_startAni = null;
            }
            base.dispose();
        }

        // 设置一些基本信息
        override public void setBaseInfo(EnDZPlayer m_playerSide, CardArea area, CardType cardType)
        {
            dragControl.m_centerPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerSide, (int)area].transform.localPosition;
            // 设置初始位置为发牌位置
            m_sceneCardBaseData.m_behaviorControl.moveToDestDirect(m_sceneDZData.m_cardCenterGOArr[(int)m_playerSide, (int)CardArea.CARDCELLTYPE_NONE].transform.localPosition); // 移动到发牌位置

            // 设置是否可以动画
            if (m_playerSide == EnDZPlayer.ePlayerEnemy)        // 如果是 enemy 的卡牌
            {
                dragControl.disableDrag();
                //if (area == CardArea.CARDCELLTYPE_SKILL || area == CardArea.CARDCELLTYPE_EQUIP)
                //{
                //    trackAniControl.destScale = SceneDZCV.SMALLFACT;
                //}
            }
            // 如果是放在技能或者装备的位置，是不允许拖放的
            else if (area == CardArea.CARDCELLTYPE_SKILL || area == CardArea.CARDCELLTYPE_EQUIP)
            {
                //trackAniControl.destScale = SceneDZCV.SMALLFACT;
                dragControl.disableDrag();
            }

            // 更新边框
            if (EnDZPlayer.ePlayerSelf == m_playerSide)
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
                    m_sceneCardBaseData.m_effectControl.startConvModel(1);
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

            // 动画设置
            if (m_startAni != null)
            {
                m_startAni.setGO(this.gameObject());
                m_startAni.syncUpdateControl();
            }

            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    m_sceneCardBaseData.m_effectControl.endConvModel(1);
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
                    m_sceneCardBaseData.m_effectControl.startConvModel(0);
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
            updateCardDataChangeBySvr();    // 更新服务器属性

            // 动画设置
            if (m_startAni != null)
            {
                m_startAni.setGO(this.gameObject());
                m_startAni.syncUpdateControl();
            }

            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    m_sceneCardBaseData.m_effectControl.endConvModel(0);
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
        public override void updateCardDataChangeBySvr(t_Card svrCard_ = null)
        {
            base.updateCardDataChangeBySvr(svrCard_);

            if (svrCard_ == null)
            {
                svrCard_ = m_sceneCardItem.svrCard;
            }

            AuxLabel text = new AuxLabel();
            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_HAND)     // 手牌不同更新
                {
                    text.setSelfGo(m_render.gameObject(), "UIRoot/AttText");        // 攻击
                    text.text = svrCard_.damage.ToString();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/MpText");         // Magic
                    text.text = svrCard_.mpcost.ToString();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/HpText");         // HP
                    text.text = svrCard_.hp.ToString();
                }
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_COMMON)        // 场牌更新
                {
                    text.setSelfGo(m_render.gameObject(), "UIRoot/AttText");        // 攻击
                    text.text = svrCard_.damage.ToString();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/HpText");         // HP
                    text.text = svrCard_.hp.ToString();
                }
            }
        }

        // 这个主要是更新卡牌不经常改变的属性
        public override void updateCardDataNoChangeByTable()
        {
            base.updateCardDataNoChangeByTable();

            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_HAND)
                {
                    UtilLogic.updateCardDataNoChangeByTable(m_sceneCardItem.m_cardTableItem, m_render.gameObject());
                }
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_COMMON)   // 场牌区没有不变属性显示
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
                UtilLogic.updateCardDataNoChangeByTable(cardTableData, m_render.gameObject());
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
                string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneAnimatorController], "SelfCardAni.asset");
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

            m_startAni.stateId = convIdx2StateId(0);
            m_startAni.play();
        }

        // 开始卡牌动画播放结束，注意开始有 3 张或者 4 张卡牌做动画，只有一个有回调函数
        protected void onFaPai2MinAniEnd(NumAniBase ani)
        {
            //m_startAni.stateId = 0;
            //m_startAni.play();
            //m_startAni.stop();

            //m_sceneCardBaseData.m_behaviorControl.moveToDestDirect(trackAniControl.destPos);    // 移动到终点位置
            Ctx.m_instance.m_logSys.log("自己卡牌从发牌区到场景区域动画结束");
        }

        // 开始动画，场景中间到手牌区域动画
        override public void min2HandleAni()
        {
            m_startAni.setAniEndDisp(onMin2HandleEnd);
            m_startAni.stateId = convIdx2StateId(1);
            m_startAni.play();
        }

        protected void onMin2HandleEnd(NumAniBase ani)
        {
            //m_startAni.stateId = 0;
            //m_startAni.play();
            m_onEnterHandleEntryDisp.dispatchEvent(this);
            Ctx.m_instance.m_logSys.log("自己卡牌从场景区域到手牌区域动画结束");
        }

        // 发牌区域到手牌区域
        override public void start2HandleAni()
        {
            createAni();

            m_startAni.setAniEndDisp(onStart2HandleAni);
            m_startAni.stateId = convIdx2StateId(3);
            m_startAni.play();
        }

        protected void onStart2HandleAni(NumAniBase ani)
        {
            Ctx.m_instance.m_logSys.log("自己卡牌从发牌区域到手牌区域动画结束");
            m_onEnterHandleEntryDisp.dispatchEvent(this);
        }

        override public void addEnterHandleEntryDisp(System.Action<IDispatchObject> eventHandle)
        {
            m_onEnterHandleEntryDisp.addEventHandle(eventHandle);
        }

        override public void setStartIdx(int rhv)
        {
            m_startIdx = rhv;
        }

        protected int convIdx2StateId(int type)
        {
            if(0 == type)       // 获取发牌到场牌中心动画 Id
            {
                return m_startIdx + 1;
            }
            else if(1 == type)  // 场牌到手牌动画 Id
            {
                return (m_startIdx + 1 + 10);
            }
            else if(2 == type) // 不要的牌，回退发牌区
            {
                return (m_startIdx + 1 + 20);
            }
            else if(3 == type)  // 直接从发牌区域到手牌区域
            {
                return 31;
            }

            return 1;
        }

        // 更新初始卡牌场景位置信息
        override public void updateInitCardSceneInfo(Transform trans)
        {
            UtilApi.setPos(this.transform(), trans.localPosition);
            UtilApi.setScale(this.transform(), trans.localScale);
            UtilApi.setRot(this.transform(), trans.localRotation);
        }

        // 更新场牌区域卡牌缩放信息
        override public void updateOutCardScaleInfo(Transform trans)
        {
            m_sceneCardBaseData.m_trackAniControl.updateOutCardScaleInfo(trans);
            UtilApi.setScale(this.transform(), trans.localScale);
        }
    }
}