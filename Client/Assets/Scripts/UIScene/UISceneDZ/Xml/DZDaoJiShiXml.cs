using SDK.Common;
using SDK.Lib;
using System.Xml;
namespace Game.UI
{
    /**
     * @brief 对战倒计时
     */
    public class DZDaoJiShiXml : XmlCfgBase
    {
        public DZDaoJiShiXml()
        {
            m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathXmlCfg], "battleconfig.xml");
        }

        public override void parseXml(string str)
        {
            parseXml<DZDaoJiShiXmlLimit>(str, "limit");
        }

        public override XmlNodeList getXmlNodeList(XmlNode config, string itemNode)
        {
            XmlNodeList itemNodeList = config.SelectNodes(itemNode);
            return itemNodeList;
        }
    }

    public class DZDaoJiShiXmlLimit : XmlItemBase
    {
        public uint m_preparetime;
        public uint m_roundtime;
        public uint m_peaceNum;
        public uint m_luckyCoin;

        public uint m_tiredCard;
        public uint m_lastpreparetime;
        public uint m_lastroundtime;
        public uint m_roundTimes = 90;           // 回合时间

        public override void parseXml(XmlElement xmlelem)
        {
            m_preparetime = UtilApi.getXmlAttrUInt(xmlelem.Attributes["preparetime"]);
            m_roundtime = UtilApi.getXmlAttrUInt(xmlelem.Attributes["roundtime"]);
            m_peaceNum = UtilApi.getXmlAttrUInt(xmlelem.Attributes["peaceNum"]);
            m_luckyCoin = UtilApi.getXmlAttrUInt(xmlelem.Attributes["luckyCoin"]);

            m_tiredCard = UtilApi.getXmlAttrUInt(xmlelem.Attributes["tiredCard"]);
            m_lastpreparetime = UtilApi.getXmlAttrUInt(xmlelem.Attributes["lastpreparetime"]);
            m_lastroundtime = UtilApi.getXmlAttrUInt(xmlelem.Attributes["lastroundtime"]);
        }
    }
}