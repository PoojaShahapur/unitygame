using SDK.Lib;
using UnityEngine.UI;

namespace SDK.Common
{
    /**
     * @brief 动态 UI Image，但是 Image 使用的是地图集
     */
    public class AuxDynAtlasImage : AuxComponent
    {
        protected string m_goName;          // 场景中有 Image 的 GameObject 的名字
        protected Image m_image;            // 图像
        protected ImageItem m_imageItem;    // 图像资源

        protected string m_atlasName;       // 图集名字
        protected string m_imageName;       // 图集中图片的名字
        protected bool m_bNeedUpdateImage = false;  // 是否需要更新图像
        protected bool m_bImageGoChange = false;    // Image 组件所在的 GameObject 改变

        protected EventDispatch m_imageLoadedDisp;  // 图像加载完成事件分发，改变 GameObject 或者 Image 图像内容都会分发

        public AuxDynAtlasImage()
        {
            m_imageLoadedDisp = new EventDispatch();
        }

        public string goName
        {
            set
            {
                m_goName = value;
            }
        }

        public string atlasName
        {
            set
            {
                if (m_atlasName != value)
                {
                    m_bNeedUpdateImage = true;
                }
                m_atlasName = value;
            }
        }

        public string imageName
        {
            set
            {
                if (m_imageName != value)
                {
                    m_bNeedUpdateImage = true;
                }
                m_imageName = value;
            }
        }

        public EventDispatch imageLoadedDisp
        {
            get
            {
                return m_imageLoadedDisp;
            }
        }

        // 设置图像信息
        public void setImageInfo(string atlasName, string imageName)
        {
            if (m_atlasName != atlasName)
            {
                m_bNeedUpdateImage = true;
            }
            if (m_imageName != imageName)
            {
                m_bNeedUpdateImage = true;
            }

            m_atlasName = atlasName;
            m_imageName = imageName;
        }

        // 查找 UI 组件
        virtual public void findWidget()
        {

        }

        // 资源改变更新图像
        protected void updateImage()
        {
            if (m_bNeedUpdateImage)
            {
                if (m_imageItem != null)
                {
                    Ctx.m_instance.m_atlasMgr.unloadImage(m_imageItem, null);
                    m_imageItem = null;
                }
                m_imageItem = Ctx.m_instance.m_atlasMgr.getAndSyncLoadImage(m_atlasName, m_imageName);
                m_imageItem.setImageImage(m_image);
            }
            else if (m_bImageGoChange)
            {
                if (m_imageItem == null)
                {
                    m_imageItem = Ctx.m_instance.m_atlasMgr.getAndSyncLoadImage(m_atlasName, m_imageName);
                }
                m_imageItem.setImageImage(m_image);
            }

            m_bImageGoChange = false;
            m_bNeedUpdateImage = false;
        }

        override public void dispose()
        {
            base.dispose();
            if (m_imageItem != null)
            {
                Ctx.m_instance.m_atlasMgr.unloadImage(m_imageItem, null);
                m_imageItem = null;
            }
        }

        // 同步更新显示
        virtual public void syncUpdateCom()
        {
            updateImage();
            m_imageLoadedDisp.dispatchEvent(this);
        }
    }
}