namespace BehaviorLibrary.Components.Actions
{
    /**
     * @brief 徘徊
     */
    public class BehaviorActionPatrol : BehaviorAction
    {
        public BehaviorActionPatrol()
            : base(null)
        {
            
        }

        public override BehaviorReturnCode Behave(InsParam inputParam)
        {

            return BehaviorReturnCode.Success;
        }
    }
}
