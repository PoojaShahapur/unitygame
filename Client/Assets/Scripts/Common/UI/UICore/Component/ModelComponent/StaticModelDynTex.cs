using SDK.Lib;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 静态模型，动态纹理
     */
    public class StaticModelDynTex : AuxComponent
    {
        protected string m_texPath;     // 纹理目录
        protected TextureRes m_texRes;  // 纹理资源
        protected Material m_mat;       // Unity 材质
        protected bool m_bNeedReloadTex;        // 是否需要重新加载纹理
        protected bool m_bModelChanged;         // 模型是否改变

        public StaticModelDynTex()
        {
            m_bNeedReloadTex = false;
            m_bModelChanged = false;
        }

        public string texPath
        {
            get
            {
                return m_texPath;
            }
            set
            {
                if (m_texPath != value)
                {
                    m_bNeedReloadTex = true;
                }
                m_texPath = value;
            }
        }

        public TextureRes texRes
        {
            get
            {
                return m_texRes;
            }
            set
            {
                if (m_texRes != value)
                {
                    m_bNeedReloadTex = true;

                    if(m_texRes != null)
                    {
                        Ctx.m_instance.m_texMgr.unload(m_texRes.GetPath(), null);
                        m_texRes = null;
                    }
                }
                m_texRes = value;
            }
        }

        override public void dispose()
        {
            if (m_texRes != null)
            {
                Ctx.m_instance.m_texMgr.unload(m_texRes.GetPath(), null);
                m_texRes = null;
            }

            base.dispose();
        }

        protected override void onSelfChanged()
        {
            m_bModelChanged = true;
            base.onSelfChanged();
            m_mat = m_selfGo.GetComponent<Renderer>().material;
        }

        public void syncUpdateTex()
        {
            if(m_bNeedReloadTex)
            {
                if (m_texRes != null)
                {
                    Ctx.m_instance.m_texMgr.unload(m_texRes.GetPath(), null);
                    m_texRes = null;
                }

                m_texRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_texPath);
                m_mat.mainTexture = m_texRes.getTexture();
            }
            else if (m_bModelChanged)
            {
                if (m_texRes == null)
                {
                    m_texRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_texPath);
                }

                m_mat.mainTexture = m_texRes.getTexture();
            }

            m_bNeedReloadTex = false;
            m_bModelChanged = false;
        }
    }
}