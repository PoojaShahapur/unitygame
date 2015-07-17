using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 一次被击连招序列
     */
    public class OneHurtFlowSeq
    {
        protected MList<HurtSeqItem> m_hurtSeqItem;

        public OneHurtFlowSeq()
        {
            m_hurtSeqItem = new MList<HurtSeqItem>();
        }

        // 创建攻击流程序列
        public void initHurtFlowSeq(AttackActionSeq actionSeq)
        {
            HurtSeqItem seqItem = null;
            foreach(var actionItem in actionSeq.itemList.list)
            {
                seqItem = new HurtSeqItem();
                m_hurtSeqItem.Add(seqItem);
                if (actionItem.hurtActionNode != null)
                {
                    seqItem.hurtActionNode = actionItem.hurtActionNode;
                }
            }
        }
    }
}