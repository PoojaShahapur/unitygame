using SDK.Common;
using System;

/**
 * @brief 系统循环
 */
namespace SDK.Lib
{
    class ProcessSys : IProcessSys
    {
        protected float m_LastSec = 0f;         // 上一次更新时的秒数
        protected float m_curSec = 0f;          // 正在获取的时间
        protected float mDeltaSec = 0f;         // 两帧之间的间隔

        public ProcessSys()
        {

        }

        public void ProcessNextFrame()
        {
            m_curSec = DateTime.Now.Second;
            if(m_LastSec != 0f)     // 第一帧跳过，因为这一帧不好计算间隔
            {
                mDeltaSec = m_curSec - m_LastSec;
                Advance(mDeltaSec);
            }
            m_LastSec = m_curSec;
        }

        public void Advance(float delta)
        {
            Ctx.m_instance.m_TickMgr.Advance(delta);            // 心跳
            Ctx.m_instance.m_TimerMgr.Advance(delta);            // 定时器
        }
    }
}
