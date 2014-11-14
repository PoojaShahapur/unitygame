using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    class ResCfgParse
    {
        public void parseXml(string path, List<Pack> packList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            XmlNodeList packNodeList = rootNode.ChildNodes;
            XmlElement packElem;
            Pack pack;

            foreach (XmlNode packNode in packNodeList)
            {
                packElem = (XmlElement)packNode;
                pack = new Pack();
                packList.Add(pack);
                pack.parseXml(packElem);
            }
        }
    }
}