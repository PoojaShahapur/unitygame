using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public class AttackedFS : FSMState
    {
        public AttackedFS(FSM fsm, SceneCardBase card)
            : base(fsm, card)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            m_card.fightData.attackData.endCurItem();
            mFSM.MoveToState(SceneStateId.SSDest2InplaceStart);
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