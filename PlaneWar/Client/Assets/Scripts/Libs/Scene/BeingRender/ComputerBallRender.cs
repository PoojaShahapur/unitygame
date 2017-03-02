namespace SDK.Lib
{
    public class ComputerBallRender : BeingEntityRender
    {
        public ComputerBallRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            //this.mResPath = "World/Model/PlayerOtherTest.prefab";
            this.mResPath = (this.mEntity as BeingEntity).getPrefabPath();
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            AuxComputerBallUserData auxData = UtilApi.AddComponent<AuxComputerBallUserData>(this.selfGo);

            auxData.setUserData(this.mEntity);
        }

        override protected void onRetPool()
        {
            base.onRetPool();

            if (null != this.mSelfGo)
            {
                // 关闭组件
                UtilApi.enableSpriteRenderComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.MODEL_RENDER_NAME), false);
                UtilApi.enableAnimatorComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.MODEL_NAME), false);
                //UtilApi.enableCollider<UnityEngine.SphereCollider>(this.mSelfGo, false);
                UtilApi.enableCollider2D<UnityEngine.BoxCollider2D>(this.mSelfGo, false);

                UtilApi.enableTrailRendererComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.TRIAL_NAME), false);
                UtilApi.enableTrailRendererComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.TRIAL_1_NAME), false);
            }
        }

        override protected void onGetPool()
        {
            base.onGetPool();

            if (null != this.mSelfGo)
            {
                // 关闭组件
                UtilApi.enableSpriteRenderComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.MODEL_RENDER_NAME), true);
                UtilApi.enableAnimatorComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.MODEL_NAME), true);
                UtilApi.enableCollider2D<UnityEngine.BoxCollider2D>(this.mSelfGo, true);

                UtilApi.enableTrailRendererComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.TRIAL_NAME), true);
                UtilApi.enableTrailRendererComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.TRIAL_1_NAME), true);
            }
        }
    }
}
