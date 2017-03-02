using System;
using System.Threading;

namespace SDK.Lib
{
    /**
     *@brief 基本的线程
     */
    public class MThread
    {
        protected static int msMainThreadID;           // 主线程 id
        protected int mCurThreadID;                    // 当前线程的 id

        // 数据区域
        protected Thread mThread;
        protected Action<object> mCb;
        protected object mParam;           // 参数数据
        protected bool mIsExitFlag;           // 退出标志

        public MThread(Action<object> func, object param)
        {
            mCb = func;
            mParam = param;
        }

        public bool ExitFlag
        {
            set
            {
                mIsExitFlag = value;
            }
        }

        public Action<object> cb
        {
            set
            {
                mCb = value;
            }
        }

        public object param
        {
            set
            {
                mParam = value;
            }
        }

        // 函数区域
        /**
         *@brief 开启一个线程
         */
        public void start()
        {
            mThread = new Thread(new ThreadStart(threadHandle));
            mThread.Priority = ThreadPriority.Lowest;
            //mThread.IsBackground = true;             // 继续作为前台线程
            mThread.Start();
        }

        public void join()
        {
            //mThread.Interrupt();           // 直接线程终止
            mThread.Join();
        }

        /**
         *@brief 线程回调函数
         */
        virtual public void threadHandle()
        {
            getCurThreadID();

            if(mCb != null)
            {
                mCb(mParam);
            }
        }

        protected void getCurThreadID()
        {
            mCurThreadID = Thread.CurrentThread.ManagedThreadId;       // 当前线程的 ID
        }

        public bool isCurThread(int threadID)
        {
            return (mCurThreadID == threadID);
        }

        static public void getMainThreadID()
        {
            msMainThreadID = Thread.CurrentThread.ManagedThreadId;
        }

        static public bool isMainThread()
        {
            return (msMainThreadID == Thread.CurrentThread.ManagedThreadId);
        }

        static public void needMainThread()
        {
            if (!isMainThread())
            {
                Ctx.mInstance.mLogSys.error("error: log out in other thread");
                throw new Exception("cannot call function in thread");
            }
        }
    }
}