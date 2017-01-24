namespace SDK.Lib
{
    public class AnimIdleFS : FSMState
    {
        public AnimIdleFS(FSM fsm, BeingEntity beingEntity)
            : base(fsm, beingEntity)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            if (null != this.mEntity.mAnimatorControl)
            {
                this.mEntity.mAnimatorControl.play(CVAnimState.Idle);
            }
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