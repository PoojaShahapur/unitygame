using System.Collections;
using System.Security;

namespace SDK.Lib
{
    /**
     * @brief 初始配置
     */
    public class XmlItemInit : XmlItemBase
    {
        public float mRadius;
        public float mMassFactor;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrFloat(xmlelem, "Radius", ref mRadius);
            UtilXml.getXmlAttrFloat(xmlelem, "MassFactor", ref mMassFactor);
        }
    }

    /**
     * @brief 分裂配置
     */
    public class XmlItemSplit : XmlItemBase
    {
        public float mK;
        public float mN;
        public float mCanSplitFactor;
        public int mMax;
        public float mRelStartPos;
        public float mRelDist;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrFloat(xmlelem, "K", ref mK);
            UtilXml.getXmlAttrFloat(xmlelem, "N", ref mN);
            UtilXml.getXmlAttrFloat(xmlelem, "CanSplitFactor", ref mCanSplitFactor);
            UtilXml.getXmlAttrInt(xmlelem, "Max", ref mMax);
            UtilXml.getXmlAttrFloat(xmlelem, "RelStartPos", ref mRelStartPos);
            UtilXml.getXmlAttrFloat(xmlelem, "RelDist", ref mRelDist);
        }
    }

    /**
     * @brief 初始融合
     */
    public class XmlItemMerge : XmlItemBase
    {
        public float mContactTime;
        public float mCoolTime;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrFloat(xmlelem, "ContactTime", ref mContactTime);
            UtilXml.getXmlAttrFloat(xmlelem, "CoolTime", ref mCoolTime);
        }
    }

    /**
     * @brief 吞吐配置
     */
    public class XmlItemEmit : XmlItemBase
    {
        public float mSnowMass;
        public float mSnowRadius;
        public float mRelStartPos;
        public float mRelDist;
        public float mS;
        public float mA;
        public float mL;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrFloat(xmlelem, "SnowMass", ref mSnowMass);
            UtilXml.getXmlAttrFloat(xmlelem, "SnowRadius", ref mSnowRadius);
            UtilXml.getXmlAttrFloat(xmlelem, "RelStartPos", ref mRelStartPos);
            UtilXml.getXmlAttrFloat(xmlelem, "RelDist", ref mRelDist);

            UtilXml.getXmlAttrFloat(xmlelem, "S", ref mS);
            UtilXml.getXmlAttrFloat(xmlelem, "A", ref mA);
            UtilXml.getXmlAttrFloat(xmlelem, "L", ref mL);
        }
    }

    /**
    * @brief 吞噬
    */
    public class XmlItemAttack : XmlItemBase
    {
        public float mFactor;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrFloat(xmlelem, "Factor", ref mFactor);
        }
    }

    /**
    * @brief 速度
    */
    public class XmlItemMoveSpeed : XmlItemBase
    {
        public float mMoveSpeed_k;
        public float mMoveSpeed_b;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrFloat(xmlelem, "MoveSpeed_k", ref mMoveSpeed_k);
            UtilXml.getXmlAttrFloat(xmlelem, "MoveSpeed_b", ref mMoveSpeed_b);
        }
    }

    /**
     * @brief 雪块配置
     */
    public class XmlSnowBallCfg : XmlCfgBase
    {
        public XmlItemInit mXmlItemInit;
        public XmlItemSplit mXmlItemSplit;
        public XmlItemMerge mXmlItemMerge;
        public XmlItemEmit mXmlItemEmit;
        public XmlItemAttack mXmlItemAttack;
        public XmlItemMoveSpeed mXmlItemMoveSpeed;

        public XmlSnowBallCfg()
        {
            this.mPath = string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathXmlCfg], "SnowBall.xml");
        }

        public override void parseXml(string str)
        {
            base.parseXml(str);

            SecurityElement snowBallBasicElem = null;
            UtilXml.getXmlChild(this.mXmlConfig, "SnowBallBasic", ref snowBallBasicElem);

            mXmlItemInit = parseXml<XmlItemInit>(snowBallBasicElem, "Init")[0] as XmlItemInit;
            mXmlItemSplit = parseXml<XmlItemSplit>(snowBallBasicElem, "Split")[0] as XmlItemSplit;
            mXmlItemMerge = parseXml<XmlItemMerge>(snowBallBasicElem, "Merge")[0] as XmlItemMerge;
            mXmlItemEmit = parseXml<XmlItemEmit>(snowBallBasicElem, "Emit")[0] as XmlItemEmit;
            mXmlItemAttack = parseXml<XmlItemAttack>(snowBallBasicElem, "Attack")[0] as XmlItemAttack;
            mXmlItemMoveSpeed = parseXml<XmlItemMoveSpeed>(snowBallBasicElem, "MoveSpeed")[0] as XmlItemMoveSpeed;
        }
    }
}