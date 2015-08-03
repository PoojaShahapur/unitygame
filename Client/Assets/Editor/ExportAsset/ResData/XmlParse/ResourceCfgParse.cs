using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    public class ResourceCfgParse
    {
        public void parseXml(string path, List<ResourcesPathItem> packList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            ResExportSys.m_instance.m_pResourcesCfgPackData.m_resListOutpath = ExportUtil.getXmlAttrStr(rootNode.Attributes["reslistoutpath"]);
            XmlNodeList packNodeList = rootNode.ChildNodes;
            XmlElement packElem;
            ResourcesPathItem packType;

            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;
                packType = new ResourcesPathItem();
                packList.Add(packType);
                packType.parseXml(packElem);
            }
        }
    }
}