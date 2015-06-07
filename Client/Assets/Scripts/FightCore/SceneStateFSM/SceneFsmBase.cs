using FightCore;

namespace FSM
{
    public class SceneFSMBase : FSM
    {
        protected SceneCardBase m_card;

        public SceneFSMBase(SceneCardBase card)
        {
            m_card = card;
        }

        public SceneCardBase card
        {
            get
            {
                return m_card;
            }
        }

        protected override FSMState CreateState(StateId state)
        {
            return null;
        }
    }
}