using SDK.Common;
using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    class SkelMeshCfgParse
    {
        public void parseXml(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            SkinAnimSys.m_instance.m_xmlSkinMeshRoot.m_outPath = ExportUtil.getXmlAttrStr(rootNode.Attributes["outpath"]);
            SkinAnimSys.m_instance.m_xmlSkinMeshRoot.m_exportFileType = (eExportFileType)ExportUtil.getXmlAttrInt(rootNode.Attributes["ExportFileType"]);
            //XmlNodeList packNodeList = rootNode.ChildNodes;
            // Mesh 节点
            XmlNodeList packNodeList = rootNode.SelectNodes("Mesh");
            XmlElement packElem;
            Mesh mesh;
            
            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;
                mesh = new Mesh();
                SkinAnimSys.m_instance.m_xmlSkinMeshRoot.m_skinMeshList.Add(mesh);
                mesh.parseXml(packElem);
            }

            // ModelTypes 节点
            XmlNode xmlModelTypesNode = rootNode.SelectSingleNode("ModelTypes");
            SkinAnimSys.m_instance.m_xmlSkinMeshRoot.m_modelTypes.parseXml(xmlModelTypesNode as XmlElement);

            // Path 节点
            ModelPath modelPath;
            packNodeList = rootNode.SelectNodes("Path");
            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;
                modelPath = new ModelPath();
                SkinAnimSys.m_instance.m_xmlSkinMeshRoot.m_modelPathList.Add(modelPath);
                modelPath.parseXml(packElem);
            }
        }
    }
}