﻿namespace SDK.Lib
{
    public class AnimWalkFS : FSMState
    {
        public AnimWalkFS(FSM fsm, BeingEntity beingEntity)
            : base(fsm, beingEntity)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            this.mEntity.mAnimatorControl.play(CVAnimState.Walk);
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