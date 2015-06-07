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
            UtilApi.getXmlChildList(config, itemNode, ref itemNodeList);

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
            m_preparetime = UtilApi.getXmlAttrUInt(xmlelem, "preparetime");
            m_roundtime = UtilApi.getXmlAttrUInt(xmlelem, "roundtime");
            m_peaceNum = UtilApi.getXmlAttrUInt(xmlelem, "peaceNum");
            m_luckyCoin = UtilApi.getXmlAttrUInt(xmlelem, "luckyCoin");

            m_tiredCard = UtilApi.getXmlAttrUInt(xmlelem, "tiredCard");
            m_lastpreparetime = UtilApi.getXmlAttrUInt(xmlelem, "lastpreparetime");
            m_lastroundtime = UtilApi.getXmlAttrUInt(xmlelem, "lastroundtime");
        }
    }
}