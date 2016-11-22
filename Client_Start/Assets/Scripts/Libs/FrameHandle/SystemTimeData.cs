using System;

namespace SDK.Lib
{
    public class SystemTimeData
    {
        protected long mPreTime;           // 上一次更新时的秒数
        protected long mCurTime;           // 正在获取的时间
        protected float mDeltaSec;         // 两帧之间的间隔

        public SystemTimeData()
        {
            this.mPreTime = 0;
            this.mCurTime = 0;
            this.mDeltaSec = 0.0f; 
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
            this.mCurTime = DateTime.Now.Ticks;
            if (this.mPreTime != 0f)     // 第一帧跳过，因为这一帧不好计算间隔
            {
                TimeSpan ts = new TimeSpan(this.mCurTime - this.mPreTime);
                this.mDeltaSec = (float)(ts.TotalSeconds);
            }
            else
            {
                this.mDeltaSec = 1 / 24;        // 每秒 24 帧
            }
            this.mPreTime = this.mCurTime;
        }
    }
}