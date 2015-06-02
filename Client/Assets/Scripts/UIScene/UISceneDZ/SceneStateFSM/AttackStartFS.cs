using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public class AttackStartFS : FSMState
    {
        public AttackStartFS(FSM fsm, SceneCardBase card)
            : base(fsm, card)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            mFSM.MoveToState(SceneStateId.SSAttacking);
        }

        override public void OnStateExit()
        {

        }

        override public void Update()
        {
            base.Update();
        }
    }
}