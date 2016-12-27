namespace SDK.Lib
{
    // 如果要自定义 int Compare(T x, T y); 继承 IComparer ，如果需要一个默认实现，继承 Comparer
    //public abstract class FSMState : System.Collections.Generic.IComparer<FSMState>
    //public abstract class FSMState : System.Collections.Generic.Comparer<FSMState>
    public abstract class FSMState
    {
        protected FSM mFSM;
        protected BeingEntity mEntity;

        public FSMState(FSM fsm, BeingEntity beingEntity)
        {
            mFSM = fsm;
            mEntity = beingEntity;
        }

        protected FSMState(FSM fsm)
        {
            mFSM = fsm;
        }

        virtual public void OnStateEnter()
        {

        }

        virtual public void OnStateExit()
        {

        }

        virtual public void OnUpdate()
        {
            
        }

        //override public int Compare(FSMState x, FSMState y)
        //{
        //    return 0;
        //}
    }
}