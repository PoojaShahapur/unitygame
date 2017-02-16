using SDK.Lib;

namespace EditorTool
{
    /**
     * @brief 重定向 Sprite
     */
    public class SpriteRedirectSys : Singleton<SpriteRedirectSys>, IMyDispose
    {
        protected SpriteRedirectInfo mSpriteRedirectInfo;

        public SpriteRedirectSys()
        {

        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void redirectSprite()
        {
            string path = "Editor/Config/SpriteRedirect.xml";
            path = UtilEditor.convAssetPath2FullPath(path);

            mSpriteRedirectInfo = new SpriteRedirectInfo();
            mSpriteRedirectInfo.parseXmlByPath(path);
            mSpriteRedirectInfo.redirectSprite();

            UtilEditor.SaveAssets();
            UtilEditor.Refresh();
        }

        // 是否有精灵别名
        public SpriteRedirectAliasItemXml getSpriteAliasItem(string oldName)
        {
            if(null != this.mSpriteRedirectInfo)
            {
                return this.mSpriteRedirectInfo.getSpriteAliasItem(oldName);
            }

            return null;
        }
    }
}