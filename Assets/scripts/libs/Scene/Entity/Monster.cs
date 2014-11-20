using SDK.Common;
using UnityEngine;
using UnitySteer.Behaviors;

namespace SDK.Lib
{
    class Monster : BeingEntity, IMonster
    {
        public Monster()
            : base()
        {
            m_skinAniModel.m_modelList = new PartInfo[(int)MonstersModelDef.eModelTotal];
        }

        override protected void initSteerings()
        {
            base.initSteerings();

            // 初始化 vehicle
            aiController.vehicle.MaxSpeed = 10;
            aiController.vehicle.setSpeed(5);

            // 初始化 Steerings
            aiController.vehicle.Steerings = new Steering[2];
            aiController.vehicle.Steerings[0] = new SteerToFollow();
            (aiController.vehicle.Steerings[0] as SteerToFollow).Target = (Ctx.m_instance.m_playerMgr.getHero() as BeingEntity).skinAniModel.transform;
            aiController.vehicle.Steerings[0].Vehicle = aiController.vehicle as Vehicle;

            SteerForNeighbors[] behaviors = new SteerForNeighbors[1];
            behaviors[0] = new SteerForCohesion();
            aiController.vehicle.Steerings[1] = new SteerForNeighborGroup();
            (aiController.vehicle.Steerings[1] as SteerForNeighborGroup).addBehaviors(behaviors);
        }
    }
}