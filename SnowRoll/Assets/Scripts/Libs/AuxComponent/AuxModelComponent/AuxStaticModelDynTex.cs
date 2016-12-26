using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 静态模型，动态纹理
     */
    public class AuxStaticModelDynTex : AuxComponent
    {
        protected string mTexPath;     // 纹理目录
        protected TextureRes mTexRes;  // 纹理资源
        protected Material mMat;       // Unity 材质
        protected bool mIsNeedReloadTex;        // 是否需要重新加载纹理
        protected bool mIsModelChanged;         // 模型是否改变

        public AuxStaticModelDynTex()
        {
            this.mIsNeedReloadTex = false;
            this.mIsModelChanged = false;
        }

        public string texPath
        {
            get
            {
                return this.mTexPath;
            }
            set
            {
                if (this.mTexPath != value)
                {
                    this.mIsNeedReloadTex = true;
                }
                this.mTexPath = value;
            }
        }

        public TextureRes texRes
        {
            get
            {
                return this.mTexRes;
            }
            set
            {
                if (this.mTexRes != value)
                {
                    this.mIsNeedReloadTex = true;

                    if(this.mTexRes != null)
                    {
                        Ctx.mInstance.mTexMgr.unload(this.mTexRes.getResUniqueId(), null);
                        this.mTexRes = null;
                    }
                }
                this.mTexRes = value;
            }
        }

        override public void dispose()
        {
            if (this.mTexRes != null)
            {
                Ctx.mInstance.mTexMgr.unload(this.mTexRes.getResUniqueId(), null);
                this.mTexRes = null;
            }

            base.dispose();
        }

        protected override void onSelfChanged()
        {
            this.mIsModelChanged = true;
            base.onSelfChanged();
            this.mMat = this.mSelfGo.GetComponent<Renderer>().material;
        }

        public void syncUpdateTex()
        {
            if(this.mIsNeedReloadTex)
            {
                if (this.mTexRes != null)
                {
                    Ctx.mInstance.mTexMgr.unload(this.mTexRes.getResUniqueId(), null);
                    this.mTexRes = null;
                }

                this.mTexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(this.mTexPath, null);
                this.mMat.mainTexture = this.mTexRes.getTexture();
            }
            else if (this.mIsModelChanged)
            {
                if (this.mTexRes == null)
                {
                    this.mTexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(this.mTexPath, null);
                }

                mMat.mainTexture = mTexRes.getTexture();
            }

            this.mIsNeedReloadTex = false;
            this.mIsModelChanged = false;
        }
    }
}