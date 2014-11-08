﻿using UnitySteer.Behaviors;

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

        public void init(InsParam inputParam)
        {
            inputParam.m_beingEntity.vehicle.Steerings[0] = new SteerForWander();
            inputParam.m_beingEntity.vehicle.Steerings[0].Vehicle = inputParam.m_beingEntity.vehicle as Vehicle;
        }

        public override BehaviorReturnCode Behave(InsParam inputParam)
        {
            inputParam.m_beingEntity.vehicle.Update();
            return BehaviorReturnCode.Success;
        }

        public void exit()
        {

        }
    }
}
