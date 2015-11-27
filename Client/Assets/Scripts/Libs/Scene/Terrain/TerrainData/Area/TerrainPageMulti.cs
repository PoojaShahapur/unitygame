﻿namespace SDK.Lib
{
    /**
     * @brief 一个 Terrain Page，这个地形 Page 有多个 Multi
     */
    public class TerrainPageMulti
    {
        protected HeightMapData m_heightMapData;        // 高度图数据
        protected HeightMapMeshMulti m_heightMapMesh;   // 高度图 Mesh
        protected TerrainPageCfg m_terrainPageCfg;      // Page 配置

        public TerrainPageMulti()
        {
            m_terrainPageCfg = new TerrainPageCfg();
        }

        /**
         * @brief 生成地形 Page
         */
        public void buildPage()
        {
            m_heightMapData = new HeightMapData();
            m_heightMapData.loadHeightMap("Terrain/terrain.png");

            m_terrainPageCfg.setPixelWidthAndHeight(m_heightMapData.getWidth() - 1, m_heightMapData.getHeight() - 1);

            m_heightMapMesh = new HeightMapMeshMulti(m_heightMapData, m_terrainPageCfg);

            render();
        }

        protected void render()
        {
            m_heightMapMesh.render();
        }
    }
}