using SDK.Common;
using SDK.Lib;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    /**
     * @brief 对战倒计时
     */
    public class MapXml : XmlCfgBase
    {
        public MapXml()
        {
            m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathXmlCfg], "battleconfig.xml");
        }

        public override void parseXml(string str)
        {
            base.parseXml(str);
            parseXml<DZDaoJiShiXmlLimit>(str, "limit");
            parseXml<MapXmlItem>(str, "scenesPVP");
        }

        public override ArrayList getXmlNodeList(SecurityElement config, string itemNode)
        {
            ArrayList itemNodeList = new ArrayList();
            UtilXml.getXmlChildList(config, itemNode, ref itemNodeList);
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

    public class MapXmlItem : XmlItemBase
    {
        public uint m_sceneId;
        public string m_sceneName;
        public string m_levelName;

        public override void parseXml(SecurityElement xmlelem)
        {
            SecurityElement itemXml = null;
            UtilXml.getXmlChild(xmlelem, "item", ref itemXml);
            m_sceneId = UtilXml.getXmlAttrUInt(itemXml, "id");
            m_sceneName = UtilXml.getXmlAttrStr(itemXml, "name");
            m_levelName = UtilXml.getXmlAttrStr(itemXml, "res");
        }
    }
}