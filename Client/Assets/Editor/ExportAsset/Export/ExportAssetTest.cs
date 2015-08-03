using UnityEditor;

namespace EditorTool
{
    class ExportAssetTest
    {
        [MenuItem("Assets/SelfAssetBundlesTest/Build AssetLabels AssetBundles")]
        static public void BuildAssetLabelsAssetBundles()
        {
            BuildScriptTest.BuildAssetLabelsAssetBundles();
        }

        [MenuItem("Assets/SelfAssetBundlesTest/Build Custom AssetBundles")]
        static public void BuildCustomAssetBundles()
        {
            BuildScriptTest.BuildCustomAssetBundles();
        }

        [MenuItem("Assets/SelfAssetBundlesTest/Build Select AssetBundles")]
        static public void BuildSelectAssetBundles()
        {
            BuildScriptTest.BuildSelectAssetBundles();
        }

        [MenuItem("Assets/SelfAssetBundlesTest/Build Streamed Scene")]
        static public void BuildStreamedSceneAssetBundles()
        {
            BuildScriptTest.BuildStreamedSceneAssetBundles();
        }

        // 生成可执行 image 
        [MenuItem("Assets/SelfAssetBundlesTest/Build Player")]
        static void BuildPlayer()
        {
            BuildScriptTest.BuildPlayer();
        }
    }
}
