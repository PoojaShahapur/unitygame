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
            this.mResLoadState = CVResLoadState.eNotLoad;
        }

        public CVResLoadState resLoadState
        {
            get
            {
                return this.mResLoadState;
            }
            set
            {
                this.mResLoadState = value;
            }
        }

        public void reset()
        {
            this.mResLoadState = CVResLoadState.eNotLoad;
        }

        // 是否加载完成，可能成功可能失败
        public bool hasLoaded()
        {
            return this.mResLoadState == CVResLoadState.eFailed || mResLoadState == CVResLoadState.eLoaded;
        }

        public bool hasSuccessLoaded()
        {
            return this.mResLoadState == CVResLoadState.eLoaded;
        }

        public bool hasFailed()
        {
            return this.mResLoadState == CVResLoadState.eFailed;
        }

        // 没有加载或者正在加载中
        public bool hasNotLoadOrLoading()
        {
            return (this.mResLoadState == CVResLoadState.eLoading || this.mResLoadState == CVResLoadState.eNotLoad);
        }

        public void setSuccessLoaded()
        {
            this.mResLoadState = CVResLoadState.eLoaded;
        }

        public void setFailed()
        {
            this.mResLoadState = CVResLoadState.eFailed;
        }

        public void setLoading()
        {
            this.mResLoadState = CVResLoadState.eLoading;
        }

        public void copyFrom(ResLoadState rhv)
        {
            this.mResLoadState = rhv.resLoadState;
        }
    }
}