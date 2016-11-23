using SDK.Lib;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ExportTerrain : EditorWindow
{
    public enum SaveFormat
    {
        Triangles,
        Quads,
    }

    public enum SaveResolution
    {
        Full,
        Half,
        Quarter,
        Eighth,
        Sixteenth,
    }

    SaveFormat saveFormat = SaveFormat.Triangles;
    SaveResolution saveResolution = SaveResolution.Full;
    static TerrainData terrainData;
    static Vector3 terrainPos;

    protected int tCount;
    protected int counter;
    protected int totalCount;
    protected string mTerrainId;   // 地形唯一 Id
    protected string mHeightMapNamePrefix = "HeightMap"; // 地形高度文件名字前缀
    protected string mAlphaMapNamePrefix = "AlphaMap";   // Alpha 贴图文件名字前缀

    [MenuItem("My/Terrain/Export Terrain")]
    static public void Init()
    {
        terrainData = null;
        Terrain terrainObject = Selection.activeObject as Terrain;
        if (!terrainObject)
        {
            terrainObject = Terrain.activeTerrain;
        }
        if (terrainObject)
        {
            terrainData = terrainObject.terrainData;
            terrainPos = terrainObject.transform.position;
        }
        EditorWindow.GetWindow(typeof(ExportTerrain)).Show();
    }

    public void OnGUI()
    {
        if (!terrainData)
        {
            GUILayout.Label("No terrain found");
            if (GUILayout.Button("Cancel"))
            {
                EditorWindow.GetWindow(typeof(ExportTerrain)).Close();
            }
            return;
        }

        GUILayout.Label("map name, please use number");
        mTerrainId = GUILayout.TextField("1000");
        mTerrainId = string.Format("T{0}", mTerrainId);
        if (GUILayout.Button("ExportTexture"))
        {
            exportSplatTexture();
        }
        if (GUILayout.Button("ExportHeightMap"))
        {
            exportHeightMap();
            //exportScaleHeightMap();
        }
        if (GUILayout.Button("ExportHeightData"))
        {
            exportHeightData();
        }
        if (GUILayout.Button("ExportAlphaMap"))
        {
            exportAlphaTexture();
            //exportAlphaMap();
        }
        if (GUILayout.Button("ExportXml"))
        {
            exportXml();
        }
        if (GUILayout.Button("ExportSplatXml"))
        {
            exportSplatXml();
        }
        if (GUILayout.Button("ExportTerrain"))
        {
            exportSplatXml();
            exportAlphaTexture();
            exportHeightData();
            exportSplatTexture();
        }
    }

    public void exportAlphaTexture()
    {
        string path = "";
        int idx = 0;
        while (idx < terrainData.alphamapTextures.Length)
        {
            path = string.Format("{0}/Resources/Materials/Textures/Terrain/{1}_{2}_{3}.png", Application.dataPath, mAlphaMapNamePrefix, mTerrainId, idx);
            UtilPath.saveTex2File(terrainData.alphamapTextures[0], path);
            ++idx;
        }
    }

    public void exportSplatTexture_NotRun()
    {
        string path = Application.streamingAssetsPath;
        int idx = 0;
        SplatPrototype splatLayer = null;
        Texture2D writeTex = null;
        Color color;
        for (idx = 0; idx < terrainData.splatPrototypes.Length; ++idx)
        {
            splatLayer = terrainData.splatPrototypes[idx];
            writeTex = new Texture2D(splatLayer.texture.width, splatLayer.texture.height, TextureFormat.RGB24, false);
            
            for(int imageY = 0; imageY < splatLayer.texture.height; ++imageY)
            {
                for(int imageX = 0; imageX < splatLayer.texture.width; ++imageX)
                {
                    // 这个纹理是不能读写的，需要使用 AssetDatabase.GetAssetPath 读取纹理目录
                    color = splatLayer.texture.GetPixel(imageX, imageY);
                    writeTex.SetPixel(imageX, imageY, color);
                }
            }
            UtilPath.saveTex2File(splatLayer.texture, path + "/SplatTextures" + idx + ".png");
        }
    }

    public void exportSplatTexture()
    {
        string fileAllPath = "";

        int idx = 0;
        SplatPrototype splatLayer = null;
        string resPath = "";
        string fileName = "";
        int slashIdx = 0;
        string srcPath = "";
        for (idx = 0; idx < terrainData.splatPrototypes.Length; ++idx)
        {
            splatLayer = terrainData.splatPrototypes[idx];
            resPath = AssetDatabase.GetAssetPath(splatLayer.texture);
            // 保存目录
            slashIdx = resPath.LastIndexOf("/");
            fileName = resPath.Substring(slashIdx + 1);
            fileAllPath = string.Format("{0}/Resources/Materials/Textures/Terrain/{1}", Application.dataPath, fileName);

            if(!File.Exists(fileAllPath))
            {
                slashIdx = Application.dataPath.LastIndexOf("/");
                srcPath = Application.dataPath.Substring(0, slashIdx);
                srcPath = string.Format("{0}/{1}",srcPath , resPath);
                File.Copy(srcPath, fileAllPath);
            }
        }
    }

    public void exportHeightMap()
    {
        string fileName = string.Format("{0}/Resources/Materials/Textures/Terrain/{1}_{2}.png", Application.dataPath, mHeightMapNamePrefix, mTerrainId);
        int w = terrainData.heightmapWidth;
        int h = terrainData.heightmapHeight;
        Vector3 meshScale = terrainData.size;
        float[,] tData = terrainData.GetHeights(0, 0, w, h);

        //float fx = 0;
        //float fy = 0;

        Color color = new Color(0, 0, 0, 0);
        float height = 0;
        Texture2D heightMap = new Texture2D(w, h, TextureFormat.BGRA32, true);
        for(int idy = 0; idy < h; ++idy)
        {
            for(int idx = 0; idx < w; ++idx)
            {
                height = tData[idy, idx];
                // 这个是插值的高度
                //fx = ((float)idx) / w;
                //fy = ((float)idy) / h;
                //height = terrainData.GetInterpolatedHeight(fx, fy);
                //height /= meshScale.z;
                color = new Color(height, height, height, height);
                heightMap.SetPixel(idx, idy, color);
            }
        }

        UtilPath.saveTex2File(heightMap, fileName);
    }

    public void exportScaleHeightMap()
    {
        string fileName = string.Format("{0}/{1}.png", Application.dataPath, "heightmap");

        int w = terrainData.heightmapWidth;
        int h = terrainData.heightmapHeight;
        Vector3 meshScale = terrainData.size;
        float tRes = Mathf.Pow(2, (int)saveResolution);
        meshScale = new Vector3(meshScale.x / (w - 1) * tRes, meshScale.y, meshScale.z / (h - 1) * tRes);
        float[,] tData = terrainData.GetHeights(0, 0, w, h);

        w = (int)((w - 1) / tRes + 1);
        h = (int)((h - 1) / tRes + 1);
        Vector3[] tVertices = new Vector3[w * h];
        float height = 0;
        Texture2D heightMap = new Texture2D(w, h, TextureFormat.BGRA32, true);
        Color color = new Color(0, 0, 0, 0);

        for (int idy = 0; idy < h; idy++)
        {
            for (int idx = 0; idx < w; idx++)
            {
                //tVertices[y * w + x] = Vector3.Scale(meshScale, new Vector3(x, (int)(tData[(int)(x * tRes), (int)(y * tRes)]), y)) + terrainPos;
                height = tData[(int)(idy * tRes), (int)(idx * tRes)];
                color = new Color(height, height, height, height);
                heightMap.SetPixel(idx, idy, color);
            }
        }

        UtilPath.saveTex2File(heightMap, fileName);
    }

    // 导出高度数据
    public void exportHeightData()
    {
        string fileName = string.Format("{0}/Resources/TerrainData/{1}_{2}.bytes", Application.dataPath, mHeightMapNamePrefix, mTerrainId);

        int w = terrainData.heightmapWidth;
        int h = terrainData.heightmapHeight;
        Vector3 meshScale = terrainData.size;
        float tRes = Mathf.Pow(2, (int)saveResolution);
        meshScale = new Vector3(meshScale.x / (w - 1) * tRes, meshScale.y, meshScale.z / (h - 1) * tRes);
        Vector2 uvScale = new Vector2(1.0f / (w - 1), 1.0f / (h - 1));
        float[,] tData = terrainData.GetHeights(0, 0, w, h);

        w = (int)((w - 1) / tRes + 1);
        h = (int)((h - 1) / tRes + 1);
        Vector3[] tVertices = new Vector3[w * h];

        float height = 0;
        ByteBuffer buffer = new ByteBuffer();
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                height = tData[(int)(y * tRes), (int)(x * tRes)];
                buffer.writeFloat(height);
            }
        }

        UtilPath.saveByte2File(fileName, buffer.dynBuffer.buffer);
    }

    // 导出 AlphaMap
    public void exportAlphaMap_a()
    {
        string fileName = "";
        Texture2D[] alphamapTextures = terrainData.alphamapTextures;
        for(int idx = 0; idx < alphamapTextures.Length; ++idx)
        {
            fileName = string.Format("{0}/{1}_{2}.png", Application.dataPath, "AlphaMap", idx);
            UtilPath.saveTex2File(alphamapTextures[idx], fileName);
        }
    }

    public void exportAlphaMap()
    {
        float[,,] splatmapData = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        int alphamapWidth = terrainData.alphamapWidth;
        int alphamapHeight = terrainData.alphamapHeight;
        int alphamapLayers = terrainData.alphamapLayers;

        int channel = 4;
        int channelIdx = 0;

        float r = 0;
        float g = 0;
        float b = 0;
        float a = 0;

        string fileName = "";
        Color color = new Color(0, 0, 0, 0);
        Texture2D alphaMap = new Texture2D(alphamapWidth, alphamapHeight, TextureFormat.BGRA32, true);

        int imageIdx = 0;

        while (channelIdx < alphamapLayers)
        {
            r = 0;
            g = 0;
            b = 0;
            a = 0;

            for (int idy = 0; idy < alphamapHeight; ++idy)
            {
                for(int idx = 0; idx < alphamapWidth; ++idx)
                {
                    r = splatmapData[idx, alphamapHeight - 1 - idy, channelIdx];
                    if(channelIdx + 1 < alphamapLayers)
                    {
                        g = splatmapData[idx, alphamapHeight - 1 - idy, channelIdx + 1];
                    }
                    if (channelIdx + 2 < alphamapLayers)
                    {
                        b = splatmapData[idx, alphamapHeight - 1 - idy, channelIdx + 2];
                    }
                    if (channelIdx + 3 < alphamapLayers)
                    {
                        a = splatmapData[idx, alphamapHeight - 1 - idy, channelIdx + 3];
                    }

                    color = new Color(r, g, b, a);
                    alphaMap.SetPixel(idx, idy, color);
                }
            }

            fileName = string.Format("{0}/{1}_{2}.png", Application.dataPath, "AlphaMap", imageIdx);
            UtilPath.saveTex2File(alphaMap, fileName);

            imageIdx += 1;
            channelIdx += channel;
        }
    }

    // 导出地形 Xml 配置文件
    public void exportXml()
    {
        string path = string.Format("{0}/{1}/{2}.xml", Application.dataPath, "Resources/TerrainData", mTerrainId);
        string fileName = "";
        SplatPrototype splatLayer = null;
        string resPath = "";
        string xmlStr = "";
        string tmp = "";
        // 头信息
        xmlStr += "<?xml version='1.0' encoding='utf-8' ?>\r\n";
        // 开始标签
        xmlStr += "<Config>\r\n";

        // 输出 Diffuse 贴图名字
        if(terrainData.splatPrototypes.Length > 0)
        {
            splatLayer = terrainData.splatPrototypes[0];
            resPath = AssetDatabase.GetAssetPath(splatLayer.texture);
            // 保存目录
            fileName = UtilPath.getFileNameWithExt(resPath);
            tmp = string.Format("\t<SplatMapName name=\"Materials/Textures/Terrain/{0}\" />\r\n", fileName);
            xmlStr += tmp;
        }

        // 输出高度数据
        tmp = string.Format("\t<HeightMapName name=\"Materials/Textures/Terrain/{0}_{1}.png\" />\r\n", mHeightMapNamePrefix, mTerrainId);
        xmlStr += tmp;

        // 输出高度数据
        tmp = string.Format("\t<HeightDataName name=\"TerrainData/{0}_{1}.bytes\" />\r\n", mHeightMapNamePrefix, mTerrainId);
        xmlStr += tmp;

        // 输出地形大小信息
        tmp = string.Format("\t<WorldSize Width=\"{0}\" Depth=\"{1}\" Height=\"{2}\" />\r\n", terrainData.size.x, terrainData.size.z, terrainData.size.y);
        xmlStr += tmp;

        // 结束标签
        xmlStr += "</Config>";

        UtilPath.saveStr2File(xmlStr, path, Encoding.UTF8);
    }

    // 导出 Splat 地形 Xml 配置文件
    public void exportSplatXml()
    {
        string path = string.Format("{0}/{1}/{2}.xml", Application.dataPath, "Resources/TerrainData", mTerrainId);

        int idx = 0;
        string fileName = "";
        SplatPrototype splatLayer = null;
        Vector2 tileSize;
        string resPath = "";
        string xmlStr = "";
        string tmp = "";

        // 头信息
        xmlStr += "<?xml version='1.0' encoding='utf-8' ?>\r\n";
        // 开始标签
        xmlStr += "<Config>\r\n";

        // 输出 Splat 贴图名字
        for (idx = 0; idx < terrainData.splatPrototypes.Length; ++idx)
        {
            splatLayer = terrainData.splatPrototypes[idx];
            resPath = AssetDatabase.GetAssetPath(splatLayer.texture);
            // 保存目录
            fileName = UtilPath.getFileNameWithExt(resPath);
            tileSize = splatLayer.tileSize;
            tmp = string.Format("\t<SplatMapName name=\"Materials/Textures/Terrain/{0}\" worldSize=\"{1}\" />\r\n", fileName, tileSize.x / 2);
            xmlStr += tmp;
        }

        // 输出 Alpha Map
        for(idx = 0; idx < terrainData.alphamapTextures.Length; ++idx)
        {
            tmp = string.Format("\t<AlphaMapName name=\"Materials/Textures/Terrain/{0}_{1}_{2}.png\" />\r\n", "AlphaMap", mTerrainId, idx);
            xmlStr += tmp;
        }

        // 输出高度数据
        tmp = string.Format("\t<HeightDataName name=\"TerrainData/{0}_{1}.bytes\" />\r\n", mHeightMapNamePrefix, mTerrainId);
        xmlStr += tmp;

        // 输出地形大小信息
        tmp = string.Format("\t<WorldSize Width=\"{0}\" Depth=\"{1}\" Height=\"{2}\" />\r\n", terrainData.size.x, terrainData.size.z, terrainData.size.y);
        xmlStr += tmp;

        // 输出批次大小
        tmp = string.Format("\t<BatchSize Max=\"{0}\" Min=\"{1}\" />\r\n", 9, 5);
        xmlStr += tmp;

        // 输出高度图分辨率
        tmp = string.Format("\t<HeightMapResolution Size=\"{0}\" />\r\n", terrainData.heightmapResolution);
        xmlStr += tmp;

        // 结束标签
        xmlStr += "</Config>";

        UtilPath.saveStr2File(xmlStr, path, Encoding.UTF8);
    }
}