using SDK.Lib;

namespace EditorTool
{
    /**
     * @brief �ض��� Sprite
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

        // �Ƿ��о������
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