using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 一项精灵单
     */
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

        public UnityEditor.SpriteMetaData toMetaData()
        {
            UnityEditor.SpriteMetaData data = new UnityEditor.SpriteMetaData();

            data.alignment = 0;
            data.border.x = 0;
            data.border.y = 0;
            data.border.z = 0;
            data.border.w = 0;

            data.name = mName;
            data.pivot.x = 0.5f;
            data.pivot.y = 0.5f;

            data.rect.x = mX;
            data.rect.y = mY;
            data.rect.width = mW;
            data.rect.height = mH;

            return data;
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

        public List<UnityEditor.SpriteMetaData> getSpriteMetaList()
        {
            List<UnityEditor.SpriteMetaData> list = new List<UnityEditor.SpriteMetaData>();

            UnityEditor.SpriteMetaData data;

            int idx = 0;
            int len = this.mItemList.Count();

            while(idx < len)
            {
                data = (this.mItemList[idx] as SpriteSheetItemXmlItem).toMetaData();

                ++idx;
            }

            return list;
        }
    }
}