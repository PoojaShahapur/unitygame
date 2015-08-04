using SDK.Common;
using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    class SkeletonCfgParse
    {
        public string m_outPath = "";
        public string m_tmpPath = "";

        public void parseXml(string path, List<Mesh> meshList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            SkinAnimSys.m_instance.m_xmlSkinMeshRoot.m_outPath = ExportUtil.getXmlAttrStr(rootNode.Attributes["outpath"]);
            SkinAnimSys.m_instance.m_xmlSubMeshRoot.m_tmpPath = ExportUtil.getXmlAttrStr(rootNode.Attributes["tmppath"]);
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