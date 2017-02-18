using System.Threading;

namespace SDK.Lib
{
    /**
     * @同步使用的 Event
     */
    public class MEvent
    {
        private ManualResetEvent mEvent;

        public MEvent(bool initialState)
        {
            mEvent = new ManualResetEvent(initialState);
        }

        public void WaitOne()
        {
            mEvent.WaitOne();
        }

        public bool Reset()
        {
            return mEvent.Reset();
        }

        public bool Set()
        {
            return mEvent.Set();
        }
    }
}