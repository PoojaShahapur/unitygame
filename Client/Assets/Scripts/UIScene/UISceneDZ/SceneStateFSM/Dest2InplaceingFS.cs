using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public class Dest2InplaceingFS : FSMState
    {
        public Dest2InplaceingFS(FSM fsm, SceneCardBase card)
            : base(fsm, card)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            m_card.behaviorControl.playAttackAni(m_card.transform().localPosition, m_card.behaviorControl.srcPos, m_card.behaviorControl.onMove2InplaceEnd);
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