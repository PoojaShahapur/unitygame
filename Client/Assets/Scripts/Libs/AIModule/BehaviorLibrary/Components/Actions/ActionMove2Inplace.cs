namespace BehaviorLibrary
{
    /**
     * @brief 从目标返回原地动作
     */
    public class ActionMove2Inplace : Action
    {
        public ActionMove2Inplace()
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