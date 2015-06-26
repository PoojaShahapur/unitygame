using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 能放到场景上的牌，随从牌、法术牌，不包括技能牌、武器牌、英雄牌
     */
    public class CanOutCard : ExceptBlackSceneCard
    {
        protected int m_startIdx;                   // 开始卡牌索引，因为播放动画需要用到索引，索引从 0 开始

        public CanOutCard(SceneDZData data) : 
            base(data)
        {
            
        }

        override public void dispose()
        {
            base.dispose();
        }

        // 设置一些基本信息
        override public void setBaseInfo(EnDZPlayer m_playerSide, CardArea area, CardType cardType)
        {
            ioControl.setCenterPos(m_sceneDZData.m_placeHolderGo.m_cardCenterGOArr[(int)m_playerSide, (int)area].transform.localPosition);
            ioControl.setOutSplitZ(m_sceneDZData.m_placeHolderGo.m_cardCenterGOArr[(int)m_playerSide, (int)area].transform.localPosition.z + 0.65f);
            // 设置初始位置为发牌位置
            m_sceneCardBaseData.m_behaviorControl.moveToDestDirect(m_sceneDZData.m_placeHolderGo.m_cardCenterGOArr[(int)m_playerSide, (int)CardArea.CARDCELLTYPE_NONE].transform.localPosition); // 移动到发牌位置

            // 设置是否可以动画
            if (m_playerSide == EnDZPlayer.ePlayerEnemy)        // 如果是 enemy 的卡牌
            {
                ioControl.disableDrag();
                //if (area == CardArea.CARDCELLTYPE_SKILL || area == CardArea.CARDCELLTYPE_EQUIP)
                //{
                //    trackAniControl.destScale = SceneDZCV.SMALLFACT;
                //}
            }
            // 如果是放在技能或者装备的位置，是不允许拖放的
            else if (area == CardArea.CARDCELLTYPE_SKILL || area == CardArea.CARDCELLTYPE_EQUIP)
            {
                //trackAniControl.destScale = SceneDZCV.SMALLFACT;
                ioControl.disableDrag();
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
            startConvModel(1);

            if (m_render != null)
            {
                m_render.dispose();
                m_render = null;
            }

            m_render = new ChangCardRender(this);
            (m_render as ChangCardRender).setIdAndPnt(this.sceneCardItem.svrCard.dwObjectID, m_sceneDZData.m_placeHolderGo.m_centerGO);
            UtilApi.setScale(m_render.transform(), Vector3.one);

            endConvModel(1);
        }

        // 转换成手牌模型
        override public void convHandleModel()
        {
            startConvModel(0);

            if (m_render != null)
            {
                if (UtilApi.CheckComponent<SpriteRenderer>(m_render.gameObject()))
                {
                    Ctx.m_instance.m_logSys.log("GameObject Has SpriteRenderer");
                }
                m_render.dispose();
                m_render = null;
            }

            m_render = new SelfHandCardRender(this);
            (m_render as SelfHandCardRender).setIdAndPnt(this.sceneCardItem.svrCard.dwObjectID, m_sceneDZData.m_placeHolderGo.m_centerGO);
            updateCardDataChangeBySvr();    // 更新服务器属性

            endConvModel(0);
        }

        protected void startConvModel(int type)
        {
            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    m_sceneCardBaseData.m_effectControl.startConvModel(type);
                }
                if (m_sceneCardBaseData.m_ioControl != null)
                {
                    m_sceneCardBaseData.m_ioControl.startConvModel(type);
                }
                if (m_sceneCardBaseData.m_trackAniControl != null)
                {
                    m_sceneCardBaseData.m_trackAniControl.startConvModel(type);
                }
            }
        }

        protected void endConvModel(int type)
        {
            if (m_sceneCardBaseData != null)
            {
                if (m_sceneCardBaseData.m_effectControl != null)
                {
                    m_sceneCardBaseData.m_effectControl.endConvModel(type);
                }
                if (m_sceneCardBaseData.m_ioControl != null)
                {
                    m_sceneCardBaseData.m_ioControl.endConvModel(type);
                }
                if (m_sceneCardBaseData.m_trackAniControl != null)
                {
                    m_sceneCardBaseData.m_trackAniControl.endConvModel(type);
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

        override public void setStartIdx(int rhv)
        {
            m_startIdx = rhv;
        }

        override public int getStartIdx()
        {
            return m_startIdx;
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

        override public void updateStateEffect()
        {
            sceneCardBaseData.m_effectControl.updateStateEffect();
        }

        override public void loadChaHaoModel(GameObject pntGo_)
        {
            if (chaHaoModel == null)
            {
                chaHaoModel = new AuxDynModel();
            }
            chaHaoModel.pntGo = pntGo_;
            chaHaoModel.modelResPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "ChaHao.prefab");
            chaHaoModel.syncUpdateModel();
        }

        override public void destroyChaHaoModel()
        {
            if (chaHaoModel != null)
            {
                chaHaoModel.dispose();
                chaHaoModel = null;
            }
        }
    }
}