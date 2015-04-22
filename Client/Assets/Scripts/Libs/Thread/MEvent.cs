using System.Threading;

namespace SDK.Lib
{
    /**
     * @同步使用的 Event
     */
    public class MEvent
    {
        private ManualResetEvent m_event;

        public MEvent(bool initialState)
        {
            m_event = new ManualResetEvent(initialState);
        }

        public void WaitOne()
        {
            m_event.WaitOne();
        }

        public bool Reset()
        {
            return m_event.Reset();
        }

        public bool Set()
        {
            return m_event.Set();
        }
    }
}