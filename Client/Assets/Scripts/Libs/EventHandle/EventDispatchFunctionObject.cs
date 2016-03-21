using LuaInterface;
using System;

namespace SDK.Lib
{
    public class EventDispatchFunctionObject : IDelayHandleItem
    {
        public bool m_bClientDispose;       // 是否释放了资源
        public Action<IDispatchObject> m_handle;

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

        public void setFuncObject(Action<IDispatchObject> func)
        {
            this.m_handle = func;
        }

        public void setLuaTable(LuaTable luaTable)
        {
            if (m_luaCSDispatchFunctionObject == null)
            {
                m_luaCSDispatchFunctionObject = new LuaCSDispatchFunctionObject();
            }

            m_luaCSDispatchFunctionObject.setTable(luaTable);
        }

        public void setLuaFunction(LuaFunction function)
        {
            if (m_luaCSDispatchFunctionObject == null)
            {
                m_luaCSDispatchFunctionObject = new LuaCSDispatchFunctionObject();
            }

            m_luaCSDispatchFunctionObject.setFunction(function);
        }

        public void setLuaFunctor(LuaTable luaTable, LuaFunction function)
        {
            if (m_luaCSDispatchFunctionObject == null)
            {
                m_luaCSDispatchFunctionObject = new LuaCSDispatchFunctionObject();
            }

            m_luaCSDispatchFunctionObject.setTable(luaTable);
            m_luaCSDispatchFunctionObject.setFunction(function);
        }

        public bool isValid()
        {
            return m_handle != null || (m_luaCSDispatchFunctionObject != null && m_luaCSDispatchFunctionObject.isValid());
        }

        public bool isEqual(Action<IDispatchObject> handle, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            bool ret = false;
            if (handle != null)
            {
                ret = UtilApi.isAddressEqual(this.m_handle, handle);
                if (!ret)
                {
                    return ret;
                }
            }
            if (luaTable != null)
            {
                ret = m_luaCSDispatchFunctionObject.isTableEqual(luaTable);
                if (!ret)
                {
                    return ret;
                }
            }
            if (luaTable != null)
            {
                ret = m_luaCSDispatchFunctionObject.isFunctionEqual(luaFunction);
                if (!ret)
                {
                    return ret;
                }
            }

            return ret;
        }

        public void call(IDispatchObject dispObj)
        {
            if (null !=  m_handle)
            {
                m_handle(dispObj);
            }

            if (m_luaCSDispatchFunctionObject != null)
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