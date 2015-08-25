using SDK.Lib;

namespace SDK.Lib
{
    /**
     * @brief 一次攻击连招序列
     */
    public class OneAttackFlowSeq
    {
        protected MList<AttackSeqItem> m_attackItemList;
        protected ImmeAttackItemBase m_attackItem;

        public OneAttackFlowSeq()
        {
            m_attackItemList = new MList<AttackSeqItem>();
        }

        public ImmeAttackItemBase attackItem
        {
            get
            {
                return m_attackItem;
            }
            set
            {
                m_attackItem = value;
            }
        }

        // 创建攻击流程序列
        public void initAttackFlowSeq(AttackActionSeq actionSeq)
        {
            AttackSeqItem seqItem = null;
            foreach(var actionItem in actionSeq.itemList.list)
            {
                seqItem = new AttackSeqItem(this);
                m_attackItemList.Add(seqItem);
                seqItem.attackActionNode = actionItem.attackActionNode;
            }
        }

        public void onTime(float delta)
        {
            foreach (var item in m_attackItemList.list)
            {
                item.onTime(delta);
            }
        }
    }
}