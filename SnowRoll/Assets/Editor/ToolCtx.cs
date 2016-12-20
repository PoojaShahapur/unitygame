namespace EditorTool
{
    public class ToolCtx
    {
        static public ToolCtx mInstance;

        public ExportAssetBundleNameSys mExportAssetBundleNameSys;

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
    }
}