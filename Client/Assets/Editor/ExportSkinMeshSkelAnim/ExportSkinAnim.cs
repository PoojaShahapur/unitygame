using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class ExportSkinAnim
    {
        // 导出蒙皮
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
            SkinAnimSys.m_instance.parseSkeletonCfgXml();
            SkinAnimSys.m_instance.exportSkeletonFile();
        }

        // 导出骨骼动画控制器
        [MenuItem("Assets/SelfAssetBundles/ExportSkelAnimController")]
        static public void ExportAnimatorController()
        {
            SkinAnimSys.instance();
            SkinAnimSys.m_instance.parseSkelAnimControllerXml();
            //SkinAnimSys.m_instance.exportSkelAnimController();
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