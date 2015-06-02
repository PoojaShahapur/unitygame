using UnityEngine;
using System.Collections;
using SDK.Lib;
using Game.UI;

namespace FSM
{
    public class SceneStateFSM : FSM
    {
        protected SceneCardBase m_card;

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
            currentState = CreateState(SceneStateId.SSInplace);
        }

        public override void UpdateFSM()
        {
            //base.UpdateFSM();
        }

        protected override FSMState CreateState(StateId state)
        {
            switch (state.GetId())
            {
                case (int)SceneState.eInplace:
                {
                    return new InplaceFS(this, m_card);
                }
                case (int)SceneState.eInplace2DestStart:
                {
                    return new Inplace2DestStartFS(this, m_card);
                }
                case (int)SceneState.eInplace2Desting:
                {
                    return new Inplace2DestingFS(this, m_card);
                }
                case (int)SceneState.eInplace2Dested:
                {
                    return new Inplace2DestedFS(this, m_card);
                }
                case (int)SceneState.eAttackStart:
                {
                    return new AttackStartFS(this, m_card);
                }
                case (int)SceneState.eAttacking:
                {
                    return new AttackingFS(this, m_card);
                }
                case (int)SceneState.eAttacked:
                {
                    return new AttackedFS(this, m_card);
                }
                case (int)SceneState.eDest2InplaceStart:
                {
                    return new Dest2InplaceStartFS(this, m_card);
                }
                case (int)SceneState.eDest2Inplaceing:
                {
                    return new Dest2InplaceingFS(this, m_card);
                }
                case (int)SceneState.eDest2Inplaced:
                {
                    return new Dest2InplacedFS(this, m_card);
                }
                case (int)SceneState.eHurtStart:
                {
                    return new HurtStartFS(this, m_card);
                }
                case (int)SceneState.eHurting:
                {
                    return new HurtingFS(this, m_card);
                }
                case (int)SceneState.eHurted:
                {
                    return new HurtedFS(this, m_card);
                }
                default:
                {
                    return null;
                }
            }
        }

        public override void StopFSM()
        {
            base.StopFSM();
        }
    }
}