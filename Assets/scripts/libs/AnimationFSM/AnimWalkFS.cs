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
        
    }

    override public void OnStateExit()
    {
        
    }

    override public void Update()
    {
        //float distance = Vector3.Distance(mPlayer.transform.position, mAgent.transform.position);

        //if (distance < maxAttackDistance)
        //{
        //    mFSM.MoveToState(EnemyStateId.Attack);
        //} 
        //else if (mEnemyNavAgent.remainingDistance < 2.0f)
        //{
        //    if(distance < 10.0f)
        //    {
        //        mEnemyNavAgent.SetDestination(mPlayer.transform.position);
        //    }
        //    else
        //    {
        //        mFSM.MoveToState(EnemyStateId.Patrol);
        //    }
        //}
    }

    public override void OnDrawGizmos()
    {
        //Gizmos.DrawLine(m_sceneGo.transform.position, m_sceneGo.transform.forward * maxChaseDistance);
    }
}