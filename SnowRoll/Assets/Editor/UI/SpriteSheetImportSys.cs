using SDK.Lib;

namespace EditorTool
{
    /**
     * @brief SpriteSheetImportSys 精灵单导入系统
     */
    public class SpriteSheetImportSys : Singleton<SpriteSheetImportSys>, IMyDispose
    {
        public SpriteSheetImportSys()
        {

        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void parseSpriteSheet(string path)
        {
            SpriteSheetInfo info = new SpriteSheetInfo();
            info.parseXmlByPath(path);
        }
    }
}