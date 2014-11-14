using UnityEditor;

namespace EditorTool
{
    class ExportUtil
    {
        static public void BuildAssetBundle(AssetBundleParam param)
        {
            BuildPipeline.BuildAssetBundle(param.m_mainAsset, param.m_assets, param.m_pathName, param.m_assetBundleOptions, param.m_targetPlatform);
        }

        static public void BuildStreamedSceneAssetBundle(StreamedSceneAssetBundleParam param)
        {
            BuildPipeline.BuildStreamedSceneAssetBundle(param.m_levels, param.m_locationPath, param.m_target, param.m_options);
        }
    }
}