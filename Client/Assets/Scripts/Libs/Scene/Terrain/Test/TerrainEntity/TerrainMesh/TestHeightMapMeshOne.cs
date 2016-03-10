namespace SDK.Lib
{
    /**
     * @brief 高度地形 Mesh
     */
    public class TestHeightMapMeshOne : MTestMesh
    {
        protected int m_segmentsW;      // 世界空间高度图宽度划分的线段数量， X 轴线段数量
		protected int m_segmentsH;      // 世界空间高度图高度划分的线段数量， Z 轴线段数量
        protected float m_width;        // 世界空间高度图宽度， X 轴宽度
        protected float m_height;       // 世界空间高度图高度， Z 轴高度
        protected float m_depth;        // 世界空间高度图深度，这个等价于 Z 的世界空间高度
        protected TestHeightMapData m_heightMap;                // 高度图
		protected TestHeightMapData m_smoothedHeightMap;        // 平滑的高度图
		protected TestHeightMapData m_activeMap;                // 获取高度的高度图
		protected uint m_minHeight;                      // 高度图最小高度
		protected uint m_maxHeight;                      // 高度图最大高度
		protected bool m_geomDirty;                         // Geometry 数据是否是被修改
		protected bool m_uvDirty;                           // UV 数据是否被修改
		protected MTestSubGeometry m_subGeometry;               // SubGeometry 数据

        /**
         * @brief 构造函数
         */
        public TestHeightMapMeshOne(TestHeightMapData heightMap, float width = 1000, float height = 100, float depth = 1000, int segmentsW = 30, int segmentsH = 30, uint maxHeight = 255, uint minHeight = 0, bool smoothMap = false)
            : base(new MTestGeometry(), new TestSingleTileRender())
        {
            buildOneTileMesh(heightMap, width, height, depth, segmentsW, segmentsH, maxHeight, minHeight, smoothMap);
        }

        /**
         * @breif 一个 Page 就是一个 Tile 的地形生成方法
         */
        protected void buildOneTileMesh(TestHeightMapData heightMap, float width = 1000, float height = 100, float depth = 1000, int segmentsW = 30, int segmentsH = 30, uint maxHeight = 255, uint minHeight = 0, bool smoothMap = false)
        {
            m_subGeometry = new MTestSubGeometry();
            this.getGeometry().addSubGeometry(m_subGeometry);
            m_meshRender.setSubGeometry(m_subGeometry);      // 设置几何信息

            m_geomDirty = true;
            m_uvDirty = true;

            m_heightMap = heightMap;
            m_activeMap = m_heightMap;
            m_segmentsW = segmentsW;
            m_segmentsH = segmentsH;
            m_width = width;
            m_height = height;
            m_depth = depth;
            m_maxHeight = maxHeight;
            m_minHeight = minHeight;

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
       public void setMinHeight(uint val)
		{
            if (m_minHeight == val)
            {
                return;
            }

            m_minHeight = val;
            invalidateGeometry();
        }

        /**
         * @brief 获取最小高度
         */
        public uint getMinHeight()
		{
			return m_minHeight;
		}

        /**
         * @brief 设置高度图最大高度
         */
        public void setMaxHeight(uint val)
		{
            if (m_maxHeight == val)
            {
                return;
            }

            m_maxHeight = val;
            invalidateGeometry();
		}
		
        /**
         * @brief 获取最大高度
         */
		public uint getMaxHeight()
		{
			return m_maxHeight;
		}

        /**
         * @brief 获取 Z 轴世界空间分割的 Segment 的数量
		 */
        public int getSegmentsH()
		{
			return m_segmentsH;
		}
		
        /**
         * @brief 设置 Z 空间划分的线段数量
         */
		public void setSegmentsH(int value)
		{
			m_segmentsH = value;
            invalidateGeometry();
            invalidateUVs();
		}
		
		/**
		 * @brief 获取世界空间 X 轴大小
		 */
		public float getWidth()
		{
			return m_width;
		}
		
        /**
         * @brief 设置世界空间 X 轴大小
         */
		public void setWidth(float value)
		{
			m_width = value;
            invalidateGeometry();
		}
		
        /**
         * @brief 获取世界空间 Y 轴的高度
         */
		public float getHeight()
		{
			return m_height;
		}
		
        /**
         * @brief 设置世界空间 Y 轴的高度
         */
		public void setHeight(float value)
		{
			m_height = value;
		}
		
		/**
		 * @brief 地形 Mesh 的深度，就是 Z 值的世界空间的长度
		 */
		public float getDepth()
		{
			return m_depth;
		}
		
        /**
         * @brief 设置世界空间 Z 轴的深度
         */
		public void setDepth(float value)
		{
			m_depth = value;
            invalidateGeometry();
		}
		
		/**
		 * @brief 获取地形的某一个坐标的高度，注意 x 和 z 是世界场景中的，世界中的坐标中心点 (0, 0) 在地形的中间，因此 x 和 z 可能是负值
         * @param x 是世界中的坐标， x 值可能是负值，如果要转换成高度图中的坐标，需要 + 0.5，因为世界中心点 (0, 0) 在地形的中间， + 0.5 将世界坐标点转成高度图坐标点
		 */
		public float getHeightAt(float x, float z)
		{
            int pixX = (int)(x / m_width + 0.5) * (m_activeMap.getWidth() - 1);       // + 0.5 将地形世界坐标点转换成高度图中图像的像素的坐标点
            int pixZ = (int)(-z / m_depth + 0.5) * (m_activeMap.getHeight() - 1);
            uint col = (uint)(m_activeMap.getPixel(pixX, pixZ)) & 0xff;
            return (col > m_maxHeight) ? ((float)m_maxHeight / 0xff) * m_height : ((col < m_minHeight) ? ((float)m_minHeight / 0xff) * m_height : ((float)col / 0xff) * m_height);
        }

        /**
         * @brief 生成平滑的高度图
         */
        public TestHeightMapData generateSmoothedHeightMap()
		{
            if (m_smoothedHeightMap != null)
            {
                m_smoothedHeightMap.dispose();
            }
			m_smoothedHeightMap = new TestHeightMapData(m_heightMap.getWidth(), m_heightMap.getHeight());

            int w = m_smoothedHeightMap.getWidth();
            int h = m_smoothedHeightMap.getHeight();
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
			
			m_smoothedHeightMap.lockMem();
			
			float incXL = 0;
            float incXR = 0;
            float incYL = 0;
            float incYR = 0;
            float pxx = 0;
            float pxy = 0;
			
			for (i = 0; i < w + 1; i += m_segmentsW)         // 遍历像素宽度，这个遍历的一个前提就是 _segmentsW 世界空间划分的段数要小于等于像素的数量
            {
                if (i + m_segmentsW > w - 1)     // 遍历的像素的空间是 [0, w - 1]，如果当前 i 超出这个范围，就取最后的一个像素
                {
                    lockx = w - 1;
                }
                else    // 如果当前像素的下一个 i + _segmentsW 像素在 [0, w - 1] 范围内
                {
                    lockx = i + m_segmentsW;
                }
				
				for (j = 0; j < h + 1; j += m_segmentsH)
                {
                    if (j + m_segmentsH > h - 1)
                    {
                        locky = h - 1;
                    }
                    else
                    {
                        locky = j + m_segmentsH;
                    }
					
					if (j == 0)         // 如果是在像素的第一行，就是矩形的四个顶点
                    {
						px1 = (uint)(m_heightMap.getPixel(i, j)) & 0xFF;
						px1 = (px1 > m_maxHeight) ? m_maxHeight : ((px1 < m_minHeight) ? m_minHeight : px1);
						px2 = (uint)(m_heightMap.getPixel(lockx, j)) & 0xFF;
						px2 = (px2 > m_maxHeight) ? m_maxHeight : ((px2 < m_minHeight) ? m_minHeight : px2);
						px3 = (uint)(m_heightMap.getPixel(lockx, locky)) & 0xFF;
						px3 = (px3 > m_maxHeight) ? m_maxHeight : ((px3 < m_minHeight) ? m_minHeight : px3);
						px4 = (uint)(m_heightMap.getPixel(i, locky)) & 0xFF;
						px4 = (px4 > m_maxHeight) ? m_maxHeight : ((px4 < m_minHeight) ? m_minHeight : px4);
					}
                    else                // 如果不是第一行， px1 就直接取上一行的 px4
                    {
						px1 = px4;      // px1 就直接取上一行的 px4
                        px2 = px3;
						px3 = (uint)(m_heightMap.getPixel(lockx, locky)) & 0xFF;
						px3 = (px3 > m_maxHeight) ? m_maxHeight : ((px3 < m_minHeight) ? m_minHeight : px3);
						px4 = (uint)(m_heightMap.getPixel(i, locky)) & 0xFF;
						px4 = (px4 > m_maxHeight) ? m_maxHeight : ((px4 < m_minHeight) ? m_minHeight : px4);
					}
					
					for (k = 0; k < m_segmentsW; ++k)    // 遍历当前获取的矩形像素区域的所有像素
                    {
						incXL = (float)1 / m_segmentsW * k;      // 1 / _segmentsW * k 范围是 [0, 1)，当前点在矩形区域中距离左边的比例
                        incXR = 1 - incXL;                      // 当前点在矩形区域中距离右边的比例，比例范围是 [0, 1]
						
						for (l = 0; l < m_segmentsH; ++l)
                        {
							incYL = (float)1 / m_segmentsH * l;
                            incYR = 1 - incYL;
							
							pxx = ((px1 * incXR) + (px2 * incXL)) * incYR;  // 矩形区域插值计算高度，就是两次线性差值
                            pxy = ((px4 * incXR) + (px3 * incXL)) * incYL;
                            
                            m_smoothedHeightMap.setPixel(k + i, l + j, (uint)((int)(pxy + pxx) << 16 | (int)(pxy + pxx) << 8 | (int)(pxy + pxx)));       // pxy + pxx 第二次线性差值计算的高度
                        }
					}
				}
			}
			m_smoothedHeightMap.unlock();
			
			m_activeMap = m_smoothedHeightMap;
			
			return m_smoothedHeightMap;
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
            int tw = m_segmentsW + 1;               // 宽度方向顶点数量
            int numVerts = (m_segmentsH + 1)* tw;   // 高度方向顶点数量
            float uDiv = (float)(m_heightMap.getWidth() - 1) / m_segmentsW;     // X 方向单位分段对应当像素数量
			float vDiv = (float)(m_heightMap.getHeight() - 1) / m_segmentsH;    // Z 方向单位分段对应当像素数量
            float u = 0, v = 0;
			float y = 0;
			
			if (numVerts == m_subGeometry.getNumVertices())
            {
				vertices = m_subGeometry.getVertexData();
				indices = m_subGeometry.getIndexData();
			}
            else
            {
				vertices = new MList<float>(numVerts * 3); // 顶点的数量
				indices = new MList<int>(m_segmentsH * m_segmentsW * 6);  // 索引的数量
			}

            numVerts = 0;
            // 初始化
            for (int zi = 0; zi <= m_segmentsH; ++zi)
            {
                for (int xi = 0; xi <= m_segmentsW; ++xi)
                {
                    vertices.Add(0);
                    vertices.Add(0);
                    vertices.Add(0);

                    if (xi != m_segmentsW && zi != m_segmentsH)
                    {
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
			
			for (int zi = 0; zi <= m_segmentsH; ++zi)
            {
				for (int xi = 0; xi <= m_segmentsW; ++xi)
                {
                    // (float) 一定要先转换成 (float) ，否则 xi / m_segmentsW 整数除总是 0 ，导致结果总是在一个顶点
                    x = (int)(((float)xi / m_segmentsW - 0.5f) * m_width);            // -0.5 保证原点放在地形的中心点
                    z = (int)(((float)zi / m_segmentsH - 0.5f) * m_depth);
                    u = xi * uDiv;
                    v = (m_segmentsH - zi) * vDiv;

                    col = (uint)(m_heightMap.getPixel((int)u, (int)v)) & 0xff;
					y = (col > m_maxHeight) ? ((float)m_maxHeight / 0xff)* m_height : ((col < m_minHeight) ? ((float)m_minHeight / 0xff) * m_height : ((float)col / 0xff) * m_height);         // col 是 [0, 255] 的灰度值，col / 0xff 就是 [0, 1] 的灰度值，col / 0xff 两个整数除，如果要得到 float ，一定要写成 (float)col / 0xff，否则是四舍五入的整数值

                    vertices[numVerts++] = x;
					vertices[numVerts++] = y;
					vertices[numVerts++] = z;
					
					if (xi != m_segmentsW && zi != m_segmentsH)   // 循环中计数已经多加了 1 ，因此，这里如果超过范围直接返回，只有在范围内的值，才更新
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
			
			m_subGeometry.setAutoDeriveVertexNormals(true);
			m_subGeometry.setAutoDeriveVertexTangents(true);
			m_subGeometry.updateVertexData(vertices);
			m_subGeometry.updateIndexData(indices);

            //Ctx.m_instance.m_fileSys.serializeArray<float>("buildVertex_1.txt", vertices.ToArray(), 3);
            //Ctx.m_instance.m_fileSys.serializeArray<int>("buildIndex_1.txt", indices.ToArray(), 3);
        }
		
		/**
		 * @brief 生成顶点的 UV
		 */
		private void buildUVs()
		{
            MList<float> uvs = null;
            int numUvs = (m_segmentsH + 1)*(m_segmentsW + 1)*2;

            if (m_subGeometry.getUVData() != null && numUvs == m_subGeometry.getUVData().length())
            {
                uvs = m_subGeometry.getUVData();
            }
            else
            {
                uvs = new MList<float>(numUvs);
            }
			
			numUvs = 0;
            // 初始化，遍历范围 [0, m_segmentsH] * [0, m_segmentsW]
            for (uint yi = 0; yi <= m_segmentsH; ++yi)
            {
                for (uint xi = 0; xi <= m_segmentsW; ++xi)
                {
                    uvs.Add(0);
                    uvs.Add(0);
                }
            }

            // 计算 UV
            numUvs = 0;
            for (uint yi = 0; yi <= m_segmentsH; ++yi) 
            {
				for (uint xi = 0; xi <= m_segmentsW; ++xi) 
                {
					uvs[numUvs++] = (float)xi / m_segmentsW;
					uvs[numUvs++] = 1 - (float)yi / m_segmentsH;    // UV 坐标的 Y 轴是向下的，而顶点的 Z 轴是向上的，因此需要使用 1 - (float)yi / m_segmentsH 获取正确的值
                }
			}
			
			m_subGeometry.updateUVData(uvs);
            //Ctx.m_instance.m_fileSys.serializeArray<float>("buildVU_1.txt", uvs.ToArray(), 2);
        }
		
        /**
         * @brief 设置几何无效
         */
		protected void invalidateGeometry()
		{
			m_geomDirty = true;
            invalidateBounds();
		}

        /**
         * @brief 设置 UV 无效
         */
		protected void invalidateUVs()
		{
			m_uvDirty = true;
		}
    }
}