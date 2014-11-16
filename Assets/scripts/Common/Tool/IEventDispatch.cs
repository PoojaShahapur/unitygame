using System;
namespace SDK.Common
{
    public interface IEventDispatch
    {
        void addEventListener(EventID evtID, Action<EventDisp> cb);
        void removeEventListener(EventID evtID, Action<EventDisp> cb);
    }
}