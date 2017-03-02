using LuaInterface;
using System;

namespace SDK.Lib
{
    public class EventDispatchFunctionObject : IDelayHandleItem
    {
        public bool mIsClientDispose;       // 是否释放了资源
        public ICalleeObject mThis;
        public MAction<IDispatchObject> mHandle;
        public uint mEventId;   // 事件唯一 Id

        protected LuaCSDispatchFunctionObject mLuaCSDispatchFunctionObject;

        public EventDispatchFunctionObject()
        {
            this.mIsClientDispose = false;
        }

        public LuaCSDispatchFunctionObject luaCSDispatchFunctionObject
        {
            get
            {
                return this.mLuaCSDispatchFunctionObject;
            }
            set
            {
                this.mLuaCSDispatchFunctionObject = value;
            }
        }

        public void setFuncObject(ICalleeObject pThis, MAction<IDispatchObject> function, uint eventId = 0)
        {
            this.mThis = pThis;
            this.mHandle = function;
            this.mEventId = eventId;
        }

        public void setLuaTable(LuaTable luaTable)
        {
            if(this.mLuaCSDispatchFunctionObject == null)
            {
                this.mLuaCSDispatchFunctionObject = new LuaCSDispatchFunctionObject();
            }

            this.mLuaCSDispatchFunctionObject.setTable(luaTable);
        }

        public void setLuaFunction(LuaFunction function)
        {
            if(this.mLuaCSDispatchFunctionObject == null)
            {
                this.mLuaCSDispatchFunctionObject = new LuaCSDispatchFunctionObject();
            }

            this.mLuaCSDispatchFunctionObject.setFunction(function);
        }

        public void setLuaFunctor(LuaTable luaTable, LuaFunction function, uint eventId = 0)
        {
            if(this.mLuaCSDispatchFunctionObject == null)
            {
                this.mLuaCSDispatchFunctionObject = new LuaCSDispatchFunctionObject();
            }

            this.mLuaCSDispatchFunctionObject.setTable(luaTable);
            this.mLuaCSDispatchFunctionObject.setFunction(function);
            this.mLuaCSDispatchFunctionObject.setEventId(eventId);
        }

        public bool isValid()
        {
            return this.mThis != null || this.mHandle != null || (this.mLuaCSDispatchFunctionObject != null && this.mLuaCSDispatchFunctionObject.isValid());
        }

        public bool isEventIdEqual(uint eventId)
        {
            return this.mEventId == eventId;
        }

        public bool isEqual(ICalleeObject pThis, MAction<IDispatchObject> handle, uint eventId, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            bool ret = false;

            if(pThis != null)
            {
                ret = UtilApi.isAddressEqual(this.mThis, pThis);

                if (!ret)
                {
                    return ret;
                }
            }

            if (handle != null)
            {
                //ret = UtilApi.isAddressEqual(this.mHandle, handle);
                ret = UtilApi.isDelegateEqual(ref this.mHandle, ref handle);

                if (!ret)
                {
                    return ret;
                }
            }

            ret = this.isEventIdEqual(eventId);

            if (!ret)
            {
                return ret;
            }

            if (null != luaTable && null != this.mLuaCSDispatchFunctionObject)
            {
                ret = this.mLuaCSDispatchFunctionObject.isTableEqual(luaTable);

                if(!ret)
                {
                    return ret;
                }
            }

            if (null != luaFunction && null != this.mLuaCSDispatchFunctionObject)
            {
                ret = this.mLuaCSDispatchFunctionObject.isFunctionEqual(luaFunction);

                if(!ret)
                {
                    return ret;
                }
            }

            if (null != this.mLuaCSDispatchFunctionObject)
            {
                ret = this.mLuaCSDispatchFunctionObject.isEventIdEqual(luaEventId);

                if (!ret)
                {
                    return ret;
                }
            }

            return ret;
        }

        public void call(IDispatchObject dispObj)
        {
            //if(mThis != null)
            //{
            //    mThis.call(dispObj);
            //}

            if(null != this.mHandle)
            {
                this.mHandle(dispObj);
            }

            if(this.mLuaCSDispatchFunctionObject != null)
            {
                this.mLuaCSDispatchFunctionObject.call(dispObj);
            }
        }

        public void setClientDispose(bool isDispose)
        {
            this.mIsClientDispose = isDispose;
        }

        public bool isClientDispose()
        {
            return this.mIsClientDispose;
        }
    }
}