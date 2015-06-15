using SDK.Common;
using SDK.Lib;
using System.Collections;
using System.Security;

namespace Game.UI
{
    /**
     * @brief 对战倒计时
     */
    public class DZDaoJiShiXmlBak : XmlCfgBase
    {
        public DZDaoJiShiXmlBak()
        {
            m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathXmlCfg], "battleconfig.xml");
        }

        public override void parseXml(string str)
        {
            base.parseXml(str);
            parseXml<DZDaoJiShiXmlLimit>(str, "limit");
        }

        public override ArrayList getXmlNodeList(SecurityElement config, string itemNode)
        {
            ArrayList itemNodeList = new ArrayList();
            UtilXml.getXmlChildList(config, itemNode, ref itemNodeList);

            return itemNodeList;
        }
    }

    public class DZDaoJiShiXmlLimitBak : XmlItemBase
    {
        public uint m_preparetime;
        public uint m_roundtime;
        public uint m_peaceNum;
        public uint m_luckyCoin;

        public uint m_tiredCard;
        public uint m_lastpreparetime;
        public uint m_lastroundtime;
        public uint m_roundTimes = 90;           // 回合时间

        public override void parseXml(SecurityElement xmlelem)
        {
            m_preparetime = UtilXml.getXmlAttrUInt(xmlelem, "preparetime");
            m_roundtime = UtilXml.getXmlAttrUInt(xmlelem, "roundtime");
            m_peaceNum = UtilXml.getXmlAttrUInt(xmlelem, "peaceNum");
            m_luckyCoin = UtilXml.getXmlAttrUInt(xmlelem, "luckyCoin");

            m_tiredCard = UtilXml.getXmlAttrUInt(xmlelem, "tiredCard");
            m_lastpreparetime = UtilXml.getXmlAttrUInt(xmlelem, "lastpreparetime");
            m_lastroundtime = UtilXml.getXmlAttrUInt(xmlelem, "lastroundtime");
        }
    }
}