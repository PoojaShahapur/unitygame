namespace SDK.Lib
{
    /**
     * @brief 多个 Area 的 HeightMapMesh
     */
    public class HeightMapMeshMulti : MMesh
    {
        protected TerrainPageCfg m_terrainPageCfg;  // Page 配置

        /**
         * @brief 构造函数
         */
        public HeightMapMeshMulti(HeightMapData heightMap, TerrainPageCfg terrainPageCfg)
            : base(null, null)
        {
            m_terrainPageCfg = terrainPageCfg;
            buildMutilAreaMesh(heightMap, terrainPageCfg);
        }

        /**
         * @brief 生成多个区域地图
         */
        protected void buildMutilAreaMesh(HeightMapData heightMap, TerrainPageCfg terrainPageCfg)
        {
            int idx = 0;
            int idz = 0;
            for(idz = 0; idz < terrainPageCfg.getZAreaCount(); ++idz)
            {
                for (idx = 0; idx < terrainPageCfg.getXAreaCount(); ++idx)
                {
                    buildArea(idx, idz, heightMap, terrainPageCfg);
                }
            }
        }

        /**
         * @brief 生成一个 Page 中的一个 Area
         */
        protected void buildArea(int idx, int idz, HeightMapData heightMap, TerrainPageCfg terrainPageCfg)
        {
            // 生成 SubMesh
            MSubGeometry subGeometry = new MSubGeometry();
            SingleAreaRender render = new SingleAreaRender();
            MSubMesh subMesh = new MSubMesh(subGeometry, render);
            render.setSubGeometry(subGeometry);
            this.addSubMesh(subMesh);

            // 生成 UV 坐标
            MList<float> uvs = null;
            MeshSplit.buildUVs(idx, idz, heightMap, terrainPageCfg, ref uvs);
            subGeometry.updateUVData(uvs);
            // Ctx.m_instance.m_localFileSys.serializeArray<float>("buildVU.txt", uvs.ToArray(), 2);

            // 生成顶点数据
            MList<float> vertices = null;
            //MeshSplit.buildVertex(idx, idz, heightMap, terrainPageCfg, ref vertices);
            //subGeometry.setAutoDeriveVertexNormals(true);
            //subGeometry.updateVertexData(vertices);

            // 生成索引数据
            MList<int> indices = null;
            //MeshSplit.buildIndex(idx, idz, heightMap, terrainPageCfg, ref indices);
            //subGeometry.setAutoDeriveVertexTangents(true);
            //subGeometry.updateIndexData(indices);

            MeshSplit.buildVertexAndIndex(idx, idz, heightMap, terrainPageCfg, ref vertices, ref indices);
            //Ctx.m_instance.m_localFileSys.serializeArray<float>("buildVertex.txt", vertices.ToArray(), 3);
            //Ctx.m_instance.m_localFileSys.serializeArray<int>("buildIndex.txt", indices.ToArray(), 3);
            subGeometry.setAutoDeriveVertexNormals(true);
            subGeometry.updateVertexData(vertices);
            subGeometry.setAutoDeriveVertexTangents(true);
            subGeometry.updateIndexData(indices);

            // 移动到正确的位置
            int areaWidth = terrainPageCfg.getAreaWorldWidth();
            int areaDepth = terrainPageCfg.getAreaWorldDepth();
            subMesh.moveToPos(idx * areaWidth, idz * areaDepth);
        }
    }
}