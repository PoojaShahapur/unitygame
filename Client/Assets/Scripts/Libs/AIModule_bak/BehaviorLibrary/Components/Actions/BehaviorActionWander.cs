using SDK.Common;
using SDK.Lib;
using UnitySteer.Behaviors;

namespace BehaviorLibrary.Components.Actions
{
    /**
     * @brief 徘徊
     */
    public class BehaviorActionWander : BehaviorAction
    {
        public BehaviorActionWander()
            : base(null)
        {
            base.actionFunc = onExecAction;
        }

        protected BehaviorReturnCode onExecAction()
        {
            toggleBehavior(BehaviorState.BSWander);
            //m_behaviorTree.inputParam.beingEntity.aiController.vehicle.Update();
            return BehaviorReturnCode.Success;
        }
    }
}