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

        protected CrystalPtPanel m_crystalPtPanel;

        public SceneDZArea(SceneDZData sceneDZData, EnDZPlayer playerFlag)
        {
            m_sceneDZData = sceneDZData;
            m_playerFlag = playerFlag;
            m_outSceneCardList = new OutSceneCardList(m_sceneDZData, m_playerFlag);
            m_crystalPtPanel = new CrystalPtPanel(m_playerFlag);
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

        public CrystalPtPanel crystalPtPanel
        {
            get
            {
                return m_crystalPtPanel;
            }
            set
            {
                m_crystalPtPanel = value;
            }
        }

        public void updateMp()
        {
            m_crystalPtPanel.updateMp();
        }

        // 添加卡牌不包括 CardArea.CARDCELLTYPE_COMMON 区域， enemy 对方出牌也是这个消息
        public void psstAddBattleCardPropertyUserCmd(stAddBattleCardPropertyUserCmd msg)
        {
            if (msg.byActionType == 1)
            {
                addSceneCardByItem(msg.sceneItem);
            }
            else        // 更新卡牌数据
            {
                updateSceneCardByItem(msg.sceneItem);
            }
        }

        public void addSceneCardBySvrData(t_Card mobject)
        {
            SceneCardItem sceneItem = null;
            // 填充数据
            sceneItem = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].createCardItemBySvrData(m_playerFlag, mobject);
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].addOneSceneCard(sceneItem);       // 添加数据

            addSceneCardByItem(sceneItem);
        }

        public void addSceneCardByItem(SceneCardItem sceneItem)
        {
            if (CardArea.CARDCELLTYPE_HERO == sceneItem.cardArea)
            {
                m_centerHero = Ctx.m_instance.m_sceneCardMgr.createCard(sceneItem, m_sceneDZData) as HeroCard;

                m_centerHero.setClasss((EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroOccupation);
                // 设置 hero 动画结束后的处理
                m_centerHero.heroAniEndDisp = m_sceneDZData.heroAniEndDisp;
            }
            else if (CardArea.CARDCELLTYPE_SKILL == sceneItem.cardArea)
            {
                m_sceneSkillCard = Ctx.m_instance.m_sceneCardMgr.createCard(sceneItem, m_sceneDZData);
            }
            else if (CardArea.CARDCELLTYPE_EQUIP == sceneItem.cardArea)
            {
                m_sceneEquipCard = Ctx.m_instance.m_sceneCardMgr.createCard(sceneItem, m_sceneDZData);
            }
            else if (CardArea.CARDCELLTYPE_HAND == sceneItem.cardArea)
            {
                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_recStartCardNum >= Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].getStartCardNum())        // 判断接收的数据是否是 startCardList 列表中的数据
                {
                    m_inSceneCardList.addCardByIdAndItem(sceneItem.svrCard.dwObjectID, sceneItem);
                }
                else
                {
                    m_inSceneCardList.setCardDataByIdx(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_recStartCardNum, sceneItem);
                    ++Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_recStartCardNum;
                }
            }
            else if (CardArea.CARDCELLTYPE_COMMON == sceneItem.cardArea)      // 只有对方出牌的时候才会走这里
            {
                SceneCardBase srcCard = Ctx.m_instance.m_sceneCardMgr.createCard(sceneItem, m_sceneDZData);
                srcCard.convOutModel();
                m_outSceneCardList.addCard(srcCard, srcCard.sceneCardItem.svrCard.pos.y);
                m_outSceneCardList.updateSceneCardPos();
            }
        }

        public void updateSceneCardBySvrData(t_Card mobject)
        {
            SceneCardItem sceneItem = null;
            sceneItem = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].updateCardInfoByCardItem(mobject);
            updateSceneCardByItem(sceneItem);
        }

        public void updateSceneCardByItem(SceneCardItem sceneItem)
        {
            if (CardArea.CARDCELLTYPE_HERO == sceneItem.cardArea)     // 如果是 hero ，hero 自己已经创建显示了
            {
                m_centerHero.updateCardDataChangeBySvr();      // 这个动画已经有了
            }
            else if (CardArea.CARDCELLTYPE_SKILL == sceneItem.cardArea)
            {
                m_sceneSkillCard.updateCardDataChangeBySvr();
            }
            else if (CardArea.CARDCELLTYPE_EQUIP == sceneItem.cardArea)
            {
                m_sceneEquipCard.updateCardDataChangeBySvr();
            }
            else if (CardArea.CARDCELLTYPE_HAND == sceneItem.cardArea)
            {
                m_inSceneCardList.updateCardData(sceneItem);
            }
            else if (CardArea.CARDCELLTYPE_COMMON == sceneItem.cardArea)      // 只有对方出牌的时候才会走这里
            {
                m_outSceneCardList.updateCardData(sceneItem);
            }
        }

        public void psstRetMoveGameCardUserCmd(stRetMoveGameCardUserCmd msg)
        {
            changeSceneCard(msg.qwThisID, (CardArea)msg.m_sceneCardItem.svrCard.pos.dwLocation, msg.dst.y);
        }

        // 移动卡牌，从一个位置到另外一个位置，CardArea.CARDCELLTYPE_COMMON 区域增加是从这个消息过来的，目前只处理移动到 CardArea.CARDCELLTYPE_COMMON 区域
        public void changeSceneCard(uint qwThisID, CardArea dwLocation, ushort yPos)
        {
            SceneCardBase srcCard = null;

            // 移动手里的牌的位置
            srcCard = m_inSceneCardList.getSceneCardByThisID(qwThisID);
            if (srcCard != null)
            {
                m_inSceneCardList.removeCard(srcCard);
                m_inSceneCardList.updateSceneCardPos();
                srcCard.updateCardOutState(false);        // 当前卡牌可能处于绿色高亮，因此去掉
                srcCard.convOutModel();
                m_sceneDZData.m_gameOpState.quitMoveOp();      // 退出移动操作
            }
            else            // 如果手牌没有，可能是战吼或者法术有攻击目标的卡牌，客户端已经将卡牌移动到出牌区域
            {
                srcCard = m_outSceneCardList.getSceneCardByThisID(qwThisID);
                if (srcCard.canClientMove2OutArea())
                {
                    // 更新手牌索引
                    m_inSceneCardList.updateCardIndex();
                    m_sceneDZData.m_gameOpState.endAttackOp();    // 战吼攻击结束
                }
                else
                {
                    Ctx.m_instance.m_logSys.log("这个牌客户端已经移动到出牌区域");
                }
            }

            // 更新移动后的牌的位置
            if (CardArea.CARDCELLTYPE_EQUIP == dwLocation)        // 如果出的是装备
            {
                outEquipCard(srcCard);
            }
            else        // 出的是随从
            {
                if (srcCard != null && !srcCard.canClientMove2OutArea())         // 如果不是战吼或者法术有攻击目标的牌
                {
                    m_outSceneCardList.removeWhiteCard();
                    m_outSceneCardList.addCard(srcCard, yPos);
                    m_outSceneCardList.updateSceneCardPos();
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

        // 给自己添加一张手牌
        public void addOneSelfHandCard(CardArea area)
        {

        }

        // 将当前拖放的对象移动会原始位置
        public void moveDragBack()
        {
            if(m_sceneDZData.m_curDragItem != null)
            {
                m_sceneDZData.m_curDragItem.trackAniControl.moveToDestST();
            }
        }

        // test 移动卡牌
        public void moveCard()
        {
            m_outSceneCardList.removeWhiteCard();
            m_inSceneCardList.removeCard(m_sceneDZData.m_curDragItem);
            m_outSceneCardList.addCard(m_sceneDZData.m_curDragItem, m_sceneDZData.curWhiteIdx);
            m_inSceneCardList.updateSceneCardPos();
            m_outSceneCardList.updateSceneCardPos();
        }

        public void psstAddEnemyHandCardPropertyUserCmd()
        {
            m_inSceneCardList.addCardByIdAndItem(SceneDZCV.BLACK_CARD_ID, null);
        }

        // 移除并且释放一张卡牌，这个接口只能是从外部调用，彻底删除一张卡牌
        public void removeAndDestroyOneCardByItem(SceneCardItem sceneItem)
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
                m_outSceneCardList.removeAndDestroyCardByItem(sceneItem);
                m_outSceneCardList.updateSceneCardPos();
            }
            else if ((int)CardArea.CARDCELLTYPE_HAND == sceneItem.svrCard.pos.dwLocation)
            {
                if (m_inSceneCardList.removeAndDestroyCardByItem(sceneItem))
                {
                    m_inSceneCardList.updateSceneCardPos();
                }
                else        // 可能是战吼或者法术有攻击目标的
                {
                    SceneCardBase srcCard = m_outSceneCardList.removeAndRetCardByItem(sceneItem);
                    // 如果是法术或者战吼有攻击目标的卡牌，虽然在出牌区，但是是客户端自己移动过去的
                    if (srcCard != null && srcCard.canClientMove2OutArea())
                    {
                        // 更新手牌索引
                        m_inSceneCardList.updateCardIndex();
                    }

                    srcCard.dispose();
                }
            }

            m_sceneDZData.m_gameOpState.cancelAttackOp();        // 强制退出战斗操作
        }

        // 移除一张卡牌，不释放资源，这个接口仅仅是客户端自己释放资源使用
        public void removeOneCardByItem(SceneCardItem sceneItem)
        {
            if ((int)CardArea.CARDCELLTYPE_SKILL == sceneItem.svrCard.pos.dwLocation)
            {
                m_sceneSkillCard = null;
            }
            else if ((int)CardArea.CARDCELLTYPE_EQUIP == sceneItem.svrCard.pos.dwLocation)
            {
                m_sceneEquipCard = null;
            }
            else if ((int)CardArea.CARDCELLTYPE_COMMON == sceneItem.svrCard.pos.dwLocation)
            {
                m_outSceneCardList.removeCardIByItem(sceneItem);
                m_outSceneCardList.updateSceneCardPos();
            }
            else if ((int)CardArea.CARDCELLTYPE_HAND == sceneItem.svrCard.pos.dwLocation)
            {
                if (m_inSceneCardList.removeCardIByItem(sceneItem))
                {
                    m_inSceneCardList.updateSceneCardPos();
                }
                else        // 可能是战吼或者法术有攻击目标的
                {
                    SceneCardBase srcCard = m_outSceneCardList.removeAndRetCardByItem(sceneItem);
                    // 如果是法术或者战吼有攻击目标的卡牌，虽然在出牌区，但是是客户端自己移动过去的
                    if (srcCard != null && srcCard.canClientMove2OutArea())
                    {
                        // 更新手牌索引
                        m_inSceneCardList.updateCardIndex();
                    }
                }
            }
        }

        public void removeOneCard(SceneCardBase card)
        {
            if (card is SkillCard)
            {
                m_sceneSkillCard = null;
            }
            else if (card is EquipCard)
            {
                m_sceneEquipCard = null;
            }
            else if(card is BlackCard)      // Enemy Hand 手牌卡
            {
                m_inSceneCardList.removeCard(card);
                m_inSceneCardList.updateSceneCardPos();
            }
            else if ((int)CardArea.CARDCELLTYPE_COMMON == card.sceneCardItem.svrCard.pos.dwLocation)
            {
                m_outSceneCardList.removeCard(card);
                m_outSceneCardList.updateSceneCardPos();
            }
            else if ((int)CardArea.CARDCELLTYPE_HAND == card.sceneCardItem.svrCard.pos.dwLocation)
            {
                if (m_inSceneCardList.Contains(card))
                {
                    m_inSceneCardList.removeCard(card);
                    m_inSceneCardList.updateSceneCardPos();
                }
                else        // 可能是战吼或者法术有攻击目标的
                {
                    SceneCardBase srcCard = m_outSceneCardList.removeAndRetCardByItem(card.sceneCardItem);
                    // 如果是法术或者战吼有攻击目标的卡牌，虽然在出牌区，但是是客户端自己移动过去的
                    if (srcCard != null && srcCard.canClientMove2OutArea())
                    {
                        // 更新手牌索引
                        m_inSceneCardList.updateCardIndex();
                    }
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
        public void addCardToOutList(SceneCardBase card, int idx = -1)
        {
            card.sceneCardItem.cardArea = CardArea.CARDCELLTYPE_COMMON;     // 更新卡牌区域信息
            m_outSceneCardList.addCard(card, idx);                          // 更新卡牌列表
            m_outSceneCardList.updateSceneCardPos();                        // 更新位置信息
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
                m_outSceneCardList.updateSceneCardPos();
            }
        }

        public void putHandFromOutByCard(SceneCardBase card)
        {
            // 从出牌区域移除
            m_outSceneCardList.removeCard(card);
            m_outSceneCardList.updateSceneCardPos();

            // 放入手牌区域
            card.convHandleModel();
            card.sceneCardItem.cardArea = CardArea.CARDCELLTYPE_HAND;       // 更新卡牌区域信息
            card.curIndex = card.preIndex;                                  // 更新索引信息
            card.dragControl.enableDrag();                                  // 开启拖放
            m_inSceneCardList.addCardByServerPos(card);                     // 添加到手牌位置
            m_inSceneCardList.updateSceneCardPos();                         // 更新位置信息，索引就不更新了，因为如果退回来索引还是原来的，没有改变
        }

        public void cancelFashuOp(SceneCardBase card)
        {
            card.show();
            m_inSceneCardList.updateSceneCardPos();
            m_centerHero.effectControl.stopSkillAttPrepareEffect();
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

            // 检查英雄
            if (m_centerHero != null)
            {
                if (m_centerHero.sceneCardItem.svrCard.qwThisID == thisID)
                {
                    slot = CardArea.CARDCELLTYPE_HERO;
                    return m_centerHero;
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

        // 从手牌区域出了一张装备卡
        protected void outEquipCard(SceneCardBase outCard)
        {
            if (m_sceneEquipCard == null)       // 直接替换数据
            {
                m_sceneEquipCard = Ctx.m_instance.m_sceneCardMgr.createCard(outCard.sceneCardItem, m_sceneDZData);
            }
            else
            {
                m_sceneEquipCard.setIdAndPnt(outCard.sceneCardItem.svrCard.dwObjectID, outCard.getPnt());
            }

            m_sceneEquipCard.behaviorControl.moveToDestDirect(m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_EQUIP].transform.localPosition);

            outCard.dispose();      // 释放原来的资源
        }
    }
}