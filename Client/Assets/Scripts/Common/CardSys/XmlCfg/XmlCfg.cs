using System.Collections.Generic;
using System.Xml;

namespace SDK.Common
{
    public enum XmlCfgID
    {
        eXmlMarketCfg,
    }

    public class XmlCfgBase
    {
        public string m_path;
        public string m_prefabName;

        public List<XmlItemBase> m_list = new List<XmlItemBase>();        // 商城

        public virtual void parseXml(string str)
        {

        }

        protected void parseXml<T>(string str) where T : XmlItemBase, new()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode config = xmlDoc.SelectSingleNode("config");
            XmlElement objElem = config.SelectSingleNode("obj") as XmlElement;

            XmlNodeList itemNodeList = objElem.ChildNodes;

            XmlItemBase item;
            foreach (XmlNode itemElem in itemNodeList)
            {
                item = new T();
                item.parseXml(itemElem as XmlElement);
                m_list.Add(item);
            }
        }
    }

    public class XmlItemBase
    {
        public virtual void parseXml(XmlElement xmlelem)
        {

        }
    }
}