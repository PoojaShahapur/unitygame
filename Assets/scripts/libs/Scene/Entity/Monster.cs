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
            int idx = 0;
            while (idx < (int)MonstersModelDef.eModelTotal)
            {
                m_skinAniModel.m_modelList[idx] = new PartInfo();
                ++idx;
            }
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
            aiController.vehicle.Steerings[0].Vehicle = aiController.vehicle;
            (aiController.vehicle.Steerings[0] as SteerToFollow).Target = (Ctx.m_instance.m_playerMgr.getHero() as BeingEntity).skinAniModel.transform;
            aiController.vehicle.Steerings[0].Vehicle = aiController.vehicle as Vehicle;

            SteerForNeighbors[] behaviors = new SteerForNeighbors[1];
            behaviors[0] = new SteerForCohesion();
            behaviors[0].Vehicle = aiController.vehicle;
            aiController.vehicle.Steerings[1] = new SteerForNeighborGroup();
            (aiController.vehicle.Steerings[1] as SteerForNeighborGroup).addBehaviors(behaviors);
            aiController.vehicle.Steerings[1].Vehicle = aiController.vehicle;
        }
    }
}