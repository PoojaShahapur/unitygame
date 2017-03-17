using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 动态 UI Image，但是 Image 使用的是一张单独的图片，不是地图集
     */
    public class AuxDynTexImage : AuxWindow
    {
        protected string mGoName;          // 场景中有 Image 的 GameObject 的名字
        protected Image mImage;            // 图像
        protected TextureRes mTexRes;      // 图像的纹理资源

        protected string mTexPath;         // 图像纹理目录
        protected bool mIsNeedUpdateImage = false;  // 是否需要更新图像
        protected bool mIsImageGoChange;    // Image 组件所在的 GameObject 改变

        protected EventDispatch mTexLoadedDisp;  // 图像加载完成事件分发，改变 GameObject 或者 Image 图像内容都会分发

        public AuxDynTexImage()
        {
            this.mIsNeedUpdateImage = false;
            this.mIsImageGoChange = false;
            this.mTexLoadedDisp = new EventDispatch();
        }

        public string goName
        {
            set
            {
                this.mGoName = value;
            }
        }

        public string texPath
        {
            set
            {
                if (this.mTexPath != value)
                {
                    this.mIsNeedUpdateImage = true;
                }
                this.mTexPath = value;
            }
        }

        public EventDispatch texLoadedDisp
        {
            get
            {
                return this.mTexLoadedDisp;
            }
        }

        // 查找 UI 组件
        virtual public void findWidget()
        {

        }

        // 资源改变更新图像
        protected void updateImage()
        {
            if (this.mIsNeedUpdateImage)
            {
                if (this.mTexRes != null)
                {
                    Ctx.mInstance.mTexMgr.unload(this.mTexRes.getResUniqueId(), null);
                    this.mTexRes = null;
                }
                this.mTexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(this.mTexPath, null);
                this.mTexRes.setImageTex(this.mImage);
            }
            else if (this.mIsImageGoChange)
            {
                if (this.mTexRes == null)
                {
                    this.mTexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(this.mTexPath, null);
                }
                this.mTexRes.setImageTex(this.mImage);
            }

            this.mIsImageGoChange = false;
            this.mIsNeedUpdateImage = false;
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

        // 同步更新显示
        virtual public void syncUpdateCom()
        {
            updateImage();
            this.mTexLoadedDisp.dispatchEvent(this);
        }
    }
}