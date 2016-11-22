using System;
using System.Threading;

namespace SDK.Lib
{
    /**
     *@brief 基本的线程
     */
    public class MThread
    {
        protected static int m_sMainThreadID;           // 主线程 id
        protected int m_curThreadID;                    // 当前线程的 id

        // 数据区域
        protected Thread m_thread;
        protected Action<object> m_cb;
        protected object m_param;           // 参数数据
        protected bool m_ExitFlag;           // 退出标志

        public MThread(Action<object> func, object param)
        {
            m_cb = func;
            m_param = param;
        }

        public bool ExitFlag
        {
            set
            {
                m_ExitFlag = value;
            }
        }

        public Action<object> cb
        {
            set
            {
                m_cb = value;
            }
        }

        public object param
        {
            set
            {
                m_param = value;
            }
        }

        // 函数区域
        /**
         *@brief 开启一个线程
         */
        public void start()
        {
            m_thread = new Thread(new ThreadStart(threadHandle));
            m_thread.Priority = ThreadPriority.Lowest;
            //m_thread.IsBackground = true;             // 继续作为前台线程
            m_thread.Start();
        }

        public void join()
        {
            //m_thread.Interrupt();           // 直接线程终止
            m_thread.Join();
        }

        /**
         *@brief 线程回调函数
         */
        virtual public void threadHandle()
        {
            getCurThreadID();

            if(m_cb != null)
            {
                m_cb(m_param);
            }
        }

        protected void getCurThreadID()
        {
            m_curThreadID = Thread.CurrentThread.ManagedThreadId;       // 当前线程的 ID
        }

        public bool isCurThread(int threadID)
        {
            return (m_curThreadID == threadID);
        }

        static public void getMainThreadID()
        {
            m_sMainThreadID = Thread.CurrentThread.ManagedThreadId;
        }

        static public bool isMainThread()
        {
            return (m_sMainThreadID == Thread.CurrentThread.ManagedThreadId);
        }

        static public void needMainThread()
        {
            if (!isMainThread())
            {
                Ctx.mInstance.mLogSys.error("error: log 输出在另外一个线程");
                throw new Exception("cannot call function in thread");
            }
        }
    }
}