namespace SDK.Lib
{
    /**
     * @brief 高度地形 Mesh
     */
    public class HeightMapMesh : MMesh
    {
        protected int _segmentsW;  // 世界空间高度图宽度划分的线段数量， X 轴线段数量
		protected int _segmentsH;  // 世界空间高度图高度划分的线段数量， Z 轴线段数量
        protected float _width;     // 世界空间高度图宽度， X 轴宽度
        protected float _height;    // 世界空间高度图高度， Z 轴高度
        protected float _depth;     // 世界空间高度图深度，这个等价于 Z 的世界空间高度
        protected HeightMapData _heightMap;     // 高度图
		protected HeightMapData _smoothedHeightMap;     // 平滑的高度图
		protected HeightMapData _activeMap;             // 获取高度的高度图
		protected uint _minElevation;                   // 高度图最小高度
		protected uint _maxElevation;                   // 高度图最大高度
		protected bool _geomDirty;                      // Geometry 数据是否是被修改
		protected bool _uvDirty;                        // UV 数据是否被修改
		protected MSubGeometry _subGeometry;            // SubGeometry 数据

        public HeightMapMesh(MatRes material, HeightMapData heightMap, float width = 1000, float height = 100, float depth = 1000, int segmentsW = 30, int segmentsH = 30, uint maxElevation = 255, uint minElevation = 0, bool smoothMap = false)
            : base(new MGeometry(), new SingleAreaRender())
        {
            _subGeometry = new MSubGeometry();
            this.getGeometry().addSubGeometry(_subGeometry);
            m_meshRender.setSubGeometry(_subGeometry);      // 设置几何信息

            _geomDirty = true;
            _uvDirty = true;

            _heightMap = heightMap;
            _activeMap = _heightMap;
            _segmentsW = segmentsW;
            _segmentsH = segmentsH;
            _width = width;
            _height = height;
            _depth = depth;
            _maxElevation = maxElevation;
            _minElevation = minElevation;

            buildUVs();
            buildGeometry();

            if (smoothMap)
            {
                generateSmoothedHeightMap();
            }
        }

        /**
         * @brief 返回高度图最小高度
         */
        public void setMinElevation(uint val)
		{
            if (_minElevation == val)
            {
                return;
            }
			
			_minElevation = val;
            invalidateGeometry();
        }

        public uint getMinElevation()
		{
			return _minElevation;
		}

        /**
         * @brief 设置高度图最大高度
         */
        public void setMaxElevation(uint val)
		{
			if (_maxElevation == val)
				return;
			
			_maxElevation = val;
            invalidateGeometry();
		}
		
		public uint getMaxElevation()
		{
			return _maxElevation;
		}

        /**
         * @brief 获取 Z 轴世界空间分割的 Segment 的数量
		 */
        public int getSegmentsH()
		{
			return _segmentsH;
		}
		
		public void setSegmentsH(int value)
		{
			_segmentsH = value;
            invalidateGeometry();
            invalidateUVs();
		}
		
		/**
		 * @brief 获取世界空间 X 轴大小
		 */
		public float getWidth()
		{
			return _width;
		}
		
		public void setWidth(float value)
		{
			_width = value;
            invalidateGeometry();
		}
		
		public float getHeight()
		{
			return _height;
		}
		
		public void setHeight(float value)
		{
			_height = value;
		}
		
		/**
		 * @brief 地形 Mesh 的深度，就是 Z 值的世界空间的长度
		 */
		public float getDepth()
		{
			return _depth;
		}
		
		public void setDepth(float value)
		{
			_depth = value;
            invalidateGeometry();
		}
		
		/**
		 * @brief 获取地形的某一个坐标的高度
		 */
		public float getHeightAt(float x, float z)
		{
            int pixX = (int)(x / _width + 0.5) * (_activeMap.getWidth() - 1);       // + 0.5 就是将地形的原点放在中心点
            int pixZ = (int)(-z / _depth + 0.5) * (_activeMap.getHeight() - 1);
            uint col = (uint)(_activeMap.getPixel(pixX, pixZ)) & 0xff;
            return (col > _maxElevation) ? ((float)_maxElevation / 0xff) * _height : ((col < _minElevation) ? ((float)_minElevation / 0xff) * _height : ((float)col / 0xff) * _height);
        }

