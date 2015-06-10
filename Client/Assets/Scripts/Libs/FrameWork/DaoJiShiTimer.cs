namespace SDK.Lib
{
    /**
     * @brief 倒计时定时器
     */
    public class DaoJiShiTimer : TimerItemBase
    {
        public override void OnTimer(float delta)
        {
            if (m_disposed)
            {
                return;
            }

            m_curTime -= delta;
            m_curLeftTimer += delta;

            if (m_bInfineLoop)
            {
                checkAndDisp();
            }
            else
            {
                if (m_curTime <= 0)
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
            m_curTime = m_totalTime;
            m_curLeftTimer = 0;
            m_disposed = false;
        }
    }
}