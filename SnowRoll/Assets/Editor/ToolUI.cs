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
            MFileSys.init();
            ToolCtx.instance();
            ToolCtx.mInstance.exportAssetBundleName();
            AssetDatabase.Refresh();
        }

        // 导出 AssetBundles
        [MenuItem("MyNew/ExportWindowDebugPackage")]
        public static void BuildWindowDebug()
        {
            MFileSys.init();
            ToolCtx.instance();
            ToolCtx.mInstance.exportAssetBundleName();
            BuildScript.BuildPlayer(BuildTarget.StandaloneWindows, false);
            ToolCtx.instance().dispose();
        }

        [MenuItem("MyNew/ExportWindowAssetBundlesList")]
        public static void BuildWindowAssetBundlesList()
        {
            Ctx.instance();
            Ctx.mInstance.editorToolInit();

            ExportAssetRelation exportAssetRelation = new ExportAssetRelation();
			exportAssetRelation.setBuildTarget(BuildTarget.StandaloneWindows64);
			exportAssetRelation.setCurPath(UtilEditor.getAssetBundlesOutpath(BuildTarget.StandaloneWindows64));
            exportAssetRelation.setOutFileName(UtilEditor.getBuildOutPath() + "/AssetBundlesList.txt");
            exportAssetRelation.buildOutFile();
        }

        [MenuItem("MyNew/ExportMacAssetBundlesList")]
        public static void BuildMacAssetBundlesList()
        {
            Ctx.instance();
            Ctx.mInstance.editorToolInit();

            ExportAssetRelation exportAssetRelation = new ExportAssetRelation();
            exportAssetRelation.setBuildTarget(BuildTarget.StandaloneOSXUniversal);
            exportAssetRelation.setCurPath(UtilEditor.getAssetBundlesOutpath(BuildTarget.StandaloneOSXUniversal));
            exportAssetRelation.setOutFileName(UtilEditor.getBuildOutPath() + "/AssetBundlesList.txt");
            exportAssetRelation.buildOutFile();
        }

        [MenuItem("MyNew/ExportIOSAssetBundlesList")]
		public static void BuildIOSAssetBundlesList()
		{
			Ctx.instance();
			Ctx.mInstance.editorToolInit();

			ExportAssetRelation exportAssetRelation = new ExportAssetRelation();
			exportAssetRelation.setBuildTarget(BuildTarget.iOS);
			exportAssetRelation.setCurPath(UtilEditor.getAssetBundlesOutpath(BuildTarget.iOS));
			exportAssetRelation.setOutFileName(UtilEditor.getBuildOutPath() + "/AssetBundlesList.txt");
			exportAssetRelation.buildOutFile();
		}

        // 导出 AssetBundles
        [MenuItem("MyNew/ExportAndroidDebugPackage")]
        public static void BuildAndroidDebug()
        {
            MFileSys.init();
            BuildScript.BuildPlayer(BuildTarget.Android, false);
        }

        [MenuItem("MyNew/ExportMacDebugPackage")]
        public static void BuildMacDebug()
        {
            MFileSys.init();
            BuildScript.BuildPlayer(BuildTarget.StandaloneOSXUniversal, false);
        }

		[MenuItem("MyNew/ExportIOSDebugPackage")]
		public static void BuildIOSDebug()
		{
			MFileSys.init();
			BuildScript.BuildPlayer(BuildTarget.iOS, false);
		}

        // 测试命令行
        [MenuItem("MyNew/TestCmdSys")]
        public static void TestCmdSys()
        {
            CmdSys.cmdMain();
        }

        // 导出精灵图集
        [MenuItem("MyNew/ExportSprite")]
        public static void ExportSprite()
        {
            ToolCtx.instance().spriteSheetImport();
        }
    }
}