using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @breif 主要进行不可见检测
     */
    public class TerrainVisibleCheck
    {
        protected int mCurIndex;        // 当前的索引
        protected int mPreIndex;        // 之前的索引
        protected MDictionary<MTerrainQuadTreeNode, bool>[] mTreeNode2VisibleDic;
        protected MList<MTerrainQuadTreeNode>[] mVisibleTreeNodeList;       // 当前可视化 TreeNode 列表
        protected MList<MTerrain> mWillRemoveTerrainList;

        public TerrainVisibleCheck()
        {
            init();
        }

        public void init()
        {
            mCurIndex = 0;
            mPreIndex = (mCurIndex + 1) % 2;
            mTreeNode2VisibleDic = new MDictionary<MTerrainQuadTreeNode, bool>[2];
            mVisibleTreeNodeList = new MList<MTerrainQuadTreeNode>[2];

            int idx = 0;
            while(idx < 2)
            {
                mTreeNode2VisibleDic[idx] = new MDictionary<MTerrainQuadTreeNode, bool>();
                mVisibleTreeNodeList[idx] = new MList<MTerrainQuadTreeNode>();
                ++idx;
            }

            mWillRemoveTerrainList = new MList<MTerrain>();
        }

        public void addVisibleTreeNode(MTerrainQuadTreeNode treeNode)
        {
            if(!mTreeNode2VisibleDic[mCurIndex].ContainsKey(treeNode))
            {
                mTreeNode2VisibleDic[mCurIndex][treeNode] = true;
                mVisibleTreeNodeList[mCurIndex].Add(treeNode);
            }
            else
            {
            }
        }

        // 检查当前可见
        public void checkVisible()
        {
            int idx = 0;
            int len = mVisibleTreeNodeList[mCurIndex].Count();
            if(mVisibleTreeNodeList[mPreIndex].Count() > 0)
            {
                while(idx < len)
                {
                    if(mTreeNode2VisibleDic[mPreIndex].ContainsKey(mVisibleTreeNodeList[mCurIndex][idx]))
                    {
                        mTreeNode2VisibleDic[mPreIndex].Remove(mVisibleTreeNodeList[mCurIndex][idx]);
                        mVisibleTreeNodeList[mPreIndex].Remove(mVisibleTreeNodeList[mCurIndex][idx]);
                    }
                    ++idx;
                }
            }

            idx = 0;
            len = mVisibleTreeNodeList[mPreIndex].Count();
            while(idx < len)
            {
                mVisibleTreeNodeList[mPreIndex][idx].hide(Ctx.mInstance.mCamSys.getLocalCamera());
                ++idx;
            }

            mTreeNode2VisibleDic[mPreIndex].Clear();
            mVisibleTreeNodeList[mPreIndex].Clear();

            mPreIndex = mCurIndex;
            mCurIndex = (mCurIndex + 1) % 2;
        }

        public void addWillRemoveTerrain(MTerrain ter)
        {
            mWillRemoveTerrainList.Add(ter);
        }

        public void delayRemoveTerrain()
        {
            int idx = 0;
            int len = mWillRemoveTerrainList.Count();
            while (idx < len)
            {
                mWillRemoveTerrainList[idx].getParentSceneNode().detachObject(mWillRemoveTerrainList[idx].getName());
                mWillRemoveTerrainList[idx].onFirstShow();
                ++idx;
            }

            mWillRemoveTerrainList.Clear();
        }
    }
}