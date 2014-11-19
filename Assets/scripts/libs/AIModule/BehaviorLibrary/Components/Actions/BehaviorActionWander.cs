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
            inputParam.m_beingEntity.vehicle.Steerings[0] = new SteerForWander();
            inputParam.m_beingEntity.vehicle.Steerings[0].Vehicle = inputParam.m_beingEntity.vehicle as Vehicle;
        }

        protected BehaviorReturnCode onExecAction(InsParam inputParam)
        {
            inputParam.m_beingEntity.vehicle.Update();
            return BehaviorReturnCode.Success;
        }

        public void exit()
        {

        }
    }
}