using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 地形的一个区域，一个区域就是单独的一个逻辑单元
     */
    public class Area : AreaBase
    {
        protected MeshRender m_render;  // 区域渲染器

        public Area()
        {
            m_render = new SingleAreaRender();      // 默认创建漫反射的地形
        }

        public MeshRender getAreaRender()
        {
            return m_render;
        }

        public void setAreaRender(MeshRender render_)
        {
            m_render = render_;
        }

        public void setTexture(Texture tex)
        {
            //m_render.mainTexture = tex;
        }

        public void render()
        {
            //m_render.buildMesh();
        }
    }
}