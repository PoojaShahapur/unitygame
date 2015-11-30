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
        protected int m_xGridCountPerArea = 32;     // 每一个 Area 的 Grid 宽度数量， x 轴 Grid 的数量，默认值是 32，测试的时候使用 512
        protected int m_zGridCountPerArea = 32;     // 每一个 Area 的 Grid 高度数量， z 轴 Grid 的数量
        // 这两个值是可变的
        protected int m_xAreaCount = 16;     // 一个地形 Area 宽度的数量，x 轴 Area 的数量，默认值是 512 / 16
        protected int m_zAreaCount = 16;     // 一个地形 Area 高度的数量，z 轴 Area 的数量

        protected int m_xTotalGrid;             // X 轴总共格子数
        protected int m_zTotalGrid;             // Z 轴总共格子数

        protected int m_minElevation;                   // 高度图最小高度
        protected int m_maxElevation;                   // 高度图最大高度
        protected int m_height;    // 世界空间高度图高度， Z 轴高度，这个高度要和 HeightMapMeshOne 中的 m_height 高度一样，因为计算高度依赖这个值

        /**
         * @brief 地形配置，尽量 pixelWidth 和 pixelHeight 尽量相等
         * @param pixelWidth 像素宽度，注意是 2 的 n 次幂 - 1 ，例如 512 ，不是 513
         */
        public TerrainPageCfg(int pixelWidth = 512, int pixelHeight = 512)
        {
            setPixelWidthAndHeight(pixelWidth, pixelHeight);

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
        public int getXGridCountPerArea()
        {
            return m_xGridCountPerArea;
        }

        /**
         * @brief 
         */
        public void setXGridCountPerArea(int value)
        {
            m_xGridCountPerArea = value;
        }

        /**
         * @brief 
         */
        public int getZGridCountPerArea()
        {
            return m_zGridCountPerArea;
        }

        /**
         * @brief 
         */
        public void setZGridCountPerArea(int value)
        {
            m_zGridCountPerArea = value;
        }

        /**
         * @brief 
         */
        public int getXAreaCount()
        {
            return m_xAreaCount;
        }

        /**
         * @brief 
         */
        public void setXAreaCount(int value)
        {
            m_xAreaCount = value;
        }

        /**
         * @brief 
         */
        public int getZAreaCount()
        {
            return m_zAreaCount;
        }

        /**
         * @brief 
         */
        public void setZAreaCount(int value)
        {
            m_zAreaCount = value;
        }

        /**
         * @brief 获取 Area 世界空间中的宽度，这个现在就是 Area 中 Grid 的数量
         */
        public int getAreaWorldWidth()
        {
            return m_xGridCountPerArea * m_xGridWidth;
        }

        /**
         * @brief 获取 Area 世界空间中的深度
         */
        public int getAreaWorldDepth()
        {
            return m_zGridCountPerArea * m_zGridHeight;
        }

        /**
         * @brief 设置像素的宽度和高度
         */
        public void setPixelWidthAndHeight(int pixelWidth, int pixelHeight)
        {
            m_xAreaCount = pixelWidth / m_xGridCountPerArea;
            m_zAreaCount = pixelHeight / m_zGridCountPerArea;

            m_xTotalGrid = m_xGridCountPerArea * m_xAreaCount;
            m_zTotalGrid = m_zGridCountPerArea * m_zAreaCount;
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
         * @brief 计算 Area 顶点 base 片段的偏移，其实就是顶点的偏移
         */
        public void calcAreaSegmentOffset(ref int xSegmentOffset, ref int zSegmentOffset, int idx, int idz)
        {
            xSegmentOffset = idx * m_xGridCountPerArea;
            zSegmentOffset = idz * m_zGridCountPerArea;
        }
    }
}