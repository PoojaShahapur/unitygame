using UnityEditor;

namespace EditorTool
{
    public class ExportAssetBundleName
    {
        [MenuItem("Assets/AssetBundleName/SetAssetBundleName")]
        static public void SetAssetBundleName()
        {
            setAssetBundleName();
        }

        public static void setAssetBundleName()
        {
            ExportAssetBundleNameSys.instance();
            ExportAssetBundleNameSys.m_instance.clear();
            ExportAssetBundleNameSys.m_instance.parseXml();
            ExportAssetBundleNameSys.m_instance.setAssetBundleName();
            ExportAssetBundleNameSys.m_instance.exportResABKV();
            AssetDatabase.Refresh();
        }
    }
}