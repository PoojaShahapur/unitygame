using SDK.Common;

namespace Game.UI
{
    public enum EnGameOp
    {
        eOpNone,        // 没进行任何操作
        eOpAttack,      // 攻击操作
        eOpFaShu,       // 法术操作
        eOpTotal
    }

    /**
     * @brief 游戏当前操作状态，仅处理场景中交互的内容，如果触摸到不交互的场景的内容，不打断场景逻辑
     */
    public class GameOpState : IGameOpState
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
            m_curOp = op;
            m_opCard = card;
            // 开始拖动箭头
            m_sceneDZData.m_attackArrow.startArrow();
        }

        // 退出攻击操作
        public void quitAttackOp()
        {
            m_curOp = EnGameOp.eOpNone;
            m_opCard = null;
            m_sceneDZData.m_attackArrow.stopArrow();
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
                }
            }

            return ret;
        }

        protected bool canNormalAttack(SceneCardEntityBase card, EnGameOp gameOp)
        {
            if (m_opCard.sceneCardItem.m_playerFlag != card.sceneCardItem.m_playerFlag && !UtilMath.checkState(StateID.CARD_STATE_SLEEP, card.sceneCardItem.m_svrCard.state))
            {
                return true;
            }

            return false;
        }

        protected bool canFaShuAttack(SceneCardEntityBase card, EnGameOp gameOp)
        {
            if (UtilMath.checkAttackState(AttackTarget.ATTACK_TARGET_SHERO, m_opCard.sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget))
            {
                if (EnDZPlayer.ePlayerSelf == card.sceneCardItem.m_playerFlag)       // 如果是自己
                {
                    if (CardArea.CARDCELLTYPE_HERO == card.sceneCardItem.m_cardArea)     // 如果是主角
                    {
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
                        return true;
                    }
                }
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
    }
}