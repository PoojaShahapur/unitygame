using SDK.Common;
using System;

namespace SDK.Lib
{
    /**
     * @brief 定时器，这个是不断增长的
     */
    public class TimerItemBase : IDelayHandleItem
    {
        public float m_internal = 1;        // 定时器间隔
        public float m_totalCount = 1;      // 总共定时器时间
        public float m_curCount = 0;        // 当前已经调用的定时器的时间
        public bool m_bInfineLoop = false;  // 是否是无限循环
        public float m_curLeftTimer = 0;    // 当前定时器剩余的次数
        public Action<TimerItemBase> m_timerDisp = null;       // 定时器分发
        public bool m_disposed = false;             // 是否已经被释放

        protected float m_deltaTotal = 0;

        public virtual bool OnTimer(float delta)
        {
            if (m_disposed)
            {
                return true;
            }

            if (m_bInfineLoop)
            {
                if (m_curLeftTimer + delta >= m_internal)
                {
                    m_curLeftTimer = m_curLeftTimer + delta - m_internal;

                    if (m_timerDisp != null)
                    {
                        m_timerDisp(this);
                    }
                }
                else
                {
                    m_curLeftTimer += delta;
                }
            }
            else
            {
                if (m_curLeftTimer + delta >= m_internal)
                {
                    m_curLeftTimer = m_curLeftTimer + delta - m_internal;

                    bool ret = checkEnd(m_internal - m_deltaTotal);
                    m_deltaTotal = 0;

                    if (m_timerDisp != null)
                    {
                        m_timerDisp(this);
                    }

                    return ret;
                }
                else
                {
                    m_curLeftTimer += delta;
                    m_deltaTotal += delta;
                    if(checkEnd(delta))
                    {
                        if (m_timerDisp != null)
                        {
                            m_timerDisp(this);
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        protected virtual bool checkEnd(float delta)
        {
            m_curCount += delta;
            if(m_curCount >= m_totalCount)
            {
                m_disposed = true;
                return true;
            }

            return false;
        }

        public virtual void reset()
        {
            m_disposed = false;
            m_curCount = 0;
            m_curLeftTimer = 0;
            m_disposed = false;
        }
    }
}