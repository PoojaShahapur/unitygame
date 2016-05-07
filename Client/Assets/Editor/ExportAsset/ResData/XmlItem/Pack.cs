using SDK.Lib;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class Pack
    {
        public List<PackItem> m_packList = new List<PackItem>();

        public string m_name;

        public void parseXml(XmlElement elem)
        {
            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);

            XmlNodeList itemNodeList = elem.ChildNodes;
            XmlElement itemElem;
            PackItem item;

            foreach (XmlNode itemNode in itemNodeList)
            {
                itemElem = (XmlElement)itemNode;
                item = new PackItem();
                m_packList.Add(item);
                item.parseXml(itemElem);
            }
        }

        public void packOnePack(PackParam param)
        {
            List<string> pathList = new List<string>();
            pathList.Add(param.m_outPath);
            pathList.Add(m_name);
            string path = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
            ExportUtil.recurseCreateStreamDirectory(path);

            if (ExportUtil.BUNDLE == param.m_type)
            {
                packOneBundlePack(param);
            }
            else if (ExportUtil.LEVEL == param.m_type)
            {
                packOneLevelPack(param);
            }
        }

        protected void packOneBundlePack(PackParam param)
        {
            string resPath = "";
            List<Object> objList = new List<Object>();
            List<string> assetNamesList = new List<string>();
            UnityEngine.Object go;

            List<string> pathList = new List<string>();
            foreach (PackItem packItem in m_packList)
            {
                pathList.Clear();
                pathList.Add(param.m_inPath);
                pathList.Add(packItem.m_path);

                resPath = ExportUtil.getRelDataPath(ExportUtil.combine(pathList.ToArray()));
                assetNamesList.Add(resPath);
                go = AssetDatabase.LoadAssetAtPath(resPath, ExportUtil.convResStr2Type(packItem.m_resType));
                if (go)
                {
                    objList.Add(go);
                }
                else
                {
                    LoggerTool.error(string.Format("AssetDatabase.LoadAssetAtPath 不能加载资源 {0}", resPath));
                }
            }

            AssetBundleParam bundleParam = new AssetBundleParam();

#if UNITY_5
            bundleParam.m_buildList = new AssetBundleBuild[1];
            bundleParam.m_buildList[0].assetBundleName = m_name;
            bundleParam.m_buildList[0].assetBundleVariant = ExportUtil.UNITY3D;
            bundleParam.m_buildList[0].assetNames = assetNamesList.ToArray();
            pathList.Clear();
            pathList.Add(param.m_outPath);
            bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
#elif UNITY_4_6 || UNITY_4_5
            bundleParam.m_assets = objList.ToArray();
            pathList.Clear();
            pathList.Add(param.m_outPath);
            pathList.Add(m_name);
            bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
#endif

            ExportUtil.BuildAssetBundle(bundleParam);
        }

        protected void packOneLevelPack(PackParam param)
        {
            string resPath = "";
            List<string> nameList = new List<string>();
            List<string> pathList = new List<string>();

            foreach (PackItem packItem in m_packList)
            {
                pathList.Clear();
                pathList.Add(param.m_inPath);
                pathList.Add(packItem.m_path);
                resPath = ExportUtil.getRelDataPath(ExportUtil.combine(pathList.ToArray()));
                nameList.Add(resPath);
            }

            StreamedSceneAssetBundleParam bundleParam = new StreamedSceneAssetBundleParam();
            bundleParam.m_levels = nameList.ToArray();
            pathList.Clear();
            pathList.Add(param.m_outPath);
            pathList.Add(m_name);
            bundleParam.m_locationPath = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));

            ExportUtil.BuildStreamedSceneAssetBundle(bundleParam);
        }
    }
}