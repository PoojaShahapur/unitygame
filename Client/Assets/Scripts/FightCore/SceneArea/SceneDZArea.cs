using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 场景对战区域
     */
    public class SceneDZArea
    {
        public SceneDZData m_sceneDZData;
        public EnDZPlayer m_playerFlag;                 // 指示玩家的位置

        protected OutSceneCardList m_outSceneCardList; // 已经出的牌，在场景中心
        protected InSceneCardList m_inSceneCardList;   // 场景可拖放的卡牌列表，最底下的，还没有出的牌
        protected HeroCard m_centerHero;                                            // 主角自己的 hero 
        protected SceneCardBase m_sceneSkillCard;                // skill
        protected SceneCardBase m_sceneEquipCard;                // equip

        public SceneDZArea(SceneDZData sceneDZData, EnDZPlayer playerFlag)
        {
            m_sceneDZData = sceneDZData;
            m_playerFlag = playerFlag;
            m_outSceneCardList = new OutSceneCardList(m_sceneDZData, m_playerFlag);
        }

        virtual public void dispose()
        {
            m_outSceneCardList.dispose();
            m_inSceneCardList.dispose();
            m_centerHero.dispose();
            m_sceneSkillCard.dispose();
            m_sceneEquipCard.dispose();
        }

        public OutSceneCardList outSceneCardList
        {
            get
            {
                return m_outSceneCardList;
            }
            set
            {
                m_outSceneCardList = value;
            }
        }

        public InSceneCardList inSceneCardList
        {
            get
            {
                return m_inSceneCardList;
            }
            set
            {
                InSceneCardList m_inSceneCardList = value;
            }
        }

        public HeroCard centerHero
        {
            get
            {
                return m_centerHero;
            }
            set
            {
                m_centerHero = value;
            }
        }

        // 添加卡牌不包括 CardArea.CARDCELLTYPE_COMMON 区域， enemy 对方出牌也是这个消息
        public void psstAddBattleCardPropertyUserCmd(stAddBattleCardPropertyUserCmd msg, SceneCardItem sceneItem)
        {
            if (msg.byActionType == 1)
            {
                if ((int)CardArea.CARDCELLTYPE_HERO == msg.slot)     // 如果是 hero ，hero 自己已经创建显示了
                {
                    m_centerHero = Ctx.m_instance.m_sceneCardMgr.createCard(msg.mobject.dwObjectID, m_playerFlag, (CardArea)msg.slot, CardType.CARDTYPE_HERO, m_sceneDZData) as HeroCard;
                    m_centerHero.sceneCardItem = sceneItem;      // 这个动画已经有了
                    // 设置 hero 动画结束后的处理
                    m_centerHero.heroAniEndDisp = m_sceneDZData.heroAniEndDisp;
                    m_centerHero.updateHp();
                }
                else if ((int)CardArea.CARDCELLTYPE_SKILL == msg.slot)
                {
                    m_sceneSkillCard = Ctx.m_instance.m_sceneCardMgr.createCard(msg.mobject.dwObjectID, m_playerFlag, (CardArea)msg.slot, CardType.CARDTYPE_SKILL, m_sceneDZData);
                    m_sceneSkillCard.sceneCardItem = sceneItem;
                    m_sceneSkillCard.trackAniControl.moveToDestRST();
                }
                else if ((int)CardArea.CARDCELLTYPE_EQUIP == msg.slot)
                {
                    m_sceneEquipCard = Ctx.m_instance.m_sceneCardMgr.createCard(msg.mobject.dwObjectID, m_playerFlag, (CardArea)msg.slot, CardType.CARDTYPE_EQUIP, m_sceneDZData);
                    m_sceneEquipCard.sceneCardItem = sceneItem;
                    m_sceneEquipCard.trackAniControl.moveToDestRST();
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
                    SceneCardBase srcCard = Ctx.m_instance.m_sceneCardMgr.createCard(msg.mobject.dwObjectID, m_playerFlag, (CardArea)msg.slot, CardType.CARDTYPE_ATTEND, m_sceneDZData);
                    srcCard.sceneCardItem = sceneItem;
                    m_outSceneCardList.addCard(srcCard);
                    m_outSceneCardList.updateSceneCardRST();
                    m_outSceneCardList.updateCardIndex();
                }
            }
            else        // 更新卡牌数据
            {
                if ((int)CardArea.CARDCELLTYPE_HERO == msg.slot)     // 如果是 hero ，hero 自己已经创建显示了
                {
                    m_centerHero.updateCardDataChange();      // 这个动画已经有了
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
            SceneCardBase srcCard = null;

            // 移动手里的牌的位置
            srcCard = m_inSceneCardList.getSceneCardByThisID(msg.qwThisID);
            if (srcCard != null)
            {
                m_inSceneCardList.removeCard(srcCard);
                srcCard.updateCardOutState(false);        // 当前卡牌可能处于绿色高亮，因此去掉
                m_sceneDZData.m_gameOpState.quitMoveOp();      // 退出移动操作
            }
            else            // 如果手牌没有，可能是战吼或者法术有攻击目标的卡牌，客户端已经将卡牌移动到出牌区域
            {
                srcCard = m_outSceneCardList.getSceneCardByThisID(msg.qwThisID);
                if (srcCard.canClientMove2OutArea())
                {
                    // 更新手牌索引
                    m_inSceneCardList.updateCardIndex();
                    m_sceneDZData.m_gameOpState.quitAttackOp(false);    // 强制退出战斗操作
                }
                else
                {
                    Ctx.m_instance.m_logSys.log("这个牌客户端已经移动到出牌区域");
                }
            }

            m_inSceneCardList.updateCardIndex();
            m_inSceneCardList.updateSceneCardRST();

            // 更新移动后的牌的位置
            if ((int)CardArea.CARDCELLTYPE_EQUIP == msg.m_sceneCardItem.svrCard.pos.dwLocation)        // 如果出的是装备
            {
                m_sceneEquipCard = srcCard;
                m_sceneEquipCard.trackAniControl.destPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_EQUIP].transform.localPosition;
                m_sceneEquipCard.trackAniControl.moveToDestRST();
            }
            else        // 出的是随从
            {
                if (srcCard != null && !srcCard.canClientMove2OutArea())         // 如果不是战吼或者法术有攻击目标的牌
                {
                    m_outSceneCardList.removeWhiteCard();
                    m_outSceneCardList.addCard(srcCard, msg.dst.y);
                    m_outSceneCardList.updateSceneCardRST();
                    m_outSceneCardList.updateCardIndex();
                }
            }

            // 修改 mp 
            //if (msg.side == 1)      // 如果是自己
            //{
            //    // 更新 mp 显示
            //    if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[msg.side - 1].m_heroMagicPoint.mp >= (uint)srcCard.sceneCardItem.m_cardTableItem.m_magicConsume)
            //    {
            //        Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[msg.side - 1].m_heroMagicPoint.mp -= (uint)srcCard.sceneCardItem.m_cardTableItem.m_magicConsume;
            //    }
            //    updateMp();
            //}
        }

        // 将当前拖放的对象移动会原始位置
        public void moveDragBack()
        {
            if(m_sceneDZData.m_curDragItem != null)
            {
                m_sceneDZData.m_curDragItem.trackAniControl.moveToDestRST();
            }
        }

        // test 移动卡牌
        public void moveCard()
        {
            m_outSceneCardList.removeWhiteCard();
            m_inSceneCardList.removeCard(m_sceneDZData.m_curDragItem);
            m_outSceneCardList.addCard(m_sceneDZData.m_curDragItem, m_sceneDZData.curWhiteIdx);
            m_inSceneCardList.updateSceneCardRST();
            m_outSceneCardList.updateSceneCardRST();
            m_inSceneCardList.updateCardIndex();
            m_outSceneCardList.updateCardIndex();
        }

        public void psstAddEnemyHandCardPropertyUserCmd()
        {
            m_inSceneCardList.addSceneCard(uint.MaxValue, null);
        }

        public void delOneCard(SceneCardItem sceneItem)
        {
            if ((int)CardArea.CARDCELLTYPE_SKILL == sceneItem.svrCard.pos.dwLocation)
            {
                m_sceneSkillCard.dispose();
                m_sceneSkillCard = null;
            }
            else if ((int)CardArea.CARDCELLTYPE_EQUIP == sceneItem.svrCard.pos.dwLocation)
            {
                m_sceneEquipCard.dispose();
                m_sceneEquipCard = null;
            }
            else if ((int)CardArea.CARDCELLTYPE_COMMON == sceneItem.svrCard.pos.dwLocation)
            {
                m_outSceneCardList.removeCard(sceneItem);
                m_outSceneCardList.updateSceneCardRST();
                m_outSceneCardList.updateCardIndex();
            }
            else if ((int)CardArea.CARDCELLTYPE_HAND == sceneItem.svrCard.pos.dwLocation)
            {
                if (m_inSceneCardList.removeCard(sceneItem))
                {
                    m_inSceneCardList.updateSceneCardRST();
                    m_inSceneCardList.updateCardIndex();
                }
                else        // 可能是战吼或者法术有攻击目标的
                {
                    SceneCardBase srcCard = m_outSceneCardList.removeAndRetCardByItemNoDestroy(sceneItem);
                    // 如果是法术或者战吼有攻击目标的卡牌，虽然在出牌区，但是是客户端自己移动过去的
                    if (srcCard != null && srcCard.canClientMove2OutArea())
                    {
                        // 更新手牌索引
                        m_inSceneCardList.updateCardIndex();
                    }

                    srcCard.dispose();      // 释放这个卡牌
                }
            }

            m_sceneDZData.m_gameOpState.quitAttackOp(false);        // 强制退出战斗操作
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
                    //m_sceneDZData.m_mpGridArr[(int)m_playerFlag].AddChild((UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getcostModel().getObject()) as GameObject).transform);
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

        public SceneCardBase getUnderSceneCard(GameObject underGo)
        {
            SceneCardBase cardBase;
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
                if (UtilApi.isAddressEqual(m_sceneSkillCard.gameObject(), underGo))
                {
                    return m_sceneSkillCard;
                }
            }

            if (m_sceneEquipCard != null)
            {
                if (UtilApi.isAddressEqual(m_sceneEquipCard.gameObject(), underGo))
                {
                    return m_sceneEquipCard;
                }
            }

            return null;
        }

        // 从输入卡牌列表到输出卡牌列表
        public void addCardToOutList(SceneCardBase card, int idx = 0)
        {
            card.sceneCardItem.cardArea = CardArea.CARDCELLTYPE_COMMON;     // 更新卡牌区域信息
            m_outSceneCardList.addCard(card, idx);                          // 更新卡牌列表
            m_outSceneCardList.updateSceneCardRST();                        // 更新位置信息
            m_outSceneCardList.updateCardIndex();                           // 更新卡牌索引信息
        }

        // 从手牌区域移除一个卡牌
        public void removeFormInList(SceneCardBase card)
        {
            inSceneCardList.removeCard(card);
        }

        // 将 Out 区域中的第一个牌退回到 handle 中
        public void putHandFromOut()
        {
            SceneCardBase card = m_outSceneCardList.removeNoDestroyAndRet() as SceneCardBase;
            if(card != null)
            {
                card.trackAniControl.retFormOutAreaToHandleArea();
                m_outSceneCardList.updateSceneCardRST();
                m_outSceneCardList.updateCardIndex();
            }
        }

        public void putHandFromOutByCard(SceneCardBase card)
        {
            // 从出牌区域移除
            m_outSceneCardList.removeCardNoDestroy(card);
            //(card as SceneCardBase).retFormOutAreaToHandleArea();
            m_outSceneCardList.updateSceneCardRST();
            m_outSceneCardList.updateCardIndex();

            // 放入手牌区域
            card.sceneCardItem.cardArea = CardArea.CARDCELLTYPE_HAND;       // 更新卡牌区域信息
            card.curIndex = card.preIndex;                                  // 更新索引信息
            card.dragControl.enableDrag();                                              // 开启拖放
            m_inSceneCardList.addCardByServerPos(card);                     // 添加到手牌位置
            m_inSceneCardList.updateSceneCardRST();                         // 更新位置信息，索引就不更新了，因为如果退回来索引还是原来的，没有改变
        }

        public void psstRetCardAttackFailUserCmd(stRetCardAttackFailUserCmd cmd)
        {
            if (m_sceneDZData.m_curDragItem != null && m_sceneDZData.m_curDragItem.sceneCardItem.svrCard.qwThisID == cmd.dwAttThisID)
            {
                m_sceneDZData.m_curDragItem.dragControl.backCard2Orig();
            }
        }

        // 更新卡牌绿色边框，说明可以出牌
        public void updateInCardOutState(bool benable)
        {
            m_inSceneCardList.updateCardOutState(benable);
        }

        // 更新卡牌绿色边框，说明可以出牌
        public void updateOutCardOutState(bool benable)
        {
            m_outSceneCardList.updateCardOutState(benable);
        }

        public SceneCardBase getSceneCardByThisID(uint thisID, ref CardArea slot)
        {
            // 出牌列表
            SceneCardBase cardBase;
            cardBase = m_outSceneCardList.getSceneCardByThisID(thisID);
            if (cardBase != null)
            {
                slot = CardArea.CARDCELLTYPE_COMMON;
                return cardBase;
            }

            // 对手区域手牌是不检查的
            if (EnDZPlayer.ePlayerSelf == m_playerFlag)
            {
                cardBase = m_inSceneCardList.getSceneCardByThisID(thisID);
                if (cardBase != null)
                {
                    slot = CardArea.CARDCELLTYPE_HAND;
                    return cardBase;
                }
            }

            // 技能区
            if (m_sceneSkillCard != null)
            {
                if (m_sceneSkillCard.sceneCardItem.svrCard.qwThisID == thisID)
                {
                    slot = CardArea.CARDCELLTYPE_SKILL;
                    return m_sceneSkillCard;
                }
            }

            // 装备区
            if (m_sceneEquipCard != null)
            {
                if (m_sceneEquipCard.sceneCardItem.svrCard.qwThisID == thisID)
                {
                    slot = CardArea.CARDCELLTYPE_EQUIP;
                    return m_sceneEquipCard;
                }
            }

            return null;
        }

        virtual public void updateCardAttackedState(GameOpState opt)
        {

        }

        virtual public void clearCardAttackedState()
        {

        }

        // 出牌区域卡牌是否满了
        public bool bOutAreaCardFull()
        {
            return m_outSceneCardList.bAreaCardFull();
        }

        // 除了 card 禁止所有手牌区域卡牌拖动
        virtual public void disableAllInCardDragExceptOne(SceneCardBase card)
        {

        }

        virtual public void enableAllInCardDragExceptOne(SceneCardBase card)
        {

        }
    }
}