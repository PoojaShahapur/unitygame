using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;

namespace FSM
{
    public class Dest2InplacedFS : FSMSceneState
    {
        public Dest2InplacedFS(FSM fsm, SceneCardBase card)
            : base(fsm)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            card.fightData.attackData.endCurItem();
            mFSM.MoveToState(SceneStateId.SSInplace);

            // 检查是否已经因为攻击死亡
            if (card.canDelFormClient())
            {
                card.delSelf();
            }
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