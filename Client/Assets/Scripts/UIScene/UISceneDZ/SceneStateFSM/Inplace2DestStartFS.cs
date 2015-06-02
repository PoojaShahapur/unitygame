using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public class Inplace2DestStartFS : FSMState
    {
        public Inplace2DestStartFS(FSM fsm, SceneCardBase card)
            : base(fsm, card)
        {

        }

        override public void OnStateEnter()
        {
            base.OnStateEnter();

            m_card.behaviorControl.srcPos = m_card.transform().localPosition;
            // 获取一项攻击数值
            m_card.fightData.attackData.getNextItem();
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