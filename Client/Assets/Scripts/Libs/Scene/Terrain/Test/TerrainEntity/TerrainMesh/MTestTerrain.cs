namespace SDK.Lib
{
    /**
     * @brief 地形
     */
    public class MTestTerrain
    {
        protected MTestQuadTree m_quadTree;     // 保存的四叉树

        public MTestQuadTree getQuadTree()
        {
            return m_quadTree;
        }

        virtual public TestTerrainPageCfg getTerrainPageCfg()
        {
            return null;
        }

        public void buildQuadTree()
        {
            m_quadTree = new MTestQuadTree(this);
        }

        /**
         * @brief 根据 Tile 在 Tile 坐标系中的坐标获取 Tile 对应的 Mesh
         * Tile 坐标系是[向右 X 向下]
         */
        virtual public MTestSubMesh getTileMesh(int xTile, int zTile)
        {
            return null;
        }

        /**
         * @brief 根据 Tile 在 Tile 数组中的索引获取 Tile 对应的 Mesh
         */
        virtual public MTestSubMesh getTileMesh(int tileIndex)
        {
            return null;
        }

        virtual public void buildPage()
        {

        }

        public void updateClip()
        {
            MList<MPlane> planes = new MList<MPlane>();
            MPlane[] planesArray = Ctx.m_instance.m_camSys.getFrustumPlanes();
            for (int idx = 0; idx < planesArray.Length; ++idx)
            {
                planes.Add(planesArray[idx]);
            }
            m_quadTree.updateClip(planes);
        }

        virtual public float getHeightAt(float x, float z)
        {
            return 0.0f;
        }
    }
}