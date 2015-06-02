using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    /**
     * Extend this class to define your own state machine.
     * */
    public abstract class FSM
    {
        //public Renderer meshRenderer;
        public StateId initialState;
        public bool updateOnlyWhenVisible = true;

        protected StateId m_preStateId;
        protected StateId m_curStateId;

        protected FSMState preState;            // 前一个状态
        protected FSMState currentState;        // 当前状态

        public void Start()
        {
            InitFSM();
        }

        public void Update()
        {
            UpdateFSM();
        }

        public void MoveToState(StateId state)
        {
            currentState.OnStateExit();
            m_preStateId = m_curStateId;
            m_curStateId = state;
            preState = currentState;
            currentState = CreateState(state);
            currentState.OnStateEnter();
        }

        protected abstract FSMState CreateState(StateId state);

        public virtual void StopFSM()
        {
            currentState = null;
        }

        public virtual void InitFSM()
        {
            currentState = CreateState(AnimStateId.ASIDLE);
        }

        public virtual void UpdateFSM()
        {
            if (currentState != null)
            {
                if (updateOnlyWhenVisible)
                {
                    //if (meshRenderer.isVisible)
                    //{
                        currentState.Update();
                    //}
                }
                else
                {
                    currentState.Update();
                }
            }
        }

        public bool equalCurState(StateId state)
        {
            return m_curStateId.GetId() == state.GetId();
        }
    }
}