using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;
using SDK.Common;

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

            // 播放攻击者掉血特效
            card.behaviorControl.playAttackHurt(card.fightData.attackData.curAttackItem);
            card.fightData.attackData.endCurItem();
            mFSM.MoveToState(SceneStateId.SSInplace);
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