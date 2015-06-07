using FightCore;

namespace FSM
{
    public class FSMSceneState : FSMState
    {
        public FSMSceneState(FSM fsm) :
            base(fsm)
        {

        }

        public SceneCardBase card
        {
            get
            {
                return (mFSM as SceneFSMBase).card;
            }
        }
    }
}