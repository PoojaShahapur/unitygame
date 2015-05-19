using EditorTool;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AtlasPrefabSys
{
    public class DirData
    {
        protected string m_fullDirPath;
        protected string m_dirPath;
        protected List<FileData> m_filesList;

        public DirData(string path)
        {
            m_fullDirPath = ExportUtil.getDataPath(m_dirPath);
            m_dirPath = path;
            m_filesList = new List<FileData>();
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

        public void findAllFiles()
        {
            ExportUtil.recrueDirs(m_fullDirPath, onFindFile, onFindDir);
        }

        protected void onFindFile(string path)
        {
            FileData file = new FileData(path, this);
            m_filesList.Add(file);
            file.buildData();
        }

        protected void onFindDir(string path)
        {

        }

        public GameObject createImageGo()
        {
            int slashIdx = m_dirPath.LastIndexOf("/");
            string dirName = m_dirPath.Substring(slashIdx + 1);

            GameObject _go = new GameObject(dirName);
            foreach (FileData file in m_filesList)
            {
                file.addImageComponent(_go);
            }

            return _go;
        }
    }
}