namespace SDK.Lib
{
    /**
     * @brief 单一对象同步
     */
    public class MCondition
    {
        protected MMutex mMutex;
        protected MEvent mEvent;
        protected bool mCanEnterWait;  // 是否可以进入等待

        public MCondition(string name)
        {
            mMutex = new MMutex(false, name);
            mEvent = new MEvent(false);
            mCanEnterWait = true;      // 允许进入等待状态
        }

        public bool canEnterWait
        {
            get
            {
                return mCanEnterWait;
            }
        }

        public void wait()
        {
            //using (MLock mlock = new MLock(mMutex))
            //{
                mMutex.WaitOne();
                if (mCanEnterWait)
                {
                    mMutex.ReleaseMutex();   // 这个地方需要释放锁，否则 notifyAll 进不来
                    mEvent.WaitOne();
                    mEvent.Reset();      // 重置信号
                }
                else
                {
                    mCanEnterWait = true;
                    mMutex.ReleaseMutex();
                }
            //}
        }

        public void notifyAll()
        {
            using (MLock mlock = new MLock(mMutex))
            {
                if (mCanEnterWait) // 如果 mCanEnterWait == false，必然不能进入等待
                {
                    mCanEnterWait = false;
                    mEvent.Set();        // 唤醒线程
                }
            }
        }
    }
}