using System.Threading;

namespace SDK.Lib
{
    /**
     * @brief 基本的锁
     */
    public class MutexWrap
    {
        public Mutex m_visitMutex;   // 主要是添加和获取数据互斥

        public MutexWrap(bool initiallyOwned, string name)
        {
            #if NETMULTHREAD
            m_visitMutex = new Mutex(initiallyOwned, name);
            #endif
        }

        public void WaitOne()
        {
            #if NETMULTHREAD
            m_visitMutex.WaitOne();
            #endif
        }

        public void ReleaseMutex()
        {
            #if NETMULTHREAD
            m_visitMutex.ReleaseMutex();
            #endif
        }

        public void close()
        {
            #if NETMULTHREAD
            m_visitMutex.Close();
            #endif
        }
    }
}