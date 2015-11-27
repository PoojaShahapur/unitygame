﻿namespace SDK.Lib
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
        protected int m_xGridCountPerArea = 16;     // 每一个 Area 的 Grid 宽度数量， x 轴 Grid 的数量
        protected int m_zGridCountPerArea = 16;     // 每一个 Area 的 Grid 高度数量， z 轴 Grid 的数量
        // 这两个值是可变的
        protected int m_xAreaCount = 32;     // 一个地形 Area 宽度的数量，x 轴 Area 的数量，默认值是 512 / 16
        protected int m_zAreaCount = 32;     // 一个地形 Area 高度的数量，z 轴 Area 的数量

        protected int m_xTotalGrid;             // X 轴总共格子数
        protected int m_zTotalGrid;             // Z 轴总共格子数

        /**
         * @brief 地形配置，尽量 pixelWidth 和 pixelHeight 尽量相等
         * @param pixelWidth 像素宽度，注意是 2 的 n 次幂 - 1 ，例如 512 ，不是 513
         */
        public TerrainPageCfg(int pixelWidth = 512, int pixelHeight = 512)
        {
            m_xAreaCount = pixelWidth / m_xGridCountPerArea;
            m_zAreaCount = pixelHeight / m_zGridCountPerArea;
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
         * @brief 设置像素的宽度和高度
         */
        public void setPixelWidthAndHeight(int pixelWidth, int pixelHeight)
        {
            m_xAreaCount = pixelWidth / m_xGridCountPerArea;
            m_zAreaCount = pixelHeight / m_zGridCountPerArea;
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
         * @brief 计算 Area 顶点 base 顶点索引
         */
        public void calcAreaBaseVertex(ref int xBaseVertex, ref int zBaseVertex, int idx, int idz)
        {
            xBaseVertex = idx * m_xGridCountPerArea;
            zBaseVertex = idz * m_zGridCountPerArea;
        }
    }
}