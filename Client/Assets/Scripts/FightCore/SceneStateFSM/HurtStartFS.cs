using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;

namespace FSM
{
    public class HurtStartFS : FSMSceneState
    {
        public HurtStartFS(FSM fsm, SceneCardBase card)
            : base(fsm)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            card.fightData.hurtData.getNextItem();
            mFSM.MoveToState(SceneStateId.SSHurting);
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