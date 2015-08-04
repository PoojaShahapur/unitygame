using UnityEditor;
using UnityEngine;

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

        // 导出子网格
        [MenuItem("Assets/SelfAssetBundles/ExportSubmeshCfg")]
        static void ExportSubMeshCfg()
        {
            SkinAnimSys.instance();
            SkinAnimSys.m_instance.parseSkelSubMeshPackXml();
            SkinAnimSys.m_instance.exportSubMeshPackFile();
        }

        // 导出骨骼
        [MenuItem("Assets/SelfAssetBundles/ExportSkeletonCfg")]
        static void ExportSkeletonCfg()
        {
            SkinAnimSys.instance();
            SkinAnimSys.m_instance.parseSkelSubMeshPackXml();
            SkinAnimSys.m_instance.exportSkeletonFile();
        }

        static public void testLoadAsset()
        {
            string resPath = ExportUtil.getRelDataPath("Locomotion Setup/Locomotion/Animations/DefaultAvatar.fbx");
            GameObject go = AssetDatabase.LoadAssetAtPath(resPath, typeof(GameObject)) as GameObject;
            if(go != null)
            {
            
            }
        }
    }
}