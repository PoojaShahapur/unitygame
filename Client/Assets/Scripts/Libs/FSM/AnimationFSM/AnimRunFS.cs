using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public class AnimRunFS : FSMState
    {
        //public AnimRunFS(FSM fsm, BeingEntity beingEntity)
        //    : base(fsm, beingEntity)
        //{

        //}

        public AnimRunFS(FSM fsm, SceneCardBase card)
            : base(fsm)
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
            //Gizmos.DrawWireSphere(m_sceneGo.transform.position, 10);
            //Gizmos.DrawRay(m_sceneGo.transform.position, m_sceneGo.transform.forward * 10);
        }
    }
}