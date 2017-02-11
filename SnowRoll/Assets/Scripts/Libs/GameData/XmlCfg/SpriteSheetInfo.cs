namespace SDK.Lib
{
    /**
     * @brief 一项精灵单
     */
    public class SpriteSheetItem
    {

    }

    public class SpriteSheetItemXmlItem : XmlItemBase
    {
        public string mName;
        public int mX;
        public int mY;
        public int mW;
        public int mH;
        public float mPX;
        public float mPY;

        public override void parseXml(System.Security.SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrStr(xmlelem, "n", ref mName);
            UtilXml.getXmlAttrInt(xmlelem, "x", ref mX);
            UtilXml.getXmlAttrInt(xmlelem, "y", ref mY);
            UtilXml.getXmlAttrInt(xmlelem, "w", ref mW);
            UtilXml.getXmlAttrInt(xmlelem, "h", ref mH);
            UtilXml.getXmlAttrFloat(xmlelem, "pX", ref mPX);
            UtilXml.getXmlAttrFloat(xmlelem, "pY", ref mPY);
        }
    }

    /**
     * @brief 一个 xml 配置文件
     */
    public class SpriteSheetInfo : XmlCfgBase
    {
        protected MList<XmlItemBase> mItemList;

        public SpriteSheetInfo()
        {
            
        }

        public void parseXmlByPath(string path)
        {
            MFileStream fileStream = new MFileStream(path);

            this.parseXml(fileStream.readText());
            fileStream.dispose();
            fileStream = null;
        }

        override public void parseXml(string str)
        {
            base.parseXml(str);

            this.mItemList = this.parseXml<SpriteSheetItemXmlItem>(null, "sprite");
        }
    }
}