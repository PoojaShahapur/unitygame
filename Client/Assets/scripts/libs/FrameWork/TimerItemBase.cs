using SDK.Common;
using System;

namespace SDK.Lib
{
    /**
     * @brief 定时器
     */
    public class TimerItemBase : IDelayHandleItem
    {
        public float m_internal = 1;        // 定时器间隔
        public int m_totalCount = 1;        // 总共回调次数
        public int m_curCount = 0;          // 当前已经调用的次数
        public float m_curLeftTimer = 0;    // 当前定时器剩余的次数
        public Action<TimerItemBase> m_timerDisp = null;       // 定时器分发
        public bool m_disposed = false;             // 是否已经被释放

        public bool OnTimer(float delta)
        {
            if (m_disposed)
            {
                return true;
            }
            if (m_curLeftTimer + delta >= m_internal)
            {
                m_curLeftTimer = m_curLeftTimer + delta - m_internal;
                if(m_timerDisp != null)
                {
                    m_timerDisp(this);
                }

                ++m_curCount;
                if(m_curCount == m_totalCount)
                {
                    m_disposed = true;
                    return true;
                }
            }
            else
            {
                m_curLeftTimer += delta;
            }

            return false;
        }
    }
}