using System.Collections.Generic;
using UnityEditor;

namespace EditorTool
{
    public class AssetBundleNameFileData
    {
        protected string m_fullPath;
        protected string m_subPathNoExt;
        protected AssetBundleNameDirData m_dirData;

        protected string[] m_pathArr;
        protected string m_resPath;     // 就是放在 Resources 中的目录
        protected string m_abPath;      // 就是打包进 AB 中的目录
        protected string m_abSetPath;   // 就是给 AssetBundle 设置的目录
        protected string m_resNameNoExt;     // 资源名字

        public AssetBundleNameFileData(string path, AssetBundleNameDirData dir)
        {
            m_fullPath = path;
            m_dirData = dir;
        }

        public void buildData()
        {
            int dotIdx = m_fullPath.IndexOf(".");
            string pathNoExt = m_fullPath.Substring(0, dotIdx);
            m_subPathNoExt = pathNoExt.Substring(m_dirData.fullDirPath.Length + 1);

            pathNoExt = pathNoExt.Substring(m_dirData.fullDirPath.Length + 1);
            char[] separator = new char[1];
            separator[0] = '/';
            m_pathArr = pathNoExt.Split(separator);

            int resIndex = m_fullPath.IndexOf(ExportUtil.RESOURCES);
            if (resIndex != -1)
            {
                m_resPath = m_fullPath.Substring(resIndex + ExportUtil.RESOURCES.Length + 1);

                dotIdx = m_resPath.IndexOf(".");
                if(dotIdx != -1)
                {
                    m_resPath = m_resPath.Substring(0, dotIdx);
                }
            }

            m_abSetPath = m_resPath + ExportUtil.DOTUNITY3D;
            m_abSetPath = ExportUtil.toLower(m_abSetPath);

            int lastSlash = -1;
            lastSlash = m_resPath.LastIndexOf(ExportUtil.SLASH);
            if (lastSlash != -1)
            {
                m_resNameNoExt = m_resPath.Substring(lastSlash + 1, m_resPath.Length - (lastSlash + 1));
            }
            else
            {
                // 说明资源直接放在 Resources 目录下，没有建立一个目录放进去
                m_resNameNoExt = "";
            }

            int assetIndex = m_fullPath.IndexOf(ExportUtil.ASSETS);
            if (resIndex != -1)
            {
                m_abPath = m_fullPath.Substring(assetIndex);
            }
        }

        public void setAssetBundleName()
        {
            AssetImporter asset = AssetImporter.GetAtPath(m_abPath);
            // 这个设置后都是小写的字符串，即使自己设置的是大写的字符串，也会被转换成小写的
            asset.assetBundleName = m_abSetPath;
            asset.SaveAndReimport();
        }

        public void exportResABKV(List<string> list)
        {
            string str = m_resPath + "=" + m_abSetPath + "=" + m_abPath;
            list.Add(str);
        }
    }
}