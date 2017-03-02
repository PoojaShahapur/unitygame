using Mono.Xml;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    /**
     * @brief Id 配置
     */
    public enum XmlCfgID
    {
        eXmlMarketCfg,
        eXmlMapCfg,
        eXmlSnowBallCfg,
    }

    /**
     * @brief 基本的 XmlItem
     */
    public class XmlItemBase
    {
        public virtual void parseXml(SecurityElement xmlelem)
        {

        }
    }

    public class XmlCfgBase
    {
        public string mPath;

        //internal SecurityParser mXmlDoc;
        //internal SecurityElement mXmlConfig;
        public SecurityParser mXmlDoc;
        public SecurityElement mXmlConfig;
        public MList<XmlItemBase> mList;

        public XmlCfgBase()
        {
            mXmlDoc = new SecurityParser();
            mList = new MList<XmlItemBase>();
        }

        public virtual void parseXml(string str)
        {
            mXmlDoc.LoadXml(str);
            mXmlConfig = mXmlDoc.ToXml();
        }

        protected MList<XmlItemBase> parseXml<T>(SecurityElement xmlElem, string itemNode) where T : XmlItemBase, new()
        {
            if(null == xmlElem)
            {
                xmlElem = this.mXmlConfig;
            }

            ArrayList itemNodeList = new ArrayList();
            UtilXml.getXmlChildList(xmlElem, itemNode, ref itemNodeList);

            MList<XmlItemBase> retList = new MList<XmlItemBase>();
            XmlItemBase item;

            int idx = 0;
            int len = itemNodeList.Count;
            SecurityElement itemElem = null;

            //foreach (SecurityElement itemElem in itemNodeList)
            while (idx < len)
            {
                itemElem = itemNodeList[idx] as SecurityElement;

                item = new T();

                mList.Add(item);
                retList.Add(item);

                item.parseXml(itemElem);

                ++idx;
            }

            return retList;
        }

        protected void parseAllXml<T>(SecurityElement xmlElem, string itemNode) where T : XmlItemBase, new()
        {
            if (null == xmlElem)
            {
                xmlElem = mXmlConfig;
            }

            ArrayList itemNodeList = null;
            UtilXml.getXmlElementAllChildList(xmlElem, itemNode, ref itemNodeList);
            
            MList<XmlItemBase> retList = new MList<XmlItemBase>();
            XmlItemBase item;

            int idx = 0;
            int len = itemNodeList.Count;
            SecurityElement itemElem = null;

            //foreach (SecurityElement itemElem in itemNodeList)
            while (idx < len)
            {
                itemElem = itemNodeList[idx] as SecurityElement;
                item = new T();

                mList.Add(item);
                retList.Add(item);

                item.parseXml(itemElem);

                ++idx;
            }
        }

        public void unload()
        {
            
        }

        public static T loadAndRetXml<T>(XmlCfgID id) where T : XmlCfgBase, new()
        {
            return Ctx.mInstance.mXmlCfgMgr.getXmlCfg<T>(id);
        }
    }
}