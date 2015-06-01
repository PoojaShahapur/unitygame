namespace BehaviorLibrary
{
    /**
     * @brief 攻击动作
     */
    public class ActionAttack : Action
    {
        public ActionAttack()
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
