namespace SDK.Lib
{
    public enum CVResLoadState
    {
        eNotLoad,       // 没有加载
        eLoading,       // 正在加载
        eLoaded,        // 加载成功
        eFailed         // 加载失败
    }

    public class ResLoadState
    {
        protected CVResLoadState m_resLoadState;

        public ResLoadState()
        {
            m_resLoadState = CVResLoadState.eNotLoad;
        }

        public CVResLoadState resLoadState
        {
            get
            {
                return m_resLoadState;
            }
            set
            {
                m_resLoadState = value;
            }
        }

        public void reset()
        {
            m_resLoadState = CVResLoadState.eNotLoad;
        }

        // 是否加载完成，可能成功可能失败
        public bool hasLoaded()
        {
            return m_resLoadState == CVResLoadState.eFailed || m_resLoadState == CVResLoadState.eLoaded;
        }

        public bool hasSuccessLoaded()
        {
            return m_resLoadState == CVResLoadState.eLoaded;
        }

        public bool hasFailed()
        {
            return m_resLoadState == CVResLoadState.eFailed;
        }

        // 没有加载或者正在加载中
        public bool hasNotLoadOrLoading()
        {
            return (m_resLoadState == CVResLoadState.eLoading || m_resLoadState == CVResLoadState.eNotLoad);
        }

        public void setSuccessLoaded()
        {
            m_resLoadState = CVResLoadState.eLoaded;
        }

        public void setFailed()
        {
            m_resLoadState = CVResLoadState.eFailed;
        }

        public void setLoading()
        {
            m_resLoadState = CVResLoadState.eLoading;
        }

        public void copyFrom(ResLoadState rhv)
        {
            m_resLoadState = rhv.resLoadState;
        }
    }
}