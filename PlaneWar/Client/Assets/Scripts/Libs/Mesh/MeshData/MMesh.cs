namespace SDK.Lib
{
    /**
     * @brief 基本 Mesh，这个是场景中可显示的， Mesh 是可显示的 Entity，显示的几何信息在 _geometry 中，但是只使用 _geometry 中的第一个 SubGeometry
     */
    public class MMesh
    {
        protected MList<MSubMesh> mSubMeshes;       // 所有的子 mesh , 子 mesh 有自己单独的渲染器
        // MeshRender 主要是渲染 _geometry 中的数据的
		protected MGeometry mGeometry;              // 几何数据
        protected MeshRender mMeshRender;          // Mesh 渲染器
        protected bool mBoundsInvalid;
        protected bool mWorldBoundsInvalid;

        public MMesh(MGeometry geometry_, MeshRender meshRender_)
        {
            mGeometry = geometry_;
            mMeshRender = meshRender_;
        }

        public MGeometry getGeometry()
		{
			return mGeometry;
		}

        /**
		 * @brief 无效 bounding volume
		 */
        protected void invalidateBounds()
		{
			mBoundsInvalid = true;
			mWorldBoundsInvalid = true;
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
            if(this.mSubMeshes == null)
            {
                this.mSubMeshes = new MList<MSubMesh>();
            }
            this.mSubMeshes.Add(subMesh_);
        }

        public void moveToPos(int xPos, int zPos)
        {
            if (mSubMeshes != null)
            {
                for (int idx = 0; idx < mSubMeshes.Count(); ++idx)
                {
                    mSubMeshes[idx].moveToPos(xPos, zPos);
                }
            }
        }

        public void show()
        {
            // 渲染 Mesh 数据
            if (mMeshRender != null)
            {
                mMeshRender.show();
            }

            if (mSubMeshes != null)
            {
                // 渲染 SubMesh 数据
                for (int idx = 0; idx < mSubMeshes.Count(); ++idx)
                {
                    mSubMeshes[idx].show();
                }
            }
        }
    }
}