using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 基本的 SubGeometry，包含 UV 、法向量、切向量
     */
    public class MSubGeometry : MSubGeometryBase
    {
        protected MList<float> m_uvs;
		protected MList<float> m_secondaryUvs;
		protected MList<float> m_vertexNormals;
		protected MList<float> m_vertexTangents;
		
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

        public void updateVertexData(MList<float> vertices)
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
            int numVertices = vertices.length() / 3;
            if (numVertices != m_numVertices)
            {
                disposeAllVertexBuffers();
            }
            m_numVertices = (uint)numVertices;

            invalidateBuffers(m_verticesInvalid);
            invalidateBounds();
            updateAABox();      // 更新包围盒子
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
        public MList<float> getVertexNormalData()
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
        override protected MList<float> updateVertexNormals(MList<float> target)
		{
            invalidateBuffers(m_normalsInvalid);
			return base.updateVertexNormals(target);
		}

        override public int getVertexStride()
		{
			return 3;
		}

        override public int getVertexOffset()
		{
			return 0;
		}

        /**
         * @brief 获取法线数据
         */
        override public MList<float> getVertexNormalsData()
        {
            return m_vertexNormals;
        }

        /**
         * @brief 获取顶点法线数组
         */
        override public Vector3[] getVertexNormalArray()
        {
            getVertexNormalData();          // 确保法线是可以获取的
            Vector3[] normalArray = new Vector3[m_vertexNormals.length() / 3];
            int normalArrIdx = 0;
            for(int idx = 0; idx < m_vertexNormals.length(); idx += 3, ++normalArrIdx)
            {
                normalArray[normalArrIdx] = new Vector3(m_vertexNormals[idx], m_vertexNormals[idx + 1], m_vertexNormals[idx + 2]);
            }

            return normalArray;
        }

        override public int getVertexNormalOffset()
		{
			return 0;
		}

        override public uint getVertexNormalStride()
		{
			return 3;
		}

        override public MList<float> getUVData()
		{
            if (m_uvsDirty && m_autoGenerateUVs)
            {
                m_uvs = updateDummyUVs(m_uvs);
            }
			return m_uvs;
		}

        /**
         * @brief 获取 UV 数组数据
         */
        override public Vector2[] getUVDataArray()
        {
            Vector2[] uvArray = new Vector2[m_uvs.length() / 2];
            int uvArrIdx = 0;
            for (int idx = 0; idx < m_uvs.length(); idx += 2, ++uvArrIdx)
            {
                uvArray[uvArrIdx] = new Vector2(m_uvs[idx], m_uvs[idx + 1]);
            }

            return uvArray;
        }

        override public int getUVOffset()
		{
			return 0;
		}

        override public uint getUVStride()
		{
			return 2;
		}

        override protected MList<float> updateDummyUVs(MList<float> target)
		{
            invalidateBuffers(m_uvsInvalid);
			return base.updateDummyUVs(target);
		}

        /**
         * @brief 更新顶点切线
         */
        override protected MList<float> updateVertexTangents(MList<float> target)
        {
            invalidateBuffers(m_tangentsInvalid);
            return base.updateVertexTangents(target);
        }

        /**
         * @brief 获取切线数据
         */
        override public MList<float> getVertexTangentsData()
        {
            if (m_autoDeriveVertexTangents && m_vertexTangentsDirty)
            {
                m_vertexTangents = updateVertexTangents(m_vertexTangents);
            }
            return m_vertexTangents;
        }

        /**
         * @brief 获取顶点切线数组
         */
        override public Vector4[] getVertexTangentArray()
        {
            getVertexTangentsData();
            Vector4[] tangentArray = new Vector4[m_vertexTangents.length() / 3];
            int tangentArrIdx = 0;
            for (int idx = 0; idx < m_vertexNormals.length(); idx += 3, ++tangentArrIdx)
            {
                tangentArray[tangentArrIdx] = new Vector4(m_vertexTangents[idx], m_vertexTangents[idx + 1], m_vertexTangents[idx + 2], 0);
            }

            return tangentArray;
        }

        override public int getVertexTangentOffset()
		{
			return 0;
		}

        override public uint getVertexTangentStride()
		{
			return 3;
		}

        public void updateUVData(MList<float> uvs)
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
                m_aaBox.addPoint(m_vertexData[idx * 3], m_vertexData[idx * 3 + 1], m_vertexData[idx * 3 + 2]);
                ++idx;
            }
        }

        override public MAxisAlignedBox getAABox()
        {
            return m_aaBox;
        }
    }
}