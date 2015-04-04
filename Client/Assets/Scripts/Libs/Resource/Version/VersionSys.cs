namespace SDK.Lib
{
    /**
     * @brief 版本系统，文件格式   path=value
     */
    public class VersionSys
    {
        protected FilesVer m_streamingAssetsVer = new FilesVer();
        protected FilesVer m_persistentDataVer = new FilesVer();
        protected FilesVer m_webVer = new FilesVer();

        public VersionSys()
        {
            m_streamingAssetsVer.m_type = FilesVerType.eStreamingAssetsVer;
            m_persistentDataVer.m_type = FilesVerType.ePersistentDataVer;
            m_webVer.m_type = FilesVerType.eWebVer;
        }

        public void loadMiniVerFile()
        {
            m_streamingAssetsVer.loadMiniVerFile();
            m_persistentDataVer.loadMiniVerFile();
            m_webVer.loadMiniVerFile();
        }

        public void loadVerFile()
        {
            m_streamingAssetsVer.loadVerFile();
            m_persistentDataVer.loadVerFile();
            m_webVer.loadVerFile();
        }
    }
}