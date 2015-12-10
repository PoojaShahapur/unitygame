namespace SDK.Lib
{
    /**
     * @brief 场景中 Tile 区域管理器
     */
    public class TileMgr
    {
        protected TerrainPageCfg m_terrainPageCfg;
        protected MList<Tile> m_tileList;       // 所有的区域列表

        public TileMgr()
        {
            m_terrainPageCfg = new TerrainPageCfg();

            // TODO: 测试一个区域数据
            m_tileList = new MList<Tile>();
            Tile tile = new Tile();
            m_tileList.Add(tile);
        }
    }
}