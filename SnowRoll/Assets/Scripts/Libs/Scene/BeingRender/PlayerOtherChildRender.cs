namespace SDK.Lib
{
    public class PlayerOtherChildRender : PlayerChildRender
    {
        public PlayerOtherChildRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/PlayerOtherTest.prefab";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            //UnityEngine.GameObject collide = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
            //AuxPlayerOtherChildUserData auxData = UtilApi.AddComponent<AuxPlayerOtherChildUserData>(collide);
            AuxPlayerOtherChildUserData auxData = UtilApi.AddComponent<AuxPlayerOtherChildUserData>(this.selfGo);
            auxData.setUserData(this.mEntity);

            UtilApi.setLayer(this.selfGo, "PlayerOtherChild");
        }
    }
}