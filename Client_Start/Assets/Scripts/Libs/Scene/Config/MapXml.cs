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
            mPath = string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathXmlCfg], "battleconfig.xml");
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
            UtilXml.getXmlAttrUInt(xmlelem, "preparetime", ref m_preparetime);
            UtilXml.getXmlAttrUInt(xmlelem, "roundtime", ref m_roundtime);
            UtilXml.getXmlAttrUInt(xmlelem, "peaceNum", ref m_peaceNum);
            UtilXml.getXmlAttrUInt(xmlelem, "luckyCoin", ref m_luckyCoin);

            UtilXml.getXmlAttrUInt(xmlelem, "tiredCard", ref m_tiredCard);
            UtilXml.getXmlAttrUInt(xmlelem, "lastpreparetime", ref m_lastpreparetime);
            UtilXml.getXmlAttrUInt(xmlelem, "lastroundtime", ref m_lastroundtime);
        }
    }

    public class MapXmlItem : XmlItemBase
    {
        public uint m_sceneId;
        public string m_sceneName;
        public string mLevelName;

        public override void parseXml(SecurityElement xmlelem)
        {
            SecurityElement itemXml = null;
            UtilXml.getXmlChild(xmlelem, "item", ref itemXml);
            UtilXml.getXmlAttrUInt(itemXml, "id", ref m_sceneId);
            UtilXml.getXmlAttrStr(itemXml, "name", ref m_sceneName);
            UtilXml.getXmlAttrStr(itemXml, "res", ref mLevelName);
        }
    }
}