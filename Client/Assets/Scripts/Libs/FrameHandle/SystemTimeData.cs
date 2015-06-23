using System;

namespace SDK.Lib
{
    public class SystemTimeData
    {
        protected long m_preTime;           // 上一次更新时的秒数
        protected long m_curTime;           // 正在获取的时间
        protected float m_deltaSec;         // 两帧之间的间隔

        public SystemTimeData()
        {
            m_preTime = 0;
            m_curTime = 0;
            m_deltaSec = 0.0f; 
        }

        public float deltaSec
        {
            get
            {
                return m_deltaSec;
            }
            set
            {
                m_deltaSec = value;
            }
        }

        public long curTime
        {
            get
            {
                return m_curTime;
            }
            set
            {
                m_curTime = value;
            }
        }

        public void nextFrame()
        {
            m_curTime = DateTime.Now.Ticks;
            if (m_preTime != 0f)     // 第一帧跳过，因为这一帧不好计算间隔
            {
                TimeSpan ts = new TimeSpan(m_curTime - m_preTime);
                m_deltaSec = (float)(ts.TotalSeconds);
            }
            else
            {
                m_deltaSec = 1 / 24;        // 每秒 24 帧
            }
            m_preTime = m_curTime;
        }
    }
}