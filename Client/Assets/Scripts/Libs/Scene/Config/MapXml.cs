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
            UtilApi.getXmlChildList(config, itemNode, ref itemNodeList);
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
            m_preparetime = UtilApi.getXmlAttrUInt(xmlelem, "preparetime");
            m_roundtime = UtilApi.getXmlAttrUInt(xmlelem, "roundtime");
            m_peaceNum = UtilApi.getXmlAttrUInt(xmlelem, "peaceNum");
            m_luckyCoin = UtilApi.getXmlAttrUInt(xmlelem, "luckyCoin");

            m_tiredCard = UtilApi.getXmlAttrUInt(xmlelem, "tiredCard");
            m_lastpreparetime = UtilApi.getXmlAttrUInt(xmlelem, "lastpreparetime");
            m_lastroundtime = UtilApi.getXmlAttrUInt(xmlelem, "lastroundtime");
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
            UtilApi.getXmlChild(xmlelem, "item", ref itemXml);
            m_sceneId = UtilApi.getXmlAttrUInt(itemXml, "id");
            m_sceneName = UtilApi.getXmlAttrStr(itemXml, "name");
            m_levelName = UtilApi.getXmlAttrStr(itemXml, "res");
        }
    }
}