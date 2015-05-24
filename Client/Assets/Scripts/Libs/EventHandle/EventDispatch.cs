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
        protected bool m_bInLoop;       // 是否是在循环遍历中

        public EventDispatch()
        {
            m_bInLoop = false;
        }

        protected List<Action<IDispatchObject>> handleList
        {
            get
            {
                return m_handleList;
            }
        }

        public void addEventHandle(Action<IDispatchObject> handle)
        {
            if (handle != null)
            {
                if (m_handleList.IndexOf(handle) == -1)     // 这个判断说明相同的函数只能加一次，但是如果不同资源使用相同的回调函数就会有问题，但是这个判断可以保证只添加一次函数，值得，因此不同资源需要不同回调函数
                {
                    m_handleList.Add(handle);
                }
                else
                {
                    Ctx.m_instance.m_logSys.log("Event Handle already exist");
                }
            }
            else
            {
                Ctx.m_instance.m_logSys.log("Event Handle is null");
            }
        }

        public void removeEventHandle(Action<IDispatchObject> handle)
        {
            if (!m_bInLoop)
            {
                if (!m_handleList.Remove(handle))
                {
                    Ctx.m_instance.m_logSys.log("Event Handle not exist");
                }
            }
            else
            {
                Ctx.m_instance.m_logSys.log("looping cannot delete element");
            }
        }

        public void dispatchEvent(IDispatchObject dispatchObject)
        {
            m_bInLoop = true;
            foreach(var handle in m_handleList)
            {
                handle(dispatchObject);
            }
            m_bInLoop = false;

            clearEventHandle();
        }

        public void clearEventHandle()
        {
            if (!m_bInLoop)
            {
                m_handleList.Clear();
            }
            else
            {
                Ctx.m_instance.m_logSys.log("looping cannot delete element");
            }
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