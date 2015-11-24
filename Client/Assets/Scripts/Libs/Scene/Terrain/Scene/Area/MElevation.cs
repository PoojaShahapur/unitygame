﻿namespace SDK.Lib
{
    /**
     * @brief 高度地形
     */
    public class MElevation : MMesh
    {
        protected uint _segmentsW;  // 世界空间高度图宽度划分的线段数量， X 轴线段数量
		protected uint _segmentsH;  // 世界空间高度图高度划分的线段数量， Z 轴线段数量
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

        public MElevation(MatRes material, HeightMapData heightMap, float width = 1000, float height = 100, float depth = 1000, uint segmentsW = 30, uint segmentsH = 30, uint maxElevation = 255, uint minElevation = 0, bool smoothMap = false)
            : base(new MGeometry(), material)
        {
            _subGeometry = new MSubGeometry();
            this.getGeometry().addSubGeometry(_subGeometry);

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
				return;
			
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
        public uint getSegmentsH()
		{
			return _segmentsH;
		}
		
		public void setSegmentsH(uint value)
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
		 * The depth of the terrain plane.
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
            int pixX = (int)(x / _width + 0.5) * (_activeMap.getWidth() - 1);
            int pixZ = (int)(-z / _depth + 0.5) * (_activeMap.getHeight() - 1);
            uint col = (uint)(_activeMap.getPixel(pixX, pixZ)) & 0xff;
            return (col > _maxElevation) ? (_maxElevation / 0xff) * _height : ((col < _minElevation) ? (_minElevation / 0xff) * _height : (col / 0xff) * _height);
        }

        public HeightMapData generateSmoothedHeightMap()
		{
            if (_smoothedHeightMap != null)
            {
                _smoothedHeightMap.dispose();
            }
			_smoothedHeightMap = new HeightMapData(_heightMap.getWidth(), _heightMap.getHeight());

            uint w = (uint)(_smoothedHeightMap.getWidth());
            uint h = (uint)(_smoothedHeightMap.getHeight());
            uint i;
            uint j;
            uint k;
            uint l;

            uint px1;
            uint px2;
            uint px3 = 0;
            uint px4 = 0;

            uint lockx;
            uint locky;
			
			_smoothedHeightMap.lockMem();
			
			float incXL;
            float incXR;
            float incYL;
            float incYR;
            float pxx;
            float pxy;
			
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
						incXL = 1/_segmentsW* k;
                        incXR = 1 - incXL;
						
						for (l = 0; l<_segmentsH; ++l)
                        {
							incYL = 1/_segmentsH* l;
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

		private void buildGeometry()
		{
            MList<float> vertices;
			MList<uint> indices;
			float x = 0, z = 0;
            uint numInds = 0;
            uint baseIdx = 0;
            uint tw = _segmentsW + 1;
            uint numVerts = (_segmentsH + 1)* tw;
            float uDiv = (_heightMap.getWidth() - 1)/_segmentsW;
			float vDiv = (_heightMap.getHeight() - 1)/_segmentsH;
			float u = 0, v = 0;
			float y = 0;
			
			if (numVerts == _subGeometry.getNumVertices()) {
				vertices = _subGeometry.getVertexData();
				indices = _subGeometry.getIndexData();
			} else {
				vertices = new MList<float>((int)numVerts*3);
				indices = new MList<uint>((int)(_segmentsH * _segmentsW*6));
			}
			
			numVerts = 0;
            uint col;
			
			for (uint zi = 0; zi <= _segmentsH; ++zi) 
            {
				for (uint xi = 0; xi <= _segmentsW; ++xi) 
                {
					x = (int)(xi/_segmentsW - 0.5)* _width;
                    z = (int)(zi/_segmentsH - 0.5)* _depth;
                    u = xi* uDiv;
                    v = (_segmentsH - zi)* vDiv;

                    col = (uint)(_heightMap.getPixel((int)u, (int)v)) & 0xff;
					y = (col > _maxElevation)? (_maxElevation/0xff)* _height : ((col<_minElevation)? (_minElevation/0xff)* _height : (col/0xff)* _height);
					
					vertices[(int)numVerts++] = x;
					vertices[(int)numVerts++] = y;
					vertices[(int)numVerts++] = z;
					
					if (xi != _segmentsW && zi != _segmentsH)
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
            uint numUvs = (_segmentsH + 1)*(_segmentsW + 1)*2;

            if (_subGeometry.getUVData() != null && numUvs == _subGeometry.getUVData().length())
            {
                uvs = _subGeometry.getUVData();
            }
            else
            {
                uvs = new MList<float>((int)numUvs);
            }
			
			numUvs = 0;
			for (uint yi = 0; yi <= _segmentsH; ++yi) 
            {
				for (uint xi = 0; xi <= _segmentsW; ++xi) 
                {
					uvs[(int)numUvs++] = xi/_segmentsW;
					uvs[(int)numUvs++] = 1 - yi/_segmentsH;
				}
			}
			
			_subGeometry.updateUVData(uvs);
		}
		
		protected void invalidateGeometry()
		{
			_geomDirty = true;
            invalidateBounds();
		}

		protected void invalidateUVs()
		{
			_uvDirty = true;
		}
    }
}