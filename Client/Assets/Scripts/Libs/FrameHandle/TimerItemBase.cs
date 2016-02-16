using System;

namespace SDK.Lib
{
    /**
     * @brief 定时器，这个是不断增长的
     */
    public class TimerItemBase : IDelayHandleItem, IDispatchObject
    {
        public float m_internal;        // 定时器间隔
        public float m_totalTime;       // 总共定时器时间
        public float m_curTime;         // 当前已经调用的定时器的时间
        public bool m_bInfineLoop;      // 是否是无限循环
        public float m_curLeftTimer;    // 当前定时器剩余的次数
        public TimerFunctionObject m_timerDisp;       // 定时器分发
        public bool m_disposed;             // 是否已经被释放

        public TimerItemBase()
        {
            m_internal = 1;
            m_totalTime = 1;
            m_curTime = 0;
            m_bInfineLoop = false;
            m_curLeftTimer = 0;
            m_timerDisp = new TimerFunctionObject();
            m_disposed = false;
        }

        public void setFuncObject(Action<TimerItemBase> handle)
        {
            m_timerDisp.setFuncObject(handle);
        }

        // 在调用回调函数之前处理
        protected virtual void preCallBack()
        {

        }

        public virtual void OnTimer(float delta)
        {
            if (m_disposed)
            {
                return;
            }

            m_curTime += delta;
            m_curLeftTimer += delta;

            if (m_bInfineLoop)
            {
                checkAndDisp();
            }
            else
            {
                if (m_curTime >= m_totalTime)
                {
                    disposeAndDisp();
                }
                else
                {
                    checkAndDisp();
                }
            }
        }

        public virtual void disposeAndDisp()
        {
            m_disposed = true;
            this.preCallBack();

            if (m_timerDisp.isValid())
            {
                m_timerDisp.call(this);
            }
        }

        public virtual void checkAndDisp()
        {
            if (m_curLeftTimer >= m_internal)
            {
                m_curLeftTimer = m_curLeftTimer - m_internal;
                this.preCallBack();

                if (m_timerDisp.isValid())
                {
                    m_timerDisp.call(this);
                }
            }
        }

        public virtual void reset()
        {
            m_curTime = 0;
            m_curLeftTimer = 0;
            m_disposed = false;
        }

        public void setClientDispose()
        {

        }

        public bool getClientDispose()
        {
            return false;
        }
    }
}