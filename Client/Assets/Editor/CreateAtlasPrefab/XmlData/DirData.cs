using EditorTool;
using SDK.Lib;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AtlasPrefabSys
{
    public class DirData
    {
        protected string m_fullDirPath;
        protected string m_dirPath;
        protected AtlasXmlPath m_xmlPath;

        protected List<FileData> m_filesList;

        public DirData(string path, AtlasXmlPath xmlPath)
        {
            m_dirPath = path;
            m_xmlPath = xmlPath;
            m_fullDirPath = ExportUtil.getDataPath(m_dirPath);
            m_fullDirPath = ExportUtil.normalPath(m_fullDirPath);
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

        public AtlasXmlPath xmlPath
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
                FileData file = new FileData(path, this);
                m_filesList.Add(file);
                file.buildData();
            }
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

        public SOSpriteList createScriptSprite()
        {
            SOSpriteList retSOSprite = ScriptableObject.CreateInstance<SOSpriteList>();
            foreach (FileData file in m_filesList)
            {
                file.addSprite2SO(retSOSprite);
            }

            return retSOSprite;
        }

        public SOAnimatorList createScriptAnimator()
        {
            SOAnimatorList retSOAnimator = ScriptableObject.CreateInstance<SOAnimatorList>();
            foreach (FileData file in m_filesList)
            {
                file.addAnimator2SO(retSOAnimator);
            }

            return retSOAnimator;
        }
    }
}