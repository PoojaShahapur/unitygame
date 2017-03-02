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
                UtilApi.enableSpriteRenderComponent(this.mModelRender, false);
                UtilApi.enableAnimatorComponent(this.mModel, false);
                //UtilApi.enableCollider<UnityEngine.SphereCollider>(this.mSelfGo, false);
                //UtilApi.enableCollider<UnityEngine.SphereCollider>(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.COLLIDE_NAME), false);

                UtilApi.enableCollider2D<UnityEngine.BoxCollider2D>(this.mSelfGo, false);
                UtilApi.enableCollider2D<UnityEngine.BoxCollider2D>(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.COLLIDE_NAME), false);

                UtilApi.enableRigidbodyComponent(this.mSelfGo, false);

                // 关闭拖尾
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
                //UtilApi.enableCollider<UnityEngine.SphereCollider>(this.mSelfGo, true);
                //UtilApi.enableCollider<UnityEngine.SphereCollider>(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.COLLIDE_NAME), true);

                UtilApi.enableCollider2D<UnityEngine.BoxCollider2D>(this.mSelfGo, true);
                UtilApi.enableCollider2D<UnityEngine.BoxCollider2D>(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.COLLIDE_NAME), true);

                UtilApi.enableRigidbodyComponent(this.mSelfGo, true);

                UtilApi.enableTrailRendererComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.TRIAL_NAME), true);
                UtilApi.enableTrailRendererComponent(UtilApi.TransFindChildByPObjAndPath(this.mSelfGo, UtilApi.TRIAL_1_NAME), true);
            }
        }
    }
}