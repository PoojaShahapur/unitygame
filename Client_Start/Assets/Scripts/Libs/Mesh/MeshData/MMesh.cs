namespace SDK.Lib
{
    /**
     * @brief 基本 Mesh，这个是场景中可显示的， Mesh 是可显示的 Entity，显示的几何信息在 _geometry 中，但是只使用 _geometry 中的第一个 SubGeometry
     */
    public class MMesh
    {
        protected MList<MSubMesh> m_subMeshes;       // 所有的子 mesh , 子 mesh 有自己单独的渲染器
        // MeshRender 主要是渲染 _geometry 中的数据的
		protected MGeometry m_geometry;              // 几何数据
        protected MeshRender m_meshRender;          // Mesh 渲染器
        protected bool m_boundsInvalid;
        protected bool m_worldBoundsInvalid;

        public MMesh(MGeometry geometry_, MeshRender meshRender_)
        {
            m_geometry = geometry_;
            m_meshRender = meshRender_;
        }

        public MGeometry getGeometry()
		{
			return m_geometry;
		}

        /**
		 * @brief 无效 bounding volume
		 */
        protected void invalidateBounds()
		{
			m_boundsInvalid = true;
			m_worldBoundsInvalid = true;
            notifySceneBoundsInvalid();
        }

        /**
         * @brief 通知场景 Bound 发生变化
         */
        private void notifySceneBoundsInvalid()
        {

        }

        protected void addSubMesh(MSubMesh subMesh_)
        {
            if(this.m_subMeshes == null)
            {
                this.m_subMeshes = new MList<MSubMesh>();
            }
            this.m_subMeshes.Add(subMesh_);
        }

        public void moveToPos(int xPos, int zPos)
        {
            if (m_subMeshes != null)
            {
                for (int idx = 0; idx < m_subMeshes.Count(); ++idx)
                {
                    m_subMeshes[idx].moveToPos(xPos, zPos);
                }
            }
        }

        public void show()
        {
            // 渲染 Mesh 数据
            if (m_meshRender != null)
            {
                m_meshRender.show();
            }

            if (m_subMeshes != null)
            {
                // 渲染 SubMesh 数据
                for (int idx = 0; idx < m_subMeshes.Count(); ++idx)
                {
                    m_subMeshes[idx].show();
                }
            }
        }
    }
}