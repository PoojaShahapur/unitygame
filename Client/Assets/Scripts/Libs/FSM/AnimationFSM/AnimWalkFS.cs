using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public class AnimWalkFS : FSMState
    {
        //public AnimWalkFS(FSM fsm, BeingEntity beingEntity)
        //    : base(fsm, beingEntity)
        //{

        //}

        public AnimWalkFS(FSM fsm, SceneCardBase card)
            : base(fsm, card)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();
        }

        override public void OnStateExit()
        {

        }

        override public void Update()
        {
            base.Update();
        }

        public override void OnDrawGizmos()
        {
            //Gizmos.DrawLine(m_sceneGo.transform.position, m_sceneGo.transform.forward * maxChaseDistance);
        }
    }
}