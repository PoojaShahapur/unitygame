using SDK.Common;
using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    class SkelSubMeshPackParse
    {
        public string m_outPath = "";
        public string m_tmpPath = "";

        public void parseXml(string path, List<Mesh> meshList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            m_outPath = ExportUtil.getXmlAttrStr(rootNode.Attributes["outpath"]);
            m_tmpPath = ExportUtil.getXmlAttrStr(rootNode.Attributes["tmppath"]);
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