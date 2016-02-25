using SDK.Lib;
using System.IO;
using UnityEngine;

namespace UnitTest
{
    /**
     * @brief 测试内存 AssetBundles
     */
    public class TestAssetBundles
    {
        public void run()
        {
            //testMemPrefabAssetBundles();
            //testMemSceneAssetBundles();
        }

        // 测试从 AssetBundles 中加载 Prefab
        protected void testMemPrefabAssetBundles()
        {
            // 加载资源到内存
            byte[] memBytes = Ctx.m_instance.m_localFileSys.LoadFileByte(Path.Combine(getStreamingDataPath(""), "TestExportPrefab.unity3d"));
            // 从内存创建资源
            AssetBundle bundles = AssetBundle.LoadFromMemory(memBytes);
            string[] nameList = bundles.GetAllAssetNames();
            Ctx.m_instance.m_logSys.log("TestExportPrefab");

            GameObject go = bundles.LoadAsset<GameObject>("Assets/Resources/Module/Login.prefab");
            UtilApi.Instantiate(go);
            Ctx.m_instance.m_logSys.log("TestPrefabUnity3d");
        }

        // 测试从 AssetBundles 中加载 Scene
        protected void testMemSceneAssetBundles()
        {
            // 加载资源到内存
            byte[] memBytes = Ctx.m_instance.m_localFileSys.LoadFileByte(Path.Combine(getStreamingDataPath(""), "testexportscene.unity3d"));
            // 从内存创建资源
            AssetBundle bundles = AssetBundle.LoadFromMemory(memBytes);
            string[] nameList = bundles.GetAllAssetNames();
            Ctx.m_instance.m_logSys.log("testexportscene");

            GameObject go = bundles.LoadAsset<GameObject>("Assets/Scenes/Start.unity");
            Ctx.m_instance.m_logSys.log("TestPrefabUnity3d");

            Application.LoadLevel("dz");
        }

        protected string getStreamingDataPath(string path)
        {
            string outputPath = System.Environment.CurrentDirectory;
            outputPath = Path.Combine(outputPath, "AssetBundles");
            outputPath = Path.Combine(outputPath, "Windows");

            if (string.IsNullOrEmpty(path))
            {
                outputPath = Path.Combine(outputPath, path);
            }
            return outputPath;
        }
    }
}