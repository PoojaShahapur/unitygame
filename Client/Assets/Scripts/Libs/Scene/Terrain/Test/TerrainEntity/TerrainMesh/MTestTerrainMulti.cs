namespace SDK.Lib
{
    /**
     * @brief 一个 Terrain，这个地形有多个 Tile
     */
    public class MTestTerrainMulti : MTestTerrain
    {
        protected TestHeightMapData m_heightMapData;        // 高度图数据
        protected TestHeightMapMeshMulti m_heightMapMesh;   // 高度图 Mesh
        protected TestTerrainPageCfg m_terrainPageCfg;      // Page 配置

        public MTestTerrainMulti()
        {
            m_terrainPageCfg = new TestTerrainPageCfg();
        }

        override public TestTerrainPageCfg getTerrainPageCfg()
        {
            return m_terrainPageCfg;
        }

        override public float getHeightAt(float x, float z)
        {
            return m_heightMapMesh.getHeightAt(x, z);
        }

        /**
         * @brief 生成地形 Page
         */
        override public void buildPage()
        {
            m_heightMapData = new TestHeightMapData();
            m_heightMapData.loadHeightMap("Terrain/terrain.png");

            m_terrainPageCfg.setWorldWidthAndHeight(m_heightMapData.getWidth() - 1, m_heightMapData.getHeight() - 1);
            //m_terrainPageCfg.setWorldWidthAndHeight(1000, 1000);

            //m_terrainPageCfg.setWorldWidthAndHeight(64, 64);

            m_heightMapMesh = new TestHeightMapMeshMulti(m_heightMapData, m_terrainPageCfg);

            render();
        }

        protected void render()
        {
            m_heightMapMesh.render();
        }

        override public MTestSubMesh getTileMesh(int xTile, int zTile)
        {
            return m_heightMapMesh.getTileMesh(xTile, zTile);
        }

        override public MTestSubMesh getTileMesh(int tileIndex)
        {
            return m_heightMapMesh.getTileMesh(tileIndex);
        }
    }
}