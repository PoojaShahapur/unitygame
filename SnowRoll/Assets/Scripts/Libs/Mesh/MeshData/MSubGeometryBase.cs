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
        protected Vector3[] m_vertexData;
        protected bool m_faceNormalsDirty;
        protected bool m_faceTangentsDirty;
        protected Vector3[] m_faceTangents;
        protected int[] m_indices;
        protected uint m_numIndices;
        protected MList<Boolean> m_indicesInvalid;
        protected uint m_numTriangles;

        protected bool m_autoDeriveVertexNormals;
        protected bool m_autoDeriveVertexTangents;
        protected bool m_autoGenerateUVs;
        protected bool m_useFaceWeights;         // 计算顶点法向量的时候，使用面法向量插值生成
        protected bool m_vertexNormalsDirty;
        protected bool m_vertexTangentsDirty;

        protected Vector3[] m_faceNormals;
        protected float[] m_faceWeights;

        protected bool m_uvsDirty = true;

        public MSubGeometryBase()
        {
            m_faceNormalsDirty = true;
            m_faceTangentsDirty = true;
            m_indicesInvalid = new MList<Boolean>(8);

            m_autoDeriveVertexNormals = true;
            m_autoDeriveVertexTangents = true;
            m_autoGenerateUVs = false;
            m_useFaceWeights = true;
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
        public Vector3[] getVertexData()
        {
            return m_vertexData;
        }

        /**
         * @brief 获取顶点数量
         */
        public int getVertexDataCount()
        {
            return m_vertexData.Length / getVertexStride();
        }

        /**
		 * @brief 获取索引数据
		 */
        public int[] getIndexData()
        {
            return m_indices;
        }

        /**
         * @brief 获取三角形的数量，就是获取面的数量
         */
        public int getTriangleCount()
        {
            return m_indices.Length / 3;
        }

        /**
         * @brief 获取法线数据
         */
        virtual public Vector3[] getVertexNormalsData()
        {
            throw new Exception();
        }

        /**
         * @brief 获取切线数据
         */
        virtual public Vector4[] getVertexTangentsData()
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
        virtual public Vector2[] getUVData()
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
            uint len = (uint)m_indices.Length;
            uint ui = 0, vi = 0;
            float v0 = 0;
            float dv1 = 0, dv2 = 0;
            float denom = 0;
            float x0 = 0, y0 = 0, z0 = 0;
            float dx1 = 0, dy1 = 0, dz1 = 0;
            float dx2 = 0, dy2 = 0, dz2 = 0;
            float cx = 0, cy = 0, cz = 0;
            Vector3[] vertices = m_vertexData;
            Vector2[] uvs = getUVData();
            int posStride = (int)getVertexStride();
            int posOffset = getVertexOffset();
            int texStride = (int)getUVStride();
            int texOffset = getUVOffset();

            if (m_faceTangents == null)
            {
                m_faceTangents = new Vector3[m_indices.Length/3];
            }

            i = 0;
            while (i < len)     // 一个面是 3 个顶点，遍历一次就是一个面
            {
                index1 = m_indices[(int)i];
                index2 = m_indices[(int)i + 1];
                index3 = m_indices[(int)i + 2];

                ui = (uint)(texOffset + index1 * texStride);
                v0 = uvs[(int)ui].y;
                ui = (uint)(texOffset + index2 * texStride);
                dv1 = uvs[(int)ui].y - v0;
                ui = (uint)(texOffset + index3 * texStride);
                dv2 = uvs[(int)ui].y - v0;

                vi = (uint)(posOffset + index1 * posStride);
                x0 = vertices[(int)vi].x;
                y0 = vertices[(int)vi].y;
                z0 = vertices[(int)vi].z;
                vi = (uint)(posOffset + index2 * posStride);
                dx1 = vertices[(int)(vi)].x - x0;
                dy1 = vertices[(int)(vi)].y - y0;
                dz1 = vertices[(int)(vi)].z - z0;
                vi = (uint)(posOffset + index3 * posStride);
                dx2 = vertices[(int)(vi)].x - x0;
                dy2 = vertices[(int)(vi)].y - y0;
                dz2 = vertices[(int)(vi)].z - z0;

                cx = dv2 * dx1 - dv1 * dx2;
                cy = dv2 * dy1 - dv1 * dy2;
                cz = dv2 * dz1 - dv1 * dz2;
                denom = (float)(1 / UtilMath.Sqrt(cx * cx + cy * cy + cz * cz));
                m_faceTangents[(int)i/3].x = denom * cx;
                m_faceTangents[(int)i/3].y = denom * cy;
                m_faceTangents[(int)i/3].z = denom * cz;

                i += 3;     // 移动 3 个顶点，就是一个面
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
            uint len = (uint)m_indices.Length;         // 三角形索引的数量， len/3 就是面的数量，一个三角形有 3 个顶点
            float x1 = 0, x2 = 0, x3 = 0;
            float y1 = 0, y2 = 0, y3 = 0;
            float z1 = 0, z2 = 0, z3 = 0;
            float dx1 = 0, dy1 = 0, dz1 = 0;
            float dx2 = 0, dy2 = 0, dz2 = 0;
            float cx = 0, cy = 0, cz = 0;
            float d = 0;
            Vector3[] vertices = m_vertexData;
            int posStride = (int)getVertexStride();
            int posOffset = getVertexOffset();

            if (m_faceNormals == null)
            {
                m_faceNormals = new Vector3[(int)len/3];
            }
            if (m_useFaceWeights)
            {
                if (m_faceWeights == null)
                {
                    m_faceWeights = new float[(int)len / 3];         // len / 3 面的数量
                }
            }

            i = 0;
            j = 0;
            // 每一次遍历就是一个面， 3 个顶点
            while (i < len)
            {
                index = (uint)(posOffset + m_indices[(int)i] * posStride);
                x1 = vertices[(int)index].x;
                y1 = vertices[(int)index].y;
                z1 = vertices[(int)index].z;
                index = (uint)(posOffset + m_indices[(int)i+1] * posStride);
                x2 = vertices[(int)index].x;
                y2 = vertices[(int)index].y;
                z2 = vertices[(int)index].z;
                index = (uint)(posOffset + m_indices[(int)i+2] * posStride);
                x3 = vertices[(int)index].x;
                y3 = vertices[(int)index].y;
                z3 = vertices[(int)index].z;
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
                    m_faceWeights[(int)k] = w;
                }
                d = 1 / d;
                m_faceNormals[(int)j].x = cx * d;
                m_faceNormals[(int)j].y = cy * d;
                m_faceNormals[(int)j].z = cz * d;

                i += 3;     // 移动到下一个顶点
                ++j;        // 下一个面
                ++k;        // 下一个面
            }

            m_faceNormalsDirty = false;

            //Ctx.mInstance.m_fileSys.serializeArray<float>("buildFaceFNormal.txt", m_faceNormals.ToArray(), 3);
        }

        /**
         * @brief 更新顶点法向量
         */
        virtual protected Vector3[] updateVertexNormals(Vector3[] target)
        {
            if (m_faceNormalsDirty)
            {
                updateFaceNormals();
            }

            uint v1 = 0;
            uint f1 = 0, f2 = 1, f3 = 2;
            uint lenV = (uint)m_vertexData.Length;
            int normalStride = (int)getVertexNormalStride();
            int normalOffset = getVertexNormalOffset();

            if (target == null)
            {
                target = new Vector3[((int)lenV)];
            }

            uint i = 0, k = 0;
            uint lenI = (uint)m_indices.Length;
            uint index = 0;
            float weight = 0;

            // 计算未经单位化的顶点法向量
            while (i < lenI)
            {
                weight = m_useFaceWeights ? m_faceWeights[(int)k] : 1;
                index = (uint)(normalOffset + m_indices[(int)i] * normalStride);
                target[(int)index].x += m_faceNormals[(int)f1].x * weight;
                target[(int)index].y += m_faceNormals[(int)f1].y * weight;
                target[(int)index].z += m_faceNormals[(int)f1].z * weight;
                index = (uint)(normalOffset + m_indices[(int)i + 1] * normalStride);
                target[(int)index].x += m_faceNormals[(int)f1].x * weight;
                target[(int)index].y += m_faceNormals[(int)f1].y * weight;
                target[(int)index].z += m_faceNormals[(int)f1].z * weight;
                index = (uint)(normalOffset + m_indices[(int)i + 2] * normalStride);
                if(index >= target.Length)
                {
                    Debug.Log("aa");
                }
                target[(int)index].x += m_faceNormals[(int)f1].x * weight;
                target[(int)index].y += m_faceNormals[(int)f1].y * weight;
                target[(int)index].z += m_faceNormals[(int)f1].z * weight;

                i += 3;
                ++k;
                ++f1;
                ++f2;
                ++f3;
            }

            v1 = (uint)normalOffset;
            // 顶点法向量单位化
            while (v1 < lenV)
            {
                float vx = target[(int)v1].x;
                float vy = target[(int)v1].y;
                float vz = target[(int)v1].z;
                float d = (float)(1.0 / UtilMath.Sqrt(vx * vx + vy * vy + vz * vz));
                target[(int)v1].x = vx * d;
                target[(int)v1].y = vy * d;
                target[(int)v1].z = vz * d;
                v1 += (uint)normalStride;
            }

            m_vertexNormalsDirty = false;

            return target;
        }

        /**
         * @brief 更新顶点切线
         */
        virtual protected Vector4[] updateVertexTangents(Vector4[] target)
        {
            if (m_faceTangentsDirty)
            {
                updateFaceTangents();
            }

            uint i = 0;
            uint lenV = (uint)m_vertexData.Length;
            int tangentStride = (int)getVertexTangentStride();
            int tangentOffset = (int)getVertexTangentOffset();

            if (target == null)
            {
                target = new Vector4[((int)lenV)];
            }

            uint k = 0;
            uint lenI = (uint)m_indices.Length;
            uint index = 0;
            float weight = 0;
            uint f1 = 0, f2 = 1, f3 = 2;

            i = 0;

            while (i < lenI)
            {
                weight = m_useFaceWeights ? m_faceWeights[(int)k] : 1;
                index = (uint)(tangentOffset + m_indices[(int)i] * tangentStride);
                target[(int)index].x += m_faceTangents[(int)f1].x * weight;
                target[(int)index].y += m_faceTangents[(int)f1].y * weight;
                target[(int)index].z += m_faceTangents[(int)f1].z * weight;
                target[(int)index].w = 0;
                index = (uint)(tangentOffset + m_indices[(int)i + 1] * tangentStride);
                target[(int)index].x += m_faceTangents[(int)f1].x * weight;
                target[(int)index].y += m_faceTangents[(int)f1].y * weight;
                target[(int)index].z += m_faceTangents[(int)f1].z * weight;
                target[(int)index].w = 0;
                index = (uint)(tangentOffset + m_indices[(int)i + 2] * tangentStride);
                target[(int)index].x += m_faceTangents[(int)f1].x * weight;
                target[(int)index].y += m_faceTangents[(int)f1].y * weight;
                target[(int)index].z += m_faceTangents[(int)f1].z * weight;
                target[(int)index].w = 0;

                i += 3;
                ++k;
                index += 3;
                f1 += 1;
                f2 += 1;
                f3 += 1;
            }

            i = (uint)tangentOffset;
            while (i < lenV)
            {
                float vx = target[(int)i].x;
                float vy = target[(int)i].y;
                float vz = target[(int)i].z;
                float d = (float)(1.0 / UtilMath.Sqrt(vx * vx + vy * vy + vz * vz));
                target[(int)i].x = vx * d;
                target[(int)i].y = vy * d;
                target[(int)i].z = vz * d;
                i += (uint)tangentStride;
            }

            m_vertexTangentsDirty = false;

            return target;
        }

        virtual protected Vector2[] updateDummyUVs(Vector2[] target)
        {
            m_uvsDirty = false;

            uint idx = 0, uvIdx = 0;
            int stride = (int)getUVStride();
            int skip = stride - 1;
            uint len = (uint)(m_vertexData.Length / getVertexStride() * stride);

            if (target == null)
            {
                target = new Vector2[(int)len];
            }

            idx = (uint)getUVOffset();
            uvIdx = 0;
            while (idx < len)
            {
                target[(int)idx].x = (float)(uvIdx * 0.5);
                target[(int)idx].y = (float)(1.0 - (uvIdx & 1));

                ++idx;
                idx += (uint)skip;

                if (++uvIdx == 3)
                {
                    uvIdx = 0;
                }
            }

            return target;
        }

        public void updateIndexData(int[] indices)
        {
            m_indices = indices;
            m_numIndices = (uint)(indices.Length);

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