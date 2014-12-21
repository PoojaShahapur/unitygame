using System;

namespace SDK.Common
{
    public interface IEventDispatch
    {
        void addEventListener(EventID evtID, Action<IDispatchObject> cb);
        void removeEventListener(EventID evtID, Action<IDispatchObject> cb);
    }
}