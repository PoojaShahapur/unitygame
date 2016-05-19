using FightCore;
using Game.UI;

namespace BehaviorLibrary
{
    public class ConditionSceneStateEqual : Condition
    {
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
                
            }

            return false;
        }
    }
}