namespace SDK.Lib
{
    /**
     * @brief 基本 Mesh
     */
    public class MMesh
    {
        protected MList<MSubMesh> _subMeshes;       // 所有的子 mesh
		protected MGeometry _geometry;              // 几何数据
        protected bool _boundsInvalid;
        protected bool _worldBoundsInvalid;
        protected MeshRender m_meshRender;          // Mesh 渲染器

        public MMesh(MGeometry geometry, MatRes material = null)
        {

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
    }
}