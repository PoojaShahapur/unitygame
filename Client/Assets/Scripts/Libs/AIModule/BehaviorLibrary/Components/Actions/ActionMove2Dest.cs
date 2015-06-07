using FightCore;
using Game.UI;
using SDK.Common;

namespace BehaviorLibrary
{
    /**
     * @brief 移动到目标动作
     */
    public class ActionMove2Dest : Action
    {
        public ActionMove2Dest()
            : base(null)
        {
            base.actionFunc = onExecAction;
        }

        protected BehaviorReturnCode onExecAction()
        {
            SceneCardBase attackCard = behaviorTree.blackboardData.GetData(BlackboardKey.PSCARD) as SceneCardBase;

            return BehaviorReturnCode.Success;
        }
    }
}