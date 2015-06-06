using EditorTool;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace AtlasPrefabSys
{
    public class AtlasXmlData
    {
        public List<AtlasXmlPath> m_pathList = new List<AtlasXmlPath>();

        public void clear()
        {
            m_pathList.Clear();
        }

        public void parseXml()
        {
            string path = ExportUtil.getDataPath("Config/Tool/CreateAtlasPrefab.xml");
            AtlasXmlParse parse = new AtlasXmlParse();
            parse.parseXml(path, this);
        }

        public void exportPrefab()
        {
            foreach(AtlasXmlPath xmlPath in m_pathList)
            {
                xmlPath.createDirData();
                xmlPath.createPrefab();
            }
        }

        public void exportAsset()
        {
            foreach (AtlasXmlPath xmlPath in m_pathList)
            {
                xmlPath.createDirData();
                xmlPath.createAsset();
            }
        }
    }

    public class AtlasXmlPath
    {
        protected string m_inPath;
        protected string m_outPath;
        protected List<string> m_ignoreExtList;             // 或略的扩展名列表

        protected DirData m_dirData;
        protected uint m_resType;

        public List<string> ignoreExtList
        {
            get
            {
                return m_ignoreExtList;
            }
        }

        public void parseXml(XmlElement packElem)
        {
            m_inPath = ExportUtil.getXmlAttrStr(packElem.Attributes["inpath"]);
            m_outPath = ExportUtil.getXmlAttrStr(packElem.Attributes["outpath"]);
            m_resType = ExportUtil.getXmlAttrUInt(packElem.Attributes["restype"]);

            string ignoreext = ExportUtil.getXmlAttrStr(packElem.Attributes["ignoreext"]);
            char[] separator = new char[1];
            separator[0] = ',';
            string[] strArr = ignoreext.Split(separator);
            m_ignoreExtList = new List<string>(strArr);
        }

        public void setPathParam(string inpath, string outpath, string ignoreExtStr, uint resType_)
        {
            m_inPath = inpath;
            m_outPath = outpath;
            m_resType = resType_;

            string ignoreext = ignoreExtStr;
            char[] separator = new char[1];
            separator[0] = ',';
            string[] strArr = ignoreext.Split(separator);
            m_ignoreExtList = new List<string>(strArr);
        }

        public void createDirData()
        {
            m_dirData = new DirData(m_inPath, this);
            m_dirData.findAllFiles();
        }

        public void createPrefab()
        {
            GameObject _go = m_dirData.createImageGo();

            string assetsPrefabPath = ExportUtil.getRelDataPath(m_outPath);
            // 创建预制，并且添加到编辑器中，以便进行检查
            PrefabUtility.CreatePrefab(assetsPrefabPath, _go, ReplacePrefabOptions.ConnectToPrefab);
            //PrefabUtility.CreatePrefab(assetsPrefabPath, _go);
            //刷新编辑器
            AssetDatabase.Refresh();
        }

        public void createAsset()
        {
            if(0 == m_resType)
            {
                createSpriteAsset();
            }
            else if (1 == m_resType)
            {
                createAnimatorAsset();
            }
        }

        public void createSpriteAsset()
        {
            SOSpriteList soSprite = m_dirData.createScriptSprite();

            string assetsPrefabPath = ExportUtil.getRelDataPath(m_outPath);
            // 创建预制，并且添加到编辑器中，以便进行检查
            AssetDatabase.CreateAsset(soSprite, assetsPrefabPath);
            //刷新编辑器
            AssetDatabase.Refresh();
        }

        public void createAnimatorAsset()
        {

        }
    }

    public class AtlasXmlParentPath
    {
        protected string m_inPath;
        protected string m_outPath;
        protected string m_ignoreExtStr;
        protected string m_fullDirPath;
        protected AtlasXmlData m_data;

        protected uint m_resType;

        public void parseXml(XmlElement packElem, AtlasXmlData data)
        {
            m_data = data;
            m_inPath = ExportUtil.getXmlAttrStr(packElem.Attributes["inpath"]);
            m_outPath = ExportUtil.getXmlAttrStr(packElem.Attributes["outpath"]);
            m_ignoreExtStr = ExportUtil.getXmlAttrStr(packElem.Attributes["ignoreext"]);
            m_resType = ExportUtil.getXmlAttrUInt(packElem.Attributes["restype"]);

            m_fullDirPath = ExportUtil.getDataPath(m_inPath);
            m_fullDirPath = ExportUtil.normalPath(m_fullDirPath);

            findAllFiles();
        }

        public void findAllFiles()
        {
            ExportUtil.traverseSubDirInOneDir(m_fullDirPath, onFindDir);
        }

        protected void onFindDir(DirectoryInfo dirInfo)
        {
            AtlasXmlPath xmlPath = new AtlasXmlPath();
            m_data.m_pathList.Add(xmlPath);
            
            string inpath = string.Format("{0}/{1}", m_inPath, dirInfo.Name);
            string outpath = string.Format("{0}/{1}.asset", m_outPath, dirInfo.Name);

            xmlPath.setPathParam(inpath, outpath, m_ignoreExtStr, m_resType);
        }
    }

    public class AtlasXmlParse
    {
        public void parseXml(string path, AtlasXmlData data)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");

            XmlNodeList packNodeList = rootNode.ChildNodes;
            XmlElement packElem;
            AtlasXmlPath xmlPath;

            AtlasXmlParentPath atlasXmlParentPath = null;

            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;
                if (ExportUtil.getXmlAttrStr(packElem.Attributes["parentpath"]) == UtilApi.TRUE)
                {
                    if(atlasXmlParentPath == null)
                    {
                        atlasXmlParentPath = new AtlasXmlParentPath();
                    }
                    atlasXmlParentPath.parseXml(packElem, data);
                }
                else
                {
                    xmlPath = new AtlasXmlPath();
                    data.m_pathList.Add(xmlPath);
                    xmlPath.parseXml(packElem);
                }
            }
        }
    }
}