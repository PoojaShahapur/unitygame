namespace SDK.Lib
{
    /**
     * @brief 所有的局部状态
     */
    public class AILocalState
    {
        protected BehaviorState m_behaviorState = BehaviorState.BSIdle;        // 当前行为状态
        protected BeingState m_beingState = BeingState.BSIdle;          // 状态
        protected BeingSubState m_beingSubState;    // 子状态

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

        public BeingState beingState
        {
            get
            {
                return m_beingState;
            }
            set
            {
                m_beingState = value;
            }
        }

        public BeingSubState beingSubState
        {
            get
            {
                return m_beingSubState;
            }
            set
            {
                m_beingSubState = value;
            }
        }
    }
}