namespace SDK.Lib
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
            mTerrainWorldSizeInAll = 200;
            mTerrainSize = 65;
            mTerrainWorldSize = 200;
            mTerrainPageCount = (int)(mTerrainWorldSizeInAll / mTerrainWorldSize);

            mMaxBatchSize = 17;
            mMinBatchSize = 9;
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
    }
}