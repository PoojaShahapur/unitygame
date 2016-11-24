namespace SDK.Lib
{
    public class SystemFrameData
    {
        protected uint mTotalFrameCount;       // 当前帧数
        protected uint mCurFrameCount;         // 当前帧数
        protected float mCurTime;          // 当前一秒内时间
        protected int mFps;                // 帧率

        public void nextFrame(float delta)
        {
            ++this.mTotalFrameCount;
            ++this.mCurFrameCount;
            this.mCurTime += delta;

            if(this.mCurTime > 1.0f)
            {
                this.mFps = (int)(this.mCurFrameCount / this.mCurTime);
                this.mCurFrameCount = 0;
                this.mCurTime = 0;

                //Ctx.mInstance.mLogSys.log(string.Format("Current fps {0}", m_fps));
            }
        }
    }
}