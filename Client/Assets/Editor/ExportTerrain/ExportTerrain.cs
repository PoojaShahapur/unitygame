using SDK.Lib;
using System;
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

    int tCount;
    int counter;
    int totalCount;

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
        saveFormat = (SaveFormat)EditorGUILayout.EnumPopup("Export Format", (Enum)saveFormat);
        saveResolution = (SaveResolution)EditorGUILayout.EnumPopup("Resolution", (Enum)saveResolution);

        if (GUILayout.Button("ExportTexture"))
        {
            exportSplatTexture();
        }
        if (GUILayout.Button("ExportHeightMap"))
        {
            exportHeightMap();
            //exportScaleHeightMap();
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
    }

    public void exportAlphaTexture()
    {
        string path = "";
        int idx = 0;
        while (idx < terrainData.alphamapTextures.Length)
        {
            path = string.Format("{0}/{1}_{2}.png", Application.dataPath, "alphamapTextures", idx);
            UtilApi.saveTex2File(terrainData.alphamapTextures[0], path);
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
            UtilApi.saveTex2File(splatLayer.texture, path + "/SplatTextures" + idx + ".png");
        }
    }

    public void exportSplatTexture()
    {
        string path = Application.streamingAssetsPath;
        int idx = 0;
        SplatPrototype splatLayer = null;
        string resPath = "";
        for (idx = 0; idx < terrainData.splatPrototypes.Length; ++idx)
        {
            splatLayer = terrainData.splatPrototypes[idx];
            resPath = AssetDatabase.GetAssetPath(splatLayer.texture);
            // 保存目录
        }
    }

    public void exportHeightMap()
    {
        string fileName = string.Format("{0}/{1}.png", Application.dataPath, "heightmap");
        int w = terrainData.heightmapWidth;
        int h = terrainData.heightmapHeight;
        Vector3 meshScale = terrainData.size;
        float[,] tData = terrainData.GetHeights(0, 0, w, h);

        Color color = new Color(0, 0, 0, 0);
        float height = 0;
        Texture2D heightMap = new Texture2D(w, h, TextureFormat.BGRA32, true);
        for(int idy = 0; idy < h; ++idy)
        {
            for(int idx = 0; idx < w; ++idx)
            {
                height = tData[idx, idy];
                color = new Color(height, height, height, height);
                heightMap.SetPixel(idx, idy, color);
            }
        }

        UtilApi.saveTex2File(heightMap, fileName);
    }

    public void exportScaleHeightMap()
    {
        string fileName = string.Format("{0}/{1}.png", Application.dataPath, "heightmap");
        int w = terrainData.heightmapWidth;
        int h = terrainData.heightmapHeight;
        Vector3 meshScale = terrainData.size;
        var tRes = Mathf.Pow(2, (int)(saveResolution));
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
                height = tData[(int)(idx * tRes), (int)(idy * tRes)];
                color = new Color(height, height, height, height);
                heightMap.SetPixel(idx, idy, color);
            }
        }

        UtilApi.saveTex2File(heightMap, fileName);
    }

    // 导出 AlphaMap
    public void exportAlphaMap_a()
    {
        string fileName = "";
        Texture2D[] alphamapTextures = terrainData.alphamapTextures;
        for(int idx = 0; idx < alphamapTextures.Length; ++idx)
        {
            fileName = string.Format("{0}/{1}_{2}.png", Application.dataPath, "AlphaMap", idx);
            UtilApi.saveTex2File(alphamapTextures[idx], fileName);
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
            UtilApi.saveTex2File(alphaMap, fileName);

            imageIdx += 1;
            channelIdx += channel;
        }
    }

    // 导出地形 Xml 配置文件
    public void exportXml()
    {
        string path = Application.streamingAssetsPath + "/1000.xml";
        int idx = 0;
        string fileName = "";
        SplatPrototype splatLayer = null;
        string resPath = "";
        string xmlStr = "";
        string tmp = "";
        xmlStr += "<?xml version='1.0' encoding='utf-8' ?>\r\n";
        xmlStr += "<Config>\r\n";
        // 输出 Splat 贴图名字
        for (idx = 0; idx < terrainData.splatPrototypes.Length; ++idx)
        {
            splatLayer = terrainData.splatPrototypes[idx];
            resPath = AssetDatabase.GetAssetPath(splatLayer.texture);
            // 保存目录
            fileName = UtilApi.getFileNameNoPath(resPath);
            tmp = string.Format("\t<SplatName name=\"{0}\" />\r\n", fileName);
            xmlStr += tmp;
        }
        // 输出 Alpha Map
        for(idx = 0; idx < terrainData.alphamapTextures.Length; ++idx)
        {
            tmp = string.Format("\t<AlphaName name=\"{0}_{1}.png\" />\r\n", "AlphaMap", idx);
            xmlStr += tmp;
        }
        xmlStr += "</Config>";
        UtilApi.saveStr2File(xmlStr, path, Encoding.UTF8);
    }
}