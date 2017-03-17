namespace SDK.Lib
{
    /**
     * @brief 帧间隔
     */
    public class FrameInterval
    {
        protected int mInterval;   // 帧间隔
        protected int mTotalFrame;      // 总共帧
        protected int mCurFrame;	    // 当前帧

        public FrameInterval()
        {
            this.mInterval = 1;
            this.mTotalFrame = 0;
            this.mCurFrame = 0;
        }

        public void setInterval(int value)
        {
            this.mInterval = value;
        }

        public void setTotalFrame(int value)
        {
            this.mTotalFrame = value;
        }

        public void setCurFrame(int value)
        {
            this.mCurFrame = value;
        }

        public bool canExec(int delta)
        {
            bool ret = false;

            this.mTotalFrame += delta;
            this.mCurFrame += delta;

            if (this.mCurFrame >= this.mInterval)
            {
                ret = true;
                this.mCurFrame -= this.mInterval;
            }

            return ret;
        }
    }
}