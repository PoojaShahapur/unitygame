namespace SDK.Lib
{
    /**
     * @brief 地形的几何数据
     */
    public class TerrainPageCfg
    {
        protected int m_xGridWidth = 1;     // 一个 Grid 的宽度
        protected int m_zGridWidth = 1;

        protected int m_xGridCount = 4;     // 每一个 Area 的 Grid 数量
        protected int m_zGridCount = 4;

        protected int m_xAreaCount = 1;     // 一个地形 Area 的数量
        protected int m_zAreaCount = 1;
    }
}