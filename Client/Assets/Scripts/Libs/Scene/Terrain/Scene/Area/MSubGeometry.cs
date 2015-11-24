namespace SDK.Lib
{
    /**
     * @brief 基本的 SubGeometry
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
        public MList<float> getVertexData()
		{
			return _vertexData;
		}

        public void updateVertexData(MList<float> vertices)
		{
			if (_autoDeriveVertexNormals)
				_vertexNormalsDirty = true;
			if (_autoDeriveVertexTangents)
				_vertexTangentsDirty = true;
			
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

        protected void invalidateBuffers(MList<bool> invalid)
		{
            for (int i = 0; i < 8; ++i)
            {
                invalid[i] = true;
            }
		}

        protected void disposeAllVertexBuffers()
        {

        }

        public MList<float> getVertexNormalData()
		{
            if (_autoDeriveVertexNormals && _vertexNormalsDirty)
            {
                _vertexNormals = updateVertexNormals(_vertexNormals);
            }
			return _vertexNormals;
		}

        protected MList<float> updateVertexNormals(MList<float> target)
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
				_uvs = updateDummyUVs(_uvs);
			return _uvs;
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