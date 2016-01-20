namespace EditorTool
{
    public class ExportAssetBundleNameSys
    {
        static public ExportAssetBundleNameSys m_instance;

        protected AssetBundleNameXmlData m_abNameXmlData;

        public static ExportAssetBundleNameSys instance()
        {
            if (m_instance == null)
            {
                m_instance = new ExportAssetBundleNameSys();
            }
            return m_instance;
        }

        public ExportAssetBundleNameSys()
        {
            m_abNameXmlData = new AssetBundleNameXmlData();
        }

        public void clear()
        {
            m_abNameXmlData.clear();
        }

        public void parseXml()
        {
            m_abNameXmlData.parseXml();
        }

        public void exportPrefab()
        {
            m_abNameXmlData.exportPrefab();
        }

        public void exportAsset()
        {
            m_abNameXmlData.exportAsset();
        }
    }
}