using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;

namespace FSM
{
    public class Dest2InplaceingFS : FSMSceneState
    {
        public Dest2InplaceingFS(FSM fsm, SceneCardBase card)
            : base(fsm)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            if (card.fightData.attackData.curAttackItem.attackType == EAttackType.eCommon)
            {
                card.moveControl.moveToDest(card.transform().localPosition, card.behaviorControl.srcPos, card.fightData.attackData.curAttackItem.getMoveTime(), card.behaviorControl.onMove2InplaceEnd);
            }
            else
            {
                mFSM.MoveToState(SceneStateId.SSDest2Inplaced);
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