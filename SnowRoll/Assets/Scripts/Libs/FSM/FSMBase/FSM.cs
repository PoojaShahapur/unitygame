namespace SDK.Lib
{
    public abstract class FSM
    {
        public StateId mInitState;
        public bool mUpdateOnlyWhenVisible;

        protected StateId mPreStateId;
        protected StateId mCurStateId;

        protected FSMState mPreState;            // 前一个状态
        protected FSMState mCurState;        // 当前状态

        public FSM()
        {
            mUpdateOnlyWhenVisible = true;
        }

        virtual public void init()
        {
            InitFSM();
        }

        virtual public void dispose()
        {

        }

        public void Update()
        {
            UpdateFSM();
        }

        public void MoveToState(StateId state)
        {
            mCurState.OnStateExit();
            mPreStateId = mCurStateId;
            mCurStateId = state;
            mPreState = mCurState;
            mCurState = CreateState(state);
            mCurState.OnStateEnter();
        }

        protected abstract FSMState CreateState(StateId state);

        public virtual void StopFSM()
        {
            mCurState = null;
        }

        public virtual void InitFSM()
        {
            mCurStateId = AnimStateId.ASIDLE;
            mCurState = CreateState(AnimStateId.ASIDLE);
        }

        public virtual void UpdateFSM()
        {
            if (null != mCurState)
            {
                if (mUpdateOnlyWhenVisible)
                {
                    //if (meshRenderer.isVisible)
                    //{
                        mCurState.OnUpdate();
                    //}
                }
                else
                {
                    mCurState.OnUpdate();
                }
            }
        }

        public bool equalCurState(StateId state)
        {
            return mCurStateId.GetId() == state.GetId();
        }
    }
}