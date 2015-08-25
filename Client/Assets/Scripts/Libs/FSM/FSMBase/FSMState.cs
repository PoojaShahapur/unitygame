using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public abstract class FSMState
    {
        protected FSM mFSM;
        //protected BeingEntity m_beingEntity;

        //protected FSMState(FSM fsm, BeingEntity beingEntity)
        //{
        //    mFSM = fsm;
        //    m_beingEntity = beingEntity;
        //}

        protected FSMState(FSM fsm)
        {
            mFSM = fsm;
        }

        virtual public void OnStateEnter()
        {
            //m_beingEntity.skinAniModel.animSys.playAnim(UtilApi.convBeingState2ActState(m_beingEntity.aiLocalState.beingState, m_beingEntity.aiLocalState.beingSubState));
        }

        virtual public void OnStateExit()
        {

        }

        virtual public void Update()
        {
            //BeingState beingState = UtilApi.convBehaviorState2BeingState(m_beingEntity.aiLocalState.behaviorState);
            //if (beingState != m_beingEntity.aiLocalState.beingState)        // 行为状态改变，需要改变生物状态
            //{
            //    mFSM.MoveToState(AnimStateId.getStateIdByBeingState(m_beingEntity.aiLocalState.beingState));
            //}
        }

        virtual public void OnDrawGizmos()
        {

        }
    }
}