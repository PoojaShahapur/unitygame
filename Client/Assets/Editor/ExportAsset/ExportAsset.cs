using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class ExportAsset
    {
        // 将 Resources 目录下的所有的 prefab 文件都打包成 unity3d，然后拷贝到另外一个目录下，整个目录结构不变
        [MenuItem("Assets/SelfAssetBundles/ExportByResourcesCfg")]
        static void ExportByResourcesCfg()
        {
            ResCfgData resCfgData = new ResCfgData();
            resCfgData.parseResourceXml();
            resCfgData.packResourceList();
        }

        // 根据配置文件导出资源打包
        [MenuItem("Assets/SelfAssetBundles/ExportByCfg")]
        static void ExportByCfg()
        {
            ResCfgData resCfgData = new ResCfgData();
            resCfgData.parseXml();
            resCfgData.pack();
        }

        // 导出骨骼动画的蒙皮
        [MenuItem("Assets/SelfAssetBundles/ExportSkinsCfg")]
        static void ExportSkinsCfg()
        {
            ResCfgData resCfgData = new ResCfgData();
            resCfgData.parseSkinsXml();
            resCfgData.exportSkinsFile();
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

            ResCfgData resCfgData = new ResCfgData();
            resCfgData.parseSkelSubMeshPackXml();
            resCfgData.skelSubMeshPackFile();
        }
    }
}