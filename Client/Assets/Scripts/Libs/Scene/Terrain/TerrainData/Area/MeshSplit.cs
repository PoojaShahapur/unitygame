namespace SDK.Lib
{
    /**
     * @brief Mesh 分割
     */
    public class MeshSplit
    {
        /**
         * @brief 生成顶点数据，注意这个顶点数据在局部空间已经移动到具体的位置了
         * @param inLocal 是否是在局部空间中生成数据
         */
        static public void buildVertex(int idx, int idz, HeightMapData heightMap, TerrainPageCfg terrainPageCfg, ref MList<float> vertices, bool bInLocal = false)
        {
            int segmentsW = terrainPageCfg.getXGridCountPerArea();
            int segmentsH = terrainPageCfg.getZGridCountPerArea();

            int totalSegmentsW = terrainPageCfg.getXTotalGrid();
            int totalSegmentsH = terrainPageCfg.getZTotalGrid();

            int xSegmentOffset = 0;
            int zSegmentOffset = 0;
            terrainPageCfg.calcAreaSegmentOffset(ref xSegmentOffset, ref zSegmentOffset, idx, idz);

            int width = terrainPageCfg.getWorldWidth();
            int depth = terrainPageCfg.getWorldDepth();

            int minElevation = terrainPageCfg.getMinElevation();
            int maxElevation = terrainPageCfg.getMaxElevation();
            int height = terrainPageCfg.getWorldHeight();

            int areaWidth = terrainPageCfg.getAreaWorldWidth();
            int areaDepth = terrainPageCfg.getAreaWorldDepth();

            float x = 0, z = 0;
            int tw = segmentsW + 1;
            int numVerts = (segmentsH + 1) * tw;    // 矩形区域中顶点的总数
            float uDiv = (float)(heightMap.getWidth() - 1) / totalSegmentsW;    // 世界空间中单位长度对应像素的个数
            float vDiv = (float)(heightMap.getHeight() - 1) / totalSegmentsH;
            float u = 0, v = 0;
            float y = 0;

            if (vertices == null)
            {
                vertices = new MList<float>(numVerts * 3); // 顶点的数量
            }
            else
            {
                vertices.Clear();
            }

            numVerts = 0;
            // 初始化
            for (int zi = 0; zi <= segmentsH; ++zi)
            {
                for (int xi = 0; xi <= segmentsW; ++xi)
                {
                    vertices.Add(0);
                    vertices.Add(0);
                    vertices.Add(0);
                }
            }

            numVerts = 0;
            uint col = 0;

            for (int zi = 0; zi <= segmentsH; ++zi)
            {
                for (int xi = 0; xi <= segmentsW; ++xi)
                {
                    // (float) 一定要先转换成 (float) ，否则 xi / segmentsW 整数除总是 0 ，导致结果总是在一个顶点
                    if (!bInLocal)  // 如果直接放在世界空间中
                    {
                        x = (int)((((float)xi + xSegmentOffset) / totalSegmentsW - 0.5f) * width);            // -0.5 保证原点放在地形的中心点
                        z = (int)((((float)zi + zSegmentOffset) / totalSegmentsH - 0.5f) * depth);
                    }
                    else    // 否则放在局部空间中，需要自己移动 GameObject 的 Transform 位置信息
                    {
                        x = (int)(((float)xi / segmentsW - 0.5f) * areaWidth);            // -0.5 保证原点放在地形的中心点
                        z = (int)(((float)zi / segmentsH - 0.5f) * areaDepth);
                    }
                    u = (xi + xSegmentOffset) * uDiv;
                    v = (totalSegmentsH - (zi + zSegmentOffset)) * vDiv;

                    col = (uint)(heightMap.getPixel((int)u, (int)v)) & 0xff;
                    y = (col > maxElevation) ? ((float)maxElevation / 0xff) * height : ((col < minElevation) ? ((float)minElevation / 0xff) * height : ((float)col / 0xff) * height);         // col 是 [0, 255] 的灰度值，col / 0xff 就是 [0, 1] 的灰度值，col / 0xff 两个整数除，如果要得到 float ，一定要写成 (float)col / 0xff，否则是四舍五入的整数值

                    vertices[numVerts++] = x;
                    vertices[numVerts++] = y;
                    vertices[numVerts++] = z;
                }
            }
        }

        /**
         * @brief 生成索引数据
         */
        static public void buildIndex(int idx, int idz, HeightMapData heightMap, TerrainPageCfg terrainPageCfg, ref MList<int> indices)
        {
            int segmentsW = terrainPageCfg.getXGridCountPerArea();
            int segmentsH = terrainPageCfg.getZGridCountPerArea();

            int totalSegmentsW = terrainPageCfg.getXTotalGrid();
            int totalSegmentsH = terrainPageCfg.getZTotalGrid();

            uint numInds = 0;
            int baseIdx = 0;
            int tw = segmentsW + 1;
            float uDiv = (float)(heightMap.getWidth() - 1) / totalSegmentsW;
            float vDiv = (float)(heightMap.getHeight() - 1) / totalSegmentsH;

            indices = new MList<int>(segmentsH * segmentsW * 6);  // 索引的数量

            // 初始化
            for (int zi = 0; zi <= segmentsH; ++zi)
            {
                for (int xi = 0; xi <= segmentsW; ++xi)
                {
                    if (xi != segmentsW && zi != segmentsH)
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

            for (int zi = 0; zi <= segmentsH; ++zi)
            {
                for (int xi = 0; xi <= segmentsW; ++xi)
                {
                    if (xi != segmentsW && zi != segmentsH)   // 循环中计数已经多加了 1 ，因此，这里如果超过范围直接返回，只有在范围内的值，才更新
                    {
                        baseIdx = xi + zi * tw;
                        indices[(int)numInds++] = baseIdx;
                        indices[(int)numInds++] = baseIdx + tw;
                        indices[(int)numInds++] = baseIdx + tw + 1;
                        indices[(int)numInds++] = baseIdx;
                        indices[(int)numInds++] = baseIdx + tw + 1;
                        indices[(int)numInds++] = baseIdx + 1;
                    }
                }
            }
        }

        /**
         * @brief 同时生成顶点和索引数据
         */
        static public void buildVertexAndIndex(int idx, int idz, HeightMapData heightMap, TerrainPageCfg terrainPageCfg, ref MList<float> vertices, ref MList<int> indices, bool bInLocal = false)
        {
            int segmentsW = terrainPageCfg.getXGridCountPerArea();
            int segmentsH = terrainPageCfg.getZGridCountPerArea();

            int totalSegmentsW = terrainPageCfg.getXTotalGrid();
            int totalSegmentsH = terrainPageCfg.getZTotalGrid();

            int xSegmentOffset = 0;
            int zSegmentOffset = 0;
            terrainPageCfg.calcAreaSegmentOffset(ref xSegmentOffset, ref zSegmentOffset, idx, idz);

            int width = terrainPageCfg.getWorldWidth();
            int depth = terrainPageCfg.getWorldDepth();

            int minElevation = terrainPageCfg.getMinElevation();
            int maxElevation = terrainPageCfg.getMaxElevation();
            int height = terrainPageCfg.getWorldHeight();

            int areaWidth = terrainPageCfg.getAreaWorldWidth();
            int areaDepth = terrainPageCfg.getAreaWorldDepth();

            float x = 0, z = 0;
            uint numInds = 0;
            int baseIdx = 0;
            int tw = segmentsW + 1;
            int numVerts = (segmentsH + 1) * tw;
            float uDiv = (float)(heightMap.getWidth() - 1) / totalSegmentsW;
            float vDiv = (float)(heightMap.getHeight() - 1) / totalSegmentsH;
            float u = 0, v = 0;
            float y = 0;

            if(vertices == null && indices == null)
            {
                vertices = new MList<float>(numVerts * 3); // 顶点的数量
                indices = new MList<int>(segmentsH * segmentsW * 6);  // 索引的数量
            }

            // 初始化
            for (int zi = 0; zi <= segmentsH; ++zi)
            {
                for (int xi = 0; xi <= segmentsW; ++xi)
                {
                    vertices.Add(0);
                    vertices.Add(0);
                    vertices.Add(0);

                    if (xi != segmentsW && zi != segmentsH)
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

            for (int zi = 0; zi <= segmentsH; ++zi)
            {
                for (int xi = 0; xi <= segmentsW; ++xi)
                {
                    // (float) 一定要先转换成 (float) ，否则 xi / segmentsW 整数除总是 0 ，导致结果总是在一个顶点
                    if (!bInLocal)
                    {
                        x = (int)((((float)xi + xSegmentOffset) / totalSegmentsW - 0.5f) * width);            // -0.5 保证原点放在地形的中心点
                        z = (int)((((float)zi + zSegmentOffset) / totalSegmentsH - 0.5f) * depth);
                    }
                    else    // 否则放在局部空间中，需要自己移动 GameObject 的 Transform 位置信息
                    {
                        x = (int)(((float)xi / segmentsW - 0.5f) * areaWidth);            // -0.5 保证原点放在地形的中心点
                        z = (int)(((float)zi / segmentsH - 0.5f) * areaDepth);
                    }
                    u = (xi + xSegmentOffset) * uDiv;
                    v = (totalSegmentsH - (zi + zSegmentOffset)) * vDiv;

                    col = (uint)(heightMap.getPixel((int)u, (int)v)) & 0xff;
                    y = (col > maxElevation) ? ((float)maxElevation / 0xff) * height : ((col < minElevation) ? ((float)minElevation / 0xff) * height : ((float)col / 0xff) * height);         // col 是 [0, 255] 的灰度值，col / 0xff 就是 [0, 1] 的灰度值，col / 0xff 两个整数除，如果要得到 float ，一定要写成 (float)col / 0xff，否则是四舍五入的整数值

                    vertices[numVerts++] = x;
                    vertices[numVerts++] = y;
                    vertices[numVerts++] = z;

                    if (xi != segmentsW && zi != segmentsH)   // 循环中计数已经多加了 1 ，因此，这里如果超过范围直接返回，只有在范围内的值，才更新
                    {
                        baseIdx = xi + zi * tw;
                        indices[(int)numInds++] = baseIdx;
                        indices[(int)numInds++] = baseIdx + tw;
                        indices[(int)numInds++] = baseIdx + tw + 1;
                        indices[(int)numInds++] = baseIdx;
                        indices[(int)numInds++] = baseIdx + tw + 1;
                        indices[(int)numInds++] = baseIdx + 1;
                    }
                }
            }
        }

        /**
         * @brief 生成顶点的 UV
         * @param idx 在 area 中偏移的 X 方向的位置
         * @param idz 在 area 中偏移的 Z 方向的位置
         */
        static public bool buildUVs(int idx, int idz, HeightMapData heightMap, TerrainPageCfg terrainPageCfg, ref MList<float> uvs)
        {
            // 当前 Area 中划分的片段数量，其实就是 Area 中 Grid 的数量
            int segmentsW = terrainPageCfg.getXGridCountPerArea();
            int segmentsH = terrainPageCfg.getZGridCountPerArea();

            int totalSegmentsW = terrainPageCfg.getXTotalGrid();
            int totalSegmentsH = terrainPageCfg.getZTotalGrid();

            int xSegmentOffset = 0;
            int zSegmentOffset = 0;
            terrainPageCfg.calcAreaSegmentOffset(ref xSegmentOffset, ref zSegmentOffset, idx, idz);

            int numUvs = (segmentsH + 1) * (segmentsW + 1) * 2;
            if (uvs == null)
            {
                uvs = new MList<float>(numUvs);
            }
            else
            {
                uvs.Clear();
            }

            numUvs = 0;
            // 初始化，遍历范围 [0, segmentsH] * [0, segmentsW]
            for (uint yi = 0; yi <= segmentsH; ++yi)
            {
                for (uint xi = 0; xi <= segmentsW; ++xi)
                {
                    uvs.Add(0);
                    uvs.Add(0);
                }
            }

            // 计算 UV
            numUvs = 0;
            for (uint yi = 0; yi <= segmentsH; ++yi)
            {
                for (uint xi = 0; xi <= segmentsW; ++xi)
                {
                    uvs[numUvs++] = ((float)xi + xSegmentOffset) / totalSegmentsW;
                    uvs[numUvs++] = 1 - ((float)yi + zSegmentOffset) / totalSegmentsH;
                }
            }

            return true;
        }
    }
}