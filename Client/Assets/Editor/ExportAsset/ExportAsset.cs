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
            ResCfgData.Instance();
            ResCfgData.m_ins.m_targetPlatform = BuildTarget.StandaloneWindows;

            ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath = ExportUtil.getStreamingDataPath("");
            ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath = ExportUtil.normalPath(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath);
            ExportUtil.DeleteDirectory(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath);
            ExportUtil.CreateDirectory(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath);

            ResCfgData.m_ins.parseResourceXml();
            ResCfgData.m_ins.packResourceList();

            ExportUtil.CopyAssetBundlesTo(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath, ResCfgData.m_ins.m_targetPlatform);
        }

        // 根据配置文件导出资源打包
        [MenuItem("Assets/SelfAssetBundles/ExportByCfg")]
        static void ExportByCfg()
        {
            ResCfgData.Instance();
            ResCfgData.m_ins.parseXml();
            ResCfgData.m_ins.pack();
        }
    }
}