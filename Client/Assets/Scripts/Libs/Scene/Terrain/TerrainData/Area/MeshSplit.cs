namespace SDK.Lib
{
    /**
     * @brief Mesh 分割
     */
    public class MeshSplit
    {
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

            int xBaseVertex = 0;
            int zBaseVertex = 0;
            terrainPageCfg.calcAreaBaseVertex(ref xBaseVertex, ref zBaseVertex, idx, idz);

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
                    uvs[numUvs++] = (float)xi / segmentsW;
                    uvs[numUvs++] = 1 - (float)yi / segmentsH;
                }
            }

            return true;
        }
    }
}