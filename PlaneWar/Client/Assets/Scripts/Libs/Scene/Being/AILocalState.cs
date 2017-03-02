namespace SDK.Lib
{
    /**
     * @brief 所有的局部状态
     */
    public class AILocalState
    {
        protected BehaviorState mBehaviorState;        // 当前行为状态
        protected BeingState mBeingState;          // 状态
        protected BeingSubState mBeingSubState;    // 子状态

        public AILocalState()
        {
            this.mBehaviorState = BehaviorState.eBSIdle;
            this.mBeingState = BeingState.eBSIdle;
        }

        public BehaviorState behaviorState
        {
            get
            {
                return mBehaviorState;
            }
            set
            {
                mBehaviorState = value;
            }
        }

        public BeingState beingState
        {
            get
            {
                return mBeingState;
            }
            set
            {
                mBeingState = value;
            }
        }

        public BeingSubState beingSubState
        {
            get
            {
                return mBeingSubState;
            }
            set
            {
                mBeingSubState = value;
            }
        }
    }
}