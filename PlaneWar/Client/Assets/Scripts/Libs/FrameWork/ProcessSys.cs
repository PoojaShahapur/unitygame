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
            Ctx.mInstance.mSystemTimeData.nextFrame();
            this.Advance(Ctx.mInstance.mSystemTimeData.deltaSec);
        }

        public void Advance(float delta)
        {
            Ctx.mInstance.mFrameCollideMgr.clear();
            Ctx.mInstance.mSystemFrameData.nextFrame(delta);
            Ctx.mInstance.mLuaSystem.advance(delta);        // lua 脚本 Advance
            Ctx.mInstance.mTickMgr.Advance(delta);            // 心跳
            Ctx.mInstance.mTimerMgr.Advance(delta);           // 定时器
            Ctx.mInstance.mFrameTimerMgr.Advance(delta);      // 帧定时器
        }

        public void ProcessNextFixedFrame()
        {
            this.FixedAdvance(Ctx.mInstance.mSystemTimeData.getFixedTimestep());
        }

        public void FixedAdvance(float delta)
        {
            Ctx.mInstance.mFixedTickMgr.Advance(delta);
        }
    }
}