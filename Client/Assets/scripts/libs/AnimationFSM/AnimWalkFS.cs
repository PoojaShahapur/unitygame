using UnityEngine;
using System.Collections;
using AIEngine;
using SDK.Lib;

public class AnimWalkFS : FSMState 
{
    public AnimWalkFS(FSM fsm, BeingEntity beingEntity)
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
        //Gizmos.DrawLine(m_sceneGo.transform.position, m_sceneGo.transform.forward * maxChaseDistance);
    }
}