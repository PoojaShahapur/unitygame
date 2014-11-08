using System.Collections.Generic;
using System.Xml;

namespace ResCfg
{
    class Pack
    {
        public List<Item> m_packList = new List<Item>();

        public string m_name;
        public string m_path;

        public void parseXml(XmlElement elem)
        {
            XmlNodeList itemNodeList = elem.ChildNodes;
            XmlElement itemElem;
            Item item;

            foreach (XmlNode itemNode in itemNodeList)
            {
                itemElem = (XmlElement)itemNode;
                item = new Item();
                m_packList.Add(item);
                item.parseXml(itemElem);
            }
        }
    }
}
