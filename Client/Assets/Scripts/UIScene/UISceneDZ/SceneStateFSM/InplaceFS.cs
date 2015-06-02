﻿using UnityEngine;
using System.Collections;
using SDK.Lib;
using SDK.Common;
using Game.UI;

namespace FSM
{
    public class InplaceFS : FSMState
    {
        public InplaceFS(FSM fsm, SceneCardBase card)
            : base(fsm, card)
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
    }
}