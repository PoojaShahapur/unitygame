using EditorTool;
using SDK.Lib;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public class AssetBundleNameXmlData
    {
        public List<AssetBundleNameXmlPath> m_pathList = new List<AssetBundleNameXmlPath>();

        public void clear()
        {
            m_pathList.Clear();
        }

        public void parseXml()
        {
            string path = ExportUtil.getDataPath("Res/Config/Tool/ExportAssetBundleName.xml");
            AssetBundleNameXmlParse parse = new AssetBundleNameXmlParse();
            parse.parseXml(path, this);
        }

        public void setAssetBundleName()
        {
            foreach (AssetBundleNameXmlPath xmlPath in m_pathList)
            {
                xmlPath.createDirData();
                xmlPath.setAssetBundleName();
            }
        }

        public void exportResABKV(List<string> list)
        {
            foreach (AssetBundleNameXmlPath xmlPath in m_pathList)
            {
                xmlPath.exportResABKV(list);
            }
        }
    }

    public class AssetBundleNameXmlPath
    {
        protected string m_inPath;
        protected List<string> m_includeExtList;             // 包含的扩展名列表

        protected AssetBundleNameDirData m_dirData;

        public List<string> includeExtList
        {
            get
            {
                return m_includeExtList;
            }
        }

        public void parseXml(XmlElement packElem)
        {
            m_inPath = ExportUtil.getXmlAttrStr(packElem.Attributes["inpath"]);

            string includeext = ExportUtil.getXmlAttrStr(packElem.Attributes["includeext"]);
            char[] separator = new char[1];
            separator[0] = ',';
            string[] strArr = includeext.Split(separator);
            m_includeExtList = new List<string>(strArr);
        }

        public void setPathParam(string inpath, string includeExtStr)
        {
            m_inPath = inpath;

            string ignoreext = includeExtStr;
            char[] separator = new char[1];
            separator[0] = ',';
            string[] strArr = ignoreext.Split(separator);
            m_includeExtList = new List<string>(strArr);
        }

        public void createDirData()
        {
            m_dirData = new AssetBundleNameDirData(m_inPath, this);
            m_dirData.findAllFiles();
        }

        public void setAssetBundleName()
        {
            m_dirData.setAssetBundleName();
        }

        public void exportResABKV(List<string> list)
        {
            m_dirData.exportResABKV(list);
        }
    }

    public class AssetBundleNameXmlParse
    {
        public void parseXml(string path, AssetBundleNameXmlData data)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");

            XmlNodeList packNodeList = rootNode.ChildNodes;
            XmlElement packElem;
            AssetBundleNameXmlPath xmlPath;

            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;

                xmlPath = new AssetBundleNameXmlPath();
                data.m_pathList.Add(xmlPath);
                xmlPath.parseXml(packElem);
            }
        }
    }
}