namespace SDK.Lib
{
    /**
     * @brief 倒计时定时器
     */
    public class DaoJiShiTimer : TimerItemBase
    {
        override public void setTotalTime(float value)
        {
            base.setTotalTime(value);
            this.mCurRunTime = value;
        }

        override public float getRunTime()
        {
            return this.mTotalTime - this.mCurRunTime;
        }

        // 如果要获取剩余的倒计时时间，使用 getLeftCallTime
        override public float getLeftRunTime()
        {
            return this.mCurRunTime;
        }

        public override void OnTimer(float delta)
        {
            if (this.mDisposed)
            {
                return;
            }

            this.mCurRunTime -= delta;
            if(this.mCurRunTime < 0)
            {
                this.mCurRunTime = 0;
            }
            this.mIntervalLeftTime += delta;

            if (this.mIsInfineLoop)
            {
                checkAndDisp();
            }
            else
            {
                if (this.mCurRunTime <= 0)
                {
                    disposeAndDisp();
                }
                else
                {
                    checkAndDisp();
                }
            }
        }

        public override void reset()
        {
            this.mCurRunTime = this.mTotalTime;
            this.mCurCallTime = 0;
            this.mIntervalLeftTime = 0;
            this.mDisposed = false;
        }
    }
}