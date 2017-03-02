namespace SDK.Lib
{
    public class ComputerBallHud : HudItemBase
    {
        public ComputerBallHud()
        {
            this.mResPath = "World/Hud/OtherChildHud.prefab";
        }

        public ComputerBall mComputerBall
        {
            get
            {
                return this.mEntity as ComputerBall;
            }
        }
    }
}