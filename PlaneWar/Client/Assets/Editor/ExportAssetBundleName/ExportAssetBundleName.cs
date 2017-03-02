using UnityEditor;

namespace EditorTool
{
    public class ExportAssetBundleName
    {
        [MenuItem("My/Assets/AssetBundleName/SetAssetBundleName")]
        static public void SetAssetBundleName()
        {
            setAssetBundleName();
        }

        public static void setAssetBundleName()
        {
            ToolCtx.getSingletonPtr().exportAssetBundleName();
            AssetDatabase.Refresh();
        }
    }
}