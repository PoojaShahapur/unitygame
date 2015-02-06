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
    public class SceneDZArea : ISceneDZArea
    {
        public SceneDZData m_sceneDZData;
        public EnDZPlayer m_playerFlag;                 // 指示玩家的位置

        public OutSceneCardList m_outSceneCardList; // 已经出的牌，在场景中心
        public InSceneCardList m_inSceneCardList;   // 场景可拖放的卡牌列表，最底下的，还没有出的牌
        public hero m_hero = new hero();                                            // 主角自己的 hero 
        public SceneDragCard m_sceneSkillCard;                // skill
        public SceneDragCard m_sceneEquipCard;                // equip

        public SceneDZArea(SceneDZData sceneDZData, EnDZPlayer playerFlag)
        {
            m_sceneDZData = sceneDZData;
            m_playerFlag = playerFlag;

            m_outSceneCardList = new OutSceneCardList(m_sceneDZData, m_playerFlag);
        }

        // 添加卡牌不包括 CardArea.CARDCELLTYPE_COMMON 区域， enemy 对方出牌也是这个消息
        public void psstAddBattleCardPropertyUserCmd(stAddBattleCardPropertyUserCmd msg, SceneCardItem sceneItem)
        {
            if (msg.byActionType == 1)
            {
                if ((int)CardArea.CARDCELLTYPE_HERO == msg.slot)     // 如果是 hero ，hero 自己已经创建显示了
                {
                    m_hero.sceneCardItem = sceneItem;      // 这个动画已经有了
                }
                else if ((int)CardArea.CARDCELLTYPE_SKILL == msg.slot)
                {
                    m_sceneSkillCard = m_sceneDZData.createOneCard(msg.mobject.dwObjectID, m_playerFlag, (CardArea)msg.slot);
                    m_sceneSkillCard.sceneCardItem = sceneItem;
                    m_sceneSkillCard.moveToDest();
                }
                else if ((int)CardArea.CARDCELLTYPE_HAND == msg.slot)
                {
                    if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_recStartCardNum >= Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].getStartCardNum())        // 判断接收的数据是否是 startCardList 列表中的数据
                    {
                        m_inSceneCardList.addSceneCard(msg.mobject.dwObjectID, sceneItem);
                    }
                    else
                    {
                        m_inSceneCardList.setCardDataByIdx(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_recStartCardNum, sceneItem);
                        ++Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_recStartCardNum;
                    }
                }
                else if ((int)CardArea.CARDCELLTYPE_COMMON == msg.slot)      // 只有对方出牌的时候才会走这里
                {
                    SceneDragCard srcCard = m_sceneDZData.createOneCard(msg.mobject.dwObjectID, m_playerFlag, (CardArea)msg.slot);
                    srcCard.sceneCardItem = sceneItem;
                    m_outSceneCardList.addCard(srcCard);
                    m_outSceneCardList.updateSceneCardPos();
                }
            }
            else        // 更新卡牌数据
            {
                if ((int)CardArea.CARDCELLTYPE_HERO == msg.slot)     // 如果是 hero ，hero 自己已经创建显示了
                {
                    m_hero.updateCardDataChange();      // 这个动画已经有了
                }
                else if ((int)CardArea.CARDCELLTYPE_SKILL == msg.slot)
                {
                    m_sceneSkillCard.updateCardDataChange();
                }
                else if ((int)CardArea.CARDCELLTYPE_EQUIP == msg.slot)
                {
                    m_sceneEquipCard.updateCardDataChange();
                }
                else if ((int)CardArea.CARDCELLTYPE_HAND == msg.slot)
                {
                    m_inSceneCardList.updateCardData(sceneItem);
                }
                else if ((int)CardArea.CARDCELLTYPE_COMMON == msg.slot)      // 只有对方出牌的时候才会走这里
                {
                    m_outSceneCardList.updateCardData(sceneItem);
                }
            }
        }

        // 移动卡牌，从一个位置到另外一个位置，CardArea.CARDCELLTYPE_COMMON 区域增加是从这个消息过来的，目前只处理移动到 CardArea.CARDCELLTYPE_COMMON 区域
        public void changeSceneCard(stRetMoveGameCardUserCmd msg)
        {
            SceneDragCard srcCard = null;

            // 移动手里的牌的位置
            srcCard = m_inSceneCardList.getSceneCardByThisID(msg.qwThisID);
            if (srcCard != null)
            {
                m_inSceneCardList.removeCard(srcCard);
            }

            m_inSceneCardList.updateSceneCardPos();

            // 更新移动后的牌的位置
            if ((int)CardArea.CARDCELLTYPE_EQUIP == msg.m_sceneCardItem.m_svrCard.pos.dwLocation)        // 如果出的是装备
            {
                m_sceneEquipCard = srcCard;
                m_sceneEquipCard.destPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_EQUIP].transform.localPosition;
                m_sceneEquipCard.moveToDest();
            }
            else        // 出的是随从
            {
                m_outSceneCardList.removeWhiteCard();
                m_outSceneCardList.addCard(srcCard, msg.dst.y);
                m_outSceneCardList.updateSceneCardPos();
            }
        }

        // 将当前拖放的对象移动会原始位置
        public void moveDragBack()
        {
            if(m_sceneDZData.m_curDragItem != null)
            {
                m_sceneDZData.m_curDragItem.moveToDest();
            }
        }

        // test 移动卡牌
        public void moveCard()
        {
            m_outSceneCardList.removeWhiteCard();
            m_inSceneCardList.removeCard(m_sceneDZData.m_curDragItem);
            m_outSceneCardList.addCard(m_sceneDZData.m_curDragItem, m_sceneDZData.m_curWhiteIdx);
            m_inSceneCardList.updateSceneCardPos();
            m_outSceneCardList.updateSceneCardPos();
        }

        public void psstAddEnemyHandCardPropertyUserCmd()
        {
            m_inSceneCardList.addSceneCard(uint.MaxValue, null);
        }

        public void delOneCard(SceneCardItem sceneItem)
        {
            if ((int)CardArea.CARDCELLTYPE_SKILL == sceneItem.m_svrCard.pos.dwLocation)
            {
                m_sceneSkillCard.destroy();
                m_sceneSkillCard = null;
            }
            else if ((int)CardArea.CARDCELLTYPE_EQUIP == sceneItem.m_svrCard.pos.dwLocation)
            {
                m_sceneEquipCard.destroy();
                m_sceneEquipCard = null;
            }
            else if ((int)CardArea.CARDCELLTYPE_COMMON == sceneItem.m_svrCard.pos.dwLocation)
            {
                m_outSceneCardList.removeCard(sceneItem);
            }
        }

        public void updateMp()
        {
            // 更新 MP 数据显示
            m_sceneDZData.m_textArr[(int)m_playerFlag].text = string.Format("{0}/{1}", Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.mp, Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.maxmp);

            int idx = 0;
            // 更新 MP 模型显示
            if(m_sceneDZData.m_mpGridArr[(int)m_playerFlag].transform.childCount < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.maxmp)  // 如果 maxmp 多了
            {
                idx = m_sceneDZData.m_mpGridArr[(int)m_playerFlag].transform.childCount;
                while(idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.maxmp)
                {
                    m_sceneDZData.m_mpGridArr[(int)m_playerFlag].AddChild((UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getcostModel().getObject()) as GameObject).transform);
                    ++idx;
                }
            }

            GameObject go = null;

            // 更新哪些是可以使用的 mp
            idx = 0;
            while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.maxmp - Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.mp)
            {
                go = UtilApi.TransFindChildByPObjAndPath(m_sceneDZData.m_mpGridArr[(int)m_playerFlag].GetChild(idx).gameObject, "light");
                go.SetActive(false);

                ++idx;
            }

            while(idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.maxmp)
            {
                go = UtilApi.TransFindChildByPObjAndPath(m_sceneDZData.m_mpGridArr[(int)m_playerFlag].GetChild(idx).gameObject, "light");
                go.SetActive(true);

                ++idx;
            }

            // 继续更新可用的 Mp 也可能是锁住的
            if(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.forbid > 0)
            {
                // 显示一把锁
                idx = 0;
                int endidx = (int)(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.maxmp - 1);
                while(idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.forbid)
                {
                    go = UtilApi.TransFindChildByPObjAndPath(m_sceneDZData.m_mpGridArr[(int)m_playerFlag].GetChild(endidx - idx).gameObject, "light");

                    // 改成一把锁

                    ++idx;
                }
            }
        }

        public SceneCardEntityBase getUnderSceneCard(GameObject underGo)
        {
            SceneCardEntityBase cardBase;
            cardBase = m_outSceneCardList.getUnderSceneCard(underGo);
            if(cardBase != null)
            {
                return cardBase;
            }
            cardBase = m_inSceneCardList.getUnderSceneCard(underGo);
            if(cardBase != null)
            {
                return cardBase;
            }

            if (m_sceneSkillCard != null)
            {
                if (UtilApi.isAddressEqual(m_sceneSkillCard.getGameObject(), underGo))
                {
                    return m_sceneSkillCard;
                }
            }

            if (m_sceneEquipCard != null)
            {
                if (UtilApi.isAddressEqual(m_sceneEquipCard.getGameObject(), underGo))
                {
                    return m_sceneEquipCard;
                }
            }

            return null;
        }

        public void addCardToOutList(SceneDragCard card, int idx = 0)
        {
            m_outSceneCardList.addCard(card);
            m_outSceneCardList.updateSceneCardPos();
        }

        // 将 Out 区域中的第一个牌退回到 handle 中
        public void putHandFromOut()
        {
            SceneDragCard card = m_outSceneCardList.removeNoDestroyAndRet() as SceneDragCard;
            if(card != null)
            {
                card.retFormOutAreaToHandleArea();
            }
        }
    }
}