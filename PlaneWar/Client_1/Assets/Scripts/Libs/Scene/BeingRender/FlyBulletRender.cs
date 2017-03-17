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
            if ((this.mEntity as FlyBullet).isSelfBullet())
            {
                this.mResPath = "World/Model/FlyBulletTest.prefab";
            }
            else
            {
                this.mResPath = "World/Model/FlyBulletTestOther.prefab";
            }
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            AuxFlyBulletUserData auxData = UtilApi.AddComponent<AuxFlyBulletUserData>(this.selfGo);
            auxData.setUserData(this.mEntity);

            //if((this.mEntity as FlyBullet).isSelfBullet())
            //{
            //    if(!Ctx.mInstance.mPlayerMgr.getHero().isChildEnableRigidByThisId((this.mEntity as FlyBullet).getOwnerThisId()))
            //    {
            //        this.enableRigid(false);
            //    }
            //}
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

                //if ((this.mEntity as FlyBullet).isSelfBullet())
                //{
                    //if (Ctx.mInstance.mPlayerMgr.getHero().isChildEnableRigidByThisId((this.mEntity as FlyBullet).getOwnerThisId()))
                    //{
                        UtilApi.enableCollider2D<UnityEngine.BoxCollider2D>(this.mSelfGo, true);
                    //}
                    //else
                    //{
                    //    this.enableRigid(false);
                    //}
                //}
            }
        }

        // 资源加载
        override public void load()
        {
            if (null == this.mAuxPrefabLoader)
            {
                //this.mAuxPrefabLoader = new AuxPrefabLoader("", true, false);
                this.mAuxPrefabLoader = AuxPrefabLoader.newObject(this.mResPath);
                this.mAuxPrefabLoader.setDestroySelf(true);
                this.mAuxPrefabLoader.setIsNeedInsPrefab(true);
                this.mAuxPrefabLoader.setIsInsNeedCoroutine(false);
                this.mAuxPrefabLoader.setIsInitOrientPos(true);
                this.mAuxPrefabLoader.setIsFakePos(true);
                this.mAuxPrefabLoader.setIsUsePool(true);
            }

            this.mAuxPrefabLoader.asyncLoad(this.mResPath, this.onResLoaded);
        }

        override public void enableRigid(bool enable)
        {
            UtilApi.enableCollider2D<UnityEngine.BoxCollider2D>(this.mSelfGo, enable);
        }
    }
}
