using SDK.Common;
using System;
using System.Collections;

namespace SDK.Lib
{
    public class DeferredMethod
    {
        public Action<IDelayHandleItem, float> m_addMethod = null;
        public Action<IDelayHandleItem> m_delMethod = null;
		public ArrayList m_args = null;
    }
}