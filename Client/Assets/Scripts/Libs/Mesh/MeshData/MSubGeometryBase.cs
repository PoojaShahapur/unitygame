using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief SubGeometryBase 基类，只包含基本的顶点索引数据
     */
    public class MSubGeometryBase : MISubGeometry
    {
        protected MGeometry m_parentGeometry;
        protected MList<float> m_vertexData;
        protected bool m_faceNormalsDirty;
        protected bool m_faceTangentsDirty;
        protected MList<float> m_faceTangents;
        protected MList<int> m_indices;
        protected uint m_numIndices;
        protected MList<Boolean> m_indicesInvalid;
        protected uint m_numTriangles;

        protected bool m_autoDeriveVertexNormals;
        protected bool m_autoDeriveVertexTangents;
        protected bool m_autoGenerateUVs;
        protected bool m_useFaceWeights;         // 计算顶点法向量的时候，使用面法向量插值生成
        protected bool m_vertexNormalsDirty;
        protected bool m_vertexTangentsDirty;

        protected MList<float> m_faceNormals;
        protected MList<float> m_faceWeights;

        protected bool m_uvsDirty = true;

        public MSubGeometryBase()
        {
            m_faceNormalsDirty = true;
            m_faceTangentsDirty = true;
            m_indicesInvalid = new MList<Boolean>(8);

            m_autoDeriveVertexNormals = true;
            m_autoDeriveVertexTangents = true;
            m_autoGenerateUVs = false;
            m_useFaceWeights = false;
            m_vertexNormalsDirty = true;
            m_vertexTangentsDirty = true;

            for(int idx = 0; idx < 8; ++idx)
            {
                m_indicesInvalid.Add(true);
            }
        }

        public MGeometry getParentGeometry()
        {
            return m_parentGeometry;
        }

        public void setParentGeometry(MGeometry parentGeom)
        {
            m_parentGeometry = parentGeom;
        }

        /**
         * @brief 获取顶点数据
         */
        public MList<float> getVertexData()
        {
            return m_vertexData;
        }

        /**
         * @brief 获取顶点向量数据
         */
        public Vector3[] getVertexDataArray()
        {
            Vector3[] vertexVec = new Vector3[getVertexDataCount()];        // 必然是 3 个
            int idxVec = 0;
            for (int idx = 0; idx < m_vertexData.length(); idx += getVertexStride(), ++idxVec)
            {
                vertexVec[idxVec] = new Vector3(m_vertexData[idx], m_vertexData[idx + 1], m_vertexData[idx + 2]);
            }

            return vertexVec;
        }

        /**
         * @brief 获取顶点数量
         */
        public int getVertexDataCount()
        {
            return m_vertexData.length() / getVertexStride();
        }

        /**
		 * @brief 获取索引数据
		 */
        public MList<int> getIndexData()
        {
            return m_indices;
        }

        /**
         * @brief 获取三角形的数量，就是获取面的数量
         */
        public int getTriangleCount()
        {
            return m_indices.length() / 3;
        }

        /**
         * @brief 获取法线数据
         */
        virtual public MList<float> getVertexNormalsData()
        {
            throw new Exception();
        }

        /**
         * @brief 获取切线数据
         */
        virtual public MList<float> getVertexTangentsData()
        {
            throw new Exception();
        }

        public bool getAutoDeriveVertexNormals()
        {
            return m_autoDeriveVertexNormals;
        }

        public void setAutoDeriveVertexNormals(bool value)
        {
            m_autoDeriveVertexNormals = value;
        }

        public bool getAutoDeriveVertexTangents()
        {
            return m_autoDeriveVertexTangents;
        }

        public void setAutoDeriveVertexTangents(bool value)
        {
            m_autoDeriveVertexTangents = value;
        }

        protected void invalidateBounds()
        {
            if (m_parentGeometry != null)
            {
                m_parentGeometry.invalidateBounds(this as MISubGeometry);
            }
        }

        virtual public int getVertexStride()
        {
            throw new Exception();
        }

        virtual public int getVertexOffset()
        {
            throw new Exception();
        }

        /**
         * @brief 获取顶点法线数组
         */
        virtual public Vector3[] getVertexNormalArray()
        {
            throw new Exception();
        }

        virtual public uint getVertexNormalStride()
        {
            throw new Exception();
        }

        /**
         * @brief 获取顶点偏移
         */
        virtual public int getVertexNormalOffset()
        {
            throw new Exception();
        }

        /**
         * @brief 获取 UV 数据
         */
        virtual public MList<float> getUVData()
        {
            throw new Exception();
        }

        /**
         * @brief 获取 UV 数组数据
         */
        virtual public Vector2[] getUVDataArray()
        {
            throw new Exception();
        }

        virtual public uint getUVStride()
        {
            throw new Exception();
        }

        virtual public int getUVOffset()
        {
            throw new Exception();
        }

        /**
         * @brief 获取顶点切线数组
         */
        virtual public Vector4[] getVertexTangentArray()
        {
            throw new Exception();
        }

        virtual public uint getVertexTangentStride()
        {
            throw new Exception();
        }

        virtual public int getVertexTangentOffset()
        {
            throw new Exception();
        }

        /**
         * @brief 获取顶点颜色
         */
        public MList<byte> getVertexColor()
        {
            return null;
        }

        /**
         * @brief 获取顶点颜色数组
         */
        public Color32[] getVectexColorArray()
        {
            return null;
        }

        /**
         * @brief 清除 Cpu 缓冲区
         */
        virtual public void clear()
        {

        }

        /**
         * @brief 更新面的切线
         */
        protected void updateFaceTangents()
        {
            uint i = 0;
            int index1 = 0, index2 = 0, index3 = 0;
            uint len = (uint)m_indices.length();
            uint ui = 0, vi = 0;
            float v0 = 0;
            float dv1 = 0, dv2 = 0;
            float denom = 0;
            float x0 = 0, y0 = 0, z0 = 0;
            float dx1 = 0, dy1 = 0, dz1 = 0;
            float dx2 = 0, dy2 = 0, dz2 = 0;
            float cx = 0, cy = 0, cz = 0;
            MList<float> vertices = m_vertexData;
            MList<float> uvs = getUVData();
            int posStride = (int)getVertexStride();
            int posOffset = getVertexOffset();
            int texStride = (int)getUVStride();
            int texOffset = getUVOffset();

            if (m_faceTangents == null)
            {
                m_faceTangents = new MList<float>(m_indices.length());
            }

            // 初始化
            i = 0;
            while (i < len)
            {
                m_faceTangents.Add(0);
                m_faceTangents.Add(0);
                m_faceTangents.Add(0);

                i += 3;
            }

            i = 0;
            while (i < len)     // 一个面是 3 个顶点，遍历一次就是一个面
            {
                index1 = m_indices[(int)i];
                index2 = m_indices[(int)i + 1];
                index3 = m_indices[(int)i + 2];

                ui = (uint)(texOffset + index1 * texStride + 1);
                v0 = uvs[(int)ui];
                ui = (uint)(texOffset + index2 * texStride + 1);
                dv1 = uvs[(int)ui] - v0;
                ui = (uint)(texOffset + index3 * texStride + 1);
                dv2 = uvs[(int)ui] - v0;

                vi = (uint)(posOffset + index1 * posStride);
                x0 = vertices[(int)vi];
                y0 = vertices[(int)(vi + 1)];
                z0 = vertices[(int)(vi + 2)];
                vi = (uint)(posOffset + index2 * posStride);
                dx1 = vertices[(int)(vi)] - x0;
                dy1 = vertices[(int)(vi + 1)] - y0;
                dz1 = vertices[(int)(vi + 2)] - z0;
                vi = (uint)(posOffset + index3 * posStride);
                dx2 = vertices[(int)(vi)] - x0;
                dy2 = vertices[(int)(vi + 1)] - y0;
                dz2 = vertices[(int)(vi + 2)] - z0;

                cx = dv2 * dx1 - dv1 * dx2;
                cy = dv2 * dy1 - dv1 * dy2;
                cz = dv2 * dz1 - dv1 * dz2;
                denom = (float)(1 / UtilMath.Sqrt(cx * cx + cy * cy + cz * cz));
                m_faceTangents[(int)i++] = denom * cx;
                m_faceTangents[(int)i++] = denom * cy;
                m_faceTangents[(int)i++] = denom * cz;
            }

            m_faceTangentsDirty = false;
        }

        /**
         * @brief 更新面法向量
         */
        private void updateFaceNormals()
        {
            uint i = 0, j = 0, k = 0;
            uint index = 0;
            uint len = (uint)m_indices.length();         // 三角形索引的数量， len/3 就是面的数量，一个三角形有 3 个顶点
            float x1 = 0, x2 = 0, x3 = 0;
            float y1 = 0, y2 = 0, y3 = 0;
            float z1 = 0, z2 = 0, z3 = 0;
            float dx1 = 0, dy1 = 0, dz1 = 0;
            float dx2 = 0, dy2 = 0, dz2 = 0;
            float cx = 0, cy = 0, cz = 0;
            float d = 0;
            MList<float> vertices = m_vertexData;
            int posStride = (int)getVertexStride();
            int posOffset = getVertexOffset();

            if (m_faceNormals == null)
            {
                m_faceNormals = new MList<float>((int)len);
            }
            if (m_useFaceWeights)
            {
                if (m_faceWeights == null)
                {
                    m_faceWeights = new MList<float>((int)len / 3);         // len / 3 面的数量
                }
            }

            // 初始化
            i = 0;
            j = 0;
            while (i < len)
            {
                m_faceNormals.Add(0);
                m_faceNormals.Add(0);
                m_faceNormals.Add(0);

                i += 3;
            }

            i = 0;
            j = 0;
            // 每一次遍历就是一个面， 3 个顶点
            while (i < len)
            {
                index = (uint)(posOffset + m_indices[(int)i++] * posStride);
                x1 = vertices[(int)index];
                y1 = vertices[(int)index + 1];
                z1 = vertices[(int)index + 2];
                index = (uint)(posOffset + m_indices[(int)i++] * posStride);
                x2 = vertices[(int)index];
                y2 = vertices[(int)index + 1];
                z2 = vertices[(int)index + 2];
                index = (uint)(posOffset + m_indices[(int)i++] * posStride);
                x3 = vertices[(int)index];
                y3 = vertices[(int)index + 1];
                z3 = vertices[(int)index + 2];
                dx1 = x3 - x1;
                dy1 = y3 - y1;
                dz1 = z3 - z1;
                dx2 = x2 - x1;
                dy2 = y2 - y1;
                dz2 = z2 - z1;
                cx = dz1 * dy2 - dy1 * dz2;
                cy = dx1 * dz2 - dz1 * dx2;
                cz = dy1 * dx2 - dx1 * dy2;
                d = (float)UtilMath.Sqrt(cx * cx + cy * cy + cz * cz);
                // 叉乘的方向是垂直于两个向量的向量方向，叉乘值是两个向量组成的平行四边形的面积，就是两个三角形面积的大小
                if (m_useFaceWeights)
                {
                    float w = d * 10000;  // 放大权重数量级
                    if (w < 1)          // 如果太小
                    {
                        w = 1;          // 至少等于 1
                    }
                    m_faceWeights[(int)k++] = w;
                }
                d = 1 / d;
                m_faceNormals[(int)j++] = cx * d;
                m_faceNormals[(int)j++] = cy * d;
                m_faceNormals[(int)j++] = cz * d;
            }

            m_faceNormalsDirty = false;

            //Ctx.m_instance.m_fileSys.serializeArray<float>("buildFaceFNormal.txt", m_faceNormals.ToArray(), 3);
        }

        /**
         * @brief 更新顶点法向量
         */
        virtual protected MList<float> updateVertexNormals(MList<float> target)
        {
            if (m_faceNormalsDirty)
            {
                updateFaceNormals();
            }

            uint v1 = 0;
            uint f1 = 0, f2 = 1, f3 = 2;
            uint lenV = (uint)m_vertexData.length();
            int normalStride = (int)getVertexNormalStride();
            int normalOffset = getVertexNormalOffset();

            if (target == null)
            {
                target = new MList<float>((int)lenV);
            }
            v1 = (uint)normalOffset;
            // 初始化向量
            while (v1 < lenV)
            {
                //target[(int)v1] = 0.0f;
                //target[(int)v1 + 1] = 0.0f;
                //target[(int)v1 + 2] = 0.0f;
                target.Add(0);
                target.Add(0);
                target.Add(0);
                v1 += (uint)normalStride;
            }

            uint i = 0, k = 0;
            uint lenI = (uint)m_indices.length();
            uint index = 0;
            float weight = 0;

            // 计算未经单位化的顶点法向量
            while (i < lenI)
            {
                weight = m_useFaceWeights ? m_faceWeights[(int)k++] : 1;
                index = (uint)(normalOffset + m_indices[(int)i++] * normalStride);
                target[(int)index++] += m_faceNormals[(int)f1] * weight;
                target[(int)index++] += m_faceNormals[(int)f2] * weight;
                target[(int)index] += m_faceNormals[(int)f3] * weight;
                index = (uint)(normalOffset + m_indices[(int)i++] * normalStride);
                target[(int)index++] += m_faceNormals[(int)f1] * weight;
                target[(int)index++] += m_faceNormals[(int)f2] * weight;
                target[(int)index] += m_faceNormals[(int)f3] * weight;
                index = (uint)(normalOffset + m_indices[(int)i++] * normalStride);
                target[(int)index++] += m_faceNormals[(int)f1] * weight;
                target[(int)index++] += m_faceNormals[(int)f2] * weight;
                target[(int)index] += m_faceNormals[(int)f3] * weight;
                f1 += 3;
                f2 += 3;
                f3 += 3;
            }

            v1 = (uint)normalOffset;
            // 顶点法向量单位化
            while (v1 < lenV)
            {
                float vx = target[(int)v1];
                float vy = target[(int)v1 + 1];
                float vz = target[(int)v1 + 2];
                float d = (float)(1.0 / UtilMath.Sqrt(vx * vx + vy * vy + vz * vz));
                target[(int)v1] = vx * d;
                target[(int)v1 + 1] = vy * d;
                target[(int)v1 + 2] = vz * d;
                v1 += (uint)normalStride;
            }

            m_vertexNormalsDirty = false;

            return target;
        }

        /**
         * @brief 更新顶点切线
         */
        virtual protected MList<float> updateVertexTangents(MList<float> target)
        {
            if (m_faceTangentsDirty)
            {
                updateFaceTangents();
            }

            uint i = 0;
            uint lenV = (uint)m_vertexData.length();
            int tangentStride = (int)getVertexTangentStride();
            int tangentOffset = (int)getVertexTangentOffset();

            if (target == null)
            {
                target = new MList<float>((int)lenV);
            }

            i = (uint)tangentOffset;
            // 初始化切线缓冲区
            while (i < lenV)
            {
                //target[(int)i] = 0.0f;
                //target[(int)i + 1] = 0.0f;
                //target[(int)i + 2] = 0.0f;
                target.Add(0);
                target.Add(0);
                target.Add(0);
                i += (uint)tangentStride;
            }

            uint k = 0;
            uint lenI = (uint)m_indices.length();
            uint index = 0;
            float weight = 0;
            uint f1 = 0, f2 = 1, f3 = 2;

            i = 0;

            while (i < lenI)
            {
                weight = m_useFaceWeights ? m_faceWeights[(int)k++] : 1;
                index = (uint)(tangentOffset + m_indices[(int)i++] * tangentStride);
                target[(int)index++] += m_faceTangents[(int)f1] * weight;
                target[(int)index++] += m_faceTangents[(int)f2] * weight;
                target[(int)index] += m_faceTangents[(int)f3] * weight;
                index = (uint)(tangentOffset + m_indices[(int)i++] * tangentStride);
                target[(int)index++] += m_faceTangents[(int)f1] * weight;
                target[(int)index++] += m_faceTangents[(int)f2] * weight;
                target[(int)index] += m_faceTangents[(int)f3] * weight;
                index = (uint)(tangentOffset + m_indices[(int)i++] * tangentStride);
                target[(int)index++] += m_faceTangents[(int)f1] * weight;
                target[(int)index++] += m_faceTangents[(int)f2] * weight;
                target[(int)index] += m_faceTangents[(int)f3] * weight;
                f1 += 3;
                f2 += 3;
                f3 += 3;
            }

            i = (uint)tangentOffset;
            while (i < lenV)
            {
                float vx = target[(int)i];
                float vy = target[(int)i + 1];
                float vz = target[(int)i + 2];
                float d = (float)(1.0 / UtilMath.Sqrt(vx * vx + vy * vy + vz * vz));
                target[(int)i] = vx * d;
                target[(int)i + 1] = vy * d;
                target[(int)i + 2] = vz * d;
                i += (uint)tangentStride;
            }

            m_vertexTangentsDirty = false;

            return target;
        }

        virtual protected MList<float> updateDummyUVs(MList<float> target)
        {
            m_uvsDirty = false;

            uint idx = 0, uvIdx = 0;
            int stride = (int)getUVStride();
            int skip = stride - 2;
            uint len = (uint)(m_vertexData.length() / getVertexStride() * stride);

            if (target == null)
            {
                target = new MList<float>();
            }

            target.setLength((int)len);

            idx = (uint)getUVOffset();
            uvIdx = 0;
            while (idx < len)
            {
                target[(int)idx++] = (float)(uvIdx * 0.5);
                target[(int)idx++] = (float)(1.0 - (uvIdx & 1));
                idx += (uint)skip;

                if (++uvIdx == 3)
                {
                    uvIdx = 0;
                }
            }

            return target;
        }

        public void updateIndexData(MList<int> indices)
        {
            m_indices = indices;
            m_numIndices = (uint)(indices.length());

            int numTriangles = (int)(m_numIndices / 3);
            if (m_numTriangles != numTriangles)
            {
                disposeIndexBuffers();
            }
            m_numTriangles = (uint)numTriangles;
            invalidateBuffers(m_indicesInvalid);
            m_faceNormalsDirty = true;

            if (m_autoDeriveVertexNormals)
            {
                m_vertexNormalsDirty = true;
            }
            if (m_autoDeriveVertexTangents)
            {
                m_vertexTangentsDirty = true;
            }
        }

        protected void disposeIndexBuffers()
        {

        }

        virtual protected void invalidateBuffers(MList<Boolean> invalid)
		{
			for (int i = 0; i< 8; ++i)
            {
				invalid[i] = true;
            }
		}

        virtual public MAxisAlignedBox getAABox()
        {
            return MAxisAlignedBox.BOX_NULL;
        }
    }
}