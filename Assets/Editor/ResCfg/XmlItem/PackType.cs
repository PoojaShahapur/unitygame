using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    class PackType
    {
        public string m_type;
        public string m_path;
        public string m_extName;

        public List<Pack> m_packList = new List<Pack>();

        public void parseXml(XmlElement elem)
        {
            m_type = elem.Attributes["type"].Value;
            m_path = elem.Attributes["path"].Value;
            m_extName = elem.Attributes["extname"].Value;

            XmlNodeList itemNodeList = elem.ChildNodes;
            XmlElement itemElem;
            Pack pack;

            foreach (XmlNode itemNode in itemNodeList)
            {
                itemElem = (XmlElement)itemNode;
                pack = new Pack();
                m_packList.Add(pack);
                pack.parseXml(itemElem);
            }
        }
    }
}