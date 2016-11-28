namespace SDK.Lib
{
    public class PlayerMainRender : PlayerRender
    {
        public PlayerMainRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/Player";
        }
    }
}