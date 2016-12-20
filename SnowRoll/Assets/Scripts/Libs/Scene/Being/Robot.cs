namespace SDK.Lib
{
    public class Robot : Player
    {
        protected RobotAIControl mAIControl;

        public Robot()
        {
            mTypeId = "Robot";
            this.mEntityType = EntityType.eRobot;
            this.mEntityUniqueId = Ctx.mInstance.mRobotMgr.genNewStrId();

            this.mAIControl = new RobotAIControl(this);
        }

        override public void preInit()
        {
            base.preInit();
            this.mAIControl.init();
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);

            mAIControl.onTick(delta);
        }

        override public void dispose()
        {
            base.dispose();

            Ctx.mInstance.mRobotMgr.removeEntity(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

            Ctx.mInstance.mRobotMgr.addEntity(this);
        }

        override public void initRender()
        {
            mRender = new RobotRender(this);
            mRender.init();
        }

        public void SetPlayer(PlayerMain player)
        {
            if(null != this.mAIControl)
            {
                //this.mAIControl.SetPlayer(player);
            }
        }
    }
}