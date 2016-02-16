using System;

namespace SDK.Lib
{
    public class TimerFunctionObject
    {
        public Action<TimerItemBase> m_handle;

        public TimerFunctionObject()
        {
            m_handle = null;
        }

        public void setFuncObject(Action<TimerItemBase> handle)
        {
            m_handle = handle;
        }

        public bool isValid()
        {
            return m_handle != null;
        }

        public void call(TimerItemBase dispObj)
        {
            if (null != m_handle)
            {
                m_handle(dispObj);
            }
        }
    }
}