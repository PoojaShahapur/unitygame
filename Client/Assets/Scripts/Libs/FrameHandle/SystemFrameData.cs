using SDK.Lib;
namespace SDK.Lib
{
    public class SystemFrameData
    {
        protected uint m_totalFrameCount;       // 当前帧数
        protected uint m_curFrameCount;         // 当前帧数
        protected float m_curTime;          // 当前一秒内时间
        protected int m_fps;                // 帧率

        public void nextFrame(float delta)
        {
            ++m_totalFrameCount;
            ++m_curFrameCount;
            m_curTime += delta;

            if(m_curTime > 1.0f)
            {
                m_fps = (int)(m_curFrameCount / m_curTime);
                m_curFrameCount = 0;
                m_curTime = 0;

                //Ctx.m_instance.m_logSys.log(string.Format("当前帧率 {0}", m_fps));
            }
        }
    }
}