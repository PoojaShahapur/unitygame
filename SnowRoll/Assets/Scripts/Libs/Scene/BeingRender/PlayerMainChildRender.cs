﻿namespace SDK.Lib
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
                UtilApi.enableRigidbodyComponent(this.mSelfGo, false);
            }
        }

        override protected void onGetPool()
        {
            base.onGetPool();

            if (null != this.mSelfGo)
            {
                // 关闭组件
                UtilApi.enableMeshRenderComponent(this.mModelRender, true);
                UtilApi.enableAnimatorComponent(this.mModel, true);
                UtilApi.enableCollider<UnityEngine.SphereCollider>(this.mSelfGo, true);
                UtilApi.enableRigidbodyComponent(this.mSelfGo, true);
            }
        }
    }
}