namespace SDK.Lib
{
    /**
     * @brief 精灵动画配置
     */
    public class TableSpriteAniItemBody : TableItemBodyBase
    {
        public int mFrameRate;     // 帧率 FPS，每秒帧数
        public int mFrameCount;    // 帧数，总共多少帧
        public string mAniResNameNoExt; // 动画资源的名字，没有扩展名

        public float mInvFrameRate;    // 一帧需要的时间
        public string mAniResName;     // 动画资源的名字，有扩展名
        public string mAniPrefabName;  // 动画预制资源

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            bytes.readInt32(ref mFrameRate);
            bytes.readInt32(ref mFrameCount);
            UtilTable.readString(bytes, ref mAniResNameNoExt);

            mInvFrameRate = 1 / (float)mFrameRate;
            mAniResName = string.Format("{0}.asset", mAniResNameNoExt);

            mAniPrefabName = string.Format("{0}prefab.prefab", mAniResNameNoExt);
        }
    }
}