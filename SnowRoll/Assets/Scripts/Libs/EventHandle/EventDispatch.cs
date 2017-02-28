using LuaInterface;

namespace SDK.Lib
{
    /**
     * @brief 事件分发，之分发一类事件，不同类型的事件使用不同的事件分发
     * @brief 注意，事件分发缺点就是，可能被调用的对象已经释放，但是没有清掉事件处理器，结果造成空指针
     */
    public class EventDispatch : DelayHandleMgrBase
    {
        protected int mEventId;
        protected MList<EventDispatchFunctionObject> mHandleList;
        protected int mUniqueId;       // 唯一 Id ，调试使用
        protected LuaCSBridgeDispatch mLuaCSBridgeDispatch;

        public EventDispatch(int eventId_ = 0)
        {
            this.mEventId = eventId_;
            this.mHandleList = new MList<EventDispatchFunctionObject>();
        }

        protected MList<EventDispatchFunctionObject> handleList
        {
            get
            {
                return this.mHandleList;
            }
        }

        public int uniqueId
        {
            get
            {
                return this.mUniqueId;
            }
            set
            {
                this.mUniqueId = value;
                this.mHandleList.uniqueId = this.mUniqueId;
            }
        }

        public LuaCSBridgeDispatch luaCSBridgeDispatch
        {
            get
            {
                return this.mLuaCSBridgeDispatch;
            }
            set
            {
                this.mLuaCSBridgeDispatch = value;
            }
        }

        override public void init()
        {

        }

        override public void dispose()
        {

        }

        public void addDispatch(EventDispatchFunctionObject dispatch)
        {
            this.addObject(dispatch);
        }

        public void removeDispatch(EventDispatchFunctionObject dispatch)
        {
            this.removeObject(dispatch);
        }

        // 相同的函数只能增加一次，Lua ，Python 这些语言不支持同时存在几个相同名字的函数，只支持参数可以赋值，因此不单独提供同一个名字不同参数的接口了，但是 java 不支持参数默认值，只能通过重载实现参数默认值，真是悲剧中的悲剧， eventId: 分发事件上层唯一 Id，这样一个事件处理函数可以根据 EventId 处理不同的事件
        virtual public void addEventHandle(ICalleeObject pThis, MAction<IDispatchObject> handle, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            if (null != pThis || null != handle || null != luaTable || null != luaFunction)
            {
                EventDispatchFunctionObject funcObject = new EventDispatchFunctionObject();

                if (null != handle)
                {
                    funcObject.setFuncObject(pThis, handle, eventId);
                }
                if(null != luaTable || null != luaFunction)
                {
                    funcObject.setLuaFunctor(luaTable, luaFunction, luaEventId);
                }

                this.addDispatch(funcObject);
            }
            else
            {
            
            }
        }

        public void removeEventHandle(ICalleeObject pThis, MAction<IDispatchObject> handle, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            int idx = 0;
            int elemLen = 0;
            elemLen = this.mHandleList.Count();

            while (idx < elemLen)
            {
                if (this.mHandleList[idx].isEqual(pThis, handle, eventId, luaTable, luaFunction, luaEventId))
                {
                    break;
                }

                idx += 1;
            }

            if (idx < this.mHandleList.Count())
            {
                this.removeDispatch(this.mHandleList[idx]);
            }
            else
            {
            
            }
        }

        override protected void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if (this.mLoopDepth.isInDepth())
            {
                base.addObject(delayObject, priority);
            }
            else
            {
                // 这个判断说明相同的函数只能加一次，但是如果不同资源使用相同的回调函数就会有问题，但是这个判断可以保证只添加一次函数，值得，因此不同资源需要不同回调函数
                this.mHandleList.Add(delayObject as EventDispatchFunctionObject);
            }
        }

        override protected void removeObject(IDelayHandleItem delayObject)
        {
            if (this.mLoopDepth.isInDepth())
            {
                base.removeObject(delayObject);
            }
            else
            {
                if (!this.mHandleList.Remove(delayObject as EventDispatchFunctionObject))
                {
                
                }
            }
        }

        virtual public void dispatchEvent(IDispatchObject dispatchObject)
        {
            //try
            //{
            this.mLoopDepth.incDepth();

            //foreach (EventDispatchFunctionObject handle in this.mHandleList.list())

            int idx = 0;
            int len = this.mHandleList.Count();
            EventDispatchFunctionObject handle = null;

            while (idx < len)
            {
                handle = this.mHandleList[idx];

                if (!handle.mIsClientDispose)
                {
                    handle.call(dispatchObject);
                }

                ++idx;
            }

            if (this.mLuaCSBridgeDispatch != null)
            {
                this.mLuaCSBridgeDispatch.handleGlobalEvent(this.mEventId, dispatchObject);
            }

            this.mLoopDepth.decDepth();
            //}
            //catch (Exception ex)
            //{
            //    Ctx.mInstance.mLogSys.catchLog(ex.ToString());
            //}
        }

        public void clearEventHandle()
        {
            if (this.mLoopDepth.isInDepth())
            {
                //foreach (EventDispatchFunctionObject item in this.mHandleList.list())
                int idx = 0;
                int len = this.mHandleList.Count();
                EventDispatchFunctionObject item = null;

                while (idx < len)
                {
                    item = this.mHandleList[idx];

                    this.removeDispatch(item);

                    ++idx;
                }
            }
            else
            {
                this.mHandleList.Clear();
            }
        }

        // 这个判断说明相同的函数只能加一次，但是如果不同资源使用相同的回调函数就会有问题，但是这个判断可以保证只添加一次函数，值得，因此不同资源需要不同回调函数
        public bool isExistEventHandle(ICalleeObject pThis, MAction<IDispatchObject> handle, uint eventId, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            bool bFinded = false;
            //foreach (EventDispatchFunctionObject item in this.mHandleList.list())
            int idx = 0;
            int len = this.mHandleList.Count();
            EventDispatchFunctionObject item = null;

            while (idx < len)
            {
                item = this.mHandleList[idx];

                if (item.isEqual(pThis, handle, eventId, luaTable, luaFunction, luaEventId))
                {
                    bFinded = true;
                    break;
                }

                ++idx;
            }

            return bFinded;
        }

        public void copyFrom(EventDispatch rhv)
        {
            //foreach(EventDispatchFunctionObject handle in rhv.handleList.list())
            int idx = 0;
            int len = this.mHandleList.Count();
            EventDispatchFunctionObject handle = null;

            while (idx < len)
            {
                handle = this.mHandleList[idx];

                this.mHandleList.Add(handle);

                ++idx;
            }
        }

        public bool hasEventHandle()
        {
            return this.mHandleList.Count() > 0;
        }

        public int getEventHandle()
        {
            return this.mHandleList.Count();
        }
    }
}