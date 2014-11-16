using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace EditorTool
{
    class Mesh
    {
        protected SkelMeshParam m_skelMeshParam = new SkelMeshParam();
        public List<SubMesh> m_subMeshList = new List<SubMesh>();
        public string m_resType;    // 资源类型

        public void parseXml(XmlElement elem)
        {
            m_skelMeshParam.m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);
            m_skelMeshParam.m_inPath = ExportUtil.getXmlAttrStr(elem.Attributes["inpath"]);
            m_skelMeshParam.m_outPath = ExportUtil.getXmlAttrStr(elem.Attributes["outpath"]);
            m_resType = ExportUtil.getXmlAttrStr(elem.Attributes["restype"]);

            XmlNodeList itemNodeList = elem.ChildNodes;
            XmlElement itemElem;
            SubMesh item;

            foreach (XmlNode itemNode in itemNodeList)
            {
                itemElem = (XmlElement)itemNode;
                item = new SubMesh();
                
                m_subMeshList.Add(item);
                item.parseXml(itemElem);
                item.m_resType = m_resType;
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
    }
}