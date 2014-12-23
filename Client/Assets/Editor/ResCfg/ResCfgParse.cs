using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    class ResCfgParse
    {
        public void parseXml(string path, List<PackType> packList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            XmlNodeList packNodeList = rootNode.ChildNodes;
            XmlElement packElem;
            PackType packType;

            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;
                packType = new PackType();
                packList.Add(packType);
                packType.parseXml(packElem);
            }
        }
    }
}