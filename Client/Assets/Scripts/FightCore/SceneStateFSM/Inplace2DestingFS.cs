using UnityEngine;
using System.Collections;
using SDK.Lib;
using FightCore;

namespace FSM
{
    public class Inplace2DestingFS : FSMSceneState
    {
        public Inplace2DestingFS(FSM fsm, SceneCardBase card)
            : base(fsm)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            if (card.fightData.attackData.curAttackItem.attackType == EAttackType.eCommon)         // 普通攻击需要移动过去
            {
                SceneCardBase hurtCard = Ctx.m_instance.m_sceneCardMgr.getCardByThisId(card.fightData.attackData.curAttackItem.getHurterId());
                card.moveControl.moveToDest(card.behaviorControl.srcPos, hurtCard.transform().localPosition, card.fightData.attackData.curAttackItem.getMoveTime(), card.behaviorControl.onMove2DestEnd);
            }
            else    // 技能攻击不需要移动过去
            {
                mFSM.MoveToState(SceneStateId.SSInplace2Dested);
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