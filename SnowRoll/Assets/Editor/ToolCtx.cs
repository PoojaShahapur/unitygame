using SDK.Lib;

namespace EditorTool
{
    public class ToolCtx : Singleton<ToolCtx>, IMyDispose
    {
        public ExportAssetBundleNameSys mExportAssetBundleNameSys;
        public SpriteSheetImportSys mSpriteSheetImportSys;

        public ToolCtx()
        {
            mExportAssetBundleNameSys = new ExportAssetBundleNameSys();

            init();
        }

        public void init()
        {
            mExportAssetBundleNameSys.init();
        }

        public void dispose()
        {

        }

        public void clear()
        {
            mExportAssetBundleNameSys.clear();
        }

        public void exportAssetBundleName()
        {
            mExportAssetBundleNameSys.clear();
            mExportAssetBundleNameSys.parseXml();
            mExportAssetBundleNameSys.setAssetBundleName();
            mExportAssetBundleNameSys.exportResABKV();
        }

        public void spriteSheetImport()
        {
            if(null == this.mSpriteSheetImportSys)
            {
                this.mSpriteSheetImportSys = new SpriteSheetImportSys();
                this.mSpriteSheetImportSys.parseSpriteSheet("F:/File/opensource/unity-game-git/unitygame/unitygame/SnowRoll/Assets/Resources/UiImage/TestAtlas/TestAtlas.xml");
            }
        }
    }
}