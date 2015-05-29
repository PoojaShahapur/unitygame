using BehaviorLibrary;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief AI 所有数据结构都存放在这里
     */
    public class AISystem
    {
        protected BehaviorTreeMgr m_behaviorTreeMgr;

        public AISystem()
        {
            m_behaviorTreeMgr = new BehaviorTreeMgr();
        }

        public BehaviorTreeMgr behaviorTreeMgr
        {
            get
            {
                return m_behaviorTreeMgr;
            }
        }
    }
}