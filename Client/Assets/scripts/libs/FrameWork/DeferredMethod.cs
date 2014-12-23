using SDK.Common;
using System;
using System.Collections;

namespace SDK.Lib
{
    public class DeferredMethod
    {
        public Action<ITickedObject, float> m_method = null;
		public ArrayList m_args = null;
    }
}
