﻿using UnityEngine;
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

        protected FSMState currentState;

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
    }
}