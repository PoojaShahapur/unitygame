using LuaInterface;
using System;

namespace SDK.Lib
{
    public class EventDispatchFunctionObject : IDelayHandleItem
    {
        public bool mIsClientDispose;       // 是否释放了资源
        public ICalleeObject mThis;
        public MAction<IDispatchObject> mHandle;

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

        public void setFuncObject(ICalleeObject pThis, MAction<IDispatchObject> func)
        {
            this.mThis = pThis;
            this.mHandle = func;
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
            return this.mThis != null || this.mHandle != null || (this.mLuaCSDispatchFunctionObject != null && this.mLuaCSDispatchFunctionObject.isValid());
        }

        public bool isEqual(ICalleeObject pThis, MAction<IDispatchObject> handle, LuaTable luaTable = null, LuaFunction luaFunction = null)
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

            if(luaTable != null)
            {
                ret = this.mLuaCSDispatchFunctionObject.isTableEqual(luaTable);

                if(!ret)
                {
                    return ret;
                }
            }

            if (luaFunction != null)
            {
                ret = this.mLuaCSDispatchFunctionObject.isFunctionEqual(luaFunction);

                if(!ret)
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