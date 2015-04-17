using System.Collections.Generic;
using System.Xml;

namespace SDK.Common
{
    public enum XmlCfgID
    {
        eXmlMarketCfg,
        eXmlDZCfg,
        eXmlMapCfg,
    }

    public class XmlCfgBase
    {
        public string m_path;
        public XmlDocument m_xmlDoc = new XmlDocument();
        public List<XmlItemBase> m_list = new List<XmlItemBase>();

        public virtual void parseXml(string str)
        {
            m_xmlDoc.LoadXml(str);
        }

        protected void parseXml<T>(string str, string itemNode) where T : XmlItemBase, new()
        {
            XmlNode config = m_xmlDoc.SelectSingleNode("config");
            XmlNodeList itemNodeList = getXmlNodeList(config, itemNode);

            XmlItemBase item;
            foreach (XmlNode itemElem in itemNodeList)
            {
                item = new T();
                item.parseXml(itemElem as XmlElement);
                m_list.Add(item);
            }
        }

        public virtual XmlNodeList getXmlNodeList(XmlNode config, string itemNode)
        {
            XmlElement objElem = config.SelectSingleNode(itemNode) as XmlElement;
            XmlNodeList itemNodeList = objElem.ChildNodes;
            return itemNodeList;
        }

        public void unload()
        {
            
        }
    }

    public class XmlItemBase
    {
        public virtual void parseXml(XmlElement xmlelem)
        {

        }
    }
}