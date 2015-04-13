namespace SDK.Lib
{
    /**
     * @brief 倒计时定时器
     */
    public class DaoJiShiTimer : TimerItemBase
    {
        protected override bool checkEnd(float delta)
        {
            m_curCount -= delta;
            if (m_curCount <= 0)            // 保证肯定退出
            {
                m_disposed = true;
                return true;
            }

            return false;
        }

        public override void reset()
        {
            m_curCount = m_totalCount;
            m_curLeftTimer = 0;
            m_disposed = false;
        }
    }
}