﻿namespace SDK.Lib
{
    /**
     * @brief 一个 Terrain，这个地形有多个 Tile
     */
    public class MTerrainMulti : MTerrainOld
    {
        protected HeightMapData m_heightMapData;        // 高度图数据
        protected HeightMapMeshMulti m_heightMapMesh;   // 高度图 Mesh
        protected TerrainPageCfg m_terrainPageCfg;      // Page 配置

        public MTerrainMulti()
        {
            m_terrainPageCfg = new TerrainPageCfg();
        }

        override public void init()
        {
            base.init();
            mHeightData = new float[m_heightMapData.getWidth() * m_heightMapData.getHeight()];
            getHeightData();
        }

        protected void getHeightData()
        {
            int srcy = 0;
            float height = 0;
            for (int idy = 0; idy < mSize; ++idy)
            {
                srcy = mSize - idy - 1;
                for(int idx = 0; idx < mSize; ++idx)
                {
                    height = m_heightMapData.getOrigHeight(idx, idy);
                    mHeightData[idy * m_heightMapData.getWidth() + idx] = height * m_terrainPageCfg.getInputScale() + m_terrainPageCfg.getInputBias();
                }
            }
        }

        override public TerrainPageCfg getTerrainPageCfg()
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
            m_heightMapData = new HeightMapData();
            m_heightMapData.loadHeightMap("Materials/Texture/Terrain/terrain.png");

            //m_terrainPageCfg.setWorldWidthAndHeight(m_heightMapData.getWidth() - 1, m_heightMapData.getHeight() - 1);
            m_terrainPageCfg.setWorldWidthAndHeight(1024, 1024);

            //m_terrainPageCfg.setWorldWidthAndHeight(64, 64);

            m_heightMapMesh = new HeightMapMeshMulti(m_heightMapData, m_terrainPageCfg);

            show();
        }

        protected void show()
        {
            m_heightMapMesh.show();
        }

        override public MSubMesh getTileMesh(int xTile, int zTile)
        {
            return m_heightMapMesh.getTileMesh(xTile, zTile);
        }

        override public MSubMesh getTileMesh(int tileIndex)
        {
            return m_heightMapMesh.getTileMesh(tileIndex);
        }
    }
}