namespace SDK.Lib
{
    /**
     * @breif 地形全局选项
     */
    public class TerrainGlobalOption
    {
        public ushort mTerrainSize;         // 地形大小
        public float mTerrainWorldSize;     // 地形世界大小

        public ushort mMaxBatchSize;        // 地形最大批
        public ushort mMinBatchSize;        // 地形最小批

        public float mInputScale;
        public float mInputBias;

        public bool mIsReadFile; // 是否从文件读取所有的数据

        public TerrainGlobalOption()
        {
            mTerrainSize = 65;
            mTerrainWorldSize = 200;

            mMaxBatchSize = 17;
            mMinBatchSize = 9;
            mInputScale = 200;
            mInputBias = 0;

            mIsReadFile = false;
        }
    }
}