using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief AI 所有的局部状态
     */
    public class AILocalState
    {
        protected BehaviorState m_behaviorState = BehaviorState.BSIdle;        // 当前行为状态

        public BehaviorState behaviorState
        {
            get
            {
                return m_behaviorState;
            }
            set
            {
                m_behaviorState = value;
            }
        }
    }
}