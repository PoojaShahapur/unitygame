using SDK.Lib;
using UnitySteer.Behaviors;

namespace BehaviorLibrary
{
    /**
     * @brief 四处徘徊
     */
    public class ActionWander : Action
    {
        public ActionWander()
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