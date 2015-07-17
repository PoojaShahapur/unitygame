namespace SDK.Lib
{
    /**
     * @breif 攻击序列中的一个 Item
     */
    public class AttackSeqItem
    {
        protected AttackActionNode m_attackActionNode;      // 攻击动作节点
        protected OneAttackFlowSeq m_attackSeq;

        public AttackSeqItem(OneAttackFlowSeq attackSeq_)
        {
            m_attackSeq = attackSeq_;
        }

        public AttackActionNode attackActionNode
        {
            get
            {
                return m_attackActionNode;
            }
            set
            {
                m_attackActionNode = value;
            }
        }

        public void onTime(float delta)
        {
            
        }
    }
}