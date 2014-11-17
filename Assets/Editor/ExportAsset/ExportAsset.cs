using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
            resCfgData.pack();
        }

        [MenuItem("Assets/ExportSkinsCfg")]
        static void ExportSkinsCfg()
        {
            ResCfgData resCfgData = new ResCfgData();
            resCfgData.parseSkinsXml();
            resCfgData.exportSkinsFile();
        }

        [MenuItem("Assets/ExportSkelSubmeshCfg")]
        static void ExportSkelSubMeshCfg()
        {
            string resPath = ExportUtil.getRelDataPath("Locomotion Setup/Locomotion/Animations/DefaultAvatar.fbx");
            GameObject go = AssetDatabase.LoadAssetAtPath(resPath, typeof(GameObject)) as GameObject;
            if(go != null)
            {

            }
        }
    }
}