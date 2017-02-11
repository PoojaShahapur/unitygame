using SDK.Lib;

namespace EditorTool
{
    /**
     * @brief 一项精灵单
     */
    public class SpriteSheetItem
    {

    }

    public class SpriteSheetItemXmlItem : XmlItemBase
    {

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