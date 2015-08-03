using SDK.Common;
using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    class SkelMeshCfgParse
    {
        public void parseXml(string path, List<Mesh> meshList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            SkinAnimSys.m_instance.m_rootParam.m_outPath = ExportUtil.getXmlAttrStr(rootNode.Attributes["outpath"]);
            SkinAnimSys.m_instance.m_rootParam.m_exportFileType = (eExportFileType)ExportUtil.getXmlAttrInt(rootNode.Attributes["ExportFileType"]);
            XmlNodeList packNodeList = rootNode.ChildNodes;
            XmlElement packElem;
            Mesh mesh;
            
            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;
                mesh = new Mesh();
                meshList.Add(mesh);
                mesh.parseXml(packElem);
            }
        }
    }
}