namespace SDK.Lib
{
    public class MBarrier
    {
        protected uint mNumThreads;
        protected uint mIndex;
        protected long mLockCount;
        protected MEvent[] mSemaphores;
        protected MMutex mMutex;
        //protected bool mIsReset;

        public MBarrier(uint threadCount )
        {
            mNumThreads = threadCount;
            mIndex = 0;
            mLockCount = 0;

            mSemaphores = new MEvent[2];
            for (uint i = 0; i < 2; ++i)
            {
                mSemaphores[i] = new MEvent(false);
            }

            mMutex = new MMutex(false, "MBarrierMutex");
            //mIsReset = false;
        }

        public void disopse()
        {
            for (uint i = 0; i < 2; ++i)
            {
                mSemaphores[i].Set();
            }
        }

        public void sync()
        {
            Ctx.m_instance.m_logSys.log("10000  MBarrier sync");
            Ctx.m_instance.m_logSys.log("10000  MBarrier mMutex WaitOne");
            mMutex.WaitOne();
            uint idx = mIndex;

            long oldLockCount = mLockCount;
            mLockCount += 1;

            //if(!mIsReset)
            //{
            //    int preIndex = (int)((idx + 1) % 2);
            //    mSemaphores[preIndex].Reset();
            //    mIsReset = true;
            //}

            if (oldLockCount != mNumThreads - 1)
            {
                Ctx.m_instance.m_logSys.log("10000  MBarrier mSemaphores WaitOne");
                mMutex.ReleaseMutex();
                mSemaphores[idx].WaitOne();
            }
            else
            {
                mIndex = (idx + 1) % 2;
                mLockCount = 0;
                if (mNumThreads > 1)
                {
                    Ctx.m_instance.m_logSys.log("10000  MBarrier mSemaphores Set");
                    mMutex.ReleaseMutex();
                    // 线程一定要都唤醒后再设置同步对象无信号
                    mSemaphores[mIndex].Reset();
                    mSemaphores[idx].Set();
                    //mIsReset = false;
                }
            }
        }
    }
}