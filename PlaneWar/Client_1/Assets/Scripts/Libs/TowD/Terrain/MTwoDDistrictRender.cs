namespace SDK.Lib
{
    /**
     * @brief Terrain District Render
     */
    public class MTwoDDistrictRender : AuxComponent
    {
        protected MTwoDDistrict mEntity;
        protected AuxPrefabLoader mAuxPrefabLoader;
        protected bool mIsUsePool;
        protected string mResPath;

        public MTwoDDistrictRender(MTwoDDistrict entity)
        {
            this.mEntity = entity;
            this.mIsUsePool = true;
            this.mResPath = "World/Model/TerrainDistrictTest.prefab";
        }

        override public void init()
        {
            base.init();
        }

        override public void dispose()
        {
            this.releaseRes();

            base.dispose();
        }
        
        // �ͷ���Դ
        protected void releaseRes()
        {
            if (null != this.mAuxPrefabLoader)
            {
                if (this.mIsUsePool)
                {
                    this.onRetPool();
                    this.mAuxPrefabLoader.deleteObj();
                }
                else
                {
                    this.mAuxPrefabLoader.dispose();
                }

                this.mAuxPrefabLoader = null;
            }
        }

        public void onEnterScreenRange()
        {
            // ������ʾ��ʱ���Ȼ�ǿգ��������ж�һ��
            if (null == this.mAuxPrefabLoader)
            {
                this.load();
            }
        }

        public void onLeaveScreenRange()
        {
            this.releaseRes();

            this.selfGo = null;
        }

        // ��Դ����
        public void load()
        {
            if (null == this.mAuxPrefabLoader)
            {
                this.mAuxPrefabLoader = AuxPrefabLoader.newObject(this.mResPath);
                this.mAuxPrefabLoader.setDestroySelf(true);
                this.mAuxPrefabLoader.setIsNeedInsPrefab(true);
                this.mAuxPrefabLoader.setIsInsNeedCoroutine(true);
                this.mAuxPrefabLoader.setIsInitOrientPos(true);
                this.mAuxPrefabLoader.setIsFakePos(true);
                this.mAuxPrefabLoader.setIsUsePool(true);
            }

            this.mAuxPrefabLoader.asyncLoad(this.mResPath, this.onResLoaded);
        }

        public void onResLoaded(IDispatchObject dispObj)
        {
            this.mIsPosDirty = true;    // ǿ�Ƹ���λ��
            this.selfGo = this.mAuxPrefabLoader.getGameObject();
        }

        public void onRetPool()
        {

        }

        override public void updateLocalTransform()
        {
            if (null != this.mSelfGo)
            {
                if (this.mIsPosDirty)
                {
                    this.mIsPosDirty = false;
                    UtilApi.setPos(this.mSelfGo.transform, this.mEntity.getPos());
                }
            }
        }
    }
}