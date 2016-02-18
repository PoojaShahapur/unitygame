using System;

namespace SDK.Lib
{
    public class EventDispatchFunctionObject : IDelayHandleItem
    {
        public bool m_bClientDispose;       // 是否释放了资源
        public Action<IDispatchObject> m_handle;

        public EventDispatchFunctionObject()
        {
            m_bClientDispose = false;
        }

        public void setFuncObject(Action<IDispatchObject> func)
        {
            this.m_handle = func;
        }

        public bool isValid()
        {
            if (null != this.m_handle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isEqual(Action<IDispatchObject> handle)
        {
            return UtilApi.isAddressEqual(this.m_handle, handle);
        }

        public void call(IDispatchObject dispObj)
        {
            if (null !=  m_handle)
            {
                m_handle(dispObj);
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