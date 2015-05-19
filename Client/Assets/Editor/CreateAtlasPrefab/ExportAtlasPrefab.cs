using UnityEditor;

namespace AtlasPrefabSys
{
    public class AtlasPrefabExport
    {
        [MenuItem("Assets/AtlasPrefab/ExportAtlasPrefab")]
        static public void ExportAtlasPrefab()
        {
            CreateAtlasPrefabSys.instance();
            //AtlasPrefabUtil.createPrefab("aaaa");
            CreateAtlasPrefabSys.m_instance.parseXml();
            CreateAtlasPrefabSys.m_instance.exportPrefab();
        }
    }
}