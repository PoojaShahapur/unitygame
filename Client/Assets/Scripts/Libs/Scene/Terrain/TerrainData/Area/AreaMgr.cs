namespace SDK.Lib
{
    /**
     * @brief 场景中 Area 区域管理器
     */
    public class AreaMgr
    {
        protected TerrainPageCfg m_terrainPageCfg;
        protected MList<Area> m_areaList;       // 所有的区域列表

        public AreaMgr()
        {
            m_terrainPageCfg = new TerrainPageCfg();

            // TODO: 测试一个区域数据
            m_areaList = new MList<Area>();
            Area area = new Area();
            m_areaList.Add(area);
        }
    }
}