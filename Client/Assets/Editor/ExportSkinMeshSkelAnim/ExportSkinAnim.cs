using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class ExportSkinAnim
    {
        // ������Ƥ
        [MenuItem("Assets/SelfAssetBundles/ExportSkinsCfg")]
        static void ExportSkinsCfg()
        {
            SkinAnimSys.instance();
            SkinAnimSys.m_instance.parseSkinsXml();
            //SkinAnimSys.m_instance.exportBoneList();
            SkinAnimSys.m_instance.exportSkinsFile();
        }

        // ����������
        [MenuItem("Assets/SelfAssetBundles/ExportSubmeshCfg")]
        static void ExportSubMeshCfg()
        {
            SkinAnimSys.instance();
            SkinAnimSys.m_instance.parseSkelSubMeshPackXml();
            SkinAnimSys.m_instance.exportSubMeshPackFile();
        }

        // ��������
        [MenuItem("Assets/SelfAssetBundles/ExportSkeletonCfg")]
        static void ExportSkeletonCfg()
        {
            SkinAnimSys.instance();
            SkinAnimSys.m_instance.parseSkeletonCfgXml();
            SkinAnimSys.m_instance.exportSkeletonFile();
        }

        // ������������������
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