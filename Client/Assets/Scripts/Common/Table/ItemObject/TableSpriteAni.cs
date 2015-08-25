namespace SDK.Lib
{
    /**
     * @brief 精灵动画配置
     */
    public class TableSpriteAniItemBody : TableItemBodyBase
    {
        public int m_frameRate;     // 帧率 FPS，每秒帧数
        public int m_frameCount;    // 帧数，总共多少帧
        public string m_aniResNameNoExt; // 动画资源的名字，没有扩展名

        public float m_invFrameRate;    // 一帧需要的时间
        public string m_aniResName;     // 动画资源的名字，有扩展名
        public string m_aniPrefabName;  // 动画预制资源

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            bytes.readInt32(ref m_frameRate);
            bytes.readInt32(ref m_frameCount);
            UtilTable.readString(bytes, ref m_aniResNameNoExt);

            m_invFrameRate = 1 / (float)m_frameRate;
            m_aniResName = string.Format("{0}.asset", m_aniResNameNoExt);

            m_aniPrefabName = string.Format("{0}prefab.prefab", m_aniResNameNoExt);
        }
    }
}