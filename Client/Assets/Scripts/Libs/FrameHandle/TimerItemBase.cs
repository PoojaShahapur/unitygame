using System;

namespace SDK.Lib
{
    /**
     * @brief 定时器，这个是不断增长的
     */
    public class TimerItemBase : IDelayHandleItem
    {
        public float m_internal = 1;        // 定时器间隔
        public float m_totalTime = 1;      // 总共定时器时间
        public float m_curTime = 0;        // 当前已经调用的定时器的时间
        public bool m_bInfineLoop = false;  // 是否是无限循环
        public float m_curLeftTimer = 0;    // 当前定时器剩余的次数
        public Action<TimerItemBase> m_timerDisp = null;       // 定时器分发
        public bool m_disposed = false;             // 是否已经被释放

        public virtual void OnTimer(float delta)
        {
            if (m_disposed)
            {
                return;
            }

            m_curTime += delta;
            m_curLeftTimer += delta;

            if (m_bInfineLoop)
            {
                checkAndDisp();
            }
            else
            {
                if (m_curTime >= m_totalTime)
                {
                    disposeAndDisp();
                }
                else
                {
                    checkAndDisp();
                }
            }
        }

        public virtual void disposeAndDisp()
        {
            m_disposed = true;
            if (m_timerDisp != null)
            {
                m_timerDisp(this);
            }
        }

        public virtual void checkAndDisp()
        {
            if (m_curLeftTimer >= m_internal)
            {
                m_curLeftTimer = m_curLeftTimer - m_internal;

                if (m_timerDisp != null)
                {
                    m_timerDisp(this);
                }
            }
        }

        public virtual void reset()
        {
            m_curTime = 0;
            m_curLeftTimer = 0;
            m_disposed = false;
        }

        public void setClientDispose()
        {

        }

        public bool getClientDispose()
        {
            return false;
        }
    }
}