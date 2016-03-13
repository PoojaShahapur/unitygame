namespace SDK.Lib
{
    /**
     * @brief 地形
     */
    public class MTerrainOld
    {
        protected MQuadTree m_quadTree;     // 保存的四叉树
        protected float[] mHeightData;
        protected float mWorldSize;
        protected int mSize;
        protected int mMaxBatchSize;
        protected int mMinBatchSize;
        protected MVector3 mPos;
        protected MQuadTreeNode mQuadTree;
        protected int mNumLodLevels;
        protected int mNumLodLevelsPerLeafNode;
        protected int mTreeDepth;

        virtual public void init()
        {

        }

        public MQuadTree getQuadTree()
        {
            return m_quadTree;
        }

        virtual public TerrainPageCfg getTerrainPageCfg()
        {
            return null;
        }

        public void buildQuadTree()
        {
            m_quadTree = new MQuadTree(this);
        }

        /**
         * @brief 根据 Tile 在 Tile 坐标系中的坐标获取 Tile 对应的 Mesh
         * Tile 坐标系是[向右 X 向下]
         */
        virtual public MSubMesh getTileMesh(int xTile, int zTile)
        {
            return null;
        }

        /**
         * @brief 根据 Tile 在 Tile 数组中的索引获取 Tile 对应的 Mesh
         */
        virtual public MSubMesh getTileMesh(int tileIndex)
        {
            return null;
        }

        virtual public void buildPage()
        {

        }

        public void updateClip()
        {
            MList<MPlane> planes = Ctx.m_instance.m_camSys.getFrustumPlanes();
            m_quadTree.updateClip(planes);
        }

        virtual public float getHeightAt(float x, float z)
        {
            return 0.0f;
        }
    }
}