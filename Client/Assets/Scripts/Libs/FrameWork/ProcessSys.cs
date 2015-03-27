using SDK.Common;
using System;

/**
 * @brief 系统循环
 */
namespace SDK.Lib
{
    public class ProcessSys
    {
        protected long m_preTime = 0;         // 上一次更新时的秒数
        protected long m_curTime = 0;          // 正在获取的时间
        protected float m_deltaSec = 0f;         // 两帧之间的间隔

        public ProcessSys()
        {

        }

        public void ProcessNextFrame()
        {
            m_curTime = DateTime.Now.Ticks;
            if (m_preTime != 0f)     // 第一帧跳过，因为这一帧不好计算间隔
            {
                TimeSpan ts = new TimeSpan(m_curTime - m_preTime);
                m_deltaSec = (float)(ts.TotalSeconds);
                Advance(m_deltaSec);
            }
            m_preTime = m_curTime;
        }

        public void Advance(float delta)
        {
            Ctx.m_instance.m_tickMgr.Advance(delta);            // 心跳
            Ctx.m_instance.m_timerMgr.Advance(delta);           // 定时器
        }
    }
}