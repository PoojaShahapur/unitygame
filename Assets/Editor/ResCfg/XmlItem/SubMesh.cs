using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class SubMesh
    {
        public string m_name;
        public string m_part;
        public string m_resType;    // 资源类型

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

            string resPath = ExportUtil.getRelDataPath(ExportUtil.combine(pathList.ToArray()));
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

            string resPath = ExportUtil.getRelDataPath(ExportUtil.combine(pathList.ToArray()));
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
    }
}