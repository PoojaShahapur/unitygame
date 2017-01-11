namespace SDK.Lib
{
    /**
     * @brief 场景中可显示的对象
     */
    public class MSubMesh
    {
        protected MMesh mParentMesh;            // Parent Mesh 
		protected MSubGeometryBase mSubGeometry;   // 显示的子 Geometry
        protected MeshRender mMeshRender;      // SubMesh 渲染器，和 Mesh 渲染器是一样的
        protected float mXPos;     // X 位置，记录位置调试使用
        protected float mZPos;     // Z 位置，记录位置调试使用
        protected int mXTile;      // Tile 的 X 位置，调试使用
        protected int mZTile;      // Tile 的 Z 位置，调试使用

        public MSubMesh(MSubGeometryBase subGeometry_ = null, MeshRender meshRender_ = null)
        {
            mXPos = 0;
            mZPos = 0;
            mXTile = 0;
            mZTile = 0;

            mSubGeometry = subGeometry_;
            mMeshRender = meshRender_;
        }

        /**
         * @brief 修改 SubGeometry 数据
         */
        public void setSubGeometry(MSubGeometryBase subGeometry_)
        {
            mSubGeometry = subGeometry_;
        }

        /**
         * @brief 修改渲染器
         */
        public void setMeshRender(MeshRender meshRender_)
        {
            mMeshRender = meshRender_;
        }

        /**
         * @brief 设置 Tile 坐标
         */
        public void setTileXZ(int xTile, int zTile)
        {
            mXTile = xTile;
            mZTile = zTile;

            mMeshRender.setTileXZ(xTile, zTile);
        }

        /**
         * @brief 局部空间移动 Mesh
         */
        public void moveToPos(int xPos, int zPos)
        {
            mXPos = xPos;
            mZPos = zPos;

            if (mMeshRender != null)
            {
                mMeshRender.moveToPos(xPos, zPos);
            }
        }

        /**
         * @brief 渲染数据
         */
        public void show()
        {
            if (mMeshRender != null)
            {
                mMeshRender.show();
            }
        }

        /**
         * @brief 隐藏
         */
        public void hide()
        {
            mMeshRender.hide();
        }

        public MAxisAlignedBox getAABox()
        {
            return mSubGeometry.getAABox();
        }
    }
}