        /**
         * @brief 生成平滑的高度图
         */
        public HeightMapData generateSmoothedHeightMap()
		{
            if (_smoothedHeightMap != null)
            {
                _smoothedHeightMap.dispose();
            }
			_smoothedHeightMap = new HeightMapData(_heightMap.getWidth(), _heightMap.getHeight());

            int w = _smoothedHeightMap.getWidth();
            int h = _smoothedHeightMap.getHeight();
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;

            uint px1 = 0;
            uint px2 = 0;
            uint px3 = 0;
            uint px4 = 0;

            int lockx = 0;
            int locky = 0;
			
			_smoothedHeightMap.lockMem();
			
			float incXL = 0;
            float incXR = 0;
            float incYL = 0;
            float incYR = 0;
            float pxx = 0;
            float pxy = 0;
			
			for (i = 0; i<w + 1; i += _segmentsW)
            {
                if (i + _segmentsW > w - 1)
                {
                    lockx = w - 1;
                }
                else
                {
                    lockx = i + _segmentsW;
                }
				
				for (j = 0; j<h + 1; j += _segmentsH)
                {
                    if (j + _segmentsH > h - 1)
                    {
                        locky = h - 1;
                    }
                    else
                    {
                        locky = j + _segmentsH;
                    }
					
					if (j == 0)
                    {
						px1 = (uint)(_heightMap.getPixel((int)i, (int)j)) & 0xFF;
						px1 = (px1 > _maxElevation)? _maxElevation : ((px1<_minElevation)? _minElevation : px1);
						px2 = (uint)(_heightMap.getPixel((int)lockx, (int)j)) & 0xFF;
						px2 = (px2 > _maxElevation)? _maxElevation : ((px2<_minElevation)? _minElevation : px2);
						px3 = (uint)(_heightMap.getPixel((int)lockx, (int)locky)) & 0xFF;
						px3 = (px3 > _maxElevation)? _maxElevation : ((px3<_minElevation)? _minElevation : px3);
						px4 = (uint)(_heightMap.getPixel((int)i, (int)locky)) & 0xFF;
						px4 = (px4 > _maxElevation)? _maxElevation : ((px4<_minElevation)? _minElevation : px4);
					}
                    else
                    {
						px1 = px4;
						px2 = px3;
						px3 = (uint)(_heightMap.getPixel((int)lockx, (int)locky)) & 0xFF;
						px3 = (px3 > _maxElevation)? _maxElevation : ((px3<_minElevation)? _minElevation : px3);
						px4 = (uint)(_heightMap.getPixel((int)i, (int)locky)) & 0xFF;
						px4 = (px4 > _maxElevation)? _maxElevation : ((px4<_minElevation)? _minElevation : px4);
					}
					
					for (k = 0; k<_segmentsW; ++k)
                    {
						incXL = (float)1 / _segmentsW * k;
                        incXR = 1 - incXL;
						
						for (l = 0; l < _segmentsH; ++l)
                        {
							incYL = (float)1 / _segmentsH * l;
                            incYR = 1 - incYL;
							
							pxx = ((px1* incXR) + (px2* incXL))* incYR;
                            pxy = ((px4* incXR) + (px3* incXL))* incYL;
                            
                            _smoothedHeightMap.setPixel((int)(k + i), (int)(l + j), (uint)((int)(pxy + pxx) << 16 | (int)(pxy + pxx) << 8 | (int)(pxy + pxx)));
						}
					}
				}
			}
			_smoothedHeightMap.unlock();
			
			_activeMap = _smoothedHeightMap;
			
			return _smoothedHeightMap;
		}

