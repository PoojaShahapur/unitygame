using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public class HurtedFS : FSMState
    {
        public HurtedFS(FSM fsm, SceneCardBase card)
            : base(fsm, card)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            m_card.fightData.hurtData.endCurItem();
            mFSM.MoveToState(SceneStateId.SSInplace);
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