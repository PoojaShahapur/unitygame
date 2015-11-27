namespace SDK.Lib
{
    /**
     * @brief 场景中可显示的对象
     */
    public class MSubMesh
    {
        protected MMesh m_parentMesh;            // Parent Mesh 
		protected MSubGeometryBase m_subGeometry;   // 显示的子 Geometry
        protected MeshRender m_meshRender;      // SubMesh 渲染器，和 Mesh 渲染器是一样的

        public MSubMesh(MSubGeometryBase subGeometry_ = null, MeshRender meshRender_ = null)
        {
            m_subGeometry = subGeometry_;
            m_meshRender = meshRender_;
        }

        /**
         * @brief 修改 SubGeometry 数据
         */
        public void setSubGeometry(MSubGeometryBase subGeometry_)
        {
            m_subGeometry = subGeometry_;
        }

        /**
         * @brief 修改渲染器
         */
        public void setMeshRender(MeshRender meshRender_)
        {
            m_meshRender = meshRender_;
        }

        /**
         * @brief 渲染数据
         */
        public void render()
        {
            if (m_meshRender != null)
            {
                m_meshRender.render();
            }
        }
    }
}