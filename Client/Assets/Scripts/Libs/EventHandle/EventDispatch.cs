using SDK.Common;
using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 事件分发，之分发一类事件，不同类型的事件使用不同的事件分发
     */
    public class EventDispatch
    {
        protected List<Action<IDispatchObject>> m_handleList = new List<Action<IDispatchObject>>();

        protected List<Action<IDispatchObject>> handleList
        {
            get
            {
                return m_handleList;
            }
        }

        public void addEventHandle(Action<IDispatchObject> handle)
        {
            if(m_handleList.IndexOf(handle) == -1)
            {
                m_handleList.Add(handle);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("Event Handle already exist");
            }
        }

        public void removeEventHandle(Action<IDispatchObject> handle)
        {
            if (!m_handleList.Remove(handle))
            {
                Ctx.m_instance.m_logSys.log("Event Handle not exist");
            }
        }

        public void dispatchEvent(IDispatchObject dispatchObject)
        {
            foreach(var handle in m_handleList)
            {
                handle(dispatchObject);
            }
        }

        public void clearEventHandle()
        {
            m_handleList.Clear();
        }

        public void copyFrom(EventDispatch rhv)
        {
            foreach(var handle in rhv.handleList)
            {
                m_handleList.Add(handle);
            }
        }
    }
}