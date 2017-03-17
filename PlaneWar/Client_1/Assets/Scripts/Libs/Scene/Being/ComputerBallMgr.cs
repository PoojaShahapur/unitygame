namespace SDK.Lib
{
    public class ComputerBallMgr : EntityMgrBase
    {
        public ComputerBallMgr()
        {
            mUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.RobotPrefix, 0);
        }

        override protected void onTickExec(float delta, TickMode tickMode)
        {
            base.onTickExec(delta, tickMode);
        }

        override public void init()
        {

        }

        public void addComputerBall(ComputerBall computerBall)
        {
            this.addEntity(computerBall);
        }

        public void removeComputerBall(ComputerBall computerBall)
        {
            this.removeEntity(computerBall);
        }
    }
}