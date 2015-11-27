namespace SDK.Lib
{
    /**
     * @brief 多个 Area 的 HeightMapMesh
     */
    public class HeightMapMeshMulti : MMesh
    {
        protected TerrainPageCfg m_terrainPageCfg;  // Page 配置

        /**
         * @brief 构造函数
         */
        public HeightMapMeshMulti(HeightMapData heightMap, TerrainPageCfg terrainPageCfg)
            : base(new MGeometry(), new SingleAreaRender())
        {
            m_terrainPageCfg = terrainPageCfg;
            buildMutilAreaMesh(heightMap, terrainPageCfg);
        }

        /**
         * @brief 生成多个区域地图
         */
        protected void buildMutilAreaMesh(HeightMapData heightMap, TerrainPageCfg terrainPageCfg)
        {
            int idx = 0;
            int idz = 0;
            for(idz = 0; idz < terrainPageCfg.getZAreaCount(); ++idz)
            {
                for (idx = 0; idx < terrainPageCfg.getXAreaCount(); ++idx)
                {
                    buildArea(idx, idz, heightMap, terrainPageCfg);
                }
            }
        }

        /**
         * @brief 生成一个 Page 中的一个 Area
         */
        protected void buildArea(int idx, int idz, HeightMapData heightMap, TerrainPageCfg terrainPageCfg)
        {

        }
    }
}