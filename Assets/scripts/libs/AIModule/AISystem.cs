using BehaviorLibrary;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief AI 所有数据结构都存放在这里
     */
    public class AISystem : IAISystem
    {
        protected IBehaviorTreeMgr m_behaviorTreeMgr = new BehaviorTreeMgr();

        public IBehaviorTreeMgr getBehaviorTreeMgr()
        {
            return m_behaviorTreeMgr;
        }
    }
}