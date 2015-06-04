using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;
using SDK.Common;

namespace FSM
{
    public class Inplace2DestingFS : FSMState
    {
        public Inplace2DestingFS(FSM fsm, SceneCardBase card)
            : base(fsm, card)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            SceneCardBase hurtCard = Ctx.m_instance.m_sceneCardMgr.getCard(m_card.fightData.attackData.curAttackItem.getHurterId());
            m_card.behaviorControl.moveToDest(m_card.behaviorControl.srcPos, hurtCard.transform().localPosition, m_card.behaviorControl.onMove2DestEnd);
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