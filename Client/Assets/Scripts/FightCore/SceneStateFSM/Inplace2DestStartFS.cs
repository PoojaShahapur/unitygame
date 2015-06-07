using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;

namespace FSM
{
    public class Inplace2DestStartFS : FSMSceneState
    {
        public Inplace2DestStartFS(FSM fsm, SceneCardBase card)
            : base(fsm)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            card.behaviorControl.srcPos = card.transform().localPosition;
            // 获取一项攻击数值
            card.fightData.attackData.getNextItem();
            mFSM.MoveToState(SceneStateId.SSInplace2Desting);
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