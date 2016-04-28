﻿namespace SDK.Lib
{
    /**
     * @breif 地形全局选项
     */
    public class TerrainGlobalOption
    {
        public float mTerrainWorldSizeInAll;    // 所有地形页的大小，一定要是地形页大小的倍数
        public int mTerrainPageCount;               // 地形 Page 的数量

        public ushort mTerrainSize;         // 地形大小，每一页的大小
        public float mTerrainWorldSize;     // 地形世界大小，每一页的大小

        public ushort mMaxBatchSize;        // 地形最大批
        public ushort mMinBatchSize;        // 地形最小批

        public float mInputScale;
        public float mInputBias;

        public bool mIsReadFile;    // 是否从文件读取所有的数据
        public bool mNeedCull;      // 是否需要裁剪
        public bool mNeedSaveScene; // 是否需要保存场景，如果要保存场景，一定不要从文件读取场景

        public TerrainGlobalOption()
        {
            mTerrainWorldSizeInAll = 30000;
            mTerrainSize = 33;
            mTerrainWorldSize = 200;
            mTerrainPageCount = (int)(mTerrainWorldSizeInAll / mTerrainWorldSize);

            mMaxBatchSize = 33;
            mMinBatchSize = 17;
            mInputScale = 200;
            mInputBias = 0;

            mIsReadFile = true;
            mNeedCull = true;
            mNeedSaveScene = false;

            // 设置默认的关系
            if(mIsReadFile)
            {
                mNeedSaveScene = false;
            }
        }

        // 获取最终节点一边的个数
        public int getTreeNodeCount()
        {
            return (mTerrainSize - 1) / (mMaxBatchSize - 1);
        }

        public float getTreeNodeWorldSize()
        {
            return mTerrainWorldSize / getTreeNodeCount();
        }

        // 获取 Tree Node 的一般大小
        public int getTreeNodeHalfSize()
        {
            return (mTerrainSize - 1) / getTreeNodeCount();
        }
    }
}