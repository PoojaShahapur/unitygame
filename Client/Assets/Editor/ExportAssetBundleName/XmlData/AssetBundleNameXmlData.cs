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

        public void exportPrefab()
        {
            foreach(AssetBundleNameXmlPath xmlPath in m_pathList)
            {
                xmlPath.createDirData();
            }
        }

        public void exportAsset()
        {
            foreach (AssetBundleNameXmlPath xmlPath in m_pathList)
            {
                xmlPath.createDirData();
            }
        }
    }

    public class AssetBundleNameXmlPath
    {
        protected string m_inPath;
        protected string m_outPath;
        protected List<string> m_ignoreExtList;             // 或略的扩展名列表

        protected AssetBundleNameDirData m_dirData;
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
            m_dirData = new AssetBundleNameDirData(m_inPath, this);
            m_dirData.findAllFiles();
        }
    }

    public class AssetBundleNameXmlParentPath
    {
        protected string m_inPath;
        protected string m_outPath;
        protected string m_ignoreExtStr;
        protected string m_fullDirPath;
        protected AssetBundleNameXmlData m_data;

        protected uint m_resType;

        public void parseXml(XmlElement packElem, AssetBundleNameXmlData data)
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
            AssetBundleNameXmlPath xmlPath = new AssetBundleNameXmlPath();
            m_data.m_pathList.Add(xmlPath);
            
            string inpath = string.Format("{0}/{1}", m_inPath, dirInfo.Name);
            string outpath = string.Format("{0}/{1}.asset", m_outPath, dirInfo.Name);

            xmlPath.setPathParam(inpath, outpath, m_ignoreExtStr, m_resType);
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