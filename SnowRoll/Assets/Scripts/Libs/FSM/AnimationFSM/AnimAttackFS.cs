namespace SDK.Lib
{
    public class AnimAttackFS : FSMState
    {
        public AnimAttackFS(FSM fsm, BeingEntity beingEntity)
            : base(fsm, beingEntity)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            if (null != this.mEntity.mAnimatorControl)
            {
                this.mEntity.mAnimatorControl.play(CVAnimState.Attack);
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