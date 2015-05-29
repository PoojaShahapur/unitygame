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
            return BehaviorReturnCode.Success;
        }
    }
}