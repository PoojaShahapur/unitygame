using SDK.Lib;
using System.Collections.Generic;

namespace EditorTool
{
    /**
     * @brief SpriteSheetImportSys 精灵单导入系统
     */
    public class SpriteSheetImportSys : Singleton<SpriteSheetImportSys>, IMyDispose
    {
        SpriteSheetInfo mSpriteSheetInfo;

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
            this.mSpriteSheetInfo = new SpriteSheetInfo();
            this.mSpriteSheetInfo.parseXmlByPath(path);
        }

        public bool isSpriteSheetPath(string path)
        {
            return true;
        }

        public List<UnityEditor.SpriteMetaData> getSpriteMetaList()
        {
            return this.mSpriteSheetInfo.getSpriteMetaList();
        }
    }
}