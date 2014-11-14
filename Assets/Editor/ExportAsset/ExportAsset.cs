using UnityEditor;

namespace EditorTool
{
    class ExportAsset
    {
        // 根据配置文件导出资源打包
        [MenuItem("Assets/ExportByCfg")]
        static void ExportByCfg()
        {
            ResCfgData resCfgData = new ResCfgData();
            resCfgData.parseXml();

            foreach (Pack pack in resCfgData.m_packList)
            {

            }
        }
    }
}