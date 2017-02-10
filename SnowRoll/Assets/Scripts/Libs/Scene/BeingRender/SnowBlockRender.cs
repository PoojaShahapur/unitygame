namespace SDK.Lib
{
    public class SnowBlockRender : BeingEntityRender
    {
        public SnowBlockRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            //this.mResPath = "World/Model/SnowBlockTest.prefab";
            this.mResPath = (this.mEntity as BeingEntity).getPrefabPath();
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            this.mModelRender = this.selfGo;

            //this.setModelMat();
            //this.setModelTexTile();

            //GameObject collide = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
            //AuxSnowBlockUserData auxData = UtilApi.AddComponent<AuxSnowBlockUserData>(collide);
            AuxSnowBlockUserData auxData = UtilApi.AddComponent<AuxSnowBlockUserData>(this.selfGo);

            auxData.setUserData(this.mEntity);

            //UtilApi.setLayer(this.selfGo, "SnowBlock");
        }

        override public void updateLocalTransform()
        {
            if (null != this.mSelfGo)
            {
                // 雪块只更新位置和旋转，不更新缩放
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
                UtilApi.enableMeshRenderComponent(this.mSelfGo, false);
                UtilApi.enableAnimatorComponent(this.mSelfGo, false);
                UtilApi.enableCollider<UnityEngine.SphereCollider>(this.mSelfGo, false);
            }
        }

        override protected void onGetPool()
        {
            base.onGetPool();

            if (null != this.mSelfGo)
            {
                // 关闭组件
                UtilApi.enableMeshRenderComponent(this.mSelfGo, true);
                UtilApi.enableAnimatorComponent(this.mSelfGo, true);
                UtilApi.enableCollider<UnityEngine.SphereCollider>(this.mSelfGo, true);
            }
        }
    }
}
