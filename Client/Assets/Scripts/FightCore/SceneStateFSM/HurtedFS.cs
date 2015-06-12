using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;

namespace FSM
{
    public class HurtedFS : FSMSceneState
    {
        public HurtedFS(FSM fsm, SceneCardBase card)
            : base(fsm)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            card.fightData.hurtData.endCurItem();
            mFSM.MoveToState(SceneStateId.SSInplace);

            // 检查是否已经因为受伤死亡
            //if(card.canDelFormClient())
            //{
            //    card.delSelf();
            //}
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