using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 场景对战区域
     */
    public class SceneDZArea
    {
        public EnDZPlayer m_playerFlag;                 // 指示玩家的位置
        public SceneDZData m_sceneDZData;
        public float m_internal = 1.171349f;            // 卡牌间隔
        public float m_zOff = -3.0f;                    // 卡牌 Z 值偏移
        
        public List<SceneDragCard> m_outSceneCardList = new List<SceneDragCard>(); // 已经出的牌，在场景中心
        public List<SceneDragCard> m_inSceneCardList = new List<SceneDragCard>();   // 场景可拖放的卡牌列表，最底下的，还没有出的牌
        public hero m_hero = new hero();                                            // 主角自己的 hero 
        public SceneDragCard m_sceneSkillCard = new SceneDragCard();                // skill
        public SceneDragCard m_sceneEquipCard = new SceneDragCard();                // equip

        // 对战开始显示的卡牌
        public void addInitCard()
        {
            int idx = 0;
            while(idx < 4)
            {
                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx] > 0)
                {
                    SceneDragCard cardItem = new SceneDragCard();
                    m_inSceneCardList.Add(cardItem);
                    cardItem.setGameObject(UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getSceneCardModel(EnSceneCardType.eScene_minion).getObject()) as GameObject);
                    cardItem.getGameObject().transform.parent = m_sceneDZData.m_centerGO.transform;
                    cardItem.getGameObject().transform.Translate(idx * m_internal, 0, 0);
                    //go.transform.Rotate(-90f, -90f, 0);
                }

                ++idx;
            }
        }

        public void startDZ()
        {
            int idx = 0;
            Transform child;
            while(idx < m_sceneDZData.m_centerGO.transform.childCount)
            {
                child = m_sceneDZData.m_centerGO.transform.GetChild(idx);
                child.Translate(0, 0, m_zOff);

                ++idx;
            }
        }

        public void addOneCommonCard(uint objid)
        {
            SceneDragCard cardItem = new SceneDragCard();
            m_inSceneCardList.Add(cardItem);
            cardItem.setGameObject(UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getSceneCardModel(EnSceneCardType.eScene_minion).getObject()) as GameObject);
            cardItem.getGameObject().transform.parent = m_sceneDZData.m_centerGO.transform;

            cardItem.getGameObject().transform.Translate(m_inSceneCardList.Count - 1 * m_internal, 0, m_zOff);
        }

        public void addOneSkillCard(uint objid)
        {
            m_sceneSkillCard.setGameObject(UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getSceneCardModel(EnSceneCardType.eScene_minion).getObject()) as GameObject);
            m_sceneSkillCard.getGameObject().transform.parent = m_sceneDZData.m_centerGO.transform;

            m_sceneSkillCard.getGameObject().transform.Translate(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_sceneCardList.Count - 1 * m_internal, 0, m_zOff);
        }

        public void addOneEquipCard(uint objid)
        {
            m_sceneEquipCard.setGameObject(UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getSceneCardModel(EnSceneCardType.eScene_minion).getObject()) as GameObject);
            m_sceneEquipCard.getGameObject().transform.parent = m_sceneDZData.m_centerGO.transform;

            m_sceneEquipCard.getGameObject().transform.Translate(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_sceneCardList.Count - 1 * m_internal, 0, m_zOff);
        }

        public void addOneOutCard(uint objid)
        {
            SceneDragCard cardItem = new SceneDragCard();
            m_outSceneCardList.Add(cardItem);
            cardItem.setGameObject(UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getSceneCardModel(EnSceneCardType.eScene_minion).getObject()) as GameObject);
            cardItem.getGameObject().transform.parent = m_sceneDZData.m_centerGO.transform;

            cardItem.getGameObject().transform.Translate(m_outSceneCardList.Count - 1 * m_internal, 0, m_zOff);
        }

        // 计算最后一张卡牌的位置和旋转
        protected void calcLastCardPosAndRotate()
        {

        }

        public void psstAddBattleCardPropertyUserCmd(stAddBattleCardPropertyUserCmd msg, SceneCardItem sceneItem)
        {
            if((int)CardArea.CARDCELLTYPE_HERO == msg.slot)     // 如果是 hero ，hero 自己已经创建显示了
            {
                m_hero.setCardData(sceneItem);
            }
            else if((int)CardArea.CARDCELLTYPE_SKILL == msg.slot)
            {
                addOneSkillCard(msg.mobject.dwObjectID);
                m_sceneSkillCard.setCardData(sceneItem);
            }
            else if ((int)CardArea.CARDCELLTYPE_EQUIP == msg.slot)
            {
                addOneEquipCard(msg.mobject.dwObjectID);
                m_sceneEquipCard.setCardData(sceneItem);
            }
            else if ((int)CardArea.CARDCELLTYPE_COMMON == msg.slot)
            {
                addOneOutCard(msg.mobject.dwObjectID);
            }
            else if ((int)CardArea.CARDCELLTYPE_HAND == msg.slot)
            {
                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_recStartCardNum >= Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].getStartCardNum())        // 判断接收的数据是否是 startCardList 列表中的数据
                {
                    addOneCommonCard(msg.mobject.dwObjectID);
                    m_inSceneCardList[m_inSceneCardList.Count - 1].setCardData(sceneItem);
                }
                else
                {
                    m_inSceneCardList[Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_recStartCardNum].setCardData(sceneItem);
                    ++Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_recStartCardNum;
                }
            }
        }
    }
}