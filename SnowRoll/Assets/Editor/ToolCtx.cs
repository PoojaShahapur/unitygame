namespace EditorTool
{
    public class ToolCtx
    {
        static public ToolCtx mInstance;

        public ExportAssetBundleNameSys mExportAssetBundleNameSys;
        public SpriteSheetImportSys mSpriteSheetImportSys;

        public static ToolCtx instance()
        {
            if (mInstance == null)
            {
                mInstance = new ToolCtx();
            }
            return mInstance;
        }

        public ToolCtx()
        {
            mExportAssetBundleNameSys = new ExportAssetBundleNameSys();

            init();
        }

        public void dispose()
        {
            mInstance = null;
        }

        public void init()
        {
            mExportAssetBundleNameSys.init();
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
                this.mSpriteSheetImportSys.parseSpriteSheet("F:/File/opensource/unity-game-git/unitygame/unitygame/SnowRoll/Assets/Resources/UiImage/TestAtlas/aaa.xml");
            }
        }
    }
}