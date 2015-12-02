namespace SDK.Lib
{
    /**
     * @brief 四叉树
     */
    public class MQuadTree : MPartition3D
    {
        public MQuadTree(int maxDepth, float size, float height = 1000000)
            : base(new MQuadTreeNode(maxDepth, size, height))
        {
            
        }
    }
}