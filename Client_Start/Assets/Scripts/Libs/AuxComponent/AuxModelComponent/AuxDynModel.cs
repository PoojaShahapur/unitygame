namespace SDK.Lib
{
    /**
     * @brief 动态模型，材质都是静态的，不会改变，只会加载动态模型
     */
    public class AuxDynModel : AuxComponent
    {
        protected string mModelResPath;
        protected ModelRes mModelRes;
        protected bool mBNeedReloadModel;
        protected EventDispatch mModelInsDisp;      // 模型加载并且实例化后事件分发

        public AuxDynModel()
        {
            this.mModelInsDisp = new AddOnceEventDispatch();
            this.mBNeedReloadModel = false;
        }

        public string modelResPath
        {
            get
            {
                return this.mModelResPath;
            }
            set
            {
                if (this.mModelResPath != value)
                {
                    this.mBNeedReloadModel = true;
                }
                this.mModelResPath = value;
            }
        }

        public EventDispatch modelInsDisp
        {
            get
            {
                return this.mModelInsDisp;
            }
            set
            {
                this.mModelInsDisp = value;
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

            this.mModelInsDisp.clearEventHandle();
            this.mModelInsDisp = null;

            base.dispose();
        }

        public void attach2Parent()
        {
            if (this.mPntGo != null)
            {
                if (this.mIsNeedPlaceHolderGo)
                {
                    if (this.mPlaceHolderGo == null)
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
        }

        public void syncUpdateModel()
        {
            if (this.mBNeedReloadModel)
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

                this.mModelRes = Ctx.mInstance.mModelMgr.getAndSyncLoad<ModelRes>(this.mModelResPath);

                selfGo = this.mModelRes.InstantiateObject(this.mModelResPath);
                attach2Parent();

                this.mModelInsDisp.dispatchEvent(this);
            }

            this.mBNeedReloadModel = false;
        }
    }
}