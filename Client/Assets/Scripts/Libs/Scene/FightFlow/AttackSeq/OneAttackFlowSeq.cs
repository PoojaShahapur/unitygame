using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 一次攻击连招序列
     */
    public class OneAttackFlowSeq
    {
        protected MList<AttackSeqItem> m_attackItemList;

        public OneAttackFlowSeq()
        {
            m_attackItemList = new MList<AttackSeqItem>();
        }

        // 创建攻击流程序列
        public void initAttackFlowSeq(AttackActionSeq actionSeq)
        {
            AttackSeqItem seqItem = null;
            foreach(var actionItem in actionSeq.itemList.list)
            {
                seqItem = new AttackSeqItem();
                m_attackItemList.Add(seqItem);
                seqItem.attackActionNode = actionItem.attackActionNode;
            }
        }
    }
}