using System.Threading;

namespace SDK.Lib
{
    /**
     * @brief 互斥
     */
    public class MMutex
    {
        #if NET_MULTHREAD
        private Mutex m_mutex;   // 读互斥
        #endif

        public MMutex(bool initiallyOwned, string name)
        {
#if NET_MULTHREAD
            m_mutex = new Mutex(initiallyOwned, name);
#endif
        }

        public void WaitOne()
        {
#if NET_MULTHREAD
            m_mutex.WaitOne();
#endif
        }

        public void ReleaseMutex()
        {
#if NET_MULTHREAD
            m_mutex.ReleaseMutex();
#endif
        }

        public void close()
        {
#if NET_MULTHREAD
            m_mutex.Close();
#endif
        }
    }
}