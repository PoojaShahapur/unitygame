using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 动态 UI Image，但是 Image 使用的是一张单独的图片，不是地图集
     */
    public class AuxDynTexImage : AuxWindow
    {
        protected string m_goName;          // 场景中有 Image 的 GameObject 的名字
        protected Image m_image;            // 图像
        protected TextureRes m_texRes;      // 图像的纹理资源

        protected string m_texPath;         // 图像纹理目录
        protected bool m_bNeedUpdateImage = false;  // 是否需要更新图像
        protected bool m_bImageGoChange = false;    // Image 组件所在的 GameObject 改变

        protected EventDispatch m_texLoadedDisp;  // 图像加载完成事件分发，改变 GameObject 或者 Image 图像内容都会分发

        public AuxDynTexImage()
        {
            m_texLoadedDisp = new EventDispatch();
        }

        public string goName
        {
            set
            {
                m_goName = value;
            }
        }

        public string texPath
        {
            set
            {
                if (m_texPath != value)
                {
                    m_bNeedUpdateImage = true;
                }
                m_texPath = value;
            }
        }

        public EventDispatch texLoadedDisp
        {
            get
            {
                return m_texLoadedDisp;
            }
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
                if (m_texRes != null)
                {
                    Ctx.m_instance.m_texMgr.unload(m_texRes.getResUniqueId(), null);
                    m_texRes = null;
                }
                m_texRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_texPath);
                m_texRes.setImageTex(m_image);
            }
            else if (m_bImageGoChange)
            {
                if (m_texRes == null)
                {
                    m_texRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_texPath);
                }
                m_texRes.setImageTex(m_image);
            }

            m_bImageGoChange = false;
            m_bNeedUpdateImage = false;
        }

        override public void dispose()
        {
            base.dispose();
            if (m_texRes != null)
            {
                Ctx.m_instance.m_texMgr.unload(m_texRes.getResUniqueId(), null);
                m_texRes = null;
            }
        }

        // 同步更新显示
        virtual public void syncUpdateCom()
        {
            updateImage();
            m_texLoadedDisp.dispatchEvent(this);
        }
    }
}