using System;

namespace SDK.Lib
{
    public class SystemTimeData
    {
        protected long mPreTime;           // 上一次更新时的秒数
        protected long mCurTime;           // 正在获取的时间
        protected float mDeltaSec;         // 两帧之间的间隔
        protected bool mIsFixFrameRate;     // 固定帧率
        protected float mFixFrameRate;       // 固定帧率
        protected float mScale;             // delta 缩放

        // Edit->Project Setting->time
        protected float mFixedTimestep;

        public SystemTimeData()
        {
            this.mPreTime = 0;
            this.mCurTime = 0;
            this.mDeltaSec = 0.0f;
            this.mIsFixFrameRate = false;
            this.mFixFrameRate = 0.0417f;       //  1 / 24;
            this.mScale = 1;

            this.mFixedTimestep = 0.02f;
        }

        public float deltaSec
        {
            get
            {
                return this.mDeltaSec;
            }
            set
            {
                this.mDeltaSec = value;
            }
        }

        public float getFixedTimestep()
        {
            if (Ctx.mInstance.mCfg.mIsActorMoveUseFixUpdate)
            {
                return this.mFixedTimestep;
            }
            else
            {
                return this.mDeltaSec;
            }
        }

        public long curTime
        {
            get
            {
                return this.mCurTime;
            }
            set
            {
                this.mCurTime = value;
            }
        }

        public void nextFrame()
        {
            this.mPreTime = this.mCurTime;
            this.mCurTime = DateTime.Now.Ticks;

            if (mIsFixFrameRate)
            {
                this.mDeltaSec = this.mFixFrameRate;        // 每秒 24 帧
            }
            else
            {
                if (this.mPreTime != 0f)     // 第一帧跳过，因为这一帧不好计算间隔
                {
                    TimeSpan ts = new TimeSpan(this.mCurTime - this.mPreTime);
                    this.mDeltaSec = (float)(ts.TotalSeconds);
                }
                else
                {
                    this.mDeltaSec = this.mFixFrameRate;        // 每秒 24 帧
                }
            }

            this.mDeltaSec *= this.mScale;
        }
    }
}