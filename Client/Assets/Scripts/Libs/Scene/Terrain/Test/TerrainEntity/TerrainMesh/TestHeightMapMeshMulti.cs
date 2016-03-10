namespace SDK.Lib
{
    /**
     * @brief 多个 Tile 的 HeightMapMesh
     */
    public class TestHeightMapMeshMulti : MTestMesh
    {
        protected TestHeightMapData m_heightMapData;        // 高度图数据
        protected TestTerrainPageCfg m_terrainPageCfg;  // Page 配置
        protected bool m_bInLocal;      // 是否生成的每一个 Tile 中的顶点是放在局部空间中
        protected bool m_bCreateVertexIndexInOne;   // 同时创建顶点索引

        /**
         * @brief 构造函数
         */
        public TestHeightMapMeshMulti(TestHeightMapData heightMap, TestTerrainPageCfg terrainPageCfg)
            : base(null, null)
        {
            m_heightMapData = heightMap;
            m_bInLocal = true;
            m_bCreateVertexIndexInOne = true;
            m_terrainPageCfg = terrainPageCfg;
            buildMutilTileMesh(heightMap, terrainPageCfg);
        }

        public float getHeightAt(float x, float z)
        {
            int pixX = (int)(x / m_terrainPageCfg.getWorldWidth() + 0.5) * (m_heightMapData.getWidth() - 1);       // + 0.5 将地形世界坐标点转换成高度图中图像的像素的坐标点
            int pixZ = (int)(-z / m_terrainPageCfg.getWorldDepth() + 0.5) * (m_heightMapData.getHeight() - 1);
            uint col = (uint)(m_heightMapData.getPixel(pixX, pixZ)) & 0xff;
            return (col > m_terrainPageCfg.getMaxHeight()) ? ((float)m_terrainPageCfg.getMaxHeight() / 0xff) * m_terrainPageCfg.getWorldHeight() : ((col < m_terrainPageCfg.getMinHeight()) ? ((float)m_terrainPageCfg.getMinHeight() / 0xff) * m_terrainPageCfg.getWorldHeight() : ((float)col / 0xff) * m_terrainPageCfg.getWorldHeight());
        }

        /**
         * @brief 生成多个区域地图
         */
        protected void buildMutilTileMesh(TestHeightMapData heightMap, TestTerrainPageCfg terrainPageCfg)
        {
            int idx = 0;
            int idz = 0;
            // 从左下角到右上角，不是从左上角到右下角
            for(idz = 0; idz < terrainPageCfg.getZTileCount(); ++idz)
            {
                for (idx = 0; idx < terrainPageCfg.getXTileCount(); ++idx)
                {
                    buildTile(idx, idz, heightMap, terrainPageCfg);
                }
            }
        }

        /**
         * @brief 生成一个 Page 中的一个 Tile
         */
        protected void buildTile(int idx, int idz, TestHeightMapData heightMap, TestTerrainPageCfg terrainPageCfg)
        {
            // 生成 SubMesh
            MTestSubGeometry subGeometry = new MTestSubGeometry();
            TestSingleTileRender render = new TestSingleTileRender();
            MTestSubMesh subMesh = new MTestSubMesh(subGeometry, render);
            subMesh.setTileXZ(idx, idz);
            render.setSubGeometry(subGeometry);
            this.addSubMesh(subMesh);

            // 生成 UV 坐标
            MList<float> uvs = null;
            TestMeshSplit.buildUVs(idx, idz, heightMap, terrainPageCfg, ref uvs);
            subGeometry.updateUVData(uvs);

            // Ctx.m_instance.m_fileSys.serializeArray<float>("buildVU.txt", uvs.ToArray(), 2);

            // 生成顶点数据
            MList<float> vertices = null;
            if (!m_bCreateVertexIndexInOne)
            {
                TestMeshSplit.buildVertex(idx, idz, heightMap, terrainPageCfg, ref vertices, m_bInLocal);
                subGeometry.setAutoDeriveVertexNormals(true);
                subGeometry.updateVertexData(vertices);
            }

            // 生成索引数据
            MList<int> indices = null;
            if (!m_bCreateVertexIndexInOne)
            {
                TestMeshSplit.buildIndex(idx, idz, heightMap, terrainPageCfg, ref indices);
                subGeometry.setAutoDeriveVertexTangents(true);
                subGeometry.updateIndexData(indices);
            }

            if (m_bCreateVertexIndexInOne)
            {
                TestMeshSplit.buildVertexAndIndex(idx, idz, heightMap, terrainPageCfg, ref vertices, ref indices, m_bInLocal);
                subGeometry.setAutoDeriveVertexNormals(true);
                subGeometry.updateVertexData(vertices);
                subGeometry.setAutoDeriveVertexTangents(true);
                subGeometry.updateIndexData(indices);
            }

            //Ctx.m_instance.m_fileSys.serializeArray<float>("buildVertex.txt", vertices.ToArray(), 3);
            //Ctx.m_instance.m_fileSys.serializeArray<int>("buildIndex.txt", indices.ToArray(), 3);

            // 移动到正确的位置
            int tileWidth = terrainPageCfg.getTileWorldWidth();
            int tileDepth = terrainPageCfg.getTileWorldDepth();

            // 如果是在局部空间中放置的 Tile 中的顶点，需要移动每一块的位置，如果直接将 Area 中的顶点放在世界空间具体位置了，位置顶点中已经放置到正确的位置了
            if (m_bInLocal)
            {
                subMesh.moveToPos(idx * tileWidth + tileWidth / 2, idz * tileDepth + tileDepth / 2);    // + areaWidth / 2 是为了将所有的顶点的世界范围都放在 >= 0 的范围内
            }
        }

        // 根据 Tile 坐标获取对应的 Tile Mesh
        public MTestSubMesh getTileMesh(int xTile, int zTile)
        {
            return m_subMeshes[zTile * m_terrainPageCfg.getXTileCount() + xTile];
        }

        /**
         * @brief 根据 Tile 在数组中的索引直接获取对应的 Mesh
         */
        public MTestSubMesh getTileMesh(int tileIndex)
        {
            return m_subMeshes[tileIndex];
        }
    }
}