﻿namespace SDK.Lib
{
    /**
     * @breif 攻击序列中的一个 Item
     */
    public class AttackSeqItem
    {
        protected AttackActionNode m_attackActionNode;      // 攻击动作节点

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
    }
}