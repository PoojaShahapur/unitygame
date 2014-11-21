using UnityEngine;
using System.Collections;
using AIEngine;
using SDK.Lib;
using SDK.Common;

public class AnimIdleFS : FSMState 
{
    public AnimIdleFS(FSM fsm, BeingEntity beingEntity)
        : base(fsm, beingEntity)
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