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
            BuildScript.BuildPlayer(BuildTarget.StandaloneWindows, false);
        }
    }
}