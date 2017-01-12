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
        protected CVResLoadState mResLoadState;

        public ResLoadState()
        {
            mResLoadState = CVResLoadState.eNotLoad;
        }

        public CVResLoadState resLoadState
        {
            get
            {
                return mResLoadState;
            }
            set
            {
                mResLoadState = value;
            }
        }

        public void reset()
        {
            mResLoadState = CVResLoadState.eNotLoad;
        }

        // 是否加载完成，可能成功可能失败
        public bool hasLoaded()
        {
            return mResLoadState == CVResLoadState.eFailed || mResLoadState == CVResLoadState.eLoaded;
        }

        public bool hasSuccessLoaded()
        {
            return mResLoadState == CVResLoadState.eLoaded;
        }

        public bool hasFailed()
        {
            return mResLoadState == CVResLoadState.eFailed;
        }

        // 没有加载或者正在加载中
        public bool hasNotLoadOrLoading()
        {
            return (mResLoadState == CVResLoadState.eLoading || mResLoadState == CVResLoadState.eNotLoad);
        }

        public void setSuccessLoaded()
        {
            mResLoadState = CVResLoadState.eLoaded;
        }

        public void setFailed()
        {
            mResLoadState = CVResLoadState.eFailed;
        }

        public void setLoading()
        {
            mResLoadState = CVResLoadState.eLoading;
        }

        public void copyFrom(ResLoadState rhv)
        {
            mResLoadState = rhv.resLoadState;
        }
    }
}