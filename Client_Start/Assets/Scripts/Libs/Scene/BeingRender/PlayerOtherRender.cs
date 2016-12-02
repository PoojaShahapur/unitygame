namespace SDK.Lib
{
    public class PlayerOtherRender : PlayerRender
    {
        public PlayerOtherRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/PlayerTest.prefab";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();
        }
    }
}