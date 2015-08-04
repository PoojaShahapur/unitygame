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

        public SkelMeshParam skelMeshParam
        {
            get
            {
                return m_skelMeshParam;
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_skelMeshParam.m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);
            m_skelMeshParam.m_inPath = ExportUtil.getXmlAttrStr(elem.Attributes["inpath"]);
            m_skelMeshParam.m_outPath = ExportUtil.getXmlAttrStr(elem.Attributes["outpath"]);
            m_skelMeshParam.m_resType = ExportUtil.getXmlAttrStr(elem.Attributes["restype"]);
            m_skelMeshParam.m_modelType = (eModelType)ExportUtil.getXmlAttrInt(elem.Attributes["modeltype"]);

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
                item.m_modelType = m_skelMeshParam.m_modelType;
                item.m_outPath = m_skelMeshParam.m_outPath;
            }
        }

        // 从 Mesh 中添加 SubMesh
        public void addSubMesh()
        {
            List<string> pathList = new List<string>();
            pathList.Add(m_skelMeshParam.m_inPath);
            pathList.Add(m_skelMeshParam.m_name);

            // 目录一定是以 "Assets" 开头的相对目录
            string resPath = ExportUtil.getRelDataPath(ExportUtil.combine(pathList.ToArray()));
            UnityEngine.Object go_ = AssetDatabase.LoadAssetAtPath(resPath, ExportUtil.convResStr2Type(m_skelMeshParam.m_resType));
            GameObject go = AssetDatabase.LoadAssetAtPath(resPath, ExportUtil.convResStr2Type(m_skelMeshParam.m_resType)) as GameObject;

            int childCount = go.transform.childCount;
            int idx = 0;
            Transform childTrans = null;
            SkinnedMeshRenderer render = null;
            SubMesh item = null;

            for (idx = 0; idx < childCount; ++idx)
            {
                render = null;
                childTrans = go.transform.GetChild(idx);
                render = childTrans.gameObject.GetComponent<SkinnedMeshRenderer>();

                if (render != null)
                {
                    item = new SubMesh();
                    m_subMeshList.Add(item);
                    item.m_name = childTrans.gameObject.name;
                    item.m_part = item.m_name;
                    item.m_resType = m_skelMeshParam.m_resType;
                    item.m_modelType = m_skelMeshParam.m_modelType;
                    item.m_outPath = m_skelMeshParam.m_outPath;
                }
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
            if (m_subMeshList.Count == 0)        // 说明没有定义 SubMesh，就说明导出 GameObject 中有 Skinned Mesh Renderer 组件的 GameObject
            {
                addSubMesh();
            }

            xmlStr += string.Format("    <Mesh name=\"{0}\" >\n", ExportUtil.getFileNameNoExt(m_skelMeshParam.m_name));
            foreach (SubMesh subMesh in m_subMeshList)
            {
                subMesh.exportSubMeshBoneFile(m_skelMeshParam, ref xmlStr);
            }
            xmlStr += "    </Mesh>\n";
        }

        public void packSkelSubMesh()
        {
            packSubMesh();
        }

        public void packSubMesh()
        {
            if (m_subMeshList.Count == 0)        // 说明没有定义 SubMesh，就说明导出 GameObject 中有 Skinned Mesh Renderer 组件的 GameObject
            {
                addSubMesh();
            }

            foreach (SubMesh subMesh in m_subMeshList)
            {
                subMesh.packSubMesh(m_skelMeshParam);
            }
        }

        public void packSkel()
        {
            List<string> pathList = new List<string>();
            pathList.Add(m_skelMeshParam.m_inPath);
            pathList.Add(m_skelMeshParam.m_name);

            string resPath = ExportUtil.getRelDataPath(ExportUtil.combine(pathList.ToArray()));
            GameObject go = AssetDatabase.LoadAssetAtPath(resPath, ExportUtil.convResStr2Type("prefab")) as GameObject;
            GameObject subMeshGo = null;

            string skelNoExt = ExportUtil.getFileNameNoExt(m_skelMeshParam.m_name);
            string outPrefabPath = "";
            List<Object> objList = new List<Object>();
            List<string> assetNamesList = new List<string>();

            pathList.Clear();
            if (string.IsNullOrEmpty(m_skelMeshParam.m_outPath))
            {
                pathList.Add(SkinAnimSys.m_instance.m_xmlSubMeshRoot.m_outPath);
            }
            else
            {
                pathList.Add(m_skelMeshParam.m_outPath);
            }
            pathList.Add(SkinAnimSys.m_instance.m_xmlSkeletonRoot.m_modelTypes.modelTypeDic[m_skelMeshParam.m_modelType].subPath);
            pathList.Add(skelNoExt + ".prefab");

            outPrefabPath = ExportUtil.getRelDataPath(ExportUtil.combine(pathList.ToArray()));
            assetNamesList.Add(outPrefabPath);
            PrefabUtility.CreatePrefab(outPrefabPath, go);

            go = AssetDatabase.LoadAssetAtPath(outPrefabPath, ExportUtil.convResStr2Type("prefab")) as GameObject;

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
                        UtilApi.DestroyImmediate(subMeshGo, false, false);
                    }
                }

                AssetDatabase.Refresh();

                // 现在不直接打包，需要最后发布的时候才打包
                go = AssetDatabase.LoadAssetAtPath(outPrefabPath, ExportUtil.convResStr2Type("prefab")) as GameObject;
                objList.Add(go);

//                AssetBundleParam bundleParam = new AssetBundleParam();
//#if UNITY_5
//                bundleParam.m_buildList = new AssetBundleBuild[1];
//                bundleParam.m_buildList[0].assetBundleName = skelNoExt;
//                bundleParam.m_buildList[0].assetBundleVariant = ExportUtil.UNITY3D;
//                bundleParam.m_buildList[0].assetNames = assetNamesList.ToArray();
//                pathList.Clear();
//                pathList.Add(m_skelMeshParam.m_outPath);
//                bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
//#elif UNITY_4_6 || UNITY_4_5
//                bundleParam.m_assets = objList.ToArray();
//                pathList.Clear();
//                pathList.Add(m_skelMeshParam.m_outPath);
//                pathList.Add(skelNoExt + ".unity3d");
//                bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
//#endif

//                ExportUtil.BuildAssetBundle(bundleParam);
            }
        }
    }
}