namespace SDK.Lib
{
    /**
     * @brief 地形的几何数据，地形希望尽量是正方形的，并且一个 Grid 尽量正好是四个顶点，宽度尽量是 2 的整数次幂
     */
    public class TerrainPageCfg
    {
        // 每一个 Grid 就是四个顶点，这样编辑器写的时候阻挡点类似的比较好处理
        protected int m_gridWidth = 1;      // 一个 Grid 的宽度，世界空间中 x 轴的长度
        protected int m_gridHeight = 1;     // 一个 Grid 的高度，世界空间中 z 轴的长度

        protected int m_xGridCount = 4;     // 每一个 Area 的 Grid 宽度数量， x 轴 Grid 的数量
        protected int m_zGridCount = 4;     // 每一个 Area 的 Grid 高度数量， z 轴 Grid 的数量

        protected int m_xAreaCount = 1;     // 一个地形 Area 宽度的数量，x 轴 Area 的数量
        protected int m_zAreaCount = 1;     // 一个地形 Area 高度的数量，z 轴 Area 的数量
    }
}