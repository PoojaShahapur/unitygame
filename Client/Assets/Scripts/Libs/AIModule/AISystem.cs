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
        protected AIControllerMgr m_aiControllerMgr;

        public AISystem()
        {
            m_behaviorTreeMgr = new BehaviorTreeMgr();
            m_aiControllerMgr = new AIControllerMgr();
        }

        public BehaviorTreeMgr behaviorTreeMgr
        {
            get
            {
                return m_behaviorTreeMgr;
            }
        }

        public AIControllerMgr aiControllerMgr
        {
            get
            {
                return m_aiControllerMgr;
            }
        }
    }
}