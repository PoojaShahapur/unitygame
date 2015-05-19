using EditorTool;
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

            }
        }
    }

    public class AtlasXmlPath
    {
        protected string m_inPath;
        protected string m_outPath;

        protected DirData m_dirData;

        public void parseXml(XmlElement packElem)
        {
            m_inPath = ExportUtil.getXmlAttrStr(packElem.Attributes["inpath"]);
            m_outPath = ExportUtil.getXmlAttrStr(packElem.Attributes["outpath"]);
        }

        public void createDirData()
        {
            m_dirData = new DirData(m_inPath);
            m_dirData.findAllFiles();
        }

        public void createPrefab()
        {
            GameObject _go = m_dirData.createImageGo();

            string assetsPrefabPath = ExportUtil.getDataPath(m_outPath);
            // 创建预制，并且添加到编辑器中，以便进行检查
            PrefabUtility.CreatePrefab(assetsPrefabPath, _go, ReplacePrefabOptions.ConnectToPrefab);
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