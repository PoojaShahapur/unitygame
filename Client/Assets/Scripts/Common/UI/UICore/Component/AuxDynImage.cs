using SDK.Lib;
using UnityEngine.UI;

namespace SDK.Common
{
    public class AuxDynImage : AuxPlaceHolderComponent
    {
        protected string m_goName;          // 场景中有 Image 的 GameObject 的名字
        protected Image m_image;            // 图像
        protected ImageItem m_imageItem;    // 图像资源

        protected string m_atlasName;       // 图集名字
        protected string m_imageName;       // 图集中图片的名字
        protected bool m_bNeedUpdateImage = false;  // 是否需要更新图像

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

        // 更新图像
        public void updateImage()
        {
            if (m_bNeedUpdateImage)
            {
                if (m_imageItem != null)
                {
                    Ctx.m_instance.m_atlasMgr.unloadImage(m_imageItem);
                    m_imageItem = null;
                }
                m_imageItem = Ctx.m_instance.m_atlasMgr.getAndAsyncLoadImage(m_atlasName, m_imageName);
                m_imageItem.setImageImage(m_image);
            }

            m_bNeedUpdateImage = false;
        }

        override public void dispose()
        {
            base.dispose();
            Ctx.m_instance.m_atlasMgr.unloadImage(m_imageItem);
        }
    }
}