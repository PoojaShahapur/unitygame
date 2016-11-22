namespace SDK.Lib
{
    public class ModelItem : AuxComponent
    {
        protected string mResPath;
        protected ModelRes mModelRes;
        protected bool mIsNeedReloadModel;

        public ModelItem()
        {
            this.mIsNeedReloadModel = false;
        }

        public string resPath
        {
            get
            {
                return this.mResPath;
            }
            set
            {
                if (this.mResPath != value)
                {
                    this.mIsNeedReloadModel = true;
                }
                this.mResPath = value;
            }
        }

        override public void dispose()
        {
            if (this.mSelfGo != null)
            {
                UtilApi.Destroy(this.mSelfGo);
                this.mSelfGo = null;
            }

            if(this.mModelRes != null)
            {
                Ctx.mInstance.mModelMgr.unload(this.mModelRes.getResUniqueId(), null);
                this.mModelRes = null;
            }
            
            base.dispose();
        }

        public void updateModel()
        {
            if (this.mIsNeedReloadModel)
            {
                if(this.mModelRes != null)
                {
                    Ctx.mInstance.mModelMgr.unload(this.mModelRes.getResUniqueId(), null);
                    this.mModelRes = null;
                }
                if(this.mSelfGo != null)
                {
                    UtilApi.Destroy(this.mSelfGo);
                    this.mSelfGo = null;
                }

                LoadParam param;
                param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.setPath(this.mResPath);

                // 这个需要立即加载
                param.m_loadNeedCoroutine = false;
                param.m_resNeedCoroutine = false;

                this.mModelRes = Ctx.mInstance.mModelMgr.getAndLoad<ModelRes>(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);

                this.mSelfGo = this.mModelRes.InstantiateObject(this.mResPath);
                if (this.mIsNeedPlaceHolderGo)
                {
                    if(this.mPlaceHolderGo == null)
                    {
                        this.mPlaceHolderGo = new UnityEngine.GameObject();
                        UtilApi.SetParent(this.mPlaceHolderGo, this.mPntGo, false);
                    }
                    UtilApi.SetParent(this.mSelfGo, this.mPlaceHolderGo, false);
                }
                else
                {
                    UtilApi.SetParent(this.mSelfGo, this.mPntGo, false);
                }
            }

            this.mIsNeedReloadModel = false;
        }
    }
}