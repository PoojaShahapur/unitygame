namespace SDK.Lib
{
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
    }
}