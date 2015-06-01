using Game.UI;

namespace BehaviorLibrary
{
    public class ConditionSceneStateEqual : Condition
    {
        protected CardSceneState m_sceneState;

        public ConditionSceneStateEqual()
            : base(null)
        {
            boolFunc = onExecBoolFunc;
        }

        protected bool onExecBoolFunc()
        {
            SceneCardBase card = behaviorTree.blackboardData.GetData(BlackboardKey.PSCARD) as SceneCardBase;
            if(card != null)
            {
                return card.behaviorControl.cardSceneState == m_sceneState;
            }

            return false;
        }

        public CardSceneState sceneState
        {
            get
            {
                return m_sceneState;
            }
            set
            {
                m_sceneState = value;
            }
        }
    }
}