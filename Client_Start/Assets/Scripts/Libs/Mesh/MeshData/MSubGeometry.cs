using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 基本的 SubGeometry，包含 UV 、法向量、切向量
     */
    public class MSubGeometry : MSubGeometryBase
    {
        protected Vector2[] m_uvs;
		protected Vector2[] m_secondaryUvs;
		protected Vector3[] m_vertexNormals;
		protected Vector4[] m_vertexTangents;
		
		protected MList<bool> m_verticesInvalid;
		protected MList<bool> m_uvsInvalid;
		protected MList<bool> m_secondaryUvsInvalid;
		protected MList<bool> m_normalsInvalid;
		protected MList<bool> m_tangentsInvalid;
		
		protected uint m_numVertices;
        protected MAxisAlignedBox m_aaBox;  // 包围盒子

        public MSubGeometry()
        {
            m_aaBox = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);

            m_verticesInvalid = new MList<bool>();
            m_uvsInvalid = new MList<bool>();
            m_secondaryUvsInvalid = new MList<bool>();
            m_normalsInvalid = new MList<bool>();
            m_tangentsInvalid = new MList<bool>();

            for (int i = 0; i < 8; ++i)
            {
                m_verticesInvalid.Add(true);
                m_uvsInvalid.Add(true);
                m_secondaryUvsInvalid.Add(true);
                m_normalsInvalid.Add(true);
                m_tangentsInvalid.Add(true);
            }
        }

        /**
		 * @brief 获取顶点数量
		 */
        public uint getNumVertices()
		{
			return m_numVertices;
		}

        /**
         * @brief 获取顶点数据
         */
        //public MList<float> getVertexData()
		//{
		//	return _vertexData;
		//}

        public void updateVertexData(Vector3[] vertices)
		{
            if (m_autoDeriveVertexNormals)
            {
                m_vertexNormalsDirty = true;
            }
            if (m_autoDeriveVertexTangents)
            {
                m_vertexTangentsDirty = true;
            }
			
			m_faceNormalsDirty = true;
			
			m_vertexData = vertices;
            int numVertices = vertices.Length;
            if (numVertices != m_numVertices)
            {
                disposeAllVertexBuffers();
            }
            m_numVertices = (uint)numVertices;

            invalidateBuffers(m_verticesInvalid);
            invalidateBounds();
            //updateAABox();      // 更新包围盒子
        }

        override protected void invalidateBuffers(MList<bool> invalid)
		{
            for (int i = 0; i < 8; ++i)
            {
                invalid[i] = true;
            }
		}

        protected void disposeAllVertexBuffers()
        {

        }

        /**
         * @brief 获取顶点法线信息
         */
        override public Vector3[] getVertexNormalsData()
		{
            if (m_autoDeriveVertexNormals && m_vertexNormalsDirty)
            {
                m_vertexNormals = updateVertexNormals(m_vertexNormals);
            }
			return m_vertexNormals;
		}

        /**
         * @brief 更新顶点法线信息
         */
        override protected Vector3[] updateVertexNormals(Vector3[] target)
		{
            invalidateBuffers(m_normalsInvalid);
			return base.updateVertexNormals(target);
		}

        override public int getVertexStride()
		{
			return 1;
		}

        override public int getVertexOffset()
		{
			return 0;
		}

        override public int getVertexNormalOffset()
		{
			return 0;
		}

        override public uint getVertexNormalStride()
		{
			return 1;
		}

        override public Vector2[] getUVData()
		{
            if (m_uvsDirty && m_autoGenerateUVs)
            {
                m_uvs = updateDummyUVs(m_uvs);
            }
			return m_uvs;
		}

        override public int getUVOffset()
		{
			return 0;
		}

        override public uint getUVStride()
		{
			return 1;
		}

        override protected Vector2[] updateDummyUVs(Vector2[] target)
		{
            invalidateBuffers(m_uvsInvalid);
			return base.updateDummyUVs(target);
		}

        /**
         * @brief 更新顶点切线
         */
        override protected Vector4[] updateVertexTangents(Vector4[] target)
        {
            invalidateBuffers(m_tangentsInvalid);
            return base.updateVertexTangents(target);
        }

        /**
         * @brief 获取切线数据
         */
        override public Vector4[] getVertexTangentsData()
        {
            if (m_autoDeriveVertexTangents && m_vertexTangentsDirty)
            {
                m_vertexTangents = updateVertexTangents(m_vertexTangents);
            }
            return m_vertexTangents;
        }

        override public int getVertexTangentOffset()
		{
			return 0;
		}

        override public uint getVertexTangentStride()
		{
			return 1;
		}

        public void updateUVData(Vector2[] uvs)
		{
            if (m_autoDeriveVertexTangents)
            {
                m_vertexTangentsDirty = true;
            }
			m_faceTangentsDirty = true;
			m_uvs = uvs;
            invalidateBuffers(m_uvsInvalid);
        }

        public void updateAABox()
        {
            int idx = 0;
            while (idx < m_numVertices)
            {
                m_aaBox.addPoint(m_vertexData[idx * 3].x, m_vertexData[idx * 3].y, m_vertexData[idx * 3].z);
                ++idx;
            }
        }

        override public MAxisAlignedBox getAABox()
        {
            return m_aaBox;
        }
    }
}