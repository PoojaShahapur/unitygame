namespace SDK.Lib
{
    /**
     * @brief 倒计时定时器
     */
    public class DaoJiShiTimer : TimerItemBase
    {
        protected override bool checkEnd(float delta)
        {
            m_curTime -= delta;
            if (m_curTime <= 0)            // 保证肯定退出
            {
                m_disposed = true;
                return true;
            }

            return false;
        }

        public override void reset()
        {
            m_curTime = m_totalTime;
            m_curLeftTimer = 0;
            m_disposed = false;
        }
    }
}