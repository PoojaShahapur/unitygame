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
    public class ResourcesCfgPackData
    {
        public List<ResourcesPathItem> m_resourceList = new List<ResourcesPathItem>();
        public string m_destFullPath;
        public string m_resListOutpath;
    }

    /**
     * @brief 一项 resourcespath 
     */
    public class ResourcesPathItem
    {
        public string m_srcRoot;
        public string m_destRoot;
        public List<string> m_unity3dExtNameList;
        public List<string> m_ignoreExtList;
        public string m_srcFullPath;

        public void parseXml(XmlElement elem)
        {
            m_srcRoot = ExportUtil.getXmlAttrStr(elem.Attributes["srcroot"]);
            m_destRoot = ExportUtil.getXmlAttrStr(elem.Attributes["destroot"]);
            char[] splitChar = new char[1]{','};
            m_unity3dExtNameList = ExportUtil.getXmlAttrStr(elem.Attributes["unity3dextname"]).Split(splitChar).ToList<string>();
            m_ignoreExtList = ExportUtil.getXmlAttrStr(elem.Attributes["ignoreext"]).Split(splitChar).ToList<string>();

            int idx = 0;
            if (m_unity3dExtNameList.IndexOf("null") != -1)         // 如果扩展名字有 null ，就说明包括没有扩展名字
            {
                for (idx = 0; idx < m_unity3dExtNameList.Count; ++idx)
                {
                    if(m_unity3dExtNameList[idx] == "null")
                    {
                        m_unity3dExtNameList[idx] = "";
                    }
                }
            }

            if (m_ignoreExtList.IndexOf("null") != -1)         // 如果扩展名字有 null ，就说明包括没有扩展名字
            {
                for (idx = 0; idx < m_ignoreExtList.Count; ++idx)
                {
                    if (m_ignoreExtList[idx] == "null")
                    {
                        m_ignoreExtList[idx] = "";
                    }
                }
            }

            m_srcFullPath = Path.Combine(System.Environment.CurrentDirectory, m_srcRoot);
            m_srcFullPath = ExportUtil.normalPath(m_srcFullPath);
        }

        public void packPack()
        {
            if (!string.IsNullOrEmpty(m_destRoot))
            {
                ExportUtil.CreateDirectory(Path.Combine(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath, m_destRoot));
            }
            ExportUtil.recrueDirs(m_srcFullPath, handleFile, handleDir);
        }

        // 遍历一个文件的时候处理
        public void handleFile(string fullFileName)
        {
            fullFileName = ExportUtil.normalPath(fullFileName);
            if (m_ignoreExtList.IndexOf(ExportUtil.getFileExt(fullFileName)) == -1)
            {
                string fineNameNoExt = ExportUtil.getFileNameNoExt(fullFileName);
                string assetPath = fullFileName.Substring(fullFileName.IndexOf(ExportUtil.ASSETS));
                string destPath = "";

                if (m_unity3dExtNameList.IndexOf(ExportUtil.getFileExt(fullFileName)) != -1)
                {
                    if (fullFileName.LastIndexOf('/') != m_srcFullPath.Length)
                    {
                        destPath = fullFileName.Substring(m_srcFullPath.Length + 1, fullFileName.LastIndexOf('/') - (m_srcFullPath.Length + 1));
                    }
                    if (!string.IsNullOrEmpty(m_destRoot))
                    {
                        destPath = Path.Combine(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath, m_destRoot);
                        destPath = Path.Combine(destPath, destPath);
                    }
                    else
                    {
                        destPath = Path.Combine(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath, destPath);
                    }

                    AssetBundleParam bundleParam = new AssetBundleParam();
#if UNITY_5
                    bundleParam.m_buildList = new AssetBundleBuild[1];
                    bundleParam.m_buildList[0].assetBundleName = fineNameNoExt;
                    bundleParam.m_buildList[0].assetBundleVariant = ExportUtil.UNITY3D;
                    bundleParam.m_buildList[0].assetNames = new string[1];
                    bundleParam.m_buildList[0].assetNames[0] = assetPath;
                    bundleParam.m_targetPlatform = ResCfgData.m_ins.m_targetPlatform;
                    bundleParam.m_pathName = destPath;
#elif UNITY_4_6
                    bundleParam.m_assets = objList.ToArray();
                    pathList.Clear();
                    pathList.Add(m_skelMeshParam.m_outPath);
                    pathList.Add(skelNoExt + ".unity3d");
                    bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
#endif
                    ExportUtil.BuildAssetBundle(bundleParam);
                    // 打包成 unity3d 后文件名字会变成小写，这里修改一下
                    ExportUtil.modifyFileName(destPath, fineNameNoExt);
                }
                else        // 直接拷贝过去
                {
                    if (fullFileName.LastIndexOf('/') != m_srcFullPath.Length)
                    {
                        destPath = fullFileName.Substring(m_srcFullPath.Length + 1, fullFileName.Length - (m_srcFullPath.Length + 1));
                    }
                    if (!string.IsNullOrEmpty(m_destRoot))
                    {
                        destPath = Path.Combine(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath, m_destRoot);
                        destPath = Path.Combine(destPath, destPath);
                    }
                    else
                    {
                        destPath = Path.Combine(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath, destPath);
                        File.Copy(fullFileName, destPath);
                    }
                }

                addResListItem(fullFileName, destPath);
            }
        }

        // 遍历一个文件夹的时候处理
        public void handleDir(string fullDirName)
        {
            if (m_srcFullPath != fullDirName)
            {
                string destPath = fullDirName.Substring(m_srcFullPath.Length + 1, fullDirName.Length - (m_srcFullPath.Length + 1));
                if (!string.IsNullOrEmpty(m_destRoot))
                {
                    destPath = Path.Combine(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath, m_destRoot);
                    destPath = Path.Combine(destPath, destPath);
                }
                else
                {
                    destPath = Path.Combine(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath, destPath);
                    ExportUtil.CreateDirectory(destPath);
                }
            }
            else
            {
                ExportUtil.CreateDirectory(ResCfgData.m_ins.m_pResourcesCfgPackData.m_destFullPath);
            }
        }

        protected void addResListItem(string srcFullPath, string destFullPath)
        {
            ResListItem item = new ResListItem();

            item.m_srcName = ExportUtil.rightSubStr(srcFullPath, m_srcRoot);
            if (m_unity3dExtNameList.IndexOf(ExportUtil.getFileExt(srcFullPath)) != -1)     // 如果需要打包成 unity3d
            {
                item.m_destName = string.Format("{0}{1}", item.m_srcName.Substring(0, item.m_srcName.IndexOf('.')), ExportUtil.DOTUNITY3D);
            }
            else
            {
                item.m_destName = item.m_srcName;
            }

            if (m_destRoot.Length > 0)
            {
                item.m_destName = string.Format("{0}/{1}", m_destRoot, item.m_destName);
            }

            // 如果是 unity 扩展名字的场景文件，需要在原始文件名字前面添加 Scenes 子目录
            if (ExportUtil.getFileExt(item.m_srcName) == "unity")
            {
                item.m_srcName = string.Format("Scenes/{0}", item.m_srcName);
            }
            ResCfgData.m_ins.m_exportResList.addItem(item);
        }
    }
}