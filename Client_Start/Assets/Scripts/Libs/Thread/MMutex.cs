using System.Threading;

namespace SDK.Lib
{
    /**
     * @brief 互斥
     */
    public class MMutex
    {
        private Mutex m_mutex;   // 读互斥

        public MMutex(bool initiallyOwned, string name)
        {
            if (MacroDef.NET_MULTHREAD)
            {
                // IOS 下不支持 "Named mutexes are not supported"
                m_mutex = new Mutex(initiallyOwned, name);
            }
        }

        public void WaitOne()
        {
            if (MacroDef.NET_MULTHREAD)
            {
                m_mutex.WaitOne();
            }
        }

        public void ReleaseMutex()
        {
            if (MacroDef.NET_MULTHREAD)
            {
                m_mutex.ReleaseMutex();
            }
        }

        public void close()
        {
            if (MacroDef.NET_MULTHREAD)
            {
                m_mutex.Close();
            }
        }
    }
}