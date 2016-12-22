namespace SDK.Lib
{
    public class PlayerMainChildHud : HudItemBase
    {
        public PlayerMainChildHud()
        {
            this.mResPath = "World/Hud/MainChildHud.prefab";
        }

        public PlayerMainChild mPlayerMainChild
        {
            get
            {
                return this.mEntity as PlayerMainChild;
            }
        }
    }
}