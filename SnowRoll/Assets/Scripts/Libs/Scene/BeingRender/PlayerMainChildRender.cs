namespace SDK.Lib
{
    public class PlayerMainChildRender : PlayerChildRender
    {
        public PlayerMainChildRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            //this.mResPath = "World/Model/PlayerTest.prefab";
            this.mResPath = (this.mEntity as BeingEntity).getPrefabPath();
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            UnityEngine.GameObject collide = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
            AuxPlayerMainChildUserData auxData = UtilApi.AddComponent<AuxPlayerMainChildUserData>(collide);
            auxData.setUserData(this.mEntity);

            auxData = UtilApi.AddComponent<AuxPlayerMainChildUserData>(this.selfGo);
            auxData.setUserData(this.mEntity);

            UnityEngine.GameObject model = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.MODEL_NAME);
            if (null != (this.mEntity as Player).mAnimatorControl)
            {
                (this.mEntity as Player).mAnimatorControl.setAnimator(UtilApi.getComByP<UnityEngine.Animator>(model));
            }
            //UtilApi.setLayer(this.selfGo, "PlayerMainChild");
        }

        override protected void onRetPool()
        {
            base.onRetPool();

            if (null != this.mSelfGo)
            {
                // 关闭组件
                UtilApi.enableMeshRenderComponent(this.mModelRender, false);
                UtilApi.enableAnimatorComponent(this.mModel, false);
                UtilApi.enableCollider<UnityEngine.SphereCollider>(this.mSelfGo, false);
                UtilApi.enableCollider<UnityEngine.SphereCollider>(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.COLLIDE_NAME), false);
                UtilApi.enableRigidbodyComponent(this.mSelfGo, false);
            }
        }

        override protected void onGetPool()
        {
            base.onGetPool();

            if (null != this.mSelfGo)
            {
                // 关闭组件
                UtilApi.enableMeshRenderComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.MODEL_RENDER_NAME), true);
                UtilApi.enableAnimatorComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.MODEL_NAME), true);
                UtilApi.enableCollider<UnityEngine.SphereCollider>(this.mSelfGo, true);
                UtilApi.enableCollider<UnityEngine.SphereCollider>(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.COLLIDE_NAME), true);
                UtilApi.enableRigidbodyComponent(this.mSelfGo, true);
            }
        }
    }
}