using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class AssetBundleParam
    {
        public UnityEngine.Object m_mainAsset = null;
        public UnityEngine.Object[] m_assets;
        public string m_pathName;
        //public BuildAssetBundleOptions m_assetBundleOptions = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.UncompressedAssetBundle;
        public BuildAssetBundleOptions m_assetBundleOptions = BuildAssetBundleOptions.CompleteAssets;
        public BuildTarget m_targetPlatform = EditorUserBuildSettings.activeBuildTarget;
    }

    class StreamedSceneAssetBundleParam
    {
        public string[] m_levels;
        public string m_locationPath;
        public BuildTarget m_target = EditorUserBuildSettings.activeBuildTarget;
        public BuildOptions m_options = BuildOptions.None;
    }
}