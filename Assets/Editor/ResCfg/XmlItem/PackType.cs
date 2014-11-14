using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    class PackType
    {
        public PackParam m_packParam = new PackParam();

        public List<Pack> m_packList = new List<Pack>();

        public void parseXml(XmlElement elem)
        {
            m_packParam.m_type = elem.Attributes["type"].Value;
            m_packParam.m_path = elem.Attributes["path"].Value;
            m_packParam.m_extName = elem.Attributes["extname"].Value;

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

        public void packPack()
        {
            foreach(Pack pack in m_packList)
            {
                pack.packOnePack(m_packParam);
            }
        }
    }
}