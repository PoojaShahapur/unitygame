namespace SDK.Lib
{
    /**
     * @brief 单一对象同步
     */
    public class MCondition
    {
        protected MMutex m_pMMutex;
        protected MEvent m_pMEvent;
        protected bool m_canEnterWait;  // 是否可以进入等待

        public MCondition(string name)
        {
            m_pMMutex = new MMutex(false, name);
            m_pMEvent = new MEvent(false);
            m_canEnterWait = true;      // 允许进入等待状态
        }

        public bool canEnterWait
        {
            get
            {
                return m_canEnterWait;
            }
        }

        public void wait()
        {
            using (MLock mlock = new MLock(m_pMMutex))
            {
                if (m_canEnterWait)
                {
                    m_pMEvent.WaitOne();
                    m_pMEvent.Reset();      // 重置信号
                }
                else
                {
                    m_canEnterWait = true;
                }
            }
        }

        public void notifyAll()
        {
            using (MLock mlock = new MLock(m_pMMutex))
            {
                if (m_canEnterWait) // 如果 m_canEnterWait == false，必然不能进入等待
                {
                    m_canEnterWait = false;
                    m_pMEvent.Set();        // 唤醒线程
                }
            }
        }
    }
}