using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;

namespace FSM
{
    public class Inplace2DestedFS : FSMSceneState
    {
        public Inplace2DestedFS(FSM fsm, SceneCardBase card)
            : base(fsm)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            mFSM.MoveToState(SceneStateId.SSAttackStart);
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