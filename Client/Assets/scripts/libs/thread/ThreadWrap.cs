using SDK.Common;
using System;
using System.Threading;

namespace SDK.Lib
{
    /**
     *@brief 基本的线程 wrap
     */
    public class ThreadWrap
    {
        // 数据区域
        protected Thread m_thread;
        protected Action<object> m_cb;
        protected object m_param;           // 参数数据
        protected bool m_ExitFlag;           // 退出标志

        public ThreadWrap(Action<object> func, object param)
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
            m_thread.Start();
        }

        /**
         *@brief 线程回调函数
         */
        virtual public void threadHandle()
        {
            if(m_cb != null)
            {
                m_cb(m_param);
            }
        }
    }
}