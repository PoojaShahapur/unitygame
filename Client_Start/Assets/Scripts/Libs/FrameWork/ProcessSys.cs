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
            Advance(Ctx.mInstance.mSystemTimeData.deltaSec);
        }

        public void Advance(float delta)
        {
            Ctx.mInstance.mSystemFrameData.nextFrame(delta);
            //Ctx.mInstance.mLuaSystem.Advance(delta);            // lua 脚本 Advance
            Ctx.mInstance.mTickMgr.Advance(delta);            // 心跳
            //Ctx.mInstance.mTimerMgr.Advance(delta);           // 定时器
            //Ctx.mInstance.mFrameTimerMgr.Advance(delta);           // 帧定时器
        }
    }
}