namespace SDK.Lib
{
    class LevelAsyncLoadItem : AsyncLoadItem
    {
        protected string m_levelName;

        public string levelName
        {
            get
            {
                return m_levelName;
            }
            set
            {
                m_levelName = value;
            }
        }

        // 加载完成后，在线程中的初始化
        virtual public void AsyncInit()
        {
            m_assetBundle = m_w3File.assetBundle;
        }
    }
}
