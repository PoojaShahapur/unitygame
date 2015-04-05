using System.Threading;

namespace SDK.Lib
{
    /**
     * @同步使用的 Event
     */
    public class MEvent
    {
#if NET_MULTHREAD
        private ManualResetEvent m_event;
#endif

        public MEvent(bool initialState)
        {
#if NET_MULTHREAD
            m_event = new ManualResetEvent(initialState);
#endif
        }

        public void WaitOne()
        {
#if NET_MULTHREAD
            m_event.WaitOne();
#endif
        }

        public bool Reset()
        {
#if NET_MULTHREAD
            return m_event.Reset();
#endif
        }

        public bool Set()
        {
#if NET_MULTHREAD
            return m_event.Set();
#endif
        }
    }
}