using SDK.Common;
using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    class SkelMeshCfgParse
    {
        public string m_outPath = "";

        public void parseXml(string path, List<Mesh> meshList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            m_outPath = UtilApi.getXmlAttrStr(rootNode.Attributes["outpath"]);
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