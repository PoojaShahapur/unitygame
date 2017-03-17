using System;
using System.Threading;

namespace SDK.Lib
{
    /**
     * @brief 场景线程
     */
    public class MSceneThread : MThread
    {
        protected MSceneManager mSceneManager;
        protected MFunc<MSceneThread, long> mFunc;
        protected int mIndex;

        public MSceneThread(MFunc<MSceneThread, long> func, int index, MSceneManager sceneMgr)
            : base(null, null)
        {
            mFunc = func;
            mIndex = index;
            mSceneManager = sceneMgr;
        }

        public int getThreadIdx()
        {
            return mIndex;
        }

        public MSceneManager getSceneManager()
        {
            return mSceneManager;
        }

        /**
         *brief 线程回调函数
         */
        override public void threadHandle()
        {
            base.threadHandle();

            while (!mIsExitFlag)
            {
                mFunc(this);
            }
        }
    }
}