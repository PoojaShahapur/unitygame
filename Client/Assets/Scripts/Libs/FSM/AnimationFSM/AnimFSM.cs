using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public class AnimFSM : FSM
    {
        //private BeingEntity m_beingEntity;
        protected SceneCardBase m_card;

        //public BeingEntity beingEntity
        //{
        //    get
        //    {
        //        return m_beingEntity;
        //    }
        //    set
        //    {
        //        m_beingEntity = value;
        //    }
        //}

        public SceneCardBase card
        {
            get
            {
                return m_card;
            }
            set
            {
                m_card = value;
            }
        }

        public override void InitFSM()
        {
            base.InitFSM();
        }

        public override void UpdateFSM()
        {
            base.UpdateFSM();
        }

        protected override FSMState CreateState(StateId state)
        {
            switch (state.GetId())
            {
                case CVAnimState.Idle:
                {
                    //return new AnimIdleFS(this, m_beingEntity);
                    return new AnimIdleFS(this, m_card);
                }
                case CVAnimState.Walk:
                {
                    //return new AnimWalkFS(this, m_beingEntity);
                    return new AnimWalkFS(this, m_card);
                }
                case CVAnimState.Run:
                {
                    //return new AnimRunFS(this, m_beingEntity);
                    return new AnimRunFS(this, m_card);
                }
                default:
                    return null;
            }
        }

        public override void StopFSM()
        {
            base.StopFSM();
        }

        void OnDrawGizmos()
        {
            if (currentState != null)
            {
                currentState.OnDrawGizmos();
            }
        }
    }
}