namespace SDK.Lib
{
    /**
     * @brief 一个 Terrain Page，这个地形 Page 只有一个 Tile
     */
    public class TerrainPageOne : TerrainPage
    {
        protected HeightMapData m_heightMapData;    // 高度图数据
        protected HeightMapMeshOne m_heightMapMesh;    // 高度图 Mesh
        protected MatRes m_matRes;                  // 材质资源

        public TerrainPageOne()
        {

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
            m_heightMapData.loadHeightMap("Terrain/terrain.png");
            //m_heightMapData.loadHeightMap("Terrain/terrain_heights.jpg");

            m_heightMapMesh = new HeightMapMeshOne(m_heightMapData, 5000, 1300, 5000, 250, 250);
            //m_heightMapMesh = new HeightMapMeshOne(m_heightMapData, 20, 20, 20, 20, 20);
            //m_heightMapMesh = new HeightMapMeshOne(m_heightMapData, 128, 128, 128, 128, 128);

            render();
        }

        protected void render()
        {
            m_heightMapMesh.render();
        }
    }
}