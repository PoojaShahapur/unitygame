using Mono.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Security;

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
        internal SecurityParser m_xmlDoc = new SecurityParser();
        public List<XmlItemBase> m_list = new List<XmlItemBase>();

        public virtual void parseXml(string str)
        {
            m_xmlDoc.LoadXml(str);
        }

        protected void parseXml<T>(string str, string itemNode) where T : XmlItemBase, new()
        {
            SecurityElement config = m_xmlDoc.ToXml();
            ArrayList itemNodeList = getXmlNodeList(config, itemNode);

            XmlItemBase item;
            foreach (SecurityElement itemElem in itemNodeList)
            {
                item = new T();
                item.parseXml(itemElem);
                m_list.Add(item);
            }
        }

        public virtual ArrayList getXmlNodeList(SecurityElement config, string itemNode)
        {
            SecurityElement objElem = null;
            UtilXml.getXmlChild(config, itemNode, ref objElem);
            ArrayList itemNodeList = objElem.Children;
            return itemNodeList;
        }

        public void unload()
        {
            
        }
    }

    public class XmlItemBase
    {
        public virtual void parseXml(SecurityElement xmlelem)
        {

        }
    }
}