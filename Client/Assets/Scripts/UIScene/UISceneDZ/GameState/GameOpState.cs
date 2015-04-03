using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;

namespace Game.UI
{
    public enum EnGameOp
    {
        eOpNone,        // 没进行任何操作
        eOpAttack,      // 攻击操作
        eOpFaShu,       // 法术操作
        eOpZhanHouAttack,   // 战吼攻击
        eOpTotal
    }

    /**
     * @brief 游戏当前操作状态，仅处理场景中交互的内容，如果触摸到不交互的场景的内容，不打断场景逻辑
     */
    public class GameOpState
    {
        protected SceneDZData m_sceneDZData;

        protected EnGameOp m_curOp;         // 当前操作
        protected SceneCardEntityBase m_opCard;      // 当前操作的卡牌

        public GameOpState(SceneDZData sceneDZData)
        {
            m_curOp = EnGameOp.eOpNone;
            m_sceneDZData = sceneDZData;
        }

        // 进入攻击操作
        public void enterAttackOp(EnGameOp op, SceneCardEntityBase card)
        {
            // 如果不是自己的回合，直接返回
            if (!Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                return;
            }
            // 进入某一个状态的时候，要查看之前的状态是否需要需要处理
            checkPreAttackOp(op, m_opCard);

            m_curOp = op;
            m_opCard = card;
            // 开始拖动箭头
            m_sceneDZData.m_attackArrow.startArrow();

            addAttackTargetFlags();
        }

        // 检查之前的攻击状态
        public void checkPreAttackOp(EnGameOp op, SceneCardEntityBase card)
        {
            if(EnGameOp.eOpZhanHouAttack == m_curOp)
            {
                // 需要将其回退回去
                (m_opCard as SceneDragCard).retFormOutAreaToHandleArea();
            }
        }

        // 退出攻击操作
        public void quitAttackOp()
        {
            m_curOp = EnGameOp.eOpNone;
            m_opCard = null;
            m_sceneDZData.m_attackArrow.stopArrow();
            clearAttackTargetFlags();
        }

        // 判断是否在某个操作中
        public bool bInOp(EnGameOp op)
        {
            return op == m_curOp;
        }

        public bool canAttackOp(SceneCardEntityBase card, EnGameOp gameOp)
        {
            bool ret = false;
            if (m_opCard != null)
            {
                if (gameOp == m_curOp)  // 如果当前处于这个操作状态
                {
                    if (gameOp == EnGameOp.eOpAttack)  // 如果当前处于攻击
                    {
                        ret = canNormalAttack(card, gameOp);
                    }
                    else if (gameOp == EnGameOp.eOpFaShu)  // 当前处于法术牌攻击
                    {
                        ret = canFaShuAttack(card, gameOp);
                    }
                    else if (gameOp == EnGameOp.eOpZhanHouAttack)  // 当前处于战吼牌攻击
                    {
                        ret = canZhanHouAttack(card, gameOp);
                    }
                }
            }

            return ret;
        }

        protected bool canNormalAttack(SceneCardEntityBase card, EnGameOp gameOp)
        {
            //if (m_opCard.sceneCardItem.m_playerFlag != card.sceneCardItem.m_playerFlag && !UtilMath.checkState(StateID.CARD_STATE_SLEEP, card.sceneCardItem.m_svrCard.state))
            //{
            //    return true;
            //}

            //return false;
            bool ret = false;
            stCardAttackMagicUserCmd cmd = new stCardAttackMagicUserCmd();
            cmd.dwAttThisID = m_opCard.sceneCardItem.m_svrCard.qwThisID;
            cmd.dwDefThisID = card.sceneCardItem.m_svrCard.qwThisID;
            cmd.dwMagicType = (uint)m_opCard.sceneCardItem.m_cardTableItem.m_faShu;
            ret = Ctx.m_instance.m_dataPlayer.m_dzData.cardAttackMagic(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_opCard.sceneCardItem.m_playerFlag], cmd);

            if(ret)
            {
                Ctx.m_instance.m_uiMgr.getForm<UIChat>(UIFormID.UIChat).outMsg("Client 普通攻击验证通过");
            }

            return ret;
        }

