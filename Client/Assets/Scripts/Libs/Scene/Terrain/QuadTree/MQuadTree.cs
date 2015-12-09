namespace SDK.Lib
{
    /**
     * @brief 四叉树
     */
    public class MQuadTree : MPartition3D
    {
        protected TerrainPage m_terrain;    // 地形
        protected int m_maxDepth;           // 四叉树的深度，深度值从 0 开始，最大值就是最后一个值，不是最后一个值 + 1
        protected int m_size;               // 地形的世界空间中的大小，地形需要是正方形的，并且这个大小的公式是: size = Tile 世界空间大小 * (2 ^ depth)
        protected int m_height;             // 世界空间地形高度

        public MQuadTree(TerrainPage terrain)
            : base(null)
        {
            m_terrain = terrain;
            m_maxDepth = 0;
            m_size = m_terrain.getTerrainPageCfg().getWorldWidth();
            m_height = m_terrain.getTerrainPageCfg().getWorldHeight();
            m_maxDepth = calcDepth();

            buildTree();
        }

        protected int calcDepth()
        {
            m_maxDepth = 0;
            int splitTile = m_size / UtilApi.powerTwo(m_maxDepth);
            int tileSize = m_terrain.getTerrainPageCfg().getTileWorldWidth();   // 获取 Area 的世界空间宽度
            while (splitTile > tileSize)    // 一定不能大于地形
            {
                splitTile = m_size / UtilApi.powerTwo(m_maxDepth);
                ++m_maxDepth;
            }
            return m_maxDepth;
        }

        protected void buildTree()
        {
            m_rootNode = new MQuadTreeNode(m_terrain, m_maxDepth, m_size, m_height);
        }
    }
}