using SDK.Common;
using SDK.Lib;

namespace BehaviorLibrary.Components.Actions
{
    public class BehaviorActionFollow : BehaviorAction
    {
        public BehaviorActionFollow()
            : base(null)
        {
            base.actionFunc = onExecAction;
        }

        protected BehaviorReturnCode onExecAction()
        {
            toggleBehavior(BehaviorState.BSFollow);
            //m_behaviorTree.inputParam.beingEntity.aiController.vehicle.Update();
            return BehaviorReturnCode.Success;
        }
    }
}