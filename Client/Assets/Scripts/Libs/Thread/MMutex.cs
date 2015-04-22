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
            m_mutex = new Mutex(initiallyOwned, name);
        }

        public void WaitOne()
        {
            m_mutex.WaitOne();
        }

        public void ReleaseMutex()
        {
            m_mutex.ReleaseMutex();
        }

        public void close()
        {
            m_mutex.Close();
        }
    }
}