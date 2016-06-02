namespace EditorTool
{
    public class ToolCtx
    {
        static public ToolCtx m_instance;

        public ExportAssetBundleNameSys mExportAssetBundleNameSys;

        public static ToolCtx instance()
        {
            if (m_instance == null)
            {
                m_instance = new ToolCtx();
            }
            return m_instance;
        }

        public ToolCtx()
        {
            mExportAssetBundleNameSys = new ExportAssetBundleNameSys();
            init();
        }

        public void dispose()
        {
            m_instance = null;
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