using UnityEngine;
using System.Collections;
using SDK.Lib;

namespace AIEngine
{
    public abstract class FSMState
    {
        protected FSMState(FSM fsm, BeingEntity beingEntity)
        {
            mFSM = fsm;
            m_beingEntity = beingEntity;
        }

        abstract public void OnStateEnter();
        abstract public void OnStateExit();
        abstract public void Update();
        abstract public void OnDrawGizmos();

        protected FSM mFSM;
        protected BeingEntity m_beingEntity;
    }
}