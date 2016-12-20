namespace SDK.Lib
{
    public class PlayerMainChildRender : PlayerRender
    {
        public PlayerMainChildRender(SceneEntityBase entity_)
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
            AuxPlayerMainChildUserData auxData = UtilApi.AddComponent<AuxPlayerMainChildUserData>(collide);
            auxData.setUserData(this.mEntity);
        }
    }
}