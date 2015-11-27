namespace SDK.Lib
{
    /**
     * @brief 一个 Terrain Page
     */
    public class TerrainPage
    {
        protected HeightMapData m_heightMapData;    // 高度图数据
        protected HeightMapMeshOne m_heightMapMesh;    // 高度图 Mesh
        protected MatRes m_matRes;                  // 材质资源

        public TerrainPage()
        {

        }

        /**
         * @brief 生成地形 Page
         */
        public void buildPage()
        {
            m_heightMapData = new HeightMapData();
            m_heightMapData.loadHeightMap("Terrain/terrain.png");
            //m_heightMapData.loadHeightMap("Terrain/terrain_heights.jpg");

            m_heightMapMesh = new HeightMapMeshOne(m_heightMapData, 5000, 1300, 5000, 250, 250);
            //m_heightMapMesh = new HeightMapMeshOne(m_heightMapData, 20, 20, 20, 20, 20);

            render();
        }

        protected void render()
        {
            m_heightMapMesh.render();
        }
    }
}