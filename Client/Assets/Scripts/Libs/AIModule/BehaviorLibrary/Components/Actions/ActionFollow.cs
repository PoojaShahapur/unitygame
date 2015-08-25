using SDK.Lib;

namespace BehaviorLibrary
{
    /**
     * @brief 跟随动作
     */
    public class ActionFollow : Action
    {
        public ActionFollow()
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