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

        public void init(InsParam inputParam)
        {
            inputParam.beingEntity.vehicle.Steerings[0] = new SteerForWander();
            inputParam.beingEntity.vehicle.Steerings[0].Vehicle = inputParam.beingEntity.vehicle as Vehicle;
        }

        protected BehaviorReturnCode onExecAction(InsParam inputParam)
        {
            inputParam.beingEntity.vehicle.Update();
            return BehaviorReturnCode.Success;
        }
    }
}