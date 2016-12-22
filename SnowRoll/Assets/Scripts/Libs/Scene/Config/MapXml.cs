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
            parseXml<DZDaoJiShiXmlLimit>(null, "limit");
            parseXml<MapXmlItem>(null, "scenesPVP");
        }
    }

    public class DZDaoJiShiXmlLimit : XmlItemBase
    {
        public uint mPrepareTime;
        public uint mRoundTime;
        public uint mPeaceNum;
        public uint mLuckyCoin;

        public uint mTiredCard;
        public uint mLastPrepareTime;
        public uint mLastRoundTime;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrUInt(xmlelem, "preparetime", ref mPrepareTime);
            UtilXml.getXmlAttrUInt(xmlelem, "roundtime", ref mRoundTime);
            UtilXml.getXmlAttrUInt(xmlelem, "peaceNum", ref mPeaceNum);
            UtilXml.getXmlAttrUInt(xmlelem, "luckyCoin", ref mLuckyCoin);

            UtilXml.getXmlAttrUInt(xmlelem, "tiredCard", ref mTiredCard);
            UtilXml.getXmlAttrUInt(xmlelem, "lastpreparetime", ref mLastPrepareTime);
            UtilXml.getXmlAttrUInt(xmlelem, "lastroundtime", ref mLastRoundTime);
        }
    }

    public class MapXmlItem : XmlItemBase
    {
        public uint mSceneId;
        public string mSceneName;
        public string mLevelName;

        public override void parseXml(SecurityElement xmlelem)
        {
            SecurityElement itemXml = null;
            UtilXml.getXmlChild(xmlelem, "item", ref itemXml);
            UtilXml.getXmlAttrUInt(itemXml, "id", ref mSceneId);
            UtilXml.getXmlAttrStr(itemXml, "name", ref mSceneName);
            UtilXml.getXmlAttrStr(itemXml, "res", ref mLevelName);
        }
    }
}