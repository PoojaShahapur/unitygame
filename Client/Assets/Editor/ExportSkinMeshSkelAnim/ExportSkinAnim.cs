using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class ExportSkinAnim
    {
        // ����������������Ƥ
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