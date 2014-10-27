using SDK.Common;
using System;

namespace SDK.Lib
{
    /**
     * @brief 框架分发消息
     */
    public class MsgRoute : IEventDispatch
    {
        public void addEventListener(EventID evtID, Action<SDK.Common.Event> cb)
        {
            
        }

        public void removeEventListener(EventID evtID, Action<SDK.Common.Event> cb)
        {
            
        }
    }
}
