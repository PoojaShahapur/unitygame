using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public class HurtingFS : FSMState
    {
        public HurtingFS(FSM fsm, SceneCardBase card)
            : base(fsm, card)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            // 播放受伤动画和特效
            m_card.fightData.hurtData.execCurItem(m_card);
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