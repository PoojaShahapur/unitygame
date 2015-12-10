namespace SDK.Lib
{
    /**
     * @brief 地形的几何数据，地形希望尽量是正方形的，并且一个 Grid 尽量正好是四个顶点，宽度尽量是 2 的整数次幂
     */
    public class TerrainPageCfg
    {
        // 每一个 Grid 就是四个顶点，这样编辑器写的时候阻挡点类似的比较好处理，这两个值基本是固定的，一个 Grid 就是一个 segment
        protected int m_xGridWidth = 1;      // 一个 Grid 的宽度，世界空间中 x 轴的长度
        protected int m_zGridHeight = 1;     // 一个 Grid 的高度，世界空间中 z 轴的长度

        // 这两个值基本也是固定的
        protected int m_xGridCountPerTile = 64;     // 每一个 Tile 的 Grid 宽度数量， x 轴 Grid 的数量，默认值是 32，测试的时候使用 512
        protected int m_zGridCountPerTile = 64;     // 每一个 Tile 的 Grid 高度数量， z 轴 Grid 的数量
        // 这两个值是可变的
        protected int m_xTileCount = 16;     // 一个地形 Tile 宽度的数量，x 轴 Tile 的数量，默认值是 512 / 16
        protected int m_zTileCount = 16;     // 一个地形 Tile 高度的数量，z 轴 Tile 的数量

        protected int m_xTotalGrid;             // X 轴总共格子数
        protected int m_zTotalGrid;             // Z 轴总共格子数

        protected int m_minElevation;                   // 高度图最小高度
        protected int m_maxElevation;                   // 高度图最大高度
        protected int m_height;    // 世界空间高度图高度， Z 轴高度，这个高度要和 HeightMapMeshOne 中的 m_height 高度一样，因为计算高度依赖这个值，因为高度图暂时精度范围是 [0, 255] ，m_height 就是缩放高度图中的 [0, 1] 到具体高度，因此 m_height 这个值取值范围要和高度图的范围尽量一样 [0, 255]

        /**
         * @brief 地形配置，尽量 worldWidth 和 worldHeight 尽量相等
         * @param worldWidth 像素宽度，注意是 2 的 n 次幂 - 1 ，例如 512 ，不是 513
         */
        public TerrainPageCfg(int worldWidth = 512, int worldHeight = 512)
        {
            setPixelWidthAndHeight(worldWidth, worldHeight);

            m_minElevation = 0;
            m_maxElevation = 0xFF;      // byte 最大值 0xFF
            m_height = 128;
        }

        /**
         * @brief 
         */
        public int getXGridWidth()
        {
            return m_xGridWidth;
        }

        /**
         * @brief 
         */
        public void setXGridWidth(int value)
        {
            m_xGridWidth = value;
        }

        /**
         * @brief 
         */
        public int getZGridHeight()
        {
            return m_zGridHeight;
        }

        /**
         * @brief 
         */
        public void setZGridHeight(int value)
        {
            m_zGridHeight = value;
        }

        /**
         * @brief 
         */
        public int getXGridCountPerTile()
        {
            return m_xGridCountPerTile;
        }

        /**
         * @brief 
         */
        public void setXGridCountPerTile(int value)
        {
            m_xGridCountPerTile = value;
        }

        /**
         * @brief 
         */
        public int getZGridCountPerTile()
        {
            return m_zGridCountPerTile;
        }

        /**
         * @brief 
         */
        public void setZGridCountPerTile(int value)
        {
            m_zGridCountPerTile = value;
        }

        /**
         * @brief 
         */
        public int getXTileCount()
        {
            return m_xTileCount;
        }

        /**
         * @brief 
         */
        public void setXTileCount(int value)
        {
            m_xTileCount = value;
        }

        /**
         * @brief 
         */
        public int getZTileCount()
        {
            return m_zTileCount;
        }

        /**
         * @brief 
         */
        public void setZTileCount(int value)
        {
            m_zTileCount = value;
        }

        /**
         * @brief 获取 Tile 世界空间中的宽度，这个现在就是 Tile 中 Grid 的数量
         */
        public int getTileWorldWidth()
        {
            return m_xGridCountPerTile * m_xGridWidth;
        }

        /**
         * @brief 获取 Tile 世界空间中的深度
         */
        public int getTileWorldDepth()
        {
            return m_zGridCountPerTile * m_zGridHeight;
        }

        /**
         * @brief 设置像素的宽度和高度
         */
        public void setPixelWidthAndHeight(int worldWidth, int worldHeight)
        {
            m_xTileCount = worldWidth / getTileWorldWidth();
            m_zTileCount = worldHeight / getTileWorldDepth();

            m_xTotalGrid = m_xGridCountPerTile * m_xTileCount;
            m_zTotalGrid = m_zGridCountPerTile * m_zTileCount;
        }

        /**
         * @brief 
         */
        public int getXTotalGrid()
        {
            return m_xTotalGrid;
        }

        /**
         * @brief 
         */
        public void setXTotalGrid(int value)
        {
            m_xTotalGrid = value;
        }

        /**
         * @brief 
         */
        public int getZTotalGrid()
        {
            return m_zTotalGrid;
        }

        /**
         * @brief 
         */
        public void setZTotalGrid(int value)
        {
            m_zTotalGrid = value;
        }

        /**
         * @brief 返回世界空间中的宽度，这个宽度实际上就是格子的数量，我们规定世界空间的大小尽量是整形，不要是浮点数，这样计算方便
         */
        public int getWorldWidth()
        {
            return m_xTotalGrid * m_xGridWidth;
        }

        /**
         * @brief 返回世界空间中的深度
         */
        public int getWorldDepth()
        {
            return m_zTotalGrid * m_xGridWidth;
        }

        public int getWorldHeight()
        {
            return m_height;
        }

        public void setWorldHeight(int value)
        {
            m_height = value;
        }

        public int getMinElevation()
        {
            return m_minElevation;
        }

        public void setMinElevation(int value)
        {
            m_minElevation = value;
        }

        public int getMaxElevation()
        {
            return m_maxElevation;
        }

        public void setMaxElevation(int value)
        {
            m_maxElevation = value;
        }

        /**
         * @brief 计算 Tile 顶点 base 片段的偏移，其实就是顶点的偏移
         */
        public void calcTileSegmentOffset(ref int xSegmentOffset, ref int zSegmentOffset, int idx, int idz)
        {
            xSegmentOffset = idx * m_xGridCountPerTile;
            zSegmentOffset = idz * m_zGridCountPerTile;
        }

        /**
         * @brief 获取 Tile 在 Tile 数组中的下表索引
         */
        public int getTileIndex(int xTile, int zTile)
        {
            return zTile * m_xTileCount + xTile;
        }
    }
}