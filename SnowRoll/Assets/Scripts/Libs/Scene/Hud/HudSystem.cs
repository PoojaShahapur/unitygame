namespace SDK.Lib
{
    /**
     * @brief HUD 系统
     */
    public class HudSystem
    {
        public HudSystem()
        {

        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public HudItemBase createHud(BeingEntity being)
        {
            HudItemBase hud = null;

            if (EntityType.ePlayerMainChild == being.getEntityType())
            {
                hud = new PlayerMainChildHud();
            }
            else if (EntityType.ePlayerOtherChild == being.getEntityType())
            {
                hud = new PlayerOtherChildHud();
            }

            hud.setBeing(being);
            hud.init();

            return hud;
        }
    }
}