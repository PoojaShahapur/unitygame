namespace SDK.Lib
{
    /**
     * @brief 地形 Page
     */
    public class TerrainPage
    {
        protected MQuadTree m_quadTree;     // 保存的四叉树

        public MQuadTree getQuadTree()
        {
            return m_quadTree;
        }

        virtual public TerrainPageCfg getTerrainPageCfg()
        {
            return null;
        }

        public void buildQuadTree()
        {
            m_quadTree = new MQuadTree(this);
        }

        /**
         * @brief 根据 Tile 在 Tile 坐标系中的坐标获取 Tile 对应的 Mesh
         * Tile 坐标系是[向右 X 向下]
         */
        virtual public MSubMesh getTileMesh(int xTile, int zTile)
        {
            return null;
        }

        /**
         * @brief 根据 Tile 在 Tile 数组中的索引获取 Tile 对应的 Mesh
         */
        virtual public MSubMesh getTileMesh(int tileIndex)
        {
            return null;
        }

        public void updateClip(MList<MPlane3D> planes)
        {
            m_quadTree.updateClip(planes);
        }
    }
}