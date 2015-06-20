using SDK.Common;
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