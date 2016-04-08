using LuaInterface;
using System;

namespace SDK.Lib
{
    /**
     * @brief 事件分发，之分发一类事件，不同类型的事件使用不同的事件分发
     * @brief 注意，事件分发缺点就是，可能被调用的对象已经释放，但是没有清掉事件处理器，结果造成空指针
     */
    public class EventDispatch : DelayHandleMgrBase
    {
        protected int m_eventId;
        protected MList<EventDispatchFunctionObject> m_handleList;
        protected int m_uniqueId;       // 唯一 Id ，调试使用
        protected LuaCSBridgeDispatch m_luaCSBridgeDispatch;

        public EventDispatch(int eventId_ = 0)
        {
            m_eventId = eventId_;
            m_handleList = new MList<EventDispatchFunctionObject>();
        }

        protected MList<EventDispatchFunctionObject> handleList
        {
            get
            {
                return m_handleList;
            }
        }

        public int uniqueId
        {
            get
            {
                return m_uniqueId;
            }
            set
            {
                m_uniqueId = value;
                m_handleList.uniqueId = m_uniqueId;
            }
        }

        public LuaCSBridgeDispatch luaCSBridgeDispatch
        {
            get
            {
                return m_luaCSBridgeDispatch;
            }
            set
            {
                m_luaCSBridgeDispatch = value;
            }
        }

        // 相同的函数只能增加一次
        virtual public void addEventHandle(Action<IDispatchObject> handle, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            if (null != handle)
            {
                EventDispatchFunctionObject funcObject = new EventDispatchFunctionObject();
                funcObject.setFuncObject(handle);
                addObject(funcObject);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("Event Handle is null");
            }
        }

        override public void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if (bInDepth())
            {
                base.addObject(delayObject, priority);
            }
            else
            {
                // 这个判断说明相同的函数只能加一次，但是如果不同资源使用相同的回调函数就会有问题，但是这个判断可以保证只添加一次函数，值得，因此不同资源需要不同回调函数
                m_handleList.Add(delayObject as EventDispatchFunctionObject);
            }
        }

        public void removeEventHandle(Action<IDispatchObject> handle)
        {
            int idx = 0;
            for (idx = 0; idx < m_handleList.Count(); ++idx)
            {
                if (m_handleList[idx].isEqual(handle))
                {
                    break;
                }
            }
            if (idx < m_handleList.Count())
            {
                delObject(m_handleList[idx]);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("Event Handle not exist");
            }
        }

        override public void delObject(IDelayHandleItem delayObject)
        {
            if (bInDepth())
            {
                base.delObject(delayObject);
            }
            else
            {
                if (!m_handleList.Remove(delayObject as EventDispatchFunctionObject))
                {
                    Ctx.m_instance.m_logSys.log("Event Handle not exist");
                }
            }
        }

        virtual public void dispatchEvent(IDispatchObject dispatchObject)
        {
            //try
            //{
                incDepth();

                foreach (var handle in m_handleList.list())
                {
                    if (!handle.m_bClientDispose)
                    {
                        handle.call(dispatchObject);
                    }
                }

                if (m_luaCSBridgeDispatch != null)
                {
                    m_luaCSBridgeDispatch.handleGlobalEvent(m_eventId, dispatchObject);
                }

                decDepth();
            //}
            //catch (Exception ex)
            //{
            //    Ctx.m_instance.m_logSys.catchLog(ex.ToString());
            //}
        }

        public void clearEventHandle()
        {
            if (bInDepth())
            {
                foreach (var item in m_handleList.list())
                {
                    delObject(item);
                }
            }
            else
            {
                m_handleList.Clear();
            }
        }

        // 这个判断说明相同的函数只能加一次，但是如果不同资源使用相同的回调函数就会有问题，但是这个判断可以保证只添加一次函数，值得，因此不同资源需要不同回调函数
        public bool existEventHandle(Action<IDispatchObject> handle, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            bool bFinded = false;
            foreach (var item in m_handleList.list())
            {
                if (item.isEqual(handle, luaTable, luaFunction))
                {
                    bFinded = true;
                    break;
                }
            }

            return bFinded;
        }

        public void copyFrom(EventDispatch rhv)
        {
            foreach(var handle in rhv.handleList.list())
            {
                m_handleList.Add(handle);
            }
        }

        public int getHandleCount()
        {
            return m_handleList.Count();
        }
    }
}