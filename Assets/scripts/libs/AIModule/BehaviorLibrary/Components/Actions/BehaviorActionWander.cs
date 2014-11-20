using SDK.Common;
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

        public override void onEnter()
        {
            m_behaviorTree.inputParam.beingEntity.aiController.vehicle.Steerings[0] = new SteerForWander();
            (m_behaviorTree.inputParam.beingEntity.aiController.vehicle.Steerings[0] as SteerForWander).MaxLatitudeSide = 100;
            (m_behaviorTree.inputParam.beingEntity.aiController.vehicle.Steerings[0] as SteerForWander).MaxLatitudeUp = 100;
            m_behaviorTree.inputParam.beingEntity.aiController.vehicle.Steerings[0].Vehicle = m_behaviorTree.inputParam.beingEntity.aiController.vehicle as Vehicle;
        }

        protected BehaviorReturnCode onExecAction()
        {
            if (m_behaviorTree.inputParam.beingEntity.aiController.aiLocalState.behaviorState != BehaviorState.BSWander)
            {
                m_behaviorTree.inputParam.beingEntity.aiController.aiLocalState.behaviorState = BehaviorState.BSWander;
                onEnter();
            }
            m_behaviorTree.inputParam.beingEntity.aiController.vehicle.Update();
            return BehaviorReturnCode.Success;
        }
    }
}