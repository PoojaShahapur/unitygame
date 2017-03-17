using System.Security;

namespace SDK.Lib
{
    /**
     * @brief 商城 XmlItem
     */
    public class XmlItemMarket : XmlItemBase
    {
        public uint mIndex;
        public uint mObjId;
        public uint mNum;
        public uint mPrice;
        public uint mType;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrUInt(xmlelem, "index", ref mIndex);
            UtilXml.getXmlAttrUInt(xmlelem, "objid", ref mObjId);
            UtilXml.getXmlAttrUInt(xmlelem, "num", ref mNum);
            UtilXml.getXmlAttrUInt(xmlelem, "price", ref mPrice);
            UtilXml.getXmlAttrUInt(xmlelem, "type", ref mType);
        }
    }

    /**
     * @brief 商城
     */
    public class XmlMarketCfg : XmlCfgBase
    {
        public XmlMarketCfg()
        {
            mPath = string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathXmlCfg], "marketconfig.xml");
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
        //        mList.Add(item);
        //    }
        //    return null;
        //}

        public override void parseXml(string str)
        {
            base.parseXml(str);
            parseXml<XmlItemMarket>(null, "obj");
        }

        public XmlItemBase getXmlItem(int id)
        {
            int idx = 0;
            while(idx < mList.Count())
            {
                if ((mList[idx] as XmlItemMarket).mIndex == id)
                {
                    return mList[idx];
                }
                ++idx;
            }

            return null;
        }

        public static XmlMarketCfg loadAndRetXml()
        {
            return Ctx.mInstance.mXmlCfgMgr.getXmlCfg<XmlMarketCfg>(XmlCfgID.eXmlMarketCfg);
        }
    }
}