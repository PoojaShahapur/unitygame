namespace SDK.Lib
{
    /**
     * @brief 基本 Mesh
     */
    public class MMesh
    {
        protected MList<MSubMesh> _subMeshes;       // 所有的子 mesh , 子 mesh 有自己单独的渲染器
        // MeshRender 主要是渲染 _geometry 中的数据的
		protected MGeometry _geometry;              // 几何数据
        protected MeshRender m_meshRender;          // Mesh 渲染器
        protected bool _boundsInvalid;
        protected bool _worldBoundsInvalid;

        public MMesh(MGeometry geometry_, MeshRender meshRender_)
        {
            _geometry = geometry_;
            m_meshRender = meshRender_;
        }

        public MGeometry getGeometry()
		{
			return _geometry;
		}

        /**
		 * @brief 无效 bounding volume
		 */
        protected void invalidateBounds()
		{
			_boundsInvalid = true;
			_worldBoundsInvalid = true;
            notifySceneBoundsInvalid();
        }

        /**
         * @brief 通知场景 Bound 发生变化
         */
        private void notifySceneBoundsInvalid()
        {

        }

        public void render()
        {
            m_meshRender.render();
        }
    }
}