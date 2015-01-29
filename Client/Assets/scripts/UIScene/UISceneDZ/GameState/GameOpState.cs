using SDK.Common;

namespace Game.UI
{
    public enum EnGameOp
    {
        eOpNone,        // 没进行任何操作
        eOpAttack,      // 攻击操作
    }

    /**
     * @brief 游戏当前操作状态，仅处理场景中交互的内容，如果触摸到不交互的场景的内容，不打断场景逻辑
     */
    public class GameOpState : IGameOpState
    {
        protected SceneDZData m_sceneDZData;

        protected EnGameOp m_curOp;         // 当前操作
        protected SceneDragCard m_opCard;      // 当前操作的卡牌

        public GameOpState(SceneDZData sceneDZData)
        {
            m_curOp = EnGameOp.eOpNone;
            m_sceneDZData = sceneDZData;
        }

        // 进入攻击操作
        public void enterAttackOp(EnGameOp op, SceneDragCard card)
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

        public bool canAttackOp(SceneDragCard card)
        {
            if (m_opCard != null)
            {
                if (m_opCard.sceneCardItem.m_playerFlag != card.sceneCardItem.m_playerFlag && card.sceneCardItem.m_svrCard.awake == 1)
                {
                    return true;
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