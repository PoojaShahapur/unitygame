namespace SDK.Lib
{
    /**
     * @brief 场景中 Tile 区域管理器
     */
    public class TestTileMgr
    {
        protected TestTerrainPageCfg m_terrainPageCfg;
        protected MList<TestTile> m_tileList;       // 所有的区域列表

        public TestTileMgr()
        {
            m_terrainPageCfg = new TestTerrainPageCfg();

            // TODO: 测试一个区域数据
            m_tileList = new MList<TestTile>();
            TestTile tile = new TestTile();
            m_tileList.Add(tile);
        }
    }
}