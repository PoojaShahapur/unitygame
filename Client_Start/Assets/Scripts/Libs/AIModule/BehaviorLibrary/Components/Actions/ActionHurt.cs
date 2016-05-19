namespace BehaviorLibrary
{
    /**
     * @brief 受伤动作
     */
    public class ActionHurt : Action
    {
        public ActionHurt()
            : base(null)
        {
            base.actionFunc = onExecAction;
        }

        protected BehaviorReturnCode onExecAction()
        {
            return BehaviorReturnCode.Success;
        }
    }
}