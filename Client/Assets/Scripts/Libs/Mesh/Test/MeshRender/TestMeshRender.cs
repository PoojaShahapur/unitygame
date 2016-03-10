namespace SDK.Lib
{
    /**
     * @brief 手工设置顶点的 Mesh Render
     */
    public class TestMeshRender : AuxComponent
    {
        protected MTestSubGeometryBase m_subGeometry;       // 子几何顶点数据
        protected bool m_subGeomDirty;                  // SubGeometry 数据是否是过时的数据

        protected float m_xPos;     // X 位置，记录位置调试使用
        protected float m_zPos;     // Z 位置，记录位置调试使用
        protected int m_xTile;      // Tile 的 X 位置，调试使用
        protected int m_zTile;      // Tile 的 Z 位置，调试使用

        public TestMeshRender(MTestSubGeometryBase subGeometry_ = null)     // 需要的材质在自己的子类中去操作
        {
            m_subGeometry = subGeometry_;
            m_selfGo = UtilApi.createGameObject("MeshRender");
        }

        override protected void onPntChanged()
        {
            linkSelf2Parent();
        }

        /**
         * @brief 修改 SubGeometry 数据
         */
        public void setSubGeometry(MTestSubGeometryBase subGeometry_)
        {
            m_subGeometry = subGeometry_;
        }

        public void setTileXZ(int xTile, int zTile)
        {
            m_xTile = xTile;
            m_zTile = zTile;

            if (this.selfGo != null)
            {
                this.setSelfName("MeshRender" + "_" + m_xTile + "_" + m_zTile);
            }
        }

        /**
         * @brief 局部空间移动 Render
         */
        public void moveToPos(int xPos, int zPos)
        {
            UtilApi.setPos(this.selfGo.transform, new UnityEngine.Vector3(xPos, 0, zPos));
        }

        /**
         * @brief 子类实现具体的显示
         */
        virtual public void render()
        {

        }
    }
}