using BehaviorLibrary;
using FSM;
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 英雄卡、技能卡、装备卡、随从卡、法术卡基类，但是不是对方手里的背面卡的基类
     */
    public class SceneCard : SceneCardBase
    {
        public SceneCard(SceneDZData data) : 
            base(data)
        {
            m_sceneCardBaseData = new SceneCardBaseData();
            m_sceneCardBaseData.m_fightData = new FightData();
            m_sceneCardBaseData.m_animFSM = new AnimFSM();
            m_sceneCardBaseData.m_animFSM.card = this;
            m_sceneCardBaseData.m_animFSM.Start();

            m_sceneCardBaseData.m_moveControl = new CardMoveControl(this);

            m_sceneCardBaseData.m_aiController = new AIController();
            m_sceneCardBaseData.m_aiController.btID = BTID.e1000;
            m_sceneCardBaseData.m_aiController.possess(this);
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);
            //m_sceneCardBaseData.m_animFSM.Update();                 // 更新状态机
            if (m_sceneCardBaseData.m_fightData != null)
            {
                m_sceneCardBaseData.m_fightData.onTime(delta);          // 更新战斗数据
            }
            if(m_sceneCardBaseData.m_aiController != null)
            {
                m_sceneCardBaseData.m_aiController.onTick(delta);
            }
        }

        // 设置一些基本信息
        override public void setBaseInfo(EnDZPlayer m_playerFlag, CardArea area, CardType cardType)
        {
            dragControl.m_centerPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition;
            // 设置初始位置为发牌位置
            trackAniControl.startPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_NONE].transform.localPosition;
            trackAniControl.destPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition;

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
            if(m_render != null)
            {
                m_render.dispose();
                m_render = null;
            }

            m_render = new OutCardRender(this);
            (m_render as OutCardRender).setIdAndPnt(this.sceneCardItem.svrCard.dwObjectID, m_sceneDZData.m_centerGO);
        }

        // 转换成手牌模型
        override public void convHandleModel()
        {
            if (m_render != null)
            {
                m_render.dispose();
                m_render = null;
            }

            m_render = new SceneCardPlayerRender(this);
            (m_render as SceneCardPlayerRender).setIdAndPnt(this.sceneCardItem.svrCard.dwObjectID, m_sceneDZData.m_centerGO);
        }
    }
}