using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;

namespace FSM
{
    public class HurtingFS : FSMSceneState
    {
        public HurtingFS(FSM fsm, SceneCardBase card)
            : base(fsm)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            // 播放受伤动画和特效
            card.fightData.hurtData.execCurItem(card);
            mFSM.MoveToState(SceneStateId.SSHurted);
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