using SDK.Lib;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public class SubMesh
    {
        public string m_name;
        public string m_part;
        public string m_resType;    // 资源类型
        public eModelType m_modelType;  // 模型的类型
        public string m_outPath;        // 输出目录

        public void parseXml(XmlElement elem)
        {
            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);
            m_part = ExportUtil.getXmlAttrStr(elem.Attributes["part"]);
        }

        public void exportSubMeshBone(SkelMeshParam param, XmlDocument xmlDocSave, XmlElement meshXml)
        {
            XmlElement subMeshXml = xmlDocSave.CreateElement("SubMesh");
            meshXml.AppendChild(subMeshXml);
            
            List<string> pathList = new List<string>();
            pathList.Add(param.m_inPath);
            pathList.Add(param.m_name);

            string resPath = ExportUtil.getRelDataPath(UtilApi.combine(pathList.ToArray()));
            GameObject go = AssetDatabase.LoadAssetAtPath(resPath, ExportUtil.convResStr2Type(m_resType)) as GameObject;
            string ret = "";

            if (go != null)
            {
                SkinnedMeshRenderer render = go.transform.Find(m_name).gameObject.GetComponent<SkinnedMeshRenderer>();
                if (render != null)
                {
                    foreach (Transform tran in render.bones)
                    {
                        if(ret.Length > 0)
                        {
                            ret += ",";
                        }
                        ret += tran.gameObject.name;
                    }
                }
            }

            subMeshXml.SetAttribute("bonelist", ret);
        }

        public void exportSubMeshBoneFile(SkelMeshParam param, ref string xmlStr)
        {
            List<string> pathList = new List<string>();
            pathList.Add(param.m_inPath);
            pathList.Add(param.m_name);

            string resPath = ExportUtil.getRelDataPath(UtilApi.combine(pathList.ToArray()));
            GameObject go = AssetDatabase.LoadAssetAtPath(resPath, ExportUtil.convResStr2Type(m_resType)) as GameObject;
            string ret = "";

            if (go != null)
            {
                SkinnedMeshRenderer render = go.transform.Find(m_name).gameObject.GetComponent<SkinnedMeshRenderer>();
                if (render != null)
                {
                    foreach (Transform tran in render.bones)
                    {
                        if (ret.Length > 0)
                        {
                            ret += ",";
                        }
                        ret += tran.gameObject.name;
                    }
                }
            }

            xmlStr += string.Format("        <SubMesh name=\"{0}\" bonelist=\"{1}\" />\n", m_part, ret);
        }

        public void packSubMesh(SkelMeshParam param)
        {
            List<string> pathList = new List<string>();
            pathList.Add(param.m_inPath);
            pathList.Add(param.m_name);

            string resPath = ExportUtil.getRelDataPath(UtilApi.combine(pathList.ToArray()));
            GameObject go = AssetDatabase.LoadAssetAtPath(resPath, ExportUtil.convResStr2Type(m_resType)) as GameObject;
            GameObject subMeshGo = null;
            //GameObject insSubMeshGo = null;

            string subMeshName = "";
            string outPrefabPath = "";
            List<Object> objList = new List<Object>();
            List<string> assetNamesList = new List<string>();

            if (go != null)
            {
                subMeshGo = go.transform.Find(m_name).gameObject;
                if(subMeshGo != null)
                {
                    //insSubMeshGo = GameObject.Instantiate(subMeshGo);
                    //insSubMeshGo.transform.parent = null;
                    //subMeshName = ExportUtil.getSubMeshName(param.m_name, m_name);
                    subMeshName = ExportUtil.getSubMeshName(param.m_name, m_name);

                    pathList.Clear();
                    if (string.IsNullOrEmpty(m_outPath))
                    {
                        pathList.Add(SkinAnimSys.m_instance.m_xmlSubMeshRoot.m_outPath);
                    }
                    else
                    {
                        pathList.Add(m_outPath);
                    }
                    //pathList.Add(SkinAnimSys.m_instance.m_xmlSubMeshRoot.m_modelTypes.modelTypeDic[m_modelType].subPath);
                    pathList.Add(subMeshName + ".prefab");

                    outPrefabPath = ExportUtil.getRelDataPath(UtilApi.combine(pathList.ToArray()));
                    assetNamesList.Add(outPrefabPath);
                    //AssetDatabase.CreateAsset(insSubMeshGo, tmpPrefabPath);
                    //PrefabUtility.CreatePrefab(tmpPrefabPath, insSubMeshGo);
                    PrefabUtility.CreatePrefab(outPrefabPath, subMeshGo);

                    // 现在不打包 AssetBundle ，后期才会打包成 ab
//                    objList.Add(subMeshGo);

//                    AssetBundleParam bundleParam = new AssetBundleParam();
//#if UNITY_5
//                    bundleParam.m_buildList = new AssetBundleBuild[1];
//                    bundleParam.m_buildList[0].assetBundleName = subMeshName;
//                    bundleParam.m_buildList[0].assetBundleVariant = ExportUtil.UNITY3D;
//                    bundleParam.m_buildList[0].assetNames = assetNamesList.ToArray();
//                    pathList.Clear();
//                    pathList.Add(param.m_outPath);
//                    bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
//#elif UNITY_4_6 || UNITY_4_5
//                    bundleParam.m_assets = objList.ToArray();
//                    pathList.Clear();
//                    pathList.Add(param.m_outPath);
//                    pathList.Add(subMeshName + ".unity3d");
//                    bundleParam.m_pathName = ExportUtil.getStreamingDataPath(ExportUtil.combine(pathList.ToArray()));
//#endif

//                    ExportUtil.BuildAssetBundle(bundleParam);

                    // 释放资源
                    //UtilApi.DestroyImmediate(subMeshGo, false, false);
                    //AssetDatabase.Refresh();
                }
            }
        }
    }
}