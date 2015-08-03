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
            ResExportSys.instance();
            ResExportSys.m_instance.m_targetPlatform = BuildTarget.StandaloneWindows;

            ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath = ExportUtil.getStreamingDataPath("");
            ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath = ExportUtil.normalPath(ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath);
            ExportUtil.DeleteDirectory(ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath);
            ExportUtil.CreateDirectory(ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath);

            ResExportSys.m_instance.parseResourceXml();
            ResExportSys.m_instance.packResourceList();

            ExportUtil.CopyAssetBundlesTo(ResExportSys.m_instance.m_pResourcesCfgPackData.m_destFullPath, ResExportSys.m_instance.m_targetPlatform);
        }

        // 根据配置文件导出资源打包
        [MenuItem("Assets/SelfAssetBundles/ExportByCfg")]
        static void ExportByCfg()
        {
            ResExportSys.instance();
            ResExportSys.m_instance.parseXml();
            ResExportSys.m_instance.pack();
        }
    }
}