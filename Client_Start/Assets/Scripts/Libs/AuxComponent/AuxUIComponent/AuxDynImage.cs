using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxDynImage : AuxWindow
    {
        protected string mGoName;          // 场景中有 Image 的 GameObject 的名字
        protected Image mImage;            // 图像
        protected ImageItem mImageItem;    // 图像资源

        protected string mAtlasName;       // 图集名字
        protected string mImageName;       // 图集中图片的名字
        protected bool mIsNeedUpdateImage = false;  // 是否需要更新图像
        protected bool mIsImageGoChange = false;    // Image 组件所在的 GameObject 改变

        protected EventDispatch mImageLoadedDisp;  // 图像加载完成事件分发，改变 GameObject 或者 Image 图像内容都会分发

        public AuxDynImage()
        {
            this.mImageLoadedDisp = new EventDispatch();
        }

        public string goName
        {
            set
            {
                this.mGoName = value;
            }
        }

        public string atlasName
        {
            set
            {
                if (this.mAtlasName != value)
                {
                    this.mIsNeedUpdateImage = true;
                }
                this.mAtlasName = value;
            }
        }

        public string imageName
        {
            set
            {
                if (this.mImageName != value)
                {
                    this.mIsNeedUpdateImage = true;
                }
                this.mImageName = value;
            }
        }

        public EventDispatch imageLoadedDisp
        {
            get
            {
                return this.mImageLoadedDisp;
            }
        }

        // 设置图像信息
        public void setImageInfo(string atlasName, string imageName)
        {
            if (this.mAtlasName != atlasName)
            {
                this.mIsNeedUpdateImage = true;
            }
            if (this.mImageName != imageName)
            {
                this.mIsNeedUpdateImage = true;
            }

            this.mAtlasName = atlasName;
            this.mImageName = imageName;
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
                if (this.mImageItem != null)
                {
                    Ctx.mInstance.mAtlasMgr.unloadImage(this.mImageItem, null);
                    this.mImageItem = null;
                }
                this.mImageItem = Ctx.mInstance.mAtlasMgr.getAndSyncLoadImage(this.mAtlasName, this.mImageName);
                this.mImageItem.setImageImage(this.mImage);
            }
            else if (this.mIsImageGoChange)
            {
                if (this.mImageItem == null)
                {
                    this.mImageItem = Ctx.mInstance.mAtlasMgr.getAndSyncLoadImage(this.mAtlasName, this.mImageName);
                }
                this.mImageItem.setImageImage(this.mImage);
            }

            this.mIsImageGoChange = false;
            this.mIsNeedUpdateImage = false;
        }

        override public void dispose()
        {
            base.dispose();
            if (this.mImageItem != null)
            {
                Ctx.mInstance.mAtlasMgr.unloadImage(this.mImageItem, null);
                this.mImageItem = null;
            }
        }

        // 同步更新显示
        virtual public void syncUpdateCom()
        {
            updateImage();
            this.mImageLoadedDisp.dispatchEvent(this);
        }
    }
}