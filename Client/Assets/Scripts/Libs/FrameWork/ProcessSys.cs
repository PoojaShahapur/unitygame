using SDK.Common;
using System;

/**
 * @brief 系统循环
 */
namespace SDK.Lib
{
    public class ProcessSys
    {
        public ProcessSys()
        {

        }

        public void ProcessNextFrame()
        {
            Ctx.m_instance.m_systemTimeData.nextFrame();
            Advance(Ctx.m_instance.m_systemTimeData.deltaSec);
        }

        public void Advance(float delta)
        {
            Ctx.m_instance.m_systemFrameData.nextFrame(delta);
            Ctx.m_instance.m_tickMgr.Advance(delta);            // 心跳
            Ctx.m_instance.m_timerMgr.Advance(delta);           // 定时器
            Ctx.m_instance.m_frameTimerMgr.Advance(delta);           // 帧定时器
        }
    }
}