        protected bool canFaShuAttack(SceneCardEntityBase card, EnGameOp gameOp)
        {
            stCardAttackMagicUserCmd cmd = new stCardAttackMagicUserCmd();
            cmd.dwAttThisID = m_opCard.sceneCardItem.m_svrCard.qwThisID;
            cmd.dwDefThisID = card.sceneCardItem.m_svrCard.qwThisID;
            cmd.dwMagicType = (uint)m_opCard.sceneCardItem.m_cardTableItem.m_faShu;

            if (Ctx.m_instance.m_dataPlayer.m_dzData.cardAttackMagic(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_opCard.sceneCardItem.m_playerFlag], cmd))
            {
                if (UtilMath.checkAttackState(AttackTarget.ATTACK_TARGET_SHERO, m_opCard.sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget))
                {
                    if (EnDZPlayer.ePlayerSelf == card.sceneCardItem.m_playerFlag)       // 如果是自己
                    {
                        if (CardArea.CARDCELLTYPE_HERO == card.sceneCardItem.m_cardArea)     // 如果是主角
                        {
                            Ctx.m_instance.m_uiMgr.getForm<UIChat>(UIFormID.UIChat).outMsg("Client 法术攻击验证通过");
                            return true;
                        }
                    }
                }
                if (UtilMath.checkAttackState(AttackTarget.ATTACK_TARGET_SATTEND, m_opCard.sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget))
                {
                    if (EnDZPlayer.ePlayerSelf == card.sceneCardItem.m_playerFlag)       // 如果是自己
                    {
                        if (CardArea.CARDCELLTYPE_COMMON == card.sceneCardItem.m_cardArea)     // 如果是出牌区
                        {
                            Ctx.m_instance.m_uiMgr.getForm<UIChat>(UIFormID.UIChat).outMsg("Client 法术攻击验证通过");
                            return true;
                        }
                    }
                }
                if (UtilMath.checkAttackState(AttackTarget.ATTACK_TARGET_EHERO, m_opCard.sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget))
                {
                    if (EnDZPlayer.ePlayerEnemy == card.sceneCardItem.m_playerFlag)       // 如果是 enemy
                    {
                        if (CardArea.CARDCELLTYPE_HERO == card.sceneCardItem.m_cardArea)     // 如果是主角
                        {
                            Ctx.m_instance.m_uiMgr.getForm<UIChat>(UIFormID.UIChat).outMsg("Client 法术攻击验证通过");
                            return true;
                        }
                    }
                }
                if (UtilMath.checkAttackState(AttackTarget.ATTACK_TARGET_EATTEND, m_opCard.sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget))
                {
                    if (EnDZPlayer.ePlayerEnemy == card.sceneCardItem.m_playerFlag)       // 如果是 enemy
                    {
                        if (CardArea.CARDCELLTYPE_COMMON == card.sceneCardItem.m_cardArea)     // 如果是出牌区
                        {
                            Ctx.m_instance.m_uiMgr.getForm<UIChat>(UIFormID.UIChat).outMsg("Client 法术攻击验证通过");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected bool canZhanHouAttack(SceneCardEntityBase card, EnGameOp gameOp)
        {
            if (m_opCard.sceneCardItem.m_playerFlag != card.sceneCardItem.m_playerFlag && !UtilMath.checkState(StateID.CARD_STATE_SLEEP, card.sceneCardItem.m_svrCard.state))
            {
                return true;
            }

            return false;
        }

        public uint getOpCardID()
        {
            if(m_opCard != null)
            {
                return m_opCard.sceneCardItem.m_svrCard.qwThisID;
            }

            return 0;
        }

        public int getOpCardFaShu()
        {
            if (m_opCard != null)
            {
                return m_opCard.sceneCardItem.m_cardTableItem.m_faShu;
            }

            return 0;
        }

        // 可以攻击的目标显示效果，发送攻击消息的时候去掉显示
        protected void addAttackTargetFlags()
        {
            // 遍历所有的 enemy 对象
            List<SceneDragCard> cardList;
            cardList = m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].outSceneCardList.sceneCardList;
            
            foreach(SceneCardEntityBase cardItem in cardList)
            {
                if(canFaShuAttack(cardItem, m_curOp))
                {
                    cardItem.updateCardGreenFrame(true);
                }
            }
        }

        // 清除可攻击的标识
        protected void clearAttackTargetFlags()
        {
            // 遍历所有的 enemy 对象
            List<SceneDragCard> cardList;
            cardList = m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].outSceneCardList.sceneCardList;

            foreach (SceneCardEntityBase cardItem in cardList)
            {
                cardItem.updateCardGreenFrame(false);
            }
        }
    }
}