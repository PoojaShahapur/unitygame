namespace SDK.Lib
{
    public class Monster : BeingEntity
    {
        protected int m_groupID = 0;        // 组 id 

        public Monster()
            : base()
        {
            mName = "Monster";

            //m_skinAniModel.m_modelList = new SkinSubModel[(int)eMonstersModelType.eModelTotal];
            //int idx = 0;
            //while (idx < (int)eMonstersModelType.eModelTotal)
            //{
            //    m_skinAniModel.m_modelList[idx] = new SkinSubModel();
            //    ++idx;
            //}
        }

        public int groupID
        {
            get
            {
                return m_groupID;
            }
            set
            {
                m_groupID = value;
            }
        }

        override public void dispose()
        {
            base.dispose();
            Ctx.m_instance.m_monsterMgr.removeEntity(this);
        }

        override protected void initSteerings()
        {
            base.initSteerings();

            // 初始化 vehicle
            aiController.vehicle.MaxSpeed = 10;
            //aiController.vehicle.setSpeed(5);

            //testSteerForCohesion();
            //testSteerForAlignment();
            testSteerForSeparation();
            //testSteerForFollow();

            //aiController.radar.Vehicles = (Ctx.m_instance.m_monsterMgr as MonsterMgr).getOrAddGroup(this.groupID);
            //(Ctx.m_instance.m_monsterMgr as MonsterMgr).addGroupMember(this);

            //aiController.radar.Vehicles = (Ctx.m_instance.m_sceneSys as SceneSys).scene.zoneSys.getOrAddDangerZone(this.groupID).vehicleList;
            (Ctx.m_instance.m_sceneSys as SceneSys).scene.zoneSys.addMonster(this);
        }

        protected void testSteerForCohesion()
        {
            // 初始化 Steerings
            //aiController.vehicle.Steerings = new Steering[2];
            //aiController.vehicle.Steerings[0] = new SteerToFollow();
            //aiController.vehicle.Steerings[0].Vehicle = aiController.vehicle;
            //(aiController.vehicle.Steerings[0] as SteerToFollow).Target = (Ctx.m_instance.m_playerMgr.getHero() as BeingEntity).skinAniModel.transform;
            //(aiController.vehicle.Steerings[0] as SteerToFollow).ConsiderVelocity = false;

            //SteerForNeighbors[] behaviors = new SteerForNeighbors[1];
            //behaviors[0] = new SteerForCohesion();
            //behaviors[0].Vehicle = aiController.vehicle;
            //aiController.vehicle.Steerings[1] = new SteerForNeighborGroup();
            //aiController.vehicle.Steerings[1].Vehicle = aiController.vehicle;
            //(aiController.vehicle.Steerings[1] as SteerForNeighborGroup).addBehaviors(behaviors);
        }

        protected void testSteerForAlignment()
        {
            // 初始化 Steerings
            //aiController.vehicle.Steerings = new Steering[2];
            //aiController.vehicle.Steerings[0] = new SteerToFollow();
            //aiController.vehicle.Steerings[0].Vehicle = aiController.vehicle;
            //(aiController.vehicle.Steerings[0] as SteerToFollow).Target = (Ctx.m_instance.m_playerMgr.getHero() as BeingEntity).skinAniModel.transform;
            //(aiController.vehicle.Steerings[0] as SteerToFollow).ConsiderVelocity = false;

            //SteerForNeighbors[] behaviors = new SteerForNeighbors[1];
            //behaviors[0] = new SteerForAlignment();
            //behaviors[0].Vehicle = aiController.vehicle;
            //behaviors[0].Initialize();
            //aiController.vehicle.Steerings[1] = new SteerForNeighborGroup();
            //aiController.vehicle.Steerings[1].Vehicle = aiController.vehicle;
            //(aiController.vehicle.Steerings[1] as SteerForNeighborGroup).addBehaviors(behaviors);
        }

        protected void testSteerForSeparation()
        {
            // 初始化 Steerings
            //aiController.vehicle.Steerings = new Steering[2];
            //aiController.vehicle.Steerings[0] = new SteerToFollow();
            //aiController.vehicle.Steerings[0].Vehicle = aiController.vehicle;
            //(aiController.vehicle.Steerings[0] as SteerToFollow).Target = (Ctx.m_instance.m_playerMgr.getHero() as BeingEntity).skinAniModel.transform;
            //(aiController.vehicle.Steerings[0] as SteerToFollow).ConsiderVelocity = false;
            //(aiController.vehicle.Steerings[0] as SteerToFollow).distance = new Vector3(0, 0, -1);

            //SteerForNeighbors[] behaviors = new SteerForNeighbors[1];
            //behaviors[0] = new SteerForSeparation();
            //behaviors[0].Vehicle = aiController.vehicle;
            ////(behaviors[0] as SteerForSeparation).ComfortDistance = 10;
            ////(behaviors[0] as SteerForSeparation).multiplierInsideComfortDistance = 10;
            ////(behaviors[0] as SteerForSeparation).vehicleRadiusImpact = 10;
            //behaviors[0].Initialize();
            //(behaviors[0] as SteerForSeparation).initStart();
            //aiController.vehicle.Steerings[1] = new SteerForNeighborGroup();
            //aiController.vehicle.Steerings[1].Vehicle = aiController.vehicle;
            //(aiController.vehicle.Steerings[1] as SteerForNeighborGroup).addBehaviors(behaviors);
            ////(aiController.vehicle.Steerings[1] as SteerForNeighborGroup).MaxRadius = 100;
            ////(aiController.vehicle.Steerings[1] as SteerForNeighborGroup).MinRadius = 0;
        }

        protected void testSteerForFollow()
        {
            // 初始化 Steerings
            //aiController.vehicle.Steerings = new Steering[1];
            //aiController.vehicle.Steerings[0] = new SteerToFollow();
            //aiController.vehicle.Steerings[0].Vehicle = aiController.vehicle;
            //(aiController.vehicle.Steerings[0] as SteerToFollow).Target = (Ctx.m_instance.m_playerMgr.getHero() as BeingEntity).skinAniModel.transform;
            //(aiController.vehicle.Steerings[0] as SteerToFollow).ConsiderVelocity = false;
            //(aiController.vehicle.Steerings[0] as SteerToFollow).distance = new Vector3(0, 0, -1);
        }
    }
}