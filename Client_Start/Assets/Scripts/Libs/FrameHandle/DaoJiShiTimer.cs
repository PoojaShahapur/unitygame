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
            this.m_curRunTime = value;
        }

        override public float getRunTime()
        {
            return this.m_totalTime - this.m_curRunTime;
        }

        // 如果要获取剩余的倒计时时间，使用 getLeftCallTime
        override public float getLeftRunTime()
        {
            return this.m_curRunTime;
        }

        public override void OnTimer(float delta)
        {
            if (m_disposed)
            {
                return;
            }

            m_curRunTime -= delta;
            if(m_curRunTime < 0)
            {
                m_curRunTime = 0;
            }
            m_intervalLeftTime += delta;

            if (m_bInfineLoop)
            {
                checkAndDisp();
            }
            else
            {
                if (m_curRunTime <= 0)
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
            m_curRunTime = m_totalTime;
            m_curCallTime = 0;
            m_intervalLeftTime = 0;
            m_disposed = false;
        }
    }
}