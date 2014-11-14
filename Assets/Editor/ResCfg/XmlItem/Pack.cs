using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    class Pack
    {
        public List<PackItem> m_packList = new List<PackItem>();

        public string m_name;

        public void parseXml(XmlElement elem)
        {
            m_name = elem.Attributes["name"].Value;

            XmlNodeList itemNodeList = elem.ChildNodes;
            XmlElement itemElem;
            PackItem item;

            foreach (XmlNode itemNode in itemNodeList)
            {
                itemElem = (XmlElement)itemNode;
                item = new PackItem();
                m_packList.Add(item);
                item.parseXml(itemElem);
            }
        }
    }
}