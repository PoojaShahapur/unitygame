using UnityEngine;

namespace SDK.Lib
{
    public class MVertexData
    {
        public Vector3[] m_vertexData;
        public Vector2[] m_uvs;
        public int[] m_indices;

        public Vector3[] m_vertexNormals;
        public Vector4[] m_vertexTangents;

        protected ushort mSize;
        protected ushort mMaxBatchSize;

        public MVertexData()
        {
            mSize = 513;
            mMaxBatchSize = 65;

            m_vertexData = new Vector3[mMaxBatchSize * mMaxBatchSize];
            m_uvs = new Vector2[mMaxBatchSize * mMaxBatchSize];
            m_indices = new int[(mMaxBatchSize - 1) * (mMaxBatchSize - 1) * 6];
        }
    }
}