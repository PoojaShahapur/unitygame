namespace BehaviorLibrary.Components.Actions
{
    /**
     * @brief 巡查
     */
    public class BehaviorActionPatrol : BehaviorAction
    {
        public BehaviorActionPatrol()
            : base(null)
        {
            base.action = this.behaviorPatrol;
        }

        protected BehaviorReturnCode behaviorPatrol()
        {
            return BehaviorReturnCode.Success;
        }
    }
}
