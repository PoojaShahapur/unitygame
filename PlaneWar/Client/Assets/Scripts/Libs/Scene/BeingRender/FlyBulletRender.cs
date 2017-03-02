namespace SDK.Lib
{
    public class FlyBulletRender : BeingEntityRender
    {
        public FlyBulletRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/FlyBulletTest.prefab";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            AuxFlyBulletUserData auxData = UtilApi.AddComponent<AuxFlyBulletUserData>(this.selfGo);
            auxData.setUserData(this.mEntity);
        }

        override public void updateLocalTransform()
        {
            if (null != this.mSelfGo)
            {
                // 更新位置和旋转
                if (this.mIsPosDirty)
                {
                    this.mIsPosDirty = false;
                    UtilApi.setPos(this.mSelfGo.transform, this.mEntity.getPos());
                }
                if (this.mIsRotDirty)
                {
                    this.mIsRotDirty = false;
                    UtilApi.setRot(this.mSelfGo.transform, this.mEntity.getRotate());
                }
            }
        }

        override protected void onRetPool()
        {
            base.onRetPool();

            if (null != this.mSelfGo)
            {
                // 关闭组件
                UtilApi.enableSpriteRenderComponent(this.mSelfGo, false);
                UtilApi.enableAnimatorComponent(this.mSelfGo, false);
                //UtilApi.enableCollider<UnityEngine.SphereCollider>(this.mSelfGo, false);
                UtilApi.enableCollider2D<UnityEngine.BoxCollider2D>(this.mSelfGo, false);
            }
        }

        override protected void onGetPool()
        {
            base.onGetPool();

            if (null != this.mSelfGo)
            {
                // 关闭组件
                UtilApi.enableSpriteRenderComponent(this.mSelfGo, true);
                UtilApi.enableAnimatorComponent(this.mSelfGo, true);
                //UtilApi.enableCollider<UnityEngine.SphereCollider>(this.mSelfGo, true);
                UtilApi.enableCollider2D<UnityEngine.BoxCollider2D>(this.mSelfGo, true);
            }
        }
    }
}
