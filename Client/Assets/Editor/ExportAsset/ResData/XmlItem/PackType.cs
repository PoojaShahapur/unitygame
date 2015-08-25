using SDK.Lib;
using SDK.Lib;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class PackType
    {
        public PackParam m_packParam = new PackParam();

        public List<Pack> m_packList = new List<Pack>();

        public void parseXml(XmlElement elem)
        {
            m_packParam.m_type = ExportUtil.getXmlAttrStr(elem.Attributes["type"]);
            m_packParam.m_inPath = ExportUtil.getXmlAttrStr(elem.Attributes["inpath"]);
            m_packParam.m_outPath = ExportUtil.getXmlAttrStr(elem.Attributes["outpath"]);

            m_packParam.m_packAllFiles = ExportUtil.getXmlAttrBool(elem.Attributes["packallfiles"]);
            m_packParam.m_extArr = ExportUtil.getXmlAttrStr(elem.Attributes["infileext"]).Split(',');

            XmlNodeList itemNodeList = elem.ChildNodes;
            XmlElement itemElem;
            Pack pack;

            foreach (XmlNode itemNode in itemNodeList)
            {
                itemElem = (XmlElement)itemNode;
                pack = new Pack();
                m_packList.Add(pack);
                pack.parseXml(itemElem);
            }
        }

        public void packPack()
        {
            string path = ExportUtil.getStreamingDataPath(m_packParam.m_outPath);
            ExportUtil.CreateDirectory(path);

            if (m_packParam.m_packAllFiles)
            {
                packAll();
            }
            else
            {
                packByCfg();
            }
        }

        protected void packAll()
        {
             if (ExportUtil.BUNDLE == m_packParam.m_type)
            {
                packOneBundlePack();
            }
            else if (ExportUtil.LEVEL == m_packParam.m_type)
            {
                packOneLevelPack();
            }
        }

        protected void packByCfg()
        {
            foreach (Pack pack in m_packList)
            {
                pack.packOnePack(m_packParam);
            }
        }

        protected void packOneBundlePack()
        {
            string resPath = "";
            List<string> assetNamesList = new List<string>();
            List<Object> objList = new List<Object>();
            UnityEngine.Object go;

            List<string> pathList = new List<string>();
            List<string> filesList = ExportUtil.GetAll(ExportUtil.getDataPath(m_packParam.m_inPath));
            string ext = "";
            string nameNoExt = "";
#if UNITY_4_6
            string tmpStr = "";
#endif
            AssetBundleParam bundleParam = new AssetBundleParam();

            foreach (string filePath in filesList)
            {
                objList.Clear();
                assetNamesList.Clear();
                ext = ExportUtil.getFileExt(filePath);
                nameNoExt = ExportUtil.getFileNameNoExt(filePath);
                if (ExportUtil.isArrContainElem(ext, m_packParam.m_extArr))
                {
                    resPath = ExportUtil.convFullPath2AssetsPath(filePath);
                    assetNamesList.Add(resPath);
                    go = AssetDatabase.LoadAssetAtPath(resPath, ExportUtil.convResStr2Type(ExportUtil.convExt2ResStr(ext)));
                    if (go)
                    {
                        objList.Add(go);
                        
#if UNITY_5
                        bundleParam.m_buildList = new AssetBundleBuild[1];
                        bundleParam.m_buildList[0].assetBundleName = nameNoExt;
                        bundleParam.m_buildList[0].assetBundleVariant = ExportUtil.UNITY3D;
                        bundleParam.m_buildList[0].assetNames = assetNamesList.ToArray();
                        pathList.Clear();
                        pathList.Add(m_packParam.m_outPath);
                        bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
#elif UNITY_4_6 || UNITY_4_5
                        bundleParam.m_assets = objList.ToArray();
                        pathList.Clear();
                        pathList.Add(m_packParam.m_outPath);
                        tmpStr = string.Format("{0}{1}", nameNoExt, ExportUtil.DOTUNITY3D);
                        pathList.Add(tmpStr);
                        bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
#endif
                        ExportUtil.BuildAssetBundle(bundleParam);
                    }
                    else
                    {
                        LoggerTool.error(string.Format("AssetDatabase.LoadAssetAtPath 不能加载资源 {0}", filePath));
                    }
                }
            }
        }

        protected void packOneLevelPack()
        {
            string resPath = "";
            List<string> nameList = new List<string>();
            List<string> pathList = new List<string>();

            List<string> filesList = ExportUtil.GetAll(ExportUtil.getDataPath(m_packParam.m_inPath));
            string ext = "";
            string nameNoExt = "";
            string tmpStr = "";
            StreamedSceneAssetBundleParam bundleParam = new StreamedSceneAssetBundleParam();

            foreach (string filePath in filesList)
            {
                ext = ExportUtil.getFileExt(filePath);
                nameNoExt = ExportUtil.getFileNameNoExt(filePath);
                if (ExportUtil.isArrContainElem(ext, m_packParam.m_extArr))
                {
                    resPath = ExportUtil.convFullPath2AssetsPath(filePath);
                    nameList.Add(resPath);
                    bundleParam.m_levels = nameList.ToArray();
                    pathList.Clear();
                    pathList.Add(m_packParam.m_outPath);
                    tmpStr = string.Format("{0}{1}", nameNoExt, ExportUtil.DOTUNITY3D);
                    pathList.Add(tmpStr);
                    bundleParam.m_locationPath = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));

                    ExportUtil.BuildStreamedSceneAssetBundle(bundleParam);
                }
            }
        }
    }
}