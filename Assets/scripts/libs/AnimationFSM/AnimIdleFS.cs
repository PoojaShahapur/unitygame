using UnityEngine;
using System.Collections;
using AIEngine;
using SDK.Lib;

public class AnimIdleFS : FSMState 
{
    public AnimIdleFS(FSM fsm, BeingEntity beingEntity)
        : base(fsm, beingEntity)
    {
        
    }

    override public void OnStateEnter()
    {
        idle();
    }

    override public void OnStateExit()
    {
        
    }

    override public void Update()
    {
        //float distance = Vector3.Distance(mPlayer.transform.position, mAgent.transform.position);

        //if (distance > maxDistanceFromTarget)
        //{
        //    mFSM.MoveToState(EnemyStateId.Chase);
        //}
        //else
        //{
        //    Vector3 point = mPlayer.transform.position;
        //    point.y = mAgent.transform.position.y;
        //    mAgent.transform.LookAt(point);

        //    mAttackTimeCounter += Time.deltaTime;

        //    if (mAttackTimeCounter > 2.0f)
        //    {
        //        Attack();
        //        mAttackTimeCounter = 0.0f;
        //    }
        //}
    }

    private void idle()
    {
        // 播放空闲动画
    }

    public override void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(m_sceneGo.transform.position, 10);
    }
}