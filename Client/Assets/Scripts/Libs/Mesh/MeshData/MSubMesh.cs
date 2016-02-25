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
        protected float m_xPos;     // X 位置，记录位置调试使用
        protected float m_zPos;     // Z 位置，记录位置调试使用
        protected int m_xTile;      // Tile 的 X 位置，调试使用
        protected int m_zTile;      // Tile 的 Z 位置，调试使用

        public MSubMesh(MSubGeometryBase subGeometry_ = null, MeshRender meshRender_ = null)
        {
            m_xPos = 0;
            m_zPos = 0;
            m_xTile = 0;
            m_zTile = 0;

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
         * @brief 设置 Tile 坐标
         */
        public void setTileXZ(int xTile, int zTile)
        {
            m_xTile = xTile;
            m_zTile = zTile;

            m_meshRender.setTileXZ(xTile, zTile);
        }

        /**
         * @brief 局部空间移动 Mesh
         */
        public void moveToPos(int xPos, int zPos)
        {
            m_xPos = xPos;
            m_zPos = zPos;

            if (m_meshRender != null)
            {
                m_meshRender.moveToPos(xPos, zPos);
            }
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

        /**
         * @brief 显示
         */
        public void show()
        {
            m_meshRender.show();
        }

        /**
         * @brief 隐藏
         */
        public void hide()
        {
            m_meshRender.hide();
        }
    }
}