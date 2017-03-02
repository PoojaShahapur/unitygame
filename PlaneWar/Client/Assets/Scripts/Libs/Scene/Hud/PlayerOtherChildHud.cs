namespace SDK.Lib
{
    public class PlayerOtherChildHud : HudItemBase
    {
        public PlayerOtherChildHud()
        {
            this.mResPath = "World/Hud/OtherChildHud.prefab";
        }

        public PlayerOtherChild mPlayerOtherChild
        {
            get
            {
                return this.mEntity as PlayerOtherChild;
            }
        }
    }
}