namespace SDK.Lib
{
    public class PlayerOtherChildRender : PlayerRender
    {
        public PlayerOtherChildRender(SceneEntityBase entity_)
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
            AuxPlayerOtherChildUserData auxData = UtilApi.AddComponent<AuxPlayerOtherChildUserData>(collide);
            auxData.setUserData(this.mEntity);
        }
    }
}