using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    // 打包除 Scene 外的参数
    class AssetBundleParam
    {
        public string m_pathName;
#if UNITY_5
        public AssetBundleBuild[] m_buildList;
        public BuildAssetBundleOptions m_assetBundleOptions = BuildAssetBundleOptions.UncompressedAssetBundle;  // 打包都是打包成非压缩的，自己在外面压缩一边
#elif UNITY_4_6
        public UnityEngine.Object m_mainAsset = null;
        public UnityEngine.Object[] m_assets;
        public BuildAssetBundleOptions m_assetBundleOptions = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.UncompressedAssetBundle;
#endif
        //public BuildAssetBundleOptions m_assetBundleOptions = BuildAssetBundleOptions.CompleteAssets;
        public BuildTarget m_targetPlatform = EditorUserBuildSettings.activeBuildTarget;
    }

    // 打包 Scene 参数
    class StreamedSceneAssetBundleParam
    {
        public string[] m_levels;
        public string m_locationPath;
        public BuildTarget m_target = EditorUserBuildSettings.activeBuildTarget;
        public BuildOptions m_options = BuildOptions.UncompressedAssetBundle;
    }

    // 打包可执行镜像参数
    class PlayerParam
    {
        public string[] m_levels;
        public string m_locationPath;
        public BuildTarget m_target = EditorUserBuildSettings.activeBuildTarget;
        public BuildOptions m_options = BuildOptions.UncompressedAssetBundle;
    }
}