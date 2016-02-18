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
        public float m_curRunTime;      // 当前定时器运行的时间
        public float m_curCallTime;     // 当前定时器已经调用的时间
        public bool m_bInfineLoop;      // 是否是无限循环
        public float m_intervalLeftTime;     // 定时器调用间隔剩余时间
        public TimerFunctionObject m_timerDisp;       // 定时器分发
        public bool m_disposed;             // 是否已经被释放
        public bool m_bContinuous;          // 是否是连续的定时器

        public TimerItemBase()
        {
            m_internal = 1;
            m_totalTime = 1;
            m_curRunTime = 0;
            m_curCallTime = 0;
            m_bInfineLoop = false;
            m_intervalLeftTime = 0;
            m_timerDisp = new TimerFunctionObject();
            m_disposed = false;
            m_bContinuous = false;
        }

        public void setFuncObject(Action<TimerItemBase> handle)
        {
            m_timerDisp.setFuncObject(handle);
        }

        virtual public void setTotalTime(float value)
        {
            this.m_totalTime = value;
        }

        virtual public float getRunTime()
        {
            return this.m_curRunTime;
        }

        virtual public float getCallTime()
        {
            return this.m_curCallTime;
        }

        // 在调用回调函数之前处理
        protected virtual void onPreCallBack()
        {

        }

        public virtual void OnTimer(float delta)
        {
            if (m_disposed)
            {
                return;
            }

            m_curRunTime += delta;
            if(m_curRunTime > m_totalTime)
            {
                m_curRunTime = m_totalTime;
            }
            m_intervalLeftTime += delta;

            if (m_bInfineLoop)
            {
                checkAndDisp();
            }
            else
            {
                if (m_curRunTime >= m_totalTime)
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
            m_curCallTime = m_totalTime;
            this.onPreCallBack();

            if (m_timerDisp.isValid())
            {
                m_timerDisp.call(this);
            }
        }

        public virtual void checkAndDisp()
        {
            if(m_bContinuous)
            {
                continueCheckAndDisp();
            }
            else
            {
                discontinueCheckAndDisp();
            }
        }

        // 连续的定时器
        protected void continueCheckAndDisp()
        {
            while (m_intervalLeftTime >= m_internal)
            {
                // 这个地方 m_curCallTime 肯定会小于 m_totalTime，因为在调用这个函数的外部已经进行了判断
                m_curCallTime = m_curCallTime + m_internal;
                m_intervalLeftTime = m_intervalLeftTime - m_internal;
                this.onPreCallBack();

                if (m_timerDisp.isValid())
                {
                    m_timerDisp.call(this);
                }
            }
        }

        // 不连续的定时器
        protected void discontinueCheckAndDisp()
        {
            if (m_intervalLeftTime >= m_internal)
            {
                // 这个地方 m_curCallTime 肯定会小于 m_totalTime，因为在调用这个函数的外部已经进行了判断
                m_curCallTime = m_curCallTime + (((int)(m_intervalLeftTime / m_internal)) * m_internal);
                m_intervalLeftTime = m_intervalLeftTime % m_internal;   // 只保留余数
                this.onPreCallBack();

                if (m_timerDisp.isValid())
                {
                    m_timerDisp.call(this);
                }
            }
        }

        public virtual void reset()
        {
            m_curRunTime = 0;
            m_curCallTime = 0;
            m_intervalLeftTime = 0;
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