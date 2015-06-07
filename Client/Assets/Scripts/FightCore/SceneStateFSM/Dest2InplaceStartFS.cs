using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;

namespace FSM
{
    public class Dest2InplaceStartFS : FSMSceneState
    {
        public Dest2InplaceStartFS(FSM fsm, SceneCardBase card)
            : base(fsm)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();
            mFSM.MoveToState(SceneStateId.SSDest2Inplaceing);
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