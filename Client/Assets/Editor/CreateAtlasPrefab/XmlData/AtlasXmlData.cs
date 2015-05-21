using EditorTool;
using SDK.Lib;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace AtlasPrefabSys
{
    public class AtlasXmlData
    {
        public List<AtlasXmlPath> m_pathList = new List<AtlasXmlPath>();

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

            string ignoreext = ExportUtil.getXmlAttrStr(packElem.Attributes["ignoreext"]);
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
            SOSpriteList soSprite = m_dirData.createScriptSprite();

            string assetsPrefabPath = ExportUtil.getRelDataPath(m_outPath);
            // 创建预制，并且添加到编辑器中，以便进行检查
            AssetDatabase.CreateAsset(soSprite, assetsPrefabPath);
            //刷新编辑器
            AssetDatabase.Refresh();
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

            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;
                xmlPath = new AtlasXmlPath();
                data.m_pathList.Add(xmlPath);
                xmlPath.parseXml(packElem);
            }
        }
    }
}