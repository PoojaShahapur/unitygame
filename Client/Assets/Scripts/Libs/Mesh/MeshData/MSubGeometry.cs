using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 基本的 SubGeometry，包含 UV 、法向量、切向量
     */
    public class MSubGeometry : MSubGeometryBase
    {
        protected MList<float> _uvs;
		protected MList<float> _secondaryUvs;
		protected MList<float> _vertexNormals;
		protected MList<float> _vertexTangents;
		
		protected MList<bool> _verticesInvalid;
		protected MList<bool> _uvsInvalid;
		protected MList<bool> _secondaryUvsInvalid;
		protected MList<bool> _normalsInvalid;
		protected MList<bool> _tangentsInvalid;
		
		protected uint _numVertices;

        public MSubGeometry()
        {
            _verticesInvalid = new MList<bool>();
            _uvsInvalid = new MList<bool>();
            _secondaryUvsInvalid = new MList<bool>();
            _normalsInvalid = new MList<bool>();
            _tangentsInvalid = new MList<bool>();

            for (int i = 0; i < 8; ++i)
            {
                _verticesInvalid.Add(true);
                _uvsInvalid.Add(true);
                _secondaryUvsInvalid.Add(true);
                _normalsInvalid.Add(true);
                _tangentsInvalid.Add(true);
            }
        }

        /**
		 * @brief 获取顶点数量
		 */
        public uint getNumVertices()
		{
			return _numVertices;
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
            if (_autoDeriveVertexNormals)
            {
                _vertexNormalsDirty = true;
            }
            if (_autoDeriveVertexTangents)
            {
                _vertexTangentsDirty = true;
            }
			
			_faceNormalsDirty = true;
			
			_vertexData = vertices;
            int numVertices = vertices.length()/3;
            if (numVertices != _numVertices)
            {
                disposeAllVertexBuffers();
            }
            _numVertices = (uint)numVertices;

            invalidateBuffers(_verticesInvalid);

            invalidateBounds();
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
            if (_autoDeriveVertexNormals && _vertexNormalsDirty)
            {
                _vertexNormals = updateVertexNormals(_vertexNormals);
            }
			return _vertexNormals;
		}

        /**
         * @brief 更新顶点法线信息
         */
        override protected MList<float> updateVertexNormals(MList<float> target)
		{
            invalidateBuffers(_normalsInvalid);
			return base.updateVertexNormals(target);
		}

        override public uint getVertexStride()
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
            return _vertexNormals;
        }

        /**
         * @brief 获取顶点法线数组
         */
        override public Vector3[] getVertexNormalArray()
        {
            getVertexNormalData();          // 确保法线是可以获取的
            Vector3[] normalArray = new Vector3[_vertexNormals.length()/3];
            int normalArrIdx = 0;
            for(int idx = 0; idx < _vertexNormals.length(); idx += 3, ++normalArrIdx)
            {
                normalArray[normalArrIdx] = new Vector3(_vertexNormals[idx], _vertexNormals[idx + 1], _vertexNormals[idx + 2]);
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
            if (_uvsDirty && _autoGenerateUVs)
            {
                _uvs = updateDummyUVs(_uvs);
            }
			return _uvs;
		}

        /**
         * @brief 获取 UV 数组数据
         */
        override public Vector2[] getUVDataArray()
        {
            Vector2[] uvArray = new Vector2[_uvs.length() / 2];
            int uvArrIdx = 0;
            for (int idx = 0; idx < _uvs.length(); idx += 3, ++uvArrIdx)
            {
                uvArray[uvArrIdx] = new Vector2(_uvs[idx], _uvs[idx + 1]);
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
            invalidateBuffers(_uvsInvalid);
			return base.updateDummyUVs(target);
		}

        /**
         * @brief 更新顶点切线
         */
        override protected MList<float> updateVertexTangents(MList<float> target)
        {
            invalidateBuffers(_tangentsInvalid);
            return base.updateVertexTangents(target);
        }

        /**
         * @brief 获取切线数据
         */
        override public MList<float> getVertexTangentsData()
        {
            if (_autoDeriveVertexTangents && _vertexTangentsDirty)
            {
                _vertexTangents = updateVertexTangents(_vertexTangents);
            }
            return _vertexTangents;
        }

        /**
         * @brief 获取顶点切线数组
         */
        override public Vector4[] getVertexTangentArray()
        {
            getVertexTangentsData();
            Vector4[] tangentArray = new Vector4[_vertexTangents.length() / 3];
            int tangentArrIdx = 0;
            for (int idx = 0; idx < _vertexNormals.length(); idx += 3, ++tangentArrIdx)
            {
                tangentArray[tangentArrIdx] = new Vector4(_vertexTangents[idx], _vertexTangents[idx + 1], _vertexTangents[idx + 2], 0);
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
            if (_autoDeriveVertexTangents)
            {
                _vertexTangentsDirty = true;
            }
			_faceTangentsDirty = true;
			_uvs = uvs;
            invalidateBuffers(_uvsInvalid);
        }
    }
}