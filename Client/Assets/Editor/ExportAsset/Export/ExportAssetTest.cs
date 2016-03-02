using UnityEditor;

namespace EditorTool
{
    class ExportAssetTest
    {
        [MenuItem("My/Assets/SelfAssetBundlesTest/Build AssetLabels AssetBundles")]
        static public void BuildAssetLabelsAssetBundles()
        {
            BuildScriptTest.BuildAssetLabelsAssetBundles();
        }

        [MenuItem("My/Assets/SelfAssetBundlesTest/Build Custom AssetBundles")]
        static public void BuildCustomAssetBundles()
        {
            BuildScriptTest.BuildCustomAssetBundles();
        }

        [MenuItem("My/Assets/SelfAssetBundlesTest/Build Select AssetBundles")]
        static public void BuildSelectAssetBundles()
        {
            BuildScriptTest.BuildSelectAssetBundles();
        }

        [MenuItem("My/Assets/SelfAssetBundlesTest/Build Streamed Scene")]
        static public void BuildStreamedSceneAssetBundles()
        {
            BuildScriptTest.BuildStreamedSceneAssetBundles();
        }

        // 生成可执行 image 
        [MenuItem("My/Assets/SelfAssetBundlesTest/Build Player")]
        static void BuildPlayer()
        {
            BuildScriptTest.BuildPlayer();
        }
    }
}
