using SDK.Lib;

namespace EditorTool
{
    public class SpriteSettingSys : Singleton<SpriteRedirectSys>, IMyDispose
    {
        protected SpriteSettingInfo mSpriteSettingInfo;

        public SpriteSettingSys()
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
            string path = "Editor/Config/SpriteSetting.xml";
            path = UtilEditor.convAssetPath2FullPath(path);

            mSpriteSettingInfo = new SpriteSettingInfo();
            mSpriteSettingInfo.parseXmlByPath(path);
            mSpriteSettingInfo.spriteSetting();

            UtilEditor.SaveAssets();
            UtilEditor.Refresh();
        }
    }
}