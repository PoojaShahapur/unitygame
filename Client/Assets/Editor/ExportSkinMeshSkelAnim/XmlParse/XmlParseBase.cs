using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    public class XmlParseBase
    {
        virtual public void parseXml(string path, XmlRootBase xmlRoot)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            xmlRoot.m_outPath = ExportUtil.getXmlAttrStr(rootNode.Attributes["outpath"]);
            if (xmlRoot is XmlSkinMeshRoot)
            {
                (xmlRoot as XmlSkinMeshRoot).m_exportFileType = (eExportFileType)ExportUtil.getXmlAttrInt(rootNode.Attributes["ExportFileType"]);
            }
            //XmlNodeList packNodeList = rootNode.ChildNodes;
            // Mesh 节点
            XmlNodeList packNodeList = rootNode.SelectNodes("Mesh");
            XmlElement packElem;
            Mesh mesh;

            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;
                mesh = new Mesh();
                xmlRoot.m_meshList.Add(mesh);
                mesh.parseXml(packElem);
            }

            // ModelTypes 节点
            XmlNode xmlModelTypesNode = rootNode.SelectSingleNode("ModelTypes");
            xmlRoot.m_modelTypes.parseXml(xmlModelTypesNode as XmlElement);

            // Path 节点
            ModelPath modelPath;
            packNodeList = rootNode.SelectNodes("ModelPath");
            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;
                modelPath = new ModelPath();
                xmlRoot.m_modelPathList.Add(modelPath);
                modelPath.parseXml(packElem);
            }
        }
    }
}