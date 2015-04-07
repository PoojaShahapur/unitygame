using SDK.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEditor;

namespace EditorTool
{
    /**
     * @brief 一项 resourcespath 
     */
    public class ResourcesPathItem
    {
        public string m_srcRoot;
        public string m_srcFullPath;
        public string m_destFullPath;

        public void parseXml(XmlElement elem)
        {
            m_srcRoot = UtilApi.getXmlAttrStr(elem.Attributes["srcroot"]);
            m_srcFullPath = Path.Combine(System.Environment.CurrentDirectory, m_srcRoot);
            m_destFullPath = ExportUtil.getStreamingDataPath("");
        }

        public void packPack()
        {
            ExportUtil.CreateDirectory(m_destFullPath);
            ExportUtil.recrueDirs(m_srcFullPath, handleFile);
        }

        // 遍历一个文件的时候处理
        public void handleFile(string fullFileName)
        {
            if (ExportUtil.getFileExt(fullFileName) != ExportUtil.METAEXT)
            {
                string fineNameNoExt = ExportUtil.getFileNameNoExt(fullFileName);
                string assetPath = fullFileName.Substring(fullFileName.IndexOf(ExportUtil.ASSETS));
                string destPath = fullFileName.Substring(m_srcFullPath.Length, fullFileName.IndexOf('.') - m_srcFullPath.Length);
                destPath = Path.Combine(m_destFullPath, destPath);

                if (ExportUtil.getFileExt(fullFileName) == ExportUtil.PREFAB)
                {
                    AssetBundleParam bundleParam = new AssetBundleParam();
#if UNITY_5
                    bundleParam.m_buildList = new AssetBundleBuild[1];
                    bundleParam.m_buildList[0].assetBundleName = fineNameNoExt;
                    bundleParam.m_buildList[0].assetBundleVariant = ExportUtil.UNITY3D;
                    bundleParam.m_buildList[0].assetNames = new string[1];
                    bundleParam.m_buildList[0].assetNames[0] = assetPath;
                    bundleParam.m_pathName = destPath;
#elif UNITY_4_6
                bundleParam.m_assets = objList.ToArray();
                pathList.Clear();
                pathList.Add(m_skelMeshParam.m_outPath);
                pathList.Add(skelNoExt + ".unity3d");
                bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
#endif
                    ExportUtil.BuildAssetBundle(bundleParam);
                }
            }
            else        // 直接拷贝过去
            {
                File.Copy(fullFileName, destPath);
            }
        }

        // 遍历一个文件夹的时候处理
        public void handleDir(string fullFileName)
        {

        }
    }
}