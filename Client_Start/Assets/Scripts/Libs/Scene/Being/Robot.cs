namespace SDK.Lib
{
    public class Robot : Player
    {
        public Robot()
        {
            mTypeId = "Robot";
            this.mEntityType = EntityType.eRobot;
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
    }
}