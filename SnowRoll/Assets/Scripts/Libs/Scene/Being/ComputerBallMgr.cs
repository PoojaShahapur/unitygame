namespace SDK.Lib
{
    public class ComputerBallMgr : EntityMgrBase
    {
        public ComputerBallMgr()
        {
            mUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.RobotPrefix, 0);
        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
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