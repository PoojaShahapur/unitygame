using System.Collections;
using System.Security;

namespace SDK.Lib
{
    /**
     * @brief 初始配置
     */
    public class XmlItemInit : XmlItemBase
    {
        public float mMassFactor;

        public override void parseXml(SecurityElement xmlelem)
        {
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
        public float mA;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrFloat(xmlelem, "A", ref mA);
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
    * @brief 相机控制
    */
    public class XmlItemCameraControl : XmlItemBase
    {
        public float mCameraDistance_Z;
        public float mCameraChangeFactor_Z;
        public float mLimitRadius;
        public float mCameraDistance_Y;
        public float mCameraChangeFactor_Y;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrFloat(xmlelem, "CameraDistance_Z", ref mCameraDistance_Z);
            UtilXml.getXmlAttrFloat(xmlelem, "CameraChangeFactor_Z", ref mCameraChangeFactor_Z);
            UtilXml.getXmlAttrFloat(xmlelem, "LimitRadius", ref mLimitRadius);
            UtilXml.getXmlAttrFloat(xmlelem, "CameraDistance_Y", ref mCameraDistance_Y);
            UtilXml.getXmlAttrFloat(xmlelem, "CameraChangeFactor_Y", ref mCameraChangeFactor_Y);
        }
    }

    /**
    * @brief 商店管理
    */
    public class XmlShop
    {
        //外形
        public XmlItemGoods[] shape;
        //鱼仔
        public XmlItemGoods[] child;
    }

    /**
    * @brief 物品配置
    */
    public class XmlItemGoods : XmlItemBase
    {
        public uint mID;
        public string mName;
        public uint mNeedID;
        public uint mNeedNum;
        public string mHot;
        public string mOnly;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrUInt(xmlelem, "ID", ref mID);
            UtilXml.getXmlAttrStr(xmlelem, "Name", ref mName);
            UtilXml.getXmlAttrUInt(xmlelem, "NeedID", ref mNeedID);
            UtilXml.getXmlAttrUInt(xmlelem, "NeedNum", ref mNeedNum);
            UtilXml.getXmlAttrStr(xmlelem, "Hot", ref mHot);
            UtilXml.getXmlAttrStr(xmlelem, "Only", ref mOnly);
        }
    }

    /**
     * @brief 地图配置
     */
    public class XmlItemMap : XmlItemBase
    {
        public uint mWidth;
        public uint mHeight;

        public override void parseXml(SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrUInt(xmlelem, "Width", ref mWidth);
            UtilXml.getXmlAttrUInt(xmlelem, "Height", ref mHeight);
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
        public XmlItemCameraControl mXmlItemCameraControl;
        public XmlShop mXmlShop;
        public XmlItemMap mXmlItemMap;

        public XmlSnowBallCfg()
        {
            this.mPath = string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathXmlCfg], "SnowBall.xml");
            this.mXmlShop = new XmlShop();
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

            SecurityElement cameraBasicElem = null;
            UtilXml.getXmlChild(this.mXmlConfig, "CameraBasic", ref cameraBasicElem);
            mXmlItemCameraControl = parseXml<XmlItemCameraControl>(cameraBasicElem, "CameraControl")[0] as XmlItemCameraControl;

            //解析商店配置
            parseShopItems();

            // 地图配置
            mXmlItemMap = parseXml<XmlItemMap>(this.mXmlConfig, "Map")[0] as XmlItemMap;
        }

        private void parseShopItems()
        {
            SecurityElement ShopBasicElem = null;
            UtilXml.getXmlChild(this.mXmlConfig, "ShopBasic", ref ShopBasicElem);

            parseSkinItems(ShopBasicElem); //皮肤商店
            parseGodItems(ShopBasicElem); //圣衣商店
        }

        private void parseSkinItems(SecurityElement ShopBasicElem)
        {
            SecurityElement SkinShopElem = null;
            UtilXml.getXmlChild(ShopBasicElem, "SkinShop", ref SkinShopElem);

            //外形
            SecurityElement ShapeElem = null;
            UtilXml.getXmlChild(SkinShopElem, "Shape", ref ShapeElem);
            int count = ShapeElem.Children.Count;
            mXmlShop.shape = new XmlItemGoods[count];
            for (int i = 0; i < count; ++i)
            {
                mXmlShop.shape[i] = parseXml<XmlItemGoods>(ShapeElem, "Item")[i] as XmlItemGoods;
            }

            //鱼仔
            SecurityElement ChildElem = null;
            UtilXml.getXmlChild(SkinShopElem, "Child", ref ChildElem);
            count = ChildElem.Children.Count;
            mXmlShop.child = new XmlItemGoods[count];
            for (int i = 0; i < count; ++i)
            {
                mXmlShop.child[i] = parseXml<XmlItemGoods>(ChildElem, "Item")[i] as XmlItemGoods;
            }
        }

        private void parseGodItems(SecurityElement ShopBasicElem)
        {
            SecurityElement SkinShopElem = null;
            UtilXml.getXmlChild(ShopBasicElem, "GodShop", ref SkinShopElem);
        }
    }
}