        /**
         * @brief 生成几何
         */
		private void buildGeometry()
		{
            MList<float> vertices;
			MList<int> indices;
			float x = 0, z = 0;
            uint numInds = 0;
            int baseIdx = 0;
            int tw = _segmentsW + 1;
            int numVerts = (_segmentsH + 1)* tw;
            float uDiv = (float)(_heightMap.getWidth() - 1) / _segmentsW;
			float vDiv = (float)(_heightMap.getHeight() - 1) / _segmentsH;
			float u = 0, v = 0;
			float y = 0;
			
			if (numVerts == _subGeometry.getNumVertices())
            {
				vertices = _subGeometry.getVertexData();
				indices = _subGeometry.getIndexData();
			}
            else
            {
				vertices = new MList<float>(numVerts * 3); // 顶点的数量
				indices = new MList<int>(_segmentsH * _segmentsW * 6);  // 索引的数量
			}

            numVerts = 0;
            // 初始化
            for (int zi = 0; zi <= _segmentsH; ++zi)
            {
                for (int xi = 0; xi <= _segmentsW; ++xi)
                {
                    vertices.Add(0);
                    vertices.Add(0);
                    vertices.Add(0);

                    if (xi != _segmentsW && zi != _segmentsH)
                    {
                        baseIdx = xi + zi * tw;
                        indices.Add(0);
                        indices.Add(0);
                        indices.Add(0);
                        indices.Add(0);
                        indices.Add(0);
                        indices.Add(0);
                    }
                }
            }

            numVerts = 0;
            uint col = 0;
			
			for (int zi = 0; zi <= _segmentsH; ++zi)
            {
				for (int xi = 0; xi <= _segmentsW; ++xi)
                {
                    // (float) 一定要先转换成 (float) ，否则 xi / _segmentsW 整数除总是 0 ，导致结果总是在一个顶点
                    x = (int)(((float)xi / _segmentsW - 0.5f) * _width);            // -0.5 保证原点放在地形的中心点
                    z = (int)(((float)zi / _segmentsH - 0.5f) * _depth);
                    u = xi * uDiv;
                    v = (_segmentsH - zi) * vDiv;

                    col = (uint)(_heightMap.getPixel((int)u, (int)v)) & 0xff;
					y = (col > _maxElevation) ? ((float)_maxElevation / 0xff)* _height : ((col<_minElevation) ? ((float)_minElevation /0xff) * _height : ((float)col / 0xff) * _height);         // col 是 [0, 255] 的灰度值，col / 0xff 就是 [0, 1] 的灰度值，col / 0xff 两个整数除，如果要得到 float ，一定要写成 (float)col / 0xff，否则是四舍五入的整数值

                    vertices[numVerts++] = x;
					vertices[numVerts++] = y;
					vertices[numVerts++] = z;
					
					if (xi != _segmentsW && zi != _segmentsH)   // 循环中计数已经多加了 1 ，因此，这里如果超过范围直接返回，只有在范围内的值，才更新
                    {
						baseIdx = xi + zi* tw;
                        indices[(int)numInds++] = baseIdx;
						indices[(int)numInds++] = baseIdx + tw;
						indices[(int)numInds++] = baseIdx + tw + 1;
						indices[(int)numInds++] = baseIdx;
						indices[(int)numInds++] = baseIdx + tw + 1;
						indices[(int)numInds++] = baseIdx + 1;
					}
				}
			}
			
			_subGeometry.setAutoDeriveVertexNormals(true);
			_subGeometry.setAutoDeriveVertexTangents(true);
			_subGeometry.updateVertexData(vertices);
			_subGeometry.updateIndexData(indices);
		}
		
		/**
		 * @brief 生成顶点的 UV
		 */
		private void buildUVs()
		{
            MList<float> uvs = new MList<float>();
            int numUvs = (_segmentsH + 1)*(_segmentsW + 1)*2;

            if (_subGeometry.getUVData() != null && numUvs == _subGeometry.getUVData().length())
            {
                uvs = _subGeometry.getUVData();
            }
            else
            {
                uvs = new MList<float>(numUvs);
            }
			
			numUvs = 0;
            // 初始化
            for (uint yi = 0; yi <= _segmentsH; ++yi)
            {
                for (uint xi = 0; xi <= _segmentsW; ++xi)
                {
                    uvs.Add(0);
                    uvs.Add(0);
                }
            }

            // 计算 UV
            numUvs = 0;
            for (uint yi = 0; yi <= _segmentsH; ++yi) 
            {
				for (uint xi = 0; xi <= _segmentsW; ++xi) 
                {
					uvs[numUvs++] = (float)xi / _segmentsW;
					uvs[numUvs++] = 1 - (float)yi / _segmentsH;
				}
			}
			
			_subGeometry.updateUVData(uvs);
		}
		
        /**
         * @brief 设置几何无效
         */
		protected void invalidateGeometry()
		{
			_geomDirty = true;
            invalidateBounds();
		}

        /**
         * @brief 设置 UV 无效
         */
		protected void invalidateUVs()
		{
			_uvDirty = true;
		}
    }
}