using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;

namespace FSM
{
    public class AnimIdleFS : FSMState
    {
        //public AnimIdleFS(FSM fsm, BeingEntity beingEntity)
        //    : base(fsm, beingEntity)
        //{

        //}

        public AnimIdleFS(FSM fsm, SceneCardBase card)
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
        }
    }
}