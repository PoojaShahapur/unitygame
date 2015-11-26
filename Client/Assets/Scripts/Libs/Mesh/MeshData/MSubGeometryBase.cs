using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief SubGeometryBase 基类，只包含基本的顶点索引数据
     */
    public class MSubGeometryBase : MISubGeometry
    {
        protected MGeometry _parentGeometry;
        protected MList<float> _vertexData;
        protected bool _faceNormalsDirty;
        protected bool _faceTangentsDirty;
        protected MList<float> _faceTangents;
        protected MList<int> _indices;
        protected uint _numIndices;
        protected MList<Boolean> _indicesInvalid;
        protected uint _numTriangles;

        protected bool _autoDeriveVertexNormals;
        protected bool _autoDeriveVertexTangents;
        protected bool _autoGenerateUVs;
        protected bool _useFaceWeights;         // 计算顶点法向量的时候，使用面法向量插值生成
        protected bool _vertexNormalsDirty;
        protected bool _vertexTangentsDirty;

        protected MList<float> _faceNormals;
        protected MList<float> _faceWeights;

        private float _scaleU;
        private float _scaleV;

        protected bool _uvsDirty = true;

        public MSubGeometryBase()
        {
            _faceNormalsDirty = true;
            _faceTangentsDirty = true;
            _indicesInvalid = new MList<Boolean>(8);

            _autoDeriveVertexNormals = true;
            _autoDeriveVertexTangents = true;
            _autoGenerateUVs = false;
            _useFaceWeights = false;
            _vertexNormalsDirty = true;
            _vertexTangentsDirty = true;

            _scaleU = 1;
            _scaleV = 1;
        }

        public MGeometry getParentGeometry()
        {
            return _parentGeometry;
        }

        public void setParentGeometry(MGeometry parentGeom)
        {
            _parentGeometry = parentGeom;
        }

        /**
         * @brief 获取顶点数据
         */
        public MList<float> getVertexData()
        {
            return _vertexData;
        }

        /**
         * @brief 获取顶点向量数据
         */
        public Vector3[] getVertexDataArray()
        {
            Vector3[] vertexVec = new Vector3[_vertexData.length() / 3];        // 必然是 3 个
            int idxVec = 0;
            for (int idx = 0; idx < _vertexData.length(); idx += 3, ++idxVec)
            {
                vertexVec[idxVec] = new Vector3(_vertexData[idx], _vertexData[idx + 1], _vertexData[idx + 2]);
            }

            return vertexVec;
        }

        /**
         * @brief 获取顶点数量
         */
        public int getVertexDataCount()
        {
            return _vertexData.length();
        }

        /**
		 * @brief 获取索引数据
		 */
        public MList<int> getIndexData()
        {
            return _indices;
        }

        /**
         * @brief 获取三角形的数量，就是获取面的数量
         */
        public int getTriangleCount()
        {
            return _indices.length() / 3;
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
            return _autoDeriveVertexNormals;
        }

        public void setAutoDeriveVertexNormals(bool value)
        {
            _autoDeriveVertexNormals = value;
        }

        public bool getAutoDeriveVertexTangents()
        {
            return _autoDeriveVertexTangents;
        }

        public void setAutoDeriveVertexTangents(bool value)
        {
            _autoDeriveVertexTangents = value;
        }

        protected void invalidateBounds()
        {
            if (_parentGeometry != null)
            {
                _parentGeometry.invalidateBounds(this as MISubGeometry);
            }
        }

        virtual public uint getVertexStride()
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
            uint len = (uint)_indices.length();
            uint ui = 0, vi = 0;
            float v0 = 0;
            float dv1 = 0, dv2 = 0;
            float denom = 0;
            float x0 = 0, y0 = 0, z0 = 0;
            float dx1 = 0, dy1 = 0, dz1 = 0;
            float dx2 = 0, dy2 = 0, dz2 = 0;
            float cx = 0, cy = 0, cz = 0;
            MList<float> vertices = _vertexData;
            MList<float> uvs = getUVData();
            int posStride = (int)getVertexStride();
            int posOffset = getVertexOffset();
            int texStride = (int)getUVStride();
            int texOffset = getUVOffset();

            if (_faceTangents == null)
            {
                _faceTangents = new MList<float>((int)_indices.length());
            }

            while (i < len)
            {
                index1 = _indices[(int)i];
                index2 = _indices[(int)i + 1];
                index3 = _indices[(int)i + 2];

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
                denom = (float)(1 / UtilApi.Sqrt(cx * cx + cy * cy + cz * cz));
                _faceTangents[(int)i++] = denom * cx;
                _faceTangents[(int)i++] = denom * cy;
                _faceTangents[(int)i++] = denom * cz;
            }

            _faceTangentsDirty = false;
        }

        /**
         * @brief 更新面法向量
         */
        private void updateFaceNormals()
        {
            uint i = 0, j = 0, k = 0;
            uint index = 0;
            uint len = (uint)_indices.length();         // 三角形索引的数量， len/3 就是面的数量，一个三角形有 3 个顶点
            float x1 = 0, x2 = 0, x3 = 0;
            float y1 = 0, y2 = 0, y3 = 0;
            float z1 = 0, z2 = 0, z3 = 0;
            float dx1 = 0, dy1 = 0, dz1 = 0;
            float dx2 = 0, dy2 = 0, dz2 = 0;
            float cx = 0, cy = 0, cz = 0;
            float d = 0;
            MList<float> vertices = _vertexData;
            int posStride = (int)getVertexStride();
            int posOffset = getVertexOffset();

            if (_faceNormals == null)
            {
                _faceNormals = new MList<float>((int)len);
            }
            if (_useFaceWeights)
            {
                if (_faceWeights == null)
                {
                    _faceWeights = new MList<float>((int)len / 3);         // len / 3 面的数量
                }
            }

            // 每一次遍历就是一个面， 3 个顶点
            while (i < len)
            {
                index = (uint)(posOffset + _indices[(int)i++] * posStride);
                x1 = vertices[(int)index];
                y1 = vertices[(int)index + 1];
                z1 = vertices[(int)index + 2];
                index = (uint)(posOffset + _indices[(int)i++] * posStride);
                x2 = vertices[(int)index];
                y2 = vertices[(int)index + 1];
                z2 = vertices[(int)index + 2];
                index = (uint)(posOffset + _indices[(int)i++] * posStride);
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
                d = (float)UtilApi.Sqrt(cx * cx + cy * cy + cz * cz);
                // 叉乘的方向是垂直于两个向量的向量方向，叉乘值是两个向量组成的平行四边形的面积，就是两个三角形面积的大小
                if (_useFaceWeights)
                {
                    float w = d * 10000;  // 放大权重数量级
                    if (w < 1)          // 如果太小
                    {
                        w = 1;          // 至少等于 1
                    }
                    _faceWeights[(int)k++] = w;
                }
                d = 1 / d;
                _faceNormals[(int)j++] = cx * d;
                _faceNormals[(int)j++] = cy * d;
                _faceNormals[(int)j++] = cz * d;
            }

            _faceNormalsDirty = false;
        }

        /**
         * @brief 更新顶点法向量
         */
        virtual protected MList<float> updateVertexNormals(MList<float> target)
        {
            if (_faceNormalsDirty)
            {
                updateFaceNormals();
            }

            uint v1 = 0;
            uint f1 = 0, f2 = 1, f3 = 2;
            uint lenV = (uint)_vertexData.length();
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
                target[(int)v1] = 0.0f;
                target[(int)v1 + 1] = 0.0f;
                target[(int)v1 + 2] = 0.0f;
                v1 += (uint)normalStride;
            }

            uint i = 0, k = 0;
            uint lenI = (uint)_indices.length();
            uint index = 0;
            float weight = 0;

            // 计算未经单位化的顶点法向量
            while (i < lenI)
            {
                weight = _useFaceWeights ? _faceWeights[(int)k++] : 1;
                index = (uint)(normalOffset + _indices[(int)i++] * normalStride);
                target[(int)index++] += _faceNormals[(int)f1] * weight;
                target[(int)index++] += _faceNormals[(int)f2] * weight;
                target[(int)index] += _faceNormals[(int)f3] * weight;
                index = (uint)(normalOffset + _indices[(int)i++] * normalStride);
                target[(int)index++] += _faceNormals[(int)f1] * weight;
                target[(int)index++] += _faceNormals[(int)f2] * weight;
                target[(int)index] += _faceNormals[(int)f3] * weight;
                index = (uint)(normalOffset + _indices[(int)i++] * normalStride);
                target[(int)index++] += _faceNormals[(int)f1] * weight;
                target[(int)index++] += _faceNormals[(int)f2] * weight;
                target[(int)index] += _faceNormals[(int)f3] * weight;
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
                float d = (float)(1.0 / UtilApi.Sqrt(vx * vx + vy * vy + vz * vz));
                target[(int)v1] = vx * d;
                target[(int)v1 + 1] = vy * d;
                target[(int)v1 + 2] = vz * d;
                v1 += (uint)normalStride;
            }

            _vertexNormalsDirty = false;

            return target;
        }

        /**
         * @brief 更新顶点切线
         */
        protected MList<float> updateVertexTangents(MList<float> target)
        {
            if (_faceTangentsDirty)
            {
                updateFaceTangents();
            }

            uint i = 0;
            uint lenV = (uint)_vertexData.length();
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
                target[(int)i] = 0.0f;
                target[(int)i + 1] = 0.0f;
                target[(int)i + 2] = 0.0f;
                i += (uint)tangentStride;
            }

            uint k = 0;
            uint lenI = (uint)_indices.length();
            uint index = 0;
            float weight = 0;
            uint f1 = 0, f2 = 1, f3 = 2;

            i = 0;

            while (i < lenI)
            {
                weight = _useFaceWeights ? _faceWeights[(int)k++] : 1;
                index = (uint)(tangentOffset + _indices[(int)i++] * tangentStride);
                target[(int)index++] += _faceTangents[(int)f1] * weight;
                target[(int)index++] += _faceTangents[(int)f2] * weight;
                target[(int)index] += _faceTangents[(int)f3] * weight;
                index = (uint)(tangentOffset + _indices[(int)i++] * tangentStride);
                target[(int)index++] += _faceTangents[(int)f1] * weight;
                target[(int)index++] += _faceTangents[(int)f2] * weight;
                target[(int)index] += _faceTangents[(int)f3] * weight;
                index = (uint)(tangentOffset + _indices[(int)i++] * tangentStride);
                target[(int)index++] += _faceTangents[(int)f1] * weight;
                target[(int)index++] += _faceTangents[(int)f2] * weight;
                target[(int)index] += _faceTangents[(int)f3] * weight;
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
                float d = (float)(1.0 / UtilApi.Sqrt(vx * vx + vy * vy + vz * vz));
                target[(int)i] = vx * d;
                target[(int)i + 1] = vy * d;
                target[(int)i + 2] = vz * d;
                i += (uint)tangentStride;
            }

            _vertexTangentsDirty = false;

            return target;
        }

        virtual protected MList<float> updateDummyUVs(MList<float> target)
        {
            _uvsDirty = false;

            uint idx = 0, uvIdx = 0;
            int stride = (int)getUVStride();
            int skip = stride - 2;
            uint len = (uint)(_vertexData.length() / getVertexStride() * stride);

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
            _indices = indices;
            _numIndices = (uint)(indices.length());

            int numTriangles = (int)(_numIndices / 3);
            if (_numTriangles != numTriangles)
            {
                disposeIndexBuffers();
            }
            _numTriangles = (uint)numTriangles;
            invalidateBuffers(_indicesInvalid);
            _faceNormalsDirty = true;

            if (_autoDeriveVertexNormals)
            {
                _vertexNormalsDirty = true;
            }
            if (_autoDeriveVertexTangents)
            {
                _vertexTangentsDirty = true;
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
    }
}