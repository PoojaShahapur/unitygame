using UnityEditor;

namespace EditorTool
{
    class ExportSkinAnim
    {
        // 导出骨骼动画的蒙皮
        [MenuItem("Assets/SelfAssetBundles/ExportSkinsCfg")]
        static void ExportSkinsCfg()
        {
            SkinAnimSys.instance();
            SkinAnimSys.m_instance.parseSkinsXml();
            //SkinAnimSys.m_instance.exportBoneList();
            SkinAnimSys.m_instance.exportSkinsFile();
        }

        // 导出骨骼和子网格
        [MenuItem("Assets/SelfAssetBundles/ExportSkelSubmeshCfg")]
        static void ExportSkelSubMeshCfg()
        {
            //string resPath = ExportUtil.getRelDataPath("Locomotion Setup/Locomotion/Animations/DefaultAvatar.fbx");
            //GameObject go = AssetDatabase.LoadAssetAtPath(resPath, typeof(GameObject)) as GameObject;
            //if(go != null)
            //{
            //
            //}

            SkinAnimSys.instance();
            SkinAnimSys.m_instance.parseSkelSubMeshPackXml();
            SkinAnimSys.m_instance.skelSubMeshPackFile();
        }
    }
}