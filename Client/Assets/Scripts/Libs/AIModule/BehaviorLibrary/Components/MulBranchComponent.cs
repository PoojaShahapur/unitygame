using System.Collections.Generic;

namespace BehaviorLibrary
{
    public class MulBranchComponent : BehaviorComponent
    {
        protected List<BehaviorComponent> m_childBehaviorsList;     // 孩子节点

        // 添加子节点
        public override void addChild(BehaviorComponent child)
        {
            if(m_childBehaviorsList == null)
            {
                m_childBehaviorsList = new List<BehaviorComponent>();
            }

            m_childBehaviorsList.Add(child);
        }

        protected void execAllChild()
        {
            foreach (BehaviorComponent cmt in m_childBehaviorsList)
            {
                cmt.Behave();
            }
        }
    }
}