using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class AssetBundleParam
    {
        public UnityEngine.Object m_mainAsset;
        public UnityEngine.Object[] m_assets;
        public string m_pathName;
        public BuildAssetBundleOptions m_assetBundleOptions;
        public BuildTarget m_targetPlatform;
    }

    class StreamedSceneAssetBundleParam
    {
        public string[] m_levels;
        public string m_locationPath;
        public BuildTarget m_target;
        public BuildOptions m_options;
    }
}