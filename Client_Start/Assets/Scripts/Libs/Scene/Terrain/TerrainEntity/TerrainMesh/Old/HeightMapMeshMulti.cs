using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 多个 Tile 的 HeightMapMesh
     */
    public class HeightMapMeshMulti : MMesh
    {
        protected HeightMapData m_heightMapData;        // 高度图数据
        protected TerrainPageCfg m_terrainPageCfg;  // Page 配置
        protected bool m_bInLocal;      // 是否生成的每一个 Tile 中的顶点是放在局部空间中
        protected bool m_bCreateVertexIndexInOne;   // 同时创建顶点索引
        protected TerrainMat m_terrainMat;

        /**
         * @brief 构造函数
         */
        public HeightMapMeshMulti(HeightMapData heightMap, TerrainPageCfg terrainPageCfg)
            : base(null, null)
        {
            m_heightMapData = heightMap;
            m_terrainPageCfg = terrainPageCfg;
            m_bInLocal = true;
            m_bCreateVertexIndexInOne = true;

            m_terrainMat = new TerrainMat();
            m_terrainMat.loadDiffuseMat();
            //m_terrainMat.loadSpecularMat();
            buildMutilTileMesh();
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
        protected void buildMutilTileMesh()
        {
            int idx = 0;
            int idz = 0;
            // 从左下角到右上角，不是从左上角到右下角
            for(idz = 0; idz < m_terrainPageCfg.getZTileCount(); ++idz)
            {
                for (idx = 0; idx < m_terrainPageCfg.getXTileCount(); ++idx)
                {
                    buildTile(idx, idz);
                }
            }
        }

        /**
         * @brief 生成一个 Page 中的一个 Tile
         */
        protected void buildTile(int idx, int idz)
        {
            // 生成 SubMesh
            MSubGeometry subGeometry = new MSubGeometry();
            SingleTileRender render = new SingleTileRender();
            render.setCopyMaterial(m_terrainMat.getDiffuseMaterial());
            //render.setCopyMaterial(m_terrainMat.getSpecularMaterial());
            MSubMesh subMesh = new MSubMesh(subGeometry, render);
            subMesh.setTileXZ(idx, idz);
            render.setSubGeometry(subGeometry);
            this.addSubMesh(subMesh);

            if (m_terrainPageCfg.getIsBuildAllTileAndNoClip())
            {
                buildUVByTileIdx(idx, idz, ref subGeometry);
                buildVertexByTileIdx(idx, idz, ref subGeometry);
                buildIndexByTileIdx(idx, idz, ref subGeometry);
                buildVertexAndIndexByTileIdx(idx, idz, ref subGeometry);

                //if (1 == idx && 1 == idz)
                //{
                //    Vector3[] normals = subGeometry.getVertexNormalsData();
                //    Ctx.m_instance.m_fileSys.writeStr2File("A_buildNormal" + idx + "_" + idz + ".txt", normals);
                //    Vector4[] tangents = subGeometry.getVertexTangentsData();
                //    Ctx.m_instance.m_fileSys.writeStr2File("A_buildTangent" + idx + "_" + idz + ".txt", tangents);
                //}

                // 移动到正确的位置
                int tileWidth = m_terrainPageCfg.getTileWorldWidth();
                int tileDepth = m_terrainPageCfg.getTileWorldDepth();

                // 如果是在局部空间中放置的 Tile 中的顶点，需要移动每一块的位置，如果直接将 Area 中的顶点放在世界空间具体位置了，位置顶点中已经放置到正确的位置了
                if (m_bInLocal)
                {
                    subMesh.moveToPos(idx * tileWidth + tileWidth / 2, idz * tileDepth + tileDepth / 2);    // + areaWidth / 2 是为了将所有的顶点的世界范围都放在 >= 0 的范围内
                }
            }
        }

        public void buildUVByTileIdx(int idx, int idz, ref MSubGeometry subGeometry)
        {
            // 生成 UV 坐标
            Vector2[] uvs = null;
            MeshSplit.buildUVs(idx, idz, m_heightMapData, m_terrainPageCfg, ref uvs);
            subGeometry.updateUVData(uvs);

            //Ctx.m_instance.m_fileSys.serializeArray<float>("buildVU.txt", uvs.ToArray(), 2);

            //if (1 == idx && 1 == idz)
            //{
            //    Ctx.m_instance.m_fileSys.writeStr2File("A_buildVU" + idx + "_" + idz + ".txt", uvs);
            //}
        }

        public void buildVertexByTileIdx(int idx, int idz, ref MSubGeometry subGeometry)
        {
            // 生成顶点数据
            Vector3[] vertices = null;
            if (!m_bCreateVertexIndexInOne)
            {
                MeshSplit.buildVertex(idx, idz, m_heightMapData, m_terrainPageCfg, ref vertices, m_bInLocal);
                subGeometry.setAutoDeriveVertexNormals(true);
                subGeometry.updateVertexData(vertices);

                //Ctx.m_instance.m_fileSys.serializeArray<float>("buildVertex.txt", vertices.ToArray(), 3);

                //if (1 == idx && 1 == idz)
                //{
                //    Ctx.m_instance.m_fileSys.writeStr2File("A_buildVertex" + idx + "_" + idz + ".txt", vertices);
                //}
            }
        }

        public void buildIndexByTileIdx(int idx, int idz, ref MSubGeometry subGeometry)
        {
            // 生成索引数据
            int[] indices = null;
            if (!m_bCreateVertexIndexInOne)
            {
                MeshSplit.buildIndex(idx, idz, m_heightMapData, m_terrainPageCfg, ref indices);
                subGeometry.setAutoDeriveVertexTangents(true);
                subGeometry.updateIndexData(indices);

                //Ctx.m_instance.m_fileSys.serializeArray<int>("buildIndex.txt", indices.ToArray(), 3);

                //if (1 == idx && 1 == idz)
                //{
                //    Ctx.m_instance.m_fileSys.serializeArray<int>("A_buildIndex" + idx + "_" + idz + ".txt", indices, 3);
                //}
            }
        }

        public void buildVertexAndIndexByTileIdx(int idx, int idz, ref MSubGeometry subGeometry)
        {
            Vector3[] vertices = null;
            int[] indices = null;

            if (m_bCreateVertexIndexInOne)
            {
                MeshSplit.buildVertexAndIndex(idx, idz, m_heightMapData, m_terrainPageCfg, ref vertices, ref indices, m_bInLocal);
                subGeometry.setAutoDeriveVertexNormals(true);
                subGeometry.updateVertexData(vertices);
                subGeometry.setAutoDeriveVertexTangents(true);
                subGeometry.updateIndexData(indices);

                //Ctx.m_instance.m_fileSys.serializeArray<float>("buildVertex.txt", vertices.ToArray(), 3);
                //Ctx.m_instance.m_fileSys.serializeArray<int>("buildIndex.txt", indices.ToArray(), 3);

                //if (1 == idx && 1 == idz)
                //{
                //    Ctx.m_instance.m_fileSys.writeStr2File("A_buildVertex" + idx + "_" + idz + ".txt", vertices);
                //    Ctx.m_instance.m_fileSys.serializeArray<int>("A_buildIndex" + idx + "_" + idz + ".txt", indices, 3);
                //}
            }
        }

        // 根据 Tile 坐标获取对应的 Tile Mesh
        public MSubMesh getTileMesh(int xTile, int zTile)
        {
            return m_subMeshes[zTile * m_terrainPageCfg.getXTileCount() + xTile];
        }

        /**
         * @brief 根据 Tile 在数组中的索引直接获取对应的 Mesh
         */
        public MSubMesh getTileMesh(int tileIndex)
        {
            return m_subMeshes[tileIndex];
        }
    }
}