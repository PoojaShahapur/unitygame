using UnityEngine;

namespace SDK.Lib
{
    public class MVertexData
    {
        public Vector3[] m_vertexs;
        public Vector2[] m_uvs;
        public int[] m_indexs;

        public Vector3[] m_vertexNormals;
        public Vector4[] m_vertexTangents;

        protected ushort mSize;
        protected ushort mMaxBatchSize;

        public MVertexData()
        {
            mSize = 513;
            mMaxBatchSize = 65;

            m_vertexs = new Vector3[mMaxBatchSize * mMaxBatchSize];
            m_uvs = new Vector2[mMaxBatchSize * mMaxBatchSize];
            m_vertexNormals = new Vector3[mMaxBatchSize * mMaxBatchSize];
            m_vertexTangents = new Vector4[mMaxBatchSize * mMaxBatchSize];
            m_indexs = new int[(mMaxBatchSize - 1) * (mMaxBatchSize - 1) * 6];
        }

        public int calcTotalByte()
        {
            // 顶点， UV，法向量，切向量，索引
            return mMaxBatchSize * mMaxBatchSize * sizeof(float) * 3 + mMaxBatchSize * mMaxBatchSize * sizeof(float) * 2 + mMaxBatchSize * mMaxBatchSize * sizeof(float) * 3 + mMaxBatchSize * mMaxBatchSize * sizeof(float) * 4 + (mMaxBatchSize - 1) * (mMaxBatchSize - 1) * 6 * sizeof(int);
        }

        public void writeVertData(ByteBuffer buffer)
        {
            long count = calcTotalByte();
            long aaaa = sizeof(float);

            for (int idx = 0; idx < mMaxBatchSize * mMaxBatchSize; ++idx)
            {
                buffer.writeVector3(m_vertexs[idx]);
            }
            for (int idx = 0; idx < mMaxBatchSize * mMaxBatchSize; ++idx)
            {
                buffer.writeVector2(m_uvs[idx]);
            }
            for (int idx = 0; idx < mMaxBatchSize * mMaxBatchSize; ++idx)
            {
                buffer.writeVector3(m_vertexNormals[idx]);
            }
            for (int idx = 0; idx < mMaxBatchSize * mMaxBatchSize; ++idx)
            {
                buffer.writeVector4(m_vertexTangents[idx]);
            }
            for (int idx = 0; idx < (mMaxBatchSize - 1) * (mMaxBatchSize - 1) * 6; ++idx)
            {
                buffer.writeInt32(m_indexs[idx]);
            }
        }

        public void readVertData(ByteBuffer buffer)
        {
            for (int idx = 0; idx < mMaxBatchSize * mMaxBatchSize; ++idx)
            {
                buffer.readVector3(ref m_vertexs[idx]);
            }
            for (int idx = 0; idx < mMaxBatchSize * mMaxBatchSize; ++idx)
            {
                buffer.readVector2(ref m_uvs[idx]);
            }
            for (int idx = 0; idx < mMaxBatchSize * mMaxBatchSize; ++idx)
            {
                buffer.readVector3(ref m_vertexNormals[idx]);
            }
            for (int idx = 0; idx < mMaxBatchSize * mMaxBatchSize; ++idx)
            {
                buffer.readVector4(ref m_vertexTangents[idx]);
            }
            for (int idx = 0; idx < (mMaxBatchSize - 1) * (mMaxBatchSize - 1) * 6; ++idx)
            {
                buffer.readInt32(ref m_indexs[idx]);
            }
        }
    }
}