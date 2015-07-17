namespace SDK.Lib
{
    /**
     * @brief 被击序列中的 Item 
     */
    public class HurtSeqItem
    {
        protected HurtActionNode m_hurtActionNode;      // 被击动作节点

        public HurtActionNode hurtActionNode
        {
            get
            {
                return m_hurtActionNode;
            }
            set
            {
                m_hurtActionNode = value;
            }
        }
    }
}