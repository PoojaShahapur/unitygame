using SDK.Common;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public class Mesh
    {
        protected SkelMeshParam m_skelMeshParam = new SkelMeshParam();
        public List<SubMesh> m_subMeshList = new List<SubMesh>();

        public void parseXml(XmlElement elem)
        {
            m_skelMeshParam.m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);
            m_skelMeshParam.m_inPath = ExportUtil.getXmlAttrStr(elem.Attributes["inpath"]);
            m_skelMeshParam.m_outPath = ExportUtil.getXmlAttrStr(elem.Attributes["outpath"]);
            m_skelMeshParam.m_resType = ExportUtil.getXmlAttrStr(elem.Attributes["restype"]);
            m_skelMeshParam.m_packSkel = ExportUtil.getXmlAttrBool(elem.Attributes["packskel"]);

            XmlNodeList itemNodeList = elem.ChildNodes;
            XmlElement itemElem;
            SubMesh item;

            foreach (XmlNode itemNode in itemNodeList)
            {
                itemElem = (XmlElement)itemNode;
                item = new SubMesh();
                
                m_subMeshList.Add(item);
                item.parseXml(itemElem);
                item.m_resType = m_skelMeshParam.m_resType;
            }
        }

        public void exportMeshBone(XmlDocument xmlDocSave, XmlElement root)
        {
            XmlElement meshXml = xmlDocSave.CreateElement("Mesh");
            root.AppendChild(meshXml);
            meshXml.SetAttribute("name", ExportUtil.getFileNameNoExt(m_skelMeshParam.m_name));

            foreach (SubMesh subMesh in m_subMeshList)
            {
                subMesh.exportSubMeshBone(m_skelMeshParam, xmlDocSave, meshXml);
            }
        }

        public void exportMeshBoneFile(ref string xmlStr)
        {
            xmlStr += string.Format("    <Mesh name=\"{0}\" >\n", ExportUtil.getFileNameNoExt(m_skelMeshParam.m_name));
            foreach (SubMesh subMesh in m_subMeshList)
            {
                subMesh.exportSubMeshBoneFile(m_skelMeshParam, ref xmlStr);
            }
            xmlStr += "    </Mesh>\n";
        }

        public void packSkelSubMesh(RootParam rootParam)
        {
            packSubMesh(rootParam);

            if (m_skelMeshParam.m_packSkel)
            {
                packSkel(rootParam);
            }
        }

        public void packSubMesh(RootParam rootParam)
        {
            foreach (SubMesh subMesh in m_subMeshList)
            {
                subMesh.packSubMesh(m_skelMeshParam, rootParam);
            }
        }

        public void packSkel(RootParam rootParam)
        {
            List<string> pathList = new List<string>();
            pathList.Add(m_skelMeshParam.m_inPath);
            pathList.Add(m_skelMeshParam.m_name);

            string resPath = ExportUtil.getRelDataPath(ExportUtil.combine(pathList.ToArray()));
            GameObject go = AssetDatabase.LoadAssetAtPath(resPath, ExportUtil.convResStr2Type("prefab")) as GameObject;
            GameObject subMeshGo = null;

            string skelNoExt = ExportUtil.getFileNameNoExt(m_skelMeshParam.m_name);
            string tmpPrefabPath = "";
            List<Object> objList = new List<Object>();
            List<string> assetNamesList = new List<string>();

            pathList.Clear();
            pathList.Add(rootParam.m_tmpPath);
            pathList.Add(skelNoExt + ".prefab");

            tmpPrefabPath = ExportUtil.getRelDataPath(ExportUtil.combine(pathList.ToArray()));
            assetNamesList.Add(tmpPrefabPath);
            PrefabUtility.CreatePrefab(tmpPrefabPath, go);

            go = AssetDatabase.LoadAssetAtPath(tmpPrefabPath, ExportUtil.convResStr2Type("prefab")) as GameObject;

            if (go != null)
            {
                foreach (SubMesh subMesh in m_subMeshList)
                {
                    subMeshGo = go.transform.Find(subMesh.m_name).gameObject;
                    if (subMeshGo != null)
                    {
                        //GameObject.DestroyImmediate(subMeshGo);
                        //GameObject.Destroy(subMeshGo);
                        //GameObject.DestroyImmediate(subMeshGo, true);
                        UtilApi.DestroyImmediate(subMeshGo, true);
                    }
                }

                AssetDatabase.Refresh();
                go = AssetDatabase.LoadAssetAtPath(tmpPrefabPath, ExportUtil.convResStr2Type("prefab")) as GameObject;
                objList.Add(go);

                AssetBundleParam bundleParam = new AssetBundleParam();
#if UNITY_5
                bundleParam.m_buildList = new AssetBundleBuild[1];
                bundleParam.m_buildList[0].assetBundleName = skelNoExt;
                bundleParam.m_buildList[0].assetBundleVariant = ExportUtil.UNITY3D;
                bundleParam.m_buildList[0].assetNames = assetNamesList.ToArray();
                pathList.Clear();
                pathList.Add(m_skelMeshParam.m_outPath);
                bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
#elif UNITY_4_6
                bundleParam.m_assets = objList.ToArray();
                pathList.Clear();
                pathList.Add(m_skelMeshParam.m_outPath);
                pathList.Add(skelNoExt + ".unity3d");
                bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
#endif

                ExportUtil.BuildAssetBundle(bundleParam);
            }
        }
    }
}