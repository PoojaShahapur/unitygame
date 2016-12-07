namespace SDK.Lib
{
    public class PlayerChildRender : PlayerRender
    {
        public PlayerChildRender(SceneEntityBase entity_)
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

            UnityEngine.GameObject collide = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
            AuxPlayerChildUserData auxData = UtilApi.AddComponent<AuxPlayerChildUserData>(collide);
            auxData.setUserData(this.mEntity);
        }
    }
}