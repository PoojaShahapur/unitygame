using SDK.Lib;
using UnityEditor;

namespace EditorTool
{
    /**
     * @brief 发布产品 aux
     */
    public class PublishProductUtil
    {
        public static void createPublishProductOutputPath()
        {
            // 删除输出目录
            UtilPath.deleteDirectory(ExportUtil.getPkgOutPath());
            // 创建输出目录
            UtilPath.createDirectory(ExportUtil.getPkgOutPath());
        }

        public static void pkgResources()
        {
            ResExportSys.m_instance.m_targetPlatform = BuildTarget.StandaloneWindows;

            ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath = ExportUtil.getStreamingDataPath("");
            ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath = UtilPath.normalPath(ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath);
            UtilPath.deleteDirectory(ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath);
            UtilPath.createDirectory(ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath);

            ResExportSys.m_instance.parseResourceXml();
            ResExportSys.m_instance.packResourceList();
        }

        public static void copyRes2Dest()
        {
            ExportUtil.CopyAssetBundlesTo(ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath, ResExportSys.m_instance.m_targetPlatform);
        }

        public static void delResources()
        {
            string delPath = ExportUtil.getDataPath("");
            string bakPath = ExportUtil.getPkgWorkPath("");
            UtilPath.deleteDirectory(bakPath);
            UtilPath.createDirectory(bakPath);
            UtilPath.copyDirectory(delPath, bakPath);
            UtilPath.deleteDirectory(delPath);
        }

        public static void buildImage(BuildOptions option = BuildOptions.None)
        {
            string outputImagePath = ExportUtil.getImagePath("", ResExportSys.m_instance.m_targetPlatform);
            UtilPath.deleteDirectory(outputImagePath);
            UtilPath.createDirectory(outputImagePath);

            string[] levelsPath = new string[1];    // 打包第一个启动场景目录
            levelsPath[0] = "Assets/Scenes/Start.unity";

            string targetName = ExportUtil.GetBuildTargetName(ResExportSys.m_instance.m_targetPlatform/*EditorUserBuildSettings.activeBuildTarget*/);
            if (targetName == null)
                return;

            // Build and copy AssetBundles.
            //option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;

            PlayerParam param = new PlayerParam();
            param.m_levels = levelsPath;
            param.m_locationPath = outputImagePath + targetName;
            param.m_target = ResExportSys.m_instance.m_targetPlatform;
            param.m_options = option;

            ExportUtil.BuildPlayer(param);
        }

        public static void restoreResources()
        {
            string restorePath = ExportUtil.getDataPath("");
            string bakPath = ExportUtil.getPkgWorkPath("");
            UtilPath.createDirectory(restorePath);
            UtilPath.copyDirectory(bakPath, restorePath);
            UtilPath.deleteDirectory(bakPath);
        }
    }
}