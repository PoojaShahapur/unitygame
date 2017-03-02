namespace SDK.Lib
{
    public enum ResPoolStateCV
    {
        eRPS_NotInPool,      // 没有在 Pool
        eRPS_InPool,        // 在 Pool
    }

    /**
     * @brief 资源在 Pool 状态
     */
    public class ResPoolState
    {
        protected ResPoolStateCV mResPoolState;

        public ResPoolState()
        {
            this.reset();
        }

        public ResPoolStateCV getResPoolState()
        {
            return this.mResPoolState;
        }

        public void setResPoolState(ResPoolStateCV value)
        {
            this.mResPoolState = value;
        }

        public bool isInPool()
        {
            return this.mResPoolState == ResPoolStateCV.eRPS_InPool;
        }

        public bool isNotInPool()
        {
            return this.mResPoolState == ResPoolStateCV.eRPS_NotInPool;
        }

        public void setInPool()
        {
            this.mResPoolState = ResPoolStateCV.eRPS_InPool;
        }

        public void setNotInPool()
        {
            this.mResPoolState = ResPoolStateCV.eRPS_NotInPool;
        }

        public void reset()
        {
            this.mResPoolState = ResPoolStateCV.eRPS_NotInPool;
        }
    }
}