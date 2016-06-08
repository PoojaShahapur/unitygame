using SDK.Lib;
using UnityEditor;

namespace EditorTool
{
    /**
     * @ 导出 UI 区域
     */
    public class ToolUI
    {
        // 设置 AssetBundles 的名字
        [MenuItem("MyNew/SetAssetBundleName")]
        static public void SetAssetBundleName()
        {
            ToolCtx.instance();
            ToolCtx.m_instance.exportAssetBundleName();
            AssetDatabase.Refresh();
        }

        // 导出 AssetBundles
        [MenuItem("MyNew/ExportWindowDebugPackage")]
        public static void BuildWindowDebug()
        {
            ToolCtx.instance();
            ToolCtx.m_instance.exportAssetBundleName();
            BuildScript.BuildPlayer(BuildTarget.StandaloneWindows, false);
            ToolCtx.instance().dispose();
        }

        [MenuItem("MyNew/ExportWindowAssetBundlesList")]
        public static void BuildWindowAssetBundlesList()
        {
            ExportAssetRelation exportAssetRelation = new ExportAssetRelation();
            exportAssetRelation.setBuildTarget(BuildTarget.StandaloneWindows);
            exportAssetRelation.setCurPath(UtilEditor.getAssetBundlesOutpath(BuildTarget.StandaloneWindows));
            exportAssetRelation.setOutFileName(UtilEditor.getBuildOutPath() + "/AssetBundlesList.txt");
            exportAssetRelation.buildOutFile();
        }

        // 导出 AssetBundles
        [MenuItem("MyNew/ExportAndroidDebugPackage")]
        public static void BuildAndroidDebug()
        {
            BuildScript.BuildPlayer(BuildTarget.Android, false);
        }

        // 测试命令行
        [MenuItem("MyNew/TestCmdSys")]
        public static void TestCmdSys()
        {
            CmdSys.cmdMain();
        }
    }
}