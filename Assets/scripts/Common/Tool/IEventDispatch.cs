using System;
namespace SDK.Common
{
    public interface IEventDispatch
    {
        void addEventListener(EventID evtID, Action<Event> cb);
        void removeEventListener(EventID evtID, Action<Event> cb);
    }
}
