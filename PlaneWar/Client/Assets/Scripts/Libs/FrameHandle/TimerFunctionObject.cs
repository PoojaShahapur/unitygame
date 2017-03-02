using LuaInterface;
using System;

namespace SDK.Lib
{
    public class TimerFunctionObject
    {
        public Action<TimerItemBase> mHandle;
        protected LuaCSDispatchFunctionObject mLuaCSDispatchFunctionObject;

        public TimerFunctionObject()
        {
            this.mHandle = null;
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

        public void setFuncObject(Action<TimerItemBase> handle)
        {
            this.mHandle = handle;
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

        public void setLuaFunctor(LuaTable luaTable, LuaFunction function)
        {
            if(this.mLuaCSDispatchFunctionObject == null)
            {
                this.mLuaCSDispatchFunctionObject = new LuaCSDispatchFunctionObject();
            }

            this.mLuaCSDispatchFunctionObject.setTable(luaTable);
            this.mLuaCSDispatchFunctionObject.setFunction(function);
        }

        public bool isValid()
        {
            return this.mHandle != null || (this.mLuaCSDispatchFunctionObject != null && this.mLuaCSDispatchFunctionObject.isValid());
        }

        public bool isEqual(MAction<IDispatchObject> handle, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            bool ret = false;
            if(handle != null)
            {
                ret = UtilApi.isAddressEqual(this.mHandle, handle);
                if(!ret)
                {
                    return ret;
                }
            }
            if(luaTable != null)
            {
                ret = this.mLuaCSDispatchFunctionObject.isTableEqual(luaTable);
                if(!ret)
                {
                    return ret;
                }
            }
            if(luaTable != null)
            {
                ret = this.mLuaCSDispatchFunctionObject.isFunctionEqual(luaFunction);
                if (!ret)
                {
                    return ret;
                }
            }

            return ret;
        }

        public void call(TimerItemBase dispObj)
        {
            if (null != this.mHandle)
            {
                this.mHandle(dispObj);
            }

            if(this.mLuaCSDispatchFunctionObject != null)
            {
                this.mLuaCSDispatchFunctionObject.call(dispObj);
            }
        }
    }
}