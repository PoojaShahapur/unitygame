using System.Collections.Generic;
using System.Security;

namespace SDK.Lib
{
    /**
     * @brief 商城
     */
    public class XmlMarketCfg : XmlCfgBase
    {
        public XmlMarketCfg()
        {
            m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathXmlCfg], "marketconfig.xml");
        }

        //public override XmlElement parseXml(string str)
        //{
        //    XmlElement objElem = base.parseXml(str);
        //    XmlNodeList itemNodeList = objElem.ChildNodes;

        //    XmlItemMarket item;
        //    foreach (XmlNode itemElem in itemNodeList)
        //    {
        //        item = new XmlItemMarket();
        //        item.parseXml(itemElem as XmlElement);
        //        m_list.Add(item);
        //    }
        //    return null;
        //}

        public override void parseXml(string str)
        {
            base.parseXml(str);
            parseXml<XmlItemMarket>(str, "obj");
        }

        public XmlItemBase getXmlItem(int id)
        {
            int idx = 0;
            while(idx < m_list.Count)
            {
                if ((m_list[idx] as XmlItemMarket).m_index == id)
                {
                    return m_list[idx];
                }
                ++idx;
            }

            return null;
        }
    }

    public class XmlItemMarket : XmlItemBase
    {
        public uint m_index;
        public uint m_objid;
        public uint m_num;
        public uint m_price;
		public uint m_type;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrUInt(xmlelem, "index", ref m_index);
            UtilXml.getXmlAttrUInt(xmlelem, "objid", ref m_objid);
            UtilXml.getXmlAttrUInt(xmlelem, "num", ref m_num);
            UtilXml.getXmlAttrUInt(xmlelem, "price", ref m_price);
			UtilXml.getXmlAttrUInt(xmlelem,"type", ref m_type);
        }
    }
}