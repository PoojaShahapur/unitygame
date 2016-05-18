using LuaInterface;
using System;

namespace SDK.Lib
{
    public class EventDispatchFunctionObject : IDelayHandleItem
    {
        public bool m_bClientDispose;       // 是否释放了资源
        public ICalleeObject mThis;
        public MAction<IDispatchObject> m_handle;

        protected LuaCSDispatchFunctionObject m_luaCSDispatchFunctionObject;

        public EventDispatchFunctionObject()
        {
            m_bClientDispose = false;
        }

        public LuaCSDispatchFunctionObject luaCSDispatchFunctionObject
        {
            get
            {
                return m_luaCSDispatchFunctionObject;
            }
            set
            {
                m_luaCSDispatchFunctionObject = value;
            }
        }

        public void setFuncObject(ICalleeObject pThis, MAction<IDispatchObject> func)
        {
            this.mThis = pThis;
            this.m_handle = func;
        }

        public void setLuaTable(LuaTable luaTable)
        {
            if(m_luaCSDispatchFunctionObject == null)
            {
                m_luaCSDispatchFunctionObject = new LuaCSDispatchFunctionObject();
            }

            m_luaCSDispatchFunctionObject.setTable(luaTable);
        }

        public void setLuaFunction(LuaFunction function)
        {
            if(m_luaCSDispatchFunctionObject == null)
            {
                m_luaCSDispatchFunctionObject = new LuaCSDispatchFunctionObject();
            }

            m_luaCSDispatchFunctionObject.setFunction(function);
        }

        public void setLuaFunctor(LuaTable luaTable, LuaFunction function)
        {
            if(m_luaCSDispatchFunctionObject == null)
            {
                m_luaCSDispatchFunctionObject = new LuaCSDispatchFunctionObject();
            }

            m_luaCSDispatchFunctionObject.setTable(luaTable);
            m_luaCSDispatchFunctionObject.setFunction(function);
        }

        public bool isValid()
        {
            return mThis != null || m_handle != null || (m_luaCSDispatchFunctionObject != null && m_luaCSDispatchFunctionObject.isValid());
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
                ret = UtilApi.isAddressEqual(this.m_handle, handle);
                if(!ret)
                {
                    return ret;
                }
            }
            if(luaTable != null)
            {
                ret = m_luaCSDispatchFunctionObject.isTableEqual(luaTable);
                if(!ret)
                {
                    return ret;
                }
            }
            if (luaFunction != null)
            {
                ret = m_luaCSDispatchFunctionObject.isFunctionEqual(luaFunction);
                if(!ret)
                {
                    return ret;
                }
            }

            return ret;
        }

        public void call(IDispatchObject dispObj)
        {
            if(mThis != null)
            {
                mThis.call(dispObj);
            }

            if(null != m_handle)
            {
                m_handle(dispObj);
            }

            if(m_luaCSDispatchFunctionObject != null)
            {
                m_luaCSDispatchFunctionObject.call(dispObj);
            }
        }

        public void setClientDispose()
        {
            m_bClientDispose = true;
        }

        public bool getClientDispose()
        {
            return m_bClientDispose;
        }
    }
}