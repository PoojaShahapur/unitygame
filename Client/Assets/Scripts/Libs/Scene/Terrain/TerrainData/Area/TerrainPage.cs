namespace SDK.Lib
{
    /**
     * @brief 地形 Page
     */
    public class TerrainPage
    {
        protected MQuadTree m_quadTree;     // 保存的四叉树

        virtual public TerrainPageCfg getTerrainPageCfg()
        {
            return null;
        }

        protected void buildQuadTree()
        {
            m_quadTree = new MQuadTree(this);
        }
    }
}