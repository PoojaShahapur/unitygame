using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class TimerList
    {
        public float m_CVDelta;             // 这个是常量
        public float m_Delta;               // 间隔的时间
        public List<ITimer> m_TimerList;

        public TimerList()
        {
            m_TimerList = new List<ITimer>();
        }

        public void OnTimer(float delta)
        {
            m_Delta -= delta;
            m_Delta = m_CVDelta;
            if(m_Delta <= 0)
            {
                foreach(ITimer timer in m_TimerList)
                {
                    timer.OnTimer(m_CVDelta);
                }
            }
        }
    }
}