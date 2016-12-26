namespace SDK.Lib
{
    public class AnimRunFS : FSMState
    {
        public AnimRunFS(FSM fsm, BeingEntity beingEntity)
            : base(fsm, beingEntity)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            this.mEntity.mAnimatorControl.play(CVAnimState.Run);
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