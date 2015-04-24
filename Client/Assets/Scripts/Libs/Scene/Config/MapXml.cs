﻿using SDK.Common;
using SDK.Lib;
using System.Xml;

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

    public class MapXmlItem : XmlItemBase
    {
        public uint m_sceneId;
        public string m_sceneName;
        public string m_levelName;

        public override void parseXml(XmlElement xmlelem)
        {
            XmlElement itemXml = xmlelem.SelectSingleNode("item") as XmlElement;
            m_sceneId = UtilApi.getXmlAttrUInt(itemXml.Attributes["id"]);
            m_sceneName = UtilApi.getXmlAttrStr(itemXml.Attributes["name"]);
            m_levelName = UtilApi.getXmlAttrStr(itemXml.Attributes["res"]);
        }
    }
}