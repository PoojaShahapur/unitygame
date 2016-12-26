namespace SDK.Lib
{
    public class AnimSplitFS : FSMState
    {
        public AnimSplitFS(FSM fsm, BeingEntity beingEntity)
            : base(fsm, beingEntity)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            this.mEntity.mAnimatorControl.play(CVAnimState.Split);
        }

        override public void OnStateExit()
        {

        }

        override public void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}