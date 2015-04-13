using UnityEditor;

namespace EditorTool
{
    /**
     * @brief 发布产品 aux
     */
    public class PublishProductUtil
    {
        public static void pkgResources()
        {
            ResCfgData.m_ins.m_targetPlatform = BuildTarget.StandaloneWindows;

            ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath = ExportUtil.getStreamingDataPath("");
            ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath = ExportUtil.normalPath(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath);
            ExportUtil.DeleteDirectory(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath);
            ExportUtil.CreateDirectory(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath);

            ResCfgData.m_ins.parseResourceXml();
            ResCfgData.m_ins.packResourceList();
        }

        public static void copyRes2Dest()
        {
            ExportUtil.CopyAssetBundlesTo(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath, ResCfgData.m_ins.m_targetPlatform);
        }

        public static void delResources()
        {
            string delPath = ExportUtil.getDataPath("Prefabs");
            string bakPath = ExportUtil.getWorkPath("Prefabs");
            ExportUtil.DeleteDirectory(bakPath);
            ExportUtil.CreateDirectory(bakPath);
            ExportUtil.copyDirectory(delPath, bakPath);
            ExportUtil.DeleteDirectory(delPath);
        }

        public static void buildImage()
        {
            string outputImagePath = ExportUtil.getImagePath("", ResCfgData.m_ins.m_targetPlatform);
            ExportUtil.DeleteDirectory(outputImagePath);
            ExportUtil.CreateDirectory(outputImagePath);

            string[] levelsPath = new string[1];    // 打包第一个启动场景目录
            levelsPath[0] = "Assets/Scenes/Start.unity";

            string targetName = ExportUtil.GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
            if (targetName == null)
                return;

            // Build and copy AssetBundles.
            BuildOptions option = 0;
            option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;

            PlayerParam param = new PlayerParam();
            param.m_levels = levelsPath;
            param.m_locationPath = outputImagePath + targetName;
            param.m_target = ResCfgData.m_ins.m_targetPlatform;
            param.m_options = option;

            ExportUtil.BuildPlayer(param);
        }

        public static void restoreResources()
        {
            string restorePath = ExportUtil.getDataPath("Prefabs");
            string bakPath = ExportUtil.getWorkPath("Prefabs");
            ExportUtil.CreateDirectory(restorePath);
            ExportUtil.copyDirectory(bakPath, restorePath);
            ExportUtil.DeleteDirectory(bakPath);
        }
    }
}