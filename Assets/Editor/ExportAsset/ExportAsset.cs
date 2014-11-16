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

        [MenuItem("Assets/ExportSkelMeshCfg")]
        static void ExportSkelMeshCfg()
        {
            ResCfgData resCfgData = new ResCfgData();
            resCfgData.parseSkelMeshXml();
            resCfgData.exportBoneListFile();
        }
    }
}