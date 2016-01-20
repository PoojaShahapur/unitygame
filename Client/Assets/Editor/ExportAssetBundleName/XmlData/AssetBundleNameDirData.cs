using EditorTool;
using System.Collections.Generic;

namespace EditorTool
{
    public class AssetBundleNameDirData
    {
        protected string m_fullDirPath;
        protected string m_dirPath;
        protected AssetBundleNameXmlPath m_xmlPath;

        protected List<AssetBundleNameFileData> m_filesList;

        public AssetBundleNameDirData(string path, AssetBundleNameXmlPath xmlPath)
        {
            m_dirPath = path;
            m_xmlPath = xmlPath;
            m_fullDirPath = ExportUtil.getDataPath(m_dirPath);
            m_fullDirPath = ExportUtil.normalPath(m_fullDirPath);
            m_filesList = new List<AssetBundleNameFileData>();
        }

        public string fullDirPath
        {
            get
            {
                return m_fullDirPath;
            }
        }

        public string dirPath
        {
            get
            {
                return m_dirPath;
            }
        }

        public AssetBundleNameXmlPath xmlPath
        {
            get
            {
                return m_xmlPath;
            }
        }

        public void findAllFiles()
        {
            ExportUtil.recursiveTraversalDir(m_fullDirPath, onFindFile, onFindDir);
        }

        protected void onFindFile(string path)
        {
            path = ExportUtil.normalPath(path);
            string extName = ExportUtil.getFileExt(path);
            if (m_xmlPath.ignoreExtList.IndexOf(extName) == -1)         // 如果没有在或略的扩展名列表中
            {
                AssetBundleNameFileData file = new AssetBundleNameFileData(path, this);
                m_filesList.Add(file);
                file.buildData();
            }
        }

        protected void onFindDir(string path)
        {

        }
    }
}